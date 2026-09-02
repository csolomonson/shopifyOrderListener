using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace M1.Core;

public class M1DatabaseTableSecurityCollection : KeyedCollection<string, M1DatabaseTableSecurity>
{
	protected override string GetKeyForItem(M1DatabaseTableSecurity item)
	{
		return item.Database.ToUpper();
	}

	public bool IsNoAccessInAllDatabases()
	{
		using (IEnumerator<M1DatabaseTableSecurity> enumerator = GetEnumerator())
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

	public bool IsEditableInAnyDatabases()
	{
		using (IEnumerator<M1DatabaseTableSecurity> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if ((enumerator.Current.ResolvedAccessLevel & SecurityAccessLevel.Edit) == SecurityAccessLevel.Edit)
				{
					return true;
				}
			}
		}
		return false;
	}
}
