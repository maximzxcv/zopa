using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Zopa.Console.Validation;
using Zopa.Framework;

namespace Zopa.Console
{
    public class QuoteProgram
    {
        private readonly IMarketReader _marketReader;
        private readonly IQuoteCalculator _quoteCalculator;
        private const int PeriodMonths = 36;

        public const string ExtendedText = "\nRemarks:\n  To change market update Data/market.csv file.\n";
        public const string Description = "\nCalculates quote from Zopa’s market of lenders for 36-month loans.";

        [Argument(1, "loan_amount", "Requested amount. \nValue must be in the [1000;15000] range and multiple to 100.")]
        [Required, Range(1000, 15000), MultipleTo(100)]
        public int? LoanAmount { get; }

        public QuoteProgram(IMarketReader marketReader, IQuoteCalculator quoteCalculator)
        {
            _marketReader = marketReader;
            _quoteCalculator = quoteCalculator;
        }

        private void OnExecute()
        { 
            var market = _marketReader.ReadMarket();
            var q = _quoteCalculator.Calculate(LoanAmount ?? 0, market, PeriodMonths); 

            System.Console.ForegroundColor = q.Quote == null ? ConsoleColor.Red: ConsoleColor.DarkGreen;
            System.Console.WriteLine(q);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}