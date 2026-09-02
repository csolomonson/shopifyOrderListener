using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobMaterialDto
{
	[JsonProperty("jmmCalculatedUnitCost", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmCalculatedUnitCost { get; set; }

	[JsonProperty("jmmCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string jmmCreatedBy { get; set; }

	[JsonProperty("jmmCreatedDate", Order = 3)]
	public DateTime? jmmCreatedDate { get; set; }

	[JsonProperty("jmmDocuments", Order = 4)]
	[MaxLength(50)]
	public string jmmDocuments { get; set; }

	[JsonProperty("jmmDueInDate", Order = 5)]
	public DateTime? jmmDueInDate { get; set; }

	[JsonProperty("jmmUniqueID", Order = 6)]
	public Guid jmmUniqueID { get; set; }

	[JsonProperty("jmmEstimatedQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmEstimatedQuantity { get; set; }

	[JsonProperty("jmmEstimatedUnitCost", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmEstimatedUnitCost { get; set; }

	[JsonProperty("jmmBackflush", Order = 9)]
	public bool jmmBackflush { get; set; }

	[JsonProperty("jmmClosed", Order = 10)]
	public bool jmmClosed { get; set; }

	[JsonProperty("jmmCostOverride", Order = 11)]
	public bool jmmCostOverride { get; set; }

	[JsonProperty("jmmFirm", Order = 12)]
	public bool jmmFirm { get; set; }

	[JsonProperty("jmmKitPart", Order = 13)]
	public bool jmmKitPart { get; set; }

	[JsonProperty("jmmPullAllFromStock", Order = 14)]
	public bool jmmPullAllFromStock { get; set; }

	[JsonProperty("jmmReceivedComplete", Order = 15)]
	public bool jmmReceivedComplete { get; set; }

	[JsonProperty("jmmJobAssemblyID", Order = 16)]
	public int jmmJobAssemblyID { get; set; }

	[JsonProperty("jmmJobID", Order = 17)]
	[Required(ErrorMessage = "jmmJobID is required.")]
	[MaxLength(20)]
	public string jmmJobID { get; set; }

	[JsonProperty("jmmLeadTime", Order = 18)]
	public short jmmLeadTime { get; set; }

	[JsonProperty("jmmLeadTime1", Order = 19)]
	public short jmmLeadTime1 { get; set; }

	[JsonProperty("jmmLeadTime2", Order = 20)]
	public short jmmLeadTime2 { get; set; }

	[JsonProperty("jmmLeadTime3", Order = 21)]
	public short jmmLeadTime3 { get; set; }

	[JsonProperty("jmmLeadTime4", Order = 22)]
	public short jmmLeadTime4 { get; set; }

	[JsonProperty("jmmLeadTime5", Order = 23)]
	public short jmmLeadTime5 { get; set; }

	[JsonProperty("jmmLeadTime6", Order = 24)]
	public short jmmLeadTime6 { get; set; }

	[JsonProperty("jmmLeadTime7", Order = 25)]
	public short jmmLeadTime7 { get; set; }

	[JsonProperty("jmmLeadTime8", Order = 26)]
	public short jmmLeadTime8 { get; set; }

	[JsonProperty("jmmLeadTime9", Order = 27)]
	public short jmmLeadTime9 { get; set; }

	[JsonProperty("jmmMinimumCharge", Order = 28)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmMinimumCharge { get; set; }

	[JsonProperty("jmmOrderByDate", Order = 29)]
	public DateTime? jmmOrderByDate { get; set; }

	[JsonProperty("jmmPartBinID", Order = 30)]
	[Required(ErrorMessage = "jmmPartBinID is required.")]
	[MaxLength(15)]
	public string jmmPartBinID { get; set; }

	[JsonProperty("jmmPartID", Order = 31)]
	[Required(ErrorMessage = "jmmPartID is required.")]
	[MaxLength(30)]
	public string jmmPartID { get; set; }

	[JsonProperty("jmmPartLongDescriptionRtf", Order = 32)]
	public string jmmPartLongDescriptionRtf { get; set; }

	[JsonProperty("jmmPartLongDescriptionText", Order = 33)]
	public string jmmPartLongDescriptionText { get; set; }

	[JsonProperty("jmmPartRevisionID", Order = 34)]
	[MaxLength(15)]
	public string jmmPartRevisionID { get; set; }

	[JsonProperty("jmmPartShortDescription", Order = 35)]
	[Required(ErrorMessage = "jmmPartShortDescription is required.")]
	[MaxLength(50)]
	public string jmmPartShortDescription { get; set; }

	[JsonProperty("jmmPartWarehouseLocationID", Order = 36)]
	[Required(ErrorMessage = "jmmPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string jmmPartWarehouseLocationID { get; set; }

	[JsonProperty("jmmPullFromStockQuantity", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmPullFromStockQuantity { get; set; }

	[JsonProperty("jmmPurchaseLocationID", Order = 38)]
	[MaxLength(5)]
	public string jmmPurchaseLocationID { get; set; }

	[JsonProperty("jmmPurchaseOrderID", Order = 39)]
	[MaxLength(10)]
	public string jmmPurchaseOrderID { get; set; }

	[JsonProperty("jmmPurchaseToJobQuantity", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmPurchaseToJobQuantity { get; set; }

	[JsonProperty("jmmQuantityAllocated", Order = 41)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityAllocated { get; set; }

	[JsonProperty("jmmQuantityBreak1", Order = 42)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak1 { get; set; }

	[JsonProperty("jmmQuantityBreak2", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak2 { get; set; }

	[JsonProperty("jmmQuantityBreak3", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak3 { get; set; }

	[JsonProperty("jmmQuantityBreak4", Order = 45)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak4 { get; set; }

	[JsonProperty("jmmQuantityBreak5", Order = 46)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak5 { get; set; }

	[JsonProperty("jmmQuantityBreak6", Order = 47)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak6 { get; set; }

	[JsonProperty("jmmQuantityBreak7", Order = 48)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak7 { get; set; }

	[JsonProperty("jmmQuantityBreak8", Order = 49)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak8 { get; set; }

	[JsonProperty("jmmQuantityBreak9", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityBreak9 { get; set; }

	[JsonProperty("jmmQuantityPerAssembly", Order = 51)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityPerAssembly { get; set; }

	[JsonProperty("jmmQuantityReceived", Order = 52)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityReceived { get; set; }

	[JsonProperty("jmmQuantityToInspect", Order = 53)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityToInspect { get; set; }

	[JsonProperty("jmmQuantityToReturn", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmQuantityToReturn { get; set; }

	[JsonProperty("jmmRelatedJobOperationID", Order = 55)]
	public int jmmRelatedJobOperationID { get; set; }

	[JsonProperty("jmmRequiredDate", Order = 56)]
	public DateTime? jmmRequiredDate { get; set; }

	[JsonProperty("jmmRfqID", Order = 57)]
	[MaxLength(10)]
	public string jmmRfqID { get; set; }

	[JsonProperty("jmmRowVersion", Order = 58)]
	public byte[] jmmRowVersion { get; set; }

	[JsonProperty("jmmScrapPercent", Order = 59)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmScrapPercent { get; set; }

	[JsonProperty("jmmScrapQuantity", Order = 60)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmScrapQuantity { get; set; }

	[JsonProperty("jmmScrapQuantityReceived", Order = 61)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmScrapQuantityReceived { get; set; }

	[JsonProperty("jmmJobMaterialID", Order = 62)]
	[Required(ErrorMessage = "jmmJobMaterialID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int jmmJobMaterialID { get; set; }

	[JsonProperty("jmmSupplierOrganizationID", Order = 63)]
	[MaxLength(10)]
	public string jmmSupplierOrganizationID { get; set; }

	[JsonProperty("jmmUnitCost1", Order = 64)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost1 { get; set; }

	[JsonProperty("jmmUnitCost2", Order = 65)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost2 { get; set; }

	[JsonProperty("jmmUnitCost3", Order = 66)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost3 { get; set; }

	[JsonProperty("jmmUnitCost4", Order = 67)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost4 { get; set; }

	[JsonProperty("jmmUnitCost5", Order = 68)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost5 { get; set; }

	[JsonProperty("jmmUnitCost6", Order = 69)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost6 { get; set; }

	[JsonProperty("jmmUnitCost7", Order = 70)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost7 { get; set; }

	[JsonProperty("jmmUnitCost8", Order = 71)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost8 { get; set; }

	[JsonProperty("jmmUnitCost9", Order = 72)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmmUnitCost9 { get; set; }

	[JsonProperty("jmmUnitOfMeasure", Order = 73)]
	[MaxLength(2)]
	public string jmmUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 74)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
