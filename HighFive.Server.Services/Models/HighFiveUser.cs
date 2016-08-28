using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Services.Models
{
    public class HighFiveUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Organization Organization { get; set; }
        public string ReportsTo { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }

    }
}
