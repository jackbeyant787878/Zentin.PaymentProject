using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Agent;
using Zentin.Payment.Domain.Entities.CheckBill;
using Zentin.Payment.Domain.Entities.Device;
using Zentin.Payment.Domain.Entities.Division;
using Zentin.Payment.Domain.Entities.Finance;
using Zentin.Payment.Domain.Entities.Isv;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Entities.Notify;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.Entities.QrCode;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Infrastructure.Interceptors;

namespace Zentin.Payment.Infrastructure.Data
{
    public sealed class PaymentDbContext : DbContext
    {
        #region Order
        public DbSet<PayOrder> PayOrders { get; set; }
        public DbSet<RefundOrder> RefundOrders { get; set; }
        public DbSet<TransferOrder> TransferOrders { get; set; }
        public DbSet<OrderSnapshot> OrderSnapshots { get; set; }
        public DbSet<MchNotifyRecord> MchNotifyRecords { get; set; }
        public DbSet<PayOrderProfit> PayOrderProfits { get; set; }
        public DbSet<PayOrderDivisionRecord> PayOrderDivisionRecords { get; set; }
        #endregion

        #region Pay configuration
        public DbSet<PayWay> PayWays { get; set; }
        public DbSet<PayInterfaceDefine> PayInterfaceDefines { get; set; }
        public DbSet<PayInterfaceConfig> PayInterfaceConfigs { get; set; }
        public DbSet<PayRateConfig> PayRateConfigs { get; set; }
        public DbSet<PayRateLevelConfig> PayRateLevelConfigs { get; set; }
        public DbSet<MchPayPassage> MchPayPassages { get; set; }
        #endregion

        #region Merchant / Agent / Isv
        public DbSet<MchInfo> MchInfos { get; set; }
        public DbSet<MchApp> MchApps { get; set; }
        public DbSet<MchStore> MchStores { get; set; }
        public DbSet<MchApply> MchApplies { get; set; }
        public DbSet<AgentInfo> AgentInfos { get; set; }
        public DbSet<IsvInfo> IsvInfos { get; set; }
        public DbSet<AccountBill> AccountBills { get; set; }
        #endregion

        #region Division
        public DbSet<MchDivisionReceiverGroup> MchDivisionReceiverGroups { get; set; }
        public DbSet<MchDivisionReceiver> MchDivisionReceivers { get; set; }
        #endregion

        #region Member
        public DbSet<MbrInfo> MbrInfos { get; set; }
        public DbSet<MbrBill> MbrBills { get; set; }
        public DbSet<MbrRechargeOrder> MbrRechargeOrders { get; set; }
        public DbSet<MbrRechargeRule> MbrRechargeRules { get; set; }
        #endregion

        #region QrCode
        public DbSet<QrCodeShell> QrCodeShells { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }
        #endregion

        #region CheckBill
        public DbSet<CheckBillBatch> CheckBillBatches { get; set; }
        public DbSet<ChannelBill> ChannelBills { get; set; }
        public DbSet<DiffBill> DiffBills { get; set; }
        #endregion

        #region Device
        public DbSet<DeviceProviderDefine> DeviceProviderDefines { get; set; }
        public DbSet<DeviceProviderConfig> DeviceProviderConfigs { get; set; }
        public DbSet<DeviceInfo> DeviceInfos { get; set; }
        #endregion

        #region System
        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<SysUserAuth> SysUserAuths { get; set; }
        public DbSet<SysUserLoginAttempt> SysUserLoginAttempts { get; set; }
        public DbSet<SysUserTeam> SysUserTeams { get; set; }
        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<SysUserRoleRela> SysUserRoleRelas { get; set; }
        public DbSet<SysEntitlement> SysEntitlements { get; set; }
        public DbSet<SysRoleEntRela> SysRoleEntRelas { get; set; }
        public DbSet<SysConfig> SysConfigs { get; set; }
        public DbSet<SysLog> SysLogs { get; set; }
        public DbSet<SysArticle> SysArticles { get; set; }
        #endregion

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditFieldInterceptor());
            optionsBuilder.EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
        }
    }
}