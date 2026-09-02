using System;
using System.Collections.Generic;

namespace M1.Ax.Erp;

public class PartInformationEqualityComparer : IEqualityComparer<PartInformation>
{
	public bool Equals(PartInformation x, PartInformation y)
	{
		if (x != null)
		{
			if (x.Part.Equals(y?.Part, StringComparison.CurrentCultureIgnoreCase) && x.PartRevision.Equals(y?.PartRevision, StringComparison.CurrentCultureIgnoreCase) && x.PartWarehouse.Equals(y?.PartWarehouse, StringComparison.CurrentCultureIgnoreCase))
			{
				return x.PartBin.Equals(y?.PartBin, StringComparison.CurrentCultureIgnoreCase);
			}
			return false;
		}
		return false;
	}

	public int GetHashCode(PartInformation obj)
	{
		return (obj.Part.Trim() + "|" + obj.PartRevision.Trim() + "|" + obj.PartWarehouse.Trim() + "|" + obj.PartBin.Trim()).GetHashCode();
	}
}
