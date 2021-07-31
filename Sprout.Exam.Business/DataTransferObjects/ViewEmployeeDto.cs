using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class ViewEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Tin { get; set; }
        public string EmployeeType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
