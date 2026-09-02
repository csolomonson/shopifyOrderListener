using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDMRShipmentDto
{
	[JsonProperty("dspApInvoiceLocationID", Order = 1)]
	[MaxLength(5)]
	public string dspApInvoiceLocationID { get; set; }

	[JsonProperty("dspClosedDate", Order = 2)]
	public DateTime? dspClosedDate { get; set; }

	[JsonProperty("dspDmrShipmentID", Order = 3)]
	[Required(ErrorMessage = "dspDmrShipmentID is required.")]
	[MaxLength(10)]
	public string dspDmrShipmentID { get; set; }

	[JsonProperty("dspCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string dspCreatedBy { get; set; }

	[JsonProperty("dspCreatedDate", Order = 5)]
	public DateTime? dspCreatedDate { get; set; }

	[JsonProperty("dspCurrencyRateID", Order = 6)]
	[MaxLength(5)]
	public string dspCurrencyRateID { get; set; }

	[JsonProperty("dspUniqueID", Order = 7)]
	public Guid dspUniqueID { get; set; }

	[JsonProperty("dspExchangeRate", Order = 8)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dspExchangeRate { get; set; }

	[JsonProperty("dspFreightCharge", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dspFreightCharge { get; set; }

	[JsonProperty("dspFreightChargeForeign", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dspFreightChargeForeign { get; set; }

	[JsonProperty("dspFreightSubtotal", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dspFreightSubtotal { get; set; }

	[JsonProperty("dspFreightTotal", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dspFreightTotal { get; set; }

	[JsonProperty("dspClosed", Order = 13)]
	public bool dspClosed { get; set; }

	[JsonProperty("dspCustomRate", Order = 14)]
	public bool dspCustomRate { get; set; }

	[JsonProperty("dspPosted", Order = 15)]
	public bool dspPosted { get; set; }

	[JsonProperty("dspPrintDmrPackingSlip", Order = 16)]
	public bool dspPrintDmrPackingSlip { get; set; }

	[JsonProperty("dspPrintLabels", Order = 17)]
	public bool dspPrintLabels { get; set; }

	[JsonProperty("dspReversalEntry", Order = 18)]
	public bool dspReversalEntry { get; set; }

	[JsonProperty("dspReversed", Order = 19)]
	public bool dspReversed { get; set; }

	[JsonProperty("dspNumberOfLabels", Order = 20)]
	public short dspNumberOfLabels { get; set; }

	[JsonProperty("dspPlantDepartmentID", Order = 21)]
	[MaxLength(5)]
	public string dspPlantDepartmentID { get; set; }

	[JsonProperty("dspPlantID", Order = 22)]
	[MaxLength(5)]
	public string dspPlantID { get; set; }

	[JsonProperty("dspPostedDate", Order = 23)]
	public DateTime? dspPostedDate { get; set; }

	[JsonProperty("dspProjectID", Order = 24)]
	[MaxLength(10)]
	public string dspProjectID { get; set; }

	[JsonProperty("dspRowVersion", Order = 25)]
	public byte[] dspRowVersion { get; set; }

	[JsonProperty("dspShipContactID", Order = 26)]
	[MaxLength(5)]
	public string dspShipContactID { get; set; }

	[JsonProperty("dspShipDate", Order = 27)]
	[Required(ErrorMessage = "dspShipDate is required.")]
	public DateTime? dspShipDate { get; set; }

	[JsonProperty("dspShipLocationID", Order = 28)]
	[MaxLength(5)]
	public string dspShipLocationID { get; set; }

	[JsonProperty("dspShippingCommentsRTF", Order = 29)]
	[MaxLength(50)]
	public string dspShippingCommentsRTF { get; set; }

	[JsonProperty("dspShippingCommentsText", Order = 30)]
	[MaxLength(50)]
	public string dspShippingCommentsText { get; set; }

	[JsonProperty("dspShippingMethodID", Order = 31)]
	[MaxLength(5)]
	public string dspShippingMethodID { get; set; }

	[JsonProperty("dspShippingPaymentTypeID", Order = 32)]
	[MaxLength(5)]
	public string dspShippingPaymentTypeID { get; set; }

	[JsonProperty("dspStandardMessageID", Order = 33)]
	[MaxLength(10)]
	public string dspStandardMessageID { get; set; }

	[JsonProperty("dspSupplierOrganizationID", Order = 34)]
	[Required(ErrorMessage = "dspSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string dspSupplierOrganizationID { get; set; }

	[JsonProperty("dspTrackingNumber", Order = 35)]
	[MaxLength(30)]
	public string dspTrackingNumber { get; set; }

	[JsonProperty("customFields", Order = 36)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
