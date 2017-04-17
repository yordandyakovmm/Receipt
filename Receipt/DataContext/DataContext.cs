using System;
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
        public DbSet<WorkList> WorkLists { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Pdf> Pdfs { get; set; }

    }

    public class Company
    {
        public Company()
        {
            this.Receipts = new List<Receipt>();
        }

        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Bulstat { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Deleted { get; set; }
        public string LeftNumber { get; set; }
        public string RigthNumber { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
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

    public class Article
    {
        [Key]
        public int ArticleId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

         public virtual Receipt Receipt { get; set; }
    }

    public class WorkList
    {
        [Key]
        public int WorkListId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
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
        public Receipt()
        {
            this.Articles = new List<Article>();
        }
        [Key]
        public int ReceiptId { get; set; }
        public DateTime Date { get; set; }
        public int OrderNumner { get; set; }
        public string UniqueNumber { get; set; }
        public string OperatorS { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual Company Company { get; set; }

        public virtual WorkList WorkList { get; set; }

        public virtual ApplicationUser Operator { get; set; }

        public virtual ICollection<ProductQuantity> ProductQuantity { get; set; }

    }

    public class Pdf
    {
        [Key]
        public int PdfId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string securityCode { get; set; }
        public string url { get; set; }

        public virtual ApplicationUser User { get; set; }

        
    }

}