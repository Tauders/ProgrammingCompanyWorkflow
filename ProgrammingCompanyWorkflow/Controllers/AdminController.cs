using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProgrammingCompanyWorkflow.DAL;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public AdminController()
        : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View(_unitOfWork.ApplicationUserRepository.Get());
        }

        // GET: /Admin/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser project = _unitOfWork.ApplicationUserRepository.GetById(id.ToString());
            if (project == null)
            {
                return HttpNotFound();
            }
            PopulateRoleDropDown(project.Roles.ToList()[0].RoleId);
            return View(project);
        }

        // POST: /Admin/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,UserName")] ApplicationUser project,string Roles)
        {
            var editproject = _unitOfWork.ApplicationUserRepository.GetById(project.Id);
            editproject.UserName = project.UserName;
            if (ModelState.IsValid)
            {
                //var temp =
                UserManager.RemoveFromRole(project.Id, editproject.Roles.ToList()[0].Role.Name);
                UserManager.AddToRole(project.Id, _unitOfWork.ApplicationRoleRepository.GetById(Roles).Name);
                _unitOfWork.ApplicationUserRepository.Update(editproject);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            PopulateRoleDropDown();
            return View(project);
        }

        private void PopulateRoleDropDown(object selectedRole = null)
        {
            ViewBag.Roles = new SelectList(_unitOfWork.ApplicationRoleRepository.Get(), "Id", "Name", selectedRole);
        }

        // GET: /Admin/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser project = _unitOfWork.ApplicationUserRepository.GetById(id.ToString());
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ApplicationUser project = _unitOfWork.ApplicationUserRepository.GetById(id.ToString());
            _unitOfWork.ApplicationUserRepository.Delete(project);
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