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

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

    }

    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Bulstat { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }


    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }


}