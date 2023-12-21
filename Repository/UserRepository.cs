// Interface
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebApplicationTest.Mappings;


// Repository
public class UserRepository : IUserRepository
{
    private readonly DbContextMain _context;
    public UserMapping _userMapping;

    public UserRepository(DbContextMain context, UserMapping userMapping)
    {
        _context = context;
        _userMapping = userMapping;
    }

    public IEnumerable<UserModel> GetAllUsers()
    {
        var userModels = _context.Users.Include(u => u.UserType).Select(userEntity => _userMapping.MapUserToUserModel(userEntity)).ToList();

        return userModels;
    }

    public UserUpdateDto GetUserById(Guid guid)
    {
        var user = _context.Users.FirstOrDefault(x => x.Guid == guid);

        if(user == null)
        {
            throw new Exception("User profile not found");
        }

        var userData = new UserUpdateDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            Nationality = user.Nationality,
            Guid = guid,
            UserType = user.UserType?.Type?.Trim()
        };

        return userData;
    }

    public async Task<UserUpdateDto> EditUser(UserUpdateDto payloadData)
    {
        var existingUser = _context.Users.FirstOrDefault(x=>x.Guid==payloadData.Guid);

        if (existingUser == null || payloadData?.Guid == null)
        {
            throw new Exception("The user corresponding to the unique identifier is non-existent"); 
        }

        foreach (PropertyInfo property in payloadData.GetType().GetProperties())
        {
            if(property.PropertyType == typeof(Guid))
            {
                continue;
            }
            else
            {
                var newValue = property.GetValue(payloadData);

                // Update the property if the new value is not null
                if (newValue != null)
                {
                    var existingProperty = existingUser.GetType().GetProperty(property.Name);
                    existingProperty?.SetValue(existingUser, newValue);
                }
            }
        }
     
        existingUser.UpdatedDate = DateTime.UtcNow;

        // Save changes to the database
        await _context.SaveChangesAsync();

        return GetUserById((Guid)existingUser.Guid);
    }


        public async Task<UserUpdateDto> CreateNewUser(UserUpdateDto payloadData)
    {

        string key =  $"genTemplateUser@{_context.Users.Count()}";

        // Generate a new GUID with the key
        Guid? newGuid = null;
        User userExisting = _context.Users.FirstOrDefault(x => x.Guid == payloadData.Guid);


        if (userExisting is not null)
        {
            newGuid = (Guid)payloadData.Guid;
        }
        else
        {
           newGuid = GenerateGuidWithKey(key);
        }
     

        User newUser = new User
        {
            FirstName = payloadData.FirstName?.Trim(), // Assuming FullName contains both first and last name
            LastName = payloadData.LastName?.Trim(),
            Gender = payloadData.Gender,
            DateOfBirth = payloadData.DateOfBirth,
            Nationality = payloadData.Nationality,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Guid = newGuid
        };
        if (userExisting is not null)
        {
            newUser.Id = userExisting.Id;
            _context.Entry(userExisting).State = EntityState.Detached;
            newUser.Guid = GenerateGuidWithKey(key);
            _context.Users.Update(newUser);
        }
        else
        {
            _context.Users.Add(newUser);
        }

        return GetUserById((Guid)newGuid);

    }

    private Guid GenerateGuidWithKey(string key)
    {
        // Use key to influence GUID generation
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        Guid guidWithKey = Guid.NewGuid();
        byte[] guidBytes = guidWithKey.ToByteArray();

        for (int i = 0; i < keyBytes.Length; i++)
        {
            guidBytes[i % 16] ^= keyBytes[i];
        }

        return new Guid(guidBytes);
    }


    public bool DeleteUser(Guid guidParsed)
    {
        var user = _context.Users.FirstOrDefault(x=>x.Guid==guidParsed);

        if (user == null)
        {
            return false;
        }

        // Remove the user from the database
        _context.Users.Remove(user);
        _context.SaveChanges();

        return true;
    }

    // Implement other CRUD operations
}


public interface IUserRepository
{
    Task<UserUpdateDto> CreateNewUser(UserUpdateDto payloadData);
    bool DeleteUser(Guid guidParsed);
    Task<UserUpdateDto> EditUser(UserUpdateDto payload);
    IEnumerable<UserModel> GetAllUsers();
    UserUpdateDto GetUserById(Guid guid);
}