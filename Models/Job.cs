using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models
{
    public class Job
    {
        //las PK y FK estan definidas en el  AppDbContext
        public int JobCode { get; set; }
        public string JobDescription { get; set; }
        //este campo sera la FK lo agregamos manualmente y se indica en el AppContext que es FK
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}