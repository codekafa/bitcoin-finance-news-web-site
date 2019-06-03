using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
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
    public class PostManager
    {

        UserPostRepository _postRepo;
        PostModelRepository _postModelRepo;
        CommentRepository _cRepo;
        ImageManager _imgM;
        public PostManager()
        {
            _postRepo = new UserPostRepository();
            _postModelRepo = new PostModelRepository();
            _imgM = new ImageManager();
            _cRepo = new CommentRepository();
        }
        public ResponseModel AddNewPostValidate(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(postModel.Title) || string.IsNullOrWhiteSpace(postModel.Body) || string.IsNullOrWhiteSpace(postModel.Tags) || string.IsNullOrWhiteSpace(postModel.MetaTitle) || string.IsNullOrWhiteSpace(postModel.MetaKeywords))
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return result;
            }

            if (postModel.CategoryID <= 0)
            {
                result.Message = "Kategori alanını doldurunuz!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(postModel.Uri))
            {
                result.Message = "Url alanını doldurunuz!";
                return result;
            }


            if (postModel.ID <= 0 || postModel.IsChangeMainImage)
            {
                if (postModel.MainImage == null)
                {
                    result.Message = "Kapak fotoğrafı alanı zorunludur!";
                    return result;
                }

                /* it means 5.242.880 bytes (5 mb) */
                if (postModel.MainImage.ContentLength > ((1024 * 1024) * 5))
                {
                    result.Message = "Kapak fotoğrafı 5 MB ' tan büyük olamaz!";
                    return result;
                }

                string ext = System.IO.Path.GetExtension(postModel.MainImage.FileName);

                if (!_imgM.CheckImageType(ext))
                {
                    result.Message = "Kapak fotoğrafı formatı jpg , jpeg  yada png olmalıdır!";
                    return result;
                }
            }

            var isExistItem = _postRepo.GetByCustomQuery("select * from UserPosts where Uri = @Uri and ID != @ID", new { Uri = postModel.Uri, ID = postModel.ID }).FirstOrDefault();

            if (isExistItem != null)
            {
                result.Message = "Aynı url adresine sahip bir makale mevcuttur!";
                return result;
            }

            result.IsSuccess = true;
            return result;

        }

        public void UpdateViewCount(int iD)
        {
            _postRepo.ExecuteQuery("update UserPosts set ViewCount = ViewCount + 1 where ID = @ID", new { ID = iD });
        }

        public ResponseModel AddNewPost(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();
            result = AddNewPostValidate(postModel);

            if (!result.IsSuccess)
                return result;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    UserPosts post = new UserPosts();
                    post.Body = postModel.Body;
                    post.CategoryID = postModel.CategoryID;
                    post.CreateDate = DateTime.Now;
                    post.IsActive = true;
                    post.IsPublish = postModel.IsPublish;
                    post.MetaKeywords = postModel.MetaKeywords;
                    post.MetaDescription = postModel.MetaDescription;
                    post.MetaTitle = postModel.MetaTitle;
                    post.Summary = postModel.Summary;
                    post.Tags = postModel.Tags;
                    post.Uri = postModel.Uri;
                    post.UserID = postModel.UserID;
                    post.TopPhotoUrl = postModel.TopPhotoUrl;
                    post.Title = postModel.Title;
                    if (postModel.MainImage != null)
                    {
                        _imgM.SavePostMainPage(postModel.MainImage, postModel.FileSaveMap);
                    }

                    post.ID = _postRepo.Insert(post);

                    result.IsSuccess = true;
                    result.Message = "Makale başarı ile kaydedilmiştir.";
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
        public UserPosts GetById(int post_id)
        {
            return _postRepo.GetByID(post_id);
        }
        public ResponseModel UpdatePost(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();

            result = AddNewPostValidate(postModel);

            if (!result.IsSuccess)
                return result;

            using (TransactionScope tran = new TransactionScope())
            {

                try
                {
                    UserPosts post = new UserPosts();
                    post = _postRepo.GetByID(postModel.ID);
                    if (post == null || post.ID <= 0)
                    {
                        result.Message = "Post bulunamadı!";
                        return result;
                    }

                    post.Body = postModel.Body;
                    post.CategoryID = postModel.CategoryID;
                    post.CreateDate = DateTime.Now;
                    post.IsPublish = postModel.IsPublish;
                    post.MetaKeywords = postModel.MetaKeywords;
                    post.MetaDescription = postModel.MetaDescription;
                    post.MetaTitle = postModel.MetaTitle;
                    post.Summary = postModel.Summary;
                    post.Tags = postModel.Tags;

                    if (postModel.MainImage != null)
                    {
                        _imgM.SavePostMainPage(postModel.MainImage, postModel.FileSaveMap);
                        _imgM.RemoveOldPostImages(post.TopPhotoUrl);
                    }

                    post.Uri = postModel.Uri;
                    post.TopPhotoUrl = postModel.TopPhotoUrl;
                    bool upd_val = _postRepo.Update(post);

                    result.IsSuccess = upd_val;

                    if (upd_val)
                    {
                        tran.Complete();
                        result.IsSuccess = true;
                        result.Message = "Makale başarı ile güncellemiştir!";
                        return result;
                    }
                    else
                    {
                        tran.Dispose();
                        result.Message = "Makale güncellenirken hata oluştu!";
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

        public List<PostModel> GetPostModelsByUserId(int user_id)
        {
            List<PostModel> list = new List<PostModel>();
            list = _postModelRepo.GetByCustomQuery(@"select 
                                u.FirstName + ' ' + u.LastName as [Writer],
                                c.Name as [Category],
                                us.*
                                from UserPosts us
                                inner join Categories c on c.ID = us.CategoryID
                                inner join Users u on u.ID = us.UserID
                                where u.ID = @UserID and us.IsActive = 1", new { UserID = user_id }).ToList();
            return list;

        }

        public PostModel GetPostModelByUri(string uri)
        {
            PostModel post = new PostModel();
            post = _postModelRepo.GetByCustomQuery(@"select 
                                u.ProfilePhotoUrl as [WriterPhoto],
                                u.FirstName + ' ' + u.LastName as [Writer],
                                c.Name as [Category],
                                us.*
                                from UserPosts us
                                inner join Categories c on c.ID = us.CategoryID
                                inner join Users u on u.ID = us.UserID
                                where us.Uri = @Uri", new { Uri = uri }).FirstOrDefault();

            post.PostComments = _cRepo.GetByCustomQuery("select * from  Comments where PostID = @PostID and IsPublish = 1", new { PostID = post.ID }).ToList();

            return post;
        }

        public List<PostModel> GetPostsByFilterModel(PostFilterModel filter)
        {
            filter.CategoryName = GenerateUriFormat(filter.CategoryName);
            List<PostModel> postList = new List<PostModel>();
            postList = _postModelRepo.GetByCustomQuery(@"select  TOP 20
                                u.FirstName + ' ' + u.LastName as [Writer],
                                c.Name as [Category],
                                us.*
                                from UserPosts us
                                inner join Categories c on c.ID = us.CategoryID
                                inner join Users u on u.ID = us.UserID
                                where   us.IsPublish = 1 and us.IsActive = 1 and 
								(@CategoryName is null or c.Uri = @CategoryName)
								and 
								(@SearchKey is null or us.Title like '%'+ @SearchKey +'%')
								and 
								(@TagName is null or us.Tags like '%' +@TagName+ '%')", filter).ToList();
            return postList;
        }

        public List<PostModel> GetBestViewTop3Post()
        {

            List<PostModel> postList = new List<PostModel>();
            postList = _postModelRepo.GetByCustomQuery(@"select  TOP 3
                                u.FirstName + ' ' + u.LastName as [Writer],
                                c.Name as [Category],
                                us.*
                                from UserPosts us
                                inner join Categories c on c.ID = us.CategoryID
                                inner join Users u on u.ID = us.UserID
                                where   us.IsPublish = 1 and us.IsActive = 1 Order by  us.ViewCount desc", null).ToList();
            return postList;
        }

        public void UpdatePublishFiledPost(int post_id, bool value)
        {
            _postRepo.ExecuteQuery("update UserPosts set IsPublish = @P where ID = @ID", new { p = value, ID = post_id });
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


        public ResponseModel ValidateAddComment(Comments comment)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(comment.Name))
            {
                result.Message = "Ad alanı zorunludur!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(comment.Email))
            {
                result.Message = "Email alanı zorunludur!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(comment.Description))
            {
                result.Message = "MEsaj alanı zorunludur!";
                return result;
            }
            if (comment.TypeID <= 0)
            {
                result.Message = "Tip alanı zorunludur!";
                return result;
            }
            result.IsSuccess = true;
            return result;


        }

        public ResponseModel AddComment(Comments addComment)
        {
            ResponseModel result = new ResponseModel();
            result = ValidateAddComment(addComment);
            if (!result.IsSuccess)
            {
                return result;
            }
            addComment.ID = _cRepo.Insert(addComment);
            result.IsSuccess = true;
            result.Message = "Yorumunuz başarı ile eklenmiştir.Onaylandıktan sonra yayınlanacaktır.";
            return result;
        }
    }
}
