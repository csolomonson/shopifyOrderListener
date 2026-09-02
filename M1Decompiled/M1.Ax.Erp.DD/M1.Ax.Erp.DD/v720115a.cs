using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.115", "Add Employee Tax Credit Returns table", "2010-03-02")]
public class v720115a
{
	public v720115a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeeTaxCreditReturns"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeTaxCreditReturns");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parCAEmploymentCredit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parCAEmploymentCredit", "money", 10, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parTaxReductionAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parTaxReductionAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parDisabledDependantDeduction"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parDisabledDependantDeduction", "money", 10, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeIncomeTaxes", "pamDisabledDependantCount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", "pamDisabledDependantCount", "numeric", 2, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
