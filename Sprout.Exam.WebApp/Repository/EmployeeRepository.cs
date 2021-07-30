using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Repository
{
    public class EmployeeRepository : IRepository<EmployeeDto, int>
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Remove(employee);
            }
        }

        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<EmployeeDto> GetById(int id)
        {
            return await _context.Employee.FindAsync(id);
        }

        public async Task<EmployeeDto> Insert(EmployeeDto entity)
        {
            await _context.Employee.AddAsync(entity);
            return entity;
        }

        public async Task Update(EmployeeDto employeeDto)
        {
            var employee = await _context.Employee.FindAsync(employeeDto.Id);
            if(employee != null)
            {
                employee.FullName = employeeDto.FullName;
                employee.Tin = employeeDto.Tin;
                employee.Birthdate = employeeDto.Birthdate;
                employee.EmployeeTypeId = employeeDto.EmployeeTypeId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
