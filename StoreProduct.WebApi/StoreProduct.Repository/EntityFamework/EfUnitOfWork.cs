using StoreProduct.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProduct.Repository.EntityFamework
{
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        private EmergencyDepartmentContext context = new EmergencyDepartmentContext();
        private readonly EfGenericRepository<User> _userRepo;
        private readonly EfGenericRepository<Product> _productRepo;
        public EfUnitOfWork()
        {
            _userRepo = new EfGenericRepository<User>(context);
            _productRepo = new EfGenericRepository<Product>(context);
        }
        public IGenericRepository<User> UserRepository => _userRepo;
        public IGenericRepository<Product> ProductRepository => _productRepo;
        public void Commit(string request_id = "")
        {
            context.SaveChanges();
        }
    }
}
