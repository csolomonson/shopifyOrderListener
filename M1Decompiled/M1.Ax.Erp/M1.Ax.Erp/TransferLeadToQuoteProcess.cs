using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferLeadToQuoteProcess : ProcessParameters
{
	public TransferLeadToQuoteProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "lolLeadID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "lolLeadID", "lolLeadLineID" };
		KeyValueTableName = "LeadLines";
		Description = "Use this screen to transfer the selected lead details to a quote.";
		GridID = "M1ADDFROMQUOTELEAD";
		BindingSourceTable = "Quotes";
		ContinueMessage = "This will create a quote from the {0} selected lead lines. Are you sure you want to continue?";
		CreatedBindingSourceCaption = "Create Quote from Lead";
		HelpLink = "qm_createquote.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("lolTransferredToQuote", fieldValue: false, "Lead already transferred to Quote."));
		HeaderSourceFields = new string[16]
		{
			"lopCustomerOrganizationID", "lopLocationID", "lopContactID", "lopShipOrganizationID", "lopShipLocationID", "lopShipContactID", "lopLeadDate", "lopQuoteLocationID", "lopQuoteContactID", "lopPlantID",
			"lopPlantDepartmentID", "lopCurrencyRateID", "lopCustomRate", "lopExchangeRate", "lopProjectID", "lopQuoterEmployeeID"
		};
		HeaderDestinationFields = new string[16]
		{
			"qmpCustomerOrganizationID", "qmpARInvoiceLocationID", "qmpARInvoiceContactID", "qmpShipOrganizationID", "qmpShipLocationID", "qmpShipContactID", "qmpQuoteDate", "qmpQuoteLocationID", "qmpQuoteContactID", "qmpPlantID",
			"qmpPlantDepartmentID", "qmpCurrencyRateID", "qmpCustomRate", "qmpExchangeRate", "qmpProjectID", "qmpQuoterEmployeeID"
		};
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
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		M1DataDictionary obj = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("Leads", "Quotes", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("LeadLines, Leads", "QuoteLines", new string[9] { "lolLeadID", "lolLeadLineID", "lolPartID", "lolPartRevisionID", "lolOrgPartID", "lolOrgPartShortDescription", "lolUnitOfMeasure", "lolDescription", "lolPartGroupID" }, new string[9] { "qmlLeadID", "qmlLeadLineID", "qmlPartID", "qmlPartRevisionID", "qmlOrgPartID", "qmlOrgPartShortDescription", "qmlUnitOfMeasure", "qmlPartShortDescription", "qmlPartGroupID" });
		DataTable dataTable = databaseForRow.GetDataTable("select lolQuantity, lolDiscountPercent, lolGrossAmountForeign, lolTransferredToQuote, lopProjectID, lopProjectAreaID " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from LeadLines  Inner Join Leads On lopLeadID = lolLeadID  where " + text + " order by lolLeadID, lolLeadLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("QuoteLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("QuoteQuantities");
		foreach (DataRow row in dataTable.Rows)
		{
			childBindingSource.ClearCache();
			childBindingSource2.ClearCache();
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addQuoteLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, childBindingSource2);
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		TransferSalespeopleToQuote(parm.BindingSource.Database, sourceHeaderRow.Field<string>("lolLeadID"), parm.BindingSource);
	}

	private void TransferSalespeopleToQuote(M1Database database, string sourceLeadID, M1BindingSource bsQuote)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select lojSalesEmployeeID, lojPercent From LeadSalesPeople Inner Join Employees on lmeEmployeeID = lojSalesEmployeeID Where lojLeadID = @LeadID And lmeSalesEmployee = 1 And lmeTerminationDate is null Order by lojSequenceID");
		sqlCommand.Parameters.Add(new SqlParameter("@LeadID", SqlDbType.NVarChar)).Value = sourceLeadID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = bsQuote.PrimaryTable.GetChildBindingSource("QuoteSalespeople");
		if (childBindingSource.Count != 0)
		{
			childBindingSource.RemoveWhere(string.Empty);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow obj = (DataRow)childBindingSource.AddNew();
			obj["qmjSalesEmployeeID"] = row["lojSalesEmployeeID"];
			obj["qmjPercent"] = row["lojPercent"];
		}
	}

	private void addQuoteLine(M1Database database, DataRow quoteRow, M1BindingSource bsQuoteLines, DataRow lineRow, MatchingFieldsInfo lineMatches, M1BindingSource bsQuoteQuantities)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsQuoteLines, lineMatches, quoteRow);
		dataRow["qmlQuoteLineID"] = bsQuoteLines.GenerateNextID(dataRow);
		decimal num = default(decimal);
		num = ((!(lineRow.Field<decimal>("lolQuantity") == 0m)) ? lineRow.Field<decimal>("lolQuantity") : 1m);
		if (!string.IsNullOrWhiteSpace(lineRow.Field<string>("lopProjectID")))
		{
			dataRow["qmlProjectID"] = lineRow.Field<string>("lopProjectID");
			dataRow["qmlProjectAreaID"] = lineRow.Field<string>("lopProjectAreaID");
		}
		DataRow dataRow2 = bsQuoteQuantities.AddNew(database, dataRow, null, null) as DataRow;
		bsQuoteQuantities.SetKeyToNextAvailable(dataRow2);
		dataRow2["qmqQuoteQuantity"] = num;
		dataRow2["qmqFullRevisedUnitPriceForeign"] = M1Math.Round(lineRow.Field<decimal>("lolGrossAmountForeign") / num, 5);
		dataRow2["qmqDiscountPercent"] = lineRow.Field<decimal>("lolDiscountPercent");
		PriceCalculation sellingPrice = new Part().GetSellingPrice(database, dataRow.Field<string>("qmlPartID"), dataRow.Field<string>("qmlPartRevisionID"), dataRow.Field<string>("qmlPartGroupID"), quoteRow.Field<string>("qmpCustomerOrganizationID"), quoteRow.Field<string>("qmpARInvoiceLocationID"), num, quoteRow.Field<string>("qmpCurrencyRateID"), quoteRow.Field<DateTime?>("qmpQuoteDate"));
		if (sellingPrice != null)
		{
			dataRow2["qmqLeadTime"] = sellingPrice.LeadTime;
		}
		lineRow.SetField("lolTransferredToQuote", value: true);
		bsQuoteLines.SaveData();
		bsQuoteQuantities.SaveData();
	}
}
