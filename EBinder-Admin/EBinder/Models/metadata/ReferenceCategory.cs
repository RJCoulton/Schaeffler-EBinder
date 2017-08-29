using EBinder.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder.Models
{
    [MetadataType(typeof(ReferenceCategoryMetadata))]
    public partial class ReferenceCategory
    {
    }

    public class ReferenceCategoryMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resource))]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CategoryNameTooLong")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CategoryNameTooShort")]
        public string CategoryName_en { get; set; }
        [Display(Name = "CategoryName", ResourceType = typeof(Resource))]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CategoryNameTooLong")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CategoryNameTooShort")]
        public string CategoryName_es { get; set; }
    }
}