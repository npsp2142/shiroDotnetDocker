using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Requests;
using shiroDotnetRestfulDocker.Repositories;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class UserCredentialsController
    {
        private readonly UserCredentialsRepository _userCredentialsRepository;
        private readonly UserProfilesRepository _userProfilesRepository;
        public UserCredentialsController(
         UserCredentialsRepository userCredentialsRepository,
         UserProfilesRepository userProfilesRepository
         )
        {
            _userCredentialsRepository = userCredentialsRepository;
            _userProfilesRepository = userProfilesRepository;
        }

        [HttpPost("/api/v1/user/register")]

        public async Task<ActionResult> AddUser([FromBody] UserCredentialsAddRequest addRequest)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            Console.WriteLine("**** Start verification ****");
            if (addRequest.UserName.Length < 3)
            {
                errors.Add("Username", "Username too short.");
            }
            if (addRequest.Password.Length < 3)
            {
                errors.Add("Password", "Password too short.");
            }
            if (errors.Count > 0)
            {
                var badRequest = new BadRequestResult();
                return new JsonResult(new BadRequestObjectResult(addRequest));
            }
            try
            {
                // Add new user credentials
                var addUserResult = await _userCredentialsRepository.AddUserAsync(addRequest.UserName, addRequest.Password);
                Console.WriteLine("Added new user --- " + addUserResult.ToJson() + addUserResult.UserId);

                // Create new user profile
                var newUserProfile = new UserProfile();
                newUserProfile.Id = ObjectId.GenerateNewId();
                newUserProfile.UserId = addUserResult.UserId;
                newUserProfile.LastModifiedTime = DateTime.UtcNow;
                //Console.WriteLine("Added new newUserProfile --- " + newUserProfile.ToJson());
                var addUserProfileResult = await _userProfilesRepository.AddUserProfileAsync(newUserProfile);
                Console.WriteLine("Added new userProfile --- " + addUserProfileResult.ToJson());
                return new JsonResult(new OkObjectResult(addRequest));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new JsonResult(new BadRequestObjectResult(addRequest));
            }
        }


    }
}
