using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferSalesOrderToDepositCreditProcess : ProcessParameters
{
	public TransferSalesOrderToDepositCreditProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "omlSalesOrderID" };
		PromptFieldAllowMultiples = false;
		KeyValueFieldNames = new string[2] { "omlSalesOrderID", "omlSalesOrderLineID" };
		KeyValueTableName = "SalesOrderLines";
		Description = "Use this screen to create a credit deposit invoice for a sales order.";
		GridID = "M1ADDFROMARINVOICESOCREDITDEPOSIT";
		HelpLink = "AR_TransferSalesOrderToDepositCredit.htm";
		ContinueMessage = "This will create a credit deposit ar invoice from the {0} selected sales order lines. Are you sure you want to continue?";
		BindingSourceTable = "ARInvoices";
		CreatedBindingSourceCaption = "Create Credit Deposits from Order";
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
		DataTable dataTable = databaseForRow.GetDataTable("select omlDeposit, omlDepositCreated, omlDepositCredited, omlPartID, omlExtendedPriceBase, omlDepositPercent, (omlDepositAmountBase + IsNull((Select Sum(IsNull(arlExtendedPriceBase+arlTaxAmountBase+arlSecondTaxAmountBase+arlFreightAmountBase,0)) As arlExtendedPriceBase From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0)) As CreditAmount,omlTaxCodeID, omlSecondTaxCodeID, omlDepositAmountBase As OriginalDepositAmount,IsNull((Select Sum(IsNull(Round(arlFreightAmountBase,2),0)) As arlTaxAmountBase From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0) As omlFreightAmountBase,IsNull((Select Sum(IsNull(Round(arlFreightAmountForeign,2),0)) As arlTaxAmountBase From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0) As omlFreightAmountForeign,(omlDepositAmountForeign + IsNull((Select Sum(IsNull(arlExtendedPriceForeign+arlTaxAmountForeign+arlSecondTaxAmountForeign+arlFreightAmountForeign,0)) As arlExtendedPriceForeign From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0)) As CreditAmountForeign,IsNull((Select Sum(IsNull(Round(arlTaxAmountBase,2),0)) As arlTaxAmountBase From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0) As TaxAmountBase,IsNull((Select Sum(IsNull(Round(arlTaxAmountForeign,2),0)) As arlTaxAmountForeign From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0) As TaxAmountForeign,IsNull((Select Sum(IsNull(Round(arlSecondTaxAmountBase,2),0)) As arlSecondTaxAmountBase From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0) As SecondTaxAmountBase,IsNull((Select Sum(IsNull(Round(arlSecondTaxAmountForeign,2),0)) As arlSecondTaxAmountForeign From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0) As SecondTaxAmountForeign" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from SalesOrderLines Inner Join SalesOrders On omlSalesOrderID = ompSalesOrderID where omlSalesOrderID Not In (Select arlSalesOrderID From ARInvoices inner join ARInvoicelines On arpARInvoiceID = arlARinvoiceID Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlSalesOrderDeliveryID = 0 And arlDepositLine = 0 And arpInvoiceType = 3 And arpPostedToGL = 0) and (omlDepositAmountBase + IsNull((Select Sum(IsNull(arlExtendedPriceBase+arlTaxAmountBase+arlSecondTaxAmountBase,0)) As arlExtendedPriceBase From ARInvoiceLines Where arlSalesOrderID = omlSalesOrderID And arlSalesOrderLineID = omlSalesOrderLineID And arlDepositLine = 1), 0)) <> 0 and " + text + " order by omlSalesOrderID,omlSalesOrderLineID");
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
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["arpInvoiceType"] = 2;
		currentAsDataRow["arpDepositCredit"] = true;
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
		dataRow["arlPartShortDescription"] = Convert.ToString("Credit Deposit - " + Convert.ToString(M1Math.Round(lineRow.Field<decimal>("omlDepositPercent"), 2)) + "% for " + lineRow["omlPartID"].ToString().Trim()).Substring(0, Math.Min(Convert.ToString("Credit Deposit - " + Convert.ToString(M1Math.Round(lineRow.Field<decimal>("omlDepositPercent"), 2)) + "% for " + lineRow["omlPartID"].ToString().Trim()).Length, bsInvoiceLines.Fields["arlPartShortDescription"].FieldLength));
		dataRow["arlInvoiceQuantity"] = -1;
		dataRow["arlOrderQuantity"] = -1;
		dataRow["arlPayCommission"] = false;
		SqlCommand sqlCommand = database.NewSqlCommand("Select arlTaxCodeID, arlSecondTaxCodeID, arlTaxAmountBase, arlTaxAmountForeign, arlSecondTaxAmountBase, arlSecondTaxAmountForeign, arlFreightAmountBase, arlFreightAmountForeign From ARInvoiceLines Where arlSalesOrderID = @OrderID And arlSalesOrderLineID = @Line And arlSalesOrderDeliveryID = 0 And (arlDepositBalanceBase <> 0 Or arlDepositBalanceForeign <> 0 ) And arlDepositLine = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = lineRow["omlSalesOrderID"];
		sqlCommand.Parameters.Add(new SqlParameter("@Line", SqlDbType.Int)).Value = lineRow["omlSalesOrderLineID"];
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			dataRow["arlTaxCodeID"] = dataTable.Rows[0]["arlTaxCodeID"];
			dataRow["arlSecondTaxCodeID"] = dataTable.Rows[0]["arlSecondTaxCodeID"];
			if (!HeaderFixForeign)
			{
				dataRow["arlTaxAmountBase"] = -1m * (dataTable.Rows[0].Field<decimal>("arlTaxAmountBase") + lineRow.Field<decimal>("TaxAmountBase"));
				dataRow["arlSecondTaxAmountBase"] = -1m * (dataTable.Rows[0].Field<decimal>("arlSecondTaxAmountBase") + lineRow.Field<decimal>("SecondTaxAmountBase"));
				dataRow["arlFreightAmountBase"] = -1m * (dataTable.Rows[0].Field<decimal>("arlFreightAmountBase") + lineRow.Field<decimal>("omlFreightAmountBase"));
			}
			else
			{
				dataRow["arlTaxAmountForeign"] = -1m * (dataTable.Rows[0].Field<decimal>("arlTaxAmountForeign") + lineRow.Field<decimal>("TaxAmountForeign"));
				dataRow["arlSecondTaxAmountForeign"] = -1m * (dataTable.Rows[0].Field<decimal>("arlSecondTaxAmountForeign") + lineRow.Field<decimal>("SecondTaxAmountForeign"));
				dataRow["arlFreightAmountForeign"] = -1m * (dataTable.Rows[0].Field<decimal>("arlFreightAmountForeign") + lineRow.Field<decimal>("omlFreightAmountForeign"));
			}
		}
		decimal num;
		decimal num2;
		if (Convert.ToBoolean(database.Props("AR")["xafARIncludeFrgtInDepositCalc"]))
		{
			num = lineRow.Field<decimal>("CreditAmount") + M1Math.Round(dataRow.Field<decimal>("arlFreightAmountBase"), 2);
			num2 = lineRow.Field<decimal>("CreditAmountForeign") + M1Math.Round(dataRow.Field<decimal>("arlFreightAmountForeign"), 2);
		}
		else
		{
			num = lineRow.Field<decimal>("CreditAmount");
			num2 = lineRow.Field<decimal>("CreditAmountForeign");
		}
		if (!HeaderFixForeign)
		{
			dataRow["arlFullUnitPriceBase"] = M1Math.Round(num + dataRow.Field<decimal>("arlTaxAmountBase") + dataRow.Field<decimal>("arlSecondTaxAmountBase"), 2);
			dataRow["arlDepositBalanceBase"] = M1Math.Round(dataRow.Field<decimal>("arlExtendedPriceBase") + dataRow.Field<decimal>("arlTaxAmountBase") + dataRow.Field<decimal>("arlSecondTaxAmountBase") + dataRow.Field<decimal>("arlFreightAmountBase"), 2);
		}
		else
		{
			dataRow["arlFullUnitPriceForeign"] = M1Math.Round(num2 + dataRow.Field<decimal>("arlTaxAmountForeign") + dataRow.Field<decimal>("arlSecondTaxAmountForeign"), 2);
			dataRow["arlDepositBalanceForeign"] = M1Math.Round(dataRow.Field<decimal>("arlExtendedPriceForeign") + dataRow.Field<decimal>("arlTaxAmountForeign") + dataRow.Field<decimal>("arlSecondTaxAmountForeign") + dataRow.Field<decimal>("arlFreightAmountForeign"), 2);
		}
	}
}
