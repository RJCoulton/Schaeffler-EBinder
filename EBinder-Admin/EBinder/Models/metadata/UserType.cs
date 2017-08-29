using EBinder.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder.Models
{
    [MetadataType(typeof(UserTypeMetadata))]
    public partial class UserType { }

    public class UserTypeMetadata
    {
        [Display(Name = "UserType", ResourceType = typeof(Resource))]
        public string Name { get; set; }
    }
}