using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.038", "Add Product Categories tables", "2013-09-19")]
public class v810038f
{
	public v810038f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductCategories"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductCategories");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductCategoryLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductCategoryLines");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrProductCategoryID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrProductCategoryID", "char", 30, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrProductCategoryLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrProductCategoryLineID", "numeric", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
