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
            return View(model);
        }
    }
}