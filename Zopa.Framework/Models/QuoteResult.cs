namespace Zopa.Framework.Models
{
    public class QuoteResult
    {
        public Quote Quote { get; set; }
        public override string ToString()
        {
            return $"Requested amount: £{Quote.RequestedAmount} \n" +
                   $"Annual Interest Rate:{Quote.AnnualRate:0.0}% \n" +
                   $"Monthly repayment: £{Quote.Monthly:0.00} \n" +
                   $"Total repayment: £{Quote.Total:0.00} \n";
        }
    }

    public class FailedResult : QuoteResult
    {
        public override string ToString()
        {
            return "It is not possible to provide a quote.";
        }
    }
}