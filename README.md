## Connect to a db create  `appsettings.json`
 ```
 {
  "ConnectionStrings": {
    "DefaultConnection": "server=your_host;port=3306;database=your_bd_name;user=root;password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
 ```

 ### commands to migrate
 Initial migration
 ```
 dotnet ef migrations add MigrationName
 ```
 Update Db
 ```
 dotnet ef database update
 ```
