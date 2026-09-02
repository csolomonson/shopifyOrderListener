using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPToolMovementDto
{
	[JsonProperty("xtaCheckedOutToEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string xtaCheckedOutToEmployeeID { get; set; }

	[JsonProperty("xtaCheckoutReasonID", Order = 2)]
	[MaxLength(10)]
	public string xtaCheckoutReasonID { get; set; }

	[JsonProperty("xtaCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string xtaCreatedBy { get; set; }

	[JsonProperty("xtaCreatedDate", Order = 4)]
	public DateTime? xtaCreatedDate { get; set; }

	[JsonProperty("xtaUniqueID", Order = 5)]
	public Guid xtaUniqueID { get; set; }

	[JsonProperty("xtaJobID", Order = 6)]
	[MaxLength(20)]
	public string xtaJobID { get; set; }

	[JsonProperty("xtaLocation", Order = 7)]
	[MaxLength(30)]
	public string xtaLocation { get; set; }

	[JsonProperty("xtaMovementDate", Order = 8)]
	[Required(ErrorMessage = "xtaMovementDate is required.")]
	public DateTime? xtaMovementDate { get; set; }

	[JsonProperty("xtaMovementType", Order = 9)]
	[Required(ErrorMessage = "xtaMovementType is required.")]
	[MaxLength(10)]
	public string xtaMovementType { get; set; }

	[JsonProperty("xtaNotesRTF", Order = 10)]
	[MaxLength(50)]
	public string xtaNotesRTF { get; set; }

	[JsonProperty("xtaNotesText", Order = 11)]
	[MaxLength(50)]
	public string xtaNotesText { get; set; }

	[JsonProperty("xtaPlannedReturnDate", Order = 12)]
	public DateTime? xtaPlannedReturnDate { get; set; }

	[JsonProperty("xtaPlantDepartmentID", Order = 13)]
	[MaxLength(5)]
	public string xtaPlantDepartmentID { get; set; }

	[JsonProperty("xtaPlantID", Order = 14)]
	[MaxLength(5)]
	public string xtaPlantID { get; set; }

	[JsonProperty("xtaProductionDepartmentID", Order = 15)]
	[MaxLength(5)]
	public string xtaProductionDepartmentID { get; set; }

	[JsonProperty("xtaRowVersion", Order = 16)]
	public byte[] xtaRowVersion { get; set; }

	[JsonProperty("xtaToolMovementID", Order = 17)]
	[Required(ErrorMessage = "xtaToolMovementID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xtaToolMovementID { get; set; }

	[JsonProperty("xtaToolID", Order = 18)]
	[Required(ErrorMessage = "xtaToolID is required.")]
	[MaxLength(10)]
	public string xtaToolID { get; set; }

	[JsonProperty("xtaWorkCenterID", Order = 19)]
	[MaxLength(5)]
	public string xtaWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 20)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
