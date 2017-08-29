using EBinder.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EBinder.Models
{
    [MetadataType(typeof(CellMetadata))]
    public partial class Cell { }

    public class CellMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Cell", ResourceType = typeof(Resource))]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooLong")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooShort")]
        public string Name { get; set; }

        [Display(Name = "Workcenter", ResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "WorkcenterLength")]
        public string Workcenter { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "DepartmentRequired")]
        [Display(Name = "Department", ResourceType = typeof(Resource))]
        public int DepartmentID { get; set; }
    }
}