using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Zopa.Framework.Models;

namespace Zopa.Framework
{
    public interface IMarketReader
    {
        IEnumerable<LenderDetails> ReadMarket();
    }

    public class CsvMarketReader: IMarketReader
    {
        private readonly IConfig _config;

        public CsvMarketReader(IConfig config)
        {
            _config = config;
        }

        public IEnumerable<LenderDetails> ReadMarket()
        {
            using var reader = new StreamReader(_config.MarketFile);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<LenderDetails>().ToArray();
        }
    }
}