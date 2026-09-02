using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferARRecurringInvoiceToInvoiceProcess : ProcessParameters
{
	public TransferARRecurringInvoiceToInvoiceProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "arrARRecurringInvoiceID" };
		KeyValueTableName = "ARRecurringInvoices";
		Description = "Use this screen to create AR Invoices from your recurring AR Invoices.";
		GridID = "M1ADDFROMARINVOICERECURRING";
		HelpLink = "AR_RecurringInvoiceWizard.htm";
		ContinueMessage = "This will create AR Invoices from the {0} selected recurring AR Invoices. Are you sure you want to continue?";
		BindingSourceTable = "ARInvoices";
		MultipleDestinationRowsCreated = true;
		HeaderSourceFields = new string[26]
		{
			"arrARInvoiceType", "arrCustomerOrganizationID", "arrARInvoiceLocationID", "arrARInvoiceContactID", "arrShipOrganizationID", "arrShipLocationID", "arrShipContactID", "arrResellerOrganizationID", "arrResellerLocationID", "arrResellerContactID",
			"arrIncludeFreightInPrice", "arrShippingMethodID", "arrShippingPaymentTypeID", "arrStandardMessageID", "arrInvoiceCommentsRTF", "arrInvoiceCommentsText", "arrPlantID", "arrPlantDepartmentID", "arrProjectID", "arrPaymentTermID",
			"arrCurrencyRateID", "arrCustomRate", "arrExchangeRate", "arrFreightTaxCodeID", "arrSecondFreightTaxCodeID", "arrFreeOnBoardDescription"
		};
		HeaderDestinationFields = new string[26]
		{
			"arpInvoiceType", "arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpResellerOrganizationID", "arpResellerLocationID", "arpResellerContactID",
			"arpIncludeFreightInPrice", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpStandardMessageID", "arpInvoiceCommentsRTF", "arpInvoiceCommentsText", "arpPlantID", "arpPlantDepartmentID", "arpProjectID", "arpPaymentTermID",
			"arpCurrencyRateID", "arpCustomRate", "arpExchangeRate", "arpFreightTaxCodeID", "arpSecondFreightTaxCodeID", "arpFreeOnBoardDescription"
		};
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Recurrence Type", null, new string[1] { "arrRecurrenceType" })
		{
			ValueFields = new string[1] { "arrRecurrenceType" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Type", null, new string[1] { "arrARInvoiceType" })
		{
			ValueFields = new string[1] { "arrARInvoiceType" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Cycle Code", null, new string[1] { "arrCycleCode" })
		{
			ValueFields = new string[1] { "arrCycleCode" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Organization", null, new string[1] { "arrCustomerOrganizationID" })
		{
			ValueFields = new string[1] { "arrCustomerOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "arrPlantID", "arrPlantDepartmentID" })
		{
			AdditionalFields = "arrPlantID,arrPlantDepartmentID",
			ValueFields = new string[2] { "arrPlantID", "arrPlantDepartmentID" }
		});
		DefaultValueFieldNames = new string[3] { "arpInvoiceDate", "arpGLFiscalYearID", "arpGLFiscalYearPeriodID" };
		DefaultValueFilterExpression = "(arrInactive = 0 Or (arrInactive = 1 And arrInactiveDate > arpInvoiceDate)) And (arrStartGLFiscalYearID <= arpGLFiscalYearID And (arrStartGLFiscalYearID <> arpGLFiscalYearID Or arrStartGLFiscalYearID = arpGLFiscalYearID And arrStartGLFiscalYearPeriodID <= arpGLFiscalYearPeriodID)) And (arrEndGLFiscalYearID = 0 Or (arrEndGLFiscalYearID >= arpGLFiscalYearID And (arrEndGLFiscalYearID <> arpGLFiscalYearID Or arrEndGLFiscalYearID = arpGLFiscalYearID And arrEndGLFiscalYearPeriodID >= arpGLFiscalYearPeriodID)))";
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		Dictionary<string, object> defaultFieldValues = arg.DefaultFieldValues;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow dataRow = BindingSource.CurrentAsDataRow;
		M1Database database = BindingSource.Database;
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("ARRecurringInvoices", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("ARRecurringInvoiceLines", "ARInvoiceLines", new string[20]
		{
			"arqARRecurringInvoiceID", "arqARRecurringInvoiceLineID", "arqPartID", "arqPartRevisionID", "arqOrgPartID", "arqOrgPartShortDescription", "arqUnitOfMeasure", "arqPartShortDescription", "arqPartLongDescriptionRTF", "arqPartLongDescriptionText",
			"arqPartGroupID", "arqProjectID", "arqProjectAreaID", "arqCustomerPO", "arqPayCommission", "arqTaxCodeID", "arqNonTaxReasonID", "arqSecondTaxCodeID", "arqOrderQuantity", "arqInvoiceQuantity"
		}, new string[20]
		{
			"arlARRecurringInvoiceID", "arlARRecurringInvoiceLineID", "arlPartID", "arlPartRevisionID", "arlOrgPartID", "arlOrgPartShortDescription", "arlUnitOfMeasure", "arlPartShortDescription", "arlPartLongDescriptionRTF", "arlPartLongDescriptionText",
			"arlPartGroupID", "arlProjectID", "arlProjectAreaID", "arlCustomerPO", "arlPayCommission", "arlTaxCodeID", "arlNonTaxReasonID", "arlSecondTaxCodeID", "arlOrderQuantity", "arlInvoiceQuantity"
		});
		DataTable dataTable = database.GetDataTable("Select arrARRecurringInvoiceID,arrInvoiceDay,arrLastTransferredDate,arrFreightAmountForeign,arqFreightAmountForeign,arqFullUnitPriceForeign,arqUnitPriceForeign,arqTaxAmountForeign,arqSecondTaxAmountForeign,arqDiscountPercent,arqUnitDiscountForeign" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " From ARRecurringInvoices Inner Join ARRecurringInvoiceLines On arrARRecurringInvoiceID = arqARRecurringInvoiceID  Where " + text + " order by arqARRecurringInvoiceID, arqARRecurringInvoiceLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		short num = 0;
		byte b = 0;
		if (defaultFieldValues["arpGLFiscalYearID"] != DBNull.Value)
		{
			num = Convert.ToInt16(defaultFieldValues["arpGLFiscalYearID"]);
		}
		if (defaultFieldValues["arpGLFiscalYearPeriodID"] != DBNull.Value)
		{
			b = Convert.ToByte(defaultFieldValues["arpGLFiscalYearPeriodID"]);
		}
		DateTime dateTime = ((defaultFieldValues["arpInvoiceDate"] != DBNull.Value) ? Convert.ToDateTime(defaultFieldValues["arpInvoiceDate"]) : DateTime.Today);
		if (num == 0 || b == 0)
		{
			YearAndPeriod yearAndPeriod = new Financial().GetYearAndPeriod(database, dateTime, "AR");
			if (yearAndPeriod.Success)
			{
				num = yearAndPeriod.Year;
				b = yearAndPeriod.Period;
			}
			yearAndPeriod = null;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		int obj2 = 0;
		string text2 = string.Empty;
		string empty = string.Empty;
		foreach (DataRow row in dataTable.Rows)
		{
			if (!row.Field<int>("arqARRecurringInvoiceID").Equals(obj2))
			{
				obj2 = 0;
				text2 = string.Empty;
			}
			if (text2 == string.Empty)
			{
				dataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(dataRow);
				BindingSource.ActivateRow(dataRow, null, doFlash: false);
				empty = dataRow.Field<string>("arpARInvoiceID");
				dataRow["arpGLFiscalYearID"] = num;
				dataRow["arpGLFiscalYearPeriodID"] = b;
				setInvoiceDate(dataRow, database, num, b, dateTime, row);
			}
			else
			{
				empty = text2;
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow);
			addInvoiceLine(childBindingSource, row, dataRow, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row));
			obj2 = row.Field<int>("arqARRecurringInvoiceID");
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
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "ARInvoice";
		}
	}

	private static void setInvoiceDate(DataRow invoiceRow, M1Database database, short glYear, byte glPeriod, DateTime invoiceDate, DataRow recurringRow)
	{
		if (glYear == 0 || glPeriod == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT glfStartDate, glfEndDate FROM GLFiscalYearPeriods WHERE glfGLFiscalYearID = @GLYear and glfGLfiscalYearPeriodID = @GLPeriod");
		sqlCommand.Parameters.Add(new SqlParameter("@GLYear", SqlDbType.Int)).Value = glYear;
		sqlCommand.Parameters.Add(new SqlParameter("@GLPeriod", SqlDbType.Int)).Value = glPeriod;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		DataRow row = dataTable.Rows[0];
		num = row.Field<DateTime>("glfStartDate").Day;
		if (num <= recurringRow.Field<byte>("arrInvoiceDay"))
		{
			num = recurringRow.Field<byte>("arrInvoiceDay");
			num2 = row.Field<DateTime>("glfStartDate").Month;
			num3 = row.Field<DateTime>("glfStartDate").Year;
			int num4 = DateTime.DaysInMonth(num3, num2);
			if (num4 > 0)
			{
				num = recurringRow.Field<byte>("arrInvoiceDay");
				if (num > 0 && num > num4)
				{
					num = num4;
				}
			}
		}
		else
		{
			num = row.Field<DateTime>("glfEndDate").Day;
			if (num >= recurringRow.Field<byte>("arrInvoiceDay"))
			{
				num = recurringRow.Field<byte>("arrInvoiceDay");
				num2 = row.Field<DateTime>("glfEndDate").Month;
				num3 = row.Field<DateTime>("glfEndDate").Year;
				int num4 = DateTime.DaysInMonth(num3, num2);
				if (num4 > 0)
				{
					num = recurringRow.Field<byte>("arrInvoiceDay");
					if (num > 0 && num > num4)
					{
						num = num4;
					}
				}
			}
		}
		if (num3 == 0 || num2 == 0 || num == 0)
		{
			invoiceRow["arpInvoiceDate"] = invoiceDate;
		}
		else
		{
			invoiceRow["arpInvoiceDate"] = new DateTime(num3, num2, num);
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		TransferSalespeopleToInvoice(parm.BindingSource.Database, sourceHeaderRow.Field<int>("arrARRecurringInvoiceID"), parm.BindingSource);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["arpFreightAmountForeign"] = sourceHeaderRow["arrFreightAmountForeign"];
		currentAsDataRow["arpRecurringInvoice"] = true;
		currentAsDataRow["arpReadyToPrint"] = true;
		if (Convert.ToInt16(currentAsDataRow["arpInvoiceType"]) == 2)
		{
			currentAsDataRow["arpCreditDate"] = currentAsDataRow["arpInvoiceDate"];
		}
		currentAsDataRow["arpOrderDate"] = currentAsDataRow["arpInvoiceDate"];
		currentAsDataRow["arpOriginalExchangeRate"] = currentAsDataRow["arpExchangeRate"];
		parm.BindingSource.Database.ExecuteScalar("UPDATE ARRecurringInvoices SET arrLastTransferredDate = " + DateTime.Now.Date.ToSql() + " WHERE arrARRecurringInvoiceID = " + sourceHeaderRow["arrARRecurringInvoiceID"].ToSql());
	}

	private void TransferSalespeopleToInvoice(M1Database database, int sourceRecurringInvoiceID, M1BindingSource bsInvoice)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From ARRecurringInvoiceSalespeople Where aroARRecurringInvoiceID = @InvoiceID");
		sqlCommand.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.Int)).Value = sourceRecurringInvoiceID;
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
			obj["arjSalesEmployeeID"] = row["aroSalesEmployeeID"];
			obj["arjPercent"] = row["aroPercent"];
		}
	}

	private void addInvoiceLine(M1BindingSource bsInvoiceLines, DataRow recurringRow, DataRow invoiceRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, recurringRow, bsInvoiceLines, lineMatches, invoiceRow);
		dataRow["arlFullUnitPriceForeign"] = recurringRow["arqFullUnitPriceForeign"];
		dataRow["arlUnitPriceForeign"] = recurringRow["arqUnitPriceForeign"];
		dataRow["arlTaxAmountForeign"] = recurringRow["arqTaxAmountForeign"];
		dataRow["arlSecondTaxAmountForeign"] = recurringRow["arqSecondTaxAmountForeign"];
		dataRow["arlDiscountPercent"] = recurringRow["arqDiscountPercent"];
		dataRow["arlUnitDiscountForeign"] = recurringRow["arqUnitDiscountForeign"];
		dataRow["arlFreightAmountForeign"] = recurringRow["arqFreightAmountForeign"];
	}
}
