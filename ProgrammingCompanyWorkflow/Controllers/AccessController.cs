using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ProgrammingCompanyWorkflow.DAL;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.Controllers
{
    [Authorize(Roles = "admin")]
    public class AccessController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: /Access/
        public ActionResult Index()
        {
            return View(_unitOfWork.AccessRepository.Get().ToList());
        }

        // GET: /Access/Create
        public ActionResult Create()
        {
            PopulateFromRoleDropDown();
            PopulateToRoleDropDown();
            return View();
        }

        // POST: /Access/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id")] Access access, string FromRole, string ToRole)
        {
            ModelState.Remove("FromRole");
            ModelState.Remove("ToRole");

            if (ModelState.IsValid)
            {
                access.Id = Guid.NewGuid();
                access.FromRole = _unitOfWork.ApplicationRoleRepository.GetById(FromRole);
                access.ToRole = _unitOfWork.ApplicationRoleRepository.GetById(ToRole);
                _unitOfWork.AccessRepository.Insert(access);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            if (access.FromRole==null) PopulateFromRoleDropDown(); else PopulateFromRoleDropDown(access.FromRole.Id);
            if (access.ToRole == null) PopulateToRoleDropDown(); else PopulateToRoleDropDown(access.ToRole.Id);
            return View(access);
        }

        private void PopulateFromRoleDropDown(object selectedFromRole = null)
        {
            ViewBag.FromRole = new SelectList(_unitOfWork.ApplicationRoleRepository.Get(), "Id", "Name", selectedFromRole);
        }

        private void PopulateToRoleDropDown(object selectedToRole = null)
        {
            ViewBag.ToRole = new SelectList(_unitOfWork.ApplicationRoleRepository.Get(), "Id", "Name", selectedToRole);
        }

        // GET: /Access/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Access access = _unitOfWork.AccessRepository.GetById(id);
            if (access == null)
            {
                return HttpNotFound();
            }
            if (access.FromRole == null) PopulateFromRoleDropDown(); else PopulateFromRoleDropDown(access.FromRole.Id);
            if (access.ToRole == null) PopulateToRoleDropDown(); else PopulateToRoleDropDown(access.ToRole.Id);
            return View(access);
        }

        // POST: /Access/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] Access access, string FromRole, string ToRole)
        {
            ModelState.Remove("FromRole");
            ModelState.Remove("ToRole");
            var editAccess = _unitOfWork.AccessRepository.GetById(access.Id);
            
            if (ModelState.IsValid)
            {
                editAccess.FromRole = _unitOfWork.ApplicationRoleRepository.GetById(FromRole);
                editAccess.ToRole = _unitOfWork.ApplicationRoleRepository.GetById(ToRole);
                _unitOfWork.AccessRepository.Update(editAccess);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            if (editAccess.FromRole == null) PopulateFromRoleDropDown(); else PopulateFromRoleDropDown(access.FromRole.Id);
            if (editAccess.ToRole == null) PopulateToRoleDropDown(); else PopulateToRoleDropDown(access.ToRole.Id);
            return View(editAccess);
        }

        // GET: /Access/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Access access = _unitOfWork.AccessRepository.GetById(id);
            if (access == null)
            {
                return HttpNotFound();
            }
            return View(access);
        }

        // POST: /Access/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Access access = _unitOfWork.AccessRepository.GetById(id);
            _unitOfWork.AccessRepository.Delete(access);
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
