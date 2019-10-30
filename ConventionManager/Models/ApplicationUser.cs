using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        // UserName and Phone number already in IdentityUser class
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public byte[] Curriculum { get; set; }

        public List<int> SuscribedEvents { get; set; }
        public List<int> SuscribedConferences { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
