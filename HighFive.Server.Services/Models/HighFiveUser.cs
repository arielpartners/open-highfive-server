using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Services.Models
{
    public class HighFiveUser: IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Organization Organization { get; set; }
        [StringLength(100)]
        public string ReportsTo { get; set; }
        [StringLength(100)]
        public string Department { get; set; }
        [StringLength(100)]
        public string Location { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }

        public static string FirstNameFromName(string Name)
        {
            if (Name.IndexOf(" ") > 1)
            {
                return Name.Split()[0];
            }
            else
            {
                return Name;
            }
        }
    
        public static string LastNameFromName(string Name)
        {
            if (Name.IndexOf(" ") > 1)
            {
                return Name.Split()[1];
            }
            else
            {
                return Name;
            }
        }
    }
}
