namespace Receipt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class atricle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Receipt_ReceiptId = c.Int(),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Receipts", t => t.Receipt_ReceiptId)
                .Index(t => t.Receipt_ReceiptId);
            
            CreateTable(
                "dbo.WorkLists",
                c => new
                    {
                        WorkListId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WorkListId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Receipts", "WorkList_WorkListId", c => c.Int());
            CreateIndex("dbo.Receipts", "WorkList_WorkListId");
            AddForeignKey("dbo.Receipts", "WorkList_WorkListId", "dbo.WorkLists", "WorkListId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkLists", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Receipts", "WorkList_WorkListId", "dbo.WorkLists");
            DropForeignKey("dbo.Articles", "Receipt_ReceiptId", "dbo.Receipts");
            DropIndex("dbo.WorkLists", new[] { "User_Id" });
            DropIndex("dbo.Articles", new[] { "Receipt_ReceiptId" });
            DropIndex("dbo.Receipts", new[] { "WorkList_WorkListId" });
            DropColumn("dbo.Receipts", "WorkList_WorkListId");
            DropTable("dbo.WorkLists");
            DropTable("dbo.Articles");
        }
    }
}
