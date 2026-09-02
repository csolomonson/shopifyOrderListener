using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;

namespace M1.Ax.Erp;

public class Project
{
	public ProjectCosts ProjectCosts(M1Database database, string projectID, string projectAreaID)
	{
		if (!string.IsNullOrWhiteSpace(projectID))
		{
			bool flag = database.Props("AP").Field<bool>("xafAPUpdateJobCosts");
			byte b = database.Props("PN").Field<byte>("xapIMCostingMethod");
			string text = string.Empty;
			if (database.Props("PR").Field<bool>("xapPRUseFirmQuotesOnly"))
			{
				text = " AND qmlFirm = 1";
			}
			bool flag2 = !string.IsNullOrWhiteSpace(projectAreaID);
			SqlCommand sqlCommand = ((!flag) ? database.NewSqlCommand("Select (ISNULL((SELECT SUM((intUnitOverheadCost+intUnitLaborCost+intUnitMaterialCost+intUnitSubcontractCost+intUnitDutyCost+intUnitFreightCost+intUnitMiscCost)*((Case When imtSource = 3 Then -1 Else 1 End)*intQuantity)) FROM PartTransactions Inner Join PartTransactionCosts On imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID, ProductionProperties WHERE imtJobID=jmpJobID AND (imtSource = 3 OR imtSource = 2) AND imtNonInventoryTransaction = 0 AND imtJobType in (1,3) And (imtNonNettable = 0 Or (imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts,0) = 0)) And intCostType = @CostingMethod),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobAsmQuantityReceived+rmmJobMatQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND (rmmJobType = 1 Or rmmJobType = 3)),0) +  ISNULL((SELECT SUM(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlPurchaseOrderID <> '' AND rmlJobID=jmpJobID AND rmlJobType = 1),0) +  ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlJobID=jmpJobID AND rmlJobType = 1),0)) AS ACTMATCOST From Jobs Where jmpProjectID = @ProjectID" + (flag2 ? " AND jmpProjectAreaID = @ProjectAreaID" : string.Empty)) : database.NewSqlCommand("Select (ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN ( select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines WHERE aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID and aplJobType = 1) AND rmlJobID=jmpJobID AND rmlJobType = 1),0) +  ISNULL((SELECT SUM((intUnitOverheadCost+intUnitLaborCost+intUnitMaterialCost+intUnitSubcontractCost+intUnitDutyCost+intUnitFreightCost+intUnitMiscCost)*((Case When imtSource = 3 Then -1 Else 1 End)*intQuantity)) FROM PartTransactions Inner Join PartTransactionCosts On imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID, ProductionProperties WHERE imtJobID=jmpJobID AND (imtSource = 3 OR imtSource = 2) AND imtNonInventoryTransaction = 0 AND imtJobType in (1,3) AND (imtNonNettable = 0 Or (imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts,0) = 0)) And intCostType = @CostingMethod AND imtReceiptID = '' AND Upper(imtTableName) NOT IN ('RECEIPTLINES','MFGRECEIPTS','MFGRECEIPTCOMPONENTS','RECEIPTCOMPONENTS')),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobAsmQuantityReceived+rmmJobMatQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND (rmmJobType = 1 Or rmmJobType = 3)),0) +  ISNULL((SELECT SUM(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived+ IsNull(qalJobMatQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlPurchaseOrderID <> '' AND rmlPurchaseOrderID NOT IN ( Select pmlPurchaseOrderID From APInvoiceExpenseAccounts Inner Join LandedCostChargeDetails on rmiUniqueID = apxSourceTableUniqueID Inner Join LandedCostCharges on rmiLandedCostID = rmhLandedCostID and rmiLandedCostChargeID = rmhLandedCostChargeID Inner Join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID Where pmlJobID = jmpJobID and pmlJobType in (1,3) And rmhAPInvoiceID <> '') AND rmlJobID =jmpJobID AND rmlJobType In (1,3)),0) +  ISNULL((SELECT SUM(apxAmount) From APInvoiceExpenseAccounts Inner Join LandedCostChargeDetails on rmiUniqueID = apxSourceTableUniqueID Inner Join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID Where pmlJobID=jmpJobID And pmlJobType = 1),0) +  ISNULL((SELECT SUM(aplExtendedCostBase) FROM APInvoiceLines WHERE aplJobID=jmpJobID AND aplJobType = 1),0)) as ACTMATCOST From Jobs Where jmpProjectID = @ProjectID" + (flag2 ? " AND jmpProjectAreaID = @ProjectAreaID" : string.Empty)));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			sqlCommand.Parameters.Add(new SqlParameter("@CostingMethod", SqlDbType.TinyInt)).Value = ((b == 4 || b == 5) ? 4 : b);
			DataTable dataTable = database.GetDataTable(sqlCommand);
			sqlCommand = ((!flag) ? database.NewSqlCommand("Select(ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobOprQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND rmmJobType = 2),0) +  ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobOprQuantityReceived+ IsNull(qalJobOprQuantityAccepted,0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE rmlJobID=jmpJobID AND rmlJobType = 2),0)) AS ACTCONTRACTCOST From Jobs Where jmpProjectID = @ProjectID" + (flag2 ? " AND jmpProjectAreaID = @ProjectAreaID" : string.Empty)) : database.NewSqlCommand("Select(ISNULL((SELECT SUM((rmlInventoryUnitCost * (rmlJobOprQuantityReceived + IsNull(qalJobOprQuantityAccepted, 0))) + rmlSetupCharge) FROM ReceiptLines LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN ( select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines WHERE aplPurchaseOrderID = rmlPurchaseOrderID AND aplPurchaseOrderLineID = rmlPurchaseOrderLineID and aplJobType = 2) AND rmlJobID=jmpJobID AND rmlJobType = 2),0) +  ISNULL((SELECT SUM(((rmmUnitOverheadCost+rmmUnitLaborCost+rmmUnitMaterialCost+rmmUnitSubcontractCost)*(rmmScrapQuantity+rmmJobOprQuantityReceived)) + rmmSetupCharge) FROM MfgReceipts WHERE rmmJobID = jmpJobID AND rmmReceiptType = 1 AND rmmJobType = 2),0) +  ISNULL((SELECT SUM(aplExtendedCostBase) FROM APInvoiceLines WHERE aplJobID=jmpJobID AND aplJobType = 2),0)) AS ACTCONTRACTCOST From Jobs Where jmpProjectID = @ProjectID" + (flag2 ? " AND jmpProjectAreaID = @ProjectAreaID" : string.Empty)));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("SELECT  isnull(SUM(case when jmoOperationType = 1 then jmoSetupHours*jmoSetupRate else 0 end), 0) AS ESTJOBSETUPLABCOST,  isnull(SUM(case when jmoOperationType = 1 then jmoEstimatedProductionHours*jmoProductionRate else 0 end), 0) AS ESTJOBPRODLABCOST,  isnull(SUM(case when jmoOperationType = 1 then jmoOverheadRate*(jmoEstimatedProductionHours+jmoSetupHours) else 0 end), 0) AS ESTJOBBURDENCOST,  isnull(SUM(case when jmoOperationType = 2 then jmoCalculatedUnitCost*jmoOperationQuantity else 0 end), 0) AS ESTJOBCONTRACTCOST,  isnull(SUM(case when jmoOperationType = 1 then jmoSetupHours else 0 end), 0) AS ESTJOBSETUPHOURS,  isnull(SUM(case when jmoOperationType = 1 then jmoEstimatedProductionHours else 0 end), 0) AS ESTJOBPRODHOURS  FROM JobOperations inner join jobs on jmpjobid = jmojobid WHERE jmpProjectID = @ProjectID" + (flag2 ? " AND jmpProjectAreaID = @ProjectAreaID" : string.Empty));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable3 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("select isnull(sum(jmmCalculatedUnitCost*jmmEstimatedQuantity),0) as estJobMatCost FROM JobMaterials inner join jobs on jmpjobid = jmmjobid where jmpProjectid = @ProjectID" + (flag2 ? " AND jmpProjectAreaID = @ProjectAreaID" : string.Empty));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable4 = database.GetDataTable(sqlCommand);
			byte b2 = database.Props("PR").Field<byte>("xapPRLaborMethod");
			sqlCommand = database.NewSqlCommand("SELECT  isnull(SUM(qmqMaterialCost), 0) AS ESTQUOTEMATCOST,  isnull(SUM(" + ((b2 == 2) ? "qmqLaborCost" : "qmqQuotingCost") + "), 0) AS ESTQUOTEPRODLABCOST,  isnull(" + ((b2 == 2) ? "SUM(qmqOverHeadCost)" : "0.00") + ", 0) AS ESTQUOTEBURDENCOST,  isnull(SUM(qmqSubContractCost), 0) AS ESTQUOTECONTRACTCOST,  isnull(SUM(qmqSetupHours), 0) AS ESTQUOTESETUPHOURS,  isnull(Sum(qmqProductionHours), 0) As ESTQUOTEPRODHOURS  FROM QuoteQuantities inner join quotelines on qmlquoteid = qmqquoteid and qmlquotelineid = qmqquotelineid  WHERE qmlProjectID = @ProjectID" + text + (flag2 ? " AND qmlProjectAreaID = @ProjectAreaID" : string.Empty));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable5 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("SELECT Isnull(SUM(qmqQuoteQuantity * qmlPurchaseUnitCostBase),0) as EstPurchaseToOrder FROM QuoteLines INNER JOIN QuoteQuantities ON qmqQuoteID = qmlQuoteID AND qmqQuoteLineID = qmlQuoteLineID WHERE qmlprojectid = @ProjectID AND qmlPurchaseToOrder = 1" + text + (flag2 ? " AND qmlProjectAreaID = @ProjectAreaID" : string.Empty));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable6 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("SELECT  isnull(SUM(case when lmlWorkType=1 then lmlLaborCost else 0 end), 0) AS ACTSETUPLABCOST,  isnull(SUM(case when lmlWorkType=2 then lmlLaborCost else 0 end), 0) AS ACTPRODLABCOST,  isnull(SUM(case when lmlCompletionType = 3 Or lmlCompletionType = 4 then lmlLaborCost else 0 end), 0) AS REWORKLABCOST,  isnull(SUM(CASE lmlGoodQuantity WHEN 0 THEN 0 ELSE (lmlScrapQuantity/lmlGoodQuantity) * lmlLaborCost END ), 0) AS SCRAPLABCOST,  isnull(SUM(lmlOverheadCost), 0) AS ACTBURDENCOST,  isnull(SUM(case when lmlTimecardType = 1 AND lmlWorkType = 1 and (lmlcompletiontype <> 3 or lmlCompletionType <> 4) then lmlLaborHours else 0 end), 0) AS ACTSETUPHOURS,  isnull(SUM(case when lmlWorkType = 2 and (lmlcompletiontype <> 3 or lmlCompletionType <> 4) then lmlLaborHours else 0 end), 0) AS ACTPRODHOURS,  isnull(SUM(case when lmlCompletionType = 3 Or lmlCompletionType = 4 then lmlLaborHours else 0 end), 0) AS ACTREWORKHOURS  FROM TimecardLines WHERE lmlProjectID = @ProjectID" + (flag2 ? " AND lmlProjectAreaID = @ProjectAreaID" : string.Empty));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable7 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("SELECT Isnull(SUM(omdPurchaseUnitCostBase * omdDeliveryQuantity),0) AS OrderPurchaseToOrder FROM salesorderlines INNER JOIN SALESORDERDELIVERIES ON omlSalesOrderID = omdSalesOrderID AND omlSalesOrderLineID = omdSalesOrderLineID WHERE omdDeliveryType = 5 AND omlProjectID = @ProjectID" + (flag2 ? " AND omlProjectAreaID = @ProjectAreaID" : string.Empty));
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			if (flag2)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			}
			DataTable dataTable8 = database.GetDataTable(sqlCommand);
			ProjectCosts projectCosts = new ProjectCosts();
			if (flag)
			{
				sqlCommand = database.NewSqlCommand("SELECT ISNULL(SUM(rmlInventoryUnitCost * rmlInventoryQuantityReceived),0) as ActualPurchaseToOrderPO FROM ReceiptLines INNER JOIN PurchaseOrderLines ON rmlPurchaseOrderID=pmlPurchaseOrderID AND rmlPurchaseOrderLineID=pmlPurchaseOrderLineID WHERE rmlReceiptID NOT IN ( select aplReceiptID FROM APInvoiceLines WHERE aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID) AND pmlPurchaseType = 3 AND rmlProjectID = @ProjectID" + (flag2 ? " AND rmlProjectAreaID = @ProjectAreaID" : string.Empty));
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
				if (flag2)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
				}
				DataTable dataTable9 = database.GetDataTable(sqlCommand);
				sqlCommand = database.NewSqlCommand("SELECT ISNULL(SUM(aplExtendedCostForeign),0) as ActualPurchaseToOrderAP FROM APInvoiceLines INNER JOIN PurchaseOrderLines ON aplPurchaseOrderID=pmlPurchaseOrderID AND aplPurchaseOrderLineID=pmlPurchaseOrderLineID WHERE pmlPurchaseType = 3 AND aplProjectID = @ProjectID" + (flag2 ? " AND aplProjectAreaID = @ProjectAreaID" : string.Empty));
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
				if (flag2)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
				}
				DataTable dataTable10 = database.GetDataTable(sqlCommand);
				projectCosts.ActPurchaseToOrder = dataTable9.Rows[0].Field<decimal>("ActualPurchaseToOrderPO") + dataTable10.Rows[0].Field<decimal>("ActualPurchaseToOrderAP");
			}
			else
			{
				sqlCommand = database.NewSqlCommand("SELECT ISNULL(SUM(rmlInventoryUnitCost * rmlInventoryQuantityReceived),0) as ActualPurchaseToOrderPO FROM ReceiptLines INNER JOIN PurchaseOrderLines ON rmlPurchaseOrderID=pmlPurchaseOrderID AND rmlPurchaseOrderLineID=pmlPurchaseOrderLineID WHERE pmlPurchaseType = 3 AND rmlProjectID = @ProjectID" + (flag2 ? " AND rmlProjectAreaID = @ProjectAreaID" : string.Empty));
				sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
				if (flag2)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
				}
				DataTable dataTable11 = database.GetDataTable(sqlCommand);
				projectCosts.ActPurchaseToOrder = dataTable11.Rows[0].Field<decimal>("ActualPurchaseToOrderPO");
			}
			projectCosts.EstJobMaterialCost = dataTable4.Rows[0].Field<decimal>("EstJobMatCost");
			projectCosts.EstQuoteMaterialCost = dataTable5.Rows[0].Field<decimal>("EstQuoteMatCost");
			projectCosts.ActMaterialCost = ((dataTable.Rows.Count == 0) ? 0m : dataTable.Select().Sum((DataRow m) => m.Field<decimal>("ActMatCost")));
			projectCosts.EstJobLaborCost = dataTable3.Rows[0].Field<decimal>("EstJobProdLabCost") + dataTable3.Rows[0].Field<decimal>("EstJobSetupLabCost");
			projectCosts.EstQuoteLaborCost = dataTable5.Rows[0].Field<decimal>("EstQuoteProdLabCost");
			projectCosts.ActLaborCost = dataTable7.Rows[0].Field<decimal>("ActProdLabCost") + dataTable7.Rows[0].Field<decimal>("ScrapLabCost") + dataTable7.Rows[0].Field<decimal>("ReworkLabCost") + dataTable7.Rows[0].Field<decimal>("ActSetupLabCost");
			projectCosts.EstJobSubCCost = dataTable3.Rows[0].Field<decimal>("EstJobContractCost");
			projectCosts.EstQuoteSubCCost = dataTable5.Rows[0].Field<decimal>("EstQuoteContractCost");
			projectCosts.ActSubCCost = ((dataTable2.Rows.Count == 0) ? 0m : dataTable2.Select().Sum((DataRow m) => m.Field<decimal>("ActContractCost")));
			projectCosts.EstJobOverheadCost = dataTable3.Rows[0].Field<decimal>("EstJobBurdenCost");
			projectCosts.EstQuoteOverheadCost = dataTable5.Rows[0].Field<decimal>("EstQuoteBurdenCost");
			projectCosts.ActOverheadCost = dataTable7.Rows[0].Field<decimal>("ActBurdenCost");
			projectCosts.EstJobSetupHours = dataTable3.Rows[0].Field<decimal>("EstJobSetupHours");
			projectCosts.EstQuoteSetupHours = dataTable5.Rows[0].Field<decimal>("EstQuoteSetupHours");
			projectCosts.ActSetupHours = dataTable7.Rows[0].Field<decimal>("ActSetupHours");
			projectCosts.EstJobProdHours = dataTable3.Rows[0].Field<decimal>("EstJobProdHours");
			projectCosts.EstQuoteProdHours = dataTable5.Rows[0].Field<decimal>("EstQuoteProdHours");
			projectCosts.ActProdHours = dataTable7.Rows[0].Field<decimal>("ActProdHours");
			projectCosts.ActReworkHours = dataTable7.Rows[0].Field<decimal>("ActReworkHours");
			projectCosts.EstPurchaseToOrder = dataTable6.Rows[0].Field<decimal>("EstPurchaseToOrder");
			projectCosts.JobPurchaseToOrder = dataTable8.Rows[0].Field<decimal>("OrderPurchaseToOrder");
			return projectCosts;
		}
		return null;
	}

	public ProjectTotals ProjectTotals(M1Database database, string projectID)
	{
		if (!string.IsNullOrWhiteSpace(projectID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select isnull((select sum(ompOrderTotalBase) from salesorders where ompProjectID = prpprojectid),0) as ompOrderTotalBase,  ISNULL((Select sum(arpinvoicesubtotalbase) from Arinvoices where arpprojectid = prpprojectid),0) as arpinvoicesubtotalbase, ISNULL((Select sum(arpinvoicetaxamountbase) from Arinvoices where arpprojectid = prpprojectid),0) as arpinvoicetaxamountbase, ISNULL((Select sum(arpinvoicetotalbase) from Arinvoices where arpprojectid = prpprojectid),0) as arpinvoicetotalbase, ISNULL((Select sum(arpfreighttotalbase) from Arinvoices where arpprojectid = prpprojectid),0) as arpFreighttotalbase from projects where prpprojectid = @ProjectID");
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				return new ProjectTotals
				{
					ProjectValue = row.Field<decimal>("ompOrderTotalBase"),
					InvoiceSubTotal = row.Field<decimal>("arpInvoiceSubTotalBase"),
					InvoiceTaxAmount = row.Field<decimal>("arpInvoiceTaxAmountBase"),
					InvoiceTotal = row.Field<decimal>("arpInvoiceTotalBase"),
					FreightAmount = row.Field<decimal>("arpFreightTotalBase")
				};
			}
		}
		return null;
	}

	public ProjectTotals ProjectAreaTotals(M1Database database, string projectID, string projectAreaID)
	{
		if (!string.IsNullOrWhiteSpace(projectID) && !string.IsNullOrWhiteSpace(projectAreaID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select isnull((select sum(omlExtendedPriceBase+omlFreightAmountBase+omlTaxAmountBase+omlSecondTaxAmountBase) from SalesOrderLines where omlProjectID = praProjectID and omlProjectAreaID = praProjectAreaID),0) as omlExtendedPriceBase,  ISNULL((Select sum(arlExtendedPriceBase) from ArinvoiceLines where arlProjectID = praProjectID and arlProjectAreaID = praProjectAreaID),0) as arlExtendedPriceBase, ISNULL((Select sum(arlTaxAmountBase+arlSecondTaxAmountBase) from ArinvoiceLines where arlProjectID = praProjectID and arlProjectAreaID = praProjectAreaID),0) as arlTaxAmountBase, ISNULL((Select sum(arlExtendedPriceBase+arlTaxAmountBase+arlSecondTaxAmountBase+arlFreightAmountBase) from ArinvoiceLines where arlProjectID = praProjectID and arlProjectAreaID = praProjectAreaID),0) as arlFullExtendedPriceBase, ISNULL((Select sum(arlFreightAmountBase) from ArinvoiceLines where arlProjectID = praProjectID and arlProjectAreaID = praProjectAreaID),0) as arlFreightAmountBase from ProjectAreas where praProjectID = @ProjectID and praProjectAreaID = @ProjectAreaID");
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.NVarChar)).Value = projectID;
			sqlCommand.Parameters.Add(new SqlParameter("@ProjectAreaID", SqlDbType.NVarChar)).Value = projectAreaID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				return new ProjectTotals
				{
					ProjectValue = row.Field<decimal>("omlExtendedPriceBase"),
					InvoiceSubTotal = row.Field<decimal>("arlExtendedPriceBase"),
					InvoiceTaxAmount = row.Field<decimal>("arlTaxAmountBase"),
					InvoiceTotal = row.Field<decimal>("arlFullExtendedPriceBase"),
					FreightAmount = row.Field<decimal>("arlFreightAmountBase")
				};
			}
		}
		return null;
	}
}
