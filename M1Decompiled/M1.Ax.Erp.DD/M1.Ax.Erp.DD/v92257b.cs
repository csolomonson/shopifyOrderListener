using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.257", "Add fields to MaterialIssueComponents table", "2017-05-18")]
public class v92257b
{
	public v92257b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatParentReturnQty"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatParentReturnQty", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatReturnIssueQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatReturnIssueQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatParentReturnQtyScrap"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatParentReturnQtyScrap", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatReturnScrapQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkJobMatReturnScrapQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
