using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Field to Production Properties", "2008-05-12")]
public class v710000j
{
	public v710000j(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapPMPOWizardDisplayType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapPMPOWizardDisplayType", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
