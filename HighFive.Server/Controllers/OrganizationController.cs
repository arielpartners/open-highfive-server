#region references

using System;
using AutoMapper;
using HighFive.Server.Models;
using HighFive.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#endregion

namespace HighFive.Server.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationsController : Controller
    {
        private IHighFiveRepository _repository;
        private ILogger<OrganizationsController> _logger;

        #region OrganizationsController

        public OrganizationsController(IHighFiveRepository repository, ILogger<OrganizationsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region POST api/organizations - Post([FromBody]OrganizationViewModel newOrganization)

        // POST api/organizations
        [HttpPost]
        public IActionResult Post([FromBody]OrganizationViewModel newOrganization)
        {
            if (ModelState.IsValid)
            {
                // create organization in repo
                try
                {
                    var theNewOrganization = Mapper.Map<Organization>(newOrganization);
                    //CorporateValue corporateValue = _repository.GetCorporateValueByName(newOrganization.CorporateValueName);
                    //if (corporateValue == null)
                    //{
                    //    return NotFound(new { Message = $"Unable to find corporateValue {newOrganization.Name}" });
                    //}
                    //else
                    //{
                    //    theNewOrganization.Name = newOrganization.Name;
                    //}

                    _repository.AddOrganization(theNewOrganization);
                    _repository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to add new organization: {0}", ex);
                    return BadRequest(new { Message = $"Failed to add new organization {newOrganization.Name}" });
                }

                return Created($"api/users/{newOrganization.Name}", newOrganization);
            }
            else
            {
                return BadRequest(ModelState);
            }

            #endregion

        }
    }
}
