using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.Controllers
{
    public class ClientsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Clients
        //EFE: Muestra todos los clientes disponibles
        public ActionResult Index()
        {
            //[Display(Name= "Nombre")]
            return View(db.Client.ToList());
        }

        // GET: Clients/Details/5
        // EFE: Detalle del cliente seleccionado
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        //EFE: Crea un nuevo cliente con sus respectivos a atributos.
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // EFE: Verifica que los campos llenados sean válidos y que se puedan guardar en la base de datos.
        //REQ: Campos obligatorios llenos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idClientPK,clientName,clientLastName,clientSecondLastName,company,tel,email")] Client client)
        {

            if ( db.Client.Any(x => x.idClientPK == client.idClientPK) || db.Employee.Any(x => x.idEmployeePK == client.idClientPK) )
            {
                ModelState.AddModelError("idClientPK", "Ya existe un usuario registrado con dicha cédula");
                return View(client);
            }
            if (ModelState.IsValid)
            {
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }


        // GET: Clients/Edit/5
        // EFE: Edita la información del cliente seleccionado.
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //EFE: Valida la información intoducida. (No cambia cédula
        //REQ: Campos obligatorios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idClientPK,clientName,clientLastName,clientSecondLastName,company,tel,email")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        // EFE: Muestra información del cliente seleccionado para eliminar
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        //EFE: Confirma si la acción se realizó con éxito
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Client client = db.Client.Find(id);
            db.Client.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
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
