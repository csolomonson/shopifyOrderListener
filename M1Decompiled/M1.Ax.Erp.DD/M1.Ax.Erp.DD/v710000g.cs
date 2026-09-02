using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Resize Qty per Asm fields on Materials", "2008-05-06")]
public class v710000g
{
	public v710000g(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartMaterials", "immQuantityPerAssembly"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartMaterials", "immQuantityPerAssembly", "numeric", 13, 6, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmQuantityPerAssembly"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmQuantityPerAssembly", "numeric", 13, 6, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmQuantityPerAssembly"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmQuantityPerAssembly", "numeric", 13, 6, parms.Messages);
		}
	}
}
