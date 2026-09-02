using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.Methods;

public class PartMethodLoader : IMethodLoader
{
	public Assembly Load(M1Database database, object[] keyValues, int assemblyID)
	{
		string value = (string)keyValues[0];
		string value2 = (string)keyValues[1];
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From PartAssemblies Where imaPartID = @PartID And imaPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = value2;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From PartMaterials Where immMethodID = @PartID And immMethodRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = value2;
		DataTable dataTable2 = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From PartOperations Where imoMethodID = @PartID And imoMethodRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = value2;
		DataTable dataTable3 = database.GetDataTable(sqlCommand);
		DataRow[] array = dataTable.Select("imaMethodAssemblyID = " + assemblyID.ToLinq());
		if (array.Length == 1)
		{
			Assembly assembly = loadAsm(array[0]);
			loadSubAssemblies(database, assembly, dataTable, dataTable2, dataTable3);
			return assembly;
		}
		throw new M1Exception($"Assembly {assemblyID} not found in method loader.");
	}

	private void loadSubAssemblies(M1Database database, Assembly parentAsm, DataTable assembliesTable, DataTable materialsTable, DataTable operationsTable)
	{
		DataRow[] array = assembliesTable.Select("imaParentAssemblyID = " + M1Util.ConvertToLinq(parentAsm.AssemblyID), "imaMethodAssemblyID");
		foreach (DataRow row in array)
		{
			if (row.Field<bool>("imaUseMethod"))
			{
				parentAsm.SubAssemblies.Add(Load(database, new object[2]
				{
					row.Field<string>("imaPartID"),
					row.Field<string>("imaPartRevisionID")
				}, 0));
			}
			else
			{
				Assembly assembly = loadAsm(row);
				parentAsm.SubAssemblies.Add(assembly);
				loadSubAssemblies(database, assembly, assembliesTable, materialsTable, operationsTable);
				loadOperations(assembly, operationsTable);
				loadMaterials(assembly, materialsTable);
			}
		}
	}

	private static Assembly loadAsm(DataRow row)
	{
		Assembly assembly = new Assembly();
		assembly.MethodID = row.Field<string>("imaMethodID");
		assembly.MethodRevisionID = row.Field<string>("imaMethodRevisionID");
		assembly.AssemblyID = row.Field<int>("imaMethodAssemblyID");
		assembly.Level = row.Field<short>("imaLevel");
		assembly.ParentAssemblyID = row.Field<int>("imaParentAssemblyID");
		assembly.PartID = row.Field<string>("imaPartID");
		assembly.PartRevisionID = row.Field<string>("imaPartRevisionID");
		assembly.UnitOfMeasure = row.Field<string>("imaUnitOfMeasure");
		assembly.PartShortDescription = row.Field<string>("imaPartShortDescription");
		assembly.PartLongDescriptionRTF = row.Field<string>("imaPartLongDescriptionRTF");
		assembly.SourceMethodID = row.Field<string>("imaSourceMethodID");
		assembly.SourceRevisionID = row.Field<string>("imaSourceRevisionID");
		assembly.ProductionNotesRTF = row.Field<string>("imaProductionNotesRTF");
		assembly.QuantityPerParent = row.Field<decimal>("imaQuantityPerParent");
		assembly.Documents = row.Field<string>("imaDocuments");
		assembly.PullAllFromStock = row.Field<bool>("imaPullAllFromStock");
		assembly.OverlapOperationID = row.Field<int>("imaOverlapOperationID");
		assembly.OverlapDestinationLink = row.Field<byte>("imaOverlapDestinationLink");
		assembly.OverlapSourceOperationID = row.Field<int>("imaOverlapSourceOperationID");
		assembly.OverlapSourceLink = row.Field<byte>("imaOverlapSourceLink");
		assembly.OverlapOffsetTime = row.Field<decimal>("imaOverlapOffsetTime");
		assembly.AssemblyOverlap = row.Field<byte>("imaAssemblyOverlap");
		foreach (DataColumn column in row.Table.Columns)
		{
			if (column.ColumnName.StartsWith("uima", StringComparison.CurrentCultureIgnoreCase))
			{
				assembly.CustomFields.Add(column.ColumnName, row[column]);
			}
		}
		return assembly;
	}

