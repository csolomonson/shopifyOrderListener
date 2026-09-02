using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPServiceContractLineDto
{
	[JsonProperty("kbnContractLength", Order = 1)]
	public short kbnContractLength { get; set; }

	[JsonProperty("kbnContractLengthType", Order = 2)]
	[MaxLength(1)]
	public string kbnContractLengthType { get; set; }

	[JsonProperty("kbnCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string kbnCreatedBy { get; set; }

	[JsonProperty("kbnCreatedDate", Order = 4)]
	public DateTime? kbnCreatedDate { get; set; }

	[JsonProperty("kbnEndDate", Order = 5)]
	public DateTime? kbnEndDate { get; set; }

	[JsonProperty("kbnUniqueID", Order = 6)]
	public Guid kbnUniqueID { get; set; }

	[JsonProperty("kbnPartID", Order = 7)]
	[Required(ErrorMessage = "kbnPartID is required.")]
	[MaxLength(30)]
	public string kbnPartID { get; set; }

	[JsonProperty("kbnPartRevisionID", Order = 8)]
	[MaxLength(15)]
	public string kbnPartRevisionID { get; set; }

	[JsonProperty("kbnPartShortDescription", Order = 9)]
	[Required(ErrorMessage = "kbnPartShortDescription is required.")]
	[MaxLength(50)]
	public string kbnPartShortDescription { get; set; }

	[JsonProperty("kbnRowVersion", Order = 10)]
	public byte[] kbnRowVersion { get; set; }

	[JsonProperty("kbnServiceContractLineID", Order = 11)]
	[Required(ErrorMessage = "kbnServiceContractLineID is required.")]
	public short kbnServiceContractLineID { get; set; }

	[JsonProperty("kbnSerialNumberID", Order = 12)]
	[MaxLength(30)]
	public string kbnSerialNumberID { get; set; }

	[JsonProperty("kbnServiceContractID", Order = 13)]
	[Required(ErrorMessage = "kbnServiceContractID is required.")]
	[MaxLength(10)]
	public string kbnServiceContractID { get; set; }

	[JsonProperty("kbnStartDate", Order = 14)]
	public DateTime? kbnStartDate { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
