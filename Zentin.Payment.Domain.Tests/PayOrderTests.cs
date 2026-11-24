using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class PayOrderTests
    {
        private static PayOrder CreateOrder(long amount = 10000) => PayOrder.Create(
            payOrderId: "P123456789",
            mchNo: "M1001",
            mchName: "测试商户",
            appId: "APP001",
            mchOrderNo: "MCH-ORDER-001",
            wayCode: "WX_NATIVE",
            wayType: "WX",
            amount: amount,
            mchFeeRate: 0.0055m,
            mchFeeAmount: 55,
            subject: "测试商品",
            body: "测试商品描述",
            notifyUrl: "https://merchant.example.com/notify",
            expiredTime: DateTime.UtcNow.AddMinutes(30));

        [TestMethod]
        public void Create_initializes_state_and_guards()
        {
            var order = CreateOrder();

            Assert.AreEqual(PayOrderStatus.Created, order.State);
            Assert.IsFalse(order.NotifyState);
            Assert.AreEqual(RefundStatus.NoRefund, order.RefundState);
            Assert.AreEqual(0, order.RefundAmount);

            Assert.ThrowsExactly<ArgumentException>(() => CreateOrder(amount: 0));
            Assert.ThrowsExactly<ArgumentException>(() => PayOrder.Create(
                "P1", "M1001", "商户", "APP001", "MO1", "WX_NATIVE", "WX",
                100, 1.5m, 1, "s", "b", "https://n", DateTime.UtcNow));
            Assert.ThrowsExactly<ArgumentException>(() => PayOrder.Create(
                "P1", "M1001", "商户", "APP001", "MO1", "WX_NATIVE", "WX",
                100, -0.01m, 1, "s", "b", "https://n", DateTime.UtcNow));
            Assert.ThrowsExactly<ArgumentException>(() => PayOrder.Create(
                "P1", "M1001", "商户", "APP001", "MO1", "WX_NATIVE", "WX",
                100, 0.0055m, -1, "s", "b", "https://n", DateTime.UtcNow));
            Assert.ThrowsExactly<ArgumentException>(() => PayOrder.Create(
                "", "M1001", "商户", "APP001", "MO1", "WX_NATIVE", "WX",
                100, 0.0055m, 1, "s", "b", "https://n", DateTime.UtcNow));
        }

        [TestMethod]
        public void MarkPaying_requires_created_state()
        {
            var order = CreateOrder();
            order.MarkPaying("wxpay");

            Assert.AreEqual(PayOrderStatus.Paying, order.State);
            Assert.AreEqual("wxpay", order.IfCode);
            Assert.ThrowsExactly<InvalidOperationException>(() => order.MarkPaying("wxpay"));
        }

        [TestMethod]
        public void MarkSuccess_requires_paying_and_raises_paid_event()
        {
            var order = CreateOrder();
            Assert.ThrowsExactly<InvalidOperationException>(() => order.MarkSuccess("CH-001"));

            order.MarkPaying("wxpay");
            order.MarkSuccess("CH-001", channelUser: "openid-1", platformOrderNo: "PLAT-001");

            Assert.AreEqual(PayOrderStatus.Success, order.State);
            Assert.AreEqual("CH-001", order.ChannelOrderNo);
            Assert.AreEqual("openid-1", order.ChannelUser);
            Assert.AreEqual("PLAT-001", order.PlatformOrderNo);
            Assert.IsNotNull(order.SuccessTime);

            var paid = order.DomainEvents.OfType<PayOrderPaidDomainEvent>().Single();
            Assert.AreEqual(order.PayOrderId, paid.PayOrderId);
            Assert.AreEqual(order.Amount, paid.Amount);
        }

        [TestMethod]
        public void MarkSuccess_is_idempotent_on_duplicate_callback()
        {
            var order = CreateOrder();
            order.MarkPaying("wxpay");
            order.MarkSuccess("CH-001");
            order.ClearDomainEvents();

            order.MarkSuccess("CH-002");

            Assert.AreEqual("CH-001", order.ChannelOrderNo);
            Assert.IsFalse(order.DomainEvents.Any());
        }

        [TestMethod]
        public void MarkFail_requires_paying_and_is_idempotent()
        {
            var order = CreateOrder();
            Assert.ThrowsExactly<InvalidOperationException>(() => order.MarkFail("E01", "余额不足"));

            order.MarkPaying("wxpay");
            order.MarkFail("E01", "余额不足");

            Assert.AreEqual(PayOrderStatus.Failed, order.State);
            Assert.AreEqual("E01", order.ErrCode);

            order.MarkFail("E02", "重复回调");
            Assert.AreEqual("E01", order.ErrCode);
        }

        [TestMethod]
        public void Close_only_allowed_before_payment_result()
        {
            var created = CreateOrder();
            created.Close("用户取消");
            Assert.AreEqual(PayOrderStatus.Closed, created.State);
            created.Close("重复关单");

            var paying = CreateOrder();
            paying.MarkPaying("wxpay");
            paying.Close();
            Assert.AreEqual(PayOrderStatus.Closed, paying.State);

            var paid = CreateOrder();
            paid.MarkPaying("wxpay");
            paid.MarkSuccess("CH-001");
            Assert.ThrowsExactly<InvalidOperationException>(() => paid.Close());

            var failed = CreateOrder();
            failed.MarkPaying("wxpay");
            failed.MarkFail("E01", "失败");
            Assert.ThrowsExactly<InvalidOperationException>(() => failed.Close());
        }

        [TestMethod]
        public void RecordRefund_enforces_cumulative_limit_and_full_refund_state()
        {
            var order = CreateOrder(amount: 10000);
            Assert.ThrowsExactly<InvalidOperationException>(() => order.RecordRefund(100));

            order.MarkPaying("wxpay");
            order.MarkSuccess("CH-001");

            Assert.ThrowsExactly<ArgumentException>(() => order.RecordRefund(0));
            Assert.ThrowsExactly<InvalidOperationException>(() => order.RecordRefund(10001));

            order.RecordRefund(4000);
            Assert.AreEqual(RefundStatus.Partial, order.RefundState);
            Assert.AreEqual(4000, order.RefundAmount);
            Assert.AreEqual(1, order.RefundTimes);
            Assert.AreEqual(PayOrderStatus.Success, order.State);

            Assert.ThrowsExactly<InvalidOperationException>(() => order.RecordRefund(6001));

            order.RecordRefund(6000);
            Assert.AreEqual(RefundStatus.Full, order.RefundState);
            Assert.AreEqual(PayOrderStatus.Refunded, order.State);
            Assert.AreEqual(10000, order.RefundAmount);
        }

        [TestMethod]
        public void RecordRefund_deducts_fee_amount_and_guards_negative_fee()
        {
            var order = CreateOrder(amount: 10000);
            order.MarkPaying("wxpay");
            order.MarkSuccess("CH-001");

            Assert.AreEqual(55, order.MchFeeAmount);
            order.RecordRefund(4000, refundFeeAmount: 20);
            Assert.AreEqual(35, order.MchFeeAmount);

            Assert.ThrowsExactly<ArgumentException>(() => order.RecordRefund(100, refundFeeAmount: -1));
        }
    }
}