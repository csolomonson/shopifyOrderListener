using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.023", "Add 1099 tables and fields", "2010-05-03")]
public class v800023a
{
	public v800023a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1099Types"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Types");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1099Years"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Years");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1099YearTotals"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099YearTotals");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoForm1099Box"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoForm1099Box", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlForm1099Box"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlForm1099Box", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlForm1099Box"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlForm1099Box", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceLines", "aplForm1099Box"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceLines", "aplForm1099Box", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentHeaders", "aptForm1099Box"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", "aptForm1099Box", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
