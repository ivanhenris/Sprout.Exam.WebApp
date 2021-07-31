using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class SalaryDetails
    {
        public int Id { get; set; }
        public decimal WorkedDays { get; set; }
        public decimal AbsentDays { get; set; }
    }
}
