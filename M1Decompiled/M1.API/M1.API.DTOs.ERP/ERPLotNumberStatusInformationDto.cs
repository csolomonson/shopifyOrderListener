using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLotNumberStatusInformationDto
{
	public string absCreatedBy { get; set; }

	public DateTime? absCreatedDate { get; set; }

	public Guid absUniqueID { get; set; }

	public string absLotNumberID { get; set; }

	public string absPartBinID { get; set; }

	public string absPartID { get; set; }

	public string absPartRevisionID { get; set; }

	public string absPartWarehouseLocationID { get; set; }

	public decimal absQuantity { get; set; }

	public byte[] absRowVersion { get; set; }

	public byte absStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
