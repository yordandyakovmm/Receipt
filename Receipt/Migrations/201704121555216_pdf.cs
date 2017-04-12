namespace Receipt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pdf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pdfs",
                c => new
                    {
                        PdfId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        securityCode = c.String(),
                        url = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PdfId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pdfs", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Pdfs", new[] { "User_Id" });
            DropTable("dbo.Pdfs");
        }
    }
}
