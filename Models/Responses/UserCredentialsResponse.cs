using Newtonsoft.Json;

namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class UserCredentialsResponse
    {
        private UserCredentialsResponse()
        {

        }

        public UserCredentialsResponse(UserCredentials credentials)
        {
            Success = true;
            UserCredentials = credentials;
        }

        public UserCredentialsResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        [JsonProperty("info")]
        public UserCredentials? UserCredentials { get; set; }
    }
}
