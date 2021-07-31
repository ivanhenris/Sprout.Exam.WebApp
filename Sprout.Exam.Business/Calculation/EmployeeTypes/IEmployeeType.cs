using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Calculation.EmployeeTypes
{
    public interface IEmployeeType
    {
        decimal CalculateSalary(SalaryDetails salaryDetails);
    }
}
