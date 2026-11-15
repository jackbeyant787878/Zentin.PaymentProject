using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zentin.Payment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountBill",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    BillId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    InfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InfoName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InfoType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BeforeBalance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    ChangeAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AfterBalance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    BizType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AccountType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    RelaBizOrderType = table.Column<int>(type: "int", nullable: false),
                    RelaBizOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBill", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "AgentInfo",
                columns: table => new
                {
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AgentShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AgentType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Pid = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AddAgentFlag = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Sipw = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    InitUserId = table.Column<long>(type: "bigint", nullable: true),
                    SettAccountType = table.Column<int>(type: "int", nullable: true),
                    SettAccountTelphone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SettAccountName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SettAccountNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SettAccountBank = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SettAccountSubBank = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CashoutFeeRuleType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CashoutFeeRule = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LicenseImg = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PermitImg = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Idcard1Img = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Idcard2Img = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IdcardInHandImg = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BankCardImg = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UnAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    BalanceAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AuditProfitAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    FreezeAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentInfo", x => x.AgentNo);
                    table.ForeignKey(
                        name: "FKAgentInfoPid",
                        column: x => x.Pid,
                        principalTable: "AgentInfo",
                        principalColumn: "AgentNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChannelBill",
                columns: table => new
                {
                    ChannelBillId = table.Column<long>(type: "bigint", nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BillType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2(6)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChannelMchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelState = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ChannelAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelSuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    ChannelUser = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "CNY"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelBill", x => x.ChannelBillId);
                });

            migrationBuilder.CreateTable(
                name: "CheckBillBatch",
                columns: table => new
                {
                    BatchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2(6)", nullable: false),
                    OrgBillFilePath = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ParseStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ParseErrMsg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    HandleState = table.Column<int>(type: "int", nullable: false),
                    DiffCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UnHandleCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalRefundAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalRefundCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChannelMchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelTotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelTotalCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChannelTotalFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelTotalRefundAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelTotalRefundCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckBillBatch", x => x.BatchNo);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInfo",
                columns: table => new
                {
                    DeviceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ConfigId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    ProviderCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatchId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BindState = table.Column<int>(type: "int", nullable: false),
                    BindType = table.Column<int>(type: "int", nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    BindQrcId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInfo", x => x.DeviceInfoId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceProviderConfig",
                columns: table => new
                {
                    DeviceProviderConfigId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    ProviderCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProviderParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceProviderConfig", x => x.DeviceProviderConfigId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceProviderDefine",
                columns: table => new
                {
                    ProviderCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConfigPageType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ProviderParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    DeviceTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BgColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceProviderDefine", x => x.ProviderCode);
                });

            migrationBuilder.CreateTable(
                name: "DiffBill",
                columns: table => new
                {
                    DiffBillId = table.Column<long>(type: "bigint", nullable: false),
                    DiffType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BillType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2(6)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OrderState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OrderSuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    OrderCreatedAt = table.Column<DateTime>(type: "datetime2(6)", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    FeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    RefundAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelMchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelRefundAmount = table.Column<long>(type: "bigint", nullable: false),
                    ChannelSuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    HandleState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HandleUid = table.Column<long>(type: "bigint", nullable: true),
                    HandleBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiffBill", x => x.DiffBillId);
                });

            migrationBuilder.CreateTable(
                name: "IsvInfo",
                columns: table => new
                {
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsvName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsvShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ContactTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsvInfo", x => x.IsvNo);
                });

            migrationBuilder.CreateTable(
                name: "MbrInfo",
                columns: table => new
                {
                    MbrNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MbrName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MbrTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AliUserId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    WxMpOpenId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbrInfo", x => x.MbrNo);
                });

            migrationBuilder.CreateTable(
                name: "MbrRechargeOrder",
                columns: table => new
                {
                    RechargeOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MbrNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MbrName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MbrTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PayOrderId = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    WayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PayAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    EntryAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    GiveAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AfterBalance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AvatarUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotifyUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    ReturnUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true, defaultValue: ""),
                    ExpiredTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    SuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbrRechargeOrder", x => x.RechargeOrderId);
                });

            migrationBuilder.CreateTable(
                name: "MchApply",
                columns: table => new
                {
                    ApplyId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TopAgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AutoConfigMchAppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApplyPageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApplyDetailInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplyErrorInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ChannelApplyNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SuccResParameter = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ChannelVar1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ChannelVar2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsTempData = table.Column<bool>(type: "bit", nullable: false),
                    MchFullName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MerchantType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ContactName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ProvinceCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CityCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DistrictCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EpUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastApplyAt = table.Column<DateTime>(type: "datetime2(6)", nullable: false),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchApply", x => x.ApplyId);
                });

            migrationBuilder.CreateTable(
                name: "MchDivisionReceiver",
                columns: table => new
                {
                    ReceiverId = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverAlias = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ReceiverGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ReceiverGroupName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccType = table.Column<int>(type: "int", nullable: false),
                    AccNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: ""),
                    RelationType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RelationTypeName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DivisionProfit = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    ChannelAccNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelBindResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelExtInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BindSuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchDivisionReceiver", x => x.ReceiverId);
                });

            migrationBuilder.CreateTable(
                name: "MchDivisionReceiverGroup",
                columns: table => new
                {
                    ReceiverGroupId = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverGroupName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AutoDivisionFlag = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchDivisionReceiverGroup", x => x.ReceiverGroupId);
                });

            migrationBuilder.CreateTable(
                name: "MchInfo",
                columns: table => new
                {
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MchLevel = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false, defaultValue: "M0"),
                    RefundMode = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Sipw = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TopAgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    InitUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchInfo", x => x.MchNo);
                });

            migrationBuilder.CreateTable(
                name: "MchNotifyRecord",
                columns: table => new
                {
                    NotifyId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    MchOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    NotifyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReqMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ReqMediaType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ReqBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotifyCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotifyCountLimit = table.Column<int>(type: "int", nullable: false, defaultValue: 6),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    LastNotifyTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchNotifyRecord", x => x.NotifyId);
                });

            migrationBuilder.CreateTable(
                name: "MchPayPassage",
                columns: table => new
                {
                    MchPayPassageId = table.Column<long>(type: "bigint", nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: false),
                    RiskConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchPayPassage", x => x.MchPayPassageId);
                });

            migrationBuilder.CreateTable(
                name: "OrderSnapshot",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    MchReqData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MchReqTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    MchRespData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MchRespTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    ChannelReqData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelReqTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    ChannelRespData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelRespTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2(6)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSnapshot", x => new { x.OrderId, x.OrderType });
                });

            migrationBuilder.CreateTable(
                name: "PayInterfaceConfig",
                columns: table => new
                {
                    PayInterfaceConfigId = table.Column<long>(type: "bigint", nullable: false),
                    InfoType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IfParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    SettHoldDay = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IfRate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: true),
                    IsOpenApplyment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOpenCashout = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOpenCheckBill = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IgnoreCheckBillMchNos = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    Oauth2InfoId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UpdatedUid = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayInterfaceConfig", x => x.PayInterfaceConfigId);
                });

            migrationBuilder.CreateTable(
                name: "PayInterfaceDefine",
                columns: table => new
                {
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IfName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsMchMode = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsIsvMode = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ConfigPageType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsSupportApplyment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOpenApplyment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSupportCheckBill = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOpenCheckBill = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSupportCashout = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOpenCashout = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsvParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    IsvsubMchParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    NormalMchParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    WayCodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BgColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayInterfaceDefine", x => x.IfCode);
                });

            migrationBuilder.CreateTable(
                name: "PayOrder",
                columns: table => new
                {
                    PayOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    QrcId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MchType = table.Column<int>(type: "int", nullable: false),
                    MchOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    MchFeeRate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: false),
                    MchFeeRateDesc = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MchFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    MchOrderFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "CNY"),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotifyState = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ClientIp = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SellerRemark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BuyerRemark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChannelMchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelIsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelExtra = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ChannelUser = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChannelOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PlatformOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PlatformMchOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    RefundState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RefundTimes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RefundAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    DivisionMode = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DivisionState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DivisionLastTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    ErrCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ErrMsg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExtParam = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NotifyUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    ReturnUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true, defaultValue: ""),
                    ExpiredTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    SuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayOrder", x => x.PayOrderId);
                });

            migrationBuilder.CreateTable(
                name: "PayRateConfig",
                columns: table => new
                {
                    PayRateConfigId = table.Column<long>(type: "bigint", nullable: false),
                    ConfigType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InfoType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: false),
                    LevelMode = table.Column<int>(type: "int", nullable: true),
                    FeeRate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: true),
                    ApplymentSupport = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayRateConfig", x => x.PayRateConfigId);
                });

            migrationBuilder.CreateTable(
                name: "PayWay",
                columns: table => new
                {
                    WayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayWay", x => x.WayCode);
                });

            migrationBuilder.CreateTable(
                name: "QrCode",
                columns: table => new
                {
                    QrCodeId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    QrConstantshellId = table.Column<int>(type: "int", nullable: true),
                    BatchId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FixedFlag = table.Column<bool>(type: "bit", nullable: false),
                    FixedPayAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EntryPage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlipayWayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QrcAlias = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BindState = table.Column<int>(type: "int", nullable: false),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    UrlPlaceholder = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCode", x => x.QrCodeId);
                });

            migrationBuilder.CreateTable(
                name: "QrCodeShell",
                columns: table => new
                {
                    QrCodeShellId = table.Column<long>(type: "bigint", nullable: false),
                    StyleCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShellAlias = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConfigInfo = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodeShell", x => x.QrCodeShellId);
                });

            migrationBuilder.CreateTable(
                name: "SysArticle",
                columns: table => new
                {
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ArticleType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ArticleRange = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PublishTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysArticle", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "SysConfig",
                columns: table => new
                {
                    ConfigKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ConfigName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfigDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GroupKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfigVal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "text"),
                    SortNum = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysConfig", x => new { x.ConfigKey, x.SysType, x.BelongInfoId });
                });

            migrationBuilder.CreateTable(
                name: "SysEntitlement",
                columns: table => new
                {
                    EntId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    EntName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MenuIcon = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MenuUri = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ComponentName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    EntType = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    QuickJump = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    MatchRule = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    State = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    Pid = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    EntSort = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysEntitlement", x => new { x.EntId, x.SysType });
                });

            migrationBuilder.CreateTable(
                name: "SysLog",
                columns: table => new
                {
                    SysLogId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Os = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Device = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BrowserInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    MethodName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    LogType = table.Column<byte>(type: "tinyint", nullable: false),
                    MethodRemark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    ReqUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValue: ""),
                    ReqMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OptReqParam = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false, defaultValue: ""),
                    OptResInfo = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false, defaultValue: ""),
                    ElapsedMs = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLog", x => x.SysLogId);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, defaultValue: "0"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleEntRela",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    EntId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleEntRela", x => new { x.RoleId, x.EntId });
                });

            migrationBuilder.CreateTable(
                name: "SysUser",
                columns: table => new
                {
                    SysUserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginUsername = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Realname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Telphone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AvatarUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UserNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SafeWord = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    InitUser = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    EntRules = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    BindStoreIds = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    InviteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    IsTeamLeader = table.Column<byte>(type: "tinyint", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, defaultValue: "0"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUser", x => x.SysUserId);
                });

            migrationBuilder.CreateTable(
                name: "SysUserAuth",
                columns: table => new
                {
                    AuthId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IdentityType = table.Column<int>(type: "int", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Credential = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserAuth", x => x.AuthId);
                });

            migrationBuilder.CreateTable(
                name: "SysUserLoginAttempt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IdentityType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Identifier = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    AttemptTime = table.Column<DateTime>(type: "datetime2(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserLoginAttempt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysUserRoleRela",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserRoleRela", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "SysUserTeam",
                columns: table => new
                {
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TeamNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StatRangeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SysType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BelongInfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, defaultValue: "0"),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserTeam", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrder",
                columns: table => new
                {
                    TransferId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchType = table.Column<int>(type: "int", nullable: false),
                    MchOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "CNY"),
                    AccountNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    TransferDesc = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    ClientIp = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChannelExtra = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ChannelOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ErrCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ErrMsg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExtParam = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NotifyUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    SuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrder", x => x.TransferId);
                });

            migrationBuilder.CreateTable(
                name: "MbrBill",
                columns: table => new
                {
                    MbrBillId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MbrNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MbrName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MbrTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    BeforeBalance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    ChangeAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AfterBalance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    BizType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbrBill", x => x.MbrBillId);
                    table.ForeignKey(
                        name: "FKMbrBillMbrNo",
                        column: x => x.MbrNo,
                        principalTable: "MbrInfo",
                        principalColumn: "MbrNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MbrRechargeRule",
                columns: table => new
                {
                    RuleId = table.Column<long>(type: "bigint", nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RechargeAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    GiveAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Sort = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbrRechargeRule", x => x.RuleId);
                    table.ForeignKey(
                        name: "FKMbrRechargeRuleMchNo",
                        column: x => x.MchNo,
                        principalTable: "MchInfo",
                        principalColumn: "MchNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MchApp",
                columns: table => new
                {
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, defaultValue: ""),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DefaultFlag = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AppSignType = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    AppSecret = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AppRsa2PublicKey = table.Column<string>(type: "nvarchar(448)", maxLength: 448, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchApp", x => x.AppId);
                    table.ForeignKey(
                        name: "FKMchAppMchNo",
                        column: x => x.MchNo,
                        principalTable: "MchInfo",
                        principalColumn: "MchNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MchStore",
                columns: table => new
                {
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, defaultValue: ""),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    StoreLogo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StoreOuterImg = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StoreInnerImg = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ProvinceCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CityCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DistrictCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Lng = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DefaultFlag = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BindAppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedUid = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MchStore", x => x.StoreId);
                    table.ForeignKey(
                        name: "FKMchStoreMchNo",
                        column: x => x.MchNo,
                        principalTable: "MchInfo",
                        principalColumn: "MchNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayOrderDivisionRecord",
                columns: table => new
                {
                    PayOrderDivisionRecordId = table.Column<long>(type: "bigint", nullable: false),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MchType = table.Column<int>(type: "int", nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PayOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PayOrderChannelOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PayOrderAmount = table.Column<long>(type: "bigint", nullable: false),
                    PayOrderDivisionAmount = table.Column<long>(type: "bigint", nullable: false),
                    BatchOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChannelBatchOrderId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    ChannelRespResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ReceiverAlias = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AccType = table.Column<int>(type: "int", nullable: false),
                    AccNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: ""),
                    RelationType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RelationTypeName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DivisionProfit = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: false),
                    CalDivisionAmount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayOrderDivisionRecord", x => x.PayOrderDivisionRecordId);
                    table.ForeignKey(
                        name: "FKPayOrderDivisionRecordPayOrderId",
                        column: x => x.PayOrderId,
                        principalTable: "PayOrder",
                        principalColumn: "PayOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayOrderProfit",
                columns: table => new
                {
                    PayOrderProfitId = table.Column<long>(type: "bigint", nullable: false),
                    InfoId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InfoName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InfoType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PayOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FeeRate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: false),
                    FeeRateDesc = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    OrderFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    ProfitRate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: false),
                    ProfitAmount = table.Column<long>(type: "bigint", nullable: false),
                    OrderProfitAmount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayOrderProfit", x => x.PayOrderProfitId);
                    table.ForeignKey(
                        name: "FKPayOrderProfitPayOrderId",
                        column: x => x.PayOrderId,
                        principalTable: "PayOrder",
                        principalColumn: "PayOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefundOrder",
                columns: table => new
                {
                    RefundOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PayOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChannelPayOrderNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MchNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MchShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AgentNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AgentShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsvNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsvShortName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MchType = table.Column<int>(type: "int", nullable: false),
                    MchRefundNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    WayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WayType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IfCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PayAmount = table.Column<long>(type: "bigint", nullable: false),
                    RefundAmount = table.Column<long>(type: "bigint", nullable: false),
                    RefundFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "CNY"),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ClientIp = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RefundReason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ChannelOrderNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ErrCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ErrMsg = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ChannelExtra = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    NotifyUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ExtParam = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SuccessTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "datetime2(6)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrder", x => x.RefundOrderId);
                    table.ForeignKey(
                        name: "FKRefundOrderPayOrderId",
                        column: x => x.PayOrderId,
                        principalTable: "PayOrder",
                        principalColumn: "PayOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayRateLevelConfig",
                columns: table => new
                {
                    PayRateLevelConfigId = table.Column<long>(type: "bigint", nullable: false),
                    RateConfigId = table.Column<long>(type: "bigint", nullable: false),
                    BankCardType = table.Column<int>(type: "int", nullable: true),
                    MinAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MinFee = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxFee = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FeeRate = table.Column<decimal>(type: "decimal(20,6)", precision: 20, scale: 6, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayRateLevelConfig", x => x.PayRateLevelConfigId);
                    table.ForeignKey(
                        name: "FKPayRateLevelConfigRateConfigId",
                        column: x => x.RateConfigId,
                        principalTable: "PayRateConfig",
                        principalColumn: "PayRateConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IdxCreatedOnUtc",
                table: "AccountBill",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IdxInfoIdInfoType",
                table: "AccountBill",
                columns: new[] { "InfoId", "InfoType" });

            migrationBuilder.CreateIndex(
                name: "UniBillId",
                table: "AccountBill",
                column: "BillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IdxPid",
                table: "AgentInfo",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IdxState",
                table: "AgentInfo",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "Idx_BatchNo",
                table: "ChannelBill",
                column: "BatchNo");

            migrationBuilder.CreateIndex(
                name: "Idx_OrderId",
                table: "ChannelBill",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "Idx_BindState",
                table: "DeviceInfo",
                column: "BindState");

            migrationBuilder.CreateIndex(
                name: "Idx_MchNo",
                table: "DeviceInfo",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "Uni_ProviderCode_DeviceType_DeviceNo",
                table: "DeviceInfo",
                columns: new[] { "ProviderCode", "DeviceType", "DeviceNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Uni_DeviceType_Provider",
                table: "DeviceProviderConfig",
                columns: new[] { "DeviceType", "ProviderCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Idx_BatchNo",
                table: "DiffBill",
                column: "BatchNo");

            migrationBuilder.CreateIndex(
                name: "Idx_HandleState",
                table: "DiffBill",
                column: "HandleState");

            migrationBuilder.CreateIndex(
                name: "IdxCreatedOnUtc",
                table: "MbrBill",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IdxMbrNo",
                table: "MbrBill",
                column: "MbrNo");

            migrationBuilder.CreateIndex(
                name: "IdxMbrTel",
                table: "MbrInfo",
                column: "MbrTel");

            migrationBuilder.CreateIndex(
                name: "IdxMchNo",
                table: "MbrInfo",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "IdxCreatedOnUtc",
                table: "MbrRechargeOrder",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IdxMbrNo",
                table: "MbrRechargeOrder",
                column: "MbrNo");

            migrationBuilder.CreateIndex(
                name: "IdxMchNo",
                table: "MbrRechargeOrder",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "IdxMchNo",
                table: "MbrRechargeRule",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "IdxMchNo",
                table: "MchApp",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "IdxMchNo",
                table: "MchApply",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "IdxState",
                table: "MchApply",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "Idx_GroupId",
                table: "MchDivisionReceiver",
                column: "ReceiverGroupId");

            migrationBuilder.CreateIndex(
                name: "Idx_MchNo",
                table: "MchDivisionReceiver",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "Idx_MchNo",
                table: "MchDivisionReceiverGroup",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "Uni_OrderId_OrderType",
                table: "MchNotifyRecord",
                columns: new[] { "OrderId", "OrderType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UniAppIdIfCodeWayCode",
                table: "MchPayPassage",
                columns: new[] { "AppId", "IfCode", "WayCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IdxMchNo",
                table: "MchStore",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "UniInfoTypeInfoIdIfCode",
                table: "PayInterfaceConfig",
                columns: new[] { "InfoType", "InfoId", "IfCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IdxCreatedOnUtc",
                table: "PayOrder",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IdxState",
                table: "PayOrder",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "UniMchNoMchOrderNo",
                table: "PayOrder",
                columns: new[] { "MchNo", "MchOrderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Idx_BatchOrderId",
                table: "PayOrderDivisionRecord",
                column: "BatchOrderId");

            migrationBuilder.CreateIndex(
                name: "Idx_PayOrderId",
                table: "PayOrderDivisionRecord",
                column: "PayOrderId");

            migrationBuilder.CreateIndex(
                name: "Idx_State",
                table: "PayOrderDivisionRecord",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IdxCreatedOnUtc",
                table: "PayOrderProfit",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_PayOrderProfit_PayOrderId",
                table: "PayOrderProfit",
                column: "PayOrderId");

            migrationBuilder.CreateIndex(
                name: "UniInfoIdInfoTypePayOrderId",
                table: "PayOrderProfit",
                columns: new[] { "InfoId", "InfoType", "PayOrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UniConfigTypeInfoTypeInfoIdIfCodeWayCode",
                table: "PayRateConfig",
                columns: new[] { "ConfigType", "InfoType", "InfoId", "IfCode", "WayCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayRateLevelConfig_RateConfigId",
                table: "PayRateLevelConfig",
                column: "RateConfigId");

            migrationBuilder.CreateIndex(
                name: "Idx_BindState",
                table: "QrCode",
                column: "BindState");

            migrationBuilder.CreateIndex(
                name: "Idx_MchNo",
                table: "QrCode",
                column: "MchNo");

            migrationBuilder.CreateIndex(
                name: "IdxPayOrderId",
                table: "RefundOrder",
                column: "PayOrderId");

            migrationBuilder.CreateIndex(
                name: "IdxState",
                table: "RefundOrder",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "UniMchNoMchRefundNo",
                table: "RefundOrder",
                columns: new[] { "MchNo", "MchRefundNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IdxPublishTime",
                table: "SysArticle",
                column: "PublishTime");

            migrationBuilder.CreateIndex(
                name: "IdxSysTypeBelongInfoIdGroupKey",
                table: "SysConfig",
                columns: new[] { "SysType", "BelongInfoId", "GroupKey" });

            migrationBuilder.CreateIndex(
                name: "IdxSysTypePid",
                table: "SysEntitlement",
                columns: new[] { "SysType", "Pid" });

            migrationBuilder.CreateIndex(
                name: "IdxCreatedOnUtc",
                table: "SysLog",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IdxSysTypeUserId",
                table: "SysLog",
                columns: new[] { "SysType", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IdxSysTypeBelongInfoId",
                table: "SysRole",
                columns: new[] { "SysType", "BelongInfoId" });

            migrationBuilder.CreateIndex(
                name: "IdxEntId",
                table: "SysRoleEntRela",
                column: "EntId");

            migrationBuilder.CreateIndex(
                name: "IdxTeamId",
                table: "SysUser",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "UniInviteCode",
                table: "SysUser",
                column: "InviteCode",
                unique: true,
                filter: "[InviteCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UniSysTypeLoginUsername",
                table: "SysUser",
                columns: new[] { "SysType", "LoginUsername" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UniSysTypeTelphone",
                table: "SysUser",
                columns: new[] { "SysType", "Telphone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UniSysTypeUserNo",
                table: "SysUser",
                columns: new[] { "SysType", "UserNo" },
                unique: true,
                filter: "[UserNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IdxUserId",
                table: "SysUserAuth",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UniSysTypeIdentityTypeIdentifier",
                table: "SysUserAuth",
                columns: new[] { "SysType", "IdentityType", "Identifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IdxSysTypeIdentifierAttemptTime",
                table: "SysUserLoginAttempt",
                columns: new[] { "SysType", "Identifier", "AttemptTime" });

            migrationBuilder.CreateIndex(
                name: "IdxRoleId",
                table: "SysUserRoleRela",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IdxSysTypeBelongInfoId",
                table: "SysUserTeam",
                columns: new[] { "SysType", "BelongInfoId" });

            migrationBuilder.CreateIndex(
                name: "UniTeamNo",
                table: "SysUserTeam",
                column: "TeamNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Idx_CreatedAt",
                table: "TransferOrder",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "Idx_State",
                table: "TransferOrder",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "Uni_MchNo_MchOrderNo",
                table: "TransferOrder",
                columns: new[] { "MchNo", "MchOrderNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBill");

            migrationBuilder.DropTable(
                name: "AgentInfo");

            migrationBuilder.DropTable(
                name: "ChannelBill");

            migrationBuilder.DropTable(
                name: "CheckBillBatch");

            migrationBuilder.DropTable(
                name: "DeviceInfo");

            migrationBuilder.DropTable(
                name: "DeviceProviderConfig");

            migrationBuilder.DropTable(
                name: "DeviceProviderDefine");

            migrationBuilder.DropTable(
                name: "DiffBill");

            migrationBuilder.DropTable(
                name: "IsvInfo");

            migrationBuilder.DropTable(
                name: "MbrBill");

            migrationBuilder.DropTable(
                name: "MbrRechargeOrder");

            migrationBuilder.DropTable(
                name: "MbrRechargeRule");

            migrationBuilder.DropTable(
                name: "MchApp");

            migrationBuilder.DropTable(
                name: "MchApply");

            migrationBuilder.DropTable(
                name: "MchDivisionReceiver");

            migrationBuilder.DropTable(
                name: "MchDivisionReceiverGroup");

            migrationBuilder.DropTable(
                name: "MchNotifyRecord");

            migrationBuilder.DropTable(
                name: "MchPayPassage");

            migrationBuilder.DropTable(
                name: "MchStore");

            migrationBuilder.DropTable(
                name: "OrderSnapshot");

            migrationBuilder.DropTable(
                name: "PayInterfaceConfig");

            migrationBuilder.DropTable(
                name: "PayInterfaceDefine");

            migrationBuilder.DropTable(
                name: "PayOrderDivisionRecord");

            migrationBuilder.DropTable(
                name: "PayOrderProfit");

            migrationBuilder.DropTable(
                name: "PayRateLevelConfig");

            migrationBuilder.DropTable(
                name: "PayWay");

            migrationBuilder.DropTable(
                name: "QrCode");

            migrationBuilder.DropTable(
                name: "QrCodeShell");

            migrationBuilder.DropTable(
                name: "RefundOrder");

            migrationBuilder.DropTable(
                name: "SysArticle");

            migrationBuilder.DropTable(
                name: "SysConfig");

            migrationBuilder.DropTable(
                name: "SysEntitlement");

            migrationBuilder.DropTable(
                name: "SysLog");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysRoleEntRela");

            migrationBuilder.DropTable(
                name: "SysUser");

            migrationBuilder.DropTable(
                name: "SysUserAuth");

            migrationBuilder.DropTable(
                name: "SysUserLoginAttempt");

            migrationBuilder.DropTable(
                name: "SysUserRoleRela");

            migrationBuilder.DropTable(
                name: "SysUserTeam");

            migrationBuilder.DropTable(
                name: "TransferOrder");

            migrationBuilder.DropTable(
                name: "MbrInfo");

            migrationBuilder.DropTable(
                name: "MchInfo");

            migrationBuilder.DropTable(
                name: "PayRateConfig");

            migrationBuilder.DropTable(
                name: "PayOrder");
        }
    }
}
