using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add RMA Receipt/DMR Shipment to GLJournals", "2008-03-26")]
public class v710000b
{
	public v710000b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournals", "glpRMAReceiptID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournals", "glpRMAReceiptID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournals", "glpDMRShipmentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournals", "glpDMRShipmentID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
