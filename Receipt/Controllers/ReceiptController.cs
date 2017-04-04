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

        public ActionResult Index()
        {
            var companies = dc.Companies
               .Select(c => new CompanyViewModel
               {
                   CompanyId = c.CompanyId,
                   Name = c.Name,
                   Address = c.Address,
                   Eik = c.Bulstat,
                   Description = c.Description
               })
                .ToList();
            return View(companies);
        }

        public ActionResult Save()
        {
            var companies = dc.Companies
               .Select(c => new CompanyViewModel
               {
                   CompanyId = c.CompanyId,
                   Name = c.Name,
                   Address = c.Address,
                   Eik = c.Bulstat,
                   Description = c.Description
               })
                .ToList();
            return View(companies);
        }


    }
}