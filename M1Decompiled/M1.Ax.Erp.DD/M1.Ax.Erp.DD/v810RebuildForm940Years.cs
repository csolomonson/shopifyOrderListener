using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form940Years to support unicode", "2013-10-17")]
public class v810RebuildForm940Years
{
	public v810RebuildForm940Years(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940Years", new DmoField[17]
		{
			new DmoField("pfyForm940YearID", "smallint", 4, 0, nullable: false),
			new DmoField("pfyPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pfyEmployerIDNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("pfyEmployerName", "nvarchar", 50, 0, nullable: false),
			new DmoField("pfyEmployerAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("pfyEmployerAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("pfyEmployerCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("pfyEmployerState", "nvarchar", 3, 0, nullable: false),
			new DmoField("pfyEmployerPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("pfyAmended", "bit", 1, 0, nullable: false),
			new DmoField("pfySuccessorEmployer", "bit", 1, 0, nullable: false),
			new DmoField("pfyNoPayments", "bit", 1, 0, nullable: false),
			new DmoField("pfyFinal", "bit", 1, 0, nullable: false),
			new DmoField("pfyClosed", "bit", 1, 0, nullable: false),
			new DmoField("pfyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pfyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pfyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("PFYFORM940YEARID,PFYPLANTID", unique: true),
			new DmoIndex("PFYUNIQUEID", unique: true),
			new DmoIndex("pfyForm940YearID", unique: false),
			new DmoIndex("pfyPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
