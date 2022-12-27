using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models.Anonimo
{
    public class groupJoinAnonimo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime ContractDate { get; set; }
        //los campos de la tabla Jobs debenser de tipo IEnumerable
        public IEnumerable<int> JobCode { get; set; }
        public IEnumerable<string> Jobs { get; set; }
    }
}