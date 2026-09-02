using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLotNumberTransactionDto
{
	[JsonProperty("abtCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string abtCreatedBy { get; set; }

	[JsonProperty("abtCreatedDate", Order = 2)]
	public DateTime? abtCreatedDate { get; set; }

	[JsonProperty("abtDmrShipmentID", Order = 3)]
	[MaxLength(10)]
	public string abtDmrShipmentID { get; set; }

	[JsonProperty("abtDmrShipmentLineID", Order = 4)]
	public short abtDmrShipmentLineID { get; set; }

	[JsonProperty("abtUniqueID", Order = 5)]
	public Guid abtUniqueID { get; set; }

	[JsonProperty("abtInspectionID", Order = 6)]
	[MaxLength(10)]
	public string abtInspectionID { get; set; }

	[JsonProperty("abtInspectionLineID", Order = 7)]
	public short abtInspectionLineID { get; set; }

	[JsonProperty("abtInventoryCountID", Order = 8)]
	public int abtInventoryCountID { get; set; }

	[JsonProperty("abtInventoryCountLineID", Order = 9)]
	public int abtInventoryCountLineID { get; set; }

	[JsonProperty("abtInProgress", Order = 10)]
	public bool abtInProgress { get; set; }

	[JsonProperty("abtInspect", Order = 11)]
	public bool abtInspect { get; set; }

	[JsonProperty("abtNegativeTransaction", Order = 12)]
	public bool abtNegativeTransaction { get; set; }

	[JsonProperty("abtNonInventoryTransaction", Order = 13)]
	public bool abtNonInventoryTransaction { get; set; }

	[JsonProperty("abtJobAssemblyID", Order = 14)]
	public int abtJobAssemblyID { get; set; }

	[JsonProperty("abtJobID", Order = 15)]
	[MaxLength(20)]
	public string abtJobID { get; set; }

	[JsonProperty("abtJobMaterialComponentID", Order = 16)]
	public int abtJobMaterialComponentID { get; set; }

	[JsonProperty("abtJobMaterialID", Order = 17)]
	public int abtJobMaterialID { get; set; }

	[JsonProperty("abtLandedCostID", Order = 18)]
	[MaxLength(10)]
	public string abtLandedCostID { get; set; }

	[JsonProperty("abtLotNumberID", Order = 19)]
	[Required(ErrorMessage = "abtLotNumberID is required.")]
	[MaxLength(30)]
	public string abtLotNumberID { get; set; }

	[JsonProperty("abtOldTransactionType", Order = 20)]
	[Required(ErrorMessage = "abtOldTransactionType is required.")]
	public byte abtOldTransactionType { get; set; }

	[JsonProperty("abtPartBinID", Order = 21)]
	[MaxLength(15)]
	public string abtPartBinID { get; set; }

	[JsonProperty("abtPartID", Order = 22)]
	[MaxLength(30)]
	public string abtPartID { get; set; }

	[JsonProperty("abtPartRevisionID", Order = 23)]
	[MaxLength(15)]
	public string abtPartRevisionID { get; set; }

	[JsonProperty("abtPartTransactionID", Order = 24)]
	public int abtPartTransactionID { get; set; }

	[JsonProperty("abtPartWarehouseLocationID", Order = 25)]
	[MaxLength(5)]
	public string abtPartWarehouseLocationID { get; set; }

	[JsonProperty("abtQuantity", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal abtQuantity { get; set; }

	[JsonProperty("abtQuantityToInspect", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal abtQuantityToInspect { get; set; }

	[JsonProperty("abtReceiptID", Order = 28)]
	[MaxLength(10)]
	public string abtReceiptID { get; set; }

	[JsonProperty("abtReceiptLineID", Order = 29)]
	public short abtReceiptLineID { get; set; }

	[JsonProperty("abtRmaReceiptID", Order = 30)]
	[MaxLength(10)]
	public string abtRmaReceiptID { get; set; }

	[JsonProperty("abtRmaReceiptLineID", Order = 31)]
	public short abtRmaReceiptLineID { get; set; }

	[JsonProperty("abtRowVersion", Order = 32)]
	public byte[] abtRowVersion { get; set; }

	[JsonProperty("abtLotNumberTransactionID", Order = 33)]
	[Required(ErrorMessage = "abtLotNumberTransactionID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int abtLotNumberTransactionID { get; set; }

	[JsonProperty("abtShipmentID", Order = 34)]
	[MaxLength(10)]
	public string abtShipmentID { get; set; }

	[JsonProperty("abtShipmentLineID", Order = 35)]
	public short abtShipmentLineID { get; set; }

	[JsonProperty("abtStatus", Order = 36)]
	public byte abtStatus { get; set; }

	[JsonProperty("abtTableName", Order = 37)]
	[MaxLength(30)]
	public string abtTableName { get; set; }

	[JsonProperty("abtTableUniqueID", Order = 38)]
	public Guid abtTableUniqueID { get; set; }

	[JsonProperty("abtTransactionDate", Order = 39)]
	[Required(ErrorMessage = "abtTransactionDate is required.")]
	public DateTime? abtTransactionDate { get; set; }

	[JsonProperty("abtTransactionType", Order = 40)]
	[Required(ErrorMessage = "abtTransactionType is required.")]
	public byte abtTransactionType { get; set; }

	[JsonProperty("abtWarehouseReceiptID", Order = 41)]
	[MaxLength(10)]
	public string abtWarehouseReceiptID { get; set; }

	[JsonProperty("abtWarehouseReceiptLineID", Order = 42)]
	public short abtWarehouseReceiptLineID { get; set; }

	[JsonProperty("abtWarehouseTransferID", Order = 43)]
	[MaxLength(10)]
	public string abtWarehouseTransferID { get; set; }

	[JsonProperty("abtWarehouseTransferLineID", Order = 44)]
	public short abtWarehouseTransferLineID { get; set; }

	[JsonProperty("customFields", Order = 45)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
