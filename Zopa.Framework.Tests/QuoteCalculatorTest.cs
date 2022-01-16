using System;
using System.Collections.Generic;
using Xunit;
using Zopa.Framework.Models;

namespace Zopa.Framework.Tests
{
    public class QuoteCalculatorTest
    {
        [Fact]
        public void ShouldCalculateQuoteCorrectlyForOneLender()
        {
            var periodMonth = 36;
            var market = new List<LenderDetails>
            {
                new LenderDetails {Available = 500, Lender = "LenderName", Rate = 0.07m}
            };

            var quoteCalculator = new QuoteCalculator();
            var q = quoteCalculator.Calculate(500, market, periodMonth);

            Assert.Equal(500, q.Quote.RequestedAmount);
            Assert.Equal(7, q.Quote.AnnualRate);
            Assert.Equal(15.39m, Math.Round(q.Quote.Monthly, 2));
            Assert.Equal(q.Quote.Monthly * periodMonth, q.Quote.Total);
        }

        [Fact]
        public void ShouldCalculateQuoteCorrectlyForTwoLenders()
        {
            var periodMonth = 36;
            var market = new List<LenderDetails>
            {
                new LenderDetails {Available = 500, Lender = "LenderName", Rate = 0.07m},
                new LenderDetails {Available = 500, Lender = "LenderName", Rate = 0.05m}
            };

            var quoteCalculator = new QuoteCalculator();
            var q = quoteCalculator.Calculate(1000, market, periodMonth);

            Assert.Equal(1000, q.Quote.RequestedAmount);
            Assert.Equal(6, q.Quote.AnnualRate);
            Assert.Equal(30.35m, Math.Round(q.Quote.Monthly, 2));
            Assert.Equal(q.Quote.Monthly * periodMonth, q.Quote.Total);
        }

        [Theory]
        [InlineData(1000, 7, 30.78)]
        [InlineData(1700, 7.2, 52.46)]
        public void ShouldCalculateQuoteCorrectlyForManyLenders(decimal amount, decimal expectedRate, decimal expectedMonthly)
        {
            var periodMonth = 36;
            var market = new List<LenderDetails>
            {
                new LenderDetails {Available = 640, Lender = "LenderName", Rate = 0.075m},
                new LenderDetails {Available = 480, Lender = "LenderName", Rate = 0.069m},
                new LenderDetails {Available = 520, Lender = "LenderName", Rate = 0.071m},
                new LenderDetails {Available = 170, Lender = "LenderName", Rate = 0.104m},
                new LenderDetails {Available = 320, Lender = "LenderName", Rate = 0.081m},
                new LenderDetails {Available = 140, Lender = "LenderName", Rate = 0.074m},
                new LenderDetails {Available = 60, Lender = "LenderName", Rate = 0.071m} 
            };

            var quoteCalculator = new QuoteCalculator();
            var q = quoteCalculator.Calculate(amount, market, periodMonth);

            Assert.Equal(amount, q.Quote.RequestedAmount);
            Assert.Equal(expectedRate, Math.Round(q.Quote.AnnualRate,1));
            Assert.Equal(expectedMonthly, Math.Round(q.Quote.Monthly, 2));
            Assert.Equal(q.Quote.Monthly * periodMonth, q.Quote.Total);
        }

        [Fact]
        public void ShouldCalculateQuoteCorrectlyForZeroRate()
        {
            var periodMonth = 5;
            var market = new List<LenderDetails>
            {
                new LenderDetails {Available = 500, Lender = "LenderName", Rate = 0}
            };

            var quoteCalculator = new QuoteCalculator();
            var q = quoteCalculator.Calculate(500, market, periodMonth);

            Assert.Equal(500, q.Quote.RequestedAmount);
            Assert.Equal(0, q.Quote.AnnualRate);
            Assert.Equal(100, q.Quote.Monthly);
            Assert.Equal(q.Quote.Monthly * periodMonth, q.Quote.Total);
        }

        [Fact]
        public void ShouldChooseLenderWithLowestRate()
        {
            var periodMonth = 36;
            var market = new List<LenderDetails>
            {
                new LenderDetails {Available = 500, Lender = "LenderName", Rate = 0.07m},
                new LenderDetails {Available = 500, Lender = "LenderName", Rate = 0.06m}
            };

            var quoteCalculator = new QuoteCalculator();
            var q = quoteCalculator.Calculate(500, market, periodMonth);
             
            Assert.Equal(6m, q.Quote.AnnualRate);
        }

        [Fact]
        public void ShouldFailToGenerateQuote()
        {
            var periodMonth = 36;
            var market = new List<LenderDetails>
            {
                new LenderDetails {Available = 200, Lender = "LenderName", Rate = 0.07m},
                new LenderDetails {Available = 300, Lender = "LenderName", Rate = 0.07m}
            };

            var quoteCalculator = new QuoteCalculator();
            var q = quoteCalculator.Calculate(501, market, periodMonth);

            Assert.Null(q.Quote);
            Assert.IsType<FailedResult>(q);
        }
    }
} 