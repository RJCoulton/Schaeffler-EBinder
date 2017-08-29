namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class types : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReferenceType",
                c => new
                    {
                        ReferenceTypeID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 25),
                        CategoryID = c.Int(nullable: false),
                        typeCategory_TypeCategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ReferenceTypeID)
                .ForeignKey("dbo.TypeCategory", t => t.typeCategory_TypeCategoryID)
                .Index(t => t.typeCategory_TypeCategoryID);
            
            CreateTable(
                "dbo.TypeCategory",
                c => new
                    {
                        TypeCategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.TypeCategoryID);
            
            CreateTable(
                "dbo.ReferenceDepartment",
                c => new
                    {
                        Reference_ReferenceID = c.Int(nullable: false),
                        Department_DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reference_ReferenceID, t.Department_DepartmentID })
                .ForeignKey("dbo.Reference", t => t.Reference_ReferenceID, cascadeDelete: true)
                .ForeignKey("dbo.Department", t => t.Department_DepartmentID, cascadeDelete: true)
                .Index(t => t.Reference_ReferenceID)
                .Index(t => t.Department_DepartmentID);
            
            CreateTable(
                "dbo.ReferencePlant",
                c => new
                    {
                        Reference_ReferenceID = c.Int(nullable: false),
                        Plant_PlantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reference_ReferenceID, t.Plant_PlantID })
                .ForeignKey("dbo.Reference", t => t.Reference_ReferenceID, cascadeDelete: true)
                .ForeignKey("dbo.Plant", t => t.Plant_PlantID, cascadeDelete: true)
                .Index(t => t.Reference_ReferenceID)
                .Index(t => t.Plant_PlantID);
            
            CreateTable(
                "dbo.ReferenceRegion",
                c => new
                    {
                        Reference_ReferenceID = c.Int(nullable: false),
                        Region_RegionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reference_ReferenceID, t.Region_RegionID })
                .ForeignKey("dbo.Reference", t => t.Reference_ReferenceID, cascadeDelete: true)
                .ForeignKey("dbo.Region", t => t.Region_RegionID, cascadeDelete: true)
                .Index(t => t.Reference_ReferenceID)
                .Index(t => t.Region_RegionID);
            
            AddColumn("dbo.Reference", "TypeID", c => c.Int(nullable: false));
            AddColumn("dbo.Reference", "referenceType_ReferenceTypeID", c => c.Int());
            AddForeignKey("dbo.Reference", "referenceType_ReferenceTypeID", "dbo.ReferenceType", "ReferenceTypeID");
            CreateIndex("dbo.Reference", "referenceType_ReferenceTypeID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferenceRegion", new[] { "Region_RegionID" });
            DropIndex("dbo.ReferenceRegion", new[] { "Reference_ReferenceID" });
            DropIndex("dbo.ReferencePlant", new[] { "Plant_PlantID" });
            DropIndex("dbo.ReferencePlant", new[] { "Reference_ReferenceID" });
            DropIndex("dbo.ReferenceDepartment", new[] { "Department_DepartmentID" });
            DropIndex("dbo.ReferenceDepartment", new[] { "Reference_ReferenceID" });
            DropIndex("dbo.ReferenceType", new[] { "typeCategory_TypeCategoryID" });
            DropIndex("dbo.Reference", new[] { "referenceType_ReferenceTypeID" });
            DropForeignKey("dbo.ReferenceRegion", "Region_RegionID", "dbo.Region");
            DropForeignKey("dbo.ReferenceRegion", "Reference_ReferenceID", "dbo.Reference");
            DropForeignKey("dbo.ReferencePlant", "Plant_PlantID", "dbo.Plant");
            DropForeignKey("dbo.ReferencePlant", "Reference_ReferenceID", "dbo.Reference");
            DropForeignKey("dbo.ReferenceDepartment", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.ReferenceDepartment", "Reference_ReferenceID", "dbo.Reference");
            DropForeignKey("dbo.ReferenceType", "typeCategory_TypeCategoryID", "dbo.TypeCategory");
            DropForeignKey("dbo.Reference", "referenceType_ReferenceTypeID", "dbo.ReferenceType");
            DropColumn("dbo.Reference", "referenceType_ReferenceTypeID");
            DropColumn("dbo.Reference", "TypeID");
            DropTable("dbo.ReferenceRegion");
            DropTable("dbo.ReferencePlant");
            DropTable("dbo.ReferenceDepartment");
            DropTable("dbo.TypeCategory");
            DropTable("dbo.ReferenceType");
        }
    }
}
