using System;
using System.Collections.Generic;
using System.Text;
using ConventionManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConventionManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}

//dotnet ef migrations add MigrationName
//dotnet ef database update
//dotnet aspnet-codegenerator controller -name ConferenceController -m Conference -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries