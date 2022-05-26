
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using social_media.Entity;
using soicalMedia.Entity;

namespace social_media.Data
{
    public class DataContext:IdentityDbContext<AppUser,AppRole,int,
        IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }


        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Massage> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        { 
            base.OnModelCreating(builder);


            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();



            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s=>s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);


            //-------------------------------messages

            builder.Entity<Massage>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Massage>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
