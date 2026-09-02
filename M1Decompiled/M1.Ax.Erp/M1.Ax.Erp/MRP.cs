using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Ax.Erp.Models;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1Classes92;

namespace M1.Ax.Erp;

public class MRP
{
	private const string _minMaxDemandSelectFilter = "mrrSource = 'PartRevisions' and mrrType = 'MinMax'";

	private const string _forecastDemandSelectFilter = "mrrSource = 'PartRevisions' and mrrType = 'PartForecast'";

	private const string _orderDemandSelectFilter = "mrrSource = 'SalesOrderDeliveries' and mrrType = 'MakeToOrder'";

	private const string _notMakeToOrderDemandSelectFilter = "mrrSource = 'SalesOrderDeliveries' and not mrrType = 'MakeToOrder'";

	private const string _jobDemandSelectFilter = "mrrSource = 'JobAssemblies' or mrrSource = 'JobMaterials'";

	public string CheckOverlap(M1Database database, string sessionID, int lineID, bool messageDetail)
	{
		Cursor.Current = Cursors.WaitCursor;
		string text = string.Empty;
		bool plantFiltersExist = false;
		bool warehouseFiltersExist = false;
		bool customerFiltersExist = false;
		bool partFiltersExist = false;
		bool partGroupFiltersExist = false;
		bool partClassFiltersExist = false;
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(mrpPlantIDs,'') As mrpPlantIDs, mrpWarehouseIDs, mrpPartClassIDs, mrpCustomerIDs, mrpPartGroupIDs, mrpCompletedDate, mrpCompleted, mrpCutoffDate, mrpGenerated, mrpPartIDs From MRPSessions Where mrpSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			string text2 = SplitAndConvert(row.Field<string>("mrpPlantIDs"));
			string warehouses = SplitAndConvert(row.Field<string>("mrpWarehouseIDs"));
			string partClasses = SplitAndConvert(row.Field<string>("mrpPartClassIDs"));
			string partGroups = SplitAndConvert(row.Field<string>("mrpPartGroupIDs"));
			string customers = SplitAndConvert(row.Field<string>("mrpCustomerIDs"));
			string parts = SplitAndConvert(row.Field<string>("mrpPartIDs"));
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			text2 = text2.Replace("<None>", string.Empty);
			FillFilterExistValues(ref plantFiltersExist, ref warehouseFiltersExist, ref customerFiltersExist, ref partFiltersExist, ref partGroupFiltersExist, ref partClassFiltersExist, text2, warehouses, customers, parts, partGroups, partClasses);
			if (lineID == 0)
			{
				text = PerformCheckOverlap(database, sessionID, plantFiltersExist, warehouseFiltersExist, customerFiltersExist, partFiltersExist, partClassFiltersExist, partGroupFiltersExist, text2, warehouses, customers, parts, partClasses, partGroups, messageDetail);
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
				if (!partClassFiltersExist && !partGroupFiltersExist && !customerFiltersExist && !partFiltersExist)
				{
					DataTable mRPLinesNotCompleted = GetMRPLinesNotCompleted(database, sessionID, plantFiltersExist, warehouseFiltersExist, text2, warehouses);
					if (mRPLinesNotCompleted.Rows.Count != 0)
					{
						foreach (DataRow row2 in mRPLinesNotCompleted.Rows)
						{
							list.Add(row2.Field<string>("mrpSessionID"));
							list2.Add(row2.Field<string>("mrjPartID"));
							list3.Add(row2.Field<string>("mrjPartRevisionID"));
							list4.Add(row2.Field<string>("mrjPartWarehouseLocationID"));
						}
						string sessions = string.Join(",", list.Distinct());
						text = (messageDetail ? GetOverlapDetailMessage(list, list2, list3, list4) : GetOverlapMessage(sessions));
					}
					return text;
				}
			}
		}
		return text;
	}

	public void Clear(M1Database database, string sessionID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Delete From MRPLines Where mrlSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From MRPDemands Where mrrSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From MRPSupply Where mrsSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From MRPJobDetails Where mrjSessionID = @SessionID");
		string queryString = stringBuilder.ToString();
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		database.ExecuteCommand(sqlCommand);
		sqlCommand = database.NewSqlCommand("Update MRPSessions Set mrpGenerated = 0 Where mrpSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		database.ExecuteCommand(sqlCommand);
	}

	public string CreateMrpLines(M1Database database, string sessionID, int lineID)
	{
		Cursor.Current = Cursors.WaitCursor;
		string result = string.Empty;
		bool plantFiltersExist = false;
		bool warehouseFiltersExist = false;
		bool customerFiltersExist = false;
		bool partFiltersExist = false;
		bool partGroupFiltersExist = false;
		bool partClassFiltersExist = false;
		string linePartID = string.Empty;
		string linePartRevision = string.Empty;
		new StringBuilder();
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(mrpPlantIDs,'') As mrpPlantIDs, mrpWarehouseIDs, mrpPartClassIDs, mrpCustomerIDs, mrpPartGroupIDs, mrpCompletedDate, mrpCompleted, mrpCutoffDate, mrpGenerated, mrpPartIDs, mrpIncludePartForecasts, mrpConsolidatePartForecastJobs From MRPSessions Where mrpSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			string text = SplitAndConvert(row.Field<string>("mrpPlantIDs"));
			string text2 = SplitAndConvert(row.Field<string>("mrpWarehouseIDs"));
			string partClasses = SplitAndConvert(row.Field<string>("mrpPartClassIDs"));
			string partGroups = SplitAndConvert(row.Field<string>("mrpPartGroupIDs"));
			string customers = SplitAndConvert(row.Field<string>("mrpCustomerIDs"));
			string parts = SplitAndConvert(row.Field<string>("mrpPartIDs"));
			DateTime cutoffDate = row.Field<DateTime>("mrpCutoffDate").AddDays(1.0);
			bool includePartForecasts = row.Field<bool>("mrpIncludePartForecasts");
			bool consolidatePartForecastJob = row.Field<bool>("mrpConsolidatePartForecastJobs");
			if (lineID != 0)
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Select mrlPartID, mrlPartRevisionID, mrlPartShortDescription, mrlPlantIDs, mrlWarehouseIDs From MRPLines Where mrlSessionID = @SessionID and mrlLineID = @LineID");
				sqlCommand2.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
				sqlCommand2.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineID;
				DataTable dataTable2 = database.GetDataTable(sqlCommand2);
				if (dataTable2.Rows.Count != 0)
				{
					DataRow row2 = dataTable2.Rows[0];
					linePartID = row2.Field<string>("mrlPartID");
					linePartRevision = row2.Field<string>("mrlPartRevisionID");
				}
			}
			text = text.Replace("<None>", string.Empty);
			FillFilterExistValues(ref plantFiltersExist, ref warehouseFiltersExist, ref customerFiltersExist, ref partFiltersExist, ref partGroupFiltersExist, ref partClassFiltersExist, text, text2, customers, parts, partGroups, partClasses);
			RemoveExistingRecords(database, sessionID, lineID);
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataSet dataSet = new DataSet();
			string sODeliveriesDemandQuery = getSODeliveriesDemandQuery(database, sessionID, lineID, customerFiltersExist, partFiltersExist, partGroupFiltersExist, partClassFiltersExist, warehouseFiltersExist, plantFiltersExist, linePartID, linePartRevision, partGroups, partClasses, customers, parts, text, text2);
			addParametersAndFillDataset(database, cutoffDate, adapter, dataSet, sODeliveriesDemandQuery, "Demands");
			sODeliveriesDemandQuery = GetPartsDemandQuery(database, sessionID, lineID, plantFiltersExist, warehouseFiltersExist, partFiltersExist, partGroupFiltersExist, partClassFiltersExist, customerFiltersExist, linePartID, linePartRevision, text, text2, partGroups, partClasses, parts);
			addParametersAndFillDataset(database, cutoffDate, adapter, dataSet, sODeliveriesDemandQuery, "Demands");
			sODeliveriesDemandQuery = GetJobAssemblyDemandQuery(database, sessionID, lineID, plantFiltersExist, warehouseFiltersExist, customerFiltersExist, partFiltersExist, partGroupFiltersExist, partClassFiltersExist, linePartID, linePartRevision, text, text2, customers, partGroups, partClasses, parts);
			addParametersAndFillDataset(database, cutoffDate, adapter, dataSet, sODeliveriesDemandQuery, "Demands");
			sODeliveriesDemandQuery = GetJobMaterialDemandQuery(database, sessionID, lineID, plantFiltersExist, warehouseFiltersExist, customerFiltersExist, partFiltersExist, partGroupFiltersExist, partClassFiltersExist, linePartID, linePartRevision, text, text2, customers, partGroups, partClasses, parts);
			addParametersAndFillDataset(database, cutoffDate, adapter, dataSet, sODeliveriesDemandQuery, "Demands");
			sODeliveriesDemandQuery = GetPartForecastsDemandQuery(database, sessionID, lineID, partFiltersExist, partGroupFiltersExist, partClassFiltersExist, customerFiltersExist, linePartID, linePartRevision, partGroups, partClasses, parts, includePartForecasts);
			addParametersAndFillDataset(database, cutoffDate, adapter, dataSet, sODeliveriesDemandQuery, "Demands");
			DataView defaultView = dataSet.Tables["Demands"].DefaultView;
			defaultView.Sort = "mrrPartID Asc, mrrPartRevisionID Asc";
			FillWarehouseBinForPartDemands(database, defaultView);
			FillWarehouseBinPlantForPartForecastDemands(database, defaultView, plantFiltersExist, warehouseFiltersExist, text, text2);
			sODeliveriesDemandQuery = GetJobAssemblySupplyQuery(database, sessionID, lineID, partFiltersExist, partGroupFiltersExist, partClassFiltersExist, warehouseFiltersExist, plantFiltersExist, customerFiltersExist, linePartID, linePartRevision, partGroups, partClasses, parts, text, text2, customers);
			addParametersAndFillDataset(database, cutoffDate, adapter, dataSet, sODeliveriesDemandQuery, "Supply");
			DataView defaultView2 = dataSet.Tables["Supply"].DefaultView;
			defaultView2.Sort = "mrsPartID Asc, mrsPartRevisionID Asc";
			setDetailIDsForDemandAndSupplyDataView(defaultView, defaultView2, lineID);
			IEnumerable<DemandsInfo> enumerable = from r in defaultView.ToTable().AsEnumerable()
				group r by new
				{
					SessionID = r.Field<string>("mrrSessionID"),
					LineID = r.Field<int>("mrrLineID"),
					Part = r.Field<string>("mrrPartID"),
					Revision = r.Field<string>("mrrPartRevisionID")
				} into g
				select new DemandsInfo
				{
					SessionID = g.Key.SessionID,
					LineID = g.Key.LineID,
					Part = g.Key.Part,
					Revision = g.Key.Revision,
					Warehouse = string.Empty
				};
			IEnumerable<DemandsInfo> partDemandWithWarehouse = from r in defaultView.ToTable().AsEnumerable()
				group r by new
				{
					SessionID = r.Field<string>("mrrSessionID"),
					LineID = r.Field<int>("mrrLineID"),
					Part = r.Field<string>("mrrPartID"),
					Revision = r.Field<string>("mrrPartRevisionID"),
					Warehouse = r.Field<string>("mrrPartWarehouseLocationID")
				} into g
				select new DemandsInfo
				{
					SessionID = g.Key.SessionID,
					LineID = g.Key.LineID,
					Part = g.Key.Part,
					Revision = g.Key.Revision,
					Warehouse = g.Key.Warehouse
				};
			M1BindingSource m1BindingSource = new M1BindingSource(database);
			m1BindingSource.LoadDefinition(string.Empty, "MRPSessions", null, true, loadDataNow: false);
			m1BindingSource.ClearCache();
			m1BindingSource.NavigateTo(database, "mrpSessionID = " + M1Util.ConvertToSql(sessionID));
			if (m1BindingSource.CurrentAsDataRow == null)
			{
				return "No current session in the binding source. Try again";
			}
			DataTable mRPLinesNotCompleted = GetMRPLinesNotCompleted(database, sessionID, plantFiltersExist: false, warehouseFiltersExist: false, text, text2);
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			if (mRPLinesNotCompleted.Rows.Count != 0)
			{
				foreach (DataRow row3 in mRPLinesNotCompleted.Rows)
				{
					list2.Add(row3.Field<string>("mrrPartID"));
					list.Add(row3.Field<string>("mrrPartWarehouseLocationID"));
					list3.Add(row3.Field<string>("mrrPartRevisionID"));
				}
			}
			text2.Split(' ');
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("MRPLines");
			foreach (DemandsInfo item in enumerable)
			{
				foreach (string warehousesOfPart in GetWarehousesOfParts(item, partDemandWithWarehouse))
				{
					if (CanCreate(item.Part, item.Revision, warehousesOfPart, list2, list3, list))
					{
						DataRow obj = childBindingSource.AddNew() as DataRow;
						decimal mfgLotSize = GetMfgLotSize(database, item.Part, item.Revision);
						decimal quantityToInspect = GetQuantityToInspect(database, item.Part, item.Revision, plantFiltersExist, text, warehouseFiltersExist, text2, customerFiltersExist, customers);
						decimal quantityAllocated = GetQuantityAllocated(database, item.Part, item.Revision, plantFiltersExist, text, warehouseFiltersExist, text2, customerFiltersExist, customers, cutoffDate);
						decimal inventoryQuantityInProduction = GetInventoryQuantityInProduction(database, item.Part, item.Revision, cutoffDate, text, text2);
						decimal warehouseQuantityOnHand = GetWarehouseQuantityOnHand(database, item.Part, item.Revision, text, text2);
						decimal minimumQuantity = GetMinimumQuantity(database, item.Part, item.Revision, text, text2);
						decimal maximumQuantity = GetMaximumQuantity(database, item.Part, item.Revision, text, text2);
						decimal forecastDemandQuantity = GetForecastDemandQuantity(defaultView, item.Part, item.Revision);
						obj["mrlSessionID"] = item.SessionID;
						obj["mrlLineID"] = item.LineID;
						obj["mrlPlantIDs"] = RemoveSqlFormat(text);
						obj["mrlWarehouseIDs"] = RemoveSqlFormat(text2);
						obj["mrlPartID"] = item.Part;
						obj["mrlPartRevisionID"] = item.Revision;
						obj["mrlCreatedBy"] = database.User.ID;
						obj["mrlCreatedDate"] = DateTime.Now;
						obj["mrlMfgLotSize"] = mfgLotSize;
						obj["mrlQuantityToInspect"] = quantityToInspect;
						obj["mrlQuantityAllocated"] = quantityAllocated;
						obj["mrlInvQtyInProduction"] = inventoryQuantityInProduction;
						obj["mrlQuantityOnHand"] = warehouseQuantityOnHand;
						obj["mrlMinimumQuantity"] = minimumQuantity;
						obj["mrlMaximumQuantity"] = maximumQuantity;
						obj["mrlForecastDemand"] = forecastDemandQuantity;
						break;
					}
				}
			}
			SqlTransaction sqlTransaction = database.BeginTransaction();
			try
			{
				if (childBindingSource.Count > 0)
				{
					M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("MRPDemands");
					foreach (DataRowView item2 in defaultView)
					{
						if (CanCreate(item2.Row["mrrPartID"].ToString(), item2.Row["mrrPartRevisionID"].ToString(), item2.Row["mrrPartWarehouseLocationID"].ToString(), list2, list3, list))
						{
							DataRow destData = childBindingSource2.AddNew() as DataRow;
							M1Util.CopyMatchingFields(item2.Row, destData, "mrr,umrr");
						}
					}
					M1BindingSource childBindingSource3 = childBindingSource.PrimaryTable.GetChildBindingSource("MRPSupply");
					foreach (DataRowView item3 in defaultView2)
					{
						if (CanCreate(item3.Row["mrsPartID"].ToString(), item3.Row["mrsPartRevisionID"].ToString(), item3.Row["mrsPartWarehouseLocationID"].ToString(), list2, list3, list))
						{
							DataRow destData2 = childBindingSource3.AddNew() as DataRow;
							M1Util.CopyMatchingFields(item3.Row, destData2, "mrs,umrs");
						}
					}
					CreateJobDetailRecords(defaultView, childBindingSource, lineID, cutoffDate, text, text2, list2, list3, list, consolidatePartForecastJob);
					if (lineID == 0)
					{
						DataRow[] array = childBindingSource.GetDataTable().Select();
						for (int num = 0; num < array.Length; num++)
						{
							array[num]["mrlLineID"] = num + 1;
						}
					}
				}
				if (lineID == 0)
				{
					DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
					if (currentAsDataRow != null)
					{
						if (childBindingSource.Count > 0)
						{
							currentAsDataRow.SetField("mrpGenerated", value: true);
						}
						else
						{
							currentAsDataRow.SetField("mrpGenerated", value: false);
							result = "No job data was generated for this filter criteria. Please adjust the filters to try again.";
						}
					}
				}
				m1BindingSource.SaveData();
				database.ExecuteCommand("update MRPLines\r\n                            set mrlDataMissing =\r\n                            (\r\n                            select IIF((select count(*)\r\n                            from MRPJobDetails\r\n                            where (mrjPartWarehouseLocationID = '' or mrjPartBinID = '')\r\n                            and mrlLineID = mrjLineID and mrjSessionID=" + sessionID.ToSql() + ") >= 1,1,0)\r\n                            )\r\n                            where mrlSessionID = " + sessionID.ToSql());
			}
			catch
			{
				database.RollbackTransaction(sqlTransaction);
				throw;
			}
			database.CommitTransaction(sqlTransaction);
		}
		Cursor.Current = Cursors.Arrow;
		return result;
	}

	public string MissingDetailCheck(M1Database database, string sessionID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Declare @string nvarchar(255) ");
		stringBuilder.AppendLine("Select @string = coalesce(@string + CHAR(10), '') + 'Line:' + cast(mrjLineID as nvarchar(5)) + ' JobID:' + cast(mrjJobID as nvarchar(20)) From MRPJobDetails where mrjSessionID = " + sessionID.ToSql());
		stringBuilder.AppendLine(" and ((mrjDirectLink = 1 and mrjSalesOrderID = '') or (mrjConsolidated = 1 and mrjSalesOrderID <> '') or (mrjIndirectLink = 1 and mrjSalesOrderID = '') or mrjPartWarehouseLocationID = '' or mrjPartBinID = '') and mrjSessionID = " + sessionID.ToSql() + " order by mrjLineID");
		stringBuilder.AppendLine("Select @string");
		string queryString = stringBuilder.ToString();
		return Convert.ToString(database.ExecuteScalar(queryString));
	}

	public string CheckMissingPlants(M1Database database, string sessionID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Declare @string nvarchar(4000) ");
		stringBuilder.AppendLine("Select @string = coalesce(@string + CHAR(10), '') + 'Line:' + cast(mrjLineID as nvarchar(5)) + ' JobID:' + cast(mrjJobID as nvarchar(20)) From MRPJobDetails where mrjExistingJob=0 and mrjSessionID = " + sessionID.ToSql());
		stringBuilder.AppendLine(" and mrjPartPlantID = '' and mrjSessionID = " + sessionID.ToSql() + " order by mrjLineID");
		stringBuilder.AppendLine("Select @string");
		string queryString = stringBuilder.ToString();
		return Convert.ToString(database.ExecuteScalar(queryString));
	}

	public bool HasJobDetailRows(M1Database database, string sessionID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Count(*) as JobDetailsRows From MRPJobDetails Where mrjSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		return (int)database.GetDataTable(sqlCommand).Rows[0]["JobDetailsRows"] > 0;
	}

	public string GetLinesWithMissingWarehouseOrBin(M1Database database, string sessionID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Declare @string nvarchar(4000) ");
		stringBuilder.AppendLine("Select @string = coalesce(@string + CHAR(10), '') + 'Line:' + cast(mrjLineID as nvarchar(5)) + ' JobID:' + cast(mrjJobID as nvarchar(20)) From MRPJobDetails where mrjSessionID = " + sessionID.ToSql());
		stringBuilder.AppendLine(" and (mrjPartWarehouseLocationID = '' or mrjPartBinID = '') and mrjCompleted <> 1 order by mrjLineID");
		stringBuilder.AppendLine("Select @string");
		string queryString = stringBuilder.ToString();
		return Convert.ToString(database.ExecuteScalar(queryString));
	}

	public bool PostMRPSession(M1BindingSource bindingSource)
	{
		if (bindingSource?.CurrentAsDataRow == null)
		{
			return false;
		}
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(mrpPlantIDs,'') As mrpPlantID, mrpWarehouseIDs, mrpPartClassIDs, mrpCustomerIDs, mrpPartGroupIDs, mrpCompletedDate, mrpCompleted, mrpCutoffDate, mrpGenerated, mrpPartIDs, MRPLines.*, MRPJobDetails.* From MRPSessions inner join MRPLines on mrpSessionID = mrlSessionID Inner Join MRPJobDetails on mrlSessionID = mrjSessionID and mrlLineID = mrjLineID Where mrpSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = currentAsDataRow.Field<string>("mrpSessionID");
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return false;
		}
		List<string[]> list = new List<string[]>();
		foreach (DataRow row3 in dataTable.Rows)
		{
			string text = row3.Field<string>("mrjJobID");
			int num = row3.Field<int>("mrjJobAssemblyID");
			string text2 = row3.Field<string>("mrjSalesOrderID");
			short num2 = row3.Field<short>("mrjSalesOrderLineID");
			short num3 = row3.Field<short>("mrjSalesOrderDeliveryID");
			bool value = row3.Field<bool>("mrjFirm");
			if ((row3.Field<bool>("mrjDirectLink").Equals(obj: true) && row3.Field<bool>("mrjExistingJob").Equals(obj: false) && !string.IsNullOrWhiteSpace(text2) && !num2.Equals(0) && !num3.Equals(0)) || (row3.Field<bool>("mrjIndirectLink").Equals(obj: false) && row3.Field<bool>("mrjExistingJob").Equals(obj: false)))
			{
				if (!string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(new Job().CreateJobEx(database, null, text, row3.Field<string>("mrjPartID"), row3.Field<string>("mrjPartRevisionID"), string.Empty, string.Empty, (double)row3.Field<decimal>("mrjOrderQuantity"), row3.Field<DateTime?>("mrjProductionDueDate"), text2, num2, num3, (double)row3.Field<decimal>("mrjInventoryQuantity"), row3.Field<string>("mrjPartPlantID"), string.Empty, string.Empty, row3.Field<string>("mrjCustomerOrganizationID"), row3.Field<string>("mrjShipOrganizationID"), row3.Field<string>("mrjShipLocationID"), row3.Field<string>("mrjPartWarehouseLocationID"), row3.Field<string>("mrjPartBinID"))) && row3.Field<bool>("mrjGetPartMethod"))
				{
					DataRow row2 = database.Props("PN");
					clsJobFunctions obj = (clsJobFunctions)((ScriptApp)database.GetService(typeof(ScriptApp))).Ax("JobFunctions");
					string cSourcePartID = row3.Field<string>("mrjPartID");
					string cSourceRevisionID = row3.Field<string>("mrjPartRevisionID");
					bool bOverwriteMethod = row2.Field<bool>("xapJMOverwriteMethod");
					bool bOverwriteDescription = row2.Field<bool>("xapJMOverwriteDescription");
					bool bOverwriteDocuments = row2.Field<bool>("xapJMOverwriteDocuments");
					bool bRefreshMaterialDescriptions = row2.Field<bool>("xapJMRefreshMaterial");
					bool bRefreshMaterialCosts = row2.Field<bool>("xapJMRefreshMaterialCosts");
					object aParameters = "";
					obj.GetMethod(text, 0, cSourcePartID, cSourceRevisionID, bOverwriteMethod, bOverwriteDescription, bOverwriteDocuments, bRefreshMaterialDescriptions, bRefreshMaterialCosts, ref aParameters);
				}
			}
			else if (row3.Field<bool>("mrjIndirectLink").Equals(obj: true) && row3.Field<bool>("mrjExistingJob").Equals(obj: false))
			{
				SalesOrder salesOrder = new SalesOrder();
				int salesOrderJobLinkID = (int)database.NextIDs.GetNextIDForTable("SalesOrderJobLinks", new object[2] { text2, num2 }, null);
				salesOrder.CreateSalesOrderJobLinks(database, null, text2, num2, salesOrderJobLinkID, 3, num3, text, closed: false, database.User.ID, DateTime.Now);
			}
			else if (row3.Field<bool>("mrjExistingJob").Equals(obj: true) && row3.Field<bool>("mrjConsolidated").Equals(obj: true) && row3.Field<bool>("mrjIndirectLink").Equals(obj: false))
			{
				if (!num.Equals(0))
				{
					M1BindingSource m1BindingSource = new M1BindingSource(database)
					{
						DataSourceTable = "JobAssemblies"
					};
					m1BindingSource.NavigateTo(database, "jmaJobID = " + M1Util.ConvertToSql(text) + " and jmaJobAssemblyID = " + M1Util.ConvertToSql(num));
					if (m1BindingSource != null)
					{
						m1BindingSource.CurrentAsDataRow.SetField("jmaInventoryQuantity", m1BindingSource.CurrentAsDataRow.Field<decimal>("jmaInventoryQuantity") + row3.Field<decimal>("mrjInventoryQuantity"));
						m1BindingSource.SaveData();
					}
				}
				else
				{
					M1BindingSource m1BindingSource2 = new M1BindingSource(database)
					{
						DataSourceTable = "Jobs"
					};
					m1BindingSource2.NavigateTo(database, "jmpJobID = " + M1Util.ConvertToSql(text));
					if (m1BindingSource2 != null)
					{
						m1BindingSource2.CurrentAsDataRow.SetField("jmpInventoryQuantity", m1BindingSource2.CurrentAsDataRow.Field<decimal>("jmpInventoryQuantity") + row3.Field<decimal>("mrjInventoryQuantity"));
						m1BindingSource2.CurrentAsDataRow.SetField("jmpOrderQuantity", m1BindingSource2.CurrentAsDataRow.Field<decimal>("jmpOrderQuantity") + row3.Field<decimal>("mrjOrderQuantity"));
						m1BindingSource2.SaveData();
					}
				}
			}
			else if (row3.Field<bool>("mrjExistingJob").Equals(obj: true) && row3.Field<bool>("mrjIndirectLink").Equals(obj: true) && num.Equals(0))
			{
				M1BindingSource m1BindingSource3 = new M1BindingSource(database)
				{
					DataSourceTable = "Jobs"
				};
				m1BindingSource3.NavigateTo(database, "jmpJobID = " + M1Util.ConvertToSql(text));
				if (m1BindingSource3 != null)
				{
					m1BindingSource3.CurrentAsDataRow.SetField("jmpOrderQuantity", m1BindingSource3.CurrentAsDataRow.Field<decimal>("jmpOrderQuantity") + row3.Field<decimal>("mrjOrderQuantity"));
					m1BindingSource3.CurrentAsDataRow.SetField("jmpInventoryQuantity", m1BindingSource3.CurrentAsDataRow.Field<decimal>("jmpInventoryQuantity") + row3.Field<decimal>("mrjInventoryQuantity"));
					m1BindingSource3.SaveData();
				}
				SalesOrder salesOrder2 = new SalesOrder();
				int salesOrderJobLinkID2 = (int)database.NextIDs.GetNextIDForTable("SalesOrderJobLinks", new object[2] { text2, num2 }, null);
				salesOrder2.CreateSalesOrderJobLinks(database, null, text2, num2, salesOrderJobLinkID2, 3, num3, text, closed: false, database.User.ID, DateTime.Now);
			}
			if (!string.IsNullOrEmpty(text))
			{
				M1BindingSource m1BindingSource4 = new M1BindingSource(database)
				{
					DataSourceTable = "Jobs"
				};
				m1BindingSource4.NavigateTo(database, "jmpJobID = " + M1Util.ConvertToSql(text));
				if (m1BindingSource4 != null)
				{
					m1BindingSource4.CurrentAsDataRow.SetField("jmpFirm", value);
					m1BindingSource4.SaveData();
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				list.Add(new string[1] { text });
			}
		}
		currentAsDataRow.SetField("mrpCompleted", value: true);
		bindingSource.SaveData();
		IOpenObject openObject = (IOpenObject)database.GetService(typeof(IOpenObject));
		if (list.Count != 0)
		{
			openObject?.OpenObject("Job", list.ToArray());
		}
		return true;
	}

	private static void FillWarehouseBinForPartDemands(M1Database database, DataView demands)
	{
		string filterExpression = "mrrSource = 'PartRevisions' and mrrType = 'MinMax'";
		if (demands.Table.Select(filterExpression).Length != 0)
		{
			Part part = new Part();
			DataRow[] array = demands.Table.Select("mrrSource = 'PartRevisions'");
			foreach (DataRow dataRow in array)
			{
				dataRow["mrrPartBinID"] = part.GetPreferredWarehouseBin(database, dataRow["mrrPartID"].ToString(), dataRow["mrrPartRevisionID"].ToString(), dataRow["mrrPartWarehouseLocationID"].ToString(), dataRow["mrrPartPlantID"].ToString());
			}
		}
	}

	private static void FillWarehouseBinPlantForPartForecastDemands(M1Database database, DataView demands, bool plantFiltersExist, bool warehouseFiltersExist, string plants, string warehouses)
	{
		if (demands.Table.Select("mrrSource = 'PartRevisions' and mrrType = 'PartForecast'").Length == 0)
		{
			return;
		}
		Part part = new Part();
		Plant plant = new Plant();
		Dictionary<string, PartForecastInfo> dictionary = new Dictionary<string, PartForecastInfo>();
		DataRow[] array = demands.Table.Select("mrrSource = 'PartRevisions' and mrrType = 'PartForecast'");
		foreach (DataRow dataRow in array)
		{
			string text = dataRow["mrrPartID"].ToString();
			string text2 = dataRow["mrrPartRevisionID"].ToString();
			string key = (text + text2).Trim();
			PartForecastInfo partForecastInfo;
			if (dictionary.ContainsKey(key))
			{
				partForecastInfo = dictionary[key];
			}
			else
			{
				bool flag = true;
				bool flag2 = true;
				bool flag3 = false;
				string preferredWarehouse = part.GetPreferredWarehouse(database, text, text2, string.Empty);
				string preferredWarehouseBin = part.GetPreferredWarehouseBin(database, text, text2, preferredWarehouse, string.Empty);
				string plantID = plant.GetWarehousePlant(database, null, preferredWarehouse).PlantID;
				if (plantFiltersExist)
				{
					flag = (!string.IsNullOrEmpty(plantID) || string.IsNullOrEmpty(plants)) && plants.Contains(plantID);
				}
				if (!plantFiltersExist && warehouseFiltersExist)
				{
					flag2 = warehouses.Contains(preferredWarehouse);
				}
				if (!string.IsNullOrEmpty(preferredWarehouse) && !string.IsNullOrEmpty(preferredWarehouseBin))
				{
					flag3 = part.IsPartBinInactive(database, text, text2, preferredWarehouse, preferredWarehouseBin);
				}
				partForecastInfo = new PartForecastInfo
				{
					WarehouseId = preferredWarehouse,
					BinId = preferredWarehouseBin,
					PlantId = plantID,
					ShouldBeDeleted = (!flag || !flag2 || flag3 || string.IsNullOrEmpty(preferredWarehouse) || string.IsNullOrEmpty(preferredWarehouseBin))
				};
				dictionary.Add(key, partForecastInfo);
			}
			if (partForecastInfo.ShouldBeDeleted)
			{
				dataRow.Delete();
				continue;
			}
			dataRow["mrrPartWarehouseLocationID"] = partForecastInfo.WarehouseId;
			dataRow["mrrPartBinID"] = partForecastInfo.BinId;
			dataRow["mrrPartPlantID"] = partForecastInfo.PlantId;
		}
	}

	private static string GetWarehouseBin(M1Database database, string partId, string partRevisionId, string warehouseId)
	{
		int num = (int)database.ExecuteScalar("Select COUNT(imbPartBinId) From PartBins Where imbPartID = " + partId.ToSql() + " And imbPartRevisionID = " + partRevisionId.ToSql() + " and imbWarehouseID = " + warehouseId.ToSql());
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT imbPartBinID FROM PartBins WHERE imbPartID = @PartId AND imbPartRevisionID = @PartRevisionId AND imbWarehouseID = @WarehouseId");
		sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseId", SqlDbType.NVarChar)).Value = warehouseId;
		SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT TOP 1 ISNULL(binId, '')\r\n                                                FROM (\r\n                                                    SELECT 1 AS BinType, imbWarehouseID AS warehouseId, imbPartBinID AS binId \r\n                                                        FROM PartBins With(NoLock) LEFT OUTER JOIN WarehouseBins With(NoLock) ON imbWarehouseID = inbWarehouseID\r\n                                                        WHERE inbInactive = 0 AND imbDefaultBin = 1 AND imbPartID = @PartId AND imbPartRevisionID = @PartRevisionId AND imbWarehouseID = @WarehouseId\r\n                                                    UNION ALL\r\n                                                    SELECT 2 As BinType, inbWarehouseID as warehouseId, inbWarehouseBinID as binId\r\n                                                        FROM PartBins With(NoLock) LEFT OUTER JOIN WarehouseBins With(NoLock) ON imbWarehouseID = inbWarehouseID\r\n                                                        WHERE inbInactive = 0 AND inbDefaultBin = 1 AND imbPartID = @PartId AND imbPartRevisionID = @PartRevisionId AND imbWarehouseID = @WarehouseId \r\n                                                ) As data\r\n                                                ORDER BY BinType ASC, warehouseId ASC");
		sqlCommand2.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
		sqlCommand2.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
		sqlCommand2.Parameters.Add(new SqlParameter("@WarehouseId", SqlDbType.NVarChar)).Value = warehouseId;
		return ((num == 1) ? ((string)database.ExecuteScalar(sqlCommand)) : ((string)database.ExecuteScalar(sqlCommand2))) ?? string.Empty;
	}

	private static decimal GetMfgLotSize(M1Database database, string partID, string partRevisionID)
	{
		return (decimal)database.ExecuteScalar("(Select IsNull(imrManufacturingLotSize,0) As imrManufacturingLotSize From PartRevisions Where imrPartID = " + partID.ToSql() + " And imrPartRevisionID = " + partRevisionID.ToSql() + " )");
	}

	private static decimal GetQuantityToInspect(M1Database database, string partId, string partRevisionId, bool plantFiltersExist, string plants, bool warehouseFiltersExist, string warehouses, bool customerFiltersExist, string customers)
	{
		string queryString = "SELECT COALESCE(SUM(qalQuantityToInspect), 0) AS mrlQuantityToInspect\r\n                                FROM InspectionLines\r\n                                LEFT OUTER JOIN Warehouses w on qalPartWarehouseLocationID = imwWarehouseID\r\n                                LEFT OUTER JOIN Jobs j on jmpJobID = qalJobID\r\n                                WHERE ((qalStatus in ('P', 'O') AND qalManualInspectionFinalized = 1 AND qalInspectionType = 1) OR (qalStatus in ('P', 'O') AND qalSourceTableName != ''))\r\n                                AND qalPartID = '" + partId + "' AND qalPartRevisionId = '" + partRevisionId + "'\r\n                                " + (plantFiltersExist ? (" and imwPlantID In (" + plants + ")") : string.Empty) + "\r\n                                " + ((warehouseFiltersExist && !plantFiltersExist) ? (" and qalPartWarehouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                                " + (customerFiltersExist ? (" and jmpCustomerOrganizationID In(" + customers + ")") : string.Empty);
		return (decimal)database.ExecuteScalar(queryString);
	}

	private static decimal GetQuantityAllocated(M1Database database, string partId, string partRevisionId, bool plantFiltersExist, string plants, bool warehouseFiltersExist, string warehouses, bool customerFiltersExist, string customers, DateTime cutoffDate)
	{
		string text = "select imwWarehouseID from Warehouses where imwPlantID in (" + plants + ")";
		string value = "SELECT COALESCE(SUM((omdDeliveryQuantity - omdQuantityShipped)), 0) as quantityAllocated\r\n                From SalesOrders \r\n                left outer join SalesOrderLines on omlSalesOrderID = ompSalesOrderID \r\n                left outer join SalesOrderDeliveries on omdSalesOrderID = omlSalesOrderID and omdSalesOrderLineID = omlSalesOrderLineID \r\n                left outer join Warehouses on imwWarehouseID=omdPartWarehouseLocationID \r\n                left outer join Plants on xauPlantID = imwPlantID \r\n                Where ompClosed = 0 and omdShippedComplete = 0 and omdDeliveryType = 2\r\n                and (omdDeliveryQuantity - omdQuantityShipped) > 0\r\n                and omdDeliveryDate < @CutOffDate\r\n                " + (plantFiltersExist ? (" and omdPartWarehouseLocationID In (" + text + ")") : string.Empty) + "\r\n                " + ((!plantFiltersExist && warehouseFiltersExist) ? (" and omdPartWarehouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                " + (customerFiltersExist ? (" and ompCustomerOrganizationID In(" + customers + ")") : string.Empty) + "\r\n                and omdPartID = " + partId.ToSql() + " and omdPartRevisionID = " + partRevisionId.ToSql();
		string value2 = "SELECT COALESCE(SUM((jmaQuantityToPull - jmaQuantityIssued)), 0) As quantityAllocated\r\n                From JobAssemblies \r\n                left outer join Jobs on jmaJobID = jmpJobID\r\n                left outer join Warehouses on imwWarehouseID=jmaPartWareHouseLocationID\r\n                left outer join Plants on xauPlantID=imwPlantID\r\n                Where (jmaQuantityToPull - jmaQuantityIssued) > 0 and jmaIssuedComplete = 0\r\n                and IsNull(jmaScheduledDueDate, jmpProductionDueDate) < @CutOffDate\r\n                and jmpClosed = 0\r\n                " + (plantFiltersExist ? (" and jmaPartWarehouseLocationID In (" + text + ")") : string.Empty) + "\r\n                " + ((warehouseFiltersExist && !plantFiltersExist) ? (" and jmaPartWareHouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                " + (customerFiltersExist ? (" and jmpCustomerOrganizationID In(" + customers + ")") : string.Empty) + "\r\n                and jmaPartID = " + partId.ToSql() + " and jmaPartRevisionID = " + partRevisionId.ToSql();
		string value3 = "SELECT COALESCE(SUM((jmmPullFromStockQuantity - jmmQuantityReceived)), 0) AS quantityAllocated \r\n                From JobMaterials \r\n                left outer join Jobs on jmmJobID = jmpJobID \r\n                left outer join Warehouses on imwWarehouseID=jmmPartWareHouseLocationID\r\n                left outer join Plants on xauPlantID=imwPlantID\r\n                Where (jmmPullFromStockQuantity - jmmQuantityReceived) > 0 and jmmReceivedComplete = 0 \r\n                and IsNull(jmmRequiredDate, jmpProductionDueDate) < @CutOffDate \r\n                and jmpClosed = 0\r\n                " + (plantFiltersExist ? (" and jmmPartWareHouseLocationID In (" + text + ")") : string.Empty) + "\r\n                " + ((warehouseFiltersExist && !plantFiltersExist) ? (" and jmmPartWarehouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                " + (customerFiltersExist ? (" and jmpCustomerOrganizationID In(" + customers + ")") : string.Empty) + "\r\n                and jmmPartID = " + partId.ToSql() + " and jmmPartRevisionID = " + partRevisionId.ToSql();
		StringBuilder stringBuilder = new StringBuilder("SELECT SUM(quantityAllocated) FROM ( ");
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine("UNION ALL");
		stringBuilder.AppendLine(value2);
		stringBuilder.AppendLine("UNION ALL");
		stringBuilder.AppendLine(value3);
		stringBuilder.AppendLine(") AS tempTable");
		SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.Add(new SqlParameter("@CutOffDate", SqlDbType.DateTime)).Value = cutoffDate;
		return (decimal)database.ExecuteScalar(sqlCommand);
	}

	private static decimal GetInventoryQuantityInProduction(M1Database database, string partId, string partRevisionId, DateTime cutoffDate, string plants, string warehouses)
	{
		return new Part().GetInventoryQuantityInProduction(database, partId, partRevisionId, cutoffDate, plants, warehouses);
	}

	private decimal GetWarehouseQuantityOnHand(M1Database database, string partId, string partRevisionId, string plants, string warehouses)
	{
		bool flag = !string.IsNullOrWhiteSpace(plants);
		bool flag2 = !string.IsNullOrEmpty(warehouses);
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Sum(IsNull(imbQuantityOnHand, 0)), 0) As imbQuantityOnHand \r\n                            From PartBins \r\n                            left outer join Warehouses w on imwWarehouseID = imbWarehouseID \r\n                            Where imbPartID = @PartId And imbPartRevisionID = @PartRevisionId\r\n                            " + (flag ? (" and imwPlantID in (" + plants + ") ") : string.Empty) + "\r\n                            " + ((!flag && flag2) ? (" and imwWarehouseID in (" + warehouses + ") ") : string.Empty));
		sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
		return (decimal)database.ExecuteScalar(sqlCommand);
	}

	private static decimal GetMinimumQuantity(M1Database database, string partId, string partRevisionId, string plants, string warehouses)
	{
		bool flag = !string.IsNullOrWhiteSpace(plants);
		bool flag2 = !string.IsNullOrEmpty(warehouses);
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Sum(IsNull(imlMinimumQuantity, 0)), 0) As imlMinimumQuantity\r\n                        From PartWarehouseLocations\r\n                        left outer join Warehouses w on imwWarehouseID = imlPartWarehouseID \r\n                        Where imlPartID = @PartId And imlPartRevisionID = @PartRevisionId \r\n                        " + (flag ? (" and imwPlantID in (" + plants + ") ") : string.Empty) + "\r\n                        " + ((!flag && flag2) ? (" and imlPartWarehouseID in (" + warehouses + ") ") : string.Empty));
		sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
		return (decimal)database.ExecuteScalar(sqlCommand);
	}

	private static decimal GetMaximumQuantity(M1Database database, string partId, string partRevisionId, string plants, string warehouses)
	{
		bool flag = !string.IsNullOrWhiteSpace(plants);
		bool flag2 = !string.IsNullOrEmpty(warehouses);
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Sum(IsNull(imlMaximumQuantity, 0)), 0) As imlMaximumQuantity\r\n                        From PartWarehouseLocations\r\n                        left outer join Warehouses w on imwWarehouseID = imlPartWarehouseID \r\n                        Where imlPartID = @PartId And imlPartRevisionID = @PartRevisionId \r\n                        " + (flag ? (" and imwPlantID in (" + plants + ") ") : string.Empty) + "\r\n                        " + ((!flag && flag2) ? (" and imlPartWarehouseID in (" + warehouses + ") ") : string.Empty));
		sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
		return (decimal)database.ExecuteScalar(sqlCommand);
	}

	private static decimal GetForecastDemandQuantity(DataView demands, string partId, string partRevisionId)
	{
		decimal result = default(decimal);
		string text = "mrrSource = 'PartRevisions' and mrrType = 'PartForecast'";
		string filterExpression = text + " and mrrPartID = '" + partId + "' and mrrPartRevisionID = '" + partRevisionId + "'";
		if (demands.Table.Select(filterExpression).Length != 0)
		{
			DataRow[] array = demands.Table.Select(filterExpression);
			foreach (DataRow dataRow in array)
			{
				result += Convert.ToDecimal(dataRow["mrrDemandQuantity"]);
			}
		}
		return result;
	}

	private void addParametersAndFillDataset(M1Database database, DateTime cutoffDate, SqlDataAdapter adapter, DataSet dataset, string query, string sourceTable)
	{
		SqlCommand sqlCommand = database.NewSqlCommand(query);
		sqlCommand.Parameters.Add(new SqlParameter("@CutOffDate", SqlDbType.DateTime)).Value = cutoffDate;
		adapter.SelectCommand = sqlCommand;
		adapter.Fill(dataset, sourceTable);
	}

	private bool CanCreate(string part, string partRevision, string warehouse, List<string> partsNotCompleted, List<string> partRevisionsNotCompleted, List<string> warehousesNotCompleted)
	{
		for (int i = 0; i < partsNotCompleted.Count; i++)
		{
			if (partsNotCompleted[i] == part && partRevisionsNotCompleted[i] == partRevision && warehousesNotCompleted[i] == warehouse)
			{
				return false;
			}
		}
		return true;
	}

	private void CreateForecastJobDetailRecord(M1BindingSource mrpJobDetailsBs, DataRow demandRow, List<string> partsNotCompleted, List<string> partRevisionsNotCompleted, List<string> warehousesNotCompleted, string nextJobId, decimal demandQuantity, DateTime dueDate, decimal mfgLotSize, bool consolidatedJob, bool partForecastFirmValue, ref int detailCounter)
	{
		string part = demandRow.Field<string>("mrrPartID");
		string partRevision = demandRow.Field<string>("mrrPartRevisionID");
		string text = demandRow.Field<string>("mrrPartWarehouseLocationID");
		string value = demandRow.Field<string>("mrrPartBinID");
		if (!CanCreate(part, partRevision, text, partsNotCompleted, partRevisionsNotCompleted, warehousesNotCompleted))
		{
			return;
		}
		DataRow dataRow = (DataRow)mrpJobDetailsBs.AddNew();
		if (dataRow == null)
		{
			return;
		}
		dataRow["mrjLineID"] = demandRow["mrrLineID"];
		dataRow["mrjJobDetailID"] = ++detailCounter;
		dataRow["mrjJobID"] = nextJobId;
		dataRow["mrjSalesOrderID"] = string.Empty;
		dataRow["mrjSalesOrderLineID"] = 0;
		dataRow["mrjSalesOrderDeliveryID"] = 0;
		dataRow["mrjCustomerOrganizationID"] = string.Empty;
		dataRow["mrjShipOrganizationID"] = string.Empty;
		dataRow["mrjShipLocationID"] = string.Empty;
		dataRow["mrjPartPlantID"] = demandRow["mrrPartPlantID"];
		dataRow["mrjPartID"] = demandRow["mrrPartID"];
		dataRow["mrjPartRevisionID"] = demandRow["mrrPartRevisionID"];
		dataRow["mrjPartWarehouseLocationID"] = demandRow["mrrPartWarehouseLocationID"];
		dataRow["mrjPartBinID"] = demandRow["mrrPartBinID"];
		dataRow["mrjDataMissing"] = string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value);
		dataRow["mrjOrderQuantity"] = 0;
		dataRow["mrjFirm"] = partForecastFirmValue;
		if (mfgLotSize > 0m)
		{
			if (demandQuantity % mfgLotSize == 0m)
			{
				dataRow["mrjInventoryQuantity"] = demandQuantity;
			}
			else
			{
				dataRow["mrjInventoryQuantity"] = mfgLotSize - demandQuantity % mfgLotSize + demandQuantity;
			}
		}
		else
		{
			dataRow["mrjInventoryQuantity"] = demandQuantity;
		}
		dataRow["mrjProductionDueDate"] = dueDate;
		dataRow["mrjGetPartMethod"] = true;
		dataRow["mrjDirectLink"] = false;
		dataRow["mrjIndirectLink"] = false;
		dataRow["mrjConsolidated"] = consolidatedJob;
		dataRow["mrjExistingJob"] = false;
		dataRow["mrjCompleted"] = false;
	}

	private void CreateJobDetailRecords(DataView demandsDv, M1BindingSource mrpLinesBs, int lineID, DateTime cutoffDate, string plants, string warehouses, List<string> partsNotCompleted, List<string> partRevisionsNotCompleted, List<string> warehousesNotCompleted, bool consolidatePartForecastJob)
	{
		string arg = "mrrSource = 'SalesOrderDeliveries' and mrrType = 'MakeToOrder'";
		string arg2 = "mrrSource = 'PartRevisions' and mrrType = 'PartForecast'";
		M1BindingSource childBindingSource = mrpLinesBs.PrimaryTable.GetChildBindingSource("MRPJobDetails");
		DataRow[] array = mrpLinesBs.GetDataView().Table.Select(lineID.Equals(0) ? string.Empty : $"mrlLineID = {lineID}");
		foreach (DataRow dataRow in array)
		{
			int detailCounter = 0;
			List<string> mrpCreatedJobIDs = new List<string>();
			decimal mfgLotSize = GetMfgLotSize(mrpLinesBs.Database, dataRow["mrlPartID"].ToString(), dataRow["mrlPartRevisionID"].ToString());
			DataRow[] array2 = demandsDv.Table.Select(string.Format("mrrLineID = {0} and {1}", dataRow.Field<int>("mrlLineID"), arg));
			foreach (DataRow dataRow2 in array2)
			{
				if (!CanCreate(dataRow2["mrrPartID"].ToString(), dataRow2["mrrPartRevisionID"].ToString(), dataRow2["mrrPartWarehouseLocationID"].ToString(), partsNotCompleted, partRevisionsNotCompleted, warehousesNotCompleted))
				{
					continue;
				}
				DataRow dataRow3 = (DataRow)childBindingSource.AddNew();
				if (dataRow3 != null)
				{
					dataRow3["mrjLineID"] = dataRow2["mrrLineID"];
					dataRow3["mrjJobDetailID"] = ++detailCounter;
					dataRow3["mrjSalesOrderID"] = dataRow2["mrrSalesOrderID"];
					dataRow3["mrjSalesOrderLineID"] = dataRow2["mrrSalesOrderLineID"];
					dataRow3["mrjSalesOrderDeliveryID"] = dataRow2["mrrSalesOrderDeliveryID"];
					dataRow3["mrjJobID"] = GetJobIDFromSalesOrder(mrpLinesBs.Database, dataRow2["mrrSalesOrderID"].ToString(), dataRow2["mrrSalesOrderLineID"].ToString(), dataRow2["mrrSalesOrderDeliveryID"].ToString(), ref mrpCreatedJobIDs);
					dataRow3["mrjCustomerOrganizationID"] = dataRow2["mrrCustomerOrganizationID"];
					dataRow3["mrjShipOrganizationID"] = dataRow2["mrrShipOrganizationID"];
					dataRow3["mrjShipLocationID"] = dataRow2["mrrShipLocationID"];
					dataRow3["mrjPartID"] = dataRow2["mrrPartID"];
					dataRow3["mrjPartRevisionID"] = dataRow2["mrrPartRevisionID"];
					dataRow3["mrjPartWarehouseLocationID"] = dataRow2["mrrPartWarehouseLocationID"];
					dataRow3["mrjPartPlantID"] = dataRow2["mrrPartPlantID"];
					dataRow3["mrjPartBinID"] = dataRow2["mrrPartBinID"];
					dataRow3["mrjFirm"] = true;
					dataRow3["mrjDataMissing"] = string.IsNullOrEmpty(dataRow3["mrjPartWarehouseLocationID"].ToString()) || string.IsNullOrEmpty(dataRow3["mrjPartBinID"].ToString());
					dataRow3["mrjOrderQuantity"] = dataRow2["mrrDemandQuantity"];
					if (mfgLotSize > 0m && (decimal)dataRow3["mrjOrderQuantity"] % mfgLotSize > 0m)
					{
						dataRow3["mrjInventoryQuantity"] = Math.Floor((decimal)dataRow3["mrjOrderQuantity"] / mfgLotSize) * mfgLotSize + mfgLotSize - (decimal)dataRow3["mrjOrderQuantity"];
					}
					dataRow3["mrjDirectLink"] = true;
					dataRow3["mrjGetPartMethod"] = true;
					dataRow3["mrjProductionDueDate"] = dataRow2["mrrDueDate"];
				}
			}
			int num = dataRow.Field<int>("mrlLineID");
			string arg3 = "mrrSource = 'PartRevisions' and mrrType = 'MinMax'";
			string arg4 = "mrrSource = 'SalesOrderDeliveries' and not mrrType = 'MakeToOrder'";
			string arg5 = "mrrSource = 'JobAssemblies' or mrrSource = 'JobMaterials'";
			DataRow[] array3 = demandsDv.Table.Select($"mrrLineID = {num} and {arg3}");
			DataRow[] array4 = demandsDv.Table.Select($"mrrLineID = {num} and {arg4}");
			DataRow[] array5 = demandsDv.Table.Select($"mrrLineID = {num} and ({arg5})");
			string filterExpression = string.Format("mrrLineID = {0} and {1}", dataRow.Field<int>("mrlLineID"), arg2);
			bool flag = demandsDv.Table.Select(filterExpression).Length != 0;
			string partId = dataRow.Field<string>("mrlPartID");
			string partRevisionId = dataRow.Field<string>("mrlPartRevisionID");
			decimal num2 = array3.Sum((DataRow x) => x.Field<decimal>("mrrDemandQuantity"));
			decimal num3 = array4.Sum((DataRow x) => x.Field<decimal>("mrrDemandQuantity"));
			decimal num4 = array5.Sum((DataRow x) => x.Field<decimal>("mrrDemandQuantity"));
			decimal inventoryQuantityInProduction = GetInventoryQuantityInProduction(mrpLinesBs.Database, partId, partRevisionId, cutoffDate, plants, warehouses);
			decimal num5 = dataRow.Field<decimal>("mrlQuantityOnHand");
			decimal num6 = dataRow.Field<decimal>("mrlQuantityToInspect");
			decimal num7 = default(decimal);
			decimal num8 = default(decimal);
			decimal num9 = default(decimal);
			decimal num10 = default(decimal);
			if (array3.Length != 0 && array4.Length == 0 && array5.Length == 0 && !flag)
			{
				num9 = num2 + num3 + num4 - inventoryQuantityInProduction;
			}
			else
			{
				array2 = array3;
				foreach (DataRow row in array2)
				{
					string partId2 = row.Field<string>("mrrPartID");
					string partRevisionId2 = row.Field<string>("mrrPartRevisionID");
					string warehouseId = row.Field<string>("mrrPartWarehouseLocationID");
					num7 += GetPartDemandQuantityOnHand(mrpLinesBs.Database, partId2, partRevisionId2, warehouseId);
					num8 += GetPartDemandQuantityToInspect(mrpLinesBs.Database, partId2, partRevisionId2, warehouseId);
				}
				num5 -= num7;
				num6 -= num8;
				if (array3.Length != 0 && array4.Length == 0 && array5.Length == 0 && flag)
				{
					decimal num11 = default(decimal);
					num9 = num2 + num3 + num4 - inventoryQuantityInProduction;
					if (num9 < 0m)
					{
						num11 = Math.Abs(num9);
					}
					num10 = num5 + num6 + num11;
				}
				else
				{
					num9 = num2 + num3 + num4 - inventoryQuantityInProduction - num5 - num6;
					if (num9 < 0m)
					{
						num10 = Math.Abs(num9);
					}
				}
			}
			if (num9 > 0m && mrpLinesBs.CurrentAsDataRow != null)
			{
				DataRow dataRow4 = (DataRow)childBindingSource.AddNew();
				if (dataRow4 != null)
				{
					string plantForJobDetail = GetPlantForJobDetail(array3, array4, array5);
					string warehouseForJobDetail = GetWarehouseForJobDetail(array3, array4, array5);
					string value = (string.IsNullOrEmpty(warehouseForJobDetail) ? string.Empty : GetWarehouseBinForJobDetail(demandsDv, (string)dataRow["mrlPartID"], (string)dataRow["mrlPartRevisionID"], warehouseForJobDetail));
					dataRow4["mrjLineID"] = dataRow["mrlLineID"];
					dataRow4["mrjJobDetailID"] = ++detailCounter;
					dataRow4["mrjJobID"] = (string)mrpLinesBs.Database.NextIDs.GetNextIDForTable("Jobs");
					dataRow4["mrjPartID"] = dataRow["mrlPartID"];
					dataRow4["mrjPartRevisionID"] = dataRow["mrlPartRevisionID"];
					dataRow4["mrjPartWarehouseLocationID"] = warehouseForJobDetail;
					dataRow4["mrjPartPlantID"] = plantForJobDetail;
					dataRow4["mrjPartBinID"] = value;
					if (mfgLotSize > 0m && num9 % mfgLotSize > 0m)
					{
						num9 = Math.Floor(num9 / mfgLotSize) * mfgLotSize + mfgLotSize;
					}
					dataRow4["mrjInventoryQuantity"] = num9;
					dataRow4["mrjConsolidated"] = array3.Length + array4.Length + array5.Length > 1;
					dataRow4["mrjGetPartMethod"] = true;
					dataRow4["mrjProductionDueDate"] = DateTime.Now;
					dataRow4["mrjFirm"] = true;
				}
			}
			if (flag)
			{
				M1Database database = mrpLinesBs.Database;
				bool partForecastFirmValue = database.Props("JM").Field<bool>("xapJMMRPForecastFirmJob");
				DataRow[] array6 = demandsDv.Table.Select(filterExpression);
				decimal num12 = array6.Sum((DataRow x) => x.Field<decimal>("mrrDemandQuantity")) - num10;
				if (num12 > 0m)
				{
					if (consolidatePartForecastJob)
					{
						DateTime dueDate = array6.Min((DataRow x) => x.Field<DateTime>("mrrDueDate"));
						DataRow dataRow5 = array6.FirstOrDefault();
						if (dataRow5 != null)
						{
							string nextJobId = Convert.ToString(mrpLinesBs.Database.NextIDs.GetNextIDForTable("Jobs"));
							CreateForecastJobDetailRecord(childBindingSource, dataRow5, partsNotCompleted, partRevisionsNotCompleted, warehousesNotCompleted, nextJobId, num12, dueDate, mfgLotSize, consolidatedJob: true, partForecastFirmValue, ref detailCounter);
						}
					}
					else
					{
						array2 = array6;
						foreach (DataRow dataRow6 in array2)
						{
							decimal num13 = dataRow6.Field<decimal>("mrrDemandQuantity");
							if (num10 > num13)
							{
								num10 -= num13;
								continue;
							}
							num13 -= num10;
							num10 = default(decimal);
							if (num13 > 0m)
							{
								string nextJobId2 = Convert.ToString(database.NextIDs.GetNextIDForTable("Jobs"));
								DateTime dueDate2 = dataRow6.Field<DateTime>("mrrDueDate");
								CreateForecastJobDetailRecord(childBindingSource, dataRow6, partsNotCompleted, partRevisionsNotCompleted, warehousesNotCompleted, nextJobId2, num13, dueDate2, mfgLotSize, consolidatedJob: false, partForecastFirmValue, ref detailCounter);
							}
						}
					}
				}
			}
			if (detailCounter == 0)
			{
				mrpLinesBs.Remove(dataRow);
			}
		}
	}

	private string GetJobIDFromSalesOrder(M1Database database, string orderID, string lineID, string deliveryID, ref List<string> mrpCreatedJobIDs)
	{
		bool num = database.Props("OM").Field<bool>("xapOMIncludeOrderLineInJob");
		int val = database.Props("OM").Field<byte>("xapOMOrderLineDigits");
		int val2 = database.Props("OM").Field<byte>("xapOMOrderDeliveryDigits");
		bool flag = database.Props("OM").Field<bool>("xapOMIncludeOrderDeliveryInJob");
		string text = orderID.Trim();
		if (num)
		{
			int length = Math.Max(lineID.Length, val);
			string text2 = PadAndTrim(lineID, length, 9);
			text = text + "-" + text2;
		}
		string text4;
		if (flag)
		{
			int length2 = Math.Max(deliveryID.ToString().Length, val2);
			string text3 = PadAndTrim(deliveryID.ToString(), length2, 9);
			text4 = text + "-" + text3;
			text = text4;
		}
		else
		{
			text4 = text;
		}
		DataTable jobsByJobId = GetJobsByJobId(database, text4);
		if (!JobExists(jobsByJobId, text4) && !JobCreated(mrpCreatedJobIDs, text4))
		{
			mrpCreatedJobIDs.Add(text4);
			return text4;
		}
		string text5 = FindNextAvailableJobID(database, mrpCreatedJobIDs, 9, text, text4);
		mrpCreatedJobIDs.Add(text5);
		return text5;
	}

	private string FindNextAvailableJobID(M1Database database, List<string> mrpCreatedJobIDs, int MaxDigitsLength, string baseJobID, string tempJobID)
	{
		string result = string.Empty;
		DataTable jobsByJobId = GetJobsByJobId(database, tempJobID);
		int num = jobsByJobId.Rows.Count + mrpCreatedJobIDs.Count;
		for (int i = 0; i < num; i++)
		{
			string text = PadAndTrim((i + 1).ToString(), 2, MaxDigitsLength);
			tempJobID = baseJobID + "-" + text;
			if (!JobExists(jobsByJobId, tempJobID) && !JobCreated(mrpCreatedJobIDs, tempJobID))
			{
				result = tempJobID;
				break;
			}
		}
		return result;
	}

	private bool JobExists(DataTable jobsDataTable, string jobID)
	{
		return jobsDataTable.Select("jmpJobID = " + jobID.ToLinq()).Length != 0;
	}

	private bool JobCreated(List<string> mrpCreatedJobIDs, string jobID)
	{
		return mrpCreatedJobIDs.Any((string s) => s.Contains(jobID));
	}

	private string PadAndTrim(string input, int length, int maxLength)
	{
		int num = maxLength - length;
		return input.PadLeft(num + length, '0').Substring(num);
	}

	private DataTable GetJobsByJobId(M1Database database, string jobId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select jmpJobID from Jobs where {fn left(jmpJobID, @BaseJobLength)} = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		sqlCommand.Parameters.Add(new SqlParameter("@BaseJobLength", SqlDbType.Int)).Value = jobId.Length;
		return database.GetDataTable(sqlCommand);
	}

	private static string GetPlantForJobDetail(DataRow[] partDemands, DataRow[] salesOrderDemands, DataRow[] jobDemands)
	{
		string text4;
		if (partDemands.Length + salesOrderDemands.Length + jobDemands.Length > 1)
		{
			IEnumerable<string> source = Enumerable.Empty<string>();
			IEnumerable<string> source2 = Enumerable.Empty<string>();
			IEnumerable<string> source3 = Enumerable.Empty<string>();
			if (partDemands.Length != 0)
			{
				source = partDemands.Select((DataRow d) => d.Field<string>("mrrPartPlantID")).Distinct();
			}
			if (salesOrderDemands.Length != 0)
			{
				source2 = salesOrderDemands.Select((DataRow d) => d.Field<string>("mrrPartPlantID")).Distinct();
			}
			if (jobDemands.Length != 0)
			{
				source3 = jobDemands.Select((DataRow d) => d.Field<string>("mrrPartPlantID")).Distinct();
			}
			List<string> list = source.ToList();
			List<string> list2 = source2.ToList();
			List<string> list3 = source3.ToList();
			if (list.Count > 1 || list2.Count > 1 || list3.Count > 1)
			{
				return string.Empty;
			}
			string text = list.FirstOrDefault();
			string text2 = list2.FirstOrDefault();
			string text3 = list3.FirstOrDefault();
			List<string> list4 = new List<string>();
			if (!string.IsNullOrWhiteSpace(text))
			{
				list4.Add(text);
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				list4.Add(text2);
			}
			if (!string.IsNullOrWhiteSpace(text3))
			{
				list4.Add(text3);
			}
			if (list4.Distinct().Count() != 1)
			{
				return string.Empty;
			}
			text4 = list4[0];
		}
		else
		{
			text4 = ((partDemands.Length != 0) ? partDemands.Select((DataRow d) => d.Field<string>("mrrPartPlantID")).FirstOrDefault() : ((salesOrderDemands.Length != 0) ? salesOrderDemands.Select((DataRow d) => d.Field<string>("mrrPartPlantID")).FirstOrDefault() : ((jobDemands.Length != 0) ? jobDemands.Select((DataRow d) => d.Field<string>("mrrPartPlantID")).FirstOrDefault() : null)));
		}
		return text4 ?? string.Empty;
	}

	private static string GetWarehouseForJobDetail(DataRow[] partDemands, DataRow[] salesOrderDemands, DataRow[] jobDemands)
	{
		string text4;
		if (partDemands.Length + salesOrderDemands.Length + jobDemands.Length > 1)
		{
			IEnumerable<string> source = Enumerable.Empty<string>();
			IEnumerable<string> source2 = Enumerable.Empty<string>();
			IEnumerable<string> source3 = Enumerable.Empty<string>();
			IEnumerable<string> source4 = Enumerable.Empty<string>();
			IEnumerable<string> source5 = Enumerable.Empty<string>();
			IEnumerable<string> source6 = Enumerable.Empty<string>();
			if (partDemands.Length != 0)
			{
				source = partDemands.Select((DataRow d) => d.Field<string>("mrrPartWarehouseLocationID")).Distinct();
				source2 = partDemands.Select((DataRow d) => d.Field<string>("mrrPartBinID")).Distinct();
			}
			if (salesOrderDemands.Length != 0)
			{
				source3 = salesOrderDemands.Select((DataRow d) => d.Field<string>("mrrPartWarehouseLocationID")).Distinct();
				source4 = salesOrderDemands.Select((DataRow d) => d.Field<string>("mrrPartBinID")).Distinct();
			}
			if (jobDemands.Length != 0)
			{
				source5 = jobDemands.Select((DataRow d) => d.Field<string>("mrrPartWarehouseLocationID")).Distinct();
				source6 = jobDemands.Select((DataRow d) => d.Field<string>("mrrPartBinID")).Distinct();
			}
			List<string> list = source.ToList();
			List<string> list2 = source3.ToList();
			List<string> list3 = source5.ToList();
			if (list.Count > 1 || list2.Count > 1 || list3.Count > 1 || source2.Count() > 1 || source4.Count() > 1 || source6.Count() > 1)
			{
				return string.Empty;
			}
			string text = list.FirstOrDefault();
			string text2 = list2.FirstOrDefault();
			string text3 = list3.FirstOrDefault();
			List<string> list4 = new List<string>();
			if (!string.IsNullOrWhiteSpace(text))
			{
				list4.Add(text);
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				list4.Add(text2);
			}
			if (!string.IsNullOrWhiteSpace(text3))
			{
				list4.Add(text3);
			}
			if (list4.Distinct().Count() > 1)
			{
				return string.Empty;
			}
			text4 = list4[0];
		}
		else
		{
			text4 = ((partDemands.Length != 0) ? partDemands.Select((DataRow d) => d.Field<string>("mrrPartWarehouseLocationID")).FirstOrDefault() : ((salesOrderDemands.Length != 0) ? salesOrderDemands.Select((DataRow d) => d.Field<string>("mrrPartWarehouseLocationID")).FirstOrDefault() : ((jobDemands.Length != 0) ? jobDemands.Select((DataRow d) => d.Field<string>("mrrPartWarehouseLocationID")).FirstOrDefault() : null)));
		}
		return text4 ?? string.Empty;
	}

	private static string GetWarehouseBinForJobDetail(DataView demandsDv, string partId, string partRevisionId, string warehouse)
	{
		return (from d in demandsDv.Table.Select("mrrPartID = '" + partId + "' and mrrPartRevisionID = '" + partRevisionId + "' and mrrPartWarehouseLocationID = '" + warehouse + "' and not mrrType = 'MakeToOrder'")
			select d.Field<string>("mrrPartBinID")).FirstOrDefault();
	}

	private void FillFilterExistValues(ref bool plantFiltersExist, ref bool warehouseFiltersExist, ref bool customerFiltersExist, ref bool partFiltersExist, ref bool partGroupFiltersExist, ref bool partClassFiltersExist, string plants, string warehouses, string customers, string parts, string partGroups, string partClasses)
	{
		if (!string.IsNullOrWhiteSpace(plants))
		{
			plantFiltersExist = true;
		}
		if (!string.IsNullOrWhiteSpace(warehouses))
		{
			warehouseFiltersExist = true;
		}
		if (!string.IsNullOrWhiteSpace(customers))
		{
			customerFiltersExist = true;
		}
		if (!string.IsNullOrWhiteSpace(parts))
		{
			partFiltersExist = true;
		}
		if (!string.IsNullOrWhiteSpace(partGroups))
		{
			partGroupFiltersExist = true;
		}
		if (!string.IsNullOrWhiteSpace(partClasses))
		{
			partClassFiltersExist = true;
		}
	}

	private string GetJobAssemblySupplyQuery(M1Database database, string sessionID, int lineID, bool partFiltersExist, bool partGroupFiltersExist, bool partClassFiltersExist, bool warehouseFiltersExist, bool plantFiltersExist, bool customerFiltersExist, string linePartID, string linePartRevision, string partGroups, string partClasses, string parts, string plants, string warehouses, string customers)
	{
		string text = "select imwWarehouseID from Warehouses where imwPlantID in (" + plants + ")";
		return "Select\r\n                " + sessionID.ToSql() + " As mrsSessionID, 0 As mrsLineID, 0 As mrsSupplyID, jmpCustomerOrganizationID as mrsCustomerOrganizationID,\r\n                jmaJobID As mrsJobID, jmaJobAssemblyID As mrsJobAssemblyID, jmaPartID As mrsPartID, jmaPartRevisionID As mrsPartRevisionID, \r\n                jmaPartWareHouseLocationID As mrsPartWarehouseLocationID, jmaPartBinID As mrsPartBinID,\r\n                IsNull(jmaScheduledDueDate, jmpProductionDueDate) As mrsDueDate, jmaQuantityReceivedToInventory As mrsQuantityReceived, \r\n                CASE WHEN jmaJobAssemblyID = 0 THEN IsNull(jmpQuantityShipped, 0) ELSE 0 END AS mrsQuantityShipped,\r\n                'JobAssemblies' As mrsSource, 'Production' As mrsType,\r\n                '' As mrrCustomerOrganizationID, '' As mrrShipOrganizationID, '' As mrrShipLocationID,\r\n                " + database.User.ID.ToSql() + " As mrsCreatedBy, GetDate() As mrsCreatedDate, NewID() As mrsUniqueID\r\n                From JobAssemblies left outer join Jobs on jmaJobID = jmpJobID\r\n                left outer join PartRevisions on imrPartID = jmaPartID and imrPartRevisionID = jmaPartRevisionID Left Join Parts on jmaPartID = impPartID\r\n                Where impPartType = 2 AND (jmaQuantityToMake - jmaQuantityReceivedToInventory - IsNull(jmpQuantityShipped,0)) > 0 and jmaReceivedComplete = 0\r\n                and IsNull(jmaScheduledDueDate, jmpProductionDueDate) < @CutOffDate\r\n                and jmpClosed = 0 and jmpProductionComplete = 0\r\n                " + (plantFiltersExist ? ("and jmaPartWarehouseLocationID In (" + text + ") ") : string.Empty) + "\r\n                " + ((!plantFiltersExist && warehouseFiltersExist) ? ("and jmaPartWarehouseLocationID In (" + warehouses + ") ") : string.Empty) + "\r\n                " + (customerFiltersExist ? ("and jmpCustomerOrganizationID In (" + customers + ") ") : string.Empty) + "\r\n                " + (partFiltersExist ? ("and jmaPartID In (" + parts + ") ") : string.Empty) + "\r\n                " + (partGroupFiltersExist ? ("and impPartGroupID In (" + partGroups + ") ") : string.Empty) + "\r\n                " + (partClassFiltersExist ? ("and impPartClassID In (" + partClasses + ") ") : string.Empty) + "\r\n                " + ((lineID != 0) ? ("and jmaPartID = " + linePartID.ToSql() + " and jmaPartRevisionID = " + linePartRevision.ToSql()) : string.Empty);
	}

	private string GetJobAssemblyDemandQuery(M1Database database, string sessionID, int lineID, bool plantFiltersExist, bool warehouseFiltersExist, bool customerFiltersExist, bool partFiltersExist, bool partGroupFiltersExist, bool partClassFiltersExist, string linePartID, string linePartRevision, string plants, string warehouses, string customers, string partGroups, string partClasses, string parts)
	{
		string text = "select imwWarehouseID from Warehouses where imwPlantID in (" + plants + ")";
		return "Select \r\n                " + sessionID.ToSql() + " As mrrSessionID, 0 As mrrLineID, 0 As mrrDemandID, \r\n                '' As mrrSalesOrderID, 0 As mrrSalesOrderLineID, 0 As mrrSalesOrderDeliveryID, jmaJobID As mrrJobID, jmaJobAssemblyID As mrrJobAssemblyID, 0 As mrrJobMaterialID,\r\n                jmaPartID As mrrPartID, jmaPartRevisionID As mrrPartRevisionID, COALESCE(xauPlantID, '') as mrrPartPlantID, jmaPartWareHouseLocationID As mrrPartWarehouseLocationID, jmaPartBinID As mrrPartBinID,\r\n                IsNull(jmaScheduledDueDate, jmpProductionDueDate) As mrrDueDate,\r\n                jmaQuantityToPull As mrrOriginalQuantity, jmaQuantityIssued As mrrQuantityReceived, 0 As mrrQuantityShipped, (jmaQuantityToPull - jmaQuantityIssued) As mrrDemandQuantity,\r\n                'JobAssemblies' As mrrSource, 'PullFromStock' As mrrType,\r\n                jmpCustomerOrganizationID As mrrCustomerOrganizationID, '' As mrrShipOrganizationID, '' As mrrShipLocationID,\r\n                " + database.User.ID.ToSql() + " As mrrCreatedBy, GetDate() As mrrCreatedDate, NewID() As mrrUniqueID \r\n                From JobAssemblies left outer join Jobs on jmaJobID = jmpJobID\r\n                left outer join PartRevisions on imrPartID = jmaPartID and imrPartRevisionID = jmaPartRevisionID Left Join Parts on jmaPartID = impPartID\r\n                left outer join Warehouses on imwWarehouseID=jmaPartWareHouseLocationID\r\n                left outer join Plants on xauPlantID=imwPlantID\r\n                Where impPartType = 2 AND (jmaQuantityToPull - jmaQuantityIssued) > 0 and jmaIssuedComplete = 0\r\n                and IsNull(jmaScheduledDueDate, jmpProductionDueDate) < @CutOffDate\r\n                and jmpClosed = 0 and jmpProductionComplete = 0\r\n                " + (plantFiltersExist ? (" and jmaPartWarehouseLocationID In (" + text + ")") : string.Empty) + "\r\n                " + ((warehouseFiltersExist && !plantFiltersExist) ? (" and jmaPartWareHouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                " + (customerFiltersExist ? (" and jmpCustomerOrganizationID In(" + customers + ")") : string.Empty) + "\r\n                " + (partFiltersExist ? (" and jmaPartID In (" + parts + ")") : string.Empty) + "\r\n                " + (partGroupFiltersExist ? (" and impPartGroupID In (" + partGroups + ")") : string.Empty) + "\r\n                " + (partClassFiltersExist ? (" and impPartClassID In (" + partClasses + ")") : string.Empty) + "\r\n                " + ((lineID != 0) ? (" and jmaPartID = " + linePartID.ToSql() + " and jmaPartRevisionID = " + linePartRevision.ToSql()) : string.Empty);
	}

	private string GetJobMaterialDemandQuery(M1Database database, string sessionID, int lineID, bool plantFiltersExist, bool warehouseFiltersExist, bool customerFiltersExist, bool partFiltersExist, bool partGroupFiltersExist, bool partClassFiltersExist, string linePartID, string linePartRevision, string plants, string warehouses, string customers, string partGroups, string partClasses, string parts)
	{
		string text = "select imwWarehouseID from Warehouses where imwPlantID in (" + plants + ")";
		return "Select \r\n                " + sessionID.ToSql() + " As mrrSessionID, 0 As mrrLineID, 0 As mrrDemandID, \r\n                '' As mrrSalesOrderID, 0 As mrrSalesOrderLineID, 0 As mrrSalesOrderDeliveryID, jmmJobID As mrrJobID, jmmJobAssemblyID As mrrJobAssemblyID, jmmJobMaterialID As mrrJobMaterialID,\r\n                jmmPartID As mrrPartID, jmmPartRevisionID As mrrPartRevisionID, COALESCE(xauPlantID, '') as mrrPartPlantID, jmmPartWareHouseLocationID As mrrPartWarehouseLocationID, jmmPartBinID As mrrPartBinID,\r\n                IsNull(jmmRequiredDate, jmpProductionDueDate) As mrrDueDate,\r\n                jmmPullFromStockQuantity As mrrOriginalQuantity, jmmQuantityReceived As mrrQuantityReceived, 0 As mrrQuantityShipped, (jmmPullFromStockQuantity - jmmQuantityReceived) As mrrDemandQuantity, \r\n                'JobMaterials' As mrrSource, 'PullFromStock' As mrrType, \r\n                jmpCustomerOrganizationID As mrrCustomerOrganizationID, '' As mrrShipOrganizationID, '' As mrrShipLocationID, \r\n                " + database.User.ID.ToSql() + " As mrrCreatedBy, GetDate() As mrrCreatedDate, NewID() As mrrUniqueID \r\n                From JobMaterials left outer join Jobs on jmmJobID = jmpJobID \r\n                left outer join PartRevisions on imrPartID = jmmPartID and imrPartRevisionID = jmmPartRevisionID Left Join Parts on jmmPartID = impPartID \r\n                left outer join Warehouses on imwWarehouseID=jmmPartWareHouseLocationID\r\n                left outer join Plants on xauPlantID=imwPlantID\r\n                Where impPartType = 2 AND (jmmPullFromStockQuantity - jmmQuantityReceived) > 0 and jmmReceivedComplete = 0 \r\n                and IsNull(jmmRequiredDate, jmpProductionDueDate) < @CutOffDate \r\n                and jmpClosed = 0 and jmpProductionComplete = 0 \r\n                " + (plantFiltersExist ? (" and jmmPartWareHouseLocationID In (" + text + ")") : string.Empty) + "\r\n                " + ((warehouseFiltersExist && !plantFiltersExist) ? (" and jmmPartWarehouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                " + (customerFiltersExist ? (" and jmpCustomerOrganizationID In(" + customers + ")") : string.Empty) + "\r\n                " + (partFiltersExist ? (" and jmmPartID In (" + parts + ")") : string.Empty) + "\r\n                " + (partGroupFiltersExist ? (" and impPartGroupID In (" + partGroups + ")") : string.Empty) + "\r\n                " + (partClassFiltersExist ? (" and impPartClassID In (" + partClasses + ")") : string.Empty) + "\r\n                " + ((lineID != 0) ? (" and jmmPartID = " + linePartID.ToSql() + " and jmmPartRevisionID = " + linePartRevision.ToSql()) : string.Empty);
	}

	private string GetMRPLinesNotCompletedQuery(bool plantFiltersExist, bool warehouseFiltersExist, string plants, string warehouses)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Select mrrPartWarehouseLocationID,mrjPartWarehouseLocationID,mrpSessionID, mrrPartRevisionID,mrrPartID,mrjPartID,mrjPartRevisionID\r\n                        from MRPSessions \r\n                        left outer join MRPDemands on mrrSessionID=mrpSessionID\r\n                        left outer join MRPLines on mrlSessionID=mrpSessionID\r\n                        left outer join MRPJobDetails on mrjSessionID = mrpSessionID\r\n                        where\r\n                        MRPSessions.mrpCompleted = 0 \r\n                        and mrrPartID is not null\r\n                        and mrlCompleted=0\r\n                        and MRPSessions.mrpSessionID <> @sessionID");
		stringBuilder.AppendLine(plantFiltersExist ? (" and mrpPlantIDs in (" + (string.IsNullOrEmpty(plants) ? string.Empty.ToSql() : plants) + ")") : string.Empty);
		stringBuilder.AppendLine(warehouseFiltersExist ? (" and mrrPartWarehouseLocationID in (" + (string.IsNullOrEmpty(warehouses) ? string.Empty.ToSql() : warehouses) + ")") : string.Empty);
		stringBuilder.AppendLine(" group by mrrPartWarehouseLocationID,mrjPartWarehouseLocationID,mrpSessionID, mrrPartRevisionID,mrrPartID,mrjPartID,mrjPartRevisionID");
		return stringBuilder.ToString();
	}

	private DataTable GetMRPLinesNotCompleted(M1Database database, string sessionID, bool plantFiltersExist, bool warehouseFiltersExist, string plants, string warehouses)
	{
		string mRPLinesNotCompletedQuery = GetMRPLinesNotCompletedQuery(plantFiltersExist, warehouseFiltersExist, plants, warehouses);
		SqlCommand sqlCommand = database.NewSqlCommand(mRPLinesNotCompletedQuery);
		sqlCommand.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.NVarChar)).Value = sessionID;
		return database.GetDataTable(sqlCommand);
	}

	private string GetOverlapMessage(string sessions)
	{
		if (!string.IsNullOrEmpty(sessions))
		{
			return "Based on filter criteria selected in this MRP Session, there are Warehouse / Part Demand combinations which overlap with other open MRP Session(s).\n\nOverlap exists in the following MRP Sessions:\n\nSession: " + sessions + "\n\nOverlapping Parts will not be included in this MRP Session. Planner Lines will be created for Parts which do not overlap. To avoid this, complete open session(s) before starting new sessions.";
		}
		return string.Empty;
	}

	private string GetOverlapDetailMessage(List<string> sessions, List<string> parts, List<string> partRevisions, List<string> warehouses)
	{
		List<Tuple<string, string, string, string>> list = new List<Tuple<string, string, string, string>>();
		string text = "";
		for (int i = 0; i < sessions.Count; i++)
		{
			if (!list.Contains(new Tuple<string, string, string, string>(sessions[i], parts[i], partRevisions[i], warehouses[i])) && !string.IsNullOrEmpty(warehouses[i]))
			{
				list.Add(new Tuple<string, string, string, string>(sessions[i], parts[i], partRevisions[i], warehouses[i]));
				text = text + "Overlap Session " + sessions[i] + " Part " + parts[i] + " Rev " + partRevisions[i] + " Warehouse " + warehouses[i] + ",";
			}
		}
		return text;
	}

	private string GetPartForecastsDemandQuery(M1Database database, string sessionID, int lineID, bool partFiltersExist, bool partGroupFiltersExist, bool partClassFiltersExist, bool customerFiltersExist, string linePartID, string linePartRevision, string partGroups, string partClasses, string parts, bool includePartForecasts)
	{
		string value = "SELECT " + sessionID.ToSql() + " As mrrSessionID, 0 As mrrLineID, 0 As mrrDemandID, \r\n                        '' As mrrSalesOrderID, 0 As mrrSalesOrderLineID, 0 As mrrSalesOrderDeliveryID, \r\n                        '' As mrrJobID, 0 As mrrJobAssemblyID, 0 As mrrJobMaterialID, inlPartID As mrrPartID, \r\n                        inlPartRevisionID As mrrPartRevisionID, '' As mrrPartPlantID,\r\n                        '' As mrrPartWarehouseLocationID, '' As mrrPartBinID, \r\n                        inlStartDate As mrrDueDate, 0 As mrrOriginalQuantity, 0 As mrrQuantityReceived, \r\n                        0 As mrrQuantityShipped, inlForecastQuantity - inlActualQuantity As mrrDemandQuantity, \r\n                        'PartRevisions' As mrrSource, 'PartForecast' As mrrType, '' As mrrCustomerOrganizationID, \r\n                        '' As mrrShipOrganizationID, '' As mrrShipLocationID, " + database.User.ID.ToSql() + " As mrrCreatedBy, \r\n                        GetDate() As mrrCreatedDate, NewID() As mrrUniqueID\r\n                    FROM PartForecasts\r\n                        LEFT OUTER JOIN PartForecastLines ON inpPartID = inlPartID AND inpPartRevisionID = inlPartRevisionID\r\n                        LEFT OUTER JOIN Parts ON impPartID = inlPartID\r\n                        LEFT OUTER JOIN PartRevisions ON imrPartID = inlPartID AND imrPartRevisionID = inlPartRevisionID ";
		string value2 = ((!customerFiltersExist && includePartForecasts) ? ("Where impPartType = 2 AND (imrEffectiveEndDate >= (Convert(datetime, GetDate())) OR imrEffectiveEndDate is null) \r\n                        AND (imrInactive = 0) AND inlStartDate < @CutOffDate AND inlForecastQuantity - inlActualQuantity > 0 AND inlIncludeInMRP = 1\r\n                        " + string.Format(partFiltersExist ? (" and impPartID In (" + parts + ")") : string.Empty) + "\r\n                        " + string.Format(partGroupFiltersExist ? (" and impPartGroupID In(" + partGroups + ")") : string.Empty) + "\r\n                        " + string.Format(partClassFiltersExist ? (" and impPartClassID In (" + partClasses + ")") : string.Empty) + "\r\n                        " + string.Format((lineID != 0) ? (" and imrPartID = " + linePartID.ToSql() + " and imrPartRevisionID = " + linePartRevision.ToSql()) : string.Empty) + "\r\n                        GROUP BY inlPartID, inlPartRevisionID, inlStartDate, inlForecastQuantity, inlActualQuantity") : "Where 1 = 0");
		return new StringBuilder(value).AppendLine(value2).ToString();
	}

	private static decimal GetPartDemandQuantityOnHand(M1Database database, string partId, string partRevisionId, string warehouseId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT SUM(imbQuantityOnHand) as QuantityOnHand FROM PartBins WHERE imbPartID = @PartID AND imbPartRevisionID = @PartRevisionID AND imbWarehouseID = @WarehouseID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionId;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseId;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<decimal>("QuantityOnHand");
		}
		return 0m;
	}

	private static decimal GetPartDemandQuantityToInspect(M1Database database, string partId, string partRevisionId, string warehouseId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT COALESCE(SUM(qalQuantityToInspect), 0) as QuantityToInspect FROM InspectionLines WHERE ((qalStatus IN ('P', 'O') AND qalManualInspectionFinalized = 1 AND qalInspectionType = 1) OR (qalStatus IN ('P', 'O') AND qalSourceTableName != '')) AND qalPartID = @PartID AND qalPartRevisionID = @PartRevisionID AND qalPartWarehouseLocationID = @WarehouseID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionId;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseId;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<decimal>("QuantityToInspect");
		}
		return 0m;
	}

	private string GetPartsDemandQuery(M1Database database, string sessionID, int lineID, bool plantFiltersExist, bool warehouseFiltersExist, bool partFiltersExist, bool partGroupFiltersExist, bool partClassFiltersExist, bool customerFiltersExist, string linePartID, string linePartRevision, string plants, string warehouses, string partGroups, string partClasses, string parts)
	{
		string value = "Select " + sessionID.ToSql() + " As mrrSessionID, 0 As mrrLineID, 0 As mrrDemandID, \r\n                        '' As mrrSalesOrderID, 0 As mrrSalesOrderLineID, 0 As mrrSalesOrderDeliveryID, \r\n                        '' As mrrJobID, 0 As mrrJobAssemblyID, 0 As mrrJobMaterialID, imrPartID As mrrPartID, \r\n                        imrPartRevisionID As mrrPartRevisionID, COALESCE(xauPlantID, '') as mrrPartPlantID,\r\n                        imwWarehouseID As mrrPartWarehouseLocationID, '' As mrrPartBinID, \r\n                        GetDate() As mrrDueDate, 0 As mrrOriginalQuantity, 0 As mrrQuantityReceived, \r\n                        0 As mrrQuantityShipped, (imlMaximumQuantity - (SELECT SUM(imbQuantityOnHand) FROM PartBins WHERE imbPartID = imrPartID AND imrPartRevisionID = imbPartRevisionID AND imbWarehouseID = imwWarehouseID) - (SELECT COALESCE(SUM(qalQuantityToInspect), 0) FROM InspectionLines WHERE ((qalStatus IN ('P', 'O') AND qalManualInspectionFinalized = 1 AND qalInspectionType = 1) OR (qalStatus IN ('P', 'O') AND qalSourceTableName != '')) AND qalPartID = imrPartID AND qalPartRevisionID = imrPartRevisionID AND qalPartWarehouseLocationID = imwWarehouseID)) As mrrDemandQuantity, \r\n                        'PartRevisions' As mrrSource, 'MinMax' As mrrType, '' As mrrCustomerOrganizationID, \r\n                        '' As mrrShipOrganizationID, '' As mrrShipLocationID, " + database.User.ID.ToSql() + " As mrrCreatedBy, \r\n                        GetDate() As mrrCreatedDate, NewID() As mrrUniqueID\r\n                    From Parts \r\n                        left outer join PartRevisions on imrPartID = impPartID\r\n                        left outer join PartBins on imrPartID = imbPartID and imrPartRevisionID = imbPartRevisionID \r\n                        left outer join Warehouses w on imwWarehouseID = imbWarehouseID \r\n                        left outer join Plants on xauPlantID=imwPlantID\r\n                        left outer join PartWarehouseLocations on imrPartID = imlPartID and imrPartRevisionID = imlPartRevisionID and imbWarehouseID = imlPartWarehouseID ";
		string value2 = ((!customerFiltersExist) ? ("Where impPartType = 2 and (imrEffectiveEndDate >= (Convert(datetime, GetDate())) Or imrEffectiveEndDate is null) \r\n                        And (imrInactive = 0) AND ((SELECT SUM(imbQuantityOnHand) FROM PartBins WHERE imbPartID = imrPartID AND imrPartRevisionID = imbPartRevisionID AND imbWarehouseID = imwWarehouseID) + (SELECT COALESCE(SUM(qalQuantityToInspect), 0) FROM InspectionLines WHERE ((qalStatus IN ('P', 'O') AND qalManualInspectionFinalized = 1 AND qalInspectionType = 1) OR (qalStatus IN ('P', 'O') AND qalSourceTableName != '')) AND qalPartID = imrPartID AND qalPartRevisionID = imrPartRevisionID AND qalPartWarehouseLocationID = imwWarehouseID)) < imlMinimumQuantity \r\n                        And (Select COUNT(imbPartId) From PartBins Where imbInactiveBin = 0 And imbPartID = imrPartID AND imrPartRevisionID = imbPartRevisionID AND imbWarehouseID = imwWarehouseID) > 0 \r\n                        " + string.Format(plantFiltersExist ? (" and imwPlantID In (" + plants + ")") : string.Empty) + "\r\n                        " + string.Format((!plantFiltersExist && warehouseFiltersExist) ? (" and imwWarehouseID In (" + warehouses + ")") : string.Empty) + "\r\n                        " + string.Format(partFiltersExist ? (" and impPartID In (" + parts + ")") : string.Empty) + "\r\n                        " + string.Format(partGroupFiltersExist ? (" and impPartGroupID In(" + partGroups + ")") : string.Empty) + "\r\n                        " + string.Format(partClassFiltersExist ? (" and impPartClassID In (" + partClasses + ")") : string.Empty) + "\r\n                        " + string.Format((lineID != 0) ? (" and imrPartID = " + linePartID.ToSql() + " and imrPartRevisionID = " + linePartRevision.ToSql()) : string.Empty) + "\r\n                        GROUP BY imrPartID, imrPartRevisionID, xauPlantID, imwPlantID, imwWarehouseID, imlMaximumQuantity") : "Where 1 = 0");
		return new StringBuilder(value).AppendLine(value2).ToString();
	}

	private string getSODeliveriesDemandQuery(M1Database database, string sessionID, int lineID, bool customerFiltersExist, bool partFiltersExist, bool partGroupFiltersExist, bool partClassFiltersExist, bool warehouseFiltersExist, bool plantFiltersExist, string linePartID, string linePartRevision, string partGroups, string partClasses, string customers, string parts, string plants, string warehouses)
	{
		string text = "select imwWarehouseID from Warehouses where imwPlantID in (" + plants + ")";
		return "Select " + sessionID.ToSql() + " As mrrSessionID, 0 As mrrLineID, 0 As mrrDemandID,\r\n                omdSalesOrderID As mrrSalesOrderID, omdSalesOrderLineID As mrrSalesOrderLineID, omdSalesOrderDeliveryID As mrrSalesOrderDeliveryID,\r\n                '' As mrrJobID, 0 As mrrJobAssemblyID, 0 As mrrJobMaterialID,  omdPartID As mrrPartID, omdPartRevisionID As mrrPartRevisionID,\r\n                COALESCE(xauPlantID, '') as mrrPartPlantID, omdPartWarehouseLocationID As mrrPartWarehouseLocationID, omdPartBinID As mrrPartBinID,\r\n                omdDeliveryDate As mrrDueDate,  omdDeliveryQuantity As mrrOriginalQuantity, 0 As mrrQuantityReceived, omdQuantityShipped As mrrQuantityShipped,\r\n                (omdDeliveryQuantity - omdQuantityShipped) As mrrDemandQuantity, 'SalesOrderDeliveries' As mrrSource,\r\n                Case When omdDeliveryType = 1 Then 'MakeToOrder' When omdDeliveryType = 2 Then 'PullFromStock' Else '' End As mrrType,\r\n                ompCustomerOrganizationID As mrrCustomerOrganizationID, \r\n                CASE WHEN omdDifferentLocation = 1 THEN omdCustomerOrganizationID ELSE ompShipOrganizationID END AS mrrShipOrganizationID,\r\n                CASE WHEN omdDifferentLocation = 1 THEN omdShipLocationID ELSE ompShipLocationID END AS mrrShipLocationID,\r\n                " + database.User.ID.ToSql() + " As mrrCreatedBy, GetDate() As mrrCreatedDate,\r\n                NewID() As mrrUniqueID, (omdDeliveryQuantity - omdQuantityShipped) as mrlQuantityAllocated\r\n                From SalesOrders\r\n                left outer join SalesOrderLines on omlSalesOrderID = ompSalesOrderID\r\n                left outer join SalesOrderDeliveries on omdSalesOrderID = omlSalesOrderID and omdSalesOrderLineID = omlSalesOrderLineID\r\n                left outer join Parts on impPartID = omdPartID\r\n                left outer join PartRevisions on imrPartID = omdPartID and imrPartRevisionID = omdPartRevisionID \r\n                left outer join SalesOrderJobLinks on omjSalesOrderID = omdSalesOrderID and omjSalesOrderLineID = omdSalesOrderLineID \r\n                left outer join Warehouses on imwWarehouseID=omdPartWarehouseLocationID  \r\n                left outer join Plants on xauPlantID = imwPlantID\r\n                Where ompClosed = 0  and omdShippedComplete = 0  and impPartType = 2 and (omdDeliveryQuantity - omdQuantityShipped) > 0\r\n                and omdDeliveryType In (1, 2) and omjJobID is null\r\n                and omdDeliveryDate < @CutOffDate\r\n                " + (plantFiltersExist ? (" and omdPartWarehouseLocationID In (" + text + ")") : string.Empty) + "\r\n                " + ((!plantFiltersExist && warehouseFiltersExist) ? (" and omdPartWarehouseLocationID In (" + warehouses + ")") : string.Empty) + "\r\n                " + (partFiltersExist ? (" and omdPartID In (" + parts + ")") : string.Empty) + "\r\n                " + (customerFiltersExist ? (" and ompCustomerOrganizationID In (" + customers + ")") : string.Empty) + "\r\n                " + (partGroupFiltersExist ? (" and impPartGroupID In (" + partGroups + ")") : string.Empty) + "\r\n                " + (partClassFiltersExist ? (" and impPartClassID In (" + partClasses + ")") : string.Empty) + "\r\n                " + ((lineID != 0) ? (" and omdPartID = " + linePartID.ToSql() + " and omdPartRevisionID = " + linePartRevision.ToSql()) : string.Empty);
	}

	private IEnumerable<string> GetWarehousesOfParts(DemandsInfo partDemandWithoutWarehouse, IEnumerable<DemandsInfo> partDemandWithWarehouse)
	{
		foreach (DemandsInfo item in partDemandWithWarehouse)
		{
			if (partDemandWithoutWarehouse.LineID == item.LineID && partDemandWithoutWarehouse.Part == item.Part && partDemandWithoutWarehouse.Revision == item.Revision && partDemandWithoutWarehouse.SessionID == item.SessionID)
			{
				yield return item.Warehouse;
			}
		}
	}

	private string GetOverlapQuery(string table, string prefix, string plantColumn, bool plantFiltersExist, bool warehouseFiltersExist, bool customerFiltersExist, bool partFiltersExist, bool partClassFiltersExist, bool partGroupFiltersExist, string plants, string warehouses, string customers, string parts, string partClasses, string partGroups, string extraJoin = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Select mrpSessionID as mrxSessionID,COALESCE(" + plantColumn + ",'') as mrxPartPlantLocationID," + prefix + "PartWarehouseLocationID as mrxPartWarehouseLocationID," + prefix + "PartRevisionID as mrxPartRevisionID," + prefix + "PartID as mrxPartID");
		stringBuilder.AppendLine("From MRPSessions");
		stringBuilder.AppendLine("left outer join " + table + " on " + prefix + "SessionID = mrpSessionID");
		stringBuilder.AppendLine("left outer join MRPLines on mrlSessionID = mrpSessionID");
		stringBuilder.AppendLine("left outer join Parts on impPartID = " + prefix + "PartID");
		stringBuilder.AppendLine((!string.IsNullOrEmpty(extraJoin)) ? extraJoin : string.Empty);
		stringBuilder.AppendLine("Where");
		stringBuilder.AppendLine("MRPSessions.mrpCompleted = 0");
		stringBuilder.AppendLine("and mrlCompleted = 0");
		stringBuilder.AppendLine("and " + prefix + "PartID is not null");
		stringBuilder.AppendLine("and MRPSessions.mrpSessionID <> @SessionID");
		stringBuilder.AppendLine(plantFiltersExist ? (" and " + plantColumn + " In (" + plants + ")") : string.Empty);
		stringBuilder.AppendLine((!plantFiltersExist && warehouseFiltersExist) ? (" and " + prefix + "PartWarehouseLocationID In (" + warehouses + ")") : string.Empty);
		stringBuilder.AppendLine(partFiltersExist ? (" and " + prefix + "PartID In (" + parts + ")") : string.Empty);
		stringBuilder.AppendLine(customerFiltersExist ? (" and " + prefix + "CustomerOrganizationID In (" + customers + ")") : string.Empty);
		stringBuilder.AppendLine(partGroupFiltersExist ? (" and impPartGroupID In (" + partGroups + ")") : string.Empty);
		stringBuilder.AppendLine(partClassFiltersExist ? (" and impPartClassID In (" + partClasses + ")") : string.Empty);
		return stringBuilder.ToString();
	}

	private string PerformCheckOverlap(M1Database database, string sessionID, bool plantFiltersExist, bool warehouseFiltersExist, bool customerFiltersExist, bool partFiltersExist, bool partClassFiltersExist, bool partGroupFiltersExist, string plants, string warehouses, string customers, string parts, string partClasses, string partGroups, bool withDetail)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		string result = string.Empty;
		stringBuilder.AppendLine(GetOverlapQuery("MRPDemands", "mrr", "mrrPartPlantID", plantFiltersExist, warehouseFiltersExist, customerFiltersExist, partFiltersExist, partClassFiltersExist, partGroupFiltersExist, plants, warehouses, customers, parts, partClasses, partGroups));
		stringBuilder.AppendLine("union");
		stringBuilder.AppendLine(GetOverlapQuery("MRPJobDetails", "mrj", "mrjPartPlantID", plantFiltersExist, warehouseFiltersExist, customerFiltersExist, partFiltersExist, partClassFiltersExist, partGroupFiltersExist, plants, warehouses, customers, parts, partClasses, partGroups));
		stringBuilder.AppendLine("union");
		stringBuilder.AppendLine(GetOverlapQuery("MRPSupply", "mrs", "imwPlantID", plantFiltersExist, warehouseFiltersExist, customerFiltersExist, partFiltersExist, partClassFiltersExist, partGroupFiltersExist, plants, warehouses, customers, parts, partClasses, partGroups, "left outer join Warehouses on imwWarehouseID=mrsPartWarehouseLocationID"));
		stringBuilder.AppendLine("order by mrxSessionID,mrxPartID,mrxPartPlantLocationID,mrxPartRevisionID,mrxPartWarehouseLocationID");
		string queryString = stringBuilder.ToString();
		stringBuilder.Clear();
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				list.Add(row.Field<string>("mrxSessionID"));
				list3.Add(row.Field<string>("mrxPartID"));
				list4.Add(row.Field<string>("mrxPartRevisionID"));
				list2.Add(row.Field<string>("mrxPartWarehouseLocationID"));
			}
		}
		if (list.Count != 0)
		{
			string sessions = string.Join(",", list.Distinct());
			result = (withDetail ? GetOverlapDetailMessage(list, list3, list4, list2) : GetOverlapMessage(sessions));
		}
		return result;
	}

	private void RemoveExistingRecords(M1Database database, string sessionID, int lineID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (lineID == 0)
		{
			stringBuilder.AppendLine("Delete From MRPLines Where mrlSessionID = @SessionID");
			stringBuilder.AppendLine("Delete From MRPDemands Where mrrSessionID = @SessionID");
			stringBuilder.AppendLine("Delete From MRPSupply Where mrsSessionID = @SessionID");
			stringBuilder.AppendLine("Delete From MRPJobDetails Where mrjSessionID = @SessionID");
		}
		else
		{
			stringBuilder.AppendLine("Delete From MRPLines Where mrlSessionID = @SessionID and mrlLineID = @LineID");
			stringBuilder.AppendLine("Delete From MRPDemands Where mrrSessionID = @SessionID and mrrLineID = @LineID");
			stringBuilder.AppendLine("Delete From MRPSupply Where mrsSessionID = @SessionID and mrsLineID = @LineID");
			stringBuilder.AppendLine("Delete From MRPJobDetails Where mrjSessionID = @SessionID and mrjLineID = @LineID");
		}
		string queryString = stringBuilder.ToString();
		stringBuilder.Clear();
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionID;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineID;
		database.ExecuteCommand(sqlCommand);
	}

	private void setDetailIDsForDemandAndSupplyDataView(DataView demandsDv, DataView supplyDv, int lineID)
	{
		int tableLineID = lineID;
		int num = 0;
		string currPart = string.Empty;
		string currRevision = string.Empty;
		foreach (DataRowView item in demandsDv)
		{
			if (!item.Row.Field<string>("mrrPartID").Equals(currPart) || !item.Row.Field<string>("mrrPartRevisionID").Equals(currRevision))
			{
				if (lineID == 0)
				{
					int num2 = tableLineID;
					tableLineID = num2 + 1;
				}
				num = 1;
				currPart = item.Row.Field<string>("mrrPartID");
				currRevision = item.Row.Field<string>("mrrPartRevisionID");
				int counter = 1;
				(from r in supplyDv.Table.AsEnumerable()
					where r.Field<string>("mrsPartID").Equals(currPart) && r.Field<string>("mrsPartRevisionID").Equals(currRevision)
					select r).ToList().ForEach(delegate(DataRow r)
				{
					r.SetField("mrsLineID", tableLineID);
					r.SetField("mrsSupplyID", counter++);
				});
			}
			else
			{
				num++;
			}
			item.Row.SetField("mrrLineID", tableLineID);
			item.Row.SetField("mrrDemandID", num);
		}
		foreach (DataRow item2 in (from r in supplyDv.Table.AsEnumerable()
			where r.Field<int>("mrsLineID").Equals(0) && r.Field<int>("mrsSupplyID").Equals(0)
			select r).ToList())
		{
			item2.Delete();
		}
	}

	private string SplitAndConvert(string ids)
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

	private object RemoveSqlFormat(string input)
	{
		return input.Replace("''", "'<None>'").Replace("N'", string.Empty).Replace("'", string.Empty)
			.Replace(",", ", ");
	}
}
