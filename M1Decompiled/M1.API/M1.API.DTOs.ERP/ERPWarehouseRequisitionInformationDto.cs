using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseRequisitionInformationDto
{
	public DateTime? wqpClosedDate { get; set; }

	public string wqpWarehouseRequisitionID { get; set; }

	public string wqpCreatedBy { get; set; }

	public DateTime? wqpCreatedDate { get; set; }

	public string wqpDestinationWarehouseID { get; set; }

	public Guid wqpUniqueID { get; set; }

	public bool wqpClosed { get; set; }

	public bool wqpReadyToPrint { get; set; }

	public DateTime? wqpRequestedShipDate { get; set; }

	public string wqpRequisitionCommentsRTF { get; set; }

	public string wqpRequisitionCommentsText { get; set; }

	public DateTime? wqpRequisitionDate { get; set; }

	public byte[] wqpRowVersion { get; set; }

	public string wqpShippingMethodID { get; set; }

	public string wqpShippingPaymentTypeID { get; set; }

	public string wqpSourceWarehouseID { get; set; }

	public byte wqpStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
