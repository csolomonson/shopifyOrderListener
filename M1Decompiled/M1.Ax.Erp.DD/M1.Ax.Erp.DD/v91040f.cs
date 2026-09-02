using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.040", "Add fields to FINANCIALPROPERTIES table", "2016-03-06")]
public class v91040f
{
	public v91040f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FINANCIALPROPERTIES", "xafTransmitterControlCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FINANCIALPROPERTIES", "xafTransmitterControlCode", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FINANCIALPROPERTIES", "xafTestFileCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FINANCIALPROPERTIES", "xafTestFileCode", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FINANCIALPROPERTIES", "xafUS1094FileLocation"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FINANCIALPROPERTIES", "xafUS1094FileLocation", "nvarchar", 250, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
