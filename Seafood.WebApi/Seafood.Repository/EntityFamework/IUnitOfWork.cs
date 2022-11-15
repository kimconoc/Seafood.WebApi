using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Repository.EntityFamework
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<SessionAuthorize> SessionAuthorizeRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<FavouriteProd> FavouriteProdRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<ProdInfo> ProdInfoRepository { get; }
        IGenericRepository<ProdProcessing> ProdProcessingRepository { get; }
        IGenericRepository<ProdPromotion> ProdPromotionRepository { get; }
        IGenericRepository<Region> RegionRepository { get; }
        IGenericRepository<SeafoodPromotion> SeafoodPromotionRepository { get; }
        IGenericRepository<ShopSeafood> ShopSeafoodRepository { get; }
        IGenericRepository<Image> ImageRepository { get; }
        IGenericRepository<CheckCodeFirebase> CheckCodeFirebaseRepository { get; }

        void Commit(string request_id = "");
    }
}
