using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using WebApplicationTest.Models;

namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {


        private readonly ICompanyRepository _companyRepo;


        public CompanyController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        [HttpGet("get-all")]
        public IEnumerable<CompanyModel> GetAllUsers()
        {
            return _companyRepo.GetAllCompanies();
        }

        [HttpPost("create")]
        public Task<CompanyModel> CreateCompany(CompanyDTO payload)
        {
            return _companyRepo.CreateCompany(payload);
        }

        [HttpPatch("edit")]
        public Task<CompanyModel> EditCompany(CompanyDTO payload)
        {
            return _companyRepo.EditCompany(payload);
        }

        [HttpGet("{guid}")]
        public CompanyModel GetCompanyByGuid(string guid)
        {
            if (Guid.TryParse(guid, out Guid guidParsed) == false)
            {
                throw new Exception("GUID format is invalid");
            }

            return _companyRepo.GetCompanyById(guidParsed);

        }

        [HttpDelete("delete/{guid}")]
        public bool DeleteUser(string guid)
        {
            if (Guid.TryParse(guid, out Guid guidParsed) == false)
            {
                throw new Exception("GUID format is invalid");
            }
            return _companyRepo.DeleteCompany(guidParsed);
        }
    }

}