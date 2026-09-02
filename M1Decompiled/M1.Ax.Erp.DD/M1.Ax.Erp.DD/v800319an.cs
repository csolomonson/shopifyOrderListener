using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.319", "Add fields to INCOMETAXTYPES table", "2015-05-19")]
public class v800319an
{
	public v800319an(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14A"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14A", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14B"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14B", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14C"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14C", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14Description"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "INCOMETAXTYPES", "pafUSBox14Description", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
