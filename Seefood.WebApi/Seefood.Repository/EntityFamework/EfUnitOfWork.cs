using Seefood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Repository.EntityFamework
{
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        private EmergencyDepartmentContext context = new EmergencyDepartmentContext();
        private readonly EfGenericRepository<User> _userRepo;
        private readonly EfGenericRepository<SessionAuthorize> _sessionAuthorizeRepo;
        private readonly EfGenericRepository<Product> _productRepo;
        private readonly EfGenericRepository<FavouriteProd> _favouriteProdRepo;
        
        public EfUnitOfWork()
        {
            _userRepo = new EfGenericRepository<User>(context);
            _sessionAuthorizeRepo = new EfGenericRepository<SessionAuthorize>(context);
            _productRepo = new EfGenericRepository<Product>(context);
            _favouriteProdRepo = new EfGenericRepository<FavouriteProd>(context);
        }
        public IGenericRepository<User> UserRepository => _userRepo;
        public IGenericRepository<SessionAuthorize> SessionAuthorizeRepository => _sessionAuthorizeRepo;
        public IGenericRepository<Product> ProductRepository => _productRepo;
        public IGenericRepository<FavouriteProd> FavouriteProdRepository => _favouriteProdRepo;
        public void Commit(string request_id = "")
        {
            context.SaveChanges();
        }
    }
}
