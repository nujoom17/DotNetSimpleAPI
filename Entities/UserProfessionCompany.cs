using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplicationTest.Entities
{
    public class UserProfessionCompany
    {
        public int UserId { get; set; }
        public int ProfessionId { get; set; }
        public int CompanyId { get; set; }

        public User User { get; set; }
        public MSTR_Profession Profession { get; set; }
        public Company Company { get; set; }
        public List<UserProfessionCompany> UserProfessionCompanies { get; set; }

    }
}
