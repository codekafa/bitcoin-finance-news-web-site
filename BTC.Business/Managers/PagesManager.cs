using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using BTC.Repository.Connection;
using BTC.Repository.ViewRepository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Configuration;

namespace BTC.Business.Managers
{
    public class PagesManager
    {

        PagesRepository _pageRepo;
        PageModelRepository _pageModelRepo;

        public PagesManager()
        {
            _pageRepo = new PagesRepository();
            _pageModelRepo = new PageModelRepository();
        }
        public ResponseModel AddNewPageValidate(PageModel pageModel)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(pageModel.Title) || string.IsNullOrWhiteSpace(pageModel.Body) || string.IsNullOrWhiteSpace(pageModel.Tags) || string.IsNullOrWhiteSpace(pageModel.MetaTitle) || string.IsNullOrWhiteSpace(pageModel.MetaKeywords))
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(pageModel.Uri))
            {
                result.Message = "Url alanını doldurunuz!";
                return result;
            }

            var isExistItem = _pageRepo.GetByCustomQuery("select * from Pages where Uri = @Uri and ID != @ID", new { Uri = pageModel.Uri, ID = pageModel.ID }).FirstOrDefault();

            if (isExistItem != null)
            {
                result.Message = "Aynı url adresine sahip bir sayfa mevcuttur!";
                return result;
            }

            result.IsSuccess = true;
            return result;

        }

        public ResponseModel UpdatePageIsPublishField(int page_id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = _pageRepo.ExecuteQuery("update Pages set IsPublish = @State where ID = @ID", new { ID = page_id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarı ile gerçekleşti!";
            }

            return result;
        }

        public ResponseModel UpdatePageIsActiveField(int page_id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = _pageRepo.ExecuteQuery("update Pages set IsActive = @State where ID = @ID", new { ID = page_id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarı ile gerçekleşti!";
            }

            return result;
        }


        public void UpdateViewCount(int iD)
        {
            _pageRepo.ExecuteQuery("update Pages set ViewCount = ViewCount + 1 where ID = @ID", new { ID = iD });
        }

        public ResponseModel AddNewPage(PageModel pageModel)
        {
            ResponseModel result = new ResponseModel();
            result = AddNewPageValidate(pageModel);

            if (!result.IsSuccess)
                return result;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    Pages post = new Pages();
                    post.Body = pageModel.Body;
                    post.CreateDate = DateTime.Now;
                    post.IsActive = true;
                    post.IsPublish = pageModel.IsPublish;
                    post.MetaKeywords = pageModel.MetaKeywords;
                    post.MetaDescription = pageModel.MetaDescription;
                    post.MetaTitle = pageModel.MetaTitle;
                    post.Tags = pageModel.Tags;
                    post.Uri = pageModel.Uri;
                    post.Title = pageModel.Title;
                    post.ID = _pageRepo.Insert(post);

                    result.IsSuccess = true;
                    result.Message = "Sayfa başarı ile kaydedilmiştir.";
                    tran.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    result.Message = ex.Message;
                    return result;
                }
            }
        }
        public Pages GetById(int post_id)
        {
            return _pageRepo.GetByID(post_id);
        }
        public ResponseModel UpdatePage(PageModel pageModel)
        {
            ResponseModel result = new ResponseModel();

            result = AddNewPageValidate(pageModel);

            if (!result.IsSuccess)
                return result;

            using (TransactionScope tran = new TransactionScope())
            {

                try
                {
                    Pages post = new Pages();
                    post = _pageRepo.GetByID(pageModel.ID);
                    if (post == null || post.ID <= 0)
                    {
                        result.Message = "Sayfa bulunamadı!";
                        return result;
                    }

                    post.Body = pageModel.Body;
                    post.CreateDate = DateTime.Now;
                    post.IsPublish = pageModel.IsPublish;
                    post.MetaKeywords = pageModel.MetaKeywords;
                    post.MetaDescription = pageModel.MetaDescription;
                    post.MetaTitle = pageModel.MetaTitle;
                    post.Tags = pageModel.Tags;
                    post.Uri = pageModel.Uri;
                    bool upd_val = _pageRepo.Update(post);

                    result.IsSuccess = upd_val;

                    if (upd_val)
                    {
                        tran.Complete();
                        result.IsSuccess = true;
                        result.Message = "Sayfa başarı ile güncellemiştir!";
                        return result;
                    }
                    else
                    {
                        tran.Dispose();
                        result.Message = "Sayfa güncellenirken hata oluştu!";
                        return result;
                    }


                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    result.Message = ex.Message;
                    return result;
                }

            }
        }

        public PageModel GetPageModelByUri(string uri)
        {
            PageModel page = new PageModel();
            page = _pageModelRepo.GetByCustomQuery(@"select 
                                us.*
                                from Pages us
                                where us.Uri = @Uri", new { Uri = uri }).FirstOrDefault();
            return page;
        }


        public List<PageModel> GetAllPages()
        {
            return _pageModelRepo.GetByCustomQuery(@"select  TOP 20
                                us.*
                                from Pages us ", null).ToList();
        }

        public List<PageModel> GetAllpageModelsOnlyUriField(bool publish = true)
        {
            return _pageModelRepo.GetByCustomQuery(@"select 
                                us.Uri
                                from Pages us
                                where IsPublish = @IsPublish ", new { IsPublish = publish }).ToList();
        }

        public void UpdatePublishFiledPage(int page_id, bool value)
        {
            _pageRepo.ExecuteQuery("update Pages set IsPublish = @P where ID = @ID", new { p = value, ID = page_id });
        }
        public string GenerateUriFormat(string uri)
        {

            if (string.IsNullOrWhiteSpace(uri))
                return null;

            string url = String.Join("", uri.Normalize(NormalizationForm.FormD)
     .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
            url = Regex.Replace(url, @"^\W+|\W+$", "");
            //url = Regex.Replace(url, @"'\"", "");
            url = Regex.Replace(url, @"_", " - ");
            url = Regex.Replace(url, @"\W+", "-");
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).ToLookup(x => x.EnglishName);
            var en_culture = cultures["English"].FirstOrDefault();
            url = url.ToLower(en_culture);
            return url;
        }
    }
}
