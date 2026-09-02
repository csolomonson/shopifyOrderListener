using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.AddressService;
using Avalara.AvaTax.Adapter.TaxService;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.Financials.Avalara;

public class AvalaraTaxFunctions
{
	public static class TableFields
	{
		public static string GetField(string table, string fieldType)
		{
			string result = string.Empty;
			switch (fieldType.ToUpper())
			{
			case "CHILDTABLE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "SalesOrderLines" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "ARInvoiceLines" : "QuoteLines"));
				break;
			case "RECORDID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompSalesOrderID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpARInvoiceID" : "qmpQuoteID"));
				break;
			case "LINERECORDID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlSalesOrderID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlARInvoiceID" : "qmlQuoteID"));
				break;
			case "LINEID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlSalesOrderLineID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlARInvoiceLineID" : "qmlQuoteLineID"));
				break;
			case "CUSTOMERORGID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompCustomerOrganizationID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpCustomerOrganizationID" : "qmpCustomerOrganizationID"));
				break;
			case "PARTID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlPartID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlPartID" : "qmlPartID"));
				break;
			case "QUANTITY":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omdDeliveryQuantity" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlInvoiceQuantity" : "QuoteQuantities"));
				break;
			case "LINEQUANTITY":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlOrderQuantity" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlInvoiceQuantity" : "QuoteQuantities"));
				break;
			case "PARTDESCRIPTION":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlPartShortDescription" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlPartShortDescription" : "qmlPartShortDescription"));
				break;
			case "UNITPRICEBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlUnitPriceBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlUnitPriceBase" : "QuoteUnitPriceBase"));
				break;
			case "LINEFREIGHTBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlFreightAmountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlFreightAmountBase" : string.Empty));
				break;
			case "HEADERFREIGHTBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompFreightAmountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpFreightAmountBase" : string.Empty));
				break;
			case "PLANTID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompPlantID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpPlantID" : "qmpPlantID"));
				break;
			case "WAREHOUSEID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omdPartWarehouseLocationID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "smlPartWarehouseLocationID" : string.Empty));
				break;
			case "NONTAXREASONID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omdAvalaraNonTaxReasonID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlNonTaxReasonID" : "qmlNonTaxReasonID"));
				break;
			case "TAXCODEID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlTaxCodeID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlTaxCodeID" : "qmlTaxCodeID"));
				break;
			case "TAXAMOUNTBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlTaxAmountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlTaxAmountBase" : string.Empty));
				break;
			case "TAXAMOUNTFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlTaxAmountForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlTaxAmountForeign" : string.Empty));
				break;
			case "SECONDTAXCODEID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlSecondTaxCodeID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlSecondTaxCodeID" : "qmlSecondTaxCodeID"));
				break;
			case "SECONDTAXAMOUNTBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlSecondTaxAmountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlSecondTaxAmountBase" : string.Empty));
				break;
			case "SECONDTAXAMOUNTFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlSecondTaxAmountForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlSecondTaxAmountForeign" : string.Empty));
				break;
			case "TRANSACTIONDATE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderDate" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoiceDate" : "qmpQuoteDate"));
				break;
			case "DISCOUNTAMOUNT":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlUnitDiscountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlUnitDiscountBase" : string.Empty));
				break;
			case "FREIGHTTAXCODE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompFreightTaxCodeID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpFreightTaxCodeID" : string.Empty));
				break;
			case "FREIGHTTAXAMOUNTBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompFreightTaxAmountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpFreightTaxAmountBase" : string.Empty));
				break;
			case "FREIGHTTAXAMOUNTFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompFreightTaxAmountForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpFreightTaxAmountForeign" : string.Empty));
				break;
			case "FREIGHTSECONDTAXCODE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompSecondFreightTaxCodeID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpSecondFreightTaxCodeID" : string.Empty));
				break;
			case "FREIGHTSECONDTAXAMOUNTBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompSecondFreightTaxAmtBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpSecondFreightTaxAmtBase" : string.Empty));
				break;
			case "FREIGHTSECONDTAXAMOUNTFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompSecondFreightTaxAmtForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpSecondFreightTaxAmtForeign" : string.Empty));
				break;
			case "FREIGHTTOTALBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompFreightTotalBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpFreightTotalBase" : string.Empty));
				break;
			case "FREIGHTTOTALFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompFreightTotalForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpFreightTotalForeign" : string.Empty));
				break;
			case "SUBTOTALBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderSubtotalBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoiceSubtotalBase" : string.Empty));
				break;
			case "SUBTOTALFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderSubtotalForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoiceSubtotalForeign" : string.Empty));
				break;
			case "TOTALTAXBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderTaxAmountBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoiceTaxAmountBase" : string.Empty));
				break;
			case "TOTALTAXFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderTaxAmountForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoieTaxAmountForeign" : string.Empty));
				break;
			case "TOTALBASE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderTotalBase" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoiceTotalBsae" : string.Empty));
				break;
			case "TOTALFOREIGN":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompOrderTotalForeign" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpInvoiceTotalForeign" : string.Empty));
				break;
			case "TAXCALCULATED":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompTaxCalculated" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpTaxCalculated" : "qmpTaxCalculated"));
				break;
			case "BASESHIPORGID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompShipOrganizationID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpShipOrganizationID" : "qmpShipOrganizationID"));
				break;
			case "BASESHIPLOCID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompShipLocationID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpShipLocationID" : "qmpShipLocationID"));
				break;
			case "OTHERSHIPORGID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omdCustomerOrganizationID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "smpShipOrganizationID" : "qmpShipOrganizationID"));
				break;
			case "OTHERSHIPLOCID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omdShipLocationID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "smpShipLocationID" : "qmpShipLocationID"));
				break;
			case "CUSTOMERPO":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompCustomerPO" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlCustomerPO" : string.Empty));
				break;
			case "IGNORELINE":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "omlAvalaraIgnoreLine" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arlAvalaraIgnoreLine" : string.Empty));
				break;
			case "UNIQUEID":
				result = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "ompUniqueID" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "arpUniqueID" : "qmpUniqueID"));
				break;
			}
			return result;
		}
	}

	public enum AvalaraTransactionType : byte
	{
		Ping = 1,
		ValidateAddress,
		GetTax,
		PostTax,
		CancelTax
	}

	public M1Database Database;

	public M1User User;

	public TaxSvc TaxSvc;

	public AvalaraTaxFunctions(M1Database m1Database, M1User m1User)
	{
		Database = m1Database;
		User = m1User;
	}

	public TaxSvc CreateTaxSvcConfig()
	{
		if (TaxSvc == null)
		{
			TaxSvc taxSvc = new TaxSvc();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			taxSvc.Profile.Client = "a0o0b00000523Vc";
			taxSvc.Configuration.Url = Database.Props("FN").Field<string>("xafAvalaraURL").Trim();
			taxSvc.Configuration.Security.Account = Database.Props("FN").Field<string>("xafAvalaraAccountID").Trim();
			taxSvc.Configuration.Security.License = Database.Props("FN").Field<string>("xafAvalaraLicenseKey").Trim();
			taxSvc.Configuration.Security.UserName = "";
			taxSvc.Configuration.Security.Password = "";
			taxSvc.Configuration.RequestTimeout = ((Database.Props("FN").Field<short>("xafAvalaraTimeoutSeconds") > 0) ? Convert.ToInt32(Database.Props("FN").Field<short>("xafAvalaraTimeoutSeconds")) : 100);
			TaxSvc = taxSvc;
		}
		return TaxSvc;
	}

	public string PingInterface(string url, string account, string license, int timeout)
	{
		TaxSvc taxSvc = new TaxSvc();
		taxSvc.Configuration.Url = url;
		taxSvc.Configuration.Security.Account = account;
		taxSvc.Configuration.Security.License = license;
		taxSvc.Configuration.Security.UserName = "";
		taxSvc.Configuration.Security.Password = "";
		taxSvc.Configuration.RequestTimeout = ((timeout > 0) ? timeout : 100);
		try
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			PingResult pingResult = taxSvc.Ping("");
			AddAvalaraTransaction(pingResult);
			if (pingResult.ResultCode >= SeverityLevel.Error)
			{
				return pingResult.Messages[0].Summary;
			}
			IsAuthorizedResult isAuthorizedResult = taxSvc.IsAuthorized("GetTax, PostTax, CommitTax, CancelTax, AdjustTax");
			if (isAuthorizedResult.ResultCode >= SeverityLevel.Error)
			{
				return isAuthorizedResult.Messages[0].Summary;
			}
			return "Result Code: " + isAuthorizedResult.ResultCode.ToString() + "\r\n# Messages: " + isAuthorizedResult.Messages.Count + "\r\n# Expires: " + isAuthorizedResult.Expires.ToString() + "\r\n# Operations: " + isAuthorizedResult.Operations.Replace(",", ", ") + "\r\n# Service Version: " + pingResult.Version;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public string PingInterface()
	{
		TaxSvc taxSvc = CreateTaxSvcConfig();
		try
		{
			PingResult pingResult = taxSvc.Ping("");
			AddAvalaraTransaction(pingResult);
			if (pingResult.ResultCode >= SeverityLevel.Error)
			{
				return pingResult.Messages[0].Summary;
			}
			IsAuthorizedResult isAuthorizedResult = taxSvc.IsAuthorized("GetTax, PostTax, CommitTax, CancelTax, AdjustTax, GetTaxHistory, ReconcileTaxHistory");
			if (isAuthorizedResult.ResultCode >= SeverityLevel.Error)
			{
				return isAuthorizedResult.Messages[0].Summary;
			}
			return "Result Code: " + isAuthorizedResult.ResultCode.ToString() + "\r\n# Messages: " + isAuthorizedResult.Messages.Count + "\r\n# Expires: " + isAuthorizedResult.Expires.ToString() + "\r\n# Operations: " + isAuthorizedResult.Operations.Replace(",", ", ") + "\r\n# Service Version: " + pingResult.Version;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public string GetTax(string table, string recordID, bool postToAvalara, M1BindingSource bs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		TaxSvc taxSvc = CreateTaxSvcConfig();
		GetTaxRequest getTaxRequest = SetGetTaxRequest(table, recordID, postToAvalara);
		if (getTaxRequest != null && getTaxRequest.Lines.Count > 0)
		{
			GetTaxResult tax = taxSvc.GetTax(getTaxRequest);
			if (tax.ResultCode == SeverityLevel.Success || tax.ResultCode == SeverityLevel.Warning)
			{
				updateTax(tax, table, recordID, bs);
			}
			AddAvalaraTransaction(table, recordID, tax);
			if (tax.ResultCode != SeverityLevel.Success)
			{
				stringBuilder.AppendLine(tax.ResultCode.ToString() + ": ");
				if (tax.Messages[0].Summary.Length > 0)
				{
					stringBuilder.AppendLine(tax.Messages[0].Summary);
				}
			}
		}
		else
		{
			stringBuilder.AppendLine("Error constructing GetTaxRequest.");
		}
		return stringBuilder.ToString();
	}

	private bool updateTax(GetTaxResult getTaxResult, string table, string recordID, M1BindingSource bs)
	{
		switch (table.ToUpper())
		{
		case "QUOTES":
			updateQuoteTax(getTaxResult, recordID, bs);
			break;
		case "SALESORDERS":
			updateSalesOrderTax(getTaxResult, recordID, bs);
			break;
		case "ARINVOICES":
			updateARInvoiceTax(getTaxResult, recordID, bs);
			break;
		}
		return true;
	}

	private bool updateQuoteTax(GetTaxResult getTaxResult, string recordID, M1BindingSource bs)
	{
		M1BindingSource childBindingSource = bs.PrimaryTable.GetChildBindingSource("QuoteLines");
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		bool flag = false;
		if (!Database.Region.Equals("CAN", StringComparison.CurrentCultureIgnoreCase))
		{
			foreach (DataRow row in childBindingSource.GetDataTable().Rows)
			{
				TaxLine itemByNo = getTaxResult.TaxLines.GetItemByNo(row["qmlQuoteLineID"].ToString());
				if (itemByNo == null)
				{
					continue;
				}
				row["qmlSecondTaxCodeID"] = string.Empty;
				if (itemByNo.Taxable == 0m)
				{
					row["qmlTaxCodeID"] = string.Empty;
					num = default(decimal);
				}
				else
				{
					row["qmlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
					num = Convert.ToDecimal(itemByNo.Rate);
				}
				foreach (DataRow row2 in childBindingSource.PrimaryTable.GetChildBindingSource("QuoteQuantities").GetDataTable().Rows)
				{
					if (row["qmlQuoteLineID"].ToString() == row2["qmqQuoteLineID"].ToString())
					{
						row2["qmqUnitTaxAmountForeign"] = M1Math.Round(row2.Field<decimal>("qmqRevisedUnitPriceForeign") * num, 4);
						row2["qmqUnitSecondTaxAmountForeign"] = M1Math.Round(row2.Field<decimal>("qmqRevisedUnitPriceForeign") * num2, 4);
						row2["qmqUnitTaxAmountBase"] = M1Math.Round(row2.Field<decimal>("qmqRevisedUnitPriceBase") * num, 4);
						row2["qmqUnitSecondTaxAmountBase"] = M1Math.Round(row2.Field<decimal>("qmqRevisedUnitPriceBase") * num2, 4);
					}
				}
			}
		}
		else
		{
			foreach (DataRow row3 in childBindingSource.GetDataTable().Rows)
			{
				TaxLine itemByNo2 = getTaxResult.TaxLines.GetItemByNo(row3["qmlQuoteLineID"].ToString());
				if (itemByNo2 == null)
				{
					continue;
				}
				row3["qmlSecondTaxCodeID"] = string.Empty;
				if (itemByNo2.Taxable == 0m)
				{
					row3["qmlTaxCodeID"] = string.Empty;
					num = default(decimal);
				}
				else
				{
					flag = false;
					foreach (TaxDetail taxDetail in itemByNo2.TaxDetails)
					{
						string text = taxDetail.TaxName.Trim();
						if (text.IndexOf("GST") >= 0)
						{
							row3["qmlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
							num = Convert.ToDecimal(taxDetail.Rate);
							flag = true;
						}
						if (text.IndexOf("PST") >= 0)
						{
							row3["qmlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
							num2 = Convert.ToDecimal(taxDetail.Rate);
							flag = true;
						}
						if (text.IndexOf("HST") >= 0)
						{
							row3["qmlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
							num2 = Convert.ToDecimal(taxDetail.Rate);
							flag = true;
						}
						if (text.IndexOf("QST") >= 0)
						{
							row3["qmlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
							num2 = Convert.ToDecimal(taxDetail.Rate);
							flag = true;
						}
					}
					if (!flag)
					{
						row3["qmlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
						row3["qmlSecondTaxCodeID"] = string.Empty;
						num = Convert.ToDecimal(itemByNo2.Rate);
					}
				}
				foreach (DataRow row4 in childBindingSource.PrimaryTable.GetChildBindingSource("QuoteQuantities").GetDataTable().Rows)
				{
					row4["qmqUnitTaxAmountForeign"] = M1Math.Round(row4.Field<decimal>("qmqRevisedUnitPriceForeign") * num, 4);
					row4["qmqUnitSecondTaxAmountForeign"] = M1Math.Round(row4.Field<decimal>("qmqRevisedUnitPriceForeign") * num2, 4);
					row4["qmqUnitTaxAmountBase"] = M1Math.Round(row4.Field<decimal>("qmqRevisedUnitPriceBase") * num, 4);
					row4["qmqUnitSecondTaxAmountBase"] = M1Math.Round(row4.Field<decimal>("qmqRevisedUnitPriceBase") * num2, 4);
				}
			}
		}
		bs.CurrentAsDataRow["qmpAvalaraTaxCalculated"] = true;
		bs.SaveData();
		return true;
	}

	private bool updateSalesOrderTax(GetTaxResult getTaxResult, string recordID, M1BindingSource bs)
	{
		M1BindingSource childBindingSource = bs.PrimaryTable.GetChildBindingSource("SalesOrderLines");
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		string value = string.Empty;
		string value2 = string.Empty;
		bool flag = false;
		int num3 = 0;
		if (!Database.Region.Equals("CAN", StringComparison.CurrentCultureIgnoreCase))
		{
			foreach (TaxLine taxLine3 in getTaxResult.TaxLines)
			{
				if (taxLine3.No.EndsWith("-HF") || taxLine3.No.EndsWith("-F"))
				{
					num += taxLine3.Tax;
					value = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
				}
			}
			foreach (DataRow row in childBindingSource.GetDataTable().Rows)
			{
				foreach (DataRow row2 in childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries").GetDataTable().Rows)
				{
					TaxLine itemByNo = getTaxResult.TaxLines.GetItemByNo(row2["omdSalesOrderLineID"].ToString() + "-" + row2["omdSalesOrderDeliveryID"].ToString());
					if (itemByNo == null)
					{
						continue;
					}
					int num4 = 0;
					num3 = itemByNo.No.IndexOf('-');
					num4 = ((num3 != -1) ? Convert.ToInt16(itemByNo.No.Substring(0, num3)) : Convert.ToInt16(itemByNo.No));
					if (num4 == Convert.ToInt16(row["omlSalesOrderLineID"]))
					{
						if (string.IsNullOrWhiteSpace(row["omlNonTaxReasonID"].ToString()))
						{
							row["omlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
							row["omlSecondTaxCodeID"] = string.Empty;
						}
						row["omlTaxAmountForeign"] = M1Math.Round(itemByNo.Tax, 4);
						row["omlSecondTaxAmountForeign"] = 0;
					}
				}
			}
		}
		else
		{
			foreach (TaxLine taxLine4 in getTaxResult.TaxLines)
			{
				if (!taxLine4.No.EndsWith("-HF") && !taxLine4.No.EndsWith("-F"))
				{
					continue;
				}
				flag = false;
				foreach (TaxDetail taxDetail3 in taxLine4.TaxDetails)
				{
					string text = taxDetail3.TaxName.Trim();
					num3 = text.IndexOf("GST");
					if (num3 >= 0)
					{
						value = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
						num += Math.Round(taxDetail3.Tax, 4);
						flag = true;
					}
					num3 = text.IndexOf("PST");
					if (num3 >= 0)
					{
						value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
						num2 += Math.Round(taxDetail3.Tax, 4);
						flag = true;
					}
					num3 = text.IndexOf("HST");
					if (num3 >= 0)
					{
						value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
						num2 += Math.Round(taxDetail3.Tax, 4);
						flag = true;
					}
					num3 = text.IndexOf("QST");
					if (num3 >= 0)
					{
						value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
						num2 += Math.Round(taxDetail3.Tax, 4);
						flag = true;
					}
				}
				if (!flag)
				{
					num += taxLine4.Tax;
					value = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
				}
			}
			foreach (DataRow row3 in childBindingSource.GetDataTable().Rows)
			{
				foreach (DataRow row4 in childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries").GetDataTable().Rows)
				{
					TaxLine itemByNo2 = getTaxResult.TaxLines.GetItemByNo(row4["omdSalesOrderLineID"].ToString() + "-" + row4["omdSalesOrderDeliveryID"].ToString());
					if (itemByNo2 == null)
					{
						continue;
					}
					int num5 = 0;
					num3 = itemByNo2.No.IndexOf('-');
					num5 = ((num3 != -1) ? Convert.ToInt16(itemByNo2.No.Substring(0, num3)) : Convert.ToInt16(itemByNo2.No));
					if (num5 != Convert.ToInt16(row3["omlSalesOrderLineID"]))
					{
						continue;
					}
					flag = false;
					foreach (TaxDetail taxDetail4 in itemByNo2.TaxDetails)
					{
						string text2 = taxDetail4.TaxName.Trim();
						num3 = text2.IndexOf("GST");
						if (num3 >= 0)
						{
							row3["omlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
							row3["omlTaxAmountForeign"] = Math.Round(taxDetail4.Tax, 4);
							flag = true;
						}
						num3 = text2.IndexOf("PST");
						if (num3 >= 0)
						{
							row3["omlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
							row3["omlSecondTaxAmountForeign"] = Math.Round(taxDetail4.Tax, 4);
							flag = true;
						}
						num3 = text2.IndexOf("HST");
						if (num3 >= 0)
						{
							row3["omlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
							row3["omlSecondTaxAmountForeign"] = Math.Round(taxDetail4.Tax, 4);
							flag = true;
						}
						num3 = text2.IndexOf("QST");
						if (num3 >= 0)
						{
							row3["omlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
							row3["omlSecondTaxAmountForeign"] = Math.Round(taxDetail4.Tax, 4);
							flag = true;
						}
					}
					if (!flag)
					{
						row3["omlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
						row3["omlTaxAmountForeign"] = Math.Round(itemByNo2.Tax, 4);
						row3["omlSecondTaxCodeID"] = string.Empty;
						row3["omlSecondTaxAmountForeign"] = 0;
						flag = true;
					}
				}
			}
		}
		bs.CurrentAsDataRow["ompFreightTaxCodeID"] = value;
		bs.CurrentAsDataRow["ompSecondFreightTaxCodeID"] = value2;
		bs.CurrentAsDataRow["ompFreightTaxAmountForeign"] = M1Math.Round(num, 4);
		bs.CurrentAsDataRow["ompSecondFreightTaxAmtForeign"] = M1Math.Round(num2, 4);
		bs.CurrentAsDataRow["ompAvalaraTaxCalculated"] = true;
		bs.SaveData();
		return true;
	}

	private bool updateARInvoiceTax(GetTaxResult getTaxResult, string recordID, M1BindingSource bs)
	{
		M1BindingSource childBindingSource = bs.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		string value = string.Empty;
		string value2 = string.Empty;
		bool flag = false;
		if (!Database.Region.Equals("CAN", StringComparison.CurrentCultureIgnoreCase))
		{
			foreach (TaxLine taxLine3 in getTaxResult.TaxLines)
			{
				if (taxLine3.No.EndsWith("-HF") || taxLine3.No.EndsWith("-F"))
				{
					num += taxLine3.Tax;
					value = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
				}
			}
			foreach (DataRow row in childBindingSource.GetDataTable().Rows)
			{
				TaxLine itemByNo = getTaxResult.TaxLines.GetItemByNo(row["arlARInvoiceLineID"].ToString());
				if (itemByNo == null)
				{
					continue;
				}
				if (string.IsNullOrWhiteSpace(row["arlNonTaxReasonID"].ToString()))
				{
					row["arlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
					row["arlSecondTaxCodeID"] = string.Empty;
				}
				row["arlTaxAmountForeign"] = M1Math.Round(itemByNo.Tax, 4);
				row["arlSecondTaxAmountForeign"] = 0;
				if (itemByNo.TaxIncluded && Database.Props("FN").Field<bool>("xafARCalculateTaxOnDeposit"))
				{
					row["arlFullUnitPriceBase"] = Math.Round(itemByNo.Taxable, 2);
					row["arlInvoiceQuantity"] = 1;
					if (string.IsNullOrWhiteSpace(row["arlNonTaxReasonID"].ToString()))
					{
						row["arlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
						row["arlSecondTaxCodeID"] = string.Empty;
					}
					row["arlTaxAmountForeign"] = Math.Round(itemByNo.Tax, 4);
					row["arlSecondTaxAmountForeign"] = 0;
				}
			}
		}
		else
		{
			foreach (TaxLine taxLine4 in getTaxResult.TaxLines)
			{
				if (!taxLine4.No.EndsWith("-HF") && !taxLine4.No.EndsWith("-F"))
				{
					continue;
				}
				flag = false;
				foreach (TaxDetail taxDetail4 in taxLine4.TaxDetails)
				{
					string text = taxDetail4.TaxName.Trim();
					if (text.IndexOf("GST") >= 0)
					{
						value = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
						num += Math.Round(taxDetail4.Tax, 4);
						flag = true;
					}
					if (text.IndexOf("PST") >= 0)
					{
						value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
						num2 += Math.Round(taxDetail4.Tax, 4);
						flag = true;
					}
					if (text.IndexOf("HST") >= 0)
					{
						value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
						num2 += Math.Round(taxDetail4.Tax, 4);
						flag = true;
					}
					if (text.IndexOf("QST") >= 0)
					{
						value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
						num2 += Math.Round(taxDetail4.Tax, 4);
						flag = true;
					}
				}
				if (!flag)
				{
					num += taxLine4.Tax;
					value = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
				}
			}
			foreach (DataRow row2 in childBindingSource.GetDataTable().Rows)
			{
				TaxLine itemByNo2 = getTaxResult.TaxLines.GetItemByNo(row2["arlARInvoiceLineID"].ToString());
				if (itemByNo2 == null)
				{
					continue;
				}
				flag = false;
				foreach (TaxDetail taxDetail5 in itemByNo2.TaxDetails)
				{
					string text2 = taxDetail5.TaxName.Trim();
					if (text2.IndexOf("GST") >= 0)
					{
						row2["arlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
						row2["arlTaxAmountForeign"] = Math.Round(taxDetail5.Tax, 4);
						flag = true;
					}
					if (text2.IndexOf("PST") >= 0)
					{
						row2["arlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
						row2["arlSecondTaxAmountForeign"] = Math.Round(taxDetail5.Tax, 4);
						flag = true;
					}
					if (text2.IndexOf("HST") >= 0)
					{
						row2["arlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
						row2["arlSecondTaxAmountForeign"] = Math.Round(taxDetail5.Tax, 4);
						flag = true;
					}
					if (text2.IndexOf("QST") >= 0)
					{
						row2["arlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
						row2["arlSecondTaxAmountForeign"] = Math.Round(taxDetail5.Tax, 4);
						flag = true;
					}
				}
				if (!flag)
				{
					row2["arlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
					row2["arlSecondTaxCodeID"] = string.Empty;
					row2["arlTaxAmountForeign"] = Math.Round(itemByNo2.Tax, 4);
					row2["arlSecondTaxAmountForeign"] = 0;
					flag = true;
				}
				if (!itemByNo2.TaxIncluded || !Database.Props("FN").Field<bool>("xafARCalculateTaxOnDeposit"))
				{
					continue;
				}
				row2["arlFullUnitPriceBase"] = Math.Round(itemByNo2.Taxable, 2);
				row2["arlInvoiceQuantity"] = 1;
				flag = false;
				foreach (TaxDetail taxDetail6 in itemByNo2.TaxDetails)
				{
					string text3 = taxDetail6.TaxName.Trim();
					if (text3.IndexOf("GST") >= 0)
					{
						row2["arlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
						row2["arlTaxAmountForeign"] = Math.Round(taxDetail6.Tax, 4);
						flag = true;
					}
					if (text3.IndexOf("PST") >= 0)
					{
						row2["arlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
						row2["arlSecondTaxAmountForeign"] = Math.Round(taxDetail6.Tax, 4);
						flag = true;
					}
					if (text3.IndexOf("HST") >= 0)
					{
						row2["arlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
						row2["arlSecondTaxAmountForeign"] = Math.Round(taxDetail6.Tax, 4);
						flag = true;
					}
					if (text3.IndexOf("QST") >= 0)
					{
						row2["arlSecondTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
						row2["arlSecondTaxAmountForeign"] = Math.Round(taxDetail6.Tax, 4);
						flag = true;
					}
				}
				if (!flag)
				{
					row2["arlTaxCodeID"] = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
					row2["arlSecondTaxCodeID"] = string.Empty;
					row2["arlTaxAmountForeign"] = Math.Round(itemByNo2.Tax, 4);
					row2["arlSecondTaxAmountForeign"] = 0;
				}
			}
		}
		bs.CurrentAsDataRow["arpFreightTaxCodeID"] = value;
		bs.CurrentAsDataRow["arpSecondFreightTaxCodeID"] = value2;
		bs.CurrentAsDataRow["arpFreightTaxAmountForeign"] = M1Math.Round(num, 4);
		bs.CurrentAsDataRow["arpSecondFreightTaxAmtForeign"] = M1Math.Round(num2, 4);
		bs.CurrentAsDataRow["arpAvalaraTaxCalculated"] = true;
		bs.SaveData();
		return true;
	}

	private GetTaxRequest SetGetTaxRequest(string table, string recordID, bool postToAvalara)
	{
		string empty = string.Empty;
		bool flag = true;
		bool isDepositInvoice = false;
		if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			using SqlCommand sqlCommand = Database.NewSqlCommand("Select arpARInvoiceID, arpInvoiceType From ARInvoices Where arpARInvoiceID = @recordID");
			sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10)).Value = recordID;
			DataTable dataTable = Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				isDepositInvoice = dataTable.Rows[0].Field<byte>("arpInvoiceType") == 3;
			}
		}
		empty = setQueryString(table, isDepositInvoice);
		SqlCommand sqlCommand2 = Database.NewSqlCommand(empty);
		sqlCommand2.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10)).Value = recordID;
		DataTable dataTable2 = Database.GetDataTable(sqlCommand2);
		if (Database.Props("FN").Field<bool>("xafAvalaraForceAddressValidate"))
		{
			flag = forceAddressValidate(dataTable2, table, recordID);
			if (flag)
			{
				dataTable2 = Database.GetDataTable(sqlCommand2);
			}
		}
		if (flag)
		{
			GetTaxRequest getTaxRequest = new GetTaxRequest();
			if (dataTable2.Rows.Count > 0)
			{
				string text = dataTable2.Rows[0].Field<string>(TableFields.GetField(table, "PlantID"));
				string text2 = (table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) ? string.Empty : dataTable2.Rows[0].Field<string>(TableFields.GetField(table, "WarehouseID")));
				getTaxRequest.OriginAddress = getOriginAddress(text, text2);
				getTaxRequest.DestinationAddress = getDestinationAddress(dataTable2.Rows[0], useBaseLocation: true);
				getTaxRequest.Commit = postToAvalara;
				getTaxRequest.CompanyCode = Database.Props("FN").Field<string>("xafAvalaraCompanyCode");
				getTaxRequest.CustomerCode = dataTable2.Rows[0].Field<string>(TableFields.GetField(table, "CustomerOrgID"));
				getTaxRequest.DetailLevel = DetailLevel.Tax;
				getTaxRequest.DocCode = (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? "SO-" : (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "AR-" : "QO-")) + dataTable2.Rows[0].Field<string>(TableFields.GetField(table, "RecordID"));
				getTaxRequest.DocDate = dataTable2.Rows[0].Field<DateTime>(TableFields.GetField(table, "TransactionDate"));
				if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) && dataTable2.Rows[0].Field<byte>("arpInvoiceType") == 2 && dataTable2.Rows[0].Field<DateTime>("arpCreditDate") != dataTable2.Rows[0].Field<DateTime>(TableFields.GetField(table, "TransactionDate")))
				{
					getTaxRequest.TaxOverride.TaxOverrideType = TaxOverrideType.TaxDate;
					getTaxRequest.TaxOverride.TaxDate = dataTable2.Rows[0].Field<DateTime>("arpCreditDate");
					getTaxRequest.TaxOverride.Reason = "Credit Date and Invoice Date Different";
				}
				getTaxRequest.DocType = (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? ((dataTable2.Rows[0].Field<byte>("arpInvoiceType") != 2) ? DocumentType.SalesInvoice : DocumentType.ReturnInvoice) : DocumentType.SalesOrder);
				getTaxRequest.LocationCode = dataTable2.Rows[0].Field<string>(TableFields.GetField(table, "CustomerOrgID"));
				getTaxRequest.PurchaseOrderNo = ((!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase)) ? ("PO-" + dataTable2.Rows[0].Field<string>(TableFields.GetField(table, "CustomerPO")).Trim()) : string.Empty);
				getTaxRequest.ReferenceCode = string.Empty;
				getTaxRequest.SalespersonCode = getSalespeople(table, recordID);
				getTaxRequest.ServiceMode = ServiceMode.Remote;
				decimal num = 1m;
				if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && dataTable2.Rows[0].Field<decimal>(TableFields.GetField(table, "HeaderFreightBase")) != 0m)
				{
					num = getTotalQty(table, recordID);
					if (num == 0m)
					{
						num = 1m;
					}
				}
				foreach (DataRow row in dataTable2.Rows)
				{
					Line line = null;
					if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) && row.Field<byte>("arpInvoiceType") != 3 && row.Field<bool>("arlDepositLine"))
					{
						line = setDepositLine(row, table, text, text2);
						getTaxRequest.Lines.Add(line);
					}
					else
					{
						line = setRecordLine(row, table, text, text2);
						getTaxRequest.Lines.Add(line);
					}
					if (!table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) || row.Field<byte>("arpInvoiceType") != 3)
					{
						if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && row.Field<decimal>(TableFields.GetField(table, "LineFreightBase")) != 0m)
						{
							Line line2 = setRecordFreightLine(row, table, line);
							getTaxRequest.Lines.Add(line2);
						}
						if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && row.Field<decimal>(TableFields.GetField(table, "HeaderFreightBase")) != 0m)
						{
							Line line3 = setHeaderFreightLine(row, table, line, num);
							getTaxRequest.Lines.Add(line3);
						}
					}
				}
			}
			return getTaxRequest;
		}
		return null;
	}

	private bool forceAddressValidate(DataTable dataTable, string table, string recordID)
	{
		if (dataTable.Rows.Count > 0)
		{
			string text = dataTable.Rows[0].Field<string>(TableFields.GetField(table, "PlantID"));
			string text2 = (table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) ? string.Empty : dataTable.Rows[0].Field<string>(TableFields.GetField(table, "WarehouseID")));
			Address originAddress = getOriginAddress(text, text2);
			AvalaraAddressFunctions avalaraAddressFunctions = new AvalaraAddressFunctions(Database, User);
			AvalaraAddressFunctions.AddressInfo addressInfo = avalaraAddressFunctions.ValidateAddresses(originAddress, table, recordID);
			if (addressInfo != null)
			{
				if (addressInfo.MessageSummary.Trim().Length > 0)
				{
					return false;
				}
				if (addressInfo.Updated)
				{
					if (text2.Trim().Length > 0)
					{
						SqlCommand sqlCommand = Database.NewSqlCommand("Update Warehouses Set imwAvalaraAddressValidated = -1, imwAddressLine1 = @line1, imwAddressLine2 = @line2, imwAddressLine3 = @line3, imwCity = @city, imwState = @state, imwPostCode = @postcode, imwCountry = @country Where imwWarehouseID = @warehouseID");
						sqlCommand.Parameters.Add(new SqlParameter("@warehouseID", SqlDbType.Char, 5)).Value = text2;
						sqlCommand.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar)).Value = addressInfo.Line1;
						sqlCommand.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar)).Value = addressInfo.Line2;
						sqlCommand.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar)).Value = addressInfo.Line3;
						sqlCommand.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar)).Value = addressInfo.City;
						sqlCommand.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar)).Value = addressInfo.Region;
						sqlCommand.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar)).Value = addressInfo.PostalCode;
						sqlCommand.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar)).Value = addressInfo.Country;
						Database.ExecuteCommand(sqlCommand);
					}
					else if (text.Trim().Length > 0)
					{
						SqlCommand sqlCommand2 = Database.NewSqlCommand("Update Plants Set xauAvalaraAddressValidated = -1, xauAddressLine1 = @line1, xauAddressLine2 = @line2, xauAddressLine3 = @line3, xauCity = @city, xauState = @state, xauPostCode = @postcode, xauCountry = @country Where xauPlantID = @plantID");
						sqlCommand2.Parameters.Add(new SqlParameter("@plantID", SqlDbType.Char, 5)).Value = text;
						sqlCommand2.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar)).Value = addressInfo.Line1;
						sqlCommand2.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar)).Value = addressInfo.Line2;
						sqlCommand2.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar)).Value = addressInfo.Line3;
						sqlCommand2.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar)).Value = addressInfo.City;
						sqlCommand2.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar)).Value = addressInfo.Region;
						sqlCommand2.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar)).Value = addressInfo.PostalCode;
						sqlCommand2.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar)).Value = addressInfo.Country;
						Database.ExecuteCommand(sqlCommand2);
					}
					else
					{
						SqlCommand sqlCommand3 = Database.NewSqlCommand("Update DatasetProperties Set xadAddressLine1 = @line1, xadAddressLine2 = @line2, xadAddressLine3 = @line3, xadCity = @city, xadState = @state, xadPostCode = @postcode, xadCountry = @country");
						sqlCommand3.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar)).Value = addressInfo.Line1;
						sqlCommand3.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar)).Value = addressInfo.Line2;
						sqlCommand3.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar)).Value = addressInfo.Line3;
						sqlCommand3.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar)).Value = addressInfo.City;
						sqlCommand3.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar)).Value = addressInfo.Region;
						sqlCommand3.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar)).Value = addressInfo.PostalCode;
						sqlCommand3.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar)).Value = addressInfo.Country;
						Database.ExecuteCommand(sqlCommand3);
					}
				}
			}
			foreach (DataRow row in dataTable.Rows)
			{
				Address address = new Address();
				address = (table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) ? getDestinationAddress(row, useBaseLocation: true) : getDestinationAddress(row, useBaseLocation: false));
				addressInfo = avalaraAddressFunctions.ValidateAddresses(address, table, recordID);
				if (addressInfo == null)
				{
					continue;
				}
				if (addressInfo.MessageSummary.Trim().Length > 0)
				{
					return false;
				}
				if (!addressInfo.Updated)
				{
					continue;
				}
				SqlCommand sqlCommand4 = Database.NewSqlCommand("Update OrganizationLocations Set cmlAvalaraAddressValidated = -1,  cmlAddressLine1 = @line1, cmlAddressLine2 = @line2, cmlAddressLine3 = @line3, cmlCity = @city, cmlCounty = @county, cmlState = @state, cmlPostCode = @postcode, cmlCountry = @country Where cmlOrganizationID = @orgID And cmlLocationID = @locID");
				sqlCommand4.Parameters.Add(new SqlParameter("@orgID", SqlDbType.Char, 10));
				sqlCommand4.Parameters.Add(new SqlParameter("@locID", SqlDbType.Char, 5));
				sqlCommand4.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar));
				sqlCommand4.Parameters.Add(new SqlParameter("@county", SqlDbType.VarChar));
				SqlCommand sqlCommand5 = Database.NewSqlCommand("Update Organizations Set cmoAvalaraAddressValidated = -1, cmoAddressLine1 = @line1, cmoAddressLine2 = @line2, cmoAddressLine3 = @line3, cmoCity = @city, cmoCounty = @county, cmoState = @state, cmoPostCode = @postcode, cmoCountry = @country Where cmoOrganizationID = @orgID");
				sqlCommand5.Parameters.Add(new SqlParameter("@orgID", SqlDbType.Char, 10));
				sqlCommand5.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar));
				sqlCommand5.Parameters.Add(new SqlParameter("@county", SqlDbType.VarChar));
				if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && Convert.ToDecimal(row["omdDifferentLocation"]) != 0m)
				{
					sqlCommand4.Parameters["@orgID"].Value = row.Field<string>(TableFields.GetField(table, "OtherShipOrgID"));
					sqlCommand4.Parameters["@locID"].Value = row.Field<string>(TableFields.GetField(table, "OtherShipLocID"));
					sqlCommand4.Parameters["@line1"].Value = addressInfo.Line1;
					sqlCommand4.Parameters["@line2"].Value = addressInfo.Line2;
					sqlCommand4.Parameters["@line3"].Value = addressInfo.Line3;
					sqlCommand4.Parameters["@city"].Value = addressInfo.City;
					sqlCommand4.Parameters["@state"].Value = addressInfo.Region;
					sqlCommand4.Parameters["@postcode"].Value = addressInfo.PostalCode;
					sqlCommand4.Parameters["@country"].Value = addressInfo.Country;
					sqlCommand4.Parameters["@county"].Value = addressInfo.County;
					Database.ExecuteCommand(sqlCommand4);
					if (row.Field<string>(TableFields.GetField(table, "OtherShipLocID")).Trim().Length <= 0)
					{
						sqlCommand5.Parameters["@orgID"].Value = row.Field<string>(TableFields.GetField(table, "OtherShipOrgID"));
						sqlCommand5.Parameters["@line1"].Value = addressInfo.Line1;
						sqlCommand5.Parameters["@line2"].Value = addressInfo.Line2;
						sqlCommand5.Parameters["@line3"].Value = addressInfo.Line3;
						sqlCommand5.Parameters["@city"].Value = addressInfo.City;
						sqlCommand5.Parameters["@state"].Value = addressInfo.Region;
						sqlCommand5.Parameters["@postcode"].Value = addressInfo.PostalCode;
						sqlCommand5.Parameters["@country"].Value = addressInfo.Country;
						sqlCommand5.Parameters["@county"].Value = addressInfo.County;
						Database.ExecuteCommand(sqlCommand5);
					}
				}
				else
				{
					sqlCommand4.Parameters["@orgID"].Value = row.Field<string>(TableFields.GetField(table, "BaseShipOrgID"));
					sqlCommand4.Parameters["@locID"].Value = row.Field<string>(TableFields.GetField(table, "BaseShipLocID"));
					sqlCommand4.Parameters["@line1"].Value = addressInfo.Line1;
					sqlCommand4.Parameters["@line2"].Value = addressInfo.Line2;
					sqlCommand4.Parameters["@line3"].Value = addressInfo.Line3;
					sqlCommand4.Parameters["@city"].Value = addressInfo.City;
					sqlCommand4.Parameters["@state"].Value = addressInfo.Region;
					sqlCommand4.Parameters["@postcode"].Value = addressInfo.PostalCode;
					sqlCommand4.Parameters["@country"].Value = addressInfo.Country;
					sqlCommand4.Parameters["@county"].Value = addressInfo.County;
					Database.ExecuteCommand(sqlCommand4);
					if (row.Field<string>(TableFields.GetField(table, "BaseShipLocID")).Trim().Length <= 0)
					{
						sqlCommand5.Parameters["@orgID"].Value = row.Field<string>(TableFields.GetField(table, "BaseShipOrgID"));
						sqlCommand5.Parameters["@line1"].Value = addressInfo.Line1;
						sqlCommand5.Parameters["@line2"].Value = addressInfo.Line2;
						sqlCommand5.Parameters["@line3"].Value = addressInfo.Line3;
						sqlCommand5.Parameters["@city"].Value = addressInfo.City;
						sqlCommand5.Parameters["@state"].Value = addressInfo.Region;
						sqlCommand5.Parameters["@postcode"].Value = addressInfo.PostalCode;
						sqlCommand5.Parameters["@country"].Value = addressInfo.Country;
						sqlCommand5.Parameters["@county"].Value = addressInfo.County;
						Database.ExecuteCommand(sqlCommand5);
					}
				}
			}
		}
		return true;
	}

	private Line setRecordLine(DataRow row, string table, string plantID, string baseWarehouseID)
	{
		return setRecordLine(row, table, plantID, baseWarehouseID, 1m);
	}

	private Line setRecordLine(DataRow row, string table, string plantID, string baseWarehouseID, decimal ratio)
	{
		Line line = new Line();
		if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && !row.Field<string>(TableFields.GetField(table, "WarehouseID")).Equals(baseWarehouseID, StringComparison.CurrentCultureIgnoreCase))
		{
			line.OriginAddress = getOriginAddress(plantID, row.Field<string>(TableFields.GetField(table, "WarehouseID")));
		}
		if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && Convert.ToDecimal(row["omdDifferentLocation"]) != 0m)
		{
			line.DestinationAddress = getDestinationAddress(row, useBaseLocation: false);
		}
		if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) && row.Field<byte>("arpInvoiceType") == 3)
		{
			if (Database.Props("AR").Field<bool>("xafARCalculateTaxOnDeposit"))
			{
				line.Amount = (row.Field<decimal>(TableFields.GetField(table, "UnitPriceBase")) * row.Field<decimal>(TableFields.GetField(table, "Quantity")) + row.Field<decimal>(TableFields.GetField(table, "TaxAmountBase")) + row.Field<decimal>(TableFields.GetField(table, "SecondTaxAmountBase"))) * ratio;
				line.TaxIncluded = true;
			}
			else
			{
				line.Amount = row.Field<decimal>(TableFields.GetField(table, "UnitPriceBase")) * row.Field<decimal>(TableFields.GetField(table, "Quantity")) * ratio;
				line.TaxOverride.TaxOverrideType = TaxOverrideType.TaxAmount;
				line.TaxOverride.Reason = "Deposit Amount No Tax";
				line.TaxOverride.TaxAmount = 0m;
				line.TaxOverride.TaxDate = row.Field<DateTime>(TableFields.GetField(table, "TransactionDate"));
			}
		}
		else
		{
			line.Amount = row.Field<decimal>(TableFields.GetField(table, "UnitPriceBase")) * row.Field<decimal>(TableFields.GetField(table, "Quantity")) * ratio;
		}
		string text = ((row.Field<string>("DeliveryAvalaraUseCodes").Trim().Length > 0) ? row.Field<string>("DeliveryAvalaraUseCodes") : row.Field<string>("cmlAvalaraUseCodes"));
		if (text.Trim().Length > 0)
		{
			line.CustomerUsageType = text;
		}
		line.Description = row.Field<string>(TableFields.GetField(table, "PartDescription"));
		line.ItemCode = row.Field<string>(TableFields.GetField(table, "PartID"));
		line.No = row.Field<short>(TableFields.GetField(table, "LineID")) + (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? ("-" + row.Field<short>("omdSalesOrderDeliveryID")) : "");
		line.Qty = Convert.ToDouble(row[TableFields.GetField(table, "Quantity")]);
		line.TaxCode = row.Field<string>("imuAvalaraTaxCodeID");
		if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			line.RevAcct = getRevenueCode(row, isFreight: false);
		}
		if (row.Field<string>(TableFields.GetField(table, "NonTaxReasonID")).Trim().Length > 0)
		{
			line.TaxOverride.Reason = row.Field<string>(TableFields.GetField(table, "NonTaxReasonID"));
			line.TaxOverride.TaxOverrideType = TaxOverrideType.Exemption;
			string text2 = ((row.Field<string>("DeliveryTaxExemptNumber").Trim().Length > 0) ? row.Field<string>("DeliveryTaxExemptNumber") : row.Field<string>("cmlTaxExemptNumber"));
			if (text2.Trim().Length > 0)
			{
				line.ExemptionNo = text2;
			}
		}
		return line;
	}

	private Line setRecordFreightLine(DataRow row, string table, Line recordLine)
	{
		return setRecordFreightLine(row, table, recordLine, 1m);
	}

	private Line setRecordFreightLine(DataRow row, string table, Line recordLine, decimal ratio)
	{
		Line line = new Line();
		if (recordLine.OriginAddress != null)
		{
			line.OriginAddress = recordLine.OriginAddress;
		}
		if (recordLine.DestinationAddress != null)
		{
			line.DestinationAddress = recordLine.DestinationAddress;
		}
		decimal num = 1m;
		if (row.Field<decimal>(TableFields.GetField(table, "LineQuantity")) != 0m)
		{
			num = row.Field<decimal>(TableFields.GetField(table, "Quantity")) / row.Field<decimal>(TableFields.GetField(table, "LineQuantity")) * ratio;
		}
		line.Amount = row.Field<decimal>(TableFields.GetField(table, "LineFreightBase")) * num;
		line.CustomerUsageType = recordLine.CustomerUsageType;
		line.Description = "Line Level Freight";
		line.ExemptionNo = recordLine.ExemptionNo;
		line.ItemCode = "Freight";
		line.No = recordLine.No + "-F";
		line.Qty = 1.0;
		if (row.Field<string>("xasAvalaraTaxCodeID") != null && row.Field<string>("xasAvalaraTaxCodeID").Trim().Length > 0)
		{
			line.TaxCode = row.Field<string>("xasAvalaraTaxCodeID");
		}
		else
		{
			line.TaxCode = "FR020100";
		}
		if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			line.RevAcct = getRevenueCode(row, isFreight: true);
		}
		if (row.Field<string>(TableFields.GetField(table, "NonTaxReasonID")).Trim().Length > 0)
		{
			line.TaxOverride.Reason = row.Field<string>(TableFields.GetField(table, "NonTaxReasonID"));
			line.TaxOverride.TaxOverrideType = TaxOverrideType.Exemption;
			string text = ((row.Field<string>("DeliveryTaxExemptNumber").Trim().Length > 0) ? row.Field<string>("DeliveryTaxExemptNumber") : row.Field<string>("cmlTaxExemptNumber"));
			if (text.Trim().Length > 0)
			{
				line.ExemptionNo = text;
			}
		}
		return line;
	}

	private Line setHeaderFreightLine(DataRow row, string table, Line recordLine, decimal recordTotalQty)
	{
		return setHeaderFreightLine(row, table, recordLine, recordTotalQty, 1m);
	}

	private Line setHeaderFreightLine(DataRow row, string table, Line recordLine, decimal recordTotalQty, decimal ratio)
	{
		Line line = new Line();
		if (recordLine.OriginAddress != null)
		{
			line.OriginAddress = recordLine.OriginAddress;
		}
		if (recordLine.DestinationAddress != null)
		{
			line.DestinationAddress = recordLine.DestinationAddress;
		}
		decimal num = default(decimal);
		num = row.Field<decimal>(TableFields.GetField(table, "Quantity")) / recordTotalQty;
		line.Amount = row.Field<decimal>(TableFields.GetField(table, "HeaderFreightBase")) * num * ratio;
		line.CustomerUsageType = recordLine.CustomerUsageType;
		line.Description = "Header Level Freight";
		line.ExemptionNo = recordLine.ExemptionNo;
		line.ItemCode = "HeaderFreight";
		line.No = recordLine.No + "-HF";
		line.Qty = 1.0;
		if (row.Field<string>("xasAvalaraTaxCodeID") != null && row.Field<string>("xasAvalaraTaxCodeID").Trim().Length > 0)
		{
			line.TaxCode = row.Field<string>("xasAvalaraTaxCodeID");
		}
		else
		{
			line.TaxCode = "FR020100";
		}
		if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			line.RevAcct = getRevenueCode(row, isFreight: true);
		}
		if (row.Field<string>(TableFields.GetField(table, "NonTaxReasonID")).Trim().Length > 0)
		{
			line.TaxOverride.Reason = row.Field<string>(TableFields.GetField(table, "NonTaxReasonID"));
			line.TaxOverride.TaxOverrideType = TaxOverrideType.Exemption;
			string text = ((row.Field<string>("DeliveryTaxExemptNumber").Trim().Length > 0) ? row.Field<string>("DeliveryTaxExemptNumber") : row.Field<string>("cmlTaxExemptNumber"));
			if (text.Trim().Length > 0)
			{
				line.ExemptionNo = text;
			}
		}
		return line;
	}

	private Line setDepositLine(DataRow row, string table, string plantID, string baseWarehouseID)
	{
		return setDepositLine(row, table, plantID, baseWarehouseID, 1m);
	}

	private Line setDepositLine(DataRow row, string table, string plantID, string baseWarehouseID, decimal ratio)
	{
		Line line = new Line();
		if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && !row.Field<string>(TableFields.GetField(table, "WarehouseID")).Equals(baseWarehouseID, StringComparison.CurrentCultureIgnoreCase))
		{
			line.OriginAddress = getOriginAddress(plantID, row.Field<string>(TableFields.GetField(table, "WarehouseID")));
		}
		if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && Convert.ToDecimal(row["omdDifferentLocation"]) != 0m)
		{
			line.DestinationAddress = getDestinationAddress(row, useBaseLocation: false);
		}
		line.Amount = row.Field<decimal>(TableFields.GetField(table, "UnitPriceBase")) * row.Field<decimal>(TableFields.GetField(table, "Quantity")) * ratio;
		string text = ((row.Field<string>("DeliveryAvalaraUseCodes").Trim().Length > 0) ? row.Field<string>("DeliveryAvalaraUseCodes") : row.Field<string>("cmlAvalaraUseCodes"));
		if (text.Trim().Length > 0)
		{
			line.CustomerUsageType = text;
		}
		line.Description = row.Field<string>(TableFields.GetField(table, "PartDescription"));
		line.ItemCode = row.Field<string>(TableFields.GetField(table, "PartID"));
		line.No = row.Field<short>(TableFields.GetField(table, "LineID")) + (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase) ? ("-" + row.Field<short>("omdSalesOrderDeliveryID")) : string.Empty);
		line.Qty = Convert.ToDouble(row[TableFields.GetField(table, "Quantity")]);
		line.TaxCode = row.Field<string>("imuAvalaraTaxCodeID");
		if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			line.RevAcct = getRevenueCode(row, isFreight: false);
		}
		line.TaxOverride.TaxOverrideType = TaxOverrideType.TaxAmount;
		line.TaxOverride.Reason = "Deposit Amount Used";
		line.TaxOverride.TaxAmount = row.Field<decimal>("arlTaxAmountBase");
		return line;
	}

	private string setQueryString(string table)
	{
		return setQueryString(table, isDepositInvoice: false);
	}

	private string setQueryString(string table, bool isDepositInvoice)
	{
		string result = string.Empty;
		string empty = string.Empty;
		string empty2 = string.Empty;
		switch (table.ToUpper())
		{
		case "SALESORDERS":
			empty = "ompSalesOrderID, omlSalesOrderID, omlSalesOrderLineID, ompUniqueID, ompPlantID, omdSalesOrderDeliveryID, ompCustomerOrganizationID, ompARInvoiceLocationID, ompShipOrganizationID, ompShipLocationID, ompCurrencyRateID, ompOrderDate, OrderShipLocation.cmlAddressLine1, OrderShipLocation.cmlAddressLine2, OrderShipLocation.cmlAddressLine3, OrderShipLocation.cmlCity, OrderShipLocation.cmlPostCode, OrderShipLocation.cmlState, OrderShipLocation.cmlCountry, OrderShipLocation.cmlAvalaraUseCodes, OrderShipLocation.cmlTaxExemptNumber, omdDifferentLocation, omdCustomerOrganizationID, omdShipLocationID, IsNull(DeliveryLocation.cmlAddressLine1,'') As DeliveryAddressLine1, IsNull(DeliveryLocation.cmlAddressLine2,'') As DeliveryAddressLine2, IsNull(DeliveryLocation.cmlAddressLine3,'') As DeliveryAddressLine3, IsNull(DeliveryLocation.cmlCity,'') As DeliveryCity, IsNull(DeliveryLocation.cmlPostCode,'') As DeliveryPostCode, IsNull(DeliveryLocation.cmlState,'') As DeliveryState,  IsNull(DeliveryLocation.cmlCountry,'') As DeliveryCountry, IsNull(DeliveryLocation.cmlAvalaraUseCodes,'') As DeliveryAvalaraUseCodes, IsNull(DeliveryLocation.cmlTaxExemptNumber,'') As DeliveryTaxExemptNumber, omlUnitPriceBase, omlOrderQuantity, omdDeliveryQuantity, omlPartID, omlPartShortDescription, omlPartGroupID, omdShippingMethodID, omdPartWarehouseLocationID, ompCustomerPO, omlFreightAmountBase, ompFreightAmountBase, omdAvalaraNonTaxReasonID, omlTaxCodeID, omlSecondTaxCodeID, ompFreightTaxCodeID, ompSecondFreightTaxCodeID, IsNull(imuAvalaraTaxCodeID, '') As imuAvalaraTaxCodeID, IsNull(DeliveryMethod.xasAvalaraTaxCodeID, OrderMethod.xasAvalaraTaxCodeID) As xasAvalaraTaxCodeID ";
			empty2 = "SalesOrders Inner Join SalesOrderLines On ompSalesOrderID = omlSalesOrderID Inner Join SalesOrderDeliveries On omlSalesOrderID = omdSalesOrderID and omlSalesOrderLineID = omdSalesOrderLineID Inner Join OrganizationLocations OrderShipLocation On OrderShipLocation.cmlOrganizationID = ompShipOrganizationID and OrderShipLocation.cmlLocationID = ompShipLocationID Left Outer Join OrganizationLocations DeliveryLocation On DeliveryLocation.cmlOrganizationID = omdCustomerOrganizationID and DeliveryLocation.cmlLocationID = omdShipLocationID Left Outer Join PartGroups On omlPartGroupID = imuPartGroupID Left Outer Join ShippingMethods DeliveryMethod On omdShippingMethodID = DeliveryMethod.xasShippingMethodID Left Outer Join ShippingMethods OrderMethod On ompShippingMethodID = OrderMethod.xasShippingMethodID ";
			result = "SELECT " + empty + " FROM  " + empty2 + " WHERE ompSalesOrderID = @recordID And omdDeliveryType <> 3 And omlAvalaraIgnoreLine = 0 Order By omlSalesOrderLineID, omdSalesOrderDeliveryID";
			break;
		case "ARINVOICES":
			empty = "arpARInvoiceID, arlARInvoiceID, arlARInvoiceLineID, arpAvalaraTaxCalculated, arpUniqueID, arpPlantID, arpInvoiceType, arpInvoiceDate, arpCreditDate, arpCreditARInvoiceID, arpCustomerOrganizationID, arpShipOrganizationID, arpShipLocationID, arpInvoiceTotalBase, arlDepositLine, arlPartID, arlPartShortDescription, arlCustomerPO, arlInvoiceQuantity, arlUnitPriceBase, arpFreightAmountBase, arlFreightAmountBase, arlNonTaxReasonID, arlTaxCodeID, arlTaxAmountBase, arlSecondTaxCodeID, arlSecondTaxAmountBase, arpFreightTaxCodeID, arpSecondFreightTaxCodeID, IsNull(smlPartWarehouseLocationID,'') As smlPartWarehouseLocationID, IsNull(imuAvalaraTaxCodeID, '') As imuAvalaraTaxCodeID, IsNull(xasAvalaraTaxCodeID, '') As xasAvalaraTaxCodeID, InvoiceShipLocation.cmlAddressLine1, InvoiceShipLocation.cmlAddressLine2, InvoiceShipLocation.cmlAddressLine3, InvoiceShipLocation.cmlCity, InvoiceShipLocation.cmlPostCode, InvoiceShipLocation.cmlState, InvoiceShipLocation.cmlCountry, InvoiceShipLocation.cmlAvalaraUseCodes, InvoiceShipLocation.cmlTaxExemptNumber, (Case When RTRIM(InvoiceShipLocation.cmlOrganizationID)+'-'+RTRIM(InvoiceShipLocation.cmlLocationID) <> RTRIM(ShipmentLocation.cmlOrganizationID)+'-'+RTRIM(ShipmentLocation.cmlLocationID) Then -1 Else 0 End) As omdDifferentLocation, smpShipOrganizationID, smpShipLocationID, IsNull(ShipmentLocation.cmlAddressLine1,'') As DeliveryAddressLine1, IsNull(ShipmentLocation.cmlAddressLine2,'') As DeliveryAddressLine2, IsNull(ShipmentLocation.cmlAddressLine3,'') As DeliveryAddressLine3, IsNull(ShipmentLocation.cmlCity,'') As DeliveryCity, IsNull(ShipmentLocation.cmlPostCode,'') As DeliveryPostCode, IsNull(ShipmentLocation.cmlState,'') As DeliveryState, IsNull(ShipmentLocation.cmlCountry,'') As DeliveryCountry, IsNull(ShipmentLocation.cmlAvalaraUseCodes,'') As DeliveryAvalaraUseCodes, IsNull(ShipmentLocation.cmlTaxExemptNumber,'') As DeliveryTaxExemptNumber, ISNull(xauUseProperties, 0) As xauUseProperties, IsNull(xauARFreightGLAccountID, '') As xauARFreightGLAccountID, IsNull(xauARDepositGLAccountID, '') As xauARDepositGLAccountID, IsNull(xauARSalesGLAccountID, '') As xauARSalesGLAccountID, IsNull(imuARDepositGLAccountID, '') As imuARDepositGLAccountID, IsNull(imuSalesGLAccountID, '') As imuSalesGLAccountID ";
			empty2 = "ARInvoices Inner Join ARInvoiceLines On arpARInvoiceID = arlARInvoiceID Inner Join OrganizationLocations InvoiceShipLocation On InvoiceShipLocation.cmlOrganizationID = arpShipOrganizationID and InvoiceShipLocation.cmlLocationID = arpShipLocationID Left Outer Join Shipments On arlShipmentID = smpShipmentID Left Outer Join ShipmentLines On arlShipmentID = smlShipmentID And arlShipmentLineID = smlShipmentLineID Left Outer Join OrganizationLocations ShipmentLocation On ShipmentLocation.cmlOrganizationID = smpShipOrganizationID And ShipmentLocation.cmlLocationID = smpShipLocationID Left Outer Join PartGroups On arlPartGroupID = imuPartGroupID Left Outer Join ShippingMethods On smpShippingMethodID = xasShippingMethodID Left Outer Join Plants On arpPlantID = xauPlantID ";
			result = "SELECT " + empty + " FROM " + empty2 + " WHERE arpARInvoiceID = @recordID And arlAvalaraIgnoreLine = 0 Order By arlARInvoiceLineID";
			break;
		case "QUOTES":
			empty = "qmpQuoteID, qmlQuoteID, qmlQuoteLineID, qmpUniqueID, qmpPlantID, qmpQuoteDate, qmpCustomerOrganizationID, qmpShipOrganizationID, qmpShipLocationID, qmpShippingMethodID, qmlPartID, qmlPartShortDescription, qmlPartGroupID, qmlNonTaxReasonID, qmlTaxCodeID, qmlSecondTaxCodeID, IsNull(imuAvalaraTaxCodeID, '') As imuAvalaraTaxCodeID, IsNull(xasAvalaraTaxCodeID, '') As xasAvalaraTaxCodeID, 0 As omdDifferentLocation, cmlAddressLine1, cmlAddressLine2, cmlAddressLine3, cmlCity, cmlPostCode, cmlState, cmlCountry, cmlAvalaraUseCodes, cmlTaxExemptNumber, '' As DeliveryAvalaraUseCodes, '' As DeliveryTaxExemptNumber, Convert(decimal(15,5),1) As QuoteQuantities, Convert(decimal(15,5),1) As QuoteUnitPriceBase ";
			empty2 = "Quotes Inner Join QuoteLines On qmpQuoteID = qmlQuoteID Inner Join OrganizationLocations On cmlOrganizationID = qmpShipOrganizationID and cmlLocationID = qmpShipLocationID Left Outer Join PartGroups On qmlPartGroupID = imuPartGroupID Left Outer Join ShippingMethods On qmpShippingMethodID = xasShippingMethodID ";
			result = "SELECT " + empty + " FROM " + empty2 + " WHERE qmpQuoteID = @recordID Order By qmlQuoteLineID";
			break;
		}
		return result;
	}

	private Address getOriginAddress(string plant, string warehouse)
	{
		Address address = new Address();
		if (warehouse.Trim().Length > 0)
		{
			SqlCommand sqlCommand = Database.NewSqlCommand("SELECT imwPlantID, imwWarehouseID, imwAddressLine1, imwAddressLine2, imwAddressLine3, imwCity, imwState, imwPostCode, imwCountry FROM Warehouses WHERE imwWarehouseID = @warehouseID");
			sqlCommand.Parameters.Add(new SqlParameter("@warehouseID", SqlDbType.Char, 5)).Value = warehouse;
			DataTable dataTable = Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				address.Line1 = dataTable.Rows[0].Field<string>("imwAddressLine1");
				address.Line2 = dataTable.Rows[0].Field<string>("imwAddressLine2");
				address.Line3 = dataTable.Rows[0].Field<string>("imwAddressLine3");
				address.City = dataTable.Rows[0].Field<string>("imwCity");
				address.Region = dataTable.Rows[0].Field<string>("imwState");
				address.PostalCode = dataTable.Rows[0].Field<string>("imwPostCode");
				address.Country = dataTable.Rows[0].Field<string>("imwCountry");
			}
		}
		else if (plant.Trim().Length > 0)
		{
			SqlCommand sqlCommand2 = Database.NewSqlCommand("SELECT xauPlantID, xauAddressLine1, xauAddressLine2, xauAddressLine3, xauCity, xauState, xauPostCode, xauCountry FROM Plants WHERE xauPlantID = @plantID");
			sqlCommand2.Parameters.Add(new SqlParameter("@plantID", SqlDbType.Char, 5)).Value = plant;
			DataTable dataTable2 = Database.GetDataTable(sqlCommand2);
			if (dataTable2.Rows.Count > 0)
			{
				address.Line1 = dataTable2.Rows[0].Field<string>("xauAddressLine1");
				address.Line2 = dataTable2.Rows[0].Field<string>("xauAddressLine2");
				address.Line3 = dataTable2.Rows[0].Field<string>("xauAddressLine3");
				address.City = dataTable2.Rows[0].Field<string>("xauCity");
				address.Region = dataTable2.Rows[0].Field<string>("xauState");
				address.PostalCode = dataTable2.Rows[0].Field<string>("xauPostCode");
				address.Country = dataTable2.Rows[0].Field<string>("xauCountry");
			}
		}
		else
		{
			SqlCommand sqlCommand3 = Database.NewSqlCommand("SELECT xadAddressLine1, xadAddressLine2, xadAddressLine3, xadCity, xadState, xadPostCode, xadCountry FROM DatasetProperties");
			DataTable dataTable3 = Database.GetDataTable(sqlCommand3);
			if (dataTable3.Rows.Count > 0)
			{
				address.Line1 = dataTable3.Rows[0].Field<string>("xadAddressLine1");
				address.Line2 = dataTable3.Rows[0].Field<string>("xadAddressLine2");
				address.Line3 = dataTable3.Rows[0].Field<string>("xadAddressLine3");
				address.City = dataTable3.Rows[0].Field<string>("xadCity");
				address.Region = dataTable3.Rows[0].Field<string>("xadState");
				address.PostalCode = dataTable3.Rows[0].Field<string>("xadPostCode");
				address.Country = dataTable3.Rows[0].Field<string>("xadCountry");
			}
		}
		return address;
	}

	private Address getDestinationAddress(DataRow row, bool useBaseLocation)
	{
		Address address = new Address();
		if (!useBaseLocation && Convert.ToDecimal(row["omdDifferentLocation"]) != 0m)
		{
			address.Line1 = row.Field<string>("DeliveryAddressLine1");
			address.Line2 = row.Field<string>("DeliveryAddressLine2");
			address.Line3 = row.Field<string>("DeliveryAddressLine3");
			address.City = row.Field<string>("DeliveryCity");
			address.Region = row.Field<string>("DeliveryState");
			address.PostalCode = row.Field<string>("DeliveryPostCode");
			address.Country = row.Field<string>("DeliveryCountry");
		}
		else
		{
			address.Line1 = row.Field<string>("cmlAddressLine1");
			address.Line2 = row.Field<string>("cmlAddressLine2");
			address.Line3 = row.Field<string>("cmlAddressLine3");
			address.City = row.Field<string>("cmlCity");
			address.Region = row.Field<string>("cmlState");
			address.PostalCode = row.Field<string>("cmlPostCode");
			address.Country = row.Field<string>("cmlCountry");
		}
		return address;
	}

	private string getSalespeople(string table, string recordID)
	{
		string text = string.Empty;
		string queryString = string.Empty;
		switch (table.ToUpper())
		{
		case "SALESORDERS":
			queryString = "SELECT omiSalesEmployeeID As EmployeeID FROM SalesOrderSalespeople WHERE omiSalesOrderID = @recordID";
			break;
		case "ARINVOICES":
			queryString = "SELECT arjSalesEmployeeID As EmployeeID FROM ARInvoiceSalespeople WHERE arjARInvoiceID = @recordID";
			break;
		case "QUOTES":
			queryString = "SELECT qmjSalesEmployeeID As EmployeeID FROM QuoteSalespeople WHERE qmjQuoteID = @recordID";
			break;
		}
		SqlCommand sqlCommand = Database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10)).Value = recordID;
		foreach (DataRow row in Database.GetDataTable(sqlCommand).Rows)
		{
			text = text + "," + row.Field<string>("EmployeeID").Trim();
		}
		if (text.Trim().Length > 0)
		{
			text = text.Substring(1);
			if (text.Length > 25)
			{
				text = text.Substring(0, 25);
			}
		}
		if (text.Trim().Length <= 0)
		{
			return string.Empty;
		}
		return text;
	}

	private decimal getTotalQty(string table, string recordID)
	{
		decimal result = default(decimal);
		string queryString = string.Empty;
		string text = table.ToUpper();
		if (!(text == "SALESORDERS"))
		{
			if (text == "ARINVOICES")
			{
				queryString = "SELECT IsNull(Sum(arlInvoiceQuantity),0) As Quantity FROM ARInvoiceLines WHERE arlARInvoiceID = @recordID and arlDepositLine = 0";
			}
		}
		else
		{
			queryString = "SELECT IsNull(Sum(omlOrderQuantity),0) As Quantity FROM SalesOrderLines WHERE omlSalesOrderID = @recordID";
		}
		SqlCommand sqlCommand = Database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10)).Value = recordID;
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			return dataTable.Rows[0].Field<decimal>("Quantity");
		}
		return result;
	}

	private string getRevenueCode(DataRow row, bool isFreight)
	{
		string empty = string.Empty;
		if (isFreight)
		{
			if (row.Field<bool>("xauUseProperties"))
			{
				return row.Field<string>("xauARFreightGLAccountID");
			}
			return Database.Props("FN").Field<string>("xafARFreightGLAccountID");
		}
		if (row.Field<byte>("arpInvoiceType") == 3 || row.Field<bool>("arlDepositLine"))
		{
			if (row.Field<string>("imuARDepositGLAccountID").Trim().Length > 0)
			{
				return row.Field<string>("imuARDepositGLAccountID");
			}
			if (row.Field<bool>("xauUseProperties"))
			{
				return row.Field<string>("xauARDepositGLAccountID");
			}
			return Database.Props("FN").Field<string>("xafARDepositGLAccountID");
		}
		if (row.Field<string>("imuSalesGLAccountID").Trim().Length > 0)
		{
			return row.Field<string>("imuSalesGLAccountID");
		}
		if (row.Field<bool>("xauUseProperties"))
		{
			return row.Field<string>("xauARSalesGLAccountID");
		}
		return Database.Props("PN").Field<string>("xapOMSalesGLAccountID");
	}

	public string GetARPaymentTax(string paymentTable, object[] keyValues, M1BindingSource bs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		string empty = string.Empty;
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = "ARInvoices";
		DateTime docDate = default(DateTime);
		decimal num3 = 1m;
		SqlDataAdapter adapter = new SqlDataAdapter();
		bool flag = true;
		TaxSvc taxSvc = CreateTaxSvcConfig();
		if (keyValues.Length != 0)
		{
			num = Convert.ToInt16(keyValues[0]);
			stringBuilder2.Append("SELECT * FROM ARPaymentLines WHERE arnARPaymentSessionID = @sessionID ");
			if (keyValues.Length > 1)
			{
				num2 = Convert.ToInt16(keyValues[1]);
				if (num2 != 0)
				{
					stringBuilder2.Append(" AND arnARPaymentHeaderID = @headerID ");
				}
			}
			stringBuilder2.Append(" AND arnDiscountAmount <> 0 ");
			stringBuilder2.Append(" ORDER BY arnARPaymentHeaderID, arnARPaymentLineID ");
		}
		SqlCommand sqlCommand = Database.NewSqlCommand(stringBuilder2.ToString());
		sqlCommand.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int)).Value = num;
		if (num2 != 0)
		{
			sqlCommand.Parameters.Add(new SqlParameter("@headerID", SqlDbType.Int)).Value = num2;
		}
		DataTable dataTable = Database.GetDataTable(sqlCommand, fillSchema: true, out adapter);
		if (dataTable.Rows.Count > 0)
		{
			using (SqlCommand sqlCommand2 = Database.NewSqlCommand("SELECT arsARPaymentSessionID, arsReceiptDate, arsExchangeRate FROM ARPaymentSessions WHERE arsARPaymentSessionID = @sessionID"))
			{
				sqlCommand2.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int)).Value = num;
				DataTable dataTable2 = Database.GetDataTable(sqlCommand2);
				if (dataTable2.Rows.Count > 0)
				{
					docDate = dataTable2.Rows[0].Field<DateTime>("arsReceiptDate");
					num3 = dataTable2.Rows[0].Field<decimal>("arsExchangeRate");
				}
			}
			string queryString = setQueryString("ARInvoices");
			SqlCommand sqlCommand3 = Database.NewSqlCommand(queryString);
			sqlCommand3.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10));
			SqlCommand sqlCommand4 = Database.NewSqlCommand("UPDATE ARPaymentHeaders SET artAvalaraTaxCalculated = -1 WHERE artARPaymentSessionID = @sessionID AND artARPaymentHeaderID = @headerID");
			sqlCommand4.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int));
			sqlCommand4.Parameters.Add(new SqlParameter("@headerID", SqlDbType.Int));
			foreach (DataRow row2 in dataTable.Rows)
			{
				empty = row2.Field<string>("arnARInvoiceID").Trim();
				if (empty.Trim().Length <= 0)
				{
					continue;
				}
				sqlCommand3.Parameters["@recordID"].Value = empty;
				DataTable dataTable3 = Database.GetDataTable(sqlCommand3);
				if (dataTable3.Rows.Count <= 0 || !dataTable3.Rows[0].Field<bool>("arpAvalaraTaxCalculated"))
				{
					continue;
				}
				GetTaxRequest getTaxRequest = new GetTaxRequest();
				string text2 = dataTable3.Rows[0].Field<string>(TableFields.GetField(text, "PlantID"));
				string text3 = dataTable3.Rows[0].Field<string>(TableFields.GetField(text, "WarehouseID"));
				getTaxRequest.OriginAddress = getOriginAddress(text2, text3);
				getTaxRequest.DestinationAddress = getDestinationAddress(dataTable3.Rows[0], useBaseLocation: true);
				getTaxRequest.Commit = false;
				getTaxRequest.CompanyCode = Database.Props("FN").Field<string>("xafAvalaraCompanyCode");
				getTaxRequest.CustomerCode = dataTable3.Rows[0].Field<string>(TableFields.GetField(text, "CustomerOrgID"));
				getTaxRequest.CustomerUsageType = dataTable3.Rows[0].Field<string>("cmlAvalaraUseCodes");
				getTaxRequest.DetailLevel = DetailLevel.Tax;
				getTaxRequest.ServiceMode = ServiceMode.Remote;
				getTaxRequest.DocCode = "ARPay:" + Convert.ToInt16(row2["arnARPaymentSessionID"]) + "-" + Convert.ToInt16(row2["arnARPaymentHeaderID"]) + "-Inv:" + empty;
				getTaxRequest.DocDate = docDate;
				getTaxRequest.DocType = DocumentType.SalesInvoice;
				getTaxRequest.LocationCode = dataTable3.Rows[0].Field<string>(TableFields.GetField(text, "CustomerOrgID"));
				getTaxRequest.TaxOverride.TaxDate = dataTable3.Rows[0].Field<DateTime>("arpInvoiceDate");
				getTaxRequest.TaxOverride.Reason = "AR Payment Discount";
				decimal num4 = 1m;
				if (!text.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && dataTable3.Rows[0].Field<decimal>(TableFields.GetField(text, "HeaderFreightBase")) != 0m)
				{
					num4 = getTotalQty(text, empty);
					if (num4 == 0m)
					{
						num4 = 1m;
					}
				}
				decimal num5 = 1m;
				if (dataTable3.Rows[0].Field<decimal>("arpInvoiceTotalBase") != 0m)
				{
					num5 = row2.Field<decimal>("arnTotalDiscountAmount") / dataTable3.Rows[0].Field<decimal>("arpInvoiceTotalBase");
					if (num5 == 0m)
					{
						num5 = 1m;
					}
				}
				num5 = -num5;
				foreach (DataRow row3 in dataTable3.Rows)
				{
					Line line = null;
					if (!text.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) || row3.Field<byte>("arpInvoiceType") == 3 || !row3.Field<bool>("arlDepositLine"))
					{
						line = setRecordLine(row3, text, text2, text3, num5);
						getTaxRequest.Lines.Add(line);
					}
					if (row3.Field<decimal>("arlFreightAmountBase") != 0m)
					{
						Line line2 = setRecordFreightLine(row3, text, line, num5);
						getTaxRequest.Lines.Add(line2);
					}
					if (row3.Field<decimal>("arpFreightAmountBase") != 0m)
					{
						Line line3 = setHeaderFreightLine(row3, text, line, num4, num5);
						getTaxRequest.Lines.Add(line3);
					}
				}
				GetTaxResult tax = taxSvc.GetTax(getTaxRequest);
				if (tax.ResultCode == SeverityLevel.Success || tax.ResultCode == SeverityLevel.Warning)
				{
					decimal num6 = default(decimal);
					decimal num7 = default(decimal);
					string value = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
					string value2 = string.Empty;
					if (!Database.Region.Equals("CAN", StringComparison.CurrentCultureIgnoreCase))
					{
						foreach (TaxLine taxLine3 in tax.TaxLines)
						{
							num6 += taxLine3.Tax * -1m;
						}
					}
					else
					{
						foreach (TaxLine taxLine4 in tax.TaxLines)
						{
							bool flag2 = false;
							foreach (TaxDetail taxDetail in taxLine4.TaxDetails)
							{
								string text4 = taxDetail.TaxName.Trim();
								if (text4.IndexOf("GST") >= 0)
								{
									value = Database.Props("FN").Field<string>("xafAvalaraCanadaGSTTaxCodeID");
									num6 += taxDetail.Tax * -1m;
									flag2 = true;
								}
								if (text4.IndexOf("PST") >= 0)
								{
									value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaPSTTaxCodeID");
									num7 += taxDetail.Tax * -1m;
									flag2 = true;
								}
								if (text4.IndexOf("HST") >= 0)
								{
									value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaHSTTaxCodeID");
									num7 += taxDetail.Tax * -1m;
									flag2 = true;
								}
								if (text4.IndexOf("QST") >= 0)
								{
									value2 = Database.Props("FN").Field<string>("xafAvalaraCanadaQSTTaxCodeID");
									num7 += taxDetail.Tax * -1m;
									flag2 = true;
								}
							}
							if (!flag2)
							{
								value = Database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
								num6 += taxLine4.Tax * -1m;
								value2 = string.Empty;
								num7 = default(decimal);
								flag2 = true;
							}
						}
					}
					row2["arnDiscountTaxAmount"] = Math.Round(num6, 2);
					row2["arnDiscountTaxCodeID"] = value;
					row2["arnSecondDiscountTaxAmount"] = Math.Round(num7, 2);
					row2["arnSecondDiscountTaxCodeID"] = value2;
					row2["arnDiscountAmount"] = Math.Round(row2.Field<decimal>("arnTotalDiscountAmount") - row2.Field<decimal>("arnDiscountTaxAmount") - row2.Field<decimal>("arnSecondDiscountTaxAmount"), 2);
					row2["arnDiscountTaxAmountForeign"] = Math.Round(num6 * num3, 2);
					row2["arnSecondDisTaxAmtForeign"] = Math.Round(num7 * num3, 2);
					row2["arnDiscountAmountForeign"] = Math.Round(Convert.ToDecimal(row2["arnDiscountAmount"]) * num3, 2);
					row2["arnNonTaxReasonID"] = string.Empty;
					row2["arnAvalaraTaxCalculated"] = -1;
				}
				else
				{
					flag = false;
				}
				AddAvalaraTransaction(paymentTable, Convert.ToInt16(row2["arnARPaymentSessionID"]) + "-" + Convert.ToInt16(row2["arnARPaymentHeaderID"]) + ":" + empty, tax);
				if (tax.ResultCode != SeverityLevel.Success)
				{
					stringBuilder.AppendLine(tax.ResultCode.ToString() + ": ");
					if (tax.Messages[0].Summary.Length > 0)
					{
						stringBuilder.AppendLine(tax.Messages[0].Summary);
					}
				}
			}
			Database.UpdateData(dataTable, adapter);
			dataTable.AcceptChanges();
		}
		if (flag && num2 == 0)
		{
			Database.ExecuteCommand("UPDATE ARPaymentSessions SET arsAvalaraTaxCalculated = -1 WHERE arsARPaymentSessionID = " + num.ToSql());
		}
		return stringBuilder.ToString();
	}

	public string PostTax(string table, string recordID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if ((byte)CheckLastSuccessfulTransaction(table, new object[1] { recordID }) == 3)
		{
			TaxSvc taxSvc = CreateTaxSvcConfig();
			PostTaxRequest postTaxRequest = new PostTaxRequest();
			SqlCommand sqlCommand = Database.NewSqlCommand("SELECT * FROM " + table + " WHERE " + TableFields.GetField(table, "RecordID") + " = @recordID");
			sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10)).Value = recordID;
			DataTable dataTable = Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				postTaxRequest.CompanyCode = Database.Props("FN").Field<string>("xafAvalaraCompanyCode");
				postTaxRequest.DocCode = (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "AR-" : string.Empty) + recordID.Trim();
				postTaxRequest.DocDate = dataTable.Rows[0].Field<DateTime>(TableFields.GetField(table, "TransactionDate"));
				postTaxRequest.DocType = DocumentType.SalesInvoice;
				if (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) && dataTable.Rows[0].Field<byte>("arpInvoiceType") == 2)
				{
					postTaxRequest.DocType = DocumentType.ReturnInvoice;
				}
				decimal num = default(decimal);
				SqlCommand sqlCommand2 = Database.NewSqlCommand("SELECT * FROM " + TableFields.GetField(table, "ChildTable") + " WHERE " + TableFields.GetField(table, "LineRecordID") + " = @recordID And " + TableFields.GetField(table, "IgnoreLine") + " <> 0");
				sqlCommand2.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10)).Value = recordID;
				DataTable dataTable2 = Database.GetDataTable(sqlCommand2);
				if (dataTable2.Rows.Count > 0)
				{
					decimal num2 = 1m;
					if (!table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) && dataTable.Rows[0].Field<decimal>(TableFields.GetField(table, "HeaderFreightBase")) != 0m)
					{
						num2 = getTotalQty(table, recordID);
						if (num2 == 0m)
						{
							num2 = 1m;
						}
					}
					foreach (DataRow row in dataTable2.Rows)
					{
						num += row.Field<decimal>(TableFields.GetField(table, "UnitPriceBase")) * row.Field<decimal>(TableFields.GetField(table, "Quantity"));
						num += row.Field<decimal>(TableFields.GetField(table, "LineFreightBase"));
						decimal num3 = default(decimal);
						num3 = row.Field<decimal>(TableFields.GetField(table, "Quantity")) / num2;
						num += dataTable.Rows[0].Field<decimal>(TableFields.GetField(table, "HeaderFreightBase")) * num3;
					}
				}
				postTaxRequest.TotalAmount = dataTable.Rows[0].Field<decimal>(TableFields.GetField(table, "SubTotalBase")) + dataTable.Rows[0].Field<decimal>(TableFields.GetField(table, "FreightTotalBase")) - num;
				postTaxRequest.TotalTax = dataTable.Rows[0].Field<decimal>(TableFields.GetField(table, "TotalTaxBase"));
				postTaxRequest.Commit = true;
				PostTaxResult postTaxResult = taxSvc.PostTax(postTaxRequest);
				_ = postTaxResult.ResultCode;
				AddAvalaraTransaction(table, recordID, postTaxResult);
				if (postTaxResult.ResultCode != SeverityLevel.Success)
				{
					stringBuilder.AppendLine(postTaxResult.ResultCode.ToString() + ": ");
					if (postTaxResult.Messages[0].Summary.Length > 0)
					{
						stringBuilder.AppendLine(postTaxResult.Messages[0].Summary);
					}
				}
			}
			else
			{
				stringBuilder.Append("Record not exist in database.");
			}
		}
		return stringBuilder.ToString();
	}

	public string PostPaymentTax(int sessionID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string empty = string.Empty;
		string empty2 = string.Empty;
		DateTime docDate = default(DateTime);
		TaxSvc taxSvc = CreateTaxSvcConfig();
		empty2 = "SELECT * FROM ARPaymentLines WHERE arnARPaymentSessionID = @sessionID AND arnDiscountAmount <> 0 ORDER BY arnARPaymentHeaderID, arnARPaymentLineID ";
		SqlCommand sqlCommand = Database.NewSqlCommand(empty2);
		sqlCommand.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int)).Value = sessionID;
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			using (SqlCommand sqlCommand2 = Database.NewSqlCommand("SELECT arsARPaymentSessionID, arsReceiptDate, arsExchangeRate FROM ARPaymentSessions WHERE arsARPaymentSessionID = @sessionID"))
			{
				sqlCommand2.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int)).Value = sessionID;
				DataTable dataTable2 = Database.GetDataTable(sqlCommand2);
				if (dataTable2.Rows.Count > 0)
				{
					docDate = dataTable2.Rows[0].Field<DateTime>("arsReceiptDate");
				}
			}
			SqlCommand sqlCommand3 = Database.NewSqlCommand("Select ARInvoiceLines.*, arpInvoiceTotalBase, arpFreightAmountBase From ARInvoices Inner Join ARInvoiceLines On arpARInvoiceID = arlARInvoiceID Where arpARInvoiceID = @recordID And arlAvalaraIgnoreLine <> 0");
			sqlCommand3.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10));
			foreach (DataRow row2 in dataTable.Rows)
			{
				empty = row2.Field<string>("arnARInvoiceID").Trim();
				if (empty.Trim().Length <= 0 || (byte)CheckLastSuccessfulTransaction("ARPaymentSessions", new object[3]
				{
					row2["arnARPaymentSessionID"],
					row2["arnARPaymentHeaderID"],
					empty
				}) != 3)
				{
					continue;
				}
				PostTaxRequest postTaxRequest = new PostTaxRequest();
				postTaxRequest.CompanyCode = Database.Props("FN").Field<string>("xafAvalaraCompanyCode");
				postTaxRequest.DocCode = "ARPay:" + Convert.ToInt16(row2["arnARPaymentSessionID"]) + "-" + Convert.ToInt16(row2["arnARPaymentHeaderID"]) + "-Inv:" + empty;
				postTaxRequest.DocDate = docDate;
				postTaxRequest.DocType = DocumentType.SalesInvoice;
				decimal num = default(decimal);
				sqlCommand3.Parameters["@recordID"].Value = empty;
				DataTable dataTable3 = Database.GetDataTable(sqlCommand3);
				if (dataTable3.Rows.Count > 0)
				{
					decimal num2 = 1m;
					if (dataTable3.Rows[0].Field<decimal>("arpFreightAmountBase") != 0m)
					{
						num2 = getTotalQty("ARInvoices", empty);
						if (num2 == 0m)
						{
							num2 = 1m;
						}
					}
					decimal num3 = 1m;
					if (dataTable3.Rows[0].Field<decimal>("arpInvoiceTotalBase") != 0m)
					{
						num3 = row2.Field<decimal>("arnTotalDiscountAmount") / dataTable3.Rows[0].Field<decimal>("arpInvoiceTotalBase");
						if (num3 == 0m)
						{
							num3 = 1m;
						}
					}
					foreach (DataRow row3 in dataTable3.Rows)
					{
						num += row3.Field<decimal>("arlUnitPriceBase") * row3.Field<decimal>("arlInvoiceQuantity") * num3;
						num += row3.Field<decimal>("arlFreightAmountBase") * num3;
						decimal num4 = default(decimal);
						num4 = row3.Field<decimal>("arlInvoiceQuantity") / num2;
						num += row3.Field<decimal>("arpFreightAmountBase") * num4 * num3;
					}
				}
				postTaxRequest.TotalAmount = -(row2.Field<decimal>("arnDiscountAmount") - num);
				postTaxRequest.TotalTax = -row2.Field<decimal>("arnDiscountTaxAmount");
				postTaxRequest.Commit = true;
				PostTaxResult postTaxResult = taxSvc.PostTax(postTaxRequest);
				_ = postTaxResult.ResultCode;
				AddAvalaraTransaction("ARPaymentSessions", Convert.ToInt16(row2["arnARPaymentSessionID"]) + "-" + Convert.ToInt16(row2["arnARPaymentHeaderID"]) + ":" + empty, postTaxResult);
				if (postTaxResult.ResultCode != SeverityLevel.Success)
				{
					stringBuilder.AppendLine(postTaxResult.ResultCode.ToString() + ": ");
					if (postTaxResult.Messages[0].Summary.Length > 0)
					{
						stringBuilder.AppendLine(postTaxResult.Messages[0].Summary);
					}
				}
			}
		}
		return stringBuilder.ToString();
	}

	public string CancelTax(string table, string recordID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		AvalaraTransactionType avalaraTransactionType = (AvalaraTransactionType)CheckLastSuccessfulTransaction(table, new object[1] { recordID });
		if (avalaraTransactionType == AvalaraTransactionType.GetTax || avalaraTransactionType == AvalaraTransactionType.PostTax)
		{
			CancelTaxResult cancelTaxResult = CreateTaxSvcConfig().CancelTax(new CancelTaxRequest
			{
				CompanyCode = Database.Props("FN").Field<string>("xafAvalaraCompanyCode"),
				DocCode = (table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase) ? "AR-" : string.Empty) + recordID.Trim(),
				DocType = DocumentType.SalesInvoice,
				CancelCode = CancelCode.DocDeleted
			});
			_ = cancelTaxResult.ResultCode;
			AddAvalaraTransaction(table, recordID, cancelTaxResult);
			if (cancelTaxResult.ResultCode != SeverityLevel.Success)
			{
				stringBuilder.AppendLine(cancelTaxResult.ResultCode.ToString() + ": ");
				if (cancelTaxResult.Messages[0].Summary.Length > 0)
				{
					stringBuilder.AppendLine(cancelTaxResult.Messages[0].Summary);
				}
			}
		}
		return stringBuilder.ToString();
	}

	public string CancelPaymentTax(int sessionID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string empty = string.Empty;
		string empty2 = string.Empty;
		TaxSvc taxSvc = CreateTaxSvcConfig();
		empty2 = "SELECT * FROM ARPaymentLines WHERE arnARPaymentSessionID = @sessionID AND arnDiscountAmount <> 0 ORDER BY arnARPaymentHeaderID, arnARPaymentLineID ";
		SqlCommand sqlCommand = Database.NewSqlCommand(empty2);
		sqlCommand.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int)).Value = sessionID;
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			string queryString = setQueryString("ARInvoices");
			Database.NewSqlCommand(queryString).Parameters.Add(new SqlParameter("@recordID", SqlDbType.Char, 10));
			foreach (DataRow row in dataTable.Rows)
			{
				empty = row.Field<string>("arnARInvoiceID").Trim();
				if (empty.Trim().Length <= 0)
				{
					continue;
				}
				AvalaraTransactionType avalaraTransactionType = (AvalaraTransactionType)CheckLastSuccessfulTransaction("ARPaymentSessions", new object[3]
				{
					row["arnARPaymentSessionID"],
					row["arnARPaymentHeaderID"],
					empty
				});
				if (avalaraTransactionType != AvalaraTransactionType.GetTax && avalaraTransactionType != AvalaraTransactionType.PostTax)
				{
					continue;
				}
				CancelTaxRequest cancelTaxRequest = new CancelTaxRequest();
				cancelTaxRequest.CompanyCode = Database.Props("FN").Field<string>("xafAvalaraCompanyCode");
				cancelTaxRequest.DocCode = "ARPay:" + Convert.ToInt16(row["arnARPaymentSessionID"]) + "-" + Convert.ToInt16(row["arnARPaymentHeaderID"]) + "-Inv:" + empty;
				cancelTaxRequest.DocType = DocumentType.SalesInvoice;
				cancelTaxRequest.CancelCode = CancelCode.DocDeleted;
				CancelTaxResult cancelTaxResult = taxSvc.CancelTax(cancelTaxRequest);
				_ = cancelTaxResult.ResultCode;
				AddAvalaraTransaction("ARPaymentSessions", Convert.ToInt16(row["arnARPaymentSessionID"]) + "-" + Convert.ToInt16(row["arnARPaymentHeaderID"]) + ":" + empty, cancelTaxResult);
				if (cancelTaxResult.ResultCode != SeverityLevel.Success)
				{
					stringBuilder.AppendLine(cancelTaxResult.ResultCode.ToString() + ": ");
					if (cancelTaxResult.Messages[0].Summary.Length > 0)
					{
						stringBuilder.AppendLine(cancelTaxResult.Messages[0].Summary);
					}
				}
			}
		}
		return stringBuilder.ToString();
	}

	public bool AddAvalaraTransaction(string table, string recordID, GetTaxResult result)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = Database.GetDataTable("SELECT * FROM AvalaraTransactions WHERE 0=1", fillSchema: true, out adapter);
		DataRow dataRow = null;
		dataRow = dataTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow["avtAvalaraTransactionID"] = Database.NextIDs.GetNextIDForTable("AvalaraTransactions");
		dataRow["avtSourceTable"] = table;
		dataRow["avtSourceTableKeyFields"] = recordID;
		dataRow["avtTransactionDate"] = DateTime.Now;
		dataRow["avtTransactionType"] = AvalaraTransactionType.GetTax;
		dataRow["avtTransactionID"] = ((result.TransactionId == null) ? string.Empty : result.TransactionId);
		dataRow["avtResultCode"] = result.ResultCode;
		if (result.Messages.Count > 0)
		{
			dataRow["avtMessageSummary"] = result.ResultCode.ToString() + ": " + result.Messages[0].Summary;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Message message in result.Messages)
			{
				if (message.Details != null && message.Details.Trim().Length > 0)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(message.Details);
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				dataRow["avtMessageDetail"] = stringBuilder.ToString().Substring(1);
			}
		}
		if (result.ReferenceCode != null)
		{
			dataRow["avtReferenceCode"] = result.ReferenceCode;
		}
		dataRow["avtCreatedBy"] = User.ID;
		dataRow["avtCreatedDate"] = DateTime.Now;
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		if (dataTable.Rows.Count > 0)
		{
			Database.UpdateData(dataTable, adapter);
			return true;
		}
		return false;
	}

	public bool AddAvalaraTransaction(string table, string recordID, PostTaxResult result)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = Database.GetDataTable("SELECT * FROM AvalaraTransactions WHERE 0=1", fillSchema: true, out adapter);
		DataRow dataRow = null;
		dataRow = dataTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow["avtAvalaraTransactionID"] = Database.NextIDs.GetNextIDForTable("AvalaraTransactions");
		dataRow["avtSourceTable"] = table;
		dataRow["avtSourceTableKeyFields"] = recordID;
		dataRow["avtTransactionDate"] = DateTime.Now;
		dataRow["avtTransactionType"] = AvalaraTransactionType.PostTax;
		dataRow["avtTransactionID"] = result.TransactionId;
		dataRow["avtResultCode"] = result.ResultCode;
		if (result.Messages.Count > 0)
		{
			dataRow["avtMessageSummary"] = result.ResultCode.ToString() + ": " + result.Messages[0].Summary;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Message message in result.Messages)
			{
				if (message.Details.Trim().Length > 0)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(message.Details);
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				dataRow["avtMessageDetail"] = stringBuilder.ToString().Substring(1);
			}
		}
		if (result.ReferenceCode != null)
		{
			dataRow["avtReferenceCode"] = result.ReferenceCode;
		}
		dataRow["avtCreatedBy"] = User.ID;
		dataRow["avtCreatedDate"] = DateTime.Now;
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		if (dataTable.Rows.Count > 0)
		{
			Database.UpdateData(dataTable, adapter);
			return true;
		}
		return false;
	}

	public bool AddAvalaraTransaction(string table, string recordID, CancelTaxResult result)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = Database.GetDataTable("SELECT * FROM AvalaraTransactions WHERE 0=1", fillSchema: true, out adapter);
		DataRow dataRow = null;
		dataRow = dataTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow["avtAvalaraTransactionID"] = Database.NextIDs.GetNextIDForTable("AvalaraTransactions");
		dataRow["avtSourceTable"] = table;
		dataRow["avtSourceTableKeyFields"] = recordID;
		dataRow["avtTransactionDate"] = DateTime.Now;
		dataRow["avtTransactionType"] = AvalaraTransactionType.CancelTax;
		dataRow["avtTransactionID"] = result.TransactionId;
		dataRow["avtResultCode"] = result.ResultCode;
		if (result.Messages.Count > 0)
		{
			dataRow["avtMessageSummary"] = result.ResultCode.ToString() + ": " + result.Messages[0].Summary;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Message message in result.Messages)
			{
				if (message.Details.Trim().Length > 0)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(message.Details);
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				dataRow["avtMessageDetail"] = stringBuilder.ToString().Substring(1);
			}
		}
		if (result.ReferenceCode != null)
		{
			dataRow["avtReferenceCode"] = result.ReferenceCode;
		}
		dataRow["avtCreatedBy"] = User.ID;
		dataRow["avtCreatedDate"] = DateTime.Now;
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		if (dataTable.Rows.Count > 0)
		{
			Database.UpdateData(dataTable, adapter);
			return true;
		}
		return false;
	}

	public bool AddAvalaraTransaction(PingResult result)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = Database.GetDataTable("SELECT * FROM AvalaraTransactions WHERE 0=1", fillSchema: true, out adapter);
		DataRow dataRow = null;
		dataRow = dataTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow["avtAvalaraTransactionID"] = Database.NextIDs.GetNextIDForTable("AvalaraTransactions");
		dataRow["avtSourceTable"] = string.Empty;
		dataRow["avtSourceTableKeyFields"] = string.Empty;
		dataRow["avtTransactionDate"] = DateTime.Now;
		dataRow["avtTransactionType"] = AvalaraTransactionType.Ping;
		dataRow["avtTransactionID"] = result.TransactionId;
		dataRow["avtResultCode"] = result.ResultCode;
		if (result.Messages.Count > 0)
		{
			dataRow["avtMessageSummary"] = result.ResultCode.ToString() + ": " + result.Messages[0].Summary;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Message message in result.Messages)
			{
				string details = message.Details;
				if (details != null && details.Trim().Length > 0)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(message.Details);
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				dataRow["avtMessageDetail"] = stringBuilder.ToString().Substring(1);
			}
		}
		if (result.ReferenceCode != null)
		{
			dataRow["avtReferenceCode"] = result.ReferenceCode;
		}
		dataRow["avtCreatedBy"] = User.ID;
		dataRow["avtCreatedDate"] = DateTime.Now;
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		if (dataTable.Rows.Count > 0)
		{
			Database.UpdateData(dataTable, adapter);
			return true;
		}
		return false;
	}

	public int CheckLastSuccessfulTransaction(string table, object[] keyValues)
	{
		AvalaraTransactionType result = (AvalaraTransactionType)0;
		string value = string.Empty;
		if (table.Equals("ARPaymentSessions", StringComparison.CurrentCultureIgnoreCase))
		{
			if (keyValues.Length > 2)
			{
				value = Convert.ToInt16(keyValues[0]) + "-" + Convert.ToInt16(keyValues[1]) + ":" + keyValues[2].ToString();
			}
		}
		else if (keyValues.Length != 0)
		{
			value = keyValues[0].ToString();
		}
		using (SqlCommand sqlCommand = Database.NewSqlCommand("SELECT Top 1 avtTransactionType FROM AvalaraTransactions WHERE avtSourceTable = @table AND avtSourceTableKeyFields = @recordID AND avtResultCode < 2 ORDER BY avtTransactionDate DESC"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.Char, 30)).Value = table;
			sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				result = (AvalaraTransactionType)dataTable.Rows[0].Field<byte>("avtTransactionType");
			}
		}
		return (int)result;
	}
}
