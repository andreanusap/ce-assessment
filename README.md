# ce-assessment
.NET application with two entry points, a console app and an ASP.NET web app, which are connected to the ChannelEngine REST-API

## Requirements & Assumptions
1. Get In Progress orders and display the result.
2. Get top 5 ordered products from the In Progress Orders which is retrieved from the first function/service.
3. Update the stock of a product from the top 5 ordered products. The trigger from the UI will update the stock to 25.
4. The solution gets the data from the ChannelEngine REST API.
5. The solution and codes are not using any ChannelEngine libraries.

## Solution & Projects
### 1. Business Logic
The business logic off the solution. A single ```HttpClientHelper``` class is implemented to handle the communication between the services and the external services or API, in this case the ChannelEngine's API.
### 2. Shared
A shared library containing common classes that can be referenced and used by the other projects.
### 3. Business Logic Test
A Test project containing unit tests for the business logic. Implemented using xUnit.
### 4. Console App
A console application displaying the result of the main functionalities of the solution. It is directly refence and use the BusinessLogic.
### 5. Web Api
The project to provide an API for the solution. It is consumed by the Web App and have it's own swagger for documentation and testing. 
An API Key authentication is implemented to keep a secure line between the API and the clients. The authentication works by using a middleware that will check and validate the key in the header of every request to the API.
### 6. Web App
A .NET MVC Web Application for displaying the In Progress Order and Top 5 Ordered Products, also updating product's stock from the Top 5 Ordered Product. The Web App consumes the Web Api using a HttpClient from its controller.

## Library
The solution utilizes various libraries from Microsoft and third party developers that can be found in the NuGet package manager. Some of the libraries are:
1. ConsoleTables
- This library is used in the ConsoleApp to display the result in a table.
2. Newtonsoft.JSON
- JSON framework for .NET. Used for serializing or deserializing Json used in HTTP request or response.
3. AutoFixture
- An open source library for .NET designed to minimize the 'Arrange' phase of the unit tests in order to maximize maintainability.
4. FluentAssertion
- Library providing extensive set of extension methods that allow specifying the expected outcome of an unit tests.
5. AutoMapper
- Object-to-object mapping library that used to map objects belonging to dissimilar types in the codes.

## How to Run
### ConsoleApp
The ConsoleApp can be started directly from VisualStudio or by running ```dotnet run``` command inside the project folder.
### WebApi
Similar to the ConsoleApp, it can be started directly from VisualStudio or by running ```dotnet run``` command inside the project folder. The swagger will be automatically opened in a browser when started form VisualStudio, or you can through https://localhost:7078/swagger/index.html or http://localhost:5078/swagger/index.html
### WebApp
The WebApi needs to be started to run the WebApp properly as it consumes the API for retrieving/sending data. To keep it simple, please run the WebApi using ```dotnet run``` command and do the same for the WebApp. Then access the web application using https://localhost:7207 or http://localhost:5207 
