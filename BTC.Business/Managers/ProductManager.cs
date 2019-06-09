using BTC.Model.Entity;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class ProductManager
    {
        UserProductRepository _proRepo;
        ProductPhotoRepository _photoRepo;
        ImageManager _imM;
        public ProductManager()
        {
            _proRepo = new UserProductRepository();
            _photoRepo = new ProductPhotoRepository();
            _imM = new ImageManager();
        }

        public List<UserProducts> GetProductByUserID(int id)
        {
            var list = _proRepo.GetByCustomQuery("select * from UserProducts where UserID = @ID", new { ID = id });
            return list;
        }

        public UserProducts GetProductByID(int product_id,int user_id)
        {
            var product = _proRepo.GetByCustomQuery("select * from UserProducts where UserID = @ID and ID = @PID", new { ID = user_id , PID = product_id });
            return product;
        }
    }
}
