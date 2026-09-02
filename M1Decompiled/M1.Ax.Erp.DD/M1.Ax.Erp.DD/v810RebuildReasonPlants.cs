using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ReasonPlants to support unicode", "2013-10-17")]
public class v810RebuildReasonPlants
{
	public v810RebuildReasonPlants(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReasonPlants", new DmoField[6]
		{
			new DmoField("xajReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xajReasonPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xajReasonGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xajCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xajCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xajUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XAJREASONID,XAJREASONPLANTID", unique: true),
			new DmoIndex("XAJUNIQUEID", unique: true),
			new DmoIndex("xajReasonID", unique: false),
			new DmoIndex("xajReasonPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