	private static void loadMaterials(Assembly parentAsm, DataTable materialsTable)
	{
		DataRow[] array = materialsTable.Select("immMethodAssemblyID = " + M1Util.ConvertToLinq(parentAsm.AssemblyID), "immMethodMaterialID");
		foreach (DataRow row in array)
		{
			parentAsm.Materials.Add(loadMaterial(row));
		}
	}

	private static Material loadMaterial(DataRow row)
	{
		Material material = new Material();
		material.MethodID = row.Field<string>("immMethodID");
		material.MethodRevisionID = row.Field<string>("immMethodRevisionID");
		material.AssemblyID = row.Field<int>("immMethodAssemblyID");
		material.MaterialID = row.Field<int>("immMethodMaterialID");
		material.PartID = row.Field<string>("immPartID");
		material.PartRevisionID = row.Field<string>("immPartRevisionID");
		material.PartWarehouseLocationID = row.Field<string>("immPartWarehouseLocationID");
		material.PartBinID = row.Field<string>("immPartBinID");
		material.UnitOfMeasure = row.Field<string>("immUnitOfMeasure");
		material.PartShortDescription = row.Field<string>("immPartShortDescription");
		material.PartLongDescriptionText = row.Field<string>("immPartLongDescriptionText");
		material.PartLongDescriptionRTF = row.Field<string>("immPartLongDescriptionRTF");
		material.QuantityPerAssembly = row.Field<decimal>("immQuantityPerAssembly");
		material.ScrapPercent = row.Field<decimal>("immScrapPercent");
		material.ScrapQuantity = row.Field<decimal>("immScrapQuantity");
		material.EstimatedUnitCost = row.Field<decimal>("immEstimatedUnitCost");
		material.SupplierOrganizationID = row.Field<string>("immSupplierOrganizationID");
		material.PurchaseLocationID = row.Field<string>("immPurchaseLocationID");
		material.LeadTime = row.Field<short>("immLeadTime");
		material.MinimumCharge = row.Field<decimal>("immMinimumCharge");
		material.RelatedOperationID = row.Field<int>("immRelatedPartOperationID");
		material.Backflush = row.Field<bool>("immBackflush");
		material.Documents = row.Field<string>("immDocuments");
		foreach (DataColumn column in row.Table.Columns)
		{
			if (column.ColumnName.StartsWith("uimm", StringComparison.CurrentCultureIgnoreCase))
			{
				material.CustomFields.Add(column.ColumnName, row[column]);
			}
		}
		return material;
	}

	private static void loadOperations(Assembly parentAsm, DataTable operationsTable)
	{
		DataRow[] array = operationsTable.Select("imoMethodAssemblyID = " + M1Util.ConvertToLinq(parentAsm.AssemblyID), "imoMethodOperationID");
		foreach (DataRow row in array)
		{
			parentAsm.Operations.Add(loadOperation(row));
		}
	}

