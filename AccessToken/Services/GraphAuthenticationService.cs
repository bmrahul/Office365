using Microsoft.Identity.Client;
using System.Security;
using System.Threading.Tasks;

namespace AccessToken.Services
{
    public class GraphAuthenticationService
    {
        protected TokenAcquisitionHelper tokenAcquisitionHelper;
        public GraphAuthenticationService(IPublicClientApplication app)
        {
            tokenAcquisitionHelper = new TokenAcquisitionHelper(app);
        }

        private static string[] Scopes { get; set; } = new string[] { "User.Read", "User.ReadBasic.All" };

        public async Task<AuthenticationResult> AquireToken(string username, string password)
        {
            AuthenticationResult authenticationResult = await tokenAcquisitionHelper.AcquireATokenFromUsernamePasswordAsync(Scopes, username, ConvertToSecureString(password));
            return authenticationResult;
        }

        private SecureString ConvertToSecureString(string secret)
        {
            SecureString password = new SecureString();
            char[] _secret = secret.ToCharArray();
            foreach (char ch in _secret)
            {
                password.AppendChar(ch);
            }
            return password;
        }
    }
}