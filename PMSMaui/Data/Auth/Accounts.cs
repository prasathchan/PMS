using RestSharp;
using PMSMaui.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;


namespace PMSMaui.Data.Auth
{
    public class Accounts
    {
        public static RestResponse Login(string username, string password)
        {
            RestResponse response = new();
            try
            {
                var baseURL = DeviceInfo.Platform == DevicePlatform.Android ? Resources.apk_baseURL : Resources.win_baseURL;
                var options = new RestClientOptions(baseURL)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/Auth/Login", Method.Post);
                request.AddJsonBody(new { username, password });
                response = client.ExecutePost(request);
            }
            catch (Exception ex)
            {
                response.Content = ex.Message;
            }
            return response;
        }
    }
}
