using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _4_EntityFramework.Models;
using _4_EntityFramework.Models.Anonimo;

namespace _4_EntityFramework.Controllers
{
    public class PersonasController : Controller
    {
       //propiedad tipo DbContext
        private AppDbContext db = new AppDbContext();

        // GET: Personas
        public ActionResult Index()
        {
            return View(db.Persona.ToList());
        }

        // GET: Personas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        // ADD RANGE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Nacimiento,Edad")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                //Crear uN listado de personas que se agregaran automaticamente al insertar un registro
                var personas = new List<Persona>() { persona }; //agregamos al objeto persona que viene como parametro
                //vamos a agregar 5 registoes
                for(int i=0; i<=5; i++)
                {
                    personas.Add(new Persona() { Nombre = "Persona_"+i,Edad = 42, Nacimiento = new DateTime(2022, 07, 07) });
                }

                db.Persona.AddRange(personas);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(persona);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Nacimiento,Edad")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(persona);
        }




        //****************************** EDIT PERSONALIZAZDO *****************************************************

        // GET: Personas/Edit/5
        public ActionResult EditPesonalizado(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPesonalizado([Bind(Include = "Id,Nombre,Nacimiento,Edad")] Persona persona)
        {
            if (ModelState.IsValid)
            {

                //Método 1: Trae todo el objeto y lo actualiza
                //el primer registro que tenga el Id == 2
                var personaEditar = db.Persona.FirstOrDefault(x => x.Id == 2);
                personaEditar.Nombre = "Nombre Editado Con Método Personalizado 1";
                personaEditar.Edad = 2000;

                //Método 2: Actualizacion Parcial
                //Solo vamos a Actualizar el campo Nombre el Edad no se debe Actualizar
                var personaEditar2 = new Persona();
                personaEditar2.Id = 6;
                personaEditar2.Nombre = "Nombre Editado Con Método Personalizado 2";
                personaEditar2.Edad = 500;
                //indicamos el objeto que se va actualizar en la db
                db.Persona.Attach(personaEditar2);
                //indicamos que propiedad campo es el que se va a Actualizar
                db.Entry(personaEditar2).Property(x => x.Nombre).IsModified = true;
            
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(persona);
        }

        //***************************************************************************************


        // GET: Personas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Persona persona = db.Persona.Find(id);
                db.Persona.Remove(persona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityErr.ValidationErrors)
                    {
                        ViewBag.Error = "Error en el Campo:";
                        ViewBag.Campo = error.PropertyName;
                        ViewBag.Mensaje = error.ErrorMessage;
                    }
                }
                return View("DeleteError");
            }
        }


        //**************************************** DELETE LISTA ****************************************
        public ActionResult DeleteLista()
        {
             //le enviamos una lista con los Id que vamos a eliminar
             //en este caso los Id maayores a 13

            var personas = db.Persona.Where(x => x.Id >= 13).ToList();

            db.Persona.RemoveRange(personas);
            db.SaveChanges();
         
            return RedirectToAction("Index"); 
        }
        //**********************************************************************************************

        //********************************* FORMAS DE SELECCIONAR CAMPOS  ******************************
        //1:
        //Seleccionar Todas Las Columnas de una Tabla:
        public ActionResult AllCampos()
        {
            var listaCampos = db.Persona.ToList();
            return View(listaCampos);
        }

        //2:
        //Seleccionar una Columna:
        //La consulta devuelve una lista tipo String
        public ActionResult SelectCampo() 
        {
            var model = db.Persona.Select(x => x.Nombre).ToList();

            //en la vista debemos indicar que vamos a recibir una lista de tipo string
            return View(model);
        }

        //3: OBJETO ANONIMO
        //Seleccionar varios campos y proyectarlos a una clase de tipo anomima, es anomima por que no tiene nombre.
        //Para poder usar el objeto anomimo en la Vista es necesario mamper a una nueva clase 
        //que contengoa solo los campos a utilizar en este caso Nombre y Edad.
        public ActionResult VariosCampos()
        {
            List<PersonaNombreEdad> model = db.Persona.Select(x => new PersonaNombreEdad() { Nombre = x.Nombre, Edad = x.Edad }).ToList();

            return View(model);
        }

        //4: PROYECCTAR A CLASE PARA NO USAR ANONIMO
        //Seleccionar varis campos y proyectarlo sobre la misma clase para no usar objetos anonimos:
        //en este metodo retornamos un listado de objeto persona.
        //para poder devolver un listado de personas sin usar objeto anonimo. realializamos un select a persona con un objeto anonimo
        //y devuelve un list
        //sobre ese list realizamos nuevamente otro select pero ahora crearmos un objeto persona que retornamos a la vista.
        public ActionResult VariosCamposPersona()
        {
            var model = db.Persona.Select(x => new { Nombre = x.Nombre, Edad = x.Edad }).ToList()
                .Select(x => new Persona() { Nombre = x.Nombre, Edad = x.Edad }).ToList();

            return View(model);
        }
        //**********************************************************************************************


        //***************************** LLAVES FORANEAS API FLUENTE ************************************
        // GET: Direciones
        public ActionResult AgregarDireccionFK()
        {
           
            //creamos un objeto para indicar que Id vamos a usar
            var persona = new Persona() { Id = 11 };
            //con Attach indicamos que el registro ya existe en la base de datos, para que no intete crear uno nuevo
            //sin no que es un exitente
            db.Persona.Attach(persona);
            //indicamos que queremos asignar una nueva direccion a persona con Id=9, si no usamos Attach intenta crear uno nuevo.
            db.Direccion.Add(new Direccion() { Calle = "Calle principal 1", Persona = persona });
            db.SaveChanges();
           
            return View();
        }

        //LAZY LOADING:
        //VIRTUAL:
        //Obtener la direccion de una persona usando Virtual Lazy Loading en el modelo
        //esto utiliza varias consultas.
        public ActionResult PersonaDireccionVirtual()
        {
          //SE REALIZAN DOS CONSULTAS SEPARADAS:

            //obtenemos a la persona con Id ==11
            var persona = db.Persona.FirstOrDefault(x => x.Id == 11);
            //de la persona con Id = 11 Obtenemos las direcciones que tiene registradas
            //para obtener las direcciones devemos agregar Virtual en el modelo.
            var model = persona.Direcciones.ToList();

            ViewBag.PersonaId11 = persona;

            return View(model);
        }

        //EAGER LOADING
        //INCLUDE:
        //obtener todas las personas con su direcciones
        //Con include usamos una sola consulta
        //para obtener los datos del campo direcciones de la tabla direcion
        public ActionResult PersonaDireccionInclude()
        {
            var model = db.Persona.Include(x => x.Direcciones).ToList();
            return View(model);
        }

        //Obtener a una sola persona:
        public ActionResult UnaPersonaInclude()
        {

            //buscamos a la persona con Id = 11, y incluimos su direccion en una sola consulta:
            var model = db.Persona.Include("Direcciones").FirstOrDefault(x => x.Id == 11);

            //retorna un solo Objeto en la vista no necesitamos un IEnumerable para recorrerlo.
            return View(model);
        }

        //LAZY LOADING
        //VIRTUAL:
        //Obtener el Nombre de la pesona por medio de su dirección:
        //para tener acceso al nombre debemos colocar virtual en campo persona del modelo direccion:
        public ActionResult ObtenerNombrePorDireccion()
        {
            //hacemos una consulta para obtener la direccion con CodigoDireccion = 4:
            var direccion = db.Direccion.FirstOrDefault(d => d.CodigoDireccion == 4);
  
            //Enviar solo el nombre usando un Viewbag
            ViewBag.nombre = direccion.Persona.Nombre;

            //retornamos un objeto direccion que contiene todos los campos
            return View(direccion);
        }


        //**********************************************************************************************


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
