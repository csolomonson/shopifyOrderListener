using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseTransferComponentInformationDto
{
	public decimal mwoAdditionalQuantity { get; set; }

	public string mwoCreatedBy { get; set; }

	public DateTime? mwoCreatedDate { get; set; }

	public string mwoDescription { get; set; }

	public string mwoDestinationWarehouseID { get; set; }

	public Guid mwoUniqueID { get; set; }

	public bool mwoClosed { get; set; }

	public bool mwoPosted { get; set; }

	public bool mwoReceivedComplete { get; set; }

	public bool mwoReversed { get; set; }

	public bool mwoShippedComplete { get; set; }

	public decimal mwoParentQuantity { get; set; }

	public string mwoPartID { get; set; }

	public string mwoPartRevisionID { get; set; }

	public decimal mwoQuantityInTransit { get; set; }

	public decimal mwoQuantityPerParent { get; set; }

	public decimal mwoReceivedQuantity { get; set; }

	public short mwoReverseWHTransComponentID { get; set; }

	public string mwoReverseWHTransferID { get; set; }

	public short mwoReverseWHTransferLineID { get; set; }

	public byte[] mwoRowVersion { get; set; }

	public decimal mwoShipQuantity { get; set; }

	public string mwoSourcePartBinID { get; set; }

	public string mwoSourceWarehouseID { get; set; }

	public string mwoUnitOfMeasure { get; set; }

	public short mwoWarehouseReqComponentID { get; set; }

	public string mwoWarehouseRequisitionID { get; set; }

	public short mwoWarehouseRequisitionLineID { get; set; }

	public short mwoWarehouseTransComponentID { get; set; }

	public string mwoWarehouseTransferID { get; set; }

	public short mwoWarehouseTransferLineID { get; set; }

	public decimal mwoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
