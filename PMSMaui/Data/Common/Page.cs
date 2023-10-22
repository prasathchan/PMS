using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJSRuntime = Microsoft.JSInterop.IJSRuntime;



namespace PMSMaui.Data.Common
{
	public class Page
	{
		private static bool Isreadonly { get; set; } = true;

		//static void ReloadPage(this NavigationManager manager)
		//{
		//	manager.NavigateTo(manager.Uri, true);
		//}

		//private static void Print()
		//{
		//	IJSRuntime JSRuntime = null;
		//	JSRuntime.InvokeVoidAsync("printJS", "table", "html");
		//}

		public static Dictionary<string, object> SetReadOnly()
		{
			var dict = new Dictionary<string, object>();
			if (Isreadonly) dict.Add("readonly", "readonly");
			return dict;
		}
	}
}