	private static Operation loadOperation(DataRow row)
	{
		Operation operation = new Operation();
		operation.MethodID = row.Field<string>("imoMethodID");
		operation.MethodRevisionID = row.Field<string>("imoMethodRevisionID");
		operation.AssemblyID = row.Field<int>("imoMethodAssemblyID");
		operation.OperationID = row.Field<int>("imoMethodOperationID");
		operation.OperationType = row.Field<byte>("imoOperationType");
		operation.PlantID = row.Field<string>("imoPlantID");
		operation.PlantDepartmentID = row.Field<string>("imoPlantDepartmentID");
		operation.WorkCenterID = row.Field<string>("imoWorkCenterID");
		operation.ProcessID = row.Field<string>("imoProcessID");
		operation.ProcessShortDescription = row.Field<string>("imoProcessShortDescription");
		operation.ProcessLongDescriptionText = row.Field<string>("imoProcessLongDescriptionText");
		operation.ProcessLongDescriptionRTF = row.Field<string>("imoProcessLongDescriptionRTF");
		operation.QuantityPerAssembly = row.Field<decimal>("imoQuantityPerAssembly");
		operation.OverheadRate = row.Field<decimal>("imoOverheadRate");
		operation.SetupRate = row.Field<decimal>("imoSetupRate");
		operation.QueueTime = row.Field<decimal>("imoQueueTime");
		operation.MoveTime = row.Field<decimal>("imoMoveTime");
		operation.SetupHours = row.Field<decimal>("imoSetupHours");
		operation.ProductionStandard = row.Field<decimal>("imoProductionStandard");
		operation.StandardFactor = row.Field<string>("imoStandardFactor");
		operation.OverlapOperationID = row.Field<int>("imoOverlapOperationID");
		operation.OverlapSourceLink = row.Field<byte>("imoOverlapSourceLink");
		operation.OverlapDestinationLink = row.Field<byte>("imoOverlapDestinationLink");
		operation.OverlapOffsetTime = row.Field<decimal>("imoOverlapOffsetTime");
		operation.PartID = row.Field<string>("imoPartID");
		operation.PartRevisionID = row.Field<string>("imoPartRevisionID");
		operation.UnitOfMeasure = row.Field<string>("imoUnitOfMeasure");
		operation.EstimatedUnitCost = row.Field<decimal>("imoEstimatedUnitCost");
		operation.MinimumCharge = row.Field<decimal>("imoMinimumCharge");
		operation.SetupCharge = row.Field<decimal>("imoSetupCharge");
		operation.SupplierOrganizationID = row.Field<string>("imoSupplierOrganizationID");
		operation.PurchaseLocationID = row.Field<string>("imoPurchaseLocationID");
		operation.Documents = row.Field<string>("imoDocuments");
		operation.SFEMessageText = row.Field<string>("imoSFEMessageText");
		operation.SFEMessageRTF = row.Field<string>("imoSFEMessageRTF");
		operation.InspectionType = row.Field<byte>("imoInspectionType");
		operation.MachineType = row.Field<byte>("imoMachineType");
		operation.WorkCenterMachineID = row.Field<short>("imoWorkCenterMachineID");
		operation.MachinesToSchedule = row.Field<short>("imoMachinesToSchedule");
		operation.PriceBreak1.QuantityBreak = row.Field<decimal>("imoQuantityBreak1");
		operation.PriceBreak1.UnitCost = row.Field<decimal>("imoUnitCost1");
		operation.PriceBreak2.QuantityBreak = row.Field<decimal>("imoQuantityBreak2");
		operation.PriceBreak2.UnitCost = row.Field<decimal>("imoUnitCost2");
		operation.PriceBreak3.QuantityBreak = row.Field<decimal>("imoQuantityBreak3");
		operation.PriceBreak3.UnitCost = row.Field<decimal>("imoUnitCost3");
		operation.PriceBreak4.QuantityBreak = row.Field<decimal>("imoQuantityBreak4");
		operation.PriceBreak4.UnitCost = row.Field<decimal>("imoUnitCost4");
		operation.PriceBreak5.QuantityBreak = row.Field<decimal>("imoQuantityBreak5");
		operation.PriceBreak5.UnitCost = row.Field<decimal>("imoUnitCost5");
		operation.PriceBreak6.QuantityBreak = row.Field<decimal>("imoQuantityBreak6");
		operation.PriceBreak6.UnitCost = row.Field<decimal>("imoUnitCost6");
		operation.PriceBreak7.QuantityBreak = row.Field<decimal>("imoQuantityBreak7");
		operation.PriceBreak7.UnitCost = row.Field<decimal>("imoUnitCost7");
		operation.PriceBreak8.QuantityBreak = row.Field<decimal>("imoQuantityBreak8");
		operation.PriceBreak8.UnitCost = row.Field<decimal>("imoUnitCost8");
		operation.PriceBreak9.QuantityBreak = row.Field<decimal>("imoQuantityBreak9");
		operation.PriceBreak9.UnitCost = row.Field<decimal>("imoUnitCost9");
		foreach (DataColumn column in row.Table.Columns)
		{
			if (column.ColumnName.StartsWith("uimo", StringComparison.CurrentCultureIgnoreCase))
			{
				operation.CustomFields.Add(column.ColumnName, row[column]);
			}
		}
		return operation;
	}

