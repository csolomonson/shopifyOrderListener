using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSerialNumberTransactionInformationDto
{
	public string sntCreatedBy { get; set; }

	public DateTime? sntCreatedDate { get; set; }

	public string sntDmrShipmentID { get; set; }

	public short sntDmrShipmentLineID { get; set; }

	public Guid sntUniqueID { get; set; }

	public string sntInspectionID { get; set; }

	public short sntInspectionLineID { get; set; }

	public int sntInventoryCountID { get; set; }

	public int sntInventoryCountLineID { get; set; }

	public bool sntInspect { get; set; }

	public bool sntNegativeTransaction { get; set; }

	public int sntJobAssemblyID { get; set; }

	public string sntJobID { get; set; }

	public int sntJobMaterialComponentID { get; set; }

	public int sntJobMaterialID { get; set; }

	public string sntJobPartBinID { get; set; }

	public string sntJobPartID { get; set; }

	public string sntJobPartRevisionID { get; set; }

	public string sntJobPartWarehouseLocationID { get; set; }

	public string sntJobSerialNumberID { get; set; }

	public string sntLandedCostID { get; set; }

	public byte sntOldTransactionType { get; set; }

	public string sntPartBinID { get; set; }

	public string sntPartID { get; set; }

	public string sntPartRevisionID { get; set; }

	public int sntPartTransactionID { get; set; }

	public string sntPartWarehouseLocationID { get; set; }

	public decimal sntQuantity { get; set; }

	public string sntReceiptID { get; set; }

	public short sntReceiptLineID { get; set; }

	public string sntRmaReceiptID { get; set; }

	public short sntRmaReceiptLineID { get; set; }

	public byte[] sntRowVersion { get; set; }

	public int sntSerialNumberTransactionID { get; set; }

	public string sntSerialNumberID { get; set; }

	public string sntShipmentID { get; set; }

	public short sntShipmentLineID { get; set; }

	public byte sntStatus { get; set; }

	public string sntTableName { get; set; }

	public Guid sntTableUniqueID { get; set; }

	public DateTime? sntTransactionDate { get; set; }

	public byte sntTransactionType { get; set; }

	public string sntWarehouseReceiptID { get; set; }

	public short sntWarehouseReceiptLineID { get; set; }

	public string sntWarehouseTransferID { get; set; }

	public short sntWarehouseTransferLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
