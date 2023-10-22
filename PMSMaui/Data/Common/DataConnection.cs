using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace PMSMaui.Data.Common
{
	public class DataConnection
	{
		public static bool ServerStatus(string URL)
		{
			if (string.IsNullOrEmpty(URL))
			{
				return false;
				throw new Exception("URL is empty");
			}
			else
			{
				try
				{
					HttpClient httpClient = new()
					{
						BaseAddress = new Uri(URL+ "/swagger/index.html")
					};
					HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress).Result;
					if (response.StatusCode == HttpStatusCode.OK)
					{
						return true;
					}
					else
					{
						return false;
						throw new Exception("Server is not responding");
					}
				}
				catch
				{
					return false;
					throw new Exception("Server Error");
				}
			}
		}
	}
}
