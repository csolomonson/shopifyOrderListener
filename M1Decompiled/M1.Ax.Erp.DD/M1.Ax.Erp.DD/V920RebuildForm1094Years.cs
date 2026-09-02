using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.589", "Convert Form1094Years to support unicode", "2013-10-17")]
public class V920RebuildForm1094Years
{
	public V920RebuildForm1094Years(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094Years", new DmoField[36]
		{
			new DmoField("hcpForm1094YearID", "smallint", 4, 0, nullable: false),
			new DmoField("hcpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("hcpCorrectedIndicator", "bit", 1, 0, nullable: false),
			new DmoField("hcpTestScenario", "bit", 1, 0, nullable: false),
			new DmoField("hcpTestScenarioID", "smallint", 1, 0, nullable: false),
			new DmoField("hcpEmployerName", "nvarchar", 50, 0, nullable: false),
			new DmoField("hcpEmployerIDNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("hcpEmployerAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("hcpEmployerAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("hcpEmployerCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("hcpEmployerState", "nvarchar", 3, 0, nullable: false),
			new DmoField("hcpEmployerPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("hcpContactPersonFirstName", "nvarchar", 50, 0, nullable: false),
			new DmoField("hcpContactPersonLastName", "nvarchar", 50, 0, nullable: false),
			new DmoField("hcpContactPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("hcpTotal1095Attached", "smallint", 4, 0, nullable: false),
			new DmoField("hcpAuthoritativeTransmittal", "bit", 1, 0, nullable: false),
			new DmoField("hcpTotal1095Count", "smallint", 4, 0, nullable: false),
			new DmoField("hcpAggregatedGroupMember", "bit", 1, 0, nullable: false),
			new DmoField("hcpQualifyingOfferMethod", "bit", 1, 0, nullable: false),
			new DmoField("hcpQlfyOfferMethodTrnstRelief", "bit", 1, 0, nullable: false),
			new DmoField("hcpSection4980HRelief", "bit", 1, 0, nullable: false),
			new DmoField("hcpNinetyEightPctOfferMethod", "bit", 1, 0, nullable: false),
			new DmoField("hcpYearMinEssentialCvrOffr", "bit", 1, 0, nullable: false),
			new DmoField("hcpYearALEFullTimeCount", "smallint", 4, 0, nullable: false),
			new DmoField("hcpYearTotalEmployeesCount", "smallint", 4, 0, nullable: false),
			new DmoField("hcpYearAggregatedGroup", "bit", 1, 0, nullable: false),
			new DmoField("hcpYearSection4980HRelief", "nvarchar", 1, 0, nullable: false),
			new DmoField("hcpClosed", "bit", 1, 0, nullable: false),
			new DmoField("hcpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("hcpPriorYearData", "bit", 1, 0, nullable: false),
			new DmoField("hcpTransmissionType", "nvarchar", 1, 0, nullable: false),
			new DmoField("hcpOriginalReceiptID", "nvarchar", 80, 0, nullable: false),
			new DmoField("hcpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("hcpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("hcpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("HCPFORM1094YEARID,HCPPLANTID", unique: true),
			new DmoIndex("HCPUNIQUEID", unique: true),
			new DmoIndex("hcpForm1094YearID", unique: false),
			new DmoIndex("hcpPlantID", unique: false)
		}, mergeCustomFields: true);
	}
}
