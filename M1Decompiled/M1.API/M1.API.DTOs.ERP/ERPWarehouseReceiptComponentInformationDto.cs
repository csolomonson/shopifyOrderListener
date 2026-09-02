using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseReceiptComponentInformationDto
{
	public decimal wroAdditionalQuantity { get; set; }

	public string wroCreatedBy { get; set; }

	public DateTime? wroCreatedDate { get; set; }

	public string wroDescription { get; set; }

	public string wroDestinationPartBinID { get; set; }

	public string wroDestinationWarehouseID { get; set; }

	public Guid wroUniqueID { get; set; }

	public bool wroClosed { get; set; }

	public bool wroPosted { get; set; }

	public bool wroReceivedComplete { get; set; }

	public bool wroReversed { get; set; }

	public decimal wroParentQuantity { get; set; }

	public string wroPartID { get; set; }

	public string wroPartRevisionID { get; set; }

	public decimal wroQuantityPerParent { get; set; }

	public decimal wroQuantityReceived { get; set; }

	public string wroReverseWHReceiptCompID { get; set; }

	public string wroReverseWHReceiptID { get; set; }

	public short wroReverseWHReceiptLineID { get; set; }

	public byte[] wroRowVersion { get; set; }

	public short wroWarehouseReceiptComponentID { get; set; }

	public string wroSourcePartBinID { get; set; }

	public string wroSourceTableName { get; set; }

	public Guid wroSourceTableUniqueID { get; set; }

	public string wroSourceWarehouseID { get; set; }

	public string wroUnitOfMeasure { get; set; }

	public string wroWarehouseReceiptID { get; set; }

	public short wroWarehouseReceiptLineID { get; set; }

	public short wroWarehouseReqComponentID { get; set; }

	public string wroWarehouseRequisitionID { get; set; }

	public short wroWarehouseRequisitionLineID { get; set; }

	public short wroWarehouseTransComponentID { get; set; }

	public string wroWarehouseTransferID { get; set; }

	public short wroWarehouseTransferLineID { get; set; }

	public decimal wroWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
