using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferARInvoiceToCreditProcess : ProcessParameters
{
	public TransferARInvoiceToCreditProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "arlARInvoiceID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "arlARInvoiceID", "arlARInvoiceLineID" };
		KeyValueTableName = "ARInvoiceLines";
		Description = "Select the ar invoice lines to be transferred.";
		GridID = "M1ADDFROMARINVOICEINVOICE";
		BindingSourceTable = "ARInvoices";
		CheckFixForeign = false;
		HelpLink = "AR_TransferARInvoiceToCredit.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("arpPostedToGL", fieldValue: true, "Invoice is not posted."));
		HeaderSourceFields = new string[23]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpPlantID", "arpPlantDepartmentID",
			"arpProjectID", "arpInvoiceDate", "arpPaymentTermID", "arpCurrencyRateID", "arpDepositGLAccountID", "arpFreightTaxCodeID", "arpSecondFreightTaxCodeID", "-arpFreightAmountBase", "-arpFreightTaxAmountBase", "-arpSecondFreightTaxAmtBase",
			"arpCustomRate=Convert(bit,1)", "arpExchangeRate", "arpARInvoiceID"
		};
		HeaderDestinationFields = new string[23]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpPlantID", "arpPlantDepartmentID",
			"arpProjectID", "arpCreditDate", "arpPaymentTermID", "arpCurrencyRateID", "arpDepositGLAccountID", "arpFreightTaxCodeID", "arpSecondFreightTaxCodeID", "arpFreightAmountBase", "arpFreightTaxAmountBase", "arpSecondFreightTaxAmtBase",
			"arpCustomRate", "arpExchangeRate", "arpCreditARInvoiceID"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		if (currentAsDataRow.Field<byte>("arpInvoiceType") != 2)
		{
			currentAsDataRow["arpInvoiceType"] = 2;
		}
		M1DataDictionary obj = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("ARInvoices", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("ARInvoiceLines", "ARInvoiceLines", new string[46]
		{
			"arlLineType", "arlShipmentID", "arlShipmentLineID", "arlSalesOrderID", "arlSalesOrderLineID", "arlSalesOrderDeliveryID", "arlFinanceSourceInvoiceID", "arlPartID", "arlPartRevisionID", "arlOrgPartID",
			"arlOrgPartShortDescription", "arlUnitOfMeasure", "arlPartShortDescription", "arlPartLongDescriptionRTF", "arlPartLongDescriptionText", "arlPartGroupID", "arlProjectID", "arlProjectAreaID", "arlCustomerPO", "arlPayCommission",
			"arlDepositLine", "arlDepositInvoiceID", "arlDepositInvoiceLineID", "arlRetention", "arlRetentionAmountForeign", "arlRetentionDueDate", "arlTaxCodeID", "arlNonTaxReasonID", "arlSecondTaxCodeID", "-arlOrderQuantity",
			"-arlInvoiceQuantity", "arlFullUnitPriceForeign", "arlUnitPriceForeign", "-arlFreightAmountForeign", "-arlTaxAmountForeign", "-arlSecondTaxAmountForeign", "-arlEstTotalMaterialCost", "-arlEstTotalSubcontractCost", "-arlEstTotalLaborCost", "-arlEstTotalOverheadCost",
			"-arlEstTotalCostOfGoodsSold", "-arlActualTotalMaterialCost", "-arlActualTotalSubcontractCost", "-arlActualTotalLaborCost", "-arlActualTotalOverheadCost", "-arlActualTotalCostOfGoodsSold"
		}, new string[46]
		{
			"arlLineType", "arlShipmentID", "arlShipmentLineID", "arlSalesOrderID", "arlSalesOrderLineID", "arlSalesOrderDeliveryID", "arlFinanceSourceInvoiceID", "arlPartID", "arlPartRevisionID", "arlOrgPartID",
			"arlOrgPartShortDescription", "arlUnitOfMeasure", "arlPartShortDescription", "arlPartLongDescriptionRTF", "arlPartLongDescriptionText", "arlPartGroupID", "arlProjectID", "arlProjectAreaID", "arlCustomerPO", "arlPayCommission",
			"arlDepositLine", "arlDepositInvoiceID", "arlDepositInvoiceLineID", "arlRetention", "arlRetentionAmountForeign", "arlRetentionDueDate", "arlTaxCodeID", "arlNonTaxReasonID", "arlSecondTaxCodeID", "arlOrderQuantity",
			"arlInvoiceQuantity", "arlFullUnitPriceForeign", "arlUnitPriceForeign", "arlFreightAmountForeign", "arlTaxAmountForeign", "arlSecondTaxAmountForeign", "arlEstTotalMaterialCost", "arlEstTotalSubcontractCost", "arlEstTotalLaborCost", "arlEstTotalOverheadCost",
			"arlEstTotalCostOfGoodsSold", "arlActualTotalMaterialCost", "arlActualTotalSubcontractCost", "arlActualTotalLaborCost", "arlActualTotalOverheadCost", "arlActualTotalCostOfGoodsSold"
		});
		DataTable dataTable = databaseForRow.GetDataTable("Select arlARInvoiceID,arlARInvoiceLineID," + matchingFieldsInfo.GetSourceFieldList(string.Empty, ",") + matchingFieldsInfo2.GetSourceFieldList(string.Empty, ",") + "(Select Count(*) From ARPaymentLines Where arnARInvoiceID=arpARInvoiceID And arnPostedToGL=0) As OpenPaymentCount,(Select Count(*) From ARPayflowPro Where arxARInvoiceID=arpARInvoiceID And (arxTrxType = 'S' Or (arxTrxType = 'A' And arxCaptured <> 0)) And arxResult = 0) As OpenPayflowCount From ARInvoiceLines inner join ARInvoices on arlARInvoiceID=arpARInvoiceID where " + text + " And (arpPostedToGL=1 And arpInvoicePaidForeign=0 And arpInvoiceType=1 And arpInvoiceBalanceBase<>0) order by arlARInvoiceID,arlARInvoiceLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		foreach (DataRow row in dataTable.Rows)
		{
			if (checkConditions(row, currentAsDataRow, messages))
			{
				row["arpFreightTaxAmountBase"] = 0.0;
				row["arpSecondFreightTaxAmtBase"] = 0.0;
				CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
				TransferLineInfo(this, row, childBindingSource, matchingFieldsInfo2);
			}
		}
		BindingSource.SaveData();
		BindingSource.OnDataChanged(2);
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		TransferSalespeopleToInvoice(parm.BindingSource.Database, parm.BindingSource.CurrentAsDataRow.Field<string>("arpCreditARInvoiceID"), parm.BindingSource);
	}

	private void TransferSalespeopleToInvoice(M1Database database, string sourceInvoiceID, M1BindingSource bsInvoice)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From ARInvoiceSalespeople Where arjARInvoiceID = @InvoiceID");
		sqlCommand.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.NVarChar)).Value = sourceInvoiceID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = bsInvoice.PrimaryTable.GetChildBindingSource("ARInvoiceSalespeople");
		if (childBindingSource.Count != 0)
		{
			childBindingSource.RemoveWhere(string.Empty);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow obj = (DataRow)childBindingSource.AddNew();
			obj["arjSalesEmployeeID"] = row["arjSalesEmployeeID"];
			obj["arjPercent"] = row["arjPercent"];
		}
	}

	private bool checkConditions(DataRow sourceRow, DataRow invoiceRow, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (sourceRow["OpenPaymentCount"] != DBNull.Value && sourceRow.Field<int>("OpenPaymentCount") > 0)
		{
			stringBuilder.Append(string.Format("AR Invoice {0} is linked to an open AR Payment Session. ", sourceRow.Field<string>("arpARInvoiceID")));
		}
		if (sourceRow["OpenPayflowCount"] != DBNull.Value && sourceRow.Field<int>("OpenPayflowCount") > 0)
		{
			stringBuilder.Append(string.Format("AR Invoice {0} has already been processed through Payflow partially or in full and cannot be credited. ", sourceRow.Field<string>("arpARInvoiceID")));
		}
		if (stringBuilder.Length != 0)
		{
			messages.Add(stringBuilder.ToString());
			return false;
		}
		return true;
	}
}
