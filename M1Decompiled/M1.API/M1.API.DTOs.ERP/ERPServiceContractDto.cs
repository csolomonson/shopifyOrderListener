using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPServiceContractDto
{
	[JsonProperty("kbsServiceContractID", Order = 1)]
	[Required(ErrorMessage = "kbsServiceContractID is required.")]
	[MaxLength(10)]
	public string kbsServiceContractID { get; set; }

	[JsonProperty("kbsContractAmount", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbsContractAmount { get; set; }

	[JsonProperty("kbsContractLength", Order = 3)]
	public short kbsContractLength { get; set; }

	[JsonProperty("kbsContractLengthType", Order = 4)]
	[MaxLength(1)]
	public string kbsContractLengthType { get; set; }

	[JsonProperty("kbsCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string kbsCreatedBy { get; set; }

	[JsonProperty("kbsCreatedDate", Order = 6)]
	public DateTime? kbsCreatedDate { get; set; }

	[JsonProperty("kbsDescription", Order = 7)]
	[Required(ErrorMessage = "kbsDescription is required.")]
	[MaxLength(50)]
	public string kbsDescription { get; set; }

	[JsonProperty("kbsEndDate", Order = 8)]
	[Required(ErrorMessage = "kbsEndDate is required.")]
	public DateTime? kbsEndDate { get; set; }

	[JsonProperty("kbsUniqueID", Order = 9)]
	public Guid kbsUniqueID { get; set; }

	[JsonProperty("kbsLongDescriptionRtf", Order = 10)]
	public string kbsLongDescriptionRtf { get; set; }

	[JsonProperty("kbsLongDescriptionText", Order = 11)]
	public string kbsLongDescriptionText { get; set; }

	[JsonProperty("kbsOrganizationID", Order = 12)]
	[Required(ErrorMessage = "kbsOrganizationID is required.")]
	[MaxLength(10)]
	public string kbsOrganizationID { get; set; }

	[JsonProperty("kbsPartID", Order = 13)]
	[MaxLength(30)]
	public string kbsPartID { get; set; }

	[JsonProperty("kbsPartRevisionID", Order = 14)]
	[MaxLength(15)]
	public string kbsPartRevisionID { get; set; }

	[JsonProperty("kbsPartShortDescription", Order = 15)]
	[MaxLength(50)]
	public string kbsPartShortDescription { get; set; }

	[JsonProperty("kbsProjectAreaID", Order = 16)]
	[MaxLength(15)]
	public string kbsProjectAreaID { get; set; }

	[JsonProperty("kbsProjectID", Order = 17)]
	[MaxLength(10)]
	public string kbsProjectID { get; set; }

	[JsonProperty("kbsResellerOrganizationID", Order = 18)]
	[MaxLength(10)]
	public string kbsResellerOrganizationID { get; set; }

	[JsonProperty("kbsRowVersion", Order = 19)]
	public byte[] kbsRowVersion { get; set; }

	[JsonProperty("kbsSerialNumberID", Order = 20)]
	[MaxLength(30)]
	public string kbsSerialNumberID { get; set; }

	[JsonProperty("kbsServiceContractTypeID", Order = 21)]
	[MaxLength(5)]
	public string kbsServiceContractTypeID { get; set; }

	[JsonProperty("kbsStartDate", Order = 22)]
	[Required(ErrorMessage = "kbsStartDate is required.")]
	public DateTime? kbsStartDate { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
