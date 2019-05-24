using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using BTC.Repository.ViewRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class PostManager
    {

        UserPostRepository _postRepo;
        PostModelRepository _postModelRepo;

        public PostManager()
        {
            _postRepo = new UserPostRepository();
            _postModelRepo = new PostModelRepository();
        }
        public ResponseModel AddNewPostValidate(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(postModel.Title) || string.IsNullOrWhiteSpace(postModel.Body) || string.IsNullOrWhiteSpace(postModel.Tags) || string.IsNullOrWhiteSpace(postModel.MetaTitle) || string.IsNullOrWhiteSpace(postModel.MetaKeywords))
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return result;
            }

            if (postModel.CategoryID > 0)
            {
                result.Message = "Kategori alanını doldurunuz!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(postModel.Uri))
            {
                result.Message = "Url alanını doldurunuz!";
                return result;
            }

            result.IsSuccess = true;
            return result;

        }
        public ResponseModel AddNewPost(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();

            result = AddNewPostValidate(postModel);

            if (!result.IsSuccess)
                return result;

            try
            {
                UserPosts post = new UserPosts();
                post.Body = postModel.Body;
                post.CategoryID = postModel.CategoryID;
                post.CreateDate = DateTime.Now;
                post.IsActive = true;
                post.IsPublish = postModel.IsPublish;
                post.MetaKeywords = postModel.MetaKeywords;
                post.MetaTitle = postModel.MetaTitle;
                post.Summary = postModel.Summary;
                post.Tags = postModel.Tags;
                post.TopPhotoUrl = postModel.TopPhotoUrl;
                post.Uri = postModel.Uri;
                post.UserID = postModel.UserID;
                post.ID = _postRepo.Insert(post);
                result.IsSuccess = true;
                result.Message = "Makale başarı ile kaydedilmiştir.";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
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
                post.MetaTitle = postModel.MetaTitle;
                post.Summary = postModel.Summary;
                post.Tags = postModel.Tags;
                post.TopPhotoUrl = postModel.TopPhotoUrl;
                post.Uri = postModel.Uri;
                bool upd_val = _postRepo.Update(post);

                result.IsSuccess = upd_val;

                if (upd_val)
                {
                    result.IsSuccess = true;
                    result.Message = "Makale başarı ile güncellemiştir!";
                    return result;
                }
                else
                {
                    result.Message = "Makale güncellenirken hata oluştu!";
                    return result;
                }


            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
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
                                where u.ID = @UserID", new { UserID = user_id }).ToList();
            return list;

        }

        public PostModel GetPostModelByUri(string uri)
        {
            PostModel post = new PostModel();
            post = _postModelRepo.GetByCustomQuery(@"select 
                                u.FirstName + ' ' + u.LastName as [Writer],
                                c.Name as [Category],
                                us.*
                                from UserPosts us
                                inner join Categories c on c.ID = us.CategoryID
                                inner join Users u on u.ID = us.UserID
                                where us.Uri = @Uri", new { UserID = uri }).FirstOrDefault();
            return post;
        }
    }
}
