using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeTaxCreditReturns to support unicode", "2013-10-17")]
public class v810RebuildEmployeeTaxCreditReturns
{
	public v810RebuildEmployeeTaxCreditReturns(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeTaxCreditReturns", new DmoField[24]
		{
			new DmoField("lncEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lncEmployeeIncomeTaxID", "smallint", 4, 0, nullable: false),
			new DmoField("lncEmployeeTaxCreditReturnID", "smallint", 4, 0, nullable: false),
			new DmoField("lncBasicPersonalAmount", "money", 10, 2, nullable: false),
			new DmoField("lncChildAmount", "money", 10, 2, nullable: false),
			new DmoField("lncAgeAmount", "money", 10, 2, nullable: false),
			new DmoField("lncPensionIncomeAmount", "money", 10, 2, nullable: false),
			new DmoField("lncEducationAmount", "money", 10, 2, nullable: false),
			new DmoField("lncDisabilityAmount", "money", 10, 2, nullable: false),
			new DmoField("lncSpouseAmount", "money", 10, 2, nullable: false),
			new DmoField("lncEligibleDependantAmount", "money", 10, 2, nullable: false),
			new DmoField("lncCareGiverAmount", "money", 10, 2, nullable: false),
			new DmoField("lncInfirmDependantAmount", "money", 10, 2, nullable: false),
			new DmoField("lncSpouseTransferAmount", "money", 10, 2, nullable: false),
			new DmoField("lncDependantTransferAmount", "money", 10, 2, nullable: false),
			new DmoField("lncTotalClaimAmount", "money", 10, 2, nullable: false),
			new DmoField("lncPrescribedZoneAmount", "money", 10, 2, nullable: false),
			new DmoField("lncStartDate", "date", 14, 0, nullable: true),
			new DmoField("lncInactive", "bit", 1, 0, nullable: false),
			new DmoField("lncInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("lncAmountNotClaimed", "bit", 1, 0, nullable: false),
			new DmoField("lncCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lncCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lncUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("LNCEMPLOYEEID,LNCEMPLOYEEINCOMETAXID,LNCEMPLOYEETAXCREDITRETURNID", unique: true),
			new DmoIndex("LNCUNIQUEID", unique: true),
			new DmoIndex("lncEmployeeID", unique: false),
			new DmoIndex("lncEmployeeIncomeTaxID", unique: false),
			new DmoIndex("lncEmployeeTaxCreditReturnID", unique: false),
			new DmoIndex("lncInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
