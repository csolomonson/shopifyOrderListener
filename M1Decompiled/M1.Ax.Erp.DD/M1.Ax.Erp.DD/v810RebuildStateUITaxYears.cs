using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert StateUITaxYears to support unicode", "2013-10-17")]
public class v810RebuildStateUITaxYears
{
	public v810RebuildStateUITaxYears(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StateUITaxYears", new DmoField[18]
		{
			new DmoField("puyStateUITaxYearID", "smallint", 4, 0, nullable: false),
			new DmoField("puyPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("puyEmployerIDNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("puyEmployerName", "nvarchar", 50, 0, nullable: false),
			new DmoField("puyEmployerAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("puyEmployerAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("puyEmployerCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("puyEmployerState", "nvarchar", 3, 0, nullable: false),
			new DmoField("puyEmployerPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("puyContactPerson", "nvarchar", 30, 0, nullable: false),
			new DmoField("puyContactPhoneNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("puyAccountNumber", "nvarchar", 9, 0, nullable: false),
			new DmoField("puyCountyCode", "nvarchar", 3, 0, nullable: false),
			new DmoField("puyClosed", "bit", 1, 0, nullable: false),
			new DmoField("puyClosedDate", "date", 14, 0, nullable: true),
			new DmoField("puyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("puyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("puyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("PUYSTATEUITAXYEARID,PUYPLANTID", unique: true),
			new DmoIndex("PUYUNIQUEID", unique: true),
			new DmoIndex("puyStateUITaxYearID", unique: false),
			new DmoIndex("puyPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
