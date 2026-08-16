using Zentin.Payment.Domain.Abstractions;
namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class PayWay : BaseEntity
    {
        public string WayCode { get; private set; } = string.Empty;
        public string WayName { get; private set; } = string.Empty;
        public string WayType { get; private set; } = string.Empty;

        public bool State { get; private set; }


        private PayWay() { }


        /// <summary>
        /// create a new payment method
        /// </summary>
        public static PayWay Create(string wayCode, string wayName,string wayType,bool state = true)
        {
            if (string.IsNullOrWhiteSpace(wayCode))
                throw new ArgumentException("Payment Way Code cannot be null or empty", nameof(wayCode));
            if (string.IsNullOrWhiteSpace(wayName))
                throw new ArgumentException("Payment Way Name cannot be null or empty", nameof(wayName));
            if (string.IsNullOrWhiteSpace(wayType))
                throw new ArgumentException("Payment Type cannot be null or empty", nameof(wayType));

            return new PayWay
            {
                WayCode = wayCode,
                WayName = wayName,
                WayType = wayType,
                State = state
            };
        }

        // ========== business methods ==========
        /// <summary>
        /// update basic information
        /// </summary>
        public void UpdateInfo(string wayName, string wayType)
        {
            if (string.IsNullOrWhiteSpace(wayName))
                throw new ArgumentException("Payment Way Name cannot be null or empty", nameof(wayName));
            if (string.IsNullOrWhiteSpace(wayType))
                throw new ArgumentException("Payment Type cannot be null or empty", nameof(wayType));

            WayName = wayName;
            WayType = wayType;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;

    }
}
