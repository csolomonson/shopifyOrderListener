using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IM1RecordsetProxy))]
public class M1AdoRecordsetProxy : IM1RecordsetProxy
{
	internal DataView dataView;

	private DataRow _CurrentDataRow;

	private int rowNumber = -1;

	private M1AdoConnectionProxy adoConnectionProxy;

	public SqlDataAdapter dataAdapter;

	private M1AdoRecordsetFieldsProxy fieldsProxy;

	private string sqlQuery = string.Empty;

	private string _Filter = string.Empty;

	public DataRow CurrentDataRow => _CurrentDataRow;

	public M1AdoRecordsetFieldsProxy FieldsCollection
	{
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get
		{
			return fieldsProxy;
		}
	}

	public bool EOF
	{
		get
		{
			if (dataView.Count != 0)
			{
				return rowNumber >= dataView.Count;
			}
			return true;
		}
	}

	public bool BOF => rowNumber <= -1;

	public int RecordCount => dataView.Count;

	public int Bookmark
	{
		get
		{
			return rowNumber;
		}
		set
		{
			if (dataView.Count != 0)
			{
				rowNumber = value;
				_CurrentDataRow = dataView[rowNumber].Row;
			}
			else
			{
				rowNumber = -1;
				_CurrentDataRow = null;
			}
		}
	}

	public string Filter
	{
		get
		{
			return _Filter;
		}
		set
		{
			dataView.RowFilter = value;
			_Filter = dataView.RowFilter;
			if (dataView.Count != 0)
			{
				MoveFirst();
				return;
			}
			rowNumber = -1;
			_CurrentDataRow = null;
		}
	}

	public string Sort
	{
		get
		{
			return dataView.Sort;
		}
		set
		{
			dataView.Sort = value;
			MoveFirst();
		}
	}

	public object Value
	{
		get
		{
			DataRow currentDataRow = CurrentDataRow;
			if (currentDataRow == null)
			{
				return null;
			}
			if (currentDataRow[0] == DBNull.Value)
			{
				return null;
			}
			Type type = null;
			if (type == null)
			{
				type = currentDataRow.Table.Columns[0].DataType;
			}
			if (type == typeof(int))
			{
				return currentDataRow.Field<int>(0);
			}
			if (type == typeof(byte))
			{
				return currentDataRow.Field<byte>(0);
			}
			if (type == typeof(short))
			{
				return currentDataRow.Field<short>(0);
			}
			if (type == typeof(string))
			{
				return currentDataRow.Field<string>(0);
			}
			if (type == typeof(decimal))
			{
				return currentDataRow.Field<decimal>(0);
			}
			if (type == typeof(double))
			{
				return currentDataRow.Field<double>(0);
			}
			if (type == typeof(Guid))
			{
				return currentDataRow.Field<Guid>(0).ToString("B");
			}
			if (currentDataRow[0] == DBNull.Value)
			{
				return null;
			}
			return currentDataRow[0];
		}
	}

	public M1AdoRecordsetProxy()
	{
		fieldsProxy = new M1AdoRecordsetFieldsProxy(this);
	}

	public M1AdoRecordsetProxy(DataRow[] rows)
		: this()
	{
		if (rows != null && rows.Length != 0)
		{
			LoadDataTable(rows.CopyToDataTable());
		}
		else
		{
			LoadDataTable(new DataTable());
		}
	}

	public M1AdoRecordsetProxy(DataTable table)
		: this()
	{
		LoadDataTable(table);
	}

	public M1AdoRecordsetProxy(DataView view)
		: this()
	{
		LoadDataView(view);
	}

	public M1AdoRecordsetProxy(DataView view, DataRow activeRow, M1AdoConnectionProxy connection, SqlDataAdapter adapter)
		: this()
	{
		dataAdapter = adapter;
		adoConnectionProxy = connection;
		LoadDataView(view);
		if (activeRow != null)
		{
			MoveLast();
			_CurrentDataRow = activeRow;
		}
	}

	public void LoadDataTable(DataTable table)
	{
		if (fieldsProxy != null)
		{
			fieldsProxy.Clear();
		}
		dataView = new DataView();
		if (table.TableName.Length == 0)
		{
			table.TableName = "main";
		}
		dataView.Table = table;
		rowNumber = -1;
		MoveFirst();
	}

	public void LoadDataView(DataView view)
	{
		if (fieldsProxy != null)
		{
			fieldsProxy.Clear();
		}
		dataView = view;
		rowNumber = -1;
		MoveFirst();
	}

	public DataTable GetDataTable()
	{
		return dataView.Table;
	}

	public void Open(string query, object connection, int cursorType = 3, int lockType = 1, int options = 0, object transaction = null)
	{
		fieldsProxy.Clear();
		fieldsProxy.Parent = this;
		dataView = new DataView();
		_CurrentDataRow = null;
		rowNumber = -1;
		adoConnectionProxy = connection as M1AdoConnectionProxy;
		sqlQuery = query;
		if (transaction == DBNull.Value || transaction == null)
		{
			transaction = adoConnectionProxy.SqlTransaction;
		}
		if (adoConnectionProxy != null)
		{
			DataTable dataTable = adoConnectionProxy.Database.GetDataTable(query, fillSchema: false, out dataAdapter, (SqlTransaction)transaction);
			dataTable.TableName = "DynamicTable";
			dataView.Table = dataTable;
			MoveFirst();
			return;
		}
		throw new M1Exception("M1AdoRecordsetProxy.Open method requires an M1AdoConnectionProxy object for the connection parameter.");
	}

