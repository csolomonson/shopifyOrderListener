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

public class InventoryCount
{
	public bool Generate(M1Database database, int countID)
	{
		bool flag = false;
		int num = 0;
		SqlCommand sqlCommand = database.NewSqlCommand("Select imnExcludeInactivePartBins,imnCycleCodeID,imnPartWarehouseIDs,imnPartBinIDs,imnPartGroupIDs,imnPartClassIDs,imnSupplierOrganizationIDs,imnIncludeBlankPartClass,imnIncludeBlankPartGroup From InventoryCounts Where imnInventoryCountID = @CountID");
		sqlCommand.Parameters.Add(new SqlParameter("@CountID", SqlDbType.Int)).Value = countID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			string text = splitAndConvert(row.Field<string>("imnPartWarehouseIDs"));
			string text2 = row.Field<string>("imnPartBinIDs");
			string text3 = splitAndConvert(row.Field<string>("imnPartGroupIDs"));
			bool num2 = row.Field<bool>("imnExcludeInactivePartBins");
			if (row.Field<bool>("imnIncludeBlankPartGroup"))
			{
				if (!string.IsNullOrWhiteSpace(text3))
				{
					text3 += ",";
				}
				text3 += "''";
			}
			string text4 = splitAndConvert(row.Field<string>("imnPartClassIDs"));
			if (row.Field<bool>("imnIncludeBlankPartClass"))
			{
				if (!string.IsNullOrWhiteSpace(text4))
				{
					text4 += ",";
				}
				text4 += "''";
			}
			string text5 = splitAndConvert(row.Field<string>("imnSupplierOrganizationIDs"));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" WHERE impInactive = 0 And impNonStockedItem = 0 And IsNull(imrEffectiveStartDate,'19000101') <= GetDate() And IsNull(imrEffectiveEndDate,'20990101') >= GetDate() ");
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imnCycleCodeID")))
			{
				stringBuilder.Append(" And impCycleCodeID = " + row.Field<string>("imnCycleCodeID").ToSql());
			}
			if (!string.IsNullOrWhiteSpace(text4))
			{
				stringBuilder.Append(" And impPartClassID IN (" + text4 + ")");
			}
			if (!string.IsNullOrWhiteSpace(text3))
			{
				stringBuilder.Append(" And impPartGroupID IN (" + text3 + ")");
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				stringBuilder.Append(" And imbWarehouseID IN (" + text + ")");
			}
			if (!string.IsNullOrWhiteSpace(text5))
			{
				stringBuilder.Append(" And imrSupplierOrganizationID IN (" + text5 + ")");
			}
			if (num2)
			{
				stringBuilder.Append(" And (imbInactiveBin = 0 Or ( imbInactiveBin = 1 And imbQuantityOnHand > 0))");
			}
			if (!string.IsNullOrEmpty(text2))
			{
				Dictionary<string, HashSet<string>> warehousesAndBins = new Dictionary<string, HashSet<string>>();
				(from text6 in text2.Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
					select text6.ToString().Split(new string[1] { "\t" }, StringSplitOptions.None) into array
					select new
					{
						warehouseID = array[0].ToString(),
						partBinID = array[1].ToString()
					}).ToList().ForEach(anon =>
				{
					if (!warehousesAndBins.ContainsKey(anon.warehouseID))
					{
						warehousesAndBins.Add(anon.warehouseID, new HashSet<string> { anon.partBinID });
					}
					else
					{
						warehousesAndBins[anon.warehouseID].Add(anon.partBinID);
					}
				});
				stringBuilder.AppendFormat(" And ({0})", string.Join(" Or ", warehousesAndBins.Select((KeyValuePair<string, HashSet<string>> keyValuePair) => string.Format("(imbWarehouseID = {0} AND imbPartBinID IN ({1}))", keyValuePair.Key.ToSql(), string.Join(",", keyValuePair.Value.Select((string binID) => binID.ToSql()))))));
			}
			sqlCommand = database.NewSqlCommand("Delete From InventoryCountLines Where imqInventoryCountID = @CountID");
			sqlCommand.Parameters.Add(new SqlParameter("@CountID", SqlDbType.Int)).Value = countID;
			database.ExecuteCommand(sqlCommand);
			SqlTransaction sqlTransaction = database.BeginTransaction();
			try
			{
				database.ExecuteCommand("SELECT " + countID.ToSql() + " As imqInventoryCountID,IDENTITY(int,1,1) As imqInventoryCountLineID,imrPartID As imqPartID,imrPartRevisionID as imqPartRevisionID,imrShortDescription as imqPartShortDescription,imbWarehouseID as imqPartWarehouseLocationID,imbPartBinID as imqPartBinID,imbDescription As imqBinDescription,imrQuantityOnHand as imqQuantityOnHand,impPartClassID as imqPartClassID," + database.User.ID.ToSql() + " As imqCreatedBy," + DateTime.Now.ToSql() + " As imqCreatedDate INTO #InventoryCountLines FROM (select TOP 100 PERCENT imrPartID,imrPartRevisionID,imrShortDescription,IsNull(imbWarehouseID,'') as imbWarehouseID,IsNull(imbPartBinID,'') as imbPartBinID,IsNull(imbDescription,'') As imbDescription,IsNull(imbQuantityOnHand,imrQuantityOnHand) as imrQuantityOnHand,isNull(impPartClassID,'') as impPartClassID from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartBins on imrPartID=imbPartID And imrPartRevisionID = imbPartRevisionID " + stringBuilder.ToString() + " ORDER BY imbWarehouseID,imbPartBinID,imrPartID,imrPartRevisionID) as test ORDER BY imqPartWarehouseLocationID,imqPartBinID,imqPartID,imqPartRevisionID", sqlTransaction);
				try
				{
					if ((int)database.ExecuteScalar("Select Count(*) from #InventoryCountLines inner join InventoryCountLines on #InventoryCountLines.imqPartID = InventoryCountLines.imqPartID and #InventoryCountLines.imqPartRevisionID = InventoryCountLines.imqPartRevisionID and #InventoryCountLines.imqPartWarehouseLocationID = InventoryCountLines.imqPartWarehouseLocationID and #InventoryCountLines.imqPartBinID = InventoryCountLines.imqPartBinID Inner Join InventoryCounts on InventoryCountLines.imqInventoryCountID = InventoryCounts.imnInventoryCountID where InventoryCounts.imnPostedToInventory = 0", sqlTransaction) != 0)
					{
						switch (MessageBox.Show("There are parts to be included in this count which are on a Count in progress, do you wish to add them to this count also?", "Parts included in more than one open inventory count", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
						{
						case DialogResult.Yes:
							num = database.ExecuteCommand("INSERT INTO InventoryCountLines (imqInventoryCountID,imqInventoryCountLineID,imqPartID,imqPartRevisionID,imqPartShortDescription,imqPartWarehouseLocationID,imqPartBinID,imqBinDescription,imqQuantityOnHand,imqPartClassID,imqCreatedBy,imqCreatedDate) select * from #InventoryCountLines ", sqlTransaction);
							flag = true;
							break;
						case DialogResult.No:
							database.ExecuteCommand("DELETE #InventoryCountLines from #InventoryCountLines inner join InventoryCountLines on #InventoryCountLines.imqPartID = InventoryCountLines.imqPartID and #InventoryCountLines.imqPartRevisionID = InventoryCountLines.imqPartRevisionID and #InventoryCountLines.imqPartWarehouseLocationID = InventoryCountLines.imqPartWarehouseLocationID and #InventoryCountLines.imqPartBinID = InventoryCountLines.imqPartBinID Inner Join InventoryCounts on InventoryCountLines.imqInventoryCountID = InventoryCounts.imnInventoryCountID where InventoryCounts.imnPostedToInventory = 0", sqlTransaction);
							database.ExecuteCommand("SELECT imqInventoryCountID,IDENTITY(int,1,1) As imqInventoryCountLineID,imqPartID,imqPartRevisionID,imqPartShortDescription,imqPartWarehouseLocationID,imqPartBinID,imqBinDescription,imqQuantityOnHand,imqPartClassID,imqCreatedBy,imqCreatedDate INTO #InvCountWithoutOverlap FROM (Select * From #InventoryCountLines) as tempTable", sqlTransaction);
							try
							{
								num = database.ExecuteCommand("INSERT INTO InventoryCountLines (imqInventoryCountID,imqInventoryCountLineID,imqPartID,imqPartRevisionID,imqPartShortDescription,imqPartWarehouseLocationID,imqPartBinID,imqBinDescription,imqQuantityOnHand,imqPartClassID,imqCreatedBy,imqCreatedDate) Select * FROM #InvCountWithoutOverlap", sqlTransaction);
							}
							finally
							{
								database.ExecuteCommand("DROP TABLE #InvCountWithoutOverlap", sqlTransaction);
							}
							flag = true;
							break;
						case DialogResult.Cancel:
							flag = false;
							break;
						}
					}
					else
					{
						num = database.ExecuteCommand("INSERT INTO InventoryCountLines (imqInventoryCountID,imqInventoryCountLineID,imqPartID,imqPartRevisionID,imqPartShortDescription,imqPartWarehouseLocationID,imqPartBinID,imqBinDescription,imqQuantityOnHand,imqPartClassID,imqCreatedBy,imqCreatedDate) select * from #InventoryCountLines ", sqlTransaction);
						flag = true;
					}
					if (flag)
					{
						sqlCommand = database.NewSqlCommand("Update InventoryCounts Set imnRecordsGenerated = 1, imnGeneratedDate = @GenDate, imnNumberofRecordsGenerated = @Records Where imnInventoryCountID = @CountID");
						sqlCommand.Parameters.Add(new SqlParameter("@GenDate", SqlDbType.DateTime)).Value = DateTime.Now;
						sqlCommand.Parameters.Add(new SqlParameter("@CountID", SqlDbType.Int)).Value = countID;
						sqlCommand.Parameters.Add(new SqlParameter("@Records", SqlDbType.Int)).Value = num;
						database.ExecuteCommand(sqlCommand, sqlTransaction);
					}
				}
				finally
				{
					database.ExecuteCommand("DROP TABLE #InventoryCountLines", sqlTransaction);
				}
			}
			catch
			{
				database.RollbackTransaction(sqlTransaction);
				throw;
			}
			database.CommitTransaction(sqlTransaction);
		}
		return flag;
	}

