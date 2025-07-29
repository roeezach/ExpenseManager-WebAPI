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
  <img src="https://github.com/devicons/devicon/blob/master/icons/docker/docker-plain-wordmark.svg" title="Docker" alt="Docker" width="40" height="40"/>&nbsp;  
  <img src="https://github.com/devicons/devicon/blob/master/icons/csharp/csharp-original.svg" title="Csharp" alt="Csharp" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/dotnetcore/dotnetcore-original.svg" title="Csharp" alt="Csharp" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/react/react-original-wordmark.svg" title="React" alt="React" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/typescript/typescript-original.svg" title="JavaScript" alt="JavaScript" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/sqlite/sqlite-original.svg" title="SQLite"  alt="SQLite" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/css3/css3-plain-wordmark.svg"  title="CSS3" alt="CSS" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/git/git-original.svg" title="Git" **alt="Git" width="40" height="40"/>
</div>

#### Future Features
- Frontend Automation testing.
- Deploy to cloud (azure or aws)
- Budget analysis with OpenAI API
- Monthly balance Summary
- Saving and Invesment Calculator

## Architecture

### Request Flow
```mermaid
graph TD
    A[Frontend] --> B(API Controllers)
    B --> C(Services)
    C --> D[AppDbContext]
    D --> E[(Database)]
```

### Class Relationships
```mermaid
classDiagram
    class CategoryController
    class MapperController
    class ReaderController
    class RecalculateExpenseController
    class SplitewiseExpensesController
    class TotalExpensePerCategoryController
    class UsersController

    class CategoryService
    class ExpenseMapperService
    class ExpenseReadService
    class RecalculatedExpenseService
    class SplitewiseExpenseService
    class TotalExpensesPerCategoryService
    class UsersService

    class AppDbContext

    class ExpenseRecord
    class Categories
    class SwRecords
    class Users
    class TotalExpensePerCategory
    class RecalculatedExpenseRecord
    class UploadedFile

    CategoryController --> CategoryService
    MapperController --> ExpenseMapperService
    ReaderController --> ExpenseReadService
    RecalculateExpenseController --> RecalculatedExpenseService
    SplitewiseExpensesController --> SplitewiseExpenseService
    TotalExpensePerCategoryController --> TotalExpensesPerCategoryService
    UsersController --> UsersService

    CategoryService --> AppDbContext
    ExpenseMapperService --> AppDbContext
    ExpenseReadService --> AppDbContext
    RecalculatedExpenseService --> AppDbContext
    SplitewiseExpenseService --> AppDbContext
    TotalExpensesPerCategoryService --> AppDbContext
    UsersService --> AppDbContext

    AppDbContext --> ExpenseRecord
    AppDbContext --> Categories
    AppDbContext --> SwRecords
    AppDbContext --> Users
    AppDbContext --> TotalExpensePerCategory
    AppDbContext --> RecalculatedExpenseRecord
    AppDbContext --> UploadedFile
```

Diagrams are stored in [docs/architecture](docs/architecture).
