using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Documents.Reports.Report;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.DocumentExport;
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace M1.Core;

public class ExportService
{
	private AppContext currentContext;

	private UltraGrid tempGrid;

	private bool tempIncludeFieldHeadings;

	private string[] tempFieldListArray;

	private string[] tempFieldCaptionListArray;

	private bool tempOnlySelectedRows;

	private bool tempScalePrint;

	private int tempScaleWidthToPage;

	private const int MAX_COLUMNS = 100;

	private UltraGrid m1Grid;

	public ExportService(IServiceProvider provider)
	{
		currentContext = provider.GetService(typeof(AppContext)) as AppContext;
	}

	public ExportService(AppContext context)
	{
		currentContext = context;
	}

	private void UltraGridExcelExporter1_ExportStarted(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.ExportStartedEventArgs e)
	{
		e.Layout.Bands[0].ColHeadersVisible = tempIncludeFieldHeadings;
		setupColumnsExportLayout(e.Layout.Bands[0].Columns);
	}

	private void DocExporter_ExportStarted(object sender, Infragistics.Win.UltraWinGrid.DocumentExport.ExportStartedEventArgs e)
	{
		e.Layout.Bands[0].ColHeadersVisible = tempIncludeFieldHeadings;
	}

	private void doCheck(DataTable data, ref string fileName, ref string folder, string extension)
	{
		folder = Path.GetDirectoryName(fileName);
		fileName = Path.GetFileName(fileName);
		if (folder.Length == 0)
		{
			folder = (currentContext.IsHosted ? currentContext.Metadata.FileShareLocation : currentContext.Client.Location);
		}
		if (Path.GetExtension(fileName).Length == 0)
		{
			fileName = Path.ChangeExtension(fileName, extension);
		}
		if (fileName.Length == 0)
		{
			throw new M1Exception("No filename has been specified.");
		}
		doDataCheck(data);
	}

	private void doDataCheck(DataTable data)
	{
		if (data == null || data.Rows.Count == 0)
		{
			throw new M1Exception("There were no records to export.");
		}
	}

