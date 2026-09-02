using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationContactGroupLinkDto
{
	[JsonProperty("cmrContactGroupID", Order = 1)]
	[Required(ErrorMessage = "cmrContactGroupID is required.")]
	[MaxLength(5)]
	public string cmrContactGroupID { get; set; }

	[JsonProperty("cmrContactGroupLinkID", Order = 2)]
	public short cmrContactGroupLinkID { get; set; }

	[JsonProperty("cmrContactID", Order = 3)]
	[Required(ErrorMessage = "cmrContactID is required.")]
	[MaxLength(5)]
	public string cmrContactID { get; set; }

	[JsonProperty("cmrCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string cmrCreatedBy { get; set; }

	[JsonProperty("cmrCreatedDate", Order = 5)]
	public DateTime? cmrCreatedDate { get; set; }

	[JsonProperty("cmrUniqueID", Order = 6)]
	public Guid cmrUniqueID { get; set; }

	[JsonProperty("cmrLocationID", Order = 7)]
	[MaxLength(5)]
	public string cmrLocationID { get; set; }

	[JsonProperty("cmrOrganizationID", Order = 8)]
	[Required(ErrorMessage = "cmrOrganizationID is required.")]
	[MaxLength(10)]
	public string cmrOrganizationID { get; set; }

	[JsonProperty("cmrRowVersion", Order = 9)]
	public byte[] cmrRowVersion { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
