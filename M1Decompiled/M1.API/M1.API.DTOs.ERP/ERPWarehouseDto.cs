using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseDto
{
	[JsonProperty("imwAddressLine1", Order = 1)]
	[MaxLength(50)]
	public string imwAddressLine1 { get; set; }

	[JsonProperty("imwAddressLine2", Order = 2)]
	[MaxLength(50)]
	public string imwAddressLine2 { get; set; }

	[JsonProperty("imwAddressLine3", Order = 3)]
	[MaxLength(50)]
	public string imwAddressLine3 { get; set; }

	[JsonProperty("imwCity", Order = 4)]
	[MaxLength(30)]
	public string imwCity { get; set; }

	[JsonProperty("imwWarehouseID", Order = 5)]
	[Required(ErrorMessage = "imwWarehouseID is required.")]
	[MaxLength(5)]
	public string imwWarehouseID { get; set; }

	[JsonProperty("imwCountry", Order = 6)]
	[MaxLength(20)]
	public string imwCountry { get; set; }

	[JsonProperty("imwCreatedBy", Order = 7)]
	[MaxLength(20)]
	public string imwCreatedBy { get; set; }

	[JsonProperty("imwCreatedDate", Order = 8)]
	public DateTime? imwCreatedDate { get; set; }

	[JsonProperty("imwDefaultBinCount", Order = 9)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imwDefaultBinCount { get; set; }

	[JsonProperty("imwEmailAddress", Order = 10)]
	[MaxLength(50)]
	public string imwEmailAddress { get; set; }

	[JsonProperty("imwUniqueID", Order = 11)]
	public Guid imwUniqueID { get; set; }

	[JsonProperty("imwEstablishedDate", Order = 12)]
	public DateTime? imwEstablishedDate { get; set; }

	[JsonProperty("imwFaxNumber", Order = 13)]
	[MaxLength(20)]
	public string imwFaxNumber { get; set; }

	[JsonProperty("imwInactiveDate", Order = 14)]
	public DateTime? imwInactiveDate { get; set; }

	[JsonProperty("imwInactive", Order = 15)]
	public bool imwInactive { get; set; }

	[JsonProperty("imwAvalaraAddressValidated", Order = 16)]
	public bool imwAvalaraAddressValidated { get; set; }

	[JsonProperty("imwDefaultWarehouse", Order = 17)]
	public bool imwDefaultWarehouse { get; set; }

	[JsonProperty("imwDoNotIncludeInJobCosts", Order = 18)]
	public bool imwDoNotIncludeInJobCosts { get; set; }

	[JsonProperty("imwNonNettable", Order = 19)]
	public bool imwNonNettable { get; set; }

	[JsonProperty("imwName", Order = 20)]
	[Required(ErrorMessage = "imwName is required.")]
	[MaxLength(50)]
	public string imwName { get; set; }

	[JsonProperty("imwNonNettableType", Order = 21)]
	public byte imwNonNettableType { get; set; }

	[JsonProperty("imwPhoneNumber", Order = 22)]
	[MaxLength(20)]
	public string imwPhoneNumber { get; set; }

	[JsonProperty("imwPlantDepartmentID", Order = 23)]
	[MaxLength(5)]
	public string imwPlantDepartmentID { get; set; }

	[JsonProperty("imwPlantID", Order = 24)]
	[MaxLength(5)]
	public string imwPlantID { get; set; }

	[JsonProperty("imwPostCode", Order = 25)]
	[MaxLength(10)]
	public string imwPostCode { get; set; }

	[JsonProperty("imwRowVersion", Order = 26)]
	public byte[] imwRowVersion { get; set; }

	[JsonProperty("imwShippingMethodID", Order = 27)]
	[MaxLength(5)]
	public string imwShippingMethodID { get; set; }

	[JsonProperty("imwState", Order = 28)]
	[MaxLength(3)]
	public string imwState { get; set; }

	[JsonProperty("customFields", Order = 29)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
