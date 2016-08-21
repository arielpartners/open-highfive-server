using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace highfive_server.Models
{
    public class HighFiveContext: DbContext
    {
        private IConfigurationRoot _config;

        public HighFiveContext(IConfigurationRoot config, DbContextOptions<HighFiveContext> options) 
            : base(options)
        {
            _config = config;
        }

        public DbSet<HighFiveUser> Users { get; set; }
        public DbSet<Organization> Organizations { get;set;}
        public DbSet<Recognition> Recognitions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                // DBContext for unit tests will already be pre-configured
                optionsBuilder.UseSqlServer(_config["ConnectionStrings:HighFiveContextConnection"]);
            }
        }
    }
}
