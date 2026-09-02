using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.005", "Add Previous Amounts to Invoice Revaluations table", "2009-03-26")]
public class v720005
{
	public v720005(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceRevaluations", "arvGLFiscalYearID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations", "arvGLFiscalYearID", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceRevaluations", "arvGLFiscalYearPeriodID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations", "arvGLFiscalYearPeriodID", "numeric", 2, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceRevaluations Set arvGLFiscalYearID = glfGLFiscalYearID, arvGLFiscalYearPeriodID = glfGLFiscalYearPeriodID From GLFiscalYearPeriods Where glfStartDate <= arvRevalueDate and glfEndDate >= arvRevalueDate");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceRevaluations", "apvGLFiscalYearID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations", "apvGLFiscalYearID", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceRevaluations", "apvGLFiscalYearPeriodID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations", "apvGLFiscalYearPeriodID", "numeric", 2, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APInvoiceRevaluations Set apvGLFiscalYearID = glfGLFiscalYearID, apvGLFiscalYearPeriodID = glfGLFiscalYearPeriodID From GLFiscalYearPeriods Where glfStartDate <= apvRevalueDate and glfEndDate >= apvRevalueDate");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceRevaluations", "arvPrevAmountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations", "arvPrevAmountBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceRevaluations", "arvPrevAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations", "arvPrevAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceRevaluations", "apvPrevAmountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations", "apvPrevAmountBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceRevaluations", "apvPrevAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations", "apvPrevAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
