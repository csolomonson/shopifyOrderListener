using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPServiceContractOwnerDto
{
	[JsonProperty("kboAddressLine1", Order = 1)]
	[MaxLength(50)]
	public string kboAddressLine1 { get; set; }

	[JsonProperty("kboAddressLine2", Order = 2)]
	[MaxLength(50)]
	public string kboAddressLine2 { get; set; }

	[JsonProperty("kboAddressLine3", Order = 3)]
	[MaxLength(50)]
	public string kboAddressLine3 { get; set; }

	[JsonProperty("kboCity", Order = 4)]
	[MaxLength(30)]
	public string kboCity { get; set; }

	[JsonProperty("kboCountry", Order = 5)]
	[MaxLength(20)]
	public string kboCountry { get; set; }

	[JsonProperty("kboCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string kboCreatedBy { get; set; }

	[JsonProperty("kboCreatedDate", Order = 7)]
	public DateTime? kboCreatedDate { get; set; }

	[JsonProperty("kboDeliveryDate", Order = 8)]
	public DateTime? kboDeliveryDate { get; set; }

	[JsonProperty("kboEmailAddress", Order = 9)]
	[MaxLength(50)]
	public string kboEmailAddress { get; set; }

	[JsonProperty("kboUniqueID", Order = 10)]
	public Guid kboUniqueID { get; set; }

	[JsonProperty("kboFaxNumber", Order = 11)]
	[MaxLength(20)]
	public string kboFaxNumber { get; set; }

	[JsonProperty("kboFirstName", Order = 12)]
	[MaxLength(30)]
	public string kboFirstName { get; set; }

	[JsonProperty("kboHomePhoneNumber", Order = 13)]
	[MaxLength(20)]
	public string kboHomePhoneNumber { get; set; }

	[JsonProperty("kboCurrentOwner", Order = 14)]
	public bool kboCurrentOwner { get; set; }

	[JsonProperty("kboSameAsAbove", Order = 15)]
	public bool kboSameAsAbove { get; set; }

	[JsonProperty("kboTermsAccepted", Order = 16)]
	public bool kboTermsAccepted { get; set; }

	[JsonProperty("kboLastName", Order = 17)]
	[MaxLength(20)]
	public string kboLastName { get; set; }

	[JsonProperty("kboMiddleName", Order = 18)]
	[MaxLength(20)]
	public string kboMiddleName { get; set; }

	[JsonProperty("kboMobileNumber", Order = 19)]
	[MaxLength(20)]
	public string kboMobileNumber { get; set; }

	[JsonProperty("kboOrganizationID", Order = 20)]
	[MaxLength(10)]
	public string kboOrganizationID { get; set; }

	[JsonProperty("kboPhysicalAddressLine1", Order = 21)]
	[MaxLength(50)]
	public string kboPhysicalAddressLine1 { get; set; }

	[JsonProperty("kboPhysicalAddressLine2", Order = 22)]
	[MaxLength(50)]
	public string kboPhysicalAddressLine2 { get; set; }

	[JsonProperty("kboPhysicalAddressLine3", Order = 23)]
	[MaxLength(50)]
	public string kboPhysicalAddressLine3 { get; set; }

	[JsonProperty("kboPhysicalCity", Order = 24)]
	[MaxLength(30)]
	public string kboPhysicalCity { get; set; }

	[JsonProperty("kboPhysicalCountry", Order = 25)]
	[MaxLength(20)]
	public string kboPhysicalCountry { get; set; }

	[JsonProperty("kboPhysicalLocationCity", Order = 26)]
	[MaxLength(30)]
	public string kboPhysicalLocationCity { get; set; }

	[JsonProperty("kboPhysicalLocationState", Order = 27)]
	[MaxLength(3)]
	public string kboPhysicalLocationState { get; set; }

	[JsonProperty("kboPhysicalPostCode", Order = 28)]
	[MaxLength(10)]
	public string kboPhysicalPostCode { get; set; }

	[JsonProperty("kboPhysicalState", Order = 29)]
	[MaxLength(3)]
	public string kboPhysicalState { get; set; }

	[JsonProperty("kboPostCode", Order = 30)]
	[MaxLength(10)]
	public string kboPostCode { get; set; }

	[JsonProperty("kboRegisteredDate", Order = 31)]
	public DateTime? kboRegisteredDate { get; set; }

	[JsonProperty("kboRowVersion", Order = 32)]
	public byte[] kboRowVersion { get; set; }

	[JsonProperty("kboServiceContractOwnerID", Order = 33)]
	[Required(ErrorMessage = "kboServiceContractOwnerID is required.")]
	public short kboServiceContractOwnerID { get; set; }

	[JsonProperty("kboServiceContractID", Order = 34)]
	[Required(ErrorMessage = "kboServiceContractID is required.")]
	[MaxLength(10)]
	public string kboServiceContractID { get; set; }

	[JsonProperty("kboStartDate", Order = 35)]
	public DateTime? kboStartDate { get; set; }

	[JsonProperty("kboState", Order = 36)]
	[MaxLength(3)]
	public string kboState { get; set; }

	[JsonProperty("kboWorkPhoneNumber", Order = 37)]
	[MaxLength(20)]
	public string kboWorkPhoneNumber { get; set; }

	[JsonProperty("customFields", Order = 38)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
