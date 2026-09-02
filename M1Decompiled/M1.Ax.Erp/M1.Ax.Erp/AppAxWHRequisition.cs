using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("WHRequisition")]
[ComVisible(true)]
public class AppAxWHRequisition
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxWHRequisition(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void UpdateQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new WHRequisition().UpdateQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}
}
