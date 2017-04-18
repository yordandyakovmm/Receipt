using Receipt.DataContext;
using Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Receipt.Controllers
{

    [Authorize]
    public class PdfController : Controller
    {
        private ReceiptDataContext dc = new ReceiptDataContext();

        public PdfController()
        {
            ViewBag.location = "pdf";
        }

        public ActionResult Index()
        {
            var mode = dc.Pdfs.ToList();
            return View(mode);
        }

        public ActionResult Details(int id)
        {
            var model = dc.Pdfs.Where(p => p.PdfId == id).SingleOrDefault();
            ViewBag.link = GetBaseUrl() + "/pdf/download/?file=" + model.Name;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Download(string file)
        {
            file += ".pdf";
            string filePath = Server.MapPath("~/Content/pdf/") + file;
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=foo.pdf");
            Response.TransmitFile(filePath);
            Response.End();
            return null;
        }

        private string GetBaseUrl()
        {
            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
        }

    }
}