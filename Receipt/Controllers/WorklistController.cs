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
    public class WorklistController : Controller
    {
        public WorklistController()
        {
            ViewBag.location = "worklist";
        }

        private ReceiptDataContext dc = new ReceiptDataContext();

        public ActionResult Index()
        {
            ReceiptDataContext dc = new ReceiptDataContext();
            List<WorkList> list = dc.WorkList.ToList();
            return View(list);
        }


        [HttpPost]
        public ActionResult DeleteReceipt(int id)
        {
            var receipt = dc.Receipts.Where(r => r.ReceiptId == id).SingleOrDefault();
            dc.Receipts.Remove(receipt);
            dc.SaveChanges();
            return null;
        }


        [HttpPost]
        public bool Edit(int id, string name)
        {
            var list = dc.WorkList.Where(w => w.WorkListId == id).SingleOrDefault();
            list.Name = name;
            dc.SaveChanges();
            return true;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var wl = dc.WorkList.Where(w => w.WorkListId == id).SingleOrDefault();
            var model = new WorkListViewModel
            {
                name = wl.Name,
                user = wl.User,
                dateCreated = wl.Date,
                isActive = wl.IsActive,
                receipts = wl.Receipts.Select(receipt => new ReceiptViewModel
                {
                    CompanyId = receipt.Company.CompanyId,
                    ReceiptId = receipt.ReceiptId,
                    Articles = receipt.Articles.Select(a => new ArticleViewModel
                    {
                        ArticleName = a.Name,
                        Price = a.Price
                    }).ToList(),
                    Company = new CompanyViewModel
                    {
                        CompanyId = receipt.Company.CompanyId,
                        Name = receipt.Company.Name,
                        Address = receipt.Company.Address,
                        Eik = receipt.Company.Bulstat,
                        Description = receipt.Company.Description
                    },
                    Operator = receipt.OperatorS,
                    Date = receipt.Date
                }).ToList()
            };
            
            return View(model);
        }
                
    }
}