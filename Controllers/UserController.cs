using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;

namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
       

        private readonly IUserRepository _userRepo;


        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("get-all")]
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        [HttpPost("create")]
        public Task<UserUpdateDto> CreateNewUser(UserUpdateDto payload)
        {
            return _userRepo.CreateNewUser(payload);
        }

        [HttpPatch("edit")]
        public Task<UserUpdateDto> EditUser(UserUpdateDto payload)
        {
            return _userRepo.EditUser(payload);
        }

        [HttpGet("{guid}")]
        public UserUpdateDto GetUserByGuid(string guid)
        {
            if (Guid.TryParse(guid, out Guid guidParsed)==false)
            {
                throw new Exception("GUID format is invalid");
            }

            return _userRepo.GetUserById(guidParsed);

        }

        [HttpDelete("delete/{guid}")]
        public bool DeleteUser(string guid)
        {
            if (Guid.TryParse(guid, out Guid guidParsed) == false)
            {
                throw new Exception("GUID format is invalid");
            }
            return _userRepo.DeleteUser(guidParsed);
        }


    }

}