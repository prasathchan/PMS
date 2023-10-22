using Microsoft.AspNetCore.Components;
using PMS_Library.Models.DataModel;
using PMSMaui.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMSMaui.Data.ClientDetails
{
	public class Clients
	{
		//Fetch All Client Details
		public static RestResponse FetchAllClients()
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
				var request = new RestRequest("/api/clients", Method.Get);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

		//Fetch Specific Client Details
		public static RestResponse FetchSpecificClient(string clientname)
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
				var request = new RestRequest("/api/clients/" + clientname, Method.Get);
				request.AddUrlSegment("clientname", clientname);
				response = client.ExecuteGet(request);

			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}


		//Fetch Client Details By Name and Category
		public static RestResponse FetchSpecificClientwithCategory(string cname, string ccat)
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
				var request = new RestRequest("/api/clients/" + @cname + "/" + @ccat, Method.Get);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

		//OnSaveButtonClicked
		public static RestResponse UpdateClient(string cname, string email, string phone, string ccat)
		{
			RestResponse response;
			try
			{
				var baseURL = DeviceInfo.Platform == DevicePlatform.Android ? Resources.apk_baseURL : Resources.win_baseURL;
				var options = new RestClientOptions(baseURL)
				{
					MaxTimeout = -1,
				};
				var client = new RestClient(options);
				var body = new Client_Details()
				{
					ClientName = cname,
					Phone = phone,
					EmailID = email,
					Category = ccat,
				};
				var request = new RestRequest("/api/clients/update", Method.Put);
				request.AddHeader("Content-Type", "application/json");
				request.AddJsonBody(body);
				response = client.ExecutePut(request);
			}
			catch (Exception ex)
			{
				response = new()
				{
					ErrorException = ex.InnerException
				};
			}
			return response;
		}

		//Post CLient
		public static RestResponse AddClient(string clientname, string emailid, string phone, string category)
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
				var request = new RestRequest("/api/clients/add", Method.Post);
				request.AddJsonBody(new { clientname, category, phone, emailid });
				response = client.ExecutePost(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}


		//Delete Client
		public static RestResponse DeleteClient(string clientname)
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
				var request = new RestRequest("/api/clients/" + clientname, Method.Delete);
				request.AddUrlSegment("clientname", clientname);
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