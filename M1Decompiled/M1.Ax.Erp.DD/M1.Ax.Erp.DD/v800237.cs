using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.237", "Add Lead Time fields in Job Materials and Quote Materials table", "2012-03-30")]
public class v800237
{
	public v800237(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime1"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime1", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime2"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime2", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime3"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime3", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime4"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime4", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime5"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime5", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime6"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime6", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime7"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime7", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime8"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime8", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmLeadTime9"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmLeadTime9", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime1"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime1", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime2"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime2", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime3"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime3", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime4"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime4", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime5"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime5", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime6"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime6", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime7"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime7", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime8"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime8", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime9"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmLeadTime9", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
