using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLeadDto
{
	[JsonProperty("lopClosedByEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string lopClosedByEmployeeID { get; set; }

	[JsonProperty("lopClosedDate", Order = 2)]
	public DateTime? lopClosedDate { get; set; }

	[JsonProperty("lopClosedReasonID", Order = 3)]
	[MaxLength(5)]
	public string lopClosedReasonID { get; set; }

	[JsonProperty("lopLeadID", Order = 4)]
	[Required(ErrorMessage = "lopLeadID is required.")]
	[MaxLength(10)]
	public string lopLeadID { get; set; }

	[JsonProperty("lopContactID", Order = 5)]
	[MaxLength(5)]
	public string lopContactID { get; set; }

	[JsonProperty("lopCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string lopCreatedBy { get; set; }

	[JsonProperty("lopCreatedDate", Order = 7)]
	public DateTime? lopCreatedDate { get; set; }

	[JsonProperty("lopCurrencyRateID", Order = 8)]
	[MaxLength(5)]
	public string lopCurrencyRateID { get; set; }

	[JsonProperty("lopCustomerOrganizationID", Order = 9)]
	[Required(ErrorMessage = "lopCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string lopCustomerOrganizationID { get; set; }

	[JsonProperty("lopUniqueID", Order = 10)]
	public Guid lopUniqueID { get; set; }

	[JsonProperty("lopExchangeRate", Order = 11)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lopExchangeRate { get; set; }

	[JsonProperty("lopExpectedCloseDate", Order = 12)]
	public DateTime? lopExpectedCloseDate { get; set; }

	[JsonProperty("lopExpirationDate", Order = 13)]
	public DateTime? lopExpirationDate { get; set; }

	[JsonProperty("lopCreatedFromMobile", Order = 14)]
	public bool lopCreatedFromMobile { get; set; }

	[JsonProperty("lopCustomRate", Order = 15)]
	public bool lopCustomRate { get; set; }

	[JsonProperty("lopLeadDate", Order = 16)]
	[Required(ErrorMessage = "lopLeadDate is required.")]
	public DateTime? lopLeadDate { get; set; }

	[JsonProperty("lopLeadTotal", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lopLeadTotal { get; set; }

	[JsonProperty("lopLeadTotalForeign", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lopLeadTotalForeign { get; set; }

	[JsonProperty("lopLocationID", Order = 19)]
	[MaxLength(5)]
	public string lopLocationID { get; set; }

	[JsonProperty("lopLongDescriptionRtf", Order = 20)]
	public string lopLongDescriptionRtf { get; set; }

	[JsonProperty("lopLongDescriptionText", Order = 21)]
	public string lopLongDescriptionText { get; set; }

	[JsonProperty("lopMarketingProgramID", Order = 22)]
	[MaxLength(5)]
	public string lopMarketingProgramID { get; set; }

	[JsonProperty("lopMilestoneDate", Order = 23)]
	[Required(ErrorMessage = "lopMilestoneDate is required.")]
	public DateTime? lopMilestoneDate { get; set; }

	[JsonProperty("lopMilestoneID", Order = 24)]
	[Required(ErrorMessage = "lopMilestoneID is required.")]
	[MaxLength(5)]
	public string lopMilestoneID { get; set; }

	[JsonProperty("lopPlantDepartmentID", Order = 25)]
	[MaxLength(5)]
	public string lopPlantDepartmentID { get; set; }

	[JsonProperty("lopPlantID", Order = 26)]
	[MaxLength(5)]
	public string lopPlantID { get; set; }

	[JsonProperty("lopProjectAreaID", Order = 27)]
	[MaxLength(15)]
	public string lopProjectAreaID { get; set; }

	[JsonProperty("lopProjectID", Order = 28)]
	[MaxLength(10)]
	public string lopProjectID { get; set; }

	[JsonProperty("lopQuoteContactID", Order = 29)]
	[MaxLength(5)]
	public string lopQuoteContactID { get; set; }

	[JsonProperty("lopQuoteLocationID", Order = 30)]
	[MaxLength(5)]
	public string lopQuoteLocationID { get; set; }

	[JsonProperty("lopQuoterEmployeeID", Order = 31)]
	[Required(ErrorMessage = "lopQuoterEmployeeID is required.")]
	[MaxLength(10)]
	public string lopQuoterEmployeeID { get; set; }

	[JsonProperty("lopReferredBy", Order = 32)]
	[MaxLength(50)]
	public string lopReferredBy { get; set; }

	[JsonProperty("lopResponseMethodID", Order = 33)]
	[Required(ErrorMessage = "lopResponseMethodID is required.")]
	[MaxLength(5)]
	public string lopResponseMethodID { get; set; }

	[JsonProperty("lopRowVersion", Order = 34)]
	public byte[] lopRowVersion { get; set; }

	[JsonProperty("lopShipContactID", Order = 35)]
	[MaxLength(5)]
	public string lopShipContactID { get; set; }

	[JsonProperty("lopShipLocationID", Order = 36)]
	[MaxLength(5)]
	public string lopShipLocationID { get; set; }

	[JsonProperty("lopShipOrganizationID", Order = 37)]
	[Required(ErrorMessage = "lopShipOrganizationID is required.")]
	[MaxLength(10)]
	public string lopShipOrganizationID { get; set; }

	[JsonProperty("lopShortDescription", Order = 38)]
	[Required(ErrorMessage = "lopShortDescription is required.")]
	[MaxLength(50)]
	public string lopShortDescription { get; set; }

	[JsonProperty("lopSplitPercentTotal", Order = 39)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lopSplitPercentTotal { get; set; }

	[JsonProperty("lopStatus", Order = 40)]
	[Required(ErrorMessage = "lopStatus is required.")]
	[MaxLength(1)]
	public string lopStatus { get; set; }

	[JsonProperty("customFields", Order = 41)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
