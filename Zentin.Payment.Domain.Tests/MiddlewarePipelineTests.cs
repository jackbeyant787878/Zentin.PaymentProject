using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Zentin.Payment.Infrastructure.Extensions;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class MiddlewarePipelineTests
    {
        private static TestServer BuildServer(RequestDelegate terminal)
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services => services.AddLogging())
                .Configure(app =>
                {
                    app.UseZentinExceptionHandling();
                    app.UseZentinRequestLogging();
                    app.Run(terminal);
                });
            return new TestServer(builder);
        }

        [TestMethod]
        public async Task Unhandled_exception_returns_500_problem_details()
        {
            using var server = BuildServer(_ => throw new Exception("boom"));

            var response = await server.CreateClient().GetAsync("/");

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/problem+json", response.Content.Headers.ContentType?.MediaType);
            StringAssert.Contains(await response.Content.ReadAsStringAsync(), "An unexpected error occurred.");
        }

        [TestMethod]
        public async Task Argument_exception_returns_400_with_message()
        {
            using var server = BuildServer(_ => throw new ArgumentException("bad input"));

            var response = await server.CreateClient().GetAsync("/");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            StringAssert.Contains(await response.Content.ReadAsStringAsync(), "bad input");
        }

        [TestMethod]
        public async Task Missing_key_returns_404()
        {
            using var server = BuildServer(_ => throw new KeyNotFoundException("order not found"));

            var response = await server.CreateClient().GetAsync("/");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Healthy_request_passes_through()
        {
            using var server = BuildServer(ctx => ctx.Response.WriteAsync("ok"));

            var response = await server.CreateClient().GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("ok", await response.Content.ReadAsStringAsync());
        }
    }
}
