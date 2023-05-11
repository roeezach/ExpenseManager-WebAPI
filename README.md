# ExpensesManager

App for handling expenses on a monthly basis -  mapping, storing and categorizing the expenses

## App Structure

The app is now working as a WebApi with .NET Core 6 and Entitiy Framework.
Currently, there is only Backend for this app and soon I will implement a Frontend.
The app can take an expense file that is generated via the bank system and mapping it to customized categories.
the app can integrate with [Splitwise](https://dev.splitwise.com/#section/Terms-of-Use/TERMS-OF-USE) 
and is using `GetExpenses` API of splitwise.
currently, the app is using `swagger` to interact with the API endpoints.

### Technologies:
- Backend : C# , Entity Core Framework
- database : sqlite
- nuget packages: ExcelDataReader, System.Data.Sqlite

#### Future Features
- UI with Javascript.
- Automation testing.
- CI/CD with github actions.
- Users managment : register and login functionality.
- improvement of reading functionality - drag and drop instead of local file path
- additional coverage of different bank files structures ( currently only the Beinleumi and Hapoalim are covered).

### Dependencies

to use this repo please install the following dependencies: 

`dotnet add package ExcelDataReader -v 3.6.0`

`dotnet add package ExcelDataReader.DataSet -v 3.6.0`

`dotnet add package Microsoft.AspNet.WebApi.Client -v 5.2.9`

`dotnet add package Microsoft.AspNetCore.WebUtilities -v 2.2.0`

`dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.8`

`dotnet add package Swashbuckle.AspNetCore.SwaggerGen -v 6.4.0`

`dotnet add package Swashbuckle.AspNetCore -v 6.2.3`

`dotnet add package System.Data.SQLite -v 1.0.116`

`dotnet add package System.Data.SQLite.Core -v 1.0.116`

`dotnet add package System.Text.Encoding.CodePages -v 6.0.0`

`dotnet add package Microsoft.AspNetCore.JsonPatch -v 6.0.7`
