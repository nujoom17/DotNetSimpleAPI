using AutoMapper;
using WebApplicationTest.Entities;
using WebApplicationTest.Models;


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
                UserType = user.UserType?.Type
                // Other properties in UserModel are ignored
            };

            return userModel;
        }

    }

    public class CompanyMapping
    {

        public CompanyModel MapCompanyToCompanyModel(Company company)
        {
            // Manual mapping from entity to model
            var companyModel = new CompanyModel
            {
                Name= company.Name,
                UserProfessionCompanies = company.UserProfessionCompanies,
                Guid = company.Guid,
                LocationCoords = company.Location.coordinates,
                LocationName = company.Location.Name,
                
            };

            return companyModel;
        }

    }

}