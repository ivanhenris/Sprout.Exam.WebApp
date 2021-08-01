using Sprout.Exam.WebApp.Controllers;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Repository;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sprout.Exam.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Test
{
    public class EmployeeControllerTests
    {
        [Fact]
        public async void GetAllEmployeeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeDto, int>>();
            var employeesInMemory = new List<EmployeeDto>
            {
                new EmployeeDto{Id = 1, FullName = "Test Unit1", Birthdate = DateTime.Now, Tin = "1E2", TypeId = 1, IsDeleted = false, BasePay = 20000 },
                new EmployeeDto{Id = 2, FullName = "Test Unit2", Birthdate = DateTime.Now, Tin = "EE2", TypeId = 2, IsDeleted = false, BasePay = 500 }
            };
            repo.Setup(m => m.GetAll()).ReturnsAsync(employeesInMemory);
            var controller = new EmployeesController(repo.Object);

            // act
            var actionResult = await controller.Get();

            // assert
            var result = actionResult as OkObjectResult;
            var employees = result.Value as IEnumerable<ViewEmployeeDto>;
            repo.Verify();
            Assert.Equal(employeesInMemory.Count, employees.Count());
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetEmployeeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeDto, int>>();
            var employeesInMemory = new List<EmployeeDto>
            {
                new EmployeeDto{Id = 1, FullName = "Test Unit1", Birthdate = DateTime.Now, Tin = "1E2", TypeId = 1, IsDeleted = false, BasePay = 20000 },
                new EmployeeDto{Id = 2, FullName = "Test Unit2", Birthdate = DateTime.Now, Tin = "EE2", TypeId = 2, IsDeleted = false, BasePay = 500 }
            };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeesController(repo.Object);

            // act
            var actionResult = await controller.GetById(2);

            // assert
            var result = actionResult as OkObjectResult;
            var employee = result.Value as EmployeeDto;
            repo.Verify();
            Assert.Equal("Test Unit2", employee.FullName);
            Assert.Equal(2, employee.Id);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void SaveEmployeeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeDto, int>>();
            var employeesInMemory = new List<EmployeeDto>
            {
                new EmployeeDto{Id = 1, FullName = "Test Unit1", Birthdate = DateTime.Now, Tin = "1E2", TypeId = 1, IsDeleted = false, BasePay = 20000 },
                new EmployeeDto{Id = 2, FullName = "Test Unit2", Birthdate = DateTime.Now, Tin = "EE2", TypeId = 2, IsDeleted = false, BasePay = 500 }
            };
            var newEmployee = new CreateEmployeeDto { FullName = "Test Unit3", Birthdate = DateTime.Now, Tin = "442", TypeId = 2, BasePay = 800 };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeesController(repo.Object);

            // act
            var actionResult = await controller.Post(newEmployee);

            // assert
            var result = actionResult as CreatedResult;
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async void EditEmployeeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeDto, int>>();
            var employeesInMemory = new List<EmployeeDto>
            {
                new EmployeeDto{Id = 1, FullName = "Test Unit1", Birthdate = DateTime.Now, Tin = "1E2", TypeId = 1, IsDeleted = false, BasePay = 20000 },
                new EmployeeDto{Id = 2, FullName = "Test Unit2", Birthdate = DateTime.Now, Tin = "EE2", TypeId = 2, IsDeleted = false, BasePay = 500 }
            };
            var editEmployee = new EditEmployeeDto { Id = 2, FullName = "Test Unit3", Birthdate = DateTime.Now, Tin = "442", TypeId = 2, BasePay = 800 };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeesController(repo.Object);

            // act
            var actionResult = await controller.Put(editEmployee);
            var actionResultGet = await controller.GetById(2);

            // assert
            var result = actionResultGet as OkObjectResult;
            var employee = result.Value as EmployeeDto;
            Assert.Equal("Test Unit3", employee.FullName);

            var resultPut = actionResult as OkObjectResult;
            Assert.Equal(200, resultPut.StatusCode);
        }

        [Fact]
        public async void DeleteEmployeeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeDto, int>>();
            var employeesInMemory = new List<EmployeeDto>
            {
                new EmployeeDto{Id = 1, FullName = "Test Unit1", Birthdate = DateTime.Now, Tin = "1E2", TypeId = 1, IsDeleted = false, BasePay = 20000 },
                new EmployeeDto{Id = 2, FullName = "Test Unit2", Birthdate = DateTime.Now, Tin = "EE2", TypeId = 2, IsDeleted = false, BasePay = 500 }
            };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeesController(repo.Object);

            // act
            var actionResult = await controller.Delete(1);

            // assert
            var resultPut = actionResult as OkObjectResult;
            Assert.Equal(200, resultPut.StatusCode);
        }
    }
}
