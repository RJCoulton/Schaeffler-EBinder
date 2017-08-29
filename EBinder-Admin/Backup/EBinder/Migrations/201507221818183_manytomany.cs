namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID", "dbo.ReferenceType");
            DropForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID1", "dbo.ReferenceType");
            DropForeignKey("dbo.ReferenceType", "Plant_PlantID", "dbo.Plant");
            DropForeignKey("dbo.ReferenceType", "Plant_PlantID1", "dbo.Plant");
            DropIndex("dbo.Plant", new[] { "ReferenceType_ReferenceTypeID" });
            DropIndex("dbo.Plant", new[] { "ReferenceType_ReferenceTypeID1" });
            DropIndex("dbo.ReferenceType", new[] { "Plant_PlantID" });
            DropIndex("dbo.ReferenceType", new[] { "Plant_PlantID1" });
            CreateTable(
                "dbo.ReferenceTypePlant",
                c => new
                    {
                        ReferenceType_ReferenceTypeID = c.Int(nullable: false),
                        Plant_PlantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReferenceType_ReferenceTypeID, t.Plant_PlantID })
                .ForeignKey("dbo.ReferenceType", t => t.ReferenceType_ReferenceTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Plant", t => t.Plant_PlantID, cascadeDelete: true)
                .Index(t => t.ReferenceType_ReferenceTypeID)
                .Index(t => t.Plant_PlantID);
            
            CreateTable(
                "dbo.HomePageReferences",
                c => new
                    {
                        PlantId = c.Int(nullable: false),
                        ReferenceTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PlantId, t.ReferenceTypeId })
                .ForeignKey("dbo.Plant", t => t.PlantId, cascadeDelete: true)
                .ForeignKey("dbo.ReferenceType", t => t.ReferenceTypeId, cascadeDelete: true)
                .Index(t => t.PlantId)
                .Index(t => t.ReferenceTypeId);
            
            DropColumn("dbo.Plant", "ReferenceType_ReferenceTypeID");
            DropColumn("dbo.Plant", "ReferenceType_ReferenceTypeID1");
            DropColumn("dbo.ReferenceType", "Plant_PlantID");
            DropColumn("dbo.ReferenceType", "Plant_PlantID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReferenceType", "Plant_PlantID1", c => c.Int());
            AddColumn("dbo.ReferenceType", "Plant_PlantID", c => c.Int());
            AddColumn("dbo.Plant", "ReferenceType_ReferenceTypeID1", c => c.Int());
            AddColumn("dbo.Plant", "ReferenceType_ReferenceTypeID", c => c.Int());
            DropIndex("dbo.HomePageReferences", new[] { "ReferenceTypeId" });
            DropIndex("dbo.HomePageReferences", new[] { "PlantId" });
            DropIndex("dbo.ReferenceTypePlant", new[] { "Plant_PlantID" });
            DropIndex("dbo.ReferenceTypePlant", new[] { "ReferenceType_ReferenceTypeID" });
            DropForeignKey("dbo.HomePageReferences", "ReferenceTypeId", "dbo.ReferenceType");
            DropForeignKey("dbo.HomePageReferences", "PlantId", "dbo.Plant");
            DropForeignKey("dbo.ReferenceTypePlant", "Plant_PlantID", "dbo.Plant");
            DropForeignKey("dbo.ReferenceTypePlant", "ReferenceType_ReferenceTypeID", "dbo.ReferenceType");
            DropTable("dbo.HomePageReferences");
            DropTable("dbo.ReferenceTypePlant");
            CreateIndex("dbo.ReferenceType", "Plant_PlantID1");
            CreateIndex("dbo.ReferenceType", "Plant_PlantID");
            CreateIndex("dbo.Plant", "ReferenceType_ReferenceTypeID1");
            CreateIndex("dbo.Plant", "ReferenceType_ReferenceTypeID");
            AddForeignKey("dbo.ReferenceType", "Plant_PlantID1", "dbo.Plant", "PlantID");
            AddForeignKey("dbo.ReferenceType", "Plant_PlantID", "dbo.Plant", "PlantID");
            AddForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID1", "dbo.ReferenceType", "ReferenceTypeID");
            AddForeignKey("dbo.Plant", "ReferenceType_ReferenceTypeID", "dbo.ReferenceType", "ReferenceTypeID");
        }
    }
}
