
Technical task is in [ZopaBackendTechnicalTest.pdf](https://github.com/maximzxcv/zopa/blob/main/ZopaBackendTechnicalTest.pdf) file.

## Run application
To prepare application, run the following commands:
```
git clone https://github.com/maximzxcv/zopa
dotnet publish -o .\app .\zopa\Zopa.Console
cd .\app 
```
Now application can be used, to get more details run:
``` 
.\zopa-rate -h
```
To change the market, update **Data/market.csv** file.

## Solution
Main application solution contains two projects Zopa.Console and Zopa.Framework plus unit tests. Zopa.Frameworks is reposible for application buisness logic, while Zopa.Console  runs the program. The major functionality is emplemented by the following services.
#### Zopa.Console/QuoteProgram.cs
That is the presentation layer of application. It is responsible for reading the request, runs all the functionality and outputs the result.
#### Zopa.Framework/MarketReader.cs
This service reads market data (list of lenders) required to run application. I decided to use external source to separate data from application. Service trasforms .csv file (path in appsetting.json) to the list of application objects. 
#### Zopa.Framework/QuoteCalculator.cs
Running over the market (ordered by rate) it calculates the loan details for each lender required to gather the full requested amount. When it's done, those details are used to generate the final quote.

## Ways to improve
- **Smarter MarketReader**: service can be better configurable and more flexible to data (for example to ignore lenders with bad data).
- **Dynamic restrictions**: application limitation details can be moved to configuration. 
- **Testing**: think about integration tests.
