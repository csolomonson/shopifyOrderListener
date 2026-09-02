using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLandedCostCategoryInformationDto
{
	public byte rmaCategoryType { get; set; }

	public string rmaLandedCostCategoryID { get; set; }

	public string rmaCreatedBy { get; set; }

	public DateTime? rmaCreatedDate { get; set; }

	public string rmaDescription { get; set; }

	public Guid rmaUniqueID { get; set; }

	public decimal rmaExpenseSplitPercentTotal { get; set; }

	public bool rmaDefault { get; set; }

	public byte rmaLandedCostMethod { get; set; }

	public byte[] rmaRowVersion { get; set; }

	public string rmaSupplierLocationID { get; set; }

	public string rmaSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
