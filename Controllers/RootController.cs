using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;

namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("root")]
    public class RootController : ControllerBase
    {
       

        private readonly IRootRepository _rootRepo;


        public RootController(IRootRepository rootRepo)
        {
            _rootRepo = rootRepo;
        }

        [HttpGet("get-all-users")]
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _rootRepo.GetAllUsers();
        }

        [HttpPost("create-user")]
        public Task<IEnumerable<UserModel>> CreateNewUser(UserModel payload)
        {
            return _rootRepo.CreateNewUser(payload);
        }

        [HttpPatch("edit-user")]
        public Task<UserUpdateDto> EditUser(UserUpdateDto payload)
        {
            return _rootRepo.EditUser(payload);
        }

        [HttpGet("user/{guid}")]
        public UserUpdateDto GetUserByGuid(string guid)
        {
            if (Guid.TryParse(guid, out Guid guidParsed)==false)
            {
                throw new Exception("GUID format is invalid");
            }

            return _rootRepo.GetUserById(guidParsed);

        }

        [HttpDelete("delete-user/{guid}")]
        public bool DeleteUser(string guid)
        {
            if (Guid.TryParse(guid, out Guid guidParsed) == false)
            {
                throw new Exception("GUID format is invalid");
            }
            return _rootRepo.DeleteUser(guidParsed);
        }


    }

}