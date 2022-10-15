using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Requests;
using shiroDotnetRestfulDocker.Repositories;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersRepository _userRepository;
        private readonly FoodOrdersRepository _foodOrdersRepository;
        private readonly RestaurantsRepository _restaurantsRepository;

        public UserController(
            UsersRepository usersRepository,
            FoodOrdersRepository foodOrdersRepository,
            RestaurantsRepository restaurantsRepository
            )
        {
            _userRepository = usersRepository;
            _foodOrdersRepository = foodOrdersRepository;
            _restaurantsRepository = restaurantsRepository;
        }

        [HttpPost("/api/v1/user/register")]

        public async Task<ActionResult> AddUser([FromBody] UserCredentials userCredentials)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            Console.WriteLine("**** Start verification ****");
            if (userCredentials.UserName.Length < 3)
            {
                errors.Add("Username", "Username too short.");
            }
            if (userCredentials.Password.Length < 3)
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
                await _userRepository.AddUserAsync(userCredentials.UserName, userCredentials.Password);

                Console.WriteLine("Added new restaurant --- " + userCredentials.ToJson());
                return new OkResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new BadRequestResult();
            }
        }

        [HttpPost("/api/v1/user/order/add")]

        public async Task<ActionResult> AddFoodOrder([FromBody] FoodOrderAddRequest addRequest)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            Console.WriteLine("**** Start verification ****");

            var restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(addRequest.RestaurantId);

            if (restaurant == null)
            {
                errors.Add("RestaurantId", "Restaurant not exist");
            }
            if (addRequest.NameEn.Length < 3)
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
                var foodOrder = new FoodOrder();
                foodOrder.RestaurantId = new ObjectId(addRequest.RestaurantId);
                foodOrder.UserName = addRequest.UserName;
                foodOrder.NameTc = addRequest.NameTc;
                foodOrder.NameEn = addRequest.NameEn;
                foodOrder.Description = addRequest.Description;
                foodOrder.ShoppingBasket = addRequest.ShoppingBasket;
                foodOrder.Remarks = addRequest.Remarks;
                foodOrder.Status = FoodOrder.OrderStatus.Pending.ToString();
                foodOrder.LastModified = DateTime.UtcNow;
                foodOrder.CreationTime = DateTime.UtcNow;
                await _foodOrdersRepository.AddFoodOrderAsync(foodOrder);

                Console.WriteLine("Added new food order --- " + addRequest.ToJson());
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
