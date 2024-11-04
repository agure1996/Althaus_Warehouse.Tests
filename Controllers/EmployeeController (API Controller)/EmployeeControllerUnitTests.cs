using Althaus_Warehouse.Controllers;
using Althaus_Warehouse.Models.DTO.EmployeeDTOs;
using Althaus_Warehouse.Models.Entities;
using Althaus_Warehouse.Services;
using Althaus_Warehouse.Services.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Althaus_Warehouse.Tests.Controllers.EmployeeController__API_Controller_
{
    [TestClass]
    internal class EmployeeControllerUnitTests
    {

        private Mock<IEmployeeService> _mockEmployeeService;
        private Mock<ILogger<EmployeeController>> _mockLogger;
        private Mock<IMapper> _mockMapper;
        private EmployeeController _controller;

        [TestInitialize]
        public void InitialSetup()
        {
            // Initialize the mock and the controller before each test
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockLogger = new Mock<ILogger<EmployeeController>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new EmployeeController(_mockLogger.Object, (IEmployeeRepository)_mockEmployeeService.Object, _mockMapper.Object);
        }


        [TestMethod]
        public async Task UpdateEmployee_WhenEmployeeExists_ReturnsNoContent()
        {
            // Arrange
            int employeeId = 1;
            var updateEmployeeDTO = new UpdateEmployeeDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                EmployeeType = EmployeeType.Sales
            };

            var existingEmployee = new Employee
            {
                Id = employeeId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Email = "old.email@example.com",
                EmployeeType = EmployeeType.Manager
            };

            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(employeeId))
                                .ReturnsAsync(existingEmployee);
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(employeeId, It.IsAny<EditEmployeeDTO>()))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEmployee(employeeId, updateEmployeeDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            // Verify that UpdateEmployeeAsync was called with the correct EditEmployeeDTO
            _mockEmployeeService.Verify(s => s.UpdateEmployeeAsync(employeeId, It.Is<EditEmployeeDTO>(dto =>
                dto.FirstName == updateEmployeeDTO.FirstName &&
                dto.LastName == updateEmployeeDTO.LastName &&
                dto.Email == updateEmployeeDTO.Email &&
                dto.EmployeeType == updateEmployeeDTO.EmployeeType.ToString())), Times.Once);
        }

    }
}