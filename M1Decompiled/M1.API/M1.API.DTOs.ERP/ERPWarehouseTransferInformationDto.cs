using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseTransferInformationDto
{
	public DateTime? mwpClosedDate { get; set; }

	public string mwpWarehouseTransferID { get; set; }

	public string mwpCreatedBy { get; set; }

	public DateTime? mwpCreatedDate { get; set; }

	public string mwpDestinationWarehouseID { get; set; }

	public Guid mwpUniqueID { get; set; }

	public decimal mwpFreightCharge { get; set; }

	public bool mwpClosed { get; set; }

	public bool mwpPosted { get; set; }

	public bool mwpPrintLabels { get; set; }

	public bool mwpPrintPacker { get; set; }

	public bool mwpReversalEntry { get; set; }

	public bool mwpReversed { get; set; }

	public short mwpNumberOfLabels { get; set; }

	public DateTime? mwpPostedDate { get; set; }

	public byte[] mwpRowVersion { get; set; }

	public DateTime? mwpShipDate { get; set; }

	public string mwpShippingCommentsRTF { get; set; }

	public string mwpShippingCommentsText { get; set; }

	public string mwpShippingMethodID { get; set; }

	public string mwpShippingPaymentTypeID { get; set; }

	public string mwpSourceWarehouseID { get; set; }

	public string mwpTrackingNumber { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
