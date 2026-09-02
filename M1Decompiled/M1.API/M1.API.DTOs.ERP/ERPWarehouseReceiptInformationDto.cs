using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseReceiptInformationDto
{
	public DateTime? wrpClosedDate { get; set; }

	public string wrpWarehouseReceiptID { get; set; }

	public string wrpCreatedBy { get; set; }

	public DateTime? wrpCreatedDate { get; set; }

	public string wrpDestinationWarehouseID { get; set; }

	public Guid wrpUniqueID { get; set; }

	public decimal wrpFreightCharge { get; set; }

	public bool wrpClosed { get; set; }

	public bool wrpPosted { get; set; }

	public bool wrpReversalEntry { get; set; }

	public bool wrpReversed { get; set; }

	public DateTime? wrpPostedDate { get; set; }

	public DateTime? wrpReceiptDate { get; set; }

	public byte[] wrpRowVersion { get; set; }

	public string wrpShippingMethodID { get; set; }

	public string wrpShippingPaymentTypeID { get; set; }

	public string wrpSourceWarehouseID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
