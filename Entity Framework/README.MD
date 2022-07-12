# Entity Framework Example

## Install Packages

```powershell
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Design
```

## Common

- ArticlesWithTag is a View Entity <-> Articles_With_Tag View in database

## DB First Example

1. Create Database in SQL Server using `data.sql`. This will create a Database named "DbFirstExamples".
1. Use the following command in Package Manager. Change `$server` to SQL Server's server name.
   ```powershell
   $server = "MY-LEGION"
   $database = "DbFirstExamples"
   Scaffold-DbContext "Server=$($server);Database=$($database);Trusted_Connection=True;Encrypt=No;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
   ```
1. Change connectionString in Program.cs

## Code First Example

### Initial Migration

Change connection string in `EFCoreExamplesContext.cs`

If there is no existing Migration, use

```powershell
    add-migration InitialCreate
```

This create a new migration folder and a migration file.

Then run the project. Data is seeded by `DbInitializer.cs`.
