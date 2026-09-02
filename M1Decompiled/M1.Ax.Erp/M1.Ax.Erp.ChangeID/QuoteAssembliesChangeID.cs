using System;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("QuoteAssemblies")]
public class QuoteAssembliesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (!parm.NewKeyValues[0].ToString().Trim().Equals(parm.OldKeyValues[0].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase) || !Convert.ToInt32(parm.NewKeyValues[1]).Equals(Convert.ToInt32(parm.OldKeyValues[1])))
		{
			throw new M1Exception("Quote Assemblies may not be moved between quote lines.");
		}
		parm.Database.ExecuteCommand("UPDATE QuoteAssemblies SET qmaParentAssemblyID = " + parm.NewKeyValues[2].ToSql() + " WHERE qmaQuoteID = " + parm.OldKeyValues[0].ToSql() + " AND qmaQuoteLineID = " + parm.OldKeyValues[1].ToSql() + " AND qmaParentAssemblyID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
