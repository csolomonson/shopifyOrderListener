using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class PurchasePlanner
{
	public bool performExtendedOverlapCheck { get; set; }

	public void Clear(M1Database database, string sessionId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Delete From PurchasePlannerLines Where pplSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From PurchasePlannerRequirements Where pprSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From PurchasePlannerOrderDetails Where ppoSessionID = @SessionID");
		stringBuilder.AppendLine("Update PurchasePlannerSessions set ppsSessionSubtotalBase = 0 Where ppsSessionID = @SessionID");
		string queryString = stringBuilder.ToString();
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		database.ExecuteCommand(sqlCommand);
		sqlCommand = database.NewSqlCommand("Update PurchasePlannerSessions Set ppsGenerated = 0 Where ppsSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		database.ExecuteCommand(sqlCommand);
	}

	private void CreatePartRevisionsTemporaryTable(M1Database database, string sessionId, string suppliers)
	{
		string queryString = "SELECT imrPartID as PartId, imrPartRevisionID as PartRevId INTO PurchasePlannerPartRevisionSuppliersList" + sessionId + " FROM PartRevisions WHERE imrSupplierOrganizationID IN (" + suppliers + ") ";
		try
		{
			database.ExecuteCommand(queryString);
		}
		catch (Exception)
		{
			database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerPartRevisionSuppliersList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerPartRevisionSuppliersList" + sessionId + " END");
		}
	}

	private void CreateOrderDeliveriesTemporaryTable(M1Database database, string sessionId, string suppliers)
	{
		string queryString = "SELECT omdPartID as PartId, omdPartRevisionID as PartRevId INTO PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + " FROM SalesOrderDeliveries WHERE omdSupplierOrganizationID IN (" + suppliers + ") ";
		try
		{
			database.ExecuteCommand(queryString);
		}
		catch (Exception)
		{
			database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + " END");
		}
	}

	public bool Generate(M1Database database, string sessionId, int lineId)
	{
		Cursor.Current = Cursors.WaitCursor;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		string s = string.Empty;
		string s2 = string.Empty;
		string s3 = string.Empty;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string empty = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		string empty2 = string.Empty;
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(ppsPlantID,'') as ppsPlantID, ppsWarehouseID, ppsPartClassIDs, ppsSupplierIDs, ppsCompletedDate, ppsCompleted, ppsCutoffDate, IsNull(ppsCutoffDatePOSupply, ppsCutoffDate) As ppsCutoffDatePOSupply, ppsCalculateForAllParts, ppsJobIDs, ppsSalesOrderIDs, ppsGenerated, ppsFirmOnly, ppsPartIDs, ppsShowAllDemandForPartsOnJobs From PurchasePlannerSessions Where ppsSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			string text4 = splitAndConvert(dataRow.Field<string>("ppsPartClassIDs"));
			string text5 = splitAndConvert(dataRow.Field<string>("ppsJobIDs"));
			string text6 = splitAndConvert(dataRow.Field<string>("ppsSalesOrderIDs"));
			string text7 = splitAndConvert(dataRow.Field<string>("ppsSupplierIDs"));
			bool flag6 = dataRow.Field<bool>("ppsCalculateForAllParts");
			string text8 = (dataRow.Field<bool>("ppsFirmOnly") ? M1Util.ConvertToSql(dataRow.Field<bool>("ppsFirmOnly")) : string.Empty);
			string text9 = splitAndConvert(dataRow.Field<string>("ppsPartIDs"));
			bool flag7 = dataRow.Field<bool>("ppsShowAllDemandForPartsOnJobs");
			if (flag7)
			{
				text9 = GetPartsFromJobs(database, text5, sessionId);
			}
			if (lineId != 0)
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Select pplPartID, pplPartRevisionID, pplPartShortDescription, pplPlantID, pplWarehouseID From PurchasePlannerLines Where pplSessionID = @SessionID and pplLineID = @LineID");
				sqlCommand2.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
				sqlCommand2.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
				DataTable dataTable2 = database.GetDataTable(sqlCommand2);
				if (dataTable2.Rows.Count != 0)
				{
					DataRow row = dataTable2.Rows[0];
					s = row.Field<string>("pplPartID");
					s2 = row.Field<string>("pplPartRevisionID");
					s3 = row.Field<string>("pplPartShortDescription");
					text = row.Field<string>("pplPlantID");
					text2 = row.Field<string>("pplWarehouseID");
				}
			}
			if (!string.IsNullOrWhiteSpace(text5))
			{
				flag2 = true;
			}
			if (!string.IsNullOrWhiteSpace(text6))
			{
				flag3 = true;
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				CreatePartRevisionsTemporaryTable(database, sessionId, text7);
				CreateOrderDeliveriesTemporaryTable(database, sessionId, text7);
				flag4 = true;
			}
			if (!string.IsNullOrWhiteSpace(text9) && !flag7)
			{
				flag5 = true;
			}
			if (lineId == 0)
			{
				if (!flag2 && string.IsNullOrWhiteSpace(text4) && !flag4 && !flag5 && !flag3)
				{
					stringBuilder.AppendLine("Declare @string nvarchar(255) ");
					stringBuilder.AppendLine("Declare @sessionID nvarchar(10) ");
					stringBuilder.AppendLine("SET @sessionID = " + sessionId.ToSql());
					if (dataRow.Field<bool>("ppsCalculateForAllParts"))
					{
						stringBuilder.AppendLine("Select @string = coalesce(@string + ',', '') + ppsSessionID from purchaseplannersessions where PurchasePlannerSessions.ppsCompleted = 0 ");
						stringBuilder.AppendLine(" and PurchasePlannerSessions.ppsSessionID <> @sessionID group by PurchasePlannerSessions.ppsSessionID order by PurchasePlannerSessions.ppsSessionID ");
					}
					else
					{
						stringBuilder.AppendLine("Select @string = coalesce(@string + ',', '') + ppsSessionID from purchaseplannersessions where (ppsCalculateForAllParts <> 0 ");
						if (dataRow.Field<string>("ppsWarehouseID").Trim().ToString() == empty2)
						{
							stringBuilder.AppendLine(" OR (ppsPlantID = " + dataRow.Field<string>("ppsPlantID").Trim().ToSql() + " AND (ppsWarehouseID = " + empty2.ToSql() + " OR ppsWarehouseID = " + dataRow.Field<string>("ppsWarehouseID").Trim().ToSql() + ")))");
						}
						else
						{
							stringBuilder.AppendLine(" OR (ppsPlantID = " + dataRow.Field<string>("ppsPlantID").Trim().ToSql() + " AND (ppsWarehouseID = " + dataRow.Field<string>("ppsWarehouseID").Trim().ToSql() + ")))");
						}
						stringBuilder.AppendLine(" and PurchasePlannerSessions.ppsCompleted = 0 ");
						stringBuilder.AppendLine(" and PurchasePlannerSessions.ppsSessionID <> @sessionID group by PurchasePlannerSessions.ppsSessionID order by PurchasePlannerSessions.ppsSessionID ");
					}
					stringBuilder.AppendLine("Select @string");
					text3 = stringBuilder.ToString();
					stringBuilder.Clear();
					empty = Convert.ToString(database.ExecuteScalar(text3));
					if (empty != string.Empty)
					{
						MessageBox.Show("The Warehouse/Plant filter criteria selected in this session overlaps with other open Purchase Planner session(s). \n\nAs a result, there will be overlap with the following open session(s): \n" + empty + "\n\nThe open session(s) should be completed first.  Otherwise, the filter criteria for the current session must be changed to avoid the overlap.  [Msg 1]", "Filter criteria overlaps with another open session", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						flag = false;
						Cursor.Current = Cursors.Arrow;
						return flag;
					}
				}
				if (!string.IsNullOrWhiteSpace(text4) && !checkFilterOverlap(stringBuilder, text3, sqlCommand, database, dataRow, sessionId, "ppsPartClassIDs", "Part Classes"))
				{
					return false;
				}
				if (flag2 && !checkFilterOverlap(stringBuilder, text3, sqlCommand, database, dataRow, sessionId, "ppsJobIDs", "Jobs"))
				{
					return false;
				}
				if (flag3 && !checkFilterOverlap(stringBuilder, text3, sqlCommand, database, dataRow, sessionId, "ppsSalesOrderIDs", "SalesOrders"))
				{
					return false;
				}
				if (flag5 && !checkFilterOverlap(stringBuilder, text3, sqlCommand, database, dataRow, sessionId, "ppsPartIDs", "Parts"))
				{
					return false;
				}
				if (flag4 && !checkFilterOverlap(stringBuilder, text3, sqlCommand, database, dataRow, sessionId, "ppsSupplierIDs", "Suppliers"))
				{
					return false;
				}
				if (!flag3 || (flag3 && performExtendedOverlapCheck))
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append(" WHERE 1=1 ");
					stringBuilder2.Append(" And (Select IsNull(Count(*),0) As OpenCount from PurchasePlannerSessions Where ppsCompleted = 0 And ppsSessionID <> " + sessionId.ToSql() + ") > 0 ");
					if (flag2)
					{
						stringBuilder2.Append(" And EXISTS ( Select jmaPartID, jmaPartRevisionID, jmaPartWarehouseLocationID from JobAssemblies left outer join Jobs on jmaJobID = jmpJobID Where jmaJobAssemblyID <> 0 and jmaClosed = 0 AND jmaIssuedComplete = 0 AND jmaQuantityToPull - jmaQuantityIssued > 0 AND ISNULL(jmaScheduledDueDate,jmpProductionDueDate) < DateAdd(d,1," + dataRow.Field<DateTime>("ppsCutoffDate").ToSql() + ") and jmaJobID IN (" + text5 + ") And jmaPartID = imrPartID and jmaPartRevisionID = imrPartRevisionID and jmaPartWareHouseLocationID = imlPartWarehouseID Union All Select jmmPartID, jmmPartRevisionID, jmmPartWarehouseLocationID from JobMaterials left outer join Jobs on jmmJobID = jmpJobID Where jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0 and ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < DateAdd(d,1," + dataRow.Field<DateTime>("ppsCutoffDate").ToSql() + ") and (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPurchaseToJobQuantity / jmmEstimatedQuantity End) > 0 and jmmJobID IN (" + text5 + ") And jmmPartID = imrPartID and jmmPartRevisionID = imrPartRevisionID and jmmPartWareHouseLocationID = imlPartWarehouseID )");
					}
					if (flag3)
					{
						stringBuilder2.Append(" And ( imrPartID+imrPartRevisionID+imlPartWarehouseID IN (Select omdPartID+omdPartRevisionID+omdPartWarehouseLocationID from SalesOrderDeliveries Left Outer Join PartRevisions on omdPartID = imrPartID and omdPartRevisionID = imrPartRevisionID Where omdDeliveryType = 5 AND omdClosed = 0 AND omdShippedComplete = 0 and omdReceivedComplete = 0 and DateAdd(d,ISNULL(imrLeadTime,0)*-1,ISNULL(omdDeliveryDate,'20991231')) < DateAdd(d,1," + dataRow.Field<DateTime>("ppsCutoffDate").ToSql() + ") and omdDeliveryQuantity-omdQuantityShipped > 0 and omdSalesOrderID IN (" + text6 + ") ) ) ");
					}
					if (dataRow.Field<string>("ppsWarehouseID").ToString() != empty2)
					{
						stringBuilder2.Append(" And imlPartWarehouseID = " + dataRow.Field<string>("ppsWarehouseID").ToString().ToSql());
					}
					if (!string.IsNullOrWhiteSpace(text4))
					{
						stringBuilder2.Append(" And impPartClassID IN (" + text4 + ")");
					}
					if (flag4)
					{
						stringBuilder2.Append(" AND ( EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)  OR EXISTS (SELECT jmmPartID, jmmPartRevisionID, jmmPartWarehouseLocationID FROM JobMaterials left outer join Jobs on jmmJobID = jmpJobID WHERE jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity - jmmQuantityReceived <> 0 and ISNULL(ISNULL(jmmOrderByDate, jmmRequiredDate), DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < DateAdd(d, 1, " + dataRow.Field<DateTime>("ppsCutoffDate").ToSql() + ") and (case when jmmEstimatedQuantity = 0 Then 0 Else(jmmEstimatedQuantity - jmmQuantityReceived) * jmmPurchaseToJobQuantity / jmmEstimatedQuantity End) > 0 and jmmSupplierOrganizationID in (" + text7 + ") and jmmPartID = imrPartID and jmmPartRevisionID = imrPartRevisionID and jmmPartWarehouseLocationID = imlPartWarehouseID UNION ALL Select omdPartID, omdPartRevisionID, omdPartWarehouseLocationID from SalesOrderDeliveries Left Outer Join PartRevisions on omdPartID = imrPartID and omdPartRevisionID = imrPartRevisionID Where omdDeliveryType = 5 AND omdClosed = 0 AND omdShippedComplete = 0 and omdReceivedComplete = 0 and DateAdd(d,ISNULL(imrLeadTime,0)*-1,ISNULL(omdDeliveryDate,'20991231')) < DateAdd(d, 1, " + dataRow.Field<DateTime>("ppsCutoffDate").ToSql() + ") and omdDeliveryQuantity - omdQuantityShipped > 0 and omdSupplierOrganizationID in (" + text7 + ") and omdPartID = imrPartID and omdPartRevisionID = imrPartRevisionID and omdPartWarehouseLocationID = imlPartWarehouseID ) )");
					}
					if (flag5)
					{
						stringBuilder2.Append(" And impPartID IN (" + text9 + ")");
					}
					SqlTransaction sqlTransaction = database.BeginTransaction();
					try
					{
						database.ExecuteCommand("SELECT " + sessionId.ToSql() + " As pplSessionID, IDENTITY(int,1,1) As pplLineID, imrPartID As pplPartID,imrPartRevisionID as pplPartRevisionID,imrShortDescription as pplPartShortDescription,imlPartWarehouseID as pplWarehouseID INTO #PurchasePlannerLines FROM ( select TOP 100 PERCENT imrPartID,imrPartRevisionID,imrShortDescription,IsNull(imlPartWarehouseID,'') as imlPartWarehouseID, isNull(impPartClassID,'') as impPartClassID from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartWarehouseLocations on imrPartID=imlPartID And imrPartRevisionID = imlPartRevisionID " + stringBuilder2.ToString() + " ) as test ", sqlTransaction);
						try
						{
							string text10 = string.Empty;
							if (flag2 && !dataRow.Field<bool>("ppsShowAllDemandForPartsOnJobs"))
							{
								text10 = " and ( ppsJobIDs = '' OR ( ppsJobIDs <> '' AND PurchasePlannerSessions.ppsShowAllDemandForPartsOnJobs <> 0 ) ) and PurchasePlannerSessions.ppsSalesOrderIDs = '' ";
							}
							if (flag3)
							{
								text10 = " and ( ppsJobIDs = '' OR ( ppsJobIDs <> '' AND PurchasePlannerSessions.ppsShowAllDemandForPartsOnJobs <> 0 ) )  ";
							}
							string text11 = " and ( PurchasePlannerSessions.ppsPlantID = " + dataRow.Field<string>("ppsPlantID").ToSql() + " or PurchasePlannerSessions.ppsShowAllDemandForPartsOnJobs <> 0 ) ";
							string text12 = (string)database.ExecuteScalar("SELECT IsNull(STUFF((SELECT distinct  ', ' + ppsSessionID FROM #PurchasePlannerLines inner join PurchasePlannerLines on #PurchasePlannerLines.pplPartID = PurchasePlannerLines.pplPartID and #PurchasePlannerLines.pplPartRevisionID = PurchasePlannerLines.pplPartRevisionID and #PurchasePlannerLines.pplWarehouseID = PurchasePlannerLines.pplWarehouseID Inner Join PurchasePlannerSessions on PurchasePlannerLines.pplSessionID = PurchasePlannerSessions.ppsSessionID where PurchasePlannerSessions.ppsSessionID <> " + sessionId.ToSql() + " and PurchasePlannerSessions.ppsCompleted = 0 " + text10 + " " + text11 + " FOR XML PATH('')), 1, 1, ''),'') AS idList", sqlTransaction);
							if (text12 != string.Empty)
							{
								text12 += " ";
								text12 = text12.Replace(", " + sessionId.ToString() + " ", "");
								text12 = text12.Replace(" " + sessionId.ToString() + ", ", " ");
								text12 = text12.Replace(sessionId.ToString() + ", ", "");
								MessageBox.Show("The filter criteria selected in this session overlaps with other open Purchase Planner session(s). \n\nAs a result, there will be overlap with the following open session(s): \n" + text12 + "\n\nThe open session(s) should be completed first.  Otherwise, the filter criteria for the current session must be changed to avoid the overlap.  [Msg 3]", "Filter criteria overlaps with another open session", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								return false;
							}
						}
						finally
						{
							database.ExecuteCommand("DROP TABLE #PurchasePlannerLines", sqlTransaction);
						}
					}
					catch
					{
						database.RollbackTransaction(sqlTransaction);
						throw;
					}
					database.CommitTransaction(sqlTransaction);
				}
			}
			if (lineId == 0)
			{
				stringBuilder.AppendLine("Delete From PurchasePlannerLines Where pplSessionID = @SessionID");
				stringBuilder.AppendLine("Delete From PurchasePlannerRequirements Where pprSessionID = @SessionID");
				stringBuilder.AppendLine("Delete From PurchasePlannerOrderDetails Where ppoSessionID = @SessionID");
			}
			else
			{
				stringBuilder.AppendLine("Delete From PurchasePlannerLines Where pplSessionID = @SessionID and pplLineID = @LineID");
				stringBuilder.AppendLine("Delete From PurchasePlannerRequirements Where pprSessionID = @SessionID and pprLineID = @LineID");
				stringBuilder.AppendLine("Delete From PurchasePlannerOrderDetails Where ppoSessionID = @SessionID and ppoLineID = @LineID");
			}
			string queryString = stringBuilder.ToString();
			stringBuilder.Clear();
			sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
			database.ExecuteCommand(sqlCommand);
			string empty3 = string.Empty;
			string empty4 = string.Empty;
			if (lineId == 0)
			{
				empty3 = dataRow.Field<string>("ppsWarehouseID");
				empty4 = dataRow.Field<string>("ppsPlantID");
			}
			else
			{
				empty3 = text2;
				empty4 = text;
			}
			bool flag8 = empty3.Equals(empty2) || flag7 || flag6;
			DataTable dataTable3 = database.GetDataTable((empty3.Equals(empty2) && (flag6 || flag7)) ? "Select imwWarehouseID From Warehouses" : ("Select imwWarehouseID From Warehouses Where imwPlantID = " + empty4.ToSql()));
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SET NOCOUNT ON ");
			stringBuilder.AppendLine("DECLARE @cPartID nvarchar (30) ");
			stringBuilder.AppendLine("--SET @cPartID = '1000' ");
			stringBuilder.AppendLine("DECLARE @cPartRevisionID nvarchar (15) ");
			stringBuilder.AppendLine("--SET @cPartRevisionID = 'A' ");
			stringBuilder.AppendLine("DECLARE @nPartTypeRangeStart int ");
			stringBuilder.AppendLine("SET @nPartTypeRangeStart = 1 ");
			stringBuilder.AppendLine("DECLARE @nPartTypeRangeEnd int ");
			stringBuilder.AppendLine("SET @nPartTypeRangeEnd = 1 ");
			stringBuilder.AppendLine("DECLARE @nCostMethod int ");
			stringBuilder.AppendLine("SET @nCostMethod = (Select xapPMCostingMethod from productionproperties ) ");
			stringBuilder.AppendLine("DECLARE @cWarehouseID nvarchar(5) ");
			stringBuilder.AppendLine("DECLARE @cPlantID nvarchar(5) ");
			stringBuilder.AppendLine("SET @cWarehouseID = " + empty3.ToSql());
			stringBuilder.AppendLine("SET @cPlantID = " + empty4.ToSql());
			stringBuilder.AppendLine("DECLARE @dCutoffDate datetime ");
			stringBuilder.AppendLine("SET @dCutoffDate = DateAdd(d,1," + dataRow.Field<DateTime>("ppsCutoffDate").ToSql() + ") ");
			stringBuilder.AppendLine("DECLARE @dCutoffDatePOSupply datetime ");
			stringBuilder.AppendLine("SET @dCutoffDatePOSupply = DateAdd(d,1," + dataRow.Field<DateTime>("ppsCutoffDatePOSupply").ToSql() + ") ");
			stringBuilder.AppendLine("DECLARE @cSessionID nvarchar(10) ");
			stringBuilder.AppendLine("SET @cSessionID = " + sessionId.ToSql());
			stringBuilder.AppendLine("DECLARE @cPrevPartID nvarchar(30), @cPrevPartRevisionID nvarchar(15), @cPrevPartDescription nvarchar(50), @cPartDescription nvarchar(50), @cPrevWH nvarchar(5), @cWH nvarchar(5) ");
			stringBuilder.AppendLine("DECLARE @cPrevPLANT nvarchar(5), @cPLANT nvarchar(5), @nRunningBalance numeric(15,5), @nLeadTimeJobMaterial int, @nLeadTimeSalesOrderDelivery int, @dDueDate datetime  ");
			stringBuilder.AppendLine("DECLARE @nOrigKeyField int, @nLineID int, @nNewLineID int, @nRequirementID int, @nOrderDetailID int, @nPrevLineID int, @nNewKeyField int, @nSKIPBAL int, @nQOH numeric(15,5), @nQTI numeric(15,5) ");
			stringBuilder.AppendLine("DECLARE @nSupplyQty numeric(15,5), @nDemandQty numeric(15,5), @nMinQty numeric(15,5), @cPurchaseUoM nvarchar(2), @cInventoryUoM nvarchar(2), @PurchaseType int, @JobID nvarchar(20), @JobAssemblyID int, @JobMaterialID int ");
			stringBuilder.AppendLine("DECLARE @SalesOrderID nvarchar(10), @SalesOrderLineID int, @SalesOrderDeliveryID int, @nUnitPriceJobMaterial numeric(15,5), @nUnitPriceSalesOrderDelivery numeric(15,5) ");
			stringBuilder.AppendLine("DECLARE @nConversionFactor numeric(14,8), @nInvQtyToBuy numeric(15,5), @nPurQtyToBuy numeric(15,5), @SupplierID nvarchar(10), @LocationID nvarchar(5), @cCurrencyRateID nvarchar(5) ");
			stringBuilder.AppendLine("SELECT DemandOrSupply, jmpPartID, PartRevisionID, (Case When PullFromStock > 0 AND PURCHASETOORDER = 0 Then 2 When RTrim(JOBID) <> '' Then 1 When RTrim(ORDERID) <> '' Then 3 Else 2 End) as PurchaseType, UM, DemandQty, OnOrderQty, convert(decimal(15,5), PURCHASETOJOB) as PURCHASETOJOB, convert(decimal(15,5), PULLFROMSTOCK) as PULLFROMSTOCK,  ");
			stringBuilder.AppendLine("  convert(decimal(15,5), PURCHASETOORDER) as PURCHASETOORDER, '' as Currency, JOBID, ASM ,SEQ, PLANT, WH, BIN, imlLastRunDatePurchasePlanner as imrLastRunDatePurchasePlanner, ");
			stringBuilder.AppendLine("  ORDERID, ORDERLINE, ORDERDELIVERY, TYPE, PROJECT, PROJECTAREA, convert(decimal(15,5), 0) as UnitCostBase, convert(decimal(15,5), 0) as UnitCostForeign, convert(decimal(14,8), 0) as ConversionFactor, ");
			stringBuilder.AppendLine("  convert(varchar(3),'') as PurchaseUoM, convert(decimal(3,0), 0) as LeadTime, SOURCE, SKIPBAL, ");
			stringBuilder.AppendLine("  imrQuantityToInspect = Isnull(imrQuantityToInspect,0), INVENTORYSupplyQty, JOBSupplyQty, ORDERSupplyQty, SupplyQty = INVENTORYSupplyQty+JOBSupplyQty+ORDERSupplyQty, PO, PODATE, ReplenishCalculation, ");
			stringBuilder.AppendLine("  IDENTITY(int,1,1) AS OrigKeyField, @cSessionID as SessionID, 0 as LineID, 0 AS RequirementID, DUEDATE, LotSize = ISNULL(imrManufacturingLotSize,0),  ");
			stringBuilder.AppendLine("  MinimumQty = ISNULL(imlMinimumQuantity,0), MaximumQty = ISNULL(imlMaximumQuantity,0), imrMinimumQty = ISNULL(imrMinimumQuantity,0), imrMaximumQty = ISNULL(imrMaximumQuantity,0),  ");
			stringBuilder.AppendLine("  imrQuantityOnHand = ISNULL(imrQuantityOnHand,0), DESCRIPTION, imrLongDescriptionText, imrquantityonhand as ProjectedBalance, SubQuery, IsNull(impNonStockedItem,1) as impNonStockedItem, IsNull(impPhantomOrKitPart,0) as impPhantomOrKitPart, ");
			stringBuilder.AppendLine("  QuantityOnHand = (select ISNULL(sum(imbquantityonhand),0) from partbins where imbpartid = imlpartid and imbPartRevisionID = imlPartRevisionID  and imbWarehouseID = imlPartWarehouseID ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imbWarehouseID") : " and imbWarehouseID = @cWarehouseID ");
			}
			stringBuilder.AppendLine(" ), ");
			stringBuilder.AppendLine(" REORDER_METHOD = isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
			stringBuilder.AppendLine("                    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = jmpPartID),1), ");
			stringBuilder.AppendLine("  SupplierID = isnull((select top 1 SupplierID from  ");
			stringBuilder.AppendLine("                (select omdSupplierOrganizationID as SupplierID, 1 as Sequence from SalesOrderDeliveries Where omdDeliveryType = 5 and omdSalesOrderID = ORDERID and omdSalesOrderLineID = ORDERLINE and omdSalesOrderDeliveryID = ORDERDELIVERY and omdSupplierOrganizationID <> '' ");
			stringBuilder.AppendLine("                union all ");
			stringBuilder.AppendLine("                select jmmSupplierOrganizationID as SupplierID, 2 as Sequence from JobMaterials Where jmmJobID = JOBID and jmmJobAssemblyID = ASM and jmmJobMaterialID = SEQ and jmmSupplierOrganizationID <> '' ");
			stringBuilder.AppendLine("                union all ");
			stringBuilder.AppendLine("                select imrSupplierOrganizationID as SupplierID, 3 as Sequence from PartRevisions Where imrPartID = jmpPartID and imrPartRevisionID = PartRevisionID) as sub order by sequence),''),  ");
			stringBuilder.AppendLine("  LocationID = isnull((select top 1 LocationID from  ");
			stringBuilder.AppendLine("                (select omdPurchaseLocationID as LocationID, 1 as Sequence from SalesOrderDeliveries Where omdDeliveryType = 5 and omdSalesOrderID = ORDERID and omdSalesOrderLineID = ORDERLINE and omdSalesOrderDeliveryID = ORDERDELIVERY and omdSupplierOrganizationID <> '' ");
			stringBuilder.AppendLine("                union all ");
			stringBuilder.AppendLine("                select jmmPurchaseLocationID as LocationID, 2 as Sequence from JobMaterials Where jmmJobID = JOBID and jmmJobAssemblyID = ASM and jmmJobMaterialID = SEQ and jmmSupplierOrganizationID <> '' ");
			stringBuilder.AppendLine("                union all ");
			stringBuilder.AppendLine("                select imrPurchaseLocationID as LocationID, 3 as Sequence from PartRevisions Where imrPartID = jmpPartID and imrPartRevisionID = PartRevisionID) as sub order by sequence),'') ");
			stringBuilder.AppendLine("INTO #PurchasePlannerResults" + sessionId.ToString());
			stringBuilder.AppendLine("FROM ");
			stringBuilder.AppendLine("( ");
			stringBuilder.AppendLine(" SELECT 110 as SubQuery, 'Supply' as DemandOrSupply, '' as ReplenishCalculation, jmpPartID AS jmpPartID, jmpPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE, '' AS ORDERDELIVERY,  ");
			stringBuilder.AppendLine("  '' AS PO, jmpJobID AS JOBID,ISNULL(jmpScheduledDueDate,jmpProductionDueDate) AS DUEDATE,'Jobs' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,1 AS TYPE, jmpProjectID as PROJECT, jmpProjectAreaID as PROJECTAREA,  ");
			stringBuilder.AppendLine("  INVENTORYSupplyQty = (CASE WHEN jmpQuantityCompleted = 0 THEN (CASE WHEN jmpInventoryQuantity - jmpQuantityReceivedToInventory < 0 THEN 0 ELSE jmpInventoryQuantity - jmpQuantityReceivedToInventory END) ELSE (CASE WHEN jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped) - (jmpQuantityShipped + jmpQuantityReceivedToInventory) < 0 THEN 0 ELSE jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped) - (jmpQuantityShipped + jmpQuantityReceivedToInventory) END) END), ");
			stringBuilder.AppendLine("  JOBSupplyQty = (CASE WHEN jmpQuantityCompleted = 0 THEN (CASE WHEN jmpOrderQuantity - jmpQuantityShipped < 0 THEN 0 ELSE jmpOrderQuantity - jmpQuantityShipped END) ELSE (CASE WHEN jmpQuantityCompleted <= jmpQuantityShipped + jmpQuantityReceivedToInventory THEN 0 ELSE (CASE WHEN jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped) - (jmpQuantityShipped + jmpQuantityReceivedToInventory) > 0 THEN jmpOrderQuantity - jmpQuantityShipped ELSE jmpOrderQuantity - jmpQuantityShipped + jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped ) - (jmpQuantityShipped  + jmpQuantityReceivedToInventory) END) END) END), ");
			stringBuilder.AppendLine("  ORDERSupplyQty = 0, 0 AS DemandQty, 0 as OnOrderQty, jmpPartShortDescription AS DESCRIPTION,0 AS LEADTIME,jmpUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, ");
			stringBuilder.AppendLine("  jmpPartWareHouseLocationID as WH, jmpPartBinID as BIN, jmpPlantID as PLANT ");
			stringBuilder.AppendLine(" FROM Jobs Left Outer Join PartRevisions on jmpPartID = imrPartID and jmpPartRevisionID = imrPartRevisionID ");
			stringBuilder.AppendLine(" WHERE jmpClosed = 0  ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine(" AND jmpFirm = " + text8);
			}
			stringBuilder.AppendLine("  AND ISNULL(jmpScheduledDueDate,jmpProductionDueDate) < @dCutoffDate  ");
			stringBuilder.AppendLine("  AND ( (CASE WHEN jmpQuantityCompleted = 0 THEN (CASE WHEN jmpInventoryQuantity - jmpQuantityReceivedToInventory < 0 THEN 0 ELSE jmpInventoryQuantity - jmpQuantityReceivedToInventory END) ELSE (CASE WHEN jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped) - (jmpQuantityShipped + jmpQuantityReceivedToInventory) < 0 THEN 0 ELSE jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped) - (jmpQuantityShipped + jmpQuantityReceivedToInventory) END) END) > 0 ");
			stringBuilder.AppendLine("   OR (CASE WHEN jmpQuantityCompleted = 0 THEN (CASE WHEN jmpOrderQuantity - jmpQuantityShipped < 0 THEN 0 ELSE jmpOrderQuantity - jmpQuantityShipped END) ELSE (CASE WHEN jmpQuantityCompleted <= jmpQuantityShipped + jmpQuantityReceivedToInventory THEN 0 ELSE (CASE WHEN jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped) - (jmpQuantityShipped + jmpQuantityReceivedToInventory) > 0 THEN jmpOrderQuantity - jmpQuantityShipped ELSE jmpOrderQuantity - jmpQuantityShipped + jmpQuantityCompleted - (jmpOrderQuantity - jmpQuantityShipped ) - (jmpQuantityShipped  + jmpQuantityReceivedToInventory) END) END) END) > 0 ) ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? "" : " AND jmpPartWareHouseLocationID = @cWarehouseID ");
			}
			if (!flag7 && !flag6)
			{
				stringBuilder.AppendLine(" and jmpPlantID = @cPlantID ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = jmpPartID AND PartRevId = jmpPartRevisionID)) ");
			}
			if (flag7)
			{
				stringBuilder.AppendLine(" And ( jmpJobID IN (" + text5 + ")");
				stringBuilder.AppendLine(" Or jmpPartID IN (" + text9 + ") )");
			}
			else
			{
				if (flag2)
				{
					stringBuilder.AppendLine(" And jmpJobID IN (" + text5 + ")");
				}
				if (flag5)
				{
					stringBuilder.AppendLine(" And jmpPartID IN (" + text9 + ")");
				}
			}
			stringBuilder.AppendLine(" UNION ALL  ");
			stringBuilder.AppendLine(" SELECT 120 as SubQuery, 'Supply' as DemandOrSupply, '' as ReplenishCalculation, jmaPartID AS jmpPartID,jmaPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE, '' AS ORDERDELIVERY,  ");
			stringBuilder.AppendLine("  '' AS PO,jmaJobID AS JOBID,ISNULL(jmaScheduledDueDate,jmpProductionDueDate) AS DUEDATE,'Jobs' AS SOURCE,jmaJobAssemblyID AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,1 AS TYPE, jmpProjectID as PROJECT, jmpProjectAreaID as PROJECTAREA,  ");
			stringBuilder.AppendLine("  INVENTORYSupplyQty = (CASE WHEN jmaQuantityCompleted = 0 THEN (CASE WHEN jmaInventoryQuantity - jmaQuantityReceivedToInventory < 0 THEN 0 ELSE jmaInventoryQuantity - jmaQuantityReceivedToInventory END) ELSE (CASE WHEN jmaQuantityCompleted - (jmaOrderQuantity + jmaQuantityReceivedToInventory) < 0 THEN 0 ELSE jmaQuantityCompleted - (jmaOrderQuantity + jmaQuantityReceivedToInventory) END) END), ");
			stringBuilder.AppendLine("  JOBSupplyQty = 0, ORDERSupplyQty = 0, 0 AS DemandQty, 0 as OnOrderQty, jmaPartShortDescription AS DESCRIPTION, ");
			stringBuilder.AppendLine("  0 AS LEADTIME,jmaUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, jmaPartWareHouseLocationID as WH, jmaPartBinID as BIN, jmpPlantID as PLANT ");
			stringBuilder.AppendLine(" FROM JobAssemblies INNER JOIN Jobs on jmaJobID = jmpJobID Left Outer Join PartRevisions on jmaPartID = imrPartID and jmaPartRevisionID = imrPartRevisionID ");
			stringBuilder.AppendLine(" WHERE jmajobassemblyid <> 0 AND jmaClosed = 0  ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine(" AND jmpFirm = " + text8);
			}
			stringBuilder.AppendLine("  AND ISNULL(jmaScheduledDueDate,jmpProductionDueDate) < @dCutoffDate  ");
			stringBuilder.AppendLine("  AND (CASE WHEN jmaQuantityCompleted = 0 THEN (CASE WHEN jmaInventoryQuantity - jmaQuantityReceivedToInventory < 0 THEN 0 ELSE jmaInventoryQuantity - jmaQuantityReceivedToInventory END) ELSE (CASE WHEN jmaQuantityCompleted - (jmaOrderQuantity + jmaQuantityReceivedToInventory) < 0 THEN 0 ELSE jmaQuantityCompleted - (jmaOrderQuantity + jmaQuantityReceivedToInventory) END) END) > 0 ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? "" : " AND jmaPartWareHouseLocationID = @cWarehouseID ");
			}
			if (!flag7 && !flag6)
			{
				stringBuilder.AppendLine(" and jmpPlantID = @cPlantID ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = jmaPartID AND PartRevId = jmaPartRevisionID)) ");
			}
			if (flag7)
			{
				stringBuilder.AppendLine(" And ( jmaJobID IN (" + text5 + ")");
				stringBuilder.AppendLine(" Or jmaPartID IN (" + text9 + ") )");
			}
			else
			{
				if (flag2)
				{
					stringBuilder.AppendLine(" And jmaJobID IN (" + text5 + ")");
				}
				if (flag5)
				{
					stringBuilder.AppendLine(" And jmaPartID IN (" + text9 + ")");
				}
			}
			stringBuilder.AppendLine(" UNION ALL ");
			stringBuilder.AppendLine(" SELECT 131 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, jmaPartID AS jmpPartID,jmaPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
			stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,jmaJobID AS JOBID,ISNULL(jmaScheduledDueDate,jmpProductionDueDate) AS DUEDATE,'JobAssemblies' AS SOURCE,jmaJobAssemblyID AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,3 AS TYPE, jmpProjectID as PROJECT, jmpProjectAreaID as PROJECTAREA,  ");
			stringBuilder.AppendLine("  INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, jmaQuantityToPull-jmaQuantityIssued AS DemandQty, 0 as OnOrderQty, jmaPartShortDescription AS DESCRIPTION,0 AS LEADTIME, ");
			stringBuilder.AppendLine("  jmaUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, jmaQuantityToPull-jmaQuantityIssued as PULLFROMSTOCK, 0 as PURCHASETOORDER, jmaPartWareHouseLocationID as WH, jmaPartBinID as BIN, jmpPlantID as PLANT ");
			stringBuilder.AppendLine(" FROM JobAssemblies INNER JOIN Jobs on jmaJobID = jmpJobID Left Outer Join PartRevisions on jmaPartID = imrPartID and jmaPartRevisionID = imrPartRevisionID ");
			stringBuilder.AppendLine(" WHERE jmaClosed = 0 AND jmaIssuedComplete = 0 AND jmaQuantityToPull-jmaQuantityIssued > 0 ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine(" AND jmpFirm = " + text8);
			}
			stringBuilder.AppendLine("  AND ISNULL(jmaScheduledDueDate,jmpProductionDueDate) < @dCutoffDate ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? "" : " AND jmaPartWareHouseLocationID = @cWarehouseID ");
			}
			if (!flag7 && !flag6)
			{
				stringBuilder.AppendLine(" and jmpPlantID = @cPlantID ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = jmaPartID AND PartRevId = jmaPartRevisionID)) ");
			}
			if (flag7)
			{
				stringBuilder.AppendLine(" And ( jmaJobID IN (" + text5 + ")");
				stringBuilder.AppendLine(" Or jmaPartID IN (" + text9 + ") )");
			}
			else
			{
				if (flag2)
				{
					stringBuilder.AppendLine(" And jmaJobID IN (" + text5 + ")");
				}
				if (flag5)
				{
					stringBuilder.AppendLine(" And jmaPartID IN (" + text9 + ")");
				}
			}
			if (!flag2 || flag7)
			{
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 132 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'JobAssemblyTransactions' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE,  '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select -isnull(sum(imtInventoryQuantityReceived),0) from parttransactions inner join jobs on imtjobid = jmpjobid where imtjobid <> '' and imtJobMaterialID = 0 and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID and imtPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and jmpPlantID = @cPlantID " : " and imtPartWarehouseLocationID = @cWarehouseID and jmpPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and imtsource = 3 and imttransactiondate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and imttransactiondate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE ");
				stringBuilder.AppendLine("   isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWareHouseID") : " AND imlPartWareHouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID and PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And impPartID IN (" + text9 + ")");
				}
			}
			stringBuilder.AppendLine(" UNION ALL ");
			stringBuilder.AppendLine(" SELECT 140 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, jmmPartID AS jmpPartID,jmmPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
			stringBuilder.AppendLine("  '' AS ORDERDELIVERY, jmmPurchaseOrderID AS PO,jmmJobID AS JOBID,ISNULL(jmmRequiredDate,jmpProductionDueDate) AS DUEDATE,'JobMaterials' AS SOURCE,jmmJobAssemblyID AS ASM,jmmJobMaterialID AS SEQ, ");
			stringBuilder.AppendLine("  jmmRelatedJobOperationID AS OPERATION, 0 AS SKIPBAL,3 AS TYPE,  jmpProjectID as PROJECT, jmpProjectAreaID as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
			stringBuilder.AppendLine("  DemandQty = (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPurchaseToJobQuantity / jmmEstimatedQuantity End), ");
			stringBuilder.AppendLine("  OnOrderQty = isnull((select sum(pmlInventoryQuantity - pmlInventoryQuantityReceived) from purchaseorderlines where pmlPartID = jmmPartID and pmlPartRevisionID = jmmPartRevisionID and pmlPurchaseType = 1 and pmlJobType = 1 and pmlJobID = jmmjobid and pmlJobAssemblyID = jmmjobassemblyid and pmlJobMaterialID = jmmjobmaterialid and pmlDueDate < @dCutoffDatePOSupply and pmlInventoryQuantityReceived < pmlinventoryquantity and pmlReceivedComplete = 0 and pmlClosed = 0),0),  ");
			stringBuilder.AppendLine("  jmmPartShortDescription AS DESCRIPTION,jmmLeadTime AS LEADTIME,jmmUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, jmmPurchaseToJobQuantity as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
			stringBuilder.AppendLine("  jmmPartWareHouseLocationID as WH, jmmPartBinID as BIN, jmpPlantID as PLANT ");
			stringBuilder.AppendLine(" FROM JobMaterials INNER JOIN Jobs on jmmJobID = jmpJobID LEFT OUTER JOIN Parts on jmmPartID = impPartID ");
			stringBuilder.AppendLine(" WHERE ");
			stringBuilder.AppendLine(" jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0  ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine(" AND jmmFirm = " + text8 + " AND jmpFirm = " + text8);
			}
			stringBuilder.AppendLine("  AND ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < @dCutoffDate ");
			stringBuilder.AppendLine("  AND (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPurchaseToJobQuantity / jmmEstimatedQuantity End) > 0 ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? "" : " AND jmmPartWareHouseLocationID = @cWarehouseID ");
			}
			if (!flag7 && !flag6)
			{
				stringBuilder.AppendLine(" and jmpPlantID = @cPlantID ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine(" And jmmSupplierOrganizationID IN (" + text7 + ")");
			}
			if (flag7)
			{
				stringBuilder.AppendLine(" And ( jmmJobID IN (" + text5 + ")");
				stringBuilder.AppendLine(" Or jmmPartID IN (" + text9 + ") )");
			}
			else
			{
				if (flag2)
				{
					stringBuilder.AppendLine(" And jmmJobID IN (" + text5 + ")");
				}
				if (flag5)
				{
					stringBuilder.AppendLine(" And jmmPartID IN (" + text9 + ")");
				}
			}
			stringBuilder.AppendLine(" UNION ALL ");
			stringBuilder.AppendLine(" SELECT 150 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, jmmPartID AS jmpPartID,jmmPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
			stringBuilder.AppendLine("  '' AS ORDERDELIVERY, jmmPurchaseOrderID AS PO,jmmJobID AS JOBID,ISNULL(jmmRequiredDate, jmpProductionDueDate) AS DUEDATE, 'JobMaterials' AS SOURCE, jmmJobAssemblyID AS ASM, jmmJobMaterialID AS SEQ, ");
			stringBuilder.AppendLine("  jmmRelatedJobOperationID AS OPERATION, 0 AS SKIPBAL,3 AS TYPE,  jmpProjectID as PROJECT, jmpProjectAreaID as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
			stringBuilder.AppendLine("  DemandQty = (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End), ");
			stringBuilder.AppendLine("   0 as OnOrderQty, jmmPartShortDescription AS DESCRIPTION,jmmLeadTime AS LEADTIME,jmmUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End) as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
			stringBuilder.AppendLine("  jmmPartWareHouseLocationID as WH, jmmPartBinID as BIN, jmpPlantID as PLANT ");
			stringBuilder.AppendLine(" FROM JobMaterials INNER JOIN Jobs on jmmJobID = jmpJobID LEFT OUTER JOIN Parts on jmmPartID = impPartID ");
			stringBuilder.AppendLine(" WHERE (impPhantomOrKitPart = 0 or impPhantomOrKitPart is null) AND jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0  ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine(" AND jmmFirm = " + text8 + " AND jmpFirm = " + text8);
			}
			stringBuilder.AppendLine("  AND ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < @dCutoffDate ");
			stringBuilder.AppendLine("  AND (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End) > 0 ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? "" : " AND jmmPartWareHouseLocationID = @cWarehouseID ");
			}
			if (!flag7 && !flag6)
			{
				stringBuilder.AppendLine(" and jmpPlantID = @cPlantID ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine(" And jmmSupplierOrganizationID IN (" + text7 + ")");
			}
			if (flag7)
			{
				stringBuilder.AppendLine(" And ( jmmJobID IN (" + text5 + ")");
				stringBuilder.AppendLine(" Or jmmPartID IN (" + text9 + ") )");
			}
			else
			{
				if (flag2)
				{
					stringBuilder.AppendLine(" And jmmJobID IN (" + text5 + ")");
				}
				if (flag5)
				{
					stringBuilder.AppendLine(" And jmmPartID IN (" + text9 + ")");
				}
			}
			if (!flag2 || flag7)
			{
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 151 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'JobMaterialTransactions' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE, '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select -isnull(sum(imtInventoryQuantityReceived),0) from parttransactions inner join jobs on imtjobid = jmpjobid where imtjobid <> '' and imtJobMaterialID <> 0 and imtJobMaterialComponentID = 0 and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID and imtPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and jmpPlantID = @cPlantID " : " and imtPartWarehouseLocationID = @cWarehouseID and jmpPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and imtsource = 3 and imttransactiondate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and imttransactiondate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE (impPhantomOrKitPart = 0 or impPhantomOrKitPart is null)  ");
				stringBuilder.AppendLine("   and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : "   AND imlPartWarehouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 152 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'JobMaterialIssues' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE, '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select isnull(sum(injJobMatIssueQuantity+injJobMatScrapQuantity),0) from MaterialIssueLines inner join MaterialIssues on injMaterialIssueID = iniMaterialIssueID inner join jobs on injJobID = jmpJobID where injJobID <> '' and injJobMaterialID <> 0 and injPartID = imrPartID and injPartRevisionID = imrPartRevisionID and injPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and jmpPlantID = @cPlantID " : " and injPartWarehouseLocationID = @cWarehouseID and jmpPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and iniMaterialIssueDate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and iniMaterialIssueDate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE impPhantomOrKitPart <> 0   ");
				stringBuilder.AppendLine("   and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : " AND imlPartWarehouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 161 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, omlPartID AS jmpPartID,omlPartRevisionID As PartRevisionID, ");
				stringBuilder.AppendLine("  omdSalesOrderID as ORDERID, omdSalesOrderLineID AS ORDERLINE, omdSalesOrderDeliveryID AS ORDERDELIVERY, ");
				stringBuilder.AppendLine("  '' AS PO,'' AS JOBID,ISNULL(omdDeliveryDate,'20991231') AS DUEDATE,'SalesOrderDeliveries' AS SOURCE, ");
				stringBuilder.AppendLine("  0 AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,4 AS TYPE, omlProjectID as PROJECT, omlProjectAreaID as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = omdDeliveryQuantity-omdQuantityShipped-omdQuantityReceived, ");
				stringBuilder.AppendLine("  OnOrderQty = isnull((select sum(pmlInventoryQuantity - pmlInventoryQuantityReceived) from purchaseorderlines where pmlPartID = omlPartID and pmlPartRevisionID = omlPartRevisionID and pmlPurchaseType = 3 and pmlSalesOrderID = omdSalesOrderID and pmlSalesOrderLineID = omdSalesOrderLineID and pmlSalesOrderDeliveryID = omdSalesOrderDeliveryID and pmlDueDate < @dCutoffDatePOSupply and pmlInventoryQuantityReceived < pmlinventoryquantity and pmlReceivedComplete = 0 and pmlClosed = 0),0), ");
				stringBuilder.AppendLine("  omlPartShortDescription AS DESCRIPTION,0 AS LEADTIME,omlUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, omdDeliveryQuantity-omdQuantityShipped as PULLFROMSTOCK, omdDeliveryQuantity-omdQuantityShipped as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  omdPartWareHouseLocationID as WH, omdPartBinID as BIN, ompPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM SalesOrderDeliveries INNER JOIN SalesOrderLines ON omlSalesOrderID=omdSalesOrderID AND omlSalesOrderLineID=omdSalesOrderLineID INNER JOIN SalesOrders on omlSalesOrderID = ompSalesOrderID ");
				stringBuilder.AppendLine(" LEFT OUTER JOIN PartRevisions ON imrPartID = omlPartID And imrPartRevisionID = omlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE omdDeliveryType = 5 AND omdClosed = 0 AND omdShippedComplete = 0  ");
				if (!string.IsNullOrEmpty(text8))
				{
					stringBuilder.AppendLine(" AND omdFirm = " + text8);
				}
				stringBuilder.AppendLine("  AND omdReceivedComplete = 0  ");
				stringBuilder.AppendLine("  AND DateAdd(d,ISNULL(imrLeadTime,0)*-1,ISNULL(omdDeliveryDate,'20991231')) < @dCutoffDate ");
				stringBuilder.AppendLine("  AND omdDeliveryQuantity-omdQuantityShipped > 0 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : " AND omdPartWareHouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and ompPlantID = @cPlantID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + " WHERE PartId = omlPartID AND PartRevId = omlPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And omlPartID IN (" + text9 + ")");
				}
				if (flag3)
				{
					stringBuilder.AppendLine(" And omdSalesOrderID IN (" + text6 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 162 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, omlPartID AS jmpPartID,omlPartRevisionID As PartRevisionID, ");
				stringBuilder.AppendLine("  omdSalesOrderID as ORDERID, omdSalesOrderLineID AS ORDERLINE, omdSalesOrderDeliveryID AS ORDERDELIVERY, ");
				stringBuilder.AppendLine("  '' AS PO,'' AS JOBID,ISNULL(omdDeliveryDate,'20991231') AS DUEDATE,'SalesOrderDeliveries' AS SOURCE, ");
				stringBuilder.AppendLine("  0 AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,4 AS TYPE, omlProjectID as PROJECT, omlProjectAreaID as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = omdDeliveryQuantity-omdQuantityShipped-omdQuantityReceived, ");
				stringBuilder.AppendLine("  OnOrderQty = isnull((select sum(pmlInventoryQuantity - pmlInventoryQuantityReceived) from purchaseorderlines Left Outer Join Parts on pmlPartID = impPartID where pmlPartID = imrPartID and pmlPartRevisionID = imrPartRevisionID and (impPhantomOrKitPart = 0 or impPhantomOrKitPart is null) and pmlPurchaseType in (2,5) and pmlDueDate < @dCutoffDatePOSupply and pmlInventoryQuantityReceived < pmlinventoryquantity and pmlReceivedComplete = 0 and pmlClosed = 0),0), ");
				stringBuilder.AppendLine("  omlPartShortDescription AS DESCRIPTION,0 AS LEADTIME,omlUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, omdDeliveryQuantity-omdQuantityShipped as PULLFROMSTOCK, 0 as PURCHASETOORDER, ");
				stringBuilder.AppendLine("  omdPartWareHouseLocationID as WH, omdPartBinID as BIN, ompPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM SalesOrderDeliveries INNER JOIN SalesOrderLines ON omlSalesOrderID=omdSalesOrderID AND omlSalesOrderLineID=omdSalesOrderLineID INNER JOIN SalesOrders on omlSalesOrderID = ompSalesOrderID ");
				stringBuilder.AppendLine(" LEFT OUTER JOIN PartRevisions ON imrPartID = omlPartID And imrPartRevisionID = omlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE omdDeliveryType = 2 AND omdClosed = 0 AND omdShippedComplete = 0  ");
				if (!string.IsNullOrEmpty(text8))
				{
					stringBuilder.AppendLine(" AND omdFirm = " + text8);
				}
				stringBuilder.AppendLine("  AND omdReceivedComplete = 0  ");
				stringBuilder.AppendLine("  AND DateAdd(d,ISNULL(imrLeadTime,0)*-1,ISNULL(omdDeliveryDate,'20991231')) < @dCutoffDate ");
				stringBuilder.AppendLine("  AND omdDeliveryQuantity-omdQuantityShipped > 0 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : "  AND omdPartWareHouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and ompPlantID = @cPlantID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + " WHERE PartId = omlPartID AND PartRevId = omlPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And omlPartID IN (" + text9 + ")");
				}
				if (flag3)
				{
					stringBuilder.AppendLine(" And 0 = 1 ");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 163 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID,  ");
				stringBuilder.AppendLine("  '' AS ORDERLINE,'' as ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'SalesOrderShipmentTransactions' AS SOURCE, ");
				stringBuilder.AppendLine("  0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,4 AS TYPE, '' as PROJECT, '' as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select isnull(sum(smlQuantityShipped),0) from shipmentlines inner join shipments on smlShipmentID = smpShipmentID ");
				stringBuilder.AppendLine("  left join SalesOrderDeliveries on smlSalesOrderID = omdSalesOrderID and smlSalesOrderLineID = omdSalesOrderLineID and smlSalesOrderDeliveryID = omdSalesOrderDeliveryID and isnull(omdDeliveryType, 2) in (2,5) ");
				stringBuilder.AppendLine("  Where smlPartID = imrPartID and smlPartRevisionID = imrPartRevisionID and smlPartWarehouseLocationID = queryOuter.imlPartWarehouseID  ");
				if (!string.IsNullOrEmpty(text8))
				{
					stringBuilder.AppendLine(" AND omdFirm = " + text8);
				}
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and smpPlantID = @cPlantID " : " and smlPartWarehouseLocationID = @cWarehouseID and smpPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and smpShipDate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID ");
				stringBuilder.AppendLine(" and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and smpShipDate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,0 AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWareHouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = queryOuter.imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID  ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : "  AND imlPartWarehouseID = @cWarehouseID ");
				}
				stringBuilder.AppendLine(" WHERE (impPhantomOrKitPart = 0 or impPhantomOrKitPart is null)  ");
				stringBuilder.AppendLine("  and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : "  AND imlPartWarehouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				if (flag3)
				{
					stringBuilder.AppendLine(" And 0 = 1 ");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 164 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID,  ");
				stringBuilder.AppendLine("  '' AS ORDERLINE,'' as ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'ShipmentLines' AS SOURCE, ");
				stringBuilder.AppendLine("  0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,4 AS TYPE, '' as PROJECT, '' as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select isnull(sum(smlQuantityShipped),0) from shipmentlines inner join shipments on smlShipmentID = smpShipmentID ");
				stringBuilder.AppendLine("  left join SalesOrderDeliveries on smlSalesOrderID = omdSalesOrderID and smlSalesOrderLineID = omdSalesOrderLineID and smlSalesOrderDeliveryID = omdSalesOrderDeliveryID and isnull(omdDeliveryType,4) = 4 ");
				stringBuilder.AppendLine("  Where  smlPartID = imrPartID and smlPartRevisionID = imrPartRevisionID and smlPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!string.IsNullOrEmpty(text8))
				{
					stringBuilder.AppendLine(" AND omdFirm = " + text8);
				}
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and smpPlantID = @cPlantID " : " and smlPartWarehouseLocationID = @cWarehouseID and smpPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and smpShipDate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and smpShipDate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,0 AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWareHouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE impPhantomOrKitPart <> 0 ");
				stringBuilder.AppendLine("  and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWareHouseID") : "  AND imlPartWareHouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 171 as SubQuery, 'Supply' as DemandOrSupply, '' as ReplenishCalculation, pmlPartID AS jmpPartID,pmlPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, pmlPurchaseOrderID AS PO,'' AS JOBID,ISNULL(pmlDueDate,pmpDueDate) AS DUEDATE,'PurchaseOrderLines' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,2 AS TYPE, pmlProjectID as PROJECT, pmlProjectAreaID as PROJECTAREA,  ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = pmlInventoryQuantity-pmlInventoryQuantityReceived, JOBSupplyQty = 0, ORDERSupplyQty = 0, 0 AS DemandQty, 0 as OnOrderQty, pmlPartShortDescription AS DESCRIPTION,0 AS LEADTIME, ");
				stringBuilder.AppendLine("  pmlInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, pmpOrderDate AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, pmlPartWareHouseLocationID as WH, pmlPartBinID as BIN, pmpPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PurchaseOrderLines INNER JOIN PurchaseOrders ON pmlPurchaseOrderID = pmpPurchaseOrderID  ");
				stringBuilder.AppendLine(" WHERE pmlJobID = '' AND pmlSalesOrderID = '' AND pmlClosed = 0 AND pmlReceivedComplete = 0 AND pmlJobType <> 2 AND pmlInventoryQuantity - pmlInventoryQuantityReceived <> 0  ");
				stringBuilder.AppendLine("  AND ISNULL(pmlDueDate,'20991231') < @dCutoffDatePOSupply ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and pmpPlantID = @cPlantID " : "  AND pmlPartWareHouseLocationID = @cWarehouseID and pmpPlantID = @cPlantID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine(" And pmpSupplierOrganizationID IN (" + text7 + ")");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And pmlPartID IN (" + text9 + ")");
				}
			}
			stringBuilder.AppendLine(" UNION ALL ");
			stringBuilder.AppendLine(" SELECT 172 as SubQuery, 'Supply' as DemandOrSupply, '' as ReplenishCalculation, pmlPartID AS jmpPartID,pmlPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE, '' AS ORDERDELIVERY,  ");
			stringBuilder.AppendLine("  pmlPurchaseOrderID AS PO,pmlJobID AS JOBID,ISNULL(pmlDueDate,pmpDueDate) AS DUEDATE,'PurchaseOrderLines' AS SOURCE,pmlJobAssemblyID AS ASM,pmlJobMaterialID AS SEQ, ");
			stringBuilder.AppendLine("  pmlJobOperationID AS OPERATION, 0 AS SKIPBAL,2 AS TYPE, pmlProjectID as PROJECT, pmlProjectAreaID as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = pmlInventoryQuantity-pmlInventoryQuantityReceived, ORDERSupplyQty = 0, 0 AS DemandQty, ");
			stringBuilder.AppendLine("  0 as OnOrderQty, pmlPartShortDescription AS DESCRIPTION,0 AS LEADTIME,pmlInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, pmpOrderDate AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, pmlPartWareHouseLocationID as WH,  ");
			stringBuilder.AppendLine("  pmlPartBinID as BIN, pmpPlantID as PLANT ");
			stringBuilder.AppendLine(" FROM PurchaseOrderLines INNER JOIN PurchaseOrders ON pmlPurchaseOrderID = pmpPurchaseOrderID  ");
			stringBuilder.AppendLine(" WHERE pmlJobID <> '' AND pmlClosed = 0 AND pmlReceivedComplete = 0 AND pmlJobType <> 2 AND pmlInventoryQuantity - pmlInventoryQuantityReceived <> 0  ");
			stringBuilder.AppendLine("  AND ISNULL(pmlDueDate,'20991231') < @dCutoffDatePOSupply ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? "" : " AND pmlPartWareHouseLocationID = @cWarehouseID ");
			}
			if (!flag7 && !flag6)
			{
				stringBuilder.AppendLine(" and pmpPlantID = @cPlantID ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine(" And pmpSupplierOrganizationID IN (" + text7 + ")");
			}
			if (flag7)
			{
				stringBuilder.AppendLine(" And ( pmlJobID IN (" + text5 + ")");
				stringBuilder.AppendLine(" Or pmlPartID IN (" + text9 + ") )");
			}
			else
			{
				if (flag2 || flag7)
				{
					stringBuilder.AppendLine(" And pmlJobID IN (" + text5 + ")");
				}
				if (flag5)
				{
					stringBuilder.AppendLine(" And pmlPartID IN (" + text9 + ")");
				}
			}
			if (!flag2 || flag7)
			{
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 173 as SubQuery, 'Supply' as DemandOrSupply, '' as ReplenishCalculation, pmlPartID AS jmpPartID,pmlPartRevisionID As PartRevisionID,pmlSalesOrderID AS ORDERID,  ");
				stringBuilder.AppendLine("  pmlSalesOrderLineID AS ORDERLINE, pmlSalesOrderDeliveryID AS ORDERDELIVERY, pmlPurchaseOrderID AS PO,'' AS JOBID,ISNULL(pmlDueDate,pmpDueDate) AS DUEDATE,'PurchaseOrderLines' AS SOURCE, ");
				stringBuilder.AppendLine("  0 AS ASM,0 AS SEQ,0 AS OPERATION, 0 AS SKIPBAL,2 AS TYPE, pmlProjectID as PROJECT, pmlProjectAreaID as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = pmlInventoryQuantity-pmlInventoryQuantityReceived, 0 AS DemandQty, ");
				stringBuilder.AppendLine("  0 as OnOrderQty, pmlPartShortDescription AS DESCRIPTION,0 AS LEADTIME,pmlInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, pmpOrderDate AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, pmlPartWareHouseLocationID as WH,  ");
				stringBuilder.AppendLine("  pmlPartBinID as BIN, pmpPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PurchaseOrderLines INNER JOIN PurchaseOrders ON pmlPurchaseOrderID = pmpPurchaseOrderID  ");
				stringBuilder.AppendLine(" WHERE pmlSalesOrderID <> '' AND pmlClosed = 0 AND pmlReceivedComplete = 0 AND pmlJobType <> 2 AND pmlInventoryQuantity - pmlInventoryQuantityReceived <> 0  ");
				stringBuilder.AppendLine("  AND ISNULL(pmlDueDate,'20991231') < @dCutoffDatePOSupply ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : " AND pmlPartWareHouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and pmpPlantID = @cPlantID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine(" And pmpSupplierOrganizationID IN (" + text7 + ")");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And pmlPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 174 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'AdjustmentTransactions' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE, '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select -isnull(sum(imtInventoryQuantityReceived),0) from parttransactions where imtsource in (5,6,8) and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID and imtPartWarehouseLocationID = queryOuter.imlPartWarehouseID  ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and imtPlantID = @cPlantID " : " and imtPartWarehouseLocationID = @cWarehouseID and imtPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and imtnoninventorytransaction = 0 and imttransactiondate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and imttransactiondate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE (impPhantomOrKitPart = 0 or impPhantomOrKitPart is null)  ");
				stringBuilder.AppendLine("   and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				stringBuilder.AppendLine("   AND (select isnull(sum(imtInventoryQuantityReceived),0) from parttransactions where imtsource in (5,6,8) and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : "and imtPartWarehouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and imtPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and imtnoninventorytransaction = 0 and imttransactiondate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID ) and imttransactiondate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts ");
				stringBuilder.AppendLine(" left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ) < 0 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : "   AND imlPartWarehouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine(" SELECT 175 as SubQuery, 'Supply' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,  ");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'AdjustmentTransactions' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE, '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = (select isnull(sum(imtInventoryQuantityReceived),0) from parttransactions where imtsource in (5,6,8) and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID and imtPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and imtPlantID = @cPlantID " : " and imtPartWarehouseLocationID = @cWarehouseID and imtPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and imtnoninventorytransaction = 0 and imttransactiondate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and imttransactiondate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts  ");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = 0, ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT ");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE (impPhantomOrKitPart = 0 or impPhantomOrKitPart is null)  ");
				stringBuilder.AppendLine("   and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ");
				stringBuilder.AppendLine("   AND (select isnull(sum(imtInventoryQuantityReceived),0) from parttransactions where imtsource in (5,6,8) and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : "and imtPartWarehouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and imtPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and imtnoninventorytransaction = 0 and imttransactiondate > (select isnull(imlLastRunDatePurchasePlanner,GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and imttransactiondate < dateadd(d,1,GetDate()) and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts ");
				stringBuilder.AppendLine(" left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ) > 0 ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : " AND imlPartWarehouseID = @cWarehouseID  ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine(" UNION ALL ");
				stringBuilder.AppendLine("SELECT 176 as SubQuery, 'Demand' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'Misc.Issues' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE, '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = (select isnull(sum(injInvIssueQuantity + injInvScrapQuantity), 0) from MaterialIssueLines inner join MaterialIssues on injMaterialIssueID = iniMaterialIssueID where injIssueType = 2 and injPartID = imrPartID and injPartRevisionID = imrPartRevisionID and injPartWarehouseLocationID = queryOuter.imlPartWarehouseID");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and injPlantID = @cPlantID " : " and injPartWarehouseLocationID = @cWarehouseID and injPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and iniMaterialIssueDate > (select isnull(imlLastRunDatePurchasePlanner, GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and iniMaterialIssueDate < dateadd(d, 1, GetDate()) and isnull((Select(case when isnull(impReorderMethod, 0) = 0 then(case when isnull(imcReorderMethod, 0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts");
				stringBuilder.AppendLine("   left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 2 ), ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE(impPhantomOrKitPart = 0 or impPhantomOrKitPart is null)");
				stringBuilder.AppendLine("   and isnull((Select(case when isnull(impReorderMethod, 0) = 0 then(case when isnull(imcReorderMethod, 0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID), 1) = 2");
				stringBuilder.AppendLine("   AND(select isnull(sum(injInvIssueQuantity + injInvScrapQuantity), 0) from MaterialIssueLines inner join MaterialIssues on injMaterialIssueID = iniMaterialIssueID where injIssueType = 2 and injPartID = imrPartID and injPartRevisionID = imrPartRevisionID and injPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : " and injPartWarehouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and injPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and iniMaterialIssueDate > (select isnull(imlLastRunDatePurchasePlanner, GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) and iniMaterialIssueDate < dateadd(d, 1, GetDate()) and isnull((Select(case when isnull(impReorderMethod, 0) = 0 then(case when isnull(imcReorderMethod, 0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID), 1) = 2) > 0");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : " AND imlPartWarehouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
				stringBuilder.AppendLine("UNION ALL");
				stringBuilder.AppendLine(" SELECT 177 as SubQuery, 'Supply' as DemandOrSupply, 'dynamic' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID,'' AS ORDERID, '' AS ORDERLINE,");
				stringBuilder.AppendLine("  '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,'20991231' AS DUEDATE,'Misc.Receipts' AS SOURCE,0 AS ASM,0 AS SEQ,0 AS OPERATION, 1 AS SKIPBAL,3 AS TYPE, '' as PROJECT, '' as PROJECTAREA, ");
				stringBuilder.AppendLine("  INVENTORYSupplyQty = (select isnull(sum(rmmMiscInvQuantityReceived), 0) from MfgReceipts where rmmReceiptType = 2 and rmmPartID = imrPartID and rmmPartRevisionID = imrPartRevisionID and rmmPartWarehouseLocationID = queryOuter.imlPartWarehouseID ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? " and rmmPlantID = @cPlantID " : " and rmmPartWarehouseLocationID = @cWarehouseID and rmmPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and rmmReceiptDate > (select isnull(imlLastRunDatePurchasePlanner, GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) ");
				stringBuilder.AppendLine(" and rmmReceiptDate < dateadd(d, 1, GetDate()) and isnull((Select(case when isnull(impReorderMethod, 0) = 0 then(case when isnull(imcReorderMethod, 0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts");
				stringBuilder.AppendLine("      left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID), 1) = 2),");
				stringBuilder.AppendLine("  JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
				stringBuilder.AppendLine("  DemandQty = 0, ");
				stringBuilder.AppendLine("  0 as OnOrderQty, imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER,  ");
				stringBuilder.AppendLine("  imlPartWarehouseID as WH, (select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = queryOuter.imlPartWarehouseID order by imbDefaultBin desc, imbQuantityAllocated desc) as BIN, @cPlantID as PLANT");
				stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations queryOuter on imrPartID = queryOuter.imlPartID and imrPartRevisionID = queryOuter.imlPartRevisionID ");
				stringBuilder.AppendLine(" WHERE(impPhantomOrKitPart = 0 or impPhantomOrKitPart is null)");
				stringBuilder.AppendLine("   and isnull((Select(case when isnull(impReorderMethod, 0) = 0 then(case when isnull(imcReorderMethod, 0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID), 1) = 2");
				stringBuilder.AppendLine("   AND(select isnull(sum(rmmMiscInvQuantityReceived), 0) from MfgReceipts where rmmReceiptType = 2 and rmmPartID = imrPartID and rmmPartRevisionID = imrPartRevisionID and rmmPartWarehouseLocationID = queryOuter.imlPartWarehouseID  ");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? "" : " and rmmPartWarehouseLocationID = @cWarehouseID ");
				}
				if (!flag7 && !flag6)
				{
					stringBuilder.AppendLine(" and rmmPlantID = @cPlantID ");
				}
				stringBuilder.AppendLine(" and rmmReceiptDate > (select isnull(imlLastRunDatePurchasePlanner, GetDate()) from PartWarehouseLocations where imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imlPartWarehouseID = queryOuter.imlPartWarehouseID) ");
				stringBuilder.AppendLine(" and rmmReceiptDate < dateadd(d, 1, GetDate()) and isnull((Select(case when isnull(impReorderMethod, 0) = 0 then(case when isnull(imcReorderMethod, 0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts");
				stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID), 1) = 2) > 0");
				if (!flag6)
				{
					stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : " AND imlPartWarehouseID = @cWarehouseID ");
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
				}
				if (flag5 || flag7)
				{
					stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
				}
			}
			stringBuilder.AppendLine(" UNION ALL ");
			stringBuilder.AppendLine(" SELECT 180 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID, ");
			stringBuilder.AppendLine("  '' AS ORDERID, '' AS ORDERLINE, '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,Convert(datetime,GetDate()) AS DUEDATE, 'PartRevisions' AS SOURCE, 0 AS ASM, 0 AS SEQ, 0 AS OPERATION, (Case When imlMinimumQuantity = 0 Then 1 else 0 end) AS SKIPBAL, 5 AS TYPE,  ");
			stringBuilder.AppendLine("  '' as PROJECT, '' as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
			stringBuilder.AppendLine("  DemandQty = (CASE WHEN imlMaximumQuantity = 0 THEN imlMinimumQuantity ELSE imlMaximumQuantity END) - (select isnull(sum(imbquantityonhand),0) from partbins where imbpartid = imlpartid and imbPartRevisionID = imlPartRevisionID and imbWarehouseID = imlPartWarehouseID), ");
			stringBuilder.AppendLine("  OnOrderQty = isnull((select sum(pmlInventoryQuantity - pmlInventoryQuantityReceived) from purchaseorderlines where pmlPurchaseType in (2,5) and pmlPartID = imrPartID and pmlPartRevisionID = imrPartRevisionID and pmlPartWarehouseLocationID = imlPartWarehouseID and pmlDueDate < @dCutoffDatePOSupply and pmlInventoryQuantityReceived < pmlinventoryquantity and pmlReceivedComplete = 0 and pmlClosed = 0),0), ");
			stringBuilder.AppendLine("  imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL  AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, imlPartWarehouseID as WH, ");
			stringBuilder.AppendLine("  isnull((select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityOnHand desc),'') as BIN, ");
			stringBuilder.AppendLine("  isnull((select imwPlantID from Warehouses where imwWarehouseID = imlPartWarehouseID),'') as PLANT ");
			stringBuilder.AppendLine(" FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations on imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : "  and imlPartWarehouseID = @cWarehouseID  ");
			}
			stringBuilder.AppendLine(" WHERE (imrEffectiveEndDate >= (Convert(datetime,GetDate())) Or imrEffectiveEndDate is null) And (imrInactive = 0 And impNonStockedItem = 0 And impPhantomOrKitPart = 0) ");
			stringBuilder.AppendLine(" AND (select sum(imbquantityonhand) from partbins where imbpartid = imlpartid and imbPartRevisionID = imlPartRevisionID and imbWarehouseID = imlPartWarehouseID) < imlMinimumQuantity ");
			stringBuilder.AppendLine(" AND (select count(imbPartID) from partbins where imbpartid = imlpartid and imbPartRevisionID = imlPartRevisionID and imbWarehouseID = imlPartWarehouseID and imbInactiveBin = 0) > 0 ");
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
			}
			if (flag5 || flag7)
			{
				stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
			}
			stringBuilder.AppendLine(" UNION ALL ");
			stringBuilder.AppendLine(" SELECT 190 as SubQuery, 'Demand' as DemandOrSupply, 'min/max' as ReplenishCalculation, imrPartID AS jmpPartID,imrPartRevisionID As PartRevisionID, ");
			stringBuilder.AppendLine("  '' AS ORDERID, '' AS ORDERLINE, '' AS ORDERDELIVERY, '' AS PO,'' AS JOBID,Convert(datetime,GetDate()) AS DUEDATE, 'KitDemand' AS SOURCE, 0 AS ASM, 0 AS SEQ, 0 AS OPERATION, 0 AS SKIPBAL, 5 AS TYPE,  ");
			stringBuilder.AppendLine("  '' as PROJECT, '' as PROJECTAREA, INVENTORYSupplyQty = 0, JOBSupplyQty = 0, ORDERSupplyQty = 0, ");
			stringBuilder.AppendLine("  DemandQty = ");
			if (flag2 && !flag7)
			{
				if (!string.IsNullOrEmpty(text8))
				{
					stringBuilder.AppendLine("   (select isnull(sum((case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End)),0) from JobMaterials INNER JOIN Jobs on jmmJobID = jmpJobID INNER JOIN Parts on jmmPartID = impPartID WHERE jmmJobID IN (" + text5 + ") And jmmPartID = imrPartID and jmmPartRevisionID = imrPartRevisionID and impPhantomOrKitPart <> 0 AND jmmClosed = 0 AND jmmFirm = " + text8 + " AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0 AND ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < @dCutoffDate AND (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End) > 0 ) ");
				}
				else
				{
					stringBuilder.AppendLine("   (select isnull(sum((case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End)),0) from JobMaterials INNER JOIN Jobs on jmmJobID = jmpJobID INNER JOIN Parts on jmmPartID = impPartID WHERE jmmJobID IN (" + text5 + ") And jmmPartID = imrPartID and jmmPartRevisionID = imrPartRevisionID and impPhantomOrKitPart <> 0 AND jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0 AND ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < @dCutoffDate AND (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End) > 0 ) ");
				}
			}
			else if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine("   (select isnull(sum((case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End)),0) from JobMaterials INNER JOIN Jobs on jmmJobID = jmpJobID INNER JOIN Parts on jmmPartID = impPartID WHERE jmmPartID = imrPartID and jmmPartRevisionID = imrPartRevisionID and impPhantomOrKitPart <> 0 AND jmmClosed = 0 AND jmmFirm = " + text8 + " AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0 AND ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < @dCutoffDate AND (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End) > 0 ) ");
			}
			else
			{
				stringBuilder.AppendLine("   (select isnull(sum((case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End)),0) from JobMaterials INNER JOIN Jobs on jmmJobID = jmpJobID INNER JOIN Parts on jmmPartID = impPartID WHERE jmmPartID = imrPartID and jmmPartRevisionID = imrPartRevisionID and impPhantomOrKitPart <> 0 AND jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmEstimatedQuantity-jmmQuantityReceived <> 0 AND ISNULL(ISNULL(jmmOrderByDate,jmmRequiredDate),DateAdd(d,jmmLeadTime*-1,jmpProductionDueDate)) < @dCutoffDate AND (case when jmmEstimatedQuantity = 0 Then 0 Else (jmmEstimatedQuantity-jmmQuantityReceived) * jmmPullFromStockQuantity / jmmEstimatedQuantity End) > 0 ) ");
			}
			stringBuilder.AppendLine("   + ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine("   (select isnull(sum(omdDeliveryQuantity-omdQuantityShipped),0) from SalesOrderDeliveries INNER JOIN SalesOrderLines ON omlSalesOrderID=omdSalesOrderID AND omlSalesOrderLineID=omdSalesOrderLineID INNER JOIN SalesOrders on omlSalesOrderID = ompSalesOrderID INNER JOIN Parts on omlPartID = impPartID WHERE omlPartID = imrPartID and omlPartRevisionID = imrPartRevisionID and impPhantomOrKitPart <> 0 AND omdDeliveryType = 4 AND omdClosed = 0 AND omdFirm = " + text8 + " AND omdShippedComplete = 0 AND omdReceivedComplete = 0 AND DateAdd(d,ISNULL(imrLeadTime,0)*-1,ISNULL(omdDeliveryDate,'20991231')) < @dCutoffDate AND omdDeliveryQuantity-omdQuantityShipped > 0 ) ");
			}
			else
			{
				stringBuilder.AppendLine("   (select isnull(sum(omdDeliveryQuantity-omdQuantityShipped),0) from SalesOrderDeliveries INNER JOIN SalesOrderLines ON omlSalesOrderID=omdSalesOrderID AND omlSalesOrderLineID=omdSalesOrderLineID INNER JOIN SalesOrders on omlSalesOrderID = ompSalesOrderID INNER JOIN Parts on omlPartID = impPartID WHERE omlPartID = imrPartID and omlPartRevisionID = imrPartRevisionID and impPhantomOrKitPart <> 0 AND omdDeliveryType = 4 AND omdClosed = 0 AND omdShippedComplete = 0 AND omdReceivedComplete = 0 AND DateAdd(d,ISNULL(imrLeadTime,0)*-1,ISNULL(omdDeliveryDate,'20991231')) < @dCutoffDate AND omdDeliveryQuantity-omdQuantityShipped > 0 ) ");
			}
			stringBuilder.AppendLine("   - ");
			stringBuilder.AppendLine("   (Round(isnull((select top 1  ");
			stringBuilder.AppendLine("   (case when ( isnull((select sum(imbquantityonhand) from partbins where imbpartid = immpartid and imbpartrevisionid = immpartrevisionid),0) ) / (case when (immQuantityPerAssembly + immscrapquantity + (immQuantityPerAssembly * immScrapPercent *.01 )) = 0 then 1 else (immQuantityPerAssembly + immscrapquantity + (immQuantityPerAssembly * immScrapPercent *.01 )) end) < 0 Then 0 else ( isnull((select sum(imbquantityonhand) from partbins where imbpartid = immpartid and imbpartrevisionid = immpartrevisionid),0) ) / (case when (immQuantityPerAssembly + immscrapquantity + (immQuantityPerAssembly * immScrapPercent *.01 )) = 0 then 1 else (immQuantityPerAssembly + immscrapquantity + (immQuantityPerAssembly * immScrapPercent *.01 )) end) end)  ");
			stringBuilder.AppendLine("   from partmaterials inner join PartRevisions pr2 on immpartid = pr2.imrpartid and immPartRevisionID = pr2.imrPartRevisionID inner join parts on immmethodid = imppartid inner join PartBins on imbPartID = immmethodid and imbPartRevisionID = immMethodRevisionID  ");
			stringBuilder.AppendLine("   where impPhantomOrKitPart <> 0  ");
			stringBuilder.AppendLine("   And immmethodid = PartRevisions.imrPartID and immmethodrevisionid = PartRevisions.imrPartRevisionID  ");
			stringBuilder.AppendLine("   order by ( isnull((select sum(imbquantityonhand) from partbins where imbpartid = immpartid and imbpartrevisionid = immpartrevisionid),0) ) / (case when (immQuantityPerAssembly + immscrapquantity + (immQuantityPerAssembly * immScrapPercent *.01 )) = 0 then 1 else (immQuantityPerAssembly + immscrapquantity + (immQuantityPerAssembly * immScrapPercent *.01 )) end) asc),0),0,1)), ");
			stringBuilder.AppendLine("  OnOrderQty = isnull((select sum(pmlInventoryQuantity - pmlInventoryQuantityReceived) from purchaseorderlines where pmlPurchaseType in (2,5) and pmlPartID = imrPartID and pmlPartRevisionID = imrPartRevisionID and pmlDueDate < @dCutoffDatePOSupply and pmlInventoryQuantityReceived < pmlinventoryquantity and pmlReceivedComplete = 0 and pmlClosed = 0),0), ");
			stringBuilder.AppendLine("  imrShortDescription AS DESCRIPTION,imrLeadTime AS LEADTIME,imrInventoryUnitOfMeasure AS UM, 0 as PurchaseUoM, NULL  AS PODATE, 0 as PURCHASETOJOB, 0 as PULLFROMSTOCK, 0 as PURCHASETOORDER, imlPartWarehouseID as WH, ");
			stringBuilder.AppendLine("   isnull((select top 1 imbPartBinID from PartBins where imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID and imbWarehouseID = imlPartWarehouseID order by imbDefaultBin desc, imbQuantityOnHand desc),'') as BIN, ");
			stringBuilder.AppendLine("   isnull((select imwPlantID from Warehouses where imwWarehouseID = imlPartWarehouseID),'') as PLANT ");
			stringBuilder.AppendLine("  FROM PartRevisions Inner Join Parts On imrPartID = impPartID Inner Join PartWarehouseLocations on imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID ");
			stringBuilder.AppendLine("  WHERE impPhantomOrKitPart <> 0 ");
			stringBuilder.AppendLine("   and isnull((Select (case when isnull(impReorderMethod,0) = 0 then (case when isnull(imcReorderMethod,0) = 0 then 1 else imcReorderMethod end) else impReorderMethod end) from parts   ");
			stringBuilder.AppendLine("    left outer join PartClasses on impPartClassID = imcPartClassID where impPartID = imrPartID),1) = 1 ");
			if (!flag6)
			{
				stringBuilder.AppendLine(flag8 ? getWarehouseQuery(dataTable3, "imlPartWarehouseID") : "  and imlPartWarehouseID = @cWarehouseID  ");
			}
			if (!string.IsNullOrWhiteSpace(text7))
			{
				stringBuilder.AppendLine("And (EXISTS (SELECT distinct PartId FROM PurchasePlannerPartRevisionSuppliersList" + sessionId + " WHERE PartId = imrPartID AND PartRevId = imrPartRevisionID)) ");
			}
			if (flag5 || flag7)
			{
				stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
			}
			stringBuilder.AppendLine(") AS TEST  ");
			stringBuilder.AppendLine("LEFT OUTER JOIN PartRevisions ON imrPartID = jmpPartID And imrPartRevisionID = PartRevisionID  ");
			stringBuilder.AppendLine("LEFT OUTER JOIN PartWarehouseLocations on imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID ");
			stringBuilder.AppendLine(" and imlPartWarehouseID = TEST.WH ");
			stringBuilder.AppendLine("LEFT OUTER JOIN Parts On imrPartID = impPartID  ");
			stringBuilder.AppendLine("Where  ");
			stringBuilder.AppendLine("(impparttype is null or impparttype in (@nPartTypeRangeStart,@nPartTypeRangeEnd)) ");
			if (flag2 && !flag7)
			{
				stringBuilder.AppendLine(" And JOBID <> ''");
			}
			if (flag3)
			{
				stringBuilder.AppendLine(" And ORDERID IN (" + text6 + ")");
			}
			if (!string.IsNullOrWhiteSpace(text4))
			{
				stringBuilder.AppendLine(" And impPartClassID IN (" + text4 + ")");
			}
			if (flag5 || flag7)
			{
				stringBuilder.AppendLine(" And imrPartID IN (" + text9 + ")");
			}
			if (lineId != 0)
			{
				stringBuilder.AppendLine(" And jmpPartID = " + s.ToSql() + " And PartRevisionID = " + s2.ToSql() + " And DESCRIPTION = " + s3.ToSql() + " And PLANT = " + text.ToSql() + " And WH = " + text2.ToSql());
			}
			stringBuilder.AppendLine("ORDER BY jmpPARTID,PartRevisionID,DUEDATE,TYPE ");
			stringBuilder.AppendLine("CREATE CLUSTERED INDEX IDX_C_PPR_OrigKeyField ON #PurchasePlannerResults" + sessionId.ToString() + "(OrigKeyField) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_SupplyQty ON #PurchasePlannerResults" + sessionId.ToString() + "(SupplyQty) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_DemandQty ON #PurchasePlannerResults" + sessionId.ToString() + "(DemandQty) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_REORDER_METHOD ON #PurchasePlannerResults" + sessionId.ToString() + "(REORDER_METHOD) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_ReplenishCalculation ON #PurchasePlannerResults" + sessionId.ToString() + "(ReplenishCalculation) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_JOBID ON #PurchasePlannerResults" + sessionId.ToString() + "(JOBID) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_PO ON #PurchasePlannerResults" + sessionId.ToString() + "(PO) ");
			stringBuilder.AppendLine("CREATE INDEX IDX_PPR_ORDERID ON #PurchasePlannerResults" + sessionId.ToString() + "(ORDERID) ");
			stringBuilder.AppendLine("DELETE FROM #PurchasePlannerResults" + sessionId);
			stringBuilder.AppendLine(" WHERE  ");
			stringBuilder.AppendLine("  (SupplyQty = 0 and DemandQty = 0) or  ");
			stringBuilder.AppendLine("  (SupplyQty < 0 or DemandQty < 0) or ");
			stringBuilder.AppendLine("  (REORDER_METHOD = 2 and ReplenishCalculation = 'min/max') or  ");
			stringBuilder.AppendLine("  (REORDER_METHOD = 1 and ReplenishCalculation = 'dynamic') ");
			stringBuilder.AppendLine("DELETE FROM #PurchasePlannerResults" + sessionId);
			stringBuilder.AppendLine(" WHERE  ");
			stringBuilder.AppendLine("  (select isnull(sum(DemandQty-OnOrderQty),0)  ");
			stringBuilder.AppendLine("   from #PurchasePlannerResults" + sessionId + " Requirements  ");
			stringBuilder.AppendLine(" WHERE  ");
			stringBuilder.AppendLine("    Requirements.SessionID = #PurchasePlannerResults" + sessionId + ".SessionID and  ");
			stringBuilder.AppendLine("    Requirements.jmpPartID = #PurchasePlannerResults" + sessionId + ".jmpPartID and  ");
			stringBuilder.AppendLine("\tRequirements.PartRevisionID = #PurchasePlannerResults" + sessionId + ".PartRevisionID and  ");
			stringBuilder.AppendLine("\tRequirements.DESCRIPTION = #PurchasePlannerResults" + sessionId + ".DESCRIPTION and  ");
			stringBuilder.AppendLine("\tRequirements.WH = #PurchasePlannerResults" + sessionId + ".WH and  ");
			stringBuilder.AppendLine("\tRequirements.PLANT = #PurchasePlannerResults" + sessionId + ".PLANT) <= 0 ");
			stringBuilder.AppendLine(" Update #PurchasePlannerResults" + sessionId.ToString() + " set QuantityOnHand = 0 where (WH = '' and PLANT <> '') /* or (WH <> '' and WH not in (select imwwarehouseid from warehouses where imwPlantID = PLANT) ) */ ");
			stringBuilder.AppendLine("DECLARE PlannedCursor CURSOR READ_ONLY FOR SELECT OrigKeyField, jmpPartID,PartRevisionID,DESCRIPTION,WH,PLANT,SupplyQty,DemandQty,QuantityOnHand,SKIPBAL FROM #PurchasePlannerResults" + sessionId.ToString() + " ORDER BY jmpPARTID,PartRevisionID,DESCRIPTION,PLANT,WH,DUEDATE,TYPE ");
			stringBuilder.AppendLine("OPEN PlannedCursor ");
			stringBuilder.AppendLine("SET @cPrevPartID = '' ");
			stringBuilder.AppendLine("SET @cPrevPartRevisionID = '' ");
			stringBuilder.AppendLine("SET @cPrevPartDescription = '' ");
			stringBuilder.AppendLine("SET @cPrevWH = '' ");
			stringBuilder.AppendLine("SET @cPrevPLANT = '' ");
			stringBuilder.AppendLine("SET @cPartID = '' ");
			stringBuilder.AppendLine("SET @cPartRevisionID = '' ");
			stringBuilder.AppendLine("SET @cPartDescription = '' ");
			stringBuilder.AppendLine("SET @cWH = '' ");
			stringBuilder.AppendLine("SET @cPLANT = '' ");
			stringBuilder.AppendLine("SET @nSKIPBAL = 0 ");
			stringBuilder.AppendLine("FETCH NEXT FROM PlannedCursor INTO @nOrigKeyField, @cPartID, @cPartRevisionID, @cPartDescription, @cWH, @cPLANT, @nSupplyQty, @nDemandQty, @nQOH, @nSKIPBAL ");
			stringBuilder.AppendLine("SET @nLineID = 0 ");
			stringBuilder.AppendLine("SET @nRequirementID = 0 ");
			stringBuilder.AppendLine("WHILE @@FETCH_STATUS = 0 ");
			stringBuilder.AppendLine("BEGIN ");
			stringBuilder.AppendLine("\tIF @cPrevPartID <> @cPartID Or @cPrevPartRevisionID <> @cPartRevisionID Or @cPrevPartDescription <> @cPartDescription Or @cPrevWH <> @cWH Or @cPrevPLANT <> @cPLANT ");
			stringBuilder.AppendLine("\tBEGIN ");
			stringBuilder.AppendLine("\t\tSET @cPrevPartID = @cPartID ");
			stringBuilder.AppendLine("\t\tSET @cPrevPartRevisionID = @cPartRevisionID ");
			stringBuilder.AppendLine("\t\tSET @cPrevPartDescription = @cPartDescription ");
			stringBuilder.AppendLine("\t\tSET @cPrevWH = @cWH ");
			stringBuilder.AppendLine("\t\tSET @cPrevPLANT = @cPLANT ");
			stringBuilder.AppendLine("\t\tSET @nRunningBalance = @nQOH  ");
			if (lineId == 0)
			{
				stringBuilder.AppendLine("\t\tSET @nLineID = @nLineID + 1 ");
			}
			else
			{
				stringBuilder.AppendLine("\t\tSET @nLineID = " + lineId);
			}
			stringBuilder.AppendLine("\t\tSET @nRequirementID = 0 ");
			stringBuilder.AppendLine("\tEND ");
			stringBuilder.AppendLine("\tIF @nSKIPBAL = 0 ");
			stringBuilder.AppendLine("\tBEGIN ");
			stringBuilder.AppendLine("\t    SET @nRunningBalance = @nRunningBalance + @nSupplyQty - @nDemandQty ");
			stringBuilder.AppendLine("\tEND ");
			stringBuilder.AppendLine("\tSET @nRequirementID = @nRequirementID + 1 ");
			stringBuilder.AppendLine("\tUPDATE #PurchasePlannerResults" + sessionId.ToString() + " SET ProjectedBalance = @nRunningBalance, LineID = @nLineID, RequirementID = @nRequirementID WHERE OrigKeyField = @nOrigKeyField ");
			stringBuilder.AppendLine("\tFETCH NEXT FROM PlannedCursor INTO @nOrigKeyField, @cPartID, @cPartRevisionID, @cPartDescription, @cWH, @cPLANT, @nSupplyQty, @nDemandQty, @nQOH, @nSKIPBAL ");
			stringBuilder.AppendLine("END ");
			stringBuilder.AppendLine("CLOSE PlannedCursor ");
			stringBuilder.AppendLine("DEALLOCATE PlannedCursor ");
			stringBuilder.AppendLine(" UPDATE #PurchasePlannerResults" + sessionId.ToString() + " set PULLFROMSTOCK = DemandQty Where REORDER_METHOD = 2 and DemandOrSupply = 'Demand' ");
			stringBuilder.AppendLine(" SELECT 'Lines' as Grid, SessionID, LineID, PLANT, WH, jmpPartID, PartRevisionID, imrLastRunDatePurchasePlanner, DESCRIPTION, LotSize, MinimumQty, MaximumQty, QuantityOnHand, ");
			stringBuilder.AppendLine("  REORDER_METHOD, (select impparttype from parts where imppartid = jmppartid) as PartType, impNonStockedItem, impPhantomOrKitPart ");
			stringBuilder.AppendLine(" INTO #PurchasePlannerLines" + sessionId.ToString());
			stringBuilder.AppendLine(" FROM #PurchasePlannerResults" + sessionId.ToString());
			stringBuilder.AppendLine(" Group By SessionID, LineID, jmpPartID, PartRevisionID, DESCRIPTION, PLANT, WH, imrLastRunDatePurchasePlanner, LotSize, MinimumQty, MaximumQty, QuantityOnHand, ");
			stringBuilder.AppendLine("  REORDER_METHOD, impNonStockedItem, impPhantomOrKitPart ");
			stringBuilder.AppendLine(" SELECT 'Requirements' as Grid, SessionID, LineID, RequirementID, PurchaseType, ");
			stringBuilder.AppendLine("  JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, PO, DUEDATE, PODATE, PurchaseToJob, PullFromStock, SupplyQty, DemandQty, ProjectedBalance, Source, jmpPartID, PartRevisionID  ");
			stringBuilder.AppendLine("  INTO #PurchasePlannerRequirements" + sessionId.ToString());
			stringBuilder.AppendLine("  FROM #PurchasePlannerResults" + sessionId.ToString());
			stringBuilder.AppendLine(" DELETE FROM #PurchasePlannerResults" + sessionId.ToString());
			stringBuilder.AppendLine("  WHERE /* (DemandQty <= 0) */");
			stringBuilder.AppendLine("   /* or */ ( DemandOrSupply = 'Supply' and REORDER_METHOD = 2 and (JOBID <> '' or ORDERID <> '') ) ");
			stringBuilder.AppendLine("   or ( DemandOrSupply = 'Supply' and REORDER_METHOD = 1 and PO <> '' and (JOBID <> '' or ORDERID <> '') ) ");
			stringBuilder.AppendLine(" SELECT 'PO Detail' as Grid, SessionID, LineID, WH, BIN, 0 as OrderDetailID, jmpPartID, PartRevisionID,  ");
			stringBuilder.AppendLine("  SupplierID, LocationID, PurchaseType, UM, (case When Sum(DemandQty-OnOrderQty) < 0 Then 0 Else Sum(DemandQty-OnOrderQty) end) as InvQtyToBuy,  ");
			stringBuilder.AppendLine("  convert(decimal(15,5), 0) as PurQtyToBuy, Convert(varchar(5),'') as Currency, (case when PURCHASETOJOB = 0 then '' else JOBID end) as JOBID, (case when PURCHASETOJOB = 0 then 0 else ASM end) as ASM , ");
			stringBuilder.AppendLine("  (case when PURCHASETOJOB = 0 then 0 else SEQ end) as SEQ, (case when PURCHASETOORDER = 0 then '' else ORDERID end) as ORDERID, (case when PURCHASETOORDER = 0 then '' else ORDERLINE end) as ORDERLINE, (case when PURCHASETOORDER = 0 then '' else ORDERDELIVERY end) as ORDERDELIVERY, convert(decimal(14,8), 0) as ConversionFactor, ");
			stringBuilder.AppendLine("  LeadTime, Convert(varchar(2),'') as PurchaseUoM, ");
			stringBuilder.AppendLine("  IDENTITY(int,1,1) AS OrigKeyField, DESCRIPTION, DUEDATE, PROJECT, PROJECTAREA, convert(decimal(15,5), 0) as UnitCostBase, convert(decimal(15,5), 0) as UnitCostForeign, PULLFROMSTOCK, SOURCE, INVENTORYSupplyQty, MinimumQty, MaximumQty, QuantityOnHand, ReplenishCalculation ");
			stringBuilder.AppendLine(" INTO #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString());
			stringBuilder.AppendLine(" FROM #PurchasePlannerResults" + sessionId.ToString());
			stringBuilder.AppendLine(" Group By jmpPartID, PartRevisionID,  SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, PURCHASETOJOB, JOBID, ASM ,SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, PURCHASETOORDER,  ");
			stringBuilder.AppendLine("  PurchaseType, DUEDATE, PROJECT, PROJECTAREA, UM, UnitCostBase, UnitCostForeign, Currency, ConversionFactor, LeadTime, PurchaseUoM, PULLFROMSTOCK, SOURCE, INVENTORYSupplyQty, MinimumQty, MaximumQty, QuantityOnHand, ReplenishCalculation ");
			stringBuilder.AppendLine("CREATE CLUSTERED INDEX IDX_C_PPOD_OrigKeyField ON #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + "(OrigKeyField) ");
			stringBuilder.AppendLine("DECLARE PlannedCursor CURSOR READ_ONLY FOR SELECT OrigKeyField,LineID,jmpPartID,PartRevisionID,InvQtyToBuy,PurQtyToBuy,SupplierID,LocationID,PurchaseType,JOBID,ASM,SEQ,ORDERID,ORDERLINE,ORDERDELIVERY,UM FROM #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " ORDER BY LINEID,jmpPARTID,PartRevisionID ");
			stringBuilder.AppendLine("OPEN PlannedCursor ");
			stringBuilder.AppendLine("SET @nPrevLineID = 0 ");
			stringBuilder.AppendLine("SET @nLineID = 0 ");
			stringBuilder.AppendLine("SET @nOrderDetailID = 0 ");
			stringBuilder.AppendLine("FETCH NEXT FROM PlannedCursor INTO @nOrigKeyField, @nLineID, @cPartID, @cPartRevisionID, @nInvQtyToBuy, @nPurQtyToBuy, @SupplierID, @LocationID, @PurchaseType, @JobID, @JobAssemblyID, @JobMaterialID, @SalesOrderID, @SalesOrderLineID, @SalesOrderDeliveryID, @cInventoryUOM ");
			stringBuilder.AppendLine("SET @nOrderDetailID = 0 ");
			stringBuilder.AppendLine("WHILE @@FETCH_STATUS = 0 ");
			stringBuilder.AppendLine("BEGIN ");
			stringBuilder.AppendLine("\tIF @nPrevLineID <> @nLineID  ");
			stringBuilder.AppendLine("\tBEGIN ");
			stringBuilder.AppendLine("\t\tSET @nPrevLineID = @nLineID ");
			if (lineId == 0)
			{
				stringBuilder.AppendLine("\t\tSET @nLineID = @nLineID + 1 ");
			}
			else
			{
				stringBuilder.AppendLine("\t\tSET @nLineID = " + lineId);
			}
			stringBuilder.AppendLine("\t\tSET @nOrderDetailID = 0 ");
			stringBuilder.AppendLine("\tEND ");
			stringBuilder.AppendLine("\tSET @nOrderDetailID = @nOrderDetailID + 1 ");
			stringBuilder.AppendLine("\tSET @nConversionFactor = (Select  ");
			stringBuilder.AppendLine("\t                         (Case When IsNull(imxConversionFactor,0) <> 0 Then imxConversionFactor else (Case When IsNull(imzConversionFactor,0) <> 0 then imzConversionFactor else (Case when IsNull(imrConversionFactor,0) <> 0 then imrConversionFactor else 1 end) end) end) as ConversionFactor from ");
			stringBuilder.AppendLine("                              PartRevisions  ");
			stringBuilder.AppendLine("                              Left Outer Join PartOrgReferences on imrPartID = imzPartID And imrPartRevisionID = imzPartRevisionID and imzOrganizationID = @SupplierID  ");
			stringBuilder.AppendLine("                              Left Outer Join PartCrossReferences on imzOrganizationID = imxOrganizationID And imzPartID = imxPartID And imzPartRevisionID = imxPartRevisionID and imxOrganizationID = @SupplierID AND imxLocationID = @LocationID ");
			stringBuilder.AppendLine("                              Where imrPartID = @cPartID  ");
			stringBuilder.AppendLine("                              And imrPartRevisionID = @cPartRevisionID) ");
			stringBuilder.AppendLine("\tIf @nConversionFactor is Null SET @nConversionFactor = 1 ");
			stringBuilder.AppendLine("\tIf @nConversionFactor = 0 Set @nConversionFactor = 1 ");
			stringBuilder.AppendLine("\tSET @cPurchaseUoM = (Select  ");
			stringBuilder.AppendLine("\t                    (Case When IsNull(imxPurchaseUnitOfMeasure,'') <> '' Then imxPurchaseUnitOfMeasure else (Case When IsNull(imzPurchaseUnitOfMeasure,'') <> '' then imzPurchaseUnitOfMeasure else (Case when IsNull(imrPurchaseUnitOfMeasure,'') <> '' then imrPurchaseUnitOfMeasure else '' end) end) end) as PurchaseUnitOfMeasure from ");
			stringBuilder.AppendLine("                              PartRevisions  ");
			stringBuilder.AppendLine("                              Left Outer Join PartOrgReferences on imrPartID = imzPartID And imrPartRevisionID = imzPartRevisionID and imzOrganizationID = @SupplierID  ");
			stringBuilder.AppendLine("                              Left Outer Join PartCrossReferences on imzOrganizationID = imxOrganizationID And imzPartID = imxPartID And imzPartRevisionID = imxPartRevisionID and imxOrganizationID = @SupplierID AND imxLocationID = @LocationID ");
			stringBuilder.AppendLine("                              Where imrPartID = @cPartID  ");
			stringBuilder.AppendLine("                              And imrPartRevisionID = @cPartRevisionID) ");
			stringBuilder.AppendLine("\tIf @cPurchaseUoM is Null SET @cPurchaseUoM = @cInventoryUoM ");
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine("\tSET @nUnitPriceJobMaterial = (Select (Case When @PurchaseType = 1 Then IsNull(jmmCalculatedUnitCost/@nConversionFactor,0) Else 0 End) as UnitPrice From JobMaterials Where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID and jmmJobMaterialID = @JobMaterialID and jmmFirm = " + text8 + ") ");
				stringBuilder.AppendLine("\tSET @nUnitPriceSalesOrderDelivery = (Select (Case When @PurchaseType = 3 Then IsNull(omdPurchaseUnitCostBase,0) Else 0 End) as UnitPrice From SalesOrderDeliveries Where omdSalesOrderID = @SalesOrderID and omdSalesOrderLineID = @SalesOrderLineID and omdSalesOrderDeliveryID = @SalesOrderDeliveryID AND omdFirm = " + text8 + ") ");
				stringBuilder.AppendLine("\tSET @nLeadTimeJobMaterial  = (Select (Case When @PurchaseType = 1 Then IsNull(jmmLeadTime,0) Else 0 End) as LeadTime From JobMaterials Where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID and jmmJobMaterialID = @JobMaterialID and jmmFirm = " + text8 + ") ");
			}
			else
			{
				stringBuilder.AppendLine("\tSET @nUnitPriceJobMaterial = (Select (Case When @PurchaseType = 1 Then IsNull(jmmCalculatedUnitCost/@nConversionFactor,0) Else 0 End) as UnitPrice From JobMaterials Where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID and jmmJobMaterialID = @JobMaterialID ) ");
				stringBuilder.AppendLine("\tSET @nUnitPriceSalesOrderDelivery = (Select (Case When @PurchaseType = 3 Then IsNull(omdPurchaseUnitCostBase,0) Else 0 End) as UnitPrice From SalesOrderDeliveries Where omdSalesOrderID = @SalesOrderID and omdSalesOrderLineID = @SalesOrderLineID and omdSalesOrderDeliveryID = @SalesOrderDeliveryID ) ");
				stringBuilder.AppendLine("\tSET @nLeadTimeJobMaterial  = (Select (Case When @PurchaseType = 1 Then IsNull(jmmLeadTime,0) Else 0 End) as LeadTime From JobMaterials Where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID and jmmJobMaterialID = @JobMaterialID ) ");
			}
			stringBuilder.AppendLine("\tSET @nLeadTimeSalesOrderDelivery = (Select (Case When @PurchaseType = 3 Then IsNull(imrLeadTime,0) Else 0 End) as LeadTime From PartRevisions Where imrPartID = @cPartID and imrPartRevisionID = @cPartRevisionID) ");
			stringBuilder.AppendLine("\tSET @cCurrencyRateID = (Case When IsNull((SELECT cmlCurrencyRateID from OrganizationLocations Where cmlOrganizationID = @SupplierID AND cmlLocationID = @LocationID),'') <> '' Then(SELECT cmlCurrencyRateID from OrganizationLocations Where cmlOrganizationID = @SupplierID AND cmlLocationID = @LocationID) Else (SELECT top 1 IsNull(xadCurrencyRateID,'') as CurrencyRateID from DatasetProperties ) End) ");
			stringBuilder.AppendLine("\tUPDATE #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " SET OrderDetailID = @nOrderDetailID, PurQtyToBuy = @nInvQtyToBuy * @nConversionFactor, ConversionFactor = @nConversionFactor, ");
			stringBuilder.AppendLine("\t     LeadTime =        (Case When PurchaseType = 1 Then @nLeadTimeJobMaterial  When PurchaseType = 3 Then @nLeadTimeSalesOrderDelivery Else 0 End), ");
			stringBuilder.AppendLine("\t \t UnitCostBase =    (Case When PurchaseType = 1 Then @nUnitPriceJobMaterial When PurchaseType = 3 Then @nUnitPriceSalesOrderDelivery Else 0 End), ");
			stringBuilder.AppendLine("\t \t UnitCostForeign = (Case When PurchaseType = 1 Then @nUnitPriceJobMaterial When PurchaseType = 3 Then @nUnitPriceSalesOrderDelivery Else 0 End), ");
			stringBuilder.AppendLine("\t \t Currency = @cCurrencyRateID , PurchaseUoM = @cPurchaseUoM WHERE OrigKeyField = @nOrigKeyField ");
			stringBuilder.AppendLine("\tFETCH NEXT FROM PlannedCursor INTO @nOrigKeyField, @nLineID, @cPartID, @cPartRevisionID, @nInvQtyToBuy, @nPurQtyToBuy, @SupplierID, @LocationID, @PurchaseType, @JobID, @JobAssemblyID, @JobMaterialID, @SalesOrderID, @SalesOrderLineID, @SalesOrderDeliveryID, @cInventoryUOM ");
			stringBuilder.AppendLine("END ");
			stringBuilder.AppendLine("CLOSE PlannedCursor ");
			stringBuilder.AppendLine("DEALLOCATE PlannedCursor ");
			stringBuilder.AppendLine(" Select Grid, SessionID, LineID, WH, BIN, ROW_NUMBER() OVER(Partition By LineID ORDER BY LineID) as OrderDetailID, jmpPartID, PartRevisionID, SupplierID, LocationID, DESCRIPTION, PurchaseType, IDENTITY(int,1,1) AS KeyField, ");
			stringBuilder.AppendLine(" UM, PurchaseUoM, ConversionFactor, LeadTime, InvQtyToBuy, PurQtyToBuy, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, ");
			stringBuilder.AppendLine("   MinimumQty, MaximumQty, QuantityOnHand, InvDemandTotal, InvSupplyTotal, ReplenishCalculation ");
			stringBuilder.AppendLine("  INTO #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + "  ");
			stringBuilder.AppendLine("  FROM ");
			stringBuilder.AppendLine("  ( ");
			stringBuilder.AppendLine("   SELECT '1_PO_Detail_PurchaseToJob' as Grid, SessionID, LineID, WH, BIN, jmpPartID, PartRevisionID, ");
			stringBuilder.AppendLine("    SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, MinimumQty, MaximumQty, QuantityOnHand, ");
			stringBuilder.AppendLine("    isnull((select sum(pullfromstock) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D2 where D2.sessionid = D1.SessionID and D2.LineID = D1.LineID and D2.JOBID = '' group by D2.SessionID, D2.LineID),0) as InvDemandTotal,   ");
			stringBuilder.AppendLine("    isnull((select sum(InventorySupplyQty) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D2 where D2.sessionid = D1.SessionID and D2.LineID = D1.LineID and D2.JOBID = '' group by D2.SessionID, D2.LineID),0) as InvSupplyTotal,  ");
			stringBuilder.AppendLine("    Sum(InvQtyToBuy) as InvQtyToBuy, ");
			stringBuilder.AppendLine("    Sum(PurQtyToBuy) as PurQtyToBuy, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, IsNull(UnitCostBase, 0) as UnitCostBase, IsNull(UnitCostForeign, 0) as UnitCostForeign, ReplenishCalculation ");
			stringBuilder.AppendLine("   FROM #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D1  ");
			stringBuilder.AppendLine("   WHERE(PULLFROMSTOCK <= 0 and SOURCE <> 'PartRevisions') ");
			stringBuilder.AppendLine("   Group By jmpPartID, PartRevisionID, SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, JOBID, ASM, SEQ, ");
			stringBuilder.AppendLine("    ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, UnitCostBase, UnitCostForeign, Currency, MinimumQty, MaximumQty, QuantityOnHand, SOURCE, InvQtyToBuy, PurQtyToBuy, ReplenishCalculation ");
			stringBuilder.AppendLine("   UNION ALL ");
			stringBuilder.AppendLine("   SELECT '2_PO_Detail_PurchaseToOrder' as Grid, SessionID, LineID, WH, BIN, jmpPartID, PartRevisionID, ");
			stringBuilder.AppendLine("    SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, MinimumQty, MaximumQty, QuantityOnHand, ");
			stringBuilder.AppendLine("    isnull((select sum(pullfromstock) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D2 where D2.sessionid = D1.SessionID and D2.LineID = D1.LineID and D2.JOBID = '' group by D2.SessionID, D2.LineID),0) as InvDemandTotal,   ");
			stringBuilder.AppendLine("    isnull((select sum(InventorySupplyQty) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D2 where D2.sessionid = D1.SessionID and D2.LineID = D1.LineID and D2.JOBID = '' group by D2.SessionID, D2.LineID),0) as InvSupplyTotal,  ");
			stringBuilder.AppendLine("    Sum(InvQtyToBuy) as InvQtyToBuy, ");
			stringBuilder.AppendLine("    Sum(PurQtyToBuy) as PurQtyToBuy, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, IsNull(UnitCostBase, 0) as UnitCostBase, IsNull(UnitCostForeign, 0) as UnitCostForeign, ReplenishCalculation ");
			stringBuilder.AppendLine("   FROM #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D1  ");
			stringBuilder.AppendLine("   WHERE(SOURCE <> 'PartRevisions' and PurchaseType = 3) ");
			stringBuilder.AppendLine("   Group By jmpPartID, PartRevisionID, SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, JOBID, ASM, SEQ, ");
			stringBuilder.AppendLine("    ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, UnitCostBase, UnitCostForeign, Currency, MinimumQty, MaximumQty, QuantityOnHand, SOURCE, InvQtyToBuy, PurQtyToBuy, ReplenishCalculation ");
			stringBuilder.AppendLine("   UNION ALL ");
			stringBuilder.AppendLine("   SELECT '3_PO_Detail_Inv' as Grid, sessionid, lineid, WH, BIN, jmppartid, PartRevisionID, ");
			stringBuilder.AppendLine("    SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, MinimumQty, MaximumQty, QuantityOnHand, ");
			stringBuilder.AppendLine("    InvDemandTotal, InvSupplyTotal, ");
			stringBuilder.AppendLine("    (InvDemandTotal) as InvQtyToBuy, ");
			stringBuilder.AppendLine("    (InvDemandTotal) * ConversionFactor as PurQtyToBuy, ");
			stringBuilder.AppendLine("    Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, ReplenishCalculation ");
			stringBuilder.AppendLine("   from( ");
			stringBuilder.AppendLine("   select D5.sessionid, D5.lineid, WH, BIN, jmppartid, PartRevisionID, ");
			stringBuilder.AppendLine("     SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, SOURCE, ");
			stringBuilder.AppendLine("     isnull((select sum(PULLFROMSTOCK) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D6 where (PULLFROMSTOCK > 0) AND (PurchaseType <> 3) AND D6.sessionid = D5.sessionid and D6.LineID = D5.lineid and D6.JobID = '' group by D6.SessionID, D6.LineID),0) as InvDemandTotal,   ");
			stringBuilder.AppendLine("     isnull((select sum(InventorySupplyQty) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D6 where (InventorySupplyQty > 0) AND (PurchaseType <> 3) AND D6.sessionid = D5.sessionid and D6.LineID = D5.lineid and D6.JobID = '' group by D6.SessionID, D6.LineID),0) as InvSupplyTotal,   ");
			stringBuilder.AppendLine("     InvQtyToBuy, PurQtyToBuy, ");
			stringBuilder.AppendLine("     PULLFROMSTOCK, INVENTORYSupplyQty, ");
			stringBuilder.AppendLine("     Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, IsNull(UnitCostBase, 0) as UnitCostBase, IsNull(UnitCostForeign, 0) as UnitCostForeign, MinimumQty, MaximumQty, QuantityOnHand, ReplenishCalculation ");
			stringBuilder.AppendLine("    FROM #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D5 ");
			stringBuilder.AppendLine("    Group By jmpPartID, PartRevisionID, SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, JOBID, ASM, SEQ, ");
			stringBuilder.AppendLine("     ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, UnitCostBase, UnitCostForeign, Currency, MinimumQty, MaximumQty, QuantityOnHand, SOURCE, InvQtyToBuy, PurQtyToBuy, PULLFROMSTOCK, INVENTORYSupplyQty, ReplenishCalculation ");
			stringBuilder.AppendLine("    ) as InnerQuery1 ");
			stringBuilder.AppendLine("    WHERE (PULLFROMSTOCK > 0) AND (PurchaseType <> 3) ");
			stringBuilder.AppendLine("     AND (ReplenishCalculation = 'dynamic' OR ((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal) - InvDemandTotal) >= 0) ");
			stringBuilder.AppendLine("    Group By jmpPartID, PartRevisionID, SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, JOBID, ASM, SEQ, ");
			stringBuilder.AppendLine("    ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, UnitCostBase, UnitCostForeign, Currency, MinimumQty, MaximumQty, QuantityOnHand, SOURCE, InvQtyToBuy, PurQtyToBuy, PULLFROMSTOCK, InventorySupplyQty, InvDemandTotal, InvSupplyTotal, ReplenishCalculation ");
			if (!flag2 || (flag2 && flag7))
			{
				stringBuilder.AppendLine("    UNION ALL ");
				stringBuilder.AppendLine("   SELECT '4_PO_Detail_InvBelowMin' as Grid, sessionid, lineid, WH, BIN, jmppartid, PartRevisionID, ");
				stringBuilder.AppendLine("    SupplierID, LocationID, DESCRIPTION, 2 as PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, MinimumQty, MaximumQty, QuantityOnHand, ");
				stringBuilder.AppendLine("    InvDemandTotal, InvSupplyTotal, ");
				stringBuilder.AppendLine("    (CASE WHEN(QuantityOnHand - InvDemandTotal + InvSupplytotal) > MinimumQty THEN 0 ");
				stringBuilder.AppendLine("     ELSE CASE WHEN((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal) - InvDemandTotal) < 0 ");
				stringBuilder.AppendLine("     THEN ((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal)) ");
				stringBuilder.AppendLine("     ELSE ((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal) - InvDemandTotal) END END) as InvQtyToBuy, ");
				stringBuilder.AppendLine("    (CASE WHEN(QuantityOnHand - InvDemandTotal + InvSupplytotal) > MinimumQty THEN 0 ");
				stringBuilder.AppendLine("     ELSE CASE WHEN((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal) - InvDemandTotal) < 0 ");
				stringBuilder.AppendLine("     THEN ((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal)) ");
				stringBuilder.AppendLine("     ELSE ((CASE WHEN MaximumQty = 0 THEN MinimumQty ELSE MaximumQty END) - (QuantityOnHand - InvDemandTotal + InvSupplytotal) - InvDemandTotal) END END) *ConversionFactor as PurQtyToBuy, ");
				stringBuilder.AppendLine("    Currency, '' as JOBID, 0 as ASM, 0 as SEQ, '' as ORDERID, ORDERLINE, 0 as ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, ReplenishCalculation ");
				stringBuilder.AppendLine("   from( ");
				stringBuilder.AppendLine("   select D7.sessionid, D7.lineid, WH, BIN, jmppartid, PartRevisionID, ");
				stringBuilder.AppendLine("    SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, 'BelowMinimum' as SOURCE, ");
				stringBuilder.AppendLine("    isnull((select sum(PULLFROMSTOCK) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D8 where (PULLFROMSTOCK > 0) AND (PurchaseType <> 3) and D8.sessionid = D7.sessionid and D8.LineID = D7.lineid and D8.JobID = '' group by D8.SessionID, D8.LineID),0) as InvDemandTotal,   ");
				stringBuilder.AppendLine("    isnull((select sum(InventorySupplyQty) from #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D8 where (InventorySupplyQty > 0) AND (PurchaseType <> 3) and D8.sessionid = D7.sessionid and D8.LineID = D7.lineid and D8.JobID = '' group by D8.SessionID, D8.LineID),0) as InvSupplyTotal,   ");
				stringBuilder.AppendLine("    PULLFROMSTOCK, INVENTORYSupplyQty, ");
				stringBuilder.AppendLine("    Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, IsNull(UnitCostBase, 0) as UnitCostBase, IsNull(UnitCostForeign, 0) as UnitCostForeign, MinimumQty, MaximumQty, QuantityOnHand, ReplenishCalculation ");
				stringBuilder.AppendLine("   FROM #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString() + " D7  ");
				stringBuilder.AppendLine("   Group By jmpPartID, PartRevisionID, SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, JOBID, ASM, SEQ, ");
				stringBuilder.AppendLine("    ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, UnitCostBase, UnitCostForeign, Currency, MinimumQty, MaximumQty, QuantityOnHand, SOURCE, PULLFROMSTOCK, INVENTORYSupplyQty, ReplenishCalculation ");
				stringBuilder.AppendLine("   ) as InnerQuery2 ");
				stringBuilder.AppendLine("   Where ReplenishCalculation <> 'dynamic' And (QuantityOnHand - InvDemandTotal + InvSupplyTotal) < MinimumQty AND PurchaseType <> 3");
				stringBuilder.AppendLine("   Group By jmpPartID, PartRevisionID, SessionID, LineID, WH, BIN, SupplierID, LocationID, DESCRIPTION, JOBID, ASM, SEQ, ");
				stringBuilder.AppendLine("    ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, UnitCostBase, UnitCostForeign, Currency, MinimumQty, MaximumQty, QuantityOnHand, SOURCE, PULLFROMSTOCK, INVENTORYSupplyQty, InvDemandTotal, InvSupplyTotal, ReplenishCalculation ");
			}
			stringBuilder.AppendLine("   ) as OuterQuery ");
			stringBuilder.AppendLine("  Where InvQtyToBuy > 0 and (grid LIKE '1%' or grid LIKE '2%' or ReplenishCalculation = 'dynamic' or (QuantityOnHand - InvDemandTotal + InvSupplyTotal) < MinimumQty) ");
			stringBuilder.AppendLine("  order by jmppartid ");
			stringBuilder.AppendLine(" DECLARE PlannedCursor CURSOR READ_ONLY FOR SELECT SessionID, LineID, OrderDetailID, jmpPartID, PartRevisionID FROM #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " Where Grid LIKE '3%' ORDER BY SessionID,LineID,DUEDATE,OrderDetailID ");
			stringBuilder.AppendLine(" OPEN PlannedCursor ");
			stringBuilder.AppendLine(" SET @nPrevLineID = 0 ");
			stringBuilder.AppendLine(" SET @cPartID = '' ");
			stringBuilder.AppendLine(" SET @cPartRevisionID = '' ");
			stringBuilder.AppendLine(" FETCH NEXT FROM PlannedCursor INTO @cSessionID, @nLineID, @nOrderDetailID, @cPartID, @cPartRevisionID ");
			stringBuilder.AppendLine(" WHILE @@FETCH_STATUS = 0 ");
			stringBuilder.AppendLine(" BEGIN ");
			stringBuilder.AppendLine("     IF @nLineID = @nPrevLineID ");
			stringBuilder.AppendLine("     BEGIN ");
			stringBuilder.AppendLine("         DELETE #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " WHERE SessionID = @cSessionID and LineID = @nLineID and OrderDetailID = @nOrderDetailID ");
			stringBuilder.AppendLine(" \tEND ");
			stringBuilder.AppendLine("     SET @nPrevLineID = @nLineID ");
			stringBuilder.AppendLine("     FETCH NEXT FROM PlannedCursor INTO @cSessionID, @nLineID, @nOrderDetailID, @cPartID, @cPartRevisionID ");
			stringBuilder.AppendLine(" END ");
			stringBuilder.AppendLine(" CLOSE PlannedCursor ");
			stringBuilder.AppendLine(" DEALLOCATE PlannedCursor ");
			stringBuilder.AppendLine(" Delete from temp from ((select ((row_number() over (partition by grid, jmppartid, PartRevisionID, wh order by sessionid, lineid, supplierid desc))) as line from #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " Where (Grid like '%BelowMin%' or Grid like '%_Inv'))) as temp where temp.line >= 2  ");
			stringBuilder.AppendLine(" Select SessionID, LineID, WH, BIN, jmpPartID, PartRevisionID, SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, sum(invqtytobuy) as InvQtyToBuy, sum(purqtytobuy) as PurQtyToBuy, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, MinimumQty, MaximumQty, QuantityOnHand, InvDemandTotal, InvSupplyTotal ");
			stringBuilder.AppendLine(" into #PurchasePlannerPurchaseOrderDetailsSummed" + sessionId.ToString() + " ");
			stringBuilder.AppendLine(" from #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " ");
			stringBuilder.AppendLine(" group by ");
			stringBuilder.AppendLine(" SessionID, LineID, WH, BIN, jmpPartID, PartRevisionID, SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, MinimumQty, MaximumQty, QuantityOnHand, InvDemandTotal, InvSupplyTotal ");
			stringBuilder.AppendLine(" Delete from #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " ");
			stringBuilder.AppendLine(" Insert Into #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " (Grid, SessionID, LineID, WH, BIN, OrderDetailID, jmpPartID, PartRevisionID, SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, InvQtyToBuy, PurQtyToBuy, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, MinimumQty, MaximumQty, QuantityOnHand, InvDemandTotal, InvSupplyTotal, ReplenishCalculation) ");
			stringBuilder.AppendLine(" (select '', SessionID, LineID, WH, BIN, ROW_NUMBER() OVER (ORDER BY SessionID), jmpPartID, PartRevisionID, SupplierID, LocationID, DESCRIPTION, PurchaseType, UM, PurchaseUoM, ConversionFactor, LeadTime, InvQtyToBuy, PurQtyToBuy, Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, UnitCostBase, UnitCostForeign, MinimumQty, MaximumQty, QuantityOnHand, InvDemandTotal, InvSupplyTotal, '' From #PurchasePlannerPurchaseOrderDetailsSummed" + sessionId.ToString() + ") ");
			stringBuilder.AppendLine(" Delete #PurchasePlannerLines" + sessionId.ToString() + " Where LineID not in (select lineid from #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " where LineID = #PurchasePlannerLines" + sessionId.ToString() + ".lineid) ");
			stringBuilder.AppendLine(" Delete #PurchasePlannerRequirements" + sessionId.ToString() + " Where LineID not in (select lineid from #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " where LineID = #PurchasePlannerRequirements" + sessionId.ToString() + ".lineid) ");
			if (lineId == 0)
			{
				stringBuilder.AppendLine(" ALTER TABLE #PurchasePlannerLines" + sessionId.ToString() + " add NewLineID int  ");
				stringBuilder.AppendLine(" ALTER TABLE #PurchasePlannerRequirements" + sessionId.ToString() + " add NewLineID int  ");
				stringBuilder.AppendLine(" ALTER TABLE #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " add NewLineID int  ");
				stringBuilder.AppendLine(" DECLARE PlannedCursor CURSOR READ_ONLY FOR SELECT LineID FROM #PurchasePlannerLines" + sessionId.ToString() + " ORDER BY LineID ");
				stringBuilder.AppendLine(" OPEN PlannedCursor ");
				stringBuilder.AppendLine(" FETCH NEXT FROM PlannedCursor INTO @nLineID ");
				stringBuilder.AppendLine(" SET @nNewLineID = 0 ");
				stringBuilder.AppendLine(" WHILE @@FETCH_STATUS = 0 ");
				stringBuilder.AppendLine(" BEGIN  ");
				stringBuilder.AppendLine(" \tSET @nNewLineID = @nNewLineID + 1  ");
				stringBuilder.AppendLine("\tUPDATE #PurchasePlannerLines" + sessionId.ToString() + " SET NewLineID = @nNewLineID WHERE LineID = @nLineID ");
				stringBuilder.AppendLine("\tFETCH NEXT FROM PlannedCursor INTO @nLineID ");
				stringBuilder.AppendLine(" END ");
				stringBuilder.AppendLine(" CLOSE PlannedCursor ");
				stringBuilder.AppendLine(" DEALLOCATE PlannedCursor ");
				stringBuilder.AppendLine(" update #PurchasePlannerRequirements" + sessionId.ToString() + " set #PurchasePlannerRequirements" + sessionId.ToString() + ".NewLineID = #PurchasePlannerLines" + sessionId.ToString() + ".NewLineID from #PurchasePlannerLines" + sessionId.ToString() + " inner join #PurchasePlannerRequirements" + sessionId.ToString() + " on #PurchasePlannerLines" + sessionId.ToString() + ".LineID = #PurchasePlannerRequirements" + sessionId.ToString() + ".LineID  ");
				stringBuilder.AppendLine(" update #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " set #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + ".NewLineID = #PurchasePlannerLines" + sessionId.ToString() + ".NewLineID from #PurchasePlannerLines" + sessionId.ToString() + " inner join #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " on #PurchasePlannerLines" + sessionId.ToString() + ".LineID = #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + ".LineID  ");
				stringBuilder.AppendLine(" update #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " set lineid = NewLineID ");
				stringBuilder.AppendLine(" update #PurchasePlannerRequirements" + sessionId.ToString() + " set lineid = NewLineID ");
				stringBuilder.AppendLine(" update #PurchasePlannerLines" + sessionId.ToString() + " set lineid = NewLineID ");
				stringBuilder.AppendLine(" alter table #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + " drop column NewLineID ");
				stringBuilder.AppendLine(" alter table #PurchasePlannerRequirements" + sessionId.ToString() + " drop column NewLineID ");
				stringBuilder.AppendLine(" alter table #PurchasePlannerLines" + sessionId.ToString() + " drop column NewLineID ");
			}
			stringBuilder.AppendLine("SET NOCOUNT OFF ");
			string queryString2 = stringBuilder.ToString();
			stringBuilder.Clear();
			SqlTransaction sqlTransaction2 = database.BeginTransaction();
			try
			{
				database.ExecuteCommand(queryString2, sqlTransaction2);
				try
				{
					int num = 0;
					string text13 = string.Empty;
					string text14 = string.Empty;
					if (lineId != 0)
					{
						text13 = " Where LineID = " + lineId;
						text14 = " And pplLineID = " + lineId;
					}
					num = database.ExecuteCommand("INSERT INTO PurchasePlannerLines (pplSessionID, pplLineID, pplPlantID, pplWarehouseID, pplPartID, pplPartRevisionID, pplLastRunDate, pplReorderMethod, pplPartShortDescription,  pplLotSize, pplMinimumQuantity, pplMaximumQuantity, pplQuantityOnHand, pplNonStockedItem, pplPhantomOrKitPart, pplCreatedBy, pplCreatedDate) Select SessionID, LineID, PLANT, WH, jmpPartID, PartRevisionID, imrLastRunDatePurchasePlanner, REORDER_METHOD, DESCRIPTION,  LotSize, MinimumQty, MaximumQty, QuantityOnHand, impNonStockedItem, impPhantomOrKitPart, " + database.User.ID.ToSql() + " as CreatedBy, " + DateTime.Today.ToSql() + " as CreatedDate  from #PurchasePlannerLines" + sessionId.ToString() + text13, sqlTransaction2);
					if (num > 0)
					{
						database.ExecuteCommand("INSERT INTO PurchasePlannerRequirements (pprSessionID, pprLineID, pprRequirementID, pprPurchaseType, pprJobID, pprJobAssemblyID, pprJobMaterialID,  pprSalesOrderID, pprSalesOrderLineID, pprSalesOrderDeliveryID, pprPurchaseOrderID, pprDueDate, pprPurchaseOrderDate, pprPurchaseToJobQuantity, pprPullFromStockQuantity,  pprPlannedReceiptQuantity, pprPlannedRequirementQuantity, pprProjectedBalance, pprSource, pprCreatedBy, pprCreatedDate) Select SessionID, LineID, RequirementID, PurchaseType, JOBID, ASM, SEQ,  ORDERID, ORDERLINE, ORDERDELIVERY, PO, DUEDATE, PODATE, PURCHASETOJOB, PULLFROMSTOCK,  SupplyQty, DemandQty, ProjectedBalance, Source, " + database.User.ID.ToSql() + " as CreatedBy, " + DateTime.Today.ToSql() + " as CreatedDate  from #PurchasePlannerRequirements" + sessionId.ToString() + text13, sqlTransaction2);
						database.ExecuteCommand("INSERT INTO PurchasePlannerOrderDetails (ppoSessionID, ppoLineID, ppoPartWarehouseLocationID, ppoPartBinID, ppoOrderDetailID, ppoPurchaseType, ppoSupplierOrganizationID, ppoPurchaseLocationID, ppoDataMissing,  ppoPartID, ppoPartRevisionID, ppoInventoryUnitOfMeasure, ppoPurchaseUnitOfMeasure, ppoConversionFactor, ppoInventoryQuantity, ppoPurchaseQuantity, ppoUnitCostBase, ppoUnitCostForeign,  ppoCurrencyRateID, ppoJobID, ppoJobAssemblyID, ppoJobMaterialID, ppoSalesOrderID, ppoSalesOrderLineID, ppoSalesOrderDeliveryID, ppoDueDate, ppoProjectID, ppoProjectAreaID, ppoLeadTime, ppoCreatedBy, ppoCreatedDate) Select SessionID, LineID, WH, IsNull(BIN,'') as BIN, OrderDetailID, PurchaseType, SupplierID, LocationID, (case when RTrim(SupplierID) = '' Then 1 Else 0 End),  jmpPartID, PartRevisionID, UM, PurchaseUoM, ConversionFactor, InvQtyToBuy,  PurQtyToBuy, UnitCostBase, UnitCostForeign,  Currency, JOBID, ASM, SEQ, ORDERID, ORDERLINE, ORDERDELIVERY, DUEDATE, PROJECT, PROJECTAREA, IsNull(LeadTime,0) as LeadTime, " + database.User.ID.ToSql() + " as CreatedBy, " + DateTime.Today.ToSql() + " as CreatedDate  from #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString() + text13, sqlTransaction2);
						database.ExecuteCommand("Update PurchasePlannerLines  set pplDataMissing = isnull((select sum(isnull(ppoDataMissing,0)) from PurchasePlannerOrderDetails where ppoSessionID = pplSessionID and ppoLineID = pplLineID),0)  Where pplSessionID = " + sessionId.ToSql() + text14, sqlTransaction2);
						RefreshSupplierRequirementsFromGetData(database, sqlTransaction2, sessionId);
					}
					flag = true;
					if (lineId == 0)
					{
						if (flag)
						{
							sqlCommand = database.NewSqlCommand("Update PurchasePlannerSessions Set ppsGenerated = 1 Where ppsSessionID = @SessionID");
							sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
							database.ExecuteCommand(sqlCommand, sqlTransaction2);
							InitializeBins(database, sqlTransaction2, sessionId, empty4);
						}
						if (num == 0)
						{
							sqlCommand = database.NewSqlCommand("Update PurchasePlannerSessions Set ppsGenerated = 0 Where ppsSessionID = @SessionID");
							sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
							database.ExecuteCommand(sqlCommand, sqlTransaction2);
							MessageBox.Show("No purchase data was generated for this filter criteria. Please adjust the filters to try again.", "No Results Returned", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
				}
				finally
				{
					database.ExecuteCommand("DROP TABLE #PurchasePlannerResults" + sessionId.ToString(), sqlTransaction2);
					database.ExecuteCommand("DROP TABLE #PurchasePlannerLines" + sessionId.ToString(), sqlTransaction2);
					database.ExecuteCommand("DROP TABLE #PurchasePlannerRequirements" + sessionId.ToString(), sqlTransaction2);
					database.ExecuteCommand("DROP TABLE #PurchasePlannerPurchaseOrderDetailsPREP" + sessionId.ToString(), sqlTransaction2);
					database.ExecuteCommand("DROP TABLE #PurchasePlannerPurchaseOrderDetails" + sessionId.ToString(), sqlTransaction2);
					database.ExecuteCommand("DROP TABLE #PurchasePlannerPurchaseOrderDetailsSummed" + sessionId.ToString(), sqlTransaction2);
					database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerJobList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerJobList" + sessionId + " END", sqlTransaction2);
					database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerPartsList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerPartsList" + sessionId + " END", sqlTransaction2);
					database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerPartRevisionSuppliersList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerPartRevisionSuppliersList" + sessionId + " END", sqlTransaction2);
					database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerSalesOrderDeliveriesSuppliersList" + sessionId + " END", sqlTransaction2);
				}
			}
			catch
			{
				database.RollbackTransaction(sqlTransaction2);
				throw;
			}
			database.CommitTransaction(sqlTransaction2);
		}
		updatePricesAndLeadTimes(database, sessionId, lineId);
		database.ExecuteCommand("Update PurchasePlannerOrderDetails Set ppoExtendedCostBase = Round(ppoUnitCostBase * ppoPurchaseQuantity,2) Where ppoSessionID = " + sessionId.ToSql());
		database.ExecuteCommand("Update PurchasePlannerLines Set pplExtendedCostBase = (Select IsNull(Sum(ppoExtendedCostBase),0) from PurchasePlannerOrderDetails Where ppoSessionID = pplSessionID and ppoLineID = pplLineID) Where pplSessionID = " + sessionId.ToSql());
		database.ExecuteCommand("Update PurchasePlannerSessions Set ppsSessionSubtotalBase = (Select IsNull(Sum(pplExtendedCostBase),0) from PurchasePlannerLines Where pplSessionID = ppsSessionID) Where ppsSessionID = " + sessionId.ToSql());
		Cursor.Current = Cursors.Arrow;
		return flag;
	}

	private void InitializeBins(M1Database database, SqlTransaction transaction, string sessionId, string plantId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * from PurchasePlannerOrderDetails Where ppoSessionID = @SessionID and ppoPurchaseType = 2");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		foreach (DataRow row in database.GetDataTable(sqlCommand, transaction).Rows)
		{
			string preferredWarehouseBin = new Part().GetPreferredWarehouseBin(database, row.Field<string>("ppoPartID").ToString(), row.Field<string>("ppoPartRevisionID").ToString(), row.Field<string>("ppoPartWarehouseLocationID").ToString(), plantId);
			database.ExecuteCommand("Update PurchasePlannerOrderDetails Set ppoPartBinID = " + preferredWarehouseBin.ToSql() + " Where ppoSessionID = " + sessionId.ToSql() + " and ppoLineID = " + row.Field<int>("ppoLineID").ToSql() + " and ppoOrderDetailID = " + row.Field<int>("ppoOrderDetailID").ToSql(), transaction);
		}
	}

	private bool checkFilterOverlap(StringBuilder sB, string overlapQuery, SqlCommand command, M1Database database, DataRow sessionRow, string sessionId, string filter, string table)
	{
		string text = string.Empty;
		bool result = true;
		string text2 = "";
		sB.AppendLine("Select ppsSessionID, ppsJobIDs, ppsPartIDs, ppsPartClassIDs, ppsSupplierIDs, ppsSalesOrderIDs, " + filter + " from purchaseplannersessions ");
		if (sessionRow.Field<string>("ppsWarehouseID").Trim().ToString() == text2)
		{
			sB.AppendLine(" Where 1 = 1 ");
		}
		else
		{
			sB.AppendLine(" Where ( ppsWarehouseID = " + text2.ToSql() + " OR ppsWarehouseID = " + sessionRow.Field<string>("ppsWarehouseID").Trim().ToSql() + ") ");
		}
		sB.AppendLine(" and PurchasePlannerSessions.ppsCompleted = 0 and PurchasePlannerSessions.ppsSessionID <> @sessionID ");
		sB.AppendLine("order by PurchasePlannerSessions.ppsSessionID ");
		overlapQuery = sB.ToString();
		sB.Clear();
		command = database.NewSqlCommand(overlapQuery);
		command.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		DataTable dataTable = database.GetDataTable(command);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				bool flag = false;
				if (row[filter] == DBNull.Value)
				{
					flag = true;
				}
				else if (row.Field<string>("ppsJobIDs").ToString() == string.Empty && row.Field<string>("ppsSupplierIDs").ToString() == string.Empty && row.Field<string>("ppsPartIDs").ToString() == string.Empty && row.Field<string>("ppsPartClassIDs").ToString() == string.Empty && row.Field<string>("ppsSalesOrderIDs").ToString() == string.Empty && !filter.Equals("ppsJobIDs") && !filter.Equals("ppsSalesOrderIDs"))
				{
					flag = true;
				}
				else
				{
					flag = itemListsHaveMatches(sessionRow.Field<string>(filter).ToString(), row.Field<string>(filter).ToString());
					if (filter.ToString() == "ppsSalesOrderIDs" && row.Field<string>(filter).ToString() == string.Empty)
					{
						performExtendedOverlapCheck = true;
					}
				}
				if (flag)
				{
					text = ((!(text == string.Empty)) ? (text + ", " + row.Field<string>("ppsSessionID").ToString()) : row.Field<string>("ppsSessionID").ToString());
				}
			}
		}
		if (text != string.Empty)
		{
			MessageBox.Show("The " + table + " filter criteria selected in this session overlaps with other open Purchase Planner session(s). \n\nAs a result, there will be overlap with the following open session(s): \n" + text + "\n\nThe open session(s) should be completed first.  Otherwise, the filter criteria for the current session must be changed to avoid the overlap.  [Msg 4]", "Filter criteria overlaps with another open session", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			result = false;
			Cursor.Current = Cursors.Arrow;
		}
		return result;
	}

	private string getWarehouseQuery(DataTable whTable, string whFieldName)
	{
		string text = string.Empty;
		int count = whTable.Rows.Count;
		if (count != 0)
		{
			for (int i = 0; i < count; i++)
			{
				text = ((i != count - 1) ? (text + string.Format("{0} = {1} Or ", whFieldName, M1Util.ConvertToSql(whTable.Rows[i].Field<string>("imwWarehouseID")))) : (text + string.Format("{0} = {1}", whFieldName, M1Util.ConvertToSql(whTable.Rows[i].Field<string>("imwWarehouseID")))));
			}
			if (count == 1)
			{
				return $" And {text} ";
			}
			return $" And ( {text} ) ";
		}
		return $" And {whFieldName} = {M1Util.ConvertToSql(text)} ";
	}

	private string splitAndConvert(string ids)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrWhiteSpace(ids))
		{
			string[] array = ids.Split('\r');
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(text.ToSql());
				}
			}
		}
		return stringBuilder.ToString();
	}

	private static string convertDataTableToString(DataTable dataTable)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in dataTable.Rows)
		{
			stringBuilder.Append(row[0].ToString() + "\r");
		}
		return stringBuilder.ToString();
	}

	private static string GetPartsFromJobs(M1Database database, string jobIdList, string sessionId)
	{
		try
		{
			database.ExecuteCommand("SELECT jmpJobID as JobId INTO PurchasePlannerJobList" + sessionId + " FROM Jobs WHERE jmpJobID IN (" + jobIdList + ")");
			string text = "SELECT distinct jmmPartID as PartID FROM JobMaterials WHERE EXISTS (SELECT distinct JobId FROM PurchasePlannerJobList" + sessionId + " WHERE JobId = JobMaterials.jmmJobID) UNION ALL SELECT distinct jmaPartID as PartID FROM JobAssemblies WHERE jmaJobAssemblyID <> 0 and EXISTS (SELECT distinct JobId FROM PurchasePlannerJobList" + sessionId + " WHERE JobId = JobAssemblies.jmaJobID)";
			database.ExecuteCommand("SELECT PartID INTO PurchasePlannerPartsList" + sessionId + " FROM (" + text + ") As Test");
			return "(SELECT PartID FROM PurchasePlannerPartsList" + sessionId + ")";
		}
		catch (Exception)
		{
			database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerJobList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerJobList" + sessionId + " END");
			database.ExecuteCommand("IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PurchasePlannerPartsList" + sessionId + "')) BEGIN DROP TABLE PurchasePlannerPartsList" + sessionId + " END");
		}
		return string.Empty;
	}

	public bool MissingLastRunDates(M1Database database)
	{
		if (Convert.ToInt32(database.ExecuteScalar("Select Isnull(Count(*),0) From PartRevisions Where imrLastRunDatePurchasePlanner Is Null")) > 0 || Convert.ToInt32(database.ExecuteScalar("Select Isnull(Count(*),0) From PartWarehouseLocations Where imlLastRunDatePurchasePlanner Is Null")) > 0)
		{
			return true;
		}
		return false;
	}

	public string VerifyInactiveParts(M1Database database, string sessionId)
	{
		List<string> list = new List<string>();
		string queryString = "SELECT pplLineID, pplPartID, pplPartRevisionID, imrEffectiveEndDate\r\n                        FROM PurchasePlannerLines \r\n\t                        LEFT OUTER JOIN PartRevisions ON pplPartID = imrPartID AND pplPartRevisionID = imrPartRevisionID\r\n                        WHERE pplSessionID = @SessionID\r\n\t                        AND pplLineID in (SELECT ppoLineID FROM PurchasePlannerOrderDetails WHERE ppoSessionID = @SessionID)\r\n\t                        AND GETDATE() > imrEffectiveEndDate;";
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				int num = row.Field<int>("pplLineID");
				string text = row.Field<string>("pplPartID");
				string text2 = row.Field<string>("pplPartRevisionID");
				DateTime dateTime = row.Field<DateTime>("imrEffectiveEndDate");
				string item = $"Part Revision is inactive. [Line: '{num}', Part: '{text}', Revision: '{text2}', Effective End Date: '{dateTime.ToShortDateString()}']";
				list.Add(item);
			}
		}
		return string.Join("\n", list);
	}

	public string MissingSuppliers(M1Database database, string sessionId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Declare @string nvarchar(255) ");
		stringBuilder.AppendLine("Select @string = coalesce(@string + ',', '') + cast(pplLineID as nvarchar(5)) From PurchasePlannerLines where pplSessionID = " + sessionId.ToSql());
		stringBuilder.AppendLine(" and pplLineID in (select ppoLineID from PurchasePlannerOrderDetails Where RTrim(ppoSupplierOrganizationID) = '' and ppoSessionID = " + sessionId.ToSql() + ")");
		stringBuilder.AppendLine("Select @string");
		string queryString = stringBuilder.ToString();
		return Convert.ToString(database.ExecuteScalar(queryString));
	}

	private static bool itemListsHaveMatches(string currentSessionItems, string otherSessionItems)
	{
		List<string> source = splitAndTrimCarriageReturnDelimitedString(currentSessionItems);
		List<string> otherSessionItemsList = splitAndTrimCarriageReturnDelimitedString(otherSessionItems);
		return source.Any((string currentSessionItem) => otherSessionItemsList.Any((string otherSessionItem) => currentSessionItem == otherSessionItem));
	}

	private static List<string> splitAndTrimCarriageReturnDelimitedString(string line)
	{
		return (from s in line.Split('\r')
			select s.Trim()).ToList();
	}

	private static bool updatePricesAndLeadTimes(M1Database database, string sessionId, int lineId)
	{
		bool flag = database.Props("PM").Field<bool>("xapPMPurPlannerUseBestPrice");
		SqlCommand sqlCommand = ((lineId == 0) ? (flag ? database.NewSqlCommand("select * from PurchasePlannerOrderDetails Where ppoSessionID = @SessionID ") : database.NewSqlCommand("select * from PurchasePlannerOrderDetails Where (ppoPurchaseType = 2 or (ppoPurchaseType = 3 and ppoSupplierOrganizationID = '' and ppoUnitCostBase = 0) ) and ppoSessionID = @SessionID ")) : (flag ? database.NewSqlCommand("select * from PurchasePlannerOrderDetails Where ppoSessionID = @SessionID and ppoLineID = @LineID ") : database.NewSqlCommand("select * from PurchasePlannerOrderDetails Where (ppoPurchaseType = 2 or (ppoPurchaseType = 3 and ppoSupplierOrganizationID = '' and ppoUnitCostBase = 0) ) and ppoSessionID = @SessionID and ppoLineID = @LineID ")));
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter, null);
		sqlCommand = ((lineId != 0) ? database.NewSqlCommand("select ppoSessionID,ppoLineID,ppoSupplierOrganizationID,ppoPurchaseLocationID,ppoPartID,ppoPartRevisionID,ppoPartWarehouseLocationID,ppoPartBinID,ppoPurchaseQuantity,ppoInventoryQuantity from PurchasePlannerOrderDetails Where ppoSessionID = @SessionID and ppoLineID = @LineID And ppoSupplierOrganizationID <> '' ") : database.NewSqlCommand("select ppoSessionID,ppoLineID,ppoSupplierOrganizationID,ppoPurchaseLocationID,ppoPartID,ppoPartRevisionID,ppoPartWarehouseLocationID,ppoPartBinID,ppoPurchaseQuantity,ppoInventoryQuantity from PurchasePlannerOrderDetails Where ppoSessionID = @SessionID And ppoSupplierOrganizationID <> '' "));
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		var source = from r in database.GetDataTable(sqlCommand).AsEnumerable()
			group r by new
			{
				SessionID = r.Field<string>("ppoSessionID"),
				LineID = r.Field<int>("ppoLineID"),
				Supplier = r.Field<string>("ppoSupplierOrganizationID"),
				Location = r.Field<string>("ppoPurchaseLocationID"),
				Part = r.Field<string>("ppoPartID"),
				Revision = r.Field<string>("ppoPartRevisionID"),
				WH = r.Field<string>("ppoPartWarehouseLocationID"),
				Bin = r.Field<string>("ppoPartBinID")
			} into g
			select new
			{
				SessionID = g.Key.SessionID,
				LineID = g.Key.LineID,
				Supplier = g.Key.Supplier,
				Location = g.Key.Location,
				Part = g.Key.Part,
				Revision = g.Key.Revision,
				WH = g.Key.WH,
				Bin = g.Key.Bin,
				PurQty = g.Sum((DataRow x) => x.Field<decimal>("ppoPurchaseQuantity")),
				InvQty = g.Sum((DataRow x) => x.Field<decimal>("ppoInventoryQuantity"))
			};
		Part part = new Part();
		foreach (DataRow row in dataTable.Rows)
		{
			PriceCalculation priceCalculation = null;
			var anon = source.FirstOrDefault(x => x.Supplier.Equals(row.Field<string>("ppoSupplierOrganizationID")) && x.Location.Equals(row.Field<string>("ppoPurchaseLocationID")) && x.Part.Equals(row.Field<string>("ppoPartID")) && x.Revision.Equals(row.Field<string>("ppoPartRevisionID")) && x.WH.Equals(row.Field<string>("ppoPartWarehouseLocationID")) && x.Bin.Equals(row.Field<string>("ppoPartBinID")));
			priceCalculation = ((anon == null) ? part.GetPurchasePrice(database, row.Field<string>("ppoPartID"), row.Field<string>("ppoPartRevisionID"), row.Field<string>("ppoSupplierOrganizationID"), row.Field<string>("ppoPurchaseLocationID"), row.Field<decimal>("ppoInventoryQuantity"), "MATERIAL", row.Field<string>("ppoCurrencyRateID"), DateTime.Now, row.Field<decimal>("ppoPurchaseQuantity"), null) : part.GetPurchasePrice(database, row.Field<string>("ppoPartID"), row.Field<string>("ppoPartRevisionID"), row.Field<string>("ppoSupplierOrganizationID"), row.Field<string>("ppoPurchaseLocationID"), anon.InvQty, "MATERIAL", row.Field<string>("ppoCurrencyRateID"), DateTime.Now, anon.PurQty, null));
			if (!(priceCalculation.FullPrice != 0m))
			{
				continue;
			}
			decimal exchangeRate = database.GetExchangeRate(row.Field<string>("ppoCurrencyRateID"), DateTime.Now);
			if (priceCalculation.IsForeignCurrency)
			{
				if (priceCalculation.FullPrice != row.Field<decimal>("ppoUnitCostForeign") && (!flag || (flag && (priceCalculation.FullPrice <= row.Field<decimal>("ppoUnitCostForeign") || row.Field<decimal>("ppoUnitCostForeign").Equals(0m)))))
				{
					row.SetField("ppoUnitCostForeign", priceCalculation.FullPrice);
					row.SetField("ppoUnitCostBase", M1Math.Round(priceCalculation.FullPrice / exchangeRate, 5));
				}
			}
			else if (priceCalculation.FullPrice != row.Field<decimal>("ppoUnitCostBase") && (!flag || (flag && (priceCalculation.FullPrice <= row.Field<decimal>("ppoUnitCostBase") || row.Field<decimal>("ppoUnitCostBase").Equals(0m)))))
			{
				row.SetField("ppoUnitCostBase", priceCalculation.FullPrice);
				row.SetField("ppoUnitCostForeign", M1Math.Round(priceCalculation.FullPrice * exchangeRate, 5));
			}
			if (priceCalculation.LeadTime != row.Field<short>("ppoLeadTime") && (!flag || (flag && (priceCalculation.FullPrice <= row.Field<decimal>("ppoUnitCostForeign") || row.Field<decimal>("ppoUnitCostForeign").Equals(0m)))))
			{
				row.SetField("ppoLeadTime", (decimal)priceCalculation.LeadTime);
			}
			if (priceCalculation.CurrencyID != row.Field<string>("ppoCurrencyRateID") && (!flag || (flag && (priceCalculation.FullPrice <= row.Field<decimal>("ppoUnitCostForeign") || row.Field<decimal>("ppoUnitCostForeign").Equals(0m)))))
			{
				if (!string.IsNullOrWhiteSpace(priceCalculation.CurrencyID) || string.IsNullOrWhiteSpace(database.HomeCurrencyID))
				{
					row.SetField("ppoCurrencyRateID", priceCalculation.CurrencyID);
				}
				else
				{
					row.SetField("ppoCurrencyRateID", database.HomeCurrencyID);
				}
			}
			if (priceCalculation.ConversionFactor != row.Field<decimal>("ppoConversionFactor") && (!flag || (flag && (priceCalculation.FullPrice <= row.Field<decimal>("ppoUnitCostForeign") || row.Field<decimal>("ppoUnitCostForeign").Equals(0m)))))
			{
				row.SetField("ppoConversionFactor", priceCalculation.ConversionFactor);
			}
		}
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			database.UpdateData(dataTable, adapter, sqlTransaction);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
		return true;
	}

	public void AddSupplierRequirementsLine(M1BindingSource detailsBs, DataRow parentRow)
	{
		foreach (DataRowView detailsB in detailsBs)
		{
			if (detailsB.Row.Field<bool>("ppoSupplierRequirement"))
			{
				detailsB.Delete();
			}
		}
		DataTable dataTable = detailsBs.GetDataView(parentRow).ToTable();
		M1Database database = detailsBs.Database;
		if (dataTable == null)
		{
			return;
		}
		foreach (var item in from r in dataTable.AsEnumerable()
			where !string.IsNullOrWhiteSpace(r.Field<string>("ppoSupplierOrganizationID"))
			group r by new
			{
				SessionID = r.Field<string>("ppoSessionID"),
				LineID = r.Field<int>("ppoLineID"),
				Supplier = r.Field<string>("ppoSupplierOrganizationID"),
				Location = r.Field<string>("ppoPurchaseLocationID"),
				Part = r.Field<string>("ppoPartID"),
				Revision = r.Field<string>("ppoPartRevisionID"),
				WH = r.Field<string>("ppoPartWarehouseLocationID"),
				Bin = r.Field<string>("ppoPartBinID")
			} into g
			select new
			{
				SessionID = g.Key.SessionID,
				LineID = g.Key.LineID,
				Supplier = g.Key.Supplier,
				Location = g.Key.Location,
				Part = g.Key.Part,
				Revision = g.Key.Revision,
				WH = g.Key.WH,
				Bin = g.Key.Bin,
				PurQty = g.Sum((DataRow x) => x.Field<decimal>("ppoPurchaseQuantity")),
				InvQty = g.Sum((DataRow x) => x.Field<decimal>("ppoInventoryQuantity"))
			})
		{
			Part.SupplierRequirement supplierRequirements = new Part().GetSupplierRequirements(database, null, item.Part, item.Revision, item.Supplier, item.Location);
			if (supplierRequirements != null)
			{
				decimal num = default(decimal);
				decimal minPurQty = supplierRequirements.MinPurQty;
				decimal lotSize = supplierRequirements.LotSize;
				num = ((!(item.PurQty < minPurQty)) ? item.PurQty : minPurQty);
				if (lotSize > 0m && num % lotSize > 0m)
				{
					num = Math.Floor(num / lotSize) * lotSize + lotSize;
				}
				if (num - item.PurQty > 0m)
				{
					DataRow dataRow = detailsBs.AddNew() as DataRow;
					detailsBs.SetKeyToNextAvailable(dataRow);
					dataRow["ppoSupplierOrganizationID"] = item.Supplier;
					dataRow["ppoPurchaseLocationID"] = item.Location;
					dataRow["ppoPurchaseQuantity"] = num - item.PurQty;
					PriceCalculation priceCalculation = null;
					priceCalculation = new Part().GetPurchasePrice(database, dataRow.Field<string>("ppoPartID"), dataRow.Field<string>("ppoPartRevisionID"), dataRow.Field<string>("ppoSupplierOrganizationID"), dataRow.Field<string>("ppoPurchaseLocationID"), dataRow.Field<decimal>("ppoInventoryQuantity") + item.InvQty, "MATERIAL", dataRow.Field<string>("ppoCurrencyRateID"), DateTime.Now, dataRow.Field<decimal>("ppoPurchaseQuantity") + item.PurQty, null);
					setPricing(database, dataRow, priceCalculation);
					priceCalculation = null;
					dataRow["ppoDueDate"] = DateTime.Now;
					dataRow["ppoSupplierRequirement"] = true;
				}
			}
		}
	}

	public void RefreshOrderDetailsPricing(M1BindingSource detailsBs, DataRow parentRow, DataRow supplierRow, bool applyToAllLines)
	{
		DataTable dataTable = detailsBs.GetDataView(parentRow).ToTable();
		M1Database database = detailsBs.Database;
		if (dataTable == null)
		{
			return;
		}
		var source = from r in dataTable.AsEnumerable()
			where !string.IsNullOrWhiteSpace(r.Field<string>("ppoSupplierOrganizationID"))
			group r by new
			{
				SessionID = r.Field<string>("ppoSessionID"),
				LineID = r.Field<int>("ppoLineID"),
				Supplier = r.Field<string>("ppoSupplierOrganizationID"),
				Location = r.Field<string>("ppoPurchaseLocationID"),
				Part = r.Field<string>("ppoPartID"),
				Revision = r.Field<string>("ppoPartRevisionID"),
				WH = r.Field<string>("ppoPartWarehouseLocationID"),
				Bin = r.Field<string>("ppoPartBinID")
			} into g
			select new
			{
				SessionID = g.Key.SessionID,
				LineID = g.Key.LineID,
				Supplier = g.Key.Supplier,
				Location = g.Key.Location,
				Part = g.Key.Part,
				Revision = g.Key.Revision,
				WH = g.Key.WH,
				Bin = g.Key.Bin,
				PurQty = g.Sum((DataRow x) => x.Field<decimal>("ppoPurchaseQuantity")),
				InvQty = g.Sum((DataRow x) => x.Field<decimal>("ppoInventoryQuantity"))
			};
		database.Props("PM").Field<bool>("xapPMPurPlannerUseBestPrice");
		Part part = new Part();
		if (applyToAllLines)
		{
			foreach (DataRowView item in detailsBs.GetDataView(parentRow))
			{
				DataRow row = item.Row;
				PriceCalculation priceCalculation = null;
				var anon = source.FirstOrDefault(x => x.Supplier.Equals(row.Field<string>("ppoSupplierOrganizationID")) && x.Location.Equals(row.Field<string>("ppoPurchaseLocationID")) && x.Part.Equals(row.Field<string>("ppoPartID")) && x.Revision.Equals(row.Field<string>("ppoPartRevisionID")) && x.WH.Equals(row.Field<string>("ppoPartWarehouseLocationID")) && x.Bin.Equals(row.Field<string>("ppoPartBinID")));
				setPricing(priceData: (anon == null) ? part.GetPurchasePrice(database, row.Field<string>("ppoPartID"), row.Field<string>("ppoPartRevisionID"), row.Field<string>("ppoSupplierOrganizationID"), row.Field<string>("ppoPurchaseLocationID"), row.Field<decimal>("ppoInventoryQuantity"), "MATERIAL", supplierRow.Field<string>("imiCurrencyRateID"), DateTime.Now, row.Field<decimal>("ppoPurchaseQuantity"), null) : part.GetPurchasePrice(database, row.Field<string>("ppoPartID"), row.Field<string>("ppoPartRevisionID"), row.Field<string>("ppoSupplierOrganizationID"), row.Field<string>("ppoPurchaseLocationID"), anon.InvQty, "MATERIAL", supplierRow.Field<string>("imiCurrencyRateID"), DateTime.Now, anon.PurQty, null), database: database, row: row);
			}
		}
		else
		{
			DataRow row2 = detailsBs.CurrentAsDataRow;
			if (row2 != null)
			{
				PriceCalculation priceCalculation2 = null;
				var anon2 = source.FirstOrDefault(x => x.Supplier.Equals(row2.Field<string>("ppoSupplierOrganizationID")) && x.Location.Equals(row2.Field<string>("ppoPurchaseLocationID")) && x.Part.Equals(row2.Field<string>("ppoPartID")) && x.Revision.Equals(row2.Field<string>("ppoPartRevisionID")) && x.WH.Equals(row2.Field<string>("ppoPartWarehouseLocationID")) && x.Bin.Equals(row2.Field<string>("ppoPartBinID")));
				setPricing(priceData: (anon2 == null) ? part.GetPurchasePrice(database, row2.Field<string>("ppoPartID"), row2.Field<string>("ppoPartRevisionID"), row2.Field<string>("ppoSupplierOrganizationID"), row2.Field<string>("ppoPurchaseLocationID"), row2.Field<decimal>("ppoInventoryQuantity"), "MATERIAL", supplierRow.Field<string>("imiCurrencyRateID"), DateTime.Now, row2.Field<decimal>("ppoPurchaseQuantity"), null) : part.GetPurchasePrice(database, row2.Field<string>("ppoPartID"), row2.Field<string>("ppoPartRevisionID"), row2.Field<string>("ppoSupplierOrganizationID"), row2.Field<string>("ppoPurchaseLocationID"), anon2.InvQty, "MATERIAL", supplierRow.Field<string>("imiCurrencyRateID"), DateTime.Now, anon2.PurQty, null), database: database, row: row2);
			}
		}
		part = null;
	}

	private void setPricing(M1Database database, DataRow row, PriceCalculation priceData)
	{
		if (priceData == null || !(priceData.FullPrice != 0m))
		{
			return;
		}
		if (priceData.LeadTime != row.Field<short>("ppoLeadTime"))
		{
			row.SetField("ppoLeadTime", (decimal)priceData.LeadTime);
		}
		if (priceData.CurrencyID != row.Field<string>("ppoCurrencyRateID"))
		{
			row.SetField("ppoCurrencyRateID", priceData.CurrencyID);
		}
		if (priceData.ConversionFactor != row.Field<decimal>("ppoConversionFactor"))
		{
			row.SetField("ppoConversionFactor", priceData.ConversionFactor);
		}
		decimal exchangeRate = database.GetExchangeRate(priceData.CurrencyID, DateTime.Now);
		if (priceData.IsForeignCurrency)
		{
			if (priceData.FullPrice != row.Field<decimal>("ppoUnitCostForeign"))
			{
				row.SetField("ppoUnitCostForeign", priceData.FullPrice);
				row.SetField("ppoUnitCostBase", M1Math.Round(priceData.FullPrice / exchangeRate, 5));
			}
		}
		else if (priceData.FullPrice != row.Field<decimal>("ppoUnitCostBase"))
		{
			row.SetField("ppoUnitCostBase", priceData.FullPrice);
			row.SetField("ppoUnitCostForeign", M1Math.Round(priceData.FullPrice * exchangeRate, 5));
		}
	}

	public void RefreshSupplierRequirementsFromGetData(M1Database database, SqlTransaction transaction, string sessionId)
	{
		DataTable dataTable = database.GetDataTable($"Select * From PurchasePlannerOrderDetails Where ppoSessionID = {sessionId.ToSql()}", transaction);
		if (dataTable == null)
		{
			return;
		}
		foreach (var item in from r in dataTable.AsEnumerable()
			where !string.IsNullOrWhiteSpace(r.Field<string>("ppoSupplierOrganizationID"))
			group r by new
			{
				SessionID = r.Field<string>("ppoSessionID"),
				LineID = r.Field<int>("ppoLineID"),
				Supplier = r.Field<string>("ppoSupplierOrganizationID"),
				Location = r.Field<string>("ppoPurchaseLocationID"),
				Part = r.Field<string>("ppoPartID"),
				Revision = r.Field<string>("ppoPartRevisionID"),
				WH = r.Field<string>("ppoPartWarehouseLocationID"),
				Bin = r.Field<string>("ppoPartBinID")
			} into g
			select new
			{
				SessionID = g.Key.SessionID,
				LineID = g.Key.LineID,
				Supplier = g.Key.Supplier,
				Location = g.Key.Location,
				Part = g.Key.Part,
				Revision = g.Key.Revision,
				WH = g.Key.WH,
				Bin = g.Key.Bin,
				PurQty = g.Sum((DataRow x) => x.Field<decimal>("ppoPurchaseQuantity")),
				InvQty = g.Sum((DataRow x) => x.Field<decimal>("ppoInventoryQuantity"))
			})
		{
			decimal num = default(decimal);
			Part.SupplierRequirement supplierRequirements = new Part().GetSupplierRequirements(database, transaction, item.Part, item.Revision, item.Supplier, item.Location);
			if (supplierRequirements == null)
			{
				continue;
			}
			decimal minPurQty = supplierRequirements.MinPurQty;
			decimal lotSize = supplierRequirements.LotSize;
			num = ((!(item.PurQty < minPurQty)) ? item.PurQty : minPurQty);
			if (lotSize > 0m && num % lotSize > 0m)
			{
				num = Math.Floor(num / lotSize) * lotSize + lotSize;
			}
			if (!(num - item.PurQty > 0m))
			{
				continue;
			}
			using M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
			m1BindingSource.DataSourceTable = "PurchasePlannerLines";
			m1BindingSource.NavigateTo(database, "pplSessionID = " + M1Util.ConvertToSql(item.SessionID) + " And pplLineID = " + M1Util.ConvertToSql(item.LineID));
			if (m1BindingSource.Count == 0)
			{
				continue;
			}
			using M1BindingSource m1BindingSource2 = m1BindingSource.PrimaryTable.GetChildBindingSource("PurchasePlannerOrderDetails");
			if (m1BindingSource2 != null)
			{
				DataRow dataRow = m1BindingSource2.AddNew() as DataRow;
				m1BindingSource2.SetKeyToNextAvailable(dataRow);
				dataRow["ppoSupplierOrganizationID"] = item.Supplier;
				dataRow["ppoPurchaseLocationID"] = item.Location;
				dataRow["ppoPurchaseQuantity"] = num - item.PurQty;
				dataRow["ppoSupplierRequirement"] = true;
				dataRow["ppoDueDate"] = DateTime.Now;
				m1BindingSource2.SaveData();
			}
		}
	}

	public void CreateNewSession(IServiceProvider provider, M1Database database, string sourceTable, object[] topLevelIDs)
	{
		object[] defaultValues = new object[0];
		IOpenObject openObject = provider.GetService(typeof(IOpenObject)) as IOpenObject;
		sourceTable = sourceTable.ToLower();
		if (topLevelIDs != null && topLevelIDs.Length != 0)
		{
			string text = string.Empty;
			object[] array = new object[0];
			string empty = string.Empty;
			string fieldId = string.Empty;
			string text2 = string.Empty;
			try
			{
				array = topLevelIDs.Select((object levelId) => (levelId as object[])[0]).ToArray();
			}
			catch
			{
				array = new object[0];
			}
			string text3 = string.Join("\r", array);
			switch (sourceTable)
			{
			case "jobs":
				fieldId = "jmpJobID";
				text2 = "ppsJobIDs";
				empty = string.Join(",", array.Select((object topId) => topId.ToSql()));
				text = "SELECT jmpJobID FROM Jobs WHERE jmpJobID IN (" + empty + ") AND jmpClosed = 0";
				break;
			case "salesorders":
				fieldId = "ompSalesOrderID";
				text2 = "ppsSalesOrderIDs";
				empty = string.Join(",", array.Select((object topId) => topId.ToSql()));
				text = "SELECT ompSalesOrderID FROM SalesOrders WHERE ompSalesOrderID IN (" + empty + ") AND ompClosed = 0";
				break;
			case "parts":
				fieldId = "impPartID";
				text2 = "ppsPartIDs";
				empty = string.Join(",", array.Select((object topId) => topId.ToSql()));
				text = "SELECT impPartID FROM Parts WHERE impPartID IN (" + empty + ") AND impInactive = 0";
				break;
			case "partclasses":
				fieldId = "imcPartClassID";
				text2 = "ppsPartClassIDs";
				empty = string.Join(",", array.Select((object topId) => topId.ToSql()));
				text = "SELECT imcPartClassID FROM PartClasses WHERE imcPartClassID IN (" + empty + ") AND imcInactive = 0";
				break;
			case "organizations":
				fieldId = "cmoOrganizationID";
				text2 = "ppsSupplierIDs";
				empty = string.Join(",", array.Select((object topId) => topId.ToSql()));
				text = "SELECT cmoOrganizationID FROM Organizations WHERE cmoOrganizationID IN (" + empty + ") AND cmoSupplierStatus = 2";
				break;
			}
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
			{
				using SqlCommand sqlCommand = new SqlCommand(text);
				DataTable dataTable = database.GetDataTable(sqlCommand);
				text3 = string.Join("\r", from row in dataTable.AsEnumerable()
					select row[fieldId]);
				defaultValues = new object[2] { text2, text3 };
			}
		}
		openObject.OpenObject("PURCHASEPLANNER", null, string.Empty, newForm: false, string.Empty, null, defaultValues);
	}

	public void CompletePurchasePlanner(M1BindingSource plannerBindingSource)
	{
		plannerBindingSource.CurrentAsDataRow.SetField("ppsCompleted", value: true);
		plannerBindingSource.SaveData();
	}
}
