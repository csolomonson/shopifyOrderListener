using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartTransactionDto
{
	[JsonProperty("imtCogsCalculatedDate", Order = 1)]
	public DateTime? imtCogsCalculatedDate { get; set; }

	[JsonProperty("imtCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imtCreatedBy { get; set; }

	[JsonProperty("imtCreatedDate", Order = 3)]
	public DateTime? imtCreatedDate { get; set; }

	[JsonProperty("imtUniqueID", Order = 4)]
	public Guid imtUniqueID { get; set; }

	[JsonProperty("imtHeatLot", Order = 5)]
	[MaxLength(50)]
	public string imtHeatLot { get; set; }

	[JsonProperty("imtInspectionStatus", Order = 6)]
	[MaxLength(1)]
	public string imtInspectionStatus { get; set; }

	[JsonProperty("imtInventoryQuantityReceived", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imtInventoryQuantityReceived { get; set; }

	[JsonProperty("imtInventoryUnitOfMeasure", Order = 8)]
	[MaxLength(2)]
	public string imtInventoryUnitOfMeasure { get; set; }

	[JsonProperty("imtCogsPostedToGl", Order = 9)]
	public bool imtCogsPostedToGl { get; set; }

	[JsonProperty("imtJobCompleteStatus", Order = 10)]
	public bool imtJobCompleteStatus { get; set; }

	[JsonProperty("imtNonInventoryTransaction", Order = 11)]
	public bool imtNonInventoryTransaction { get; set; }

	[JsonProperty("imtNonNettable", Order = 12)]
	public bool imtNonNettable { get; set; }

	[JsonProperty("imtPoLineReceivedComplete", Order = 13)]
	public bool imtPoLineReceivedComplete { get; set; }

	[JsonProperty("imtRequiresInspection", Order = 14)]
	public bool imtRequiresInspection { get; set; }

	[JsonProperty("imtIssueType", Order = 15)]
	public byte imtIssueType { get; set; }

	[JsonProperty("imtJobAssemblyID", Order = 16)]
	public int imtJobAssemblyID { get; set; }

	[JsonProperty("imtJobID", Order = 17)]
	[MaxLength(20)]
	public string imtJobID { get; set; }

	[JsonProperty("imtJobMaterialComponentID", Order = 18)]
	public int imtJobMaterialComponentID { get; set; }

	[JsonProperty("imtJobMaterialID", Order = 19)]
	public int imtJobMaterialID { get; set; }

	[JsonProperty("imtJobOperationID", Order = 20)]
	public int imtJobOperationID { get; set; }

	[JsonProperty("imtJobType", Order = 21)]
	public byte imtJobType { get; set; }

	[JsonProperty("imtPartBinID", Order = 22)]
	[Required(ErrorMessage = "imtPartBinID is required.")]
	[MaxLength(15)]
	public string imtPartBinID { get; set; }

	[JsonProperty("imtPartID", Order = 23)]
	[Required(ErrorMessage = "imtPartID is required.")]
	[MaxLength(30)]
	public string imtPartID { get; set; }

	[JsonProperty("imtPartRevisionID", Order = 24)]
	[MaxLength(15)]
	public string imtPartRevisionID { get; set; }

	[JsonProperty("imtPartWarehouseLocationID", Order = 25)]
	[Required(ErrorMessage = "imtPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string imtPartWarehouseLocationID { get; set; }

	[JsonProperty("imtPlantID", Order = 26)]
	[MaxLength(5)]
	public string imtPlantID { get; set; }

	[JsonProperty("imtPreviousQuantityOnHand", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imtPreviousQuantityOnHand { get; set; }

	[JsonProperty("imtProjectAreaID", Order = 28)]
	[MaxLength(15)]
	public string imtProjectAreaID { get; set; }

	[JsonProperty("imtProjectID", Order = 29)]
	[MaxLength(10)]
	public string imtProjectID { get; set; }

	[JsonProperty("imtQuantityToInspect", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imtQuantityToInspect { get; set; }

	[JsonProperty("imtQuantityToReturn", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imtQuantityToReturn { get; set; }

	[JsonProperty("imtReceiptType", Order = 32)]
	public byte imtReceiptType { get; set; }

	[JsonProperty("imtReference", Order = 33)]
	[MaxLength(30)]
	public string imtReference { get; set; }

	[JsonProperty("imtRowVersion", Order = 34)]
	public byte[] imtRowVersion { get; set; }

	[JsonProperty("imtScrapQuantity", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imtScrapQuantity { get; set; }

	[JsonProperty("imtPartTransactionID", Order = 36)]
	[Required(ErrorMessage = "imtPartTransactionID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imtPartTransactionID { get; set; }

	[JsonProperty("imtSetupCharge", Order = 37)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imtSetupCharge { get; set; }

	[JsonProperty("imtSource", Order = 38)]
	public byte imtSource { get; set; }

	[JsonProperty("imtTableName", Order = 39)]
	[MaxLength(30)]
	public string imtTableName { get; set; }

	[JsonProperty("imtTableUniqueID", Order = 40)]
	public Guid imtTableUniqueID { get; set; }

	[JsonProperty("imtTransactionDate", Order = 41)]
	[Required(ErrorMessage = "imtTransactionDate is required.")]
	public DateTime? imtTransactionDate { get; set; }

	[JsonProperty("imtTransactionType", Order = 42)]
	public byte imtTransactionType { get; set; }

	[JsonProperty("imtUserID", Order = 43)]
	[Required(ErrorMessage = "imtUserID is required.")]
	[MaxLength(20)]
	public string imtUserID { get; set; }

	[JsonProperty("customFields", Order = 44)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
