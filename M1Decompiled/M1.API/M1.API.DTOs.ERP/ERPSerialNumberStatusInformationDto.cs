using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSerialNumberStatusInformationDto
{
	public string snsCreatedBy { get; set; }

	public DateTime? snsCreatedDate { get; set; }

	public Guid snsUniqueID { get; set; }

	public string snsPartBinID { get; set; }

	public string snsPartID { get; set; }

	public string snsPartRevisionID { get; set; }

	public string snsPartWarehouseLocationID { get; set; }

	public decimal snsQuantity { get; set; }

	public byte[] snsRowVersion { get; set; }

	public string snsSerialNumberID { get; set; }

	public byte snsStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
