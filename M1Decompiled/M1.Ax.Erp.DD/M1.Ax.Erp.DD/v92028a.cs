using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.028", "Add fields to DMRShipmentComponents table", "2016-11-22")]
public class v92028a
{
	public v92028a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReverseDMRShipmentLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoReverseDMRShipmentLineID", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReverseDMRShipmentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoReverseDMRShipmentID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReverseDMRShipmentCompID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoReverseDMRShipmentCompID", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
