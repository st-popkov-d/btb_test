using Bloggy.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Bloggy.Database
{
    public class BloggyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BloggyDbContext(DbContextOptions<BloggyDbContext> options): base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<BlogPost>().ToTable("blog_posts");
            builder.Entity<Comment>().ToTable("comments");

            builder.Entity<User>(userBuilder =>
            {
                userBuilder.HasKey(x => x.Id);

                userBuilder
                    .Property(x => x.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("NEWID()")
                    .IsRequired();

                userBuilder
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                    .IsRequired();

                userBuilder
                    .Property(x => x.Username)
                    .HasColumnName("username")
                    .HasMaxLength(63)
                    .IsRequired();

                userBuilder
                    .Property(x => x.PasswordSalt)
                    .HasColumnName("password_salt")
                    .HasMaxLength(63)
                    .IsRequired();

                userBuilder
                    .Property(x => x.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasMaxLength(63)
                    .IsRequired();

                userBuilder
                    .HasMany(x => x.BlogPosts)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();

                userBuilder
                    .HasMany(x => x.Comments)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

            });

            builder.Entity<BlogPost>(blogBuilder =>
            {
                blogBuilder.HasKey(x => x.Id);

                blogBuilder
                    .Property(x => x.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("NEWID()")
                    .IsRequired();

                blogBuilder
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                    .IsRequired();

                blogBuilder
                    .Property(x => x.Title)
                    .HasColumnName("title")
                    .HasMaxLength(127)
                    .IsRequired();

                blogBuilder
                    .Property(x => x.Content)
                    .HasColumnName("content")
                    .HasMaxLength(16383)
                    .IsRequired();

                blogBuilder
                    .Property(x => x.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                blogBuilder
                    .HasMany(x => x.Comments)
                    .WithOne(x => x.BlogPost)
                    .HasForeignKey(x => x.BlogPostId)
                    .IsRequired();
            });

            builder.Entity<Comment>(commentBuilder =>
            {
                commentBuilder.HasKey(x => x.Id);

                commentBuilder
                    .Property(x => x.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("NEWID()")
                    .IsRequired();

                commentBuilder
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                    .IsRequired();

                commentBuilder
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                    .IsRequired();

                commentBuilder
                    .Property(x => x.Content)
                    .HasColumnName("content")
                    .HasMaxLength(16383)
                    .IsRequired();

                commentBuilder
                    .Property(x => x.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                commentBuilder
                    .Property(x => x.BlogPostId)
                    .HasColumnName("blog_post_id")
                    .IsRequired();
            });


            builder.Entity<User>().HasData([
                new User
                {
                    Id = Guid.Parse("e1318721-b268-4d74-83fb-c35d08525f34"),
                    Username = "jordano",
                    CreatedAt = DateTimeOffset.Parse("2/20/2024 8:00:40 AM +00:00"),
                    PasswordSalt = Convert.ToHexString("salt"u8.ToArray()),
                    PasswordHash = DummyHashFn("password", Convert.ToHexString("salt"u8.ToArray()))
                },
                new User
                {
                    Id = Guid.Parse("7004a7ce-45e5-4d79-97e6-cb791ee5eb17"),
                    Username = "alex",
                    CreatedAt = DateTimeOffset.Parse("2/20/2024 8:00:40 AM +00:00"),
                    PasswordSalt = Convert.ToHexString("salt"u8.ToArray()),
                    PasswordHash = DummyHashFn("password", Convert.ToHexString("salt"u8.ToArray()))
                },
                new User
                {
                    Id = Guid.Parse("dd985147-f7f1-49ab-9e25-cd6f5e7f3be5"),
                    Username = "simon",
                    CreatedAt = DateTimeOffset.Parse("2/20/2024 8:00:40 AM +00:00"),
                    PasswordSalt = Convert.ToHexString("salt"u8.ToArray()),
                    PasswordHash = DummyHashFn("password", Convert.ToHexString("salt"u8.ToArray()))
                },
            ]);
        }

        /*
         * This method is placed here only for seeding database with hashed passwords,
         * and is a copy of Bloggy.WebApi.Services.AuthService.DummyHashFn.
         * Ideally it should be extracted and placed in Libs/Utils project of solution.
         */
        private string DummyHashFn(string input, string salt)
        {
            var saltBytes = Convert.FromHexString(salt);
            var passwordBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: passwordBytes,
                salt: saltBytes,
                iterations: 5,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 20);
            var hash = Convert.ToHexString(hashBytes);
            return hash;
        }
    }
}
