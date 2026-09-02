using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.200", "Update Additional Cost Price on Quote Quantities Table when upgrading from V8", "2023-08-17")]
public class v96200e
{
	public v96200e(DBConversionParms parms)
	{
		string initialVersion = parms.InitialVersion;
		if ("8.00.718".CompareTo(initialVersion) == -1)
		{
			return;
		}
		M1Database database = parms.Database;
		string queryString = "SELECT qmqQuoteID, qmqQuoteLineID, qmqQuoteQuantityID, qmqQuoteMarkupType, qmqAdditionalCostAmount, qmqAdditionalMarkupPercent FROM QuoteQuantities";
		database.ExecuteCommand(queryString);
		foreach (DataRow row in database.GetDataTable(queryString).Rows)
		{
			if (row.Field<decimal>("qmqAdditionalCostAmount") != 0m)
			{
				string value = row.Field<string>("qmqQuoteID");
				short num = row.Field<short>("qmqQuoteLineID");
				byte b = row.Field<byte>("qmqQuoteQuantityID");
				byte markupType = row.Field<byte>("qmqQuoteMarkupType");
				decimal cost = row.Field<decimal>("qmqAdditionalCostAmount");
				decimal markupPercent = row.Field<decimal>("qmqAdditionalMarkupPercent");
				decimal num2 = M1Math.CalculateMarkup(markupType, cost, markupPercent, 5);
				SqlCommand sqlCommand = database.NewSqlCommand("UPDATE QuoteQuantities SET qmqAdditionalCostPrice = @AdditionalCostPrice WHERE qmqQuoteID = @QuoteId AND qmqQuoteLineID = @QuoteLineId AND qmqQuoteQuantityID = @QuoteQuantityId");
				sqlCommand.Parameters.Add(new SqlParameter("@QuoteId", SqlDbType.NVarChar)).Value = value;
				sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineId", SqlDbType.SmallInt)).Value = num;
				sqlCommand.Parameters.Add(new SqlParameter("@QuoteQuantityId", SqlDbType.TinyInt)).Value = b;
				sqlCommand.Parameters.Add(new SqlParameter("@AdditionalCostPrice", SqlDbType.Decimal)).Value = num2;
				database.ExecuteCommand(sqlCommand);
			}
		}
	}
}
