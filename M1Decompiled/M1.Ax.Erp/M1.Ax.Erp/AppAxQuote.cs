using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Quote")]
[ComVisible(true)]
public class AppAxQuote : IDisposable
{
	private IServiceProvider provider;

	private M1Database database;

	private Quote quoteFunc;

	private Quote getRef()
	{
		if (quoteFunc == null)
		{
			quoteFunc = new Quote();
		}
		return quoteFunc;
	}

	public AppAxQuote(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void DeleteQuoteAssembly(object transaction, string quoteID, int lineID, int asmID)
	{
		getRef().DeleteQuoteAssembly(database, (SqlTransaction)transaction, quoteID, lineID, asmID);
	}

	public int TransferPrices(string quoteID, int line, bool expireExisting)
	{
		return getRef().TransferPrices(database, quoteID, line, expireExisting);
	}

	public void SetQuantities(object bindingSource)
	{
		getRef().SetQuantities((M1BindingSource)bindingSource);
	}

	public void RefreshMatrix(string whereClause)
	{
		getRef().RefreshMatrix(database, whereClause);
	}

	public string CreateQuote(string customerID, string locationID, string currencyID, string partID, string revisionID)
	{
		return getRef().CreateQuote(database, customerID, locationID, currencyID, partID, revisionID);
	}

	public void CalculateQuotePriceBreakFromQuantity(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		new Quote().GetPriceForQuantity(e2.Database, e2.Row);
	}

	public void UpdateFieldsInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new Quote().UpdateFieldsInGrid(e2.Database, e2.Row, fieldDefinition.FieldName);
	}

	public void CloseRelatedFollowups(SqlTransaction transaction, string quoteID, string OrderID)
	{
		getRef().CloseRelatedFollowups(database, transaction, quoteID, OrderID);
	}

	public void Dispose()
	{
		quoteFunc = null;
		database = null;
		provider = null;
	}
}
