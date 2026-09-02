using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class LotNumber
{
	public bool IsLotTracked(M1Database database, string partID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Case When impTrackLotNumbers = 1 Then Convert(bit,1) Else Convert(bit,0) End,0) From Parts Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand));
	}

	public bool IsLotInactive(M1Database database, SqlTransaction transaction, string partID, string revisionID, string lotNumberID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT ablInactive FROM LotNumbers WHERE ablPartID = @PartID AND ablPartRevisionID = @PartRevisionID AND ablLotNumberID = @LotNumberID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@LotNumberID", SqlDbType.NVarChar)).Value = lotNumberID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsLotUnassigned(M1Database database, SqlTransaction transaction, string partID, string revisionID, string lotNumberID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Case When IsNull(Count(absQuantity),0) = 0 Then 1 Else 0 End FROM LotNumberStatuses WHERE absPartID = @PartID AND absPartRevisionID = @PartRevisionID AND absLotNumberID = @LotNumberID AND absQuantity <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@LotNumberID", SqlDbType.NVarChar)).Value = lotNumberID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public void RefreshLotNumberStatuses(M1Database database, SqlTransaction transaction, string partID, string partRevisionID, string lotNumberID)
	{
		if (!string.IsNullOrWhiteSpace(lotNumberID))
		{
			LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, lotNumberID);
			lotNumberDefinition.RefreshStatuses(database, transaction, partID, partRevisionID, lotNumberID);
		}
	}

	public void DeleteLotTransactions(M1Database database, SqlTransaction transaction, string where)
	{
		if (string.IsNullOrEmpty(where))
		{
			return;
		}
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("Select abtLotNumberID,abtPartID,abtPartRevisionID From LotNumberTransactions Where " + where, fillSchema: false, out adapter, transaction);
		database.ExecuteCommand("Delete LotNumberTransactions Where " + where, transaction);
		foreach (DataRow row in dataTable.Rows)
		{
			RefreshLotNumberStatuses(database, transaction, row.Field<string>("abtPartID"), row.Field<string>("abtPartRevisionID"), row.Field<string>("abtLotNumberID"));
		}
	}
}
