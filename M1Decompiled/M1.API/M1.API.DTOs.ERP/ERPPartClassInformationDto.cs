using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartClassInformationDto
{
	public string imcPartClassID { get; set; }

	public string imcCreatedBy { get; set; }

	public DateTime? imcCreatedDate { get; set; }

	public string imcDescription { get; set; }

	public Guid imcUniqueID { get; set; }

	public decimal imcFdxHandlingCost { get; set; }

	public int imcFdxPackageHeight { get; set; }

	public int imcFdxPackageLength { get; set; }

	public int imcFdxPackageWidth { get; set; }

	public string imcFdxPackaging { get; set; }

	public decimal imcFdxPackagingCost { get; set; }

	public decimal imcFdxShipCostMarkupPct { get; set; }

	public DateTime? imcInactiveDate { get; set; }

	public string imcInventoryGlAccountID { get; set; }

	public string imcInvInInspectionGlAccountID { get; set; }

	public string imcInvInTransferGlAccountID { get; set; }

	public string imcInvToReturnGlAccountID { get; set; }

	public bool imcInactive { get; set; }

	public bool imcFdxNonstandardContainer { get; set; }

	public bool imcFdxOneItemPerShipment { get; set; }

	public bool imcRequiresInspection { get; set; }

	public string imcParentPartClassID { get; set; }

	public string imcPartImageFileName { get; set; }

	public byte imcPickingMethod { get; set; }

	public byte imcReorderMethod { get; set; }

	public byte[] imcRowVersion { get; set; }

	public decimal imcWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
