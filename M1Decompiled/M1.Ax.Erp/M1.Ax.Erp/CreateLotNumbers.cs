using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class CreateLotNumbers : CreateSerialLotNumberBase
{
	private Action<DataTable, M1Database, DataRow> _OnSuccessAction;

	private SqlDataAdapter lotAdapter;

	private DataRow currentRow;

	public CreateLotNumbers()
		: base('L')
	{
	}

	public void Load(FieldDefinition partBinField, M1Database database, DataRow row, Action<DataTable, M1Database, DataRow> onSuccessAction)
	{
		currentRow = row;
		_OnSuccessAction = onSuccessAction;
		base.Load(partBinField, database, row);
		NumbersTable = database.GetDataTable("Select * From LotNumbers Where 0=1", fillSchema: false, out lotAdapter);
	}

	public override void Add(M1User user, string lotNumberID, DateTime? expirationDate)
	{
		base.Add(user, lotNumberID, expirationDate);
	}

	public bool SaveToDb(M1User user, M1Database database)
	{
		if (base.IsDataValid(database))
		{
			SqlTransaction sqlTransaction = database.BeginTransaction();
			try
			{
				if (database.UpdateData(NumbersTable, lotAdapter, sqlTransaction))
				{
					CreateStatusAndTransactionRecords(user, database, sqlTransaction);
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
