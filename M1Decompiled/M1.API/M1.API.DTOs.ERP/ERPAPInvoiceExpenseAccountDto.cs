using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPInvoiceExpenseAccountDto
{
	[JsonProperty("apxAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apxAmount { get; set; }

	[JsonProperty("apxApInvoiceID", Order = 2)]
	[Required(ErrorMessage = "apxApInvoiceID is required.")]
	[MaxLength(10)]
	public string apxApInvoiceID { get; set; }

	[JsonProperty("apxApInvoiceLineID", Order = 3)]
	[Required(ErrorMessage = "apxApInvoiceLineID is required.")]
	public short apxApInvoiceLineID { get; set; }

	[JsonProperty("apxCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string apxCreatedBy { get; set; }

	[JsonProperty("apxCreatedDate", Order = 5)]
	public DateTime? apxCreatedDate { get; set; }

	[JsonProperty("apxUniqueID", Order = 6)]
	public Guid apxUniqueID { get; set; }

	[JsonProperty("apxExpenseGlAccountID", Order = 7)]
	[Required(ErrorMessage = "apxExpenseGlAccountID is required.")]
	[MaxLength(11)]
	public string apxExpenseGlAccountID { get; set; }

	[JsonProperty("apxPostedToGl", Order = 8)]
	public bool apxPostedToGl { get; set; }

	[JsonProperty("apxPercent", Order = 9)]
	[Range(0.0, 9999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apxPercent { get; set; }

	[JsonProperty("apxRowVersion", Order = 10)]
	public byte[] apxRowVersion { get; set; }

	[JsonProperty("apxApInvoiceExpenseAccountID", Order = 11)]
	[Required(ErrorMessage = "apxApInvoiceExpenseAccountID is required.")]
	public short apxApInvoiceExpenseAccountID { get; set; }

	[JsonProperty("apxSourceTableName", Order = 12)]
	[MaxLength(30)]
	public string apxSourceTableName { get; set; }

	[JsonProperty("apxSourceTableUniqueID", Order = 13)]
	public Guid apxSourceTableUniqueID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
