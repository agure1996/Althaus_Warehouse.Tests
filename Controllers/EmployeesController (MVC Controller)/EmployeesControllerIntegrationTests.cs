using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Althaus_Warehouse.Models.Entities;
using Althaus_Warehouse.Controllers;
using Althaus_Warehouse.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Althaus_Warehouse.Models.DTO.EmployeeDTOs;
using Althaus_Warehouse.Services.Repositories;

namespace Althaus_Warehouse.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesControllerIntegrationTests"/> class.
        /// This constructor sets up the <see cref="WebApplicationFactory{T}"/> and <see cref="HttpClient"/>.
        /// </summary>
        public EmployeesControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        /// <summary>
        /// Tests that the Index action returns a view containing a list of employees.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task Index_ReturnsViewWithEmployees()
        {
            // Act
            var response = await _client.GetAsync("/Employees");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseString.Contains("Employee List"));
        }

        /// <summary>
        /// Tests that the Create action returns the view for creating a new employee.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task Create_ReturnsCreateView()
        {
            // Act
            var response = await _client.GetAsync("/Employees/Create");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseString.Contains("Create Employee"));
        }

        /// <summary>
        /// Tests that the Delete action redirects to the Index after successfully deleting an employee with a valid ID.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task Delete_ValidId_RedirectsToIndex()
        {
            // Arrange: Create a mock service and an employee ID.
            var dummyEmployeeId = 1;

            // Mock the employee service
            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(service => service.GetEmployeeByIdAsync(dummyEmployeeId))
                               .ReturnsAsync(new Employee { Id = dummyEmployeeId, FirstName = "Jane", LastName = "Doe" });

            // Setting up controller with the mocked service
            var controller = new EmployeesController(mockEmployeeService.Object);

            // Act: Calling the DeleteConfirmed action to delete the employee
            var deleteResponse = await controller.DeleteConfirmed(dummyEmployeeId);

            // Assert: Checking if it redirects to Index (302 Found)
            Assert.IsInstanceOfType(deleteResponse, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)deleteResponse).ActionName);
        }

        /// <summary>
        /// Tests that the GetEmployeeById action returns the employee details for a valid ID.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetEmployeeById_ValidId_ReturnsEmployee()
        {
            // Arrange: Set up a valid employee ID and the expected response
            int validId = 1;
            var expectedEmployee = new Employee
            {
                Id = validId,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                EmployeeType = EmployeeType.Manager
            };

            // Mock the employee service response
            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(service => service.GetEmployeeByIdAsync(validId))
                               .ReturnsAsync(expectedEmployee);

            // Controller
            var controller = new EmployeesController((mockEmployeeService.Object));

            // Act: Call the GetEmployeeById action
            var response = await controller.GetEmployeeById(validId);

            // Assert: Check if the response is OK and contains the expected data
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var okResult = response as OkObjectResult;
            var employee = okResult.Value as Employee;

            // Check that the employee details match
            Assert.IsNotNull(employee);
            Assert.AreEqual(expectedEmployee.FirstName, employee.FirstName);
            Assert.AreEqual(expectedEmployee.LastName, employee.LastName);
            Assert.AreEqual(expectedEmployee.Email, employee.Email);
            Assert.AreEqual(expectedEmployee.EmployeeType, employee.EmployeeType);
        }
    }

}
