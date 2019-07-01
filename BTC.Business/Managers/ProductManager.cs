using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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


        public ResponseModel ValidateAddProduct(AddProductModel product)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description) || string.IsNullOrWhiteSpace(product.Uri))
            {
                result.Message = "Ürün ismi, açıklaması ve url bilgisi zorunludur!";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ValidateEditProduct(EditProductModel product)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description) || string.IsNullOrWhiteSpace(product.Uri))
            {
                result.Message = "Ürün ismi, açıklaması ve url bilgisi zorunludur!";
                return result;
            }


            result.IsSuccess = true;
            return result;
        }

        public ResponseModel DeletePhoto(int photo_id)
        {
            ResponseModel result = new ResponseModel();
            var photo = _photoRepo.GetByID(photo_id);
            _imM.RemoveProductPhoto(photo.Photo);

            _photoRepo.ExecuteQuery("delete from ProductPhotos where ID = @ID", new { ID = photo_id });

            result.IsSuccess = true;
            result.Message = "Fotoğraf başarı ile silindi!";
            return result;
        }

        public ResponseModel UpdateIsMain(int photo_id, int product_id)
        {
            ResponseModel result = new ResponseModel();
            _photoRepo.ExecuteQuery("update ProductPhotos set IsMain = 0 where ProductID = @ID", new { ID = product_id });
            _photoRepo.ExecuteQuery("update ProductPhotos set IsMain = 1 where ID = @ID", new { ID = photo_id });
            result.IsSuccess = true;
            result.Message = " Kapak fotoğrafı güncellendi!";
            return result;
        }

        public List<UserProducts> GetProductByUserID(int id)
        {
            var list = _proRepo.GetByCustomQuery("select * from UserProducts where UserID = @ID", new { ID = id });
            return list;
        }


        public EditProductModel GetEditProductModel(int product_id, int user_id)
        {
            EditProductModel result = new EditProductModel();
            var product = GetProductByID(product_id, user_id);

            if (product == null)
                return null;

            result.ID = product.ID;
            result.Description = product.Description;
            result.Name = product.Name;
            result.Price = product.Price;
            result.IsPublish = product.IsPublish;
            result.Keywords = product.Keywords;
            result.Tags = product.Tags;
            result.Description = product.Description;
            result.Uri = product.Uri;
            result.UserID = product.UserID;

            result.Photos = _photoRepo.GetByCustomQuery("select * from ProductPhotos where ProductID = @ID", new { ID = result.ID }).ToList();
            return result;

        }

        public List<UserProducts> GetProducts(ProductSearchModel search)
        {

            if (!string.IsNullOrWhiteSpace(search.SearchKey))
                search.SearchKey = "%" + search.SearchKey + "%";

            var list = _proRepo.GetByCustomQuery("select * from UserProducts where UserID = @SupplierId and (@SearchKey is null or (Name like @SearchKey or Tags like @SearchKey or Keywords like @SearchKey))", search);
            return list;
        }

        public UserProducts GetProductByID(int product_id, int user_id)
        {
            var product = _proRepo.GetByCustomQuery("select * from UserProducts where UserID = @ID and ID = @PID", new { ID = user_id, PID = product_id }).FirstOrDefault();
            return product;
        }

        public UserProducts GetProductByUri(string uri)
        {
            var product = _proRepo.GetByCustomQuery("select * from UserProducts where Uri = @Uri and IsPublish = 1", new { Uri = uri}).FirstOrDefault();
            return product;
        }
        public ResponseModel AddProduct(AddProductModel addProduct)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateAddProduct(addProduct);

            if (!result.IsSuccess)
                return result;

            using (var t = new TransactionScope())
            {
                try
                {
                    UserProducts new_p = new UserProducts();
                    new_p.UserID = addProduct.UserID;
                    new_p.CreateDate = DateTime.Now;
                    new_p.Description = addProduct.Description;
                    new_p.IsPublish = addProduct.IsPublish;
                    new_p.Keywords = addProduct.Keywords;
                    new_p.Price = addProduct.Price;
                    new_p.Tags = addProduct.Tags;
                    new_p.Name = addProduct.Name;
                    new_p.Uri = new PostManager().GenerateUriFormat(addProduct.Uri);
                    new_p.ID = _proRepo.Insert(new_p);

                    if (addProduct.MainImage != null)
                    {
                        string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                        string baseMap = Path.Combine(addProduct.BasePhotoPath, file_name);
                        _imM.SaveProductImage(addProduct.MainImage, baseMap);
                        ProductPhotos mainPhoto = new ProductPhotos();
                        mainPhoto.IsMain = true;
                        mainPhoto.Photo = file_name;
                        mainPhoto.ProductID = new_p.ID;
                        _photoRepo.Insert(mainPhoto);
                    }

                    if (addProduct.Photos != null)
                    {
                        foreach (var item in addProduct.Photos)
                        {
                            string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                            string baseMap = Path.Combine(addProduct.BasePhotoPath, file_name);
                            _imM.SavePostMainPage(item, baseMap);
                            ProductPhotos mainPhoto = new ProductPhotos();
                            mainPhoto.IsMain = false;
                            mainPhoto.Photo = file_name;
                            mainPhoto.ProductID = new_p.ID;
                            _photoRepo.Insert(mainPhoto);
                        }
                    }
                    result.Message = "Ürün başarı ile eklendi!";
                    result.IsSuccess = true;
                    t.Complete();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                    t.Dispose();
                }
            }

            return result;
        }

        public ResponseModel EditProduct(AddProductModel editProduct)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateAddProduct(editProduct);

            if (!result.IsSuccess)
                return result;

            using (var t = new TransactionScope())
            {
                try
                {




                    t.Complete();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                    t.Dispose();
                }
            }

            return result;
        }

        public ResponseModel AddPhotosToProduct(ProductPhotos addPhoto)
        {
            ResponseModel result = new ResponseModel();
            _photoRepo.Insert(addPhoto);
            result.IsSuccess = true;
            return result;

        }

    }
}
