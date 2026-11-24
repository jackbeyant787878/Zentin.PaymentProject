using MediatR;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed class CreatePayOrderCommandHandler(
        IPayOrderRepository payOrderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreatePayOrderCommand, CreatePayOrderResult>
    {
        public async Task<CreatePayOrderResult> Handle(CreatePayOrderCommand request, CancellationToken cancellationToken)
        {
            // 供体 AbstractPayOrderController.cs:133 商户单号防重；DB 唯一索引 UniMchNoMchOrderNo 兜底并发窗口
            if (await payOrderRepository.IsExistByMchOrderNoAsync(request.MchNo, request.MchOrderNo, cancellationToken))
                throw new InvalidOperationException($"商户订单号已存在: {request.MchOrderNo}");

            // 供体 AmountUtil.cs:20：金额×费率四舍五入到分
            var feeAmount = (long)decimal.Round(request.Amount * request.MchFeeRate, 0, MidpointRounding.AwayFromZero);
            // 供体 SeqUtil.cs:25-33：订单号 = "P" + 雪花 ID
            var payOrderId = "P" + SnowflakeIdGenerator.GetSnowflakeId();

            var order = PayOrder.Create(
                payOrderId, request.MchNo, request.MchName, request.AppId, request.MchOrderNo,
                request.WayCode, request.WayType, request.Amount, request.MchFeeRate, feeAmount,
                request.Subject, request.Body, request.NotifyUrl, request.ExpiredTime,
                storeId: request.StoreId, clientIp: request.ClientIp,
                extParam: request.ExtParam, returnUrl: request.ReturnUrl);

            await payOrderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreatePayOrderResult(order.PayOrderId, order.State);
        }
    }
}