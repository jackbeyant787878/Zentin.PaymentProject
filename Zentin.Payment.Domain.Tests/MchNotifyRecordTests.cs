using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zentin.Payment.Domain.Entities.Notify;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Tests
{
    [TestClass]
    public sealed class MchNotifyRecordTests
    {
        private static MchNotifyRecord NewRecord(int limit = 6) => MchNotifyRecord.Create(
            orderId: "P123",
            orderType: NotifyOrderType.Pay,
            mchOrderNo: "M001",
            mchNo: "MCH001",
            appId: "APP001",
            notifyUrl: "https://merchant.example.com/notify",
            reqMethod: "POST",
            reqMediaType: "application/json",
            reqBody: "{\"a\":1}",
            notifyCountLimit: limit);

        [TestMethod]
        public void Create_initializes_state_and_raises_created_event()
        {
            var record = NewRecord();

            Assert.AreEqual(NotifyState.Notifying, record.State);
            Assert.AreEqual(0, record.NotifyCount);
            Assert.AreEqual(6, record.NotifyCountLimit);
            Assert.IsGreaterThan(0, record.NotifyId);
            var created = record.DomainEvents.OfType<MchNotifyCreatedDomainEvent>().Single();
            Assert.AreEqual(record.NotifyId, created.NotifyId);
        }

        [TestMethod]
        public void Create_rejects_invalid_arguments()
        {
            Assert.ThrowsExactly<ArgumentException>(() => MchNotifyRecord.Create(
                "", NotifyOrderType.Pay, "M001", "MCH001", "APP001", "https://x", "POST", "application/json"));
            Assert.ThrowsExactly<ArgumentException>(() => MchNotifyRecord.Create(
                "P1", NotifyOrderType.Pay, "M001", "", "APP001", "https://x", "POST", "application/json"));
            Assert.ThrowsExactly<ArgumentException>(() => MchNotifyRecord.Create(
                "P1", NotifyOrderType.Pay, "M001", "MCH001", "APP001", " ", "POST", "application/json"));
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => MchNotifyRecord.Create(
                "P1", NotifyOrderType.Pay, "M001", "MCH001", "APP001", "https://x", "POST", "application/json", notifyCountLimit: 0));
        }

        [TestMethod]
        public void RecordAttempt_increments_stores_response_and_schedules_retry()
        {
            var record = NewRecord();
            record.ClearDomainEvents();

            record.RecordAttempt("HTTP 500");

            Assert.AreEqual(1, record.NotifyCount);
            Assert.AreEqual("HTTP 500", record.ResResult);
            Assert.AreEqual(NotifyState.Notifying, record.State);
            Assert.IsNotNull(record.LastNotifyTime);
            var retry = record.DomainEvents.OfType<MchNotifyAttemptedDomainEvent>().Single();
            Assert.AreEqual(record.NotifyId, retry.NotifyId);
            Assert.AreEqual(30, retry.DelaySeconds);
        }

        [TestMethod]
        public void RecordAttempt_delay_grows_with_count()
        {
            var record = NewRecord();
            record.RecordAttempt("fail-1");
            record.ClearDomainEvents();

            record.RecordAttempt("fail-2");

            var retry = record.DomainEvents.OfType<MchNotifyAttemptedDomainEvent>().Single();
            Assert.AreEqual(60, retry.DelaySeconds);
        }

        [TestMethod]
        public void RecordAttempt_at_limit_fails_without_retry_event()
        {
            var record = NewRecord(limit: 1);
            record.ClearDomainEvents();

            record.RecordAttempt("timeout");

            Assert.AreEqual(NotifyState.Failed, record.State);
            Assert.AreEqual("timeout", record.ResResult);
            Assert.IsFalse(record.DomainEvents.OfType<MchNotifyAttemptedDomainEvent>().Any());
        }

        [TestMethod]
        public void RecordAttempt_rejects_non_notifying_state()
        {
            var record = NewRecord();
            record.MarkSuccess("SUCCESS");

            Assert.ThrowsExactly<InvalidOperationException>(() => record.RecordAttempt("late"));
        }

        [TestMethod]
        public void MarkSuccess_is_idempotent_under_redelivery()
        {
            var record = NewRecord();
            record.RecordAttempt("fail");
            record.MarkSuccess("SUCCESS");

            record.MarkSuccess("SUCCESS-again");

            Assert.AreEqual(NotifyState.Success, record.State);
            Assert.AreEqual("SUCCESS", record.ResResult);
        }

        [TestMethod]
        public void MarkFailed_stores_response()
        {
            var record = NewRecord();

            record.MarkFailed("connect error");

            Assert.AreEqual(NotifyState.Failed, record.State);
            Assert.AreEqual("connect error", record.ResResult);
        }
    }
}