using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchasePlannerLineInformationDto
{
	public string pplCreatedBy { get; set; }

	public DateTime? pplCreatedDate { get; set; }

	public int pplDataMissing { get; set; }

	public Guid pplUniqueID { get; set; }

	public decimal pplExtendedCostBase { get; set; }

	public bool pplCompleted { get; set; }

	public bool pplNonStockedItem { get; set; }

	public bool pplPhantomOrKitPart { get; set; }

	public DateTime? pplLastRunDate { get; set; }

	public int pplLineID { get; set; }

	public decimal pplLotSize { get; set; }

	public decimal pplMaximumQuantity { get; set; }

	public decimal pplMinimumQuantity { get; set; }

	public string pplPartID { get; set; }

	public string pplPartRevisionID { get; set; }

	public string pplPartShortDescription { get; set; }

	public string pplPlantID { get; set; }

	public decimal pplQuantityOnHand { get; set; }

	public byte pplReorderMethod { get; set; }

	public byte[] pplRowVersion { get; set; }

	public string pplSessionID { get; set; }

	public string pplWarehouseID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
