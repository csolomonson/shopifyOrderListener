using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace M1.Core;

public abstract class AdditionalFilterParameter
{
	public string Caption = string.Empty;

	public string SqlFilterExpression = string.Empty;

	public string AdoFilterExpression = string.Empty;

	public bool SqlOnly;

	public string AdditionalFields = string.Empty;

	public bool IgnoreWhenEmpty = true;

	public string SelectedItem = string.Empty;

	public event EventHandler FilterChanged;

	public AdditionalFilterParameter(string caption)
	{
		Caption = caption;
	}

	public string ProcessSqlFilterExpression()
	{
		return ProcessFilterExpression(sql: true);
	}

	public string ProcessAdoFilterExpression()
	{
		if (SqlOnly)
		{
			return string.Empty;
		}
		return ProcessFilterExpression(sql: false);
	}

	protected virtual string ProcessFilterExpression(bool sql)
	{
		return string.Empty;
	}

	public virtual void VerifyCurrentAndDefaultFields(Dictionary<string, object> currentValueFields, Dictionary<string, object> defaultValues)
	{
	}

	protected void OnFilterChanged()
	{
		this.FilterChanged?.Invoke(this, EventArgs.Empty);
	}

	public virtual void DataTableLoad(DataView table, M1BindingSource bindingSource)
	{
	}

	public virtual Control CreateControl(IServiceProvider provider)
	{
		return null;
	}
}
