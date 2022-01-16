using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zopa.Framework;
using Zopa.Framework.Models;

namespace Zopa.Console
{
    class Program
    {
        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            var services = new ServiceCollection()
                .AddSingleton(BuildConfiguration())
                .AddSingleton<IMarketReader, CsvMarketReader>()
                .AddSingleton<IQuoteCalculator, QuoteCalculator>()
                .BuildServiceProvider();

            var app = new CommandLineApplication<QuoteProgram>
            {
                Description = QuoteProgram.Description,
                ExtendedHelpText = QuoteProgram.ExtendedText
            };
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            return app.Execute(args);
        }

        #region private mthds

        private static IConfig BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build().Get<Config>();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Console.WriteLine($"UnhandledException: {((Exception) e.ExceptionObject).Message}");
            Environment.Exit(1);
        }

        #endregion
    }
}
