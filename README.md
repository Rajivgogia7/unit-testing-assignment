## Clone repository
```
git clone https://github.com/Rajivgogia7/unit-testing-assignment.git

```
## Prerequisites for running the application

* Visual Studio 2019
* ASP.NET Core 3.1
* MySQL

## How to run the project
```
* Checkout this project to a location in your disk.
* Open the solution file using the Visual Studio 2019.
* Restore the NuGet packages by rebuilding the solution.
* Run the project.
```

## MySQL Configurations

* Create a database named unit_testing in your MySQL server
* Update connection string in AppSettings.json file of ebroker-management-api project.

![image](https://user-images.githubusercontent.com/6339432/147266662-1bcedbe3-dc47-4128-8c52-c50475757b9c.png)

* Run migrations using ebroker-management-data project
```
update-database
```

## Run test cases using InMemoryDatabase 

![image](https://user-images.githubusercontent.com/6339432/147267569-53bf2eb4-922b-439d-bd87-3c0647ecfee6.png)

## Run application using MySQL Database 

![image](https://user-images.githubusercontent.com/6339432/147268800-99b5afb6-c121-4457-8256-ca9514000b2a.png)

## Test Coverage Generation

### ebroker-management-api-Tests
```
To execute test: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
To generate coverage report: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
```

![image](https://user-images.githubusercontent.com/6339432/147268922-91dbf20d-a436-4cf9-828b-d8d9d653ed5f.png)


### ebroker-management-application.Tests
```
To execute test: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
To generate coverage report: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
```

![image](https://user-images.githubusercontent.com/6339432/147269029-2eb8bb46-1b4d-46b9-9347-c67ba26feca4.png)


### ebroker-management-data-Tests
```
To execute test: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
To generate coverage report: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
```

![image](https://user-images.githubusercontent.com/6339432/147269087-7bf4aa3c-20a1-4bc3-ae5e-71cf855612c7.png)


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
