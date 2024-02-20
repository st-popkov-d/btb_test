namespace Bloggy.WebApi.Models
{
    public class AmortizationSchedule
    {
        public int Payment { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
    }
}
