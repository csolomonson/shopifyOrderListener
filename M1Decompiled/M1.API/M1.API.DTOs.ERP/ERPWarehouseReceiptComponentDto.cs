using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseReceiptComponentDto
{
	[JsonProperty("wroAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wroAdditionalQuantity { get; set; }

	[JsonProperty("wroCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string wroCreatedBy { get; set; }

	[JsonProperty("wroCreatedDate", Order = 3)]
	public DateTime? wroCreatedDate { get; set; }

	[JsonProperty("wroDescription", Order = 4)]
	[Required(ErrorMessage = "wroDescription is required.")]
	[MaxLength(50)]
	public string wroDescription { get; set; }

	[JsonProperty("wroDestinationPartBinID", Order = 5)]
	[MaxLength(15)]
	public string wroDestinationPartBinID { get; set; }

	[JsonProperty("wroDestinationWarehouseID", Order = 6)]
	[MaxLength(5)]
	public string wroDestinationWarehouseID { get; set; }

	[JsonProperty("wroUniqueID", Order = 7)]
	public Guid wroUniqueID { get; set; }

	[JsonProperty("wroClosed", Order = 8)]
	public bool wroClosed { get; set; }

	[JsonProperty("wroPosted", Order = 9)]
	public bool wroPosted { get; set; }

	[JsonProperty("wroReceivedComplete", Order = 10)]
	public bool wroReceivedComplete { get; set; }

	[JsonProperty("wroReversed", Order = 11)]
	public bool wroReversed { get; set; }

	[JsonProperty("wroParentQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wroParentQuantity { get; set; }

	[JsonProperty("wroPartID", Order = 13)]
	[Required(ErrorMessage = "wroPartID is required.")]
	[MaxLength(30)]
	public string wroPartID { get; set; }

	[JsonProperty("wroPartRevisionID", Order = 14)]
	[MaxLength(15)]
	public string wroPartRevisionID { get; set; }

	[JsonProperty("wroQuantityPerParent", Order = 15)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wroQuantityPerParent { get; set; }

	[JsonProperty("wroQuantityReceived", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wroQuantityReceived { get; set; }

	[JsonProperty("wroReverseWHReceiptCompID", Order = 17)]
	[MaxLength(10)]
	public string wroReverseWHReceiptCompID { get; set; }

	[JsonProperty("wroReverseWHReceiptID", Order = 18)]
	[MaxLength(10)]
	public string wroReverseWHReceiptID { get; set; }

	[JsonProperty("wroReverseWHReceiptLineID", Order = 19)]
	public short wroReverseWHReceiptLineID { get; set; }

	[JsonProperty("wroRowVersion", Order = 20)]
	public byte[] wroRowVersion { get; set; }

	[JsonProperty("wroWarehouseReceiptComponentID", Order = 21)]
	[Required(ErrorMessage = "wroWarehouseReceiptComponentID is required.")]
	public short wroWarehouseReceiptComponentID { get; set; }

	[JsonProperty("wroSourcePartBinID", Order = 22)]
	[MaxLength(15)]
	public string wroSourcePartBinID { get; set; }

	[JsonProperty("wroSourceTableName", Order = 23)]
	[MaxLength(30)]
	public string wroSourceTableName { get; set; }

	[JsonProperty("wroSourceTableUniqueID", Order = 24)]
	public Guid wroSourceTableUniqueID { get; set; }

	[JsonProperty("wroSourceWarehouseID", Order = 25)]
	[MaxLength(5)]
	public string wroSourceWarehouseID { get; set; }

	[JsonProperty("wroUnitOfMeasure", Order = 26)]
	[MaxLength(2)]
	public string wroUnitOfMeasure { get; set; }

	[JsonProperty("wroWarehouseReceiptID", Order = 27)]
	[Required(ErrorMessage = "wroWarehouseReceiptID is required.")]
	[MaxLength(10)]
	public string wroWarehouseReceiptID { get; set; }

	[JsonProperty("wroWarehouseReceiptLineID", Order = 28)]
	[Required(ErrorMessage = "wroWarehouseReceiptLineID is required.")]
	public short wroWarehouseReceiptLineID { get; set; }

	[JsonProperty("wroWarehouseReqComponentID", Order = 29)]
	public short wroWarehouseReqComponentID { get; set; }

	[JsonProperty("wroWarehouseRequisitionID", Order = 30)]
	[MaxLength(10)]
	public string wroWarehouseRequisitionID { get; set; }

	[JsonProperty("wroWarehouseRequisitionLineID", Order = 31)]
	public short wroWarehouseRequisitionLineID { get; set; }

	[JsonProperty("wroWarehouseTransComponentID", Order = 32)]
	public short wroWarehouseTransComponentID { get; set; }

	[JsonProperty("wroWarehouseTransferID", Order = 33)]
	[MaxLength(10)]
	public string wroWarehouseTransferID { get; set; }

	[JsonProperty("wroWarehouseTransferLineID", Order = 34)]
	public short wroWarehouseTransferLineID { get; set; }

	[JsonProperty("wroWeight", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wroWeight { get; set; }

	[JsonProperty("customFields", Order = 36)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
