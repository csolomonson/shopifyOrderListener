using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferPurchaseOrderToSalesOrderProcess : ProcessParameters
{
	public TransferPurchaseOrderToSalesOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "pmpPurchaseOrderID" };
		KeyValueTableName = "PurchaseOrders";
		Description = "Use this screen to create intra company sales orders from purchase orders.";
		GridID = "M1ADDFROMSOPO";
		BindingSourceTable = "SalesOrders";
		CreatedBindingSourceCaption = "Create Sales Order From Purchase Order";
		ContinueMessage = "This will create an intra company sales order from the {0} selected purchase orders. Are you sure you want to continue?";
		HelpLink = "PM_PostIntraCoPOs.htm";
		HeaderSourceFields = new string[1] { "pmpPurchaseOrderID" };
		HeaderDestinationFields = new string[1] { "ompCustomerPO" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		M1DataDictionary m1DataDictionary = BindingSource.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		if (string.IsNullOrWhiteSpace(BindingSource.Database.Props("DatasetProperties").Field<string>("xadIntraCompanyOrganizationID")))
		{
			messages.Add("No rows were processed as there is no intra company " + LanguageChooser.ChooseLanguage(m1DataDictionary, "organizations") + " setup in Dataset Properties.");
			return;
		}
		M1.Core.AppContext context = BindingSource.Context;
		M1User user = BindingSource.User;
		M1Database database = BindingSource.Database;
		M1Database m1Database = null;
		bool flag = false;
		M1BindingSource m1BindingSource = null;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("PurchaseOrders", "SalesOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("PurchaseOrderLines", "SalesOrderLines", new string[3] { "pmlPartID", "pmlPartRevisionID", "pmlPurchaseQuantity" }, new string[3] { "omlPartID", "omlPartRevisionID", "omlOrderQuantity" });
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("PurchaseOrderComponents", "SalesOrderComponents", new string[5] { "pmoPartID", "pmoPartRevisionID", "pmoQuantityPerParent", "pmoAdditionalQuantity", "pmoWeight" }, new string[5] { "omoPartID", "omoPartRevisionID", "omoQuantityPerParent", "omoAdditionalQuantity", "omoWeight" });
		DataTable dataTable = database.GetDataTable("select pmlPurchaseOrderID, pmlPurchaseOrderLineID, pmlDueDate, cmoIntraCompanyDatasetID, " + matchingFieldsInfo2.GetSourceFieldList(string.Empty, string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from PurchaseOrderlines Inner Join PurchaseOrders On pmpPurchaseOrderID = pmlPurchaseOrderID Inner Join Organizations on pmpSupplierOrganizationID = cmoOrganizationID where " + text + " and cmoIntraCompanyDatasetID <> '' order by cmoIntraCompanyDatasetID, pmlPurchaseOrderID, pmlPurchaseOrderLineID");
		DataTable dataTable2 = database.GetDataTable("select pmoPurchaseOrderID, pmoPurchaseOrderLineID, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from PurchaseOrderComponents inner join PurchaseOrders on pmpPurchaseOrderID=pmoPurchaseOrderID where " + text + " order by pmoPurchaseOrderID,pmoPurchaseOrderLineID,pmoPurchaseOrderComponentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource m1BindingSource2 = null;
		M1BindingSource m1BindingSource3 = null;
		DataTable dtDeliveries = null;
		M1BindingSource bsComponents = null;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string value = string.Empty;
		DataRow dataRow = null;
		List<string> list = new List<string>();
		foreach (DataRow row in dataTable.Rows)
		{
			if (!row.Field<string>("cmoIntraCompanyDatasetID").Equals(text2, StringComparison.CurrentCultureIgnoreCase))
			{
				if (m1BindingSource != null)
				{
					doSave(m1BindingSource, database, list, arg);
					m1BindingSource = null;
					if (flag)
					{
						user.Databases.LogoutAndRemove(m1Database);
						m1Database = null;
					}
				}
				text2 = row.Field<string>("cmoIntraCompanyDatasetID");
				if (!context.InstalledDatabases.Contains(text2))
				{
					throw new M1Exception($"{text2} database does not exist.");
				}
				LoginReturnInfo loginReturnInfo = user.Databases.LoginUsingPassedCredentials(text2, database.LoginCredentials, readOnlyLogin: false);
				flag = loginReturnInfo.DatabaseCreated;
				m1Database = loginReturnInfo.Database;
				m1Database.KeepOpen = true;
				m1BindingSource = new M1BindingSource(m1Database);
				m1BindingSource.DataSourceTable = "SalesOrders";
				m1BindingSource2 = m1BindingSource.PrimaryTable.GetChildBindingSource("SalesOrderLines");
				m1BindingSource3 = m1BindingSource2.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries");
				dtDeliveries = m1BindingSource3.GetDataTable();
				bsComponents = m1BindingSource3.PrimaryTable.GetChildBindingSource("SalesOrderComponents");
			}
			if (!row.Field<string>("pmpPurchaseOrderID").Equals(text3, StringComparison.CurrentCultureIgnoreCase))
			{
				value = string.Empty;
				text3 = row.Field<string>("pmpPurchaseOrderID");
				list.Add(text3);
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				dataRow = (DataRow)m1BindingSource.AddNew();
				m1BindingSource.SetKeyToNextAvailable(dataRow);
				m1BindingSource.ActivateRow(dataRow, null, doFlash: false);
				value = dataRow.Field<string>("ompSalesOrderID");
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow);
			addOrderLine(m1Database, dataRow, m1BindingSource2, row, matchingFieldsInfo2, bsComponents, dataTable2, matchingFieldsInfo3, m1BindingSource3, dtDeliveries);
		}
		if (m1BindingSource != null)
		{
			doSave(m1BindingSource, database, list, arg);
			m1BindingSource = null;
			if (flag)
			{
				user.Databases.LogoutAndRemove(m1Database);
				m1Database = null;
			}
		}
	}

	private void doSave(M1BindingSource curBs, M1Database originalDatabase, List<string> transferredPOs, StartProcessEventArgs arg)
	{
		if (curBs == null)
		{
			return;
		}
		if (ValidateAndSave(curBs, arg) && transferredPOs.Count != 0)
		{
			M1BindingSource m1BindingSource = new M1BindingSource(originalDatabase);
			m1BindingSource.DataSourceTable = "PurchaseOrders";
			foreach (string transferredPO in transferredPOs)
			{
				m1BindingSource.NavigateTo(originalDatabase, "pmpPurchaseOrderID = " + transferredPO.ToSql());
				m1BindingSource.CurrentAsDataRow.SetField("pmpIntraCompanyPosted", value: true);
				m1BindingSource.SaveData();
				m1BindingSource.ClearCache();
			}
			transferredPOs.Clear();
		}
		curBs.Dispose();
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		string value = parm.BindingSource.Database.Props("DatasetProperties").Field<string>("xadIntraCompanyOrganizationID");
		if (!string.IsNullOrWhiteSpace(value))
		{
			destinationHeaderRow["ompCustomerOrganizationID"] = value;
		}
	}

	private void addOrderLine(M1Database database, DataRow soRow, M1BindingSource bsSOLines, DataRow lineRow, MatchingFieldsInfo lineMatches, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo componentMatch, M1BindingSource bsSODeliveries, DataTable dtDeliveries)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsSOLines, lineMatches, soRow);
		if (dataRow != null)
		{
			string partID = dataRow.Field<string>("omlPartID");
			string partRevisionID = dataRow.Field<string>("omlPartRevisionID");
			decimal quantity = dataRow.Field<decimal>("omlOrderQuantity");
			string orgID = soRow.Field<string>("ompCustomerOrganizationID");
			string locationID = soRow.Field<string>("ompARInvoiceLocationID");
			string currencyID = soRow.Field<string>("ompCurrencyRateID");
			string partGroupID = dataRow.Field<string>("omlPartGroupID");
			PriceCalculation sellingPrice = new Part().GetSellingPrice(database, partID, partRevisionID, partGroupID, orgID, locationID, quantity, currencyID, DateTime.Now);
			decimal fullPrice = sellingPrice.FullPrice;
			decimal discount = sellingPrice.Discount;
			dataRow.SetField("omlFullUnitPriceBase", fullPrice);
			dataRow.SetField("omlDiscountPercent", discount);
		}
		DataRow[] array = dtDeliveries.Select("omdSalesOrderID = " + dataRow.Field<string>("omlSalesOrderID").ToLinq() + " And omdSalesOrderLineID = " + dataRow.Field<short>("omlSalesOrderLineID").ToLinq());
		DataRow dataRow2 = ((array.Length != 0) ? array[0] : (bsSODeliveries.AddNew(database, dataRow, null, null) as DataRow));
		if (dataRow2 != null)
		{
			new Part();
			dataRow2["omdDeliveryDate"] = lineRow["pmlDueDate"];
			dataRow2.SetField("omdDeliveryQuantity", dataRow.Field<decimal>("omlOrderQuantity"));
			string salesOrderID = dataRow2.Field<string>("omdSalesOrderID");
			string partID2 = dataRow2.Field<string>("omdPartID");
			string partRevisionID2 = dataRow2.Field<string>("omdPartRevisionID");
			string returnWarehouseID = "";
			string returnWarehouseBinID = "";
			string returnMessage = "";
			if (new Part().SearchProperWarehouseAndBinForAPart(database, partID2, partRevisionID2, salesOrderID, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage))
			{
				dataRow2["omdPartWarehouseLocationID"] = returnWarehouseID;
				dataRow2["omdPartBinID"] = returnWarehouseBinID;
			}
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow2);
		}
		DataRow[] array2 = dtComponents.Select("pmoPurchaseOrderID = " + lineRow.Field<string>("pmlPurchaseOrderID").Trim().ToLinq() + " and pmoPurchaseOrderLineID = " + Convert.ToInt32(lineRow["pmlPurchaseOrderLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array2)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow2);
		}
	}
}
