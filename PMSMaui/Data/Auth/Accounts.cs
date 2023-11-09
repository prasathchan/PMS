using RestSharp;
using PMSMaui.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using PMS_Library.Models.DataModel;
using PMS_Library.Models.CustomModel;
using System.Text.Json;


namespace PMSMaui.Data.Auth
{
    public class Accounts
    {
        public static RestResponse Login(LoginModel lm)
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
                var request = new RestRequest("/api/Auth", Method.Post);
                var body = JsonSerializer.Serialize(lm);
                request.AddStringBody(body, DataFormat.Json);
                response = client.ExecutePost(request);
            }
            catch (Exception ex)
            {
                response.Content = ex.Message;
            }
            return response;
        }

        public static RestResponse Register(User_Auth user)
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
                var request = new RestRequest("/api/Auth/Register", Method.Post);
                var body = JsonSerializer.Serialize(user);
                request.AddStringBody(body, DataFormat.Json);
                response = client.ExecutePost(request);
            }
            catch (Exception ex)
            {
                response.Content = ex.Message;
            }
            return response;
        }

		public static RestResponse GetByEmail(string email)
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
				var request = new RestRequest("/api/Auth/" + email, Method.Get);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

        public static RestResponse GetAuthDetails()
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
				var request = new RestRequest("/api/Auth", Method.Get);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
            {
				response.Content = ex.Message;
			}
			return response;
        }

        public static RestResponse DeleteAuthDetails(string email)
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
                var request = new RestRequest("api/User/Delete/" + email, Method.Delete);
                response = client.ExecuteDelete(request);
            }
            catch (Exception ex)
            {
                response.Content = ex.Message;
            }
            return response;
        }

        public static RestResponse RevokeUserToken(string email)
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
                var request = new RestRequest("api/Auth/Revoke/"+ email, Method.Post);
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
