using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderJobLinkInformationDto
{
	public string omjCreatedBy { get; set; }

	public DateTime? omjCreatedDate { get; set; }

	public Guid omjUniqueID { get; set; }

	public bool omjClosed { get; set; }

	public string omjJobID { get; set; }

	public byte omjLinkType { get; set; }

	public byte[] omjRowVersion { get; set; }

	public short omjSalesOrderDeliveryID { get; set; }

	public string omjSalesOrderID { get; set; }

	public short omjSalesOrderLineID { get; set; }

	public int omjSalesOrderJobLinkID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
