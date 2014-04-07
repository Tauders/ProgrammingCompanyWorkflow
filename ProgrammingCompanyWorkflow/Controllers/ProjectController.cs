using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProgrammingCompanyWorkflow.DAL;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        //public UserManager<ApplicationUser> UserManager { get; set; }
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Project/
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var temp = new List<Project>();
            temp.AddRange(_unitOfWork.ProjectRepository.Get().Where(x => x.CreatingUser.Id == id.ToString()).ToList());
            temp.AddRange(_unitOfWork.ProjectRepository.Get().Where(x => x.CurrentResponseUser.Id == id.ToString()).ToList());

            return View(temp.Distinct());
        }

        // GET: /Project/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _unitOfWork.ProjectRepository.GetById(id);
            ViewBag.ListOfComponent =
                _unitOfWork.ProjectComponentRepository.Get().Where(x => x.Project.Id == project.Id).ToList();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: /Project/Create
        public ActionResult Create()
        {
            PopulateComponentDropDown();
            return View();
        }

        // POST: /Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TemporalClass temporal)
        {

            
            if (ModelState.IsValid)
            {
                temporal.Project.Id = Guid.NewGuid();

                temporal.Project.CreatingTime = DateTime.Now;
                temporal.Project.LastModifyTime = DateTime.Now;

                var iduser = User.Identity.GetUserId();

                temporal.Project.CreatingUser = _unitOfWork.ApplicationUserRepository.GetById(iduser);
                temporal.Project.CurrentResponseUser = _unitOfWork.ApplicationUserRepository.GetById(iduser);

                if (temporal.ProjectComponents != null)
                {
                    foreach (var project in temporal.ProjectComponents)
                    {
                        if (project.CountView != "-1")
                        {
                            var temp = new ProjectComponent
                            {
                                Id = Guid.NewGuid(),
                                Project = temporal.Project,
                                Component = _unitOfWork.ComponentRepository.GetById(new Guid(project.ComponentId)),
                                Count = Convert.ToInt32(project.CountView)
                            };
                            _unitOfWork.ProjectComponentRepository.Insert(temp);
                        }
                    }
                }
                _unitOfWork.ProjectRepository.Insert(temporal.Project);
                _unitOfWork.Save();
                return RedirectToAction("Index",new{id=iduser});
            }

            PopulateComponentDropDown();

            return View(temporal);
        }

        private void PopulateComponentDropDown(object selectedComponent = null)
        {
            ViewBag.ComponentId = new SelectList(_unitOfWork.ComponentRepository.Get(), "Id", "Name", selectedComponent);
        }

        // GET: /Project/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _unitOfWork.ProjectRepository.GetById(id);

            //ViewBag.ListOfComponent = new SelectList(_unitOfWork.ComponentRepository.Get(), "Id", "Name");
            ViewBag.ListOfComponent = _unitOfWork.ComponentRepository.Get().ToList();
            if (project == null || project.CurrentResponseUser.Id!=User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            PopulateComponentDropDown();
            return View(project);
        }

        // POST: /Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Project project)
        {

            var editproject = _unitOfWork.ProjectRepository.GetById(project.Id);
            editproject.Body = project.Body;
            editproject.Comment = project.Comment;
            editproject.Name = project.Name;
            editproject.LastModifyTime = DateTime.Now;

            if (editproject.CurrentResponseUser.Id != User.Identity.GetUserId()) { return HttpNotFound(); }
            
            if (ModelState.IsValid)
            {
                foreach (var projectComponent in project.ProjectComponents)
                {
                    if (projectComponent.Id == Guid.Empty)
                    {
                        var temp = new ProjectComponent
                        {
                            Id = Guid.NewGuid(),
                            Project = editproject,
                            Component = _unitOfWork.ComponentRepository.GetById(projectComponent.Component.Id),
                            Count = Convert.ToInt32(projectComponent.Count)
                        };
                        _unitOfWork.ProjectComponentRepository.Insert(temp);
                        //break;
                    }
                    else
                    {
                        var proj = _unitOfWork.ProjectComponentRepository.GetById(projectComponent.Id);
                        if (projectComponent.Count == -1)
                        {
                            _unitOfWork.ProjectComponentRepository.Delete(proj);
                        }
                        else
                        {
                            proj.Count = projectComponent.Count;
                            proj.Component = _unitOfWork.ComponentRepository.GetById(projectComponent.Component.Id);
                            _unitOfWork.ProjectComponentRepository.Update(proj);
                        }
                    }
                }
                _unitOfWork.ProjectRepository.Update(editproject);
                _unitOfWork.Save();
                return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
            }
            ViewBag.ListOfComponent = _unitOfWork.ComponentRepository.Get().ToList();
            PopulateComponentDropDown();
            return View(editproject);
        }

        public ActionResult Change(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _unitOfWork.ProjectRepository.GetById(id);
            if (project == null || project.CurrentResponseUser.Id != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            PopulateRoleDropDown();
            return View(project);
        }

        // POST: /Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change([Bind(Include = "Id")] Project project, string Roles)
        {
            var editproject = _unitOfWork.ProjectRepository.GetById(project.Id);
            editproject.LastModifyTime = DateTime.Now;
            var user = new ApplicationUser();
            if (_unitOfWork.ProjectMovementRepository.Get().Any(x => x.Project.Id == project.Id))
            {
                var temp2 =
                    _unitOfWork.IdentityUserRoleRepository.Get()
                        .FirstOrDefault(x => x.RoleId == Roles);
                foreach (var source in _unitOfWork.ProjectMovementRepository.Get().Where(x=>x.Project.Id==project.Id))
                {
                    if (source.FromUser.Roles.Contains(temp2))
                        user = source.FromUser;
                    if (source.ToUser.Roles.Contains(temp2))
                        user = source.ToUser;
                }
            }
            if (String.IsNullOrEmpty(user.UserName))
            {
                var temp =
                    _unitOfWork.IdentityUserRoleRepository.Get()
                        .Where(x => x.RoleId == Roles)
                        .ToList()
                        .Select(x => x.User)
                        .ToList();
                var temp1 =
                    temp.Select(identityUser => _unitOfWork.ApplicationUserRepository.GetById(identityUser.Id)).ToList();
                    //пользователи, у которых нужная роль

                Dictionary<ApplicationUser, int> teemp = temp1.ToDictionary(applicationUser => applicationUser,
                    applicationUser => applicationUser.Projects.Count);

                foreach (var i in teemp.Where(i => i.Value == teemp.Min(x => x.Value)))
                {
                    user = i.Key;
                }
            }
            editproject.CurrentResponseUser = user;
            ModelState.Remove("Body");
            ModelState.Remove("Name");
            if (ModelState.IsValid)
            {
                var iduser = User.Identity.GetUserId();
                _unitOfWork.ProjectMovementRepository.Insert(new ProjectMovement { FromUser = _unitOfWork.ApplicationUserRepository.GetById(iduser),Id=Guid.NewGuid(),ModifyTime = DateTime.Now,Project = editproject,ToUser = user}); 

                _unitOfWork.ProjectRepository.Update(editproject);
                _unitOfWork.Save();
                return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
            }
            PopulateRoleDropDown();
            return View(editproject);
        }

        private void PopulateRoleDropDown(object selectedToRole = null)
        {
            var roles = _unitOfWork.ApplicationUserRepository.GetById(User.Identity.GetUserId()).Roles.ToList();
            var temp = new List<ApplicationRole>();
            foreach (var identityUserRole in roles)
            {
                temp.AddRange(_unitOfWork.AccessRepository.Get().Where(x => x.FromRole.Name == identityUserRole.Role.Name ).ToList().Select(source => source.ToRole));
            }
            ViewBag.Roles = new SelectList(temp, "Id", "Name", selectedToRole);
        }

        // GET: /Project/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _unitOfWork.ProjectRepository.GetById(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Project project = _unitOfWork.ProjectRepository.GetById(id);
            foreach (var source in _unitOfWork.ProjectComponentRepository.Get().Where(x=>x.Project.Id==id))
            {
                _unitOfWork.ProjectComponentRepository.Delete(source);
            }
            _unitOfWork.ProjectRepository.Delete(project);
            _unitOfWork.Save();
            return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
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
