using Seefood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Repository.EntityFamework
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<SessionAuthorize> SessionAuthorizeRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<FavouriteProd> FavouriteProdRepository { get; }
        void Commit(string request_id = "");
    }
}
