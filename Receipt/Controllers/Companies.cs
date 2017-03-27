using Receipt.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Receipt.Controllers
{
    public class CompaniesController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ReceiptDataContext dc = new ReceiptDataContext();
            List<Company> companies = dc.Companies.ToList();
            return View(companies);
        }
                
        
    }
}