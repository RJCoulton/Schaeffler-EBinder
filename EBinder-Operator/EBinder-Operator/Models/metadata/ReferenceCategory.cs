using EBinder_Operator.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder_Operator.Models
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
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resource))]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CategoryNameTooLong")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CategoryNameTooShort")]
        public string CategoryName_es { get; set; }
    }
}