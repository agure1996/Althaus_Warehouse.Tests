using Althaus_Warehouse.Controllers;
using Althaus_Warehouse.Models.Entities;
using Althaus_Warehouse.Services;
using Althaus_Warehouse.Tests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Althaus_Warehouse.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerUnitTests
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private EmployeesController _controller;

        [TestInitialize]
        public void InitialSetup()
        {
            // Initialize the mock and the controller before each test
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeesController(_employeeServiceMock.Object);
        }

        [TestMethod]
        public void TestEmployeeFullName()
        {
            // Arrange
            var mockEmployee = MockEmployee.CreateMockManagerEmployee();

            // Act
            var fullName = mockEmployee.GetFullName();

            // Assert
            Assert.AreEqual("John Doe", fullName);
        }

        [TestMethod]
        public async Task TestGetEmployeeByIdReturnsEmployeeAsync()
        {
            // Arrange
            var mockEmployee = MockEmployee.CreateMockHREmployee();
            _employeeServiceMock.Setup(service => service.GetEmployeeByIdAsync(mockEmployee.Id))
                .ReturnsAsync(mockEmployee);

            // Act
            var result = await _controller.GetEmployeeById(mockEmployee.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected result to be of type OkObjectResult.");

            // Now we can cast the result to OkObjectResult to extract the employee
            var okResult = (OkObjectResult)result;
            var employeeResult = okResult.Value as Employee; // Extract the Employee from the OkObjectResult

            Assert.IsNotNull(employeeResult, "Expected result to be of type Employee.");
            Assert.AreEqual(mockEmployee.Id, employeeResult.Id, "Employee IDs do not match.");
        }

        [TestMethod]
        public async Task TestGetEmployeeByIdReturnsNotFoundAsync()
        {
            // Arrange
            var employeeId = 1;
            _employeeServiceMock.Setup(service => service.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync((Employee)null); // Simulate not found

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult), "Expected result to be NotFoundResult.");
        }

        [TestMethod]
        public async Task IndexReturnsViewWithEmployees()
        {
            // Arrange
            var mockEmployeesList = MockEmployee.CreateMockEmployees();
            _employeeServiceMock.Setup(service => service.GetAllEmployeesAsync())
                .ReturnsAsync(mockEmployeesList);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Expected a ViewResult.");

            var model = viewResult.Model as List<Employee>;
            Assert.IsNotNull(model, "Expected a List<Employee> as the model.");
            Assert.AreEqual(mockEmployeesList.Count, model.Count, "Employee count does not match.");
        }

        [TestMethod]
        public async Task IndexReturnsViewWithNoEmployees()
        {
            // Arrange
            var mockEmployeesList = new List<Employee>(); 
            _employeeServiceMock.Setup(service => service.GetAllEmployeesAsync())
                .ReturnsAsync(mockEmployeesList);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Expected a ViewResult.");

            var model = viewResult.Model as List<Employee>;
            Assert.IsNotNull(model, "Expected a List<Employee> as the model.");
            Assert.AreEqual(0, model.Count, "Expected no employees in the list.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _employeeServiceMock.VerifyAll();
        }
    }
}
