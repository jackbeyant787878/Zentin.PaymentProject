using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Division;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Division
{
    public sealed class MchDivisionReceiverConfiguration : IEntityTypeConfiguration<MchDivisionReceiver>
    {
        public void Configure(EntityTypeBuilder<MchDivisionReceiver> builder)
        {
            builder.ToTable("MchDivisionReceiver");
            builder.HasKey(e => e.ReceiverId);

            builder.Property(e => e.ReceiverId).HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.ReceiverAlias).HasMaxLength(64).IsRequired();
            builder.Property(e => e.ReceiverGroupId).HasColumnType("bigint");
            builder.Property(e => e.ReceiverGroupName).HasMaxLength(64);
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.AccType).HasConversion<int>().IsRequired();
            builder.Property(e => e.AccNo).HasMaxLength(50).IsRequired();
            builder.Property(e => e.AccName).HasMaxLength(30).HasDefaultValue("").IsRequired();
            builder.Property(e => e.RelationType).HasMaxLength(30).IsRequired();
            builder.Property(e => e.RelationTypeName).HasMaxLength(30).IsRequired();
            builder.Property(e => e.DivisionProfit).HasPrecision(20, 6);
            builder.Property(e => e.State).HasConversion<int>().IsRequired();
            builder.Property(e => e.ChannelAccNo).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ChannelBindResult).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ChannelExtInfo).HasColumnType("nvarchar(max)");
            builder.Property(e => e.BindSuccessTime).HasColumnType("datetime2(6)");


            builder.HasIndex(e => e.MchNo).HasDatabaseName("Idx_MchNo");
            builder.HasIndex(e => e.ReceiverGroupId).HasDatabaseName("Idx_GroupId");
        }
    }
}
