using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class SerialNumber
{
	public void DeleteSerialTransactions(M1Database database, SqlTransaction transaction, string where)
	{
		if (string.IsNullOrEmpty(where))
		{
			return;
		}
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("Select sntSerialNumberID,sntPartID,sntPartRevisionID From SerialNumberTransactions Where " + where, fillSchema: false, out adapter, transaction);
		database.ExecuteCommand("Delete SerialNumberTransactions Where " + where, transaction);
		foreach (DataRow row in dataTable.Rows)
		{
			RefreshSerialNumberStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
		}
	}

	public void RefreshSerialNumberStatuses(M1Database database, SqlTransaction transaction, string partID, string partRevisionID, string serialNumberID)
	{
		if (!string.IsNullOrWhiteSpace(serialNumberID))
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, serialNumberID);
			serialNumberDefinition.RefreshStatuses(database, transaction, partID, partRevisionID, serialNumberID);
		}
	}

	public bool IsSerialTracked(M1Database database, string partID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Case When impTrackSerialNumbers = 1 Then Convert(bit,1) Else Convert(bit,0) End,0) From Parts Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand));
	}

	public byte GetCurrentStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string serialNumberID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT ISNULL((SELECT Top 1 IsNull(snsStatus,0) FROM SerialNumberStatuses WHERE snsPartID = @PartID AND snsPartRevisionID = @PartRevisionID AND snsSerialNumberID = @SerialNumberID AND snsQuantity = 1 Order by snsCreatedDate Desc), 0)");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@SerialNumberID", SqlDbType.NVarChar)).Value = serialNumberID;
		return Convert.ToByte(database.ExecuteScalar(sqlCommand, transaction));
	}
}
