using System.Linq;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class RefundOrderTests
    {
        private static RefundOrder NewRefund(long refundAmount = 4000, long payAmount = 10000) => RefundOrder.Create(
            refundOrderId: "R123456789",
            payOrderId: "P123456789",
            mchNo: "M1001",
            mchName: "测试商户",
            appId: "APP001",
            mchRefundNo: "MR-001",
            wayCode: "WX_NATIVE",
            wayType: "WX",
            ifCode: "wxpay",
            payAmount: payAmount,
            refundAmount: refundAmount,
            refundFeeAmount: 0,
            refundReason: "测试退款");

        [TestMethod]
        public void Create_initializes_state_and_guards()
        {
            var refund = NewRefund();

            Assert.AreEqual(RefundOrderStatus.Created, refund.State);
            Assert.AreEqual(4000, refund.RefundAmount);
            Assert.AreEqual(10000, refund.PayAmount);

            Assert.ThrowsExactly<ArgumentException>(() => RefundOrder.Create("", "P1", "M", "N", "A", "MR", "WX", "WX", "wx", 100, 50, 0, "r"));
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => NewRefund(refundAmount: 0));
            Assert.ThrowsExactly<InvalidOperationException>(() => NewRefund(refundAmount: 10001));
        }

        [TestMethod]
        public void MarkProcessing_requires_created_state()
        {
            var refund = NewRefund();
            refund.MarkProcessing("CH-REFUND-1");

            Assert.AreEqual(RefundOrderStatus.Processing, refund.State);
            Assert.AreEqual("CH-REFUND-1", refund.ChannelOrderNo);

            Assert.ThrowsExactly<InvalidOperationException>(() => refund.MarkProcessing("CH-REFUND-2"));
        }

        [TestMethod]
        public void MarkSuccess_transitions_and_raises_event()
        {
            var refund = NewRefund();
            refund.MarkProcessing("CH-REFUND-1");

            refund.MarkSuccess();

            Assert.AreEqual(RefundOrderStatus.Success, refund.State);
            Assert.IsNotNull(refund.SuccessTime);
            var evt = refund.DomainEvents.OfType<RefundSuccessDomainEvent>().Single();
            Assert.AreEqual(refund.RefundOrderId, evt.RefundOrderId);
            Assert.AreEqual(refund.PayOrderId, evt.PayOrderId);
            Assert.AreEqual(refund.RefundAmount, evt.RefundAmount);
        }

        [TestMethod]
        public void MarkSuccess_is_idempotent_under_callback_redelivery()
        {
            var refund = NewRefund();
            refund.MarkProcessing("CH-REFUND-1");
            refund.MarkSuccess();
            refund.ClearDomainEvents();

            refund.MarkSuccess();

            Assert.AreEqual(RefundOrderStatus.Success, refund.State);
            Assert.IsFalse(refund.DomainEvents.OfType<RefundSuccessDomainEvent>().Any());
        }

        [TestMethod]
        public void MarkSuccess_rejects_non_processing_state()
        {
            var refund = NewRefund();
            Assert.ThrowsExactly<InvalidOperationException>(() => refund.MarkSuccess());
        }

        [TestMethod]
        public void MarkFailed_transitions_and_is_idempotent()
        {
            var refund = NewRefund();
            refund.MarkProcessing("CH-REFUND-1");

            refund.MarkFailed("E01", "渠道失败");

            Assert.AreEqual(RefundOrderStatus.Failed, refund.State);
            Assert.AreEqual("E01", refund.ErrCode);
            Assert.AreEqual("渠道失败", refund.ErrMsg);

            refund.MarkFailed("E02", "重复回调");
            Assert.AreEqual(RefundOrderStatus.Failed, refund.State);
            Assert.AreEqual("E01", refund.ErrCode);
        }

        [TestMethod]
        public void MarkFailed_rejects_non_processing_state()
        {
            var refund = NewRefund();
            Assert.ThrowsExactly<InvalidOperationException>(() => refund.MarkFailed("E01", "x"));

            var succeeded = NewRefund();
            succeeded.MarkProcessing("CH-REFUND-1");
            succeeded.MarkSuccess();
            Assert.ThrowsExactly<InvalidOperationException>(() => succeeded.MarkFailed("E01", "x"));
        }

        [TestMethod]
        public void Close_allows_created_and_processing_but_not_terminal()
        {
            var created = NewRefund();
            created.Close();
            Assert.AreEqual(RefundOrderStatus.Closed, created.State);
            created.Close(); // 幂等
            Assert.AreEqual(RefundOrderStatus.Closed, created.State);

            var processing = NewRefund();
            processing.MarkProcessing("CH-REFUND-1");
            processing.Close();
            Assert.AreEqual(RefundOrderStatus.Closed, processing.State);

            var succeeded = NewRefund();
            succeeded.MarkProcessing("CH-REFUND-1");
            succeeded.MarkSuccess();
            Assert.ThrowsExactly<InvalidOperationException>(() => succeeded.Close());
        }
    }
}