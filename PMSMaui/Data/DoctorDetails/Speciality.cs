using PMSMaui.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSMaui.Data.DoctorDetails
{
    public class Speciality
    {
        public static RestResponse FetchDoctorSpecialty()
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
                var request = new RestRequest("/api/doctors/specialty", Method.Get);
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
