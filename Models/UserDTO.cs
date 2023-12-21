using WebApplicationTest.Entities;

public class UserUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Nationality { get; set; }

    public Guid? Guid { get; set; }

    public string? UserType { get; set; }
}