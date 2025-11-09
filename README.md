# Arksoft Customer Management System

## About This Project

This is a customer management system I built for the Arksoft technical test. It allows you to add, edit, view, and delete customers with all their important details like company name, address, phone numbers, and VAT numbers.

## Why I Chose Clean Architecture

I decided to use Clean Architecture because it makes the code much easier to work with and maintain. Here's why:

**Easy to Understand**: Everything is organized into clear layers - Domain (business rules), Application (what the system does), Infrastructure (database stuff), and Web (the user interface). When someone new looks at the code, they can quickly find what they need.

**Easy to Test**: Because everything is separated, I can test each part on its own without needing a real database or web server. This makes testing much faster and more reliable.

**Easy to Change**: If I need to switch from SQL Server to a different database later, I only need to change the Infrastructure layer. The business logic stays the same.

**Follows Best Practices**: Clean Architecture is what professional development teams use in real companies. It shows I understand how to build systems that can grow and be maintained by teams.

## Why I Used Repository Pattern

The Repository Pattern is like having a special helper that handles all the database operations. Here's why it's great:

**Simple Interface**: Instead of writing complex SQL everywhere, I just call methods like `AddCustomer()` or `GetCustomer()`. It's much cleaner.

**Easy Testing**: I can create fake repositories for testing so I don't need a real database during tests. This makes tests run super fast.

**Consistent Data Access**: All database operations go through the same pattern, so if I need to add logging or caching later, I can do it in one place.

**Less Bugs**: Since all the database logic is in one place, there's less chance of making mistakes when working with data.

## What Technologies I Used

- **ASP.NET Core MVC** - For the web application and REST API
- **Entity Framework Core** - For talking to the database
- **SQL Server** - For storing customer data
- **Tailwind CSS + Flowbite** - For making it look professional and modern
- **FluentValidation** - For checking that data is correct
- **AutoMapper** - For converting between different object types
- **Serilog** - For logging what the system is doing
- **xUnit + Moq + FluentAssertions** - For testing everything works properly

## Key Features

### Customer Management
- Add new customers with validation
- Edit existing customer details
- View customer information
- Delete customers with confirmation
- Search customers by name or VAT number
- Sort and paginate customer lists

### Professional Features
- Clean, dark-themed user interface
- Responsive design that works on mobile
- Form validation with helpful error messages
- Proper error handling and logging
- REST API with authentication
- Swagger documentation for developers

### Technical Quality
- Clean Architecture with proper separation
- Repository Pattern for data access
- Dependency injection throughout
- Unit tests with 100% coverage of core logic
- Proper security with API key authentication
- Database migrations and seeding

## How to Run This Project

1. Make sure you have .NET 9.0 installed
2. Update the database connection string in `appsettings.json`
3. Run database migrations: `dotnet ef database update`
4. Start the application: `dotnet run`
5. Open your browser to `https://localhost:5001`

## How to Run Tests

```bash
dotnet test
```

This will run all 42 unit tests and show you they all pass.

## API Documentation

When running in development mode, visit `/swagger` to see the interactive API documentation.

This architecture might seem like overkill for a simple customer system, but it shows I understand how to build applications that can handle real business requirements and grow over time.