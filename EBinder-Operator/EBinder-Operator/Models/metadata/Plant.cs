using EBinder_Operator.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder_Operator.Models
{
    [MetadataType(typeof(PlantMetadata))]
    public partial class Plant { }

    public class PlantMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Plant", ResourceType = typeof(Resource))]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooLong")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooShort")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "ContactEmail", ResourceType = typeof(Resource))]
        public string ContactEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RegionRequired")]
        [Display(Name = "Region", ResourceType = typeof(Resource))]
        public int RegionID { get; set; }
        
        [Display(Name = "Language", ResourceType = typeof(Resource))]
        public string Language { get; set; }
    }
}