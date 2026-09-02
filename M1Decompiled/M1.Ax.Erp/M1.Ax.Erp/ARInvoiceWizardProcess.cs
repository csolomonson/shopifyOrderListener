using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;

namespace M1.Ax.Erp;

public class ARInvoiceWizardProcess : ProcessParameters
{
	public ARInvoiceWizardProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "smpShipmentID" };
		KeyValueTableName = "Shipments";
		ExtraFieldNames = new string[13]
		{
			"smpShipmentID", "ShipmentType", "rrpRMAReceiptID", "kbpCallID", "cmlARInvoicePerShipmentLine", "smpCustomerOrganizationID", "smpARInvoiceLocationID", "smpShipOrganizationID", "smpShipLocationID", "smpCurrencyRateID",
			"smpCustomRate", "smpExchangeRate", "smpFreightCharge"
		};
		Description = "Use this screen to create invoices from your shipments.";
		ContinueMessage = "This will create invoices from the {0} selected record(s). Are you sure you want to continue?";
		GridID = "M1ADDFROMARINVOICEMULTIPLE";
		HelpLink = "AR_InvoicingWizard.htm";
		BindingSourceTable = "ARInvoices";
		MultipleDestinationRowsCreated = true;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Ship Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "smpShipDate",
			AdditionalFields = "smpShipDate"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Locations", null, new string[2] { "smpCustomerOrganizationID", "smpARInvoiceLocationID" })
		{
			ValueFields = new string[2] { "smpCustomerOrganizationID", "smpARInvoiceLocationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Ship Locations", null, new string[2] { "smpShipOrganizationID", "smpShipLocationID" })
		{
			ValueFields = new string[2] { "smpShipOrganizationID", "smpShipLocationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Types", null, new string[1] { "ShipmentType" })
		{
			ValueFields = new string[1] { "ShipmentType" }
		});
		AdditionalFilterParameterMultiValue additionalFilterParameterMultiValue = new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "smpPlantID", "smpPlantDepartmentID" });
		additionalFilterParameterMultiValue.AdditionalFields = "smpPlantID,smpPlantDepartmentID";
		additionalFilterParameterMultiValue.ValueFields = new string[2] { "smpPlantID", "smpPlantDepartmentID" };
		AdditionalFilterParameterMultiValue additionalFilterParameterMultiValue2 = additionalFilterParameterMultiValue;
		if (ServiceProvider.GetService(typeof(M1Database)) is M1Database m1Database && !Convert.ToBoolean(m1Database.Props("FN").Field<bool>("xafDisableMultiplePlants")))
		{
			using SqlCommand sqlCommand = new SqlCommand("Select lmePlantID, lmePlantDepartmentID From Employees Where lmeUserID = @UserID");
			sqlCommand.Parameters.AddWithValue("@UserID", m1Database.User.ID);
			DataTable dataTable = m1Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0 && !string.IsNullOrEmpty(dataTable.Rows[0].Field<string>("lmePlantID")))
			{
				additionalFilterParameterMultiValue2.AllowMultiples = false;
				AdditionalFilterParameterMultiValue.FilterParameterValueItem filterParameterValueItem = new AdditionalFilterParameterMultiValue.FilterParameterValueItem();
				filterParameterValueItem.Values.Add(dataTable.Rows[0].Field<string>("lmePlantID"));
				filterParameterValueItem.Values.Add(dataTable.Rows[0].Field<string>("lmePlantDepartmentID"));
				additionalFilterParameterMultiValue2.SelectedItem = filterParameterValueItem.GetIDFromValues();
			}
		}
		AdditionalFilterParameters.Add(additionalFilterParameterMultiValue2);
		DefaultValueFieldNames = new string[4] { "arpInvoiceDate", "arpGLFiscalYearID", "arpGLFiscalYearPeriodID", "FinancialProperties.xafARGroupShipmentsByCustomer" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		Dictionary<string, object> defaultFieldValues = arg.DefaultFieldValues;
		List<string> messages = arg.Messages;
		M1Database database = BindingSource.Database;
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		string key = string.Empty;
		string key2 = string.Empty;
		_ = string.Empty;
		byte b = Convert.ToByte(defaultFieldValues["xafARGroupShipmentsByCustomer"]);
		DateTime dateTime = Convert.ToDateTime(defaultFieldValues["arpInvoiceDate"]);
		Func<ProcessSelectedItemValues, object> keySelector;
		switch (b)
		{
		case 0:
			keySelector = (ProcessSelectedItemValues i) => i.ExtraFieldValues["smpShipmentID"].ToString();
			break;
		case 1:
			key = "smpShipOrganizationID";
			key2 = "smpShipLocationID";
			keySelector = (ProcessSelectedItemValues i) => i.ExtraFieldValues["smpCustomerOrganizationID"].ToString().PadRight(10) + i.ExtraFieldValues["smpShipOrganizationID"].ToString().PadRight(10) + i.ExtraFieldValues["smpShipLocationID"].ToString().PadRight(10) + i.ExtraFieldValues["smpCurrencyRateID"].ToString().PadRight(10) + i.ExtraFieldValues["smpCustomRate"].ToString().PadRight(10) + i.ExtraFieldValues["smpExchangeRate"].ToString().PadRight(10) + i.ExtraFieldValues["ShipmentType"].ToString();
			break;
		default:
			key = "smpCustomerOrganizationID";
			key2 = "smpARInvoiceLocationID";
			keySelector = (ProcessSelectedItemValues i) => i.ExtraFieldValues["smpCustomerOrganizationID"].ToString().PadRight(10) + i.ExtraFieldValues["smpARInvoiceLocationID"].ToString().PadRight(10) + i.ExtraFieldValues["smpCurrencyRateID"].ToString().PadRight(10) + i.ExtraFieldValues["smpCustomRate"].ToString().PadRight(10) + i.ExtraFieldValues["smpExchangeRate"].ToString().PadRight(10) + i.ExtraFieldValues["ShipmentType"].ToString();
			break;
		}
		database.GetService(typeof(M1DataDictionary));
		TransferShipmentToARInvoiceProcess transferShipmentToARInvoiceProcess = new TransferShipmentToARInvoiceProcess(ServiceProvider, multipleDestinationRowsCreated: true)
		{
			BindingSource = BindingSource
		};
		TransferShipmentFreightToARInvoiceProcess transferShipmentFreightToARInvoiceProcess = new TransferShipmentFreightToARInvoiceProcess(ServiceProvider, multipleDestinationRowsCreated: true)
		{
			BindingSource = BindingSource
		};
		TransferRMAReceiptToARInvoiceProcess transferRMAReceiptToARInvoiceProcess = new TransferRMAReceiptToARInvoiceProcess(ServiceProvider, multipleDestinationRowsCreated: true)
		{
			BindingSource = BindingSource
		};
		TransferCallToARInvoiceProcess transferCallToARInvoiceProcess = new TransferCallToARInvoiceProcess(ServiceProvider, multipleDestinationRowsCreated: true)
		{
			BindingSource = BindingSource
		};
		List<ProcessSelectedItemValues> list = new List<ProcessSelectedItemValues>();
		string value = string.Empty;
		string value2 = string.Empty;
		string value3 = string.Empty;
		string value4 = string.Empty;
		_ = string.Empty;
		string text = string.Empty;
		string value5 = string.Empty;
		decimal num = default(decimal);
		new List<string>();
		List<string> list2 = new List<string>();
		foreach (ProcessSelectedItemValues item2 in selectedItems.OrderBy(keySelector))
		{
			if (!item2.ExtraFieldValues["smpCustomerOrganizationID"].ToString().Equals(value, StringComparison.CurrentCultureIgnoreCase) || !item2.ExtraFieldValues["smpCurrencyRateID"].ToString().Equals(value5, StringComparison.CurrentCultureIgnoreCase) || (Convert.ToBoolean(item2.ExtraFieldValues["smpCustomRate"]) && Convert.ToDecimal(item2.ExtraFieldValues["smpExchangeRate"]) != num) || !item2.ExtraFieldValues[key].ToString().Equals(value2, StringComparison.CurrentCultureIgnoreCase) || !item2.ExtraFieldValues[key2].ToString().Equals(value3, StringComparison.CurrentCultureIgnoreCase) || !item2.ExtraFieldValues["ShipmentType"].ToString().Equals(value4, StringComparison.CurrentCultureIgnoreCase) || b == 0 || Convert.ToBoolean(item2.ExtraFieldValues["cmlARInvoicePerShipmentLine"]))
			{
				value = string.Empty;
				value2 = string.Empty;
				value3 = string.Empty;
				value4 = string.Empty;
				text = string.Empty;
				value5 = string.Empty;
				num = default(decimal);
			}
			if (item2.ExtraFieldValues["ShipmentType"].ToString().Trim().Equals("Shipment", StringComparison.CurrentCultureIgnoreCase) || item2.ExtraFieldValues["ShipmentType"].ToString().Trim().Equals("Return", StringComparison.CurrentCultureIgnoreCase))
			{
				if (Convert.ToBoolean(item2.ExtraFieldValues["cmlARInvoicePerShipmentLine"]))
				{
					if (Convert.ToDecimal(item2.ExtraFieldValues["smpFreightCharge"]) != 0m)
					{
						currentAsDataRow = (DataRow)transferShipmentFreightToARInvoiceProcess.BindingSource.AddNew();
						transferShipmentFreightToARInvoiceProcess.BindingSource.SetKeyToNextAvailable(currentAsDataRow);
						transferShipmentFreightToARInvoiceProcess.BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
						currentAsDataRow["arpInvoiceDate"] = dateTime;
						list.Add(new ProcessSelectedItemValues
						{
							KeyValues = new object[1] { item2.ExtraFieldValues["smpShipmentID"].ToString() }
						});
						StartProcessEventArgs e = new StartProcessEventArgs(null, list, messages, arg.CheckValidationForSave);
						transferShipmentFreightToARInvoiceProcess.Run(e);
						if (!e.Cancel)
						{
							BindingSource.SaveData();
							List<object[]> keysCreated = arg.KeysCreated;
							object[] item = new string[1] { BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID") };
							keysCreated.Add(item);
							if (!list2.Contains(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID")))
							{
								list2.Add(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"));
							}
						}
						list.Clear();
					}
					SqlCommand sqlCommand = database.NewSqlCommand("Select smlShipmentID,smlShipmentLineID From ShipmentLines Where smlShipmentID = @ID");
					sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = item2.ExtraFieldValues["smpShipmentID"];
					foreach (DataRow row in database.GetDataTable(sqlCommand).Rows)
					{
						currentAsDataRow = (DataRow)transferShipmentToARInvoiceProcess.BindingSource.AddNew();
						transferShipmentToARInvoiceProcess.BindingSource.SetKeyToNextAvailable(currentAsDataRow);
						transferShipmentToARInvoiceProcess.BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
						currentAsDataRow["arpInvoiceDate"] = dateTime;
						if (item2.ExtraFieldValues["ShipmentType"].ToString().Trim().Equals("Return", StringComparison.CurrentCultureIgnoreCase))
						{
							currentAsDataRow["arpInvoiceType"] = 2;
						}
						else
						{
							currentAsDataRow["arpInvoiceType"] = 1;
						}
						list.Add(new ProcessSelectedItemValues
						{
							KeyValues = new object[2]
							{
								row["smlShipmentID"],
								row["smlShipmentLineID"]
							}
						});
						StartProcessEventArgs e = new StartProcessEventArgs(null, list, messages, arg.CheckValidationForSave);
						transferShipmentToARInvoiceProcess.Run(e);
						if (!e.Cancel)
						{
							currentAsDataRow["arpFreightAmountForeign"] = 0;
							BindingSource.SaveData();
							List<object[]> keysCreated2 = arg.KeysCreated;
							object[] item = new string[1] { BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID") };
							keysCreated2.Add(item);
							if (!list2.Contains(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID")))
							{
								list2.Add(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"));
							}
						}
						list.Clear();
					}
				}
				else
				{
					if (string.IsNullOrWhiteSpace(text))
					{
						currentAsDataRow = (DataRow)transferShipmentToARInvoiceProcess.BindingSource.AddNew();
						transferShipmentToARInvoiceProcess.BindingSource.SetKeyToNextAvailable(currentAsDataRow);
						transferShipmentToARInvoiceProcess.BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
						currentAsDataRow["arpInvoiceDate"] = dateTime;
						if (item2.ExtraFieldValues["ShipmentType"].ToString().Trim().Equals("Return", StringComparison.CurrentCultureIgnoreCase))
						{
							currentAsDataRow["arpInvoiceType"] = 2;
						}
						else
						{
							currentAsDataRow["arpInvoiceType"] = 1;
						}
					}
					SqlCommand sqlCommand = database.NewSqlCommand("Select smlShipmentID,smlShipmentLineID From ShipmentLines Where smlShipmentID = @ID");
					sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = item2.ExtraFieldValues["smpShipmentID"];
					DataTable dataTable = database.GetDataTable(sqlCommand);
					list.Clear();
					foreach (DataRow row2 in dataTable.Rows)
					{
						list.Add(new ProcessSelectedItemValues
						{
							KeyValues = new object[2]
							{
								row2["smlShipmentID"],
								row2["smlShipmentLineID"]
							}
						});
					}
					StartProcessEventArgs e = new StartProcessEventArgs(null, list, messages, arg.CheckValidationForSave);
					transferShipmentToARInvoiceProcess.Run(e);
					if (!e.Cancel)
					{
						BindingSource.SaveData();
						List<object[]> keysCreated3 = arg.KeysCreated;
						object[] item = new string[1] { BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID") };
						keysCreated3.Add(item);
						if (!list2.Contains(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID")))
						{
							list2.Add(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"));
						}
					}
					list.Clear();
				}
			}
			else if (item2.ExtraFieldValues["ShipmentType"].ToString().Trim().Equals("Calls", StringComparison.CurrentCultureIgnoreCase))
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					currentAsDataRow = (DataRow)transferCallToARInvoiceProcess.BindingSource.AddNew();
					transferCallToARInvoiceProcess.BindingSource.SetKeyToNextAvailable(currentAsDataRow);
					transferCallToARInvoiceProcess.BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
					currentAsDataRow["arpInvoiceDate"] = dateTime;
				}
				SqlCommand sqlCommand = database.NewSqlCommand("Select kbpCallID From Calls Where kbpCallID = @ID");
				sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = item2.ExtraFieldValues["kbpCallID"];
				DataTable dataTable2 = database.GetDataTable(sqlCommand);
				list.Clear();
				foreach (DataRow row3 in dataTable2.Rows)
				{
					list.Add(new ProcessSelectedItemValues
					{
						KeyValues = new object[1] { row3["kbpCallID"] }
					});
				}
				StartProcessEventArgs e = new StartProcessEventArgs(null, list, messages, arg.CheckValidationForSave);
				transferCallToARInvoiceProcess.Run(e);
				if (!e.Cancel)
				{
					BindingSource.SaveData();
					List<object[]> keysCreated4 = arg.KeysCreated;
					object[] item = new string[1] { BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID") };
					keysCreated4.Add(item);
					if (!list2.Contains(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID")))
					{
						list2.Add(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"));
					}
				}
				list.Clear();
			}
			else if (item2.ExtraFieldValues["ShipmentType"].ToString().Trim().Equals("RMA Receipt", StringComparison.CurrentCultureIgnoreCase))
			{
				if (Convert.ToBoolean(item2.ExtraFieldValues["cmlARInvoicePerShipmentLine"]))
				{
					SqlCommand sqlCommand = GetRMAReceiptItemsHaveNotBeenPostedToGLYet(database, item2.ExtraFieldValues["rrpRMAReceiptID"].ToString());
					foreach (DataRow row4 in database.GetDataTable(sqlCommand).Rows)
					{
						currentAsDataRow = (DataRow)transferRMAReceiptToARInvoiceProcess.BindingSource.AddNew();
						transferRMAReceiptToARInvoiceProcess.BindingSource.SetKeyToNextAvailable(currentAsDataRow);
						transferRMAReceiptToARInvoiceProcess.BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
						currentAsDataRow["arpInvoiceDate"] = dateTime;
						list.Add(new ProcessSelectedItemValues
						{
							KeyValues = new object[2]
							{
								row4["rrlRMAReceiptID"],
								row4["rrlRMAReceiptLineID"]
							}
						});
						StartProcessEventArgs e = new StartProcessEventArgs(null, list, messages, arg.CheckValidationForSave);
						transferRMAReceiptToARInvoiceProcess.Run(e);
						if (!e.Cancel)
						{
							BindingSource.SaveData();
							List<object[]> keysCreated5 = arg.KeysCreated;
							object[] item = new string[1] { BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID") };
							keysCreated5.Add(item);
							if (!list2.Contains(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID")))
							{
								list2.Add(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"));
							}
						}
						list.Clear();
					}
				}
				else
				{
					if (string.IsNullOrWhiteSpace(text))
					{
						currentAsDataRow = (DataRow)transferRMAReceiptToARInvoiceProcess.BindingSource.AddNew();
						transferRMAReceiptToARInvoiceProcess.BindingSource.SetKeyToNextAvailable(currentAsDataRow);
						transferRMAReceiptToARInvoiceProcess.BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
						currentAsDataRow["arpInvoiceDate"] = dateTime;
					}
					SqlCommand sqlCommand = GetRMAReceiptItemsHaveNotBeenPostedToGLYet(database, item2.ExtraFieldValues["rrpRMAReceiptID"].ToString());
					DataTable dataTable3 = database.GetDataTable(sqlCommand);
					list.Clear();
					foreach (DataRow row5 in dataTable3.Rows)
					{
						list.Add(new ProcessSelectedItemValues
						{
							KeyValues = new object[2]
							{
								row5["rrlRMAReceiptID"],
								row5["rrlRMAReceiptLineID"]
							}
						});
					}
					StartProcessEventArgs e = new StartProcessEventArgs(null, list, messages, arg.CheckValidationForSave);
					transferRMAReceiptToARInvoiceProcess.Run(e);
					if (!e.Cancel)
					{
						BindingSource.SaveData();
						List<object[]> keysCreated6 = arg.KeysCreated;
						object[] item = new string[1] { BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID") };
						keysCreated6.Add(item);
						if (!list2.Contains(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID")))
						{
							list2.Add(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"));
						}
					}
					list.Clear();
				}
			}
			if (BindingSource.CurrentAsDataRow != null && !text.Equals(BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID"), StringComparison.CurrentCultureIgnoreCase))
			{
				text = BindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID");
			}
			if (b == 0)
			{
				value = string.Empty;
				value2 = string.Empty;
				value3 = string.Empty;
				value4 = string.Empty;
				text = string.Empty;
				value5 = string.Empty;
				num = default(decimal);
			}
			else
			{
				value = item2.ExtraFieldValues["smpCustomerOrganizationID"].ToString();
				value2 = item2.ExtraFieldValues[key].ToString();
				value3 = item2.ExtraFieldValues[key2].ToString();
				value4 = item2.ExtraFieldValues["ShipmentType"].ToString();
				value5 = item2.ExtraFieldValues["smpCurrencyRateID"].ToString();
				num = Convert.ToDecimal(item2.ExtraFieldValues["smpExchangeRate"]);
			}
		}
		if (arg.KeysCreated.Count > 0)
		{
			arg.OpenKeysWithObjectID = "ARINVOICE";
			object[] item = list2.ToArray();
			arg.ActionMessagesArgs = new ActionMessagesEventArgs("ARINVOICEWIZARDPROCESS_FINISHED", item, null);
		}
	}

	private static SqlCommand GetRMAReceiptItemsHaveNotBeenPostedToGLYet(M1Database database, string rmaReceiptID)
	{
		string queryString = "SELECT rrlRMAReceiptID,rrlRMAReceiptLineID, rrlPartID \r\n                                                  FROM RMAReceiptLines \r\n                                                  WHERE rrlRMAReceiptID = @ReceiptID \r\n                                                        AND rrlRMAReceiptLineID NOT IN (\r\n                                                                                            SELECT ISNULL(arlRMAReceiptLineID, 0) \r\n                                                                                            FROM ARInvoiceLines \r\n                                                                                            WHERE arlRMAReceiptID = @ReceiptID \r\n                                                                                                  AND arlPostedToGL <> 0\r\n                                                                                        )";
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		SqlParameter value = new SqlParameter("@ReceiptID", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add(value).Value = rmaReceiptID;
		return sqlCommand;
	}
}
