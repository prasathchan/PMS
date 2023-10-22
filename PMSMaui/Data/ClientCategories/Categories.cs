using PMSMaui.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSMaui.Data.ClientCategories
{
	public class Categories
	{
		public static RestResponse FetchCategories()
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
				var request = new RestRequest("/api/ClientCategories", Method.Get);
				response = client.ExecuteGet(request);
			}
			catch (Exception ex)
			{
				response.Content = ex.Message;
			}
			return response;
		}

		public static RestResponse FetchCategoryByID(string CatID)
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
				var request = new RestRequest("/api/ClientCategories/"+CatID, Method.Get);
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
