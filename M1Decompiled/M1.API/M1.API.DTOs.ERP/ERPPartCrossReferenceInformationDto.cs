using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartCrossReferenceInformationDto
{
	public decimal imxConversionFactor { get; set; }

	public string imxCreatedBy { get; set; }

	public DateTime? imxCreatedDate { get; set; }

	public Guid imxUniqueID { get; set; }

	public bool imxInactive { get; set; }

	public bool imxPurchased { get; set; }

	public bool imxSold { get; set; }

	public short imxLeadTime { get; set; }

	public string imxLocationID { get; set; }

	public decimal imxLotSize { get; set; }

	public decimal imxMinimumPurchaseQuantity { get; set; }

	public string imxOrganizationID { get; set; }

	public string imxOrgPartID { get; set; }

	public string imxOrgPartShortDescription { get; set; }

	public string imxPartID { get; set; }

	public string imxPartRevisionID { get; set; }

	public string imxPurchaseUnitOfMeasure { get; set; }

	public byte[] imxRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
