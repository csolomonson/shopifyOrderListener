using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobMaterialComponents to support unicode", "2013-10-17")]
public class v810RebuildJobMaterialComponents
{
	public v810RebuildJobMaterialComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterialComponents", new DmoField[19]
		{
			new DmoField("jmtJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmtJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("jmtJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("jmtJobMaterialComponentID", "int", 5, 0, nullable: false),
			new DmoField("jmtPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("jmtPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("jmtPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmtPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("jmtQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("jmtAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmtMaterialQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmtUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("jmtDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmtQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("jmtReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("jmtClosed", "bit", 1, 0, nullable: false),
			new DmoField("jmtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("JMTJOBID,JMTJOBASSEMBLYID,JMTJOBMATERIALID,JMTJOBMATERIALCOMPONENTID", unique: true),
			new DmoIndex("JMTUNIQUEID", unique: true),
			new DmoIndex("jmtJobID", unique: false),
			new DmoIndex("jmtJobAssemblyID", unique: false),
			new DmoIndex("jmtJobMaterialID", unique: false),
			new DmoIndex("jmtJobMaterialComponentID", unique: false),
			new DmoIndex("jmtPartID", unique: false),
			new DmoIndex("jmtPartRevisionID", unique: false),
			new DmoIndex("jmtReceivedComplete", unique: false),
			new DmoIndex("jmtClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
