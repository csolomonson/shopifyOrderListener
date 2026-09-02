using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDMRClaimDto
{
	[JsonProperty("dmpApInvoiceContactID", Order = 1)]
	[MaxLength(5)]
	public string dmpApInvoiceContactID { get; set; }

	[JsonProperty("dmpApInvoiceLocationID", Order = 2)]
	[MaxLength(5)]
	public string dmpApInvoiceLocationID { get; set; }

	[JsonProperty("dmpAuthorizationDate", Order = 3)]
	public DateTime? dmpAuthorizationDate { get; set; }

	[JsonProperty("dmpAuthorizationNumber", Order = 4)]
	[MaxLength(20)]
	public string dmpAuthorizationNumber { get; set; }

	[JsonProperty("dmpAuthorizedByEmployeeID", Order = 5)]
	[MaxLength(10)]
	public string dmpAuthorizedByEmployeeID { get; set; }

	[JsonProperty("dmpClaimDate", Order = 6)]
	[Required(ErrorMessage = "dmpClaimDate is required.")]
	public DateTime? dmpClaimDate { get; set; }

	[JsonProperty("dmpClaimTotal", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmpClaimTotal { get; set; }

	[JsonProperty("dmpClaimTotalForeign", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmpClaimTotalForeign { get; set; }

	[JsonProperty("dmpClosedDate", Order = 9)]
	public DateTime? dmpClosedDate { get; set; }

	[JsonProperty("dmpClosedReasonID", Order = 10)]
	[MaxLength(5)]
	public string dmpClosedReasonID { get; set; }

	[JsonProperty("dmpDmrClaimID", Order = 11)]
	[Required(ErrorMessage = "dmpDmrClaimID is required.")]
	[MaxLength(10)]
	public string dmpDmrClaimID { get; set; }

	[JsonProperty("dmpCreatedBy", Order = 12)]
	[MaxLength(20)]
	public string dmpCreatedBy { get; set; }

	[JsonProperty("dmpCreatedDate", Order = 13)]
	public DateTime? dmpCreatedDate { get; set; }

	[JsonProperty("dmpCurrencyRateID", Order = 14)]
	[MaxLength(5)]
	public string dmpCurrencyRateID { get; set; }

	[JsonProperty("dmpUniqueID", Order = 15)]
	public Guid dmpUniqueID { get; set; }

	[JsonProperty("dmpExchangeRate", Order = 16)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmpExchangeRate { get; set; }

	[JsonProperty("dmpCustomRate", Order = 17)]
	public bool dmpCustomRate { get; set; }

	[JsonProperty("dmpPlantDepartmentID", Order = 18)]
	[MaxLength(5)]
	public string dmpPlantDepartmentID { get; set; }

	[JsonProperty("dmpPlantID", Order = 19)]
	[MaxLength(5)]
	public string dmpPlantID { get; set; }

	[JsonProperty("dmpProcessedByEmployeeID", Order = 20)]
	[MaxLength(10)]
	public string dmpProcessedByEmployeeID { get; set; }

	[JsonProperty("dmpProjectID", Order = 21)]
	[MaxLength(10)]
	public string dmpProjectID { get; set; }

	[JsonProperty("dmpPurchaseContactID", Order = 22)]
	[MaxLength(5)]
	public string dmpPurchaseContactID { get; set; }

	[JsonProperty("dmpPurchaseLocationID", Order = 23)]
	[MaxLength(5)]
	public string dmpPurchaseLocationID { get; set; }

	[JsonProperty("dmpReference", Order = 24)]
	[MaxLength(30)]
	public string dmpReference { get; set; }

	[JsonProperty("dmpRequestedDate", Order = 25)]
	public DateTime? dmpRequestedDate { get; set; }

	[JsonProperty("dmpRowVersion", Order = 26)]
	public byte[] dmpRowVersion { get; set; }

	[JsonProperty("dmpStatus", Order = 27)]
	[Required(ErrorMessage = "dmpStatus is required.")]
	[MaxLength(1)]
	public string dmpStatus { get; set; }

	[JsonProperty("dmpSupplierOrganizationID", Order = 28)]
	[Required(ErrorMessage = "dmpSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string dmpSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 29)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
