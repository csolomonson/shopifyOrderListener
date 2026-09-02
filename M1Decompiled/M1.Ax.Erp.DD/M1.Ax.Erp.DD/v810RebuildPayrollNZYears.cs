using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollNZYears to support unicode", "2015-10-08")]
public class v810RebuildPayrollNZYears
{
	public v810RebuildPayrollNZYears(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYears", new DmoField[17]
		{
			new DmoField("nzpPayrollNZYearID", "smallint", 4, 0, nullable: false),
			new DmoField("nzpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("nzpEmployerIRDNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("nzpEmployerName", "nvarchar", 50, 0, nullable: false),
			new DmoField("nzpEmployerAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("nzpEmployerAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("nzpEmployerCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("nzpEmployerState", "nvarchar", 3, 0, nullable: false),
			new DmoField("nzpEmployerPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("nzpContactPerson", "nvarchar", 50, 0, nullable: false),
			new DmoField("nzpContactPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("nzpContactEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("nzpClosed", "bit", 1, 0, nullable: false),
			new DmoField("nzpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("nzpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("nzpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("nzpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("NZPPAYROLLNZYEARID,NZPPLANTID", unique: true),
			new DmoIndex("NZPUNIQUEID", unique: true),
			new DmoIndex("nzpPayrollNZYearID", unique: false),
			new DmoIndex("nzpPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
