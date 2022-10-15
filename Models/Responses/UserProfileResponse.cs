namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class UserProfileResponse
    {
        private UserProfileResponse()
        {

        }
        public static UserProfileResponse Of(Exception exception)
        {
            UserProfileResponse response = new UserProfileResponse();
            response.exception = exception;
            return response;
        }
        public static UserProfileResponse Of(UserProfile userProfile)
        {
            UserProfileResponse response = new UserProfileResponse();
            response.UserProfile = userProfile;
            return response;
        }
        public Exception exception { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
