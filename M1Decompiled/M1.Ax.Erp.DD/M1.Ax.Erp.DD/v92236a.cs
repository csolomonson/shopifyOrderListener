using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.236", "Remove fields from ManufacturingVarianceLog table", "2017-05-02")]
public class v92236a
{
	public v92236a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog", "mvlNewUnitDutyCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", "mvlNewUnitDutyCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog", "mvlOldUnitFreightCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", "mvlOldUnitFreightCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog", "mvlNewUnitFreightCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", "mvlNewUnitFreightCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog", "mvlOldUnitDutyCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", "mvlOldUnitDutyCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog", "mvlOldUnitMiscCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", "mvlOldUnitMiscCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog", "mvlNewUnitMiscCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", "mvlNewUnitMiscCost", dropTriggers: true);
		}
	}
}