	public static void unLoad(M1Database database, Assembly loadedAssembly, string destinationPartID, string destinationPartRevisionID, int destinationAssemblyID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From PartAssemblies Where imaPartID = @PartID And imaPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = destinationPartID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = destinationPartRevisionID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From PartMaterials Where immMethodID = @PartID And immMethodRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = destinationPartID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = destinationPartRevisionID;
		DataTable dataTable2 = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From PartOperations Where imoMethodID = @PartID And imoMethodRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = destinationPartID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = destinationPartRevisionID;
		DataTable dataTable3 = database.GetDataTable(sqlCommand);
		if (dataTable.Select("imaMethodAssemblyID = " + destinationAssemblyID).Length == 1)
		{
			unloadSubAssemblies(database, loadedAssembly, dataTable, dataTable2, dataTable3, destinationAssemblyID);
		}
	}

	private static void unloadSubAssemblies(M1Database database, Assembly loadedAssembly, DataTable assembliesTable, DataTable materialsTable, DataTable operationsTable, int destinationAssemblyID)
	{
		foreach (Assembly subAssembly in loadedAssembly.SubAssemblies)
		{
			unloadAsm(subAssembly, assembliesTable.AddBlankRow(), destinationAssemblyID);
			unloadOperations(subAssembly, operationsTable, destinationAssemblyID);
			unloadMaterials(subAssembly, materialsTable, destinationAssemblyID);
			unloadSubAssemblies(database, subAssembly, assembliesTable, materialsTable, operationsTable, destinationAssemblyID);
		}
	}

	private static void unloadAsm(Assembly asm, DataRow row, int destinationAsmID)
	{
		row.SetField("imaMethodID", asm.MethodID);
		row.SetField("imaMethodRevisionID", asm.MethodRevisionID);
		row.SetField("imaMethodAssemblyID", destinationAsmID);
		row.SetField("imaLevel", (int)asm.Level);
		row.SetField("imaParentAssemblyID", asm.ParentAssemblyID);
		row.SetField("imaPartID", asm.PartID);
		row.SetField("imaPartRevisionID", asm.PartRevisionID);
		row.SetField("imaUnitOfMeasure", asm.UnitOfMeasure);
		row.SetField("imaPartShortDescription", asm.PartShortDescription);
		row.SetField("imaPartLongDescriptionRTF", asm.PartLongDescriptionRTF);
		row.SetField("imaSourceMethodID", asm.SourceMethodID);
		row.SetField("imaSourceRevisionID", asm.SourceRevisionID);
		row.SetField("imaProductionNotesRTF", asm.ProductionNotesRTF);
		row.SetField("imaQuantityPerParent", asm.QuantityPerParent);
		row.SetField("imaDocuments", asm.Documents);
		row.SetField("imaOverlapMethodOperationID", asm.OverlapOperationID);
		row.SetField("imaPullAllFromStock", asm.PullAllFromStock);
		row.SetField("imaOverlapOperationID", asm.OverlapOperationID);
		row.SetField("imaOverlapDestinationLink", asm.OverlapDestinationLink);
		row.SetField("imaOverlapSourceOperationID", asm.OverlapSourceOperationID);
		row.SetField("imaOverlapSourceLink", asm.OverlapSourceLink);
		row.SetField("imaOverlapOffsetTime", asm.OverlapOffsetTime);
		row.SetField("imaAssemblyOverlap", asm.AssemblyOverlap);
		foreach (KeyValuePair<string, object> customField in asm.CustomFields)
		{
			if (row.Table.Columns.Contains("uimm" + customField.Key.Substring(4)))
			{
				row["uimm" + customField.Key.Substring(4)] = customField.Value;
			}
		}
	}

