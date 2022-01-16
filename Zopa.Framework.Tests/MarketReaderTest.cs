using System.Linq;
using Moq;
using Xunit;
using Zopa.Framework.Models;

namespace Zopa.Framework.Tests
{
    public class MarketReaderTest
    {
        [Theory]
        [InlineData("market1.csv", 2)]
        [InlineData("market2.csv", 7)]
        public void ShouldReadMarketCorrectly(string filename, int expectedAmount)
        { 
            var configMoq = new Mock<IConfig>();
            configMoq.Setup(x => x.MarketFile).Returns($"TestData/{filename}");

            var marketReader = new CsvMarketReader(configMoq.Object);
            var market = marketReader.ReadMarket();

            Assert.Equal(market.Count(), expectedAmount);
            Assert.Contains(market, l => l.Lender == "Fred" && l.Rate == 0.071m && l.Available == 520);
        }
    }
} 