using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Contracts.IntegrationEvents;
using Zentin.Payment.Application.Contracts.Ports;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Consumers
{
    /// <summary>
    /// Sends one merchant notify attempt; retries are scheduled by the aggregate via delayed outbox messages.
    /// Ported from donor PayOrderMchNotifyMQReceiver.
    /// </summary>
    public sealed class MchNotifyDispatchConsumer
    {
        private readonly IMchNotifyRecordRepository _mchNotifyRecordRepository;
        private readonly IPayOrderRepository _payOrderRepository;
        private readonly IMchNotifySender _sender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MchNotifyDispatchConsumer> _logger;

        public MchNotifyDispatchConsumer(
            IMchNotifyRecordRepository mchNotifyRecordRepository,
            IPayOrderRepository payOrderRepository,
            IMchNotifySender sender,
            IUnitOfWork unitOfWork,
            ILogger<MchNotifyDispatchConsumer> logger)
        {
            _mchNotifyRecordRepository = mchNotifyRecordRepository;
            _payOrderRepository = payOrderRepository;
            _sender = sender;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [CapSubscribe(IntegrationEventTopics.MchNotifyDispatch)]
        public async Task HandleAsync(MchNotifyDispatchIntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            var record = await _mchNotifyRecordRepository.GetByIdAsync(@event.NotifyId, cancellationToken);
            if (record == null || record.State != NotifyState.Notifying || record.NotifyCount >= record.NotifyCountLimit)
            {
                _logger.LogInformation("通知记录不存在或已终态，跳过: {NotifyId}", @event.NotifyId);
                return;
            }

            var res = await _sender.SendAsync(record.NotifyUrl, record.ReqMethod, record.ReqMediaType, record.ReqBody, cancellationToken);

            // 首次尝试无论成败都回写 PayOrder.NotifyState（供体 PayOrderMchNotifyMQReceiver.cs:95-98）
            if (record.NotifyCount == 0 && record.OrderType == NotifyOrderType.Pay)
            {
                var order = await _payOrderRepository.GetByIdAsync(record.OrderId, cancellationToken);
                order?.UpdateNotifyState(true);
            }

            if ("SUCCESS".Equals(res, StringComparison.OrdinalIgnoreCase))
                record.MarkSuccess(res);
            else
                record.RecordAttempt(res);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}