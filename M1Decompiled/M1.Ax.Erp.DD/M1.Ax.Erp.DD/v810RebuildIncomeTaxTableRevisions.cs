using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxTableRevisions to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxTableRevisions
{
	public v810RebuildIncomeTaxTableRevisions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", new DmoField[44]
		{
			new DmoField("parIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("parIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("parIncomeTaxTableID", "nvarchar", 10, 0, nullable: false),
			new DmoField("parIncomeTaxTableRevisionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("parDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("parCalculationMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("parStartDate", "date", 14, 0, nullable: true),
			new DmoField("parDeductTaxLimit", "money", 10, 2, nullable: false),
			new DmoField("parSecondDeductTaxLimit", "money", 10, 2, nullable: false),
			new DmoField("parPersonalExemptionAmount", "money", 10, 2, nullable: false),
			new DmoField("parPersonalSubsequentAmount", "money", 10, 2, nullable: false),
			new DmoField("parDependentExemptionAmount", "money", 10, 2, nullable: false),
			new DmoField("parDependentSubsequentAmount", "money", 10, 2, nullable: false),
			new DmoField("parFixedExemptionAmount", "money", 10, 2, nullable: false),
			new DmoField("parPersonalTaxCredit", "money", 10, 2, nullable: false),
			new DmoField("parLeaveLoadingExemptionAmount", "money", 10, 2, nullable: false),
			new DmoField("parDependentTaxCredit", "money", 10, 2, nullable: false),
			new DmoField("parDeductionPercent", "numeric", 8, 4, nullable: false),
			new DmoField("parDeductionLimit", "money", 10, 2, nullable: false),
			new DmoField("parTaxAmount", "money", 12, 4, nullable: false),
			new DmoField("parSupplementalWagesTaxPercent", "numeric", 8, 4, nullable: false),
			new DmoField("parWageExcess", "money", 10, 2, nullable: false),
			new DmoField("parWageLimit", "money", 10, 2, nullable: false),
			new DmoField("parRelatedIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("parRelatedIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("parTaxLimit", "money", 10, 2, nullable: false),
			new DmoField("parTaxPercent", "numeric", 8, 4, nullable: false),
			new DmoField("parStandardDeduction", "bit", 1, 0, nullable: false),
			new DmoField("parStdDeductionPercent", "numeric", 8, 4, nullable: false),
			new DmoField("parStdDeductionLimit", "money", 10, 2, nullable: false),
			new DmoField("parCAEmploymentCredit", "money", 10, 2, nullable: false),
			new DmoField("parTaxReductionAmount", "money", 10, 2, nullable: false),
			new DmoField("parDisabledDependantDeduction", "money", 10, 2, nullable: false),
			new DmoField("parStdDeductionLowerLimit", "money", 10, 2, nullable: false),
			new DmoField("parTaxCreditReductionPercent", "numeric", 8, 4, nullable: false),
			new DmoField("parTaxCreditExcessAmount", "money", 10, 2, nullable: false),
			new DmoField("parCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("parCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("parUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("parUseYTDAmount", "bit", 1, 0, nullable: false),
			new DmoField("parTaxAbatementPercent", "numeric", 6, 3, nullable: false),
			new DmoField("parThirdDeductTaxLimit", "money", 10, 2, nullable: false),
			new DmoField("parFourthDeductTaxLimit", "money", 10, 2, nullable: false),
			new DmoField("parStandardAdjustmentAmount", "money", 10, 2, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PARINCOMETAXID,PARINCOMETAXTYPEID,PARINCOMETAXTABLEID,PARINCOMETAXTABLEREVISIONID", unique: true),
			new DmoIndex("PARUNIQUEID", unique: true),
			new DmoIndex("parIncomeTaxID", unique: false),
			new DmoIndex("parIncomeTaxTypeID", unique: false),
			new DmoIndex("parIncomeTaxTableID", unique: false),
			new DmoIndex("parIncomeTaxTableRevisionID", unique: false),
			new DmoIndex("parRelatedIncomeTaxID", unique: false),
			new DmoIndex("parRelatedIncomeTaxTypeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