	public M1AdoFieldProxy Fields(string name)
	{
		string text = name;
		if (char.IsNumber(text[0]) && int.TryParse(text, out var result))
		{
			text = dataView.Table.Columns[result].ColumnName;
		}
		return fieldsProxy[text];
	}

	public M1AdoRecordsetProxy Rows(int rowNumber)
	{
		if (rowNumber >= 0 && rowNumber < RecordCount)
		{
			return new M1AdoRecordsetProxy(new DataRow[1] { dataView.Table.Rows[rowNumber] });
		}
		throw new M1Exception($"There is no row at position {rowNumber} in selected items collection.");
	}

	public void Dispose()
	{
		Close();
	}

	public void Close()
	{
		if (fieldsProxy != null)
		{
			fieldsProxy.Clear();
			fieldsProxy.Parent = null;
		}
		adoConnectionProxy = null;
		dataView = null;
		_CurrentDataRow = null;
		rowNumber = -1;
	}

	public void UpdateBatch(string primaryTable = "")
	{
		if (dataView != null && dataView.Table != null && adoConnectionProxy != null)
		{
			if (!string.IsNullOrEmpty(primaryTable))
			{
				M1BindingSource.ChangedRowsInfo changedRowsInfo = new M1BindingSource.ChangedRowsInfo(dataView.Table);
				adoConnectionProxy.Database.UpdateData(dataView.Table, dataAdapter, adoConnectionProxy.SqlTransaction);
				adoConnectionProxy.Database.OnTableChanged(new TableChangedEventArgs(primaryTable, changedRowsInfo.AddedRows, changedRowsInfo.ChangedRows, changedRowsInfo.DeletedRows));
			}
			else
			{
				adoConnectionProxy.Database.UpdateData(dataView.Table, dataAdapter, adoConnectionProxy.SqlTransaction);
			}
		}
	}

	public void AddNew()
	{
		DataRow row = dataView.Table.NewRow();
		dataView.Table.Rows.Add(row);
		MoveLast();
	}

	public void MoveFirst()
	{
		if (dataView.Count != 0)
		{
			rowNumber = 0;
			_CurrentDataRow = dataView[rowNumber].Row;
		}
		else
		{
			rowNumber = -1;
			_CurrentDataRow = null;
		}
	}

	public void MoveNext()
	{
		if (dataView.Count != 0)
		{
			rowNumber++;
			if (rowNumber < dataView.Count)
			{
				_CurrentDataRow = dataView[rowNumber].Row;
				return;
			}
			_CurrentDataRow = null;
			rowNumber = dataView.Count;
		}
		else
		{
			_CurrentDataRow = null;
			rowNumber = dataView.Count;
		}
	}

	public void MovePrevious()
	{
		if (dataView.Count != 0)
		{
			rowNumber--;
			if (rowNumber >= 0 && rowNumber < dataView.Count - 1)
			{
				_CurrentDataRow = dataView[rowNumber].Row;
				return;
			}
			_CurrentDataRow = null;
			rowNumber = -1;
		}
		else
		{
			_CurrentDataRow = null;
			rowNumber = -1;
		}
	}

	public void MoveLast()
	{
		if (dataView.Count != 0)
		{
			rowNumber = dataView.Count - 1;
			_CurrentDataRow = dataView[rowNumber].Row;
		}
		else
		{
			rowNumber = -1;
			_CurrentDataRow = null;
		}
	}

	public void Requery()
	{
		Open(sqlQuery, adoConnectionProxy, 0, 0);
	}

	public string FormatDateForSelect(object date)
	{
		if (date != null && date != DBNull.Value)
		{
			return "'" + Convert.ToDateTime(date).ToString() + "'";
		}
		return "null";
	}

	public M1AdoRecordsetProxy Select(string filterExpression, string sort = "")
	{
		return new M1AdoRecordsetProxy(dataView.Table.Select(filterExpression, sort));
	}

	public void Find(string findCriteria, int skipRows = 0, int direction = 1, int startPosition = 0)
	{
		DataRow[] array = dataView.Table.Select(findCriteria);
		if (array.Length == 0)
		{
			if (direction == 1)
			{
				MoveLast();
			}
			else
			{
				MoveFirst();
			}
		}
		else
		{
			Bookmark = dataView.Table.Rows.IndexOf(array[0]);
		}
	}

	public object[,] GetRows(int Rows = -1, int StartRecord = 0, object Fields = null)
	{
		if (Rows < 0)
		{
			Rows = RecordCount;
		}
		switch (StartRecord)
		{
		case 0:
			StartRecord = Bookmark;
			break;
		case 2:
			StartRecord = ((RecordCount - 1 <= Rows) ? (RecordCount - 1) : (Rows - 1));
			Rows = 1;
			break;
		default:
			StartRecord--;
			break;
		}
		DataTable dataTable = dataView.Table.AsEnumerable().Skip(StartRecord).Take(Rows)
			.CopyToDataTable();
		object[,] array = new object[dataTable.Columns.Count, dataTable.Rows.Count];
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			for (int j = 0; j < dataTable.Columns.Count; j++)
			{
				array[j, i] = dataTable.Rows[i].ItemArray[j];
			}
		}
		return array;
	}
}
