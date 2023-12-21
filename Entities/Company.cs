using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Entities
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public Guid? Guid { get; set; }


        public string? Name { get; set; }    

        public int LocationId { get; set; }
        public MSTR_Location Location { get; set; }


        public List<UserProfessionCompany> UserProfessionCompanies { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
