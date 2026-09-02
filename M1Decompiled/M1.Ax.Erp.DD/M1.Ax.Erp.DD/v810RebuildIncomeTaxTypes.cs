using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxTypes to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxTypes
{
	public v810RebuildIncomeTaxTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTypes", new DmoField[31]
		{
			new DmoField("pafIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pafTaxCategory", "nvarchar", 1, 0, nullable: false),
			new DmoField("pafPaidBy", "tinyint", 1, 0, nullable: false),
			new DmoField("pafExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pafAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pafRoundTax", "bit", 1, 0, nullable: false),
			new DmoField("pafPrintOnPaySlip", "bit", 1, 0, nullable: false),
			new DmoField("pafDeductIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafDeductIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafDeductMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("pafSecondDeductIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafSecondDeductIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafSecondDeductMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("pafPaySlipDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pafInactive", "bit", 1, 0, nullable: false),
			new DmoField("pafInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("pafCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pafCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pafUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("pafUSBox14A", "bit", 1, 0, nullable: false),
			new DmoField("pafUSBox14B", "bit", 1, 0, nullable: false),
			new DmoField("pafUSBox14C", "bit", 1, 0, nullable: false),
			new DmoField("pafUSBox14Description", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafThirdDeductIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafThirdDeductMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("pafThirdDeductIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafFourthDeductMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("pafFourthDeductIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pafFourthDeductIncomeTaxID", "nvarchar", 5, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("PAFINCOMETAXID,PAFINCOMETAXTYPEID", unique: true),
			new DmoIndex("PAFUNIQUEID", unique: true),
			new DmoIndex("pafIncomeTaxID", unique: false),
			new DmoIndex("pafIncomeTaxTypeID", unique: false),
			new DmoIndex("pafTaxCategory", unique: false),
			new DmoIndex("pafDeductIncomeTaxID", unique: false),
			new DmoIndex("pafDeductIncomeTaxTypeID", unique: false),
			new DmoIndex("pafSecondDeductIncomeTaxID", unique: false),
			new DmoIndex("pafSecondDeductIncomeTaxTypeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
