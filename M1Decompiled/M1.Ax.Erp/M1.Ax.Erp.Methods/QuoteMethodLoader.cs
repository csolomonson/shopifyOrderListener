using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.Methods;

public class QuoteMethodLoader : IMethodLoader
{
	public Assembly Load(M1Database database, object[] keyValues, int assemblyID)
	{
		string value = (string)keyValues[0];
		short num = Convert.ToInt16(keyValues[1]);
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From QuoteAssemblies Where qmaQuoteID = @QuoteID And qmaQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = num;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From QuoteMaterials Where qmmQuoteID = @QuoteID And qmmQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = num;
		DataTable dataTable2 = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From QuoteOperations Where qmoQuoteID = @QuoteID And qmoQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = num;
		DataTable dataTable3 = database.GetDataTable(sqlCommand);
		DataRow[] array = dataTable.Select("qmaQuoteAssemblyID = " + assemblyID.ToLinq());
		if (array.Length == 1)
		{
			List<string> list = new List<string>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("uqma", StringComparison.CurrentCultureIgnoreCase))
				{
					list.Add(column.ColumnName);
				}
			}
			List<string> list2 = new List<string>();
			foreach (DataColumn column2 in dataTable2.Columns)
			{
				if (column2.ColumnName.StartsWith("uqmm", StringComparison.CurrentCultureIgnoreCase))
				{
					list2.Add(column2.ColumnName);
				}
			}
			List<string> list3 = new List<string>();
			foreach (DataColumn column3 in dataTable3.Columns)
			{
				if (column3.ColumnName.StartsWith("uqmo", StringComparison.CurrentCultureIgnoreCase))
				{
					list3.Add(column3.ColumnName);
				}
			}
			return loadSubAssemblies(database, array[0], dataTable, dataTable2, dataTable3, list, list2, list3);
		}
		throw new M1Exception($"Assembly {assemblyID} not found in method loader.");
	}

	private static Assembly loadSubAssemblies(M1Database database, DataRow parentAsmRow, DataTable assembliesTable, DataTable materialsTable, DataTable operationsTable, List<string> asmCustomFields, List<string> matCustomFields, List<string> oprCustomFields)
	{
		Assembly assembly = loadAsm(parentAsmRow, asmCustomFields);
		DataRow[] array = assembliesTable.Select("qmaParentAssemblyID = " + M1Util.ConvertToLinq(assembly.AssemblyID) + " And qmaQuoteAssemblyID <> 0", "qmaQuoteAssemblyID");
		foreach (DataRow parentAsmRow2 in array)
		{
			assembly.SubAssemblies.Add(loadSubAssemblies(database, parentAsmRow2, assembliesTable, materialsTable, operationsTable, asmCustomFields, matCustomFields, oprCustomFields));
		}
		array = operationsTable.Select("qmoQuoteAssemblyID = " + M1Util.ConvertToLinq(assembly.AssemblyID), "qmoQuoteOperationID");
		foreach (DataRow row in array)
		{
			assembly.Operations.Add(loadOperation(row, oprCustomFields));
		}
		array = materialsTable.Select("qmmQuoteAssemblyID = " + M1Util.ConvertToLinq(assembly.AssemblyID), "qmmQuoteMaterialID");
		foreach (DataRow row2 in array)
		{
			assembly.Materials.Add(loadMaterial(row2, matCustomFields));
		}
		return assembly;
	}

	private static Assembly loadAsm(DataRow row, List<string> asmCustomFields)
	{
		Assembly assembly = new Assembly();
		assembly.AssemblyID = row.Field<int>("qmaQuoteAssemblyID");
		assembly.Level = row.Field<short>("qmaLevel");
		assembly.ParentAssemblyID = row.Field<int>("qmaParentAssemblyID");
		assembly.PartID = row.Field<string>("qmaPartID");
		assembly.PartRevisionID = row.Field<string>("qmaPartRevisionID");
		assembly.UnitOfMeasure = row.Field<string>("qmaUnitOfMeasure");
		assembly.PartShortDescription = row.Field<string>("qmaPartShortDescription");
		assembly.PartLongDescriptionRTF = row.Field<string>("qmaPartLongDescriptionRTF");
		assembly.SourceMethodID = row.Field<string>("qmaSourceMethodID");
		assembly.SourceRevisionID = row.Field<string>("qmaSourceRevisionID");
		assembly.ProductionNotesRTF = row.Field<string>("qmaProductionNotesRTF");
		assembly.QuantityPerParent = row.Field<decimal>("qmaQuantityPerParent");
		assembly.Documents = row.Field<string>("qmaDocuments");
		assembly.PullAllFromStock = row.Field<bool>("qmaPullAllFromStock");
		assembly.OverlapOperationID = row.Field<int>("qmaOverlapOperationID");
		assembly.OverlapDestinationLink = row.Field<byte>("qmaOverlapDestinationLink");
		assembly.OverlapSourceOperationID = row.Field<int>("qmaOverlapSourceOperationID");
		assembly.OverlapSourceLink = row.Field<byte>("qmaOverlapSourceLink");
		assembly.OverlapOffsetTime = row.Field<decimal>("qmaOverlapOffsetTime");
		assembly.AssemblyOverlap = row.Field<byte>("qmaAssemblyOverlap");
		foreach (string asmCustomField in asmCustomFields)
		{
			assembly.CustomFields.Add(asmCustomField, row[asmCustomField]);
		}
		return assembly;
	}

	private static Material loadMaterial(DataRow row, List<string> matCustomFields)
	{
		Material material = new Material();
		material.AssemblyID = row.Field<int>("qmmQuoteAssemblyID");
		material.MaterialID = row.Field<int>("qmmQuoteMaterialID");
		material.PartID = row.Field<string>("qmmPartID");
		material.PartRevisionID = row.Field<string>("qmmPartRevisionID");
		material.PartWarehouseLocationID = row.Field<string>("qmmPartWarehouseLocationID");
		material.PartBinID = row.Field<string>("qmmPartBinID");
		material.UnitOfMeasure = row.Field<string>("qmmUnitOfMeasure");
		material.PartShortDescription = row.Field<string>("qmmPartShortDescription");
		material.PartLongDescriptionText = row.Field<string>("qmmPartLongDescriptionText");
		material.PartLongDescriptionRTF = row.Field<string>("qmmPartLongDescriptionRTF");
		material.QuantityPerAssembly = row.Field<decimal>("qmmQuantityPerAssembly");
		material.ScrapPercent = row.Field<decimal>("qmmScrapPercent");
		material.ScrapQuantity = row.Field<decimal>("qmmScrapQuantity");
		material.EstimatedUnitCost = row.Field<decimal>("qmmEstimatedUnitCost");
		material.SupplierOrganizationID = row.Field<string>("qmmSupplierOrganizationID");
		material.PurchaseLocationID = row.Field<string>("qmmPurchaseLocationID");
		material.LeadTime = row.Field<short>("qmmLeadTime");
		material.MinimumCharge = row.Field<decimal>("qmmMinimumCharge");
		material.RelatedOperationID = row.Field<int>("qmmRelatedQuoteOperationID");
		material.Backflush = row.Field<bool>("qmmBackflush");
		material.Documents = row.Field<string>("qmmDocuments");
		foreach (string matCustomField in matCustomFields)
		{
			material.CustomFields.Add(matCustomField, row[matCustomField]);
		}
		return material;
	}

	private static Operation loadOperation(DataRow row, List<string> oprCustomFields)
	{
		Operation operation = new Operation();
		operation.AssemblyID = row.Field<int>("qmoQuoteAssemblyID");
		operation.OperationID = row.Field<int>("qmoQuoteOperationID");
		operation.OperationType = row.Field<byte>("qmoOperationType");
		operation.PlantID = row.Field<string>("qmoPlantID");
		operation.PlantDepartmentID = row.Field<string>("qmoPlantDepartmentID");
		operation.WorkCenterID = row.Field<string>("qmoWorkCenterID");
		operation.ProcessID = row.Field<string>("qmoProcessID");
		operation.ProcessShortDescription = row.Field<string>("qmoProcessShortDescription");
		operation.ProcessLongDescriptionText = row.Field<string>("qmoProcessLongDescriptionText");
		operation.ProcessLongDescriptionRTF = row.Field<string>("qmoProcessLongDescriptionRTF");
		operation.QuantityPerAssembly = row.Field<decimal>("qmoQuantityPerAssembly");
		operation.OverheadRate = row.Field<decimal>("qmoOverheadRate");
		operation.SetupRate = row.Field<decimal>("qmoSetupRate");
		operation.QueueTime = row.Field<decimal>("qmoQueueTime");
		operation.MoveTime = row.Field<decimal>("qmoMoveTime");
		operation.SetupHours = row.Field<decimal>("qmoSetupHours");
		operation.ProductionStandard = row.Field<decimal>("qmoProductionStandard");
		operation.StandardFactor = row.Field<string>("qmoStandardFactor");
		operation.OverlapOperationID = row.Field<int>("qmoOverlapOperationID");
		operation.OverlapSourceLink = row.Field<byte>("qmoOverlapSourceLink");
		operation.OverlapDestinationLink = row.Field<byte>("qmoOverlapDestinationLink");
		operation.OverlapOffsetTime = row.Field<decimal>("qmoOverlapOffsetTime");
		operation.PartID = row.Field<string>("qmoPartID");
		operation.PartRevisionID = row.Field<string>("qmoPartRevisionID");
		operation.UnitOfMeasure = row.Field<string>("qmoUnitOfMeasure");
		operation.EstimatedUnitCost = row.Field<decimal>("qmoEstimatedUnitCost");
		operation.MinimumCharge = row.Field<decimal>("qmoMinimumCharge");
		operation.SetupCharge = row.Field<decimal>("qmoSetupCharge");
		operation.SupplierOrganizationID = row.Field<string>("qmoSupplierOrganizationID");
		operation.PurchaseLocationID = row.Field<string>("qmoPurchaseLocationID");
		operation.Documents = row.Field<string>("qmoDocuments");
		operation.SFEMessageText = row.Field<string>("qmoSFEMessageText");
		operation.SFEMessageRTF = row.Field<string>("qmoSFEMessageRTF");
		operation.InspectionType = row.Field<byte>("qmoInspectionType");
		operation.MachineType = row.Field<byte>("qmoMachineType");
		operation.WorkCenterMachineID = row.Field<short>("qmoWorkCenterMachineID");
		operation.MachinesToSchedule = row.Field<short>("qmoMachinesToSchedule");
		operation.PriceBreak1.QuantityBreak = row.Field<decimal>("qmoQuantityBreak1");
		operation.PriceBreak1.UnitCost = row.Field<decimal>("qmoUnitCost1");
		operation.PriceBreak2.QuantityBreak = row.Field<decimal>("qmoQuantityBreak2");
		operation.PriceBreak2.UnitCost = row.Field<decimal>("qmoUnitCost2");
		operation.PriceBreak3.QuantityBreak = row.Field<decimal>("qmoQuantityBreak3");
		operation.PriceBreak3.UnitCost = row.Field<decimal>("qmoUnitCost3");
		operation.PriceBreak4.QuantityBreak = row.Field<decimal>("qmoQuantityBreak4");
		operation.PriceBreak4.UnitCost = row.Field<decimal>("qmoUnitCost4");
		operation.PriceBreak5.QuantityBreak = row.Field<decimal>("qmoQuantityBreak5");
		operation.PriceBreak5.UnitCost = row.Field<decimal>("qmoUnitCost5");
		operation.PriceBreak6.QuantityBreak = row.Field<decimal>("qmoQuantityBreak6");
		operation.PriceBreak6.UnitCost = row.Field<decimal>("qmoUnitCost6");
		operation.PriceBreak7.QuantityBreak = row.Field<decimal>("qmoQuantityBreak7");
		operation.PriceBreak7.UnitCost = row.Field<decimal>("qmoUnitCost7");
		operation.PriceBreak8.QuantityBreak = row.Field<decimal>("qmoQuantityBreak8");
		operation.PriceBreak8.UnitCost = row.Field<decimal>("qmoUnitCost8");
		operation.PriceBreak9.QuantityBreak = row.Field<decimal>("qmoQuantityBreak9");
		operation.PriceBreak9.UnitCost = row.Field<decimal>("qmoUnitCost9");
		foreach (string oprCustomField in oprCustomFields)
		{
			operation.CustomFields.Add(oprCustomField, row[oprCustomField]);
		}
		return operation;
	}

	public static void unLoad(M1Database database, Assembly loadedAssembly, object[] destinationKeys, int assemblyID)
	{
		string value = (string)destinationKeys[0];
		int num = (int)destinationKeys[1];
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From QuoteAssemblies Where qmoQuoteID = @QuoteID And qmaQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = num;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From QuoteMaterials Where qmmQuoteID = @QuoteID And qmmQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = num;
		DataTable dataTable2 = database.GetDataTable(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select * From QuoteOperations Where qmoQuoteID = @QuoteID And qmoQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = num;
		DataTable dataTable3 = database.GetDataTable(sqlCommand);
		if (dataTable.Select("qmaAssemblyID = " + assemblyID).Length == 1)
		{
			unloadSubAssemblies(database, loadedAssembly, dataTable, dataTable2, dataTable3, assemblyID);
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
		row.SetField("qmaQuoteAssemblyID", destinationAsmID);
		row.SetField("qmaLevel", (int)asm.Level);
		row.SetField("qmaParentAssemblyID", asm.ParentAssemblyID);
		row.SetField("qmaPartID", asm.PartID);
		row.SetField("qmaPartRevisionID", asm.PartRevisionID);
		row.SetField("qmaUnitOfMeasure", asm.UnitOfMeasure);
		row.SetField("qmaPartShortDescription", asm.PartShortDescription);
		row.SetField("qmaPartLongDescriptionRTF", asm.PartLongDescriptionRTF);
		row.SetField("qmaSourceMethodID", asm.SourceMethodID);
		row.SetField("qmaSourceRevisionID", asm.SourceRevisionID);
		row.SetField("qmaProductionNotesRTF", asm.ProductionNotesRTF);
		row.SetField("qmaQuantityPerParent", asm.QuantityPerParent);
		row.SetField("qmaDocuments", asm.Documents);
		row.SetField("qmaPullAllFromStock", asm.PullAllFromStock);
		row.SetField("qmaOverlapOperationID", asm.OverlapOperationID);
		row.SetField("qmaOverlapDestinationLink", asm.OverlapDestinationLink);
		row.SetField("qmaOverlapSourceOperationID", asm.OverlapSourceOperationID);
		row.SetField("qmaOverlapSourceLink", asm.OverlapSourceLink);
		row.SetField("qmaOverlapOffsetTime", asm.OverlapOffsetTime);
		row.SetField("qmaAssemblyOverlap", asm.AssemblyOverlap);
		foreach (KeyValuePair<string, object> customField in asm.CustomFields)
		{
			if (row.Table.Columns.Contains("uqmm" + customField.Key.Substring(4)))
			{
				row["uqmm" + customField.Key.Substring(4)] = customField.Value;
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
		row.SetField("qmmQuoteAssemblyID", destinationAsmID);
		row.SetField("qmmQuoteMaterialID", material.MaterialID);
		row.SetField("qmmPartID", material.PartID);
		row.SetField("qmmPartRevisionID", material.PartRevisionID);
		row.SetField("qmmPartWarehouseLocationID", material.PartWarehouseLocationID);
		row.SetField("qmmPartBinID", material.PartBinID);
		row.SetField("qmmUnitOfMeasure", material.UnitOfMeasure);
		row.SetField("qmmPartShortDescription", material.PartShortDescription);
		row.SetField("qmmPartLongDescriptionText", material.PartLongDescriptionText);
		row.SetField("qmmPartLongDescriptionRTF", material.PartLongDescriptionRTF);
		row.SetField("qmmQuantityPerAssembly", material.QuantityPerAssembly);
		row.SetField("qmmScrapPercent", material.ScrapPercent);
		row.SetField("qmmScrapQuantity", material.ScrapQuantity);
		row.SetField("qmmEstimatedUnitCost", material.EstimatedUnitCost);
		row.SetField("qmmSupplierOrganizationID", material.SupplierOrganizationID);
		row.SetField("qmmPurchaseLocationID", material.PurchaseLocationID);
		row.SetField("qmmLeadTime", (int)material.LeadTime);
		row.SetField("qmmMinimumCharge", material.MinimumCharge);
		row.SetField("qmmRelatedPartOperationID", material.RelatedOperationID);
		row.SetField("qmmBackflush", material.Backflush);
		row.SetField("qmmDocuments", material.Documents);
		foreach (KeyValuePair<string, object> customField in material.CustomFields)
		{
			if (row.Table.Columns.Contains("uqmm" + customField.Key.Substring(4)))
			{
				row["uqmm" + customField.Key.Substring(4)] = customField.Value;
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
		row.SetField("qmoQuoteAssemblyID", destinationAsmID);
		row.SetField("qmoQuoteOperationID", operation.OperationID);
		row.SetField("qmoOperationType", (int)operation.OperationType);
		row.SetField("qmoPlantID", operation.PlantID);
		row.SetField("qmoPlantDepartmentID", operation.PlantDepartmentID);
		row.SetField("qmoWorkCenterID", operation.WorkCenterID);
		row.SetField("qmoProcessID", operation.ProcessID);
		row.SetField("qmoProcessShortDescription", operation.ProcessShortDescription);
		row.SetField("qmoProcessLongDescriptionText", operation.ProcessLongDescriptionText);
		row.SetField("qmoProcessLongDescriptionRTF", operation.ProcessLongDescriptionRTF);
		row.SetField("qmoQuantityPerAssembly", operation.QuantityPerAssembly);
		row.SetField("qmoOverheadRate", operation.OverheadRate);
		row.SetField("qmoSetupRate", operation.SetupRate);
		row.SetField("qmoSetupHours", operation.SetupHours);
		row.SetField("qmoQueueTime", operation.QueueTime);
		row.SetField("qmoMoveTime", operation.MoveTime);
		row.SetField("qmoProductionStandard", operation.ProductionStandard);
		row.SetField("qmoStandardFactor", operation.StandardFactor);
		row.SetField("qmoOverlapOperationID", operation.OverlapOperationID);
		row.SetField("qmoOverlapSourceLink", operation.OverlapSourceLink);
		row.SetField("qmoOverlapDestinationLink", operation.OverlapDestinationLink);
		row.SetField("qmoOverlapOffsetTime", operation.OverlapOffsetTime);
		row.SetField("qmoPartID", operation.PartID);
		row.SetField("qmoPartRevisionID", operation.PartRevisionID);
		row.SetField("qmoUnitOfMeasure", operation.UnitOfMeasure);
		row.SetField("qmoEstimatedUnitCost", operation.EstimatedUnitCost);
		row.SetField("qmoMinimumCharge", operation.MinimumCharge);
		row.SetField("qmoSetupCharge", operation.SetupCharge);
		row.SetField("qmoSupplierOrganizationID", operation.SupplierOrganizationID);
		row.SetField("qmoPurchaseLocationID", operation.PurchaseLocationID);
		row.SetField("qmoDocuments", operation.Documents);
		row.SetField("qmoSFEMessageText", operation.SFEMessageText);
		row.SetField("qmoSFEMessageRTF", operation.SFEMessageRTF);
		row.SetField("qmoInspectionType", (int)operation.InspectionType);
		row.SetField("qmoMachineType", (int)operation.MachineType);
		row.SetField("qmoWorkCenterMachineID", operation.WorkCenterMachineID);
		row.SetField("qmoMachinesToSchedule", (decimal)operation.MachinesToSchedule);
		row.SetField("qmoQuantityBreak1", operation.PriceBreak1.QuantityBreak);
		row.SetField("qmoUnitCost1", operation.PriceBreak1.UnitCost);
		row.SetField("qmoQuantityBreak2", operation.PriceBreak2.QuantityBreak);
		row.SetField("qmoUnitCost2", operation.PriceBreak2.UnitCost);
		row.SetField("qmoQuantityBreak3", operation.PriceBreak3.QuantityBreak);
		row.SetField("qmoUnitCost3", operation.PriceBreak3.UnitCost);
		row.SetField("qmoQuantityBreak4", operation.PriceBreak4.QuantityBreak);
		row.SetField("qmoUnitCost4", operation.PriceBreak4.UnitCost);
		row.SetField("qmoQuantityBreak5", operation.PriceBreak5.QuantityBreak);
		row.SetField("qmoUnitCost5", operation.PriceBreak5.UnitCost);
		row.SetField("qmoQuantityBreak6", operation.PriceBreak6.QuantityBreak);
		row.SetField("qmoUnitCost6", operation.PriceBreak6.UnitCost);
		row.SetField("qmoQuantityBreak7", operation.PriceBreak7.QuantityBreak);
		row.SetField("qmoUnitCost7", operation.PriceBreak7.UnitCost);
		row.SetField("qmoQuantityBreak8", operation.PriceBreak8.QuantityBreak);
		row.SetField("qmoUnitCost8", operation.PriceBreak8.UnitCost);
		row.SetField("qmoQuantityBreak9", operation.PriceBreak9.QuantityBreak);
		row.SetField("qmoUnitCost9", operation.PriceBreak9.UnitCost);
		foreach (KeyValuePair<string, object> customField in operation.CustomFields)
		{
			if (row.Table.Columns.Contains("uqmo" + customField.Key.Substring(4)))
			{
				row["uqmo" + customField.Key.Substring(4)] = customField.Value;
			}
		}
	}
}
