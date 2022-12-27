using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models.Anonimo
{
    public class JobsInnerEmployeeAnonimo
    {
        public int JobCode { get; set; }
        public string JobDescription { get; set; }
        //este campo sera la FK lo agregamos manualmente
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime ContractDate { get; set; }
    }
}