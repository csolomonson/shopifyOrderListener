using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace M1.Core;

public class M1DatabaseFieldSecurityCollection : KeyedCollection<string, M1DatabaseFieldSecurity>
{
	public M1DatabaseFieldSecurityCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(M1DatabaseFieldSecurity item)
	{
		return item.Database;
	}

	public bool IsNoAccessInAnyDatabase()
	{
		using (IEnumerator<M1DatabaseFieldSecurity> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if ((enumerator.Current.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsNoAccessInAllDatabases()
	{
		using (IEnumerator<M1DatabaseFieldSecurity> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if ((enumerator.Current.ResolvedAccessLevel & SecurityAccessLevel.None) == 0)
				{
					return false;
				}
			}
		}
		return true;
	}
}
