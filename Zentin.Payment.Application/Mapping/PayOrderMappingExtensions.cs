using Zentin.Payment.Application.Dto;
using Zentin.Payment.Domain.Entities.Order;

namespace Zentin.Payment.Application.Mapping
{
    public static class PayOrderMappingExtensions
    {
        public static PayOrderDto ToDto(this PayOrder order) => new()
        {
            PayOrderId = order.PayOrderId,
            MchNo = order.MchNo,
            MchName = order.MchName,
            MchShortName = order.MchShortName!,
            AgentNo = order.AgentNo!,
            AgentName = order.AgentName!,
            AgentShortName = order.AgentShortName!,
            IsvNo = order.IsvNo!,
            IsvName = order.IsvName!,
            IsvShortName = order.IsvShortName!,
            AppId = order.AppId,
            AppName = order.AppName!,
            StoreId = order.StoreId,
            StoreName = order.StoreName!,
            QrcId = order.QrcId!,
            MchType = (byte)order.MchType,
            MchOrderNo = order.MchOrderNo,
            IfCode = order.IfCode!,
            WayCode = order.WayCode,
            WayType = order.WayType,
            Amount = order.Amount,
            MchFeeRate = order.MchFeeRate,
            MchFeeRateDesc = order.MchFeeRateDesc!,
            MchFeeAmount = order.MchFeeAmount,
            MchOrderFeeAmount = order.MchOrderFeeAmount,
            Currency = order.Currency,
            State = (byte)order.State,
            NotifyState = (byte)(order.NotifyState ? 1 : 0),
            ClientIp = order.ClientIp!,
            Subject = order.Subject,
            Body = order.Body,
            SellerRemark = order.SellerRemark!,
            BuyerRemark = order.BuyerRemark!,
            ChannelMchNo = order.ChannelMchNo!,
            ChannelIsvNo = order.ChannelIsvNo!,
            ChannelExtra = order.ChannelExtra!,
            ChannelUser = order.ChannelUser!,
            ChannelOrderNo = order.ChannelOrderNo!,
            PlatformOrderNo = order.PlatformOrderNo!,
            PlatformMchOrderNo = order.PlatformMchOrderNo!,
            RefundState = (byte)order.RefundState,
            RefundTimes = order.RefundTimes,
            RefundAmount = order.RefundAmount,
            DivisionMode = (byte)order.DivisionMode,
            DivisionState = (byte)order.DivisionState,
            DivisionLastTime = order.DivisionLastTime,
            ErrCode = order.ErrCode!,
            ErrMsg = order.ErrMsg!,
            ExtParam = order.ExtParam!,
            NotifyUrl = order.NotifyUrl,
            ReturnUrl = order.ReturnUrl!,
            ExpiredTime = order.ExpiredTime,
            SuccessTime = order.SuccessTime,
            CreatedAt = order.CreatedOnUtc,
            UpdatedAt = order.UpdatedOnUtc
        };
    }
}