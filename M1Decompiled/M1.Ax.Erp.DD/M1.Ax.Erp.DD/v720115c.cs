using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.115", "Add Income Tax Table Surtaxes table", "2010-03-02")]
public class v720115c
{
	public v720115c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableSurtaxes"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableSurtaxes");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableLines", "palTaxLimit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableLines", "palTaxLimit", "money", 13, 4, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
