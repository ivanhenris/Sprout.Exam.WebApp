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
    public class EmployeeTypeControllerTests
    {
        [Fact]
        public async void GetAllEmployeeTypeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeType, int>>();
            var employeeTypesInMemory = new List<EmployeeType>
            {
                new EmployeeType{Id = 1, TypeName = "Regular", DayLabel = "Absent Days", PayLabel = "Salary", Tax = 12},
                new EmployeeType{Id = 2, TypeName = "Contractual", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 0}
            };
            repo.Setup(m => m.GetAll()).ReturnsAsync(employeeTypesInMemory);
            var controller = new EmployeeTypeController(repo.Object);

            // act
            var actionResult = await controller.Get();

            // assert
            var result = actionResult as OkObjectResult;
            var employeeTypes = result.Value as IEnumerable<EmployeeType>;
            repo.Verify();
            Assert.Equal(employeeTypesInMemory.Count, employeeTypes.Count());
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetEmployeeTypeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeType, int>>();
            var employeeTypesInMemory = new List<EmployeeType>
            {
                new EmployeeType{Id = 1, TypeName = "Regular", DayLabel = "Absent Days", PayLabel = "Salary", Tax = 12},
                new EmployeeType{Id = 2, TypeName = "Contractual", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 0}
            };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeeTypesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeeTypeController(repo.Object);

            // act
            var actionResult = await controller.GetById(2);

            // assert
            var result = actionResult as OkObjectResult;
            var employeeType = result.Value as EmployeeType;
            repo.Verify();
            Assert.Equal("Contractual", employeeType.TypeName);
            Assert.Equal(2, employeeType.Id);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void SaveEmployeeTypeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeType, int>>();
            var employeeTypesInMemory = new List<EmployeeType>
            {
                new EmployeeType{Id = 1, TypeName = "Regular", DayLabel = "Absent Days", PayLabel = "Salary", Tax = 12},
                new EmployeeType{Id = 2, TypeName = "Contractual", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 0}
            };
            var newType = new EmployeeType { Id = 3, TypeName = "PartTime", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 5 };
            repo.Setup(m => m.Insert(newType));
            var controller = new EmployeeTypeController(repo.Object);

            // act
            var actionResult = await controller.Post(newType);
            var actionResultGet = await controller.GetById(3);

            // assert
            var result = actionResult as CreatedResult;
            Assert.Equal(201, result.StatusCode);

            var resultGet = actionResultGet as OkObjectResult;
            var employeeType = resultGet.Value as EmployeeDto;
            Assert.Equal("PartTime", employeeType.FullName);
        }

        [Fact]
        public async void EditEmployeeTypeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeType, int>>();
            var employeeTypesInMemory = new List<EmployeeType>
            {
                new EmployeeType{Id = 1, TypeName = "Regular", DayLabel = "Absent Days", PayLabel = "Salary", Tax = 12},
                new EmployeeType{Id = 2, TypeName = "Contractual", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 0}
            };
            var editType = new EmployeeType { Id = 2, TypeName = "PartTime", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 5 };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeeTypesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeeTypeController(repo.Object);

            // act
            var actionResult = await controller.Put(editType);
            var actionResultGet = await controller.GetById(2);

            // assert
            var result = actionResultGet as OkObjectResult;
            var employeeType = result.Value as EmployeeType;
            Assert.Equal("PartTime", employeeType.TypeName);

            var resultPut = actionResult as OkObjectResult;
            Assert.Equal(200, resultPut.StatusCode);
        }

        [Fact]
        public async void DeleteEmployeeTypeTest()
        {
            // arrange
            var repo = new Mock<IRepository<EmployeeType, int>>();
            var employeeTypesInMemory = new List<EmployeeType>
            {
                new EmployeeType{Id = 1, TypeName = "Regular", DayLabel = "Absent Days", PayLabel = "Salary", Tax = 12},
                new EmployeeType{Id = 2, TypeName = "Contractual", DayLabel = "Work Days", PayLabel = "Rate per day", Tax = 0}
            };
            repo.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((int i) => employeeTypesInMemory.Single(bo => bo.Id == i));
            var controller = new EmployeeTypeController(repo.Object);

            // act
            var actionResult = await controller.Delete(1);

            // assert
            var resultPut = actionResult as OkObjectResult;
            Assert.Equal(200, resultPut.StatusCode);
        }
    }
}
