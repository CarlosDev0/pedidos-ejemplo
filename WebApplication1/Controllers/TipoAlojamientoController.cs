using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class TipoAlojamientoController : Controller
    {
        private DefaultConnection db = new DefaultConnection();

        // GET: TipoAlojamiento
        public ActionResult Index()
        {
            return View(db.TipoAlojamiento.ToList());
        }

        // GET: TipoAlojamiento/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoAlojamiento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoAlojamiento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoAlojamiento collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.TipoAlojamiento.Add(collection);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(collection);
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoAlojamiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            TipoAlojamiento tpAlojamiento = db.TipoAlojamiento.Find(id);
            if (tpAlojamiento == null)
                return HttpNotFound();


            return View(tpAlojamiento);
        }

        // POST: TipoAlojamiento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoAlojamiento collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(collection).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(collection);
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoAlojamiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            TipoAlojamiento tpAlojamiento = db.TipoAlojamiento.Find(id);
            if (tpAlojamiento == null)
                return HttpNotFound();


            return View(tpAlojamiento);
        }

        // POST: TipoAlojamiento/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                TipoAlojamiento tpAloja = db.TipoAlojamiento.Find(id);
                db.TipoAlojamiento.Remove(tpAloja);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
