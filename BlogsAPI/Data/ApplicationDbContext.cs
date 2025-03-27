using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
namespace BlogsAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<LikedBlogs> LikedBlogs { get; set; }
        public DbSet<SharedBlogs> SharedBlogs { get; set; }
        public DbSet<CommentedBlogs> CommentedBlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().Property(s => s.Id).HasMaxLength(200);  

            base.OnModelCreating(builder);

            #region Manage_Relations 
            builder.Entity<LikedBlogs>(entity =>
            {
                entity.HasKey(e => new { e.Id ,e.BlogId, e.AppUserId, e.TimeOccured });

                entity.Property(e => e.TimeOccured).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.AppUser)
                      .WithMany(a => a.LikedBlogsTbl)
                      .HasForeignKey(e => e.AppUserId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(e => e.Blog)
                      .WithMany(b => b.LikedBlogsTbl)
                      .HasForeignKey(e => e.BlogId)
                      .OnDelete(DeleteBehavior.Restrict); 
            }); 

            builder.Entity<SharedBlogs>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.BlogId, e.AppUserId, e.TimeOccured });

                entity.Property(e => e.TimeOccured).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.AppUser)
                      .WithMany(a => a.SharedBlogsTbl)
                      .HasForeignKey(e => e.AppUserId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(e => e.Blog)
                      .WithMany(b => b.SharedBlogsTbl)
                      .HasForeignKey(e => e.BlogId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CommentedBlogs>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.BlogId, e.AppUserId, e.TimeOccured });

                entity.Property(e => e.TimeOccured).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.AppUser)
                      .WithMany(a => a.CommentedBlogsTbl)
                      .HasForeignKey(e => e.AppUserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Blog)
                      .WithMany(b => b.CommentedBlogsTbl)
                      .HasForeignKey(e => e.BlogId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Blog>()
                 .HasOne(b => b.AppUser) 
                 .WithMany(a => a.Blogs) 
                 .HasForeignKey(b => b.AppUserId)  
                 .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Blog>()
                .HasOne(x => x.Category)
                .WithMany(y => y.Blog)
                .HasForeignKey(f => f.CategoryId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Contact>()
                .HasOne(x=>x.AppUser)
                .WithMany(c=>c.Contacts)
                .HasForeignKey(e => e.AppUserId).OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Seeding
            var adminRoleId = "2e7cb116-e1fb-4bcb-84af-ba2dc9825faa";
            var userRoleId = "8711ea17-a376-4691-af65-3d57f39b69c6";
            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole {
                    Id = adminRoleId,
                    Name = AppConstants.ADMIN,
                    NormalizedName = AppConstants.ADMIN.ToUpper()
                    },
                    new IdentityRole { 
                    Id = userRoleId,
                    Name = AppConstants.USER,
                    NormalizedName = AppConstants.USER.ToUpper()
                    }
            }; 
            builder.Entity<IdentityRole>().HasData(roles);
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Lifestyle" },
                new Category { Id = 2, Name = "Health" },
                new Category { Id = 3, Name = "Technology" },
                new Category { Id = 4, Name = "Travel" },
                new Category { Id = 5, Name = "Finance" },
                new Category { Id = 6, Name = "Food" },
                new Category { Id = 7, Name = "Parenting" },
                new Category { Id = 8, Name = "Education" },
                new Category { Id = 9, Name = "Fashion" },
                new Category { Id = 10, Name = "Hobbies" },
                new Category { Id = 11, Name = "Business" }
            );

            var userId = "503d8962-0aee-4e4a-aff2-38638db4f282";
            var userEmail = "waficko200@example.com";
            var userUsername = "waficko200";
            var user = new AppUser
            {
                Id = userId,
                Email = userEmail,
                EmailConfirmed = true,
                UserName = userUsername,
                NormalizedUserName = userUsername.ToUpper(),
                NormalizedEmail = userEmail.ToUpper()
            };

            PasswordHasher<AppUser> ph = new();
            user.PasswordHash = ph.HashPassword(user, "Dreamo2000$");

            builder.Entity<AppUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = userId, RoleId = adminRoleId });
            #endregion
        } 

    }
}
