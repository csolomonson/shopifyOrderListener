using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.157", "Convert Form940YearTotals to support unicode", "2016-04-06")]
public class v900157d
{
	public v900157d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form940YearTotals"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940YearTotals");
		}
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940YearTotals", new DmoField[42]
		{
			new DmoField("pftForm940YearID", "smallint", 4, 0, nullable: false),
			new DmoField("pftPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pftForm940YearTotalID", "smallint", 4, 0, nullable: false),
			new DmoField("pftStateID", "nvarchar", 2, 0, nullable: false),
			new DmoField("pftMultiStateEmployer", "bit", 1, 0, nullable: false),
			new DmoField("pftCreditReduction", "bit", 1, 0, nullable: false),
			new DmoField("pftTotalPayments", "money", 12, 2, nullable: false),
			new DmoField("pftExemptFromFUTA", "money", 12, 2, nullable: false),
			new DmoField("pftFringeBenefits", "bit", 1, 0, nullable: false),
			new DmoField("pftGroupTermLifeInsurance", "bit", 1, 0, nullable: false),
			new DmoField("pftRetirementPension", "bit", 1, 0, nullable: false),
			new DmoField("pftDependentCare", "bit", 1, 0, nullable: false),
			new DmoField("pftOther", "bit", 1, 0, nullable: false),
			new DmoField("pftPaymentExcess", "money", 12, 2, nullable: false),
			new DmoField("pftExemptSubtotal", "money", 12, 2, nullable: false),
			new DmoField("pftTotalTaxableFUTA", "money", 12, 2, nullable: false),
			new DmoField("pftFUTABeforeAdjustments", "money", 12, 2, nullable: false),
			new DmoField("pftAdjustAllExcludeState", "money", 12, 2, nullable: false),
			new DmoField("pftAdjustSomeExcludeState", "money", 12, 2, nullable: false),
			new DmoField("pftAdjustCreditReduction", "money", 12, 2, nullable: false),
			new DmoField("pftFUTAAfterAdjustments", "money", 12, 2, nullable: false),
			new DmoField("pftFUTADeposited", "money", 12, 2, nullable: false),
			new DmoField("pftBalanceDue", "money", 12, 2, nullable: false),
			new DmoField("pftOverpayment", "money", 12, 2, nullable: false),
			new DmoField("pftOverpaymentOption", "tinyint", 1, 0, nullable: false),
			new DmoField("pftFUTALiabilityQ1", "money", 12, 2, nullable: false),
			new DmoField("pftFUTALiabilityQ2", "money", 12, 2, nullable: false),
			new DmoField("pftFUTALiabilityQ3", "money", 12, 2, nullable: false),
			new DmoField("pftFUTALiabilityQ4", "money", 12, 2, nullable: false),
			new DmoField("pftTotalTaxLiability", "money", 12, 2, nullable: false),
			new DmoField("pftThirdPartyDesignee", "tinyint", 1, 0, nullable: false),
			new DmoField("pftDesigneeName", "nvarchar", 50, 0, nullable: false),
			new DmoField("pftDesigneePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("pftDesigneePIN", "nvarchar", 5, 0, nullable: false),
			new DmoField("pftAuthorisedPerson", "nvarchar", 50, 0, nullable: false),
			new DmoField("pftAuthorisedPersonTitle", "nvarchar", 50, 0, nullable: false),
			new DmoField("pftAuthorisedPersonPhone", "nvarchar", 20, 0, nullable: false),
			new DmoField("pftSignDate", "date", 14, 0, nullable: true),
			new DmoField("pftClosed", "bit", 1, 0, nullable: false),
			new DmoField("pftCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pftCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pftUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PFTFORM940YEARID,PFTPLANTID,PFTFORM940YEARTOTALID", unique: true),
			new DmoIndex("PFTUNIQUEID", unique: true),
			new DmoIndex("pftForm940YearID", unique: false),
			new DmoIndex("pftPlantID", unique: false),
			new DmoIndex("pftForm940YearTotalID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
