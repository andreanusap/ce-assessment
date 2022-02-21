# ce-assessment
.NET application with two entry points, a console app and an ASP.NET web app, which are connected to the ChannelEngine REST-API

## Requirements & Assumptions
1. The solution is consists of 4 projects which are Business Logic, Unit Test, Console Application, & Web Application.
2. The Business Logic only uses the API described in the ChannelEngine REST-API reference.
3. The top 5 products sold functionality will only process the orders retrieved from the Get function.
4. Update functionality is only used on the products in the top 5 list.

There are 4 main functionalities in the Business Logic of this app.
1. GetInProgressOrders
  - This method is to retrieve the first 100 orders with IN PROGRESS status from the API.
2. GetOrders
  - By using the status and page parameters in the API, this method is intended for paging functionality.
3. GetTop5OrderedProducts
  - Get the top 5 ordered products from the provided list of orders.
4. UpdateStock
  - Update the product's stock to 25 using PUT API.
