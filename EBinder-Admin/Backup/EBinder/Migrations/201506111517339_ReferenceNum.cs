namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenceNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reference", "Number", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reference", "Number");
        }
    }
}
