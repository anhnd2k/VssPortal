using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DBConect.Farmework
{
    public partial class VssDbContext : DbContext
    {
        public VssDbContext()
            : base("name=VssDbContext")
        {
        }

        public virtual DbSet<AccountAdmin> AccountAdmins { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<CategoryPost> CategoryPosts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<ListPost> ListPosts { get; set; }
        public virtual DbSet<ListThankCardImg> ListThankCardImgs { get; set; }
        public virtual DbSet<OnboadingPost> OnboadingPosts { get; set; }
        public virtual DbSet<ThankCard> ThankCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountAdmin>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<AccountAdmin>()
                .Property(e => e.PassWord)
                .IsUnicode(false);
        }
    }
}
