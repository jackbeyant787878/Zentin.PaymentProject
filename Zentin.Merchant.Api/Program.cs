using Zentin.Payment.Application;
using Zentin.Payment.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ParamsExtension).Assembly));
builder.Services.AddZentinInfrastructure(builder.Configuration);
builder.Services.AddZentinCors(builder.Configuration);
builder.Services.AddZentinSwagger("Zentin.Merchant.Api");

var app = builder.Build();

// 异常处理最外层兜底，请求日志记录方法/路径/状态码/耗时
app.UseZentinExceptionHandling();
app.UseZentinRequestLogging();
app.UseZentinCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseZentinSwagger();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();