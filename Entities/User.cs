using System.ComponentModel.DataAnnotations.Schema;
using WebApplicationTest.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Nationality { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? Guid { get; set; }

    public int? UserTypeId { get; set; }
    public MSTR_UserType UserType { get; set; }

    public List<UserProfessionCompany> UserProfessionCompanies { get; set; }

}