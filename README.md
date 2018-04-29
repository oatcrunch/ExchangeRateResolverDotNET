## Description
This is the application is developed to solve exchange rate path problem. It is mainly used to solve following problems:
1. Determine a sequence of trades and transfers across exchanges to convert
the cryptocurrency to fiat currency with a suitable exchange rate
2. Provide the best possible exchange rate to our customers

## Version
1.0.1

## Requirements
1. Make sure you have the latest .NET framework installed on your PC (Windows OS).
2. Make sure you have Visual Studio IDE installed on your PC.

## Setup
1. Git clone the repository to your local.
2. Open `ExchangeRateResolver.sln` using Visual Studio as the IDE.
3. Hit `F5` to execute the application and the command prompt should appear.

## Commands
1. To update exchange rate pricing, enter command on command prompt based on the following format `[timestamp] [exchange] [source_currency] [destination_currency] [forward_factor] [backward_factor]`, for example `2017-11-01T09:42:23+00:00 KRAKEN BTC USD 1000.0 0.0009`.
2. To request for best exchange rate and trade path, enter the following format in command prompt `EXCHANGE_RATE_REQUEST [source_exchange] [source_currency] [destination_exchange] [destination_currency]`, for instance `EXCHANGE_RATE_REQUEST KRAKEN BTC GDAX USD`.
3. Type `exit` (case sensitive) to exit the command prompt.

## Features
1. Exchange rate pricing can be updated in 2 ways:
* Using automatic data feeder (read from file stored in `~\ExchangeRateResolver\Data\PriceUpdateData.txt`) that feeds pricing data at random interval to mimic real time traffic.
* Enter command manually on command prompt (please refer *Commands* section above)
2. By default, mock data is enabled, where fixed pricing data will be fed at random time intervals to mimic real traffic latencies.
* To disable mock data updates, set `EnableMock` value to `false` in `\ExchangeRateResolver\App.config`.
* To change minimum latency traffic, change the value (in miliseconds) in `MinTrafficLatency` to any integer.
* To change maximum latency traffic, change the value  (in miliseconds) in `MaxTrafficLatency` to any integer.

## Screenshots
![Alt text](/Sample.jpg?raw=true "Scenario where user updates the existing exchange rate pricing and checks for the newly updated trade path")
User adds in new exchange for ABCBIT and then request for new trade path and the results will be reflected based on newly added exchange.
![Alt text](/Sample2.jpg?raw=true "Scenario where user adds in new exchange-rate pair and results are reflected once user requests for new trade path associated to the new exchange-rate pair")
User adds in 1 new exchange and request for trade path and then adds in another new exchange and re-requests for updated trade path.
![Alt text](/Sample3.jpg?raw=true "Scenario where user adds in new exchange-rate pair and results are reflected once user requests for new trade path associated to the new exchange-rate pair")
Scenario where same exchange-currency-pair is updated, and user should see a change in trade path and best rate.
![Alt text](/ExchangeRatesSolverArchitecture.jpg?raw=true "General Architecture")
General architecture.

## Release Notes
| Date | Version | Description | Author |
| ------------- | ------------- | ------------- | ------------- |
| 2018-April-26 | 1.0.0  | Initial Draft | Mel |
| 2018-April-29 | 1.0.1  | Bug fixes and updated README.md | Mel |

