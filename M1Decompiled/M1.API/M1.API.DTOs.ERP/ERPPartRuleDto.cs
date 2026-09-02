using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartRuleDto
{
	[JsonProperty("pcrCode", Order = 1)]
	[MaxLength(50)]
	public string pcrCode { get; set; }

	[JsonProperty("pcrCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string pcrCreatedBy { get; set; }

	[JsonProperty("pcrCreatedDate", Order = 3)]
	public DateTime? pcrCreatedDate { get; set; }

	[JsonProperty("pcrUniqueID", Order = 4)]
	public Guid pcrUniqueID { get; set; }

	[JsonProperty("pcrField", Order = 5)]
	[Required(ErrorMessage = "pcrField is required.")]
	[MaxLength(30)]
	public string pcrField { get; set; }

	[JsonProperty("pcrMethodAssemblyID", Order = 6)]
	public int pcrMethodAssemblyID { get; set; }

	[JsonProperty("pcrMethodID", Order = 7)]
	[Required(ErrorMessage = "pcrMethodID is required.")]
	[MaxLength(30)]
	public string pcrMethodID { get; set; }

	[JsonProperty("pcrMethodMaterialID", Order = 8)]
	public int pcrMethodMaterialID { get; set; }

	[JsonProperty("pcrMethodOperationID", Order = 9)]
	public int pcrMethodOperationID { get; set; }

	[JsonProperty("pcrMethodRevisionID", Order = 10)]
	[MaxLength(15)]
	public string pcrMethodRevisionID { get; set; }

	[JsonProperty("pcrMethodType", Order = 11)]
	[Required(ErrorMessage = "pcrMethodType is required.")]
	public byte pcrMethodType { get; set; }

	[JsonProperty("pcrProcessSequence", Order = 12)]
	public short pcrProcessSequence { get; set; }

	[JsonProperty("pcrRowVersion", Order = 13)]
	public byte[] pcrRowVersion { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
