using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.543", "Add fields to ARINVOICES table", "2015-05-19")]
public class v800543r
{
	public v800543r(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARINVOICES", "arpEDITransferred"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARINVOICES", "arpEDITransferred", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARINVOICES", "arpEDITransferredDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARINVOICES", "arpEDITransferredDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
