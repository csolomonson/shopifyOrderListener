using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLJournalLineDto
{
	[JsonProperty("gllArPaymentHeaderID", Order = 1)]
	public int gllArPaymentHeaderID { get; set; }

	[JsonProperty("gllArPaymentSessionID", Order = 2)]
	public int gllArPaymentSessionID { get; set; }

	[JsonProperty("gllCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string gllCreatedBy { get; set; }

	[JsonProperty("gllCreatedDate", Order = 4)]
	public DateTime? gllCreatedDate { get; set; }

	[JsonProperty("gllCreditAmount", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gllCreditAmount { get; set; }

	[JsonProperty("gllDebitAmount", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gllDebitAmount { get; set; }

	[JsonProperty("gllDescription", Order = 7)]
	[Required(ErrorMessage = "gllDescription is required.")]
	[MaxLength(50)]
	public string gllDescription { get; set; }

	[JsonProperty("gllUniqueID", Order = 8)]
	public Guid gllUniqueID { get; set; }

	[JsonProperty("gllGlAccountID", Order = 9)]
	[Required(ErrorMessage = "gllGlAccountID is required.")]
	[MaxLength(11)]
	public string gllGlAccountID { get; set; }

	[JsonProperty("gllGlFiscalYearID", Order = 10)]
	public short gllGlFiscalYearID { get; set; }

	[JsonProperty("gllGlFiscalYearPeriodID", Order = 11)]
	public byte gllGlFiscalYearPeriodID { get; set; }

	[JsonProperty("gllGlJournalID", Order = 12)]
	[Required(ErrorMessage = "gllGlJournalID is required.")]
	public int gllGlJournalID { get; set; }

	[JsonProperty("gllPosted", Order = 13)]
	public bool gllPosted { get; set; }

	[JsonProperty("gllJobAssemblyID", Order = 14)]
	public int gllJobAssemblyID { get; set; }

	[JsonProperty("gllJobID", Order = 15)]
	[MaxLength(20)]
	public string gllJobID { get; set; }

	[JsonProperty("gllJobMaterialComponentID", Order = 16)]
	public int gllJobMaterialComponentID { get; set; }

	[JsonProperty("gllJobMaterialID", Order = 17)]
	public int gllJobMaterialID { get; set; }

	[JsonProperty("gllJobOperationID", Order = 18)]
	public int gllJobOperationID { get; set; }

	[JsonProperty("gllLocationID", Order = 19)]
	[MaxLength(5)]
	public string gllLocationID { get; set; }

	[JsonProperty("gllOrganizationID", Order = 20)]
	[MaxLength(10)]
	public string gllOrganizationID { get; set; }

	[JsonProperty("gllPartTransactionID", Order = 21)]
	public int gllPartTransactionID { get; set; }

	[JsonProperty("gllReference", Order = 22)]
	[MaxLength(30)]
	public string gllReference { get; set; }

	[JsonProperty("gllRowVersion", Order = 23)]
	public byte[] gllRowVersion { get; set; }

	[JsonProperty("gllGlJournalLineID", Order = 24)]
	[Required(ErrorMessage = "gllGlJournalLineID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int gllGlJournalLineID { get; set; }

	[JsonProperty("gllSourceTableName", Order = 25)]
	[MaxLength(30)]
	public string gllSourceTableName { get; set; }

	[JsonProperty("gllSourceTableUniqueID", Order = 26)]
	public Guid gllSourceTableUniqueID { get; set; }

	[JsonProperty("gllTaxableAmount", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gllTaxableAmount { get; set; }

	[JsonProperty("gllTaxCodeID", Order = 28)]
	[MaxLength(5)]
	public string gllTaxCodeID { get; set; }

	[JsonProperty("gllTransactionAmount", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gllTransactionAmount { get; set; }

	[JsonProperty("gllTransactionDate", Order = 30)]
	public DateTime? gllTransactionDate { get; set; }

	[JsonProperty("gllTransactionType", Order = 31)]
	public byte gllTransactionType { get; set; }

	[JsonProperty("customFields", Order = 32)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
