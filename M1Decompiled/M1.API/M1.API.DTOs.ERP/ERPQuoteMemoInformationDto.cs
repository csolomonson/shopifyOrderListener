using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteMemoInformationDto
{
	public string qmkCreatedBy { get; set; }

	public DateTime? qmkCreatedDate { get; set; }

	public Guid qmkUniqueID { get; set; }

	public bool qmkClosed { get; set; }

	public string qmkLongDescriptionRtf { get; set; }

	public string qmkLongDescriptionText { get; set; }

	public DateTime? qmkMemoDate { get; set; }

	public string qmkQuoteID { get; set; }

	public byte[] qmkRowVersion { get; set; }

	public short qmkQuoteMemoID { get; set; }

	public string qmkShortDescription { get; set; }

	public bool qmkShowInQuotes { get; set; }

	public bool qmkShowInSalesOrders { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
