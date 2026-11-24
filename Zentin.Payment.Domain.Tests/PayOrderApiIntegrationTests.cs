using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class PayOrderApiIntegrationTests
    {
        private static WebApplicationFactory<Program> _factory = null!;
        private static HttpClient _client = null!;
        private static string _connectionString = null!;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext _)
        {
            _connectionString = $"Server=(localdb)\\mssqllocaldb;Database=ZentinPayOrderTest_{Guid.NewGuid():N};Integrated Security=true;TrustServerCertificate=true";

            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using (var db = new PaymentDbContext(options))
                await db.Database.MigrateAsync();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
                builder.UseSetting("ConnectionStrings:DefaultConnection", _connectionString));
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

        private static object NewCreateBody(string mchOrderNo) => new
        {
            mchNo = "M1001",
            mchName = "集成测试商户",
            appId = "APP001",
            mchOrderNo,
            wayCode = "WX_NATIVE",
            wayType = "WX",
            amount = 10000L,
            mchFeeRate = 0.0055m,
            subject = "集成测试商品",
            body = "集成测试描述",
            notifyUrl = "https://merchant.example.com/notify",
            expiredTime = DateTime.UtcNow.AddHours(1)
        };

        private static async Task<string> CreateOrderAsync(string mchOrderNo)
        {
            var response = await _client.PostAsJsonAsync("/api/payorders", NewCreateBody(mchOrderNo));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return doc.RootElement.GetProperty("payOrderId").GetString()!;
        }

        [TestMethod]
        public async Task Golden_path_pays_order_and_writes_cap_outbox()
        {
            var payOrderId = await CreateOrderAsync("MO-" + Guid.NewGuid().ToString("N"));

            var paying = await _client.PostAsJsonAsync($"/api/payorders/{payOrderId}/paying", new { ifCode = "wxpay" });
            Assert.AreEqual(HttpStatusCode.NoContent, paying.StatusCode);

            var confirm = await _client.PostAsJsonAsync($"/api/payorders/{payOrderId}/confirm-success",
                new { channelOrderNo = "CH-" + Guid.NewGuid().ToString("N"), channelUser = "openid-test" });
            Assert.AreEqual(HttpStatusCode.NoContent, confirm.StatusCode);

            var get = await _client.GetAsync($"/api/payorders/{payOrderId}");
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);
            using var doc = JsonDocument.Parse(await get.Content.ReadAsStringAsync());
            Assert.AreEqual(2, doc.RootElement.GetProperty("state").GetInt32());
            Assert.AreEqual(55, doc.RootElement.GetProperty("mchFeeAmount").GetInt64());

            var options = new DbContextOptionsBuilder<PaymentDbContext>().UseSqlServer(_connectionString).Options;
            await using var db = new PaymentDbContext(options);
            var connection = db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Cap.Published WHERE Name = 'zentin.payorder.paid' AND Content LIKE @id";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = "%" + payOrderId + "%";
            command.Parameters.Add(parameter);
            var count = (int)(await command.ExecuteScalarAsync())!;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task Confirm_success_on_created_order_is_rejected()
        {
            var payOrderId = await CreateOrderAsync("MO-" + Guid.NewGuid().ToString("N"));

            var confirm = await _client.PostAsJsonAsync($"/api/payorders/{payOrderId}/confirm-success", new { channelOrderNo = "CH-X" });

            Assert.AreEqual(HttpStatusCode.BadRequest, confirm.StatusCode);
        }

        [TestMethod]
        public async Task Duplicate_mch_order_no_is_rejected()
        {
            var mchOrderNo = "MO-" + Guid.NewGuid().ToString("N");
            await CreateOrderAsync(mchOrderNo);

            var duplicate = await _client.PostAsJsonAsync("/api/payorders", NewCreateBody(mchOrderNo));

            Assert.AreEqual(HttpStatusCode.BadRequest, duplicate.StatusCode);
        }
    }
}