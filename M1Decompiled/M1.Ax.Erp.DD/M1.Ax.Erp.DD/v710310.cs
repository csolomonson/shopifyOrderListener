using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.310", "Add Cash Flow related fields to GL Charts", "2009-03-31")]
public class v710310
{
	public v710310(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLCharts", "glcCashEquivalents"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLCharts", "glcCashEquivalents", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLCharts", "glcCashFlowCategory"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLCharts", "glcCashFlowCategory", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
