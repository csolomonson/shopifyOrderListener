using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Financial Properties")]
public class FinancialProperties
{
	public FinancialProperties(DBCreateDefaultParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From FinancialProperties", fillSchema: true, out adapter);
		DataRow dataRow;
		if (dataTable.Rows.Count == 0)
		{
			dataRow = dataTable.NewRow();
			dataRow.BlankRow();
			dataTable.Rows.Add(dataRow);
		}
		else
		{
			dataRow = dataTable.Rows[0];
		}
		dataRow.SetField("xafARGroupShipmentsByCustomer", 1);
		dataRow.SetField("xafARShowDeposits", 2);
		dataRow.SetField("xafAgingMethod", 1);
		dataRow.SetField("xafTaxOnReportMethod", "S");
		dataRow.SetField("xafAgeByDaysInMonth", value: true);
		dataRow.SetField("xafARExpressPost", value: true);
		dataRow.SetField("xafARFinanceShowCreditBalance", 1);
		dataRow.SetField("xafAPPaymentMaxLinesPerPage", 24);
		dataRow.SetField("xafAPExpressPost", value: true);
		dataRow.SetField("xafGLExpressPost", value: true);
		dataRow.SetField("xafPAShowHolidaysForSalary", parm.Database.Region.Equals("AUS", StringComparison.InvariantCultureIgnoreCase));
		dataRow.SetField("xafPAPayrollSort", 1);
		dataRow.SetField("xafPAExpressPost", value: true);
		dataRow.SetField("xafPAAssignNumbersToEFT", parm.Database.Region.Equals("AUS", StringComparison.InvariantCultureIgnoreCase));
		dataRow.SetField("xafPADeleteZeroPayHeaders", value: true);
		dataRow.SetField("xafAPAssignNumbersToEFT", parm.Database.Region.Equals("AUS", StringComparison.InvariantCultureIgnoreCase));
		dataRow.SetField("xafIncludeLLInTermination", value: true);
		dataRow.SetField("xafARIncludeTaxInDepositCalc", value: true);
		dataRow.SetField("xafCreatedBy", parm.User.ID);
		dataRow.SetField("xafCreatedDate", DateTime.Now);
		dataRow.SetField("xafPAUseDate", 1);
		dataRow.SetField("xafAvalaraFilterCountry", 1);
		dataRow.SetField("xafAvalaraARInvoicePostOption", 2);
		dataRow.SetField("xafAvalaraDisableIgnoreLine", value: true);
		dataRow.SetField("xafMiscReceiptVarianceAccount", 1);
		if (parm.DataDictionary.ProductCode.IsCustomModulePurchased(13))
		{
			dataRow.SetField("xafPartsMustExist", value: true);
		}
		parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
	}
}
