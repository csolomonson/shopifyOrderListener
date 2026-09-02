using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartAssemblies to support unicode", "2013-10-17")]
public class v810RebuildPartAssemblies
{
	public v810RebuildPartAssemblies(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", new DmoField[24]
		{
			new DmoField("imaMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imaMethodRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imaMethodAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("imaLevel", "smallint", 3, 0, nullable: false),
			new DmoField("imaParentAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("imaPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imaPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imaUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("imaPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imaPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imaPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imaUseMethod", "bit", 1, 0, nullable: false),
			new DmoField("imaSourceMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imaSourceRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imaProductionNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imaProductionNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imaQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("imaPullAllFromStock", "bit", 1, 0, nullable: false),
			new DmoField("imaDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imaOverlapMethodOperationID", "int", 5, 0, nullable: false),
			new DmoField("imaOverlapType", "tinyint", 1, 0, nullable: false),
			new DmoField("imaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("IMAMETHODID,IMAMETHODREVISIONID,IMAMETHODASSEMBLYID", unique: true),
			new DmoIndex("IMAUNIQUEID", unique: true),
			new DmoIndex("imaMethodID", unique: false),
			new DmoIndex("imaMethodRevisionID", unique: false),
			new DmoIndex("imaMethodAssemblyID", unique: false),
			new DmoIndex("imaPartID", unique: false),
			new DmoIndex("imaSourceMethodID", unique: false),
			new DmoIndex("imaSourceRevisionID", unique: false),
			new DmoIndex("imaOverlapMethodOperationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
