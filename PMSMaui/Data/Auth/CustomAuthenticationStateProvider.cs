using System.Security.Claims;
using PMS_Library.Models.CustomModel;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using PMS_Library.Models.DataModel;


namespace PMSMaui.Data.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
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
                        new Claim(ClaimTypes.Name, DeserializeUserSession.EmailID!),
                        new Claim(ClaimTypes.Role, DeserializeUserSession.Role!),
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
                await SecureStorage.Default.SetAsync("CustomAuthenticationModel", serializeUserSession);

                  claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Name, userSession.EmailID), new(ClaimTypes.Role, userSession.Role) }, "jwtAuthType"));
            }
            else
            {
                SecureStorage.Default.Remove("CustomAuthenticationModel");
                claimsPrincipal = anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

    }
}
