using BTC.Base;
using BTC.Business.Managers;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class BlogController : BaseController
    {
        PostManager _postM;
        public BlogController()
        {
            _postM = new PostManager();
        }
        // GET: Blog
        [Route("~/blog")]
        [Route("~/blog/kategori/{category_name}")]
        [Route("~/blog/etiket/{tag}")]
        [Route("~/blog/search/{search_key}")]
        public ActionResult Blog(string category_name, string search_key,string tag)
        {
            return View();
        }

        [Route("~/blog-detay/{category}/{post_uri}")]
        public ActionResult BlogDetail(string category, string post_uri)
        {
            var result = _postM.GetPostModelByUri(post_uri);
            return View(result);
        }

        [Route("~/makalelerim")]
        public ActionResult GetMyPosts()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getMyPosts()
        {
            var list = _postM.GetPostModelsByUserId(CurrentUser.CurrentUser.ID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [Route("yeni-makale-ekle")]
        public ActionResult AddNewPost()
        {
            return View();
        }

        [HttpPost]
        public JsonResult addOrEditNewPost(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();
            if (!ModelState.IsValid)
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return Json(result);
            }

            if (postModel.ID > 0)
            {
                result = _postM.AddNewPost(postModel);
            }
            else
            {
                result = _postM.UpdatePost(postModel);
            }
            return Json(result);
        }

    }
}