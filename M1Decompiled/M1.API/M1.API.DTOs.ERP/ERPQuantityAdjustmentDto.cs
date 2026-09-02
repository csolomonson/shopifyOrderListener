using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuantityAdjustmentDto
{
	[JsonProperty("inqAdjustmentDate", Order = 1)]
	[Required(ErrorMessage = "inqAdjustmentDate is required.")]
	public DateTime? inqAdjustmentDate { get; set; }

	[JsonProperty("inqAdjustmentDescription", Order = 2)]
	[MaxLength(50)]
	public string inqAdjustmentDescription { get; set; }

	[JsonProperty("inqAdjustmentType", Order = 3)]
	[Required(ErrorMessage = "inqAdjustmentType is required.")]
	public byte inqAdjustmentType { get; set; }

	[JsonProperty("inqBinQuantityReceipted", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqBinQuantityReceipted { get; set; }

	[JsonProperty("inqBinQuantityTransferred", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqBinQuantityTransferred { get; set; }

	[JsonProperty("inqChangeQuantity", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqChangeQuantity { get; set; }

	[JsonProperty("inqQuantityAdjustmentID", Order = 7)]
	[Required(ErrorMessage = "inqQuantityAdjustmentID is required.")]
	[MaxLength(10)]
	public string inqQuantityAdjustmentID { get; set; }

	[JsonProperty("inqCountedQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqCountedQuantity { get; set; }

	[JsonProperty("inqCreatedBy", Order = 9)]
	[MaxLength(20)]
	public string inqCreatedBy { get; set; }

	[JsonProperty("inqCreatedDate", Order = 10)]
	public DateTime? inqCreatedDate { get; set; }

	[JsonProperty("inqCurrentQuantity", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqCurrentQuantity { get; set; }

	[JsonProperty("inqDestinationPartBinID", Order = 12)]
	[MaxLength(15)]
	public string inqDestinationPartBinID { get; set; }

	[JsonProperty("inqDestinationWarehouseID", Order = 13)]
	[MaxLength(5)]
	public string inqDestinationWarehouseID { get; set; }

	[JsonProperty("inqUniqueID", Order = 14)]
	public Guid inqUniqueID { get; set; }

	[JsonProperty("inqPosted", Order = 15)]
	public bool inqPosted { get; set; }

	[JsonProperty("inqNewQuantity", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqNewQuantity { get; set; }

	[JsonProperty("inqPartBinID", Order = 17)]
	[Required(ErrorMessage = "inqPartBinID is required.")]
	[MaxLength(15)]
	public string inqPartBinID { get; set; }

	[JsonProperty("inqPartID", Order = 18)]
	[Required(ErrorMessage = "inqPartID is required.")]
	[MaxLength(30)]
	public string inqPartID { get; set; }

	[JsonProperty("inqPartRevisionID", Order = 19)]
	[MaxLength(15)]
	public string inqPartRevisionID { get; set; }

	[JsonProperty("inqPartShortDescription", Order = 20)]
	[MaxLength(50)]
	public string inqPartShortDescription { get; set; }

	[JsonProperty("inqPartWarehouseLocationID", Order = 21)]
	[Required(ErrorMessage = "inqPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string inqPartWarehouseLocationID { get; set; }

	[JsonProperty("inqPlantDepartmentID", Order = 22)]
	[MaxLength(5)]
	public string inqPlantDepartmentID { get; set; }

	[JsonProperty("inqPlantID", Order = 23)]
	[MaxLength(5)]
	public string inqPlantID { get; set; }

	[JsonProperty("inqPostedDate", Order = 24)]
	public DateTime? inqPostedDate { get; set; }

	[JsonProperty("inqQuantitySince", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inqQuantitySince { get; set; }

	[JsonProperty("inqRowVersion", Order = 26)]
	public byte[] inqRowVersion { get; set; }

	[JsonProperty("inqTransactionsSince", Order = 27)]
	public short inqTransactionsSince { get; set; }

	[JsonProperty("inqUnitOfMeasure", Order = 28)]
	[MaxLength(2)]
	public string inqUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 29)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