	private static void unloadMaterials(Assembly parentAsm, DataTable materialsTable, int destinationAsmID)
	{
		foreach (Material material in parentAsm.Materials)
		{
			unloadMaterial(material, materialsTable.AddBlankRow(), destinationAsmID);
		}
	}

	private static void unloadMaterial(Material material, DataRow row, int destinationAsmID)
	{
		row.SetField("immMethodID", material.MethodID);
		row.SetField("immMethodRevisionID", material.MethodRevisionID);
		row.SetField("immMethodAssemblyID", destinationAsmID);
		row.SetField("immMethodMaterialID", material.MaterialID);
		row.SetField("immPartID", material.PartID);
		row.SetField("immPartRevisionID", material.PartRevisionID);
		row.SetField("immPartWarehouseLocationID", material.PartWarehouseLocationID);
		row.SetField("immPartBinID", material.PartBinID);
		row.SetField("immUnitOfMeasure", material.UnitOfMeasure);
		row.SetField("immPartShortDescription", material.PartShortDescription);
		row.SetField("immPartLongDescriptionText", material.PartLongDescriptionText);
		row.SetField("immPartLongDescriptionRTF", material.PartLongDescriptionRTF);
		row.SetField("immQuantityPerAssembly", material.QuantityPerAssembly);
		row.SetField("immScrapPercent", material.ScrapPercent);
		row.SetField("immScrapQuantity", material.ScrapQuantity);
		row.SetField("immEstimatedUnitCost", material.EstimatedUnitCost);
		row.SetField("immSupplierOrganizationID", material.SupplierOrganizationID);
		row.SetField("immPurchaseLocationID", material.PurchaseLocationID);
		row.SetField("immLeadTime", (int)material.LeadTime);
		row.SetField("immMinimumCharge", material.MinimumCharge);
		row.SetField("immRelatedPartOperationID", material.RelatedOperationID);
		row.SetField("immBackflush", material.Backflush);
		row.SetField("immDocuments", material.Documents);
		foreach (KeyValuePair<string, object> customField in material.CustomFields)
		{
			if (row.Table.Columns.Contains("uimm" + customField.Key.Substring(4)))
			{
				row["uimm" + customField.Key.Substring(4)] = customField.Value;
			}
		}
	}

	private static void unloadOperations(Assembly parentAsm, DataTable operationsTable, int destinationAsmID)
	{
		foreach (Operation operation in parentAsm.Operations)
		{
			unloadOperation(operation, operationsTable.AddBlankRow(), destinationAsmID);
		}
	}

