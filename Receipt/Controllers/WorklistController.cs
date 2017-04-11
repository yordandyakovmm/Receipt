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
    public class WorklistController : Controller
    {
        public WorklistController()
        {
            ViewBag.location = "worklist";
        }

        private ReceiptDataContext dc = new ReceiptDataContext();

        public ActionResult Index()
        {
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
        public bool Edit(int? id, string name)
        {
            if (id.HasValue && id != 0)
            {
                var list = dc.WorkList.Where(w => w.WorkListId == id).SingleOrDefault();
                list.Name = name;
                dc.SaveChanges();
            }
            else
            {
                dc.WorkList.Where(l => l.IsActive).ToList().ForEach(l =>
                {
                    l.IsActive = false;
                });
                var userID = User.Identity.GetUserId();
                var _user = dc.Users.Where(u => u.Id == userID).SingleOrDefault();
                WorkList wl = new WorkList
                {
                    IsActive = true,
                    Name = name,
                    Date = DateTime.Now,
                    User = _user
                };
                dc.WorkList.Add(wl);
                dc.SaveChanges();
            }
            return true;
        }

        [HttpPost]
        public bool SetActive(int id)
        {

            dc.WorkList.Where(l => l.IsActive).ToList().ForEach(l =>
            {
                l.IsActive = false;
            });
            dc.WorkList.Where(l => l.WorkListId == id).SingleOrDefault().IsActive = true;
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