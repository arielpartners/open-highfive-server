using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Utils;
using System;

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
            try
            {
                return _context.Organizations
                    .Include(o => o.Values)
                    .FirstOrDefault(o => o.Name.Equals(organizationName, StringComparison.OrdinalIgnoreCase) || o.WebPath.Equals(organizationName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting OrganizationByName " + organizationName + " Error: " + ex.Message);
                return null;
            }
        }

        #endregion

        #region CorporateValue

        public IEnumerable<CorporateValue> GetCorporateValuesByOrganizationName(string organizationName)
        {
            var organization = _context.Organizations
                .Include(o => o.Values)
                .FirstOrDefault(o => o.Name.Equals(organizationName, StringComparison.OrdinalIgnoreCase) || o.WebPath.Equals(organizationName, StringComparison.OrdinalIgnoreCase));
            return organization?.Values.ToList() ?? new List<CorporateValue>();
        }

        public void AddCorporateValue(CorporateValue corporateValue)
        {
            var corpValue = GetCorporateValueByName(corporateValue.Name);
            if (corpValue != null) throw new HighFiveException($"Corporate value {corporateValue.Name} already exists in the database");
            _context.CorporateValues.Add(corporateValue);
        }

        public CorporateValue GetCorporateValueByName(string corporateValueName)
        {
            return _context.CorporateValues
                .FirstOrDefault(o => o.Name == corporateValueName);
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

        #region Recognitions

        public Recognition GetRecognitionById(int id)
        {
            _logger.LogInformation("Getting Recognition");
            return _context.Recognitions
                    .Include(s => s.Sender)
                    .Include(r => r.Receiver)
                    .Include(o => o.Organization)
                    .Include(cv => cv.Value)
                    .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Recognition> GetRecognitionsByReceiver(string receiverName)
        {
            return _context.Recognitions
                    .Include(s => s.Sender)
                    .Include(r => r.Receiver)
                    .Include(o => o.Organization)
                    .Include(cv => cv.Value)
                    .Where(w => w.Receiver.Email.Equals(receiverName, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(x => x.DateCreated).Take(20).ToList();
        }

        public IEnumerable<Recognition> GetRecognitionsBySender(string senderName)
        {
            return _context.Recognitions
                    .Include(s => s.Sender)
                    .Include(r => r.Receiver)
                    .Include(o => o.Organization)
                    .Include(cv => cv.Value)
                    .Where(w => w.Sender.Email.Equals(senderName, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(x => x.DateCreated).Take(20).ToList();
        }

        public IEnumerable<Recognition> GetAllRecognitions()
        {
            _logger.LogInformation("Getting All Recognitions");
            return _context.Recognitions
                    .Include(s => s.Sender)
                    .Include(r => r.Receiver)
                    .Include(o => o.Organization)
                    .Include(cv => cv.Value)
                    .OrderByDescending(x => x.DateCreated).Take(20).ToList();
        }

        public void AddRecognition(Recognition recognition)
        {
            _context.Recognitions.Add(recognition);
        }

        public void UpdateRecognition(Recognition recognition)
        {
            _context.Update(recognition);
        }

        public void DeleteRecognition(Recognition recognition)
        {
            _context.Recognitions.Remove(recognition);
        }

        #endregion

        #region Metrics

        public IEnumerable<GroupedMetric> GetMetrics(string organizationName, int numberOfDaysToGoBack)
        {
            _logger.LogInformation("Getting All Metrics");
            var metricsList = new List<GroupedMetric>();
            var organizationCorpoateValues = GetCorporateValuesByOrganizationName(organizationName).ToList();
            if (!organizationCorpoateValues.Any()) return metricsList;

            var weekago = DateTime.UtcNow.AddDays(-numberOfDaysToGoBack);
            var query = from r in _context.Recognitions
                        where (r.Organization.Name.Equals(organizationName, StringComparison.OrdinalIgnoreCase) || r.Organization.WebPath.Equals(organizationName, StringComparison.OrdinalIgnoreCase)) &&
                              r.DateCreated > weekago
                        group r by r.Value.Name into m
                        select new GroupedMetric
                        {
                            CorporateValue = m.Key,
                            TotalCount = m.Count(),
                            Points = m.Sum(r => r.Points)
                        };
            metricsList = organizationCorpoateValues
                                .OrderBy(a => a.Name)
                                .Select(a => new GroupedMetric { CorporateValue = a.Name, TotalCount = 0, Points = 0 }).ToList();
            if (!metricsList.Any()) return metricsList;
            foreach (var groupedMetric in query)
            {
                var metricItem = metricsList.FirstOrDefault(x => x.CorporateValue == groupedMetric.CorporateValue);
                if (metricItem == null) continue;
                metricItem.TotalCount = groupedMetric.TotalCount;
                metricItem.Points = groupedMetric.Points;
            }
            return metricsList.ToList();
        }

        #endregion

        public async Task IsConnected()
        {
            await _context.Database.OpenConnectionAsync();
            _context.Database.CloseConnection();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
