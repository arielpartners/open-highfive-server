#region references

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Utils;

#endregion

namespace HighFive.Server.Services.Models
{
    public class HighFiveRepository : IHighFiveRepository
    {
        private HighFiveContext _context;
        private ILogger<HighFiveRepository> _logger;

        #region Constructor

        public HighFiveRepository(HighFiveContext context, ILogger<HighFiveRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region AddUser

        public void AddUser(HighFiveUser user)
        {
            HighFiveUser highFiveUser = GetUserByEmail(user.Email);

            if (highFiveUser != null)
            {
                HighFiveException ex = new HighFiveException($"User {user.Email} already exists in the database");
                throw ex;
            }

            _context.Add(user);
        }

        #endregion

        #region GetAllUsers

        public IEnumerable<HighFiveUser> GetAllUsers()
        {
            _logger.LogInformation("Getting All Users from the Database");
            return _context.Users
                      .Include(u => u.Organization.Values)
                      .ToList();
        }

        #endregion

        #region GetUserByEmail

        public HighFiveUser GetUserByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Organization.Values)
                .FirstOrDefault(u => u.Email == email);
        }

        #endregion

        #region UpdateUser

        public void UpdateUser(HighFiveUser user)
        {
            _context.Update(user);
        }

        #endregion

        #region AddOrganization

        public void AddOrganization(Organization organization)
        {
            _context.Add(organization);
        }

        #endregion

        #region GetAllRecognitions

        public IEnumerable<Recognition> GetAllRecognitions()
        {
            _logger.LogInformation("Getting All Recognitions from the Database");
            return _context.Recognitions.ToList();
        }

        #endregion

        #region GetOrganizationByName

        public Organization GetOrganizationByName(string organizationName)
        {
            return _context.Organizations
                .FirstOrDefault(o => o.Name == organizationName);
        }

        #endregion

        #region DeleteUser

        public void DeleteUser(HighFiveUser user)
        {
            _context.Users.Remove(user);
        }

        #endregion

        #region CorporateValue

        public void AddCorporateValue(CorporateValue corporateValue)
        {
            var value = GetCorporateValueByName(corporateValue.Name);

            if (value != null)
            {
                throw new HighFiveException("Corporate value already exists in the database");
            }

            _context.Add(value);
        }

        #endregion

        #region GetCorporateValueByName

        public CorporateValue GetCorporateValueByName(string name)
        {
            return _context.CorporateValues
                .FirstOrDefault(o => o.Name == name);
        }

        #endregion

        #region SaveChangesAsync

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        #endregion
    }
}
