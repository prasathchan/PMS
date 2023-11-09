using System.Text.Json;
using System.Security.Claims;
using PMS_Library.Models.CustomModel;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Cryptography.X509Certificates;

namespace PMSMaui.Data.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new (new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string getUserSessionFromStorage = await SecureStorage.Default.GetAsync("CustomAuthenticationModel");
                if (string.IsNullOrEmpty(getUserSessionFromStorage))
                {
                    return await Task.FromResult(new AuthenticationState(currentUser));
                }
                else
                {
                    var DeserializeUserSession = JsonSerializer.Deserialize<CustomAuthenticationModel>(getUserSessionFromStorage);
                    currentUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, DeserializeUserSession.EmailID!),
                        new Claim(ClaimTypes.Role, DeserializeUserSession.Role!),
                    }, "CustomAuth"));
                   
                    return await Task.FromResult(new AuthenticationState(currentUser));
                }
            }
            catch
            {
               return await Task.FromResult(new AuthenticationState(currentUser));
            }
        }

        public async Task UpdateAuthenticationState(CustomAuthenticationModel userSession)
        {
            if (!string.IsNullOrEmpty(userSession.EmailID))
            {
                string serializeUserSession = JsonSerializer.Serialize(userSession);
                await SecureStorage.Default.SetAsync("CustomAuthenticationModel", serializeUserSession);
                currentUser = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Name, userSession.EmailID), new(ClaimTypes.Role, userSession.Role) }, "jwtAuthType"));
            }
            else
            {
                SecureStorage.Default.Remove("CustomAuthenticationModel");
                currentUser = new(new ClaimsIdentity());
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
        }

        public string GetName()
        {
			return currentUser.Identity?.Name;
		}
 

    }
}
