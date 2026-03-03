📦 PShopBridgeAPI

A totally non-chaotic layered-architecture API that somehow survived development.


🚀 Running the Project

Configure the database.

Update the connection string.

Run the API.

Hope nothing explodes.


🧱 Project Structure

This project follows a classic layered approach (yes, the one everyone pretends to follow but rarely does):

API Layer

Service Layer

Repository/Data Access Layer

Database (SQL Server)

Everything is neatly separated so future-you won’t hate past-you too much.

🗄️ Database

A file named ProductDatabase.sql lives at the root of the project.
Run it in SQL Server to create the database and tables.
If you can’t find it, maybe check if your eyeballs are attached properly.

🔌 Connection String

Update the connection string in appsettings.json or appsettings.Development.json depending on your mood or environment:

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=JARVIS;Initial Catalog=ProductManagementSystem;Integrated Security=True;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

If you use a different server name, change "JARVIS" to whatever uninspired device name you picked.

🛠️ Scaffolding Command

If you ever need to regenerate the EF Core models (because you dropped a table at 2 AM), here’s the command:

dotnet ef dbcontext scaffold "Data Source=JARVIS;Initial Catalog=ProductManagementSystem;Integrated Security=True;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -o Data/Models -c ProductDbContext --context-dir Data --no-onconfiguring --force

Replace the connection string as needed so it actually points to a real database and not a fictional one.

🧪 Testing

You can test endpoints using swagger, Postman, curl, or by whispering at your computer and praying.
All methods should return JSON unless you’ve broken something.

🎯 Features

Basic CRUD operations for products

Layered architecture that won’t judge you (much)

SQL Server integration

EF Core generated models

Clean serialization without cyclical meltdown