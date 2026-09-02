using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.240", "Add new fields for the new WORKFLOWLINERESOURCES table", "2012-03-21")]
public class v800240b
{
	public v800240b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrWorkFlowId"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrWorkFlowId", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrWorkFlowLineId"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrWorkFlowLineId", "numeric", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrResourceID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrResourceID", "numeric", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrResourceType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrResourceType", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrExternalResourceId"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrExternalResourceId", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrUniqueId"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrUniqueId", "uniqueidentifier", 16, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WorkFlowLineResources Set wfrUniqueID = NewID()");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrCreatedBy"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrCreatedBy", "char", 20, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrCreatedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrCreatedDate", "datetime", 14, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
