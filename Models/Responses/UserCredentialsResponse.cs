namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class UserCredentialsResponse
    {
        private UserCredentialsResponse()
        {

        }
        public static UserCredentialsResponse Of(string userId = Constants.EmptyString, string userName = Constants.EmptyString)
        {
            UserCredentialsResponse response = new UserCredentialsResponse();
            response.UserId = userId;
            response.UserName = userName;
            return response;
        }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
