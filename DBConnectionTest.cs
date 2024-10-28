using Althaus_Warehouse.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Althaus_Warehouse.Tests;

[TestClass]
public class DatabaseConnectionTests
{
    private DbContextOptions<WarehouseDbContext> _dbContextOptions;
    private IConfiguration _configuration;

    [TestInitialize]
    public void Setup()
    {   

        //creating builder for test cases so I can grab the app.dev.json file configurations
        var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json");

        
        _configuration = builder.Build();

        // Setup mysql database connection string using pomelomysql package 
        _dbContextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseMySql(_configuration["ConnectionStrings:WarehouseDbConnection"], new MySqlServerVersion(new Version(8, 0, 21)))
            .Options;
    }
    [TestMethod]
    public async Task CanConnectToDatabase()
    {
        // Arrange
        using var context = new WarehouseDbContext(_dbContextOptions);

        
            // Act
            await context.Database.EnsureCreatedAsync();

            // Assert

        bool canConnect = await context.Database.CanConnectAsync();
        Assert.IsTrue(canConnect, "Failed to connect to the database.");
    }
}