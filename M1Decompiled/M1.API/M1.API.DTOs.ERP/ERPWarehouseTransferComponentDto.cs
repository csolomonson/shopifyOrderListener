using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseTransferComponentDto
{
	[JsonProperty("mwoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoAdditionalQuantity { get; set; }

	[JsonProperty("mwoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string mwoCreatedBy { get; set; }

	[JsonProperty("mwoCreatedDate", Order = 3)]
	public DateTime? mwoCreatedDate { get; set; }

	[JsonProperty("mwoDescription", Order = 4)]
	[Required(ErrorMessage = "mwoDescription is required.")]
	[MaxLength(50)]
	public string mwoDescription { get; set; }

	[JsonProperty("mwoDestinationWarehouseID", Order = 5)]
	[MaxLength(5)]
	public string mwoDestinationWarehouseID { get; set; }

	[JsonProperty("mwoUniqueID", Order = 6)]
	public Guid mwoUniqueID { get; set; }

	[JsonProperty("mwoClosed", Order = 7)]
	public bool mwoClosed { get; set; }

	[JsonProperty("mwoPosted", Order = 8)]
	public bool mwoPosted { get; set; }

	[JsonProperty("mwoReceivedComplete", Order = 9)]
	public bool mwoReceivedComplete { get; set; }

	[JsonProperty("mwoReversed", Order = 10)]
	public bool mwoReversed { get; set; }

	[JsonProperty("mwoShippedComplete", Order = 11)]
	public bool mwoShippedComplete { get; set; }

	[JsonProperty("mwoParentQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoParentQuantity { get; set; }

	[JsonProperty("mwoPartID", Order = 13)]
	[Required(ErrorMessage = "mwoPartID is required.")]
	[MaxLength(30)]
	public string mwoPartID { get; set; }

	[JsonProperty("mwoPartRevisionID", Order = 14)]
	[MaxLength(15)]
	public string mwoPartRevisionID { get; set; }

	[JsonProperty("mwoQuantityInTransit", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoQuantityInTransit { get; set; }

	[JsonProperty("mwoQuantityPerParent", Order = 16)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoQuantityPerParent { get; set; }

	[JsonProperty("mwoReceivedQuantity", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoReceivedQuantity { get; set; }

	[JsonProperty("mwoReverseWHTransComponentID", Order = 18)]
	public short mwoReverseWHTransComponentID { get; set; }

	[JsonProperty("mwoReverseWHTransferID", Order = 19)]
	[MaxLength(10)]
	public string mwoReverseWHTransferID { get; set; }

	[JsonProperty("mwoReverseWHTransferLineID", Order = 20)]
	public short mwoReverseWHTransferLineID { get; set; }

	[JsonProperty("mwoRowVersion", Order = 21)]
	public byte[] mwoRowVersion { get; set; }

	[JsonProperty("mwoShipQuantity", Order = 22)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoShipQuantity { get; set; }

	[JsonProperty("mwoSourcePartBinID", Order = 23)]
	[MaxLength(15)]
	public string mwoSourcePartBinID { get; set; }

	[JsonProperty("mwoSourceWarehouseID", Order = 24)]
	[MaxLength(5)]
	public string mwoSourceWarehouseID { get; set; }

	[JsonProperty("mwoUnitOfMeasure", Order = 25)]
	[MaxLength(2)]
	public string mwoUnitOfMeasure { get; set; }

	[JsonProperty("mwoWarehouseReqComponentID", Order = 26)]
	public short mwoWarehouseReqComponentID { get; set; }

	[JsonProperty("mwoWarehouseRequisitionID", Order = 27)]
	[MaxLength(10)]
	public string mwoWarehouseRequisitionID { get; set; }

	[JsonProperty("mwoWarehouseRequisitionLineID", Order = 28)]
	public short mwoWarehouseRequisitionLineID { get; set; }

	[JsonProperty("mwoWarehouseTransComponentID", Order = 29)]
	[Required(ErrorMessage = "mwoWarehouseTransComponentID is required.")]
	public short mwoWarehouseTransComponentID { get; set; }

	[JsonProperty("mwoWarehouseTransferID", Order = 30)]
	[Required(ErrorMessage = "mwoWarehouseTransferID is required.")]
	[MaxLength(10)]
	public string mwoWarehouseTransferID { get; set; }

	[JsonProperty("mwoWarehouseTransferLineID", Order = 31)]
	[Required(ErrorMessage = "mwoWarehouseTransferLineID is required.")]
	public short mwoWarehouseTransferLineID { get; set; }

	[JsonProperty("mwoWeight", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwoWeight { get; set; }

	[JsonProperty("customFields", Order = 33)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
