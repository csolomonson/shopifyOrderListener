using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartClassPlantDto
{
	[JsonProperty("imfPartClassPlantID", Order = 1)]
	[Required(ErrorMessage = "imfPartClassPlantID is required.")]
	[MaxLength(5)]
	public string imfPartClassPlantID { get; set; }

	[JsonProperty("imfCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imfCreatedBy { get; set; }

	[JsonProperty("imfCreatedDate", Order = 3)]
	public DateTime? imfCreatedDate { get; set; }

	[JsonProperty("imfUniqueID", Order = 4)]
	public Guid imfUniqueID { get; set; }

	[JsonProperty("imfInventoryGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string imfInventoryGlAccountID { get; set; }

	[JsonProperty("imfInvInInspectionGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string imfInvInInspectionGlAccountID { get; set; }

	[JsonProperty("imfInvInTransferGlAccountID", Order = 7)]
	[MaxLength(11)]
	public string imfInvInTransferGlAccountID { get; set; }

	[JsonProperty("imfInvToReturnGlAccountID", Order = 8)]
	[MaxLength(11)]
	public string imfInvToReturnGlAccountID { get; set; }

	[JsonProperty("imfPartClassID", Order = 9)]
	[Required(ErrorMessage = "imfPartClassID is required.")]
	[MaxLength(5)]
	public string imfPartClassID { get; set; }

	[JsonProperty("imfRowVersion", Order = 10)]
	public byte[] imfRowVersion { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
