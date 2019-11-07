﻿// <auto-generated />
using System;
using ConventionManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConventionManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ConventionManager.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<byte[]>("Curriculum");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<byte[]>("ProfilePicture");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ConventionManager.Models.Conference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("EventCenterId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("EventCenterId");

                    b.ToTable("Conferences");
                });

            modelBuilder.Entity("ConventionManager.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConferenceId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("RoomId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ConferenceId");

                    b.HasIndex("RoomId");

                    b.ToTable("Events");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Event");
                });

            modelBuilder.Entity("ConventionManager.Models.EventCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Latitude");

                    b.Property<string>("Location");

                    b.Property<float>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("EventCenters");
                });

            modelBuilder.Entity("ConventionManager.Models.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("FoodEventId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TypeOfFood")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("FoodEventId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("ConventionManager.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<DateTime>("SentOn");

                    b.Property<int>("SubscriptionId");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ConventionManager.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Capacity");

                    b.Property<int>("EventCenterId");

                    b.Property<float>("Latitude");

                    b.Property<string>("Location");

                    b.Property<float>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("EventCenterId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("ConventionManager.Models.Sponsor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConferenceId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ConferenceId");

                    b.ToTable("Sponsors");
                });

            modelBuilder.Entity("ConventionManager.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConferenceId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("EventId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Subscription");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "30bac4b7-9c18-46df-a0ae-f6068bb47e0f",
                            ConcurrencyStamp = "99b3ef9a-b60e-47d4-97c6-bc41d2d7135b",
                            Name = "Organizer",
                            NormalizedName = "ORGANIZER"
                        },
                        new
                        {
                            Id = "d0c55872-566c-4b6b-afde-d0c7e9df52b3",
                            ConcurrencyStamp = "46ecec52-da7e-4a36-9555-8d5c03755205",
                            Name = "Exhibitor",
                            NormalizedName = "EXHIBITOR"
                        },
                        new
                        {
                            Id = "bd06f069-ff42-4fbc-8226-9e01ca1f759a",
                            ConcurrencyStamp = "7724312c-5b3f-4f73-9a47-f2f991c639ab",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ConventionManager.Models.ExhibitorEvent", b =>
                {
                    b.HasBaseType("ConventionManager.Models.Event");

                    b.Property<string>("Topic")
                        .IsRequired();

                    b.HasDiscriminator().HasValue("ExhibitorEvent");
                });

            modelBuilder.Entity("ConventionManager.Models.FoodEvent", b =>
                {
                    b.HasBaseType("ConventionManager.Models.Event");

                    b.HasDiscriminator().HasValue("FoodEvent");
                });

            modelBuilder.Entity("ConventionManager.Models.PartyEvent", b =>
                {
                    b.HasBaseType("ConventionManager.Models.Event");

                    b.HasDiscriminator().HasValue("PartyEvent");
                });

            modelBuilder.Entity("ConventionManager.Models.AttendantSubscription", b =>
                {
                    b.HasBaseType("ConventionManager.Models.Subscription");

                    b.HasDiscriminator().HasValue("AttendantSubscription");
                });

            modelBuilder.Entity("ConventionManager.Models.ExhibitorSubscription", b =>
                {
                    b.HasBaseType("ConventionManager.Models.Subscription");

                    b.HasDiscriminator().HasValue("ExhibitorSubscription");
                });

            modelBuilder.Entity("ConventionManager.Models.ChatEvent", b =>
                {
                    b.HasBaseType("ConventionManager.Models.ExhibitorEvent");

                    b.Property<int>("ModeratorId");

                    b.HasDiscriminator().HasValue("ChatEvent");
                });

            modelBuilder.Entity("ConventionManager.Models.PracticalSessionsEvent", b =>
                {
                    b.HasBaseType("ConventionManager.Models.ExhibitorEvent");

                    b.HasDiscriminator().HasValue("PracticalSessionsEvent");
                });

            modelBuilder.Entity("ConventionManager.Models.TalkEvent", b =>
                {
                    b.HasBaseType("ConventionManager.Models.ExhibitorEvent");

                    b.HasDiscriminator().HasValue("TalkEvent");
                });

            modelBuilder.Entity("ConventionManager.Models.Conference", b =>
                {
                    b.HasOne("ConventionManager.Models.EventCenter", "EventCenter")
                        .WithMany("Conferences")
                        .HasForeignKey("EventCenterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConventionManager.Models.Event", b =>
                {
                    b.HasOne("ConventionManager.Models.Conference", "Conference")
                        .WithMany("Events")
                        .HasForeignKey("ConferenceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ConventionManager.Models.Room", "Room")
                        .WithMany("Events")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConventionManager.Models.Food", b =>
                {
                    b.HasOne("ConventionManager.Models.FoodEvent", "FoodEvent")
                        .WithMany("Menu")
                        .HasForeignKey("FoodEventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConventionManager.Models.Notification", b =>
                {
                    b.HasOne("ConventionManager.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ConventionManager.Models.Room", b =>
                {
                    b.HasOne("ConventionManager.Models.EventCenter", "EventCenter")
                        .WithMany("Rooms")
                        .HasForeignKey("EventCenterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConventionManager.Models.Sponsor", b =>
                {
                    b.HasOne("ConventionManager.Models.Conference", "Conference")
                        .WithMany("Sponsors")
                        .HasForeignKey("ConferenceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConventionManager.Models.Subscription", b =>
                {
                    b.HasOne("ConventionManager.Models.Event", "Event")
                        .WithMany("Subscriptions")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ConventionManager.Models.ApplicationUser", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ConventionManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ConventionManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ConventionManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ConventionManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
