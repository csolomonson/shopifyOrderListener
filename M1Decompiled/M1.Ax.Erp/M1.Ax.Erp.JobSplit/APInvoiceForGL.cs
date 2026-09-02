using System;
using System.Collections.Generic;
using System.Data;

namespace M1.Ax.Erp.JobSplit;

public class APInvoiceForGL
{
	public DataRow ApInvoice { get; private set; }

	public string InvoiceId { get; private set; }

	public bool IsCredit { get; private set; }

	public double InvoiceSubTotal { get; private set; }

	public double FreightAmount { get; private set; }

	public double FreightTaxAmount { get; private set; }

	public double SecondFreightTaxAmount { get; private set; }

	public double JobsAmountPortion { get; private set; }

	public double PortionFreightAmount { get; private set; }

	public double PortionFreightTaxAmount { get; private set; }

	public double PortionSecondFreightTaxAmount { get; private set; }

	public double PortionTotal { get; private set; }

	public double FreightIncludePrimaryTax { get; private set; }

	public double PortionFreightIncludePrimaryTax { get; private set; }

	public APInvoiceForGL(DataRow apInvoice)
	{
		ApInvoice = apInvoice;
		InvoiceId = Convert.ToString(apInvoice["appAPInvoiceID"]);
		FreightAmount = Convert.ToDouble(apInvoice["appFreightAmountBase"]);
		InvoiceSubTotal = Convert.ToDouble(apInvoice["appInvoiceSubtotalBase"]);
		FreightTaxAmount = Convert.ToDouble(apInvoice["appFreightTaxAmountBase"]);
		SecondFreightTaxAmount = Convert.ToDouble(apInvoice["appSecondFreightTaxAmtBase"]);
		IsCredit = Convert.ToInt32(apInvoice["appInvoiceType"]) == 2;
	}

	public void SetJobsAmount(double value)
	{
		JobsAmountPortion = Math.Abs(value);
		PortionFreightAmount = Math.Round(JobsAmountPortion / Math.Abs(InvoiceSubTotal) * Math.Abs(FreightAmount), 2);
		PortionFreightTaxAmount = Math.Round(JobsAmountPortion / Math.Abs(InvoiceSubTotal) * Math.Abs(FreightTaxAmount), 2);
		PortionSecondFreightTaxAmount = Math.Round(JobsAmountPortion / Math.Abs(InvoiceSubTotal) * Math.Abs(SecondFreightTaxAmount), 2);
		PortionTotal = PortionFreightAmount + PortionFreightTaxAmount + PortionSecondFreightTaxAmount;
		FreightIncludePrimaryTax = Math.Round(FreightAmount, 2) + Math.Round(FreightTaxAmount, 2);
		PortionFreightIncludePrimaryTax = Math.Round(PortionFreightAmount, 2) + Math.Round(PortionFreightTaxAmount, 2);
	}

	internal void AddTaxesToDictionary(Dictionary<string, double> apTaxLines, int glJournalId)
	{
		if (PortionFreightTaxAmount != 0.0)
		{
			string key = string.Format("{0}-{1}-{2}-Freight", glJournalId, InvoiceId, ApInvoice["appFreightTaxCodeID"]);
			apTaxLines.Add(key, PortionFreightTaxAmount);
		}
		if (PortionSecondFreightTaxAmount != 0.0)
		{
			string key2 = string.Format("{0}-{1}-{2}-Freight", glJournalId, InvoiceId, ApInvoice["appSecondFreightTaxCodeID"]);
			apTaxLines.Add(key2, PortionSecondFreightTaxAmount);
		}
	}
}
