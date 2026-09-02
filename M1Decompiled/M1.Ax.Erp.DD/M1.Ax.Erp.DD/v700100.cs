using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.100", "Resize Qty per Asm fields on Operations", "2008-04-16")]
public class v700100
{
	public v700100(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoQuantityPerAssembly"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoQuantityPerAssembly", "numeric", 13, 6, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoQuantityPerAssembly"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoQuantityPerAssembly", "numeric", 13, 6, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoQuantityPerAssembly"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoQuantityPerAssembly", "numeric", 13, 6, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
