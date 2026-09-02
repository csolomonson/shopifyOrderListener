using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARInvoiceLineDto
{
	[JsonProperty("arlActualTotalCostOfGoodsSold", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualTotalCostOfGoodsSold { get; set; }

	[JsonProperty("arlActualTotalLaborCost", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualTotalLaborCost { get; set; }

	[JsonProperty("arlActualTotalMaterialCost", Order = 3)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualTotalMaterialCost { get; set; }

	[JsonProperty("arlActualTotalOverheadCost", Order = 4)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualTotalOverheadCost { get; set; }

	[JsonProperty("arlActualTotalSubcontractCost", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualTotalSubcontractCost { get; set; }

	[JsonProperty("arlActualUnitCostOfGoodsSold", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualUnitCostOfGoodsSold { get; set; }

	[JsonProperty("arlActualUnitLaborCost", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualUnitLaborCost { get; set; }

	[JsonProperty("arlActualUnitMaterialCost", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualUnitMaterialCost { get; set; }

	[JsonProperty("arlActualUnitOverheadCost", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualUnitOverheadCost { get; set; }

	[JsonProperty("arlActualUnitSubcontractCost", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlActualUnitSubcontractCost { get; set; }

	[JsonProperty("arlAmtForResellerCommission", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlAmtForResellerCommission { get; set; }

	[JsonProperty("arlAmtForSalesCommission", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlAmtForSalesCommission { get; set; }

	[JsonProperty("arlArInvoiceID", Order = 13)]
	[Required(ErrorMessage = "arlArInvoiceID is required.")]
	[MaxLength(10)]
	public string arlArInvoiceID { get; set; }

	[JsonProperty("arlArRecurringInvoiceID", Order = 14)]
	public int arlArRecurringInvoiceID { get; set; }

	[JsonProperty("arlArRecurringInvoiceLineID", Order = 15)]
	public short arlArRecurringInvoiceLineID { get; set; }

	[JsonProperty("arlAssetAdjustmentID", Order = 16)]
	public int arlAssetAdjustmentID { get; set; }

	[JsonProperty("arlAssetID", Order = 17)]
	[MaxLength(10)]
	public string arlAssetID { get; set; }

	[JsonProperty("arlCallID", Order = 18)]
	[MaxLength(10)]
	public string arlCallID { get; set; }

	[JsonProperty("arlCogsCalculatedDate", Order = 19)]
	public DateTime? arlCogsCalculatedDate { get; set; }

	[JsonProperty("arlCommissionAmount", Order = 20)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlCommissionAmount { get; set; }

	[JsonProperty("arlCommissionRate", Order = 21)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlCommissionRate { get; set; }

	[JsonProperty("arlCreatedBy", Order = 22)]
	[MaxLength(20)]
	public string arlCreatedBy { get; set; }

	[JsonProperty("arlCreatedDate", Order = 23)]
	public DateTime? arlCreatedDate { get; set; }

	[JsonProperty("arlCustomerPo", Order = 24)]
	[MaxLength(40)]
	public string arlCustomerPo { get; set; }

	[JsonProperty("arlDepositAmountBase", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDepositAmountBase { get; set; }

	[JsonProperty("arlDepositAmountForeign", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDepositAmountForeign { get; set; }

	[JsonProperty("arlDepositBalanceBase", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDepositBalanceBase { get; set; }

	[JsonProperty("arlDepositBalanceForeign", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDepositBalanceForeign { get; set; }

	[JsonProperty("arlDepositInvoiceID", Order = 29)]
	[MaxLength(10)]
	public string arlDepositInvoiceID { get; set; }

	[JsonProperty("arlDepositInvoiceLineID", Order = 30)]
	public short arlDepositInvoiceLineID { get; set; }

	[JsonProperty("arlDepositTransferredBase", Order = 31)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDepositTransferredBase { get; set; }

	[JsonProperty("arlDepositTransferredForeign", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDepositTransferredForeign { get; set; }

	[JsonProperty("arlDiscountPercent", Order = 33)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlDiscountPercent { get; set; }

	[JsonProperty("arlUniqueID", Order = 34)]
	public Guid arlUniqueID { get; set; }

	[JsonProperty("arlEstTotalCostOfGoodsSold", Order = 35)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstTotalCostOfGoodsSold { get; set; }

	[JsonProperty("arlEstTotalLaborCost", Order = 36)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstTotalLaborCost { get; set; }

	[JsonProperty("arlEstTotalMaterialCost", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstTotalMaterialCost { get; set; }

	[JsonProperty("arlEstTotalOverheadCost", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstTotalOverheadCost { get; set; }

	[JsonProperty("arlEstTotalSubcontractCost", Order = 39)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstTotalSubcontractCost { get; set; }

	[JsonProperty("arlEstUnitCostOfGoodsSold", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstUnitCostOfGoodsSold { get; set; }

	[JsonProperty("arlEstUnitLaborCost", Order = 41)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstUnitLaborCost { get; set; }

	[JsonProperty("arlEstUnitMaterialCost", Order = 42)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstUnitMaterialCost { get; set; }

	[JsonProperty("arlEstUnitOverheadCost", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstUnitOverheadCost { get; set; }

	[JsonProperty("arlEstUnitSubcontractCost", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlEstUnitSubcontractCost { get; set; }

	[JsonProperty("arlExtendedDiscountBase", Order = 45)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlExtendedDiscountBase { get; set; }

	[JsonProperty("arlExtendedDiscountForeign", Order = 46)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlExtendedDiscountForeign { get; set; }

	[JsonProperty("arlExtendedPriceBase", Order = 47)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlExtendedPriceBase { get; set; }

	[JsonProperty("arlExtendedPriceForeign", Order = 48)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlExtendedPriceForeign { get; set; }

	[JsonProperty("arlFinanceSourceInvoiceID", Order = 49)]
	[MaxLength(10)]
	public string arlFinanceSourceInvoiceID { get; set; }

	[JsonProperty("arlFreightAmountBase", Order = 50)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlFreightAmountBase { get; set; }

	[JsonProperty("arlFreightAmountForeign", Order = 51)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlFreightAmountForeign { get; set; }

	[JsonProperty("arlFullExtendedPriceBase", Order = 52)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlFullExtendedPriceBase { get; set; }

	[JsonProperty("arlFullExtendedPriceForeign", Order = 53)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlFullExtendedPriceForeign { get; set; }

	[JsonProperty("arlFullUnitPriceBase", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlFullUnitPriceBase { get; set; }

	[JsonProperty("arlFullUnitPriceForeign", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlFullUnitPriceForeign { get; set; }

	[JsonProperty("arlInvoiceQuantity", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlInvoiceQuantity { get; set; }

	[JsonProperty("arlAvalaraIgnoreLine", Order = 57)]
	public bool arlAvalaraIgnoreLine { get; set; }

	[JsonProperty("arlCogsPostedToGl", Order = 58)]
	public bool arlCogsPostedToGl { get; set; }

	[JsonProperty("arlDeliveryInvoicedComplete", Order = 59)]
	public bool arlDeliveryInvoicedComplete { get; set; }

	[JsonProperty("arlDepositLine", Order = 60)]
	public bool arlDepositLine { get; set; }

	[JsonProperty("arlIncludeTaxInRetention", Order = 61)]
	public bool arlIncludeTaxInRetention { get; set; }

	[JsonProperty("arlIntraCompanyPosted", Order = 62)]
	public bool arlIntraCompanyPosted { get; set; }

	[JsonProperty("arlPayCommission", Order = 63)]
	public bool arlPayCommission { get; set; }

	[JsonProperty("arlPostedToGl", Order = 64)]
	public bool arlPostedToGl { get; set; }

	[JsonProperty("arlRetention", Order = 65)]
	public bool arlRetention { get; set; }

	[JsonProperty("arlJobAssemblyID", Order = 66)]
	public int arlJobAssemblyID { get; set; }

	[JsonProperty("arlJobID", Order = 67)]
	[MaxLength(20)]
	public string arlJobID { get; set; }

	[JsonProperty("arlJobMaterialID", Order = 68)]
	public int arlJobMaterialID { get; set; }

	[JsonProperty("arlLineType", Order = 69)]
	public byte arlLineType { get; set; }

	[JsonProperty("arlNonTaxReasonID", Order = 70)]
	[MaxLength(5)]
	public string arlNonTaxReasonID { get; set; }

	[JsonProperty("arlOrderQuantity", Order = 71)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlOrderQuantity { get; set; }

	[JsonProperty("arlOrgPartID", Order = 72)]
	[MaxLength(30)]
	public string arlOrgPartID { get; set; }

	[JsonProperty("arlOrgPartShortDescription", Order = 73)]
	[MaxLength(50)]
	public string arlOrgPartShortDescription { get; set; }

	[JsonProperty("arlPartGroupID", Order = 74)]
	[MaxLength(5)]
	public string arlPartGroupID { get; set; }

	[JsonProperty("arlPartID", Order = 75)]
	[Required(ErrorMessage = "arlPartID is required.")]
	[MaxLength(30)]
	public string arlPartID { get; set; }

	[JsonProperty("arlPartLongDescriptionRtf", Order = 76)]
	public string arlPartLongDescriptionRtf { get; set; }

	[JsonProperty("arlPartLongDescriptionText", Order = 77)]
	public string arlPartLongDescriptionText { get; set; }

	[JsonProperty("arlPartRevisionID", Order = 78)]
	[MaxLength(15)]
	public string arlPartRevisionID { get; set; }

	[JsonProperty("arlPartShortDescription", Order = 79)]
	[Required(ErrorMessage = "arlPartShortDescription is required.")]
	[MaxLength(50)]
	public string arlPartShortDescription { get; set; }

	[JsonProperty("arlProjectAreaID", Order = 80)]
	[MaxLength(15)]
	public string arlProjectAreaID { get; set; }

	[JsonProperty("arlProjectID", Order = 81)]
	[MaxLength(10)]
	public string arlProjectID { get; set; }

	[JsonProperty("arlRetentionAmountBase", Order = 82)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlRetentionAmountBase { get; set; }

	[JsonProperty("arlRetentionAmountForeign", Order = 83)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlRetentionAmountForeign { get; set; }

	[JsonProperty("arlRetentionDueDate", Order = 84)]
	public DateTime? arlRetentionDueDate { get; set; }

	[JsonProperty("arlRetentionPercent", Order = 85)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlRetentionPercent { get; set; }

	[JsonProperty("arlRmaClaimID", Order = 86)]
	[MaxLength(10)]
	public string arlRmaClaimID { get; set; }

	[JsonProperty("arlRmaClaimLineID", Order = 87)]
	public short arlRmaClaimLineID { get; set; }

	[JsonProperty("arlRmaReceiptID", Order = 88)]
	[MaxLength(10)]
	public string arlRmaReceiptID { get; set; }

	[JsonProperty("arlRmaReceiptLineID", Order = 89)]
	public short arlRmaReceiptLineID { get; set; }

	[JsonProperty("arlRowVersion", Order = 90)]
	public byte[] arlRowVersion { get; set; }

	[JsonProperty("arlSalesOrderDeliveryID", Order = 91)]
	public short arlSalesOrderDeliveryID { get; set; }

	[JsonProperty("arlSalesOrderID", Order = 92)]
	[MaxLength(10)]
	public string arlSalesOrderID { get; set; }

	[JsonProperty("arlSalesOrderLineID", Order = 93)]
	public short arlSalesOrderLineID { get; set; }

	[JsonProperty("arlSecondTaxAmountBase", Order = 94)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlSecondTaxAmountBase { get; set; }

	[JsonProperty("arlSecondTaxAmountForeign", Order = 95)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlSecondTaxAmountForeign { get; set; }

	[JsonProperty("arlSecondTaxCodeID", Order = 96)]
	[MaxLength(5)]
	public string arlSecondTaxCodeID { get; set; }

	[JsonProperty("arlArInvoiceLineID", Order = 97)]
	[Required(ErrorMessage = "arlArInvoiceLineID is required.")]
	public short arlArInvoiceLineID { get; set; }

	[JsonProperty("arlShipmentID", Order = 98)]
	[MaxLength(10)]
	public string arlShipmentID { get; set; }

	[JsonProperty("arlShipmentLineID", Order = 99)]
	public short arlShipmentLineID { get; set; }

	[JsonProperty("arlTaxAmountBase", Order = 100)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlTaxAmountBase { get; set; }

	[JsonProperty("arlTaxAmountForeign", Order = 101)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlTaxAmountForeign { get; set; }

	[JsonProperty("arlTaxCodeID", Order = 102)]
	[MaxLength(5)]
	public string arlTaxCodeID { get; set; }

	[JsonProperty("arlTaxDate", Order = 103)]
	public DateTime? arlTaxDate { get; set; }

	[JsonProperty("arlUnitDiscountBase", Order = 104)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlUnitDiscountBase { get; set; }

	[JsonProperty("arlUnitDiscountForeign", Order = 105)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlUnitDiscountForeign { get; set; }

	[JsonProperty("arlUnitOfMeasure", Order = 106)]
	[MaxLength(2)]
	public string arlUnitOfMeasure { get; set; }

	[JsonProperty("arlUnitPriceBase", Order = 107)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlUnitPriceBase { get; set; }

	[JsonProperty("arlUnitPriceForeign", Order = 108)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arlUnitPriceForeign { get; set; }

	[JsonProperty("customFields", Order = 109)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
