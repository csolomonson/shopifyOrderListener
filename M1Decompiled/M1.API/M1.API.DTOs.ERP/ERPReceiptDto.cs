using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPReceiptDto
{
	[JsonProperty("rmpApInvoiceContactID", Order = 1)]
	[MaxLength(5)]
	public string rmpApInvoiceContactID { get; set; }

	[JsonProperty("rmpApInvoiceLocationID", Order = 2)]
	[MaxLength(5)]
	public string rmpApInvoiceLocationID { get; set; }

	[JsonProperty("rmpClosedDate", Order = 3)]
	public DateTime? rmpClosedDate { get; set; }

	[JsonProperty("rmpReceiptID", Order = 4)]
	[Required(ErrorMessage = "rmpReceiptID is required.")]
	[MaxLength(10)]
	public string rmpReceiptID { get; set; }

	[JsonProperty("rmpCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string rmpCreatedBy { get; set; }

	[JsonProperty("rmpCreatedDate", Order = 6)]
	public DateTime? rmpCreatedDate { get; set; }

	[JsonProperty("rmpCurrencyRateID", Order = 7)]
	[MaxLength(5)]
	public string rmpCurrencyRateID { get; set; }

	[JsonProperty("rmpDeliveryDocket", Order = 8)]
	[MaxLength(20)]
	public string rmpDeliveryDocket { get; set; }

	[JsonProperty("rmpUniqueID", Order = 9)]
	public Guid rmpUniqueID { get; set; }

	[JsonProperty("rmpExchangeRate", Order = 10)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpExchangeRate { get; set; }

	[JsonProperty("rmpFreightCharge", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpFreightCharge { get; set; }

	[JsonProperty("rmpFreightChargeForeign", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpFreightChargeForeign { get; set; }

	[JsonProperty("rmpClosed", Order = 13)]
	public bool rmpClosed { get; set; }

	[JsonProperty("rmpCustomRate", Order = 14)]
	public bool rmpCustomRate { get; set; }

	[JsonProperty("rmpNestlinkProcessed", Order = 15)]
	public bool rmpNestlinkProcessed { get; set; }

	[JsonProperty("rmpPostedToGl", Order = 16)]
	public bool rmpPostedToGl { get; set; }

	[JsonProperty("rmpReversalEntry", Order = 17)]
	public bool rmpReversalEntry { get; set; }

	[JsonProperty("rmpReversed", Order = 18)]
	public bool rmpReversed { get; set; }

	[JsonProperty("rmpLandedCostID", Order = 19)]
	[MaxLength(10)]
	public string rmpLandedCostID { get; set; }

	[JsonProperty("rmpPlantDepartmentID", Order = 20)]
	[MaxLength(5)]
	public string rmpPlantDepartmentID { get; set; }

	[JsonProperty("rmpPlantID", Order = 21)]
	[MaxLength(5)]
	public string rmpPlantID { get; set; }

	[JsonProperty("rmpPostedDate", Order = 22)]
	public DateTime? rmpPostedDate { get; set; }

	[JsonProperty("rmpProjectID", Order = 23)]
	[MaxLength(10)]
	public string rmpProjectID { get; set; }

	[JsonProperty("rmpPurchaseContactID", Order = 24)]
	[MaxLength(5)]
	public string rmpPurchaseContactID { get; set; }

	[JsonProperty("rmpPurchaseLocationID", Order = 25)]
	[MaxLength(5)]
	public string rmpPurchaseLocationID { get; set; }

	[JsonProperty("rmpReceiptDate", Order = 26)]
	[Required(ErrorMessage = "rmpReceiptDate is required.")]
	public DateTime? rmpReceiptDate { get; set; }

	[JsonProperty("rmpReceiptSubtotal", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpReceiptSubtotal { get; set; }

	[JsonProperty("rmpReceiptSubtotalForeign", Order = 28)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpReceiptSubtotalForeign { get; set; }

	[JsonProperty("rmpReceiptTotal", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpReceiptTotal { get; set; }

	[JsonProperty("rmpReceiptTotalForeign", Order = 30)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmpReceiptTotalForeign { get; set; }

	[JsonProperty("rmpRowVersion", Order = 31)]
	public byte[] rmpRowVersion { get; set; }

	[JsonProperty("rmpShippingMethodID", Order = 32)]
	[MaxLength(5)]
	public string rmpShippingMethodID { get; set; }

	[JsonProperty("rmpSupplierOrganizationID", Order = 33)]
	[Required(ErrorMessage = "rmpSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string rmpSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 34)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
