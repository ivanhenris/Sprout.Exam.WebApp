using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Repository;
using Sprout.Exam.Business.Calculation;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<EmployeeDto, int> _employeeRepository;

        public EmployeesController(IRepository<EmployeeDto, int> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeRepository.GetAll();
            List<ViewEmployeeDto> viewResults = new List<ViewEmployeeDto>();
            foreach(var employee in result)
            {
                var employeeType = (Common.Enums.EmployeeType)employee.TypeId;
                var viewEmployee = new ViewEmployeeDto
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Birthdate = employee.Birthdate,
                    Tin = employee.Tin,
                    EmployeeType = employeeType.ToString(),
                    IsDeleted = employee.IsDeleted,
                    BasePay = employee.BasePay
                };
                viewResults.Add(viewEmployee);
            }
            return Ok(viewResults);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeRepository.GetById(id);
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            var item = await _employeeRepository.GetById(input.Id);
            if (item == null) return NotFound();
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate;
            item.TypeId = input.TypeId;
            item.BasePay = input.BasePay;

            await _employeeRepository.Update(item);
            return Ok(item);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            var id = 0;

            var employee = new EmployeeDto
            {
                Birthdate = input.Birthdate,
                FullName = input.FullName,
                Id = id,
                Tin = input.Tin,
                TypeId = input.TypeId,
                BasePay = input.BasePay
            };

            await _employeeRepository.Insert(employee);
            await _employeeRepository.Save();

            return Created($"/api/employees/{id}", id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeRepository.Delete(id);
            await _employeeRepository.Save();
            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(SalaryDetails salaryDetails)
        {
            if (salaryDetails.EmployeeType == null) return NotFound("Employee Type not found");

            var type = (Common.Enums.EmployeeType) salaryDetails.EmployeeType.Id;
            var calculator = new SalaryCalculatorFactory(type).GetEmployeeTypeSalaryCalculator();
            if(calculator != null)
            {
                var salary = await Task.Run(() => calculator.CalculateSalary(salaryDetails));
                return Ok(salary);
            }

            return NotFound("Employee Type not found");
        }
    }
}
