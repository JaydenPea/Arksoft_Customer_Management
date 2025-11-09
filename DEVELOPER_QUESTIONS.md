# Developer Questions - Answers

## 1. What is the difference between interfaces and abstract classes? Provide an example where you would use each.

**Interface** is like a contract or promise. It tells you what methods a class MUST have, but doesn't tell you HOW to do them. It's like saying "any car must be able to start, stop, and turn" but doesn't tell you if it's electric, petrol, or diesel.

**Abstract Class** is like a template with some parts already built and some parts you need to finish. It gives you some ready-made functionality but leaves some things for you to complete.

**Where I Used Them in My Project:**

**Interface Example - ICustomerRepository:**
```csharp
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByVatNumberAsync(string vatNumber);
    Task<IEnumerable<Customer>> GetPagedAsync(int pageNumber, int pageSize);
}
```

I used this because I wanted any class that handles customer data to have these exact methods, but I didn't care HOW they do it. CustomerRepository implements this interface using Entity Framework, but I could easily create TestCustomerRepository that stores data in memory for testing.

**Abstract Class Example - BaseEntity:**
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

I used this because EVERY entity in my system needs these basic properties (Id, CreatedAt, UpdatedAt), but each entity has its own specific properties too. Customer inherits from BaseEntity and adds Name, Address, etc.

## 2. What are the advantages of using interfaces?

**Easy Testing:** I can create fake versions of my classes for testing. For example, I created mock versions of ICustomerRepository in my tests so I don't need a real database when testing.

**Easy to Change:** If I want to switch from Entity Framework to Dapper or even a web API, I just need to create a new class that implements ICustomerRepository. The rest of my code doesn't need to change.

**Clear Contracts:** When I see ICustomerService, I immediately know what methods it must have. It's like a menu at a restaurant - it tells you what's available.

**Dependency Injection:** I can inject interfaces into my controllers and services. This makes the code flexible because the controller doesn't care which specific implementation it gets.

**Multiple Inheritance:** A class can implement multiple interfaces but can only inherit from one abstract class. This gives more flexibility.

**Where I Used These Advantages:**

In my CustomerService constructor, I take ICustomerRepository instead of CustomerRepository directly:
```csharp
public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
```

This means I can easily swap out the database implementation or inject a test version without changing any other code.

## 3. Give an example of inheritance i.e., real life business example where it could be used?

**Real Life Example - Vehicle Management System:**

Imagine a car rental company that has different types of vehicles:

**Base Class - Vehicle:**
```csharp
public abstract class Vehicle
{
    public string LicensePlate { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public bool IsRented { get; set; }
    
    public abstract decimal CalculateDailyRate();
    public virtual void StartEngine() => Console.WriteLine("Engine started");
}
```

**Inherited Classes:**
```csharp
public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }
    public bool HasAirCon { get; set; }
    
    public override decimal CalculateDailyRate()
    {
        return HasAirCon ? 150m : 120m; // More expensive with AC
    }
}

public class Truck : Vehicle
{
    public decimal LoadCapacity { get; set; }
    public bool RequiresSpecialLicense { get; set; }
    
    public override decimal CalculateDailyRate()
    {
        return LoadCapacity > 5000 ? 300m : 200m; // Based on load capacity
    }
    
    public override void StartEngine()
    {
        Console.WriteLine("Truck engine warming up...");
        base.StartEngine(); // Call the parent method too
    }
}

public class Motorcycle : Vehicle
{
    public int EngineSize { get; set; }
    
    public override decimal CalculateDailyRate()
    {
        return EngineSize > 600 ? 80m : 50m; // Based on engine size
    }
}
```

**How I Used Inheritance in My Customer Project:**

In my project, I used inheritance with BaseEntity:

```csharp
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? TelephoneNumber { get; set; }
    public string? ContactPersonName { get; set; }
    public string? ContactPersonEmail { get; set; }
    public string? VatNumber { get; set; }
}
```

**Why This Works Well:**

- **Code Reuse:** Every entity gets Id, CreatedAt, UpdatedAt automatically
- **Consistency:** All entities behave the same way for basic operations
- **Easy to Add New Entities:** If I add Product or Order entities later, they just inherit from BaseEntity
- **Polymorphism:** I can have a List<BaseEntity> that contains customers, products, orders, etc.

**Real Business Benefits:**

1. **Less Code:** I don't repeat Id, CreatedAt, UpdatedAt in every entity
2. **Consistent Behavior:** All entities get timestamps automatically
3. **Easy Maintenance:** If I need to add audit fields, I add them to BaseEntity once
4. **Type Safety:** The compiler ensures all entities have the required base properties