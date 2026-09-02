using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLotNumberDto
{
	[JsonProperty("ablAddedByUserID", Order = 1)]
	[Required(ErrorMessage = "ablAddedByUserID is required.")]
	[MaxLength(20)]
	public string ablAddedByUserID { get; set; }

	[JsonProperty("ablAddedDate", Order = 2)]
	[Required(ErrorMessage = "ablAddedDate is required.")]
	public DateTime? ablAddedDate { get; set; }

	[JsonProperty("ablLotNumberID", Order = 3)]
	[Required(ErrorMessage = "ablLotNumberID is required.")]
	[MaxLength(30)]
	public string ablLotNumberID { get; set; }

	[JsonProperty("ablCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string ablCreatedBy { get; set; }

	[JsonProperty("ablCreatedDate", Order = 5)]
	public DateTime? ablCreatedDate { get; set; }

	[JsonProperty("ablUniqueID", Order = 6)]
	public Guid ablUniqueID { get; set; }

	[JsonProperty("ablExpirationDate", Order = 7)]
	public DateTime? ablExpirationDate { get; set; }

	[JsonProperty("ablInactiveDate", Order = 8)]
	public DateTime? ablInactiveDate { get; set; }

	[JsonProperty("ablInactive", Order = 9)]
	public bool ablInactive { get; set; }

	[JsonProperty("ablPartID", Order = 10)]
	[MaxLength(30)]
	public string ablPartID { get; set; }

	[JsonProperty("ablPartRevisionID", Order = 11)]
	[MaxLength(15)]
	public string ablPartRevisionID { get; set; }

	[JsonProperty("ablRowVersion", Order = 12)]
	public byte[] ablRowVersion { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
