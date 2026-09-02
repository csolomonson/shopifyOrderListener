using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.3.100", "Update Length of Integration Id Field in IntegrationCrossReferences table", "2021-03-02")]
public class v93100c
{
	public v93100c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IntegrationCrossReferences", "icrIntegrationID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationCrossReferences", "icrIntegrationID", "nvarchar", 36, 0, isNullable: false, parms.Messages);
		}
	}
}
