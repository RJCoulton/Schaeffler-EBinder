using EBinder_Operator.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder_Operator.Models
{
    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department { }

    public class DepartmentMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RegionRequired")]
        [Display(Name = "Department", ResourceType=typeof(Resource))]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooLong")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooShort")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PlantRequired")]
        [Display(Name = "Plant", ResourceType = typeof(Resource))]
        public int PlantID { get; set; }
    }
}