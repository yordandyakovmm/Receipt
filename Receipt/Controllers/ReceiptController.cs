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

        public bool Save(ReceiptViewModel model)
        {

            if (model.ReceiptId == 0)
            {

                var company = dc.Companies.Where(c => c.CompanyId == model.CompanyId).SingleOrDefault();

                Receipt.DataContext.Receipt receipt = new Receipt.DataContext.Receipt()
                {
                    Date = (DateTime)model.Date,
                    Company = company,
                };

                dc.Receipts.Add(receipt);

                foreach (var a in model.Articles)
                {
                    Article artcle = new Article()
                    {
                        Name = a.ArticleName,
                        Price = a.Price
                    };
                    receipt.Articles.Add(artcle);
                }

                var userID = User.Identity.GetUserId();

                var wl = dc.WorkList.Where(w => w.User.Id == userID && w.IsActive).SingleOrDefault();

                if (wl == null)
                {
                    wl = new WorkList()
                    {
                        Date = DateTime.Now,
                        Name = "",
                        User = dc.Users.Where(u => u.Id == userID).SingleOrDefault(),
                        IsActive = true
                    };
                }

                receipt.WorkList = wl;

                dc.SaveChanges();
            }
            else
            {
                Receipt.DataContext.Receipt receipt = dc.Receipts.Where(r => r.ReceiptId == model.ReceiptId).SingleOrDefault();
                var company = dc.Companies.Where(c => c.CompanyId == model.CompanyId).SingleOrDefault();
                receipt.Company = company;
                receipt.Date = (DateTime)model.Date;
                receipt.Articles = new List<Article>();
                foreach (var a in model.Articles)
                {
                    Article artcle = new Article()
                    {
                        Name = a.ArticleName,
                        Price = a.Price
                    };
                    receipt.Articles.Add(artcle);
                }
            }

            return true;



        }


    }
}