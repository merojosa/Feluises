using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.Controllers
{
    public class CredentialsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Credentials
        // EFE: Lista todas las personas con cuenta.
        public ActionResult Index()
        {
            return View(db.Credentials.ToList());
        }

        // GET: Credentials/Details/5
        // Detalles de la persona con cuenta
        //REQ: Persona válida
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return HttpNotFound();
            }
            return View(credentials);
        }

        // GET: Credentials/Create
        //EFE: Crea un nuevo usuario para el sistema.
        public ActionResult Create()
        {
            return View();
        }

        // POST: Credentials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //EFE: Valida los campos de crear.
        //REQ: Campos obligatorios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userName,password,email")] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                db.Credentials.Add(credentials);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(credentials);
        }

        // GET: Credentials/Edit/5
        //EFE: Edita el usr
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return HttpNotFound();
            }
            return View(credentials);
        }

        // POST: Credentials/Edit/5
        //EFE: VAlida los campos
        //REQ: Campos válidos
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userName,password,email")] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credentials).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(credentials);
        }

        // GET: Credentials/Delete/5
        //EFE: Elimina al usr seleccionado
        //REQ: USR válido
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return HttpNotFound();
            }
            return View(credentials);
        }

        // POST: Credentials/Delete/5
        //EFE: Confirma que se hayan guardado los campos.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Credentials credentials = db.Credentials.Find(id);
            db.Credentials.Remove(credentials);
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
