using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("WHTransfer")]
[ComVisible(true)]
public class AppAxWHTransfer
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxWHTransfer(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void UpdateQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new WHTransfer().UpdateQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}

	public bool WHTransferPeriodCheck(M1BindingSource bindingSource)
	{
		return new WHTransfer().WHTransferPeriodCheck(bindingSource);
	}

	public bool PostWHTransferCheck(M1BindingSource bindingsource)
	{
		return new WHTransfer().PostWHTransferCheck(bindingsource);
	}

	public void PostWHTransfer(M1BindingSource bindingsource)
	{
		new WHTransfer().PostWHTransfer(bindingsource);
	}
}
