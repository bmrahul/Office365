using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Globalization;
using System.IO;

namespace AccessToken.Services
{
    public class AuthenticationConfig
    {
        public PublicClientApplicationOptions PublicClientApplicationOptions { get; set; }
        public string MicrosoftGraphBaseEndpoint { get; set; }
        public static string ClientId { get; } = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
        public static string ClientSecret { get; } = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
        public static string RedirectUri { get; } = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
        public static string AADInstance { get; } = System.Configuration.ConfigurationManager.AppSettings["AADInstance"];

        public static string Authority = string.Format(CultureInfo.InvariantCulture, AADInstance, "common", "/v2.0");

        public static AuthenticationConfig ReadFromJsonFile()
        {
            // .NET configuration
            IConfigurationRoot Configuration;

            //This full path to be used in production
            //string fullPath = Path.GetFullPath("appsettings.json");

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().AddJsonFile(@"C:/Users/1027756/source/repos/AccessToken/AccessToken/appsettings.json");

            Configuration = builder.Build();

            // Read the auth and graph endpoint config
            AuthenticationConfig config = new AuthenticationConfig()
            {
                PublicClientApplicationOptions = new PublicClientApplicationOptions()
            };
            Configuration.Bind("Authentication", config.PublicClientApplicationOptions);
            config.MicrosoftGraphBaseEndpoint = Configuration.GetValue<string>("WebAPI:MicrosoftGraphBaseEndpoint");
            return config;
        }
    }
}