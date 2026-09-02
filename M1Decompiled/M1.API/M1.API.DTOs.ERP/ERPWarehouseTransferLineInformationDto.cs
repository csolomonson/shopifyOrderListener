using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseTransferLineInformationDto
{
	public string mwlCreatedBy { get; set; }

	public DateTime? mwlCreatedDate { get; set; }

	public string mwlDestinationWarehouseID { get; set; }

	public Guid mwlUniqueID { get; set; }

	public bool mwlClosed { get; set; }

	public bool mwlKitPart { get; set; }

	public bool mwlPosted { get; set; }

	public bool mwlReceivedComplete { get; set; }

	public bool mwlReversed { get; set; }

	public bool mwlShippedComplete { get; set; }

	public string mwlPartDescription { get; set; }

	public string mwlPartID { get; set; }

	public string mwlPartRevisionID { get; set; }

	public decimal mwlQuantityInTransit { get; set; }

	public DateTime? mwlReceivedDate { get; set; }

	public decimal mwlReceivedQuantity { get; set; }

	public string mwlReverseWHTransferID { get; set; }

	public short mwlReverseWHTransferLineID { get; set; }

	public byte[] mwlRowVersion { get; set; }

	public short mwlWarehouseTransferLineID { get; set; }

	public decimal mwlShipQuantity { get; set; }

	public string mwlSourcePartBinID { get; set; }

	public string mwlSourceWarehouseID { get; set; }

	public string mwlUnitOfMeasure { get; set; }

	public string mwlWarehouseRequisitionID { get; set; }

	public short mwlWarehouseRequisitionLineID { get; set; }

	public string mwlWarehouseTransferID { get; set; }

	public decimal mwlWROpenQuantity { get; set; }

	public decimal mwlWRRequestedQuantity { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
