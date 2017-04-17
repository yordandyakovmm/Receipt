namespace Receipt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "City", c => c.String());
            AddColumn("dbo.Companies", "LeftNumber", c => c.String());
            AddColumn("dbo.Companies", "RigthNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "RigthNumber");
            DropColumn("dbo.Companies", "LeftNumber");
            DropColumn("dbo.Companies", "City");
        }
    }
}
