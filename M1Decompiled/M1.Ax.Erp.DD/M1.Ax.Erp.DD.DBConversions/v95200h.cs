using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Add tax treatment code field to STP Lines table", "2021-12-17")]
public class v95200h
{
	public v95200h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlTaxTreatmentCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlTaxTreatmentCode", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
