//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBinder_Operator.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReferenceCategory
    {
        public ReferenceCategory()
        {
            this.ReferenceTypes = new HashSet<ReferenceType>();
        }
    
        public int ReferenceCategoryID { get; set; }
        public string CategoryName_en { get; set; }
        public string CategoryName_es { get; set; }
    
        public virtual ICollection<ReferenceType> ReferenceTypes { get; set; }
    }
}
