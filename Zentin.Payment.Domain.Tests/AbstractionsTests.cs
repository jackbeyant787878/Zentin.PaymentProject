using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class AbstractionsTests
    {
        [TestMethod]
        public void DomainNotification_occurrence_time_is_fixed_at_construction()
        {
            var notification = new DomainNotification("key", "message");
            var first = notification.OccurredOnUtc;

            Thread.Sleep(20);

            Assert.AreEqual(first, notification.OccurredOnUtc);
        }
    }
}