using System;
using System.Collections.Generic;
using System.Data;

namespace M1.Core;

public class TableChangedEventArgs : EventArgs
{
	public string Table = string.Empty;

	public List<DataRow> AddedRows;

	public List<DataRow> ChangedRows;

	public List<DataRow> DeletedRows;

	public TableChangedEventArgs(string table, List<DataRow> addedRows, List<DataRow> changedRows, List<DataRow> deletedRows)
	{
		Table = table;
		AddedRows = addedRows;
		ChangedRows = changedRows;
		DeletedRows = deletedRows;
	}
}
