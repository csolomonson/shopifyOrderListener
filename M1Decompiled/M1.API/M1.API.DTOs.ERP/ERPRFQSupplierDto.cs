using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRFQSupplierDto
{
	[JsonProperty("rqsCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string rqsCreatedBy { get; set; }

	[JsonProperty("rqsCreatedDate", Order = 2)]
	public DateTime? rqsCreatedDate { get; set; }

	[JsonProperty("rqsCurrencyRateID", Order = 3)]
	[MaxLength(5)]
	public string rqsCurrencyRateID { get; set; }

	[JsonProperty("rqsDueDate", Order = 4)]
	public DateTime? rqsDueDate { get; set; }

	[JsonProperty("rqsUniqueID", Order = 5)]
	public Guid rqsUniqueID { get; set; }

	[JsonProperty("rqsExchangeRate", Order = 6)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rqsExchangeRate { get; set; }

	[JsonProperty("rqsClosed", Order = 7)]
	public bool rqsClosed { get; set; }

	[JsonProperty("rqsComplete", Order = 8)]
	public bool rqsComplete { get; set; }

	[JsonProperty("rqsCustomRate", Order = 9)]
	public bool rqsCustomRate { get; set; }

	[JsonProperty("rqsSelectedSupplier", Order = 10)]
	public bool rqsSelectedSupplier { get; set; }

	[JsonProperty("rqsUpdatedPartPrices", Order = 11)]
	public bool rqsUpdatedPartPrices { get; set; }

	[JsonProperty("rqsOrgPartID", Order = 12)]
	[MaxLength(30)]
	public string rqsOrgPartID { get; set; }

	[JsonProperty("rqsPurchaseContactID", Order = 13)]
	[MaxLength(5)]
	public string rqsPurchaseContactID { get; set; }

	[JsonProperty("rqsPurchaseLocationID", Order = 14)]
	[MaxLength(5)]
	public string rqsPurchaseLocationID { get; set; }

	[JsonProperty("rqsRfqID", Order = 15)]
	[Required(ErrorMessage = "rqsRfqID is required.")]
	[MaxLength(10)]
	public string rqsRfqID { get; set; }

	[JsonProperty("rqsRfqLineID", Order = 16)]
	[Required(ErrorMessage = "rqsRfqLineID is required.")]
	public short rqsRfqLineID { get; set; }

	[JsonProperty("rqsRowVersion", Order = 17)]
	public byte[] rqsRowVersion { get; set; }

	[JsonProperty("rqsSelectedSupplierDate", Order = 18)]
	public DateTime? rqsSelectedSupplierDate { get; set; }

	[JsonProperty("rqsRfqSupplierID", Order = 19)]
	[Required(ErrorMessage = "rqsRfqSupplierID is required.")]
	public short rqsRfqSupplierID { get; set; }

	[JsonProperty("rqsSupplierOrganizationID", Order = 20)]
	[Required(ErrorMessage = "rqsSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string rqsSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 21)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
