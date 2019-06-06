using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class ContentViewManager
    {

        ContentViewRepository _contentRepo;
        public ContentViewManager()
        {
            _contentRepo = new ContentViewRepository();
        }
        public ResponseModel ValidateAddOrEditContentView(ContentViews content)
        {
            ResponseModel result = new ResponseModel();

            if (content.RowNumber <= 0)
            {
                result.Message = "Sıra numarası 0 dan büyük olmalıdır!";
                return result;
            }



            if (string.IsNullOrWhiteSpace(content.Title) || string.IsNullOrWhiteSpace(content.Description) || string.IsNullOrWhiteSpace(content.Keywords) || string.IsNullOrWhiteSpace(content.ContentBody))
            {
                result.Message = "Başlık , anahtar kelimeler, açıklama ve içerik alanları zorunludur!";
                return result;
            }

            if (content.ID > 0)
            {
                var exist = _contentRepo.GetByCustomQuery("select * from ContentViews where ID != @ID and Uri = @Uri", new { ID = content.ID, Uri = content.Uri }).FirstOrDefault();


                if (exist != null)
                {
                    result.Message = "Aynı url bilgisine sahip bir içerik zaten mevcut!";
                    return result;
                }

                var exist_row = _contentRepo.GetByCustomQuery("select * from ContentViews where ID != @ID and RowNumber = @Row", new { ID = content.ID, Row = content.RowNumber }).FirstOrDefault();


                if (exist_row != null)
                {
                    result.Message = "Aynı sıra numarası bilgisine sahip bir içerik zaten mevcut!";
                    return result;
                }

            }
            else
            {
                var exist = _contentRepo.GetByCustomQuery("select * from ContentViews where Uri = @Uri", new { Uri = content.Uri }).FirstOrDefault();

                if (exist != null)
                {
                    result.Message = "Aynı url bilgisine sahip bir içerik zaten mevcut!";
                    return result;
                }

                var exist_row = _contentRepo.GetByCustomQuery("select * from ContentViews where RowNumber = @Row", new {  Row = content.RowNumber }).FirstOrDefault();


                if (exist_row != null)
                {
                    result.Message = "Aynı sıra numarası bilgisine sahip bir içerik zaten mevcut!";
                    return result;
                }
            }


            result.IsSuccess = true;
            return result;
        }
        public List<ContentViews> GetAllContentViews()
        {
            var list = _contentRepo.GetAll();
            return list;
        }
        public List<ContentViews> GetPublishedContents()
        {
            var list = _contentRepo.GetByCustomQuery("select * from ContentViews where IsPublish = 1", null);
            return list;
        }
        public List<ContentViewListModel> GetListViewPublished()
        {
            var cRepo = new BaseDapperRepository<BTCConnection, ContentViewListModel>();
            var list = cRepo.GetByCustomQuery("select Title, Uri, RowNumber from ContentViews where IsPublish = 1 ", null);
            return list;
        }

        public List<ContentViewListModel> GetListViewPublishedItems()
        {
            return _contentRepo.GetPublishedViewList();
        }
        public ContentViews GetContentViewByUri(string uri)
        {
            var content = _contentRepo.GetByCustomQuery("select * from ContentViews where IsPublish = 1 and Uri = @Uri", new { Uri = uri }).FirstOrDefault();
            return content;
        }
        public ContentViews GetContentViewByID(int id)
        {
            var content = _contentRepo.GetByCustomQuery("select * from ContentViews where  ID = @ID", new { ID = id }).FirstOrDefault();
            return content;
        }
        public ResponseModel AddNewContent(ContentViews content)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateAddOrEditContentView(content);

            if (!result.IsSuccess)
            {
                return result;
            }

            int val = _contentRepo.Insert(content);

            content.ID = val;
            result.IsSuccess = true;
            result.Message = "İçerik başarı ile oluşturuldu!";
            result.ResultData = val;
            return result;
        }
        public ResponseModel UpdateContent(ContentViews content)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateAddOrEditContentView(content);

            if (!result.IsSuccess)
                return result;

            bool p = _contentRepo.Update(content);
            result.IsSuccess = true;
            result.Message = "İçerik başarı ile güncellendi!";
            return result;
        }

        public ResponseModel UpdateIsPublishContent(int content_id, bool state)
        {
            ResponseModel result = new ResponseModel();
            bool p = _contentRepo.ExecuteQuery("update ContentViews set IsPublish = @P where ID = @ID", new { ID = content_id, P = state });
            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
            }
            return result;
        }
        public ResponseModel UpdateCanSeeUserContent(int content_id, bool state)
        {
            ResponseModel result = new ResponseModel();
            bool p = _contentRepo.ExecuteQuery("update ContentViews set CanSeeUser = @P where ID = @ID", new { ID = content_id, P = state });
            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
            }
            return result;
        }
        public ResponseModel UpdateCanSeeMemberContent(int content_id, bool state)
        {
            ResponseModel result = new ResponseModel();
            bool p = _contentRepo.ExecuteQuery("update ContentViews set CanSeeMember = @P where ID = @ID", new { ID = content_id, P = state });
            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
            }
            return result;
        }

        public ResponseModel UpdateCanSeeTraderContent(int content_id, bool state)
        {
            ResponseModel result = new ResponseModel();
            bool p = _contentRepo.ExecuteQuery("update ContentViews set CanSeeTrader = @P where ID = @ID", new { ID = content_id, P = state });
            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
            }
            return result;
        }

        public ResponseModel UpdateCanSeeWriterContent(int content_id, bool state)
        {
            ResponseModel result = new ResponseModel();
            bool p = _contentRepo.ExecuteQuery("update ContentViews set CanSeeWriter = @P where ID = @ID", new { ID = content_id, P = state });
            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
            }
            return result;
        }

        public ResponseModel UpdateCanSeeVipContent(int content_id, bool state)
        {
            ResponseModel result = new ResponseModel();
            bool p = _contentRepo.ExecuteQuery("update ContentViews set CanSeeVip= @P where ID = @ID", new { ID = content_id, P = state });
            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
            }
            return result;
        }
    }
}
