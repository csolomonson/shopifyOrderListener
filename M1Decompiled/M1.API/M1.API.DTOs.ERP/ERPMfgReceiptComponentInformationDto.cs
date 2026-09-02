using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMfgReceiptComponentInformationDto
{
	public decimal rmnAdditionalQuantity { get; set; }

	public string rmnCreatedBy { get; set; }

	public DateTime? rmnCreatedDate { get; set; }

	public string rmnDescription { get; set; }

	public Guid rmnUniqueID { get; set; }

	public decimal rmnExtendedCost { get; set; }

	public decimal rmnInvParentQuantity { get; set; }

	public decimal rmnInvReceiptQuantity { get; set; }

	public bool rmnPosted { get; set; }

	public bool rmnReceivedComplete { get; set; }

	public bool rmnReversed { get; set; }

	public int rmnJobAssemblyID { get; set; }

	public string rmnJobID { get; set; }

	public int rmnJobMaterialComponentID { get; set; }

	public int rmnJobMaterialID { get; set; }

	public decimal rmnJobMatParentQuantity { get; set; }

	public decimal rmnJobMatReceiptQuantity { get; set; }

	public string rmnMfgReceiptID { get; set; }

	public string rmnPartBinID { get; set; }

	public string rmnPartID { get; set; }

	public string rmnPartRevisionID { get; set; }

	public string rmnPartWarehouseLocationID { get; set; }

	public decimal rmnQuantityPerParent { get; set; }

	public int rmnReverseMfgReceiptCompID { get; set; }

	public string rmnReverseMfgReceiptID { get; set; }

	public byte[] rmnRowVersion { get; set; }

	public int rmnMfgReceiptComponentID { get; set; }

	public decimal rmnUnitCost { get; set; }

	public string rmnUnitOfMeasure { get; set; }

	public decimal rmnWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
