using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Agent;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Agent
{
    public sealed class AgentInfoEntityTypeConfiguration : IEntityTypeConfiguration<AgentInfo>
    {
        public void Configure(EntityTypeBuilder<AgentInfo> builder)
        {
            builder.ToTable("AgentInfo");
            builder.HasKey(e => e.AgentNo);

            builder.Property(e => e.AgentNo).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.AgentName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AgentShortName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.AgentType).HasConversion<int>().HasDefaultValue(AgentType.Personal);
            builder.Property(e => e.Level).HasColumnType("int").HasDefaultValue(1);
            builder.Property(e => e.Pid).HasMaxLength(64);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.ContactName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.ContactTel).HasMaxLength(32).IsRequired();
            builder.Property(e => e.ContactEmail).HasMaxLength(32);
            builder.Property(e => e.AddAgentFlag).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.Sipw).HasMaxLength(128);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(AgentState.Normal);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.InitUserId).HasColumnType("bigint");


            builder.Property(e => e.SettAccountType).HasConversion<int>();
            builder.Property(e => e.SettAccountTelphone).HasMaxLength(32);
            builder.Property(e => e.SettAccountName).HasMaxLength(32);
            builder.Property(e => e.SettAccountNo).HasMaxLength(32);
            builder.Property(e => e.SettAccountBank).HasMaxLength(32);
            builder.Property(e => e.SettAccountSubBank).HasMaxLength(32);


            builder.Property(e => e.CashoutFeeRuleType).HasConversion<int>().HasDefaultValue(CashoutFeeRuleType.SystemDefault);
            builder.Property(e => e.CashoutFeeRule).HasMaxLength(255);


            builder.Property(e => e.LicenseImg).HasMaxLength(128);
            builder.Property(e => e.PermitImg).HasMaxLength(128);
            builder.Property(e => e.Idcard1Img).HasMaxLength(128);
            builder.Property(e => e.Idcard2Img).HasMaxLength(128);
            builder.Property(e => e.IdcardInHandImg).HasMaxLength(128);
            builder.Property(e => e.BankCardImg).HasMaxLength(128);


            builder.Property(e => e.UnAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.BalanceAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.AuditProfitAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.FreezeAmount).HasColumnType("bigint").HasDefaultValue(0);

            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.Pid)
                .HasConstraintName("FKAgentInfoPid")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.Pid).HasDatabaseName("IdxPid");
            builder.HasIndex(e => e.State).HasDatabaseName("IdxState");
        }
    }
}
