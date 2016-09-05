using System.Collections.Generic;
using System.Threading.Tasks;

namespace HighFive.Server.Services.Models
{
    public interface IHighFiveRepository
    {
        #region User
        Task<bool> SaveChangesAsync(); //object GetTripsByUsername(string name);
        IEnumerable<HighFiveUser> GetAllUsers();
        HighFiveUser GetUserByEmail(string email);
        void AddUser(HighFiveUser user);
        void UpdateUser(HighFiveUser user);
        void DeleteUser(HighFiveUser user);
        #endregion

        #region CorporateValue
        CorporateValue GetCorporateValueByName(string corporateValueName);
        void AddCorporateValue(CorporateValue corporateValue);
        CorporateValue GetCorporateValueByNameAndDescription(string name, string description);
        void UpdateCorporateValue(CorporateValue cv);
        void DeleteCorporateValue(CorporateValue cv);
        #endregion

        #region Recognition
        Recognition GetRecognitionById(int id);
        IEnumerable<Recognition> GetAllRecognitions();
        void AddRecognition(Recognition recognition);
        void UpdateRecognition(Recognition recognition);
        void DeleteRecognition(Recognition recognition);
        #endregion

        #region Comment



        #endregion

        #region Organization
        void AddOrganization(Organization organization);
        IEnumerable<Organization> GetAllOrganizations();
        Organization GetOrganizationByName(string organizationName);
        void UpdateOrganization(Organization organization);
        void DeleteOrganization(Organization organization);
        #endregion

        Task IsConnected();

        int SaveChanges();
    }
}