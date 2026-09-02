using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteDto
{
	[JsonProperty("qmpArInvoiceContactID", Order = 1)]
	[MaxLength(5)]
	public string qmpArInvoiceContactID { get; set; }

	[JsonProperty("qmpArInvoiceLocationID", Order = 2)]
	[MaxLength(5)]
	public string qmpArInvoiceLocationID { get; set; }

	[JsonProperty("qmpClosedDate", Order = 3)]
	public DateTime? qmpClosedDate { get; set; }

	[JsonProperty("qmpQuoteID", Order = 4)]
	[Required(ErrorMessage = "qmpQuoteID is required.")]
	[MaxLength(10)]
	public string qmpQuoteID { get; set; }

	[JsonProperty("qmpCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string qmpCreatedBy { get; set; }

	[JsonProperty("qmpCreatedDate", Order = 6)]
	public DateTime? qmpCreatedDate { get; set; }

	[JsonProperty("qmpCurrencyRateID", Order = 7)]
	[MaxLength(5)]
	public string qmpCurrencyRateID { get; set; }

	[JsonProperty("qmpCustomerOrganizationID", Order = 8)]
	[Required(ErrorMessage = "qmpCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string qmpCustomerOrganizationID { get; set; }

	[JsonProperty("qmpDueDate", Order = 9)]
	[Required(ErrorMessage = "qmpDueDate is required.")]
	public DateTime? qmpDueDate { get; set; }

	[JsonProperty("qmpUniqueID", Order = 10)]
	public Guid qmpUniqueID { get; set; }

	[JsonProperty("qmpExchangeRate", Order = 11)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmpExchangeRate { get; set; }

	[JsonProperty("qmpExpirationDate", Order = 12)]
	public DateTime? qmpExpirationDate { get; set; }

	[JsonProperty("qmpFreeOnBoardDescription", Order = 13)]
	[MaxLength(15)]
	public string qmpFreeOnBoardDescription { get; set; }

	[JsonProperty("qmpAvalaraTaxCalculated", Order = 14)]
	public bool qmpAvalaraTaxCalculated { get; set; }

	[JsonProperty("qmpClosed", Order = 15)]
	public bool qmpClosed { get; set; }

	[JsonProperty("qmpCreatedFromMobile", Order = 16)]
	public bool qmpCreatedFromMobile { get; set; }

	[JsonProperty("qmpCustomRate", Order = 17)]
	public bool qmpCustomRate { get; set; }

	[JsonProperty("qmpPaymentTermID", Order = 18)]
	[MaxLength(5)]
	public string qmpPaymentTermID { get; set; }

	[JsonProperty("qmpPlantDepartmentID", Order = 19)]
	[MaxLength(5)]
	public string qmpPlantDepartmentID { get; set; }

	[JsonProperty("qmpPlantID", Order = 20)]
	[MaxLength(5)]
	public string qmpPlantID { get; set; }

	[JsonProperty("qmpProjectID", Order = 21)]
	[MaxLength(10)]
	public string qmpProjectID { get; set; }

	[JsonProperty("qmpQuoteContactID", Order = 22)]
	[MaxLength(5)]
	public string qmpQuoteContactID { get; set; }

	[JsonProperty("qmpQuoteDate", Order = 23)]
	public DateTime? qmpQuoteDate { get; set; }

	[JsonProperty("qmpQuoteFooterMessageRTF", Order = 24)]
	[MaxLength(50)]
	public string qmpQuoteFooterMessageRTF { get; set; }

	[JsonProperty("qmpQuoteFooterMessageText", Order = 25)]
	[MaxLength(50)]
	public string qmpQuoteFooterMessageText { get; set; }

	[JsonProperty("qmpQuoteHeaderMessageRTF", Order = 26)]
	[MaxLength(50)]
	public string qmpQuoteHeaderMessageRTF { get; set; }

	[JsonProperty("qmpQuoteHeaderMessageText", Order = 27)]
	[MaxLength(50)]
	public string qmpQuoteHeaderMessageText { get; set; }

	[JsonProperty("qmpQuoteLocationID", Order = 28)]
	[MaxLength(5)]
	public string qmpQuoteLocationID { get; set; }

	[JsonProperty("qmpQuoterEmployeeID", Order = 29)]
	[Required(ErrorMessage = "qmpQuoterEmployeeID is required.")]
	[MaxLength(10)]
	public string qmpQuoterEmployeeID { get; set; }

	[JsonProperty("qmpRowVersion", Order = 30)]
	public byte[] qmpRowVersion { get; set; }

	[JsonProperty("qmpShipContactID", Order = 31)]
	[MaxLength(5)]
	public string qmpShipContactID { get; set; }

	[JsonProperty("qmpShipLocationID", Order = 32)]
	[MaxLength(5)]
	public string qmpShipLocationID { get; set; }

	[JsonProperty("qmpShipOrganizationID", Order = 33)]
	[Required(ErrorMessage = "qmpShipOrganizationID is required.")]
	[MaxLength(10)]
	public string qmpShipOrganizationID { get; set; }

	[JsonProperty("qmpShippingMethodID", Order = 34)]
	[MaxLength(5)]
	public string qmpShippingMethodID { get; set; }

	[JsonProperty("qmpShippingPaymentTypeID", Order = 35)]
	[MaxLength(5)]
	public string qmpShippingPaymentTypeID { get; set; }

	[JsonProperty("qmpSplitPercentTotal", Order = 36)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmpSplitPercentTotal { get; set; }

	[JsonProperty("qmpStandardMessageID", Order = 37)]
	[MaxLength(10)]
	public string qmpStandardMessageID { get; set; }

	[JsonProperty("qmpTaxDate", Order = 38)]
	public DateTime? qmpTaxDate { get; set; }

	[JsonProperty("customFields", Order = 39)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
