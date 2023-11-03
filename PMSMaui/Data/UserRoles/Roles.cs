using RestSharp;
using PMSMaui.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSMaui.Data.UserRoles
{
    public class Roles
    {
        public static RestResponse FetchUserRoles()
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
                var request = new RestRequest("/api/userroles", Method.Get);
                response = client.ExecuteGet(request);
            }
            catch (Exception ex)
            {
                response.Content = ex.Message;
            }
            return response;

        }
    }
}
