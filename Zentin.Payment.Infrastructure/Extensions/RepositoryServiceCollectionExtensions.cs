using Microsoft.Extensions.DependencyInjection;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Repositories;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddZentinRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IPayOrderRepository, PayOrderRepository>();
            services.AddScoped<IRefundOrderRepository, RefundOrderRepository>();
            services.AddScoped<ITransferOrderRepository, TransferOrderRepository>();
            services.AddScoped<IOrderSnapshotRepository, OrderSnapshotRepository>();
            services.AddScoped<IPayOrderProfitRepository, PayOrderProfitRepository>();
            services.AddScoped<IPayOrderDivisionRecordRepository, PayOrderDivisionRecordRepository>();
            services.AddScoped<IMchNotifyRecordRepository, MchNotifyRecordRepository>();

            services.AddScoped<IPayInterfaceDefineRepository, PayInterfaceDefineRepository>();
            services.AddScoped<IPayInterfaceConfigRepository, PayInterfaceConfigRepository>();
            services.AddScoped<IPayWayRepository, PayWayRepository>();
            services.AddScoped<IPayRateConfigRepository, PayRateConfigRepository>();
            services.AddScoped<IPayRateLevelConfigRepository, PayRateLevelConfigRepository>();
            services.AddScoped<IMchPayPassageRepository, MchPayPassageRepository>();

            services.AddScoped<IMchInfoRepository, MchInfoRepository>();
            services.AddScoped<IMchAppRepository, MchAppRepository>();
            services.AddScoped<IMchStoreRepository, MchStoreRepository>();
            services.AddScoped<IMchApplyRepository, MchApplyRepository>();
            services.AddScoped<IAgentInfoRepository, AgentInfoRepository>();
            services.AddScoped<IIsvInfoRepository, IsvInfoRepository>();
            services.AddScoped<IAccountBillRepository, AccountBillRepository>();

            services.AddScoped<IMchDivisionReceiverRepository, MchDivisionReceiverRepository>();
            services.AddScoped<IMchDivisionReceiverGroupRepository, MchDivisionReceiverGroupRepository>();

            services.AddScoped<IMbrInfoRepository, MbrInfoRepository>();
            services.AddScoped<IMbrBillRepository, MbrBillRepository>();
            services.AddScoped<IMbrRechargeOrderRepository, MbrRechargeOrderRepository>();
            services.AddScoped<IMbrRechargeRuleRepository, MbrRechargeRuleRepository>();

            services.AddScoped<IQrCodeRepository, QrCodeRepository>();
            services.AddScoped<IQrCodeShellRepository, QrCodeShellRepository>();

            services.AddScoped<ICheckBillBatchRepository, CheckBillBatchRepository>();
            services.AddScoped<IChannelBillRepository, ChannelBillRepository>();
            services.AddScoped<IDiffBillRepository, DiffBillRepository>();

            services.AddScoped<IDeviceInfoRepository, DeviceInfoRepository>();
            services.AddScoped<IDeviceProviderDefineRepository, DeviceProviderDefineRepository>();
            services.AddScoped<IDeviceProviderConfigRepository, DeviceProviderConfigRepository>();

            services.AddScoped<ISysUserRepository, SysUserRepository>();
            services.AddScoped<ISysUserAuthRepository, SysUserAuthRepository>();
            services.AddScoped<ISysUserLoginAttemptRepository, SysUserLoginAttemptRepository>();
            services.AddScoped<ISysUserTeamRepository, SysUserTeamRepository>();
            services.AddScoped<ISysRoleRepository, SysRoleRepository>();
            services.AddScoped<ISysUserRoleRelaRepository, SysUserRoleRelaRepository>();
            services.AddScoped<ISysEntitlementRepository, SysEntitlementRepository>();
            services.AddScoped<ISysRoleEntRelaRepository, SysRoleEntRelaRepository>();
            services.AddScoped<ISysConfigRepository, SysConfigRepository>();
            services.AddScoped<ISysLogRepository, SysLogRepository>();
            services.AddScoped<ISysArticleRepository, SysArticleRepository>();

            return services;
        }
    }
}