namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        RegionID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RegionID);
            
            CreateTable(
                "dbo.Plant",
                c => new
                    {
                        PlantID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Region_RegionID = c.Int(),
                    })
                .PrimaryKey(t => t.PlantID)
                .ForeignKey("dbo.Region", t => t.Region_RegionID)
                .Index(t => t.Region_RegionID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Plant_PlantID = c.Int(),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.Plant", t => t.Plant_PlantID)
                .Index(t => t.Plant_PlantID);
            
            CreateTable(
                "dbo.Cell",
                c => new
                    {
                        CellID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Workcenter = c.Int(nullable: false),
                        Department_DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.CellID)
                .ForeignKey("dbo.Department", t => t.Department_DepartmentID)
                .Index(t => t.Department_DepartmentID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Cell_CellID = c.Int(),
                    })
                .PrimaryKey(t => t.Username)
                .ForeignKey("dbo.Cell", t => t.Cell_CellID)
                .Index(t => t.Cell_CellID);
            
            CreateTable(
                "dbo.Part",
                c => new
                    {
                        PartID = c.Int(nullable: false, identity: true),
                        PartNumber = c.String(),
                    })
                .PrimaryKey(t => t.PartID);
            
            CreateTable(
                "dbo.Reference",
                c => new
                    {
                        ReferenceID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ReferenceID);
            
            CreateTable(
                "dbo.UserType",
                c => new
                    {
                        UserTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserTypeID);
            
            CreateTable(
                "dbo.PartCell",
                c => new
                    {
                        Part_PartID = c.Int(nullable: false),
                        Cell_CellID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Part_PartID, t.Cell_CellID })
                .ForeignKey("dbo.Part", t => t.Part_PartID, cascadeDelete: true)
                .ForeignKey("dbo.Cell", t => t.Cell_CellID, cascadeDelete: true)
                .Index(t => t.Part_PartID)
                .Index(t => t.Cell_CellID);
            
            CreateTable(
                "dbo.ReferenceCell",
                c => new
                    {
                        Reference_ReferenceID = c.Int(nullable: false),
                        Cell_CellID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reference_ReferenceID, t.Cell_CellID })
                .ForeignKey("dbo.Reference", t => t.Reference_ReferenceID, cascadeDelete: true)
                .ForeignKey("dbo.Cell", t => t.Cell_CellID, cascadeDelete: true)
                .Index(t => t.Reference_ReferenceID)
                .Index(t => t.Cell_CellID);
            
            CreateTable(
                "dbo.ReferencePart",
                c => new
                    {
                        Reference_ReferenceID = c.Int(nullable: false),
                        Part_PartID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reference_ReferenceID, t.Part_PartID })
                .ForeignKey("dbo.Reference", t => t.Reference_ReferenceID, cascadeDelete: true)
                .ForeignKey("dbo.Part", t => t.Part_PartID, cascadeDelete: true)
                .Index(t => t.Reference_ReferenceID)
                .Index(t => t.Part_PartID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferencePart", new[] { "Part_PartID" });
            DropIndex("dbo.ReferencePart", new[] { "Reference_ReferenceID" });
            DropIndex("dbo.ReferenceCell", new[] { "Cell_CellID" });
            DropIndex("dbo.ReferenceCell", new[] { "Reference_ReferenceID" });
            DropIndex("dbo.PartCell", new[] { "Cell_CellID" });
            DropIndex("dbo.PartCell", new[] { "Part_PartID" });
            DropIndex("dbo.User", new[] { "Cell_CellID" });
            DropIndex("dbo.Cell", new[] { "Department_DepartmentID" });
            DropIndex("dbo.Department", new[] { "Plant_PlantID" });
            DropIndex("dbo.Plant", new[] { "Region_RegionID" });
            DropForeignKey("dbo.ReferencePart", "Part_PartID", "dbo.Part");
            DropForeignKey("dbo.ReferencePart", "Reference_ReferenceID", "dbo.Reference");
            DropForeignKey("dbo.ReferenceCell", "Cell_CellID", "dbo.Cell");
            DropForeignKey("dbo.ReferenceCell", "Reference_ReferenceID", "dbo.Reference");
            DropForeignKey("dbo.PartCell", "Cell_CellID", "dbo.Cell");
            DropForeignKey("dbo.PartCell", "Part_PartID", "dbo.Part");
            DropForeignKey("dbo.User", "Cell_CellID", "dbo.Cell");
            DropForeignKey("dbo.Cell", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "Plant_PlantID", "dbo.Plant");
            DropForeignKey("dbo.Plant", "Region_RegionID", "dbo.Region");
            DropTable("dbo.ReferencePart");
            DropTable("dbo.ReferenceCell");
            DropTable("dbo.PartCell");
            DropTable("dbo.UserType");
            DropTable("dbo.Reference");
            DropTable("dbo.Part");
            DropTable("dbo.User");
            DropTable("dbo.Cell");
            DropTable("dbo.Department");
            DropTable("dbo.Plant");
            DropTable("dbo.Region");
        }
    }
}
