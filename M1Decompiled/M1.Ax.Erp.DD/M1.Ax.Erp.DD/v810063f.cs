using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.063", "Add fields to PartForecasts table", "2013-12-23")]
public class v810063f
{
	public v810063f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartForecasts", "inpCreatedBy"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecasts", "inpCreatedBy", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartForecasts", "inpCreatedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecasts", "inpCreatedDate", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
