using System.Data;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.052", "Populate PartBinDetails table", "2016-12-12")]
public class v92052b
{
	public v92052b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartBinDetails"))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Delete From PartBinDetails;");
		stringBuilder.AppendLine("Insert Into PartBinDetails (imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgPartBinDetailID, imgCreatedBy, imgCreatedDate, imgTransactionDate, imgQuantityType, imgOriginalQuantity, imgRemainingQuantity, imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost)");
		stringBuilder.AppendLine("Select");
		stringBuilder.AppendLine("imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID,");
		stringBuilder.AppendLine("ROW_NUMBER() OVER(PARTITION BY imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID ORDER BY imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgQuantityType) As imgPartBinDetailID,");
		stringBuilder.AppendLine("imgCreatedBy, imgCreatedDate, imgTransactionDate, imgQuantityType, imgOriginalQuantity, imgRemainingQuantity,");
		stringBuilder.AppendLine("imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost");
		stringBuilder.AppendLine("From");
		stringBuilder.AppendLine("(");
		stringBuilder.AppendLine("--QOH");
		stringBuilder.AppendLine("Select imbPartID as imgPartID, imbPartRevisionID as imgPartRevisionID, imbWarehouseID as imgWarehouseID, imbPartBinID as imgPartBinID, 'CONVERSION' as imgCreatedBy, GETDATE() as imgCreatedDate, GETDATE() as imgTransactionDate, 1 as imgQuantityType, imbQuantityOnHand as imgOriginalQuantity, imbQuantityOnHand as imgRemainingQuantity,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardLaborCost When xapIMCostingMethod = 2 Then imrLastLaborCost Else imrAverageLaborCost End As imgUnitLaborCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardOverheadCost When xapIMCostingMethod = 2 Then imrLastOverheadCost Else imrAverageOverheadCost End As imgUnitOverheadCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardMaterialCost When xapIMCostingMethod = 2 Then imrLastMaterialCost Else imrAverageMaterialCost End As imgUnitMaterialCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardSubcontractCost When xapIMCostingMethod = 2 Then imrLastSubcontractCost Else imrAverageSubcontractCost End As imgUnitSubcontractCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardDutyCost When xapIMCostingMethod = 2 Then imrLastDutyCost Else imrAverageDutyCost End As imgUnitDutyCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardFreightCost When xapIMCostingMethod = 2 Then imrLastFreightCost Else imrAverageFreightCost End As imgUnitFreightCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardMiscCost When xapIMCostingMethod = 2 Then imrLastMiscCost Else imrAverageMiscCost End As imgUnitMiscCost");
		stringBuilder.AppendLine("From PartBins, PartRevisions, ProductionProperties");
		stringBuilder.AppendLine("Where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbQuantityOnHand > 0");
		stringBuilder.AppendLine("Union All");
		stringBuilder.AppendLine("-- QTI");
		stringBuilder.AppendLine("Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, 'CONVERSION', GETDATE(), GETDATE(), 2, imbQuantityToInspect, imbQuantityToInspect,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardLaborCost When xapIMCostingMethod = 2 Then imrLastLaborCost Else imrAverageLaborCost End As LaborCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardOverheadCost When xapIMCostingMethod = 2 Then imrLastOverheadCost Else imrAverageOverheadCost End As OverheadCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardMaterialCost When xapIMCostingMethod = 2 Then imrLastMaterialCost Else imrAverageMaterialCost End As MaterialCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardSubcontractCost When xapIMCostingMethod = 2 Then imrLastSubcontractCost Else imrAverageSubcontractCost End As SubcontractCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardDutyCost When xapIMCostingMethod = 2 Then imrLastDutyCost Else imrAverageDutyCost End As DutyCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardFreightCost When xapIMCostingMethod = 2 Then imrLastFreightCost Else imrAverageFreightCost End As FreightCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardMiscCost When xapIMCostingMethod = 2 Then imrLastMiscCost Else imrAverageMiscCost End As MiscCost");
		stringBuilder.AppendLine("From PartBins, PartRevisions, ProductionProperties");
		stringBuilder.AppendLine("Where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbQuantityToInspect > 0");
		stringBuilder.AppendLine("Union All");
		stringBuilder.AppendLine("-- QTR");
		stringBuilder.AppendLine("Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, 'CONVERSION', GETDATE(), GETDATE(), 3, imbQuantityToReturn, imbQuantityToReturn,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardLaborCost When xapIMCostingMethod = 2 Then imrLastLaborCost Else imrAverageLaborCost End As LaborCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardOverheadCost When xapIMCostingMethod = 2 Then imrLastOverheadCost Else imrAverageOverheadCost End As OverheadCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardMaterialCost When xapIMCostingMethod = 2 Then imrLastMaterialCost Else imrAverageMaterialCost End As MaterialCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardSubcontractCost When xapIMCostingMethod = 2 Then imrLastSubcontractCost Else imrAverageSubcontractCost End As SubcontractCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardDutyCost When xapIMCostingMethod = 2 Then imrLastDutyCost Else imrAverageDutyCost End As DutyCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardFreightCost When xapIMCostingMethod = 2 Then imrLastFreightCost Else imrAverageFreightCost End As FreightCost,");
		stringBuilder.AppendLine("Case When xapIMCostingMethod = 3 Then imrStandardMiscCost When xapIMCostingMethod = 2 Then imrLastMiscCost Else imrAverageMiscCost End As MiscCost");
		stringBuilder.AppendLine("From PartBins, PartRevisions, ProductionProperties");
		stringBuilder.AppendLine("Where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbQuantityToReturn > 0");
		stringBuilder.AppendLine(") as X");
		parms.Database.ExecuteCommand(stringBuilder.ToString());
		DataTable dataTable = parms.Database.GetDataTable("Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbQuantityToInspect, imbQuantityToReturn From PartBins Where imbQuantityOnHand < 0 Or imbQuantityToInspect < 0 Or imbQuantityToReturn < 0;");
		if (dataTable.Rows.Count != 0 && parms.Messages != null)
		{
			string text = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			foreach (DataRow row in dataTable.Rows)
			{
				empty = (string.IsNullOrWhiteSpace(row.Field<string>("imbPartRevisionID")) ? "<Blank>" : row.Field<string>("imbPartRevisionID"));
				empty2 = "Part: " + row.Field<string>("imbPartID") + ", Rev: " + empty + ", Warehouse: " + row.Field<string>("imbWarehouseID") + ", Bin: " + row.Field<string>("imbPartBinID") + ", Qty On Hand: " + row.Field<decimal>("imbQuantityOnHand") + ", Qty To Inspect: " + row.Field<decimal>("imbQuantityToInspect") + ", Qty To Return: " + row.Field<decimal>("imbQuantityToReturn");
				text = text + empty2 + "\n\r";
			}
			text = "The following records have negative quantites in the PartBins table and will need to be manually adjusted:\n\r" + text;
			parms.Messages.Add(text);
		}
		dataTable = null;
	}
}
