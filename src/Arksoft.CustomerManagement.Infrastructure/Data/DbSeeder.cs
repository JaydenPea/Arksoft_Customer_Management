using Arksoft.CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arksoft.CustomerManagement.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Customers.AnyAsync())
            return;

        var customers = new[]
        {
            new Customer
            {
                Name = "ABC Manufacturing Ltd",
                Address = "123 Industrial Park, Cape Town, 8001",
                TelephoneNumber = "+27-21-555-0123",
                ContactPersonName = "John Smith",
                ContactPersonEmail = "john.smith@abcmanufacturing.co.za",
                VatNumber = "ZA123456789",
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new Customer
            {
                Name = "Tech Solutions (Pty) Ltd",
                Address = "456 Business Centre, Johannesburg, 2000",
                TelephoneNumber = "+27-11-555-0456",
                ContactPersonName = "Sarah Johnson",
                ContactPersonEmail = "sarah.johnson@techsolutions.co.za",
                VatNumber = "ZA987654321",
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            },
            new Customer
            {
                Name = "Green Energy Co",
                Address = "789 Eco Park, Durban, 4000",
                TelephoneNumber = "+27-31-555-0789",
                ContactPersonName = "Mike Wilson",
                ContactPersonEmail = "mike.wilson@greenenergy.co.za",
                VatNumber = "ZA456789123",
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new Customer
            {
                Name = "Retail Masters",
                Address = "321 Shopping Complex, Pretoria, 0001",
                TelephoneNumber = "+27-12-555-0321",
                ContactPersonName = "Lisa Brown",
                ContactPersonEmail = "lisa.brown@retailmasters.co.za",
                VatNumber = "ZA654321987",
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new Customer
            {
                Name = "Construction Plus",
                Address = "654 Building Site, Port Elizabeth, 6000",
                TelephoneNumber = "+27-41-555-0654",
                ContactPersonName = "David Miller",
                ContactPersonEmail = "david.miller@constructionplus.co.za",
                VatNumber = "ZA789123456",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Customer
            {
                Name = "Food Services International",
                Address = "987 Culinary Avenue, Bloemfontein, 9300",
                TelephoneNumber = "+27-51-555-0987",
                ContactPersonName = "Emma Davis",
                ContactPersonEmail = "emma.davis@foodservices.co.za",
                VatNumber = "ZA321987654",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Customer
            {
                Name = "Medical Supplies Corp",
                Address = "147 Health Plaza, East London, 5200",
                TelephoneNumber = "+27-43-555-0147",
                ContactPersonName = "Dr. James Taylor",
                ContactPersonEmail = "james.taylor@medicalsupplies.co.za",
                VatNumber = "ZA147258369",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Customer
            {
                Name = "Transport Logistics",
                Address = "258 Freight Terminal, Polokwane, 0700",
                TelephoneNumber = "+27-15-555-0258",
                ContactPersonName = "Karen White",
                ContactPersonEmail = "karen.white@transportlogistics.co.za",
                VatNumber = "ZA258369147",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Customer
            {
                Name = "Financial Advisors",
                Address = "369 Money Street, Kimberley, 8300",
                TelephoneNumber = "+27-53-555-0369",
                ContactPersonName = "Robert Anderson",
                ContactPersonEmail = "robert.anderson@financialadvisors.co.za",
                VatNumber = "ZA369147258",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Customer
            {
                Name = "Educational Resources",
                Address = "741 Learning Campus, Nelspruit, 1200",
                TelephoneNumber = "+27-13-555-0741",
                ContactPersonName = "Helen Garcia",
                ContactPersonEmail = "helen.garcia@educational.co.za",
                VatNumber = "ZA741852963",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}