using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.145", "Add PAYG fields to IncomeTaxYearTotals table", "2008-04-21")]
public class v700145
{
	public v700145(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSAmendIndicator"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSAmendIndicator", "char", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE IncomeTaxYearTotals SET pahAUSAmendIndicator = 'O' ");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSTransitTermination"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSTransitTermination", "char", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSRelatedPriorTermination"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSRelatedPriorTermination", "char", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSTaxFreeAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSTaxFreeAmount", "money", 8, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
