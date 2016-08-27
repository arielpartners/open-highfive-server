#region references

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#endregion

namespace HighFive.Server.Api.Models
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
                Exception ex = new Exception("Email address for this user already exists in the database");
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
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        #endregion

        #region UpdateUser

        public void UpdateUser(HighFiveUser user)
        {
            _context.Update(user);
        }

        #endregion

        #region AddOrganization

        public void AddOrganization(Organization org)
        {
            _context.Add(org);
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
                .Where(o => o.Name == organizationName)
                .FirstOrDefault();
        }

        #endregion

        #region DeleteUser

        public void DeleteUser(HighFiveUser user)
        {
            _context.Users.Remove(user);
        }

        #endregion

        #region CorporateValue

        public void AddCorporateValue(CorporateValue corporateValueName)
        {
            CorporateValue corporateValue = GetCorporateValueByName(corporateValueName.Name);

            if (corporateValue != null)
            {
                Exception ex = new Exception("Corporate value already exists in the database");
                throw ex;
            }

            _context.Add(corporateValueName);
        }

        #endregion

        #region GetCorporateValueByName

        public CorporateValue GetCorporateValueByName(string corporateValueName)
        {
            return _context.CorporateValues
                .Where(o => o.Name == corporateValueName)
                .FirstOrDefault();
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
