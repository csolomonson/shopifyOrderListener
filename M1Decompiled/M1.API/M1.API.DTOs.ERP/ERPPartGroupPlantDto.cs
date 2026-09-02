using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartGroupPlantDto
{
	[JsonProperty("imvArDepositGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string imvArDepositGlAccountID { get; set; }

	[JsonProperty("imvPartGroupPlantID", Order = 2)]
	[Required(ErrorMessage = "imvPartGroupPlantID is required.")]
	[MaxLength(5)]
	public string imvPartGroupPlantID { get; set; }

	[JsonProperty("imvCogsLaborGlAccountID", Order = 3)]
	[MaxLength(11)]
	public string imvCogsLaborGlAccountID { get; set; }

	[JsonProperty("imvCogsMaterialGlAccountID", Order = 4)]
	[MaxLength(11)]
	public string imvCogsMaterialGlAccountID { get; set; }

	[JsonProperty("imvCogsOverheadGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string imvCogsOverheadGlAccountID { get; set; }

	[JsonProperty("imvCogsSubcontractGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string imvCogsSubcontractGlAccountID { get; set; }

	[JsonProperty("imvCreatedBy", Order = 7)]
	[MaxLength(20)]
	public string imvCreatedBy { get; set; }

	[JsonProperty("imvCreatedDate", Order = 8)]
	public DateTime? imvCreatedDate { get; set; }

	[JsonProperty("imvDiscountGlAccountID", Order = 9)]
	[MaxLength(11)]
	public string imvDiscountGlAccountID { get; set; }

	[JsonProperty("imvUniqueID", Order = 10)]
	public Guid imvUniqueID { get; set; }

	[JsonProperty("imvPartGroupID", Order = 11)]
	[Required(ErrorMessage = "imvPartGroupID is required.")]
	[MaxLength(5)]
	public string imvPartGroupID { get; set; }

	[JsonProperty("imvRowVersion", Order = 12)]
	public byte[] imvRowVersion { get; set; }

	[JsonProperty("imvSalesGlAccountID", Order = 13)]
	[MaxLength(11)]
	public string imvSalesGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
