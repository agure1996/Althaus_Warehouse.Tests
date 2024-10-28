using Althaus_Warehouse.Models.Entities;

namespace Althaus_Warehouse.Tests.Models;
public static class MockEmployee
{
    public static Employee CreateMockEmployee()
    {
        return new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
            EmployeeType = EmployeeType.Manager, // Ensure EmployeeType is defined properly
            DateHired = DateTime.Today,
            IsActive = true
        };
    }
}


