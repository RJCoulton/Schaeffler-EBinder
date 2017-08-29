using EBinder.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder.Models
{
    [MetadataType(typeof(ReferenceTypeMetadata))]
    public partial class ReferenceType { }

    public class ReferenceTypeMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "ReferenceType", ResourceType = typeof(Resource))]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TypeTooLong")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TypeTooShort")]
        public string TypeName_en { get; set; }

        [Display(Name = "ReferenceType", ResourceType = typeof(Resource))]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TypeTooLong")]
        public string TypeName_es { get; set; }
    }
}