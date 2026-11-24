using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Contracts.IntegrationEvents;
using Zentin.Payment.Application.Services;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Notify;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Consumers
{
    /// <summary>
    /// Creates a merchant notify record when a pay order is paid.
    /// Ported from donor PayMchNotifyService.PayOrderNotifyAsync.
    /// </summary>
    public sealed class PayOrderPaidNotifyConsumer
    {
        private readonly IPayOrderRepository _payOrderRepository;
        private readonly IMchNotifyRecordRepository _mchNotifyRecordRepository;
        private readonly IMchAppRepository _mchAppRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayOrderPaidNotifyConsumer> _logger;

        public PayOrderPaidNotifyConsumer(
            IPayOrderRepository payOrderRepository,
            IMchNotifyRecordRepository mchNotifyRecordRepository,
            IMchAppRepository mchAppRepository,
            IUnitOfWork unitOfWork,
            ILogger<PayOrderPaidNotifyConsumer> logger)
        {
            _payOrderRepository = payOrderRepository;
            _mchNotifyRecordRepository = mchNotifyRecordRepository;
            _mchAppRepository = mchAppRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [CapSubscribe(IntegrationEventTopics.PayOrderPaid)]
        public async Task HandleAsync(PayOrderPaidIntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            var order = await _payOrderRepository.GetByIdAsync(@event.PayOrderId, cancellationToken);
            if (order == null || string.IsNullOrWhiteSpace(order.NotifyUrl))
                return;

            var existing = await _mchNotifyRecordRepository.GetByOrderAsync(
                order.PayOrderId, (int)NotifyOrderType.Pay, cancellationToken);
            if (existing != null)
            {
                _logger.LogInformation("通知记录已存在，跳过: {OrderId}", order.PayOrderId);
                return;
            }

            var mchApp = await _mchAppRepository.GetByMchNoAndAppIdAsync(order.MchNo, order.AppId, cancellationToken)
                ?? throw new KeyNotFoundException($"商户应用不存在: {order.MchNo}/{order.AppId}");

            var body = MchNotifyPayloadBuilder.BuildSignedJsonBody(order, mchApp.AppSecret);
            var record = MchNotifyRecord.Create(
                order.PayOrderId, NotifyOrderType.Pay, order.MchOrderNo, order.MchNo, order.AppId,
                order.NotifyUrl, "POST", "application/json", body, order.AgentNo, order.IsvNo);

            // 并发重复由唯一索引 Uni_OrderId_OrderType 兜底；异常交给 CAP 重试，重跑时命中上面的去重检查
            await _mchNotifyRecordRepository.AddAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}