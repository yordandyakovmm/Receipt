﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Receipt.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Receipt.DataContext
{

    public class ReceiptDataContext : IdentityDbContext<ApplicationUser>
    {
        public ReceiptDataContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ReceiptDataContext Create()
        {
            return new ReceiptDataContext();
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductQuantity> ProductQuantitys{ get; set; }
        public DbSet<Receipt> Receipts { get; set; }

    }

    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Bulstat { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }


        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Receipt> Receipt { get; set; }
    }

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
                
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ProductQuantity> ProductQuantity { get; set; }
    }

    public class ProductQuantity
    {
        [Key]
        public int ProductQuantityId{ get; set; }

        public virtual Product Product { get; set; }

        public virtual Receipt Receipt { get; set; }

        public bool Countable { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
    }

    public class Receipt
    {
        [Key]
        public int ReceiptId { get; set; }
        public DateTime Date { get; set; }
        public int OrderNumner { get; set; }
        public string UniqueNumber { get; set; }

        public virtual Company Company { get; set; }
              
        public virtual ApplicationUser Operator { get; set; }

        public virtual ICollection<ProductQuantity> ProductQuantity { get; set; }
    }



}