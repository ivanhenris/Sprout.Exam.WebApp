using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Calculation.EmployeeTypes
{
    class Regular : IEmployeeType
    {
        public Regular()
        {

        }

        public decimal CalculateSalary(SalaryDetails salaryDetails)
        {
            var tax = salaryDetails.Tax/100;
            var basePay = salaryDetails.BasePay;
            var absentDays = salaryDetails.Period;
            return basePay - ((basePay / 22) * absentDays) - (basePay * tax);
        }
    }
}
