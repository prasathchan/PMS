using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Library.Models.CustomModel
{
	public class CreateAdminAccount
	{
		public string ClientName { get; set; }
		public string EmailID { get; set; }

		public string Phone { get; set; }

		public string Category { get; set; }
		public string Password { get; set; }
	}
}
