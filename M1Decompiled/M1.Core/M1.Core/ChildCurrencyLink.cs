using System;
using System.Data;

namespace M1.Core;

public class ChildCurrencyLink : IDisposable
{
	public string ParentTable = string.Empty;

	public string ChildTable = string.Empty;

	public string ChildField = string.Empty;

	public byte ChildFieldDecimals;

	public string ChildKeyFields = string.Empty;

	public string[] ChildKeyFieldsArray;

	public M1CurrencyStyle ChildCurrencyType;

	public string ChildRelatedCurrencyField = string.Empty;

	public M1BindingSource ChildBindingSource;

	public TableDefinition ChildTableDefinition;

	public bool CodeExists;

	public ChildCurrencyLink(DataRow row)
	{
		ParentTable = row.Field<string>("ParentTable");
		ChildTable = row.Field<string>("ChildTable");
		ChildField = row.Field<string>("ChildField");
		ChildFieldDecimals = row.Field<byte>("ChildFieldDecimals");
		ChildKeyFields = row.Field<string>("ChildKeyFields");
		ChildKeyFieldsArray = ChildKeyFields.Split(',');
		ChildCurrencyType = row.Field<M1CurrencyStyle>("ChildCurrencyType");
		ChildRelatedCurrencyField = row.Field<string>("ChildRelatedCurrencyField");
		CodeExists = row.Field<bool>("CodeExists");
	}

	public void Dispose()
	{
		if (ChildBindingSource != null)
		{
			ChildBindingSource.Dispose();
			ChildBindingSource = null;
		}
		if (ChildTableDefinition != null)
		{
			ChildTableDefinition.Dispose();
			ChildTableDefinition = null;
		}
	}
}
