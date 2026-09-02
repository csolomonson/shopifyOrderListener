using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Ax.Erp.JobSchedule;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Job
{
	public void CreateJob(M1Database database, string orderID, int orderLineID, int orderDeliveryID, string jobID, double productionQty, DateTime? requiredDate, bool planningComplete)
	{
		if (string.IsNullOrWhiteSpace(jobID))
		{
			throw new M1Exception("Job ID is required.");
		}
		if (DoesJobExist(database, null, jobID))
		{
			throw new M1Exception("Job " + jobID + " already exists in the Jobs table.");
		}
		if (!(database.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary))
		{
			return;
		}
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("SalesOrderLines", "Jobs");
		SqlCommand sqlCommand = database.NewSqlCommand("select ompSalesOrderID,ompCustomerOrganizationID,ompShipOrganizationID,ompShipLocationID,ompProjectID,ompPlantID,ompPlantDepartmentID,IsNull(cmoJobPriorityID,0) As cmoJobPriorityID from SalesOrders Left Outer Join Organizations On cmoOrganizationID = ompCustomerOrganizationID where ompSalesOrderID = @OrderID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Sales Order " + orderID + " does not exist in the SalesOrders table.");
		}
		DataRow dataRow = dataTable.Rows[0];
		sqlCommand = database.NewSqlCommand("Select omlSalesOrderID,omlSalesOrderLineID,omlPartID,omlPartShortDescription,omlPartRevisionID,omlUnitOfMeasure,omlPartLongDescriptionRTF,omlPartLongDescriptionText,omlTimeAndMaterial,omlProjectID,omlProjectAreaID,omlRMAClaimID,omlRMAClaimLineID" + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " From SalesOrderLines Where omlSalesOrderID = @OrderID And omlSalesOrderLineID = @LineID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = orderLineID;
		DataTable dataTable2 = database.GetDataTable(sqlCommand);
		if (dataTable2.Rows.Count == 0)
		{
			throw new M1Exception($"Sales Order {orderID} Line {orderLineID} does not exist in the SalesOrderLines table.");
		}
		DataRow dataRow2 = dataTable2.Rows[0];
		if (orderDeliveryID == 0)
		{
			sqlCommand = database.NewSqlCommand("Select Top 1 omdPartWarehouseLocationID,omdPartBinId From SalesOrderDeliveries Where omdSalesOrderID = @OrderID And omdSalesOrderLineID = @LineID Order By omdSalesOrderDeliveryID Asc");
			sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = orderLineID;
		}
		else
		{
			sqlCommand = database.NewSqlCommand("Select omdPartWarehouseLocationID,omdPartBinId From SalesOrderDeliveries Where omdSalesOrderID = @OrderID And omdSalesOrderLineID = @LineID And omdSalesOrderDeliveryID = @DeliveryID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = orderLineID;
			sqlCommand.Parameters.Add(new SqlParameter("@DeliveryID", SqlDbType.Int)).Value = orderDeliveryID;
		}
		DataTable dataTable3 = database.GetDataTable(sqlCommand);
		if (dataTable3.Rows.Count == 0)
		{
			throw new M1Exception($"Delivery not found for Sales Order {orderID} Line {orderLineID}.");
		}
		DataRow dataRow3 = dataTable3.Rows[0];
		using M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.DataSourceTable = "Jobs";
		if (m1BindingSource.AddNew() is DataRow dataRow4)
		{
			dataRow4["jmpJobID"] = jobID;
			dataRow4["jmpPlantID"] = dataRow["ompPlantID"];
			dataRow4["jmpPlantDepartmentID"] = dataRow["ompPlantDepartmentID"];
			dataRow4["jmpCustomerOrganizationID"] = dataRow["ompCustomerOrganizationID"];
			dataRow4["jmpShipOrganizationID"] = dataRow["ompShipOrganizationID"];
			dataRow4["jmpShipLocationID"] = dataRow["ompShipLocationID"];
			dataRow4["jmpJobPriorityID"] = dataRow["cmoJobPriorityID"];
			dataRow4.SetField("jmpProductionDueDate", requiredDate);
			dataRow4["jmpPartID"] = dataRow2["omlPartID"];
			dataRow4["jmpPartRevisionID"] = dataRow2["omlPartRevisionID"];
			dataRow4["jmpPartWarehouseLocationID"] = dataRow3["omdPartWarehouseLocationID"];
			dataRow4["jmpPartBinID"] = dataRow3["omdPartBinId"];
			dataRow4["jmpUnitOfMeasure"] = dataRow2["omlUnitOfMeasure"];
			dataRow4["jmpPartShortDescription"] = dataRow2["omlPartShortDescription"];
			dataRow4["jmpPartLongDescriptionRTF"] = dataRow2["omlPartLongDescriptionRTF"];
			dataRow4["jmpPartLongDescriptionText"] = dataRow2["omlPartLongDescriptionText"];
			dataRow4["jmpOrderQuantity"] = productionQty;
			dataRow4["jmpTimeAndMaterial"] = dataRow2["omlTimeAndMaterial"];
			dataRow4["jmpProjectID"] = dataRow2["omlProjectID"];
			dataRow4["jmpProjectAreaID"] = dataRow2["omlProjectAreaID"];
			dataRow4["jmpRMAClaimID"] = dataRow2["omlRMAClaimID"];
			dataRow4["jmpRMAClaimLineID"] = dataRow2["omlRMAClaimLineID"];
			dataRow4["jmpPlanningComplete"] = planningComplete;
			dataRow4["jmpReadyToPrint"] = true;
			matchingFieldsInfo.CopyData(dataRow2, dataRow4);
		}
		m1BindingSource.SaveData();
	}

	public string CreateJobEx(M1Database database, SqlTransaction transaction, string jobID, string partID, string revisionID, string partDesc, string uoM, double orderQty, DateTime? requiredDate, string orderID, int orderLineID, int orderDeliveryID, double inventoryQty = 0.0, string plantID = "", string plantDept = "", string callID = "", string orgID = "", string shipOrgID = "", string shipLocationID = "", string warehouseID = "", string warehouseBinID = "")
	{
		string text = string.Empty;
		string value = string.Empty;
		string value2 = string.Empty;
		string text2 = string.Empty;
		string value3 = string.Empty;
		string value4 = string.Empty;
		string value5 = string.Empty;
		string value6 = string.Empty;
		byte b = 0;
		bool flag = false;
		bool flag2 = false;
		if (transaction == null)
		{
			flag2 = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			if (string.IsNullOrWhiteSpace(jobID))
			{
				text += "Job ID is required.\n";
			}
			if (string.IsNullOrWhiteSpace(partID))
			{
				text += "Part ID is required.\n";
			}
			else
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Select imrShortDescription,imrInventoryUnitOfMeasure,imrLongDescriptionRTF,imrLongDescriptionText from PartRevisions where imrPartID = @PartID And imrPartRevisionID = @PartRevisionID");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
				DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
				if (dataTable.Rows.Count != 0)
				{
					if (string.IsNullOrWhiteSpace(partDesc))
					{
						partDesc = dataTable.Rows[0].Field<string>("imrShortDescription");
					}
					if (string.IsNullOrWhiteSpace(uoM))
					{
						uoM = dataTable.Rows[0].Field<string>("imrInventoryUnitOfMeasure");
					}
					value = dataTable.Rows[0].Field<string>("imrLongDescriptionText");
					value2 = dataTable.Rows[0].Field<string>("imrLongDescriptionRTF");
				}
				dataTable = null;
			}
			if (string.IsNullOrWhiteSpace(partDesc))
			{
				text += "Part Description is required if the part does not exist in inventory.\n";
			}
			if (DoesJobExist(database, transaction, jobID))
			{
				text = text + "Job " + jobID + " already exists in the Jobs table.\n";
			}
			if (database.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary)
			{
				m1DataDictionary.FindMatchingFields("SalesOrderLines", "Jobs");
			}
			if (!string.IsNullOrWhiteSpace(orderID))
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Select ompCustomerOrganizationID,ompShipOrganizationID,ompShipLocationID,omlProjectID,omlProjectAreaID,omlPartLongDescriptionRTF,omlPartLongDescriptionText,omlTimeAndMaterial from SalesOrders INNER JOIN SalesOrderlines on ompSalesOrderID = omlSalesOrderID where ompSalesOrderID = @OrderID and omlSalesOrderLineID = @LineID");
				sqlCommand2.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
				sqlCommand2.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = orderLineID;
				DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
				if (dataTable2.Rows.Count == 0)
				{
					text += $"Sales Order {orderID} Line {orderLineID} does not exist in the SalesOrderLines table.\n";
				}
				else
				{
					DataRow row = dataTable2.Rows[0];
					text2 = row.Field<string>("ompCustomerOrganizationID");
					value3 = row.Field<string>("ompShipOrganizationID");
					value4 = row.Field<string>("ompShipLocationID");
					value2 = row.Field<string>("omlPartLongDescriptionRTF");
					value = row.Field<string>("omlPartLongDescriptionText");
					value5 = row.Field<string>("omlProjectID");
					value6 = row.Field<string>("omlProjectAreaID");
					flag = row.Field<bool>("omlTimeAndMaterial");
				}
				dataTable2 = null;
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				SqlCommand sqlCommand3 = database.NewSqlCommand("Select cmoJobPriorityID From Organizations Where cmoOrganizationID = @CustomerID");
				sqlCommand3.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = text2;
				DataTable dataTable3 = database.GetDataTable(sqlCommand3, transaction);
				if (dataTable3.Rows.Count != 0)
				{
					b = (byte)dataTable3.Rows[0].Field<short>("cmoJobPriorityID");
				}
				dataTable3 = null;
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			using (M1BindingSource m1BindingSource = new M1BindingSource(database, transaction))
			{
				m1BindingSource.DataSourceTable = "Jobs";
				DataRow dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["jmpJobID"] = jobID.ToUpper();
				dataRow["jmpPlantID"] = plantID;
				dataRow["jmpPlantDepartmentID"] = plantDept;
				dataRow["jmpCustomerOrganizationID"] = ((!string.IsNullOrWhiteSpace(orgID)) ? orgID : text2);
				dataRow["jmpShipOrganizationID"] = value3;
				dataRow["jmpShipLocationID"] = value4;
				dataRow["jmpJobPriorityID"] = b;
				dataRow.SetField("jmpJobDate", DateTime.Now);
				dataRow.SetField("jmpProductionDueDate", requiredDate);
				dataRow["jmpPartID"] = partID;
				dataRow["jmpPartRevisionID"] = revisionID;
				dataRow["jmpUnitOfMeasure"] = uoM;
				dataRow["jmpPartShortDescription"] = partDesc;
				dataRow["jmpPartLongDescriptionRTF"] = value2;
				dataRow["jmpPartLongDescriptionText"] = value;
				dataRow["jmpOrderQuantity"] = orderQty;
				dataRow["jmpInventoryQuantity"] = inventoryQty;
				dataRow["jmpProjectID"] = value5;
				dataRow["jmpProjectAreaID"] = value6;
				dataRow["jmpFirm"] = true;
				dataRow["jmpReadyToPrint"] = true;
				if (!string.IsNullOrWhiteSpace(plantID))
				{
					dataRow["jmpPlantID"] = plantID;
					dataRow["jmpPlantDepartmentID"] = plantDept;
				}
				if (!string.IsNullOrWhiteSpace(warehouseID))
				{
					dataRow["jmpPartWareHouseLocationID"] = warehouseID;
					dataRow["jmpPartBinID"] = warehouseBinID;
				}
				if (!string.IsNullOrWhiteSpace(callID))
				{
					dataRow["jmpCallID"] = callID;
					flag = true;
				}
				dataRow["jmpTimeAndMaterial"] = flag;
				m1BindingSource.SaveData();
				if (!string.IsNullOrWhiteSpace(orderID) && !orderLineID.Equals(0))
				{
					int salesOrderJobLinkID = (int)database.NextIDs.GetNextIDForTable("SalesOrderJobLinks", new object[2] { orderID, orderLineID }, transaction);
					new SalesOrder().CreateSalesOrderJobLinks(database, transaction, orderID, orderLineID, salesOrderJobLinkID, 1, orderDeliveryID, jobID, closed: false, database.User.ID, DateTime.Now);
				}
			}
			if (flag2)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag2)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
		return string.Empty;
	}

	public bool IsJobAssemblyProductionComplete(M1Database database, SqlTransaction transaction, string jobID, int asmId)
	{
		jobID = jobID.Trim();
		if (jobID.Length == 0)
		{
			throw new M1Exception("Job ID cannot be empty.");
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select jmaProductionComplete from JobAssemblies where jmaJobID = @JobID and jmaJobAssemblyID = @AsmID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmId;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Job " + jobID + " does not exist.");
		}
		return Convert.ToBoolean(dataTable.Rows[0]["jmaProductionComplete"]);
	}

	public void ChangeProductionQty(M1Database database, SqlTransaction transaction, string jobID, int asmId, double newQty, double oldQty = 0.0, bool updateAsm = true)
	{
		bool flag = false;
		jobID = jobID.Trim();
		if (jobID.Length == 0)
		{
			throw new M1Exception("Job ID cannot be empty.");
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select jmpProductionComplete,jmpClosed from Jobs where jmpJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		if (database.GetDataTable(sqlCommand, fillSchema: false, out var _, transaction).Rows.Count == 0)
		{
			throw new M1Exception("Job " + jobID + " does not exist.");
		}
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			sqlCommand = database.NewSqlCommand("select jmaJobID,jmaJobAssemblyID,jmaLevel,jmaParentAssemblyID,jmaQuantityPerParent,jmaOrderQuantity,jmaInventoryQuantity,jmaScrapQuantity,jmaReworkQuantity,jmaProductionQuantity,jmaQuantityToMake,jmaQuantityToPull,jmaPullAllFromStock,jmaQuantityIssued,jmaIssuedComplete,jmaPartID,jmaPartRevisionID,jmaPartWarehouseLocationID,jmaPartBinID from JobAssemblies where jmaJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			SqlDataAdapter adapter2;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter2, transaction);
			sqlCommand = database.NewSqlCommand("select jmoJobID,jmoJobAssemblyID,jmoJobOperationID,jmoOperationQuantity,jmoQuantityPerAssembly,jmoEstimatedProductionHours,jmoProductionStandard,jmoStandardFactor,jmoWorkCenterID, jmoEstimatedUnitCost, jmoCalculatedUnitCost, jmoMinimumCharge, jmoSetupCharge, jmoQuantityBreak1, jmoUnitCost1, jmoQuantityBreak2, jmoUnitCost2, jmoQuantityBreak3, jmoUnitCost3, jmoQuantityBreak4, jmoUnitCost4, jmoQuantityBreak5, jmoUnitCost5, jmoQuantityBreak6, jmoUnitCost6, jmoQuantityBreak7, jmoUnitCost7, jmoQuantityBreak8, jmoUnitCost8, jmoQuantityBreak9, jmoUnitCost9 from JobOperations where jmoJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			SqlDataAdapter adapter3;
			DataTable dataTable2 = database.GetDataTable(sqlCommand, fillSchema: false, out adapter3, transaction);
			sqlCommand = database.NewSqlCommand("select jmmJobID,jmmJobAssemblyID,jmmJobMaterialID,jmmPartID,jmmPartRevisionID,jmmPartWarehouseLocationID,jmmPartBinID,jmmQuantityPerAssembly,jmmScrapPercent,jmmScrapQuantity,jmmEstimatedQuantity,jmmPullAllFromStock,jmmPullFromStockQuantity,jmmPurchaseToJobQuantity,jmmQuantityReceived,jmmQuantityAllocated,jmmReceivedComplete,jmmKitPart, jmmEstimatedUnitCost, jmmCalculatedUnitCost, jmmMinimumCharge, jmmQuantityBreak1, jmmUnitCost1, jmmQuantityBreak2, jmmUnitCost2, jmmQuantityBreak3, jmmUnitCost3, jmmQuantityBreak4, jmmUnitCost4, jmmQuantityBreak5, jmmUnitCost5, jmmQuantityBreak6, jmmUnitCost6, jmmQuantityBreak7, jmmUnitCost7, jmmQuantityBreak8, jmmUnitCost8, jmmQuantityBreak9, jmmUnitCost9 from JobMaterials where jmmJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			SqlDataAdapter adapter4;
			DataTable dataTable3 = database.GetDataTable(sqlCommand, fillSchema: false, out adapter4, transaction);
			sqlCommand = database.NewSqlCommand("Select jmtJobID,jmtJobAssemblyID,jmtJobMaterialID,jmtJobMaterialComponentID,jmtPartID,jmtPartRevisionID,jmtPartWarehouseLocationID,jmtPartBinID,jmtQuantityPerParent,jmtAdditionalQuantity,jmtMaterialQuantity,jmtQuantityReceived,jmtReceivedComplete,jmtClosed,jmtQuantityAllocated From JobMaterialComponents Where jmtJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			SqlDataAdapter adapter5;
			DataTable dataTable4 = database.GetDataTable(sqlCommand, fillSchema: false, out adapter5, transaction);
			DataRow[] array = dataTable.Select("jmaJobAssemblyID = " + asmId);
			if (array.Length != 0)
			{
				DataRow dataRow = array[0];
				if (updateAsm)
				{
					oldQty = Convert.ToDouble(dataRow["jmaProductionQuantity"]);
					if (asmId == 0)
					{
						dataRow["jmaProductionQuantity"] = newQty;
						sqlCommand = database.NewSqlCommand("UPDATE Jobs SET jmpOrderQuantity = @NewQty, jmpProductionQuantity = @NewQty + jmpInventoryQuantity + jmpScrapQuantity + jmpReworkQuantity WHERE jmpJobID = @JobID");
						sqlCommand.Parameters.Add(new SqlParameter("@NewQty", SqlDbType.Decimal)).Value = newQty;
						sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
						database.ExecuteCommand(sqlCommand, transaction);
					}
				}
				updateAsmProdQty(database, transaction, dataTable, dataTable3, dataTable2, dataTable4, dataRow, asmId, newQty, oldQty, updateAsm);
				database.UpdateData(dataTable, adapter2, transaction);
				database.UpdateData(dataTable3, adapter4, transaction);
				database.UpdateData(dataTable2, adapter3, transaction);
				database.UpdateData(dataTable4, adapter5, transaction);
				if (flag)
				{
					database.CommitTransaction(transaction);
				}
				return;
			}
			throw new M1Exception($"Assembly {asmId} does not exist.");
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void updateAsmProdQty(M1Database database, SqlTransaction transaction, DataTable assembliesTable, DataTable materialsTable, DataTable operationsTable, DataTable matComponentsTable, DataRow asmRow, int parentAsm, double parentLevelMakeQty, double oldParentLevelMakeQty, bool updateAsm)
	{
		Part part = new Part();
		double num;
		double num4;
		if (updateAsm)
		{
			num = Convert.ToDouble(asmRow["jmaQuantityToMake"]);
			double num2 = CalcAllocation(Convert.ToDouble(asmRow["jmaQuantityToPull"]), Convert.ToDouble(asmRow["jmaQuantityIssued"]), Convert.ToBoolean(asmRow["jmaIssuedComplete"]));
			asmRow["jmaOrderQuantity"] = parentLevelMakeQty * Convert.ToDouble(asmRow["jmaQuantityPerParent"]);
			if (Convert.ToDouble(asmRow["jmaOrderQuantity"]) == 0.0)
			{
				asmRow["jmaInventoryQuantity"] = 0;
				asmRow["jmaScrapQuantity"] = 0;
				asmRow["jmaReworkQuantity"] = 0;
			}
			asmRow["jmaProductionQuantity"] = Convert.ToDouble(asmRow["jmaOrderQuantity"]) + Convert.ToDouble(asmRow["jmaInventoryQuantity"]) + Convert.ToDouble(asmRow["jmaScrapQuantity"]) + Convert.ToDouble(asmRow["jmaReworkQuantity"]);
			RefreshJobAsmQuantities(asmRow);
			double num3 = CalcAllocation(Convert.ToDouble(asmRow["jmaQuantityToPull"]), Convert.ToDouble(asmRow["jmaQuantityIssued"]), Convert.ToBoolean(asmRow["jmaIssuedComplete"]));
			if (num2 != num3)
			{
				part.ChangeAllocations(database, transaction, asmRow.Field<string>("jmaPartID"), asmRow.Field<string>("jmaPartRevisionID"), asmRow.Field<string>("jmaPartWarehouseLocationID"), asmRow.Field<string>("jmaPartBinID"), num2, asmRow.Field<string>("jmaPartID"), asmRow.Field<string>("jmaPartRevisionID"), asmRow.Field<string>("jmaPartWarehouseLocationID"), asmRow.Field<string>("jmaPartBinID"), num3);
			}
			num4 = Convert.ToDouble(asmRow["jmaQuantityToMake"]);
		}
		else
		{
			num4 = parentLevelMakeQty;
			num = oldParentLevelMakeQty;
		}
		byte b = database.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals");
		DataRow[] array = operationsTable.Select("jmoJobAssemblyID = " + M1Util.ConvertToLinq(parentAsm));
		foreach (DataRow dataRow in array)
		{
			dataRow["jmoOperationQuantity"] = M1Math.Round(num4 * Convert.ToDouble(dataRow["jmoQuantityPerAssembly"]), b);
			dataRow["jmoEstimatedProductionHours"] = CalculateProductionHours(database, Convert.ToDouble(dataRow["jmoOperationQuantity"]), Convert.ToDouble(dataRow["jmoProductionStandard"]), dataRow.Field<string>("jmoStandardFactor"), dataRow.Field<string>("jmoWorkCenterID"), 0);
			dataRow["jmoCalculatedUnitCost"] = CalculateJobOperationCalculatedCost(Convert.ToDouble(dataRow["jmoOperationQuantity"]), Convert.ToDouble(dataRow["jmoEstimatedUnitCost"]), Convert.ToDouble(dataRow["jmoMinimumCharge"]), Convert.ToDouble(dataRow["jmoSetupCharge"]), Convert.ToDouble(dataRow["jmoQuantityBreak1"]), Convert.ToDouble(dataRow["jmoUnitCost1"]), Convert.ToDouble(dataRow["jmoQuantityBreak2"]), Convert.ToDouble(dataRow["jmoUnitCost2"]), Convert.ToDouble(dataRow["jmoQuantityBreak3"]), Convert.ToDouble(dataRow["jmoUnitCost3"]), Convert.ToDouble(dataRow["jmoQuantityBreak4"]), Convert.ToDouble(dataRow["jmoUnitCost4"]), Convert.ToDouble(dataRow["jmoQuantityBreak5"]), Convert.ToDouble(dataRow["jmoUnitCost5"]), Convert.ToDouble(dataRow["jmoQuantityBreak6"]), Convert.ToDouble(dataRow["jmoUnitCost6"]), Convert.ToDouble(dataRow["jmoQuantityBreak7"]), Convert.ToDouble(dataRow["jmoUnitCost7"]), Convert.ToDouble(dataRow["jmoQuantityBreak8"]), Convert.ToDouble(dataRow["jmoUnitCost8"]), Convert.ToDouble(dataRow["jmoQuantityBreak9"]), Convert.ToDouble(dataRow["jmoUnitCost9"]));
		}
		array = materialsTable.Select("jmmJobAssemblyID = " + M1Util.ConvertToLinq(parentAsm));
		foreach (DataRow dataRow2 in array)
		{
			double num2 = CalcAllocation(Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]), Convert.ToDouble(dataRow2["jmmQuantityReceived"]), Convert.ToBoolean(dataRow2["jmmReceivedComplete"]) || Convert.ToBoolean(dataRow2["jmmKitPart"]));
			if (Convert.ToDouble(dataRow2["jmmQuantityPerAssembly"]) == 0.0)
			{
				if (num == 0.0)
				{
					dataRow2["jmmEstimatedQuantity"] = dataRow2["jmmScrapQuantity"];
				}
				else
				{
					if (num4 == 0.0)
					{
						dataRow2["jmmQuantityPerAssembly"] = M1Math.Round((Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]) - Convert.ToDouble(dataRow2["jmmScrapQuantity"])) / num, 6);
					}
					dataRow2["jmmEstimatedQuantity"] = CalculateQtyWithScrap(database, Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]) * num4 / num, Convert.ToDouble(dataRow2["jmmScrapPercent"]), Convert.ToDouble(dataRow2["jmmScrapQuantity"]), b);
				}
			}
			else
			{
				dataRow2["jmmEstimatedQuantity"] = CalculateQtyWithScrap(database, num4 * Convert.ToDouble(dataRow2["jmmQuantityPerAssembly"]), Convert.ToDouble(dataRow2["jmmScrapPercent"]), Convert.ToDouble(dataRow2["jmmScrapQuantity"]), b);
			}
			if (Convert.ToBoolean(dataRow2["jmmPullAllFromStock"]))
			{
				dataRow2["jmmPullFromStockQuantity"] = Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]);
				dataRow2["jmmPurchaseToJobQuantity"] = 0;
				double num3 = CalcAllocation(Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]), Convert.ToDouble(dataRow2["jmmQuantityReceived"]), Convert.ToBoolean(dataRow2["jmmReceivedComplete"]) || Convert.ToBoolean(dataRow2["jmmKitPart"]));
				part.ChangeAllocations(database, transaction, dataRow2.Field<string>("jmmPartID"), dataRow2.Field<string>("jmmPartRevisionID"), dataRow2.Field<string>("jmmPartWarehouseLocationID"), dataRow2.Field<string>("jmmPartBinID"), num2, dataRow2.Field<string>("jmmPartID"), dataRow2.Field<string>("jmmPartRevisionID"), dataRow2.Field<string>("jmmPartWarehouseLocationID"), dataRow2.Field<string>("jmmPartBinID"), num3);
			}
			else
			{
				dataRow2["jmmPurchaseToJobQuantity"] = Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]);
				dataRow2["jmmPullFromStockQuantity"] = 0;
			}
			if (dataRow2.Field<decimal>("jmmEstimatedQuantity") - dataRow2.Field<decimal>("jmmQuantityReceived") <= 0m || dataRow2.Field<bool>("jmmReceivedComplete") || !Convert.ToBoolean(dataRow2["jmmPullAllFromStock"]))
			{
				dataRow2["jmmQuantityAllocated"] = 0;
			}
			else
			{
				dataRow2["jmmQuantityAllocated"] = dataRow2.Field<decimal>("jmmEstimatedQuantity") - dataRow2.Field<decimal>("jmmQuantityReceived");
			}
			dataRow2["jmmCalculatedUnitCost"] = CalculateJobMaterialCalculatedCost(Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]), Convert.ToDouble(dataRow2["jmmEstimatedUnitCost"]), Convert.ToDouble(dataRow2["jmmMinimumCharge"]), Convert.ToDouble(dataRow2["jmmQuantityBreak1"]), Convert.ToDouble(dataRow2["jmmUnitCost1"]), Convert.ToDouble(dataRow2["jmmQuantityBreak2"]), Convert.ToDouble(dataRow2["jmmUnitCost2"]), Convert.ToDouble(dataRow2["jmmQuantityBreak3"]), Convert.ToDouble(dataRow2["jmmUnitCost3"]), Convert.ToDouble(dataRow2["jmmQuantityBreak4"]), Convert.ToDouble(dataRow2["jmmUnitCost4"]), Convert.ToDouble(dataRow2["jmmQuantityBreak5"]), Convert.ToDouble(dataRow2["jmmUnitCost5"]), Convert.ToDouble(dataRow2["jmmQuantityBreak6"]), Convert.ToDouble(dataRow2["jmmUnitCost6"]), Convert.ToDouble(dataRow2["jmmQuantityBreak7"]), Convert.ToDouble(dataRow2["jmmUnitCost7"]), Convert.ToDouble(dataRow2["jmmQuantityBreak8"]), Convert.ToDouble(dataRow2["jmmUnitCost8"]), Convert.ToDouble(dataRow2["jmmQuantityBreak9"]), Convert.ToDouble(dataRow2["jmmUnitCost9"]));
			DataRow[] array2 = matComponentsTable.Select("jmtJobAssemblyID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow2["jmmJobAssemblyID"])) + " And jmtJobMaterialID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow2["jmmJobMaterialID"])));
			foreach (DataRow dataRow3 in array2)
			{
				dataRow3["jmtMaterialQuantity"] = GetComponentQty(database, Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]), dataRow3);
				if (dataRow3.Field<decimal>("jmtMaterialQuantity") - dataRow3.Field<decimal>("jmtQuantityReceived") <= 0m || dataRow3.Field<bool>("jmtReceivedComplete"))
				{
					dataRow3["jmtQuantityAllocated"] = 0;
				}
				else
				{
					dataRow3["jmtQuantityAllocated"] = dataRow3.Field<decimal>("jmtMaterialQuantity") - dataRow3.Field<decimal>("jmtQuantityReceived");
				}
				updateComponentRecord(database, transaction, dataRow3, Convert.ToBoolean(dataRow2["jmmKitPart"]));
			}
		}
		array = assembliesTable.Select("jmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAsm) + " And jmaJobAssemblyID <> 0");
		foreach (DataRow dataRow4 in array)
		{
			updateAsmProdQty(database, transaction, assembliesTable, materialsTable, operationsTable, matComponentsTable, dataRow4, Convert.ToInt32(dataRow4["jmaJobAssemblyID"]), num4, num, updateAsm: true);
		}
	}

	public double CalculateJobMaterialCalculatedCost(double estimatedQty, double estimatedUnitCost, double minimumCharge, double qtyBreak1, double unitCost1, double qtyBreak2, double unitCost2, double qtyBreak3, double unitCost3, double qtyBreak4, double unitCost4, double qtyBreak5, double unitCost5, double qtyBreak6, double unitCost6, double qtyBreak7, double unitCost7, double qtyBreak8, double unitCost8, double qtyBreak9, double unitCost9)
	{
		double num = estimatedUnitCost;
		double[] array = new double[9] { qtyBreak1, qtyBreak2, qtyBreak3, qtyBreak4, qtyBreak5, qtyBreak6, qtyBreak7, qtyBreak8, qtyBreak9 };
		double[] array2 = new double[9] { unitCost1, unitCost2, unitCost3, unitCost4, unitCost5, unitCost6, unitCost7, unitCost8, unitCost9 };
		for (int num2 = 8; num2 >= 0; num2--)
		{
			if (array[num2] != 0.0 && estimatedQty >= array[num2])
			{
				num = array2[num2];
				break;
			}
		}
		num = M1Math.Round(estimatedQty * num, 5);
		if (minimumCharge != 0.0 && num < minimumCharge)
		{
			num = minimumCharge;
		}
		if (estimatedQty != 0.0)
		{
			num = M1Math.Round(num / estimatedQty, 5);
		}
		return num;
	}

	public double CalculateQtyWithScrap(M1Database database, double quantity, double scrapPercent, double scrapQty, short roundTo = 5)
	{
		double num = scrapPercent / 100.0 * quantity;
		double num2 = quantity + num;
		if (database.Props("ProductionProperties").Field<bool>("xapIMScrapRoundUp") && (scrapPercent != 0.0 || scrapQty != 0.0))
		{
			return Math.Ceiling(M1Math.Round(num2 + scrapQty, roundTo));
		}
		return M1Math.Round(num2 + scrapQty, roundTo);
	}

	public double CalculateJobOperationCalculatedCost(double quantity, double estimatedUnitCost, double minimumCharge, double setupCharge, double qtyBreak1, double unitCost1, double qtyBreak2, double unitCost2, double qtyBreak3, double unitCost3, double qtyBreak4, double unitCost4, double qtyBreak5, double unitCost5, double qtyBreak6, double unitCost6, double qtyBreak7, double unitCost7, double qtyBreak8, double unitCost8, double qtyBreak9, double unitCost9, bool excludeSetupCharge = false)
	{
		double num = estimatedUnitCost;
		double[] array = new double[9] { qtyBreak1, qtyBreak2, qtyBreak3, qtyBreak4, qtyBreak5, qtyBreak6, qtyBreak7, qtyBreak8, qtyBreak9 };
		double[] array2 = new double[9] { unitCost1, unitCost2, unitCost3, unitCost4, unitCost5, unitCost6, unitCost7, unitCost8, unitCost9 };
		for (int num2 = 8; num2 >= 0; num2--)
		{
			if (array[num2] != 0.0 && quantity >= array[num2])
			{
				num = array2[num2];
				break;
			}
		}
		num = M1Math.Round(quantity * num, 5);
		if (minimumCharge != 0.0 && num < minimumCharge)
		{
			num = minimumCharge;
		}
		if (!excludeSetupCharge)
		{
			num += setupCharge;
		}
		if (quantity != 0.0)
		{
			num = M1Math.Round(num / quantity, 5);
		}
		return num;
	}

	public void UpdateOperationCompleteFlags(M1Database database, string jobID, int asmID, int seq, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompleted = jmoProductionComplete FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @Seq and sxtType = 1 And sxeMinutes>0 And sxeScheduleTypeBucketID = 3");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompleted = Case When (jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then 1 Else 0 End FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @Seq and sxtType = 1 And sxeMinutes>0 And (sxeScheduleTypeBucketID = 1 Or sxeScheduleTypeBucketID = 2)");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void UpdateOperationCompleteFlags(M1Database database, string jobID, int asmID, SqlTransaction transaction)
	{
		IList<int> list = new List<int>();
		list.Add(asmID);
		SqlCommand sqlCommand = database.NewSqlCommand("select jmaJobAssemblyID from JobAssemblies where jmaJobID = @JobID and jmaParentAssemblyID=@parentAssemblyID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@parentAssemblyID", SqlDbType.Int)).Value = asmID;
		foreach (DataRow row in database.GetDataTable(sqlCommand, transaction).Rows)
		{
			list.Add(row.Field<int>("jmaJobAssemblyID"));
		}
		foreach (int item in list)
		{
			sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompleted = jmoProductionComplete FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and sxtType = 1 And sxeMinutes>0 And sxeScheduleTypeBucketID = 3");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = item;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompleted = Case When (jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then 1 Else 0 End FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and sxtType = 1 And sxeMinutes>0 And (sxeScheduleTypeBucketID = 1 Or sxeScheduleTypeBucketID = 2)");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = item;
			database.ExecuteCommand(sqlCommand, transaction);
		}
	}

	public void UpdateOperationCompleteFlags(M1Database database, string jobID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompleted = jmpProductionComplete FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' WHERE jmpJobID = @JobID and sxtType = 1 And sxeMinutes>0");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void RefreshScheduleActuals(M1Database database, string jobID, int asmID, int seq, SqlTransaction transaction)
	{
		SqlCommand sqlCommand;
		if (database.Props("JM").Field<byte>("xapJMLoadReliefMethod") == 1)
		{
			double num = 0.0;
			sqlCommand = database.NewSqlCommand("select jmoQuantityComplete,jmoProductionStandard,jmoStandardFactor,jmoWorkCenterID from JobOperations where jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @Seq");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
			sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				num = CalculateProductionHours(database, Convert.ToDouble(row.Field<decimal>("jmoQuantityComplete")), Convert.ToDouble(row.Field<decimal>("jmoProductionStandard")), row.Field<string>("jmoStandardFactor"), row.Field<string>("jmoWorkCenterID"), 2);
				if (num > 99999.99)
				{
					num = 99999.99;
				}
			}
			sqlCommand = database.NewSqlCommand("UPDATE JobOperations SET jmoCompletedSetupHours = ROUND(jmoSetupHours * (jmoSetupPercentComplete / 100.0),2),jmoCompletedProductionHours = @Load WHERE jmoJobID = @JobID AND jmoJobAssemblyID = @AsmID AND jmoJobOperationID = @Seq");
			sqlCommand.Parameters.Add(new SqlParameter("@Load", SqlDbType.Decimal)).Value = num;
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
			sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
			database.ExecuteCommand(sqlCommand, transaction);
		}
		else
		{
			sqlCommand = database.NewSqlCommand("UPDATE JobOperations SET jmoCompletedSetupHours = ROUND(jmoSetupHours * (jmoSetupPercentComplete / 100.0),2),jmoCompletedProductionHours = jmoActualProductionHours WHERE jmoJobID = @JobID AND jmoJobAssemblyID = @AsmID AND jmoJobOperationID = @Seq");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
			sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
			database.ExecuteCommand(sqlCommand, transaction);
		}
		sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompletedMinutes = jmoCompletedSetupHours * 60.0,sxePercentComplete = jmoSetupPercentComplete,sxeCompleted = jmoSetupComplete FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @Seq and sxtType = 1 And sxeScheduleTypeBucketID = 2");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompletedMinutes = jmoCompletedProductionHours  * 60.0,sxePercentComplete = Case When jmoEstimatedProductionHours = 0 Then 0 Else Round((jmoCompletedProductionHours / jmoEstimatedProductionHours) * 100.0,0) End, sxeCompleted = jmoProductionComplete FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @Seq and sxtType = 1 And sxeScheduleTypeBucketID = 3");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("UPDATE ScheduleTaskBuckets SET sxeCompletedMinutes = Case When (jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then sxeMinutes Else 0 End,sxePercentComplete = Case When (jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then 100 Else 0 End,sxeCompleted = Case When (jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then 1 Else 0 End FROM ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID And sxtSourceTable = 'Jobs' Inner Join JobOperations ON jmpJobID = jmoJobID AND jmoJobAssemblyID = sxeScheduleBranchID AND jmoJobOperationID = sxeScheduleTaskID WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @Seq and sxtType = 1 And sxeScheduleTypeBucketID = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		sqlCommand.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int)).Value = seq;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public double CalculateProductionHours(M1Database database, double operationQty, double productionStandard, string standardFactor, string workCenter, short roundTo = 0)
	{
		if (productionStandard == 0.0 || operationQty == 0.0)
		{
			return 0.0;
		}
		if (roundTo == 0)
		{
			roundTo = 2;
		}
		return standardFactor.ToUpper() switch
		{
			"PH" => M1Math.Round(operationQty / productionStandard, roundTo), 
			"PM" => M1Math.Round(operationQty / productionStandard / 60.0, roundTo), 
			"TD" => M1Math.Round(productionStandard * getWorkCenterCapacity(database, workCenter), roundTo), 
			"TH" => M1Math.Round(productionStandard, roundTo), 
			"TM" => M1Math.Round(productionStandard / 60.0, roundTo), 
			"HP" => M1Math.Round(productionStandard * operationQty, roundTo), 
			"MP" => M1Math.Round(productionStandard * operationQty / 60.0, roundTo), 
			"SP" => M1Math.Round(productionStandard * operationQty / 3600.0, roundTo), 
			"HC" => M1Math.Round(productionStandard * operationQty / 100.0, roundTo), 
			"MC" => M1Math.Round(productionStandard * operationQty / 60.0 / 100.0, roundTo), 
			"HM" => M1Math.Round(productionStandard * operationQty / 1000.0, roundTo), 
			"MM" => M1Math.Round(productionStandard * operationQty / 60.0 / 1000.0, roundTo), 
			_ => 0.0, 
		};
	}

	private double getWorkCenterCapacity(M1Database database, string workCenter)
	{
		workCenter = workCenter.Trim();
		if (workCenter.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select xawHoursMon from WorkCenters where xawWorkCenterID = @WorkCenter");
			sqlCommand.Parameters.Add(new SqlParameter("@WorkCenter", SqlDbType.NVarChar)).Value = workCenter;
			object obj = database.ExecuteScalar(sqlCommand);
			if (obj != DBNull.Value && obj != null)
			{
				return Convert.ToDouble(obj);
			}
		}
		return 8.0;
	}

	public void RefreshMaterialPriceBreaks(M1Database database, SqlTransaction transaction, DataRow materialRow)
	{
		if (string.IsNullOrWhiteSpace(materialRow.Field<string>("jmmPartID")))
		{
			return;
		}
		for (int i = 1; i <= 9; i++)
		{
			materialRow["jmmQuantityBreak" + i] = 0;
			materialRow["jmmUnitCost" + i] = 0;
		}
		PriceCalculation purchasePrice = new Part().GetPurchasePrice(database, materialRow.Field<string>("jmmPartID"), materialRow.Field<string>("jmmPartRevisionID"), materialRow.Field<string>("jmmSupplierOrganizationID"), materialRow.Field<string>("jmmPurchaseLocationID"), 0m, "Material", "", DateTime.Today, 0m, transaction);
		if (purchasePrice.PartPrice == null)
		{
			return;
		}
		int num = 0;
		decimal num2 = purchasePrice.ConversionFactor;
		if (purchasePrice.PartPrice.InventoryPrice)
		{
			num2 = 1m;
		}
		byte decimals = database.Props("DS").Field<byte>("xadInventoryQuantityDecimals");
		foreach (PriceLineData line in purchasePrice.PartPrice.Lines)
		{
			num++;
			if (num <= 9)
			{
				materialRow["jmmQuantityBreak" + num] = M1Math.Round(line.Quantity, decimals);
				materialRow["jmmUnitCost" + num] = M1Math.Round(line.UnitPrice * num2, 5);
				materialRow["jmmLeadTime" + num] = line.LeadTime;
			}
		}
	}

	public void RefreshJobAsmQuantities(DataRow asmRow)
	{
		if (Convert.ToBoolean(asmRow["jmaPullAllFromStock"]))
		{
			if (Convert.ToDouble(asmRow["jmaInventoryQuantity"]) != 0.0)
			{
				asmRow["jmaInventoryQuantity"] = 0;
			}
			if (Convert.ToDouble(asmRow["jmaScrapQuantity"]) != 0.0)
			{
				asmRow["jmaScrapQuantity"] = 0;
			}
			if (Convert.ToDouble(asmRow["jmaReworkQuantity"]) != 0.0)
			{
				asmRow["jmaReworkQuantity"] = 0;
			}
			asmRow["jmaQuantityToMake"] = Convert.ToDouble(asmRow["jmaProductionQuantity"]) - Convert.ToDouble(asmRow["jmaOrderQuantity"]);
			asmRow["jmaQuantityToPull"] = asmRow["jmaOrderQuantity"];
		}
		else if (Convert.ToDouble(asmRow["jmaProductionQuantity"]) < 0.0)
		{
			if (Convert.ToDouble(asmRow["jmaProductionQuantity"]) - Convert.ToDouble(asmRow["jmaQuantityToPull"]) > 0.0)
			{
				asmRow["jmaQuantityToMake"] = 0;
				asmRow["jmaQuantityToPull"] = asmRow["jmaProductionQuantity"];
			}
			else
			{
				asmRow["jmaQuantityToMake"] = Convert.ToDouble(asmRow["jmaProductionQuantity"]) - Convert.ToDouble(asmRow["jmaQuantityToPull"]);
			}
		}
		else if (Convert.ToDouble(asmRow["jmaProductionQuantity"]) - Convert.ToDouble(asmRow["jmaQuantityToPull"]) < 0.0)
		{
			asmRow["jmaQuantityToMake"] = 0;
			asmRow["jmaQuantityToPull"] = asmRow["jmaProductionQuantity"];
		}
		else
		{
			asmRow["jmaQuantityToMake"] = Convert.ToDouble(asmRow["jmaProductionQuantity"]) - Convert.ToDouble(asmRow["jmaQuantityToPull"]);
		}
	}

	public double GetComponentQty(M1Database database, double parentQty, DataRow componentRow)
	{
		if (parentQty == 0.0)
		{
			return 0.0;
		}
		return M1Math.Round(parentQty * Convert.ToDouble(componentRow["jmtQuantityPerParent"]), database.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals")) + Convert.ToDouble(componentRow["jmtAdditionalQuantity"]);
	}

	private void updateComponentRecord(M1Database database, SqlTransaction transaction, DataRow row, bool kitPart)
	{
		if (row["jmtMaterialQuantity"] != row["jmtMaterialQuantity", DataRowVersion.Original])
		{
			double num = ((!Convert.ToBoolean(row["jmtReceivedComplete", DataRowVersion.Original]) && !Convert.ToBoolean(row["jmtClosed", DataRowVersion.Original]) && kitPart) ? CalcAllocation(Convert.ToDouble(row["jmtMaterialQuantity", DataRowVersion.Original]), Convert.ToDouble(row["jmtQuantityReceived", DataRowVersion.Original]), complete: false) : 0.0);
			double num2 = ((!Convert.ToBoolean(row["jmtReceivedComplete"]) && !Convert.ToBoolean(row["jmtClosed"]) && kitPart) ? CalcAllocation(Convert.ToDouble(row["jmtMaterialQuantity"]), Convert.ToDouble(row["jmtQuantityReceived"]), complete: false) : 0.0);
			if (num != num2)
			{
				new Part().ChangeAllocations(database, transaction, row.Field<string>("jmtPartID", DataRowVersion.Original), row.Field<string>("jmtPartRevisionID", DataRowVersion.Original), row.Field<string>("jmtPartWarehouseLocationID", DataRowVersion.Original), row.Field<string>("jmtPartBinID", DataRowVersion.Original), num, row.Field<string>("jmtPartID"), row.Field<string>("jmtPartRevisionID"), row.Field<string>("jmtPartWarehouseLocationID"), row.Field<string>("jmtPartBinID"), num2);
			}
		}
	}

	public double CalcAllocation(double estimatedQty, double receivedQty, bool complete)
	{
		if (complete)
		{
			return 0.0;
		}
		if ((estimatedQty >= 0.0 && receivedQty >= estimatedQty) || (estimatedQty < 0.0 && receivedQty <= estimatedQty))
		{
			return 0.0;
		}
		return estimatedQty - receivedQty;
	}

	public void CompleteJob(M1Database database, SqlTransaction transaction, string jobID, bool complete, bool updateJobs = true, double qtyComplete = 0.0, int asmID = 0, DateTime? completionDate = null, bool prodCompleteChanged = true, bool qtyCompleteChanged = true, bool completeDateChanged = true, bool resetJobOperations = false)
	{
		jobID = jobID.Trim();
		if (jobID.Length == 0)
		{
			throw new M1Exception("Job ID is required.");
		}
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand;
			if (updateJobs && asmID == 0 && (prodCompleteChanged || qtyCompleteChanged || completeDateChanged))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("UPDATE Jobs SET ");
				if (prodCompleteChanged)
				{
					stringBuilder.Append(", jmpProductionComplete = @ProdComplete");
				}
				if (qtyCompleteChanged)
				{
					stringBuilder.Append(", jmpQuantityCompleted = @QtyComplete");
				}
				if (completeDateChanged)
				{
					stringBuilder.Append(", jmpCompletedDate = ISNULL(jmpCompletedDate,@CompletedDate)");
				}
				stringBuilder.AppendLine(" WHERE jmpJobID = @JobID");
				stringBuilder.Append("UPDATE JobAssemblies SET ");
				if (prodCompleteChanged)
				{
					stringBuilder.Append(", jmaProductionComplete = @ProdComplete");
					stringBuilder.Append(", jmaReceivedComplete = @ProdComplete");
				}
				if (qtyCompleteChanged)
				{
					stringBuilder.Append(", jmaQuantityCompleted = @QtyComplete");
				}
				if (completeDateChanged)
				{
					stringBuilder.Append(", jmaCompletedDate = ISNULL(jmaCompletedDate,@CompletedDate)");
				}
				stringBuilder.Append(" WHERE jmaJobID = @JobID And jmaJobAssemblyID = 0");
				stringBuilder.Replace("SET ,", "SET ");
				sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
				if (prodCompleteChanged)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@ProdComplete", SqlDbType.Decimal)).Value = complete;
				}
				if (qtyCompleteChanged)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@QtyComplete", SqlDbType.Decimal)).Value = qtyComplete;
				}
				if (completeDateChanged)
				{
					if (completionDate.HasValue)
					{
						sqlCommand.Parameters.Add(new SqlParameter("@CompletedDate", SqlDbType.DateTime)).Value = completionDate;
					}
					else
					{
						sqlCommand.Parameters.Add(new SqlParameter("@CompletedDate", SqlDbType.DateTime)).Value = DBNull.Value;
					}
				}
				sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
				database.ExecuteCommand(sqlCommand, transaction);
			}
			sqlCommand = database.NewSqlCommand("select jmaJobID,jmaJobAssemblyID,jmaParentAssemblyID,jmaQuantityPerParent,jmaProductionComplete,jmaCompletedDate,jmaQuantityCompleted,jmaReceivedComplete  from JobAssemblies where jmaJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow[] array = dataTable.Select("jmaJobAssemblyID = " + M1Util.ConvertToLinq(asmID));
				foreach (DataRow asmRow in array)
				{
					UpdateAsmCompletedQty(database, transaction, dataTable, asmRow, asmID, qtyComplete, complete, completionDate, prodCompleteChanged, qtyCompleteChanged, completeDateChanged, resetJobOperations);
				}
				database.UpdateData(dataTable, adapter, transaction);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void UpdateAsmCompletedQty(M1Database database, SqlTransaction transaction, DataTable assembliesTable, DataRow asmRow, int parentAsm, double parentLevelCompletedQty, bool complete, DateTime? completedDate, bool prodCompleteChanged, bool qtyCompleteChanged, bool completeDateChanged, bool resetJobOperations)
	{
		if (prodCompleteChanged || qtyCompleteChanged || completeDateChanged)
		{
			if (prodCompleteChanged)
			{
				asmRow["jmaProductionComplete"] = complete;
				asmRow["jmaReceivedComplete"] = complete;
			}
			if (completeDateChanged)
			{
				if (completedDate.HasValue)
				{
					asmRow["jmaCompletedDate"] = completedDate;
				}
				else
				{
					asmRow["jmaCompletedDate"] = DBNull.Value;
				}
			}
			if (qtyCompleteChanged)
			{
				asmRow["jmaQuantityCompleted"] = parentLevelCompletedQty;
			}
			string text = (complete ? "1" : "0");
			string text2 = (prodCompleteChanged ? ("jmoProductionComplete = " + text + " ") : "jmoProductionComplete = jmoProductionComplete");
			string text3 = (qtyCompleteChanged ? $"jmoQuantityComplete = IIF(jmoActualProductionHours <= 0 and jmoActualSetupHours <= 0 and jmoSetupPercentComplete <= 0 and jmoPurchaseOrderID = '', {parentLevelCompletedQty}, jmoQuantityComplete)," : string.Empty);
			string queryString = (resetJobOperations ? ("UPDATE JobOperations SET jmoSetupComplete = 0, " + text3 + " " + text2 + " WHERE jmoJobID = @JobID AND jmoJobAssemblyID = @JobAssemblyID") : ("UPDATE JobOperations SET jmoSetupComplete = 1, " + text3 + " " + text2 + " WHERE jmoJobID = @JobID AND jmoJobAssemblyID = @JobAssemblyID"));
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = asmRow.Field<string>("jmaJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobAssemblyID", SqlDbType.Int)).Value = Convert.ToInt32(asmRow["jmaJobAssemblyID"]);
			database.ExecuteCommand(sqlCommand, transaction);
		}
		double num = Convert.ToDouble(asmRow["jmaQuantityCompleted"]);
		DataRow[] array = assembliesTable.Select("jmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAsm) + " and jmaJobAssemblyID <> 0");
		foreach (DataRow dataRow in array)
		{
			UpdateAsmCompletedQty(database, transaction, assembliesTable, dataRow, Convert.ToInt32(dataRow["jmaJobAssemblyID"]), num * Convert.ToDouble(dataRow["jmaQuantityPerParent"]), complete, completedDate, prodCompleteChanged, qtyCompleteChanged, completeDateChanged, resetJobOperations);
		}
	}

	public double GetJobPercentageComplete(M1Database database, string jobID)
	{
		jobID = jobID.Trim();
		if (jobID.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT ISNULL(SUM(jmoSetupHours) + SUM(jmoEstimatedProductionHours),0) as EstimatedHours, ISNULL(SUM(CASE WHEN jmoSetupComplete <> 0 THEN jmoSetupHours ELSE jmoCompletedSetupHours END)+ SUM(CASE WHEN jmoProductionComplete <> 0 THEN jmoEstimatedProductionHours ELSE jmoCompletedProductionHours END),0) AS CompletedHours FROM JobOperations WHERE jmoJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow dataRow = dataTable.Rows[0];
				if (dataRow["EstimatedHours"] != DBNull.Value && dataRow["CompletedHours"] != DBNull.Value && Convert.ToDouble(dataRow["EstimatedHours"]) != 0.0)
				{
					return M1Math.Round(Convert.ToDouble(dataRow["CompletedHours"]) / Convert.ToDouble(dataRow["EstimatedHours"]) * 100.0, 0);
				}
			}
		}
		return 0.0;
	}

	public JobAssemblyTimeInfo DoesJobAssemblyHaveTimecardsPurchaseOrdersOrRfqs(M1Database database, string jobID, int jobAssemblyID)
	{
		JobAssemblyTimeInfo jobAssemblyTimeInfo = new JobAssemblyTimeInfo();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SET NOCOUNT ON\r");
		stringBuilder.Append("DECLARE @nSelectedAsm int, @cJob varchar(20)\r");
		stringBuilder.Append("SET @cJob = " + jobID.ToSql() + "\r");
		stringBuilder.Append("SET @nSelectedAsm = " + jobAssemblyID.ToSql() + "\r");
		stringBuilder.Append("SELECT jmaJobID,jmaJobAssemblyID INTO #TempJobQuery FROM JobAssemblies WHERE 0=1\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) VALUES (@cJob, @nSelectedAsm)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("INSERT INTO #TempJobQuery (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery)\r");
		stringBuilder.Append("SET NOCOUNT OFF\r");
		stringBuilder.Append("SELECT SUM(TimeCount) As TimeCount, SUM(POCount) As POCount, Sum(RFQCount) As RFQCount From (SELECT IsNull((select count(*) from TimeCardLines Where lmlJobID=#TempJobQuery.jmaJobID and lmlJobAssemblyID=#TempJobQuery.jmaJobAssemblyID),0) as TimeCount,IsNull((select count(*) from PurchaseOrderLines Where pmlJobID=#TempJobQuery.jmaJobID and pmlJobAssemblyID=#TempJobQuery.jmaJobAssemblyID),0) as POCount,IsNull((select count(*) from RFQLines Where rqlJobID=#TempJobQuery.jmaJobID and rqlJobAssemblyID=#TempJobQuery.jmaJobAssemblyID),0) as RFQCount\r");
		stringBuilder.Append("FROM #TempJobQuery INNER JOIN JobAssemblies ON JobAssemblies.jmaJobID = #TempJobQuery.jmaJobID And JobAssemblies.jmaJobAssemblyID = #TempJobQuery.jmaJobAssemblyID inner join Jobs on JobAssemblies.jmaJobID = jmpJobID) as test\r");
		stringBuilder.Append("DROP TABLE #TempJobQuery\r");
		DataTable dataTable = database.GetDataTable(stringBuilder.ToString());
		if (dataTable.Rows.Count != 0)
		{
			if (!dataTable.Rows[0].IsNull("TimeCount"))
			{
				jobAssemblyTimeInfo.Timecards = dataTable.Rows[0].Field<int>("TimeCount");
			}
			if (!dataTable.Rows[0].IsNull("POCount"))
			{
				jobAssemblyTimeInfo.PurchaseOrders = dataTable.Rows[0].Field<int>("POCount");
			}
			if (!dataTable.Rows[0].IsNull("RFQCount"))
			{
				jobAssemblyTimeInfo.RFQs = dataTable.Rows[0].Field<int>("RFQCount");
			}
		}
		jobAssemblyTimeInfo.HasTime = jobAssemblyTimeInfo.Timecards != 0 || jobAssemblyTimeInfo.PurchaseOrders != 0 || jobAssemblyTimeInfo.RFQs != 0;
		return jobAssemblyTimeInfo;
	}

	public bool DoesJobAssemblyHaveQuantityMovement(M1Database database, string jobID, int jobAssemblyID)
	{
		using (SqlTransaction transaction = database.BeginTransaction())
		{
			string assembliesList = GetAssembliesList(database, transaction, jobID, jobAssemblyID);
			SqlCommand sqlCommand = database.NewSqlCommand("select jmmQuantityReceived, jmmScrapQuantityReceived, jmmQuantityToInspect, jmmQuantityToReturn  from JobMaterials where jmmJobID = " + jobID.ToSql() + " And jmmJobAssemblyID in (" + assembliesList + ")");
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				foreach (DataRow row4 in dataTable.Rows)
				{
					if (row4.Field<decimal>("jmmQuantityReceived") != 0m || row4.Field<decimal>("jmmScrapQuantityReceived") != 0m || row4.Field<decimal>("jmmQuantityToInspect") != 0m || row4.Field<decimal>("jmmQuantityToReturn") != 0m)
					{
						return true;
					}
				}
			}
			sqlCommand = database.NewSqlCommand("select jmoQuantityComplete, jmoQuantityToInspect, jmoQuantityToReturn, jmoScrapQuantityReceived  from JobOperations where jmoJobID = " + jobID.ToSql() + " And jmoJobAssemblyID in (" + assembliesList + ")");
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			if (dataTable2.Rows.Count != 0)
			{
				foreach (DataRow row5 in dataTable2.Rows)
				{
					if (row5.Field<decimal>("jmoQuantityComplete") != 0m || row5.Field<decimal>("jmoQuantityToInspect") != 0m || row5.Field<decimal>("jmoQuantityToReturn") != 0m || row5.Field<decimal>("jmoScrapQuantityReceived") != 0m)
					{
						return true;
					}
				}
			}
			sqlCommand = database.NewSqlCommand("select jmaQuantityReceivedToInventory, jmaQuantityIssued, jmaScrapQuantityCompleted, jmaQuantityToInspect, jmaQuantityToReturn  from JobAssemblies where jmaJobID = " + jobID.ToSql() + " And jmaJobAssemblyID in (" + assembliesList + ")");
			DataTable dataTable3 = database.GetDataTable(sqlCommand);
			if (dataTable3.Rows.Count != 0)
			{
				foreach (DataRow row6 in dataTable3.Rows)
				{
					if (row6.Field<decimal>("jmaQuantityReceivedToInventory") != 0m || row6.Field<decimal>("jmaQuantityIssued") != 0m || row6.Field<decimal>("jmaScrapQuantityCompleted") != 0m || row6.Field<decimal>("jmaQuantityToInspect") != 0m || row6.Field<decimal>("jmaQuantityToReturn") != 0m)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool DoesJobExist(M1Database database, SqlTransaction transaction, string jobID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select jmpJobID From Jobs Where jmpJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return (string)database.ExecuteScalar(sqlCommand, transaction) != null;
	}

	public void DeleteJobAssembly(M1Database database, SqlTransaction transaction, string jobID, int asmID, bool deleteAsmInJob)
	{
		deleteJobAssembly(database, transaction, jobID, asmID, deleteAsmInJob);
	}

	public void DeleteJobAssembly(M1Database database, SqlTransaction transaction, string jobID, int asmID)
	{
		deleteJobAssembly(database, transaction, jobID, asmID, deleteAsmInJob: false);
	}

	private void deleteJobAssembly(M1Database database, SqlTransaction transaction, string jobID, int asmID, bool deleteAsmInJob)
	{
		if (string.IsNullOrWhiteSpace(jobID))
		{
			throw new M1Exception("Job ID is required.");
		}
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select jmpProductionComplete From Jobs Where jmpJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			if (obj != null && obj != DBNull.Value && !Convert.ToBoolean(obj))
			{
				ChangeProductionQty(database, transaction, jobID, asmID, 0.0);
			}
			sqlCommand = database.NewSqlCommand("select jmaJobAssemblyID,jmaParentAssemblyID,jmaIssuedComplete,jmaQuantityToPull,jmaQuantityIssued,jmaPartID,jmaPartRevisionID,jmaPartWarehouseLocationID,jmaPartBinID from JobAssemblies where jmaJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
			if (dataTable.Rows.Count != 0)
			{
				deleteNextAsmLevel(database, transaction, dataTable, jobID, asmID);
				deleteAsm(database, transaction, jobID, asmID, deleteAsmInJob);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void deleteAsm(M1Database database, SqlTransaction transaction, string jobID, int asmID, bool deleteAsm)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("DELETE FormInputValues FROM FormInputValues INNER JOIN JobAssemblies On xaiSourceUniqueID = jmaUniqueID WHERE jmaJobID = @JobID And jmaJobAssemblyID = @AsmID And xaiSourceTable = 'JOBASSEMBLIES'");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("DELETE FROM JobMaterials WHERE jmmJobID = @JobID AND jmmJobAssemblyID = @AsmID\rDELETE FROM JobMaterialComponents WHERE jmtJobID = @JobID AND jmtJobAssemblyID = @AsmID\rDELETE FROM JobOperations WHERE jmoJobID = @JobID AND jmoJobAssemblyID = @AsmID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		database.ExecuteCommand(sqlCommand, transaction);
		if (deleteAsm)
		{
			sqlCommand = database.NewSqlCommand("DELETE FROM JobAssemblies WHERE jmaJobID = @JobID AND jmaJobAssemblyID = @AsmID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
			database.ExecuteCommand(sqlCommand, transaction);
		}
		new SerialNumber().DeleteSerialTransactions(database, transaction, "sntJobID = " + M1Util.ConvertToSql(jobID) + " And sntJobAssemblyID = " + M1Util.ConvertToSql(asmID) + " And sntTransactionType In (1, 41, 47) ");
		new LotNumber().DeleteLotTransactions(database, transaction, "abtJobID = " + M1Util.ConvertToSql(jobID) + " And abtJobAssemblyID = " + M1Util.ConvertToSql(asmID) + " And abtTransactionType In (1, 41, 47) ");
	}

	private void deleteNextAsmLevel(M1Database database, SqlTransaction transaction, DataTable assembliesTable, string jobID, int parentAsm)
	{
		DataRow[] array = assembliesTable.Select("jmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAsm) + " and jmaJobAssemblyID <> 0");
		foreach (DataRow dataRow in array)
		{
			deleteNextAsmLevel(database, transaction, assembliesTable, jobID, Convert.ToInt32(dataRow["jmaJobAssemblyID"]));
			deleteAsm(database, transaction, jobID, Convert.ToInt32(dataRow["jmaJobAssemblyID"]), deleteAsm: true);
		}
	}

	public int CreateJobSequenceFromPOLine(M1BindingSource m1BindingSource, SqlTransaction transaction, DataRow currentRow)
	{
		try
		{
			M1Database currentDatabase = m1BindingSource.CurrentDatabase;
			if (currentRow == null)
			{
				return 0;
			}
			SqlCommand sqlCommand = currentDatabase.NewSqlCommand("select pmpSupplierOrganizationID,pmpPurchaseLocationID from PurchaseOrders where pmpPurchaseOrderID = @PoID");
			sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlPurchaseOrderID");
			DataTable dataTable = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count == 0)
			{
				return 0;
			}
			DataRow row = dataTable.Rows[0];
			sqlCommand = currentDatabase.NewSqlCommand("select imrLeadTime,imrLastMaterialCost from PartRevisions  where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlPartID");
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlPartRevisionID");
			short num = 0;
			decimal num2 = default(decimal);
			DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable2.Rows.Count != 0)
			{
				DataRow row2 = dataTable2.Rows[0];
				num = row2.Field<short>("imrLeadTime");
				num2 = ((!currentRow.Field<bool>("pmlPlanned")) ? default(decimal) : row2.Field<decimal>("imrLastMaterialCost"));
			}
			else
			{
				num = 0;
				num2 = default(decimal);
			}
			if (num2 == 0m && currentRow.Field<bool>("pmlPlanned"))
			{
				num2 = currentRow.Field<decimal>("pmlPurchaseUnitCostBase");
			}
			sqlCommand = currentDatabase.NewSqlCommand("select jmaProductionQuantity from JobAssemblies where jmaJobID = @JobID and jmaJobAssemblyID = @Asm");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("pmlJobAssemblyID");
			decimal num3 = currentRow.Field<decimal>("pmlInventoryQuantity");
			DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable3.Rows.Count != 0)
			{
				DataRow row3 = dataTable3.Rows[0];
				if (row3.Field<decimal>("jmaProductionQuantity") != 0m)
				{
					num3 = M1Math.Round(num3 / row3.Field<decimal>("jmaProductionQuantity"), 5);
				}
			}
			sqlCommand = currentDatabase.NewSqlCommand("select jmpScheduledStartDate,jmpScheduledDueDate from Jobs where jmpJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlJobID");
			DataTable dataTable4 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable4.Rows.Count == 0)
			{
				return 0;
			}
			object value = dataTable4.Rows[0]["jmpScheduledStartDate"];
			object value2 = dataTable4.Rows[0]["jmpScheduledDueDate"];
			switch (currentRow.Field<byte>("pmlJobType"))
			{
			case 1:
			{
				M1BindingSource m1BindingSource3 = new M1BindingSource(currentDatabase, transaction);
				m1BindingSource3.LoadDefinition(string.Empty, "JobMaterials", null, true, loadDataNow: false);
				m1BindingSource3.ClearCache();
				DataRow dataRow2 = m1BindingSource3.AddNew() as DataRow;
				dataRow2.SetField("jmmJobID", currentRow.Field<string>("pmlJobID"));
				dataRow2.SetField("jmmJobAssemblyID", currentRow.Field<int>("pmlJobAssemblyID"));
				dataRow2.SetField("jmmPartID", currentRow.Field<string>("pmlPartID"));
				dataRow2.SetField("jmmPartRevisionID", currentRow.Field<string>("pmlPartRevisionID"));
				dataRow2.SetField("jmmPartWarehouseLocationID", currentRow.Field<string>("pmlPartWarehouseLocationID"));
				dataRow2.SetField("jmmPartBinID", currentRow.Field<string>("pmlPartBinID"));
				dataRow2.SetField("jmmPartShortDescription", currentRow.Field<string>("pmlPartShortDescription"));
				dataRow2["jmmKitPart"] = currentRow.Field<bool>("pmlKitPart");
				dataRow2.SetField("jmmUnitOfMeasure", currentRow.Field<string>("pmlInventoryUnitOfMeasure"));
				dataRow2.SetField("jmmPartLongDescriptionRTF", currentRow.Field<string>("pmlPartLongDescriptionRTF"));
				dataRow2.SetField("jmmPartLongDescriptionText", currentRow.Field<string>("pmlPartLongDescriptionText"));
				dataRow2.SetField("jmmQuantityPerAssembly", num3);
				dataRow2.SetField("jmmSupplierOrganizationID", row.Field<string>("pmpSupplierOrganizationID"));
				dataRow2.SetField("jmmPurchaseLocationID", row.Field<string>("pmpPurchaseLocationID"));
				dataRow2.SetField("jmmPurchaseOrderID", currentRow.Field<string>("pmlPurchaseOrderID"));
				dataRow2.SetField("jmmLeadTime", (double)num);
				dataRow2.SetField("jmmDueInDate", currentRow.Field<DateTime>("pmlDueDate"));
				dataRow2.SetField("jmmEstimatedQuantity", currentRow.Field<decimal>("pmlInventoryQuantity"));
				dataRow2.SetField("jmmEstimatedUnitCost", num2);
				dataRow2["jmmCalculatedUnitCost"] = CalculateJobMaterialCalculatedCost(Convert.ToDouble(dataRow2["jmmEstimatedQuantity"]), Convert.ToDouble(dataRow2["jmmEstimatedUnitCost"]), Convert.ToDouble(dataRow2["jmmMinimumCharge"]), Convert.ToDouble(dataRow2["jmmQuantityBreak1"]), Convert.ToDouble(dataRow2["jmmUnitCost1"]), Convert.ToDouble(dataRow2["jmmQuantityBreak2"]), Convert.ToDouble(dataRow2["jmmUnitCost2"]), Convert.ToDouble(dataRow2["jmmQuantityBreak3"]), Convert.ToDouble(dataRow2["jmmUnitCost3"]), Convert.ToDouble(dataRow2["jmmQuantityBreak4"]), Convert.ToDouble(dataRow2["jmmUnitCost4"]), Convert.ToDouble(dataRow2["jmmQuantityBreak5"]), Convert.ToDouble(dataRow2["jmmUnitCost5"]), Convert.ToDouble(dataRow2["jmmQuantityBreak6"]), Convert.ToDouble(dataRow2["jmmUnitCost6"]), Convert.ToDouble(dataRow2["jmmQuantityBreak7"]), Convert.ToDouble(dataRow2["jmmUnitCost7"]), Convert.ToDouble(dataRow2["jmmQuantityBreak8"]), Convert.ToDouble(dataRow2["jmmUnitCost8"]), Convert.ToDouble(dataRow2["jmmQuantityBreak9"]), Convert.ToDouble(dataRow2["jmmUnitCost9"]));
				sqlCommand = m1BindingSource3.Database.NewSqlCommand("select IsNull(Max(jmmJobMaterialID),0) as jmmJobMaterialID from JobMaterials where jmmJobID = @JobID and jmmJobAssemblyID = @Asm");
				sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("pmlJobAssemblyID");
				DataTable dataTable8 = m1BindingSource3.Database.GetDataTable(sqlCommand, transaction);
				int value4 = 1;
				if (dataTable8.Rows.Count != 0 && !dataTable8.Rows[0].IsNull("jmmJobMaterialID"))
				{
					value4 = dataTable8.Rows[0].Field<int>("jmmJobMaterialID") + 1;
				}
				dataRow2.SetField("jmmJobMaterialID", value4);
				m1BindingSource3.SaveData();
				if (currentRow.Field<bool>("pmlKitPart"))
				{
					M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderComponents");
					M1BindingSource childBindingSource2 = m1BindingSource3.PrimaryTable.GetChildBindingSource("JobMaterialComponents");
					foreach (DataRowView item in childBindingSource.GetDataView(currentRow))
					{
						DataRow row4 = childBindingSource2.AddNew() as DataRow;
						row4.SetField("jmtPartID", item.Row.Field<string>("pmoPartID"));
						row4.SetField("jmtPartRevisionID", item.Row.Field<string>("pmoPartRevisionID"));
						row4.SetField("jmtPartWarehouseLocationID", item.Row.Field<string>("pmoPartWarehouseLocationID"));
						row4.SetField("jmtPartBinID", item.Row.Field<string>("pmoPartBinID"));
						row4.SetField("jmtQuantityPerParent", item.Row.Field<decimal>("pmoQuantityPerParent"));
						row4.SetField("jmtParentQuantity", item.Row.Field<decimal>("pmoParentQuantity"));
						row4.SetField("jmtAdditionalQuantity", item.Row.Field<decimal>("pmoAdditionalQuantity"));
						row4.SetField("jmtUnitOfMeasure", item.Row.Field<string>("pmoUnitOfMeasure"));
						row4.SetField("jmtDescription", item.Row.Field<string>("pmoDescription"));
						row4.SetField("jmtWeight", item.Row.Field<decimal>("pmoWeight"));
						childBindingSource2.SaveData();
						item.Row.SetField("pmoJobID", row4.Field<string>("jmtJobID"));
						item.Row.SetField("pmoJobAssemblyID", row4.Field<int>("jmtJobAssemblyID"));
						item.Row.SetField("pmoJobMaterialID", row4.Field<int>("jmtJobMaterialID"));
						item.Row.SetField("pmoJobMaterialComponentID", row4.Field<int>("jmtJobMaterialComponentID"));
					}
				}
				return dataRow2.Field<int>("jmmJobMaterialID");
			}
			case 2:
			{
				M1BindingSource m1BindingSource2 = new M1BindingSource(currentDatabase, transaction);
				m1BindingSource2.LoadDefinition(string.Empty, "JobOperations", null, true, loadDataNow: false);
				m1BindingSource2.ClearCache();
				DataRow dataRow = m1BindingSource2.AddNew() as DataRow;
				dataRow.SetField("jmoJobID", currentRow.Field<string>("pmlJobID"));
				dataRow.SetField("jmoJobAssemblyID", currentRow.Field<int>("pmlJobAssemblyID"));
				dataRow.SetField("jmoOperationType", (short)2);
				dataRow["jmoStartDate"] = value;
				dataRow["jmoDueDate"] = value2;
				dataRow.SetField("jmoPartID", currentRow.Field<string>("pmlPartID"));
				dataRow.SetField("jmoPartRevisionID", currentRow.Field<string>("pmlPartRevisionID"));
				dataRow.SetField("jmoUnitOfMeasure", currentRow.Field<string>("pmlInventoryUnitOfMeasure"));
				dataRow.SetField("jmoOperationQuantity", currentRow.Field<decimal>("pmlInventoryQuantity"));
				dataRow.SetField("jmoAddedOperation", currentRow.Field<bool>("pmlPlanned"));
				dataRow.SetField("jmoStandardFactor", currentDatabase.Props("ProductionProperties").Field<string>("xapJMStandardFactor"));
				dataRow.SetField("jmoWorkCenterID", currentRow.Field<string>("pmlWorkcenterID"));
				sqlCommand = m1BindingSource2.Database.NewSqlCommand("select xawOverheadRate from WorkCenters where xawWorkCenterID = @WorkcenterID");
				sqlCommand.Parameters.Add(new SqlParameter("@WorkcenterID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlWorkcenterID");
				DataTable dataTable5 = m1BindingSource2.Database.GetDataTable(sqlCommand, transaction);
				if (dataTable5.Rows.Count == 0)
				{
					return 0;
				}
				dataRow["jmoOverheadRate"] = dataTable5.Rows[0]["xawOverheadRate"];
				dataRow.SetField("jmoProcessID", currentRow.Field<string>("pmlProcessID"));
				sqlCommand = m1BindingSource2.Database.NewSqlCommand("select xacShortDescription,xacLongDescriptionRTF,xacLongDescriptionText,xacProjectedSetupRate,xacProjectedProductionRate from Processes where xacProcessID = @ProcessID");
				sqlCommand.Parameters.Add(new SqlParameter("@ProcessID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlProcessID");
				DataTable dataTable6 = m1BindingSource2.Database.GetDataTable(sqlCommand, transaction);
				if (dataTable6.Rows.Count == 0)
				{
					return 0;
				}
				dataRow.SetField("jmoProcessShortDescription", dataTable6.Rows[0].Field<string>("xacShortDescription"));
				dataRow.SetField("jmoProcessLongDescriptionRTF", dataTable6.Rows[0].Field<string>("xacLongDescriptionRTF"));
				dataRow.SetField("jmoProcessLongDescriptionText", dataTable6.Rows[0].Field<string>("xacLongDescriptionText"));
				dataRow.SetField("jmoSetupRate", dataTable6.Rows[0].Field<decimal>("xacProjectedSetupRate"));
				dataRow.SetField("jmoProductionRate", dataTable6.Rows[0].Field<decimal>("xacProjectedProductionRate"));
				dataRow.SetField("jmoEstimatedUnitCost", num2);
				dataRow.SetField("jmoQuantityPerAssembly", num3);
				dataRow.SetField("jmoSetupHours", 1m);
				dataRow.SetField("jmoSupplierOrganizationID", row.Field<string>("pmpSupplierOrganizationID"));
				dataRow.SetField("jmoPurchaseLocationID", row.Field<string>("pmpPurchaseLocationID"));
				dataRow.SetField("jmoPurchaseOrderID", currentRow.Field<string>("pmlPurchaseOrderID"));
				dataRow.SetField("jmoSetupCharge", currentRow.Field<decimal>("pmlSetupChargeBase"));
				dataRow["jmoCalculatedUnitCost"] = CalculateJobOperationCalculatedCost(Convert.ToDouble(dataRow["jmoOperationQuantity"]), Convert.ToDouble(dataRow["jmoEstimatedUnitCost"]), Convert.ToDouble(dataRow["jmoMinimumCharge"]), Convert.ToDouble(dataRow["jmoSetupCharge"]), Convert.ToDouble(dataRow["jmoQuantityBreak1"]), Convert.ToDouble(dataRow["jmoUnitCost1"]), Convert.ToDouble(dataRow["jmoQuantityBreak2"]), Convert.ToDouble(dataRow["jmoUnitCost2"]), Convert.ToDouble(dataRow["jmoQuantityBreak3"]), Convert.ToDouble(dataRow["jmoUnitCost3"]), Convert.ToDouble(dataRow["jmoQuantityBreak4"]), Convert.ToDouble(dataRow["jmoUnitCost4"]), Convert.ToDouble(dataRow["jmoQuantityBreak5"]), Convert.ToDouble(dataRow["jmoUnitCost5"]), Convert.ToDouble(dataRow["jmoQuantityBreak6"]), Convert.ToDouble(dataRow["jmoUnitCost6"]), Convert.ToDouble(dataRow["jmoQuantityBreak7"]), Convert.ToDouble(dataRow["jmoUnitCost7"]), Convert.ToDouble(dataRow["jmoQuantityBreak8"]), Convert.ToDouble(dataRow["jmoUnitCost8"]), Convert.ToDouble(dataRow["jmoQuantityBreak9"]), Convert.ToDouble(dataRow["jmoUnitCost9"]));
				sqlCommand = m1BindingSource2.Database.NewSqlCommand("select IsNull(Max(jmoJobOperationID),10) as jmoJobOperationID from JobOperations where jmoJobID = @JobID and jmoJobAssemblyID = @Asm");
				sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("pmlJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("pmlJobAssemblyID");
				DataTable dataTable7 = m1BindingSource2.Database.GetDataTable(sqlCommand, transaction);
				int value3 = 10;
				if (dataTable7.Rows.Count != 0 && dataTable7.Rows[0]["jmoJobOperationID"] != null)
				{
					value3 = dataTable7.Rows[0].Field<int>("jmoJobOperationID") + 10;
				}
				dataRow.SetField("jmoJobOperationID", value3);
				m1BindingSource2.SaveData();
				return dataRow.Field<int>("jmoJobOperationID");
			}
			default:
				return 0;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int CreateJobSequenceFromMaterialIssue(M1BindingSource m1BindingSource, SqlTransaction transaction, DataRow currentRow)
	{
		try
		{
			M1Database currentDatabase = m1BindingSource.CurrentDatabase;
			if (currentRow == null)
			{
				return 0;
			}
			SqlCommand sqlCommand = currentDatabase.NewSqlCommand("select imrPartID,imrShortDescription,imrInventoryUnitOfMeasure,imrLongDescriptionRTF,imrLongDescriptionText,imrLeadTime,imrLastMaterialCost  from PartRevisions where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("injPartID");
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("injPartRevisionID");
			DataTable dataTable = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count == 0)
			{
				return 0;
			}
			sqlCommand = currentDatabase.NewSqlCommand("select jmaProductionQuantity from JobAssemblies where jmaJobID = @JobID and jmaJobAssemblyID = @Asm");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("injJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("injJobAssemblyID");
			decimal num = currentRow.Field<decimal>("injJobMatIssueQuantity");
			DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable2.Rows.Count != 0)
			{
				DataRow row = dataTable2.Rows[0];
				if (row.Field<decimal>("jmaProductionQuantity") != 0m)
				{
					num = M1Math.Round(num / row.Field<decimal>("jmaProductionQuantity"), 5);
				}
			}
			sqlCommand = currentDatabase.NewSqlCommand("select jmpScheduledStartDate,jmpScheduledDueDate from Jobs where jmpJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("injJobID");
			DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable3.Rows.Count == 0)
			{
				return 0;
			}
			_ = dataTable3.Rows[0]["jmpScheduledStartDate"];
			_ = dataTable3.Rows[0]["jmpScheduledDueDate"];
			if (currentRow.Field<byte>("injJobType") == 1)
			{
				M1BindingSource m1BindingSource2 = new M1BindingSource(currentDatabase, transaction);
				m1BindingSource2.LoadDefinition(string.Empty, "JobMaterials", null, true, loadDataNow: false);
				m1BindingSource2.ClearCache();
				DataRow dataRow = m1BindingSource2.AddNew() as DataRow;
				dataRow.SetField("jmmJobID", currentRow.Field<string>("injJobID"));
				dataRow.SetField("jmmJobAssemblyID", currentRow.Field<int>("injJobAssemblyID"));
				dataRow.SetField("jmmPartID", currentRow.Field<string>("injPartID"));
				dataRow.SetField("jmmPartRevisionID", currentRow.Field<string>("injPartRevisionID"));
				dataRow.SetField("jmmPartWarehouseLocationID", currentRow.Field<string>("injPartWarehouseLocationID"));
				dataRow.SetField("jmmPartBinID", currentRow.Field<string>("injPartBinID"));
				dataRow.SetField("jmmPartShortDescription", dataTable.Rows[0].Field<string>("imrShortDescription"));
				dataRow.SetField("jmmUnitOfMeasure", dataTable.Rows[0].Field<string>("imrInventoryUnitOfMeasure"));
				dataRow.SetField("jmmPartLongDescriptionRTF", dataTable.Rows[0].Field<string>("imrLongDescriptionRTF"));
				dataRow.SetField("jmmPartLongDescriptionText", dataTable.Rows[0].Field<string>("imrLongDescriptionText"));
				dataRow.SetField("jmmQuantityPerAssembly", num);
				dataRow.SetField("jmmLeadTime", dataTable.Rows[0].Field<short>("imrLeadTime"));
				dataRow.SetField("jmmScrapQuantityReceived", currentRow.Field<decimal>("injJobMatScrapQuantity"));
				dataRow["jmmEstimatedQuantity"] = CalculateQtyWithScrap(currentDatabase, Convert.ToDouble(currentRow.Field<decimal>("injJobMatIssueQuantity")), Convert.ToDouble(dataRow.Field<decimal>("jmmScrapPercent")), Convert.ToDouble(dataRow.Field<decimal>("jmmScrapQuantity")), currentDatabase.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow.SetField("jmmEstimatedUnitCost", dataTable.Rows[0].Field<decimal>("imrLastMaterialCost"));
				dataRow["jmmCalculatedUnitCost"] = CalculateJobMaterialCalculatedCost(Convert.ToDouble(dataRow["jmmEstimatedQuantity"]), Convert.ToDouble(dataRow["jmmEstimatedUnitCost"]), Convert.ToDouble(dataRow["jmmMinimumCharge"]), Convert.ToDouble(dataRow["jmmQuantityBreak1"]), Convert.ToDouble(dataRow["jmmUnitCost1"]), Convert.ToDouble(dataRow["jmmQuantityBreak2"]), Convert.ToDouble(dataRow["jmmUnitCost2"]), Convert.ToDouble(dataRow["jmmQuantityBreak3"]), Convert.ToDouble(dataRow["jmmUnitCost3"]), Convert.ToDouble(dataRow["jmmQuantityBreak4"]), Convert.ToDouble(dataRow["jmmUnitCost4"]), Convert.ToDouble(dataRow["jmmQuantityBreak5"]), Convert.ToDouble(dataRow["jmmUnitCost5"]), Convert.ToDouble(dataRow["jmmQuantityBreak6"]), Convert.ToDouble(dataRow["jmmUnitCost6"]), Convert.ToDouble(dataRow["jmmQuantityBreak7"]), Convert.ToDouble(dataRow["jmmUnitCost7"]), Convert.ToDouble(dataRow["jmmQuantityBreak8"]), Convert.ToDouble(dataRow["jmmUnitCost8"]), Convert.ToDouble(dataRow["jmmQuantityBreak9"]), Convert.ToDouble(dataRow["jmmUnitCost9"]));
				dataRow.SetField("jmmKitPart", currentRow.Field<bool>("injKitPart"));
				dataRow.SetField("jmmReceivedComplete", currentRow.Field<bool>("injIssueComplete"));
				dataRow.SetField("jmmQuantityReceived", currentRow.Field<decimal>("injJobMatIssueQuantity"));
				sqlCommand = m1BindingSource2.Database.NewSqlCommand("select IsNull(Max(jmmJobMaterialID),0) as jmmJobMaterialID from JobMaterials where jmmJobID = @JobID and jmmJobAssemblyID = @Asm");
				sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("injJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("injJobAssemblyID");
				DataTable dataTable4 = m1BindingSource2.Database.GetDataTable(sqlCommand, transaction);
				int value = 1;
				if (dataTable4.Rows.Count != 0 && dataTable4.Rows[0]["jmmJobMaterialID"] != null)
				{
					value = dataTable4.Rows[0].Field<int>("jmmJobMaterialID") + 1;
				}
				dataRow.SetField("jmmJobMaterialID", value);
				m1BindingSource2.SaveData();
				if (currentRow.Field<bool>("injKitPart"))
				{
					M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
					M1BindingSource childBindingSource2 = m1BindingSource2.PrimaryTable.GetChildBindingSource("JobMaterialComponents");
					foreach (DataRowView item in childBindingSource.GetDataView(currentRow))
					{
						DataRow row2 = childBindingSource2.AddNew() as DataRow;
						row2.SetField("jmtPartID", item.Row.Field<string>("inkPartID"));
						row2.SetField("jmtPartRevisionID", item.Row.Field<string>("inkPartRevisionID"));
						row2.SetField("jmtPartWarehouseLocationID", item.Row.Field<string>("inkPartWarehouseLocationID"));
						row2.SetField("jmtPartBinID", item.Row.Field<string>("inkPartBinID"));
						row2.SetField("jmtQuantityPerParent", item.Row.Field<decimal>("inkQuantityPerParent"));
						row2.SetField("jmtParentQuantity", item.Row.Field<decimal>("inkJobMatParentQuantity"));
						row2.SetField("jmtAdditionalQuantity", item.Row.Field<decimal>("inkAdditionalQuantity"));
						row2.SetField("jmtUnitOfMeasure", item.Row.Field<string>("inkUnitOfMeasure"));
						row2.SetField("jmtDescription", item.Row.Field<string>("inkDescription"));
						row2.SetField("jmtWeight", item.Row.Field<decimal>("inkWeight"));
						row2.SetField("jmtReceivedComplete", item.Row.Field<bool>("inkReceivedComplete"));
						childBindingSource2.SaveData();
						item.Row.SetField("inkJobID", row2.Field<string>("jmtJobID"));
						item.Row.SetField("inkJobAssemblyID", row2.Field<int>("jmtJobAssemblyID"));
						item.Row.SetField("inkJobMaterialID", row2.Field<int>("jmtJobMaterialID"));
						item.Row.SetField("inkJobMaterialComponentID", row2.Field<int>("jmtJobMaterialComponentID"));
					}
				}
				return dataRow.Field<int>("jmmJobMaterialID");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int CreateJobSequenceFromMfgReceipt(M1BindingSource m1BindingSource, SqlTransaction transaction, DataRow currentRow)
	{
		try
		{
			M1Database currentDatabase = m1BindingSource.CurrentDatabase;
			if (currentRow == null)
			{
				return 0;
			}
			SqlCommand sqlCommand = currentDatabase.NewSqlCommand("select imrPartID,imrShortDescription,imrInventoryUnitOfMeasure,imrLongDescriptionRTF,imrLongDescriptionText,imrLeadTime,imrLastMaterialCost  from PartRevisions where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("rmmPartID");
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("rmmPartRevisionID");
			DataTable dataTable = currentDatabase.GetDataTable(sqlCommand, transaction);
			string value;
			string value2;
			string value3;
			string value4;
			short value5;
			decimal value6;
			if (dataTable.Rows.Count == 0)
			{
				value = currentRow.Field<string>("rmmPartID");
				value2 = currentDatabase.Props("OM").Field<string>("xapOMUnitOfMeasure");
				value3 = "";
				value4 = "";
				value5 = 0;
				value6 = currentRow.Field<decimal>("rmmUnitMaterialCost");
			}
			else
			{
				value = dataTable.Rows[0].Field<string>("imrShortDescription");
				value2 = dataTable.Rows[0].Field<string>("imrInventoryUnitOfMeasure");
				value3 = dataTable.Rows[0].Field<string>("imrLongDescriptionRTF");
				value4 = dataTable.Rows[0].Field<string>("imrLongDescriptionText");
				value5 = dataTable.Rows[0].Field<short>("imrLeadTime");
				value6 = dataTable.Rows[0].Field<decimal>("imrLastMaterialCost");
			}
			sqlCommand = currentDatabase.NewSqlCommand("select jmaProductionQuantity from JobAssemblies where jmaJobID = @JobID and jmaJobAssemblyID = @Asm");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("rmmJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("rmmJobAssemblyID");
			decimal num = currentRow.Field<decimal>("rmmJobMatQuantityReceived");
			DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable2.Rows.Count != 0)
			{
				DataRow row = dataTable2.Rows[0];
				if (row.Field<decimal>("jmaProductionQuantity") != 0m)
				{
					num = M1Math.Round(num / row.Field<decimal>("jmaProductionQuantity"), 5);
				}
			}
			sqlCommand = currentDatabase.NewSqlCommand("select jmpScheduledStartDate,jmpScheduledDueDate from Jobs where jmpJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("rmmJobID");
			DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand, transaction);
			if (dataTable3.Rows.Count == 0)
			{
				return 0;
			}
			_ = dataTable3.Rows[0]["jmpScheduledStartDate"];
			_ = dataTable3.Rows[0]["jmpScheduledDueDate"];
			if (currentRow.Field<byte>("rmmJobType") == 1)
			{
				M1BindingSource m1BindingSource2 = new M1BindingSource(currentDatabase, transaction);
				m1BindingSource2.LoadDefinition(string.Empty, "JobMaterials", null, true, loadDataNow: false);
				m1BindingSource2.ClearCache();
				DataRow dataRow = m1BindingSource2.AddNew() as DataRow;
				dataRow.SetField("jmmJobID", currentRow.Field<string>("rmmJobID"));
				dataRow.SetField("jmmJobAssemblyID", currentRow.Field<int>("rmmJobAssemblyID"));
				dataRow.SetField("jmmPartID", currentRow.Field<string>("rmmPartID"));
				dataRow.SetField("jmmPartRevisionID", currentRow.Field<string>("rmmPartRevisionID"));
				dataRow.SetField("jmmPartWarehouseLocationID", currentRow.Field<string>("rmmPartWarehouseLocationID"));
				dataRow.SetField("jmmPartBinID", currentRow.Field<string>("rmmPartBinID"));
				dataRow.SetField("jmmPartShortDescription", value);
				dataRow.SetField("jmmUnitOfMeasure", value2);
				dataRow.SetField("jmmPartLongDescriptionRTF", value3);
				dataRow.SetField("jmmPartLongDescriptionText", value4);
				dataRow.SetField("jmmQuantityPerAssembly", num);
				dataRow.SetField("jmmLeadTime", value5);
				dataRow["jmmEstimatedQuantity"] = CalculateQtyWithScrap(currentDatabase, Convert.ToDouble(currentRow.Field<decimal>("rmmJobMatQuantityReceived")), Convert.ToDouble(dataRow.Field<decimal>("jmmScrapPercent")), Convert.ToDouble(dataRow.Field<decimal>("jmmScrapQuantity")), currentDatabase.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow.SetField("jmmEstimatedUnitCost", value6);
				dataRow["jmmCalculatedUnitCost"] = CalculateJobMaterialCalculatedCost(Convert.ToDouble(dataRow["jmmEstimatedQuantity"]), Convert.ToDouble(dataRow["jmmEstimatedUnitCost"]), Convert.ToDouble(dataRow["jmmMinimumCharge"]), Convert.ToDouble(dataRow["jmmQuantityBreak1"]), Convert.ToDouble(dataRow["jmmUnitCost1"]), Convert.ToDouble(dataRow["jmmQuantityBreak2"]), Convert.ToDouble(dataRow["jmmUnitCost2"]), Convert.ToDouble(dataRow["jmmQuantityBreak3"]), Convert.ToDouble(dataRow["jmmUnitCost3"]), Convert.ToDouble(dataRow["jmmQuantityBreak4"]), Convert.ToDouble(dataRow["jmmUnitCost4"]), Convert.ToDouble(dataRow["jmmQuantityBreak5"]), Convert.ToDouble(dataRow["jmmUnitCost5"]), Convert.ToDouble(dataRow["jmmQuantityBreak6"]), Convert.ToDouble(dataRow["jmmUnitCost6"]), Convert.ToDouble(dataRow["jmmQuantityBreak7"]), Convert.ToDouble(dataRow["jmmUnitCost7"]), Convert.ToDouble(dataRow["jmmQuantityBreak8"]), Convert.ToDouble(dataRow["jmmUnitCost8"]), Convert.ToDouble(dataRow["jmmQuantityBreak9"]), Convert.ToDouble(dataRow["jmmUnitCost9"]));
				dataRow.SetField("jmmKitPart", currentRow.Field<bool>("rmmKitPart"));
				dataRow.SetField("jmmReceivedComplete", currentRow.Field<bool>("rmmReceivedComplete"));
				sqlCommand = m1BindingSource2.Database.NewSqlCommand("select IsNull(Max(jmmJobMaterialID),0) as jmmJobMaterialID from JobMaterials where jmmJobID = @JobID and jmmJobAssemblyID = @Asm");
				sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentRow.Field<string>("rmmJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentRow.Field<int>("rmmJobAssemblyID");
				DataTable dataTable4 = m1BindingSource2.Database.GetDataTable(sqlCommand, transaction);
				int value7 = 1;
				if (dataTable4.Rows.Count != 0 && dataTable4.Rows[0]["jmmJobMaterialID"] != null)
				{
					value7 = dataTable4.Rows[0].Field<int>("jmmJobMaterialID") + 1;
				}
				dataRow.SetField("jmmJobMaterialID", value7);
				m1BindingSource2.SaveData();
				if (currentRow.Field<bool>("rmmKitPart"))
				{
					M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("MfgReceiptComponents");
					M1BindingSource childBindingSource2 = m1BindingSource2.PrimaryTable.GetChildBindingSource("JobMaterialComponents");
					foreach (DataRowView item in childBindingSource.GetDataView(currentRow))
					{
						DataRow row2 = childBindingSource2.AddNew() as DataRow;
						row2.SetField("jmtPartID", item.Row.Field<string>("rmnPartID"));
						row2.SetField("jmtPartRevisionID", item.Row.Field<string>("rmnPartRevisionID"));
						row2.SetField("jmtPartWarehouseLocationID", item.Row.Field<string>("rmnPartWarehouseLocationID"));
						row2.SetField("jmtPartBinID", item.Row.Field<string>("rmnPartBinID"));
						row2.SetField("jmtQuantityPerParent", item.Row.Field<decimal>("rmnQuantityPerParent"));
						row2.SetField("jmtParentQuantity", item.Row.Field<decimal>("rmnJobMatParentQuantity"));
						row2.SetField("jmtAdditionalQuantity", item.Row.Field<decimal>("rmnAdditionalQuantity"));
						row2.SetField("jmtUnitOfMeasure", item.Row.Field<string>("rmnUnitOfMeasure"));
						row2.SetField("jmtDescription", item.Row.Field<string>("rmnDescription"));
						row2.SetField("jmtWeight", item.Row.Field<decimal>("rmnWeight"));
						row2.SetField("jmtReceivedComplete", item.Row.Field<bool>("rmnReceivedComplete"));
						childBindingSource2.SaveData();
						item.Row.SetField("rmnJobID", row2.Field<string>("jmtJobID"));
						item.Row.SetField("rmnJobAssemblyID", row2.Field<int>("jmtJobAssemblyID"));
						item.Row.SetField("rmnJobMaterialID", row2.Field<int>("jmtJobMaterialID"));
						item.Row.SetField("rmnJobMaterialComponentID", row2.Field<int>("jmtJobMaterialComponentID"));
					}
				}
				return dataRow.Field<int>("jmmJobMaterialID");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public JobCost GetJobCosts(M1Database database, SqlTransaction transaction, string jobID, int assemblyID, decimal qtyCompleted, byte invCostingMethod = 0)
	{
		JobCost jobCost = new JobCost();
		if (!string.IsNullOrWhiteSpace(jobID) && qtyCompleted > 0m)
		{
			string text = "";
			if (assemblyID != 0)
			{
				text = GetAssembliesList(database, transaction, jobID, assemblyID);
			}
			string value = "";
			if (assemblyID != 0)
			{
				value = " And lmlJobAssemblyID In (" + text + ")";
			}
			StringBuilder stringBuilder = new StringBuilder("select isnull(sum(lmlLaborCost),0) as lmlLaborCost, isnull(sum(lmlOverheadCost),0) as lmlOverheadCost from TimecardLines where lmlJobID = @JobID");
			stringBuilder.Append(value);
			SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				jobCost.LaborCost = M1Math.Round(row.Field<decimal>("lmlLaborCost") / qtyCompleted, 5);
				jobCost.OverheadCost = M1Math.Round(row.Field<decimal>("lmlOverheadCost") / qtyCompleted, 5);
			}
			bool useAPJobCosts = database.Props("AP").Field<bool>("xafAPUpdateJobCosts");
			byte b = ((invCostingMethod == 0) ? database.Props("PN").Field<byte>("xapIMCostingMethod") : invCostingMethod);
			value = ((assemblyID == 0) ? "" : (" And jmaJobAssemblyID In (" + text + ")"));
			stringBuilder = new StringBuilder(GetJobMaterialCostQuery(useAPJobCosts, !assemblyID.Equals(0), value));
			sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@CostingMethod", SqlDbType.TinyInt)).Value = ((b == 4 || b == 5) ? 4 : b);
			dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row2 = dataTable.Rows[0];
				jobCost.MaterialCost = M1Math.Round(row2.Field<decimal>("ACTMATCOST") / qtyCompleted, 5);
			}
			stringBuilder = new StringBuilder(GetJobSubContractCostQuery(useAPJobCosts, !assemblyID.Equals(0), value));
			sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
			dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row3 = dataTable.Rows[0];
				jobCost.SubcontractCost = M1Math.Round(row3.Field<decimal>("ACTCONTRACTCOST") / qtyCompleted, 5);
			}
		}
		return jobCost;
	}

	public string GetJobMaterialCostQuery(bool useAPJobCosts, bool nonZeroAssembly, string asmWhereClause)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (useAPJobCosts)
		{
			if (nonZeroAssembly)
			{
				stringBuilder = new StringBuilder("select ISNULL(SUM(ACTMATCOST),0) AS ACTMATCOST FROM ( Select (ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN ( select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines WHERE aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID and aplJobType In (1,3)) AND rmlJobID=jmpJobID AND rmlJobAssemblyID = jmaJobAssemblyID AND rmlJobType In (1,3) AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM((intUnitOverheadCost+intUnitLaborCost+intUnitMaterialCost+intUnitSubcontractCost+intUnitDutyCost+intUnitFreightCost+intUnitMiscCost)*((Case When imtSource = 3 Then -1 Else 1 End)*intQuantity)) FROM PartTransactions Inner Join PartTransactionCosts On imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID, ProductionProperties WHERE imtJobID=jmpJobID AND imtJobAssemblyID = jmaJobAssemblyID AND (imtSource = 3 OR imtSource = 2) AND imtNonInventoryTransaction = 0 AND imtJobType In (1,3) And (imtNonNettable = 0 OR (imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts,0) = 0)) And intCostType = @CostingMethod),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobAsmQuantityReceived+rmmJobMatQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmJobAssemblyID = jmaJobAssemblyID AND rmmReceiptType = 1 AND rmmJobType In (1,3)),0) +  ISNULL((SELECT SUM(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0)))) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlPurchaseOrderID <> '' AND rmlPurchaseOrderID NOT IN ( Select pmlPurchaseOrderID From APInvoiceExpenseAccounts Inner Join LandedCostChargeDetails on rmiUniqueID = apxSourceTableUniqueID Inner Join LandedCostCharges on rmiLandedCostID = rmhLandedCostID and rmiLandedCostChargeID = rmhLandedCostChargeID Inner Join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID Where pmlJobID = jmpJobID and pmlJobType in (1,3) And rmhAPInvoiceID <> '') AND rmlJobID =jmpJobID AND rmlJobAssemblyID = jmaJobAssemblyID AND rmlJobType In (1,3) AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM(apxAmount) From APInvoiceExpenseAccounts Inner Join LandedCostChargeDetails on rmiUniqueID = apxSourceTableUniqueID Inner Join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID Where pmlJobID=jmpJobID And pmlJobType In (1,3)),0) +  ISNULL((SELECT SUM(aplExtendedCostBase) FROM APInvoiceLines WHERE aplJobID=jmpJobID AND aplJobAssemblyID = jmaJobAssemblyID AND aplJobType In (1,3)),0)) as ACTMATCOST From Jobs Inner Join JobAssemblies on jmpJobID=jmaJobID Where jmpJobID = @JobID");
				stringBuilder.Append(asmWhereClause);
				stringBuilder.Append(") AS Test ");
			}
			else
			{
				stringBuilder = new StringBuilder("Select (ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN ( select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines WHERE aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID and aplJobType In (1,3)) AND rmlJobID=jmpJobID AND rmlJobType In (1,3) AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM((intUnitOverheadCost+intUnitLaborCost+intUnitMaterialCost+intUnitSubcontractCost+intUnitDutyCost+intUnitFreightCost+intUnitMiscCost)*((Case When imtSource = 3 Then -1 Else 1 End)*intQuantity)) FROM PartTransactions Inner Join PartTransactionCosts On imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID, ProductionProperties WHERE imtJobID=jmpJobID AND (imtSource = 3 OR imtSource = 2) AND imtNonInventoryTransaction = 0 AND imtJobType In (1,3) AND (imtNonNettable = 0 Or (imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts,0) = 0)) And intCostType = @CostingMethod),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobAsmQuantityReceived+rmmJobMatQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND rmmJobType In (1,3)),0) +  ISNULL((SELECT SUM(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0)))) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlPurchaseOrderID <> '' AND rmlJobID=jmpJobID AND rmlJobType In (1,3) AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM(aplExtendedCostBase) FROM APInvoiceLines WHERE aplJobID=jmpJobID AND aplJobType In (1,3)),0)) as ACTMATCOST From Jobs Where jmpJobID = @JobID");
			}
		}
		else if (nonZeroAssembly)
		{
			stringBuilder = new StringBuilder("select ISNULL(SUM(ACTMATCOST),0) AS ACTMATCOST FROM ( Select (ISNULL((SELECT SUM((intUnitOverheadCost+intUnitLaborCost+intUnitMaterialCost+intUnitSubcontractCost+intUnitDutyCost+intUnitFreightCost+intUnitMiscCost)*((Case When imtSource = 3 Then -1 Else 1 End)*intQuantity)) FROM PartTransactions Inner Join PartTransactionCosts On imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID, ProductionProperties WHERE imtJobID=jmpJobID AND imtJobAssemblyID = jmaJobAssemblyID AND (imtSource = 3 OR imtSource = 2) AND imtNonInventoryTransaction = 0 AND imtJobType In (1,3) And (imtNonNettable = 0 Or (imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts,0) = 0)) And intCostType = @CostingMethod),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobAsmQuantityReceived+rmmJobMatQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmJobAssemblyID = jmaJobAssemblyID AND rmmReceiptType = 1 AND rmmJobType In (1,3)),0) +  ISNULL((SELECT SUM(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0)))) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlPurchaseOrderID <> '' AND rmlJobID=jmpJobID AND rmlJobAssemblyID = jmaJobAssemblyID AND rmlJobType In (1,3) AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlJobID=jmpJobID AND rmlJobAssemblyID = jmaJobAssemblyID AND rmlJobType In (1,3)),0)) AS ACTMATCOST From Jobs Inner Join JobAssemblies on jmpJobID=jmaJobID Where jmpJobID = @JobID");
			stringBuilder.Append(asmWhereClause);
			stringBuilder.Append(") AS Test ");
		}
		else
		{
			stringBuilder = new StringBuilder("Select (ISNULL((SELECT SUM((intUnitOverheadCost+intUnitLaborCost+intUnitMaterialCost+intUnitSubcontractCost+intUnitDutyCost+intUnitFreightCost+intUnitMiscCost)*((Case When imtSource = 3 Then -1 Else 1 End)*intQuantity)) FROM PartTransactions Inner Join PartTransactionCosts On imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID, ProductionProperties WHERE imtJobID=jmpJobID AND (imtSource = 3 OR imtSource = 2) AND imtNonInventoryTransaction = 0 AND imtJobType In (1,3) And (imtNonNettable = 0 Or (imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts,0) = 0)) And intCostType = @CostingMethod),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobAsmQuantityReceived+rmmJobMatQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND rmmJobType In (1,3)),0) +  ISNULL((SELECT SUM(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0)))) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlPurchaseOrderID <> '' AND rmlJobID=jmpJobID AND rmlJobType In (1,3) AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlJobID=jmpJobID AND rmlJobType In (1,3)),0)) AS ACTMATCOST From Jobs Where jmpJobID = @JobID");
		}
		return stringBuilder.ToString();
	}

	public string GetJobSubContractCostQuery(bool useAPJobCosts, bool nonZeroAssembly, string asmWhereClause)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (useAPJobCosts)
		{
			if (nonZeroAssembly)
			{
				stringBuilder = new StringBuilder("select ISNULL(SUM(ACTCONTRACTCOST),0) AS ACTCONTRACTCOST FROM ( Select (ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobOprQuantityReceived+ IsNull(qalJobOprQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN ( select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines WHERE aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID and aplJobType = 2) AND rmlJobID=jmpJobID AND rmlJobAssemblyID = jmaJobAssemblyID AND rmlJobType = 2 AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobOprQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmJobAssemblyID = jmaJobAssemblyID AND rmmReceiptType = 1 AND rmmJobType = 2),0) +  ISNULL((SELECT SUM(aplExtendedCostBase) FROM APInvoiceLines WHERE aplJobID=jmpJobID AND aplJobAssemblyID = jmaJobAssemblyID AND aplJobType = 2),0)) AS ACTCONTRACTCOST From Jobs Inner Join JobAssemblies on jmpJobID=jmaJobID Where jmpJobID = @JobID");
				stringBuilder.Append(asmWhereClause);
				stringBuilder.Append(") AS Test ");
			}
			else
			{
				stringBuilder = new StringBuilder("Select(ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobOprQuantityReceived + IsNull(qalJobOprQuantityAccepted, 0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN ( select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines WHERE aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID and aplJobType = 2) AND rmlJobID=jmpJobID AND rmlJobType = 2 AND rmlInvoicedComplete = 0),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobOprQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND rmmJobType = 2),0) +  ISNULL((SELECT SUM(aplExtendedCostBase) FROM APInvoiceLines WHERE aplJobID=jmpJobID AND aplJobType = 2),0)) AS ACTCONTRACTCOST From Jobs Where jmpJobID = @JobID");
			}
		}
		else if (nonZeroAssembly)
		{
			stringBuilder = new StringBuilder("select ISNULL(SUM(ACTCONTRACTCOST),0) AS ACTCONTRACTCOST FROM ( Select(ISNULL((SELECT SUM((rmmUnitOverheadCost + rmmUnitLaborCost + rmmUnitMaterialCost + rmmUnitSubcontractCost) * (rmmScrapQuantity + rmmJobOprQuantityReceived)) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmJobAssemblyID = jmaJobAssemblyID AND rmmReceiptType = 1 AND rmmJobType = 2),0) +  ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobOprQuantityReceived+ IsNull(qalJobOprQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlJobID=jmpJobID AND rmlJobAssemblyID = jmaJobAssemblyID AND rmlJobType = 2),0)) AS ACTCONTRACTCOST From Jobs Inner Join JobAssemblies on jmpJobID=jmaJobID Where jmpJobID = @JobID");
			stringBuilder.Append(asmWhereClause);
			stringBuilder.Append(") AS Test ");
		}
		else
		{
			stringBuilder = new StringBuilder("Select(ISNULL((SELECT SUM(((rmmUnitOverheadCost + rmmUnitLaborCost + rmmUnitMaterialCost + rmmUnitSubcontractCost) * (rmmScrapQuantity + rmmJobOprQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND rmmJobType = 2),0) +  ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobOprQuantityReceived+ IsNull(qalJobOprQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlJobID=jmpJobID AND rmlJobType = 2),0)) AS ACTCONTRACTCOST From Jobs Where jmpJobID = @JobID");
		}
		return stringBuilder.ToString();
	}

	public string GetAssembliesList(M1Database database, SqlTransaction transaction, string jobID, int assemblyID)
	{
		string assemblyList = assemblyID.ToSql();
		SqlCommand sqlCommand = database.NewSqlCommand("select jmaJobAssemblyID,jmaParentAssemblyID From JobAssemblies Where jmaJobID = @JobID And jmaJobAssemblyID <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		if (database.GetDataTable(sqlCommand, transaction).Rows.Count != 0)
		{
			checkSubAssembly(database, transaction, jobID, assemblyID, ref assemblyList);
		}
		return assemblyList;
	}

	private void checkSubAssembly(M1Database database, SqlTransaction transaction, string jobID, int assemblyID, ref string assemblyList)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select jmaJobAssemblyID,jmaParentAssemblyID From JobAssemblies Where jmaJobID = @JobID And jmaJobAssemblyID <> 0 and jmaParentAssemblyID = @ParentID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@ParentID", SqlDbType.Int)).Value = assemblyID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			assemblyList = assemblyList + "," + row.Field<int>("jmaJobAssemblyID");
			checkSubAssembly(database, transaction, jobID, row.Field<int>("jmaJobAssemblyID"), ref assemblyList);
		}
	}

	public void JobMaterialSaveAsEvent(M1Database database, SqlTransaction transaction, string whereClause)
	{
		StringBuilder stringBuilder = new StringBuilder("UPDATE JobMaterials SET jmmEstimatedQuantity = 0, jmmQuantityReceived = 0, jmmScrapQuantityReceived = 0, jmmScrapQuantity = 0, jmmQuantityAllocated = 0, jmmQuantityToInspect = 0, jmmQuantityToReturn = 0, jmmReceivedComplete = 0, jmmRequiredDate = NULL, jmmOrderByDate = Null, jmmPurchaseOrderID = '', jmmRFQID = '' WHERE ");
		stringBuilder.Append(whereClause);
		SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		database.ExecuteCommand(sqlCommand, transaction);
		stringBuilder = new StringBuilder("UPDATE JobMaterialComponents SET jmtMaterialQuantity = 0, jmtQuantityReceived = 0, jmtReceivedComplete = 0, jmtScrapQuantityReceived = 0, jmtQuantityAllocated = 0, jmtQuantityToInspect = 0, jmtQuantityToReturn = 0 From JobMaterialComponents Inner Join JobMaterials On jmmJobID = jmtJobID And jmmJobAssemblyID = jmtJobAssemblyID and jmmJobMaterialID = jmtJobMaterialID WHERE ");
		stringBuilder.Append(whereClause);
		sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		database.ExecuteCommand(sqlCommand, transaction);
		stringBuilder = new StringBuilder("Select jmmJobID,Avg(jmpProductionQuantity) As jmpProductionQuantity From JobMaterials Inner Join Jobs On jmmJobID = jmpJobID Where ");
		stringBuilder.Append(whereClause + " Group By jmmJobID");
		sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			ChangeProductionQty(database, transaction, row.Field<string>("jmmJobID"), 0, Convert.ToDouble(row.Field<decimal>("jmpProductionQuantity")), Convert.ToDouble(row.Field<decimal>("jmpProductionQuantity")), updateAsm: false);
		}
	}

	public bool IsParentJobOnHold(M1Database database, string jobID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select jmpJobID From Jobs Where jmpJobID = @JobID And jmpOnHold = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return !string.IsNullOrEmpty((string)database.ExecuteScalar(sqlCommand));
	}

	public bool IsParentJobOnTimeAndMaterial(M1Database database, string jobID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select jmpJobID From Jobs Where jmpJobID = @JobID And jmpTimeAndMaterial = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return !string.IsNullOrEmpty((string)database.ExecuteScalar(sqlCommand));
	}

	public string GetJobIDForOrder(M1Database database, string orderID, short lineID, bool? includeLineInJobOverride = false)
	{
		string result = string.Empty;
		bool flag = Convert.ToBoolean(includeLineInJobOverride) || database.Props("OM").Field<bool>("xapOMIncludeOrderLineInJob");
		short num = database.Props("OM").Field<byte>("xapOMOrderLineDigits");
		short num2 = database.Props("OM").Field<byte>("xapOMOrderDeliveryDigits");
		bool num3 = database.Props("OM").Field<bool>("xapOMIncludeOrderDeliveryInJob");
		short num4 = 0;
		string text = orderID.Trim();
		string empty = string.Empty;
		if (flag)
		{
			short num5 = ((lineID.ToSql().Length <= num) ? num : Convert.ToInt16(lineID.ToSql().Length));
			text = text + "-" + lineID.ToSql().PadLeft(9, '0').Substring(lineID.ToSql().PadLeft(9, '0').Length - num5, num5);
		}
		if (num3)
		{
			num4 = 1;
			short num5 = ((num4.ToSql().Length <= num2) ? num2 : Convert.ToInt16(num4.ToSql().Length));
			empty = text + "-" + num4.ToSql().PadLeft(9, '0').Substring(num4.ToSql().PadLeft(9, '0').Length - num5, num5);
		}
		else
		{
			empty = text;
		}
		if (!DoesJobExist(database, null, empty))
		{
			result = empty;
		}
		else
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select jmpJobID from Jobs where {fn left(jmpJobID, @BaseJobLength)} = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = text;
			sqlCommand.Parameters.Add(new SqlParameter("@BaseJobLength", SqlDbType.Int)).Value = text.Length;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					_ = row;
					num4++;
					short num5 = ((num4.ToSql().Length <= num2) ? num2 : Convert.ToInt16(num4.ToSql().Length));
					empty = text + "-" + num4.ToSql().PadLeft(9, '0').Substring(num4.ToSql().PadLeft(9, '0').Length - num5, num5);
					if (dataTable.Select("jmpJobID = " + empty.ToLinq()).Length == 0)
					{
						result = empty;
						break;
					}
				}
			}
		}
		return result;
	}

	public int GetJobMaterialLeadTime(M1Database database, string jobID, int jobAssemblyID, int jobMaterialID, double nQty)
	{
		int num = 0;
		double num2 = 0.0;
		string empty = string.Empty;
		string empty2 = string.Empty;
		StringBuilder stringBuilder = new StringBuilder("SELECT jmmLeadTime,jmmQuantityBreak1,jmmLeadTime1,jmmQuantityBreak2,jmmLeadTime2,jmmQuantityBreak3,");
		stringBuilder.Append("jmmLeadTime3,jmmQuantityBreak4,jmmLeadTime4,jmmQuantityBreak5,jmmLeadTime5,jmmQuantityBreak6,jmmLeadTime6,jmmQuantityBreak7,");
		stringBuilder.Append("jmmLeadTime7,jmmQuantityBreak8,jmmLeadTime8,jmmQuantityBreak9,jmmUnitCost9,jmmLeadTime9 ");
		stringBuilder.Append("FROM JobMaterials WHERE jmmJobID = @jobID And jmmJobAssemblyID = @jobAssemblyID And jmmJobMaterialID = @jobMaterialID");
		SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString().Trim());
		sqlCommand.Parameters.AddWithValue("@jobID", jobID);
		sqlCommand.Parameters.AddWithValue("@jobAssemblyID", jobAssemblyID);
		sqlCommand.Parameters.AddWithValue("@jobMaterialID", jobMaterialID);
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			num = dataTable.Rows[0].Field<int>("jmmLeadTime");
			num2 = 0.0;
			for (int i = 1; i <= 9; i++)
			{
				empty = "jmmQuantityBreak" + i.ToString().Trim();
				empty2 = "jmmLeadTime" + i.ToString().Trim();
				if (!(Convert.ToDouble(row.Field<double>(empty)) <= nQty) || (!(Convert.ToDouble(row.Field<double>(empty)) >= num2) && num2 != 0.0))
				{
					break;
				}
				num2 = Convert.ToDouble(row.Field<double>(empty));
				num = Convert.ToInt32(row.Field<int>(empty2));
			}
			if (num == 0)
			{
				num = Convert.ToInt32(row.Field<int>("jmmLeadTime"));
			}
		}
		return num;
	}

	public void UpdateQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (!row.Table.Columns.Contains("ReceivedComplete") || !row.Table.Columns.Contains("ReturnQty") || !row.Table.Columns.Contains("OpenQty") || !row.Table.Columns.Contains("IssueQty") || !row.Table.Columns.Contains("ScrapQty") || !row.Table.Columns.Contains("FieldSelected"))
		{
			return;
		}
		if (!row.Field<bool>("ReturnQty"))
		{
			flag = ((!changedField.Equals("FieldSelected")) ? row.Field<bool>("ReceivedComplete") : (row.Field<bool>("FieldSelected") ? true : false));
			num = row.Field<decimal>("OpenQty");
			if (flag)
			{
				if (row.Field<decimal>("IssueQty") == 0m && row.Field<decimal>("ScrapQty") == 0m)
				{
					row.SetField("IssueQty", num);
				}
			}
			else if (changedField.Equals("FieldSelected"))
			{
				row.SetField("IssueQty", 0m);
				row.SetField("ScrapQty", 0m);
			}
			row.SetField("ReceivedComplete", flag);
		}
		else
		{
			UpdateReturnQuantitiesInGrid(row, changedField);
		}
	}

	public void UpdateReturnQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (row.Table.Columns.Contains("ReceivedComplete") && row.Table.Columns.Contains("ReturnQty") && row.Table.Columns.Contains("jmmQuantityReceived"))
		{
			flag = ((!changedField.Equals("ReturnQty")) ? row.Field<bool>("ReturnQty") : (row.Field<bool>("ReturnQty") ? true : false));
			num = row.Field<decimal>("jmmQuantityReceived");
			if (flag)
			{
				row.SetField("IssueQty", num);
			}
			row.SetField("ReceivedComplete", value: false);
		}
	}

	public void UnscheduleJob(M1Database database, string jobID)
	{
		UnscheduleJob(database, jobID, null);
	}

	public void UnscheduleJob(M1Database database, string jobID, SqlTransaction transaction)
	{
		if (string.IsNullOrWhiteSpace(jobID))
		{
			throw new M1MissingOrInvalidDataException("Job ID is required.");
		}
		Guid? guid = null;
		SqlCommand sqlCommand = database.NewSqlCommand("Select jmpUniqueID From Jobs Where jmpJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj != DBNull.Value && obj != null)
		{
			guid = (Guid)obj;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("UPDATE JobOperations SET jmoStartDate = Null, jmoStartHour = 0, jmoDueDate = Null, jmoDueHour = 0 WHERE jmoJobID = @JobID And jmoProductionComplete = 0 ");
		sqlCommand2.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			database.ExecuteCommand(sqlCommand2, transaction);
			sqlCommand2.CommandText = "UPDATE JobMaterials SET jmmRequiredDate = Null, jmmOrderByDate = Null WHERE jmmJobID = @JobID And jmmReceivedComplete = 0 ";
			database.ExecuteCommand(sqlCommand2, transaction);
			sqlCommand2.CommandText = "UPDATE JobAssemblies SET jmaScheduledStartDate = Null, jmaScheduledStartHour = 0, jmaScheduledDueDate = Null, jmaScheduledDueHour = 0 WHERE jmaJobID = @JobID And jmaProductionComplete = 0 ";
			database.ExecuteCommand(sqlCommand2, transaction);
			sqlCommand2.CommandText = "Update Jobs Set jmpScheduleComplete = 0, jmpScheduledStartDate = Null, jmpScheduledDueDate = Null, jmpScheduledDueHour = 0 Where jmpJobID = @JobID";
			database.ExecuteCommand(sqlCommand2, transaction);
			if (guid.HasValue)
			{
				ScheduleProcess.DeleteSchedule(database, "Jobs", guid.Value, transaction);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public void UnscheduleAssembly(M1Database database, string jobId, int assemblyId, SqlTransaction sqlTransaction)
	{
		if (string.IsNullOrWhiteSpace(jobId))
		{
			throw new M1MissingOrInvalidDataException("Job ID is required.");
		}
		DataTable assembliesFromJob = ScheduleOperators.GetAssembliesFromJob(jobId, database, sqlTransaction);
		ScheduleOperators.UpdateJobOperationDates(database, sqlTransaction, jobId, assemblyId);
		ScheduleOperators.UpdateJobMaterialDates(database, sqlTransaction, jobId, assemblyId);
		ScheduleOperators.UpdateJobAssemblyDates(database, sqlTransaction, jobId, assemblyId);
		ScheduleOperators.UpdateScheduleDatesForSubAssemblies(database, sqlTransaction, jobId, assemblyId);
		if (assembliesFromJob.Rows.Count == 2)
		{
			string queryString = "UPDATE Jobs SET jmpScheduledStartDate = NULL, jmpScheduledDueDate = NULL, jmpScheduledStartHour = 0, jmpScheduledDueHour = 0 WHERE jmpJobID = " + jobId.ToSql();
			database.ExecuteCommand(queryString, sqlTransaction);
			ScheduleOperators.DeleteJobOnScheduleTables(jobId, database, sqlTransaction, includeMasterScenario: true);
		}
		if (assembliesFromJob.Rows.Count > 2)
		{
			ScheduleOperators.DeleteAssemblyWithSubAssembliesOnScheduleTables(jobId, assemblyId, database, sqlTransaction, includeMasterScenario: true);
		}
	}
}
