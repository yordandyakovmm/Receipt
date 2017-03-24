namespace Receipt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "BlogId", "dbo.Blogs");
            DropIndex("dbo.Posts", new[] { "BlogId" });
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Bulstat = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        ReceiptId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OrderNumner = c.Int(nullable: false),
                        UniqueNumber = c.String(),
                        Company_CompanyId = c.Int(),
                        Operator_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReceiptId)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.Operator_Id)
                .Index(t => t.Company_CompanyId)
                .Index(t => t.Operator_Id);
            
            CreateTable(
                "dbo.ProductQuantities",
                c => new
                    {
                        ProductQuantityId = c.Int(nullable: false, identity: true),
                        Countable = c.Boolean(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        Product_ProductId = c.Int(),
                        Receipt_ReceiptId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductQuantityId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .ForeignKey("dbo.Receipts", t => t.Receipt_ReceiptId)
                .Index(t => t.Product_ProductId)
                .Index(t => t.Receipt_ReceiptId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            DropTable("dbo.Blogs");
            DropTable("dbo.Posts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostId);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BlogId);
            
            DropForeignKey("dbo.Companies", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductQuantities", "Receipt_ReceiptId", "dbo.Receipts");
            DropForeignKey("dbo.Products", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductQuantities", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.Receipts", "Operator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Receipts", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.Products", new[] { "User_Id" });
            DropIndex("dbo.ProductQuantities", new[] { "Receipt_ReceiptId" });
            DropIndex("dbo.ProductQuantities", new[] { "Product_ProductId" });
            DropIndex("dbo.Receipts", new[] { "Operator_Id" });
            DropIndex("dbo.Receipts", new[] { "Company_CompanyId" });
            DropIndex("dbo.Companies", new[] { "User_Id" });
            DropTable("dbo.Products");
            DropTable("dbo.ProductQuantities");
            DropTable("dbo.Receipts");
            DropTable("dbo.Companies");
            CreateIndex("dbo.Posts", "BlogId");
            AddForeignKey("dbo.Posts", "BlogId", "dbo.Blogs", "BlogId", cascadeDelete: true);
        }
    }
}
