using EBinder.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder.Models
{
    [MetadataType(typeof(RegionMetadata))]
    public partial class Region { }

    public class RegionMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Region", ResourceType = typeof(Resource))]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooLong")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameTooShort")]
        public string Name { get; set; }
    }
}