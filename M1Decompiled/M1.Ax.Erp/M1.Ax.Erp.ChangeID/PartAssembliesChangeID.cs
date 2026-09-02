using System;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("PartAssemblies")]
public class PartAssembliesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (!parm.NewKeyValues[0].ToString().Trim().Equals(parm.OldKeyValues[0].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
		{
			throw new M1Exception("Part Assemblies may not be moved between parts.");
		}
		parm.Database.ExecuteCommand("UPDATE PartAssemblies SET imaParentAssemblyID = " + parm.NewKeyValues[2].ToSql() + " WHERE imaMethodID = " + parm.OldKeyValues[0].ToSql() + " AND imaMethodRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND imaParentAssemblyID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
