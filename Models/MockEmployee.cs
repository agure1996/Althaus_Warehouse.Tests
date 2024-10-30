using Althaus_Warehouse.Models.Entities;

namespace Althaus_Warehouse.Tests.Models
{
    public class MockEmployee
    {
        // A list of mock employees
        public static List<Employee> CreateMockEmployees()
        {
            return new List<Employee>
            {
                CreateMockManagerEmployee(),
                CreateMockHREmployee(),
                CreateMockSalesEmployee(),
                CreateMockActiveEmployee(),
                CreateMockInactiveEmployee()
            };
        }

        // Creating a single manager employee
        public static Employee CreateMockManagerEmployee()
        {
            return new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@altwarehouse.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                EmployeeType = EmployeeType.Manager,
                DateHired = DateTime.Today,
                IsActive = true
            };
        }

        // Creating a single HR employee
        public static Employee CreateMockHREmployee()
        {
            return new Employee
            {
                Id = 2,
                FirstName = "Liam",
                LastName = "Wilson",
                Email = "liam.wilson@altwarehouse.com",
                EmployeeType = EmployeeType.HR,
                DateHired = new DateTime(2021, 05, 20),
                IsActive = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
            };
        }

        // Creating a single sales employee
        public static Employee CreateMockSalesEmployee()
        {
            return new Employee
            {
                Id = 3,
                FirstName = "Sophie",
                LastName = "Davis",
                Email = "sophie.davis@altwarehouse.com",
                EmployeeType = EmployeeType.Sales,
                DateHired = new DateTime(2022, 07, 25),
                IsActive = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
            };
        }

        // Creating a single inactive employee
        public static Employee CreateMockInactiveEmployee()
        {
            return new Employee
            {
                Id = 4,
                FirstName = "Ethan",
                LastName = "Taylor",
                Email = "ethan.taylor@altwarehouse.com",
                EmployeeType = EmployeeType.Employee,
                DateHired = new DateTime(2021, 11, 02),
                IsActive = false,  // Marked inactive
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
            };
        }
        // Creating a single active employee
        public static Employee CreateMockActiveEmployee()
        {
            return new Employee
            {
                Id = 5,
                FirstName = "Mia",
                LastName = "Anderson",
                Email = "mia.anderson@altwarehouse.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                EmployeeType = EmployeeType.Employee,
                DateHired = DateTime.Today.AddDays(-100), // Hired 100 days ago
                IsActive = true
            };
        }
    }
}
