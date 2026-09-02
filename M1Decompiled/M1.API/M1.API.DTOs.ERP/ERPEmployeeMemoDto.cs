using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeMemoDto
{
	[JsonProperty("lmkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string lmkCreatedBy { get; set; }

	[JsonProperty("lmkCreatedDate", Order = 2)]
	public DateTime? lmkCreatedDate { get; set; }

	[JsonProperty("lmkEmployeeID", Order = 3)]
	[Required(ErrorMessage = "lmkEmployeeID is required.")]
	[MaxLength(10)]
	public string lmkEmployeeID { get; set; }

	[JsonProperty("lmkUniqueID", Order = 4)]
	public Guid lmkUniqueID { get; set; }

	[JsonProperty("lmkLongDescriptionRtf", Order = 5)]
	public string lmkLongDescriptionRtf { get; set; }

	[JsonProperty("lmkLongDescriptionText", Order = 6)]
	public string lmkLongDescriptionText { get; set; }

	[JsonProperty("lmkMemoDate", Order = 7)]
	[Required(ErrorMessage = "lmkMemoDate is required.")]
	public DateTime? lmkMemoDate { get; set; }

	[JsonProperty("lmkRowVersion", Order = 8)]
	public byte[] lmkRowVersion { get; set; }

	[JsonProperty("lmkEmployeeMemoID", Order = 9)]
	[Required(ErrorMessage = "lmkEmployeeMemoID is required.")]
	public short lmkEmployeeMemoID { get; set; }

	[JsonProperty("lmkShortDescription", Order = 10)]
	[Required(ErrorMessage = "lmkShortDescription is required.")]
	[MaxLength(50)]
	public string lmkShortDescription { get; set; }

	[JsonProperty("lmkShowInEmployees", Order = 11)]
	public bool lmkShowInEmployees { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
