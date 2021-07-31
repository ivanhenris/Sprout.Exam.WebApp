using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class SalaryDetails
    {
        public int Id { get; set; }
        public decimal Period { get; set; }
        public decimal BasePay { get; set; }
        public decimal Tax { get; set; }
        public EmployeeType EmployeeType { get; set; }
    }
}
