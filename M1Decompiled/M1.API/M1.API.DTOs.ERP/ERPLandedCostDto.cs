using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLandedCostDto
{
	[JsonProperty("rmcCarrierName", Order = 1)]
	[MaxLength(30)]
	public string rmcCarrierName { get; set; }

	[JsonProperty("rmcClosedDate", Order = 2)]
	public DateTime? rmcClosedDate { get; set; }

	[JsonProperty("rmcLandedCostID", Order = 3)]
	[Required(ErrorMessage = "rmcLandedCostID is required.")]
	[MaxLength(10)]
	public string rmcLandedCostID { get; set; }

	[JsonProperty("rmcConsigneeContactID", Order = 4)]
	[MaxLength(5)]
	public string rmcConsigneeContactID { get; set; }

	[JsonProperty("rmcConsigneeLocationID", Order = 5)]
	[MaxLength(5)]
	public string rmcConsigneeLocationID { get; set; }

	[JsonProperty("rmcConsigneeOrganizationID", Order = 6)]
	[MaxLength(10)]
	public string rmcConsigneeOrganizationID { get; set; }

	[JsonProperty("rmcCreatedBy", Order = 7)]
	[MaxLength(20)]
	public string rmcCreatedBy { get; set; }

	[JsonProperty("rmcCreatedDate", Order = 8)]
	public DateTime? rmcCreatedDate { get; set; }

	[JsonProperty("rmcDischargePoint", Order = 9)]
	[MaxLength(30)]
	public string rmcDischargePoint { get; set; }

	[JsonProperty("rmcUniqueID", Order = 10)]
	public Guid rmcUniqueID { get; set; }

	[JsonProperty("rmcGlFiscalYearID", Order = 11)]
	[Required(ErrorMessage = "rmcGlFiscalYearID is required.")]
	public short rmcGlFiscalYearID { get; set; }

	[JsonProperty("rmcGlFiscalYearPeriodID", Order = 12)]
	[Required(ErrorMessage = "rmcGlFiscalYearPeriodID is required.")]
	public byte rmcGlFiscalYearPeriodID { get; set; }

	[JsonProperty("rmcChargesComplete", Order = 13)]
	public bool rmcChargesComplete { get; set; }

	[JsonProperty("rmcChargesJournalsCreated", Order = 14)]
	public bool rmcChargesJournalsCreated { get; set; }

	[JsonProperty("rmcClosed", Order = 15)]
	public bool rmcClosed { get; set; }

	[JsonProperty("rmcPoInTransitComplete", Order = 16)]
	public bool rmcPoInTransitComplete { get; set; }

	[JsonProperty("rmcPoInTransitJournalsCreated", Order = 17)]
	public bool rmcPoInTransitJournalsCreated { get; set; }

	[JsonProperty("rmcPostedToGl", Order = 18)]
	public bool rmcPostedToGl { get; set; }

	[JsonProperty("rmcReversalEntry", Order = 19)]
	public bool rmcReversalEntry { get; set; }

	[JsonProperty("rmcReversed", Order = 20)]
	public bool rmcReversed { get; set; }

	[JsonProperty("rmcLandedCostChargesTotal", Order = 21)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmcLandedCostChargesTotal { get; set; }

	[JsonProperty("rmcLandedCostDate", Order = 22)]
	[Required(ErrorMessage = "rmcLandedCostDate is required.")]
	public DateTime? rmcLandedCostDate { get; set; }

	[JsonProperty("rmcLandedCostPurchasesTotal", Order = 23)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmcLandedCostPurchasesTotal { get; set; }

	[JsonProperty("rmcLandedCostReceiptsTotal", Order = 24)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmcLandedCostReceiptsTotal { get; set; }

	[JsonProperty("rmcLandedCostTotal", Order = 25)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmcLandedCostTotal { get; set; }

	[JsonProperty("rmcLoadingPoint", Order = 26)]
	[MaxLength(30)]
	public string rmcLoadingPoint { get; set; }

	[JsonProperty("rmcLongDescriptionRtf", Order = 27)]
	public string rmcLongDescriptionRtf { get; set; }

	[JsonProperty("rmcLongDescriptionText", Order = 28)]
	public string rmcLongDescriptionText { get; set; }

	[JsonProperty("rmcPlantDepartmentID", Order = 29)]
	[MaxLength(5)]
	public string rmcPlantDepartmentID { get; set; }

	[JsonProperty("rmcPlantID", Order = 30)]
	[MaxLength(5)]
	public string rmcPlantID { get; set; }

	[JsonProperty("rmcPostedDate", Order = 31)]
	public DateTime? rmcPostedDate { get; set; }

	[JsonProperty("rmcReverseLandedCostID", Order = 32)]
	[MaxLength(10)]
	public string rmcReverseLandedCostID { get; set; }

	[JsonProperty("rmcRowVersion", Order = 33)]
	public byte[] rmcRowVersion { get; set; }

	[JsonProperty("rmcShipContactID", Order = 34)]
	[MaxLength(5)]
	public string rmcShipContactID { get; set; }

	[JsonProperty("rmcShipLocationID", Order = 35)]
	[MaxLength(5)]
	public string rmcShipLocationID { get; set; }

	[JsonProperty("rmcShipOrganizationID", Order = 36)]
	[MaxLength(10)]
	public string rmcShipOrganizationID { get; set; }

	[JsonProperty("rmcTrackingNumber", Order = 37)]
	[MaxLength(30)]
	public string rmcTrackingNumber { get; set; }

	[JsonProperty("customFields", Order = 38)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