	private static void unloadOperation(Operation operation, DataRow row, int destinationAsmID)
	{
		row.SetField("imoMethodID", operation.MethodID);
		row.SetField("imoMethodRevisionID", operation.MethodRevisionID);
		row.SetField("imoMethodAssemblyID", destinationAsmID);
		row.SetField("imoMethodOperationID", operation.OperationID);
		row.SetField("imoOperationType", (int)operation.OperationType);
		row.SetField("imoPlantID", operation.PlantID);
		row.SetField("imoPlantDepartmentID", operation.PlantDepartmentID);
		row.SetField("imoWorkCenterID", operation.WorkCenterID);
		row.SetField("imoProcessID", operation.ProcessID);
		row.SetField("imoProcessShortDescription", operation.ProcessShortDescription);
		row.SetField("imoProcessLongDescriptionText", operation.ProcessLongDescriptionText);
		row.SetField("imoProcessLongDescriptionRTF", operation.ProcessLongDescriptionRTF);
		row.SetField("imoQuantityPerAssembly", operation.QuantityPerAssembly);
		row.SetField("imoOverheadRate", operation.OverheadRate);
		row.SetField("imoSetupRate", operation.SetupRate);
		row.SetField("imoSetupHours", operation.SetupHours);
		row.SetField("imoQueueTime", operation.QueueTime);
		row.SetField("imoMoveTime", operation.MoveTime);
		row.SetField("imoProductionStandard", operation.ProductionStandard);
		row.SetField("imoStandardFactor", operation.StandardFactor);
		row.SetField("imoOverlapOperationID", operation.OverlapOperationID);
		row.SetField("imoOverlapSourceLink", operation.OverlapSourceLink);
		row.SetField("imoOverlapDestinationLink", operation.OverlapDestinationLink);
		row.SetField("imoOverlapOffsetTime", operation.OverlapOffsetTime);
		row.SetField("imoPartID", operation.PartID);
		row.SetField("imoPartRevisionID", operation.PartRevisionID);
		row.SetField("imoUnitOfMeasure", operation.UnitOfMeasure);
		row.SetField("imoEstimatedUnitCost", operation.EstimatedUnitCost);
		row.SetField("imoMinimumCharge", operation.MinimumCharge);
		row.SetField("imoSetupCharge", operation.SetupCharge);
		row.SetField("imoSupplierOrganizationID", operation.SupplierOrganizationID);
		row.SetField("imoPurchaseLocationID", operation.PurchaseLocationID);
		row.SetField("imoDocuments", operation.Documents);
		row.SetField("imoSFEMessageText", operation.SFEMessageText);
		row.SetField("imoSFEMessageRTF", operation.SFEMessageRTF);
		row.SetField("imoInspectionType", (int)operation.InspectionType);
		row.SetField("imoMachineType", (int)operation.MachineType);
		row.SetField("imoWorkCenterMachineID", operation.WorkCenterMachineID);
		row.SetField("imoMachinesToSchedule", (decimal)operation.MachinesToSchedule);
		row.SetField("imoQuantityBreak1", operation.PriceBreak1.QuantityBreak);
		row.SetField("imoUnitCost1", operation.PriceBreak1.UnitCost);
		row.SetField("imoQuantityBreak2", operation.PriceBreak2.QuantityBreak);
		row.SetField("imoUnitCost2", operation.PriceBreak2.UnitCost);
		row.SetField("imoQuantityBreak3", operation.PriceBreak3.QuantityBreak);
		row.SetField("imoUnitCost3", operation.PriceBreak3.UnitCost);
		row.SetField("imoQuantityBreak4", operation.PriceBreak4.QuantityBreak);
		row.SetField("imoUnitCost4", operation.PriceBreak4.UnitCost);
		row.SetField("imoQuantityBreak5", operation.PriceBreak5.QuantityBreak);
		row.SetField("imoUnitCost5", operation.PriceBreak5.UnitCost);
		row.SetField("imoQuantityBreak6", operation.PriceBreak6.QuantityBreak);
		row.SetField("imoUnitCost6", operation.PriceBreak6.UnitCost);
		row.SetField("imoQuantityBreak7", operation.PriceBreak7.QuantityBreak);
		row.SetField("imoUnitCost7", operation.PriceBreak7.UnitCost);
		row.SetField("imoQuantityBreak8", operation.PriceBreak8.QuantityBreak);
		row.SetField("imoUnitCost8", operation.PriceBreak8.UnitCost);
		row.SetField("imoQuantityBreak9", operation.PriceBreak9.QuantityBreak);
		row.SetField("imoUnitCost9", operation.PriceBreak9.UnitCost);
		foreach (KeyValuePair<string, object> customField in operation.CustomFields)
		{
			if (row.Table.Columns.Contains("uimo" + customField.Key.Substring(4)))
			{
				row["uimo" + customField.Key.Substring(4)] = customField.Value;
			}
		}
	}
}
