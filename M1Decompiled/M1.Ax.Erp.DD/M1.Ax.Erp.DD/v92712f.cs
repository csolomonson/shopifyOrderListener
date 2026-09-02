using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.712", "Add fields to EmployeePersonalData table", "2018-05-04")]
public class v92712f
{
	public v92712f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdResidencyStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdResidencyStatus", "nvarchar", 25, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdWorkingHolidayMaker"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdWorkingHolidayMaker", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdTaxFreeThresholdClaimed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdTaxFreeThresholdClaimed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdBasisOfPayment"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdBasisOfPayment", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdStdntFinSupplSchemeLoan"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdStdntFinSupplSchemeLoan", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdStudyTrainLoanRepayment"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdStudyTrainLoanRepayment", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
