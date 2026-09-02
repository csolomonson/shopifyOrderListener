using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProjectedPaymentDto
{
	[JsonProperty("gloAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gloAmount { get; set; }

	[JsonProperty("gloClosedDate", Order = 2)]
	public DateTime? gloClosedDate { get; set; }

	[JsonProperty("gloCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string gloCreatedBy { get; set; }

	[JsonProperty("gloCreatedDate", Order = 4)]
	public DateTime? gloCreatedDate { get; set; }

	[JsonProperty("gloDescription", Order = 5)]
	[Required(ErrorMessage = "gloDescription is required.")]
	[MaxLength(50)]
	public string gloDescription { get; set; }

	[JsonProperty("gloUniqueID", Order = 6)]
	public Guid gloUniqueID { get; set; }

	[JsonProperty("gloIgnoreAfterDate", Order = 7)]
	public DateTime? gloIgnoreAfterDate { get; set; }

	[JsonProperty("gloClosed", Order = 8)]
	public bool gloClosed { get; set; }

	[JsonProperty("gloOrganizationID", Order = 9)]
	[MaxLength(10)]
	public string gloOrganizationID { get; set; }

	[JsonProperty("gloPaymentDate", Order = 10)]
	[Required(ErrorMessage = "gloPaymentDate is required.")]
	public DateTime? gloPaymentDate { get; set; }

	[JsonProperty("gloPaymentType", Order = 11)]
	[Required(ErrorMessage = "gloPaymentType is required.")]
	public byte gloPaymentType { get; set; }

	[JsonProperty("gloPlantDepartmentID", Order = 12)]
	[MaxLength(5)]
	public string gloPlantDepartmentID { get; set; }

	[JsonProperty("gloPlantID", Order = 13)]
	[MaxLength(5)]
	public string gloPlantID { get; set; }

	[JsonProperty("gloRowVersion", Order = 14)]
	public byte[] gloRowVersion { get; set; }

	[JsonProperty("gloProjectedPaymentID", Order = 15)]
	[Required(ErrorMessage = "gloProjectedPaymentID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int gloProjectedPaymentID { get; set; }

	[JsonProperty("customFields", Order = 16)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
