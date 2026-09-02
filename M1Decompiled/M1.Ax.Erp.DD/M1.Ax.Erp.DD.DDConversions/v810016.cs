using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.016", "Convert customizations for views from com to .net format", "")]
public class v810016
{
	public v810016(DDConversionParms parms)
	{
		convertDatabaseOptionsForms(parms);
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete From DDFormDetails where (deControlName In ('cmbImoOverlap', 'cmbQmoOverlap', 'chkXawIgnoreCalendarQueue', 'chkXawIgnoreCalendarMove', 'txtSun', 'txtMon', 'txtTue', 'txtWed', 'txtThu', 'txtFri', 'txtSat', 'txtHrs', 'txtXawHoursSun', 'txtXawHoursMon', 'txtXawHoursTue', 'txtXawHoursWed', 'txtXawHoursThu', 'txtXawHoursFri','txtXawHoursSat', 'txtStart', 'txtXawDayStartTimeSun', 'txtXawDayStartTimeMon', 'txtXawDayStartTimeTue', 'txtXawDayStartTimeWed', 'txtXawDayStartTimeThu', 'txtXawDayStartTimeFri', 'txtXawDayStartTimeSat', 'chkXawIgnoreCalendarQueue','CMBJMOOVERLAP', 'txtJmoOverlapJobOperationID') and deCustom = 0) Or (deControlName = 'm1MaskedTextEditor1' and deFormID = 'M1.Ax.Erp.Forms.Production.Job.WorkCenterView' and deCustom = 0) Or (deControlName = 'objSerialNumbersRework' and deCustom = 0)");
		SqlCommand sqlCommand = new SqlCommand("IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = @TableName)) BEGIN Drop Table DDFormDetailsOriginal; END Select * Into DDFormDetailsOriginal From DDFormDetails;");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = "DDFormDetailsOriginal";
		parms.DmoDD.ExecuteCommand(null, parms.DatabaseName, sqlCommand);
		sqlCommand = new SqlCommand("IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = @TableName)) BEGIN Drop Table DDCodeOriginal; END Select * Into DDCodeOriginal From DDCode;");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = "DDCodeOriginal";
		parms.DmoDD.ExecuteCommand(null, parms.DatabaseName, sqlCommand);
		foreach (KeyValuePair<string, string> item in new V8FormMappings().GetList())
		{
			parms.DmoDD.ConvertCustomControls(parms.DatabaseName, item.Key, item.Value, string.Empty, -1, -1, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDUsers Set duDeveloperProperties = CAST(REPLACE(CAST(duDeveloperProperties AS NVARCHAR(MAX)), 'ComFormDefinition\\" + item.Key + "', 'NetFormDefinition\\" + item.Value + "') AS NTEXT) Where duDeveloperProperties Is Not Null And duDeveloperProperties Like '%ComFormDefinition\\" + item.Key + "%'");
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDUsers Set duDeveloperProperties = CAST(REPLACE(CAST(duDeveloperProperties AS NVARCHAR(MAX)), 'ComFormDefinition," + item.Key + "', 'NetFormDefinition," + item.Value + "') AS NTEXT) Where duDeveloperProperties Is Not Null And duDeveloperProperties Like '%ComFormDefinition," + item.Key + "%'");
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDSolutionDetails Set diName = '" + item.Value + "', diType = 'NetFormDefinition' Where diName = '" + item.Key + "' And diType = 'ComFormDefinition' And diCustom <> 0");
		}
		bool flag = false;
		string text = string.Empty;
		DataTable dataTable = parms.DmoDD.GetDataTable(null, parms.DatabaseName, "Select * From DDCode Where dkCustom <> 0 And dkSourceTable = 'DDScripts'", fillSchema: true, out var adapter);
		foreach (DataRow row in dataTable.Rows)
		{
			string text2 = row.Field<string>("dkCode");
			if (parms.ConvertCustomFormCode)
			{
				text = parms.DmoDD.convertCustomFormCode(text2);
			}
			if (text2 != text)
			{
				row.SetField("dkCode", text);
				flag = true;
			}
		}
		if (flag)
		{
			parms.DmoDD.UpdateData(null, null, parms.DatabaseName, dataTable, adapter, null);
		}
		dataTable = null;
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDForms set dmAssembliesUser = 'M1.Forms.Controls.dll:M1.Forms.Controls|M1.Ax.Erp.Forms.dll:M1.Ax.Erp.Forms.Financial|M1.Ax.Erp.Forms.dll:M1.Ax.Erp.Forms.Sales.Contact|M1.Ax.Erp.Forms.dll:M1.Ax.Erp.Forms.DocumentRegister|' where IsNull(dmAssembliesUser, '') = '' And dmFormID in (Select deFormID from DDFormDetails where deClassID like 'M1.Ax%' and deCustom <> 0) And IsNull(dmAssemblies,'') <> 'M1.Forms.Controls.dll:M1.Forms.Controls|M1.Ax.Erp.Forms.dll:M1.Ax.Erp.Forms.Financial|M1.Ax.Erp.Forms.dll:M1.Ax.Erp.Forms.Sales.Contact|M1.Ax.Erp.Forms.dll:M1.Ax.Erp.Forms.DocumentRegister|'");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDSolutionDetails Set diType = 'SFEFormDefinition'  From DDSolutionDetails Inner Join DDForms On diName = dmFormID Where dmFormType = 2 And diType <> 'SFEFormDefinition'");
	}

	private static void convertDatabaseOptionsForms(DDConversionParms parms)
	{
		foreach (KeyValuePair<string, string> item in new V8OptionFormMappings().GetList())
		{
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deFormID = " + item.Value.ToSql() + ", deClassID = Replace(deClassID, 'M1.UI.Controls.', 'M1.Forms.Controls.') where deFormID like " + item.Key.ToSql());
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete From DDFormDetails Where deFormID Like " + item.Value.ToSql() + " And deClassID = 'M1.BO.M1BindingSource'");
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDSolutionDetails Set diName = " + item.Value.ToSql() + ", diType = 'NetFormDefinition' Where diName = " + item.Key.ToSql() + " And diType = 'NetFormDefinition' And diCustom <> 0");
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormCodeTemp Set dmFormID = " + item.Value.ToSql() + " Where dmFormID = " + item.Key.ToSql());
			bool flag = false;
			string empty = string.Empty;
			string text = string.Empty;
			Guid? guid = null;
			DataTable dataTable = parms.DmoDD.GetDataTable(null, parms.DatabaseName, "Select dkCodeID From DDFormCodeTemp Where dmFormID = " + M1Util.ConvertToSql(item.Value));
			if (dataTable.Rows.Count != 0)
			{
				guid = dataTable.Rows[0].Field<Guid>("dkCodeID");
			}
			if (!guid.HasValue)
			{
				continue;
			}
			SqlDataAdapter adapter;
			DataTable dataTable2 = parms.DmoDD.GetDataTable(null, parms.DatabaseName, "Select * From DDCode Where dkCodeID = " + M1Util.ConvertToSql(guid.Value), fillSchema: true, out adapter);
			if (dataTable2.Rows.Count != 0)
			{
				DataRow row = dataTable2.Rows[0];
				empty = row.Field<string>("dkCode");
				if (parms.ConvertCustomFormCode)
				{
					text = parms.DmoDD.convertCustomFormCode(empty);
				}
				if (empty != text)
				{
					row.SetField("dkCode", text);
					flag = true;
				}
			}
			if (flag)
			{
				parms.DmoDD.UpdateData(null, null, parms.DatabaseName, dataTable2, adapter, null);
			}
		}
	}
}
