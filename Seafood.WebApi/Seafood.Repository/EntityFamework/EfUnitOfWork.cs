using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Repository.EntityFamework
{
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        private EmergencyDepartmentContext context = new EmergencyDepartmentContext();
        private readonly EfGenericRepository<User> _userRepo;
        private readonly EfGenericRepository<SessionAuthorize> _sessionAuthorizeRepo;
        private readonly EfGenericRepository<Product> _productRepo;
        private readonly EfGenericRepository<FavouriteProd> _favouriteProdRepo;
        private readonly EfGenericRepository<Category> _categoryRepo;
        private readonly EfGenericRepository<ProdInfo> _prodInfoRepo;
        private readonly EfGenericRepository<ProdProcessing> _prodProcessingRepo;
        private readonly EfGenericRepository<ProdPromotion> _prodPromotionRepo;
        private readonly EfGenericRepository<Region> _regionRepo;
        private readonly EfGenericRepository<RegionDistrict> _regionDistrictRepo;
        private readonly EfGenericRepository<SeafoodPromotion> _seafoodPromotionRepo;
        private readonly EfGenericRepository<ShopSeafood> _shopSeafoodRepo;

        public EfUnitOfWork()
        {
            _userRepo = new EfGenericRepository<User>(context);
            _sessionAuthorizeRepo = new EfGenericRepository<SessionAuthorize>(context);
            _productRepo = new EfGenericRepository<Product>(context);
            _favouriteProdRepo = new EfGenericRepository<FavouriteProd>(context);
            _categoryRepo = new EfGenericRepository<Category>(context);
            _prodInfoRepo = new EfGenericRepository<ProdInfo>(context);
            _prodProcessingRepo = new EfGenericRepository<ProdProcessing>(context);
            _prodPromotionRepo = new EfGenericRepository<ProdPromotion>(context);
            _regionRepo = new EfGenericRepository<Region>(context);
            _regionDistrictRepo = new EfGenericRepository<RegionDistrict>(context);
            _seafoodPromotionRepo = new EfGenericRepository<SeafoodPromotion>(context);
            _shopSeafoodRepo = new EfGenericRepository<ShopSeafood>(context);
        }

        public IGenericRepository<User> UserRepository => _userRepo;
        public IGenericRepository<SessionAuthorize> SessionAuthorizeRepository => _sessionAuthorizeRepo;
        public IGenericRepository<Product> ProductRepository => _productRepo;
        public IGenericRepository<FavouriteProd> FavouriteProdRepository => _favouriteProdRepo;
        public IGenericRepository<Category> CategoryRepository => _categoryRepo;
        public IGenericRepository<ProdInfo> ProdInfoRepository => _prodInfoRepo;
        public IGenericRepository<ProdProcessing> ProdProcessingRepository => _prodProcessingRepo;
        public IGenericRepository<ProdPromotion> ProdPromotionRepository => _prodPromotionRepo;
        public IGenericRepository<Region> RegionRepository => _regionRepo;
        public IGenericRepository<RegionDistrict> RegionDistrictRepository => _regionDistrictRepo;
        public IGenericRepository<SeafoodPromotion> SeafoodPromotionRepository => _seafoodPromotionRepo;
        public IGenericRepository<ShopSeafood> ShopSeafoodRepository => _shopSeafoodRepo;

        public void Commit(string request_id = "")
        {
            context.SaveChanges();
        }
    }
}
