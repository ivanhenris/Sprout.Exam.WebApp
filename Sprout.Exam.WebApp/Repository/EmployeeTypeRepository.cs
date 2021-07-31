using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Repository
{
    public class EmployeeTypeRepository : IRepository<EmployeeType, int>
    {
        private readonly ApplicationDbContext _context;
        public EmployeeTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var employee = await _context.EmployeeType.FindAsync(id);
            if (employee != null)
            {
                _context.Remove(employee);
            }
        }

        public async Task<IEnumerable<EmployeeType>> GetAll()
        {
            return await _context.EmployeeType.ToListAsync();
        }

        public async Task<EmployeeType> GetById(int id)
        {
            return await _context.EmployeeType.FindAsync(id);
        }

        public async Task<EmployeeType> Insert(EmployeeType entity)
        {
            await _context.EmployeeType.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(EmployeeType input)
        {
            var employeeType = await _context.EmployeeType.FindAsync(input.Id);
            if (employeeType != null)
            {
                employeeType.Id = input.Id;
                employeeType.TypeName = input.TypeName;
                employeeType.PayLabel = input.PayLabel;
                employeeType.DayLabel = input.DayLabel;
                employeeType.Tax = input.Tax;
                await _context.SaveChangesAsync();
            }
        }
    }
}
