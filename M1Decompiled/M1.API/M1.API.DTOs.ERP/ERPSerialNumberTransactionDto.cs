using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSerialNumberTransactionDto
{
	[JsonProperty("sntCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string sntCreatedBy { get; set; }

	[JsonProperty("sntCreatedDate", Order = 2)]
	public DateTime? sntCreatedDate { get; set; }

	[JsonProperty("sntDmrShipmentID", Order = 3)]
	[MaxLength(10)]
	public string sntDmrShipmentID { get; set; }

	[JsonProperty("sntDmrShipmentLineID", Order = 4)]
	public short sntDmrShipmentLineID { get; set; }

	[JsonProperty("sntUniqueID", Order = 5)]
	public Guid sntUniqueID { get; set; }

	[JsonProperty("sntInspectionID", Order = 6)]
	[MaxLength(10)]
	public string sntInspectionID { get; set; }

	[JsonProperty("sntInspectionLineID", Order = 7)]
	public short sntInspectionLineID { get; set; }

	[JsonProperty("sntInventoryCountID", Order = 8)]
	public int sntInventoryCountID { get; set; }

	[JsonProperty("sntInventoryCountLineID", Order = 9)]
	public int sntInventoryCountLineID { get; set; }

	[JsonProperty("sntInspect", Order = 10)]
	public bool sntInspect { get; set; }

	[JsonProperty("sntNegativeTransaction", Order = 11)]
	public bool sntNegativeTransaction { get; set; }

	[JsonProperty("sntJobAssemblyID", Order = 12)]
	public int sntJobAssemblyID { get; set; }

	[JsonProperty("sntJobID", Order = 13)]
	[MaxLength(20)]
	public string sntJobID { get; set; }

	[JsonProperty("sntJobMaterialComponentID", Order = 14)]
	public int sntJobMaterialComponentID { get; set; }

	[JsonProperty("sntJobMaterialID", Order = 15)]
	public int sntJobMaterialID { get; set; }

	[JsonProperty("sntJobPartBinID", Order = 16)]
	[MaxLength(15)]
	public string sntJobPartBinID { get; set; }

	[JsonProperty("sntJobPartID", Order = 17)]
	[MaxLength(30)]
	public string sntJobPartID { get; set; }

	[JsonProperty("sntJobPartRevisionID", Order = 18)]
	[MaxLength(15)]
	public string sntJobPartRevisionID { get; set; }

	[JsonProperty("sntJobPartWarehouseLocationID", Order = 19)]
	[MaxLength(5)]
	public string sntJobPartWarehouseLocationID { get; set; }

	[JsonProperty("sntJobSerialNumberID", Order = 20)]
	[MaxLength(30)]
	public string sntJobSerialNumberID { get; set; }

	[JsonProperty("sntLandedCostID", Order = 21)]
	[MaxLength(10)]
	public string sntLandedCostID { get; set; }

	[JsonProperty("sntOldTransactionType", Order = 22)]
	[Required(ErrorMessage = "sntOldTransactionType is required.")]
	public byte sntOldTransactionType { get; set; }

	[JsonProperty("sntPartBinID", Order = 23)]
	[Required(ErrorMessage = "sntPartBinID is required.")]
	[MaxLength(15)]
	public string sntPartBinID { get; set; }

	[JsonProperty("sntPartID", Order = 24)]
	[Required(ErrorMessage = "sntPartID is required.")]
	[MaxLength(30)]
	public string sntPartID { get; set; }

	[JsonProperty("sntPartRevisionID", Order = 25)]
	[MaxLength(15)]
	public string sntPartRevisionID { get; set; }

	[JsonProperty("sntPartTransactionID", Order = 26)]
	public int sntPartTransactionID { get; set; }

	[JsonProperty("sntPartWarehouseLocationID", Order = 27)]
	[Required(ErrorMessage = "sntPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string sntPartWarehouseLocationID { get; set; }

	[JsonProperty("sntQuantity", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal sntQuantity { get; set; }

	[JsonProperty("sntReceiptID", Order = 29)]
	[MaxLength(10)]
	public string sntReceiptID { get; set; }

	[JsonProperty("sntReceiptLineID", Order = 30)]
	public short sntReceiptLineID { get; set; }

	[JsonProperty("sntRmaReceiptID", Order = 31)]
	[MaxLength(10)]
	public string sntRmaReceiptID { get; set; }

	[JsonProperty("sntRmaReceiptLineID", Order = 32)]
	public short sntRmaReceiptLineID { get; set; }

	[JsonProperty("sntRowVersion", Order = 33)]
	public byte[] sntRowVersion { get; set; }

	[JsonProperty("sntSerialNumberTransactionID", Order = 34)]
	[Required(ErrorMessage = "sntSerialNumberTransactionID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sntSerialNumberTransactionID { get; set; }

	[JsonProperty("sntSerialNumberID", Order = 35)]
	[Required(ErrorMessage = "sntSerialNumberID is required.")]
	[MaxLength(30)]
	public string sntSerialNumberID { get; set; }

	[JsonProperty("sntShipmentID", Order = 36)]
	[MaxLength(10)]
	public string sntShipmentID { get; set; }

	[JsonProperty("sntShipmentLineID", Order = 37)]
	public short sntShipmentLineID { get; set; }

	[JsonProperty("sntStatus", Order = 38)]
	public byte sntStatus { get; set; }

	[JsonProperty("sntTableName", Order = 39)]
	[MaxLength(30)]
	public string sntTableName { get; set; }

	[JsonProperty("sntTableUniqueID", Order = 40)]
	public Guid sntTableUniqueID { get; set; }

	[JsonProperty("sntTransactionDate", Order = 41)]
	[Required(ErrorMessage = "sntTransactionDate is required.")]
	public DateTime? sntTransactionDate { get; set; }

	[JsonProperty("sntTransactionType", Order = 42)]
	[Required(ErrorMessage = "sntTransactionType is required.")]
	public byte sntTransactionType { get; set; }

	[JsonProperty("sntWarehouseReceiptID", Order = 43)]
	[MaxLength(10)]
	public string sntWarehouseReceiptID { get; set; }

	[JsonProperty("sntWarehouseReceiptLineID", Order = 44)]
	public short sntWarehouseReceiptLineID { get; set; }

	[JsonProperty("sntWarehouseTransferID", Order = 45)]
	[MaxLength(10)]
	public string sntWarehouseTransferID { get; set; }

	[JsonProperty("sntWarehouseTransferLineID", Order = 46)]
	public short sntWarehouseTransferLineID { get; set; }

	[JsonProperty("customFields", Order = 47)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
