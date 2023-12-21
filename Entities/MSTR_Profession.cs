using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Entities
{
    public class MSTR_Profession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        
        public string? Name { get; set; }
        public List<UserProfessionCompany> UserProfessionCompanies { get; set; }

    }
}
