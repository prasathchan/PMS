using RestSharp;
using PMSMaui.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public static RestResponse PostDoctors(PMS_Library.Models.DataModel.Doctor_Details doctor_Details)
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
	}
}
