using Sprout.Exam.Business.Calculation.EmployeeTypes;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Calculation
{
    public class SalaryCalculatorFactory
    {
        private readonly Common.Enums.EmployeeType _employeeType;

        public SalaryCalculatorFactory(Common.Enums.EmployeeType employeeType)
        {
            _employeeType = employeeType;
        }

        public IEmployeeType GetEmployeeTypeSalaryCalculator()
        {
            switch (_employeeType)
            {
                case Common.Enums.EmployeeType.Regular:
                    return new Regular();
                case Common.Enums.EmployeeType.Contractual:
                    return new Contractual();
            };
            return null;
        }
    }
}
