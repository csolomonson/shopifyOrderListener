using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLotNumberTransactionInformationDto
{
	public string abtCreatedBy { get; set; }

	public DateTime? abtCreatedDate { get; set; }

	public string abtDmrShipmentID { get; set; }

	public short abtDmrShipmentLineID { get; set; }

	public Guid abtUniqueID { get; set; }

	public string abtInspectionID { get; set; }

	public short abtInspectionLineID { get; set; }

	public int abtInventoryCountID { get; set; }

	public int abtInventoryCountLineID { get; set; }

	public bool abtInProgress { get; set; }

	public bool abtInspect { get; set; }

	public bool abtNegativeTransaction { get; set; }

	public bool abtNonInventoryTransaction { get; set; }

	public int abtJobAssemblyID { get; set; }

	public string abtJobID { get; set; }

	public int abtJobMaterialComponentID { get; set; }

	public int abtJobMaterialID { get; set; }

	public string abtLandedCostID { get; set; }

	public string abtLotNumberID { get; set; }

	public byte abtOldTransactionType { get; set; }

	public string abtPartBinID { get; set; }

	public string abtPartID { get; set; }

	public string abtPartRevisionID { get; set; }

	public int abtPartTransactionID { get; set; }

	public string abtPartWarehouseLocationID { get; set; }

	public decimal abtQuantity { get; set; }

	public decimal abtQuantityToInspect { get; set; }

	public string abtReceiptID { get; set; }

	public short abtReceiptLineID { get; set; }

	public string abtRmaReceiptID { get; set; }

	public short abtRmaReceiptLineID { get; set; }

	public byte[] abtRowVersion { get; set; }

	public int abtLotNumberTransactionID { get; set; }

	public string abtShipmentID { get; set; }

	public short abtShipmentLineID { get; set; }

	public byte abtStatus { get; set; }

	public string abtTableName { get; set; }

	public Guid abtTableUniqueID { get; set; }

	public DateTime? abtTransactionDate { get; set; }

	public byte abtTransactionType { get; set; }

	public string abtWarehouseReceiptID { get; set; }

	public short abtWarehouseReceiptLineID { get; set; }

	public string abtWarehouseTransferID { get; set; }

	public short abtWarehouseTransferLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
