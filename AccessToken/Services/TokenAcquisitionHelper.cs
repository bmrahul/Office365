using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;

namespace AccessToken.Services
{
    public class TokenAcquisitionHelper
    {
        protected IPublicClientApplication App { get; private set; }
        public TokenAcquisitionHelper(IPublicClientApplication app)
        {
            App = app;
        }

        public async Task<AuthenticationResult> AcquireATokenFromUsernamePasswordAsync(IEnumerable<String> scopes, string username, SecureString password)
        {
            AuthenticationResult result = null;
            var accounts = await App.GetAccountsAsync();

            if (accounts.Any())
            {
                try
                {
                    result = await (App as PublicClientApplication).AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    // No token for the account. Will proceed below
                }
            }

            if (result == null)
            {
                result = await GetTokenUsingUsernamePasswordAsync(scopes, username, password);
            }

            return result;
        }

        private async Task<AuthenticationResult> GetTokenUsingUsernamePasswordAsync(IEnumerable<string> scopes, string username, SecureString password)
        {
            AuthenticationResult result = null;
            try
            {
                result = await App.AcquireTokenByUsernamePassword(scopes, username, password)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                throw new Exception("Error: {0}", ex);
            }
            catch (MsalServiceException ex) when (ex.ErrorCode == "invalid_request")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            catch (MsalServiceException ex) when (ex.ErrorCode == "unauthorized_client")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            catch (MsalServiceException ex) when (ex.ErrorCode == "invalid_client")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user_type")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            catch (MsalClientException ex) when (ex.ErrorCode == "user_realm_discovery_failed")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            catch (MsalClientException ex) when (ex.ErrorCode == "parsing_wstrust_response_failed")
            {
                throw new ArgumentException("U/P: Wrong username", ex);
            }
            return result;
        }
    }
}