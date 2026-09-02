using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderLineDto
{
	[JsonProperty("pmlAssetID", Order = 1)]
	[MaxLength(10)]
	public string pmlAssetID { get; set; }

	[JsonProperty("pmlAssetTypeID", Order = 2)]
	[MaxLength(5)]
	public string pmlAssetTypeID { get; set; }

	[JsonProperty("pmlConversionFactor", Order = 3)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlConversionFactor { get; set; }

	[JsonProperty("pmlCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string pmlCreatedBy { get; set; }

	[JsonProperty("pmlCreatedDate", Order = 5)]
	public DateTime? pmlCreatedDate { get; set; }

	[JsonProperty("pmlDmrClaimID", Order = 6)]
	[MaxLength(10)]
	public string pmlDmrClaimID { get; set; }

	[JsonProperty("pmlDmrClaimLineID", Order = 7)]
	public short pmlDmrClaimLineID { get; set; }

	[JsonProperty("pmlDocuments", Order = 8)]
	[MaxLength(50)]
	public string pmlDocuments { get; set; }

	[JsonProperty("pmlDueDate", Order = 9)]
	[Required(ErrorMessage = "pmlDueDate is required.")]
	public DateTime? pmlDueDate { get; set; }

	[JsonProperty("pmlUniqueID", Order = 10)]
	public Guid pmlUniqueID { get; set; }

	[JsonProperty("pmlExpenseSplitPercentTotal", Order = 11)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlExpenseSplitPercentTotal { get; set; }

	[JsonProperty("pmlExtendedCostBase", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlExtendedCostBase { get; set; }

	[JsonProperty("pmlExtendedCostForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlExtendedCostForeign { get; set; }

	[JsonProperty("pmlForm1099Box", Order = 14)]
	public byte pmlForm1099Box { get; set; }

	[JsonProperty("pmlInventoryQuantity", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlInventoryQuantity { get; set; }

	[JsonProperty("pmlInventoryQuantityReceived", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlInventoryQuantityReceived { get; set; }

	[JsonProperty("pmlInventoryUnitOfMeasure", Order = 17)]
	[MaxLength(2)]
	public string pmlInventoryUnitOfMeasure { get; set; }

	[JsonProperty("pmlClosed", Order = 18)]
	public bool pmlClosed { get; set; }

	[JsonProperty("pmlCreateJobSeq", Order = 19)]
	public bool pmlCreateJobSeq { get; set; }

	[JsonProperty("pmlIntraCompanyPosted", Order = 20)]
	public bool pmlIntraCompanyPosted { get; set; }

	[JsonProperty("pmlInTransit", Order = 21)]
	public bool pmlInTransit { get; set; }

	[JsonProperty("pmlInTransitJournalsCreated", Order = 22)]
	public bool pmlInTransitJournalsCreated { get; set; }

	[JsonProperty("pmlInvoicedComplete", Order = 23)]
	public bool pmlInvoicedComplete { get; set; }

	[JsonProperty("pmlKitPart", Order = 24)]
	public bool pmlKitPart { get; set; }

	[JsonProperty("pmlPlanned", Order = 25)]
	public bool pmlPlanned { get; set; }

	[JsonProperty("pmlPriceOverride", Order = 26)]
	public bool pmlPriceOverride { get; set; }

	[JsonProperty("pmlReceivedComplete", Order = 27)]
	public bool pmlReceivedComplete { get; set; }

	[JsonProperty("pmlRequiresInspection", Order = 28)]
	public bool pmlRequiresInspection { get; set; }

	[JsonProperty("pmlSupplierRequirement", Order = 29)]
	public bool pmlSupplierRequirement { get; set; }

	[JsonProperty("pmlTaxable", Order = 30)]
	public bool pmlTaxable { get; set; }

	[JsonProperty("pmlItemType", Order = 31)]
	[MaxLength(1)]
	public string pmlItemType { get; set; }

	[JsonProperty("pmlJobAssemblyID", Order = 32)]
	public int pmlJobAssemblyID { get; set; }

	[JsonProperty("pmlJobID", Order = 33)]
	[MaxLength(20)]
	public string pmlJobID { get; set; }

	[JsonProperty("pmlJobMaterialID", Order = 34)]
	public int pmlJobMaterialID { get; set; }

	[JsonProperty("pmlJobOpenQuantity", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlJobOpenQuantity { get; set; }

	[JsonProperty("pmlJobOperationID", Order = 36)]
	public int pmlJobOperationID { get; set; }

	[JsonProperty("pmlJobType", Order = 37)]
	public byte pmlJobType { get; set; }

	[JsonProperty("pmlLandedCostID", Order = 38)]
	[MaxLength(10)]
	public string pmlLandedCostID { get; set; }

	[JsonProperty("pmlLeadTime", Order = 39)]
	public short pmlLeadTime { get; set; }

	[JsonProperty("pmlNonTaxReasonID", Order = 40)]
	[MaxLength(5)]
	public string pmlNonTaxReasonID { get; set; }

	[JsonProperty("pmlOrgPartID", Order = 41)]
	[MaxLength(30)]
	public string pmlOrgPartID { get; set; }

	[JsonProperty("pmlOrgPartShortDescription", Order = 42)]
	[MaxLength(50)]
	public string pmlOrgPartShortDescription { get; set; }

	[JsonProperty("pmlPartBinID", Order = 43)]
	[MaxLength(15)]
	public string pmlPartBinID { get; set; }

	[JsonProperty("pmlPartID", Order = 44)]
	[MaxLength(30)]
	public string pmlPartID { get; set; }

	[JsonProperty("pmlPartLongDescriptionRtf", Order = 45)]
	public string pmlPartLongDescriptionRtf { get; set; }

	[JsonProperty("pmlPartLongDescriptionText", Order = 46)]
	public string pmlPartLongDescriptionText { get; set; }

	[JsonProperty("pmlPartRevisionID", Order = 47)]
	[MaxLength(15)]
	public string pmlPartRevisionID { get; set; }

	[JsonProperty("pmlPartShortDescription", Order = 48)]
	[MaxLength(50)]
	public string pmlPartShortDescription { get; set; }

	[JsonProperty("pmlPartWarehouseLocationID", Order = 49)]
	[MaxLength(5)]
	public string pmlPartWarehouseLocationID { get; set; }

	[JsonProperty("pmlProcessID", Order = 50)]
	[MaxLength(5)]
	public string pmlProcessID { get; set; }

	[JsonProperty("pmlProjectAreaID", Order = 51)]
	[MaxLength(15)]
	public string pmlProjectAreaID { get; set; }

	[JsonProperty("pmlProjectID", Order = 52)]
	[MaxLength(10)]
	public string pmlProjectID { get; set; }

	[JsonProperty("pmlPurchaseOrderID", Order = 53)]
	[Required(ErrorMessage = "pmlPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmlPurchaseOrderID { get; set; }

	[JsonProperty("pmlPurchaseQuantity", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlPurchaseQuantity { get; set; }

	[JsonProperty("pmlPurchaseQuantityReceived", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlPurchaseQuantityReceived { get; set; }

	[JsonProperty("pmlPurchaseType", Order = 56)]
	[Required(ErrorMessage = "pmlPurchaseType is required.")]
	public byte pmlPurchaseType { get; set; }

	[JsonProperty("pmlPurchaseUnitCostBase", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlPurchaseUnitCostBase { get; set; }

	[JsonProperty("pmlPurchaseUnitCostForeign", Order = 58)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlPurchaseUnitCostForeign { get; set; }

	[JsonProperty("pmlPurchaseUnitOfMeasure", Order = 59)]
	[MaxLength(2)]
	public string pmlPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("pmlQuantityOnOrder", Order = 60)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlQuantityOnOrder { get; set; }

	[JsonProperty("pmlRfqID", Order = 61)]
	[MaxLength(10)]
	public string pmlRfqID { get; set; }

	[JsonProperty("pmlRfqLineID", Order = 62)]
	public short pmlRfqLineID { get; set; }

	[JsonProperty("pmlRmaClaimID", Order = 63)]
	[MaxLength(10)]
	public string pmlRmaClaimID { get; set; }

	[JsonProperty("pmlRmaClaimLineID", Order = 64)]
	public short pmlRmaClaimLineID { get; set; }

	[JsonProperty("pmlRowVersion", Order = 65)]
	public byte[] pmlRowVersion { get; set; }

	[JsonProperty("pmlSalesOrderDeliveryID", Order = 66)]
	public short pmlSalesOrderDeliveryID { get; set; }

	[JsonProperty("pmlSalesOrderID", Order = 67)]
	[MaxLength(10)]
	public string pmlSalesOrderID { get; set; }

	[JsonProperty("pmlSalesOrderLineID", Order = 68)]
	public short pmlSalesOrderLineID { get; set; }

	[JsonProperty("pmlSecondTaxAmountBase", Order = 69)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlSecondTaxAmountBase { get; set; }

	[JsonProperty("pmlSecondTaxAmountForeign", Order = 70)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlSecondTaxAmountForeign { get; set; }

	[JsonProperty("pmlSecondTaxCodeID", Order = 71)]
	[MaxLength(5)]
	public string pmlSecondTaxCodeID { get; set; }

	[JsonProperty("pmlPurchaseOrderLineID", Order = 72)]
	[Required(ErrorMessage = "pmlPurchaseOrderLineID is required.")]
	public short pmlPurchaseOrderLineID { get; set; }

	[JsonProperty("pmlSetupChargeBase", Order = 73)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlSetupChargeBase { get; set; }

	[JsonProperty("pmlSetupChargeForeign", Order = 74)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlSetupChargeForeign { get; set; }

	[JsonProperty("pmlSourcePurchaseOrderID", Order = 75)]
	[MaxLength(10)]
	public string pmlSourcePurchaseOrderID { get; set; }

	[JsonProperty("pmlSourcePurchaseOrderLineID", Order = 76)]
	public short pmlSourcePurchaseOrderLineID { get; set; }

	[JsonProperty("pmlSourceTableName", Order = 77)]
	[MaxLength(30)]
	public string pmlSourceTableName { get; set; }

	[JsonProperty("pmlSourceTableUniqueID", Order = 78)]
	public Guid pmlSourceTableUniqueID { get; set; }

	[JsonProperty("pmlTaxAmountBase", Order = 79)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlTaxAmountBase { get; set; }

	[JsonProperty("pmlTaxAmountForeign", Order = 80)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlTaxAmountForeign { get; set; }

	[JsonProperty("pmlTaxCodeID", Order = 81)]
	[MaxLength(5)]
	public string pmlTaxCodeID { get; set; }

	[JsonProperty("pmlTotalComponentCosts", Order = 82)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlTotalComponentCosts { get; set; }

	[JsonProperty("pmlTotalExtendedCostBase", Order = 83)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlTotalExtendedCostBase { get; set; }

	[JsonProperty("pmlTotalExtendedCostForeign", Order = 84)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmlTotalExtendedCostForeign { get; set; }

	[JsonProperty("pmlTrackingNumber", Order = 85)]
	[MaxLength(30)]
	public string pmlTrackingNumber { get; set; }

	[JsonProperty("pmlWorkCenterID", Order = 86)]
	[MaxLength(5)]
	public string pmlWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 87)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
