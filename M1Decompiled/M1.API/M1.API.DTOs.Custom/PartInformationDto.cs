using System;
using System.Collections.Generic;

namespace M1.API.DTOs.Custom;

public class PartInformationDto
{
	public string PartID { get; set; } = string.Empty;

	public string PartRevisionID { get; set; } = string.Empty;

	public string PartGroupID { get; set; } = string.Empty;

	public string PartShortDescription { get; set; } = string.Empty;

	public string PartLongDescriptionText { get; set; } = string.Empty;

	public string PartWarehouseLocationID { get; set; } = string.Empty;

	public string UOM { get; set; } = string.Empty;

	public byte DeliveryType { get; set; }

	public decimal Weight { get; set; }

	public string WeightUnitOfMeasure { get; set; } = string.Empty;

	public string OrgPartID { get; set; } = string.Empty;

	public string OrgPartShortDescription { get; set; } = string.Empty;

	public string PartBinID { get; set; } = string.Empty;

	public string PartTaxCodeID { get; set; } = string.Empty;

	public string PartSecondTaxCodeID { get; set; } = string.Empty;

	public string PartNonTaxReasonID { get; set; } = string.Empty;

	public bool PartAlwaysNonTaxable { get; set; }

	public string CountryOfManufacture { get; set; } = string.Empty;

	public byte PartType { get; set; }

	public string PartClassID { get; set; } = string.Empty;

	public string CreatedBy { get; set; } = string.Empty;

	public DateTime? CreatedDate { get; set; } = DateTime.Now;

	public bool BuyForInventory { get; set; }

	public bool NonStockedItem { get; set; }

	public IList<string> WarningsList { get; set; } = new List<string>();

	public IList<string> ErrorsList { get; set; } = new List<string>();
}
