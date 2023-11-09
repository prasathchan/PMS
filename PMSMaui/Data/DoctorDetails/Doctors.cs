using RestSharp;
using PMSMaui.Properties;
using PMS_Library.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS_Library.Models.CustomModel;

namespace PMSMaui.Data.DoctorDetails
{
	public class Doctors
	{
		public static RestResponse FetchDoctors()
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
				var request = new RestRequest("/api/doctors", Method.Get);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

		public static RestResponse FetchDoctorByName(string name)
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
				var request = new RestRequest("/api/doctors/{name}", Method.Get);
				request.AddUrlSegment("name", name);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

        public static RestResponse FetchDoctorByEmail(string email)
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
                var request = new RestRequest("/api/doctors/email/{email}", Method.Get);
                request.AddUrlSegment("email", email);
                response = client.ExecuteGet(request);
            }
            catch (Exception ex)
            {
                response.Content = ex.Message;
            }
            return response;
        }

		public static RestResponse FetchDoctorsByHospitalId(string hospitalid)
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
				var request = new RestRequest("/api/doctors/hospital/"+hospitalid, Method.Get);
				request.AddUrlSegment("HospitalID", hospitalid);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}


		public static RestResponse PostDoctor(Doctor_Details doctor_Details)
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
				var request = new RestRequest("/api/doctors/add", Method.Post);
				request.AddJsonBody(doctor_Details);
				response = client.ExecutePost(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

		public static RestResponse DeleteDoctor(string name, string email)
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
				var request = new RestRequest("/api/doctors/delete", Method.Delete);
				var body = new DeleteDoctor
				{
					DoctorName = name,
					EmailAddress = email
				};
				request.AddJsonBody(body);
				response = client.ExecuteDelete(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;

		}
	}
}
