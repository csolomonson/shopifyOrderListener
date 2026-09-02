using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.825", "Add fields to EmployeeIncomeTaxes table", "2019-11-19")]
public class v92825b
{
	public v92825b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeIncomeTaxes", "pamOtherIncomeAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", "pamOtherIncomeAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeIncomeTaxes", "pamWithholdingCalculationType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", "pamWithholdingCalculationType", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeIncomeTaxes", "pamDependentAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", "pamDependentAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeIncomeTaxes", "pamExtraWithholdingAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", "pamExtraWithholdingAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeIncomeTaxes", "pamOtherDeductionsAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeIncomeTaxes", "pamOtherDeductionsAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
