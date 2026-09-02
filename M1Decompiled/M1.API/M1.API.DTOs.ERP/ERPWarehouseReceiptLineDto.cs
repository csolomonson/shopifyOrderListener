using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseReceiptLineDto
{
	[JsonProperty("wrlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string wrlCreatedBy { get; set; }

	[JsonProperty("wrlCreatedDate", Order = 2)]
	public DateTime? wrlCreatedDate { get; set; }

	[JsonProperty("wrlDestinationPartBinID", Order = 3)]
	[MaxLength(15)]
	public string wrlDestinationPartBinID { get; set; }

	[JsonProperty("wrlDestinationWarehouseID", Order = 4)]
	[MaxLength(5)]
	public string wrlDestinationWarehouseID { get; set; }

	[JsonProperty("wrlUniqueID", Order = 5)]
	public Guid wrlUniqueID { get; set; }

	[JsonProperty("wrlHeatLot", Order = 6)]
	[MaxLength(50)]
	public string wrlHeatLot { get; set; }

	[JsonProperty("wrlClosed", Order = 7)]
	public bool wrlClosed { get; set; }

	[JsonProperty("wrlKitPart", Order = 8)]
	public bool wrlKitPart { get; set; }

	[JsonProperty("wrlPosted", Order = 9)]
	public bool wrlPosted { get; set; }

	[JsonProperty("wrlReceivedComplete", Order = 10)]
	public bool wrlReceivedComplete { get; set; }

	[JsonProperty("wrlReversed", Order = 11)]
	public bool wrlReversed { get; set; }

	[JsonProperty("wrlPartDescription", Order = 12)]
	[Required(ErrorMessage = "wrlPartDescription is required.")]
	[MaxLength(50)]
	public string wrlPartDescription { get; set; }

	[JsonProperty("wrlPartID", Order = 13)]
	[Required(ErrorMessage = "wrlPartID is required.")]
	[MaxLength(30)]
	public string wrlPartID { get; set; }

	[JsonProperty("wrlPartRevisionID", Order = 14)]
	[MaxLength(15)]
	public string wrlPartRevisionID { get; set; }

	[JsonProperty("wrlQuantityReceived", Order = 15)]
	[Required(ErrorMessage = "wrlQuantityReceived is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wrlQuantityReceived { get; set; }

	[JsonProperty("wrlReference", Order = 16)]
	[MaxLength(30)]
	public string wrlReference { get; set; }

	[JsonProperty("wrlReverseWHReceiptID", Order = 17)]
	[MaxLength(10)]
	public string wrlReverseWHReceiptID { get; set; }

	[JsonProperty("wrlReverseWHReceiptLineID", Order = 18)]
	public short wrlReverseWHReceiptLineID { get; set; }

	[JsonProperty("wrlRowVersion", Order = 19)]
	public byte[] wrlRowVersion { get; set; }

	[JsonProperty("wrlWarehouseReceiptLineID", Order = 20)]
	[Required(ErrorMessage = "wrlWarehouseReceiptLineID is required.")]
	public short wrlWarehouseReceiptLineID { get; set; }

	[JsonProperty("wrlSourcePartBinID", Order = 21)]
	[MaxLength(15)]
	public string wrlSourcePartBinID { get; set; }

	[JsonProperty("wrlSourceTableName", Order = 22)]
	[MaxLength(30)]
	public string wrlSourceTableName { get; set; }

	[JsonProperty("wrlSourceTableUniqueID", Order = 23)]
	public Guid wrlSourceTableUniqueID { get; set; }

	[JsonProperty("wrlSourceWarehouseID", Order = 24)]
	[MaxLength(5)]
	public string wrlSourceWarehouseID { get; set; }

	[JsonProperty("wrlUnitCost", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wrlUnitCost { get; set; }

	[JsonProperty("wrlUnitOfMeasure", Order = 26)]
	[MaxLength(2)]
	public string wrlUnitOfMeasure { get; set; }

	[JsonProperty("wrlWarehouseReceiptID", Order = 27)]
	[Required(ErrorMessage = "wrlWarehouseReceiptID is required.")]
	[MaxLength(10)]
	public string wrlWarehouseReceiptID { get; set; }

	[JsonProperty("wrlWarehouseRequisitionID", Order = 28)]
	[MaxLength(10)]
	public string wrlWarehouseRequisitionID { get; set; }

	[JsonProperty("wrlWarehouseRequisitionLineID", Order = 29)]
	public short wrlWarehouseRequisitionLineID { get; set; }

	[JsonProperty("wrlWarehouseTransferID", Order = 30)]
	[MaxLength(10)]
	public string wrlWarehouseTransferID { get; set; }

	[JsonProperty("wrlWarehouseTransferLineID", Order = 31)]
	public short wrlWarehouseTransferLineID { get; set; }

	[JsonProperty("wrlWTOpenQuantity", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wrlWTOpenQuantity { get; set; }

	[JsonProperty("wrlWTShippedQuantity", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wrlWTShippedQuantity { get; set; }

	[JsonProperty("customFields", Order = 34)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
