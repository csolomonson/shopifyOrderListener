using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferSalesOrderToDepositProcess : ProcessParameters
{
	public TransferSalesOrderToDepositProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "omlSalesOrderID" };
		PromptFieldAllowMultiples = false;
		KeyValueFieldNames = new string[2] { "omlSalesOrderID", "omlSalesOrderLineID" };
		KeyValueTableName = "SalesOrderLines";
		Description = "Use this screen to create a deposit invoice for a sales order.";
		GridID = "M1ADDFROMARINVOICESODEPOSIT";
		HelpLink = "OM_CreateDepositInvoice.htm";
		ContinueMessage = "This will create a deposit ar invoice from the {0} selected sales order lines. Are you sure you want to continue?";
		BindingSourceTable = "ARInvoices";
		CreatedBindingSourceCaption = "Create Deposits from Order";
		HeaderSourceFields = new string[20]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "ompShipOrganizationID", "ompShipLocationID", "ompShipContactID", "ompShippingMethodID", "ompShippingPaymentTypeID", "ompOrderDate", "ompFreeOnBoardDescription",
			"ompResellerOrganizationID", "ompResellerLocationID", "ompResellerContactID", "ompProjectID", "ompPlantID", "ompPlantDepartmentID", "ompPaymentTermID", "ompCurrencyRateID", "ompCustomRate", "ompExchangeRate"
		};
		HeaderDestinationFields = new string[20]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpOrderDate", "arpFreeOnBoardDescription",
			"arpResellerOrganizationID", "arpResellerLocationID", "arpResellerContactID", "arpProjectID", "arpPlantID", "arpPlantDepartmentID", "arpPaymentTermID", "arpCurrencyRateID", "arpCustomRate", "arpExchangeRate"
		};
		DefaultValueFieldNames = new string[1] { "arpInvoiceDate" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary obj = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("SalesOrders", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("SalesOrderLines", "ARInvoiceLines", new string[6] { "omlPartGroupID", "ompCustomerPO", "omlProjectID", "omlProjectAreaID", "omlSalesOrderID", "omlSalesOrderLineID" }, new string[6] { "arlPartGroupID", "arlCustomerPO", "arlProjectID", "arlProjectAreaID", "arlSalesOrderID", "arlSalesOrderLineID" });
		DataTable dataTable = databaseForRow.GetDataTable("select omlDepositCreated, omlPartID, omlExtendedPriceBase, omlDepositPercent,omlDepositAmountBase, omlFreightAmountBase, omlFreightAmountForeign, omlPartGroupID, omlTaxCodeID, omlSecondTaxCodeID, omlExtendedPriceForeign, omlDepositAmountForeign,0 As TaxAmountBase, 0 As TaxAmountForeign, 0 As SecondTaxAmountBase, 0 As SecondTaxAmountForeign,ompCustomerPO" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from SalesOrderLines Inner Join SalesOrders On ompSalesOrderID = omlSalesOrderID left outer join ShippingPaymentTypes on xayShippingPaymentTypeID = ompShippingPaymentTypeID where " + text + " order by omlSalesOrderID,omlSalesOrderLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addInvoiceLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row));
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		new AR().TransferOrderSalespeopleToInvoice(parm.BindingSource.Database, sourceHeaderRow.Field<string>("omlSalesOrderID"), parm.BindingSource);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		parm.BindingSource.CurrentAsDataRow["arpInvoiceType"] = 3;
	}

	private void addInvoiceLine(M1Database database, DataRow invoiceRow, M1BindingSource bsInvoiceLines, DataRow lineRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsInvoiceLines, lineMatches);
		string value = database.Props("FN").Field<string>("xafARDepositPartID");
		string value2 = database.Props("FN").Field<string>("xafARDepositPartRevisionID");
		if (!string.IsNullOrWhiteSpace(value))
		{
			dataRow["arlPartID"] = value;
			dataRow["arlPartRevisionID"] = value2;
		}
		else
		{
			dataRow["arlPartID"] = "DEPOSIT";
		}
		dataRow["arlPartShortDescription"] = Convert.ToString("Deposit - " + Convert.ToString(M1Math.Round(lineRow.Field<decimal>("omlDepositPercent"), 2)) + "% for " + lineRow["omlPartID"].ToString().Trim()).Substring(0, Math.Min(Convert.ToString("Deposit - " + Convert.ToString(M1Math.Round(lineRow.Field<decimal>("omlDepositPercent"), 2)) + "% for " + lineRow["omlPartID"].ToString().Trim()).Length, bsInvoiceLines.Fields["arlPartShortDescription"].FieldLength));
		dataRow["arlInvoiceQuantity"] = 1;
		dataRow["arlOrderQuantity"] = 1;
		dataRow["arlPayCommission"] = false;
		decimal num;
		decimal num2;
		if (Convert.ToBoolean(database.Props("AR")["xafARIncludeFrgtInDepositCalc"]))
		{
			num = lineRow.Field<decimal>("omlDepositAmountBase") - M1Math.Round(lineRow.Field<decimal>("omlDepositPercent") / 100m * lineRow.Field<decimal>("omlFreightAmountBase"), 2);
			num2 = lineRow.Field<decimal>("omlDepositAmountForeign") - M1Math.Round(lineRow.Field<decimal>("omlDepositPercent") / 100m * lineRow.Field<decimal>("omlFreightAmountForeign"), 2);
		}
		else
		{
			num = lineRow.Field<decimal>("omlDepositAmountBase");
			num2 = lineRow.Field<decimal>("omlDepositAmountForeign");
		}
		if (Convert.ToBoolean(database.Props("AR")["xafARCalculateTaxOnDeposit"]))
		{
			if (new Financial().IsAvalaraActivated(database))
			{
				dataRow["arlTaxCodeID"] = database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
				dataRow["arlSecondTaxCodeID"] = string.Empty;
				dataRow["arlTaxAmountForeign"] = 0;
				dataRow["arlSecondTaxAmountForeign"] = 0;
			}
			else
			{
				dataRow["arlTaxCodeID"] = lineRow["omlTaxCodeID"];
				dataRow["arlSecondTaxCodeID"] = lineRow["omlSecondTaxCodeID"];
				AppAxFinancial appAxFinancial = new AppAxFinancial(database);
				if (!HeaderFixForeign)
				{
					dataRow["arlTaxAmountBase"] = appAxFinancial.CalculateTaxOnTotal(Convert.ToString(dataRow["arlTaxCodeID"]), Convert.ToDouble(num), invoiceRow["arpInvoiceDate"], Convert.ToString(dataRow["arlSecondTaxCodeID"]), 4);
					dataRow["arlSecondTaxAmountBase"] = appAxFinancial.CalculateTaxOnTotal(Convert.ToString(dataRow["arlSecondTaxCodeID"]), Convert.ToDouble(num), invoiceRow["arpInvoiceDate"], Convert.ToString(dataRow["arlTaxCodeID"]), 4);
				}
				else
				{
					dataRow["arlTaxAmountForeign"] = appAxFinancial.CalculateTaxOnTotal(Convert.ToString(dataRow["arlTaxCodeID"]), Convert.ToDouble(num2), invoiceRow["arpInvoiceDate"], Convert.ToString(dataRow["arlSecondTaxCodeID"]), 4);
					dataRow["arlSecondTaxAmountForeign"] = appAxFinancial.CalculateTaxOnTotal(Convert.ToString(dataRow["arlSecondTaxCodeID"]), Convert.ToDouble(num2), invoiceRow["arpInvoiceDate"], Convert.ToString(dataRow["arlTaxCodeID"]), 4);
				}
			}
		}
		else
		{
			dataRow["arlTaxCodeID"] = string.Empty;
			dataRow["arlSecondTaxCodeID"] = string.Empty;
		}
		if (!HeaderFixForeign)
		{
			dataRow["arlFullUnitPriceBase"] = M1Math.Round(num - dataRow.Field<decimal>("arlTaxAmountBase") - dataRow.Field<decimal>("arlSecondTaxAmountBase"), 2);
		}
		else
		{
			dataRow["arlFullUnitPriceForeign"] = M1Math.Round(num2 - dataRow.Field<decimal>("arlTaxAmountForeign") - dataRow.Field<decimal>("arlSecondTaxAmountForeign"), 2);
		}
		if (Convert.ToBoolean(database.Props("AR")["xafARIncludeFrgtInDepositCalc"]))
		{
			if (!HeaderFixForeign)
			{
				dataRow["arlFreightAmountBase"] = M1Math.Round(lineRow.Field<decimal>("omlDepositPercent") / 100m * lineRow.Field<decimal>("omlFreightAmountBase"), 2);
			}
			else
			{
				dataRow["arlFreightAmountForeign"] = M1Math.Round(lineRow.Field<decimal>("omlDepositPercent") / 100m * lineRow.Field<decimal>("omlFreightAmountForeign"), 2);
			}
		}
		if (!HeaderFixForeign)
		{
			dataRow["arlDepositBalanceBase"] = M1Math.Round(dataRow.Field<decimal>("arlExtendedPriceBase") + dataRow.Field<decimal>("arlTaxAmountBase") + dataRow.Field<decimal>("arlSecondTaxAmountBase") + dataRow.Field<decimal>("arlFreightAmountBase"), 2);
		}
		else
		{
			dataRow["arlDepositBalanceForeign"] = M1Math.Round(dataRow.Field<decimal>("arlExtendedPriceForeign") + dataRow.Field<decimal>("arlTaxAmountForeign") + dataRow.Field<decimal>("arlSecondTaxAmountForeign") + dataRow.Field<decimal>("arlFreightAmountForeign"), 2);
		}
	}
}
