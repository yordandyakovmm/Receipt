﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Linq;

namespace Receipt.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }


    public class CompanyViewModel
    {

        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Невалидна дължина", MinimumLength = 4)]
        [Display(Name = "Град")]
        public string City { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Невалидна дължина", MinimumLength = 6)]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "ЕИК")]
        public string Eik { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public bool hasReceipt { get; set; }

        public bool Selected { get; set; }

        public string LeftNumber { get; set; }

        public string RigthNumber { get; set; }

    }


    public class ReceiptViewModel
    {

        public int ReceiptId { get; set; }
               
        public int CompanyId { get; set; }

        public List<ArticleViewModel> Articles { get; set; }
        
        public DateTime? Date { get; set; }

        public decimal TotalPrice {
            get {
                return Articles.ToList().Sum(a => a.Price);
            }
        }

        public List<CompanyViewModel> Companies { get; set; }

        public CompanyViewModel Company { get; set; }

        public string Operator { get; set; }

        public string DateF { get; set; }

        public string Number { get; set; }

        public string BugNumber { get; set; }

    }


    public class ArticleViewModel
    {
        public string ArticleName { get; set; }

        public decimal Price { get; set; }
    }

    public class WorkListViewModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public List<ReceiptViewModel> receipts { get; set; } 
        
        public bool isActive { get; set; }

        public DateTime  dateCreated { get; set; }

        public ApplicationUser user { get; set; }

        public string link { get; set; }

    }

    public class UserViewModel
    {
       
        public string userID { get; set; }

        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Display(Name = "Фамиля")]
        public string LastName { get; set; }

        

    }

}