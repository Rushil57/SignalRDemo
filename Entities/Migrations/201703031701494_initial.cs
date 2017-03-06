namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DevTests", "Date", c => c.DateTime());
            AlterColumn("dbo.DevTests", "Clicks", c => c.Int());
            AlterColumn("dbo.DevTests", "Conversions", c => c.Int());
            AlterColumn("dbo.DevTests", "Impressions", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DevTests", "Impressions", c => c.Int(nullable: false));
            AlterColumn("dbo.DevTests", "Conversions", c => c.Int(nullable: false));
            AlterColumn("dbo.DevTests", "Clicks", c => c.Int(nullable: false));
            AlterColumn("dbo.DevTests", "Date", c => c.DateTime(nullable: false));
        }
    }
}
