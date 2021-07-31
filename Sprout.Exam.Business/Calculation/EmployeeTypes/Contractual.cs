using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Calculation.EmployeeTypes
{
    class Contractual : IEmployeeType
    {
        public Contractual()
        {

        }

        public decimal CalculateSalary(SalaryDetails salaryDetails)
        {
            var basePay = salaryDetails.BasePay;
            var workDays = salaryDetails.Period;
            return basePay * workDays;
        }
    }
}
