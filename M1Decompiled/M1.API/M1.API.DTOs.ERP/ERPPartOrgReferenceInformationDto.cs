using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartOrgReferenceInformationDto
{
	public decimal imzConversionFactor { get; set; }

	public string imzCreatedBy { get; set; }

	public DateTime? imzCreatedDate { get; set; }

	public Guid imzUniqueID { get; set; }

	public bool imzInactive { get; set; }

	public bool imzPurchased { get; set; }

	public bool imzSold { get; set; }

	public short imzLeadTime { get; set; }

	public decimal imzLotSize { get; set; }

	public decimal imzMinimumPurchaseQuantity { get; set; }

	public string imzOrganizationID { get; set; }

	public string imzOrgPartID { get; set; }

	public string imzOrgPartShortDescription { get; set; }

	public string imzPartID { get; set; }

	public string imzPartRevisionID { get; set; }

	public string imzPurchaseUnitOfMeasure { get; set; }

	public byte[] imzRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
