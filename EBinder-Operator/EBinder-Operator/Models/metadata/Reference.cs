﻿using EBinder_Operator.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder_Operator.Models
{
    [MetadataType(typeof(ReferenceMetadata))]
    public partial class Reference { }

    public class ReferenceMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TitleRequired")]
        [Display(Name = "ReferenceTitle", ResourceType = typeof(Resource))]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RefTitleTooLong")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RefTitleTooShort")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RefNumberRequired")]
        [Display(Name = "ReferenceNumber", ResourceType = typeof(Resource))]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RefNumberTooLong")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RefNumberTooShort")]
        public string Number { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "URLRequired")]
        [Display(Name = "URL", ResourceType = typeof(Resource))]
        [MaxLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "URLTooLong")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "URLTooShort")]
        public string URL { get; set; }

        [Display(Name = "FlagDate", ResourceType = typeof(Resource))]
        public DateTime FlagDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RefTypeRequired")]
        [Display(Name = "ReferenceType", ResourceType = typeof(Resource))]
        public int ReferenceTypeID { get; set; }
    }
}