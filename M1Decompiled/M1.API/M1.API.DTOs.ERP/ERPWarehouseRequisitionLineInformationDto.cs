using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseRequisitionLineInformationDto
{
	public string wqlCreatedBy { get; set; }

	public DateTime? wqlCreatedDate { get; set; }

	public Guid wqlUniqueID { get; set; }

	public bool wqlClosed { get; set; }

	public bool wqlKitPart { get; set; }

	public bool wqlTransferredComplete { get; set; }

	public string wqlPartDescription { get; set; }

	public string wqlPartID { get; set; }

	public string wqlPartRevisionID { get; set; }

	public decimal wqlQuantityTransferred { get; set; }

	public decimal wqlRequestedQuantity { get; set; }

	public byte[] wqlRowVersion { get; set; }

	public short wqlWarehouseRequisitionLineID { get; set; }

	public string wqlSourceWarehouseID { get; set; }

	public string wqlUnitOfMeasure { get; set; }

	public string wqlWarehouseRequisitionID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
