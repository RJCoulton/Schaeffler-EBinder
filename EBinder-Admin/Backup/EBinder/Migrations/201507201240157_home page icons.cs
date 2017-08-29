namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class homepageicons : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plant", "ReferenceType_ReferenceTypeID", c => c.Int());
            AddColumn("dbo.Plant", "ReferenceType_ReferenceTypeID1", c => c.Int());
            AddColumn("dbo.ReferenceType", "Plant_PlantID", c => c.Int());
            AddColumn("dbo.ReferenceType", "Plant_PlantID1", c => c.Int());
            AddForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID", "dbo.ReferenceType", "ReferenceTypeID");
            AddForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID1", "dbo.ReferenceType", "ReferenceTypeID");
            AddForeignKey("dbo.ReferenceType", "Plant_PlantID", "dbo.Plant", "PlantID");
            AddForeignKey("dbo.ReferenceType", "Plant_PlantID1", "dbo.Plant", "PlantID");
            CreateIndex("dbo.Plant", "ReferenceType_ReferenceTypeID");
            CreateIndex("dbo.Plant", "ReferenceType_ReferenceTypeID1");
            CreateIndex("dbo.ReferenceType", "Plant_PlantID");
            CreateIndex("dbo.ReferenceType", "Plant_PlantID1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferenceType", new[] { "Plant_PlantID1" });
            DropIndex("dbo.ReferenceType", new[] { "Plant_PlantID" });
            DropIndex("dbo.Plant", new[] { "ReferenceType_ReferenceTypeID1" });
            DropIndex("dbo.Plant", new[] { "ReferenceType_ReferenceTypeID" });
            DropForeignKey("dbo.ReferenceType", "Plant_PlantID1", "dbo.Plant");
            DropForeignKey("dbo.ReferenceType", "Plant_PlantID", "dbo.Plant");
            DropForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID1", "dbo.ReferenceType");
            DropForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID", "dbo.ReferenceType");
            DropColumn("dbo.ReferenceType", "Plant_PlantID1");
            DropColumn("dbo.ReferenceType", "Plant_PlantID");
            DropColumn("dbo.Plant", "ReferenceType_ReferenceTypeID1");
            DropColumn("dbo.Plant", "ReferenceType_ReferenceTypeID");
        }
    }
}
