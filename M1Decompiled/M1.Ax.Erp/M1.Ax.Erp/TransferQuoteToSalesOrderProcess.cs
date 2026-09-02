using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

public class TransferQuoteToSalesOrderProcess : ProcessParameters
{
	public TransferQuoteToSalesOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "qmlQuoteID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "qmlQuoteID", "qmlQuoteLineID" };
		KeyValueTableName = "QuoteLines";
		Description = "Use this screen to create a sales order from quotes.";
		GridID = "M1ADDFROMSOQUOTE";
		BindingSourceTable = "SalesOrders";
		CreatedBindingSourceCaption = "Create Sales Order From Quote";
		HelpLink = "OM_SOWizard.htm";
		ContinueMessage = "This will create a sales order from the {0} selected quote lines. Are you sure you want to continue?";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Exclude quote line records already transferred to order?")
		{
			Value = true,
			AdoFilterExpression = "qmlTransferredToOrder = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "qmlTransferredToOrder"
		});
		DefaultValueFieldNames = new string[2] { "ompRequestedShipDate", "ompCustomerPO" };
		HeaderSourceFields = new string[16]
		{
			"qmpCustomerOrganizationID", "qmpARInvoiceLocationID", "qmpARInvoiceContactID", "qmpShipOrganizationID", "qmpShipLocationID", "qmpShipContactID", "qmpQuoteLocationID", "qmpQuoteContactID", "qmpCurrencyRateID", "qmpCustomRate",
			"qmpExchangeRate", "qmpProjectID", "qmpShippingMethodID", "qmpShippingPaymentTypeID", "qmpPaymentTermID", "qmpFreeOnBoardDescription"
		};
		HeaderDestinationFields = new string[16]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "ompShipOrganizationID", "ompShipLocationID", "ompShipContactID", "ompQuoteLocationID", "ompQuoteContactID", "ompCurrencyRateID", "ompCustomRate",
			"ompExchangeRate", "ompProjectID", "ompShippingMethodID", "ompShippingPaymentTypeID", "ompPaymentTermID", "ompFreeOnBoardDescription"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		Dictionary<string, object> defaultFieldValues = arg.DefaultFieldValues;
		List<string> messages = arg.Messages;
		List<string> list = new List<string>();
		CreatedBindingSource = false;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		M1Database database = BindingSource.Database;
		DateTime? requestedShipDate = null;
		if (defaultFieldValues["ompRequestedShipDate"] != DBNull.Value)
		{
			requestedShipDate = Convert.ToDateTime(defaultFieldValues["ompRequestedShipDate"]);
		}
		if (!(database.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary))
		{
			return;
		}
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("Quotes", "SalesOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("QuoteLines", "SalesOrderLines", new string[17]
		{
			"qmlPartID", "qmlPartRevisionID", "qmlOrgPartID", "qmlOrgPartShortDescription", "qmlUnitOfMeasure", "qmlPartShortDescription", "qmlPartLongDescriptionRTF", "qmlPartLongDescriptionText", "qmlPartGroupID", "qmlProjectID",
			"qmlProjectAreaID", "qmlQuoteID", "qmlQuoteLineID", "qmlDocuments", "qmlTaxCodeID", "qmlNonTaxReasonID", "qmlSecondTaxCodeID"
		}, new string[17]
		{
			"omlPartID", "omlPartRevisionID", "omlOrgPartID", "omlOrgPartShortDescription", "omlUnitOfMeasure", "omlPartShortDescription", "omlPartLongDescriptionRTF", "omlPartLongDescriptionText", "omlPartGroupID", "omlProjectID",
			"omlProjectAreaID", "omlQuoteID", "omlQuoteLineID", "omlDocuments", "omlTaxCodeID", "omlNonTaxReasonID", "omlSecondTaxCodeID"
		});
		MatchingFieldsInfo addlChargeMatches = m1DataDictionary.FindMatchingFields("QuoteLines", "SalesOrderLines", new string[8] { "qmlProjectID", "qmlProjectAreaID", "qmlQuoteID", "qmlQuoteLineID", "qmlDocuments", "qmlTaxCodeID", "qmlNonTaxReasonID", "qmlSecondTaxCodeID" }, new string[8] { "omlProjectID", "omlProjectAreaID", "omlQuoteID", "omlQuoteLineID", "omlDocuments", "omlTaxCodeID", "omlNonTaxReasonID", "omlSecondTaxCodeID" });
		MatchingFieldsInfo jobMatches = m1DataDictionary.FindMatchingFields("QuoteLines, Quotes", "Job", new string[18]
		{
			"qmlPartID", "qmlPartRevisionID", "qmlPartShortDescription", "qmlUnitOfMeasure", "qmlPartLongDescriptionRTF", "qmlPartLongDescriptionText", "qmlProductionNotesRTF", "qmlProductionNotesText", "qmlDocuments", "qmlProjectID",
			"qmlProjectAreaID", "qmlQuoteID", "qmlQuoteLineID", "qmpCustomerOrganizationID", "qmpShipOrganizationID", "qmpShipLocationID", "qmpPlantID", "qmpPlantDepartmentID"
		}, new string[18]
		{
			"jmpPartID", "jmpPartRevisionID", "jmpPartShortDescription", "jmpUnitOfMeasure", "jmpPartLongDescriptionRTF", "jmpPartLongDescriptionText", "jmpProductionNotesRTF", "jmpProductionNotesText", "jmpDocuments", "jmpProjectID",
			"jmpProjectAreaID", "jmpQuoteID", "jmpQuoteLineID", "jmpCustomerOrganizationID", "jmpShipOrganizationID", "jmpShipLocationID", "jmpPlantID", "jmpPlantDepartmentID"
		});
		DataTable dataTable = database.GetDataTable("select qmpPlantID,qmpPlantDepartmentID,qmlSupplierOrganizationID, qmlPurchaseLocationID, qmlPurchaseUnitCostBase, qmlPurchaseUnitCostForeign, qmlPurchaseToOrder, qmlProductionNotesRTF, qmlProductionNotesText, qmlUniqueID, cmoCustomerStatus " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from QuoteLines Inner Join Quotes On qmpQuoteID = qmlQuoteID Inner Join Organizations on qmpCustomerOrganizationID = cmoOrganizationID where " + text + " order by qmlQuoteID,qmlQuoteLineID");
		DataTable dataTable2 = database.GetDataTable("select qmqQuoteID,qmqQuoteLineID,qmqQuoteQuantityID,qmqQuoteQuantity,qmqScrapPercent,qmqRevisedUnitPriceBase,qmqRevisedUnitPriceForeign,qmqAdditionalChargeBase,qmqAdditionalChargeForeign,qmqAdditionalChargeDescription,qmqLeadTime from QuoteQuantities Inner Join QuoteLines on qmqQuoteID = qmlQuoteID and qmqQuoteLineID = qmlQuoteLineID where " + text + " order by qmqQuoteLineID,qmqQuoteQuantityID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("SalesOrderLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries");
		DataTable dataTable3 = childBindingSource2.GetDataTable();
		M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "Jobs", null, true, loadDataNow: false);
		M1BindingSource m1BindingSource2 = new M1BindingSource(database);
		m1BindingSource2.LoadDefinition(string.Empty, "SalesOrderJobLinks", null, true, loadDataNow: false);
		List<string> list2 = new List<string>();
		foreach (DataRow row in dataTable.Rows)
		{
			if (!list2.Contains(row.Field<string>("qmlQuoteID")))
			{
				list2.Add(row.Field<string>("qmlQuoteID"));
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			if (checkConditions(row, GetItemValuesFromList(selectedItems, row), requestedShipDate, messages) && !processOrderLine(database, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, childBindingSource2, dataTable3, m1BindingSource, jobMatches, m1BindingSource2, addlChargeMatches, dataTable2, GetItemValuesFromList(selectedItems, row), messages, requestedShipDate, arg, list))
			{
				break;
			}
		}
		if (arg.KeysCreated.Count != 0)
		{
			foreach (string item in list)
			{
				messages.Add(item);
			}
			arg.OpenKeysWithObjectID = "SalesOrder";
			BindingSource.OnDataChanged(2);
			database.OnTableChanged(new TableChangedEventArgs("Quotes", null, null, null));
		}
		else
		{
			messages.Add("No Sales Order lines were added");
		}
		object[] parameters = ((currentAsDataRow.RowState == DataRowState.Detached) ? null : new object[1] { currentAsDataRow.Field<string>("ompSalesOrderID") });
		object[] parametersEx = list2.ToArray();
		arg.ActionMessagesArgs = new ActionMessagesEventArgs("PULLFROMQUOTE_FINISHED", parameters, parametersEx);
	}

	private bool doSave(M1BindingSource curBs, StartProcessEventArgs arg, List<string> warningMessages)
	{
		if (curBs != null)
		{
			ErrorItemsList errors = curBs.GetErrors();
			if (errors.Count != 0)
			{
				foreach (ValidationInfo item2 in errors)
				{
					if ((item2.Field != null && item2.Field.Custom) || item2.ErrorCount != 0)
					{
						continue;
					}
					foreach (ErrorItem error in item2.Errors)
					{
						if ((error.IsWarning() || error.IsMessage()) && !warningMessages.Contains(error.ErrorText))
						{
							warningMessages.Add(error.ErrorText);
						}
					}
				}
			}
			if (ValidateAndSave(curBs, arg, hideWarnings: true))
			{
				if (!string.IsNullOrWhiteSpace(curBs.CurrentAsDataRow.Field<string>("ompSalesOrderID")))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { curBs.CurrentAsDataRow.Field<string>("ompSalesOrderID") };
					keysCreated.Add(item);
				}
				return true;
			}
		}
		return false;
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["ompPlantID"] = sourceHeaderRow["qmpPlantID"];
		currentAsDataRow["ompPlantDepartmentID"] = sourceHeaderRow["qmpPlantDepartmentID"];
		TransferSalespeopleToOrder(parm.BindingSource.Database, sourceHeaderRow.Field<string>("qmlQuoteID"), parm.BindingSource);
	}

	private void TransferSalespeopleToOrder(M1Database database, string sourceQuoteID, M1BindingSource bsOrder)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select qmjSalesEmployeeID, qmjPercent From QuoteSalespeople Inner Join Employees on lmeEmployeeID = qmjSalesEmployeeID Where qmjQuoteID = @QuoteID And lmeSalesEmployee = 1 And lmeTerminationDate is null Order by qmjSequenceID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = sourceQuoteID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = bsOrder.PrimaryTable.GetChildBindingSource("SalesOrderSalespeople");
		if (childBindingSource.Count != 0)
		{
			childBindingSource.RemoveWhere(string.Empty);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = (DataRow)childBindingSource.AddNew();
			if (dataRow2 != null)
			{
				dataRow2["omiSalesEmployeeID"] = row["qmjSalesEmployeeID"];
				dataRow2["omiPercent"] = row["qmjPercent"];
			}
		}
	}

	private bool processOrderLine(M1Database database, DataRow soRow, M1BindingSource bsSOLines, DataRow lineRow, MatchingFieldsInfo lineMatches, M1BindingSource bsDeliveries, DataTable dtDeliveries, M1BindingSource bsJobs, MatchingFieldsInfo jobMatches, M1BindingSource bsJobLinks, MatchingFieldsInfo addlChargeMatches, DataTable dtQuoteQuantities, ProcessSelectedItemValues itemValues, List<string> messages, DateTime? requestedShipDate, StartProcessEventArgs arg, List<string> warningMessages)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsSOLines, lineMatches, soRow);
		bsSOLines.SetKeyToNextAvailable(dataRow);
		decimal num = default(decimal);
		if (itemValues.EditableValues.ContainsKey("OrderQty"))
		{
			num = Convert.ToDecimal(itemValues.EditableValues["OrderQty"]);
		}
		dataRow.SetField("omlOrderQuantity", num);
		decimal value = Convert.ToDecimal(itemValues.EditableValues["UnitDiscountForeign"]);
		dataRow.SetField("omlUnitPriceForeign", Convert.ToDecimal(itemValues.EditableValues["UnitPriceForeign"]));
		dataRow.SetField("omlFullUnitPriceForeign", Convert.ToDecimal(itemValues.EditableValues["UnitPriceForeign"]) + Convert.ToDecimal(value));
		dataRow.SetField("omlUnitDiscountForeign", Convert.ToDecimal(itemValues.EditableValues["UnitDiscountForeign"]));
		DateTime? dateTime = null;
		if (itemValues.EditableValues.ContainsKey("DeliveryDate") && itemValues.EditableValues["DeliveryDate"] != DBNull.Value)
		{
			dateTime = Convert.ToDateTime(itemValues.EditableValues["DeliveryDate"]);
		}
		if (!dateTime.HasValue && requestedShipDate.HasValue)
		{
			dateTime = Convert.ToDateTime(requestedShipDate);
		}
		if (!dateTime.HasValue)
		{
			dateTime = DateTime.Now;
		}
		Job job = new Job();
		bool flag = false;
		string text = string.Empty;
		if (itemValues.EditableValues.ContainsKey("CreateJob"))
		{
			flag = Convert.ToBoolean(itemValues.EditableValues["CreateJob"]);
		}
		if (itemValues.EditableValues.ContainsKey("JobID"))
		{
			text = Convert.ToString(itemValues.EditableValues["JobID"]).Trim().ToUpper();
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			text = job.GetJobIDForOrder(database, dataRow.Field<string>("omlSalesOrderID"), dataRow.Field<short>("omlSalesOrderLineID"), false);
		}
		dataRow.SetField("omlQuoteQuantityID", getQuoteQuantityForLine(dtQuoteQuantities, Convert.ToString(lineRow["qmlQuoteID"]), Convert.ToInt32(lineRow["qmlQuoteLineID"]), num));
		DataRow[] array = dtDeliveries.Select("omdSalesOrderID = " + dataRow.Field<string>("omlSalesOrderID").ToLinq() + " And omdSalesOrderLineID = " + dataRow.Field<short>("omlSalesOrderLineID").ToLinq());
		DataRow dataRow2 = ((array.Length != 0) ? array[0] : (bsDeliveries.AddNew(database, dataRow, null, null) as DataRow));
		if (dataRow2 != null)
		{
			dataRow2["omdDeliveryDate"] = Convert.ToDateTime(dateTime);
			dataRow2.SetField("omdDeliveryQuantity", num);
			byte b = 0;
			if (dataRow2["omdDeliveryType"] != null)
			{
				b = Convert.ToByte(dataRow2["omdDeliveryType"]);
			}
			if (b == 0)
			{
				b = database.Props("OM").Field<byte>("xapOMDeliveryType");
				if (b == 0)
				{
					b = 2;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(text))
			{
				dataRow2["omdDeliveryType"] = 1;
			}
			else if (lineRow.Field<bool>("qmlPurchaseToOrder"))
			{
				dataRow2["omdDeliveryType"] = 5;
				dataRow2["omdSupplierOrganizationID"] = lineRow["qmlSupplierOrganizationID"];
				dataRow2["omdPurchaseLocationID"] = lineRow["qmlPurchaseLocationID"];
				if (!HeaderFixForeign)
				{
					dataRow2["omdPurchaseUnitCostBase"] = lineRow["qmlPurchaseUnitCostBase"];
				}
				else
				{
					dataRow2["omdPurchaseUnitCostForeign"] = lineRow["qmlPurchaseUnitCostForeign"];
				}
			}
			else
			{
				dataRow2["omdDeliveryType"] = b;
			}
		}
		if (itemValues.EditableValues.ContainsKey("AdditionalChargeDescription") && itemValues.EditableValues.ContainsKey("AdditionalChargeBase") && !string.IsNullOrWhiteSpace(Convert.ToString(itemValues.EditableValues["AdditionalChargeDescription"])) && Convert.ToDecimal(itemValues.EditableValues["AdditionalChargeBase"]) != 0m)
		{
			DataRow dataRow3 = TransferLineInfo(this, lineRow, bsSOLines, addlChargeMatches, soRow);
			string value2 = database.Props("OM").Field<string>("xapOMAddlChargePartID");
			string value3 = database.Props("OM").Field<string>("xapOMAddlChargePartRevisionID");
			if (!string.IsNullOrWhiteSpace(value2))
			{
				dataRow3["omlPartID"] = value2;
				dataRow3["omlPartRevisionID"] = value3;
			}
			else
			{
				dataRow3["omlPartID"] = Convert.ToString(itemValues.EditableValues["AdditionalChargeDescription"]).Substring(0, Math.Min(Convert.ToString(itemValues.EditableValues["AdditionalChargeDescription"]).Length, bsSOLines.Fields["omlPartID"].FieldLength));
			}
			dataRow3["omlPartShortDescription"] = Convert.ToString(itemValues.EditableValues["AdditionalChargeDescription"]).Substring(0, Math.Min(Convert.ToString(itemValues.EditableValues["AdditionalChargeDescription"]).Length, bsSOLines.Fields["omlPartShortDescription"].FieldLength));
			if (itemValues.EditableValues.ContainsKey("kbpPartGroupID"))
			{
				dataRow3["omlPartGroupID"] = Convert.ToString(itemValues.EditableValues["kbpPartGroupID"]);
			}
			dataRow3["omlUnitOfMeasure"] = database.Props("OM").Field<string>("xapOMUnitOfMeasure");
			dataRow3["omlOrderQuantity"] = 1;
			dataRow3["omlFullUnitPriceForeign"] = Convert.ToDecimal(itemValues.EditableValues["AdditionalChargeForeign"]);
			dataRow3["omlUnitPriceForeign"] = Convert.ToDecimal(itemValues.EditableValues["AdditionalChargeForeign"]);
			dataRow3["omlPayCommission"] = true;
			array = dtDeliveries.Select("omdSalesOrderID = " + dataRow3.Field<string>("omlSalesOrderID").ToLinq() + " And omdSalesOrderLineID = " + dataRow3.Field<short>("omlSalesOrderLineID").ToLinq());
			DataRow dataRow4 = ((array.Length != 0) ? array[0] : (bsDeliveries.AddNew(database, dataRow3, null, null) as DataRow));
			dataRow4["omdDeliveryDate"] = Convert.ToDateTime(dateTime);
			dataRow4["omdDeliveryQuantity"] = 1;
			dataRow4["omdDeliveryType"] = 2;
		}
		if (doSave(BindingSource, arg, warningMessages))
		{
			if (flag && !string.IsNullOrWhiteSpace(text))
			{
				processJob(database, lineRow, dataRow, dataRow2, text, bsJobs, jobMatches, bsJobLinks, job, itemValues, messages);
			}
			return true;
		}
		arg.Cancel = true;
		return false;
	}

	private void processJob(M1Database database, DataRow lineRow, DataRow soLineRow, DataRow delRow, string jobID, M1BindingSource bsJobs, MatchingFieldsInfo jobMatches, M1BindingSource bsJobLinks, Job jobObj, ProcessSelectedItemValues itemValues, List<string> messages)
	{
		if (!jobObj.DoesJobExist(database, null, jobID))
		{
			DataRow dataRow = TransferLineInfo(this, lineRow, bsJobs, jobMatches);
			dataRow["jmpJobID"] = jobID;
			dataRow["jmpOrderQuantity"] = soLineRow.Field<decimal>("omlOrderQuantity");
			dataRow["jmpProductionDueDate"] = delRow.Field<DateTime>("omdDeliveryDate");
			dataRow["jmpPartWareHouseLocationID"] = delRow.Field<string>("omdPartWarehouseLocationId");
			dataRow["jmpPartBinID"] = delRow.Field<string>("omdPartBinId");
			bsJobs.SaveData();
			SqlCommand sqlCommand = database.NewSqlCommand("DELETE FROM FormInputValues Where xaiSourceUniqueID = @UniqueID And xaiSourceTable = 'JOBS'");
			sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow.Field<Guid>("jmpUniqueID");
			database.ExecuteCommand(sqlCommand);
			sqlCommand = database.NewSqlCommand("INSERT INTO FormInputValues (xaiFormID,xaiControlName,xaiValue,xaiSourceUniqueID,xaiSourceTable,xaiLastRunDate,xaiParentFormID,xaiTopLevelFormID) SELECT xaiFormID,xaiControlName,xaiValue,@JobUniqueID,'JOBS',xaiLastRunDate,xaiParentFormID,xaiTopLevelFormID FROM FormInputValues Where xaiSourceUniqueID = @QuoteUniqueID And xaiSourceTable = 'QUOTELINES'");
			sqlCommand.Parameters.Add(new SqlParameter("@JobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow.Field<Guid>("jmpUniqueID");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteUniqueID", SqlDbType.UniqueIdentifier)).Value = lineRow.Field<Guid>("qmlUniqueID");
			database.ExecuteCommand(sqlCommand);
			if (itemValues.EditableValues.ContainsKey("TransferQuoteMethod") && Convert.ToBoolean(itemValues.EditableValues["TransferQuoteMethod"]))
			{
				bool bRefreshMaterialCost = database.Props("PN").Field<bool>("xapQMRefreshMaterialCosts");
				clsJobFunctionsClass obj = new clsJobFunctionsClass();
				((_clsJobFunctions)obj).SetReferences(ServiceProvider.GetService(typeof(ScriptApp)), ServiceProvider.GetService(typeof(IForms)));
				((_clsJobFunctions)obj).GetQuoteMethod(jobID, 0, lineRow.Field<string>("qmlQuoteID"), (int)lineRow.Field<short>("qmlQuoteLineID"), 0, true, true, true, true, bRefreshMaterialCost);
			}
			DataRow dataRow2 = (DataRow)bsJobLinks.AddNew(database, null, new object[2]
			{
				soLineRow["omlSalesOrderID"],
				soLineRow["omlSalesOrderLineID"]
			}, null);
			if (Convert.ToInt16(dataRow2["omjLinkType"]) == 0)
			{
				dataRow2["omjLinkType"] = 1;
			}
			dataRow2["omjJobID"] = jobID;
			dataRow2["omjSalesOrderDeliveryID"] = delRow["omdSalesOrderDeliveryID"];
			bsJobLinks.SaveData();
		}
		else
		{
			messages.Add("Job for " + lineRow.Field<string>("omlSalesOrderID").Trim() + "/" + Convert.ToInt32(lineRow["omlSalesOrderLineID"]).ToString().Trim() + " was not added because job id already exists.");
		}
	}

	private bool checkConditions(DataRow sourceLineRow, ProcessSelectedItemValues itemValues, DateTime? requestedShipDate, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (itemValues.EditableValues.ContainsKey("OrderQty") && Convert.ToDecimal(itemValues.EditableValues["OrderQty"]) == 0m)
		{
			stringBuilder.Append(", order quantity is required");
		}
		if (itemValues.EditableValues.ContainsKey("DeliveryDate"))
		{
			if (itemValues.EditableValues["DeliveryDate"] == DBNull.Value && !requestedShipDate.HasValue)
			{
				stringBuilder.Append(", delivery date is required");
			}
		}
		else if (!requestedShipDate.HasValue)
		{
			stringBuilder.Append(", requested ship date is required");
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Remove(0, 2);
			messages.Add("Quote " + sourceLineRow.Field<string>("qmlQuoteID").Trim() + "/" + Convert.ToInt32(sourceLineRow["qmlQuoteLineID"]).ToString().Trim() + " was not added because " + stringBuilder.ToString() + ".");
			itemValues.DiscardSave = true;
			return false;
		}
		return true;
	}

	private byte getQuoteQuantityForLine(DataTable dtQuoteQuantities, string quoteID, int quoteLineID, decimal qty)
	{
		byte result = 0;
		if (qty > 0m)
		{
			DataRow[] array = dtQuoteQuantities.Select("qmqQuoteID = " + quoteID.ToLinq() + " and qmqQuoteLineID = " + quoteLineID.ToLinq() + " and qmqQuoteQuantity <> 0", "qmqQuoteQuantity");
			if (array.Length != 0)
			{
				DataRow[] array2 = array;
				foreach (DataRow row in array2)
				{
					if (row.Field<decimal>("qmqQuoteQuantity") > qty)
					{
						break;
					}
					result = row.Field<byte>("qmqQuoteQuantityID");
				}
			}
		}
		return result;
	}
}
