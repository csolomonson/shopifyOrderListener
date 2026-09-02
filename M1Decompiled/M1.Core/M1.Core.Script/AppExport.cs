using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using ADODB;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
public class AppExport : IExport
{
	private IServiceProvider provider;

	public AppExport(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	private DataTable RecordSetToDataTable(Recordset record)
	{
		DataTable dataTable = new DataTable();
		foreach (string item in ExtractFieldNames(record))
		{
			dataTable.Columns.Add(item);
		}
		record.MoveFirst();
		while (!record.EOF)
		{
			DataRow dataRow = dataTable.NewRow();
			int num = 0;
			foreach (object item2 in ExtractFieldValues(record))
			{
				dataRow[num++] = item2;
			}
			dataTable.Rows.Add(dataRow);
			record.MoveNext();
		}
		return dataTable;
	}

	private IEnumerable<object> ExtractFieldValues(Recordset record)
	{
		for (int i = 0; i < record.Fields.Count; i++)
		{
			Field field = record.Fields[i];
			yield return field.Value;
		}
	}

	private IEnumerable<string> ExtractFieldNames(Recordset record)
	{
		for (int i = 0; i < record.Fields.Count; i++)
		{
			Field field = record.Fields[i];
			yield return field.Name;
		}
	}

	public bool XLS(object rsData, string cFileName, string cFieldList = "", bool bShowFieldCaptions = false)
	{
		cFieldList = checkNull(cFieldList);
		DataTable dataTable = TranslateDataToDatatable(rsData);
		if (dataTable == null)
		{
			return false;
		}
		new ExportService(provider).Excel(dataTable, cFileName, bShowFieldCaptions, cFieldList);
		return true;
	}

	public bool XML(object rsData, string cFileName, string cFieldList = "")
	{
		cFieldList = checkNull(cFieldList);
		DataTable dataTable = TranslateDataToDatatable(rsData);
		if (dataTable != null)
		{
			new ExportService(provider).Xml(dataTable, cFileName, cFieldList);
			return true;
		}
		return false;
	}

	public bool CSV(object rsData, string cFileName, string cFieldList = "", string cSeparator = "", bool bIncludeFieldNames = false)
	{
		cFieldList = checkNull(cFieldList);
		cSeparator = checkNull(cSeparator);
		DataTable dataTable = TranslateDataToDatatable(rsData);
		if (dataTable != null)
		{
			new ExportService(provider).Csv(dataTable, cFileName, cSeparator, bIncludeFieldNames, cFieldList);
			return true;
		}
		return false;
	}

	public bool DBF(object rsData, string cFileName, string cFieldList = "")
	{
		throw new M1Exception("Dbf export is not currently supported.");
	}

	public bool HTML(object rsData, string cFileName, string cFieldList = "")
	{
		cFieldList = checkNull(cFieldList);
		DataTable dataTable = TranslateDataToDatatable(rsData);
		if (dataTable != null)
		{
			new ExportService(provider).Html(dataTable, cFileName, cFieldList);
			return true;
		}
		return false;
	}

	public bool PDF(object rsData, string cFileName, string cFieldList = "")
	{
		cFieldList = checkNull(cFieldList);
		DataTable dataTable = TranslateDataToDatatable(rsData);
		if (dataTable != null)
		{
			new ExportService(provider).Pdf(dataTable, cFileName, includeFieldHeadings: true, cFieldList);
			return true;
		}
		return false;
	}

	private DataTable TranslateDataToDatatable(object rsData)
	{
		if (rsData is string)
		{
			return (provider.GetService(typeof(M1Database)) as M1Database).GetDataTable(new SqlCommand(rsData.ToString()));
		}
		if (rsData is M1AdoRecordsetProxy m1AdoRecordsetProxy)
		{
			return m1AdoRecordsetProxy.GetDataTable();
		}
		if (!(rsData is Recordset))
		{
			return null;
		}
		return RecordSetToDataTable((Recordset)rsData);
	}

	private string checkNull(string data)
	{
		if (data != null)
		{
			return data;
		}
		return string.Empty;
	}
}
