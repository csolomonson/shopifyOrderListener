using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAClaimDto
{
	[JsonProperty("rapActualHoursTotal", Order = 1)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapActualHoursTotal { get; set; }

	[JsonProperty("rapArInvoiceContactID", Order = 2)]
	[MaxLength(5)]
	public string rapArInvoiceContactID { get; set; }

	[JsonProperty("rapArInvoiceLocationID", Order = 3)]
	[MaxLength(5)]
	public string rapArInvoiceLocationID { get; set; }

	[JsonProperty("rapAuthorizationDate", Order = 4)]
	public DateTime? rapAuthorizationDate { get; set; }

	[JsonProperty("rapAuthorizationNumber", Order = 5)]
	[MaxLength(20)]
	public string rapAuthorizationNumber { get; set; }

	[JsonProperty("rapAuthorizedByEmployeeID", Order = 6)]
	[MaxLength(10)]
	public string rapAuthorizedByEmployeeID { get; set; }

	[JsonProperty("rapClaimDate", Order = 7)]
	[Required(ErrorMessage = "rapClaimDate is required.")]
	public DateTime? rapClaimDate { get; set; }

	[JsonProperty("rapClaimTotal", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapClaimTotal { get; set; }

	[JsonProperty("rapClaimTotalForeign", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapClaimTotalForeign { get; set; }

	[JsonProperty("rapClosedDate", Order = 10)]
	public DateTime? rapClosedDate { get; set; }

	[JsonProperty("rapClosedReasonID", Order = 11)]
	[MaxLength(5)]
	public string rapClosedReasonID { get; set; }

	[JsonProperty("rapRmaClaimID", Order = 12)]
	[Required(ErrorMessage = "rapRmaClaimID is required.")]
	[MaxLength(10)]
	public string rapRmaClaimID { get; set; }

	[JsonProperty("rapCreatedBy", Order = 13)]
	[MaxLength(20)]
	public string rapCreatedBy { get; set; }

	[JsonProperty("rapCreatedDate", Order = 14)]
	public DateTime? rapCreatedDate { get; set; }

	[JsonProperty("rapCurrencyRateID", Order = 15)]
	[MaxLength(5)]
	public string rapCurrencyRateID { get; set; }

	[JsonProperty("rapCustomerOrganizationID", Order = 16)]
	[Required(ErrorMessage = "rapCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string rapCustomerOrganizationID { get; set; }

	[JsonProperty("rapDiscountAmount", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapDiscountAmount { get; set; }

	[JsonProperty("rapDiscountAmountForeign", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapDiscountAmountForeign { get; set; }

	[JsonProperty("rapUniqueID", Order = 19)]
	public Guid rapUniqueID { get; set; }

	[JsonProperty("rapExchangeRate", Order = 20)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapExchangeRate { get; set; }

	[JsonProperty("rapFreightAmount", Order = 21)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapFreightAmount { get; set; }

	[JsonProperty("rapFreightAmountForeign", Order = 22)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapFreightAmountForeign { get; set; }

	[JsonProperty("rapCustomRate", Order = 23)]
	public bool rapCustomRate { get; set; }

	[JsonProperty("rapLaborRate", Order = 24)]
	[Range(0.0, 9999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapLaborRate { get; set; }

	[JsonProperty("rapLaborRateForeign", Order = 25)]
	[Range(0.0, 9999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapLaborRateForeign { get; set; }

	[JsonProperty("rapLaborTotal", Order = 26)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapLaborTotal { get; set; }

	[JsonProperty("rapLaborTotalForeign", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapLaborTotalForeign { get; set; }

	[JsonProperty("rapLongDescriptionRtf", Order = 28)]
	public string rapLongDescriptionRtf { get; set; }

	[JsonProperty("rapLongDescriptionText", Order = 29)]
	public string rapLongDescriptionText { get; set; }

	[JsonProperty("rapPartID", Order = 30)]
	[MaxLength(30)]
	public string rapPartID { get; set; }

	[JsonProperty("rapPartRevisionID", Order = 31)]
	[MaxLength(15)]
	public string rapPartRevisionID { get; set; }

	[JsonProperty("rapPartShortDescription", Order = 32)]
	[MaxLength(50)]
	public string rapPartShortDescription { get; set; }

	[JsonProperty("rapPartsTotal", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapPartsTotal { get; set; }

	[JsonProperty("rapPartsTotalForeign", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapPartsTotalForeign { get; set; }

	[JsonProperty("rapPayTo", Order = 35)]
	[Required(ErrorMessage = "rapPayTo is required.")]
	public byte rapPayTo { get; set; }

	[JsonProperty("rapPlantDepartmentID", Order = 36)]
	[MaxLength(5)]
	public string rapPlantDepartmentID { get; set; }

	[JsonProperty("rapPlantID", Order = 37)]
	[MaxLength(5)]
	public string rapPlantID { get; set; }

	[JsonProperty("rapProcessedByEmployeeID", Order = 38)]
	[Required(ErrorMessage = "rapProcessedByEmployeeID is required.")]
	[MaxLength(10)]
	public string rapProcessedByEmployeeID { get; set; }

	[JsonProperty("rapProjectID", Order = 39)]
	[MaxLength(10)]
	public string rapProjectID { get; set; }

	[JsonProperty("rapReference", Order = 40)]
	[MaxLength(30)]
	public string rapReference { get; set; }

	[JsonProperty("rapRequestedDate", Order = 41)]
	public DateTime? rapRequestedDate { get; set; }

	[JsonProperty("rapResellerContactID", Order = 42)]
	[MaxLength(5)]
	public string rapResellerContactID { get; set; }

	[JsonProperty("rapResellerLocationID", Order = 43)]
	[MaxLength(5)]
	public string rapResellerLocationID { get; set; }

	[JsonProperty("rapResellerOrganizationID", Order = 44)]
	[MaxLength(10)]
	public string rapResellerOrganizationID { get; set; }

	[JsonProperty("rapRowVersion", Order = 45)]
	public byte[] rapRowVersion { get; set; }

	[JsonProperty("rapSerialNumberID", Order = 46)]
	[MaxLength(30)]
	public string rapSerialNumberID { get; set; }

	[JsonProperty("rapShipContactID", Order = 47)]
	[MaxLength(5)]
	public string rapShipContactID { get; set; }

	[JsonProperty("rapShipLocationID", Order = 48)]
	[MaxLength(5)]
	public string rapShipLocationID { get; set; }

	[JsonProperty("rapShipOrganizationID", Order = 49)]
	[Required(ErrorMessage = "rapShipOrganizationID is required.")]
	[MaxLength(10)]
	public string rapShipOrganizationID { get; set; }

	[JsonProperty("rapStatus", Order = 50)]
	[Required(ErrorMessage = "rapStatus is required.")]
	[MaxLength(1)]
	public string rapStatus { get; set; }

	[JsonProperty("rapSubcontractTotal", Order = 51)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapSubcontractTotal { get; set; }

	[JsonProperty("rapSubcontractTotalForeign", Order = 52)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rapSubcontractTotalForeign { get; set; }

	[JsonProperty("customFields", Order = 53)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
