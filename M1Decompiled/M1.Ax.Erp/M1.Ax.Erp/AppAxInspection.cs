using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Inspection")]
[ComVisible(true)]
public class AppAxInspection
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxInspection(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool InspectorApprovedCheck(SqlTransaction transaction, string inspector, bool inspComplete)
	{
		return new Inspection().InspectorApprovedCheck(_Database, transaction, inspector, inspComplete);
	}

	public void PostInspection(M1BindingSource bindingsource)
	{
		new Inspection().PostInspection(bindingsource);
	}

	public bool InspectionPeriodCheck(M1BindingSource bindingSource)
	{
		return new Inspection().InspectionPeriodCheck(bindingSource);
	}

	public bool PostInspectionCheck(M1BindingSource bindingsource)
	{
		return new Inspection().PostInspectionCheck(bindingsource);
	}

	public void UpdateInspectorInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new Inspection().UpdateInspectorInGrid(e2.Database, e2.Row, fieldDefinition.FieldName);
	}

	public void PostQtyToInspect(M1BindingSource bindingSource)
	{
		new Inspection().PostQtyToInspect(bindingSource);
	}

	public bool PostQtyToInspectCheck(M1BindingSource bindingSource)
	{
		return new Inspection().PostQtyToInspectCheck(bindingSource);
	}

	public void CreateQtyToInspectJournals(M1BindingSource bindingSource)
	{
		new Inspection().CreateQtyToInspectJournals(bindingSource);
	}

	public string ValidatePartsWithInactiveBinInInspection(M1BindingSource bindingsource)
	{
		return new Inspection().ValidatePartsWithInactiveBinInInspection(bindingsource);
	}
}
