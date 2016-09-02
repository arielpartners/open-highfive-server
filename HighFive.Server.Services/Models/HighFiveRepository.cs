using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Utils;

namespace HighFive.Server.Services.Models
{
    public class HighFiveRepository : IHighFiveRepository
    {
        private readonly HighFiveContext _context;
        private readonly ILogger<HighFiveRepository> _logger;

        #region Constructor

        public HighFiveRepository(HighFiveContext context, ILogger<HighFiveRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Users

        public void AddUser(HighFiveUser user)
        {
            var highFiveUser = GetUserByEmail(user.Email);
            if (highFiveUser != null) throw new HighFiveException($"User {user.Email} already exists in the database");
            _context.Users.Add(user);
        }

        public IEnumerable<HighFiveUser> GetAllUsers()
        {
            _logger.LogInformation("Getting All Users from the Database");
            return _context.Users
                      .Include(u => u.Organization.Values)
                      .ToList();
        }

        public HighFiveUser GetUserByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Organization.Values)
                .FirstOrDefault(u => u.Email == email);
        }

        public void UpdateUser(HighFiveUser user)
        {
            _context.Update(user);
        }

        public void DeleteUser(HighFiveUser user)
        {
            _context.Users.Remove(user);
        }

        #endregion

        #region Organizations

        public void AddOrganization(Organization organization)
        {
            var org = GetOrganizationByName(organization.Name);
            if (org != null) throw new HighFiveException($"Organization {organization.Name} already exists in the database");
            _context.Organizations.Add(organization);
        }

        public IEnumerable<Organization> GetAllOrganizations()
        {
            _logger.LogInformation("Getting All Organizations from the Database");
            return _context.Organizations
                        .Include(o => o.Values)
                        .ToList();
        }

        public IEnumerable<Recognition> GetAllRecognitions()
        {
            _logger.LogInformation("Getting All Recognitions from the Database");
            return _context.Recognitions.ToList();
        }

        public void UpdateOrganization(Organization organization)
        {
            foreach (var cv in organization.Values)
            {
                var corpval = GetCorporateValueByNameAndDescription(cv.Name, cv.Description);
                if (corpval == null) AddCorporateValue(cv);
            }
            _context.Update(organization);
        }

        public void DeleteOrganization(Organization organization)
        {
            foreach (var cv in organization.Values)
            {
                DeleteCorporateValue(cv);
            }
            _context.Organizations.Remove(organization);
        }

        public Organization GetOrganizationByName(string organizationName)
        {
            return _context.Organizations
                .Include(o => o.Values)
                .FirstOrDefault(o => o.Name == organizationName);
        }

        #endregion

        #region CorporateValue

        public void AddCorporateValue(CorporateValue corporateValue)
        {
            var corpValue = GetCorporateValueByName(corporateValue.Name);
            if (corpValue != null) throw new HighFiveException($"Corporate value {corporateValue.Name} already exists in the database");
            _context.CorporateValues.Add(corporateValue);
        }

        public CorporateValue GetCorporateValueByName(string name)
        {
            return _context.CorporateValues
                .FirstOrDefault(o => o.Name == name);
        }

        public CorporateValue GetCorporateValueByNameAndDescription(string name, string description)
        {
            return _context.CorporateValues
                .FirstOrDefault(o => o.Name == name && o.Description == description);
        }

        public void UpdateCorporateValue(CorporateValue cv)
        {
            _context.CorporateValues.Update(cv);
        }

        public void DeleteCorporateValue(CorporateValue cv)
        {
            _context.CorporateValues.Remove(cv);
        }

        #endregion

        #region SaveChangesAsync

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        #endregion
    }
}
