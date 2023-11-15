# ExpensesManager

App for handling expenses on a monthly basis -  mapping, storing and categorizing the expenses


### Demo

[Expenser.webm](https://github.com/roeezach/ExpenseManager-WebAPI/assets/106396740/e0174c6b-f2f0-4604-8295-21cf51bab15b)

## App Structure

The app is now working as a WebApi with `.NET Core 6` and `Entitiy Framework` and the Frontend is using `React` and `Typescript`.
The app can take an expense file that is generated via the bank system and mapping it to customized categories.
the app can integrate with [Splitwise](https://dev.splitwise.com/#section/Terms-of-Use/TERMS-OF-USE) 
and is using `GetExpenses` API of splitwise.
th app has a user managment machenisem based on `JWT`.

### Technologies:
<div>
  <img src="https://github.com/devicons/devicon/blob/master/icons/csharp/csharp-original.svg" title="Csharp" alt="Csharp" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/dotnetcore/dotnetcore-original.svg" title="Csharp" alt="Csharp" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/react/react-original-wordmark.svg" title="React" alt="React" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/typescript/typescript-original.svg" title="JavaScript" alt="JavaScript" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/sqlite/sqlite-original.svg" title="SQLite"  alt="SQLite" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/css3/css3-plain-wordmark.svg"  title="CSS3" alt="CSS" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/git/git-original.svg" title="Git" **alt="Git" width="40" height="40"/>
</div>

#### Future Features
- Automation testing.
- CI/CD with github actions or Travis.
- Dockrize the app.
- Monthly balance Summary
- Saving and Invesment Calculator

### Dependencies

to use this repo please install the following dependencies: 

```dotnet add package ExcelDataReader -v 3.6.0

dotnet add package ExcelDataReader.DataSet -v 3.6.0

dotnet add package Microsoft.AspNet.WebApi.Client -v 5.2.9

dotnet add package Microsoft.AspNetCore.WebUtilities -v 2.2.0

dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.8

dotnet add package Swashbuckle.AspNetCore.SwaggerGen -v 6.4.0

dotnet add package Swashbuckle.AspNetCore -v 6.2.3

dotnet add package System.Data.SQLite -v 1.0.116

dotnet add package System.Data.SQLite.Core -v 1.0.116

dotnet add package System.Text.Encoding.CodePages -v 6.0.0

dotnet add package Microsoft.AspNetCore.JsonPatch -v 6.0.7```
