namespace EBinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNavigationProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Plant", "Region_RegionID", "dbo.Region");
            DropForeignKey("dbo.Department", "Plant_PlantID", "dbo.Plant");
            DropForeignKey("dbo.Cell", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.User", "Cell_CellID", "dbo.Cell");
            DropIndex("dbo.Plant", new[] { "Region_RegionID" });
            DropIndex("dbo.Department", new[] { "Plant_PlantID" });
            DropIndex("dbo.Cell", new[] { "Department_DepartmentID" });
            DropIndex("dbo.User", new[] { "Cell_CellID" });
            AddColumn("dbo.Plant", "RegionID", c => c.Int(nullable: false));
            AddColumn("dbo.Department", "PlantID", c => c.Int(nullable: false));
            AddColumn("dbo.Cell", "DepartmentID", c => c.Int(nullable: false));
            AddColumn("dbo.User", "CellID", c => c.Int(nullable: false));
            AddColumn("dbo.User", "UserTypeID", c => c.Int(nullable: false));
            AddForeignKey("dbo.Plant", "RegionID", "dbo.Region", "RegionID", cascadeDelete: true);
            AddForeignKey("dbo.Department", "PlantID", "dbo.Plant", "PlantID", cascadeDelete: true);
            AddForeignKey("dbo.Cell", "DepartmentID", "dbo.Department", "DepartmentID", cascadeDelete: true);
            AddForeignKey("dbo.User", "CellID", "dbo.Cell", "CellID", cascadeDelete: true);
            AddForeignKey("dbo.User", "UserTypeID", "dbo.UserType", "UserTypeID", cascadeDelete: true);
            CreateIndex("dbo.Plant", "RegionID");
            CreateIndex("dbo.Department", "PlantID");
            CreateIndex("dbo.Cell", "DepartmentID");
            CreateIndex("dbo.User", "CellID");
            CreateIndex("dbo.User", "UserTypeID");
            DropColumn("dbo.Plant", "Region_RegionID");
            DropColumn("dbo.Department", "Plant_PlantID");
            DropColumn("dbo.Cell", "Department_DepartmentID");
            DropColumn("dbo.User", "Cell_CellID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Cell_CellID", c => c.Int());
            AddColumn("dbo.Cell", "Department_DepartmentID", c => c.Int());
            AddColumn("dbo.Department", "Plant_PlantID", c => c.Int());
            AddColumn("dbo.Plant", "Region_RegionID", c => c.Int());
            DropIndex("dbo.User", new[] { "UserTypeID" });
            DropIndex("dbo.User", new[] { "CellID" });
            DropIndex("dbo.Cell", new[] { "DepartmentID" });
            DropIndex("dbo.Department", new[] { "PlantID" });
            DropIndex("dbo.Plant", new[] { "RegionID" });
            DropForeignKey("dbo.User", "UserTypeID", "dbo.UserType");
            DropForeignKey("dbo.User", "CellID", "dbo.Cell");
            DropForeignKey("dbo.Cell", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "PlantID", "dbo.Plant");
            DropForeignKey("dbo.Plant", "RegionID", "dbo.Region");
            DropColumn("dbo.User", "UserTypeID");
            DropColumn("dbo.User", "CellID");
            DropColumn("dbo.Cell", "DepartmentID");
            DropColumn("dbo.Department", "PlantID");
            DropColumn("dbo.Plant", "RegionID");
            CreateIndex("dbo.User", "Cell_CellID");
            CreateIndex("dbo.Cell", "Department_DepartmentID");
            CreateIndex("dbo.Department", "Plant_PlantID");
            CreateIndex("dbo.Plant", "Region_RegionID");
            AddForeignKey("dbo.User", "Cell_CellID", "dbo.Cell", "CellID");
            AddForeignKey("dbo.Cell", "Department_DepartmentID", "dbo.Department", "DepartmentID");
            AddForeignKey("dbo.Department", "Plant_PlantID", "dbo.Plant", "PlantID");
            AddForeignKey("dbo.Plant", "Region_RegionID", "dbo.Region", "RegionID");
        }
    }
}
