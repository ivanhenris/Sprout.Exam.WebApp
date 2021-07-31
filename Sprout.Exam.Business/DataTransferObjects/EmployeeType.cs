using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class EmployeeType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string PayLabel { get; set; }
        public string DayLabel { get; set; }
        public decimal Tax { get; set; }
    }
}
