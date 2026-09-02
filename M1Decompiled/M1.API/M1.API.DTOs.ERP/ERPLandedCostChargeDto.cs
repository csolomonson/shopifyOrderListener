using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLandedCostChargeDto
{
	[JsonProperty("rmhApInvoiceID", Order = 1)]
	[MaxLength(10)]
	public string rmhApInvoiceID { get; set; }

	[JsonProperty("rmhApInvoiceLineID", Order = 2)]
	public short rmhApInvoiceLineID { get; set; }

	[JsonProperty("rmhCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string rmhCreatedBy { get; set; }

	[JsonProperty("rmhCreatedDate", Order = 4)]
	public DateTime? rmhCreatedDate { get; set; }

	[JsonProperty("rmhCurrencyRateID", Order = 5)]
	[MaxLength(5)]
	public string rmhCurrencyRateID { get; set; }

	[JsonProperty("rmhDescription", Order = 6)]
	[Required(ErrorMessage = "rmhDescription is required.")]
	[MaxLength(50)]
	public string rmhDescription { get; set; }

	[JsonProperty("rmhUniqueID", Order = 7)]
	public Guid rmhUniqueID { get; set; }

	[JsonProperty("rmhEstExchangeRate", Order = 8)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmhEstExchangeRate { get; set; }

	[JsonProperty("rmhEstTotalCost", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmhEstTotalCost { get; set; }

	[JsonProperty("rmhEstTotalCostForeign", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmhEstTotalCostForeign { get; set; }

	[JsonProperty("rmhExchangeRate", Order = 11)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmhExchangeRate { get; set; }

	[JsonProperty("rmhCustomRate", Order = 12)]
	public bool rmhCustomRate { get; set; }

	[JsonProperty("rmhInTransitJournalsCreated", Order = 13)]
	public bool rmhInTransitJournalsCreated { get; set; }

	[JsonProperty("rmhInvoicedComplete", Order = 14)]
	public bool rmhInvoicedComplete { get; set; }

	[JsonProperty("rmhReversed", Order = 15)]
	public bool rmhReversed { get; set; }

	[JsonProperty("rmhLandedCostCategoryID", Order = 16)]
	[Required(ErrorMessage = "rmhLandedCostCategoryID is required.")]
	[MaxLength(5)]
	public string rmhLandedCostCategoryID { get; set; }

	[JsonProperty("rmhLandedCostID", Order = 17)]
	[Required(ErrorMessage = "rmhLandedCostID is required.")]
	[MaxLength(10)]
	public string rmhLandedCostID { get; set; }

	[JsonProperty("rmhLandedCostMethod", Order = 18)]
	[Required(ErrorMessage = "rmhLandedCostMethod is required.")]
	public byte rmhLandedCostMethod { get; set; }

	[JsonProperty("rmhReverseLandedCostChargeID", Order = 19)]
	public short rmhReverseLandedCostChargeID { get; set; }

	[JsonProperty("rmhReverseLandedCostID", Order = 20)]
	[MaxLength(10)]
	public string rmhReverseLandedCostID { get; set; }

	[JsonProperty("rmhRowVersion", Order = 21)]
	public byte[] rmhRowVersion { get; set; }

	[JsonProperty("rmhLandedCostChargeID", Order = 22)]
	[Required(ErrorMessage = "rmhLandedCostChargeID is required.")]
	public short rmhLandedCostChargeID { get; set; }

	[JsonProperty("rmhSupplierContactID", Order = 23)]
	[MaxLength(5)]
	public string rmhSupplierContactID { get; set; }

	[JsonProperty("rmhSupplierLocationID", Order = 24)]
	[MaxLength(5)]
	public string rmhSupplierLocationID { get; set; }

	[JsonProperty("rmhSupplierOrganizationID", Order = 25)]
	[Required(ErrorMessage = "rmhSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string rmhSupplierOrganizationID { get; set; }

	[JsonProperty("rmhTotalCost", Order = 26)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmhTotalCost { get; set; }

	[JsonProperty("rmhTotalCostForeign", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmhTotalCostForeign { get; set; }

	[JsonProperty("customFields", Order = 28)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
