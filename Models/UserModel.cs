using System;
using System.ComponentModel.DataAnnotations.Schema;

public class UserModel
{
    public string? FullName { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? NationalityPrimary { get; set; }

    public Guid? Guid { get; set; }

    public string? UserType { get; set; }


}