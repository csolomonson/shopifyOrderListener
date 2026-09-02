using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Lot Number Inspection fields", "2009-02-12")]
public class v710500a
{
	public v710500a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtInspectionID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtInspectionID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtInspectionLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtInspectionLineID", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtInspect", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtQualityRegisterID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtQualityRegisterID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
