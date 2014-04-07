using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProgrammingCompanyWorkflow.DAL;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.Controllers
{
    public class RoleController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        //
        // GET: /Role/
        public ActionResult Index()
        {
            return View(_unitOfWork.ApplicationRoleRepository.Get());
        }

        // GET: /Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Role/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")] ApplicationRole project)
        {


            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid().ToString();

                _unitOfWork.ApplicationRoleRepository.Insert(project);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: /Role/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole project = _unitOfWork.ApplicationRoleRepository.GetById(id.ToString());
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Role/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name")] ApplicationRole project)
        {
            var editproject = _unitOfWork.ApplicationRoleRepository.GetById(project.Id);
            editproject.Name = project.Name;
            if (ModelState.IsValid)
            {
                _unitOfWork.ApplicationRoleRepository.Update(editproject);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: /Role/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole project = _unitOfWork.ApplicationRoleRepository.GetById(id.ToString());
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ApplicationRole project = _unitOfWork.ApplicationRoleRepository.GetById(id.ToString());
            _unitOfWork.ApplicationRoleRepository.Delete(project);
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