using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class NonConformance
{
	public bool ConvertRMAClaimProblemsToNonConformances(M1Database database, SqlTransaction transaction, List<KeyValuePair<string, string>> customFields = null)
	{
		DataTable dataTable = database.GetDataTable("select * from RMAClaimProblems", transaction);
		if (dataTable.Rows.Count == 0)
		{
			return false;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
		m1BindingSource.LoadDefinition(string.Empty, "NonConformances", null, true, loadDataNow: false);
		m1BindingSource.ClearCache();
		M1BindingSource m1BindingSource2 = new M1BindingSource(database, transaction);
		m1BindingSource2.LoadDefinition(string.Empty, "NonConformances", null, true, loadDataNow: false);
		string text = string.Empty;
		foreach (DataRow row in dataTable.Rows)
		{
			m1BindingSource2.ClearCache();
			m1BindingSource2.NavigateTo(database, "qarUniqueID = " + M1Util.ConvertToSql(row.Field<Guid>("rarUniqueID")));
			if (m1BindingSource2.CurrentAsDataRow == null)
			{
				DataRow dataRow2 = m1BindingSource.AddNew() as DataRow;
				m1BindingSource.SetKeyToNextAvailable(dataRow2);
				dataRow2.SetField("qarRMAClaimID", row.Field<string>("rarRMAClaimID"));
				dataRow2.SetField("qarRMAClaimLineID", row.Field<short>("rarRMAClaimLineID"));
				dataRow2.SetField("qarNonConformanceCategoryID", row.Field<string>("rarNonConformanceCategoryID"));
				dataRow2.SetField("qarNonConformanceCodeID", row.Field<string>("rarNonConformanceCodeID"));
				dataRow2.SetField("qarNonConformanceCauseID", row.Field<string>("rarNonConformanceCauseID"));
				dataRow2.SetField("qarCorrectiveActionType", row.Field<byte>("rarRepairType"));
				dataRow2.SetField("qarCorrectiveActionComplete", row.Field<bool>("rarRepairsComplete"));
				dataRow2.SetField("qarCorrectiveActionCategoryID", row.Field<string>("rarCorrectiveActionCategoryID"));
				dataRow2.SetField("qarCorrectiveActionCodeID", row.Field<string>("rarCorrectiveActionCodeID"));
				dataRow2.SetField("qarCorrectiveActionDate", row.Field<DateTime?>("rarRepairedDate"));
				dataRow2.SetField("qarHoursAllowed", row.Field<decimal>("rarHoursAllowed"));
				dataRow2.SetField("qarHoursRequested", row.Field<decimal>("rarHoursRequested"));
				dataRow2.SetField("qarActualHours", row.Field<decimal>("rarActualHours"));
				dataRow2.SetField("qarRepairedByOrganizationID", row.Field<string>("rarRepairedByOrganizationID"));
				dataRow2.SetField("qarSubcontractAmount", row.Field<decimal>("rarSubcontractAmount"));
				dataRow2.SetField("qarSubcontractAmountForeign", row.Field<decimal>("rarSubcontractAmtForeign"));
				dataRow2.SetField("qarNonConformanceText", row.Field<string>("rarNonConformanceText"));
				dataRow2.SetField("qarNonConformanceRTF", row.Field<string>("rarNonConformanceRTF"));
				dataRow2.SetField("qarCorrectiveActionText", row.Field<string>("rarCorrectiveActionText"));
				dataRow2.SetField("qarCorrectiveActionRTF", row.Field<string>("rarCorrectiveActionRTF"));
				dataRow2.SetField("qarCreatedBy", row.Field<string>("rarCreatedBy"));
				if (row["rarCreatedDate"] != DBNull.Value)
				{
					dataRow2.SetField("qarCreatedDate", row.Field<DateTime>("rarCreatedDate"));
				}
				dataRow2.SetField("qarUniqueID", row.Field<Guid>("rarUniqueID"));
				if (customFields.Count != 0)
				{
					text += BuildUpdateDataCustomFields(row, customFields);
				}
				SqlCommand sqlCommand = database.NewSqlCommand("Update Jobs set jmpNonConformanceID = @NonConfID from Jobs Inner Join RMAClaimProblems on jmpRMAClaimID = rarRMAClaimID and jmpRMAClaimLineID = rarRMAClaimLineID and jmpRMAClaimProblemID = rarRMAClaimProblemID where jmpRMAClaimID = @RmaClaimID and jmpRMAClaimLineID = @RmaClaimLineID and jmpRMAClaimProblemID = @RmaClaimProblemID and jmpRMAClaimProblemID <> 0 and jmpNonConformanceID = ''");
				sqlCommand.Parameters.Add(new SqlParameter("@NonConfID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("qarNonConformanceID");
				sqlCommand.Parameters.Add(new SqlParameter("@RmaClaimID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("qarRMAClaimID");
				sqlCommand.Parameters.Add(new SqlParameter("@RmaClaimLineID", SqlDbType.Int)).Value = dataRow2.Field<short>("qarRMAClaimLineID");
				sqlCommand.Parameters.Add(new SqlParameter("@RmaClaimProblemID", SqlDbType.Int)).Value = row.Field<short>("rarRMAClaimProblemID");
				database.ExecuteCommand(sqlCommand, transaction);
			}
		}
		m1BindingSource.SaveData();
		if (text.Length != 0)
		{
			database.ExecuteCommand(text);
		}
		return true;
	}

	private string BuildUpdateDataCustomFields(DataRow sourceDataRow, List<KeyValuePair<string, string>> customFields)
	{
		string format = "UPDATE NonConformances SET {0} where qarRMAClaimID = {1} AND qarRMAClaimLineId = {2}; ";
		List<string> list = new List<string>();
		if (customFields != null)
		{
			foreach (KeyValuePair<string, string> customField in customFields)
			{
				list.Add($"{customField.Value} = {M1Util.ConvertToSql(sourceDataRow[customField.Key])}");
			}
		}
		string arg = string.Join(", ", list);
		return string.Format(format, arg, M1Util.ConvertToSql(sourceDataRow.Field<string>("rarRMAClaimID")), M1Util.ConvertToSql(sourceDataRow.Field<short>("rarRMAClaimLineID")));
	}

	public DataTable GetCustomFieldsbyTable(M1Database database)
	{
		return database.GetDataTable("SELECT COLUMN_NAME, DATA_TYPE, isnull(ISNULL(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION), 0) as LENGTH, ISNULL(NUMERIC_SCALE, 0) as SCALE, SUBSTRING(COLUMN_NAME, 1, 4) AS PREFIX FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RMAClaimProblems'AND COLUMN_NAME like 'u%' ");
	}
}
