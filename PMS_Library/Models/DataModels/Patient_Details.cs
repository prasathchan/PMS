//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS_Library.Models.DataModels
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public partial class Patient_Details
    {
        [Key]
        public string PatientID { get; set; }
        public System.DateTime Date { get; set; }
        public string PatientName { get; set; }
        public string HospitalID { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public string ProofMediaID { get; set; }
    }
}
