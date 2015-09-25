using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SharePointAppProdDeployment_TESTWeb.Models;

namespace SharePointAppProdDeployment_TESTWeb.Controllers
{
    public class AuditLogEntriesController : Controller
    {
        private Model1 db = new Model1();

        // GET: AuditLogEntries
        public ActionResult Index()
        {
            return View(db.AuditLogEntries.ToList().AsEnumerable().Reverse());
        }

        // GET: AuditLogEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditLogEntry auditLogEntry = db.AuditLogEntries.Find(id);
            if (auditLogEntry == null)
            {
                return HttpNotFound();
            }
            return View(auditLogEntry);
        }

        // GET: AuditLogEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuditLogEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Date,Message")] AuditLogEntry auditLogEntry)
        {
            if (ModelState.IsValid)
            {
                db.AuditLogEntries.Add(auditLogEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auditLogEntry);
        }

        // GET: AuditLogEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditLogEntry auditLogEntry = db.AuditLogEntries.Find(id);
            if (auditLogEntry == null)
            {
                return HttpNotFound();
            }
            return View(auditLogEntry);
        }

        // POST: AuditLogEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,Message")] AuditLogEntry auditLogEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditLogEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auditLogEntry);
        }

        // GET: AuditLogEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditLogEntry auditLogEntry = db.AuditLogEntries.Find(id);
            if (auditLogEntry == null)
            {
                return HttpNotFound();
            }
            return View(auditLogEntry);
        }

        // POST: AuditLogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditLogEntry auditLogEntry = db.AuditLogEntries.Find(id);
            db.AuditLogEntries.Remove(auditLogEntry);
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
