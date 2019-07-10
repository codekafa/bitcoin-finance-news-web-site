using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Constants;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace BTC.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {

        }

        [Route("~/")]
        [Route("~/ana-sayfa")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("~/hakkimizda")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Route("~/iletisim")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("~/bir-sorun-var")]
        public ActionResult ErrorPage()
        {
            ResponseModel result = TempData["ResponseModel"] as ResponseModel;

            if (result == null)
            {
                result = new ResponseModel();
                result.IsSuccess = false;
                result.Message = "Birşeyler ters gitti. Bunun için ilgili birimleri bilgilendirdik.İşlemlerinize devam edebilirsiniz.";
            }

            return View(result);
        }

        [Route("~/yetkiniz-yok")]
        public ActionResult NotAuthority()
        {
            ResponseModel result = new ResponseModel();
            result.IsSuccess = false;
            result.Message = "Bu işlem için yetkiniz bulunmamaktadır!";
            return View(result);
        }

        [Route("~/sitemap.xml")]
        public ActionResult SiteMapXml()
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            XmlTextWriter xr = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xr.WriteStartDocument();
            xr.WriteStartElement("urlset");//urlset etiketi açıyoruz
            xr.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xr.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xr.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");


            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl);
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "monthly");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "blog");
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "daily");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "hakkimizda");
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "monthly");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "iletisim");
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "monthly");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "giris-yap");
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "monthly");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "yeni-uyelik-olustur");
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "monthly");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();


            var postList = new PostManager().GetAllPostModelsOnlyUriField();
            foreach (var post in postList)
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "blog/" + post.Uri);
                xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xr.WriteElementString("priority", "1");
                xr.WriteElementString("changefreq", "daily");
                xr.WriteEndElement();
            }

            var contentList = new ContentViewManager().GetListViewPublished().Where(x=> x.CanSeeUser);
            foreach (var content in contentList)
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", ConstantProxy.BaseSiteUrl + "icerik/" + content.Uri);
                xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xr.WriteElementString("priority", "1");
                xr.WriteElementString("changefreq", "monthly");
                xr.WriteEndElement();
            }

            xr.WriteEndDocument();
            xr.Flush();
            xr.Close();
            Response.End();
            return View();
        }

        public PartialViewResult _GetNavMenu()
        {
            MainMenuManager _menuM = new MainMenuManager();
            var list = _menuM.GetAllMenuItems();
            return PartialView(list);

        }

        [HttpPost]
        public JsonResult sendContactMessage(ContactModel msj)
        {
            ResponseModel result = new ResponseModel();
            result = msj.IsValid();
            if (result.IsSuccess)
            {
                EmailManager _emailM = new EmailManager();
                result = _emailM.SendMail("İletişim Talebi", new string[] { StaticSettings.SiteSettings.ComtactEmail }, msj.ToString());
            }
            return Json(result);

        }
    }
}