using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.Methods;

public class Material
{
	public Dictionary<string, object> CustomFields = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

	public string MethodID = string.Empty;

	public string MethodRevisionID = string.Empty;

	public int AssemblyID;

	public int MaterialID;

	public string PartID = string.Empty;

	public string PartRevisionID = string.Empty;

	public string PartWarehouseLocationID = string.Empty;

	public string PartBinID = string.Empty;

	public string UnitOfMeasure = string.Empty;

	public string PartShortDescription = string.Empty;

	public string PartLongDescriptionRTF = string.Empty;

	public string PartLongDescriptionText = string.Empty;

	public decimal QuantityPerAssembly;

	public decimal ScrapPercent;

	public decimal ScrapQuantity;

	public decimal EstimatedQuantity;

	public decimal EstimatedUnitCost;

	public string SupplierOrganizationID = string.Empty;

	public string PurchaseLocationID = string.Empty;

	public short LeadTime;

	public decimal MinimumCharge;

	public int RelatedOperationID;

	public bool Backflush;

	public string Documents = string.Empty;

	public PriceBreak PriceBreak1 = new PriceBreak();

	public PriceBreak PriceBreak2 = new PriceBreak();

	public PriceBreak PriceBreak3 = new PriceBreak();

	public PriceBreak PriceBreak4 = new PriceBreak();

	public PriceBreak PriceBreak5 = new PriceBreak();

	public PriceBreak PriceBreak6 = new PriceBreak();

	public PriceBreak PriceBreak7 = new PriceBreak();

	public PriceBreak PriceBreak8 = new PriceBreak();

	public PriceBreak PriceBreak9 = new PriceBreak();

	public override string ToString()
	{
		return $"{AssemblyID} - {MaterialID}, \"{PartID}\", Qty = {EstimatedQuantity}";
	}
}
