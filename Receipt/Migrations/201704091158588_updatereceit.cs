namespace Receipt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatereceit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipts", "OperatorS", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receipts", "OperatorS");
        }
    }
}
