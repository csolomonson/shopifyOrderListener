using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to MaterialIssueLines table", "2014-10-23")]
public class v900008d
{
	public v900008d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injMaterialIssueID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", new DmoIndex[1]
			{
				new DmoIndex("injMaterialIssueID", unique: false)
			}, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injIssueQuantity"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injIssueQuantity", "injInvIssueQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injMaterialIssueLineID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", new DmoIndex[1]
			{
				new DmoIndex("injMaterialIssueLineID", unique: false)
			}, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injCreateJobMatSeq"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injCreateJobMatSeq", "injCreateJobSeq", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injScrapQuantity"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injScrapQuantity", "injInvScrapQuantity", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injJobMatScrapQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injJobMatScrapQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injJobAsmScrapQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injJobAsmScrapQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injJobAsmIssueQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injJobAsmIssueQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injPlantID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injPlantID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injJobMatIssueQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injJobMatIssueQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injKitPart"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injKitPart", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injMaterialIssueDate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injMaterialIssueDate", dropTriggers: true);
		}
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injLongDescriptionText");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injQuantityAllocated");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injLongDescriptionRTF");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injQuantityOnHand");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injPlantID");
	}
}
