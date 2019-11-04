﻿// <auto-generated />
using System;
using System.Collections.Generic;
using ConventionManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConventionManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191104170732_AddModelsForSubscription")]
    partial class AddModelsForSubscription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<List<int>>("SuscribedConferences");

                    b.Property<List<int>>("SuscribedEvents");

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

                    b.Property<string>("Location");

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

            modelBuilder.Entity("ConventionManager.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Capacity");

                    b.Property<int>("EventCenterId");

                    b.Property<string>("Location");

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
                            Id = "9a4d4347-23c2-4ae4-9f6c-9a5688f0f64f",
                            ConcurrencyStamp = "79051323-c91f-492e-928f-18d8e92b828f",
                            Name = "Organizer",
                            NormalizedName = "ORGANIZER"
                        },
                        new
                        {
                            Id = "4fd00700-48a5-42b9-8dcf-50b39d7b3df9",
                            ConcurrencyStamp = "9331da2d-fb0c-457d-a6cb-cfd2743014f0",
                            Name = "Exhibitor",
                            NormalizedName = "EXHIBITOR"
                        },
                        new
                        {
                            Id = "c766afbf-f91b-4c9c-aecb-4f37470c706d",
                            ConcurrencyStamp = "cec73bd4-db20-4184-97f1-46088905e36f",
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
