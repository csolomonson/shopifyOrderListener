using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWorkCenterMachineDto
{
	[JsonProperty("xaqDescription", Order = 1)]
	[Required(ErrorMessage = "xaqDescription is required.")]
	[MaxLength(50)]
	public string xaqDescription { get; set; }

	[JsonProperty("xaqUniqueID", Order = 2)]
	public Guid xaqUniqueID { get; set; }

	[JsonProperty("xaqRowVersion", Order = 3)]
	public byte[] xaqRowVersion { get; set; }

	[JsonProperty("xaqWorkCenterMachineID", Order = 4)]
	[Required(ErrorMessage = "xaqWorkCenterMachineID is required.")]
	public short xaqWorkCenterMachineID { get; set; }

	[JsonProperty("xaqWorkCenterID", Order = 5)]
	[Required(ErrorMessage = "xaqWorkCenterID is required.")]
	[MaxLength(5)]
	public string xaqWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 6)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
