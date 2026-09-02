using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.065", "Alter key fields in OrganizationContactGroupLinks table", "2015-07-27")]
public class v900065a
{
	public v900065a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinks", "cmrContactGroupLinkID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationContactGroupLinks", "cmrContactGroupLinkID", "smallint", 4, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinksTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinksTemp");
			}
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "SELECT cmrOrganizationID,cmrLocationID,cmrContactID,cmrContactGroupID,ROW_NUMBER() OVER (PARTITION BY cmrOrganizationID,cmrLocationID,cmrContactID ORDER BY cmrContactGroupID) As RowFilter Into OrganizationContactGroupLinksTemp FROM OrganizationContactGroupLinks ORDER BY RowFilter");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationContactGroupLinks SET cmrContactGroupLinkID=b.RowFilter FROM OrganizationContactGroupLinks a INNER JOIN OrganizationContactGroupLinksTemp b On a.cmrOrganizationID=b.cmrOrganizationID AND a.cmrLocationID=b.cmrLocationID AND a.cmrContactID=b.cmrContactID AND a.cmrContactGroupID=b.cmrContactGroupID");
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinksTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinksTemp");
			}
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinks", new DmoIndex[1]
		{
			new DmoIndex("cmrOrganizationID,cmrLocationID,cmrContactID,cmrContactGroupID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationContactGroupLinks", "cmrContactGroupLinkID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationContactGroupLinks", new DmoIndex[1]
			{
				new DmoIndex("cmrOrganizationID,cmrLocationID,cmrContactID,cmrContactGroupLinkID", unique: true)
			}, parms.Messages);
		}
	}
}
