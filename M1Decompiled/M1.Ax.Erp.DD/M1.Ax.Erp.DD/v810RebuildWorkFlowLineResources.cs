using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WorkFlowLineResources to support unicode", "2013-10-17")]
public class v810RebuildWorkFlowLineResources
{
	public v810RebuildWorkFlowLineResources(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", new DmoField[8]
		{
			new DmoField("wfrWorkFlowId", "nvarchar", 10, 0, nullable: false),
			new DmoField("wfrWorkFlowlineId", "smallint", 4, 0, nullable: false),
			new DmoField("wfrResourceId", "smallint", 4, 0, nullable: false),
			new DmoField("wfrResourceType", "nvarchar", 50, 0, nullable: false),
			new DmoField("wfrExternalResourceId", "nvarchar", 10, 0, nullable: false),
			new DmoField("wfrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wfrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wfrUniqueId", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("WFRWORKFLOWID,WFRWORKFLOWLINEID,WFREXTERNALRESOURCEID,WFRRESOURCEID", unique: true),
			new DmoIndex("WFRUNIQUEID", unique: true),
			new DmoIndex("wfrWorkFlowId", unique: false),
			new DmoIndex("wfrWorkFlowlineId", unique: false),
			new DmoIndex("wfrResourceId", unique: false),
			new DmoIndex("wfrExternalResourceId", unique: false),
			new DmoIndex("wfrCreatedBy", unique: false),
			new DmoIndex("wfrCreatedDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
