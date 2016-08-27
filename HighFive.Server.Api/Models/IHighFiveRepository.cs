using System.Collections.Generic;
using System.Threading.Tasks;

namespace HighFive.Server.Api.Models
{
    public interface IHighFiveRepository
    {
        IEnumerable<HighFiveUser> GetAllUsers();
        HighFiveUser GetUserByEmail(string email);
        Organization GetOrganizationByName(string email);
        CorporateValue GetCorporateValueByName(string name);

        void AddUser(HighFiveUser user);
        void UpdateUser(HighFiveUser user);
        void DeleteUser(HighFiveUser user);
        void AddOrganization(Organization organization);
        void AddCorporateValue(CorporateValue corporateValue);

        Task<bool> SaveChangesAsync(); //object GetTripsByUsername(string name);

        IEnumerable<Recognition> GetAllRecognitions();
    }
}