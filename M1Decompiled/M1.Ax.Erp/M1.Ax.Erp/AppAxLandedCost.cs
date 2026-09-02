using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.Forms.Controls;

namespace M1.Ax.Erp;

[AxScript("LandedCost")]
[ComVisible(true)]
public class AppAxLandedCost
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxLandedCost(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public DateTime GetLandedCostDate(object transaction, string landedCostID)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new LandedCost().GetLandedCostDate(_Database, (SqlTransaction)transaction, landedCostID);
	}

	public void AddPOLineToLandedCost(object bindingSource, object e)
	{
		M1BindingSource m1BindingSource = (M1BindingSource)bindingSource;
		if (m1BindingSource != null)
		{
			SearchEventArgs e2 = (SearchEventArgs)e;
			new LandedCost().AddPOLineToLandedCost(m1BindingSource, e2.DataRows);
		}
	}

	public void RefreshLandedCostChargeDetails(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		new LandedCost().RefreshLandedCostChargeDetails(bindingsource, transaction);
	}

	public string AddReceiptToLandedCost(object bindingSource, object e)
	{
		M1BindingSource m1BindingSource = (M1BindingSource)bindingSource;
		if (m1BindingSource != null)
		{
			SearchEventArgs e2 = (SearchEventArgs)e;
			return new LandedCost().AddReceiptToLandedCost(m1BindingSource, e2.DataRows);
		}
		return string.Empty;
	}

	public void RefreshReceiptLineDetails(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		new LandedCost().RefreshReceiptLineDetails(bindingsource, transaction);
	}

	public string CreateLandedCostTransitJournals(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		return new LandedCost().CreateLandedCostTransitJournals(bindingsource, transaction);
	}

	public string CreateLandedCostChargesJournals(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		return new LandedCost().CreateLandedCostChargesJournals(bindingsource, transaction);
	}

	public string PostLandedCost(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		return new LandedCost().PostLandedCost(bindingsource, transaction);
	}

	public string AddLandedCostChargeToInvoice(string landedCostID, int chargeID, string invoice, object invoiceDate, int year, int period, bool calcTaxOnFreight, ref string errorMessage, int invoiceType = 0, bool suppressAlerts = false, object transaction = null)
	{
		return new LandedCost().AddLandedCostChargeToInvoice(_Database, (SqlTransaction)transaction, landedCostID, chargeID, invoice, invoiceDate, year, period, calcTaxOnFreight, ref errorMessage, invoiceType, suppressAlerts);
	}

	public string UpdateVarianceExpenseAccounts(string landedCostID, int chargeID, string invoice, int invoiceLine, decimal newExpenseAmount, object transaction = null)
	{
		return new LandedCost().UpdateVarianceExpenseAccounts(_Database, (SqlTransaction)transaction, landedCostID, chargeID, invoice, invoiceLine, newExpenseAmount);
	}
}
