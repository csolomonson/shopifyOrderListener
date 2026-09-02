using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert OrganizationLocSalesPeople to support unicode", "2013-10-17")]
public class v810RebuildOrganizationLocSalesPeople
{
	public v810RebuildOrganizationLocSalesPeople(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocSalesPeople", new DmoField[8]
		{
			new DmoField("cmkOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmkLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmkSequenceID", "smallint", 4, 0, nullable: false),
			new DmoField("cmkSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmkPercent", "numeric", 6, 2, nullable: false),
			new DmoField("cmkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("CMKORGANIZATIONID,CMKLOCATIONID,CMKSEQUENCEID", unique: true),
			new DmoIndex("CMKUNIQUEID", unique: true),
			new DmoIndex("cmkOrganizationID", unique: false),
			new DmoIndex("cmkLocationID", unique: false),
			new DmoIndex("cmkSequenceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
