using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ImportTaxTableProcess : ProcessParameters
{
	public ImportTaxTableProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "paxIncomeTaxID" };
		KeyValueTableName = "IncomeTaxes";
		Description = "Use this tool to import the selected taxes from the Tools\\TaxTables folder where M1 is installed.";
		GridID = "M1INCOMETAXESIMPORT";
		BindingSourceTable = string.Empty;
		HelpLink = "payroll_loadTaxTables.htm";
		SecurityRole = "PayrollAdmin";
		ContinueMessage = "This will import the {0} selected taxes from the Tools\\TaxTables folder. Are you sure you want to continue?";
	}

	protected override void OnGetData(GetDataEventArgs arg)
	{
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		DataTable table = arg.Table;
		string fileName = getFileName();
		if (!File.Exists(fileName))
		{
			return;
		}
		DataSet dataSet = new DataSet("IncomeTaxes");
		dataSet.ReadXml(fileName, XmlReadMode.ReadSchema);
		DataTable dataTable = dataSet.Tables["IncomeTaxes"];
		DataTable dataTable2 = m1Database.GetDataTable("Select paxIncomeTaxID From IncomeTaxes Order By paxIncomeTaxID");
		List<string> list = new List<string>();
		foreach (DataRow row in dataTable2.Rows)
		{
			list.Add(row.Field<string>("paxIncomeTaxID"));
		}
		foreach (DataRow row2 in dataTable.Rows)
		{
			DataRow dataRow2 = arg.BindingSource.AddNew() as DataRow;
			foreach (DataColumn column in table.Columns)
			{
				if (dataTable.Columns.Contains(column.ColumnName))
				{
					dataRow2[column] = row2[column.ColumnName];
				}
			}
			if (table.Columns.Contains("FieldSelected") && list.Contains(row2.Field<string>("paxIncomeTaxID"), StringComparer.CurrentCultureIgnoreCase))
			{
				dataRow2["FieldSelected"] = true;
			}
		}
	}

	private string getFileName()
	{
		M1.Core.AppContext appContext = ServiceProvider.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
		return Path.Combine(Path.Combine(appContext.IsHosted ? appContext.Metadata.FileShareLocation : appContext.Server.Location, "Tools\\TaxTables"), "Taxes.xml");
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.SelectedItems;
		_ = arg.Messages;
		string fileName = getFileName();
		if (!File.Exists(fileName))
		{
			return;
		}
		DataSet dataSet = new DataSet("IncomeTaxes");
		dataSet.ReadXml(fileName, XmlReadMode.ReadSchema);
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		SqlTransaction sqlTransaction = m1Database.BeginTransaction();
		try
		{
			foreach (ProcessSelectedItemValues selectedItem in arg.SelectedItems)
			{
				string text = selectedItem.KeyValues[0].ToString();
				if (!string.IsNullOrEmpty(text))
				{
					ProcessTax(m1Database, dataSet, text, sqlTransaction);
				}
			}
			m1Database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			m1Database.RollbackTransaction(sqlTransaction);
			throw;
		}
		if (arg.SelectedItems.Find((ProcessSelectedItemValues item) => item.KeyValues[0].ToString().Equals("FED", StringComparison.CurrentCultureIgnoreCase)) != null)
		{
			arg.Messages.Add("FED Tax Tables have been imported. Please review FUTA rates to ensure payroll is processed with the correct rates.");
		}
	}

	protected void ProcessTax(M1Database database, DataSet taxesDs, string incomeTaxID, SqlTransaction transaction)
	{
		if (ProcessTaxSingleTable(database, taxesDs, incomeTaxID, "IncomeTaxes", new string[1] { "paxIncomeTaxID" }, transaction) > 0 && ProcessTaxSingleTable(database, taxesDs, incomeTaxID, "IncomeTaxTypes", new string[2] { "pafIncomeTaxID", "pafIncomeTaxTypeID" }, transaction) > 0 && ProcessTaxSingleTable(database, taxesDs, incomeTaxID, "IncomeTaxTables", new string[3] { "pazIncomeTaxID", "pazIncomeTaxTypeID", "pazIncomeTaxTableID" }, transaction) > 0 && ProcessTaxSingleTable(database, taxesDs, incomeTaxID, "IncomeTaxTableRevisions", new string[4] { "parIncomeTaxID", "parIncomeTaxTypeID", "parIncomeTaxTableID", "parIncomeTaxTableRevisionID" }, transaction) > 0 && ProcessTaxSingleTable(database, taxesDs, incomeTaxID, "IncomeTaxTableLines", new string[5] { "palIncomeTaxID", "palIncomeTaxTypeID", "palIncomeTaxTableID", "palIncomeTaxTableRevisionID", "palIncomeTaxTableLineID" }, transaction) > 0 && database.Region.Equals("CAN", StringComparison.CurrentCultureIgnoreCase))
		{
			ProcessTaxSingleTable(database, taxesDs, incomeTaxID, "IncomeTaxTableSurtaxes", new string[5] { "pacIncomeTaxID", "pacIncomeTaxTypeID", "pacIncomeTaxTableID", "pacIncomeTaxTableRevisionID", "pacIncomeTaxTableSurtaxID" }, transaction);
		}
	}

	protected int ProcessTaxSingleTable(M1Database database, DataSet taxesDs, string incomeTaxID, string tableName, string[] keyFields, SqlTransaction transaction)
	{
		int num = 0;
		DataRow[] array = taxesDs.Tables[tableName].Select(keyFields[0] + " = " + incomeTaxID.ToLinq());
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("Select * From " + tableName + " Where " + keyFields[0] + " = " + incomeTaxID.ToSql(), fillSchema: false, out adapter, transaction);
		StringBuilder stringBuilder = new StringBuilder();
		DataRow[] array2 = array;
		foreach (DataRow dataRow in array2)
		{
			stringBuilder.Length = 0;
			for (int j = 0; j < keyFields.Length; j++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(keyFields[j] + " = " + dataRow[keyFields[j]].ToLinq());
			}
			DataRow[] array3 = dataTable.Select(stringBuilder.ToString());
			if (tableName.Equals("IncomeTaxTableRevisions", StringComparison.CurrentCultureIgnoreCase))
			{
				database.ExecuteCommand("Delete From IncomeTaxTableLines Where palIncomeTaxID = " + dataRow["parIncomeTaxID"].ToSql() + " And palIncomeTaxTypeID = " + dataRow["parIncomeTaxTypeID"].ToSql() + " And palIncomeTaxTableID = " + dataRow["parIncomeTaxTableID"].ToSql() + " And palIncomeTaxTableRevisionID = " + dataRow["parIncomeTaxTableRevisionID"].ToSql(), transaction);
			}
			DataRow dataRow2;
			if (array3.Length == 0)
			{
				dataRow2 = dataTable.NewRow().BlankRow();
				dataTable.Rows.Add(dataRow2);
			}
			else
			{
				dataRow2 = array3[0];
			}
			foreach (DataColumn column in dataRow.Table.Columns)
			{
				if (!SystemGeneratedFields.IsGenerated(column.ColumnName))
				{
					dataRow2[column.ColumnName] = dataRow[column];
				}
			}
			num++;
		}
		database.UpdateData(dataTable, adapter, transaction);
		return num;
	}
}
