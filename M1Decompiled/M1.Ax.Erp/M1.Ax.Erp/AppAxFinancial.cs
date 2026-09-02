using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using M1.Core;
using M1.Core.Script;
using M1.Script.Interfaces;
using M1.ServiceCore.AxScript;
using M1Classes92;

namespace M1.Ax.Erp;

[AxScript("Financial")]
[ComVisible(true)]
public class AppAxFinancial : IDisposable, IWebAxFinancial
{
	private IServiceProvider provider;

	private M1Database databaseRef;

	private CFinancial vbRef;

	private bool? _NET1Activated;

	private bool? _PaypalActivated;

	private bool? _AvalaraActivated;

	public bool NET1Activated
	{
		get
		{
			if (!_NET1Activated.HasValue)
			{
				Financial financial = new Financial();
				_NET1Activated = financial.IsNET1Activated(databaseRef);
			}
			return _NET1Activated.Value;
		}
	}

	public bool PaypalActivated
	{
		get
		{
			if (!_PaypalActivated.HasValue)
			{
				Financial financial = new Financial();
				_PaypalActivated = financial.IsPaypalActivated(databaseRef);
			}
			return _PaypalActivated.Value;
		}
	}

	public bool AvalaraActivated
	{
		get
		{
			if (!_AvalaraActivated.HasValue)
			{
				Financial financial = new Financial();
				_AvalaraActivated = financial.IsAvalaraActivated(databaseRef);
			}
			return _AvalaraActivated.Value;
		}
	}

	public bool ShowSecondTax
	{
		get
		{
			if (!databaseRef.Region.Equals("US", StringComparison.CurrentCultureIgnoreCase))
			{
				return databaseRef.Region.Equals("CAN", StringComparison.CurrentCultureIgnoreCase);
			}
			return true;
		}
	}

	public AppAxFinancial(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		databaseRef = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public YearAndPeriod GetYearAndPeriod(object Value, string Module, bool IgnoreClosed = false)
	{
		return new Financial().GetYearAndPeriod(databaseRef, Value, Module, IgnoreClosed);
	}

	public short GetYear(DateTime Value, string Module)
	{
		return new Financial().GetYearAndPeriod(databaseRef, Value, Module, IgnoreClosed: true).Year;
	}

	public DateTime GetFiscalYearStartDate(object dDate = null)
	{
		DateTime value = ((dDate != null && dDate != DBNull.Value) ? ((DateTime)dDate) : DateTime.Today);
		DateTime startDate = DateTime.Today;
		DateTime endDate = DateTime.Today;
		new Financial().GetFiscalYearDates(databaseRef, value, ref startDate, ref endDate);
		return startDate;
	}

	public DateTime GetFiscalYearEndDate(object dDate = null)
	{
		DateTime value = ((dDate != null && dDate != DBNull.Value) ? ((DateTime)dDate) : DateTime.Today);
		DateTime startDate = DateTime.Today;
		DateTime endDate = DateTime.Today;
		new Financial().GetFiscalYearDates(databaseRef, value, ref startDate, ref endDate);
		return endDate;
	}

	public short GetPeriod(DateTime Value, string Module)
	{
		return new Financial().GetYearAndPeriod(databaseRef, Value, Module, IgnoreClosed: true).Period;
	}

	public bool CalculateDiscountAndDueDate(object dInvoiceDate, string cTermID, ref object dDueDate, ref object dDiscountDate)
	{
		return getRef().CalculateDiscountAndDueDate(dInvoiceDate, cTermID, ref dDueDate, ref dDiscountDate);
	}

	public double CalculateSecondaryTax(double nTotal, double nPrimaryTax, double nRate, bool bIncludePrimaryTax, short nRound = 2)
	{
		return getRef().CalculateSecondaryTax(nTotal, nPrimaryTax, nRate, bIncludePrimaryTax, nRound);
	}

	public double CalculateTaxOnSubTotal(string cTaxID, double nTotal, object dCalcDate, double nPrimaryTax = 0.0, short nRound = 2)
	{
		return getRef().CalculateTaxOnSubTotal(cTaxID, nTotal, dCalcDate, nPrimaryTax, nRound);
	}

	public double CalculateTaxOnTotal(string cTaxID, double nTotal, object dCalcDate, string cSecondTaxID = "", short nRound = 2)
	{
		return getRef().CalculateTaxOnTotal(cTaxID, nTotal, dCalcDate, cSecondTaxID, nRound);
	}

	public double GetExchangeRate(string cCurrencyID, object dCalcDate)
	{
		return getRef().GetExchangeRate(cCurrencyID, dCalcDate);
	}

	private CFinancial getRef()
	{
		if (vbRef == null)
		{
			vbRef = new CFinancialClass();
			vbRef.SetReferences(provider.GetService(typeof(ScriptApp)), provider.GetService(typeof(IForms)));
		}
		return vbRef;
	}

	public void Dispose()
	{
		if (vbRef != null)
		{
			vbRef = null;
		}
		databaseRef = null;
		provider = null;
	}

	private static IEntryForm GetExistingEntryFormARPayment()
	{
		foreach (Form openForm in Application.OpenForms)
		{
			if (openForm is IEntryForm)
			{
				IEntryForm entryForm = (IEntryForm)openForm;
				if (entryForm.IsObjectLoaded("ARPayment"))
				{
					return entryForm;
				}
			}
		}
		return null;
	}

	public void UpdateRibbonMemosARInvoice(string ARInvoiceID)
	{
		SqlCommand sqlCommand = databaseRef.NewSqlCommand("\r\n            Select ariUniqueID as MemoUniqueID,arpUniqueID as LinkUniqueID,'' as LinkDescription, ariARInvoiceID As MemoIDField,'ariARInvoiceID' As MemoCaptionField,ariMemoDate As MemoDate, \r\n            ariShortDescription As ShortDescription, ariLongDescriptionText As LongDescriptionText, ariLongDescriptionRTF As LongDescriptionRTF, 'AR Invoices' as RelatedTableCaption, 'ARInvoices' as RelatedTableName \r\n            From ARInvoiceMemos Inner Join ARInvoices On ariARInvoiceID = arpARInvoiceID Inner Join Organizations On arpCustomerOrganizationID = cmoOrganizationID\r\n            Where(ariARInvoiceID = @ARInvoiceID) And ariShowInARPayments <> 0 Order By MemoDate Desc");
		sqlCommand.Parameters.Add("ARInvoiceID", SqlDbType.NVarChar).Value = ARInvoiceID;
		DataTable dataTable = databaseRef.GetDataTable(sqlCommand);
		GetExistingEntryFormARPayment()?.UpdateRibbonMemos(dataTable.AsEnumerable().ToList(), "ARInvoices", "AR Invoices");
	}
}
