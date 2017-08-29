using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace EBinder_Operator.Models
{
    public class Model { }

    public class Region : Model
    {
        public int RegionID { get; set; }
        [Required]
        [Display(Name = "Region")] 
        [MaxLength(18, ErrorMessage = "Name must be less than 18 characters"), MinLength(4, ErrorMessage = "Name must be more that 4 characters")]
        public string Name { get; set; }

        public virtual ICollection<Plant> Plants { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }

    public class Plant : Model
    {
        public int PlantID { get; set; }
        [Required]
        [Display(Name = "Plant")]
        [MaxLength(25, ErrorMessage = "Name must be less than 25 characters"), MinLength(4, ErrorMessage = "Name must be more that 4 characters")]
        public string Name { get; set; }

        public int RegionID { get; set; }
        public virtual Region Region { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Reference> References { get; set; }

        public virtual ICollection<ReferenceType> ReferenceTypes { get; set; }
        public virtual ICollection<ReferenceType> HomePageReferences { get; set; }
    }

    public class Department : Model
    {
        public int DepartmentID { get; set; }
        [Required]
        [Display(Name = "Department")]
        [MaxLength(15, ErrorMessage = "Name must be less than 15 characters"), MinLength(4, ErrorMessage = "Name must be more that 4 characters")]
        public string Name { get; set; }

        public int PlantID { get; set; }
        public virtual Plant Plant { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }

    public class Cell : Model
    {
        public int CellID { get; set; }
        [Required]
        [Display(Name = "Cell")]
        [MaxLength(15, ErrorMessage = "Name must be less than 15 characters"), MinLength(3, ErrorMessage = "Name must be more that 3 characters")]
        public string Name { get; set; }
        [Display(Name = "Workcenter #")]
        [StringLength(9, ErrorMessage = "Workcenter # must be 9 characters")]
        public string Workcenter { get; set; }

        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }

    public class User : Model
    {
        [Key]
        public string Username { get; set; }


        public int CellID { get; set; }
        public virtual Cell Cell { get; set; }

        public int UserTypeID { get; set; }
        public virtual UserType userType { get; set; }
    }

    public class UserType : Model
    {
        public int UserTypeID { get; set; }
        [Display(Name = "User Type")]
        [MaxLength(18, ErrorMessage = "Username must be less than 18 characters"), MinLength(5, ErrorMessage = "Username must be more than 5 characters")]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class Reference : Model
    {
        public int ReferenceID { get; set; }

        [Required]
        [Display(Name = "Reference Title")]
        [MaxLength(150, ErrorMessage = "Title must be less than 180 characters"), MinLength(5, ErrorMessage = "Title must be more than 5 characters")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Reference Number")]
        [MaxLength(10, ErrorMessage = "Reference # must be less than 10 characters"), MinLength(3, ErrorMessage = "Reference # must be more than 3 characters")]
        public string Number { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "URL must be less than 250 characters"), MinLength(5, ErrorMessage = "URL must be more than 5 characters")]
        public string URL { get; set; }

        public int ReferenceTypeID { get; set; }
        public virtual ReferenceType referenceType { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Plant> Plants { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
    }

    public class Part : Model
    {
        public int PartID { get; set; }
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }

    public class ReferenceCategory : Model
    {
        public int ReferenceCategoryID { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [MaxLength(40, ErrorMessage = "Name must be less than 40 characters"), MinLength(5, ErrorMessage = "Name must be more than 5 characters")]
        public string CategoryName { get; set; }
    }

    public class ReferenceType : Model
    {
        public int ReferenceTypeID { get; set; }
        [Required]
        [Display(Name = "Reference Type")]
        [MaxLength(25, ErrorMessage = "Name must be less than 25 characters"), MinLength(5, ErrorMessage = "Name must be more than 5 characters")]
        public string TypeName { get; set; }

        public int ReferenceCategoryID { get; set; }
        public virtual ReferenceCategory ReferenceCategory { get; set; }

        public virtual ICollection<Plant> Plants { get; set; }
        public virtual ICollection<Plant> PlantHomePage { get; set; }
    }
}