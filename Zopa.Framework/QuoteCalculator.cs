using System;
using System.Collections.Generic;
using System.Linq;
using Zopa.Framework.Models;

namespace Zopa.Framework
{
    public interface IQuoteCalculator
    {
        QuoteResult Calculate(decimal amount, IEnumerable<LenderDetails> market, int periodMonths);
    }

    public class QuoteCalculator : IQuoteCalculator
    {
        public QuoteResult Calculate(decimal amountToLoan, IEnumerable<LenderDetails> market, int periodMonths)
        {
            if (amountToLoan < 1 || periodMonths < 1) return new FailedResult();
            if (market == null) return new FailedResult();

            var lenderLoans = new List<LenderLoan>();
            foreach (var lender in market.OrderBy(l => l.Rate).ThenByDescending(l => l.Available))
            {
                var lenderAmount = lender.Available <= amountToLoan ? lender.Available : amountToLoan;
                lenderLoans.Add(CalculateLenderLoan(lender.Rate, lenderAmount, periodMonths));
                amountToLoan -= lenderAmount;

                if (amountToLoan == 0) // found all required lenders
                    return new QuoteResult {Quote = CalculatedQuote(lenderLoans, periodMonths)};
            }

            return new FailedResult();
        }

        #region private mthds

        private LenderLoan CalculateLenderLoan(decimal rate, decimal amount, int periodMonths)
        {
            decimal MonthlyFormula()
            {
                var i = (decimal) Math.Pow(1 + (double) rate, 1d / 12) - 1; // periodic interest rate
                var r = (decimal) Math.Pow(1 + (double) i, periodMonths) - 1; // (1 + i)^n - 1 
                r = 1m / r + 1;
                return amount * i * r;
            }

            return new LenderLoan
            {
                Amount = amount,
                Monthly = rate == 0 ? amount / periodMonths : MonthlyFormula(),
                Rate = rate
            };
        }

        private Quote CalculatedQuote(IEnumerable<LenderLoan> loans, int periodMonths)
        {
            var quote = new Quote();
            quote.RequestedAmount = loans.Sum(l => l.Amount);
            quote.Monthly = loans.Sum(l => l.Monthly);
            quote.Total = quote.Monthly * periodMonths;
            quote.AnnualRate = loans.Sum(l => l.Amount / quote.RequestedAmount * l.Rate * 100);

            return quote;
        }

        #endregion
    }
}
