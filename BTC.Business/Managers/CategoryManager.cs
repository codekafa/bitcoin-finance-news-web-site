using BTC.Common.Constants;
using BTC.Common.Util.Extension;
using BTC.Model.Entity;
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
            bool is_cached  = MemoryCacheManager.Instance.IsSet(ConstantProxy.CategoryCacheName);

            if (is_cached)
                return MemoryCacheManager.Instance.Get<List<Categories>>(ConstantProxy.CategoryCacheName);


            var list = catRepo.GetAll();
            MemoryCacheManager.Instance.Set(ConstantProxy.CategoryCacheName, list, 120);
            return list;
        }



    }
}
