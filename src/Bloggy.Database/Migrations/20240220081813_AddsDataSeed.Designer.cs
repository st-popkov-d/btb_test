﻿// <auto-generated />
using System;
using Bloggy.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bloggy.Database.Migrations
{
    [DbContext(typeof(BloggyDbContext))]
    [Migration("20240220081813_AddsDataSeed")]
    partial class AddsDataSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bloggy.Database.Entities.BlogPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(16383)
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("nvarchar(127)")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("blog_posts", (string)null);
                });

            modelBuilder.Entity("Bloggy.Database.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("BlogPostId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("blog_post_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(16383)
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("BlogPostId");

                    b.HasIndex("UserId");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("Bloggy.Database.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("nvarchar(63)")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("nvarchar(63)")
                        .HasColumnName("password_salt");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("nvarchar(63)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e1318721-b268-4d74-83fb-c35d08525f34"),
                            CreatedAt = new DateTimeOffset(new DateTime(2024, 2, 20, 8, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            PasswordHash = "ABC073BE888C36BF213A3B669E2BC892EE18F6C6",
                            PasswordSalt = "73616C74",
                            Username = "jordano"
                        },
                        new
                        {
                            Id = new Guid("7004a7ce-45e5-4d79-97e6-cb791ee5eb17"),
                            CreatedAt = new DateTimeOffset(new DateTime(2024, 2, 20, 8, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            PasswordHash = "ABC073BE888C36BF213A3B669E2BC892EE18F6C6",
                            PasswordSalt = "73616C74",
                            Username = "alex"
                        },
                        new
                        {
                            Id = new Guid("dd985147-f7f1-49ab-9e25-cd6f5e7f3be5"),
                            CreatedAt = new DateTimeOffset(new DateTime(2024, 2, 20, 8, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            PasswordHash = "ABC073BE888C36BF213A3B669E2BC892EE18F6C6",
                            PasswordSalt = "73616C74",
                            Username = "simon"
                        });
                });

            modelBuilder.Entity("Bloggy.Database.Entities.BlogPost", b =>
                {
                    b.HasOne("Bloggy.Database.Entities.User", "User")
                        .WithMany("BlogPosts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bloggy.Database.Entities.Comment", b =>
                {
                    b.HasOne("Bloggy.Database.Entities.BlogPost", "BlogPost")
                        .WithMany("Comments")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bloggy.Database.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BlogPost");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bloggy.Database.Entities.BlogPost", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Bloggy.Database.Entities.User", b =>
                {
                    b.Navigation("BlogPosts");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
