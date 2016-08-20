﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace highfive_server.Models
{
    public interface IHighFiveRepository
    {
        IEnumerable<HighFiveUser> GetAllUsers();
        HighFiveUser GetUserByEmail(string email);

        void AddUser(HighFiveUser user);
        void AddOrganization(Organization organization);

        Task<bool> SaveChangesAsync(); //object GetTripsByUsername(string name);

        IEnumerable<Recognition> GetAllRecognitions();
    }
}