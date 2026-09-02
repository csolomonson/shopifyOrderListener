using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Ax.Erp;

public class CreateSerialNumbers : CreateSerialLotNumberBase
{
	private ScriptingBase scriptEngine;

	private Action<DataTable, M1Database, DataRow> _OnSuccessAction;

	public string PartGroupID = string.Empty;

	public bool NumberPerGroup;

	public string Formula = string.Empty;

	public decimal Status;

	public int NextNumber = 1;

	private SqlDataAdapter serialAdapter;

	private DataRow currentRow;

	public CreateSerialNumbers()
		: base('S')
	{
	}

	public void Load(FieldDefinition partBinField, M1Database database, DataRow row, Action<DataTable, M1Database, DataRow> onSuccessAction)
	{
		currentRow = row;
		_OnSuccessAction = onSuccessAction;
		base.Load(partBinField, database, row);
		object obj = database.ExecuteScalar("Select xanNextID From NextIDs Where xanTable = 'SERIALNUMBERS'");
		if (obj != null && !int.TryParse(obj.ToString(), out NextNumber))
		{
			NextNumber = 1;
		}
		DataTable dataTable = database.GetDataTable($"select impNextSerialNumberIDFormula,imuNextSerialNumberIDFormula,imuNextSerialNumberOption,imuNextSerialNumberValue,impPartGroupID from Parts Left Outer Join PartGroups On impPartGroupID = imuPartGroupID Where impPartID = {PartID.ToSql()}");
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			PartGroupID = dataRow.Field<string>("impPartGroupID").Trim();
			Formula = dataRow.Field<string>("impNextSerialNumberIDFormula");
			if (string.IsNullOrWhiteSpace(Formula) && dataRow["imuNextSerialNumberIDFormula"] != DBNull.Value)
			{
				Formula = dataRow.Field<string>("imuNextSerialNumberIDFormula");
			}
			if (dataRow["imuNextSerialNumberOption"] != DBNull.Value)
			{
				if (dataRow.Field<byte>("imuNextSerialNumberOption") == 2)
				{
					NumberPerGroup = true;
				}
				else if (dataRow.Field<byte>("imuNextSerialNumberOption") == 0 && database.Props("PN").Field<bool>("xapNextSerialNumberPerGroup"))
				{
					NumberPerGroup = true;
				}
			}
			if (NumberPerGroup)
			{
				int.TryParse(dataRow.Field<string>("imuNextSerialNumberValue").Trim(), out NextNumber);
			}
		}
		if (NextNumber <= 1)
		{
			NextNumber = 1;
		}
		if (string.IsNullOrWhiteSpace(Formula))
		{
			Formula = database.Props("PN").Field<string>("xapNextSerialNumberIDFormula");
		}
		NumbersTable = database.GetDataTable("Select * From SerialNumbers Where 0=1", fillSchema: false, out serialAdapter);
	}

	private void loadScriptControl(M1Database database)
	{
		if (scriptEngine == null)
		{
			scriptEngine = new ScriptingBase(database);
		}
		scriptEngine.LoadEnvironment(useConnectionProxy: true);
		scriptEngine.AddCode("Dim formula\r\nDim NextNumber\r\nDim CurrentYear\r\nDim CurrentMonth\r\nDim CurrentDay\r\nDim PartID\r\nDim PartRevisionID\r\nDim PartGroupID\r\n");
		scriptEngine.ExecuteStatement($"NextNumber = \"0\"\r\nCurrentYear = {DateTime.Now.Year.ToString().ToScript()} \r\nCurrentMonth = {DateTime.Now.Month.ToString().PadLeft(2, '0').ToScript()}\r\nCurrentDay = {DateTime.Now.Day.ToString().PadLeft(2, '0').ToScript()}\r\nPartID = {PartID.ToScript()}\r\nPartRevisionID = {PartRevisionID.ToScript()}\r\nPartGroupID = {PartGroupID.ToScript()} \r\n");
	}

	private void evaluateCode(string codeToExecute, ref string newID)
	{
		if (codeToExecute.Length == 0)
		{
			return;
		}
		scriptEngine.ExecuteStatement($"formula = Empty\r\nNextNumber = {newID.ToScript()}\r\n");
		try
		{
			scriptEngine.ExecuteStatement(codeToExecute);
		}
		catch
		{
			throw new M1MissingOrInvalidDataException("Invalid serial number formula - please verify before trying again.");
		}
		object obj2 = scriptEngine.Eval("formula");
		if (!M1Util.IsNullOrEmpty(obj2))
		{
			string text = obj2.ToString();
			if (text.Length > 30)
			{
				throw new M1MissingOrInvalidDataException("The return value for the serial number id is larger then the size of the field.");
			}
			newID = text;
		}
	}

	public void Generate(int startingValue, int quantityToGenerate, M1User user, M1Database database)
	{
		NextNumber = startingValue;
		if (quantityToGenerate > 0)
		{
			if (!string.IsNullOrWhiteSpace(Formula))
			{
				loadScriptControl(database);
			}
			string empty = string.Empty;
			for (int i = 1; i <= quantityToGenerate; i++)
			{
				empty = NextNumber.ToString();
				if (!string.IsNullOrWhiteSpace(Formula))
				{
					evaluateCode(Formula, ref empty);
				}
				NextNumber++;
				Add(user, empty, ExpirationDate);
			}
		}
		else if (quantityToGenerate < 0)
		{
			for (int num = -1; num >= quantityToGenerate; num--)
			{
				NumbersTable.Rows.RemoveAt(NumbersTable.Rows.Count - 1);
				NextNumber--;
			}
		}
	}

	private void saveNextNumber(M1Database database, SqlTransaction sqlTransaction)
	{
		if (NumberPerGroup && PartGroupID.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Update PartGroups Set imuNextSerialNumberValue = @NewValue Where imuPartGroupID = @PartGroupID");
			sqlCommand.Parameters.Add(new SqlParameter("@NewValue", SqlDbType.NVarChar)).Value = NextNumber.ToString();
			sqlCommand.Parameters.Add(new SqlParameter("@PartGroupID", SqlDbType.NVarChar)).Value = PartGroupID;
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			return;
		}
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("select * from NextIDs Where xanTable = 'SERIALNUMBERS'", fillSchema: false, out adapter, sqlTransaction);
		DataRow row;
		if (dataTable.Rows.Count == 0)
		{
			row = dataTable.AddBlankRow();
			row.SetField("xanTable", "SERIALNUMBERS");
			row.SetField("xanNumericOnly", (byte)2);
		}
		else
		{
			row = dataTable.Rows[0];
		}
		row.SetField("xanNextID", NextNumber.ToString());
		database.UpdateData(dataTable, adapter, sqlTransaction);
	}

	public bool SaveToDb(M1User user, M1Database database)
	{
		if (base.IsDataValid(database))
		{
			SqlTransaction sqlTransaction = database.BeginTransaction();
			try
			{
				if (database.UpdateData(NumbersTable, serialAdapter, sqlTransaction))
				{
					CreateStatusAndTransactionRecords(user, database, sqlTransaction);
					saveNextNumber(database, sqlTransaction);
					database.CommitTransaction(sqlTransaction);
					if (_OnSuccessAction != null)
					{
						_OnSuccessAction(NumbersTable, database, currentRow);
					}
					return true;
				}
				database.RollbackTransaction(sqlTransaction);
			}
			catch
			{
				database.RollbackTransaction(sqlTransaction);
				throw;
			}
		}
		return false;
	}
}
