using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseReceiptLineInformationDto
{
	public string wrlCreatedBy { get; set; }

	public DateTime? wrlCreatedDate { get; set; }

	public string wrlDestinationPartBinID { get; set; }

	public string wrlDestinationWarehouseID { get; set; }

	public Guid wrlUniqueID { get; set; }

	public string wrlHeatLot { get; set; }

	public bool wrlClosed { get; set; }

	public bool wrlKitPart { get; set; }

	public bool wrlPosted { get; set; }

	public bool wrlReceivedComplete { get; set; }

	public bool wrlReversed { get; set; }

	public string wrlPartDescription { get; set; }

	public string wrlPartID { get; set; }

	public string wrlPartRevisionID { get; set; }

	public decimal wrlQuantityReceived { get; set; }

	public string wrlReference { get; set; }

	public string wrlReverseWHReceiptID { get; set; }

	public short wrlReverseWHReceiptLineID { get; set; }

	public byte[] wrlRowVersion { get; set; }

	public short wrlWarehouseReceiptLineID { get; set; }

	public string wrlSourcePartBinID { get; set; }

	public string wrlSourceTableName { get; set; }

	public Guid wrlSourceTableUniqueID { get; set; }

	public string wrlSourceWarehouseID { get; set; }

	public decimal wrlUnitCost { get; set; }

	public string wrlUnitOfMeasure { get; set; }

	public string wrlWarehouseReceiptID { get; set; }

	public string wrlWarehouseRequisitionID { get; set; }

	public short wrlWarehouseRequisitionLineID { get; set; }

	public string wrlWarehouseTransferID { get; set; }

	public short wrlWarehouseTransferLineID { get; set; }

	public decimal wrlWTOpenQuantity { get; set; }

	public decimal wrlWTShippedQuantity { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
