using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProgrammingCompanyWorkflow.DAL;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.Controllers
{
    public class ComponentController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: /Component/
        public ActionResult Index()
        {
            return View(_unitOfWork.ComponentRepository.Get());
        }

        // GET: /Component/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Component/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name")] Component component)
        {
            if (ModelState.IsValid)
            {
                component.Id = Guid.NewGuid();
                _unitOfWork.ComponentRepository.Insert(component);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(component);
        }

        // GET: /Component/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = _unitOfWork.ComponentRepository.GetById(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // POST: /Component/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name")] Component component)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ComponentRepository.Update(component);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(component);
        }

        // GET: /Component/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = _unitOfWork.ComponentRepository.GetById(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // POST: /Component/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Component component = _unitOfWork.ComponentRepository.GetById(id);
            _unitOfWork.ComponentRepository.Delete(component);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
