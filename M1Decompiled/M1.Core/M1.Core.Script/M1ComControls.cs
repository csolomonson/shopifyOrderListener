using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IM1ComCollection))]
public class M1ComControls : KeyedCollection<string, Control>, IM1ComCollection
{
	public Control.ControlCollection Controls;

	public new object this[string name] => Controls[name.ToUpper()];

	protected override string GetKeyForItem(Control item)
	{
		return item.Name.ToUpper();
	}

	public int Add(object value)
	{
		return 0;
	}

	public bool Contains(object value)
	{
		return Controls.Contains((Control)value);
	}

	public int IndexOf(object value)
	{
		return Controls.IndexOf((Control)value);
	}

	public void Remove(object value)
	{
		Controls.Remove((Control)value);
	}

	public void Insert(int index, object value)
	{
	}

	[DispId(-4)]
	public new IEnumerator GetEnumerator()
	{
		return Controls.GetEnumerator();
	}

	public void LoadCollection(object controlCollection)
	{
		throw new NotImplementedException();
	}

	object IM1ComCollection.get__Default(string name)
	{
		return this[name];
	}
}
