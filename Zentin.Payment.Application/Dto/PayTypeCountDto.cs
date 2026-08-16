namespace Zentin.Payment.Application.Dto
{
    public class PayTypeCountDto
    {
        public string WayType { get; set; }
        public string TypeName { get; set; }
        public int TypeCount { get; set; }
        public decimal TypeAmount { get; set; }
    }
}
