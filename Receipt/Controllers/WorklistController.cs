using Receipt.DataContext;
using Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

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
            List<WorkList> list = dc.WorkLists.ToList();
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
                var list = dc.WorkLists.Where(w => w.WorkListId == id).SingleOrDefault();
                list.Name = name;
                dc.SaveChanges();
            }
            else
            {
                dc.WorkLists.Where(l => l.IsActive).ToList().ForEach(l =>
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
                dc.WorkLists.Add(wl);
                dc.SaveChanges();
            }
            return true;
        }

        [HttpPost]
        public bool SetActive(int id)
        {

            dc.WorkLists.Where(l => l.IsActive).ToList().ForEach(l =>
            {
                l.IsActive = false;
            });
            dc.WorkLists.Where(l => l.WorkListId == id).SingleOrDefault().IsActive = true;
            dc.SaveChanges();

            return true;
        }





        [HttpGet]
        public ActionResult Details(int id)
        {
            var wl = dc.WorkLists.Where(w => w.WorkListId == id).SingleOrDefault();
            var model = new WorkListViewModel
            {
                id = wl.WorkListId,
                name = wl.Name,
                user = wl.User,
                dateCreated = wl.Date,
                isActive = wl.IsActive,
                receipts = wl.Receipts.Select(receipt => new ReceiptViewModel
                {
                    CompanyId = receipt.Company.CompanyId,
                    ReceiptId = receipt.ReceiptId,
                    BugNumber = receipt.UniqueNumber,
                    ArticleRow = receipt.Articles.Count().ToString() + " " + (receipt.Articles.Count() == 1 ? "артикул" : "артикула"),
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
                        Description = receipt.Company.Description,
                        LeftNumber = receipt.Company.LeftNumber,
                        RigthNumber = receipt.Company.RigthNumber
                    },
                    Operator = receipt.OperatorS,
                    Date = receipt.Date,
                    DateF = receipt.Date.ToString("dd-MM-yyyy hh:mm:ss")
                }).ToList()
            };


            return View(model);
        }

        [HttpGet]
        public ActionResult deleteReceipt(int id)
        {
            var rec = dc.Receipts.Where(w => w.ReceiptId == id).SingleOrDefault();
            rec.Articles.ToList().ForEach(a =>
            {
                dc.Articles.Remove(a);
            });
            var workListId = rec.WorkList.WorkListId;
            rec.WorkList.Receipts.Remove(rec);

            dc.Receipts.Remove(rec);
            dc.SaveChanges();

            return RedirectToAction("Details", new { id = workListId });

        }

        [HttpGet]
        public ActionResult generatePdf(int id)
        {
            var wl = dc.WorkLists.Where(w => w.WorkListId == id).SingleOrDefault();
            Random ran = new Random();
            wl.Receipts.ToList().ForEach(res => {
                var oldR = dc.Receipts.Where(tr => tr.Company.CompanyId == res.Company.CompanyId 
                && tr.Date.Year < res.Date.Year
                && tr.Date.Month < res.Date.Month
                && tr.Date.Day < res.Date.Day
                && tr.Date.Hour < res.Date.Hour
                && tr.Date.Minute < res.Date.Minute).ToList();
                var newR = dc.Receipts.Where(tr => tr.Company.CompanyId == res.Company.CompanyId 
                && tr.Date.Year > res.Date.Year
                && tr.Date.Month > res.Date.Month
                && tr.Date.Day > res.Date.Day
                && tr.Date.Hour > res.Date.Hour
                && tr.Date.Minute > res.Date.Minute).ToList();

                if (oldR.Count() == 0 && newR.Count == 0)
                {
                    res.OrderNumner = ran.Next(5600, 8900);
                    dc.SaveChanges();
                }
                else if (oldR.Count() > 0 && newR.Count == 0)
                {
                    res.OrderNumner = oldR.Max(rr => rr.OrderNumner) + ran.Next(12, 23);
                    dc.SaveChanges();
                }
                else if (oldR.Count() == 0 && newR.Count > 0)
                {
                    res.OrderNumner = newR.Min(rr => rr.OrderNumner) - ran.Next(12, 23);
                    dc.SaveChanges();
                }
                else if (oldR.Count() > 0 && newR.Count > 0)
                {
                    res.OrderNumner = ran.Next(oldR.Max(rr => rr.OrderNumner), newR.Min(rr => rr.OrderNumner));
                    dc.SaveChanges();
                }
            });

            var model = new WorkListViewModel
            {
                id = wl.WorkListId,
                name = wl.Name.Replace(" ",""),
                user = wl.User,
                dateCreated = wl.Date,
                isActive = wl.IsActive,
                link = GetBaseUrl() + "/Content/img/bg.png",
                receipts = wl.Receipts.Select(receipt => new ReceiptViewModel
                {
                    CompanyId = receipt.Company.CompanyId,
                    Number = receipt.OrderNumner.ToString("0000000"),
                    BugNumber = receipt.UniqueNumber,
                    ReceiptId = receipt.ReceiptId,
                    ArticleRow = receipt.Articles.Count().ToString() + " " + (receipt.Articles.Count() == 1 ? "артикул" : "артикула"),
                    Articles = receipt.Articles.Select(a => new ArticleViewModel
                    {
                        ArticleName = a.Name,
                        Price = a.Price
                    }).ToList(),
                    Company = new CompanyViewModel
                    {
                        CompanyId = receipt.Company.CompanyId,
                        Name = receipt.Company.Name,
                        City = receipt.Company.City,
                        Address = receipt.Company.Address,
                        Eik = receipt.Company.Bulstat,
                        Description = receipt.Company.Description,
                        LeftNumber = receipt.Company.LeftNumber,
                        RigthNumber = receipt.Company.RigthNumber
                    },
                    Operator = receipt.OperatorS,
                    Date = receipt.Date,
                    DateF = receipt.Date.ToString("dd-MM-yyyy hh:mm:ss")
                }).ToList()
            };

            Random r = new Random();
            string securityCode = r.Next(1, 1000000).ToString("000000");

            var userID = User.Identity.GetUserId();
            var _user = dc.Users.Where(u => u.Id == userID).SingleOrDefault();
            var name = string.Format("{0}-{1}-{2}", wl.Name.Replace(" ", "") != "" ? wl.Name : "XXXXXX", DateTime.Now.ToString("dd.MM.yyyy"), securityCode);
            var url = string.Format("{0}/Content/pdf/{1}.pdf", GetBaseUrl(), name);

            //return PartialView("/views/pdf/template.cshtml", model);
            var path = Path.Combine(Server.MapPath("~/Content/pdf/"), name + ".pdf");
            Hellper.ReceiptPdfGenerator.GeneratePdf(model, path);
            var pdf = new Pdf
            {
                Date = DateTime.Now,
                Name = name,
                securityCode = securityCode,
                User = _user,
                url = url
            };
            dc.Pdfs.Add(pdf);
            dc.SaveChanges();
            
            //System.IO.File.WriteAllBytes(path, data);

            return RedirectToAction("Details", "pdf", new { id = pdf.PdfId});

        }

        private string GetBaseUrl()
        {
            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
        }

    }
}