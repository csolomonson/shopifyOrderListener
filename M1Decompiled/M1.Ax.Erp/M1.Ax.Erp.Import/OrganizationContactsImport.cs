using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Import;

[ImportProcessing("OrganizationContacts")]
public class OrganizationContactsImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into Organizations (cmoOrganizationID) Select cmcOrganizationID From OrganizationContacts Where cmcOrganizationID Not In (Select cmoOrganizationID From Organizations) And cmcOrganizationID In (Select cmcOrganizationID From " + parm.TempTable + ") Group By cmcOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into OrganizationLocations (cmlOrganizationID,cmlLocationID) Select cmcOrganizationID,cmcLocationID From OrganizationContacts Where cmcOrganizationID+cmcLocationID Not In (Select cmlOrganizationID+cmlLocationID From OrganizationLocations) And cmcOrganizationID+cmcLocationID In (Select cmcOrganizationID+cmcLocationID From " + parm.TempTable + ") Group By cmcOrganizationID,cmcLocationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into OrganizationLocations (cmlOrganizationID,cmlLocationID) Select cmoOrganizationID,'' As cmlLocationID From Organizations Where cmoOrganizationID Not In (Select cmlOrganizationID From OrganizationLocations Where cmlLocationID = '') And cmoOrganizationID In (Select cmcOrganizationID From " + parm.TempTable + ") Group By cmoOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Update Organizations Set cmoName = cmcName From Organizations Inner Join OrganizationContacts On cmoOrganizationID = cmcOrganizationID Where cmoName = '' And cmoOrganizationID In (Select cmcOrganizationID From " + parm.TempTable + ")"));
		parm.Database.ExecuteCommand(new SqlCommand("Update OrganizationLocations Set cmlName = cmcName From OrganizationLocations Inner Join OrganizationContacts On cmlOrganizationID = cmcOrganizationID And cmlLocationID = cmcLocationID Where cmlName = '' And cmlOrganizationID+cmlLocationID In (Select cmcOrganizationID+cmcLocationID From " + parm.TempTable + ")"));
		parm.Database.ExecuteCommand(new SqlCommand("Update OrganizationLocations Set cmlName = cmoName From OrganizationLocations Inner Join Organizations On cmlOrganizationID = cmoOrganizationID Where cmlName = '' And cmlLocationID = '' And cmlOrganizationID In (Select cmcOrganizationID From " + parm.TempTable + ")"));
	}
}
