using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPInventoryCountInformationDto
{
	public string imnCreatedBy { get; set; }

	public DateTime? imnCreatedDate { get; set; }

	public string imnCycleCodeID { get; set; }

	public Guid imnUniqueID { get; set; }

	public DateTime? imnGeneratedDate { get; set; }

	public bool imnExcludeInactivePartBins { get; set; }

	public bool imnIncludeBlankPartClass { get; set; }

	public bool imnIncludeBlankPartGroup { get; set; }

	public bool imnPostedToInventory { get; set; }

	public bool imnRecordsGenerated { get; set; }

	public int imnNumberofRecordsGenerated { get; set; }

	public string imnPartBinIDs { get; set; }

	public string imnPartClassIDs { get; set; }

	public string imnPartGroupIDs { get; set; }

	public string imnPartWarehouseIDs { get; set; }

	public DateTime? imnPostedDate { get; set; }

	public byte[] imnRowVersion { get; set; }

	public int imnInventoryCountID { get; set; }

	public byte imnStatus { get; set; }

	public string imnSupplierOrganizationIDs { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
