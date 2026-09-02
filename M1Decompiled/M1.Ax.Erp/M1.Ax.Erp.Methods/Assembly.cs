using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.Methods;

public class Assembly
{
	public List<Assembly> SubAssemblies = new List<Assembly>();

	public List<Material> Materials = new List<Material>();

	public List<Operation> Operations = new List<Operation>();

	public Dictionary<string, object> CustomFields = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

	public string MethodID = string.Empty;

	public string MethodRevisionID = string.Empty;

	public int AssemblyID;

	public short Level;

	public int ParentAssemblyID;

	public string PartID = string.Empty;

	public string PartRevisionID = string.Empty;

	public string UnitOfMeasure = string.Empty;

	public string PartShortDescription = string.Empty;

	public string PartLongDescriptionRTF = string.Empty;

	public string PartLongDescriptionText = string.Empty;

	public string SourceMethodID = string.Empty;

	public string SourceRevisionID = string.Empty;

	public string ProductionNotesRTF = string.Empty;

	public string ProductionNotesText = string.Empty;

	public decimal QuantityPerParent;

	public string Documents = string.Empty;

	public decimal EstUnitCost;

	public int OverlapSourceOperationID;

	public byte OverlapSourceLink;

	public byte OverlapDestinationLink;

	public int OverlapOperationID;

	public decimal OverlapOffsetTime;

	public byte AssemblyOverlap;

	public bool PullAllFromStock;

	public decimal OrderQuantity;

	public decimal InventoryQuantity;

	public decimal ScrapQuantity;

	public decimal ProductionQuantity;

	public decimal QuantityToPull;

	public decimal QuantityIssued;

	public decimal QuantityToMake;

	public bool IssuedComplete;

	public Assembly()
	{
	}

	public Assembly(int assemblyID)
	{
		AssemblyID = assemblyID;
	}

	public Assembly(string partID, string revisionID, int assemblyID)
	{
		PartID = partID;
		PartRevisionID = revisionID;
		AssemblyID = assemblyID;
	}

	public override string ToString()
	{
		return $"{AssemblyID}, \"{PartID}\", Qty = {ProductionQuantity}";
	}
}
