using WebApplicationTest.Entities;

namespace WebApplicationTest.Models
{
    public class CompanyModel
    {
        public string? Name { get; set; }

        public string? LocationCoords { get; set; }

        public string? LocationName { get; set; }

        public Guid? Guid { get; set; }

        public List<UserProfessionCompany> UserProfessionCompanies { get; set; }


    }
}
