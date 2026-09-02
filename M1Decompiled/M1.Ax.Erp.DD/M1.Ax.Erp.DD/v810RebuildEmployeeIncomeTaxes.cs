using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeIncomeTaxes to support unicode", "2013-10-17")]
public class v810RebuildEmployeeIncomeTaxes
{
	public v810RebuildEmployeeIncomeTaxes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", new DmoField[21]
		{
			new DmoField("pamEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pamEmployeeIncomeTaxID", "smallint", 4, 0, nullable: false),
			new DmoField("pamIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pamIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pamIncomeTaxTableID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pamPersonalExemptions", "tinyint", 2, 0, nullable: false),
			new DmoField("pamDependentExemptions", "tinyint", 2, 0, nullable: false),
			new DmoField("pamAdditionalTaxAmount", "money", 10, 2, nullable: false),
			new DmoField("pamExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pamPersonalExemptionAmount", "money", 10, 2, nullable: false),
			new DmoField("pamDisabledDependantCount", "tinyint", 2, 0, nullable: false),
			new DmoField("pamOtherIncomeAmount", "money", 10, 2, nullable: false),
			new DmoField("pamWithholdingCalculationType", "smallint", 4, 0, nullable: false),
			new DmoField("pamDependentAmount", "money", 10, 2, nullable: false),
			new DmoField("pamExtraWithholdingAmount", "money", 10, 2, nullable: false),
			new DmoField("pamOtherDeductionsAmount", "money", 10, 2, nullable: false),
			new DmoField("pamInactive", "bit", 1, 0, nullable: false),
			new DmoField("pamInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("pamCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pamCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pamUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PAMEMPLOYEEID,PAMEMPLOYEEINCOMETAXID", unique: true),
			new DmoIndex("PAMUNIQUEID", unique: true),
			new DmoIndex("pamEmployeeID", unique: false),
			new DmoIndex("pamEmployeeIncomeTaxID", unique: false),
			new DmoIndex("pamIncomeTaxID", unique: false),
			new DmoIndex("pamIncomeTaxTypeID", unique: false),
			new DmoIndex("pamIncomeTaxTableID", unique: false),
			new DmoIndex("pamInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
