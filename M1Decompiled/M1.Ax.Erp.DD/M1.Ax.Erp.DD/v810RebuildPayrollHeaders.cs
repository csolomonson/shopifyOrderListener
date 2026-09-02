using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollHeaders to support unicode", "2013-10-17")]
public class v810RebuildPayrollHeaders
{
	public v810RebuildPayrollHeaders(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaders", new DmoField[35]
		{
			new DmoField("patPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("patPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("patTerminationPay", "bit", 1, 0, nullable: false),
			new DmoField("patPayrollEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("patPayrollDefinitionID", "nvarchar", 5, 0, nullable: false),
			new DmoField("patGrossPayAmount", "money", 10, 2, nullable: false),
			new DmoField("patTotalEmployeeAllowances", "money", 10, 2, nullable: false),
			new DmoField("patTotalCompanyAllowances", "money", 10, 2, nullable: false),
			new DmoField("patTotalAllowances", "money", 10, 2, nullable: false),
			new DmoField("patTotalEmployeeDeductions", "money", 10, 2, nullable: false),
			new DmoField("patTotalCompanyDeductions", "money", 10, 2, nullable: false),
			new DmoField("patTotalDeductions", "money", 10, 2, nullable: false),
			new DmoField("patTotalEmployeeTaxes", "money", 10, 2, nullable: false),
			new DmoField("patTotalCompanyTaxes", "money", 10, 2, nullable: false),
			new DmoField("patTotalTaxes", "money", 10, 2, nullable: false),
			new DmoField("patNetPayAmount", "money", 10, 2, nullable: false),
			new DmoField("patTotalStandardHours", "numeric", 6, 2, nullable: false),
			new DmoField("patTotalOvertimeHours", "numeric", 6, 2, nullable: false),
			new DmoField("patTotalOtherHours", "numeric", 6, 2, nullable: false),
			new DmoField("patPaymentNumber", "int", 6, 0, nullable: false),
			new DmoField("patPaymentAmount", "money", 10, 2, nullable: false),
			new DmoField("patPayrollNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("patPayrollNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("patEFTAmount", "money", 10, 2, nullable: false),
			new DmoField("patCompleted", "bit", 1, 0, nullable: false),
			new DmoField("patVoidedPayment", "bit", 1, 0, nullable: false),
			new DmoField("patVoidedPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("patVoidedPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("patPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("patNumberOfPays", "tinyint", 2, 0, nullable: false),
			new DmoField("patTotalSalarySacrifice", "money", 10, 2, nullable: false),
			new DmoField("patTotalAmount", "money", 10, 2, nullable: false),
			new DmoField("patCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("patCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("patUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("PATPAYROLLSESSIONID,PATPAYROLLHEADERID", unique: true),
			new DmoIndex("PATUNIQUEID", unique: true),
			new DmoIndex("patPayrollSessionID", unique: false),
			new DmoIndex("patPayrollHeaderID", unique: false),
			new DmoIndex("patPayrollEmployeeID", unique: false),
			new DmoIndex("patPayrollDefinitionID", unique: false),
			new DmoIndex("patPaymentNumber", unique: false),
			new DmoIndex("patCompleted", unique: false),
			new DmoIndex("patVoidedPayment", unique: false),
			new DmoIndex("patVoidedPayrollSessionID", unique: false),
			new DmoIndex("patVoidedPayrollHeaderID", unique: false),
			new DmoIndex("patPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
