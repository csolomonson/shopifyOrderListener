using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderPickListLineInformationDto
{
	public string omyCreatedBy { get; set; }

	public DateTime? omyCreatedDate { get; set; }

	public DateTime? omyDeliveryDate { get; set; }

	public Guid omyUniqueID { get; set; }

	public decimal omyOpenQuantity { get; set; }

	public string omyPartBinID { get; set; }

	public string omyPartID { get; set; }

	public string omyPartRevisionID { get; set; }

	public string omyPartWareHouseLocationID { get; set; }

	public DateTime? omyPickDate { get; set; }

	public short omyPickListLineID { get; set; }

	public int omyPickListSessionID { get; set; }

	public decimal omyPickQuantity { get; set; }

	public byte[] omyRowVersion { get; set; }

	public short omySalesOrderDeliveryID { get; set; }

	public string omySalesOrderID { get; set; }

	public short omySalesOrderLineID { get; set; }

	public byte omyStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
