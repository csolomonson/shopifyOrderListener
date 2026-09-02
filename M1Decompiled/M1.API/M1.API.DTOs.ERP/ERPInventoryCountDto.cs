using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPInventoryCountDto
{
	[JsonProperty("imnCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imnCreatedBy { get; set; }

	[JsonProperty("imnCreatedDate", Order = 2)]
	public DateTime? imnCreatedDate { get; set; }

	[JsonProperty("imnCycleCodeID", Order = 3)]
	[MaxLength(5)]
	public string imnCycleCodeID { get; set; }

	[JsonProperty("imnUniqueID", Order = 4)]
	public Guid imnUniqueID { get; set; }

	[JsonProperty("imnGeneratedDate", Order = 5)]
	public DateTime? imnGeneratedDate { get; set; }

	[JsonProperty("imnExcludeInactivePartBins", Order = 6)]
	public bool imnExcludeInactivePartBins { get; set; }

	[JsonProperty("imnIncludeBlankPartClass", Order = 7)]
	public bool imnIncludeBlankPartClass { get; set; }

	[JsonProperty("imnIncludeBlankPartGroup", Order = 8)]
	public bool imnIncludeBlankPartGroup { get; set; }

	[JsonProperty("imnPostedToInventory", Order = 9)]
	public bool imnPostedToInventory { get; set; }

	[JsonProperty("imnRecordsGenerated", Order = 10)]
	public bool imnRecordsGenerated { get; set; }

	[JsonProperty("imnNumberofRecordsGenerated", Order = 11)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imnNumberofRecordsGenerated { get; set; }

	[JsonProperty("imnPartBinIDs", Order = 12)]
	[MaxLength(4)]
	public string imnPartBinIDs { get; set; }

	[JsonProperty("imnPartClassIDs", Order = 13)]
	[MaxLength(4)]
	public string imnPartClassIDs { get; set; }

	[JsonProperty("imnPartGroupIDs", Order = 14)]
	[MaxLength(4)]
	public string imnPartGroupIDs { get; set; }

	[JsonProperty("imnPartWarehouseIDs", Order = 15)]
	[MaxLength(4)]
	public string imnPartWarehouseIDs { get; set; }

	[JsonProperty("imnPostedDate", Order = 16)]
	public DateTime? imnPostedDate { get; set; }

	[JsonProperty("imnRowVersion", Order = 17)]
	public byte[] imnRowVersion { get; set; }

	[JsonProperty("imnInventoryCountID", Order = 18)]
	[Required(ErrorMessage = "imnInventoryCountID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imnInventoryCountID { get; set; }

	[JsonProperty("imnStatus", Order = 19)]
	[Required(ErrorMessage = "imnStatus is required.")]
	public byte imnStatus { get; set; }

	[JsonProperty("imnSupplierOrganizationIDs", Order = 20)]
	[MaxLength(4)]
	public string imnSupplierOrganizationIDs { get; set; }

	[JsonProperty("customFields", Order = 21)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
