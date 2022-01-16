namespace Zopa.Framework.Models
{
    public class LenderDetails
    {
        public string Lender { get; set; }
        public decimal Rate { get; set; }
        public decimal Available { get; set; }
    }

    public class LenderLoan
    {
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal Monthly { get; set; }
    }

    public class Quote
    {
        public decimal RequestedAmount { get; set; }
        public decimal AnnualRate { get; set; }
        public decimal Monthly { get; set; }
        public decimal Total { get; set; }
    }
}