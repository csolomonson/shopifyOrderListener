using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Forms.Controls;

namespace M1.Ax.Erp;

[AxScript("MRP")]
[ComVisible(true)]
public class AppAxMRP : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	private List<string> getDataSessionClicked = new List<string>();

	private M1UserSettings _UserSettings;

	private string _UserID;

	private M1DataDictionary _DataDictionary;

	public AppAxMRP(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
		_UserSettings = _Database.User.Settings;
		_UserID = _Database.User.ID;
		_DataDictionary = _Database.User.DataDictionary;
	}

	public bool GetDataClicked(string sessionID)
	{
		return getDataSessionClicked.Contains(sessionID);
	}

	public string CheckOverlap(object sessionID, object lineID, bool messageDetail)
	{
		string text = Convert.ToString(sessionID);
		if (!getDataSessionClicked.Contains(text))
		{
			getDataSessionClicked.Add(text);
		}
		return new MRP().CheckOverlap(_Database, text, Convert.ToInt16(lineID), messageDetail);
	}

	public string CreateMrpLines(object sessionID, object lineID)
	{
		return new MRP().CreateMrpLines(_Database, Convert.ToString(sessionID), Convert.ToInt16(lineID));
	}

	public string Generate(object sessionID, object lineID)
	{
		string text = new MRP().CheckOverlap(_Database, Convert.ToString(sessionID), Convert.ToInt16(lineID), messageDetail: false);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return new MRP().CreateMrpLines(_Database, Convert.ToString(sessionID), Convert.ToInt16(lineID));
	}

	public void Clear(object sessionID)
	{
		string text = Convert.ToString(sessionID);
		getDataSessionClicked.Remove(text);
		new MRP().Clear(_Database, text);
	}

	public void SaveGrid(M1BindingSource bindingSource)
	{
		bindingSource.SaveData();
	}

	public string CheckMissingPlants(object sessionID)
	{
		return new MRP().CheckMissingPlants(_Database, Convert.ToString(sessionID));
	}

	public bool HasJobDetailRows(object sessionID)
	{
		return new MRP().HasJobDetailRows(_Database, Convert.ToString(sessionID));
	}

	public string GetLinesWithMissingWarehouseOrBin(object sessionID)
	{
		return new MRP().GetLinesWithMissingWarehouseOrBin(_Database, Convert.ToString(sessionID));
	}

	public string MissingDetailCheck(object sessionID)
	{
		return new MRP().MissingDetailCheck(_Database, Convert.ToString(sessionID));
	}

	public bool PostMRPSession(M1BindingSource bindingSource)
	{
		return new MRP().PostMRPSession(bindingSource);
	}

	public void PostMRPSessionScript(string mrpSessionID)
	{
		if (!string.IsNullOrWhiteSpace(mrpSessionID))
		{
			M1BindingSource m1BindingSource = new M1BindingSource(_Database);
			m1BindingSource.LoadDefinition(string.Empty, "MRPSessions", null, true);
			m1BindingSource.ClearCache();
			m1BindingSource.NavigateTo(_Database, "mrpSessionID = " + M1Util.ConvertToSql(mrpSessionID));
			new MRP().PostMRPSession(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	public void ShowMRPLineQtys(string partId, string partRevisionId, string plantsFilter, string warehousesFilter, string customersFilter, string sessionId, DateTime cutoffDateFilter)
	{
		string text = SplitAndConvert(plantsFilter);
		text = text.Replace("<None>", string.Empty);
		string text2 = SplitAndConvert(warehousesFilter);
		string text3 = SplitAndConvert(customersFilter);
		DateTime dateTime = cutoffDateFilter.AddDays(1.0);
		bool flag = !string.IsNullOrWhiteSpace(text);
		bool flag2 = !string.IsNullOrWhiteSpace(text2);
		bool flag3 = !string.IsNullOrWhiteSpace(text3);
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		SqlCommand sqlCommand = m1Database.NewSqlCommand(string.Empty);
		string value = "SELECT COALESCE(SUM((omdDeliveryQuantity - omdQuantityShipped)), 0) as quantityAllocated\r\n                From SalesOrders \r\n                left outer join SalesOrderLines on omlSalesOrderID = ompSalesOrderID \r\n                left outer join SalesOrderDeliveries on omdSalesOrderID = omlSalesOrderID and omdSalesOrderLineID = omlSalesOrderLineID \r\n                left outer join Warehouses on imwWarehouseID=omdPartWarehouseLocationID \r\n                left outer join Plants on xauPlantID = imwPlantID \r\n                Where ompClosed = 0 and omdShippedComplete = 0 and omdDeliveryType = 2\r\n                and (omdDeliveryQuantity - omdQuantityShipped) > 0\r\n                and omdDeliveryDate < @CutOffDate\r\n                and omdPartWarehouseLocationID = imlPartWarehouseID\r\n                " + (flag3 ? (" and ompCustomerOrganizationID In(" + text3 + ")") : string.Empty) + "\r\n                and omdPartID = imlPartID and omdPartRevisionID = imlPartRevisionID";
		string value2 = "SELECT COALESCE(SUM((jmaQuantityToPull - jmaQuantityIssued)), 0) As quantityAllocated\r\n                From JobAssemblies \r\n                left outer join Jobs on jmaJobID = jmpJobID\r\n                left outer join Warehouses on imwWarehouseID=jmaPartWareHouseLocationID\r\n                left outer join Plants on xauPlantID=imwPlantID\r\n                Where (jmaQuantityToPull - jmaQuantityIssued) > 0 and jmaIssuedComplete = 0\r\n                and IsNull(jmaScheduledDueDate, jmpProductionDueDate) < @CutOffDate\r\n                and jmpClosed = 0\r\n                and jmaPartWareHouseLocationID = imlPartWarehouseID\r\n                " + (flag3 ? (" and jmpCustomerOrganizationID In(" + text3 + ")") : string.Empty) + "\r\n                and jmaPartID = imlPartID and jmaPartRevisionID = imlPartRevisionID";
		string value3 = "SELECT COALESCE(SUM((jmmPullFromStockQuantity - jmmQuantityReceived)), 0) AS quantityAllocated \r\n                From JobMaterials \r\n                left outer join Jobs on jmmJobID = jmpJobID \r\n                left outer join Warehouses on imwWarehouseID=jmmPartWareHouseLocationID\r\n                left outer join Plants on xauPlantID=imwPlantID\r\n                Where (jmmPullFromStockQuantity - jmmQuantityReceived) > 0 and jmmReceivedComplete = 0 \r\n                and IsNull(jmmRequiredDate, jmpProductionDueDate) < @CutOffDate \r\n                and jmpClosed = 0\r\n                and jmmPartWarehouseLocationID = imlPartWarehouseID\r\n                " + (flag3 ? (" and jmpCustomerOrganizationID In(" + text3 + ")") : string.Empty) + "\r\n                and jmmPartID = imlPartID and jmmPartRevisionID = imlPartRevisionID";
		StringBuilder stringBuilder = new StringBuilder("SELECT SUM(quantityAllocated) FROM ( ");
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine("UNION ALL");
		stringBuilder.AppendLine(value2);
		stringBuilder.AppendLine("UNION ALL");
		stringBuilder.AppendLine(value3);
		stringBuilder.AppendLine(") AS tempTable");
		string commandText = string.Format("Select imwWarehouseID AS Warehouse,\r\n                    IsNull(imlMinimumQuantity, 0) As mrlMinimumQuantity,\r\n                    IsNull(imlMaximumQuantity, 0) As mrlMaximumQuantity,\r\n                    ({0}) AS mrlQuantityOnHand,\r\n                    ({1}) AS mrlQuantityToInspect,\r\n                    ({2}) AS mrlQuantityAllocated,\r\n                    ({3}) AS mrlInvQtyInProduction,\r\n                    ({4}) AS mrlForecastDemand\r\n                    From PartWarehouseLocations\r\n                    left outer join Warehouses w on imwWarehouseID = imlPartWarehouseID \r\n                    Where imlPartID = @PartId And imlPartRevisionID = @PartRevisionId \r\n                    {5}\r\n                    {6}", "SELECT SUM(imbQuantityOnHand) \r\n                    FROM PartBins \r\n                    WHERE imbPartID = imlPartID AND imlPartRevisionID = imbPartRevisionID AND imbWarehouseID = imwWarehouseID", "SELECT COALESCE(SUM(qalQuantityToInspect), 0) \r\n                    FROM InspectionLines \r\n                    WHERE ((qalStatus IN ('P', 'O') AND qalManualInspectionFinalized = 1 AND qalInspectionType = 1) OR (qalStatus IN ('P', 'O') AND qalSourceTableName != ''))\r\n                        AND qalPartID = imlPartID AND qalPartRevisionID = imlPartRevisionID AND qalPartWarehouseLocationID = imwWarehouseID", stringBuilder, "Select isnull(sum(Case When isnull(jmaQuantityReceivedToInventory,0) = 0 Then IsNull(jmaInventoryQuantity, 0) When isnull(jmaQuantityReceivedToInventory,0) > 0 AND jmaProductionComplete = 0 Then IsNull(jmaInventoryQuantity - jmaQuantityReceivedToInventory, 0) When isnull(jmaQuantityReceivedToInventory,0) > 0 AND jmaProductionComplete = 1 Then 0 Else 0 End),0) As InvQtyInProduction\r\n                    From JobAssemblies \r\n                    left outer Join Jobs on jmpJobID = jmaJobID \r\n                    Where jmaPartID = imlPartID And jmaPartRevisionID = imlPartRevisionID \r\n                    and jmaPartWarehouseLocationID = imlPartWarehouseID \r\n                    and IsNull(jmaScheduledDueDate, jmpProductionDueDate) < @CutOffDate \r\n                    and jmaClosed = 0 ", "SELECT IsNull(Sum(mrrDemandQuantity), 0) As mrrDemandQuantity\r\n                  FROM MRPDemands\r\n                  WHERE mrrType='PartForecast' and mrrPartID = @PartId And mrrPartRevisionID = @PartRevisionId And mrrSessionID = @SessionId And mrrPartWarehouseLocationID = imwWarehouseID ", flag ? (" and imwPlantID in (" + text + ") ") : string.Empty, (!flag && flag2) ? (" and imlPartWarehouseID in (" + text2 + ") ") : string.Empty);
		sqlCommand.CommandText = commandText;
		sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
		sqlCommand.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@CutOffDate", SqlDbType.DateTime)).Value = dateTime;
		SearchForm searchForm = new SearchForm(m1Database);
		searchForm.RowSourceCommand = sqlCommand;
		searchForm.SearchID = "M1MRPLINEQTYS";
		searchForm.Show();
	}

	public void SaveMRPConfiguration(M1BindingSource bindingSource)
	{
		M1UserSettings userSettings = _UserSettings;
		string userID = _UserID;
		M1DataDictionary dataDictionary = _DataDictionary;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (currentAsDataRow != null)
		{
			bool includePartForecasts = currentAsDataRow.Field<bool>("mrpIncludePartForecasts");
			bool consolidatePartForecastJobs = currentAsDataRow.Field<bool>("mrpConsolidatePartForecastJobs");
			userSettings.IncludePartForecasts = includePartForecasts;
			userSettings.ConsolidatePartForecastJobs = consolidatePartForecastJobs;
			userSettings.SaveSettings(dataDictionary, userID);
		}
	}

	public bool GetIncludePartForecastsDefaultValue()
	{
		return _Database.User.Settings.IncludePartForecasts;
	}

	public bool GetConsolidatePartForecastJobsDefaultValue()
	{
		return _Database.User.Settings.ConsolidatePartForecastJobs;
	}

	public void Dispose()
	{
		provider = null;
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
}
