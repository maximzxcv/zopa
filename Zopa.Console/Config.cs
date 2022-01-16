using CsvHelper;
using Zopa.Framework.Models;

namespace Zopa.Console
{
    public class Config: IConfig
    {
        public string MarketFile { get; set; }
    }
}
