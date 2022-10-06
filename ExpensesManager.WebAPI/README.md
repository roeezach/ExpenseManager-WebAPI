# ExpensesManager

App for handling expenses on a monthly basis -  mapping, storing and categorizing the expenses

## App Structure

The app is now working as a WebApi with .NET Core 6 and Entitiy Framework.
Currently, there is only Backend for this app and soon I will implement a Frontend.
The app can take an expense file that is generated via the bank system and mapping it to customized categories.
In the future, the app will integrates with [Splitwise](https://dev.splitwise.com/#section/Terms-of-Use/TERMS-OF-USE) 
and will use `GetExpenses` API of splitwise.

### Technologies:
- Backend : C# , Entity Core Framework
- database : sqlite
- nuget packages: Dapper, ExcelDataReader, System.Data.Sqlite

#### Future Features
- UI with Javascript.
- Automation testing.
- CI/CD with github actions.
- Users managment : register and login functionality.
- improvement of reading functionality - drage and drop instead of local file path
- coverage of other bank file structures ( currently only the Beinleumi bank is covered).
