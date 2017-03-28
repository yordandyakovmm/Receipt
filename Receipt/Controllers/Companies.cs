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
        private ReceiptDataContext dc = new ReceiptDataContext();

        public ActionResult Index()
        {
            ReceiptDataContext dc = new ReceiptDataContext();
            List<Company> companies = dc.Companies.ToList();
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
                    Address = company.Address,
                    Eik = company.Bulstat
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
        public bool Edit(CompanyViewModel model)
        {
            
            if (model.CompanyId == 0)
            {
                var company = new Company
                {
                    CompanyId = model.CompanyId,
                    Name = model.Name,
                    Address = model.Address,
                    Bulstat = model.Eik
                };
                dc.Companies.Add(company);
                dc.SaveChanges();
            }
            else
            {
                var company = dc.Companies.Where(c => c.CompanyId == model.CompanyId).SingleOrDefault();
                company.Address = model.Address;
                company.Name = model.Name;
                company.Bulstat = model.Eik;
                dc.SaveChanges();
            }

            return true;
        }


    }
}