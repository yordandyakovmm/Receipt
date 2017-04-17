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
    public class CompaniesController : Controller
    {
        public CompaniesController()
        {
            ViewBag.location = "companies";
        }

        private ReceiptDataContext dc = new ReceiptDataContext();

        public ActionResult Index()
        {
            ReceiptDataContext dc = new ReceiptDataContext();
            var companies = dc.Companies
               .Select(c => new CompanyViewModel
               {
                   CompanyId = c.CompanyId,
                   Name = c.Name,
                   City = c.City,
                   Address = c.Address,
                   Eik = c.Bulstat,
                   Description = c.Description,
                   hasReceipt = c.Receipts.Count() > 0
               })
                .ToList();
            return View(companies);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            CompanyViewModel vm;
            if (id.HasValue)
            {
                ViewBag.Button = "Запиши";
                ViewBag.Title = "Редактиране";
                var company = dc.Companies.Where(c => c.CompanyId == id).SingleOrDefault();
                vm = new CompanyViewModel
                {
                    CompanyId = company.CompanyId,
                    Name = company.Name,
                    City = company.City,
                    Address = company.Address,
                    Eik = company.Bulstat,
                    Description = company.Description,
                    hasReceipt = company.Receipts.Count() > 0
                };
            }
            else
            {
                ViewBag.Button = "Добави";
                ViewBag.Title = "Добавяне";
                vm = new CompanyViewModel();
            }

            return PartialView(vm);
        }


        [HttpPost]
        public ActionResult Edit(CompanyViewModel model)
        {

            if (model.CompanyId == 0)
            {
                var random = new Random();
                var numver = random.Next(100000, 999999);
                var company = new Company
                {
                    
                    CompanyId = model.CompanyId,
                    Name = model.Name,
                    City = model.City,
                    Address = model.Address,
                    Bulstat = model.Eik,
                    LeftNumber = $"DT{numver}",
                    RigthNumber = $"02{numver}",
                    Description = model.Description != null ? model.Description : ""
                };
                dc.Companies.Add(company);
                dc.SaveChanges();
            }
            else
            {
                var company = dc.Companies.Where(c => c.CompanyId == model.CompanyId).SingleOrDefault();
                company.Address = model.Address;
                company.City = model.City;
                company.Name = model.Name;
                company.Bulstat = model.Eik;
                company.Description = model.Description != null ? model.Description : "";
                dc.SaveChanges();
            }

            return RedirectToAction("list");
        }

        [HttpGet]
        public ActionResult List(string search)
        {
            var companies = dc.Companies
               .Select(c => new CompanyViewModel
               {
                   CompanyId = c.CompanyId,
                   Name = c.Name,
                   Address = c.Address,
                   Eik = c.Bulstat,
                   Description = c.Description,
                   hasReceipt = c.Receipts.Count() > 0
               })
                .ToList();

            return PartialView(companies);
        }

        [HttpGet]
        public bool delete(int id)
        {
            var company = dc.Companies.Where(c => c.CompanyId == id).SingleOrDefault();
            dc.Companies.Remove(company);
            dc.SaveChanges();

            return true;
        }
    }
}