	private string splitAndConvert(string ids)
	{
		new StringBuilder();
		HashSet<string> uniqueIDs = new HashSet<string>();
		if (!string.IsNullOrWhiteSpace(ids))
		{
			ids.Split('\r').ToList().ForEach(delegate(string id)
			{
				uniqueIDs.Add(id.ToSql());
			});
		}
		return string.Join(",", uniqueIDs);
	}

	public bool InventoryCountPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		string value = bindingSource.CurrentAsDataRow.Field<int>("imnInventoryCountID").ToString();
		bool result = true;
		if (!string.IsNullOrWhiteSpace(value))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select imqcounteddate,imqInventoryCountLineID from InventoryCountLines where imqInventoryCountID = @ID and imqcounteddate is not null order by imqInventoryCountLineID");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					if (row.Table.Columns.Contains("imqCountedDate"))
					{
						dateTime = row.Field<DateTime>("imqCountedDate");
					}
					if (!new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
					{
						result = false;
						break;
					}
				}
			}
		}
		return result;
	}

	private static string VerifyNegativeFinalCount(M1BindingSource bindingsource, int InventoryCountLineID, PartInformation partInformation, decimal finalCount)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (finalCount < 0m)
		{
			using SqlCommand sqlCommand = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand FROM PartBins WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID)");
			sqlCommand.Parameters.AddWithValue("@PartID", partInformation.Part);
			sqlCommand.Parameters.AddWithValue("@PartRevisionID", partInformation.PartRevision);
			sqlCommand.Parameters.AddWithValue("@WarehouseID", partInformation.PartWarehouse);
			sqlCommand.Parameters.AddWithValue("@PartBinID", partInformation.PartBin);
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
			decimal num = ((obj == null) ? 0m : Convert.ToDecimal(obj));
			stringBuilder.AppendLine($"\nQty on Hand [{num}] Final Count [{finalCount}] [Tag No: '{InventoryCountLineID}', " + "Part: '" + partInformation.Part + "', Revision: '" + partInformation.PartRevision + "', Warehouse: '" + partInformation.PartWarehouse + "', Bin: '" + partInformation.PartBin + "'].");
		}
		return stringBuilder.ToString();
	}

	public string PostInventoryCountCheck(M1BindingSource bindingsource)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (bindingsource.CurrentAsDataRow != null)
		{
			foreach (DataRowView item in bindingsource.PrimaryTable.GetChildBindingSource("InventoryCountLines").GetDataView())
			{
				decimal finalCount = item.Row.Field<decimal>("imqFinalCount");
				int inventoryCountLineID = item.Row.Field<int>("imqInventoryCountLineID");
				PartInformation partInformation = new PartInformation
				{
					Part = item.Row.Field<string>("imqPartID").Trim(),
					PartRevision = item.Row.Field<string>("imqPartRevisionID").Trim(),
					PartWarehouse = item.Row.Field<string>("imqPartWarehouseLocationID").Trim(),
					PartBin = item.Row.Field<string>("imqPartBinID").Trim()
				};
				string value = VerifyNegativeFinalCount(bindingsource, inventoryCountLineID, partInformation, finalCount);
				stringBuilder.Append(value);
			}
		}
		return stringBuilder.ToString();
	}

	public string PostInventoryCountInactiveBinsCheck(M1BindingSource bindingsource)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Part part = new Part();
		if (bindingsource.CurrentAsDataRow != null)
		{
			foreach (DataRowView item in bindingsource.PrimaryTable.GetChildBindingSource("InventoryCountLines").GetDataView())
			{
				decimal num = item.Row.Field<decimal>("imqFinalCount");
				int num2 = item.Row.Field<int>("imqInventoryCountLineID");
				PartInformation partInformation = new PartInformation
				{
					Part = item.Row.Field<string>("imqPartID").Trim(),
					PartRevision = item.Row.Field<string>("imqPartRevisionID").Trim(),
					PartWarehouse = item.Row.Field<string>("imqPartWarehouseLocationID").Trim(),
					PartBin = item.Row.Field<string>("imqPartBinID").Trim()
				};
				if (part.IsPartBinInactive(bindingsource.Database, partInformation.Part, partInformation.PartRevision, partInformation.PartWarehouse, partInformation.PartBin) && num < 0m)
				{
					stringBuilder.AppendLine(string.Format(" [Tag No. '{0}', Part ID '{1}', Warehouse '{2}', Bin '{3}', Qty on Hand '{4}', Final Count '{5}']", num2, partInformation.Part, partInformation.PartWarehouse, partInformation.PartBin, item.Row.Field<decimal>("imqQuantityOnHand"), num));
				}
			}
		}
		return stringBuilder.ToString();
	}

	public void PostInventoryCount(M1BindingSource bindingsource)
	{
		M1Database database = bindingsource.Database;
		if (bindingsource.CurrentAsDataRow == null)
		{
			return;
		}
		int num = bindingsource.CurrentAsDataRow.Field<int>("imnInventoryCountID");
		if (num == 0)
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select Count(*) from InventoryCountLines where imqInventoryCountID = @CountID And Not imqCountedDate Is Null And imqCountedDate > @CurDate");
		sqlCommand.Parameters.Add(new SqlParameter("@CountID", SqlDbType.Int)).Value = num;
		sqlCommand.Parameters.Add(new SqlParameter("@CurDate", SqlDbType.DateTime)).Value = DateTime.Now;
		int num2 = (int)database.ExecuteScalar(sqlCommand);
		if (num2 > 0)
		{
			throw new M1MissingOrInvalidDataException($"{num2.ToString()} of the count lines has a counted date greater than today's date.");
		}
		int d = (int)database.ExecuteScalar("Select IsNull(Max(imtPartTransactionID),0)+1 From PartTransactions");
		database.GetService(typeof(M1.Core.AppContext));
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			sqlCommand = database.NewSqlCommand("select imtPartID, imtPartRevisionID, imtPartWarehouseLocationID, imtPartBinID, imtTransactionDate, imtNonInventoryTransaction, imtTransactionType, imtJobID, imtJobAssemblyID, imtJobMaterialID, imtSource, imtInventoryQuantityReceived into #InvCount from InventoryCountLines with (NoLock) inner join PartTransactions with (NoLock) on imqPartID = imtPartID and imqPartRevisionID = imtPartRevisionID and imqPartWarehouseLocationID = imtPartWarehouseLocationID and imqPartBinID = imtPartBinID  where imqInventoryCountID = " + num.ToSql() + " and imtTransactionDate >= imqCountedDate");
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			try
			{
				sqlCommand = database.NewSqlCommand("SELECT IDENTITY(int," + d.ToSql() + ",1) As imtPartTransactionID, 3 as imtTransactionType, 5 as imtSource, 2 as imtReceiptType, 2 as imtIssueType, imqPartID as imtPartID, imqPartRevisionID as imtPartRevisionID, imqPartWarehouseLocationID as imtPartWarehouseLocationID, imqPartBinID as imtPartBinID,IsNull(imwPlantID,'') As imtPlantID, imqCountedDate as imtTransactionDate,imqInventoryCountID As imtInventoryCountID,imqInventoryCountLineID As imtInventoryCountLineID, imqFinalCount - IsNull(imbQuantityOnHand,imrQuantityOnHand) + IsNull((select sum(imtInventoryQuantityReceived) as InvTrans  From #InvCount pt3 where pt3.imtPartID = InventoryCountLines.imqPartID And pt3.imtPartRevisionID = InventoryCountLines.imqPartRevisionID And pt3.imtPartWarehouseLocationID = InventoryCountLines.imqPartWarehouseLocationID And pt3.imtPartBinID = InventoryCountLines.imqPartBinID And pt3.imtTransactionDate > InventoryCountLines.imqCountedDate And pt3.imtNonInventoryTransaction = 0 AND NOT (pt3.imtTransactionType = 1 AND pt3.imtjobID <> '' AND pt3.imtJobAssemblyID <> 0 AND pt3.imtJobMaterialID = 0)),0) As imtInventoryQuantityReceived,IsNull(imbQuantityOnHand,imrQuantityOnHand) As imtPreviousQuantityOnHand, imrInventoryUnitOfMeasure as imtInventoryUnitOfMeasure, " + database.User.ID.ToSql() + " As imtUserID, 'PHYS COUNT' As imtReference  , ISNULL((SELECT MIN(imtTransactionDate) FROM #InvCount pt2 WHERE (pt2.imtTransactionType = 3 AND pt2.imtSource <> 7) AND pt2.imtPartID = InventoryCountLines.imqPartID AND pt2.imtPartRevisionID = InventoryCountLines.imqPartRevisionID AND pt2.imtPartWarehouseLocationID = InventoryCountLines.imqPartWarehouseLocationID AND pt2.imtPartBinID = InventoryCountLines.imqPartBinID AND pt2.imtTransactionDate > InventoryCountLines.imqCountedDate),'20990101') As NextAdjustmentDate, 0 as NextTranID  , imqUniqueID  INTO #InvCountPost FROM  InventoryCountLines INNER JOIN PartRevisions ON imrPartID = imqPartID And imrPartRevisionID = imqPartRevisionID Left Outer Join PartBins On imqPartID = imbPartID and imqPartRevisionID = imbPartRevisionID and imqPartWarehouseLocationID = imbWarehouseID and imqPartBinID = imbPartBinID Left Outer Join Warehouses On imqPartWarehouseLocationID = imwWarehouseID WHERE imqInventoryCountID = " + num.ToSql() + " And imqCountedDate IS NOT NULL");
				database.ExecuteCommand(sqlCommand, sqlTransaction);
				try
				{
					database.ExecuteCommand("UPDATE #InvCountPost set NextTranID = isnull((Select Top 1 ptSub.imtPartTransactionID From PartTransactions ptSub where ptSub.imtPartID = #InvCountPost.imtPartID And ptSub.imtPartRevisionID = #InvCountPost.imtPartRevisionID And ptSub.imtTransactionDate <= #InvCountPost.imtTransactionDate Order By ptSub.imtTransactionDate Desc) ,0)", sqlTransaction);
					database.ExecuteCommand("INSERT INTO PartTransactions          (imtPartTransactionID,imtTransactionType,imtSource,imtReceiptType,imtIssueType,imtPartID,imtPartRevisionID,imtPartWarehouseLocationID,imtPartBinID,imtTransactionDate,imtInventoryQuantityReceived,imtInventoryUnitOfMeasure,imtUserID,imtReference,imtPreviousQuantityOnHand,imtInventoryCountID,imtInventoryCountLineID,imtPlantID,imtCreatedBy,imtCreatedDate,imtTableName,imtTableUniqueID) SELECT imtPartTransactionID,imtTransactionType,imtSource,imtReceiptType,imtIssueType,imtPartID,imtPartRevisionID,imtPartWarehouseLocationID,imtPartBinID,imtTransactionDate,imtInventoryQuantityReceived,imtInventoryUnitOfMeasure,imtUserID,imtReference,imtPreviousQuantityOnHand,imtInventoryCountID,imtInventoryCountLineID,imtPlantID,imtUserID As imtCreatedBy,GetDate() As imtCreatedDate,'InventoryCountLines',imqUniqueID FROM #InvCountPost", sqlTransaction);
					sqlCommand = database.NewSqlCommand("UPDATE PartBins SET imbQuantityOnHand = imbQuantityOnHand + imtInventoryQuantityReceived, imbBinQuantityOnHand = CASE WHEN imbConversionFactor = 0 THEN imbQuantityOnHand + imtInventoryQuantityReceived ELSE (imbQuantityOnHand + imtInventoryQuantityReceived) / imbConversionFactor END FROM PartBins INNER JOIN #InvCountPost ON imbPartID = imtPartID And imbPartRevisionID = imtPartRevisionID And imbWarehouseID = imtPartWarehouseLocationID And imbPartBinID = imtPartBinID And NextAdjustmentDate > @CurDate");
					sqlCommand.Parameters.Add(new SqlParameter("@CurDate", SqlDbType.DateTime)).Value = DateTime.Now;
					database.ExecuteCommand(sqlCommand, sqlTransaction);
					database.ExecuteCommand("UPDATE PartRevisions SET imrQuantityOnHand = imbQuantityOnHand FROM PartRevisions INNER JOIN (SELECT imbPartID,imbPartRevisionID,Sum(imbQuantityOnHand) as imbQuantityOnHand From PartBins Where imbPartID+imbPartRevisionID In (Select imtPartID+imtPartRevisionID From #InvCountPost) GROUP BY imbPartID,imbPartRevisionID ) As test ON imrPartID = imbPartID and imrPartRevisionID = imbPartRevisionID", sqlTransaction);
					database.ExecuteCommand("UPDATE PartTransactions Set imtPreviousQuantityOnHand = ISNULL((SELECT TOP 1 CASE WHEN ptSub.imtSource = 7 THEN ptSub.imtInventoryQuantityReceived ELSE ptSub.imtPreviousQuantityOnHand END AS imtPreviousQuantityOnHand FROM PartTransactions ptSub WHERE ptSub.imtPartID = ptUpdate.imtPartID AND ptSub.imtPartRevisionID = ptUpdate.imtPartRevisionID AND ptSub.imtPartWarehouseLocationID = ptUpdate.imtPartWarehouseLocationID AND ptSub.imtPartBinID = ptUpdate.imtPartBinID  AND ptSub.imtTransactionDate > ptUpdate.imtTransactionDate ORDER BY ptSub.imtTransactionDate ASC),ic1.imtPreviousQuantityOnHand) FROM PartTransactions ptUpdate INNER JOIN #InvCountPost ic1 ON ptUpdate.imtPartTransactionID = ic1.imtPartTransactionID", sqlTransaction);
					database.ExecuteCommand("UPDATE PartTransactions SET imtPreviousQuantityOnHand = CASE WHEN pt1.imtSource <> 7 THEN pt1.imtPreviousQuantityOnHand + ic1.imtInventoryQuantityReceived ELSE pt1.imtPreviousQuantityOnHand END , imtInventoryQuantityReceived = CASE WHEN pt1.imtTransactionType = 3 AND pt1.imtSource <> 7 THEN pt1.imtInventoryQuantityReceived - ic1.imtInventoryQuantityReceived WHEN pt1.imtTransactionType = 3 AND pt1.imtSource = 7 THEN pt1.imtInventoryQuantityReceived + ic1.imtInventoryQuantityReceived ELSE pt1.imtInventoryQuantityReceived END  FROM PartTransactions pt1 INNER JOIN #InvCountPost ic1 ON pt1.imtPartID = ic1.imtPartID AND pt1.imtPartRevisionID = ic1.imtPartRevisionID AND pt1.imtPartWarehouseLocationID = ic1.imtPartWarehouseLocationID AND pt1.imtPartBinID = ic1.imtPartBinID AND pt1.imtTransactionDate > ic1.imtTransactionDate And pt1.imtTransactionDate <= ic1.NextAdjustmentDate", sqlTransaction);
					if ((bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"])
					{
						database.ExecuteCommand(CreatePartTransactionCostsQuery(database.User.ID), sqlTransaction);
					}
					else
					{
						database.ExecuteCommand(CreatePartBinDetailsAndPartTransactionCostsQuery(database.User.ID), sqlTransaction);
					}
					bool? flag = (bool?)database.ExecuteScalar("Select xafGLCreateStockJournals From FinancialProperties", sqlTransaction);
					if (flag.HasValue && flag.Value)
					{
						M1BindingSource m1BindingSource = new M1BindingSource(database, sqlTransaction);
						m1BindingSource.DataSourceTable = "PartTransactions";
						foreach (DataRow row in database.GetDataTable("SELECT * FROM #InvCountPost", sqlTransaction).Rows)
						{
							new CostOfGoodSoldDefinition(m1BindingSource, "imtInventoryQuantityReceived", "imtPartBinID", row.Field<DateTime>("imtTransactionDate"), 31, 3, reverseSign: false, row.Field<decimal>("imtInventoryQuantityReceived"), "ManualJournalCreation,", "InventoryCountLines", "imqUniqueID").AddJournal(database, row, DataRowVersion.Current, sqlTransaction);
						}
						m1BindingSource = null;
					}
				}
				finally
				{
					database.ExecuteCommand("DROP TABLE #InvCountPost", sqlTransaction);
				}
			}
			finally
			{
				database.ExecuteCommand("DROP TABLE #InvCount", sqlTransaction);
			}
			PostSerialLotNumbers(database, sqlTransaction, num);
			bindingsource.CurrentAsDataRow.SetField("imnPostedToInventory", value: true);
			bindingsource.CurrentAsDataRow.SetField("imnStatus", (byte)3);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	private void PostSerialLotNumbers(M1Database database, SqlTransaction transaction, int countID)
	{
		if (countID == 0)
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select snsSerialNumberID, snsPartID, snsPartRevisionID, snsPartWarehouseLocationID, snsPartBinID, snsQuantity, imqUniqueID from InventoryCountLines inner join SerialNumberStatuses on snsPartID = imqPartID And snsPartRevisionID = imqPartRevisionID And snsPartWarehouseLocationID = imqPartWarehouseLocationID And snsPartBinID = imqPartBinID inner join InventoryCounts on imnInventoryCountID = imqInventoryCountID inner join SerialNumbers on snsSerialNumberID = imsSerialNumberID and snsPartID = imsPartID and snsPartRevisionID = imsPartRevisionID where imnInventoryCountID = @ID and imnPostedToInventory = 0 And imqCountedBy <> '' And snsStatus = 2 And snsQuantity <> 0 and snsPartID+snsPartRevisionID+snsPartWarehouseLocationID+snsPartBinID+snsSerialNumberID Not In (Select sntPartID+sntPartRevisionID+sntPartWarehouseLocationID+sntPartBinID+sntSerialNumberID From SerialNumberTransactions Where imqUniqueID = sntTableUniqueID And sntTransactionType = 6) and snsPartID+snsPartRevisionID+snsPartWarehouseLocationID+snsPartBinID+snsSerialNumberID Not In (Select sntPartID+sntPartRevisionID+sntPartWarehouseLocationID+sntPartBinID+sntSerialNumberID From SerialNumberTransactions Where sntTableUniqueID <> imqUniqueID And sntTransactionType <> 19 And sntTransactionDate > imqCountedDate) order by snsSerialNumberID, snsPartID, snsPartRevisionID, snsPartWarehouseLocationID, snsPartBinID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = countID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row5 in dataTable.Rows)
			{
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row5.Field<string>("snsSerialNumberID"));
				byte status = 0;
				byte transType = 27;
				serialNumberDefinition.AddSerialTransaction(database, transaction, row5.Field<string>("snsPartID"), row5.Field<string>("snsPartRevisionID"), row5.Field<string>("snsPartWarehouseLocationID"), row5.Field<string>("snsPartBinID"), row5.Field<string>("snsSerialNumberID"), 0m, status, transType, "InventoryCountLines", row5.Field<Guid>("imqUniqueID"), string.Empty, 0, 0, 0, negativeTrans: false);
				serialNumberDefinition.RefreshStatuses(database, transaction, row5.Field<string>("snsPartID"), row5.Field<string>("snsPartRevisionID"), row5.Field<string>("snsSerialNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, imqUniqueID, sntNegativeTransaction from InventoryCountLines inner join SerialNumberTransactions on imqUniqueID = sntTableUniqueID inner join InventoryCounts on imnInventoryCountID = imqInventoryCountID where imnInventoryCountID = @ID and imnPostedToInventory = 0 And sntTransactionType = 6 and sntPartID+sntPartRevisionID+sntPartWarehouseLocationID+sntPartBinID+sntSerialNumberID Not In (Select sntPartID+sntPartRevisionID+sntPartWarehouseLocationID+sntPartBinID+sntSerialNumberID From SerialNumberTransactions Where sntTableUniqueID <> imqUniqueID And sntTransactionType <> 19 And sntTransactionDate > imqCountedDate) order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = countID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
			foreach (DataRow row6 in dataTable.Rows)
			{
				byte status2 = 0;
				byte transType2 = 0;
				serialNumberDefinition2.LoadLotOrSerialNumbers(database, transaction, row6.Field<string>("sntSerialNumberID"));
				if (row6.Field<byte>("sntTransactionType") == 6)
				{
					status2 = 2;
					transType2 = 27;
				}
				serialNumberDefinition2.AddSerialTransaction(database, transaction, row6.Field<string>("sntPartID"), row6.Field<string>("sntPartRevisionID"), row6.Field<string>("sntPartWarehouseLocationID"), row6.Field<string>("sntPartBinID"), row6.Field<string>("sntSerialNumberID"), row6.Field<decimal>("sntQuantity"), status2, transType2, "InventoryCountLines", row6.Field<Guid>("imqUniqueID"), string.Empty, 0, 0, 0, row6.Field<bool>("sntNegativeTransaction"), row6.Field<DateTime>("sntTransactionDate"));
				serialNumberDefinition2.RefreshStatuses(database, transaction, row6.Field<string>("sntPartID"), row6.Field<string>("sntPartRevisionID"), row6.Field<string>("sntSerialNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("select absLotNumberID, absPartID, absPartRevisionID, absPartWarehouseLocationID, absPartBinID, absQuantity, imqUniqueID from InventoryCountLines inner join LotNumberStatuses on absPartID = imqPartID And absPartRevisionID = imqPartRevisionID And absPartWarehouseLocationID = imqPartWarehouseLocationID And absPartBinID = imqPartBinID inner join InventoryCounts on imnInventoryCountID = imqInventoryCountID inner join LotNumbers on absLotNumberID = ablLotNumberID and absPartID = ablPartID and absPartRevisionID = ablPartRevisionID where imnInventoryCountID = @ID and imnPostedToInventory = 0 And imqCountedBy <> '' And absStatus = 2 And absQuantity <> 0 and absPartID+absPartRevisionID+absPartWarehouseLocationID+absPartBinID+absLotNumberID Not In (Select abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID From LotNumberTransactions Where imqUniqueID = abtTableUniqueID And abtTransactionType <> 19 And abtTransactionType = 6) and absPartID+absPartRevisionID+absPartWarehouseLocationID+absPartBinID+absLotNumberID Not In (Select abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID From LotNumberTransactions Where abtTableUniqueID <> imqUniqueID And  abtTransactionDate > imqCountedDate) order by absLotNumberID, absPartID, absPartRevisionID, absPartWarehouseLocationID, absPartBinID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = countID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
			foreach (DataRow row7 in dataTable.Rows)
			{
				lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row7.Field<string>("absLotNumberID"));
				byte status3 = 0;
				byte transType3 = 27;
				lotNumberDefinition.AddLotTransaction(database, transaction, row7.Field<string>("absPartID"), row7.Field<string>("absPartRevisionID"), row7.Field<string>("absPartWarehouseLocationID"), row7.Field<string>("absPartBinID"), row7.Field<string>("absLotNumberID"), 0m, status3, transType3, "InventoryCountLines", row7.Field<Guid>("imqUniqueID"), string.Empty, 0, 0, 0, negativeTrans: false);
				lotNumberDefinition.RefreshStatuses(database, transaction, row7.Field<string>("absPartID"), row7.Field<string>("absPartRevisionID"), row7.Field<string>("absLotNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, imqUniqueID, abtNegativeTransaction from InventoryCountLines inner join LotNumberTransactions on imqUniqueID = abtTableUniqueID inner join InventoryCounts on imnInventoryCountID = imqInventoryCountID where imnInventoryCountID = @ID and imnPostedToInventory = 0 And abtTransactionType = 6 and abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID Not In (Select abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID From LotNumberTransactions Where abtTableUniqueID <> imqUniqueID And abtTransactionType <> 19 And abtTransactionDate > imqCountedDate) order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = countID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
			foreach (DataRow row8 in dataTable.Rows)
			{
				byte status4 = 0;
				byte transType4 = 0;
				lotNumberDefinition2.LoadLotOrSerialNumbers(database, transaction, row8.Field<string>("abtLotNumberID"));
				if (row8.Field<byte>("abtTransactionType") == 6)
				{
					status4 = 2;
					transType4 = 27;
				}
				lotNumberDefinition2.AddLotTransaction(database, transaction, row8.Field<string>("abtPartID"), row8.Field<string>("abtPartRevisionID"), row8.Field<string>("abtPartWarehouseLocationID"), row8.Field<string>("abtPartBinID"), row8.Field<string>("abtLotNumberID"), row8.Field<decimal>("abtQuantity"), status4, transType4, "InventoryCountLines", row8.Field<Guid>("imqUniqueID"), string.Empty, 0, 0, 0, row8.Field<bool>("abtNegativeTransaction"), row8.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition2.RefreshStatuses(database, transaction, row8.Field<string>("abtPartID"), row8.Field<string>("abtPartRevisionID"), row8.Field<string>("abtLotNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("Update LotNumberTransactions Set abtPartTransactionID = imtPartTransactionID From InventoryCountLines Inner Join LotNumberTransactions on imqUniqueID = abtTableUniqueID Inner Join PartTransactions on imtTableUniqueID = abtTableUniqueID Where abtTransactionType = 27 And imqInventoryCountID = @CountID");
		sqlCommand.Parameters.Add(new SqlParameter("@CountID", SqlDbType.Int)).Value = countID;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("Update SerialNumberTransactions Set sntPartTransactionID = imtPartTransactionID From InventoryCountLines Inner Join SerialNumberTransactions on imqUniqueID = sntTableUniqueID Inner Join PartTransactions on imtTableUniqueID = sntTableUniqueID Where sntTransactionType = 27 And imqInventoryCountID = @CountID");
		sqlCommand.Parameters.Add(new SqlParameter("@CountID", SqlDbType.Int)).Value = countID;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	private string CreatePartBinDetailsAndPartTransactionCostsQuery(string userID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin ");
		stringBuilder.AppendLine("SET NOCOUNT ON ");
		stringBuilder.AppendLine("DECLARE ");
		stringBuilder.AppendLine("@SQL varchar(max) ");
		stringBuilder.AppendLine("set @sql = ' ");
		stringBuilder.AppendLine("DECLARE ");
		stringBuilder.AppendLine("@part_trans_id      AS INT, ");
		stringBuilder.AppendLine("@temp_detail_id      AS INT, ");
		stringBuilder.AppendLine("@part_id       AS varchar(100), ");
		stringBuilder.AppendLine("@part_revision_id          AS varchar(100), ");
		stringBuilder.AppendLine("@warehouse_id           AS varchar(100), ");
		stringBuilder.AppendLine("@bin_id        AS varchar(100), ");
		stringBuilder.AppendLine("@trans_date     AS datetime, ");
		stringBuilder.AppendLine("@qty_rec        AS decimal(15, 5), ");
		stringBuilder.AppendLine("@costing_method as varchar(10), ");
		stringBuilder.AppendLine("@unique_id as uniqueidentifier, ");
		stringBuilder.AppendLine("@imgUnitLaborCost as decimal(15, 5),@imgUnitOverheadCost as decimal(15,5),@imgUnitMaterialCost as decimal(15,5),@imgUnitSubcontractCost as decimal(15,5),@imgUnitDutyCost as decimal(15,5),@imgUnitFreightCost as decimal(15,5), ");
		stringBuilder.AppendLine("@imgUnitMiscCost as decimal(15,5),@imgRemainingQuantity as decimal(15,5),@imgUniqueID as uniqueidentifier, ");
		stringBuilder.AppendLine("@imrLastLaborCost as decimal(15,5),@imrLastOverheadCost as decimal(15,5),@imrLastMaterialCost as decimal(15,5),@imrLastSubcontractCost as decimal(15,5),@imrLastDutyCost as decimal(15,5),@imrLastFreightCost as decimal(15,5),@imrLastMiscCost as decimal(15,5), ");
		stringBuilder.AppendLine("@imrStandardLaborCost as decimal(15,5),@imrStandardOverheadCost as decimal(15,5),@imrStandardMaterialCost as decimal(15,5),@imrStandardSubcontractCost as decimal(15,5),@imrStandardDutyCost as decimal(15,5),@imrStandardFreightCost as decimal(15,5),@imrStandardMiscCost as decimal(15,5), ");
		stringBuilder.AppendLine("@imrAverageLaborCost as decimal(15,5),@imrAverageOverheadCost as decimal(15,5),@imrAverageMaterialCost as decimal(15,5),@imrAverageSubcontractCost as decimal(15,5),@imrAverageDutyCost as decimal(15,5),@imrAverageFreightCost as decimal(15,5),@imrAverageMiscCost as decimal(15,5) ");
		stringBuilder.AppendLine("SET @costing_method = (Select xapIMCostingMethod From ProductionProperties) ");
		stringBuilder.AppendLine("DECLARE TRANS_CURSOR CURSOR FOR ");
		stringBuilder.AppendLine("SELECT imtPartTransactionID, imtPartID, imtPartRevisionID, imtPartWarehouseLocationID, imtPartBinID, imtTransactionDate, imtInventoryQuantityReceived, imqUniqueID FROM #InvCountPost ORDER BY imtPartTransactionID ASC ");
		stringBuilder.AppendLine("OPEN TRANS_CURSOR ");
		stringBuilder.AppendLine("FETCH NEXT FROM TRANS_CURSOR ");
		stringBuilder.AppendLine("INTO @part_trans_id, @part_id, @part_revision_id, @warehouse_id, @bin_id, @trans_date, @qty_rec, @unique_id ");
		stringBuilder.AppendLine("WHILE(@@FETCH_STATUS = 0) ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- if less than 0, reducing partbindetail records ");
		stringBuilder.AppendLine("IF @qty_rec < 0 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("DECLARE ");
		stringBuilder.AppendLine("@costQuantity as decimal(15,5), ");
		stringBuilder.AppendLine("@quantityToIssue as decimal(15,5), ");
		stringBuilder.AppendLine("@partTransCostID as int ");
		stringBuilder.AppendLine("Set @costQuantity = 0 ");
		stringBuilder.AppendLine("Set @quantityToIssue = abs(@qty_rec) ");
		stringBuilder.AppendLine("Set @partTransCostID = 1 ");
		stringBuilder.AppendLine("IF @costing_method = 4 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("DECLARE PBD_CURSOR CURSOR FOR ");
		stringBuilder.AppendLine("Select ");
		stringBuilder.AppendLine("imgUnitLaborCost,imgUnitOverheadCost,imgUnitMaterialCost,imgUnitSubcontractCost,imgUnitDutyCost,imgUnitFreightCost,imgUnitMiscCost,imgRemainingQuantity,imgUniqueID, ");
		stringBuilder.AppendLine("imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost, ");
		stringBuilder.AppendLine("imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost, ");
		stringBuilder.AppendLine("imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost ");
		stringBuilder.AppendLine("from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartWarehouseLocations On imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID And imlPartWarehouseID = @warehouse_id ");
		stringBuilder.AppendLine("Inner Join PartBinDetails On imgPartID = imrPartID AND imgPartRevisionID = imrPartRevisionID AND imgWarehouseID = imlPartWarehouseID AND imgPartBinID = @bin_id ");
		stringBuilder.AppendLine("where imrPartID = @part_id And imrPartRevisionID = @part_revision_id And imgQuantityType = 1 And imgRemainingQuantity <> 0 ORDER BY imgTransactionDate DESC, imgRemainingQuantity ASC ");
		stringBuilder.AppendLine("FOR UPDATE OF imgRemainingQuantity ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("ELSE ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("DECLARE PBD_CURSOR CURSOR FOR ");
		stringBuilder.AppendLine("Select ");
		stringBuilder.AppendLine("imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost, imgRemainingQuantity, imgUniqueID, ");
		stringBuilder.AppendLine("imrLastLaborCost, imrLastOverheadCost, imrLastMaterialCost, imrLastSubcontractCost, imrLastDutyCost, imrLastFreightCost, imrLastMiscCost, ");
		stringBuilder.AppendLine("imrStandardLaborCost, imrStandardOverheadCost, imrStandardMaterialCost, imrStandardSubcontractCost, imrStandardDutyCost, imrStandardFreightCost, imrStandardMiscCost, ");
		stringBuilder.AppendLine("imrAverageLaborCost, imrAverageOverheadCost, imrAverageMaterialCost, imrAverageSubcontractCost, imrAverageDutyCost, imrAverageFreightCost, imrAverageMiscCost ");
		stringBuilder.AppendLine("from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartWarehouseLocations On imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID And imlPartWarehouseID = @warehouse_id ");
		stringBuilder.AppendLine("Inner Join PartBinDetails On imgPartID = imrPartID AND imgPartRevisionID = imrPartRevisionID AND imgWarehouseID = imlPartWarehouseID AND imgPartBinID = @bin_id ");
		stringBuilder.AppendLine("where imrPartID = @part_id And imrPartRevisionID = @part_revision_id And imgQuantityType = 1 And imgRemainingQuantity <> 0 ORDER BY imgTransactionDate ASC, imgRemainingQuantity ASC ");
		stringBuilder.AppendLine("FOR UPDATE OF imgRemainingQuantity ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("OPEN PBD_CURSOR ");
		stringBuilder.AppendLine("FETCH NEXT FROM PBD_CURSOR ");
		stringBuilder.AppendLine("INTO @imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, @imgRemainingQuantity, @imgUniqueID, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost ");
		stringBuilder.AppendLine("WHILE(@@FETCH_STATUS = 0) ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("IF @quantityToIssue > 0 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("IF @quantityToIssue < @imgRemainingQuantity ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("Set @costQuantity = @quantityToIssue ");
		stringBuilder.AppendLine("If @qty_rec < 0 ");
		stringBuilder.AppendLine("Set @costQuantity = @costQuantity * -1 ");
		stringBuilder.AppendLine("Set @quantityToIssue = 0 ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("ELSE ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("Set @quantityToIssue = @quantityToIssue - @imgRemainingQuantity ");
		stringBuilder.AppendLine("Set @costQuantity = @imgRemainingQuantity ");
		stringBuilder.AppendLine("If @qty_rec < 0 ");
		stringBuilder.AppendLine("Set @costQuantity = @costQuantity * -1 ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("-- average ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts (intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, @partTransCostID, 1, @costQuantity, @imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ''PartBinDetails'', @imgUniqueID, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("Set @partTransCostID = @partTransCostID + 1 ");
		stringBuilder.AppendLine("-- last ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, @partTransCostID, 2, @costQuantity, @imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ''PartBinDetails'', @imgUniqueID, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("Set @partTransCostID = @partTransCostID + 1 ");
		stringBuilder.AppendLine("-- standard ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, @partTransCostID, 3, @costQuantity, @imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ''PartBinDetails'', @imgUniqueID, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("Set @partTransCostID = @partTransCostID + 1 ");
		stringBuilder.AppendLine("-- actual ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, @partTransCostID, 4, @costQuantity, @imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, ");
		stringBuilder.AppendLine("@imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, ''PartBinDetails'', @imgUniqueID, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("Set @partTransCostID = @partTransCostID + 1 ");
		stringBuilder.AppendLine("-- update the remaining quantity on the partbindetail record ");
		stringBuilder.AppendLine("UPDATE PartBinDetails SET imgRemainingQuantity = imgRemainingQuantity + @costQuantity WHERE CURRENT OF PBD_CURSOR ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("ELSE ");
		stringBuilder.AppendLine("BREAK ");
		stringBuilder.AppendLine("FETCH NEXT FROM PBD_CURSOR ");
		stringBuilder.AppendLine("INTO @imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, @imgRemainingQuantity, @imgUniqueID, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("CLOSE PBD_CURSOR ");
		stringBuilder.AppendLine("DEALLOCATE PBD_CURSOR ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("ELSE-- if greater than 0, need to create partbindetail records with latest cost ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("DECLARE PBD_CURSOR CURSOR FOR ");
		stringBuilder.AppendLine("Select Top 1 ");
		stringBuilder.AppendLine("imgUnitLaborCost,imgUnitOverheadCost,imgUnitMaterialCost,imgUnitSubcontractCost,imgUnitDutyCost,imgUnitFreightCost,imgUnitMiscCost,imgRemainingQuantity,imgUniqueID, ");
		stringBuilder.AppendLine("imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost, ");
		stringBuilder.AppendLine("imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost, ");
		stringBuilder.AppendLine("imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost ");
		stringBuilder.AppendLine("from PartRevisions Inner Join Parts On imrPartID = impPartID ");
		stringBuilder.AppendLine("Left Join PartBinDetails On imgPartID = imrPartID AND imgPartRevisionID = imrPartRevisionID AND IsNull(imgWarehouseID, @warehouse_id) = @warehouse_id AND IsNull(imgPartBinID, @bin_id) = @bin_id ");
		stringBuilder.AppendLine("where imrPartID = @part_id And imrPartRevisionID = @part_revision_id And IsNull(imgQuantityType, 1) = 1 ORDER BY imgTransactionDate DESC, imgRemainingQuantity ASC ");
		stringBuilder.AppendLine("OPEN PBD_CURSOR ");
		stringBuilder.AppendLine("FETCH NEXT FROM PBD_CURSOR ");
		stringBuilder.AppendLine("INTO @imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, @imgRemainingQuantity, @imgUniqueID, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost ");
		stringBuilder.AppendLine("IF(@@FETCH_STATUS = 0) ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("DECLARE @pbd_guid uniqueidentifier ");
		stringBuilder.AppendLine("SET @pbd_guid = NEWID() ");
		stringBuilder.AppendLine("SET @temp_detail_id = (Select ISNULL(MAX(imgPartBinDetailID), 0) + 1 from PartBinDetails where imgPartID = @part_id and imgPartRevisionID = @part_revision_id and imgWarehouseID = @warehouse_id and imgPartBinID = @bin_id) ");
		stringBuilder.AppendLine("--create partbindetails record ");
		stringBuilder.AppendLine("-- if there is no currentpart bin details, bring in the default costing method ");
		stringBuilder.AppendLine("IF @imgUniqueID Is Not Null ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("Insert Into PartBinDetails (imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgPartBinDetailID, imgTransactionDate, imgQuantityType, imgOriginalQuantity, imgRemainingQuantity, ");
		stringBuilder.AppendLine("imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost, imgSourceTableName, imgSourceTableUniqueID, imgCreatedBy, imgCreatedDate, imgUniqueID) ");
		stringBuilder.AppendLine("Values(@part_id, @part_revision_id, @warehouse_id, @bin_id, @temp_detail_id, @trans_date, 1, @qty_rec, @qty_rec, @imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, ");
		stringBuilder.AppendLine("''InventoryCountLines'', @unique_id, ''" + userID + "'', GETDATE(), @pbd_guid) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("ELSE ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("IF @costing_method = 3 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- standard ");
		stringBuilder.AppendLine("Insert Into PartBinDetails(imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgPartBinDetailID, imgTransactionDate, imgQuantityType, imgOriginalQuantity, imgRemainingQuantity, ");
		stringBuilder.AppendLine("imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost, imgSourceTableName, imgSourceTableUniqueID, imgCreatedBy, imgCreatedDate, imgUniqueID) ");
		stringBuilder.AppendLine("Values(@part_id, @part_revision_id, @warehouse_id, @bin_id, @temp_detail_id, @trans_date, 1, @qty_rec, @qty_rec, @imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("''InventoryCountLines'', @unique_id, ''" + userID + "'', GETDATE(), @pbd_guid) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("IF @costing_method = 2 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- last ");
		stringBuilder.AppendLine("Insert Into PartBinDetails(imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgPartBinDetailID, imgTransactionDate, imgQuantityType, imgOriginalQuantity, imgRemainingQuantity, ");
		stringBuilder.AppendLine("imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost, imgSourceTableName, imgSourceTableUniqueID, imgCreatedBy, imgCreatedDate, imgUniqueID) ");
		stringBuilder.AppendLine("Values(@part_id, @part_revision_id, @warehouse_id, @bin_id, @temp_detail_id, @trans_date, 1, @qty_rec, @qty_rec, @imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("''InventoryCountLines'', @unique_id, ''" + userID + "'', GETDATE(), @pbd_guid) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("IF @costing_method = 1 OR @costing_method = 4 OR @costing_method = 5 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- average ");
		stringBuilder.AppendLine("Insert Into PartBinDetails(imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgPartBinDetailID, imgTransactionDate, imgQuantityType, imgOriginalQuantity, imgRemainingQuantity, ");
		stringBuilder.AppendLine("imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost, imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost, imgSourceTableName, imgSourceTableUniqueID, imgCreatedBy, imgCreatedDate, imgUniqueID) ");
		stringBuilder.AppendLine("Values(@part_id, @part_revision_id, @warehouse_id, @bin_id, @temp_detail_id, @trans_date, 1, @qty_rec, @qty_rec, @imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ");
		stringBuilder.AppendLine("''InventoryCountLines'', @unique_id, ''" + userID + "'', GETDATE(), @pbd_guid) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("-- average ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 1, 1, @qty_rec, @imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("-- last ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 2, 2, @qty_rec, @imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("-- standard ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 3, 3, @qty_rec, @imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("-- actual ");
		stringBuilder.AppendLine("-- if there is no currentpart bin details, bring in the default costing method ");
		stringBuilder.AppendLine("IF @imgUniqueID Is Not Null ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts (intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, ");
		stringBuilder.AppendLine("@imgUnitLaborCost, @imgUnitOverheadCost, @imgUnitMaterialCost, @imgUnitSubcontractCost, @imgUnitDutyCost, @imgUnitFreightCost, @imgUnitMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("ELSE ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("IF @costing_method = 3 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- standard ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("IF @costing_method = 2 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- last ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("IF @costing_method = 1 OR @costing_method = 4 OR @costing_method = 5 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- average ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intSourceTableName, intSourceTableUniqueID, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ''PartBinDetails'', @pbd_guid, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("CLOSE PBD_CURSOR ");
		stringBuilder.AppendLine("DEALLOCATE PBD_CURSOR ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("FETCH NEXT FROM TRANS_CURSOR ");
		stringBuilder.AppendLine("INTO @part_trans_id, @part_id, @part_revision_id, @warehouse_id, @bin_id, @trans_date, @qty_rec, @unique_id ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("CLOSE TRANS_CURSOR ");
		stringBuilder.AppendLine("DEALLOCATE TRANS_CURSOR ");
		stringBuilder.AppendLine("SET NOCOUNT OFF ");
		stringBuilder.AppendLine("' ");
		stringBuilder.AppendLine("exec(@sql) ");
		stringBuilder.AppendLine("SET NOCOUNT OFF ");
		stringBuilder.AppendLine("end ");
		return stringBuilder.ToString();
	}

	private string CreatePartTransactionCostsQuery(string userID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("BEGIN");
		stringBuilder.AppendLine("SET NOCOUNT ON");
		stringBuilder.AppendLine("DECLARE");
		stringBuilder.AppendLine("@SQL varchar(max)");
		stringBuilder.AppendLine("SET @sql = ' ");
		stringBuilder.AppendLine("DECLARE ");
		stringBuilder.AppendLine("@part_trans_id AS INT,");
		stringBuilder.AppendLine("@temp_detail_id AS INT,");
		stringBuilder.AppendLine("@part_id AS varchar(100),");
		stringBuilder.AppendLine("@part_revision_id AS varchar(100),");
		stringBuilder.AppendLine("@warehouse_id AS varchar(100),");
		stringBuilder.AppendLine("@bin_id AS varchar(100),");
		stringBuilder.AppendLine("@trans_date AS datetime,");
		stringBuilder.AppendLine("@qty_rec AS decimal(15, 5),");
		stringBuilder.AppendLine("@costing_method as varchar(10),");
		stringBuilder.AppendLine("@unique_id as uniqueidentifier,");
		stringBuilder.AppendLine("@imrLastLaborCost as decimal(15,5),@imrLastOverheadCost as decimal(15,5),@imrLastMaterialCost as decimal(15,5),@imrLastSubcontractCost as decimal(15,5),@imrLastDutyCost as decimal(15,5),@imrLastFreightCost as decimal(15,5),@imrLastMiscCost as decimal(15,5), ");
		stringBuilder.AppendLine("@imrStandardLaborCost as decimal(15,5),@imrStandardOverheadCost as decimal(15,5),@imrStandardMaterialCost as decimal(15,5),@imrStandardSubcontractCost as decimal(15,5),@imrStandardDutyCost as decimal(15,5),@imrStandardFreightCost as decimal(15,5),@imrStandardMiscCost as decimal(15,5), ");
		stringBuilder.AppendLine("@imrAverageLaborCost as decimal(15,5),@imrAverageOverheadCost as decimal(15,5),@imrAverageMaterialCost as decimal(15,5),@imrAverageSubcontractCost as decimal(15,5),@imrAverageDutyCost as decimal(15,5),@imrAverageFreightCost as decimal(15,5),@imrAverageMiscCost as decimal(15,5) ");
		stringBuilder.AppendLine("SET @costing_method = (Select xapIMCostingMethod From ProductionProperties) ");
		stringBuilder.AppendLine("DECLARE TRANS_CURSOR CURSOR FOR ");
		stringBuilder.AppendLine("SELECT imtPartTransactionID, imtPartID, imtPartRevisionID, imtPartWarehouseLocationID, imtPartBinID, imtTransactionDate, imtInventoryQuantityReceived, imqUniqueID FROM #InvCountPost ORDER BY imtPartTransactionID ASC ");
		stringBuilder.AppendLine("OPEN TRANS_CURSOR ");
		stringBuilder.AppendLine("FETCH NEXT FROM TRANS_CURSOR ");
		stringBuilder.AppendLine("INTO @part_trans_id, @part_id, @part_revision_id, @warehouse_id, @bin_id, @trans_date, @qty_rec, @unique_id ");
		stringBuilder.AppendLine("WHILE(@@FETCH_STATUS = 0) ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("DECLARE PTC_CURSOR CURSOR FOR ");
		stringBuilder.AppendLine("Select Top 1 ");
		stringBuilder.AppendLine("imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost, ");
		stringBuilder.AppendLine("imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost, ");
		stringBuilder.AppendLine("imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost ");
		stringBuilder.AppendLine("from PartRevisions Inner Join Parts On imrPartID = impPartID ");
		stringBuilder.AppendLine("where imrPartID = @part_id And imrPartRevisionID = @part_revision_id ");
		stringBuilder.AppendLine("OPEN PTC_CURSOR ");
		stringBuilder.AppendLine("FETCH NEXT FROM PTC_CURSOR INTO");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost ");
		stringBuilder.AppendLine("IF(@@FETCH_STATUS = 0) ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- average ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 1, 1, @qty_rec, @imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("-- last ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 2, 2, @qty_rec, @imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("-- standard ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 3, 3, @qty_rec, @imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("-- actual ");
		stringBuilder.AppendLine("IF @costing_method = 3 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- standard ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ");
		stringBuilder.AppendLine("@imrStandardLaborCost, @imrStandardOverheadCost, @imrStandardMaterialCost, @imrStandardSubcontractCost, @imrStandardDutyCost, @imrStandardFreightCost, @imrStandardMiscCost, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("IF @costing_method = 2 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- last ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ");
		stringBuilder.AppendLine("@imrLastLaborCost, @imrLastOverheadCost, @imrLastMaterialCost, @imrLastSubcontractCost, @imrLastDutyCost, @imrLastFreightCost, @imrLastMiscCost, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("IF @costing_method = 1 OR @costing_method = 4 OR @costing_method = 5 ");
		stringBuilder.AppendLine("BEGIN ");
		stringBuilder.AppendLine("-- average ");
		stringBuilder.AppendLine("Insert Into PartTransactionCosts(intPartTransactionID, intPartTransactionCostID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, ");
		stringBuilder.AppendLine("intActualUnitLaborCost, intActualUnitOverheadCost, intActualUnitMaterialCost, intActualUnitSubcontractCost, intActualUnitDutyCost, intActualUnitFreightCost, intActualUnitMiscCost, intCreatedBy, intCreatedDate) ");
		stringBuilder.AppendLine("Values(@part_trans_id, 4, 4, @qty_rec, @imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ");
		stringBuilder.AppendLine("@imrAverageLaborCost, @imrAverageOverheadCost, @imrAverageMaterialCost, @imrAverageSubcontractCost, @imrAverageDutyCost, @imrAverageFreightCost, @imrAverageMiscCost, ''" + userID + "'', GETDATE()) ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("CLOSE PTC_CURSOR ");
		stringBuilder.AppendLine("DEALLOCATE PTC_CURSOR ");
		stringBuilder.AppendLine("FETCH NEXT FROM TRANS_CURSOR ");
		stringBuilder.AppendLine("INTO @part_trans_id, @part_id, @part_revision_id, @warehouse_id, @bin_id, @trans_date, @qty_rec, @unique_id ");
		stringBuilder.AppendLine("END ");
		stringBuilder.AppendLine("CLOSE TRANS_CURSOR ");
		stringBuilder.AppendLine("DEALLOCATE TRANS_CURSOR ");
		stringBuilder.AppendLine("SET NOCOUNT OFF");
		stringBuilder.AppendLine("' ");
		stringBuilder.AppendLine("exec(@sql)");
		stringBuilder.AppendLine("SET NOCOUNT OFF");
		stringBuilder.AppendLine("END");
		return stringBuilder.ToString();
	}
}
