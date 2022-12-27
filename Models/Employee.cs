using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; } 
        public DateTime ContractDate { get; set; }
        public int Sexo { get; set; }
    }
}