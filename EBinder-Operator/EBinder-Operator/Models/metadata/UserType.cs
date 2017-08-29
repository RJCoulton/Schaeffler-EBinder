using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBinder_Operator.Models
{
    [MetadataType(typeof(UserTypeMetadata))]
    public partial class UserType { }

    public class UserTypeMetadata
    {
        public string Name { get; set; }
    }
}