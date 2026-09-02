using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class MaterialIssue
{
	public string NegativeStockErrorMessage { get; private set; }

	public string PostMaterialIssueCheck(M1BindingSource bindingSource, bool throwError = false)
	{
		string text = string.Empty;
		if (bindingSource.CurrentAsDataRow != null)
		{
			M1Database database = bindingSource.Database;
			SqlTransaction transaction = bindingSource.Transaction;
			if (MaterialIssuePostedCheck(database, transaction, bindingSource.CurrentAsDataRow.Field<string>("iniMaterialIssueID")))
			{
				text = "This record cannot be saved or posted as it is already marked as being posted in the database.";
			}
			else
			{
				using DataTable issueLinesDt = bindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines").GetDataTable();
				text = MaterialIssueQtyCheck(database, transaction, issueLinesDt, bindingSource);
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (text.Contains(";;"))
			{
				return text.Replace(";;", "") + ";;";
			}
			string text2 = (string.IsNullOrEmpty(NegativeStockErrorMessage) ? ("Record cannot be posted for the following reasons. Please review before attempting to post again:\n\n" + text) : (NegativeStockErrorMessage + "\n\n" + text));
			if (throwError)
			{
				throw new Exception(text2);
			}
			return text2;
		}
		return string.Empty;
	}

	public string VerifyInactiveBinsMiscOrJobIssue(M1BindingSource bindingSource)
	{
		DataTable dataTable = bindingSource.Database.GetDataTable("select injInvIssueQuantity as issueQuantity,imbQuantityOnHand,imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID from MaterialIssueLines inner join PartBins on imbPartID=injPartID and imbPartRevisionID=injPartRevisionID and imbWarehouseID=injPartWarehouseLocationID and imbPartBinID=injPartBinID inner join Parts on impPartID=imbPartID where injPosted=0 and injMaterialIssueID=" + bindingSource.CurrentAsDataRow.Field<string>("iniMaterialIssueID").ToSql() + " and (imbQuantityOnHand-injInvIssueQuantity-injInvScrapQuantity<0 or imbQuantityOnHand-injJobMatIssueQuantity-injJobMatScrapQuantity<0) and imbInactiveBin=1 and injKitPart=0 and (injIssueType=1 or injIssueType=2) and impNonStockedItem=0\r\nunion\r\nselect inkInvIssueQuantity as issueQuantity,imbQuantityOnHand,imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID  from MaterialIssueComponents inner join PartBins on imbPartID = inkPartID and imbPartRevisionID = inkPartRevisionID and imbWarehouseID = inkPartWarehouseLocationID and imbPartBinID = inkPartBinID inner join MaterialIssueLines on inkMaterialIssueLineID=injMaterialIssueLineID and inkMaterialIssueID=injMaterialIssueID inner join Parts on impPartID=imbPartID where inkPosted = 0 and inkMaterialIssueID = " + bindingSource.CurrentAsDataRow.Field<string>("iniMaterialIssueID").ToSql() + " and (imbQuantityOnHand - inkInvIssueQuantity-inkInvScrapQuantity< 0 or imbQuantityOnHand-inkJobMatIssueQuantity-inkJobMatScrapQuantity<0) and imbInactiveBin = 1 and (injIssueType=1 or injIssueType=2) and impNonStockedItem=0");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in dataTable.Rows)
		{
			stringBuilder.AppendLine(string.Format("[Issue Qty [{0}] IS GREATER THAN Quantity on Hand [{1}]", row.Field<decimal>("issueQuantity"), row.Field<decimal>("imbQuantityOnHand")));
			stringBuilder.AppendLine("[Part: '" + row.Field<string>("imbPartID") + "', Revision: '" + row.Field<string>("imbPartRevisionID") + "', Warehouse: '" + row.Field<string>("imbWarehouseID") + "', Bin: '" + row.Field<string>("imbPartBinID") + "']");
		}
		return stringBuilder.ToString();
	}

	public string VerifyInactiveBinsForReturnToJob(M1BindingSource bindingSource)
	{
		DataTable dataTable = bindingSource.Database.GetDataTable("select injInvIssueQuantity as issueQuantity,imbQuantityOnHand,imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID from MaterialIssueLines inner join PartBins on imbPartID=injPartID and imbPartRevisionID=injPartRevisionID and imbWarehouseID=injPartWarehouseLocationID and imbPartBinID=injPartBinID where injPosted=0 and injMaterialIssueID=" + bindingSource.CurrentAsDataRow.Field<string>("iniMaterialIssueID").ToSql() + " and (injJobMatReturnIssueQuantity>0 or injJobMatReturnScrapQuantity>0) and imbInactiveBin=1 and injKitPart=0 and injIssueType=3\r\nunion\r\nselect inkInvIssueQuantity as issueQuantity,imbQuantityOnHand,imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID  from MaterialIssueComponents inner join PartBins on imbPartID = inkPartID and imbPartRevisionID = inkPartRevisionID and imbWarehouseID = inkPartWarehouseLocationID and imbPartBinID = inkPartBinID inner join MaterialIssueLines on inkMaterialIssueLineID=injMaterialIssueLineID and inkMaterialIssueID=injMaterialIssueID where inkPosted = 0 and inkMaterialIssueID = " + bindingSource.CurrentAsDataRow.Field<string>("iniMaterialIssueID").ToSql() + " and (inkJobMatReturnIssueQuantity>0 or inkJobMatReturnScrapQuantity>0) and imbInactiveBin = 1 and injIssueType=3");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in dataTable.Rows)
		{
			stringBuilder.AppendLine("[Part: '" + row.Field<string>("imbPartID") + "', Revision: '" + row.Field<string>("imbPartRevisionID") + "', Warehouse: '" + row.Field<string>("imbWarehouseID") + "', Bin: '" + row.Field<string>("imbPartBinID") + "']");
		}
		return stringBuilder.ToString();
	}

	public bool MaterialIssuePostedCheck(M1Database database, SqlTransaction transaction, string materialIssueID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(iniPosted,0) As iniPosted From MaterialIssues Where iniMaterialIssueID = @MaterialIssueID");
		sqlCommand.Parameters.Add(new SqlParameter("@MaterialIssueID", SqlDbType.NVarChar)).Value = materialIssueID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return false;
		}
		return (bool)obj;
	}

	public bool CheckFutureDatePost(DataRow materialIssue)
	{
		return materialIssue.Field<DateTime>("iniMaterialIssueDate") > DateTime.Now;
	}

	private string MaterialIssueQtyCheck(M1Database database, SqlTransaction transaction, DataTable issueLinesDt, M1BindingSource bindingSource)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		bool flag = (bool)bindingSource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		if (issueLinesDt != null && issueLinesDt.Rows.Count != 0)
		{
			string queryString = "select Part, Revision, Warehouse, Bin, ReversalEntry, IssueType, JobType, Sum(IssueQuantity) as IssueQuantity, Sum(ReturnQuantity) as ReturnQuantity, Sum(ReturnScrapQuantity) as ReturnScrapQuantity, Sum(QtyRecd) as QtyRecd, Sum(ScrapRecd) as ScrapRecd,    isnull(imbQuantityOnHand, imrQuantityOnHand) as CurrentQuantityOnHand, impInactive As IsActive,   ISNULL(imrEffectiveStartDate, '1900-01-01') as EffectiveStartDate ,ISNULL(imrEffectiveEndDate, '2099-01-01') As EffectiveEndDate, impTrackSerialNumbers, impTrackLotNumbers from    (    select injMaterialIssueID as IssueID, injMaterialIssueLineID as LineID, 0 as ComponentID, iniReversalEntry as ReversalEntry, injIssueType as IssueType, injJobType as JobType,     injpartid as Part, injPartRevisionID as Revision, injPartWarehouseLocationID as Warehouse, injPartBinID as Bin,     injInvIssueQuantity + injInvScrapQuantity + injJobAsmIssueQuantity + injJobAsmScrapQuantity + injJobMatIssueQuantity + injJobMatScrapQuantity as IssueQuantity,     injJobMatReturnIssueQuantity as ReturnQuantity,     injJobMatReturnScrapQuantity as ReturnScrapQuantity,     IsNull(jmmQuantityReceived,0) As QtyRecd,     IsNull(jmmScrapQuantityReceived,0) As ScrapRecd    from MaterialIssueLines Left Join JobMaterials on injJobID=jmmJobID and injJobAssemblyID=jmmJobAssemblyID and injJobMaterialID=jmmJobMaterialID    inner join MaterialIssues on injMaterialIssueID=iniMaterialIssueID Where injMaterialIssueID = @MaterialIssueID    UNION ALL    select inkMaterialIssueID as IssueID, inkMaterialIssueLineID as LineID, inkMaterialIssueComponentID as ComponentID, iniReversalEntry as ReversalEntry, injIssueType as IssueType, injJobType as JobType,     inkpartid as Part, inkPartRevisionID as Revision, inkPartWarehouseLocationID as Warehouse, inkPartBinID as Bin,     inkInvIssueQuantity + inkInvScrapQuantity + inkJobMatIssueQuantity + inkJobMatScrapQuantity as IssueQuantity,     inkJobMatReturnIssueQuantity as ReturnQuantity,     inkJobMatReturnScrapQuantity as ReturnScrapQuantity,     IsNull(jmtQuantityReceived,0) As QtyRecd,     IsNull(jmtScrapQuantityReceived,0) As ScrapRecd    from MaterialIssueComponents Inner Join MaterialIssueLines on inkMaterialIssueID=injMaterialIssueID and inkMaterialIssueLineID=injMaterialIssueLineID    Left Join JobMaterialComponents on inkJobID=jmtJobID and inkJobAssemblyID=jmtJobAssemblyID and inkJobMaterialID=jmtJobMaterialID and inkJobMaterialComponentID=jmtJobMaterialComponentID    inner join MaterialIssues on injMaterialIssueID=iniMaterialIssueID Where inkMaterialIssueID = @MaterialIssueID    )    as BaseQuery inner join parts on BaseQuery.Part = impPartID and impNonStockedItem = 0 inner join PartRevisions on BaseQuery.Part = imrPartID and BaseQuery.Revision = imrPartRevisionID left outer join PartBins on BaseQuery.Part = imbPartID and BaseQuery.Revision = imbPartRevisionID and BaseQuery.Warehouse = imbWarehouseID and BaseQuery.Bin = imbPartBinID Group By Part, Revision, Warehouse, Bin, ReversalEntry, IssueType, JobType, imbQuantityOnHand, imrQuantityOnHand, impInactive, imrEffectiveStartDate, imrEffectiveEndDate, impTrackSerialNumbers, impTrackLotNumbers";
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@MaterialIssueID", SqlDbType.NVarChar)).Value = issueLinesDt.Rows[0].Field<string>("injMaterialIssueID");
			foreach (DataRow row in database.GetDataTable(sqlCommand, transaction).Rows)
			{
				decimal num = row.Field<decimal>("CurrentQuantityOnHand");
				decimal num2 = row.Field<decimal>("IssueQuantity");
				decimal num3 = row.Field<decimal>("ReturnQuantity");
				decimal num4 = row.Field<decimal>("ReturnScrapQuantity");
				decimal num5 = row.Field<decimal>("QtyRecd");
				decimal num6 = row.Field<decimal>("ScrapRecd");
				byte num7 = row.Field<byte>("IssueType");
				byte b = row.Field<byte>("JobType");
				bool flag2 = row.Field<bool>("ReversalEntry");
				bool flag3 = row.Field<bool>("IsActive");
				DateTime dateTime = row.Field<DateTime>("EffectiveEndDate");
				DateTime dateTime2 = row.Field<DateTime>("EffectiveStartDate");
				if (num7 != 3)
				{
					if (!(num2 != 0m))
					{
						continue;
					}
					decimal num8 = num - num2;
					if (flag3 || Convert.ToDateTime(dateTime) < DateTime.Now || Convert.ToDateTime(dateTime2) > DateTime.Now)
					{
						text = text + "Part ID '" + row.Field<string>("Part") + "' Revision '" + (string.IsNullOrEmpty(row.Field<string>("Revision")) ? "<none>" : row.Field<string>("Revision")) + "' exists in the Part Revisions table but is not valid for the following reason(s):\n" + (flag3 ? "Part is Inactive" : "") + ((flag3 && (dateTime < DateTime.Now || dateTime2 > DateTime.Now)) ? " or " : "") + ((dateTime < DateTime.Now || dateTime2 > DateTime.Now) ? "Part Revision is not within the effective date range" : "") + "\n\n";
					}
					if (flag)
					{
						if (bindingSource.CurrentAsDataRow.Field<DateTime>("iniMaterialIssueDate") > DateTime.Now)
						{
							NegativeStockErrorMessage = "This transaction CAN NOT be posted because future dating is not supported when the transaction will result in a negative quantity on hand.";
							text = text + "Issue Quantity [" + decimal.Parse(num2.ToString()).ToString("G29") + "] IS GREATER THAN Quantity On Hand [" + decimal.Parse(num.ToString()).ToString("G29") + "]\n [Part: '" + row.Field<string>("Part").ToString() + "', Revision: '" + row.Field<string>("Revision").ToString() + "', Warehouse: '" + row.Field<string>("Warehouse").ToString() + "', Bin: '" + row.Field<string>("Bin").ToString() + "'].\n\n";
						}
						if (num8 < 0m)
						{
							text2 = text2 + "Issue Quantity [" + decimal.Parse(num2.ToString()).ToString("G29") + "] IS GREATER THAN Quantity On Hand [" + decimal.Parse(num.ToString()).ToString("G29") + "]\n [Part: '" + row.Field<string>("Part").ToString() + "', Revision: '" + row.Field<string>("Revision").ToString() + "', Warehouse: '" + row.Field<string>("Warehouse").ToString() + "', Bin: '" + row.Field<string>("Bin").ToString() + "'].;;\n\n";
						}
					}
					else if (num8 < 0m)
					{
						NegativeStockErrorMessage = "This transaction CAN NOT be posted because it will result in a negative quantity on hand for the part(s) indicated.";
						text = text + "Issue Quantity [" + decimal.Parse(num2.ToString()).ToString("G29") + "] IS GREATER THAN Quantity On Hand [" + decimal.Parse(num.ToString()).ToString("G29") + "]\n [Part: '" + row.Field<string>("Part").ToString() + "', Revision: '" + row.Field<string>("Revision").ToString() + "', Warehouse: '" + row.Field<string>("Warehouse").ToString() + "', Bin: '" + row.Field<string>("Bin").ToString() + "'].\n\n";
					}
					if (flag2 && b == 1 && num5 + num6 < Math.Abs(num2))
					{
						text = text + "Issue Quantity (" + decimal.Parse(Math.Abs(num2).ToString()).ToString("G29") + ") is greater than Quantity Received (" + decimal.Parse((num5 + num6).ToString()).ToString("G29") + ")\n (Part: '" + row.Field<string>("Part").ToString() + "', Revision: '" + row.Field<string>("Revision").ToString() + "', Warehouse: '" + row.Field<string>("Warehouse").ToString() + "', Bin: '" + row.Field<string>("Bin").ToString() + "').\n\n";
					}
				}
				else
				{
					if (num3 != 0m && num5 - num3 < 0m)
					{
						text = text + "Return Issue Quantity (" + decimal.Parse(num3.ToString()).ToString("G29") + ") is greater than Quantity Received (" + decimal.Parse(num5.ToString()).ToString("G29") + ")\n (Part: '" + row.Field<string>("Part").ToString() + "', Revision: '" + row.Field<string>("Revision").ToString() + "', Warehouse: '" + row.Field<string>("Warehouse").ToString() + "', Bin: '" + row.Field<string>("Bin").ToString() + "').\n\n";
					}
					if (num4 != 0m && num6 - num4 < 0m)
					{
						text = text + "Return Scrap Quantity (" + decimal.Parse(num4.ToString()).ToString("G29") + ") is greater than Scrap Quantity Received (" + decimal.Parse(num6.ToString()).ToString("G29") + ")\n (Part: '" + row.Field<string>("Part").ToString() + "', Revision: '" + row.Field<string>("Revision").ToString() + "', Warehouse: '" + row.Field<string>("Warehouse").ToString() + "', Bin: '" + row.Field<string>("Bin").ToString() + "').\n\n";
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text2;
	}

	public bool MaterialIssuePeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("iniMaterialIssueDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("iniMaterialIssueDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public void PostMaterialIssue(M1BindingSource bindingsource)
	{
		bool flag = false;
		M1Database database = bindingsource.Database;
		SqlTransaction sqlTransaction = bindingsource.Transaction;
		if (sqlTransaction == null)
		{
			sqlTransaction = database.BeginTransaction();
			flag = true;
		}
		try
		{
			if (bindingsource.CurrentAsDataRow == null)
			{
				return;
			}
			bindingsource.CurrentAsDataRow.BeginEdit();
			bindingsource.CurrentAsDataRow.SetField("iniPosted", value: true);
			bindingsource.CurrentAsDataRow.AcceptChanges();
			string value = bindingsource.CurrentAsDataRow.Field<string>("iniMaterialIssueID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, injUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, injJobMaterialID, sntNegativeTransaction from MaterialIssueLines inner join SerialNumberTransactions on injUniqueID = sntTableUniqueID where injMaterialIssueID = @ID and injPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				Part part = new Part();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte destStatus = 0;
					byte transType = 0;
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("sntSerialNumberID"));
					bool flag2 = row3.Field<bool>("sntNegativeTransaction");
					switch (row3.Field<byte>("sntTransactionType"))
					{
					case 28:
						destStatus = (byte)(flag2 ? 2 : 0);
						transType = 21;
						break;
					case 29:
						destStatus = (byte)(flag2 ? 2 : 6);
						transType = 17;
						break;
					case 30:
						destStatus = (byte)(flag2 ? 2 : 3);
						transType = 20;
						break;
					case 31:
						destStatus = (byte)(flag2 ? 2 : 6);
						transType = 23;
						break;
					case 32:
						destStatus = (byte)(flag2 ? 2 : 3);
						transType = 4;
						break;
					case 33:
						destStatus = (byte)(flag2 ? 2 : 6);
						transType = 22;
						break;
					case 70:
						destStatus = (byte)(flag2 ? 3 : 2);
						transType = 72;
						break;
					case 71:
						destStatus = (byte)(flag2 ? 3 : 6);
						transType = 73;
						break;
					}
					if (part.GetFutureAdjustmentTransactionStatus(database, sqlTransaction, row3.Field<string>("sntPartID"), row3.Field<string>("sntPartRevisionID"), row3.Field<string>("sntPartWarehouseLocationID"), row3.Field<string>("sntPartBinID"), row3.Field<DateTime>("sntTransactionDate")))
					{
						ProcessAddSerialTransaction(database, sqlTransaction, serialNumberDefinition, row3, 0, transType);
						serialNumberDefinition.RemoveSerialNumber(database, sqlTransaction, row3.Field<string>("sntSerialNumberID"), row3.Field<string>("sntPartID"), row3.Field<string>("sntPartRevisionID"), row3.Field<string>("sntPartWarehouseLocationID"), row3.Field<string>("sntPartBinID"));
					}
					else
					{
						ProcessAddSerialTransaction(database, sqlTransaction, serialNumberDefinition, row3, destStatus, transType);
						serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("sntPartID"), row3.Field<string>("sntPartRevisionID"), row3.Field<string>("sntSerialNumberID"));
					}
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, inkUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from MaterialIssueComponents inner join SerialNumberTransactions on inkUniqueID = sntTableUniqueID where inkMaterialIssueID = @ID and inkPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status = 0;
					byte transType2 = 0;
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("sntSerialNumberID"));
					bool flag3 = row4.Field<bool>("sntNegativeTransaction");
					switch (row4.Field<byte>("sntTransactionType"))
					{
					case 28:
						status = (byte)(flag3 ? 2 : 0);
						transType2 = 21;
						break;
					case 29:
						status = (byte)(flag3 ? 2 : 6);
						transType2 = 17;
						break;
					case 30:
						status = (byte)(flag3 ? 2 : 3);
						transType2 = 20;
						break;
					case 31:
						status = (byte)(flag3 ? 2 : 6);
						transType2 = 23;
						break;
					case 32:
						status = (byte)(flag3 ? 2 : 3);
						transType2 = 4;
						break;
					case 33:
						status = (byte)(flag3 ? 2 : 6);
						transType2 = 22;
						break;
					case 70:
						status = (byte)(flag3 ? 3 : 2);
						transType2 = 72;
						break;
					case 71:
						status = (byte)(flag3 ? 3 : 6);
						transType2 = 73;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row4.Field<string>("sntPartID"), row4.Field<string>("sntPartRevisionID"), row4.Field<string>("sntPartWarehouseLocationID"), row4.Field<string>("sntPartBinID"), row4.Field<string>("sntSerialNumberID"), row4.Field<decimal>("sntQuantity"), status, transType2, "MaterialIssueComponents", row4.Field<Guid>("inkUniqueID"), row4.Field<string>("sntJobID"), Convert.ToInt32(row4["sntJobAssemblyID"]), Convert.ToInt32(row4["injJobMaterialID"]), Convert.ToInt32(row4["sntJobMaterialComponentID"]), row4.Field<bool>("sntNegativeTransaction"), row4.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("sntPartID"), row4.Field<string>("sntPartRevisionID"), row4.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, injUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, injJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MaterialIssueLines inner join LotNumberTransactions on injUniqueID = abtTableUniqueID where injMaterialIssueID = @ID and injPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				Part part2 = new Part();
				foreach (DataRow row5 in dataTable.Rows)
				{
					byte destStatus2 = 0;
					byte transType3 = 0;
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row5.Field<string>("abtLotNumberID"));
					bool flag4 = row5.Field<bool>("abtNegativeTransaction");
					switch (row5.Field<byte>("abtTransactionType"))
					{
					case 28:
						destStatus2 = (byte)(flag4 ? 2 : 0);
						transType3 = 21;
						break;
					case 29:
						destStatus2 = (byte)(flag4 ? 2 : 6);
						transType3 = 17;
						break;
					case 30:
						destStatus2 = (byte)(flag4 ? 2 : 3);
						transType3 = 20;
						break;
					case 31:
						destStatus2 = (byte)(flag4 ? 2 : 6);
						transType3 = 23;
						break;
					case 32:
						destStatus2 = (byte)(flag4 ? 2 : 3);
						transType3 = 4;
						break;
					case 33:
						destStatus2 = (byte)(flag4 ? 2 : 6);
						transType3 = 22;
						break;
					case 70:
						destStatus2 = (byte)(flag4 ? 3 : 2);
						transType3 = 72;
						break;
					case 71:
						destStatus2 = (byte)(flag4 ? 3 : 6);
						transType3 = 73;
						break;
					}
					if (part2.GetFutureAdjustmentTransactionStatus(database, sqlTransaction, row5.Field<string>("abtPartID"), row5.Field<string>("abtPartRevisionID"), row5.Field<string>("abtPartWarehouseLocationID"), row5.Field<string>("abtPartBinID"), row5.Field<DateTime>("abtTransactionDate")))
					{
						ProcessAddLotTransaction(database, sqlTransaction, lotNumberDefinition, row5, 0, transType3);
						lotNumberDefinition.RemoveLotNumber(database, sqlTransaction, row5.Field<string>("abtLotNumberID"), row5.Field<string>("abtPartID"), row5.Field<string>("abtPartRevisionID"), row5.Field<string>("abtPartWarehouseLocationID"), row5.Field<string>("abtPartBinID"));
					}
					else
					{
						ProcessAddLotTransaction(database, sqlTransaction, lotNumberDefinition, row5, destStatus2, transType3);
						lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row5.Field<string>("abtPartID"), row5.Field<string>("abtPartRevisionID"), row5.Field<string>("abtLotNumberID"));
					}
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, inkUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MaterialIssueComponents inner join LotNumberTransactions on inkUniqueID = abtTableUniqueID where inkMaterialIssueID = @ID and inkPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row6 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType4 = 0;
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row6.Field<string>("abtLotNumberID"));
					bool flag5 = row6.Field<bool>("abtNegativeTransaction");
					switch (row6.Field<byte>("abtTransactionType"))
					{
					case 28:
						status2 = (byte)(flag5 ? 2 : 0);
						transType4 = 21;
						break;
					case 29:
						status2 = (byte)(flag5 ? 2 : 6);
						transType4 = 17;
						break;
					case 30:
						status2 = (byte)(flag5 ? 2 : 3);
						transType4 = 20;
						break;
					case 31:
						status2 = (byte)(flag5 ? 2 : 6);
						transType4 = 23;
						break;
					case 32:
						status2 = (byte)(flag5 ? 2 : 3);
						transType4 = 4;
						break;
					case 33:
						status2 = (byte)(flag5 ? 2 : 6);
						transType4 = 22;
						break;
					case 70:
						status2 = (byte)(flag5 ? 3 : 2);
						transType4 = 72;
						break;
					case 71:
						status2 = (byte)(flag5 ? 3 : 6);
						transType4 = 73;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row6.Field<string>("abtPartID"), row6.Field<string>("abtPartRevisionID"), row6.Field<string>("abtPartWarehouseLocationID"), row6.Field<string>("abtPartBinID"), row6.Field<string>("abtLotNumberID"), row6.Field<decimal>("abtQuantity"), status2, transType4, "MaterialIssueComponents", row6.Field<Guid>("inkUniqueID"), row6.Field<string>("abtJobID"), Convert.ToInt32(row6["abtJobAssemblyID"]), Convert.ToInt32(row6["abtJobMaterialID"]), Convert.ToInt32(row6["abtJobMaterialComponentID"]), row6.Field<bool>("abtNegativeTransaction"), row6.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row6.Field<string>("abtPartID"), row6.Field<string>("abtPartRevisionID"), row6.Field<string>("abtLotNumberID"));
				}
			}
			if (flag)
			{
				database.CommitTransaction(sqlTransaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(sqlTransaction);
			}
			throw;
		}
	}

	private static void ProcessAddSerialTransaction(M1Database database, SqlTransaction transaction, SerialNumberDefinition serialDefObj, DataRow row, byte destStatus, byte transType)
	{
		serialDefObj.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), destStatus, transType, "MaterialIssueLines", row.Field<Guid>("injUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["injJobMaterialID"]), 0, row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
	}

	private static void ProcessAddLotTransaction(M1Database database, SqlTransaction transaction, LotNumberDefinition lotDefObj, DataRow row, byte destStatus, byte transType)
	{
		lotDefObj.AddLotTransaction(database, transaction, row.Field<string>("abtPartID"), row.Field<string>("abtPartRevisionID"), row.Field<string>("abtPartWarehouseLocationID"), row.Field<string>("abtPartBinID"), row.Field<string>("abtLotNumberID"), row.Field<decimal>("abtQuantity"), destStatus, transType, "MaterialIssueLines", row.Field<Guid>("injUniqueID"), row.Field<string>("abtJobID"), Convert.ToInt32(row["abtJobAssemblyID"]), Convert.ToInt32(row["injJobMaterialID"]), 0, row.Field<bool>("abtNegativeTransaction"), row.Field<DateTime>("abtTransactionDate"));
	}

	public string CheckMaterialIssueForFutureAdjustmentTransactions(M1BindingSource bindingsource)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Dictionary<PartInformation, decimal> dicPartInfo = new Dictionary<PartInformation, decimal>(new PartInformationEqualityComparer());
		if (bindingsource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		if (bindingsource.CurrentAsDataRow.Field<bool>("iniReversed"))
		{
			return string.Empty;
		}
		M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource("MaterialIssueLines");
		DataTable dataTable = childBindingSource.GetDataTable();
		if (dataTable == null)
		{
			return string.Empty;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			PopulatePartInfoDictionary(childBindingSource, dicPartInfo, row);
		}
		IList<string> list = CheckPartForFutureDatesTransactions(bindingsource, dicPartInfo);
		if (list.Count == 0)
		{
			return string.Empty;
		}
		stringBuilder.AppendLine("There are future quantity adjustments for the following parts. If you continue, the current quantity on hand will not be adjusted for these parts.");
		stringBuilder.AppendLine("Do you wish to continue posting?" + Environment.NewLine);
		stringBuilder.AppendLine(string.Join("\n", list));
		return stringBuilder.ToString();
	}

	private static IList<string> CheckPartForFutureDatesTransactions(M1BindingSource bindingsource, IDictionary<PartInformation, decimal> dicPartInfo)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<PartInformation, decimal> item in dicPartInfo)
		{
			DateTime? tranDate = bindingsource.CurrentAsDataRow.Field<DateTime?>("iniMaterialIssueDate");
			SqlCommand sqlCommand = new SqlCommand("SELECT impNonStockedItem FROM Parts WHERE impPartID = @partID");
			sqlCommand.Parameters.Add(new SqlParameter("@partID", item.Key.Part));
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand, bindingsource.Transaction);
			bool value = obj == null || Convert.ToBoolean(obj);
			if (obj != null && !Convert.ToBoolean(value) && new Part().GetFutureAdjustmentTransactionStatus(bindingsource.Database, bindingsource.Transaction, item.Key.Part, item.Key.PartRevision, item.Key.PartWarehouse, item.Key.PartBin, tranDate))
			{
				list.Add("Part '" + item.Key.Part + "', Rev '" + item.Key.PartRevision + "', Warehouse '" + item.Key.PartWarehouse + "', Bin '" + item.Key.PartBin + "'");
			}
		}
		return list;
	}

	private void PopulatePartInfoDictionary(M1BindingSource lineBindingsource, IDictionary<PartInformation, decimal> dicPartInfo, DataRow lineRow)
	{
		M1Database database = lineBindingsource.Database;
		decimal num;
		PartInformation key;
		if (lineRow.Field<bool>("injKitPart"))
		{
			M1BindingSource childBindingSource = lineBindingsource.PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
			DataTable dataTable = childBindingSource.GetDataView(lineRow).ToTable();
			if (dataTable.Rows.Count == 0)
			{
				return;
			}
			{
				foreach (DataRow row in dataTable.Rows)
				{
					num = row.Field<decimal>("inkInvIssueQuantity");
					key = CreatePartInfoKey(database, row, childBindingSource.PrimaryTable.FieldPrefix);
					if (dicPartInfo.ContainsKey(key))
					{
						dicPartInfo[key] += num;
					}
					else
					{
						dicPartInfo.Add(key, num);
					}
				}
				return;
			}
		}
		num = lineRow.Field<decimal>("injInvIssueQuantity");
		key = CreatePartInfoKey(database, lineRow, lineBindingsource.PrimaryTable.FieldPrefix);
		if (dicPartInfo.ContainsKey(key))
		{
			dicPartInfo[key] += num;
		}
		else
		{
			dicPartInfo.Add(key, num);
		}
	}

	private static PartInformation CreatePartInfoKey(M1Database database, DataRow row, string prefix)
	{
		Part part = new Part();
		if (row != null)
		{
			string text = row.Field<string>(prefix + "PartID").Trim();
			string partRevision = row.Field<string>(prefix + "PartRevisionID").Trim();
			string text2 = row.Field<string>(prefix + "PartWarehouseLocationID").Trim();
			string text3 = row.Field<string>(prefix + "PartBinID").Trim();
			return new PartInformation
			{
				Part = text,
				PartRevision = partRevision,
				PartWarehouse = text2,
				PartBin = text3,
				IsBinInactive = part.IsPartBinInactive(database, text, partRevision, text2, text3)
			};
		}
		return null;
	}

	public void CreateMaterialIssueJournalsFromBackflush(M1Database database, string materialIssueID)
	{
		if (string.IsNullOrWhiteSpace(materialIssueID) || !database.Props("GL").Field<bool>("xafGLCreateStockJournals"))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From MaterialIssueLines Where injMaterialIssueID = @MaterialIssueID");
		sqlCommand.Parameters.Add(new SqlParameter("@MaterialIssueID", SqlDbType.NVarChar)).Value = materialIssueID;
		foreach (DataRow row in database.GetDataTable(sqlCommand).Rows)
		{
			if (!row.Field<bool>("injKitPart"))
			{
				using (M1BindingSource m1BindingSource = new M1BindingSource(database))
				{
					m1BindingSource.DataSourceTable = "MaterialIssueLines";
					if (row.Field<decimal>("injJobMatIssueQuantity") != 0m)
					{
						new CostOfGoodSoldDefinition(m1BindingSource, "injJobMatIssueQuantity", "injPartBinID", DateTime.Now, 3, 2, reverseSign: true, row.Field<decimal>("injJobMatIssueQuantity"), "CheckForKitPart,ManualJournalCreation,", "MaterialIssueLines", "injUniqueID", "injJobMaterialID").AddJournal(database, row, DataRowVersion.Current, null);
					}
					if (row.Field<decimal>("injJobMatScrapQuantity") != 0m)
					{
						new CostOfGoodSoldDefinition(m1BindingSource, "injJobMatScrapQuantity", "injPartBinID", DateTime.Now, 4, 5, reverseSign: true, row.Field<decimal>("injJobMatScrapQuantity"), "CheckForKitPart,ManualJournalCreation,", "MaterialIssueLines", "injUniqueID", "injJobMaterialID").AddJournal(database, row, DataRowVersion.Current, null);
					}
				}
				continue;
			}
			using M1BindingSource m1BindingSource2 = new M1BindingSource(database);
			m1BindingSource2.LoadDefinition("M1MATERIALISSUELINESENTRY");
			m1BindingSource2.NavigateTo("injMaterialIssueID = " + materialIssueID.ToSql() + " and injMaterialIssueLineID = " + row.Field<short>("injMaterialIssueLineID").ToSql());
			using M1BindingSource bs = m1BindingSource2.PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
			SqlCommand sqlCommand2 = database.NewSqlCommand("Select * From MaterialIssueComponents Where inkMaterialIssueID = @MaterialIssueID And inkMaterialIssueLineID = 1");
			sqlCommand2.Parameters.Add(new SqlParameter("@MaterialIssueID", SqlDbType.NVarChar)).Value = materialIssueID;
			foreach (DataRow row2 in database.GetDataTable(sqlCommand2).Rows)
			{
				if (row2.Field<decimal>("inkJobMatIssueQuantity") != 0m)
				{
					new CostOfGoodSoldDefinition(bs, "inkJobMatIssueQuantity", "inkPartBinID", DateTime.Now, 3, 2, reverseSign: true, row2.Field<decimal>("inkJobMatIssueQuantity"), "ManualJournalCreation,", "MaterialIssueComponents", "inkUniqueID", "inkJobMaterialComponentID").AddJournal(database, row2, DataRowVersion.Current, null);
				}
				if (row2.Field<decimal>("inkJobMatScrapQuantity") != 0m)
				{
					new CostOfGoodSoldDefinition(bs, "inkJobMatScrapQuantity", "inkPartBinID", DateTime.Now, 4, 5, reverseSign: true, row2.Field<decimal>("inkJobMatScrapQuantity"), "ManualJournalCreation,", "MaterialIssueComponents", "inkUniqueID", "inkJobMaterialComponentID").AddJournal(database, row2, DataRowVersion.Current, null);
				}
			}
		}
	}
}
