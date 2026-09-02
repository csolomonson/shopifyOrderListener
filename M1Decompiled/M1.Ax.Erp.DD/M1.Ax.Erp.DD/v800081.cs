using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.081", "Change field sizes on IncomeTaxYearTotals", "2010-10-28")]
public class v800081
{
	public v800081(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeeCPPContributions"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeeCPPContributions", "money", 8, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeeQPPContributions"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeeQPPContributions", "money", 8, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCACPPQPPPensionableEarnings"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCACPPQPPPensionableEarnings", "money", 9, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeeEIPremiums"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeeEIPremiums", "money", 8, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEIInsurableEarnings"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEIInsurableEarnings", "money", 9, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCARPPContributions"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCARPPContributions", "money", 9, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCACharitableDonations"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAPensionAdjustment", "money", 9, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeePPIPPremiums"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployeePPIPPremiums", "money", 8, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAPPIPInsurableEarnings"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAPPIPInsurableEarnings", "money", 9, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployerCPPContributions"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployerCPPContributions", "money", 8, 2, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployerEIPremiums"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployerEIPremiums", "money", 8, 2, parms.Messages);
		}
	}
}
