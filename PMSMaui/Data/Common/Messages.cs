using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSMaui.Data.Common
{
	public class Messages
	{
		//Display Error Alert
		public static async void DisplayErrorAlert(string Data)
		{
			await Application.Current.MainPage.DisplayAlert("Error", Data, "OK");
		}

		//Display Success Alert
		public static async void DisplaySuccessAlert(string Data)
		{
			await Application.Current.MainPage.DisplayAlert("Success", Data, "OK");
		}
	}
}
