using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTypeController : ControllerBase
    {
        private readonly IRepository<EmployeeType, int> _employeeTypeRepository;

        public EmployeeTypeController(IRepository<EmployeeType, int> employeeTypeRepository)
        {
            _employeeTypeRepository = employeeTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeTypeRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeTypeRepository.GetById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EmployeeType input)
        {
            var item = await _employeeTypeRepository.GetById(input.Id);
            if (item == null) return NotFound();
            item.TypeName = input.TypeName;

            await _employeeTypeRepository.Update(item);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmployeeType input)
        {
            var id = 0;

            var employeeType = new EmployeeType
            {
                Id = id,
                TypeName = input.TypeName
            };

            await _employeeTypeRepository.Insert(employeeType);
            await _employeeTypeRepository.Save();

            return Created($"/api/employeetype/{id}", id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeTypeRepository.Delete(id);
            await _employeeTypeRepository.Save();
            return Ok(id);
        }
    }
}
