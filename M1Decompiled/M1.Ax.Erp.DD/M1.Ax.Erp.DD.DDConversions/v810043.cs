using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.043", "Update code for references to Forms properties", "")]
public class v810043
{
	public v810043(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[17]
		{
			new TranslateInfo("Forms.Show.ScheduleMoveOperations", "Forms.Ax(\"Jobs\").ShowScheduleMoveOperations", ignoreCase: true),
			new TranslateInfo("Forms.Show.ScheduleJobs", "Forms.Ax(\"Jobs\").ShowScheduleJobs", ignoreCase: true),
			new TranslateInfo("Forms.Show.ScheduleJob", "Forms.Ax(\"Jobs\").ShowScheduleJob", ignoreCase: true),
			new TranslateInfo("Forms.Show.JobWizard", "Forms.Ax(\"Jobs\").ShowJobWizard", ignoreCase: true),
			new TranslateInfo("Forms.Show.SchedulingBoard", "Forms.Ax(\"Jobs\").ShowSchedulingBoard", ignoreCase: true),
			new TranslateInfo("Forms.Show.CallQueue", "Forms.Ax(\"Calls\").ShowCallQueue", ignoreCase: true),
			new TranslateInfo("Forms.Show.QuickCall", "Forms.Ax(\"Calls\").ShowQuickCall", ignoreCase: true),
			new TranslateInfo("Forms.Show.RFQSavePrices", "Forms.Ax(\"Rfqs\").ShowRfqSavePrices", ignoreCase: true),
			new TranslateInfo("Forms.Show.RFQPullQuote", "Forms.Ax(\"Rfqs\").ShowRfqPullQuote", ignoreCase: true),
			new TranslateInfo("Forms.Show.RFQPullJob", "Forms.Ax(\"Rfqs\").ShowRfqPullJob", ignoreCase: true),
			new TranslateInfo("Forms.Show.RFQUpdateSelectedSuppliers", "Forms.Ax(\"Rfqs\").ShowRfqUpdateSelectedSuppliers", ignoreCase: true),
			new TranslateInfo("Forms.Show.CreatePOFromRFQ", "Forms.Ax(\"Rfqs\").ShowCreatePOFromRFQ", ignoreCase: true),
			new TranslateInfo("Forms.Show.CreditCardPaymentNET1Wizard", "Forms.Ax(\"Financial\").CreditCardPaymentNET1Wizard", ignoreCase: true),
			new TranslateInfo("Forms.Show.CreditCardPaymentNET1", "Forms.Ax(\"Financial\").CreditCardPaymentNET1", ignoreCase: true),
			new TranslateInfo("Forms.Show.PurchasingWizard", "Forms.Ax(\"PurchaseOrders\").ShowPurchasingWizard", ignoreCase: true),
			new TranslateInfo("Forms.Show.LeaveBoard", "Forms.Ax(\"Payroll\").ShowLeaveBoard", ignoreCase: true),
			new TranslateInfo("frmCallQueue", "M1.Ax.Erp.Forms.Sales.Call.CallQueueForm", ignoreCase: true)
		});
	}
}
