using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseRequisitionComponentInformationDto
{
	public decimal wqoAdditionalQuantity { get; set; }

	public string wqoCreatedBy { get; set; }

	public DateTime? wqoCreatedDate { get; set; }

	public string wqoDescription { get; set; }

	public Guid wqoUniqueID { get; set; }

	public bool wqoClosed { get; set; }

	public bool wqoTransferredComplete { get; set; }

	public decimal wqoParentQuantity { get; set; }

	public string wqoPartID { get; set; }

	public string wqoPartRevisionID { get; set; }

	public decimal wqoQuantityPerParent { get; set; }

	public decimal wqoQuantityRequested { get; set; }

	public decimal wqoQuantityTransferred { get; set; }

	public byte[] wqoRowVersion { get; set; }

	public string wqoSourceWarehouseID { get; set; }

	public string wqoUnitOfMeasure { get; set; }

	public short wqoWarehouseReqComponentID { get; set; }

	public string wqoWarehouseRequisitionID { get; set; }

	public short wqoWarehouseRequisitionLineID { get; set; }

	public decimal wqoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
