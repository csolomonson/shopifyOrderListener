using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APRecurringPayments to support unicode", "2013-10-17")]
public class v810RebuildAPRecurringPayments
{
	public v810RebuildAPRecurringPayments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", new DmoField[24]
		{
			new DmoField("aprRecurringPaymentID", "int", 6, 0, nullable: false),
			new DmoField("aprPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("aprPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("aprSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("aprAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("aprDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("aprCycleCode", "nvarchar", 5, 0, nullable: false),
			new DmoField("aprRecurrenceInterval", "tinyint", 2, 0, nullable: false),
			new DmoField("aprRecurrenceType", "nvarchar", 1, 0, nullable: false),
			new DmoField("aprLastTransferredDate", "date", 14, 0, nullable: true),
			new DmoField("aprPaymentDay", "tinyint", 2, 0, nullable: false),
			new DmoField("aprStartGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("aprStartGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("aprEndGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("aprEndGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("aprPaymentType", "tinyint", 1, 0, nullable: false),
			new DmoField("aprCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("aprPaymentTotalBase", "money", 12, 2, nullable: false),
			new DmoField("aprPaymentTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("aprInactive", "bit", 1, 0, nullable: false),
			new DmoField("aprInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("aprCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("aprCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("aprUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("APRRECURRINGPAYMENTID", unique: true),
			new DmoIndex("APRUNIQUEID", unique: true),
			new DmoIndex("aprPlantDepartmentID", unique: false),
			new DmoIndex("aprPlantID", unique: false),
			new DmoIndex("aprAPInvoiceLocationID", unique: false),
			new DmoIndex("aprRecurrenceType", unique: false),
			new DmoIndex("aprInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
