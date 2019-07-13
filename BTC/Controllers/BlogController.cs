using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Setting;
using System;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class BlogController : BaseController
    {
        PostManager _postM;
        CategoryManager _catRepo;

        public BlogController()
        {
            _postM = new PostManager();
            _catRepo = new CategoryManager();
        }
        // GET: Blog
        [Route("~/blog")]
        [Route("~/blog/kategori/{category_name}")]
        [Route("~/blog/etiket/{tag}")]
        [Route("~/blog/search/{search_key}")]
        [Route("~/yayinlar/yazar/{writer}")]
        public ActionResult Blog(string category_name, string search_key, string tag, string writer)
        {
            PostFilterModel filter = new PostFilterModel();
            filter.CategoryName = category_name;
            filter.SearchKey = search_key;
            filter.TagName = tag;
            filter.Writer = writer;
            ViewBag.Filter = filter;
            return View();
        }

        [Route("~/blog-detay/{post_uri}")]
        [Route("~/blog-detay/{category}/{post_uri}")]
        public ActionResult BlogDetail(string category, string post_uri)
        {
            var result = _postM.GetPostModelByUri(post_uri);
            _postM.UpdateViewCount(result.ID);
            return View(result);
        }

        public PartialViewResult _GetPartialViewPosts(PostFilterModel filter)
        {
            var postList = _postM.GetPostsByFilterModel(filter);
            ViewBag.Filter = filter;
            return PartialView(postList);
        }

        public PartialViewResult _GetPartialViewBestViewPosts()
        {
            var postList = _postM.GetBestViewTop3Post();
            return PartialView(postList);
        }

        public PartialViewResult _GetCategories()
        {
            var cateList = _catRepo.GetCategories().Where(x => x.IsActive == true).ToList();
            return PartialView(cateList);
        }


        [HttpPost]
        public JsonResult addComment(Comments addComment)
        {
            ResponseModel result = new ResponseModel();
            addComment.IsPublish = false;
            addComment.CreateDate = DateTime.Now;
            if (SessionVariables.User != null)
            {
                addComment.UserID = SessionVariables.User.CurrentUser.ID;
            }
            result = _postM.AddComment(addComment);
            return Json(result);
        }
    }
}