using Microsoft.Extensions.Configuration;

namespace TaskScheduler
{
    public class StaticAuthentication
    {
        private string ClientSecret { get; set; }
        public StaticAuthentication(IConfiguration secret)
        {
            ClientSecret = secret["clientSecret"];
        }

        public bool IsValid(string token)
        {
            return !string.IsNullOrWhiteSpace(ClientSecret) && token == ClientSecret;
        }
    }
}