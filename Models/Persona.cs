using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Nacimiento { get; set; }
        public int Edad { get; set; }
        //relacion cero a muchos, una persona puede tener muchas direccione.
        public virtual List<Direccion> Direcciones { get; set; }
    }
}