	private string checkFieldList(DataTable data, string fieldList)
	{
		if (fieldList == null)
		{
			return string.Empty;
		}
		if (fieldList.Length == 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataColumn column in data.Columns)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(column.ColumnName);
			}
			return stringBuilder.ToString();
		}
		return fieldList;
	}

	private string getValidColumnName(string columnName)
	{
		char newChar = '_';
		columnName = columnName.Replace('\n', newChar);
		columnName = columnName.Replace('\t', newChar);
		columnName = columnName.Replace('\r', newChar);
		columnName = columnName.Replace(' ', newChar);
		columnName = columnName.Replace('~', newChar);
		columnName = columnName.Replace('`', newChar);
		columnName = columnName.Replace('!', newChar);
		columnName = columnName.Replace('@', newChar);
		columnName = columnName.Replace('#', newChar);
		columnName = columnName.Replace('$', newChar);
		columnName = columnName.Replace('%', newChar);
		columnName = columnName.Replace('^', newChar);
		columnName = columnName.Replace('&', newChar);
		columnName = columnName.Replace('*', newChar);
		columnName = columnName.Replace('(', newChar);
		columnName = columnName.Replace(')', newChar);
		columnName = columnName.Replace('+', newChar);
		columnName = columnName.Replace('=', newChar);
		columnName = columnName.Replace('{', newChar);
		columnName = columnName.Replace('}', newChar);
		columnName = columnName.Replace('[', newChar);
		columnName = columnName.Replace(']', newChar);
		columnName = columnName.Replace('|', newChar);
		columnName = columnName.Replace('\\', newChar);
		columnName = columnName.Replace(':', newChar);
		columnName = columnName.Replace(';', newChar);
		columnName = columnName.Replace('"', newChar);
		columnName = columnName.Replace('\'', newChar);
		columnName = columnName.Replace('<', newChar);
		columnName = columnName.Replace('>', newChar);
		columnName = columnName.Replace(',', newChar);
		columnName = columnName.Replace('?', newChar);
		columnName = columnName.Replace('/', newChar);
		return columnName;
	}

	private DataTable getDataTableToExport(DataTable data, string[] fieldListArray, string[] fieldCaptionListArray, bool includeFieldHeadings, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		DataTable dataTable = new DataTable();
		if (fieldListArray.Length != 0)
		{
			for (int i = 0; i < fieldListArray.Length; i++)
			{
				string text = fieldListArray[i];
				DataColumn dataColumn = new DataColumn(text, data.Columns[text].DataType);
				if (includeFieldHeadings && fieldListArray.Length == fieldCaptionListArray.Length)
				{
					dataColumn.Caption = fieldCaptionListArray[i];
				}
				dataTable.Columns.Add(dataColumn);
			}
			if (onlySelectedRows && selectedRows != null)
			{
				foreach (DataRow selectedRow in selectedRows)
				{
					DataRow dataRow = dataTable.NewRow();
					foreach (DataColumn column in dataTable.Columns)
					{
						dataRow[column.ColumnName] = selectedRow[column.ColumnName];
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			else
			{
				foreach (DataRow row in data.Rows)
				{
					DataRow dataRow3 = dataTable.NewRow();
					foreach (DataColumn column2 in dataTable.Columns)
					{
						dataRow3[column2.ColumnName] = row[column2.ColumnName];
					}
					dataTable.Rows.Add(dataRow3);
				}
			}
		}
		return dataTable;
	}

	private UltraGrid getUltraGridToExport(DataTable data, string[] fieldListArray, string[] fieldCaptionListArray, bool includeFieldNames, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		DataTable dataTableToExport = getDataTableToExport(data, fieldListArray, fieldCaptionListArray, includeFieldNames, onlySelectedRows, selectedRows);
		UltraGrid obj = new UltraGrid
		{
			BindingContext = new BindingContext(),
			DataSource = dataTableToExport
		};
		Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
		Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
		Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
		Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
		appearance.BackColor = SystemColors.Window;
		appearance.BorderColor = SystemColors.InactiveCaption;
		obj.DisplayLayout.Appearance = appearance;
		obj.DisplayLayout.BorderStyle = UIElementBorderStyle.Rounded1;
		obj.DisplayLayout.CaptionVisible = DefaultableBoolean.False;
		obj.DisplayLayout.Appearance = appearance;
		obj.DisplayLayout.Override.BorderStyleCell = UIElementBorderStyle.Solid;
		obj.DisplayLayout.Override.BorderStyleRow = UIElementBorderStyle.Solid;
		appearance2.BorderColor = Color.Silver;
		appearance2.TextTrimming = TextTrimming.EllipsisCharacter;
		obj.DisplayLayout.Override.CellAppearance = appearance2;
		obj.DisplayLayout.Override.CellPadding = 0;
		appearance3.TextHAlignAsString = "Left";
		if (includeFieldNames)
		{
			appearance3.BackColor = Color.FromArgb(191, 219, 255);
		}
		obj.DisplayLayout.Override.HeaderAppearance = appearance3;
		obj.DisplayLayout.Override.HeaderStyle = HeaderStyle.Standard;
		appearance4.BackColor = SystemColors.Window;
		appearance4.BorderColor = Color.Silver;
		obj.DisplayLayout.Override.RowAppearance = appearance4;
		obj.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
		return obj;
	}

	private void writeCsvRow(StreamWriter sw, DataRow dr, int iColCount, DataTable data, DataColumn column, string[] fieldListArray, string quoteChar, string separator)
	{
		for (int i = 0; i < iColCount; i++)
		{
			column = data.Columns[fieldListArray[i]];
			if (column.DataType == typeof(string))
			{
				if (Convert.IsDBNull(dr[column]))
				{
					sw.Write(quoteChar + quoteChar);
				}
				else
				{
					sw.Write(quoteChar);
					if (quoteChar.Length != 0)
					{
						sw.Write(dr[column].ToString().Trim().Replace(quoteChar, string.Empty));
					}
					else
					{
						sw.Write(dr[column].ToString().Trim());
					}
					sw.Write(quoteChar);
				}
			}
			else if (!(column.DataType == typeof(byte[])))
			{
				if (column.DataType == typeof(decimal))
				{
					if (!Convert.IsDBNull(dr[column]))
					{
						sw.Write(dr.Field<decimal>(column).ToString("0.########"));
					}
				}
				else if (!Convert.IsDBNull(dr[column]))
				{
					sw.Write(dr[column].ToString());
				}
			}
			if (i < iColCount - 1)
			{
				sw.Write(separator);
			}
		}
		sw.Write(sw.NewLine);
	}

	private void getDbfTypeForColumn(DataColumn column, StringBuilder fieldData)
	{
		_ = string.Empty;
		if (!(column.DataType == typeof(bool)) && (!(column.DataType == typeof(string)) || column.MaxLength <= 255) && !(column.DataType == typeof(byte[])) && !(column.DataType == typeof(decimal)) && !(column.DataType == typeof(DateTime)) && !(column.DataType == typeof(short)) && !(column.DataType == typeof(int)))
		{
			_ = column.DataType == typeof(float);
		}
	}

	private DataTable getDataTable(UltraGrid grid)
	{
		DataTable result = null;
		if (grid.DataSource is DataTable)
		{
			result = (DataTable)grid.DataSource;
		}
		else if (grid.DataSource is M1BindingSource)
		{
			return ((M1BindingSource)grid.DataSource).GetDataView().ToTable();
		}
		return result;
	}

	private bool shouldRowBeExported(UltraGridRow currentRow, bool isCorrectLayout)
	{
		if (isCorrectLayout)
		{
			UltraGridRow rowFromPrintRow = tempGrid.GetRowFromPrintRow(currentRow);
			if (tempOnlySelectedRows)
			{
				return rowFromPrintRow.Selected;
			}
			return !rowFromPrintRow.IsFilteredOut;
		}
		return true;
	}

	private void setupColumnsExportLayout(ColumnsCollection columns)
	{
		foreach (UltraGridColumn column in columns)
		{
			bool hidden = true;
			for (int i = 0; i < tempFieldListArray.Length; i++)
			{
				string value = tempFieldListArray[i];
				if (column.Key.Equals(value, StringComparison.CurrentCultureIgnoreCase))
				{
					hidden = false;
					if (tempIncludeFieldHeadings && tempFieldListArray.Length == tempFieldCaptionListArray.Length)
					{
						column.Header.Caption = tempFieldCaptionListArray[i];
						column.PerformAutoResize();
					}
					break;
				}
			}
			column.Hidden = hidden;
		}
	}

	private void exportByPortions(ref UltraGrid grid, string path, FileFormat format, int columnsOnPage)
	{
		using UltraGridDocumentExporter ultraGridDocumentExporter = new UltraGridDocumentExporter();
		ultraGridDocumentExporter.ExportStarted += DocExporter_ExportStarted;
		ultraGridDocumentExporter.InitializeRow += docExporter_InitializeRow;
		int num = 0;
		Infragistics.Documents.Reports.Report.Report report = new Infragistics.Documents.Reports.Report.Report();
		grid.BeginUpdate();
		int num2 = hideAll(ref grid);
		while (num < num2)
		{
			unhideInDeep(num, ref grid);
			num++;
			if (num % columnsOnPage == 0)
			{
				ultraGridDocumentExporter.Export(grid, report);
				hideAll(ref grid);
			}
		}
		if (num2 % columnsOnPage != 0)
		{
			ultraGridDocumentExporter.Export(grid, report);
		}
		unHideAll(ref grid);
		grid.EndUpdate();
		report.Publish(path, format);
		ultraGridDocumentExporter.ExportStarted -= DocExporter_ExportStarted;
		ultraGridDocumentExporter.InitializeRow -= docExporter_InitializeRow;
	}

	private void unHideAll(ref UltraGrid grid)
	{
		foreach (UltraGridBand band in grid.DisplayLayout.Bands)
		{
			foreach (UltraGridColumn column in band.Columns)
			{
				if (!column.IsChaptered)
				{
					grid.DisplayLayout.Bands[band.Index].Columns[column.Index].Hidden = false;
				}
			}
		}
	}

	private int hideAll(ref UltraGrid grid)
	{
		int num = -1;
		foreach (UltraGridBand band in grid.DisplayLayout.Bands)
		{
			if (num < band.Columns.Count)
			{
				num = band.Columns.Count;
			}
			foreach (UltraGridColumn column in band.Columns)
			{
				grid.DisplayLayout.Bands[band.Index].Columns[column.Index].Hidden = true;
			}
		}
		return num;
	}

	private void unhideInDeep(int colIndex, ref UltraGrid grid)
	{
		for (int num = grid.DisplayLayout.Bands.Count - 1; num > -1; num--)
		{
			UltraGridBand ultraGridBand = grid.DisplayLayout.Bands[num];
			if (ultraGridBand.Columns.Count > colIndex)
			{
				for (int i = 0; i < tempFieldListArray.Length; i++)
				{
					string value = tempFieldListArray[i];
					if (ultraGridBand.Columns[colIndex].Key.Equals(value, StringComparison.CurrentCultureIgnoreCase))
					{
						if (tempIncludeFieldHeadings && tempFieldListArray.Length == tempFieldCaptionListArray.Length)
						{
							grid.DisplayLayout.Bands[ultraGridBand.Index].Columns[colIndex].Header.Caption = tempFieldCaptionListArray[i];
							grid.DisplayLayout.Bands[ultraGridBand.Index].Columns[colIndex].PerformAutoResize();
						}
						grid.DisplayLayout.Bands[ultraGridBand.Index].Columns[colIndex].Hidden = false;
						break;
					}
				}
			}
		}
	}

	private List<DataRow> GetRecordsToExportFromGrid(UltraGrid grid, bool onlySelectedRows)
	{
		List<DataRow> list = new List<DataRow>();
		if (onlySelectedRows)
		{
			foreach (UltraGridRow row in grid.Selected.Rows)
			{
				if (row.ListObject != null)
				{
					list.Add(((DataRowView)row.ListObject).Row);
				}
			}
		}
		else if (grid.ActiveRow != null && grid.ActiveRow.ListObject != null)
		{
			list.Add(((DataRowView)grid.ActiveRow.ListObject).Row);
		}
		return list;
	}

	private void OrderGridFields(UltraGrid grid, string fieldList, string fieldCaptionList, ref string orderedFieldList, ref string orderedFieldCaptionList)
	{
		DataTable dataTable = getDataTable(grid);
		string[] array = checkFieldList(dataTable, fieldList).Split(',');
		string[] array2 = checkFieldList(dataTable, fieldCaptionList).Split(',');
		int num = array.Length;
		ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
		for (int i = 0; i < num; i++)
		{
			foreach (UltraGridColumn item in columns)
			{
				if (item.Key.Equals(array[i], StringComparison.OrdinalIgnoreCase))
				{
					if (array.Length == array2.Length)
					{
						dictionary.Add(item.Header.VisiblePosition, array2[i]);
					}
					else
					{
						dictionary.Add(item.Header.VisiblePosition, array[i]);
					}
					dictionary2.Add(item.Header.VisiblePosition, array[i]);
					break;
				}
			}
		}
		int num2 = 0;
		foreach (KeyValuePair<int, string> item2 in dictionary.OrderBy((KeyValuePair<int, string> pos) => pos.Key))
		{
			orderedFieldCaptionList += item2.Value;
			if (num2 < num - 1)
			{
				orderedFieldCaptionList += ",";
				num2++;
			}
		}
		int num3 = 0;
		foreach (KeyValuePair<int, string> item3 in dictionary2.OrderBy((KeyValuePair<int, string> pos) => pos.Key))
		{
			orderedFieldList += item3.Value;
			if (num3 < num - 1)
			{
				orderedFieldList += ",";
				num3++;
			}
		}
	}

	private void OrderGridRows(UltraGrid grid, DataTable dataTable)
	{
		dataTable.Rows.Clear();
		if (grid.Rows.IsGroupByRows)
		{
			List<UltraGridRow> list = new List<UltraGridRow>();
			GetUltraGridRows(grid.Rows, list);
			{
				foreach (UltraGridRow item in from ultraGridRow in list.OfType<UltraGridRow>()
					where !ultraGridRow.IsFilteredOut
					select ultraGridRow)
				{
					if ((DataRowView)item.ListObject == null)
					{
						continue;
					}
					DataRow row = ((DataRowView)item.ListObject).Row;
					DataRow dataRow = dataTable.NewRow();
					foreach (DataColumn column in dataTable.Columns)
					{
						dataRow[column.ColumnName] = row[column.ColumnName];
					}
					dataTable.Rows.Add(dataRow);
				}
				return;
			}
		}
		foreach (UltraGridRow item2 in from ultraGridRow in grid.Rows.OfType<UltraGridRow>()
			where !ultraGridRow.IsFilteredOut
			select ultraGridRow)
		{
			DataRow row2 = ((DataRowView)item2.ListObject).Row;
			DataRow dataRow2 = dataTable.NewRow();
			foreach (DataColumn column2 in dataTable.Columns)
			{
				dataRow2[column2.ColumnName] = row2[column2.ColumnName];
			}
			dataTable.Rows.Add(dataRow2);
		}
	}

	private void GetUltraGridRows(RowsCollection rowsCollection, List<UltraGridRow> rowList)
	{
		foreach (UltraGridRow item in rowsCollection)
		{
			if (item is UltraGridGroupByRow)
			{
				UltraGridGroupByRow ultraGridGroupByRow = (UltraGridGroupByRow)item;
				GetUltraGridRows(ultraGridGroupByRow.Rows, rowList);
			}
			else
			{
				rowList.Add(item);
			}
		}
	}

	private void SetGroupBySettigs()
	{
		if (m1Grid == null || tempGrid == null)
		{
			return;
		}
		RowsCollection rows = tempGrid.Rows;
		List<UltraGridRow> list = new List<UltraGridRow>();
		if (tempOnlySelectedRows)
		{
			foreach (UltraGridRow row in m1Grid.Selected.Rows)
			{
				list.Add(row);
			}
		}
		else
		{
			List<UltraGridRow> list2 = new List<UltraGridRow>();
			GetUltraGridRows(m1Grid.Rows, list2);
			list = (from row in list2.OfType<UltraGridRow>()
				where !row.IsFilteredOut
				select row).ToList();
		}
		if (list.Count == rows.Count)
		{
			for (int num = 0; num < rows.Count; num++)
			{
				rows[num].Appearance = list[num].Appearance;
			}
		}
		SummarySettingsCollection summaries = m1Grid.DisplayLayout.Bands[0].Summaries;
		ColumnsCollection columns = tempGrid.DisplayLayout.Bands[0].Columns;
		tempGrid.DisplayLayout.Bands[0].Summaries.Clear();
		foreach (SummarySettings item in (IEnumerable)summaries)
		{
			foreach (UltraGridColumn item2 in columns)
			{
				if (item2.Key.Equals(item.SourceColumn.Key, StringComparison.OrdinalIgnoreCase))
				{
					SummarySettings summarySettings2 = null;
					summarySettings2 = ((item.SummaryType != SummaryType.Custom) ? tempGrid.DisplayLayout.Bands[0].Summaries.Add(item.SummaryType, item2, item.SummaryPosition) : tempGrid.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Custom, item.CustomSummaryCalculator, item2, item.SummaryPosition, item.SummaryPositionColumn));
					summarySettings2.Appearance = item.Appearance;
					break;
				}
			}
		}
		tempGrid.DisplayLayout.Bands[0].Override.HeaderPlacement = HeaderPlacement.FixedOnTop;
		tempGrid.DisplayLayout.Bands[0].Override.HeaderAppearance.BackColor = m1Grid.DisplayLayout.Bands[0].Override.HeaderAppearance.BackColor;
		tempGrid.DisplayLayout.Bands[0].Override.GroupByRowAppearance.BackColor = m1Grid.DisplayLayout.Bands[0].Override.GroupByRowAppearance.BackColor;
		tempGrid.DisplayLayout.Bands[0].Override.GroupBySummaryValueAppearance.BackColor = m1Grid.DisplayLayout.Bands[0].Override.GroupBySummaryValueAppearance.BackColor;
		tempGrid.DisplayLayout.Bands[0].Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
		tempGrid.DisplayLayout.Bands[0].Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
		tempGrid.DisplayLayout.Bands[0].Override.SummaryFooterAppearance.BackColor = m1Grid.DisplayLayout.Bands[0].Override.SummaryFooterAppearance.BackColor;
		tempGrid.DisplayLayout.Bands[0].Override.SummaryFooterCaptionVisible = DefaultableBoolean.False;
		tempGrid.DisplayLayout.Bands[0].Override.GroupByColumnsHidden = DefaultableBoolean.False;
		SortedColumnsCollection sortedColumns = m1Grid.DisplayLayout.Bands[0].SortedColumns;
		tempGrid.DisplayLayout.ViewStyleBand = m1Grid.DisplayLayout.ViewStyleBand;
		foreach (UltraGridColumn item3 in (IEnumerable)sortedColumns)
		{
			if (!item3.IsGroupByColumn)
			{
				continue;
			}
			foreach (UltraGridColumn item4 in columns)
			{
				if (item3.Key.Equals(item4.Key, StringComparison.OrdinalIgnoreCase))
				{
					tempGrid.Rows.Band.SortedColumns.Add(item4, item3.SortIndicator == SortIndicator.Descending, groupBy: true);
					break;
				}
			}
		}
	}

	private UltraGrid GetFormattedTempGrid(UltraGrid tempUltraGrid)
	{
		foreach (UltraGridRow row in tempUltraGrid.Rows)
		{
			if (string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()))
			{
				row.Cells[0].Appearance.ForeColor = Color.FromArgb(40, 95, 21);
				row.Cells[0].Appearance.FontData.Bold = DefaultableBoolean.True;
			}
			else
			{
				row.Cells[0].Appearance.ForeColor = Color.FromArgb(129, 190, 247);
				row.Cells[0].Appearance.TextHAlign = HAlign.Right;
				row.Cells[0].Appearance.FontData.Bold = DefaultableBoolean.True;
			}
		}
		return tempUltraGrid;
	}

	public void Csv(DataTable data, string fileName, string separator, bool includeFieldHeadings, string fieldList)
	{
		Csv(data, fileName, separator, "\"", includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Csv(DataTable data, string fileName, string separator, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Csv(data, fileName, separator, "\"", includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Csv(DataTable data, string fileName, string separator, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Csv(data, fileName, separator, "\"", includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows);
	}

	public void Csv(DataTable data, string fileName, string separator, string quoteChar, bool includeFieldHeadings, string fieldList)
	{
		Csv(data, fileName, separator, quoteChar, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Csv(DataTable data, string fileName, string separator, string quoteChar, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Csv(data, fileName, separator, quoteChar, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Csv(DataTable data, string fileName, string separator, string quoteChar, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "csv");
		string[] array = checkFieldList(data, fieldList).Split(',');
		string[] array2 = checkFieldList(data, fieldCaptionList).Split(',');
		if (separator.Length == 0)
		{
			separator = ",";
		}
		using StreamWriter streamWriter = new StreamWriter(Path.Combine(folder, fileName), append: false);
		int num = array.Length;
		if (includeFieldHeadings)
		{
			for (int i = 0; i < num; i++)
			{
				if (array.Length == array2.Length)
				{
					streamWriter.Write(quoteChar + array2[i] + quoteChar);
				}
				else
				{
					streamWriter.Write(quoteChar + array[i] + quoteChar);
				}
				if (i < num - 1)
				{
					streamWriter.Write(separator);
				}
			}
			streamWriter.Write(streamWriter.NewLine);
		}
		DataColumn column = null;
		if (onlySelectedRows && selectedRows != null)
		{
			foreach (DataRow selectedRow in selectedRows)
			{
				writeCsvRow(streamWriter, selectedRow, num, data, column, array, quoteChar, separator);
			}
		}
		else
		{
			foreach (DataRow row in data.Rows)
			{
				writeCsvRow(streamWriter, row, num, data, column, array, quoteChar, separator);
			}
		}
		streamWriter.Close();
	}

	public void Csv(UltraGrid grid, DataTable data, string fileName, string separator, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "csv");
		if (separator.Length == 0)
		{
			separator = ",";
		}
		using StreamWriter streamWriter = new StreamWriter(Path.Combine(folder, fileName), append: false);
		string orderedFieldList = string.Empty;
		string orderedFieldCaptionList = string.Empty;
		OrderGridFields(grid, fieldList, fieldCaptionList, ref orderedFieldList, ref orderedFieldCaptionList);
		if (includeFieldHeadings)
		{
			string[] array = orderedFieldCaptionList.Split(',');
			int num = array.Length;
			int num2 = 0;
			string[] array2 = array;
			foreach (string arg in array2)
			{
				streamWriter.Write($"\"{arg}\"");
				if (num2 < num - 1)
				{
					streamWriter.Write(separator);
					num2++;
				}
			}
			streamWriter.Write(streamWriter.NewLine);
		}
		string[] array3 = orderedFieldList.Split(',');
		int iColCount = array3.Length;
		DataColumn column = null;
		if (onlySelectedRows && selectedRows != null)
		{
			foreach (DataRow selectedRow in selectedRows)
			{
				writeCsvRow(streamWriter, selectedRow, iColCount, data, column, array3, "\"", separator);
			}
		}
		else if (grid.Rows.IsGroupByRows)
		{
			List<UltraGridRow> list = new List<UltraGridRow>();
			GetUltraGridRows(grid.Rows, list);
			foreach (UltraGridRow item in from row in list.OfType<UltraGridRow>()
				where !row.IsFilteredOut
				select row)
			{
				if ((DataRowView)item.ListObject != null)
				{
					DataRowView dataRowView = (DataRowView)item.ListObject;
					writeCsvRow(streamWriter, dataRowView.Row, iColCount, data, column, array3, "\"", separator);
				}
			}
		}
		else
		{
			foreach (UltraGridRow item2 in from row in grid.Rows.OfType<UltraGridRow>()
				where !row.IsFilteredOut
				select row)
			{
				DataRowView dataRowView2 = (DataRowView)item2.ListObject;
				writeCsvRow(streamWriter, dataRowView2.Row, iColCount, data, column, array3, "\"", separator);
			}
		}
		streamWriter.Close();
	}

	public void Csv(M1BindingSource data, string fileName, string separator, bool includeFieldHeadings, string fieldList)
	{
		Csv(data.GetDataTable(), fileName, separator, "\"", includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Csv(M1BindingSource data, string fileName, string separator, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Csv(data.GetDataTable(), fileName, separator, "\"", includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Csv(M1BindingSource data, string fileName, string separator, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Csv(data.GetDataTable(), fileName, separator, "\"", includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows);
	}

	public void Csv(M1BindingSource data, string fileName, string separator, string quoteChar, bool includeFieldHeadings, string fieldList)
	{
		Csv(data.GetDataTable(), fileName, separator, quoteChar, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Csv(M1BindingSource data, string fileName, string separator, string quoteChar, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Csv(data.GetDataTable(), fileName, separator, quoteChar, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Csv(M1BindingSource data, string fileName, string separator, string quoteChar, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Csv(data.GetDataTable(), fileName, separator, quoteChar, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows);
	}

	public void Html(DataTable data, string fileName, string fieldList)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "html");
		if (!string.IsNullOrEmpty(fileName) & !string.IsNullOrEmpty(folder))
		{
			string path = Path.Combine(folder, fileName);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using FileStream fileStream = File.Create(path);
			byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(DatatableToHtml(data, "BORDER=1", ShowFieldName: true, IncludeWhiteSpace: true));
			fileStream.Write(bytes, 0, bytes.Length);
		}
	}

	private string DatatableToHtml(DataTable dtTable, string sTableAttribute, bool ShowFieldName, bool IncludeWhiteSpace, string NullValues = "&nbsp;")
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = "<TABLE " + sTableAttribute + ">" + text;
		string text4 = string.Empty;
		if (IncludeWhiteSpace)
		{
			text = Environment.NewLine;
			text2 = "\t";
		}
		if (ShowFieldName)
		{
			text3 = text3 + text2 + "<HEAD>" + text;
			if (dtTable.Columns.Count > 0)
			{
				foreach (string item in (from DataColumn col in dtTable.Columns
					select col.ColumnName).ToList())
				{
					text3 = text3 + text2 + text2 + "<TD><B>" + item.Trim() + "</B></TD>" + text;
				}
			}
		}
		foreach (DataRow row in dtTable.Rows)
		{
			for (int num = 0; num < dtTable.Columns.Count; num++)
			{
				text4 = ((!string.IsNullOrEmpty(row[num].ToString())) ? (text4 + row[num].ToString().Trim() + "</TD>" + text + text2 + text2 + "<TD>") : (text4 + NullValues + "</TD>" + text + text2 + text2 + "<TD>"));
			}
			text4 = text4 + "</TD>" + text + text2 + "</TR>" + text + text2 + "<TR>" + text + text2 + text2 + "<TD>";
		}
		text4 = text4.Substring(0, text4.Length - (text + text2 + "<TR>" + text + text2 + text2 + "<TD>").Length);
		return text3 + text2 + "<TR>" + text + text2 + text2 + "<TD>" + text4 + text + "</TABLE>";
	}

	public void Dbf(DataTable data, string fileName, string fieldList)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "dbf");
		fileName = fileName.Replace(' ', '_');
		StringBuilder fieldData = new StringBuilder();
		fieldList = fieldList.Replace(" ", "");
		if (fieldList.Length == 0)
		{
			return;
		}
		string[] array = fieldList.Split(',');
		foreach (string text in array)
		{
			if (text.Length != 0)
			{
				getDbfTypeForColumn(data.Columns[text], fieldData);
			}
		}
	}

	public void Excel(DataTable data, string fileName, bool includeFieldHeadings, string fieldList)
	{
		Excel(data, fileName, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Excel(DataTable data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Excel(data, fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Excel(DataTable data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "xls");
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(data, fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(data, fieldCaptionList).Split(',');
		tempGrid = getUltraGridToExport(data, tempFieldListArray, tempFieldCaptionListArray, includeFieldHeadings, onlySelectedRows, selectedRows);
		tempOnlySelectedRows = onlySelectedRows;
		if (m1Grid != null && m1Grid.Rows.IsGroupByRows)
		{
			SetGroupBySettigs();
		}
		excel(fileName, folder);
	}

	public void Excel(DataTable data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, bool isScheduleBoardExport)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "xls");
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(data, fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(data, fieldCaptionList).Split(',');
		tempGrid = getUltraGridToExport(data, tempFieldListArray, tempFieldCaptionListArray, includeFieldHeadings, onlySelectedRows, selectedRows);
		if (isScheduleBoardExport)
		{
			tempGrid = GetFormattedTempGrid(tempGrid);
		}
		tempOnlySelectedRows = onlySelectedRows;
		if (m1Grid != null && m1Grid.Rows.IsGroupByRows)
		{
			SetGroupBySettigs();
		}
		excel(fileName, folder);
	}

	public void Excel(M1BindingSource data, string fileName, bool includeFieldHeadings, string fieldList)
	{
		Excel(data.GetDataTable(), fileName, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Excel(M1BindingSource data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Excel(data.GetDataTable(), fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Excel(M1BindingSource data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Excel(data.GetDataTable(), fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows);
	}

	public void Excel(UltraGrid grid, string fileName, bool includeFieldHeadings, string fieldList)
	{
		Excel(grid, fileName, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false);
	}

	public void Excel(UltraGrid grid, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Excel(grid, fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false);
	}

	public void Excel(UltraGrid grid, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows)
	{
		DataTable dataTable = getDataTable(grid);
		List<DataRow> recordsToExportFromGrid = GetRecordsToExportFromGrid(grid, onlySelectedRows);
		m1Grid = grid;
		string orderedFieldList = string.Empty;
		string orderedFieldCaptionList = string.Empty;
		OrderGridFields(grid, fieldList, fieldCaptionList, ref orderedFieldList, ref orderedFieldCaptionList);
		OrderGridRows(grid, dataTable);
		Excel(dataTable, fileName, includeFieldHeadings, orderedFieldList, orderedFieldCaptionList, onlySelectedRows, recordsToExportFromGrid);
	}

	private void excel(string fileName, string folder)
	{
		using UltraGridExcelExporter ultraGridExcelExporter = new UltraGridExcelExporter();
		ultraGridExcelExporter.ExportStarted += UltraGridExcelExporter1_ExportStarted;
		ultraGridExcelExporter.InitializeRow += ultraGridExcelExporter1_InitializeRow;
		ultraGridExcelExporter.Export(tempGrid, Path.Combine(folder, fileName));
		ultraGridExcelExporter.ExportStarted -= UltraGridExcelExporter1_ExportStarted;
		ultraGridExcelExporter.InitializeRow -= ultraGridExcelExporter1_InitializeRow;
	}

	public void Pdf(DataTable data, string fileName, bool includeFieldHeadings, string fieldList)
	{
		Pdf(data, fileName, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Pdf(DataTable data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Pdf(data, fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Pdf(DataTable data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "pdf");
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(data, fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(data, fieldCaptionList).Split(',');
		tempGrid = getUltraGridToExport(data, tempFieldListArray, tempFieldCaptionListArray, includeFieldHeadings, onlySelectedRows, selectedRows);
		tempOnlySelectedRows = onlySelectedRows;
		if (m1Grid != null && m1Grid.Rows.IsGroupByRows)
		{
			SetGroupBySettigs();
		}
		pdf(fileName, folder);
	}

	public void Pdf(DataTable data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, bool isScheduleBoardExport)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "pdf");
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(data, fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(data, fieldCaptionList).Split(',');
		tempGrid = getUltraGridToExport(data, tempFieldListArray, tempFieldCaptionListArray, includeFieldHeadings, onlySelectedRows, selectedRows);
		if (isScheduleBoardExport)
		{
			tempGrid = GetFormattedTempGrid(tempGrid);
		}
		tempOnlySelectedRows = onlySelectedRows;
		if (m1Grid != null && m1Grid.Rows.IsGroupByRows)
		{
			SetGroupBySettigs();
		}
		pdf(fileName, folder);
	}

	public void Pdf(M1BindingSource data, string fileName, bool includeFieldHeadings, string fieldList)
	{
		Pdf(data.GetDataTable(), fileName, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null);
	}

	public void Pdf(M1BindingSource data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Pdf(data.GetDataTable(), fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null);
	}

	public void Pdf(M1BindingSource data, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Pdf(data.GetDataTable(), fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows);
	}

	public void Pdf(UltraGrid grid, string fileName, bool includeFieldHeadings, string fieldList)
	{
		Pdf(grid, fileName, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false);
	}

	public void Pdf(UltraGrid grid, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Pdf(grid, fileName, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false);
	}

	public void Pdf(UltraGrid grid, string fileName, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows)
	{
		DataTable dataTable = getDataTable(grid);
		List<DataRow> recordsToExportFromGrid = GetRecordsToExportFromGrid(grid, onlySelectedRows);
		m1Grid = grid;
		string orderedFieldList = string.Empty;
		string orderedFieldCaptionList = string.Empty;
		OrderGridFields(grid, fieldList, fieldCaptionList, ref orderedFieldList, ref orderedFieldCaptionList);
		OrderGridRows(grid, dataTable);
		Pdf(dataTable, fileName, includeFieldHeadings, orderedFieldList, orderedFieldCaptionList, onlySelectedRows, recordsToExportFromGrid);
	}

	private void pdf(string fileName, string folder)
	{
		exportByPortions(ref tempGrid, Path.Combine(folder, fileName), FileFormat.PDF, 100);
	}

	public void Xml(DataTable data, string fileName, string fieldList)
	{
		Xml(data, fileName, fieldList, string.Empty, onlySelectedRows: false, null, string.Empty);
	}

	public void Xml(DataTable data, string fileName, string fieldList, string fieldCaptionList)
	{
		Xml(data, fileName, fieldList, fieldCaptionList, onlySelectedRows: false, null, string.Empty);
	}

	public void Xml(DataTable data, string fileName, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Xml(data, fileName, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, string.Empty);
	}

	public void Xml(DataTable data, string fileName, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, string exportTableName)
	{
		string folder = string.Empty;
		doCheck(data, ref fileName, ref folder, "xml");
		string[] array = checkFieldList(data, fieldList).Split(',');
		string[] array2 = checkFieldList(data, fieldCaptionList).Split(',');
		if (data.TableName.Length != 0)
		{
			exportTableName = data.TableName;
		}
		if (exportTableName.Length == 0)
		{
			exportTableName = "M1EXPORT";
		}
		DataTable dataTableToExport = getDataTableToExport(data, array, array2, includeFieldHeadings: true, onlySelectedRows, selectedRows);
		dataTableToExport.TableName = exportTableName;
		if (array.Length == array2.Length)
		{
			foreach (DataColumn column in dataTableToExport.Columns)
			{
				if (column.Caption.Length != 0)
				{
					column.ColumnName = getValidColumnName(column.Caption);
				}
			}
		}
		dataTableToExport.WriteXml(Path.Combine(folder, fileName));
	}

	public void Xml(UltraGrid grid, string fileName, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, string exportTableName)
	{
		DataTable dataTable = getDataTable(grid);
		string folder = string.Empty;
		doCheck(dataTable, ref fileName, ref folder, "xml");
		if (dataTable.TableName.Length != 0)
		{
			exportTableName = dataTable.TableName;
		}
		if (exportTableName.Length == 0)
		{
			exportTableName = "M1EXPORT";
		}
		OrderGridRows(grid, dataTable);
		string orderedFieldList = string.Empty;
		string orderedFieldCaptionList = string.Empty;
		OrderGridFields(grid, fieldList, fieldCaptionList, ref orderedFieldList, ref orderedFieldCaptionList);
		string[] array = orderedFieldList.Split(',');
		string[] array2 = orderedFieldCaptionList.Split(',');
		DataTable dataTableToExport = getDataTableToExport(dataTable, array, array2, includeFieldHeadings: true, onlySelectedRows, selectedRows);
		dataTableToExport.TableName = exportTableName;
		if (array.Length == array2.Length)
		{
			string empty = string.Empty;
			bool flag = false;
			int num = 1;
			foreach (DataColumn column in dataTableToExport.Columns)
			{
				if (column.Caption.Length == 0)
				{
					continue;
				}
				empty = getValidColumnName(column.Caption);
				if (empty.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				while (!flag)
				{
					if (!dataTableToExport.Columns.Contains(empty))
					{
						flag = true;
						continue;
					}
					empty = getValidColumnName($"{column.Caption}_{num}");
					num++;
				}
				column.ColumnName = empty;
				flag = false;
				num = 1;
			}
		}
		dataTableToExport.WriteXml(Path.Combine(folder, fileName));
	}

	public void Xml(M1BindingSource data, string fileName, string fieldList)
	{
		Xml(data.GetDataTable(), fileName, fieldList, string.Empty, onlySelectedRows: false, null, string.Empty);
	}

	public void Xml(M1BindingSource data, string fileName, string fieldList, string fieldCaptionList)
	{
		Xml(data.GetDataTable(), fileName, fieldList, fieldCaptionList, onlySelectedRows: false, null, string.Empty);
	}

	public void Xml(M1BindingSource data, string fileName, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Xml(data.GetDataTable(), fileName, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, string.Empty);
	}

	public void Xml(M1BindingSource data, string fileName, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, string exportTableName)
	{
		Xml(data.GetDataTable(), fileName, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, exportTableName);
	}

	public void PrintPreview(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList)
	{
		PrintPreview(data, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void PrintPreview(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(data, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		PrintPreview(data, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void PrintPreview(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(data, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		PrintPreview(data, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, scalePrint: false, 0);
	}

	public void PrintPreview(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, bool scalePrint, int scaleWidthToPage)
	{
		doDataCheck(data);
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(data, fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(data, fieldCaptionList).Split(',');
		tempGrid = getUltraGridToExport(data, tempFieldListArray, tempFieldCaptionListArray, includeFieldHeadings, onlySelectedRows, selectedRows);
		tempOnlySelectedRows = false;
		tempScalePrint = scalePrint;
		tempScaleWidthToPage = scaleWidthToPage;
		printPreview(printDocument);
	}

	public void PrintPreview(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList)
	{
		PrintPreview(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void PrintPreview(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		PrintPreview(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void PrintPreview(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		PrintPreview(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, scalePrint: false, 0);
	}

	public void PrintPreview(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList)
	{
		PrintPreview(grid, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, scalePrint: false, 0);
	}

	public void PrintPreview(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(grid, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		PrintPreview(grid, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, scalePrint: false, 0);
	}

	public void PrintPreview(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool scalePrint, int scaleWidthToPage)
	{
		PrintPreview(grid, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, scalePrint, scaleWidthToPage);
	}

	public void PrintPreview(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows)
	{
		PrintPreview(grid, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, scalePrint: false, 0);
	}

	public void PrintPreview(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, bool scalePrint, int scaleWidthToPage)
	{
		tempGrid = grid;
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(getDataTable(grid), fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(getDataTable(grid), fieldCaptionList).Split(',');
		tempOnlySelectedRows = onlySelectedRows;
		tempScalePrint = scalePrint;
		tempScaleWidthToPage = scaleWidthToPage;
		printPreview(printDocument);
	}

	private void printPreview(PrintDocument printDocument)
	{
		tempGrid.InitializePrintPreview += grid_InitializePrintPreview;
		tempGrid.InitializeRow += grid_InitializeRow;
		if (printDocument == null)
		{
			printDocument = new PrintDocument();
		}
		tempGrid.PrintPreview(tempGrid.DisplayLayout, printDocument, RowPropertyCategories.All);
		tempGrid.InitializePrintPreview -= grid_InitializePrintPreview;
		tempGrid.InitializeRow -= grid_InitializeRow;
	}

	public void Print(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList)
	{
		Print(data, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void Print(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, bool scalePrint, int scaleWidthToPage)
	{
		Print(data, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void Print(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Print(data, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void Print(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool scalePrint, int scaleWidthToPage)
	{
		Print(data, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void Print(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Print(data, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, scalePrint: false, 0);
	}

	public void Print(DataTable data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, bool scalePrint, int scaleWidthToPage)
	{
		doDataCheck(data);
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(data, fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(data, fieldCaptionList).Split(',');
		tempGrid = getUltraGridToExport(data, tempFieldListArray, tempFieldCaptionListArray, includeFieldHeadings, onlySelectedRows, selectedRows);
		tempOnlySelectedRows = false;
		tempScalePrint = scalePrint;
		tempScaleWidthToPage = scaleWidthToPage;
		print(printDocument);
	}

	public void Print(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList)
	{
		Print(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void Print(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, bool scalePrint, int scaleWidthToPage)
	{
		Print(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void Print(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Print(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint: false, 0);
	}

	public void Print(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool scalePrint, int scaleWidthToPage)
	{
		Print(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, null, scalePrint, scaleWidthToPage);
	}

	public void Print(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows)
	{
		Print(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, scalePrint: false, 0);
	}

	public void Print(M1BindingSource data, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, List<DataRow> selectedRows, bool scalePrint, int scaleWidthToPage)
	{
		Print(data.GetDataTable(), printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, selectedRows, scalePrint, scaleWidthToPage);
	}

	public void Print(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList)
	{
		Print(grid, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, scalePrint: false, 0);
	}

	public void Print(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, bool scalePrint, int scaleWidthToPage)
	{
		Print(grid, printDocument, includeFieldHeadings, fieldList, string.Empty, onlySelectedRows: false, scalePrint, scaleWidthToPage);
	}

	public void Print(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList)
	{
		Print(grid, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, scalePrint: false, 0);
	}

	public void Print(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool scalePrint, int scaleWidthToPage)
	{
		Print(grid, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows: false, scalePrint, scaleWidthToPage);
	}

	public void Print(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows)
	{
		Print(grid, printDocument, includeFieldHeadings, fieldList, fieldCaptionList, onlySelectedRows, scalePrint: false, 0);
	}

	public void Print(UltraGrid grid, PrintDocument printDocument, bool includeFieldHeadings, string fieldList, string fieldCaptionList, bool onlySelectedRows, bool scalePrint, int scaleWidthToPage)
	{
		tempGrid = grid;
		tempIncludeFieldHeadings = includeFieldHeadings;
		tempFieldListArray = checkFieldList(getDataTable(grid), fieldList).Split(',');
		tempFieldCaptionListArray = checkFieldList(getDataTable(grid), fieldCaptionList).Split(',');
		tempOnlySelectedRows = onlySelectedRows;
		tempScalePrint = scalePrint;
		tempScaleWidthToPage = scaleWidthToPage;
		print(printDocument);
	}

	private void print(PrintDocument printDocument)
	{
		tempGrid.InitializePrint += grid_InitializePrint;
		tempGrid.InitializeRow += grid_InitializeRow;
		if (printDocument == null)
		{
			printDocument = new PrintDocument();
		}
		tempGrid.Print(tempGrid.DisplayLayout, printDocument, RowPropertyCategories.All);
		tempGrid.InitializePrint -= grid_InitializePrint;
		tempGrid.InitializeRow -= grid_InitializeRow;
	}

	private void ultraGridExcelExporter1_InitializeRow(object sender, ExcelExportInitializeRowEventArgs e)
	{
		SetRowAppearance(e.Row);
	}

	private void docExporter_InitializeRow(object sender, DocumentExportInitializeRowEventArgs e)
	{
		SetRowAppearance(e.Row);
	}

	private void SetRowAppearance(UltraGridRow ultraGridRow)
	{
		if (m1Grid != null && !m1Grid.Rows.IsGroupByRows)
		{
			UltraGridRow ultraGridRow2 = null;
			ultraGridRow2 = ((!tempOnlySelectedRows) ? (from gridRow in m1Grid.Rows.OfType<UltraGridRow>()
				where !gridRow.IsFilteredOut
				select gridRow).ElementAt(ultraGridRow.Index) : ((UltraGridRow)m1Grid.Selected.Rows.GetItem(ultraGridRow.Index)));
			ultraGridRow.Appearance = ultraGridRow2.Appearance;
		}
	}

	private void grid_InitializeRow(object sender, InitializeRowEventArgs e)
	{
		e.Row.Hidden = !shouldRowBeExported(e.Row, e.Row.Band.Layout.IsPrintLayout);
	}

	private void grid_InitializePrintPreview(object sender, CancelablePrintPreviewEventArgs e)
	{
		e.PrintLayout.Bands[0].ColHeadersVisible = tempIncludeFieldHeadings;
		if (tempScalePrint && tempScaleWidthToPage > 0)
		{
			e.DefaultLogicalPageLayoutInfo.FitWidthToPages = tempScaleWidthToPage;
		}
		setupColumnsExportLayout(e.PrintLayout.Bands[0].Columns);
	}

	private void grid_InitializePrint(object sender, CancelablePrintEventArgs e)
	{
		e.PrintLayout.Bands[0].ColHeadersVisible = tempIncludeFieldHeadings;
		if (tempScalePrint && tempScaleWidthToPage > 0)
		{
			e.DefaultLogicalPageLayoutInfo.FitWidthToPages = tempScaleWidthToPage;
		}
		setupColumnsExportLayout(e.PrintLayout.Bands[0].Columns);
	}
}
