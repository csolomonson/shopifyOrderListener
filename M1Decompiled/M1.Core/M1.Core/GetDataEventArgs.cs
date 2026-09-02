using System;
using System.Data;

namespace M1.Core;

public class GetDataEventArgs : EventArgs
{
	public M1BindingSource BindingSource;

	public QueryDefinition QueryDefinition;

	public DataTable Table;

	public GetDataEventArgs()
	{
	}

	public GetDataEventArgs(M1BindingSource bindingSource, QueryDefinition queryDef, DataTable table)
	{
		BindingSource = bindingSource;
		QueryDefinition = queryDef;
		Table = table;
	}
}
