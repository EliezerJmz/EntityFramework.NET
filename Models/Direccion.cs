using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models
{
    public class Direccion
    {
        //las PK y FK estan definidas en el  AppDbContext
        public int CodigoDireccion { get; set; }
        public string Calle { get; set; }
        //relaciones cero a muchos a una direccion le corresponde un objeto Persona
        public virtual Persona Persona { get; set; }
    }
}