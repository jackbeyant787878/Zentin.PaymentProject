using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.CheckBill;
using Zentin.Payment.Domain.Entities.Device;
using Zentin.Payment.Domain.Entities.Division;
using Zentin.Payment.Domain.Entities.Notify;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.Entities.QrCode;
using Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay;
using Zentin.Payment.Infrastructure.Interceptors;

namespace Zentin.Payment.Infrastructure.Data
{
    public sealed class PaymentDbContext : DbContext
    {
        #region DbSets
        public DbSet<Domain.Entities.Order.PayOrder> PayOrders { get; set; }
        public DbSet<Domain.Entities.Order.RefundOrder> RefundOrders { get; set; }
        public DbSet<TransferOrder> TransferOrders { get; set; }
        public DbSet<MchNotifyRecord> MchNotifyRecords { get; set; }
        public DbSet<OrderSnapshot> OrderSnapshots { get; set; }
        public DbSet<PayWay> PayWays { get; set; }
        public DbSet<PayInterfaceDefine> PayInterfaceDefines { get; set; }
        public DbSet<PayInterfaceConfig> PayInterfaceConfigs { get; set; }
        public DbSet<PayRateConfig> PayRateConfigs { get; set; }
        public DbSet<MchPayPassage> MchPayPassages { get; set; }
        public DbSet<MchDivisionReceiverGroup> MchDivisionReceiverGroups { get; set; }
        public DbSet<MchDivisionReceiver> MchDivisionReceivers { get; set; }
        public DbSet<QrCodeShell> QrCodeShells { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }
        public DbSet<CheckBillBatch> CheckBillBatches { get; set; }
        public DbSet<ChannelBill> ChannelBills { get; set; }
        public DbSet<DiffBill> DiffBills { get; set; }
        public DbSet<DeviceProviderDefine> DeviceProviderDefines { get; set; }
        public DbSet<DeviceProviderConfig> DeviceProviderConfigs { get; set; }
        public DbSet<DeviceInfo> DeviceInfos { get; set; }
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

            // auto load all IEntityTypeConfiguration configurations in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
        }
    }
}

