using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ARCalculateFinanceChargeProcess : ProcessParameters
{
	public ARCalculateFinanceChargeProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "arpARInvoiceID" };
		KeyValueTableName = "ARInvoices";
		Description = "Use this screen to calculate finance charges on invoices that are past the due date for customers marked as calculate finance charges. Please ensure to enter a finance charge percent and GL account ID.";
		GridID = "M1ADDFROMARINVOICEFINANCECHARGE";
		HelpLink = "CalculateFinanceCharges.htm";
		ContinueMessage = "This will create finance charge invoices from the {0} selected items. Are you sure you want to continue?";
		BindingSourceTable = "ARInvoices";
		ExtraFieldNames = new string[2] { "FinanceChargeAmount", "arpInvoiceBalanceBase" };
		MultipleDestinationRowsCreated = true;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Organization", null, new string[1] { "arpCustomerOrganizationID" })
		{
			ValueFields = new string[1] { "arpCustomerOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "arpPlantID", "arpPlantDepartmentID" })
		{
			AdditionalFields = "arpPlantID,arpPlantDepartmentID",
			ValueFields = new string[2] { "arpPlantID", "arpPlantDepartmentID" }
		});
		DefaultValueFieldNames = new string[8] { "arpInvoiceDate", "arpGLFiscalYearID", "arpGLFiscalYearPeriodID", "FinancialProperties.xafARFinanceChargeGLAccountID", "FinancialProperties.xafARFinanceChargePercent", "FinancialProperties.xafARFinanceChargeGraceDays", "FinancialProperties.xafARFinanceChargeLastRunDate", "FinancialProperties.xafARFinanceShowCreditBalance" };
		DefaultValueFilterExpression = "App.IIf(Fields(\"xafARFinanceShowCreditBalance\").Value = 2, \"IsNull((Select sum(A.arpInvoiceBalanceBase) as TotalBalance from ARInvoices A, FinancialProperties where A.arpCustomerOrganizationID = ARInvoices.arpCustomerOrganizationID And A.arpARInvoiceLocationID = ARInvoices.arpARInvoiceLocationID And A.arpPostedToGL <> 0 And A.arpPaidComplete = 0 ), 0) > 0\", App.IIf(Fields(\"xafARFinanceShowCreditBalance\").Value = 3, \"IsNull((Select sum(A.arpInvoiceBalanceBase) as TotalBalance from ARInvoices A, FinancialProperties where A.arpCustomerOrganizationID = ARInvoices.arpCustomerOrganizationID And A.arpARInvoiceLocationID = ARInvoices.arpARInvoiceLocationID And A.arpPostedToGL <> 0 And A.arpPaidComplete = 0  And A.arpDueDate < DateAdd(d, -xafARFinanceChargeGraceDays, GETDATE()) ), 0) > 0\", \"\"))";
		HeaderSourceFields = new string[3] { "arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpPlantID" };
		HeaderDestinationFields = new string[3] { "arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpPlantID" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.DefaultFieldValues;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow dataRow = BindingSource.CurrentAsDataRow;
		M1Database database = BindingSource.Database;
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("ARInvoices", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("ARInvoices, ARInvoiceLines", "ARInvoiceLines", new string[1] { "arpARInvoiceID" }, new string[1] { "arlFinanceSourceInvoiceID" });
		DataTable dataTable = database.GetDataTable("select xafARFinanceChargeGLAccountID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from ARInvoices Inner Join Organizations On arpCustomerOrganizationID = cmoOrganizationID Inner Join OrganizationLocations On arpCustomerOrganizationID = cmlOrganizationID And arpARInvoiceLocationID = cmlLocationID Inner Join FinancialProperties on 1=1 where " + text + " order by arpCustomerOrganizationID,arpPlantID,arpARInvoiceID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		string value = string.Empty;
		string value2 = string.Empty;
		_ = string.Empty;
		string text2 = string.Empty;
		string empty = string.Empty;
		foreach (DataRow row in dataTable.Rows)
		{
			if (Convert.ToDecimal(GetItemValuesFromList(selectedItems, row).ExtraFieldValues["FinanceChargeAmount"]) > 0m)
			{
				if (!row.Field<string>("arpCustomerOrganizationID").Equals(value, StringComparison.CurrentCultureIgnoreCase) || (row.Field<string>("arpCustomerOrganizationID").Equals(value, StringComparison.CurrentCultureIgnoreCase) && !row.Field<string>("arpPlantID").Equals(value2, StringComparison.CurrentCultureIgnoreCase)))
				{
					value = string.Empty;
					value2 = string.Empty;
					_ = string.Empty;
					text2 = string.Empty;
				}
				if (text2 == string.Empty)
				{
					dataRow = (DataRow)BindingSource.AddNew();
					BindingSource.SetKeyToNextAvailable(dataRow);
					SetDefaultFieldValues(arg, dataRow);
					BindingSource.ActivateRow(dataRow, null, doFlash: false);
					empty = dataRow.Field<string>("arpARInvoiceID");
				}
				else
				{
					empty = text2;
				}
				CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow);
				addInvoiceLine(database, dataRow, childBindingSource, row, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row));
				value = row.Field<string>("arpCustomerOrganizationID");
				value2 = row.Field<string>("arpPlantID");
				row.Field<string>("arpARInvoiceID");
				if (!text2.Equals(empty, StringComparison.CurrentCultureIgnoreCase))
				{
					text2 = empty;
				}
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
			}
			else
			{
				messages.Add("Invoice " + row.Field<string>("arpARInvoiceID").Trim() + " was not added because the finance charge amount was less than or equal to 0.");
			}
		}
		if (arg.KeysCreated.Count != 0)
		{
			database.ExecuteScalar("Update FinancialProperties Set xafARFinanceChargeLastRunDate = " + DateTime.Now.ToSql());
			database.PropsRefresh();
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "ARINVOICE";
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["arpARGLAccountID"] = parm.BindingSource.Database.Props("AR")["xafARARGLAccountID"];
		currentAsDataRow["arpSalesGLAccountID"] = sourceHeaderRow.Field<string>("xafARFinanceChargeGLAccountID");
		currentAsDataRow["arpFreightGLAccountID"] = parm.BindingSource.Database.Props("AR")["xafARFreightGLAccountID"];
		currentAsDataRow["arpDiscountGLAccountID"] = parm.BindingSource.Database.Props("AR")["xafARDiscountGLAccountID"];
	}

	private void addInvoiceLine(M1Database database, DataRow invoiceRow, M1BindingSource bsInvoiceLines, DataRow lineRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsInvoiceLines, lineMatches, invoiceRow);
		dataRow["arlPartID"] = "FINANCE CHARGE";
		dataRow["arlPartShortDescription"] = Convert.ToString("Finance Charge for Inv" + lineRow.Field<string>("arpARInvoiceID") + " Bal:$" + M1Math.Round(Convert.ToDecimal(itemValues.ExtraFieldValues["arpInvoiceBalanceBase"]), 2));
		dataRow["arlInvoiceQuantity"] = 1;
		dataRow["arlOrderQuantity"] = 1;
		dataRow["arlPayCommission"] = false;
		dataRow["arlFullUnitPriceBase"] = Convert.ToDecimal(itemValues.ExtraFieldValues["FinanceChargeAmount"]);
	}
}
