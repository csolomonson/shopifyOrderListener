using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.064", "Add fields to FormDefinitions table", "2014-01-03")]
public class v810064a
{
	public v810064a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FormDefinitions", "xaoCustom"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormDefinitions", "xaoCustom", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoCustom = 1 Where xaoType <> 0");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FormDefinitions", "xaoSequence"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormDefinitions", "xaoSequence", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FormDefinitions", "xaoParentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormDefinitions", "xaoParentID", "nvarchar", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FormDefinitions", "xaoNestedName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormDefinitions", "xaoNestedName", "nvarchar", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
