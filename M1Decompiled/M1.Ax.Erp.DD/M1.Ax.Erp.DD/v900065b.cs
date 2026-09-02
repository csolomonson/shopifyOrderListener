using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.065", "Alter key fields in OrganizationIndustryTypeLinks table", "2015-07-27")]
public class v900065b
{
	public v900065b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinks", "cmdIndustryTypeLinkID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationIndustryTypeLinks", "cmdIndustryTypeLinkID", "smallint", 4, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinksTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinksTemp");
			}
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "SELECT cmdOrganizationID,cmdIndustryTypeID,ROW_NUMBER() OVER (PARTITION BY cmdOrganizationID ORDER BY cmdIndustryTypeID) As RowFilter Into OrganizationIndustryTypeLinksTemp FROM OrganizationIndustryTypeLinks ORDER BY RowFilter");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationIndustryTypeLinks SET cmdIndustryTypeLinkID=b.RowFilter FROM OrganizationIndustryTypeLinks a INNER JOIN OrganizationIndustryTypeLinksTemp b On a.cmdOrganizationID=b.cmdOrganizationID AND a.cmdIndustryTypeID=b.cmdIndustryTypeID");
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinksTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinksTemp");
			}
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinks", new DmoIndex[1]
		{
			new DmoIndex("cmdOrganizationID,cmdIndustryTypeID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationIndustryTypeLinks", "cmdIndustryTypeLinkID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationIndustryTypeLinks", new DmoIndex[1]
			{
				new DmoIndex("cmdOrganizationID,cmdIndustryTypeLinkID", unique: true)
			}, parms.Messages);
		}
	}
}
