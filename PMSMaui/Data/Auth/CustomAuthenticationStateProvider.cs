using System.Security.Claims;
using PMS_Library.Models.CustomModel;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;


namespace PMSMaui.Data.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string getUserSessionFromStorage = await SecureStorage.Default.GetAsync("CustomAuthenticationModel");
                if (string.IsNullOrEmpty(getUserSessionFromStorage))
                {
                    return await Task.FromResult(new AuthenticationState(anonymous));
                }
                else
                {
                    var DeserializeUserSession = JsonSerializer.Deserialize<CustomAuthenticationModel>(getUserSessionFromStorage);
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, DeserializeUserSession.EmailID!) 
                    }, "CustomAuth"));
                    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
                }

            }
            catch
            {
               return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async Task UpdateAuthenticationState(CustomAuthenticationModel userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (!string.IsNullOrEmpty(userSession.EmailID))
            {
                string serializeUserSession = JsonSerializer.Serialize(userSession);
                await SecureStorage.Default.SetAsync("UserSession", serializeUserSession);

                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.EmailID!),
                }));
            }
            else
            {
                SecureStorage.Default.Remove("UserSession");
                claimsPrincipal = anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

    }
}
