using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.DAL
{
    public class UnitOfWork: IDisposable
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private GenericRepository<Access> _accessRepository;
        private GenericRepository<Project> _projectRepository;
        private GenericRepository<ProjectMovement> _projectMovementRepository;
        private GenericRepository<ApplicationUser> _applicationUserRepository;
        private GenericRepository<ApplicationRole> _applicationRoleRepository;
        private GenericRepository<IdentityUserRole> _identityUserRoleRepository;
        private GenericRepository<Component> _componentRepository;
        private GenericRepository<ProjectComponent> _projectComponentRepository;

        public GenericRepository<Component> ComponentRepository
        {
            get { return _componentRepository ?? (_componentRepository = new GenericRepository<Component>(_context)); }
        }

        public GenericRepository<ProjectComponent> ProjectComponentRepository
        {
            get { return _projectComponentRepository ?? (_projectComponentRepository = new GenericRepository<ProjectComponent>(_context)); }
        }

        public GenericRepository<Access> AccessRepository
        {
            get { return _accessRepository ?? (_accessRepository = new GenericRepository<Access>(_context)); }
        }
        public GenericRepository<IdentityUserRole> IdentityUserRoleRepository
        {
            get { return _identityUserRoleRepository ?? (_identityUserRoleRepository = new GenericRepository<IdentityUserRole>(_context)); }
        }

        public GenericRepository<ProjectMovement> ProjectMovementRepository
        {
            get { return _projectMovementRepository ?? (_projectMovementRepository = new GenericRepository<ProjectMovement>(_context)); }
        }

        public GenericRepository<ApplicationUser> ApplicationUserRepository
        {
            get { return _applicationUserRepository ?? (_applicationUserRepository = new GenericRepository<ApplicationUser>(_context)); }
        }

        public GenericRepository<ApplicationRole> ApplicationRoleRepository
        {
            get { return _applicationRoleRepository ?? (_applicationRoleRepository = new GenericRepository<ApplicationRole>(_context)); }
        }

        public GenericRepository<Project> ProjectRepository
        {
            get { return _projectRepository ?? (_projectRepository = new GenericRepository<Project>(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    
    }
}