using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace highfive_server.Models
{
    public class HighFiveRepository : IHighFiveRepository
    {
        private HighFiveContext _context;
        private ILogger<HighFiveRepository> _logger;

        public HighFiveRepository(HighFiveContext context, ILogger<HighFiveRepository> logger)
        {
          _context = context;
          _logger = logger;
        }

        public void AddUser(HighFiveUser user)
        {
            HighFiveUser highFiveUser = GetUserByEmail(user.Email);

            if(highFiveUser != null)
            {
                Exception ex = new Exception("Email address for this user already exists in the database");
                throw ex;
            }
            
            _context.Add(user);
        }

        public void DeleteUser(HighFiveUser user)
        {
                _context.Users.Remove(user);
        }

        public void AddOrganization(Organization org)
        {
            _context.Add(org);
        }

        public IEnumerable<HighFiveUser> GetAllUsers()
        {
            _logger.LogInformation("Getting All Users from the Database");
            return _context.Users
                      .Include(u => u.Organization.Values)
                      .ToList();
        }

        public IEnumerable<Recognition> GetAllRecognitions()
        {
            _logger.LogInformation("Getting All Recognitions from the Database");
            return _context.Recognitions.ToList();
        }

        public HighFiveUser GetUserByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Organization.Values)
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        public void UpdateUser(HighFiveUser user)
        {
            _context.Update(user);
        }

        public Organization GetOrganizationByName(string organizationName)
        {
            return _context.Organizations
                .Where(o => o.Name == organizationName)
                .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
          return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
