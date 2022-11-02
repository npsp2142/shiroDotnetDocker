using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Requests;
using shiroDotnetRestfulDocker.Repositories;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserCredentialsRepository _userCredentialsRepository;
        private readonly UserProfilesRepository _userProfilesRepository;
        private readonly FoodOrdersRepository _foodOrdersRepository;
        private readonly RestaurantsRepository _restaurantsRepository;

        public UserProfileController(
            UserCredentialsRepository userCredentialsRepository,
            UserProfilesRepository userProfilesRepository,
        FoodOrdersRepository foodOrdersRepository,
            RestaurantsRepository restaurantsRepository
            )
        {
            _userCredentialsRepository = userCredentialsRepository;
            _userProfilesRepository = userProfilesRepository;
            _foodOrdersRepository = foodOrdersRepository;
            _restaurantsRepository = restaurantsRepository;
        }

        [HttpPost("/api/v1/user/order/add")]

        public async Task<ActionResult> AddFoodOrder([FromBody] FoodOrderAddRequest addRequest)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            Console.WriteLine("**** Start verification ****");
            Console.WriteLine("order add called", addRequest.ToString());
            var restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(ObjectId.Parse(addRequest.RestaurantId));

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
                var badRequest = new BadRequestObjectResult(errors);
                return new JsonResult(badRequest);
            }
            try
            {
                var foodOrder = new FoodOrder();
                Console.WriteLine("order add called", addRequest.RestaurantId);
                foodOrder.RestaurantId = ObjectId.Parse(addRequest.RestaurantId);
                foodOrder.UserId = addRequest.UserId;
                foodOrder.NameTc = addRequest.NameTc;
                foodOrder.NameEn = addRequest.NameEn;
                foodOrder.Description = addRequest.Description;
                foodOrder.ShoppingBasket = addRequest.ShoppingBasket.Select(request =>
                {
                    Food food = new Food();
                    //food.Id = ObjectId.GenerateNewId();
                    food.NameTc = request.NameTc;
                    food.NameEn = request.NameEn;
                    food.Description = request.Description;
                    food.Price = request.Price;
                    food.Remarks = request.Remarks;
                    return food;
                }).ToList();
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
                var badRequest = new BadRequestObjectResult(exception);
                return new JsonResult(exception);
            }
        }

        [HttpGet("/api/v1/user/profile")]
        public async Task<ActionResult> GetUserProfile(string userId)
        {
            try
            {
                var profileResponse = await _userProfilesRepository.GetUserProfileAsync(userId);
                return new JsonResult(profileResponse);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                var badRequest = new BadRequestObjectResult(exception);
                return new JsonResult(exception);
            }
        }
    }
}
