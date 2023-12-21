using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Entities
{
    public class MSTR_UserType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Type { get; set; }

    }
}
