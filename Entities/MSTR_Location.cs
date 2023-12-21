using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Entities
{
    public class MSTR_Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string? coordinates { get; set; }

        public string? Name { get; set; }

    }
}
