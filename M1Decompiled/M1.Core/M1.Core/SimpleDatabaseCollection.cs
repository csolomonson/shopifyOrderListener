using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace M1.Core;

public class SimpleDatabaseCollection : KeyedCollection<string, M1Database>
{
	public event EventHandler<SimpleDatabaseAddedEventArgs> Added;

	public SimpleDatabaseCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(M1Database item)
	{
		return item.ID;
	}

	protected override void InsertItem(int index, M1Database item)
	{
		base.InsertItem(index, item);
		OnAdded(item);
	}

	public void OnAdded(M1Database database)
	{
		this.Added?.Invoke(this, new SimpleDatabaseAddedEventArgs(database));
	}
}
