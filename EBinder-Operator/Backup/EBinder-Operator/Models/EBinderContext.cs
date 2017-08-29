using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EBinder_Operator.Models
{
    public class EBinderContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ReferenceType> ReferenceTypes { get; set; }
        public DbSet<ReferenceCategory> ReferenceCategory { get; set; }

        public EBinderContext() : base("EBinderConnection") { }        


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Plant>()
                    .HasMany(x => x.ReferenceTypes)
                    .WithMany(x => x.Plants)
                    .Map(x =>
                    {
                        x.ToTable("PlantReferenceTypes");
                        x.MapLeftKey("PlantId");
                        x.MapRightKey("ReferenceTypeId");
                    });

            modelBuilder.Entity<Plant>()
                    .HasMany(x => x.ReferenceTypes)
                    .WithMany(x => x.Plants)
                    .Map(x =>
                    {
                        x.ToTable("HomePageReferences");
                        x.MapLeftKey("PlantId");
                        x.MapRightKey("ReferenceTypeId");
                    });


        }



    }
}