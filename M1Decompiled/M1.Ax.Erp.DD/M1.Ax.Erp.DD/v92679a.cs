using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.679", "Add fields to Shipments table", "2018-04-11")]
public class v92679a
{
	public v92679a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shipments", "smpEDITransferredDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shipments", "smpEDITransferredDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shipments", "smpEDITransferred"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shipments", "smpEDITransferred", "bit", 1, 0, isNullable: false, parms.Messages);
		}
	}
}
