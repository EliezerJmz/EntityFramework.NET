using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace _4_EntityFramework.Models
{
    public class AppDbContext: DbContext
    {
        
        //cada DbSet representa a una tabla que tengamos en la base de datos
        //cada tabla esta representada por un modelo en este caso el modelo Persona
        public DbSet<Persona> Persona { get; set; }

        //Esta tabla contiene el campo que es de tipo int y que su nombre inicia con Codigo para que se cree como
        //primary key como lo indica en el metodo especial 
        public DbSet<Direccion> Direccion { get; set; }



        //TABLAS USADAS EN EL INNER JON:
        //cada DbSet representa a una tabla que tengamos en la base de datos
        public DbSet<Employee> Employees { get; set; }

        //Esta tabla contiene el campo que es de tipo int y que su nombre inicia con Codigo para que se cree como
        //primary key como lo indica en el metodo especial 
        public DbSet<Job> Jobs { get; set; }



        //API FLUENTE:
        //Metosdos Especiales API FLUENTE
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Permite cambiar el tipo de dato de un campo
            modelBuilder.Properties<DateTime>().Configure(x => x.HasColumnType("DateTime2"));


            //Permite indicar que campo sera primary Key
            //en este caso el campo que sea de tipo int y que su nombre inicio con Codigo
            modelBuilder.Properties<int>().Where(p => p.Name.StartsWith("Codigo")).Configure(p => p.IsKey());


            //Relacionar Modelos Direccion Persona 
            //toda entidad Direccion debe tener asignada una entidad Persona
            //se crea en la tabla Direccion un campo PersonaID que hace referencia una llave foranea a la tabla persona
            modelBuilder.Entity<Direccion>().HasRequired(x => x.Persona);



            //INNER JOIN
            //PK
            //Permite indicar que campo sera primary Key
            //en este caso el campo que sea de tipo int y que su nombre termine con Code
            modelBuilder.Properties<int>().Where(p => p.Name.EndsWith("Code")).Configure(p => p.IsKey());

            //FK
            //debemos indicar cual sera la FK en la tabla Job, aqui es EmployeeId
            modelBuilder.Entity<Job>().HasRequired(p => p.Employee).WithMany().HasForeignKey(P => P.EmployeeId);



            base.OnModelCreating(modelBuilder);
        }


        //Activar que se pueda validar al momento de eliminar
        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            if(entityEntry.State == EntityState.Deleted)
            {
                return true;
            }

            return base.ShouldValidateEntity(entityEntry);
        }

        //Permite que se valide eliminar en el modelo Persona
        //https://www.entityframeworktutorial.net/EntityFramework4.3/validate-entity-in-entity-framework.aspx

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if(entityEntry.Entity is Persona && entityEntry.State == EntityState.Deleted)
            {
                var entidad = entityEntry.Entity as Persona;
                if(entidad.Edad < 18)
                {
                    return new DbEntityValidationResult(entityEntry, new DbValidationError[]
                    {
                        new DbValidationError("Edad", "No Esta permitido Eliminar a un menor de 18 años.")
                    });
                }
            }

            return base.ValidateEntity(entityEntry, items);
        }

    }
}