using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteMaterialDto
{
	[JsonProperty("qmmCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string qmmCreatedBy { get; set; }

	[JsonProperty("qmmCreatedDate", Order = 2)]
	public DateTime? qmmCreatedDate { get; set; }

	[JsonProperty("qmmDocuments", Order = 3)]
	[MaxLength(50)]
	public string qmmDocuments { get; set; }

	[JsonProperty("qmmUniqueID", Order = 4)]
	public Guid qmmUniqueID { get; set; }

	[JsonProperty("qmmEstimatedUnitCost", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmEstimatedUnitCost { get; set; }

	[JsonProperty("qmmBackflush", Order = 6)]
	public bool qmmBackflush { get; set; }

	[JsonProperty("qmmClosed", Order = 7)]
	public bool qmmClosed { get; set; }

	[JsonProperty("qmmCostOverride", Order = 8)]
	public bool qmmCostOverride { get; set; }

	[JsonProperty("qmmLeadTime", Order = 9)]
	public short qmmLeadTime { get; set; }

	[JsonProperty("qmmLeadTime1", Order = 10)]
	public short qmmLeadTime1 { get; set; }

	[JsonProperty("qmmLeadTime2", Order = 11)]
	public short qmmLeadTime2 { get; set; }

	[JsonProperty("qmmLeadTime3", Order = 12)]
	public short qmmLeadTime3 { get; set; }

	[JsonProperty("qmmLeadTime4", Order = 13)]
	public short qmmLeadTime4 { get; set; }

	[JsonProperty("qmmLeadTime5", Order = 14)]
	public short qmmLeadTime5 { get; set; }

	[JsonProperty("qmmLeadTime6", Order = 15)]
	public short qmmLeadTime6 { get; set; }

	[JsonProperty("qmmLeadTime7", Order = 16)]
	public short qmmLeadTime7 { get; set; }

	[JsonProperty("qmmLeadTime8", Order = 17)]
	public short qmmLeadTime8 { get; set; }

	[JsonProperty("qmmLeadTime9", Order = 18)]
	public short qmmLeadTime9 { get; set; }

	[JsonProperty("qmmMinimumCharge", Order = 19)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmMinimumCharge { get; set; }

	[JsonProperty("qmmPartBinID", Order = 20)]
	[Required(ErrorMessage = "qmmPartBinID is required.")]
	[MaxLength(15)]
	public string qmmPartBinID { get; set; }

	[JsonProperty("qmmPartID", Order = 21)]
	[Required(ErrorMessage = "qmmPartID is required.")]
	[MaxLength(30)]
	public string qmmPartID { get; set; }

	[JsonProperty("qmmPartLongDescriptionRtf", Order = 22)]
	public string qmmPartLongDescriptionRtf { get; set; }

	[JsonProperty("qmmPartLongDescriptionText", Order = 23)]
	public string qmmPartLongDescriptionText { get; set; }

	[JsonProperty("qmmPartRevisionID", Order = 24)]
	[MaxLength(15)]
	public string qmmPartRevisionID { get; set; }

	[JsonProperty("qmmPartShortDescription", Order = 25)]
	[Required(ErrorMessage = "qmmPartShortDescription is required.")]
	[MaxLength(50)]
	public string qmmPartShortDescription { get; set; }

	[JsonProperty("qmmPartWarehouseLocationID", Order = 26)]
	[Required(ErrorMessage = "qmmPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string qmmPartWarehouseLocationID { get; set; }

	[JsonProperty("qmmPurchaseLocationID", Order = 27)]
	[MaxLength(5)]
	public string qmmPurchaseLocationID { get; set; }

	[JsonProperty("qmmQuantityBreak1", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak1 { get; set; }

	[JsonProperty("qmmQuantityBreak2", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak2 { get; set; }

	[JsonProperty("qmmQuantityBreak3", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak3 { get; set; }

	[JsonProperty("qmmQuantityBreak4", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak4 { get; set; }

	[JsonProperty("qmmQuantityBreak5", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak5 { get; set; }

	[JsonProperty("qmmQuantityBreak6", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak6 { get; set; }

	[JsonProperty("qmmQuantityBreak7", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak7 { get; set; }

	[JsonProperty("qmmQuantityBreak8", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak8 { get; set; }

	[JsonProperty("qmmQuantityBreak9", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityBreak9 { get; set; }

	[JsonProperty("qmmQuantityPerAssembly", Order = 37)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmQuantityPerAssembly { get; set; }

	[JsonProperty("qmmQuoteAssemblyID", Order = 38)]
	public int qmmQuoteAssemblyID { get; set; }

	[JsonProperty("qmmQuoteID", Order = 39)]
	[Required(ErrorMessage = "qmmQuoteID is required.")]
	[MaxLength(10)]
	public string qmmQuoteID { get; set; }

	[JsonProperty("qmmQuoteLineID", Order = 40)]
	[Required(ErrorMessage = "qmmQuoteLineID is required.")]
	public short qmmQuoteLineID { get; set; }

	[JsonProperty("qmmRelatedQuoteOperationID", Order = 41)]
	public int qmmRelatedQuoteOperationID { get; set; }

	[JsonProperty("qmmRowVersion", Order = 42)]
	public byte[] qmmRowVersion { get; set; }

	[JsonProperty("qmmScrapPercent", Order = 43)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmScrapPercent { get; set; }

	[JsonProperty("qmmScrapQuantity", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmScrapQuantity { get; set; }

	[JsonProperty("qmmQuoteMaterialID", Order = 45)]
	[Required(ErrorMessage = "qmmQuoteMaterialID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int qmmQuoteMaterialID { get; set; }

	[JsonProperty("qmmSourcePriceID", Order = 46)]
	public int qmmSourcePriceID { get; set; }

	[JsonProperty("qmmSourceRfqID", Order = 47)]
	[MaxLength(10)]
	public string qmmSourceRfqID { get; set; }

	[JsonProperty("qmmSupplierOrganizationID", Order = 48)]
	[MaxLength(10)]
	public string qmmSupplierOrganizationID { get; set; }

	[JsonProperty("qmmUnitCost1", Order = 49)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost1 { get; set; }

	[JsonProperty("qmmUnitCost2", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost2 { get; set; }

	[JsonProperty("qmmUnitCost3", Order = 51)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost3 { get; set; }

	[JsonProperty("qmmUnitCost4", Order = 52)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost4 { get; set; }

	[JsonProperty("qmmUnitCost5", Order = 53)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost5 { get; set; }

	[JsonProperty("qmmUnitCost6", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost6 { get; set; }

	[JsonProperty("qmmUnitCost7", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost7 { get; set; }

	[JsonProperty("qmmUnitCost8", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost8 { get; set; }

	[JsonProperty("qmmUnitCost9", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmmUnitCost9 { get; set; }

	[JsonProperty("qmmUnitOfMeasure", Order = 58)]
	[MaxLength(2)]
	public string qmmUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 59)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
