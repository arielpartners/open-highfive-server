using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HighFive.Server.Models
{
    public class HighFiveContext : IdentityDbContext<HighFiveUser>
    {
        private IConfigurationRoot _config;

        public HighFiveContext(IConfigurationRoot config, DbContextOptions<HighFiveContext> options)
            : base(options)
        {
            _config = config;
        }

        public DbSet<HighFiveUser> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Recognition> Recognitions { get; set; }
        public DbSet<CorporateValue> CorporateValues { get; set; }
    }
}
