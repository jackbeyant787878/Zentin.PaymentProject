using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Zentin.Payment.Application.Consumers;
using Zentin.Payment.Application.Contracts.IntegrationEvents;
using Zentin.Payment.Application.Contracts.Ports;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class MchNotifyConsumerIntegrationTests
    {
        private static WebApplicationFactory<Program> _factory = null!;
        private static HttpClient _client = null!;
        private static string _connectionString = null!;

        private sealed class FakeMchNotifySender : IMchNotifySender
        {
            public string Response { get; set; } = "SUCCESS";
            public Task<string> SendAsync(string notifyUrl, string method, string mediaType, string? body, CancellationToken cancellationToken = default)
                => Task.FromResult(Response);
        }

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext _)
        {
            _connectionString = $"Server=(localdb)\\mssqllocaldb;Database=ZentinMchNotifyTest_{Guid.NewGuid():N};Integrated Security=true;TrustServerCertificate=true";

            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using (var db = new PaymentDbContext(options))
                await db.Database.MigrateAsync();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ConnectionStrings:DefaultConnection", _connectionString);
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IMchNotifySender));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddSingleton<FakeMchNotifySender>();
                    services.AddSingleton<IMchNotifySender>(sp => sp.GetRequiredService<FakeMchNotifySender>());
                });
            });
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            _client?.Dispose();
            if (_factory is not null) await _factory.DisposeAsync();

            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            await db.Database.EnsureDeletedAsync();
        }

        private static async Task SeedMerchantAndAppAsync(string mchNo, string appId)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
            var mchInfo = MchInfo.Create(mchNo, "通知测试商户", "通知商户", MchType.Normal, "联系人", "13800000000");
            mchInfo.AddApp(MchApp.Create(appId, "通知测试应用", "test-secret-001", defaultFlag: 1));
            db.MchInfos.Add(mchInfo);
            await db.SaveChangesAsync();
        }

        private static async Task<string> CreatePaidOrderAsync(string mchNo, string appId)
        {
            var createBody = new
            {
                mchNo,
                mchName = "通知测试商户",
                appId,
                mchOrderNo = "MO-" + Guid.NewGuid().ToString("N"),
                wayCode = "WX_NATIVE",
                wayType = "WX",
                amount = 10000L,
                mchFeeRate = 0.0055m,
                subject = "通知测试商品",
                body = "通知测试描述",
                notifyUrl = "https://merchant.example.com/notify",
                expiredTime = DateTime.UtcNow.AddHours(1)
            };
            var response = await _client.PostAsJsonAsync("/api/payorders", createBody);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var payOrderId = doc.RootElement.GetProperty("payOrderId").GetString()!;

            Assert.AreEqual(HttpStatusCode.NoContent,
                (await _client.PostAsJsonAsync($"/api/payorders/{payOrderId}/paying", new { ifCode = "wxpay" })).StatusCode);
            Assert.AreEqual(HttpStatusCode.NoContent,
                (await _client.PostAsJsonAsync($"/api/payorders/{payOrderId}/confirm-success", new { channelOrderNo = "CH-" + Guid.NewGuid().ToString("N") })).StatusCode);
            return payOrderId;
        }

        private static async Task DispatchPaidEventAsync(string payOrderId, string mchNo)
        {
            using var scope = _factory.Services.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<PayOrderPaidNotifyConsumer>();
            await consumer.HandleAsync(new PayOrderPaidIntegrationEvent(payOrderId, mchNo, 10000L, DateTime.UtcNow));
        }

        private static async Task<long> GetNotifyIdAsync(string payOrderId)
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            return await db.MchNotifyRecords.Where(r => r.OrderId == payOrderId).Select(r => r.NotifyId).SingleAsync();
        }

        private static async Task RunDispatchConsumerAsync(long notifyId)
        {
            using var scope = _factory.Services.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<MchNotifyDispatchConsumer>();
            await consumer.HandleAsync(new MchNotifyDispatchIntegrationEvent(notifyId));
        }

        private static async Task<(int State, int NotifyCount, string? ResResult)> ReadRecordAsync(long notifyId)
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            var record = await db.MchNotifyRecords.AsNoTracking().SingleAsync(r => r.NotifyId == notifyId);
            return ((int)record.State, record.NotifyCount, record.ResResult);
        }

        private static async Task<int> CountDispatchMessagesAsync(long notifyId)
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            var connection = db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Cap.Published WHERE Name = 'zentin.mchnotify.dispatch' AND Content LIKE @id";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = "%" + notifyId + "%";
            command.Parameters.Add(parameter);
            return (int)(await command.ExecuteScalarAsync())!;
        }

        private static async Task<(string StatusName, DateTime Added, DateTime? ExpiresAt)> ReadLatestDispatchRowAsync(long notifyId)
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            var connection = db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT TOP (1) StatusName, Added, ExpiresAt FROM Cap.Published WHERE Name = 'zentin.mchnotify.dispatch' AND Content LIKE @id ORDER BY Id DESC";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = "%" + notifyId + "%";
            command.Parameters.Add(parameter);
            await using var reader = await command.ExecuteReaderAsync();
            Assert.IsTrue(await reader.ReadAsync());
            return (reader.GetString(0), reader.GetDateTime(1), await reader.IsDBNullAsync(2) ? null : reader.GetDateTime(2));
        }

        private static async Task<bool> ReadPayOrderNotifyStateAsync(string payOrderId)
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            var connection = db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT NotifyState FROM PayOrder WHERE PayOrderId = @id";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = payOrderId;
            command.Parameters.Add(parameter);
            return (bool)(await command.ExecuteScalarAsync())!;
        }

        [TestMethod]
        public async Task Paid_event_creates_signed_record_and_success_terminates()
        {
            var mchNo = "MN-" + Guid.NewGuid().ToString("N")[..8];
            var appId = "APP-" + Guid.NewGuid().ToString("N")[..8];
            await SeedMerchantAndAppAsync(mchNo, appId);
            var payOrderId = await CreatePaidOrderAsync(mchNo, appId);

            await DispatchPaidEventAsync(payOrderId, mchNo);

            var notifyId = await GetNotifyIdAsync(payOrderId);
            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using (var db = new PaymentDbContext(options))
            {
                var record = await db.MchNotifyRecords.AsNoTracking().SingleAsync(r => r.NotifyId == notifyId);
                Assert.AreEqual(NotifyState.Notifying, record.State);
                Assert.AreEqual("POST", record.ReqMethod);
                Assert.AreEqual("application/json", record.ReqMediaType);
                StringAssert.Contains(record.ReqBody, "\"sign\"");
                StringAssert.Contains(record.ReqBody, payOrderId);
            }
            Assert.AreEqual(1, await CountDispatchMessagesAsync(notifyId));

            await RunDispatchConsumerAsync(notifyId);

            var (state, count, res) = await ReadRecordAsync(notifyId);
            Assert.AreEqual((int)NotifyState.Success, state);
            Assert.AreEqual(0, count);
            Assert.AreEqual("SUCCESS", res);
            Assert.IsTrue(await ReadPayOrderNotifyStateAsync(payOrderId));

            // 终态后重投幂等：状态与计数不变
            await RunDispatchConsumerAsync(notifyId);
            var (stateAfter, countAfter, _) = await ReadRecordAsync(notifyId);
            Assert.AreEqual((int)NotifyState.Success, stateAfter);
            Assert.AreEqual(0, countAfter);
        }

        [TestMethod]
        public async Task Failed_attempt_schedules_delayed_retry_and_case_insensitive_success_terminates()
        {
            var mchNo = "MN-" + Guid.NewGuid().ToString("N")[..8];
            var appId = "APP-" + Guid.NewGuid().ToString("N")[..8];
            await SeedMerchantAndAppAsync(mchNo, appId);
            var payOrderId = await CreatePaidOrderAsync(mchNo, appId);
            await DispatchPaidEventAsync(payOrderId, mchNo);
            var notifyId = await GetNotifyIdAsync(payOrderId);

            var fake = _factory.Services.GetRequiredService<FakeMchNotifySender>();
            fake.Response = "FAIL";
            try
            {
                await RunDispatchConsumerAsync(notifyId);

                var (state, count, res) = await ReadRecordAsync(notifyId);
                Assert.AreEqual((int)NotifyState.Notifying, state);
                Assert.AreEqual(1, count);
                Assert.AreEqual("FAIL", res);
                // 首次尝试无论成败都回写 NotifyState
                Assert.IsTrue(await ReadPayOrderNotifyStateAsync(payOrderId));
                Assert.AreEqual(2, await CountDispatchMessagesAsync(notifyId));
                var retryRow = await ReadLatestDispatchRowAsync(notifyId);
                Assert.IsNotNull(retryRow.ExpiresAt, $"status={retryRow.StatusName} added={retryRow.Added:O} expires={retryRow.ExpiresAt:O}");
                Assert.IsTrue(retryRow.ExpiresAt > retryRow.Added, $"status={retryRow.StatusName} added={retryRow.Added:O} expires={retryRow.ExpiresAt:O}");

                fake.Response = "success";
                await RunDispatchConsumerAsync(notifyId);

                var (finalState, finalCount, finalRes) = await ReadRecordAsync(notifyId);
                Assert.AreEqual((int)NotifyState.Success, finalState);
                Assert.AreEqual(1, finalCount);
                Assert.AreEqual("success", finalRes);
            }
            finally
            {
                fake.Response = "SUCCESS";
            }
        }

        [TestMethod]
        public async Task Duplicate_paid_events_create_single_record()
        {
            var mchNo = "MN-" + Guid.NewGuid().ToString("N")[..8];
            var appId = "APP-" + Guid.NewGuid().ToString("N")[..8];
            await SeedMerchantAndAppAsync(mchNo, appId);
            var payOrderId = await CreatePaidOrderAsync(mchNo, appId);

            await DispatchPaidEventAsync(payOrderId, mchNo);
            await DispatchPaidEventAsync(payOrderId, mchNo);

            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            var count = await db.MchNotifyRecords.CountAsync(r => r.OrderId == payOrderId);
            Assert.AreEqual(1, count);
        }
    }
}