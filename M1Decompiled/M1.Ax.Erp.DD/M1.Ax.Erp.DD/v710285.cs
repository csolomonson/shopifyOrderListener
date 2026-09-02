using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.285", "Add Tax Code Lines Table", "2009-03-12")]
public class v710285
{
	public v710285(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "TaxCodeLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodeLines");
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TaxCodes", "xaxTaxRate"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Insert Into TaxCodeLines (xabTaxCodeID,xabTaxCodeLineID,xabEffectiveDate,xabTaxRate) Select xaxTaxCodeID,1,'20000101',xaxTaxRate From TaxCodes ");
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodes", "xaxTaxRate", dropTriggers: true);
			}
		}
	}
}
