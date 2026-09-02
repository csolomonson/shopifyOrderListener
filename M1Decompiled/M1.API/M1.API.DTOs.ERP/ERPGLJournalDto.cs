using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLJournalDto
{
	[JsonProperty("glpApInvoiceID", Order = 1)]
	[MaxLength(10)]
	public string glpApInvoiceID { get; set; }

	[JsonProperty("glpApPaymentHeaderID", Order = 2)]
	public int glpApPaymentHeaderID { get; set; }

	[JsonProperty("glpApPaymentSessionID", Order = 3)]
	public int glpApPaymentSessionID { get; set; }

	[JsonProperty("glpArInvoiceID", Order = 4)]
	[MaxLength(10)]
	public string glpArInvoiceID { get; set; }

	[JsonProperty("glpArPaymentHeaderID", Order = 5)]
	public int glpArPaymentHeaderID { get; set; }

	[JsonProperty("glpArPaymentSessionID", Order = 6)]
	public int glpArPaymentSessionID { get; set; }

	[JsonProperty("glpAssetAdjustmentID", Order = 7)]
	public int glpAssetAdjustmentID { get; set; }

	[JsonProperty("glpAssetID", Order = 8)]
	[MaxLength(10)]
	public string glpAssetID { get; set; }

	[JsonProperty("glpBankStatementID", Order = 9)]
	public int glpBankStatementID { get; set; }

	[JsonProperty("glpCreatedBy", Order = 10)]
	[MaxLength(20)]
	public string glpCreatedBy { get; set; }

	[JsonProperty("glpCreatedDate", Order = 11)]
	public DateTime? glpCreatedDate { get; set; }

	[JsonProperty("glpDescription", Order = 12)]
	[Required(ErrorMessage = "glpDescription is required.")]
	[MaxLength(50)]
	public string glpDescription { get; set; }

	[JsonProperty("glpDetailSource", Order = 13)]
	[Required(ErrorMessage = "glpDetailSource is required.")]
	public byte glpDetailSource { get; set; }

	[JsonProperty("glpDmrShipmentID", Order = 14)]
	[MaxLength(10)]
	public string glpDmrShipmentID { get; set; }

	[JsonProperty("glpUniqueID", Order = 15)]
	public Guid glpUniqueID { get; set; }

	[JsonProperty("glpGlFiscalYearID", Order = 16)]
	[Required(ErrorMessage = "glpGlFiscalYearID is required.")]
	public short glpGlFiscalYearID { get; set; }

	[JsonProperty("glpGlFiscalYearPeriodID", Order = 17)]
	[Required(ErrorMessage = "glpGlFiscalYearPeriodID is required.")]
	public byte glpGlFiscalYearPeriodID { get; set; }

	[JsonProperty("glpPosted", Order = 18)]
	public bool glpPosted { get; set; }

	[JsonProperty("glpReversingEntry", Order = 19)]
	public bool glpReversingEntry { get; set; }

	[JsonProperty("glpJobAssemblyID", Order = 20)]
	public int glpJobAssemblyID { get; set; }

	[JsonProperty("glpJobID", Order = 21)]
	[MaxLength(20)]
	public string glpJobID { get; set; }

	[JsonProperty("glpLandedCostID", Order = 22)]
	[MaxLength(10)]
	public string glpLandedCostID { get; set; }

	[JsonProperty("glpLocationID", Order = 23)]
	[MaxLength(5)]
	public string glpLocationID { get; set; }

	[JsonProperty("glpLongDescriptionRtf", Order = 24)]
	public string glpLongDescriptionRtf { get; set; }

	[JsonProperty("glpLongDescriptionText", Order = 25)]
	public string glpLongDescriptionText { get; set; }

	[JsonProperty("glpOrganizationID", Order = 26)]
	[MaxLength(10)]
	public string glpOrganizationID { get; set; }

	[JsonProperty("glpPostedDate", Order = 27)]
	public DateTime? glpPostedDate { get; set; }

	[JsonProperty("glpReceiptID", Order = 28)]
	[MaxLength(10)]
	public string glpReceiptID { get; set; }

	[JsonProperty("glpReference", Order = 29)]
	[MaxLength(30)]
	public string glpReference { get; set; }

	[JsonProperty("glpRmaReceiptID", Order = 30)]
	[MaxLength(10)]
	public string glpRmaReceiptID { get; set; }

	[JsonProperty("glpRowVersion", Order = 31)]
	public byte[] glpRowVersion { get; set; }

	[JsonProperty("glpGlJournalID", Order = 32)]
	[Required(ErrorMessage = "glpGlJournalID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int glpGlJournalID { get; set; }

	[JsonProperty("glpShipmentID", Order = 33)]
	[MaxLength(10)]
	public string glpShipmentID { get; set; }

	[JsonProperty("glpSource", Order = 34)]
	[Required(ErrorMessage = "glpSource is required.")]
	public byte glpSource { get; set; }

	[JsonProperty("glpTimecardID", Order = 35)]
	public int glpTimecardID { get; set; }

	[JsonProperty("glpTotalCredits", Order = 36)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glpTotalCredits { get; set; }

	[JsonProperty("glpTotalDebits", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glpTotalDebits { get; set; }

	[JsonProperty("glpTransactionDate", Order = 38)]
	[Required(ErrorMessage = "glpTransactionDate is required.")]
	public DateTime? glpTransactionDate { get; set; }

	[JsonProperty("customFields", Order = 39)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
