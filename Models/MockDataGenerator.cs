using Althaus_Warehouse.Models.Entities;


namespace Althaus_Warehouse.Tests.Models
{
    public class MockDataGenerator
    {
        public static List<Employee> CreateMockEmployees(int count)
        {
            var employees = new List<Employee>();
            for (int i = 1; i <= count; i++)
            {
                employees.Add(new Employee
                {
                    Id = i,
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    Email = $"email{i}@example.com",
                    EmployeeType = (EmployeeType)(i % 3),
                    DateHired = DateTime.Today.AddDays(-i),
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword($"Password{i}")
                });
            }
            return employees;
        }

        public static List<ItemType> CreateMockItemTypes()
        {
            return new List<ItemType>
        {
            new ItemType { Id = 1, Name = "Dairy", Description = "Perishable dairy products." },
            new ItemType { Id = 2, Name = "Meat", Description = "Fresh meat products." },
            new ItemType { Id = 3, Name = "Seafood", Description = "Various seafood items." },
            // Add more item types as needed
        };
        }

        public static List<Item> CreateMockItems(int count)
        {
            var items = new List<Item>();
            var itemTypes = CreateMockItemTypes();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                items.Add(new Item
                {
                    Id = i,
                    Name = $"Item{i}",
                    Description = $"Description for Item{i}",
                    Quantity = random.Next(1, 100),
                    Price = (decimal)(random.NextDouble() * 100),
                    CreatedById = random.Next(1, 4),
                    ItemTypeId = itemTypes[random.Next(itemTypes.Count)].Id,
                    DateCreated = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }
            return items;
        }
    }
}
