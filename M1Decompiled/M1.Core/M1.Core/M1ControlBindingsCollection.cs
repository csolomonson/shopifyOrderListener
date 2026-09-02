using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace M1.Core;

public class M1ControlBindingsCollection : ControlBindingsCollection
{
	private List<Binding> delayedBindings;

	public M1ControlBindingsCollection(IBindableComponent control)
		: base(control)
	{
	}

	protected override void AddCore(Binding dataBinding)
	{
		if (dataBinding.DataSource is M1BindingSource && !((M1BindingSource)dataBinding.DataSource).IsDefinitionLoaded)
		{
			((M1BindingSource)dataBinding.DataSource).LoadDefinitionCompleted -= M1ControlBindingsCollection_LoadDefinitionCompleted;
			((M1BindingSource)dataBinding.DataSource).LoadDefinitionCompleted += M1ControlBindingsCollection_LoadDefinitionCompleted;
			if (delayedBindings == null)
			{
				delayedBindings = new List<Binding>();
			}
			delayedBindings.Add(dataBinding);
		}
		else
		{
			base.AddCore(dataBinding);
		}
	}

	private void M1ControlBindingsCollection_LoadDefinitionCompleted(object sender, EventArgs e)
	{
		if (delayedBindings == null)
		{
			return;
		}
		foreach (Binding delayedBinding in delayedBindings)
		{
			((M1BindingSource)delayedBinding.DataSource).LoadDefinitionCompleted -= M1ControlBindingsCollection_LoadDefinitionCompleted;
			Add(delayedBinding);
		}
		delayedBindings.Clear();
		delayedBindings = null;
	}
}
