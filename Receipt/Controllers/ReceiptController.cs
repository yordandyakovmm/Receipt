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

        public ActionResult Index(int? companyId, int? receiptId)
        {
            ReceiptViewModel model;
            if (receiptId.HasValue)
            {
                var receipt = dc.Receipts.Where(r => r.ReceiptId == receiptId).SingleOrDefault();
               
                model = new ReceiptViewModel
                {
                    ReceiptId = receipt.ReceiptId,
                    Articles = receipt.Articles.Select(a => new ArticleViewModel
                    {
                        ArticleName = a.Name,
                        Price = a.Price
                    }).ToList(),
                    companies = dc.Companies.Select(c => new CompanyViewModel
                    {
                        CompanyId = c.CompanyId,
                        Name = c.Name,
                        Address = c.Address,
                        Eik = c.Bulstat,
                        Description = c.Description,
                        Selected = c.CompanyId == receipt.Company.CompanyId
                    }).ToList(),
                    Date = receipt.Date
                };
                return View(model);
            }
            model = new ReceiptViewModel
            {
                ReceiptId = 0,
                Articles = new List<ArticleViewModel>(),
                companies = dc.Companies.Select(c => new CompanyViewModel
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Address = c.Address,
                    Eik = c.Bulstat,
                    Description = c.Description,
                    Selected = c.CompanyId == companyId
                }).ToList(),
                Date = null
            };
            return View(model);

        }

        public ActionResult Save(ReceiptViewModel receipt)
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