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
    
    public partial class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
            this.References = new HashSet<Reference>();
        }
    
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public int PlantID { get; set; }
    
        public virtual ICollection<Cell> Cells { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }
}
