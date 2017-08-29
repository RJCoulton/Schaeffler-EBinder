namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Region", "Name", c => c.String(nullable: false, maxLength: 18));
            AlterColumn("dbo.Plant", "Name", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Department", "Name", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Cell", "Name", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Cell", "Workcenter", c => c.String(maxLength: 9));
            AlterColumn("dbo.UserType", "Name", c => c.String(maxLength: 18));
            AlterColumn("dbo.Reference", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Reference", "URL", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reference", "URL", c => c.String());
            AlterColumn("dbo.Reference", "Title", c => c.String());
            AlterColumn("dbo.UserType", "Name", c => c.String());
            AlterColumn("dbo.Cell", "Workcenter", c => c.Int(nullable: false));
            AlterColumn("dbo.Cell", "Name", c => c.String());
            AlterColumn("dbo.Department", "Name", c => c.String());
            AlterColumn("dbo.Plant", "Name", c => c.String());
            AlterColumn("dbo.Region", "Name", c => c.String());
        }
    }
}
