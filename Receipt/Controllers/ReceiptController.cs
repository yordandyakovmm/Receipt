using Microsoft.AspNet.Identity;
using Receipt.DataContext;
using Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Receipt.Controllers
{
    [Authorize]
    public class ReceiptController : Controller
    {
        private ReceiptDataContext dc = new ReceiptDataContext();

        public ReceiptController()
        {
            ViewBag.location = "receipt";
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