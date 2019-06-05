using BTC.Common.Constants;
using BTC.Common.Util.Extension;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class CategoryManager
    {

        CategoryRepository catRepo;
        public CategoryManager()
        {
            catRepo = new CategoryRepository();
        }
        public List<Categories> GetCategories()
        {
            bool is_cached = MemoryCacheManager.Instance.IsSet(ConstantProxy.CategoryCacheName);

            if (is_cached)
                return MemoryCacheManager.Instance.Get<List<Categories>>(ConstantProxy.CategoryCacheName);


            var list = catRepo.GetAll();
            MemoryCacheManager.Instance.Set(ConstantProxy.CategoryCacheName, list, 120);
            return list;
        }
        public ResponseModel ValidateAddOrEditCategory(Categories category)
        {
            ResponseModel result = new ResponseModel();
            Categories exist = new Categories();

            if (string.IsNullOrWhiteSpace(category.Name) || string.IsNullOrWhiteSpace(category.Uri))
            {
                result.Message = "Kategori adı ve kategori url bilgisi boş olamaz!";
                return result;
            }


            if (category.ID > 0)
            {
                exist = catRepo.GetByCustomQuery("select * from Categories where Uri = @Uri and ID != @ID", new { Uri = category.Uri, ID = category.ID }).FirstOrDefault();
            }
            else
            {
                exist = catRepo.GetByCustomQuery("select * from Categories where Uri = @Uri ", new { Uri = category.Uri }).FirstOrDefault();
            }


            if (exist != null)
            {
                result.Message = "Aynı url e sahip bir kategori zaten mevcut!";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }
        public ResponseModel AddCategory(Categories category)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateAddOrEditCategory(category);

            if (!result.IsSuccess)
                return result;
            category.ID = catRepo.Insert(category);
            result.IsSuccess = true;
            result.Message = "Kategori başarı ile eklenmiştir!";
            result.ResultData = category;
            return result;

        }
        public ResponseModel UpdateCategory(Categories category)
        {
            ResponseModel result = new ResponseModel();
            result = ValidateAddOrEditCategory(category);

            if (!result.IsSuccess)
                return result;

            var exist = catRepo.GetByID(category.ID);
            category.CreateDate = exist.CreateDate;
            catRepo.Update(category);
            result.IsSuccess = true;
            result.Message = "Kategori başarı ile güncellenmiştir!";
            return result;

        }

        public ResponseModel UpdateCategoryState(int id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = catRepo.ExecuteQuery("update Categories set IsActive = @State  where ID =@ID", new { ID = id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Kategori durumu başarı ile güncellenmiştir!";
            }

            return result;
        }
    }
}
