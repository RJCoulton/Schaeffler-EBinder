﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBinder.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CDContext : DbContext
    {
        public CDContext()
            : base("name=CDContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADInfo> ADInfoes { get; set; }
        public virtual DbSet<SAPCostCenter> SAPCostCenters { get; set; }
        public virtual DbSet<SAPEmployee> SAPEmployees { get; set; }
        public virtual DbSet<SAPPart> SAPParts { get; set; }
    }
}