using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProjectedPayments to support unicode", "2013-10-17")]
public class v810RebuildProjectedPayments
{
	public v810RebuildProjectedPayments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProjectedPayments", new DmoField[14]
		{
			new DmoField("gloProjectedPaymentID", "int", 9, 0, nullable: false),
			new DmoField("gloPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gloPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gloPaymentType", "tinyint", 1, 0, nullable: false),
			new DmoField("gloOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("gloDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("gloAmount", "money", 12, 2, nullable: false),
			new DmoField("gloPaymentDate", "date", 14, 0, nullable: true),
			new DmoField("gloIgnoreAfterDate", "date", 14, 0, nullable: true),
			new DmoField("gloClosed", "bit", 1, 0, nullable: false),
			new DmoField("gloClosedDate", "date", 14, 0, nullable: true),
			new DmoField("gloCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gloCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gloUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("GLOPROJECTEDPAYMENTID", unique: true),
			new DmoIndex("GLOUNIQUEID", unique: true),
			new DmoIndex("gloPlantDepartmentID", unique: false),
			new DmoIndex("gloPlantID", unique: false),
			new DmoIndex("gloPaymentType", unique: false),
			new DmoIndex("gloOrganizationID", unique: false),
			new DmoIndex("gloPaymentDate", unique: false),
			new DmoIndex("gloIgnoreAfterDate", unique: false),
			new DmoIndex("gloClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
