using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseTransferLineDto
{
	[JsonProperty("mwlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string mwlCreatedBy { get; set; }

	[JsonProperty("mwlCreatedDate", Order = 2)]
	public DateTime? mwlCreatedDate { get; set; }

	[JsonProperty("mwlDestinationWarehouseID", Order = 3)]
	[MaxLength(5)]
	public string mwlDestinationWarehouseID { get; set; }

	[JsonProperty("mwlUniqueID", Order = 4)]
	public Guid mwlUniqueID { get; set; }

	[JsonProperty("mwlClosed", Order = 5)]
	public bool mwlClosed { get; set; }

	[JsonProperty("mwlKitPart", Order = 6)]
	public bool mwlKitPart { get; set; }

	[JsonProperty("mwlPosted", Order = 7)]
	public bool mwlPosted { get; set; }

	[JsonProperty("mwlReceivedComplete", Order = 8)]
	public bool mwlReceivedComplete { get; set; }

	[JsonProperty("mwlReversed", Order = 9)]
	public bool mwlReversed { get; set; }

	[JsonProperty("mwlShippedComplete", Order = 10)]
	public bool mwlShippedComplete { get; set; }

	[JsonProperty("mwlPartDescription", Order = 11)]
	[Required(ErrorMessage = "mwlPartDescription is required.")]
	[MaxLength(50)]
	public string mwlPartDescription { get; set; }

	[JsonProperty("mwlPartID", Order = 12)]
	[Required(ErrorMessage = "mwlPartID is required.")]
	[MaxLength(30)]
	public string mwlPartID { get; set; }

	[JsonProperty("mwlPartRevisionID", Order = 13)]
	[MaxLength(15)]
	public string mwlPartRevisionID { get; set; }

	[JsonProperty("mwlQuantityInTransit", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwlQuantityInTransit { get; set; }

	[JsonProperty("mwlReceivedDate", Order = 15)]
	public DateTime? mwlReceivedDate { get; set; }

	[JsonProperty("mwlReceivedQuantity", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwlReceivedQuantity { get; set; }

	[JsonProperty("mwlReverseWHTransferID", Order = 17)]
	[MaxLength(10)]
	public string mwlReverseWHTransferID { get; set; }

	[JsonProperty("mwlReverseWHTransferLineID", Order = 18)]
	public short mwlReverseWHTransferLineID { get; set; }

	[JsonProperty("mwlRowVersion", Order = 19)]
	public byte[] mwlRowVersion { get; set; }

	[JsonProperty("mwlWarehouseTransferLineID", Order = 20)]
	[Required(ErrorMessage = "mwlWarehouseTransferLineID is required.")]
	public short mwlWarehouseTransferLineID { get; set; }

	[JsonProperty("mwlShipQuantity", Order = 21)]
	[Required(ErrorMessage = "mwlShipQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwlShipQuantity { get; set; }

	[JsonProperty("mwlSourcePartBinID", Order = 22)]
	[MaxLength(15)]
	public string mwlSourcePartBinID { get; set; }

	[JsonProperty("mwlSourceWarehouseID", Order = 23)]
	[MaxLength(5)]
	public string mwlSourceWarehouseID { get; set; }

	[JsonProperty("mwlUnitOfMeasure", Order = 24)]
	[MaxLength(2)]
	public string mwlUnitOfMeasure { get; set; }

	[JsonProperty("mwlWarehouseRequisitionID", Order = 25)]
	[MaxLength(10)]
	public string mwlWarehouseRequisitionID { get; set; }

	[JsonProperty("mwlWarehouseRequisitionLineID", Order = 26)]
	public short mwlWarehouseRequisitionLineID { get; set; }

	[JsonProperty("mwlWarehouseTransferID", Order = 27)]
	[Required(ErrorMessage = "mwlWarehouseTransferID is required.")]
	[MaxLength(10)]
	public string mwlWarehouseTransferID { get; set; }

	[JsonProperty("mwlWROpenQuantity", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwlWROpenQuantity { get; set; }

	[JsonProperty("mwlWRRequestedQuantity", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwlWRRequestedQuantity { get; set; }

	[JsonProperty("customFields", Order = 30)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
