## Clone repository
```
git clone https://github.com/Rajivgogia7/unit-testing-assignment.git

```
## Prerequisites

* Visual Studio 2019
* ASP.NET Core 3.1

## How to run the project
```
* Checkout this project to a location in your disk.
* Open the solution file using the Visual Studio 2019.
* Restore the NuGet packages by rebuilding the solution.
* Run the project.
```
## Test

### ebroker-management-api-Tests
```
To execute test: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
To generate coverage report: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
```

### ebroker-management-application.Tests
```
To execute test: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
To generate coverage report: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
```

### ebroker-management-data-Tests
```
To execute test: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
To generate coverage report: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
```

## APIs
```
Uploaded a file in the repository itself - Unit Testing Assignment.postman_collection containing the request for:

* Adding Trader
* Getting All Traders
* Getting Trader by Trader Code
* Adding funds to trader's account

* Adding Equity
* Get All Equities
* Get Equity by Equity Code
* Add more stock to existing equity

* Buy Equity
* Sell Equity

```
