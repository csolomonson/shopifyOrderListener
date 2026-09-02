using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSerialNumberDto
{
	[JsonProperty("imsAddedByUserID", Order = 1)]
	[Required(ErrorMessage = "imsAddedByUserID is required.")]
	[MaxLength(20)]
	public string imsAddedByUserID { get; set; }

	[JsonProperty("imsAddedDate", Order = 2)]
	[Required(ErrorMessage = "imsAddedDate is required.")]
	public DateTime? imsAddedDate { get; set; }

	[JsonProperty("imsSerialNumberID", Order = 3)]
	[Required(ErrorMessage = "imsSerialNumberID is required.")]
	[MaxLength(30)]
	public string imsSerialNumberID { get; set; }

	[JsonProperty("imsCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string imsCreatedBy { get; set; }

	[JsonProperty("imsCreatedDate", Order = 5)]
	public DateTime? imsCreatedDate { get; set; }

	[JsonProperty("imsUniqueID", Order = 6)]
	public Guid imsUniqueID { get; set; }

	[JsonProperty("imsExpirationDate", Order = 7)]
	public DateTime? imsExpirationDate { get; set; }

	[JsonProperty("imsInactiveDate", Order = 8)]
	public DateTime? imsInactiveDate { get; set; }

	[JsonProperty("imsInactive", Order = 9)]
	public bool imsInactive { get; set; }

	[JsonProperty("imsPartID", Order = 10)]
	[Required(ErrorMessage = "imsPartID is required.")]
	[MaxLength(30)]
	public string imsPartID { get; set; }

	[JsonProperty("imsPartRevisionID", Order = 11)]
	[MaxLength(15)]
	public string imsPartRevisionID { get; set; }

	[JsonProperty("imsRowVersion", Order = 12)]
	public byte[] imsRowVersion { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
