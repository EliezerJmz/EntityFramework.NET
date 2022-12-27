using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _4_EntityFramework.Models;
using _4_EntityFramework.Models.Anonimo;
using _4_EntityFramework.Models.SqlPeersonalizado;

namespace _4_EntityFramework.Controllers
{
    public class EmployeesController : Controller
    {
        //Propiedad tipo DbContext:
        public AppDbContext db = new AppDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,ContractDate,Sexo")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,ContractDate,Sexo")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //******************************** INNER JOIN ******************************************
        //INNER JOIN NO NECESITA PROPIEDADES DE NAVEGACION.
        //obtener el Employee y su tabajo con codigo JobCode == 1:
        //ANONIMO utilizamos una un objeto anomino y su clase para maperar los campos
        public ActionResult OneEmployeeInnerJob()
        {
            //Hacemos el INNER JOIN ENTRE Jobs y Employees:
            var JobInnerJoinEmployee = db.Jobs.Join(db.Employees,    //Indicamos la union entre la tabla Jobs y la tabla Employees.
                    job => job.EmployeeId, emp => emp.Id,   //la union sera usando los campos FK Job.EmployeeId y PK Employee.Id, usando lambda.
                        (job, emp) => new JobsInnerEmployeeAnonimo() {    //Mapeamos los campos a la clase anonima
                            JobCode = job.JobCode,
                            JobDescription = job.JobDescription, 
                            EmployeeId = job.EmployeeId,
                            Employee = job.Employee,
                            //campos tabla Employee
                            Id = emp.Id,
                            Name = emp.Name,
                            Age = emp.Age,
                            ContractDate = emp.ContractDate
                        }).FirstOrDefault(x => x.JobCode == 1);    //De la union  (job, emp) Creamos un objeto anonimo y obtenemos el primer resultado de JobCode == 1.


            //Retornamos la consulta con los campos del objeto y la clase anonima JobsInnerEmployeeAnonimo:
            return View(JobInnerJoinEmployee);
        }

        //GROUPJOIN
        //USA CLASE Y OBJETO ANONIMO: Para mapear los campos de ambas tablas y enviarlos a la vista.
        //los campos de una taba seran de tipo IEnumerable, en este caso la tabla Job
        //Obtener a una persoan y su listado de trabajos:
        public ActionResult GroupJoinEmployeeJob()
        {
            var query = db.Employees.GroupJoin(db.Jobs, //la tabla que devolvera IEnumerable
            emp => emp.Id,          //la relación entre las llaves PK y FK                    
            job => job.EmployeeId,
            (emp, jobCollection) => //Lambda para emp y jobCollection indicamos que job sera de tipo collction para tnenerlo presente
                new groupJoinAnonimo()  //Objeto de tipo  groupJoinAnonimo, la clase que definimos para mapear los campos de ambas tablas
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Age = emp.Age,
                    ContractDate = emp.ContractDate,
                    //los campos de la tabla job son de tipo IEnumerable, los definimos asi enel objeto y clae anonimo
                    JobCode = jobCollection.Select(job => job.JobCode), //seleccina todos los registros
                    Jobs = jobCollection.Select(job => job.JobDescription)
                }).FirstOrDefault(x => x.Id == 1);

            return View(query); //enviamos la consulta en la vista debe recibir un model de tipo  groupJoinAnonimo,
        }

        //GROUPJOIN
        //obtener a todos los empleados y sus trabajos:
        public ActionResult GroupJoinAllEmployeeJob()
        {
            var query = db.Employees.GroupJoin(db.Jobs,
                emp => emp.Id,
                job => job.EmployeeId,
                (emp, jobCollection) =>
                    new groupJoinAnonimo()
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Age = emp.Age,
                        ContractDate = emp.ContractDate,

                        JobCode = jobCollection.Select(job => job.JobCode),
                        Jobs= jobCollection.Select(job => job.JobDescription)
                    }
                );

            return View(query);
        }


        //********************************** CONSULTAS SQL *********************************
        //Obtener a todos los Empleados:
        public ActionResult SqlAllEmployees()
        {
            var query = db.Employees.SqlQuery("SELECT * FROM dbo.Employees").ToList();

            return View(query);
        }

        //Consulta SQL con paso de parametros e indicando el model de la consuta:
        //buscar al Employee con Id = 1:
        public ActionResult SqlParametros()
        {
            var query = db.Database.SqlQuery<Employee>(
                @"SELECT * FROM dbo.Employees
                    WHERE  Id = @Id", new SqlParameter("@Id", 1)).FirstOrDefault();

            return View(query);
        }

        //CONCULTA PERSONZLIZADA:
        //Cargar a un modelo no mapeado en al base de datos, la consulta de una tabla de la base de datos:
        //Debemos crear el modelo no mapeado.
        //GROUPBY
        //usanso group by contar la cantidad de hombre y mujeres.
        //creamo una clase HombresMujeres para almacenar la consulta
        //creamos una clase Enum para el tipo Sexo que pueda tener valores de 1 y 2.
        public ActionResult SqlPersonalizado()
        {
            var query = db.Database.SqlQuery<HombresMujers>(
                    @"SELECT Sexo, COUNT(*) AS Cantidad
                      FROM dbo.Employees
                        GROUP BY Sexo").ToList();

            return View(query);
        }


       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
