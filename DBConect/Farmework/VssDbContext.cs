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
        public virtual DbSet<ManageRegisterIdea> ManageRegisterIdeas { get; set; }
        public virtual DbSet<CategoryPost> CategoryPosts { get; set; }
        public virtual DbSet<CommentIdeaPost> CommentIdeaPosts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FieldInIdeal> FieldInIdeals { get; set; }
        public virtual DbSet<FieldRealTalk> FieldRealTalks { get; set; }
        public virtual DbSet<InitativeManage> InitativeManages { get; set; }
        public virtual DbSet<LikeCountPostIdea> LikeCountPostIdeas { get; set; }
        public virtual DbSet<LikeIdeaPost> LikeIdeaPosts { get; set; }
        public virtual DbSet<ListPost> ListPosts { get; set; }
        public virtual DbSet<ListThankCardImg> ListThankCardImgs { get; set; }
        public virtual DbSet<OnboadingPost> OnboadingPosts { get; set; }
        public virtual DbSet<PersionManageRealTalk> PersionManageRealTalks { get; set; }
        public virtual DbSet<StatusIdeaInitative> StatusIdeaInitatives { get; set; }
        public virtual DbSet<TalkReal> TalkReals { get; set; }
        public virtual DbSet<ThankCard> ThankCards { get; set; }
        public virtual DbSet<TruthStatu> TruthStatus { get; set; }
        public virtual DbSet<UnitApply> UnitApplies { get; set; }

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
