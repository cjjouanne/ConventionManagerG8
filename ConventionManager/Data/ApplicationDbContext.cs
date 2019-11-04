using System;
using System.Collections.Generic;
using System.Text;
using ConventionManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConventionManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<EventCenter> EventCenters { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ExhibitorEvent> ExhibitorEvents { get; set; }
        public DbSet<PracticalSessionsEvent> PracticalSessionsEvents { get; set; }
        public DbSet<TalkEvent> TalkEvents { get; set; }
        public DbSet<ChatEvent> ChatEvents { get; set; }
        public DbSet<PartyEvent> PartyEvents { get; set; }
        public DbSet<FoodEvent> FoodEvents { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<AttendantSubscription> AttendantSubscriptions { get; set; }
        public DbSet<ExhibitorSubscription> ExhibitorSubscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Organizer", NormalizedName = "Organizer".ToUpper() },
                new IdentityRole { Name = "Exhibitor", NormalizedName = "Exhibitor".ToUpper() },
                new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() }
            );
        }
    }
}

/* Agregar en este archivo
 * public DbSet<Modelo> Modelos {get;set;}
 *
 * Crear Migracion
 * dotnet ef migrations add MigrationName
 *
 * Correr Migracion
 * dotnet ef database update
 *
 * Crear Controldor
 * dotnet aspnet-codegenerator controller -name ConferenceController -m Conference -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
 */
