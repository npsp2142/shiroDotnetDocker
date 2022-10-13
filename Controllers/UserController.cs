using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Repositories;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersRepository _userRepository;

        public UserController(UsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }

        [HttpPost("/api/v1/user/register")]

        public async Task<ActionResult> AddUser([FromBody] UserDetails userDetails)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            Console.WriteLine("**** Start verification ****");
            if (userDetails.Username.Length < 3)
            {
                errors.Add("Username", "Username too short.");
            }
            if (userDetails.Password.Length < 3)
            {
                errors.Add("Password", "Password too short.");
            }
            if (errors.Count > 0)
            {
                var badRequest = new BadRequestResult();
                return badRequest;
            }
            try
            {
                await _userRepository.AddUserAsync(userDetails.Username, userDetails.Password);

                Console.WriteLine("Added new restaurant --- " + userDetails.ToJson());
                return new OkResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new BadRequestResult();
            }
        }
    }
}
