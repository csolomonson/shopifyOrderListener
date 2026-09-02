using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMRPSupplyDto
{
	[JsonProperty("mrsCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string mrsCreatedBy { get; set; }

	[JsonProperty("mrsCreatedDate", Order = 2)]
	public DateTime? mrsCreatedDate { get; set; }

	[JsonProperty("mrsCustomerOrganizationID", Order = 3)]
	[MaxLength(10)]
	public string mrsCustomerOrganizationID { get; set; }

	[JsonProperty("mrsDueDate", Order = 4)]
	[Required(ErrorMessage = "mrsDueDate is required.")]
	public DateTime? mrsDueDate { get; set; }

	[JsonProperty("mrsUniqueID", Order = 5)]
	public Guid mrsUniqueID { get; set; }

	[JsonProperty("mrsJobAssemblyID", Order = 6)]
	public int mrsJobAssemblyID { get; set; }

	[JsonProperty("mrsJobID", Order = 7)]
	[MaxLength(20)]
	public string mrsJobID { get; set; }

	[JsonProperty("mrsLineID", Order = 8)]
	[Required(ErrorMessage = "mrsLineID is required.")]
	public int mrsLineID { get; set; }

	[JsonProperty("mrsPartBinID", Order = 9)]
	[Required(ErrorMessage = "mrsPartBinID is required.")]
	[MaxLength(15)]
	public string mrsPartBinID { get; set; }

	[JsonProperty("mrsPartID", Order = 10)]
	[Required(ErrorMessage = "mrsPartID is required.")]
	[MaxLength(30)]
	public string mrsPartID { get; set; }

	[JsonProperty("mrsPartRevisionID", Order = 11)]
	[MaxLength(15)]
	public string mrsPartRevisionID { get; set; }

	[JsonProperty("mrsPartWarehouseLocationID", Order = 12)]
	[Required(ErrorMessage = "mrsPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string mrsPartWarehouseLocationID { get; set; }

	[JsonProperty("mrsQuantityReceived", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrsQuantityReceived { get; set; }

	[JsonProperty("mrsQuantityShipped", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrsQuantityShipped { get; set; }

	[JsonProperty("mrsRowVersion", Order = 15)]
	public byte[] mrsRowVersion { get; set; }

	[JsonProperty("mrsSessionID", Order = 16)]
	[Required(ErrorMessage = "mrsSessionID is required.")]
	[MaxLength(10)]
	public string mrsSessionID { get; set; }

	[JsonProperty("mrsSource", Order = 17)]
	[MaxLength(20)]
	public string mrsSource { get; set; }

	[JsonProperty("mrsSupplyID", Order = 18)]
	[Required(ErrorMessage = "mrsSupplyID is required.")]
	public int mrsSupplyID { get; set; }

	[JsonProperty("mrsType", Order = 19)]
	[MaxLength(20)]
	public string mrsType { get; set; }

	[JsonProperty("customFields", Order = 20)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
