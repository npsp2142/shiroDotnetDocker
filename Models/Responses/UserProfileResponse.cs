namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class UserProfileResponse
    {
        public UserProfileResponse(UserProfile profile)
        {
            Success = true;
            UserProfile = profile;
        }

        public UserProfileResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public UserProfile? UserProfile { get; set; }
    }
}
