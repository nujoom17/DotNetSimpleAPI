using AutoMapper;


namespace WebApplicationTest.Mappings
{
    public class UserMapping
    {

        public UserModel MapUserToUserModel(User user)
        {
            // Manual mapping from entity to model
            var userModel = new UserModel
            {
                FullName = (user?.FirstName + " " +  user?.LastName)?.Trim(),
                DateOfBirth = user?.DateOfBirth,
                NationalityPrimary = user.Nationality,
                Gender = user.Gender,
                Guid = user.Guid,
                // Other properties in UserModel are ignored
            };

            return userModel;
        }
    }
 
}