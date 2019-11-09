using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        // UserName and Phone number already in IdentityUser class
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CurriculumUrl { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
