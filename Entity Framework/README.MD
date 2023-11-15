# Entity Framework Examples

## Install Packages

```powershell
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Design
```

## Code First Examples

### Initial Migration

Change connection string in Tests and in Program.cs to

```c#
 private readonly string _connectionString = "Server=MY-LEGION;Database=EFCoreExamples;Trusted_Connection=True;Encrypt=false";
```

If there is no existing Migration, use

```powershell
    add-migration InitialCreate
```

This create a new migration folder and a migration file.

Then run the project. Data is seeded by `DbInitializer.cs`.

## DB First Examples

1. If `EFCoreExamples` database is not already created, run EFCoreExamples to initialize the database.
1. Use the following command in Package Manager. Change `$server` to SQL Server's server name.
   ```powershell
   $server = "MY-LEGION"
   $database = "EFCoreExamples"
   Scaffold-DbContext "Server=$($server);Database=$($database);Trusted_Connection=True;Encrypt=No;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
   ```
