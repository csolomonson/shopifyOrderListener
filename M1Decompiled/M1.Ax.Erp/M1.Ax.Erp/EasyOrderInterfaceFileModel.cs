using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using M1.Core;
using M1.WebCore;
using M1.WebCore.DTO.Core;
using M1.WebCore.DTO.EOD;
using M1.WebCore.Repository.Core;
using M1.WebCore.Utility;

namespace M1.Ax.Erp;

public class EasyOrderInterfaceFileModel
{
	private M1Database Database;

	private M1.Core.AppContext Context;

	private SalesOrderRepository salesOrderRepository;

	private PartRepository partRepository;

	private OrganizationRepository organizationRepository;

	private ShipmentRepository shipmentRepository;

	public string EasyOrderFileExportPath = string.Empty;

	public string EasyOrderReceiveLibraryID = string.Empty;

	private string ExportLogFilesPath = string.Empty;

	private string PendingExportFilesPath = string.Empty;

	private string ExportedFilesPath = string.Empty;

	private string ExportZipFilesPath = string.Empty;

	private string ExportZipFilesNamePreFix = string.Empty;

	private readonly StringBuilder offlineOrderHeaderfieldLengthString = new StringBuilder();

	private readonly StringBuilder offlineOrderLinefieldLengthString = new StringBuilder();

	private readonly StringBuilder offlineOrderTextLinefieldLengthString = new StringBuilder();

	private readonly StringBuilder offlineOrderDeletefieldLengthString = new StringBuilder();

	private readonly StringBuilder syncorderHeaderfieldLengthString = new StringBuilder();

	private readonly StringBuilder syncorderLinefieldLengthString = new StringBuilder();

	private readonly StringBuilder syncorderTextLinefieldLengthString = new StringBuilder();

	private readonly StringBuilder syncorderTracAndTraceLinefieldLengthString = new StringBuilder();

	private readonly StringBuilder syncorderDeletefieldLengthString = new StringBuilder();

	private readonly StringBuilder controlFilefieldLengthString = new StringBuilder();

	public string EasyOrderUrl { get; set; }

	public bool OverwriteExistingFiles { get; set; }

	public string WebAPIBaseUrl { get; set; }

	public string CurrentLogFile { get; set; }

	public string EasyOrderReceiveMachine { get; set; }

	public string SecretKey { get; set; }

	public string SyncFilesPath { get; set; }

	public string SyncLogFilesPath { get; set; }

	public string ShippingRequestFilesPath { get; set; }

	public string ShippingRequestLogFilesPath { get; set; }

	public EasyOrderInterfaceFileModel(M1Database database, M1.Core.AppContext context)
	{
		Database = database;
		Context = context;
		Context.LoadThirdPartyInformation();
		salesOrderRepository = new SalesOrderRepository(database);
		partRepository = new PartRepository(database);
		shipmentRepository = new ShipmentRepository(database);
		organizationRepository = new OrganizationRepository(database);
		EasyOrderUrl = (string.IsNullOrEmpty(database.Props("PM").Field<string>("xapEasyOrderURL")) ? "" : database.Props("PM").Field<string>("xapEasyOrderURL").Trim());
		EasyOrderReceiveLibraryID = (string.IsNullOrEmpty(database.Props("PM").Field<string>("xapEasyOrderReceiveLibraryID")) ? "" : database.Props("PM").Field<string>("xapEasyOrderReceiveLibraryID").Trim());
		SecretKey = (string.IsNullOrEmpty(database.Props("PM").Field<string>("xapEasyOrderSharedSecretKey")) ? "" : context.DBServerManager.Decrypt(database.Props("PM").Field<string>("xapEasyOrderSharedSecretKey").Trim()));
		EasyOrderFileExportPath = Path.Combine(context.Server.Location, "Tools\\EasyOrder\\");
		EasyOrderReceiveMachine = (string.IsNullOrEmpty(database.Props("PM").Field<string>("xapEasyOrderReceiveMachine")) ? "" : database.Props("PM").Field<string>("xapEasyOrderReceiveMachine").Trim());
		ExportLogFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ExportData\\LogData");
		if (!string.IsNullOrEmpty(EasyOrderFileExportPath))
		{
			PendingExportFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ExportData\\PendingExport");
			ExportLogFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ExportData\\LogData");
			ExportedFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ExportData\\SentData");
			ExportZipFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ExportData");
			ExportZipFilesNamePreFix = "EasyOrderExportDataLog";
			SyncFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "SyncData\\ProcessedData");
			SyncLogFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "SyncData\\LogData");
			ShippingRequestFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ShippingRequestData\\ProcessedData");
			ShippingRequestLogFilesPath = Path.Combine(EasyOrderFileExportPath, database.ID, "ShippingRequestData\\LogData");
			M1File.CreateDirectory(SyncFilesPath);
			M1File.CreateDirectory(SyncLogFilesPath);
		}
		controlFilefieldLengthString.Append("{0,-2}").Append("{1,-5}").Append("{2,-22}")
			.Append("{3,-11}")
			.Append("{4,-6}")
			.Append("{5,-11}")
			.Append("{6,-11}")
			.Append("{7,-13}");
		offlineOrderHeaderfieldLengthString.Append("{0,-3}").Append("{1,14}").Append("{2,8}")
			.Append("{3,-6}")
			.Append("{4,-15}")
			.Append("{5,-14}")
			.Append("{6,-8}")
			.Append("{7,30}")
			.Append("{8,8}")
			.Append("{9,8}")
			.Append("{10,1}")
			.Append("{11,-3}")
			.Append("{12,-15}")
			.Append("{13,35}")
			.Append("{14,-50}")
			.Append("{15,-50}")
			.Append("{16,-16}")
			.Append("{17,1}")
			.Append("{18,-35}")
			.Append("{19,-35}")
			.Append("{20,-35}")
			.Append("{21,-35}")
			.Append("{22,-12}")
			.Append("{23,-35}")
			.Append("{24,35}")
			.Append("{25,-5}")
			.Append("{26,-35}")
			.Append("{27,1}")
			.Append("{28,-30}")
			.Append("{29,-20}")
			.Append("{30,-3}")
			.Append("{31,5}")
			.Append("{32,13}")
			.Append("{33,-1}")
			.Append("{34,-10}")
			.Append("{35,-1}")
			.Append("{36,8}")
			.Append("{37,-20}")
			.Append("{38,-35}")
			.Append("{39,-3}")
			.Append("{40,-20}")
			.Append("{41,-20}")
			.Append("{42,-10}")
			.Append("{43,-3}")
			.Append("{44,2}")
			.Append("{45,-13}")
			.Append("{46,-3}")
			.Append("{47,-256}");
		offlineOrderLinefieldLengthString.Append("{0,-3}").Append("{1,29}").Append("{2,5}")
			.Append("{3,-14}")
			.Append("{4,-14}")
			.Append("{5,-20}")
			.Append("{6,-10}")
			.Append("{7,40}")
			.Append("{8,9}")
			.Append("{9,8}")
			.Append("{10,12}")
			.Append("{11,5}")
			.Append("{12,12}")
			.Append("{13,14}")
			.Append("{14,1}")
			.Append("{15,-1}")
			.Append("{16,-1}")
			.Append("{17,1}")
			.Append("{18,9}")
			.Append("{19,8}")
			.Append("{20,-1}")
			.Append("{21,-1}")
			.Append("{22,-1}")
			.Append("{23,-60}")
			.Append("{24,5}")
			.Append("{25,-1}")
			.Append("{26,5}")
			.Append("{27,-1}")
			.Append("{28,5}")
			.Append("{29,-1}")
			.Append("{30,5}")
			.Append("{31,-1}")
			.Append("{32,-3}")
			.Append("{33,-5}")
			.Append("{34,-30}")
			.Append("{35,9}")
			.Append("{36,13}")
			.Append("{37,-1}")
			.Append("{38,9}")
			.Append("{39,-651}");
		offlineOrderTextLinefieldLengthString.Append("{0,-3}").Append("{1,29}").Append("{2,5}")
			.Append("{3,5}")
			.Append("{4,-500}")
			.Append("{5,-482}");
		offlineOrderDeletefieldLengthString.Append("{0,-3}").Append("{1,-15}").Append("{2,-1006}");
		syncorderDeletefieldLengthString.Append("{0,-3}").Append("{1,-15}").Append("{2,8}")
			.Append("{3,998}");
		syncorderHeaderfieldLengthString.Append("{0,-3}").Append("{1,-14}").Append("{2,8}")
			.Append("{3,-6}")
			.Append("{4,-15}")
			.Append("{5,-14}")
			.Append("{6,-8}")
			.Append("{7,-30}")
			.Append("{8,8}")
			.Append("{9,8}")
			.Append("{10,-1}")
			.Append("{11,-3}")
			.Append("{12,-15}")
			.Append("{13,-10}")
			.Append("{14,-20}")
			.Append("{15,-5}")
			.Append("{16,-50}")
			.Append("{17,-50}")
			.Append("{18,-16}")
			.Append("{19,1}")
			.Append("{20,-35}")
			.Append("{21,-35}")
			.Append("{22,-35}")
			.Append("{23,-35}")
			.Append("{24,-12}")
			.Append("{25,-35}")
			.Append("{26,-35}")
			.Append("{27,-5}")
			.Append("{28,-35}")
			.Append("{29,1}")
			.Append("{30,-30}")
			.Append("{31,-1}")
			.Append("{32,-3}")
			.Append("{33,37}")
			.Append("{34,-1}")
			.Append("{35,-10}")
			.Append("{36,-1}")
			.Append("{37,-8}")
			.Append("{38,-20}")
			.Append("{39,-35}")
			.Append("{40,-1}")
			.Append("{41,-20}")
			.Append("{42,-20}")
			.Append("{43,-3}")
			.Append("{44,-1}")
			.Append("{45,-2}")
			.Append("{46,-1}")
			.Append("{47,13}")
			.Append("{48,269}");
		syncorderLinefieldLengthString.Append("{0,-3}").Append("{1,-14}").Append("{2,-7}")
			.Append("{3,3}")
			.Append("{4,5}")
			.Append("{5,5}")
			.Append("{6,-14}")
			.Append("{7,-14}")
			.Append("{8,-20}")
			.Append("{9,-10}")
			.Append("{10,40}")
			.Append("{11,9}")
			.Append("{12,8}")
			.Append("{13,12}")
			.Append("{14,5}")
			.Append("{15,27}")
			.Append("{16,-1}")
			.Append("{17,-1}")
			.Append("{18,1}")
			.Append("{19,9}")
			.Append("{20,8}")
			.Append("{21,-1}")
			.Append("{22,-1}")
			.Append("{23,-1}")
			.Append("{24,-60}")
			.Append("{25,9}")
			.Append("{26,-1}")
			.Append("{27,8}")
			.Append("{28,-3}")
			.Append("{29,7}")
			.Append("{30,8}")
			.Append("{31,5}")
			.Append("{32,-1}")
			.Append("{33,5}")
			.Append("{34,-1}")
			.Append("{35,5}")
			.Append("{36,-1}")
			.Append("{37,13}")
			.Append("{38,-1}")
			.Append("{39,9}")
			.Append("{40,5}")
			.Append("{41,-1}")
			.Append("{42,9}")
			.Append("{43,-5}")
			.Append("{44,-30}")
			.Append("{45,9}")
			.Append("{46,13}")
			.Append("{47,596}");
		syncorderTextLinefieldLengthString.Append("{0,-3}").Append("{1,14}").Append("{2,-7}")
			.Append("{3,3}")
			.Append("{4,5}")
			.Append("{5,5}")
			.Append("{6,5}")
			.Append("{7,-500}")
			.Append("{8,482}");
		syncorderTracAndTraceLinefieldLengthString.Append("{0,-3}").Append("{1,-15}").Append("{2,8}")
			.Append("{3,7}")
			.Append("{4,-10}")
			.Append("{5,-20}")
			.Append("{6,8}")
			.Append("{7,953}");
	}

	private bool IsDatasetPropertiesSet()
	{
		if (string.IsNullOrEmpty(EasyOrderUrl) || string.IsNullOrEmpty(EasyOrderReceiveLibraryID) || string.IsNullOrEmpty(EasyOrderFileExportPath) || string.IsNullOrEmpty(EasyOrderReceiveMachine))
		{
			return false;
		}
		return true;
	}

	private decimal GetCalculatedTaxRate(decimal lineItemValue, decimal taxValue)
	{
		return Math.Round(taxValue / lineItemValue * 100m, 2);
	}

	private List<SalesOrderDto> GetEODSalesOrderList(List<EODSalesOrderCustomDto> salesOrders)
	{
		List<SalesOrderDto> list = new List<SalesOrderDto>();
		SalesOrderDto salesOrderDto = new SalesOrderDto();
		foreach (EODSalesOrderCustomDto salesOrder in salesOrders)
		{
			salesOrderDto = salesOrderRepository.GetSalesOrderInfor(salesOrder);
			list.Add(salesOrderDto);
		}
		return list;
	}

	private List<EODSalesOrderCustomDto> GetOrders_ForIntefaceFiles(EODInterfaceFileParamDto requestParameter)
	{
		new List<EODSalesOrderCustomDto>();
		_ = string.Empty;
		return salesOrderRepository.GetSalesOrderList_ForEODIntefaceFileManager(requestParameter.EOInterfaceType, requestParameter.SalesOrders.Select((EODSalesOrderCustomDto x) => x.SalesOrderID).ToList());
	}

	private string SetIntefaceFileDirectoryAndName_ExportOrderInterfaceFiles(Enums.EasyOrderInterfaceFileType eoType, bool overwriteExistingFile, ref StringBuilder errorString, ref StringBuilder infoString)
	{
		string text = string.Empty;
		string empty = string.Empty;
		string pendingExportFilesPath = PendingExportFilesPath;
		string empty2 = string.Empty;
		if (!M1File.CreateDirectory(pendingExportFilesPath, ref errorString, ref infoString))
		{
			return empty;
		}
		switch (eoType)
		{
		case Enums.EasyOrderInterfaceFileType.SyncExistingSalesOrder:
		case Enums.EasyOrderInterfaceFileType.SyncDeletedOrder:
			text = "REOS";
			break;
		case Enums.EasyOrderInterfaceFileType.OfflineSalesOrder:
			text = "RORR";
			break;
		}
		empty2 = text + GetOrderSyncNumberFile(text);
		empty = Path.Combine(PendingExportFilesPath, empty2);
		if (M1File.CheckFileExists(empty))
		{
			if (overwriteExistingFile)
			{
				infoString.Append($"File [{empty}] already exists and has overwritten.\n");
			}
			else
			{
				errorString.Append($"File [{empty}] already exists. File creation failed!.\n");
				empty = string.Empty;
			}
		}
		return empty;
	}

	private string GetOrderSyncNumberFile(string type)
	{
		M1DataDictionary dataDictionary = Database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		string property = $"{type}_{Database.ID}";
		string value = ThirdPartyInfo.GetValue(dataDictionary, ThirdParty.EasyOrder, property, "000001");
		int num = int.Parse(value) + 1;
		ThirdPartyInfo.Set(ThirdParty.EasyOrder, property, ((num > 999999) ? 1 : num).ToString("D6"));
		ThirdPartyInfo.SaveTo(dataDictionary);
		return value;
	}

	private void Process_OfflineOrderSyncFile(string[] orderLines, out string orderNumbers)
	{
		int num = 3;
		int num2 = 15;
		int length = 8;
		StringBuilder stringBuilder = new StringBuilder();
		string empty = string.Empty;
		string empty2 = string.Empty;
		SalesOrderDto salesOrderDto = null;
		stringBuilder.Length = 0;
		foreach (string text in orderLines)
		{
			if (text.Trim().Length > 0)
			{
				empty = text.Trim().Substring(num, num2).Trim();
				empty2 = text.Trim().Substring(num + num2, length).Trim();
				salesOrderDto = new SalesOrderDto
				{
					SalesOrderID = empty,
					EasyOrderStatus = 6,
					EasyOrderID = empty2
				};
				salesOrderRepository.SaveSalesOrderHeader(salesOrderDto);
				stringBuilder.Append(empty2).Append("|");
			}
		}
		orderNumbers = stringBuilder.ToString().Substring(0, stringBuilder.ToString().Trim().Length - 1);
	}

	private bool GetHeaderRow_OfflineOrder(SalesOrderDto orderH, ref StringBuilder errorString, ref StringBuilder infoString, out EODOfflineOrderHeaderRowDto headerRow)
	{
		EODOfflineOrderHeaderRowDto eODOfflineOrderHeaderRowDto = new EODOfflineOrderHeaderRowDto();
		OrganizationLocationDto organizationLocationDto = new OrganizationLocationDto();
		bool result = true;
		string empty = string.Empty;
		headerRow = eODOfflineOrderHeaderRowDto;
		errorString.Append($"Order {orderH.SalesOrderID.Trim()} : ORH - Processing Started.\n");
		eODOfflineOrderHeaderRowDto.RecordType_0 = "ORH";
		orderH.CustomerOrganizationID = orderH.CustomerOrganizationID.Trim();
		if (orderH.CustomerOrganizationID.Length > 20)
		{
			errorString.Append($"Customer code length cannot be greater than 20 in sales order {orderH.SalesOrderID}.\n");
			result = false;
			errorString.Append($"Order {orderH.SalesOrderID.Trim()} : ORH - Processing Failed.\n");
			return result;
		}
		if (orderH.CustomerOrganizationID.Length > 8)
		{
			eODOfflineOrderHeaderRowDto.CustomerCode_2 = orderH.CustomerOrganizationID.Substring(0, 8);
			eODOfflineOrderHeaderRowDto.AlfaDebtors_3 = orderH.CustomerOrganizationID.Substring(9);
		}
		else
		{
			eODOfflineOrderHeaderRowDto.CustomerCode_2 = (Regex.IsMatch(orderH.CustomerOrganizationID.Trim(), "^[a-zA-Z0-9]") ? (orderH.CustomerOrganizationID + empty.PadRight(8 - orderH.CustomerOrganizationID.Length)) : $"{orderH.CustomerOrganizationID:00000000}");
		}
		eODOfflineOrderHeaderRowDto.CustomerCodeAlfa_40 = orderH.CustomerOrganizationID;
		eODOfflineOrderHeaderRowDto.OrderCode_4 = orderH.SalesOrderID.Trim();
		eODOfflineOrderHeaderRowDto.OrderDate_8 = orderH.OrderDate.ToString("yyyyMMdd");
		eODOfflineOrderHeaderRowDto.DelDate_9 = orderH.RequestedShipDate?.ToString("yyyyMMdd");
		eODOfflineOrderHeaderRowDto.CurrencyCode_11 = ((orderH.CurrencyRateID.Trim().Length > 3) ? orderH.CurrencyRateID.Trim().Substring(0, 3) : orderH.CurrencyRateID.Trim());
		if (orderH.CustomerPO.Trim().Length > 15)
		{
			eODOfflineOrderHeaderRowDto.ExternalOrderNo_12 = orderH.CustomerPO.Trim().Substring(0, 15);
			errorString.Append($"CustomerPO length is greater than 15 in sales order {orderH.SalesOrderID}. First 15 charactors was taken.\n");
		}
		else
		{
			eODOfflineOrderHeaderRowDto.ExternalOrderNo_12 = orderH.CustomerPO.Trim();
		}
		if (!string.IsNullOrEmpty(orderH.ShipLocationID))
		{
			organizationLocationDto = GetOrganizationLocationInformation(orderH.CustomerOrganizationID.Trim(), orderH.ShipLocationID.Trim());
			if (organizationLocationDto.Name != null)
			{
				eODOfflineOrderHeaderRowDto.DelAddressName1_18 = ((organizationLocationDto.Name.Trim().Length > 35) ? organizationLocationDto.Name.Trim().Substring(0, 35) : organizationLocationDto.Name.Trim());
				eODOfflineOrderHeaderRowDto.DelAddressStreet1_20 = ((organizationLocationDto.AddressLine1.Trim().Length > 35) ? organizationLocationDto.AddressLine1.Trim().Substring(0, 35) : organizationLocationDto.AddressLine1.Trim());
				eODOfflineOrderHeaderRowDto.DelAddressStreet2_21 = ((organizationLocationDto.AddressLine2.Trim().Length > 35) ? organizationLocationDto.AddressLine2.Trim().Substring(0, 35) : organizationLocationDto.AddressLine2.Trim());
				eODOfflineOrderHeaderRowDto.DelAddressZipcode_22 = ((organizationLocationDto.PostCode.Trim().Length > 12) ? organizationLocationDto.PostCode.Trim().Substring(0, 12) : organizationLocationDto.PostCode.Trim());
				eODOfflineOrderHeaderRowDto.DelAddressCity_23 = ((organizationLocationDto.City.Trim().Length > 35) ? organizationLocationDto.City.Trim().Substring(0, 35) : organizationLocationDto.City.Trim());
				eODOfflineOrderHeaderRowDto.DelAddressCountryName_26 = ((organizationLocationDto.Country.Trim().Length > 35) ? organizationLocationDto.Country.Trim().Substring(0, 35) : organizationLocationDto.Country.Trim());
			}
		}
		if (orderH.PaymentTermID.Trim().Length > 3)
		{
			eODOfflineOrderHeaderRowDto.PaymentType_30 = orderH.PaymentTermID.Trim().Substring(0, 3);
			errorString.Append($"PaymentTermID length is greater than 3 in sales order {orderH.SalesOrderID.Trim()}. First 3 characters was taken.\n");
		}
		else
		{
			eODOfflineOrderHeaderRowDto.PaymentType_30 = orderH.PaymentTermID.Trim();
		}
		eODOfflineOrderHeaderRowDto.AcceptImportAlways_33 = "1";
		eODOfflineOrderHeaderRowDto.Synchronize_35 = "1";
		eODOfflineOrderHeaderRowDto.DeliveryAddressCodeAlfa_41 = orderH.ShipLocationID.Trim();
		eODOfflineOrderHeaderRowDto.CodeStaat_46 = orderH.ShipLocationState;
		errorString.Append($"Order {orderH.SalesOrderID.Trim()} : ORH - Processing End.\n");
		return result;
	}

	private bool GetTextLineRow_OfflineOrder(SalesOrderDto order, ref StringBuilder errorString, ref StringBuilder infoTextString, out EODOfflineOrderTextLineRowDto textLineRow)
	{
		textLineRow = new EODOfflineOrderTextLineRowDto();
		bool result = true;
		textLineRow.RecordType_0 = "ORT";
		textLineRow.DoNotUse_1 = string.Empty;
		textLineRow.OrderLineNumber_2 = string.Empty;
		textLineRow.TextLineNumber_3 = string.Empty;
		order.OrderCommentsText = order.OrderCommentsText.Replace(Environment.NewLine, "\\r\\n");
		if (order.OrderCommentsText.Length <= 500)
		{
			textLineRow.Text_4 = order.OrderCommentsText;
			textLineRow.FreeForUse_5 = string.Empty;
			return result;
		}
		errorString.Append("Long Description cannot be greater than 500 characters");
		return false;
	}

	private bool GetLineRow_OfflineOrder(SalesOrderLineDto orderL, SalesOrderDto order, ref StringBuilder errorString, ref StringBuilder infoString, out EODOfflineOrderLineRowDto lineRow)
	{
		EODOfflineOrderLineRowDto eODOfflineOrderLineRowDto = new EODOfflineOrderLineRowDto();
		bool result = true;
		lineRow = eODOfflineOrderLineRowDto;
		string empty = string.Empty;
		infoString.Append($"Order {order.SalesOrderID.Trim()} : ORL {orderL.SalesOrderLineID}- Processing Started.\n");
		eODOfflineOrderLineRowDto.RecordType_0 = "ORL";
		eODOfflineOrderLineRowDto.OrderLineNumber_2 = $"{orderL.SalesOrderLineID * 100:00000}";
		if (orderL.PartID.Trim().Length > 30)
		{
			errorString.Append($"Supplier product code length cannot be greater than 30 in sales order {order.SalesOrderID}.\n");
			result = false;
			infoString.Append($"Order {order.SalesOrderID.Trim()} : ORL {orderL.SalesOrderLineID} - Processing Failed.Check [ERRORS:].\n");
			return result;
		}
		if (string.IsNullOrEmpty(orderL.PartRevisionID.Trim()))
		{
			if (orderL.PartID.Trim().Length > 14)
			{
				eODOfflineOrderLineRowDto.ProductIDSupplierLong_34 = orderL.PartID.Trim();
			}
			else
			{
				eODOfflineOrderLineRowDto.ProductIDSupplier_3 = orderL.PartID.Trim();
			}
		}
		else
		{
			empty = partRepository.GetPartRevisionInfo(orderL.PartID.Trim(), orderL.PartRevisionID.Trim()).EasyOrderPartID;
			if (string.IsNullOrEmpty(empty))
			{
				errorString.Append($"EasyorderPartID is not found for the Part [{orderL.PartID.Trim()}], Revision [{orderL.PartRevisionID.Trim()}] in sales order [{order.SalesOrderID.Trim()}].\n");
				result = false;
				infoString.Append($"Order {order.SalesOrderID.Trim()} : ORL {orderL.SalesOrderLineID} - Processing Failed.Check [ERRORS:].\n");
				return result;
			}
			if (empty.Length > 14)
			{
				eODOfflineOrderLineRowDto.ProductIDSupplierLong_34 = empty;
			}
			else
			{
				eODOfflineOrderLineRowDto.ProductIDSupplier_3 = empty;
			}
		}
		eODOfflineOrderLineRowDto.OrderUOM_6 = ((orderL.UnitOfMeasure.Trim().Length > 10) ? orderL.UnitOfMeasure.Trim().Substring(0, 10) : orderL.UnitOfMeasure.Trim());
		eODOfflineOrderLineRowDto.Quantity_8 = $"{Math.Round(orderL.OrderQuantity, 2) * 100m:000000000}";
		eODOfflineOrderLineRowDto.DelDate_9 = order.RequestedShipDate.Value.ToString("yyyyMMdd");
		if (orderL.DiscountPercent > decimal.Parse("0"))
		{
			eODOfflineOrderLineRowDto.Discount_11 = $"{orderL.DiscountPercent * 100m:00000}";
			eODOfflineOrderLineRowDto.SignDiscount_16 = "-";
		}
		eODOfflineOrderLineRowDto.GrossPrice_10 = $"{Math.Round(orderL.FullUnitPriceForeign, 2) * 100m:000000000000}";
		eODOfflineOrderLineRowDto.NetPrice_12 = $"{Math.Round(orderL.UnitPriceForeign, 2) * 100m:000000000000}";
		eODOfflineOrderLineRowDto.RegAmount_36 = $"{Math.Round(orderL.ExtendedPriceForeign, 2) * 100m:0000000000000}";
		if (orderL.TaxAmountForeign > 0m)
		{
			eODOfflineOrderLineRowDto.PercentageVAT_24 = $"{GetCalculatedTaxRate(orderL.ExtendedPriceForeign, orderL.TaxAmountForeign) * 100m:00000}";
		}
		if (orderL.SecondTaxAmountForeign > 0m)
		{
			eODOfflineOrderLineRowDto.PercentageVAT2_30 = $"{GetCalculatedTaxRate(orderL.ExtendedPriceForeign, orderL.SecondTaxAmountForeign) * 100m:00000}";
		}
		infoString.Append($"Order {order.SalesOrderID.Trim()} : ORL {orderL.SalesOrderLineID} - Processing End.\n");
		return result;
	}

	private bool GetLineRow_Freight_OfflineOrder(SalesOrderDto order, ref StringBuilder errorString, ref StringBuilder infoString, out EODOfflineOrderLineRowDto lineRow)
	{
		EODOfflineOrderLineRowDto eODOfflineOrderLineRowDto = new EODOfflineOrderLineRowDto();
		decimal num = default(decimal);
		lineRow = eODOfflineOrderLineRowDto;
		infoString.Append($"Order {order.SalesOrderID.Trim()} : ORH Freight - Processing Started.\n");
		num = order.SalesOrderLines.Select((SalesOrderLineDto x) => x.SalesOrderLineID).Max() + 1;
		eODOfflineOrderLineRowDto.RecordType_0 = "ORL";
		eODOfflineOrderLineRowDto.OrderLineNumber_2 = $"{num * 100m:00000}";
		eODOfflineOrderLineRowDto.Quantity_8 = $"{Math.Round(1.0, 2) * 100.0:000000000}";
		eODOfflineOrderLineRowDto.OrderCostsIndication_15 = "2";
		eODOfflineOrderLineRowDto.RegAmount_36 = $"{Math.Round(order.FreightTotalForeign, 2) * 100m:0000000000000}";
		infoString.Append($"Order {order.SalesOrderID.Trim()} : ORH Freight - Processing End.\n");
		return true;
	}

	public bool GetTextFileString_OfflineOrder(List<EODSalesOrderCustomDto> salesOrders, ref StringBuilder errTextString, ref StringBuilder infoTextString, out StringBuilder textFileString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		new List<SalesOrderDto>();
		EODOfflineOrderHeaderRowDto eODOfflineOrderHeaderRowDto = null;
		EODOfflineOrderLineRowDto eODOfflineOrderLineRowDto = null;
		EODOfflineOrderTextLineRowDto eODOfflineOrderTextLineRowDto = null;
		bool result = true;
		textFileString = stringBuilder;
		foreach (SalesOrderDto eODSalesOrder in GetEODSalesOrderList(salesOrders))
		{
			eODOfflineOrderHeaderRowDto = new EODOfflineOrderHeaderRowDto();
			if (GetHeaderRow_OfflineOrder(eODSalesOrder, ref errTextString, ref infoTextString, out eODOfflineOrderHeaderRowDto))
			{
				stringBuilder.AppendLine(string.Format(offlineOrderHeaderfieldLengthString.ToString().Trim(), eODOfflineOrderHeaderRowDto.RecordType_0, eODOfflineOrderHeaderRowDto.SupplierCode_1, eODOfflineOrderHeaderRowDto.CustomerCode_2, eODOfflineOrderHeaderRowDto.AlfaDebtors_3, eODOfflineOrderHeaderRowDto.OrderCode_4, eODOfflineOrderHeaderRowDto.DelAddressCode_5, eODOfflineOrderHeaderRowDto.CostCenterCode_6, eODOfflineOrderHeaderRowDto.CostCenterName_7, eODOfflineOrderHeaderRowDto.OrderDate_8, eODOfflineOrderHeaderRowDto.DelDate_9, eODOfflineOrderHeaderRowDto.DoNotUse_10, eODOfflineOrderHeaderRowDto.CurrencyCode_11, eODOfflineOrderHeaderRowDto.ExternalOrderNo_12, eODOfflineOrderHeaderRowDto.DoNotUse_13, eODOfflineOrderHeaderRowDto.Reference1_14, eODOfflineOrderHeaderRowDto.Reference2_15, eODOfflineOrderHeaderRowDto.CreditcardNo_16, eODOfflineOrderHeaderRowDto.DoNotUse_17, eODOfflineOrderHeaderRowDto.DelAddressName1_18, eODOfflineOrderHeaderRowDto.DelAddressName2_19, eODOfflineOrderHeaderRowDto.DelAddressStreet1_20, eODOfflineOrderHeaderRowDto.DelAddressStreet2_21, eODOfflineOrderHeaderRowDto.DelAddressZipcode_22, eODOfflineOrderHeaderRowDto.DelAddressCity_23, eODOfflineOrderHeaderRowDto.DoNotUse_24, eODOfflineOrderHeaderRowDto.DelAddressCountryCode_25, eODOfflineOrderHeaderRowDto.DelAddressCountryName_26, eODOfflineOrderHeaderRowDto.DoNotUse_27, eODOfflineOrderHeaderRowDto.Reference3_28, eODOfflineOrderHeaderRowDto.AuthorisationCCD_29, eODOfflineOrderHeaderRowDto.PaymentType_30, eODOfflineOrderHeaderRowDto.DoNotUse_31, eODOfflineOrderHeaderRowDto.DateAuth_32, eODOfflineOrderHeaderRowDto.AcceptImportAlways_33, eODOfflineOrderHeaderRowDto.UserImport_34, eODOfflineOrderHeaderRowDto.Synchronize_35, eODOfflineOrderHeaderRowDto.OrderRoes_36, eODOfflineOrderHeaderRowDto.LongCostCenterCode_37, eODOfflineOrderHeaderRowDto.LongCostCenterName_38, eODOfflineOrderHeaderRowDto.OrderProcessCode_39, eODOfflineOrderHeaderRowDto.CustomerCodeAlfa_40, eODOfflineOrderHeaderRowDto.DeliveryAddressCodeAlfa_41, eODOfflineOrderHeaderRowDto.Supplier_42, eODOfflineOrderHeaderRowDto.ExternalApplication_43, eODOfflineOrderHeaderRowDto.ExternalApplication_43, eODOfflineOrderHeaderRowDto.DoNotUse_44, eODOfflineOrderHeaderRowDto.ShippingCosts_45, eODOfflineOrderHeaderRowDto.CodeStaat_46, eODOfflineOrderHeaderRowDto.FreeForUse_47));
				if (!string.IsNullOrEmpty(eODSalesOrder.OrderCommentsText))
				{
					eODOfflineOrderTextLineRowDto = new EODOfflineOrderTextLineRowDto();
					if (!GetTextLineRow_OfflineOrder(eODSalesOrder, ref errTextString, ref infoTextString, out eODOfflineOrderTextLineRowDto))
					{
						return false;
					}
					stringBuilder.AppendLine(string.Format(offlineOrderTextLinefieldLengthString.ToString().Trim(), eODOfflineOrderTextLineRowDto.RecordType_0, eODOfflineOrderTextLineRowDto.DoNotUse_1, eODOfflineOrderTextLineRowDto.OrderLineNumber_2, eODOfflineOrderTextLineRowDto.TextLineNumber_3, eODOfflineOrderTextLineRowDto.Text_4, eODOfflineOrderTextLineRowDto.FreeForUse_5));
				}
				foreach (SalesOrderLineDto salesOrderLine in eODSalesOrder.SalesOrderLines)
				{
					eODOfflineOrderLineRowDto = new EODOfflineOrderLineRowDto();
					if (GetLineRow_OfflineOrder(salesOrderLine, eODSalesOrder, ref errTextString, ref infoTextString, out eODOfflineOrderLineRowDto))
					{
						stringBuilder.AppendLine(string.Format(offlineOrderLinefieldLengthString.ToString().Trim(), eODOfflineOrderLineRowDto.RecordType_0, eODOfflineOrderLineRowDto.DoNotUse_1, eODOfflineOrderLineRowDto.OrderLineNumber_2, eODOfflineOrderLineRowDto.ProductIDSupplier_3, eODOfflineOrderLineRowDto.ProductIDEAN_4, eODOfflineOrderLineRowDto.ProductIDCustomer_5, eODOfflineOrderLineRowDto.OrderUOM_6, eODOfflineOrderLineRowDto.DoNotUse2_7, eODOfflineOrderLineRowDto.Quantity_8, eODOfflineOrderLineRowDto.DelDate_9, eODOfflineOrderLineRowDto.GrossPrice_10, eODOfflineOrderLineRowDto.Discount_11, eODOfflineOrderLineRowDto.NetPrice_12, eODOfflineOrderLineRowDto.MainProductAlt_13, eODOfflineOrderLineRowDto.PriceAlt_14, eODOfflineOrderLineRowDto.OrderCostsIndication_15, eODOfflineOrderLineRowDto.SignDiscount_16, eODOfflineOrderLineRowDto.SignOrder_17, eODOfflineOrderLineRowDto.QuantityBackorder_18, eODOfflineOrderLineRowDto.ExpectedBackorderDeldate_19, eODOfflineOrderLineRowDto.SignQuantity_20, eODOfflineOrderLineRowDto.SignQuantityBO_21, eODOfflineOrderLineRowDto.SignGrossPrice_22, eODOfflineOrderLineRowDto.Description1_23, eODOfflineOrderLineRowDto.PercentageVAT_24, eODOfflineOrderLineRowDto.SignVAT_25, eODOfflineOrderLineRowDto.Discount2_26, eODOfflineOrderLineRowDto.SignDiscount2_27, eODOfflineOrderLineRowDto.Discount3_28, eODOfflineOrderLineRowDto.SignDiscount3_29, eODOfflineOrderLineRowDto.PercentageVAT2_30, eODOfflineOrderLineRowDto.SignVAT2_31, eODOfflineOrderLineRowDto.LineProcessCode_32, eODOfflineOrderLineRowDto.CatalogCode_33, eODOfflineOrderLineRowDto.ProductIDSupplierLong_34, eODOfflineOrderLineRowDto.OrderUOMValue_35, eODOfflineOrderLineRowDto.RegAmount_36, eODOfflineOrderLineRowDto.SignRegAmount_37, eODOfflineOrderLineRowDto.PriceUni_38, eODOfflineOrderLineRowDto.FreeForUse_39));
						continue;
					}
					result = false;
					return result;
				}
				if (eODSalesOrder.FreightTotalForeign > 0m)
				{
					eODOfflineOrderLineRowDto = new EODOfflineOrderLineRowDto();
					if (!GetLineRow_Freight_OfflineOrder(eODSalesOrder, ref errTextString, ref infoTextString, out eODOfflineOrderLineRowDto))
					{
						return false;
					}
					stringBuilder.AppendLine(string.Format(offlineOrderLinefieldLengthString.ToString().Trim(), eODOfflineOrderLineRowDto.RecordType_0, eODOfflineOrderLineRowDto.DoNotUse_1, eODOfflineOrderLineRowDto.OrderLineNumber_2, eODOfflineOrderLineRowDto.ProductIDSupplier_3, eODOfflineOrderLineRowDto.ProductIDEAN_4, eODOfflineOrderLineRowDto.ProductIDCustomer_5, eODOfflineOrderLineRowDto.OrderUOM_6, eODOfflineOrderLineRowDto.DoNotUse2_7, eODOfflineOrderLineRowDto.Quantity_8, eODOfflineOrderLineRowDto.DelDate_9, eODOfflineOrderLineRowDto.GrossPrice_10, eODOfflineOrderLineRowDto.Discount_11, eODOfflineOrderLineRowDto.NetPrice_12, eODOfflineOrderLineRowDto.MainProductAlt_13, eODOfflineOrderLineRowDto.PriceAlt_14, eODOfflineOrderLineRowDto.OrderCostsIndication_15, eODOfflineOrderLineRowDto.SignDiscount_16, eODOfflineOrderLineRowDto.SignOrder_17, eODOfflineOrderLineRowDto.QuantityBackorder_18, eODOfflineOrderLineRowDto.ExpectedBackorderDeldate_19, eODOfflineOrderLineRowDto.SignQuantity_20, eODOfflineOrderLineRowDto.SignQuantityBO_21, eODOfflineOrderLineRowDto.SignGrossPrice_22, eODOfflineOrderLineRowDto.Description1_23, eODOfflineOrderLineRowDto.PercentageVAT_24, eODOfflineOrderLineRowDto.SignVAT_25, eODOfflineOrderLineRowDto.Discount2_26, eODOfflineOrderLineRowDto.SignDiscount2_27, eODOfflineOrderLineRowDto.Discount3_28, eODOfflineOrderLineRowDto.SignDiscount3_29, eODOfflineOrderLineRowDto.PercentageVAT2_30, eODOfflineOrderLineRowDto.SignVAT2_31, eODOfflineOrderLineRowDto.LineProcessCode_32, eODOfflineOrderLineRowDto.CatalogCode_33, eODOfflineOrderLineRowDto.ProductIDSupplierLong_34, eODOfflineOrderLineRowDto.OrderUOMValue_35, eODOfflineOrderLineRowDto.RegAmount_36, eODOfflineOrderLineRowDto.SignRegAmount_37, eODOfflineOrderLineRowDto.PriceUni_38, eODOfflineOrderLineRowDto.FreeForUse_39));
				}
				continue;
			}
			return false;
		}
		return result;
	}

	private OrganizationLocationDto GetOrganizationLocationInformation(string organizationId, string locationId)
	{
		SqlCommand sqlCommand = null;
		DataTable dataTable = null;
		OrganizationLocationDto organizationLocationDto = new OrganizationLocationDto();
		sqlCommand = new SqlCommand("SELECT cmlName, cmlAddressLine1 ,cmlAddressLine2,cmlCity,cmlPostCode,cmlCountry,cmlPhoneNumber FROM OrganizationLocations WHERE (cmlOrganizationID=@OrganizationID AND cmlLocationID=@LocationID)");
		sqlCommand.Parameters.AddWithValue("@OrganizationID", organizationId);
		sqlCommand.Parameters.AddWithValue("@LocationID", locationId);
		dataTable = Database.GetDataTable(sqlCommand);
		foreach (DataRow row in dataTable.Rows)
		{
			organizationLocationDto.Name = row["cmlName"].ToString().Trim();
			organizationLocationDto.AddressLine1 = row["cmlAddressLine1"].ToString().Trim();
			organizationLocationDto.AddressLine2 = row["cmlAddressLine2"].ToString().Trim();
			organizationLocationDto.City = row["cmlCity"].ToString().Trim();
			organizationLocationDto.PostCode = row["cmlPostCode"].ToString().Trim();
			organizationLocationDto.PhoneNumber = row["cmlPhoneNumber"].ToString().Trim();
			organizationLocationDto.Country = row["cmlCountry"].ToString().Trim();
		}
		sqlCommand.Dispose();
		dataTable.Dispose();
		return organizationLocationDto;
	}

	private bool GetHeaderRow_SyncExistingOrder(SalesOrderDto orderH, ref StringBuilder errorString, ref StringBuilder infoString, out EODSyncOrderHeaderRowDto headerRow)
	{
		EODSyncOrderHeaderRowDto eODSyncOrderHeaderRowDto = new EODSyncOrderHeaderRowDto();
		OrganizationLocationDto organizationLocationDto = new OrganizationLocationDto();
		bool result = true;
		string empty = string.Empty;
		headerRow = eODSyncOrderHeaderRowDto;
		eODSyncOrderHeaderRowDto.RecordType_0 = "ORH";
		if (orderH.CustomerOrganizationID.Trim().Length > 20)
		{
			errorString.Append($"Customer code length cannot be greater than 20 in sales order {orderH.SalesOrderID}.\n");
			return false;
		}
		if (orderH.CustomerOrganizationID.Trim().Length > 14)
		{
			infoString.Append($"Customer code length is greater than 14 in sales order {orderH.SalesOrderID}. [CustomerCodeAlfa] field is used.\n");
			eODSyncOrderHeaderRowDto.CustomerCodeAlfa_41 = orderH.CustomerOrganizationID.Trim();
		}
		else if (orderH.CustomerOrganizationID.Trim().Length > 8)
		{
			eODSyncOrderHeaderRowDto.CustomerCode_2 = orderH.CustomerOrganizationID.Trim().Substring(0, 8);
			eODSyncOrderHeaderRowDto.AlfaDebtors_3 = orderH.CustomerOrganizationID.Trim().Substring(9);
		}
		else if (Regex.IsMatch(orderH.CustomerOrganizationID.Trim(), "^[a-zA-Z0-9]"))
		{
			eODSyncOrderHeaderRowDto.CustomerCode_2 = orderH.CustomerOrganizationID.Trim() + empty.PadRight(8 - orderH.CustomerOrganizationID.Trim().Length);
		}
		else
		{
			eODSyncOrderHeaderRowDto.CustomerCode_2 = $"{orderH.CustomerOrganizationID.Trim():00000000}";
		}
		eODSyncOrderHeaderRowDto.OrderCodeBackOffice_4 = orderH.SalesOrderID.Trim();
		eODSyncOrderHeaderRowDto.OrderDate_8 = orderH.OrderDate.ToString("yyyyMMdd");
		eODSyncOrderHeaderRowDto.DelDate_9 = orderH.RequestedShipDate.Value.ToString("yyyyMMdd");
		eODSyncOrderHeaderRowDto.CurrencyCode_11 = ((orderH.CurrencyRateID.Trim().Length > 3) ? orderH.CurrencyRateID.Trim().Substring(0, 3) : orderH.CurrencyRateID.Trim());
		if (orderH.CustomerPO.Trim().Length > 15)
		{
			eODSyncOrderHeaderRowDto.OrderCodeCustomer_12 = orderH.CustomerPO.Trim().Substring(0, 15);
			infoString.Append($"CustomerPO length is greater than 15 in sales order {orderH.SalesOrderID}. First 15 charactors was taken.\n");
		}
		else
		{
			eODSyncOrderHeaderRowDto.OrderCodeCustomer_12 = orderH.CustomerPO.Trim();
		}
		if (!string.IsNullOrEmpty(orderH.ShipLocationID))
		{
			organizationLocationDto = organizationRepository.GetOrganizationLocationInfor(orderH.CustomerOrganizationID.Trim(), orderH.ShipLocationID.Trim());
			if (organizationLocationDto.Name != null)
			{
				eODSyncOrderHeaderRowDto.DelAddressName1_20 = ((organizationLocationDto.Name.Trim().Length > 35) ? organizationLocationDto.Name.Trim().Substring(0, 35) : organizationLocationDto.Name.Trim());
				eODSyncOrderHeaderRowDto.DelAddressStreet1_22 = ((organizationLocationDto.AddressLine1.Trim().Length > 35) ? organizationLocationDto.AddressLine1.Trim().Substring(0, 35) : organizationLocationDto.AddressLine1.Trim());
				eODSyncOrderHeaderRowDto.DelAddressStreet2_23 = ((organizationLocationDto.AddressLine2.Trim().Length > 35) ? organizationLocationDto.AddressLine2.Trim().Substring(0, 35) : organizationLocationDto.AddressLine2.Trim());
				eODSyncOrderHeaderRowDto.DelAddressZipcode_24 = ((organizationLocationDto.PostCode.Trim().Length > 12) ? organizationLocationDto.PostCode.Trim().Substring(0, 12) : organizationLocationDto.PostCode.Trim());
				eODSyncOrderHeaderRowDto.DelAddressCity_25 = ((organizationLocationDto.City.Trim().Length > 35) ? organizationLocationDto.City.Trim().Substring(0, 35) : organizationLocationDto.City.Trim());
				eODSyncOrderHeaderRowDto.DelAddressPhone_26 = ((organizationLocationDto.PhoneNumber.Trim().Length > 35) ? organizationLocationDto.PhoneNumber.Trim().Substring(0, 35) : organizationLocationDto.PhoneNumber.Trim());
				eODSyncOrderHeaderRowDto.DelAddressCountryName_28 = ((organizationLocationDto.Country.Trim().Length > 35) ? organizationLocationDto.Country.Trim().Substring(0, 35) : organizationLocationDto.Country.Trim());
			}
		}
		eODSyncOrderHeaderRowDto.AcceptImportAlways_34 = "1";
		eODSyncOrderHeaderRowDto.OrderNumberEO_37 = orderH.EasyOrderID.Trim();
		if (orderH.FreightAmountForeign > 0m)
		{
			eODSyncOrderHeaderRowDto.ShippingCosts_47 = $"{Math.Round(orderH.FreightAmountForeign, 2) * 100m:0000000000000}".Trim();
		}
		return result;
	}

	private bool GetLineRow_SyncExistingOrder(SalesOrderLineDto orderL, SalesOrderDto order, ref StringBuilder errorString, ref StringBuilder infoString, out EODSyncOrderLineRowDto lineRow)
	{
		EODSyncOrderLineRowDto eODSyncOrderLineRowDto = new EODSyncOrderLineRowDto();
		bool result = true;
		string empty = string.Empty;
		lineRow = eODSyncOrderLineRowDto;
		eODSyncOrderLineRowDto.RecordType_0 = "ORL";
		eODSyncOrderLineRowDto.OrderNumberEOBO_2 = orderL.SalesOrderID.ToString().Trim();
		eODSyncOrderLineRowDto.OrderLineNumber_5 = $"{orderL.SalesOrderLineID * 100:00000}";
		if (orderL.PartID.Trim().Length > 30)
		{
			errorString.Append($"Supplier product code length cannot be greater than 30 in sales order {order.SalesOrderID}.\n");
			result = false;
			infoString.Append($"Order {order.SalesOrderID.Trim()} : ORL {orderL.SalesOrderLineID} - Processing Failed.Check [ERRORS:].\n");
			return result;
		}
		if (string.IsNullOrEmpty(orderL.PartRevisionID.Trim()))
		{
			if (orderL.PartID.Trim().Length > 14)
			{
				eODSyncOrderLineRowDto.ProductIDSupplierLong_44 = orderL.PartID.Trim();
			}
			else
			{
				eODSyncOrderLineRowDto.ProductIDSupplier_6 = orderL.PartID.Trim();
			}
		}
		else
		{
			empty = partRepository.GetPartRevisionInfo(orderL.PartID.Trim(), orderL.PartRevisionID.Trim()).EasyOrderPartID;
			if (string.IsNullOrEmpty(empty))
			{
				errorString.Append($"EasyorderPartID is not found for the Part [{orderL.PartID.Trim()}], Revision [{orderL.PartRevisionID.Trim()}] in sales order [{order.SalesOrderID.Trim()}].\n");
				result = false;
				infoString.Append($"Order {order.SalesOrderID.Trim()} : ORL {orderL.SalesOrderLineID} - Processing Failed.Check [ERRORS:].\n");
				return result;
			}
			if (empty.Length > 14)
			{
				eODSyncOrderLineRowDto.ProductIDSupplierLong_44 = empty;
			}
			else
			{
				eODSyncOrderLineRowDto.ProductIDSupplier_6 = empty;
			}
		}
		eODSyncOrderLineRowDto.QuantityOrdered_11 = $"{Math.Round(orderL.OrderQuantity, 2) * 100m:000000000}";
		eODSyncOrderLineRowDto.RequestedDeliveryDate_12 = order.RequestedShipDate.Value.ToString("yyyyMMdd");
		eODSyncOrderLineRowDto.PricePerPriceUnit_13 = $"{Math.Round(orderL.FullUnitPriceForeign, 2) * 100m:000000000000}";
		if (orderL.DiscountPercent > decimal.Parse("0"))
		{
			eODSyncOrderLineRowDto.Discount_14 = $"{orderL.DiscountPercent * 100m:00000}";
			eODSyncOrderLineRowDto.SignDiscount_17 = "-";
		}
		return result;
	}

	private bool GetTrackTraceRowList_SyncExistingOrder(M1Database database, string orderNo, ref StringBuilder errorString, ref StringBuilder infoString, out List<EODSyncOrderTracTraceLineRowDto> eodSyncOrderTracTraceLineRowDtoList)
	{
		EODSyncOrderTracTraceLineRowDto eODSyncOrderTracTraceLineRowDto = new EODSyncOrderTracTraceLineRowDto();
		List<ShipmentDto> shipmentList = shipmentRepository.GetShipmentList(orderNo);
		List<EODSyncOrderTracTraceLineRowDto> list = new List<EODSyncOrderTracTraceLineRowDto>();
		decimal result = default(decimal);
		bool result2 = true;
		eodSyncOrderTracTraceLineRowDtoList = list;
		if (shipmentList.Count > 0)
		{
			foreach (ShipmentDto item in shipmentList)
			{
				eODSyncOrderTracTraceLineRowDto = new EODSyncOrderTracTraceLineRowDto();
				eODSyncOrderTracTraceLineRowDto.RecordType_0 = "TRT";
				eODSyncOrderTracTraceLineRowDto.ExternalOrderNo_1 = orderNo;
				if (decimal.TryParse(item.ShipmentID.Trim(), out result))
				{
					if (result.ToString().Trim().Length > 7)
					{
						errorString.Append("ShipmentID length cannot be greater than 7.\n");
						result2 = false;
						return result2;
					}
					eODSyncOrderTracTraceLineRowDto.PackingNote_3 = $"{result:0000000}";
					eODSyncOrderTracTraceLineRowDto.SendDate_6 = item.ShipDate.ToString("yyyyMMdd");
					if (item.TrackingNumber.Trim().Length > 20)
					{
						errorString.Append("TrackingNumber length cannot be greater than 20.\n");
						result2 = false;
						return result2;
					}
					eODSyncOrderTracTraceLineRowDto.ConsignmentNumber_5 = item.TrackingNumber.Trim();
					list.Add(eODSyncOrderTracTraceLineRowDto);
					continue;
				}
				errorString.Append("ShipmentID cannot be alphanumeric.\n");
				result2 = false;
				return result2;
			}
		}
		return result2;
	}

	public bool GetTextFileString_SyncExistingOrder(List<EODSalesOrderCustomDto> salesOrders, ref StringBuilder errTextString, ref StringBuilder infoTextString, out StringBuilder textFileString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		new List<SalesOrderDto>();
		EODSyncOrderHeaderRowDto eODSyncOrderHeaderRowDto = null;
		EODSyncOrderLineRowDto eODSyncOrderLineRowDto = null;
		bool result = true;
		textFileString = stringBuilder;
		foreach (SalesOrderDto eODSalesOrder in GetEODSalesOrderList(salesOrders))
		{
			eODSyncOrderHeaderRowDto = new EODSyncOrderHeaderRowDto();
			if (GetHeaderRow_SyncExistingOrder(eODSalesOrder, ref errTextString, ref infoTextString, out eODSyncOrderHeaderRowDto))
			{
				stringBuilder.AppendLine(string.Format(syncorderHeaderfieldLengthString.ToString().Trim(), eODSyncOrderHeaderRowDto.RecordType_0, eODSyncOrderHeaderRowDto.SupplierCode_1, eODSyncOrderHeaderRowDto.CustomerCode_2, eODSyncOrderHeaderRowDto.AlfaDebtors_3, eODSyncOrderHeaderRowDto.OrderCodeBackOffice_4, eODSyncOrderHeaderRowDto.DelAddressCode_5, eODSyncOrderHeaderRowDto.CostCenterCode_6, eODSyncOrderHeaderRowDto.CostCenterName_7, eODSyncOrderHeaderRowDto.OrderDate_8, eODSyncOrderHeaderRowDto.DelDate_9, eODSyncOrderHeaderRowDto.CreditBlock_10, eODSyncOrderHeaderRowDto.CurrencyCode_11, eODSyncOrderHeaderRowDto.OrderCodeCustomer_12, eODSyncOrderHeaderRowDto.Transporter_13, eODSyncOrderHeaderRowDto.ConsignmentNumber_14, eODSyncOrderHeaderRowDto.DoNotUse_15, eODSyncOrderHeaderRowDto.Reference1_16, eODSyncOrderHeaderRowDto.Reference2_17, eODSyncOrderHeaderRowDto.CreditCardNo_18, eODSyncOrderHeaderRowDto.DoNotUse_19, eODSyncOrderHeaderRowDto.DelAddressName1_20, eODSyncOrderHeaderRowDto.DelAddressName2_21, eODSyncOrderHeaderRowDto.DelAddressStreet1_22, eODSyncOrderHeaderRowDto.DelAddressStreet2_23, eODSyncOrderHeaderRowDto.DelAddressZipcode_24, eODSyncOrderHeaderRowDto.DelAddressCity_25, eODSyncOrderHeaderRowDto.DelAddressPhone_26, eODSyncOrderHeaderRowDto.DelAddressCountryCode_27, eODSyncOrderHeaderRowDto.DelAddressCountryName_28, eODSyncOrderHeaderRowDto.DoNotUse_29, eODSyncOrderHeaderRowDto.Reference3_30, eODSyncOrderHeaderRowDto.PickListBlocked_31, eODSyncOrderHeaderRowDto.PaymentType_32, eODSyncOrderHeaderRowDto.DoNotUse_33, eODSyncOrderHeaderRowDto.AcceptImportAlways_34, eODSyncOrderHeaderRowDto.UserImport_35, eODSyncOrderHeaderRowDto.DoNotUse_36, eODSyncOrderHeaderRowDto.OrderNumberEO_37, eODSyncOrderHeaderRowDto.LongCostCenterCode_38, eODSyncOrderHeaderRowDto.LongCostCenterName_39, eODSyncOrderHeaderRowDto.PrintOrderConfirmation_40, eODSyncOrderHeaderRowDto.CustomerCodeAlfa_41, eODSyncOrderHeaderRowDto.DeliveryAddressCodeAlfa_42, eODSyncOrderHeaderRowDto.OrderType_43, eODSyncOrderHeaderRowDto.CalcEoOrdCost_44, eODSyncOrderHeaderRowDto.dlaState_45, eODSyncOrderHeaderRowDto.onlyOrh_46, eODSyncOrderHeaderRowDto.ShippingCosts_47, eODSyncOrderHeaderRowDto.FreeForUse_48));
				foreach (SalesOrderLineDto salesOrderLine in eODSalesOrder.SalesOrderLines)
				{
					eODSyncOrderLineRowDto = new EODSyncOrderLineRowDto();
					if (GetLineRow_SyncExistingOrder(salesOrderLine, eODSalesOrder, ref errTextString, ref infoTextString, out eODSyncOrderLineRowDto))
					{
						stringBuilder.AppendLine(string.Format(syncorderLinefieldLengthString.ToString().Trim(), eODSyncOrderLineRowDto.RecordType_0, eODSyncOrderLineRowDto.SupplierCode_1, eODSyncOrderLineRowDto.OrderNumberEOBO_2, eODSyncOrderLineRowDto.OrderSequenceNo_3, eODSyncOrderLineRowDto.DoNotUse_4, eODSyncOrderLineRowDto.OrderLineNumber_5, eODSyncOrderLineRowDto.ProductIDSupplier_6, eODSyncOrderLineRowDto.ProductIDEAN_7, eODSyncOrderLineRowDto.ProductIDCustomer_8, eODSyncOrderLineRowDto.OrderUOM_9, eODSyncOrderLineRowDto.DoNotUse_10, eODSyncOrderLineRowDto.QuantityOrdered_11, eODSyncOrderLineRowDto.RequestedDeliveryDate_12, eODSyncOrderLineRowDto.PricePerPriceUnit_13, eODSyncOrderLineRowDto.Discount_14, eODSyncOrderLineRowDto.DoNotUse_15, eODSyncOrderLineRowDto.OrderCostIndication_16, eODSyncOrderLineRowDto.SignDiscount_17, eODSyncOrderLineRowDto.Reserved_18, eODSyncOrderLineRowDto.QuantityInBackorder_19, eODSyncOrderLineRowDto.ExpectedBackorderDelDate_20, eODSyncOrderLineRowDto.SignQuantity_21, eODSyncOrderLineRowDto.SignQuantityInBackorder_22, eODSyncOrderLineRowDto.SignGrossPrice_23, eODSyncOrderLineRowDto.ProductDescription_24, eODSyncOrderLineRowDto.QuantityNextDelivery_25, eODSyncOrderLineRowDto.SignQuantityDelivered_26, eODSyncOrderLineRowDto.NextDeliveryDate_27, eODSyncOrderLineRowDto.Status_28, eODSyncOrderLineRowDto.PackingNote_29, eODSyncOrderLineRowDto.PackingNoteDate_30, eODSyncOrderLineRowDto.VATPercentage_31, eODSyncOrderLineRowDto.SignVAT_32, eODSyncOrderLineRowDto.Discount2_33, eODSyncOrderLineRowDto.SignDiscount2_34, eODSyncOrderLineRowDto.Discount3_35, eODSyncOrderLineRowDto.SignDiscount3_36, eODSyncOrderLineRowDto.LineAmount_37, eODSyncOrderLineRowDto.SignLineAmount_38, eODSyncOrderLineRowDto.PriceUnit_39, eODSyncOrderLineRowDto.VATPercentage2_40, eODSyncOrderLineRowDto.SignVAT2_41, eODSyncOrderLineRowDto.UOMFactor_42, eODSyncOrderLineRowDto.CatalogCode_43, eODSyncOrderLineRowDto.ProductIDSupplierLong_44, eODSyncOrderLineRowDto.OrderUOMVaue_45, eODSyncOrderLineRowDto.VVPOrdLin_46, eODSyncOrderLineRowDto.FreeForUse_47));
						continue;
					}
					result = false;
					return result;
				}
				new List<EODSyncOrderTracTraceLineRowDto>();
				continue;
			}
			return false;
		}
		return result;
	}

	public bool GetTextFileString_ControlFile(out string textFileString)
	{
		EODControlFileRowDto eODControlFileRowDto = new EODControlFileRowDto();
		StringBuilder stringBuilder = new StringBuilder();
		eODControlFileRowDto.Start_0 = "!".PadRight(2, ' ');
		eODControlFileRowDto.ReceiverApp_1 = "EOWEB";
		eODControlFileRowDto.ReceiverLibrary_2 = EasyOrderReceiveLibraryID;
		eODControlFileRowDto.ReceiverMachine_3 = EasyOrderReceiveMachine;
		eODControlFileRowDto.Softwareversion_4 = Context.Version.Replace(".", "").PadRight(6, ' ');
		eODControlFileRowDto.CreationDate_5 = DateTime.Today.ToString("dd/MM/yyyy").PadRight(11, ' ');
		eODControlFileRowDto.Comment_6 = "M1 Export".PadRight(11, ' ');
		eODControlFileRowDto.EOF_7 = "! End of file";
		stringBuilder.Length = 0;
		stringBuilder.Append(string.Format(controlFilefieldLengthString.ToString().Trim(), eODControlFileRowDto.Start_0, eODControlFileRowDto.ReceiverApp_1, eODControlFileRowDto.ReceiverLibrary_2, eODControlFileRowDto.ReceiverMachine_3, eODControlFileRowDto.Softwareversion_4, eODControlFileRowDto.CreationDate_5, eODControlFileRowDto.Comment_6, eODControlFileRowDto.EOF_7));
		textFileString = stringBuilder.ToString().Trim();
		return true;
	}

	private bool CreateControlFile_ExportOrderInterfaceFiles(ref StringBuilder errorString, ref StringBuilder infoString)
	{
		string textFileString = string.Empty;
		string text = Path.Combine(PendingExportFilesPath, "_CONTROL.Txt").Trim();
		GetTextFileString_ControlFile(out textFileString);
		if (!string.IsNullOrEmpty(text))
		{
			if (!M1File.CreateFile(text, textFileString, ref errorString, ref infoString))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private bool CreateCSVFile_ExportOrderInterfaceFiles(EODInterfaceFileParamDto postRequestParameter, ref StringBuilder errTextString, ref StringBuilder infoTextString)
	{
		StringBuilder textFileString = new StringBuilder();
		string empty = string.Empty;
		switch ((int)postRequestParameter.EOInterfaceType)
		{
		case 2:
			if (GetTextFileString_OfflineOrder(postRequestParameter.SalesOrders, ref errTextString, ref infoTextString, out textFileString))
			{
				if (string.IsNullOrEmpty(textFileString.ToString()))
				{
					return true;
				}
				empty = SetIntefaceFileDirectoryAndName_ExportOrderInterfaceFiles(postRequestParameter.EOInterfaceType, postRequestParameter.OverwriteExistingFile, ref errTextString, ref infoTextString);
				if (!string.IsNullOrEmpty(empty))
				{
					if (!M1File.CreateFile(empty, textFileString.ToString(), ref errTextString, ref infoTextString))
					{
						return false;
					}
					string text2 = string.Join("|", postRequestParameter.SalesOrders.Select((EODSalesOrderCustomDto x) => x.SalesOrderID));
					infoTextString.Append("Sales Orders included in file :[" + text2.Trim() + "].\n");
					break;
				}
				return false;
			}
			return false;
		case 1:
			if (GetTextFileString_SyncExistingOrder(postRequestParameter.SalesOrders, ref errTextString, ref infoTextString, out textFileString))
			{
				if (string.IsNullOrEmpty(textFileString.ToString()))
				{
					return true;
				}
				empty = SetIntefaceFileDirectoryAndName_ExportOrderInterfaceFiles(postRequestParameter.EOInterfaceType, postRequestParameter.OverwriteExistingFile, ref errTextString, ref infoTextString);
				if (!string.IsNullOrEmpty(empty))
				{
					if (!M1File.CreateFile(empty, textFileString.ToString(), ref errTextString, ref infoTextString))
					{
						return false;
					}
					string text = string.Join("|", postRequestParameter.SalesOrders.Select((EODSalesOrderCustomDto x) => x.SalesOrderID));
					infoTextString.Append("Sales Orders included in file :[" + text.Trim() + "].\n");
					break;
				}
				return false;
			}
			return false;
		}
		return true;
	}

	private bool CreateZipFile_ExportOrderInterfaceFiles(ref StringBuilder errTextString, ref StringBuilder infoTextString, out byte[] zippedData)
	{
		string pendingExportFilesPath = PendingExportFilesPath;
		string exportZipFilesPath = ExportZipFilesPath;
		string path = "Upload.zip";
		byte[] array = new byte[0];
		zippedData = array;
		string text = Path.Combine(exportZipFilesPath, path);
		if (M1File.CheckFileExists(text))
		{
			M1File.DeleteFile(text);
		}
		if (!M1Zip.CreateZipFile(pendingExportFilesPath, text, ref errTextString, ref infoTextString))
		{
			return false;
		}
		array = M1Zip.ConvertZipToByteArray(text.Trim());
		zippedData = array;
		return true;
	}

	private bool UpdateSalesOrderExportStatus(EODInterfaceFileParamDto requestParameter, ref StringBuilder errTextString, ref StringBuilder infoTextString)
	{
		bool result = true;
		byte b = 0;
		SalesOrderDto salesOrderDto = null;
		try
		{
			if (requestParameter.EOInterfaceType == Enums.EasyOrderInterfaceFileType.OfflineSalesOrder)
			{
				b = 2;
				foreach (string item in requestParameter.SalesOrders.Select((EODSalesOrderCustomDto x) => x.SalesOrderID))
				{
					salesOrderDto = new SalesOrderDto
					{
						SalesOrderID = item,
						EasyOrderStatus = b
					};
					salesOrderRepository.SaveSalesOrderHeader(salesOrderDto);
				}
			}
		}
		catch (Exception ex)
		{
			result = false;
			errTextString.Append("Failed to update Easyorder status. \n EXCEPTION : " + ex.Source + "   :   " + ex.Message + ".\n" + ex.StackTrace + ".\n");
		}
		return result;
	}

	private bool CopyEasyOrderGeneratedFilesToSentFolder(ref StringBuilder errorString, ref StringBuilder infoString)
	{
		bool result = true;
		DateTime now = DateTime.Now;
		try
		{
			string exportedFilesPath = ExportedFilesPath;
			string pendingExportFilesPath = PendingExportFilesPath;
			string newFilePrefix = "Exported Data Files_" + Database.User?.ToString() + "_" + now.ToString("ddMMyyyy_hhmmss");
			M1File.MoveFilesFromSourceToDestinationWithNewFileName(pendingExportFilesPath, exportedFilesPath, newFilePrefix, ref errorString, ref infoString);
			M1File.MoveFilesFromSourceToDestinationWithNewFileName(ExportZipFilesPath, exportedFilesPath, newFilePrefix, ref errorString, ref infoString);
		}
		catch (Exception ex)
		{
			result = false;
			errorString.Append("Failed to create the _Control file. \n EXCEPTION : " + ex.Source + "   :   " + ex.Message + ".\r" + ex.StackTrace + ".\n");
		}
		return result;
	}

	public List<EODSalesOrderCustomDto> GetSelectedSalesOrdersToExport(List<DataRow> gridselectedRows)
	{
		return gridselectedRows.Select((DataRow row) => new EODSalesOrderCustomDto
		{
			SalesOrderID = row.Field<string>("SalesOrderID"),
			CustomerID = row.Field<string>("OrganizationID"),
			EasyOrderID = row.Field<string>("EasyOrderID")
		}).ToList();
	}

	private bool Process_ExportOrderInterfaceFiles(EODInterfaceFileParamDto requestParameter, ref StringBuilder errorString, ref StringBuilder infoString)
	{
		bool result = true;
		_ = new byte[0];
		_ = string.Empty;
		if (requestParameter != null)
		{
			if (requestParameter.EOExportMgrAction == Enums.EasyOrderInterfaceFileMgrAction.CreateAndExport || requestParameter.EOExportMgrAction == Enums.EasyOrderInterfaceFileMgrAction.CreateFiles)
			{
				if (requestParameter.SalesOrders.Count == 0)
				{
					errorString.Append("At least one sales order should be selected to create the file.\n");
					return false;
				}
				if (!CreateCSVFile_ExportOrderInterfaceFiles(requestParameter, ref errorString, ref infoString))
				{
					result = false;
				}
			}
			return result;
		}
		errorString.Append("Request parameters are invalid.\n");
		return false;
	}

	private DataTable ConvertToDatatable(List<EODSalesOrderCustomDto> list)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("SalesOrderID");
		dataTable.Columns.Add("OrganizationID");
		dataTable.Columns.Add("EasyOrderID");
		foreach (EODSalesOrderCustomDto item in list)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow["SalesOrderID"] = item.SalesOrderID;
			dataRow["OrganizationID"] = item.CustomerID;
			dataRow["EasyOrderID"] = item.EasyOrderID;
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}

	public EODInterfaceFileParamDto Process_GetSalesordersForExportManager(IList<string> ordersToVerify, ref EODInterfaceFileParamDto requestParameter)
	{
		List<EODSalesOrderCustomDto> list = new List<EODSalesOrderCustomDto>();
		if (!string.IsNullOrEmpty(string.Join("", ordersToVerify).Trim()))
		{
			foreach (string item in ordersToVerify)
			{
				list.Add(new EODSalesOrderCustomDto(item, string.Empty, string.Empty));
			}
			requestParameter.SalesOrders = list;
		}
		list = GetOrders_ForIntefaceFiles(requestParameter);
		requestParameter.SalesOrders = list;
		return requestParameter;
	}

	public void ViewLog()
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(CurrentLogFile))
		{
			empty = Path.Combine(ExportLogFilesPath, CurrentLogFile);
			if (File.Exists(empty))
			{
				Process.Start("notepad.exe", empty);
			}
			CurrentLogFile = string.Empty;
		}
		else if (!string.IsNullOrEmpty(ExportLogFilesPath))
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Log File";
			openFileDialog.DefaultExt = ".txt";
			openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
			openFileDialog.Multiselect = false;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.InitialDirectory = ExportLogFilesPath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				text = openFileDialog.FileName;
				openFileDialog.Dispose();
			}
			if (!string.IsNullOrEmpty(text))
			{
				Process.Start("notepad.exe", text);
			}
		}
	}

	public void CreateLogFile(StringBuilder errTextString, StringBuilder infoTextString, string logFilePath, string logFileName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Length = 0;
		if (infoTextString.Length > 0)
		{
			stringBuilder.Append("INFO:");
			stringBuilder.AppendLine();
			stringBuilder.Append(string.Join("\n", infoTextString));
		}
		if (errTextString.Length > 0)
		{
			stringBuilder.Append("ERRORS:\n");
			stringBuilder.Append(string.Join("\n", errTextString));
		}
		string filePath = Path.Combine(logFilePath, logFileName);
		M1File.CreateDirectory(logFilePath);
		M1File.CreateFile(filePath, stringBuilder.ToString(), overwriteExistingFiles: true);
	}

	public bool ValidateRequest_ProcessExportFiles(out HttpStatusCode httpErrorCode, ref StringBuilder errorString, ref StringBuilder infoString)
	{
		bool flag = true;
		HttpStatusCode httpStatusCode = (httpErrorCode = HttpStatusCode.OK);
		infoString.Append("Request validation started.\n");
		if (!IsDatasetPropertiesSet())
		{
			errorString.Append("EasyOrder Dataset Properties in M1 were not set.\n");
			httpStatusCode = HttpStatusCode.InternalServerError;
			httpErrorCode = httpStatusCode;
			infoString.Append("Request validation failed.\n");
			return false;
		}
		httpErrorCode = httpStatusCode;
		if (flag)
		{
			infoString.Append("Request validation completed.\n");
		}
		else
		{
			infoString.Append("Request validation failed.\n");
		}
		return flag;
	}

	public bool ProccessInfoCreateFile(Enums.EasyOrderInterfaceFileMgrAction exportMgrAction, Enums.EasyOrderInterfaceFileType InterfaceType, List<EODSalesOrderCustomDto> selectedOrders, bool overWriteFiles, ref StringBuilder errorString, ref StringBuilder infoString)
	{
		_ = string.Empty;
		bool result = true;
		CurrentLogFile = "EasyOrderExportDataLog_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".txt";
		try
		{
			EODInterfaceFileParamDto requestParameter = new EODInterfaceFileParamDto
			{
				EOInterfaceType = InterfaceType,
				EOExportMgrAction = exportMgrAction,
				OverwriteExistingFile = overWriteFiles,
				SalesOrders = selectedOrders
			};
			if (!Process_ExportOrderInterfaceFiles(requestParameter, ref errorString, ref infoString))
			{
				result = false;
				errorString.Append($"Data processing Fail.See Log file [{CurrentLogFile}] for error details.");
			}
			else
			{
				infoString.Append($"Data processing successfully completed.See Log file [{CurrentLogFile}] for more details.");
			}
		}
		catch (Exception ex)
		{
			result = false;
			errorString.AppendFormat("\nException: {0}, StackTrace: {1}", ex.Message, ex.StackTrace);
			errorString.Append($"Error occured:[{ex.Message}].View Log file [{CurrentLogFile}] for error details.");
		}
		finally
		{
			CreateLogFile(errorString, infoString, ExportLogFilesPath, CurrentLogFile);
		}
		return result;
	}

	public bool GetPendingExportOrders(Enums.EasyOrderInterfaceFileMgrAction exportMgrAction, Enums.EasyOrderInterfaceFileType interfaceType, string salesOrderList, out List<string> invalidOrders, out DataTable dataTable)
	{
		List<EODSalesOrderCustomDto> list = new List<EODSalesOrderCustomDto>();
		List<EODSalesOrderCustomDto> list2 = new List<EODSalesOrderCustomDto>();
		IEnumerable<string> source = new List<string>();
		invalidOrders = source.ToList();
		if (interfaceType == Enums.EasyOrderInterfaceFileType.SyncExistingSalesOrder && !salesOrderList.Trim().Equals(string.Empty))
		{
			if (salesOrderList.Trim().IndexOf(',') > 0)
			{
				string[] array = salesOrderList.Trim().Split(',');
				foreach (string salesOrderID in array)
				{
					list.Add(new EODSalesOrderCustomDto(salesOrderID, "", ""));
				}
			}
			else
			{
				list.Add(new EODSalesOrderCustomDto(salesOrderList.Trim(), "", ""));
			}
		}
		EODInterfaceFileParamDto requestParameter = new EODInterfaceFileParamDto
		{
			EOExportMgrAction = exportMgrAction,
			EOInterfaceType = interfaceType,
			OverwriteExistingFile = true,
			SalesOrders = list
		};
		list2 = GetOrders_ForIntefaceFiles(requestParameter);
		dataTable = ConvertToDatatable(list2);
		if (list.Count > 0)
		{
			source = list.Select((EODSalesOrderCustomDto x) => x.SalesOrderID).ToArray().Except(list2.Select((EODSalesOrderCustomDto x) => x.SalesOrderID).ToArray());
			invalidOrders = source.ToList();
		}
		return true;
	}

	public bool ProcessExportManagerActions(string salesOrderID, string easyOrderID, string customerID, int easyOrderStatus, ref M1BindingSource salesOrderBS, ref StringBuilder errorString, ref StringBuilder infoString)
	{
		Enums.EasyOrderInterfaceFileType interfaceType = Enums.EasyOrderInterfaceFileType.None;
		EODSalesOrderCustomDto item = new EODSalesOrderCustomDto
		{
			SalesOrderID = salesOrderID,
			EasyOrderID = easyOrderID,
			CustomerID = customerID,
			BindingSource = salesOrderBS
		};
		List<EODSalesOrderCustomDto> list = new List<EODSalesOrderCustomDto>();
		list.Add(item);
		if (easyOrderStatus == 1)
		{
			interfaceType = Enums.EasyOrderInterfaceFileType.OfflineSalesOrder;
		}
		if (easyOrderStatus >= 2)
		{
			interfaceType = Enums.EasyOrderInterfaceFileType.SyncExistingSalesOrder;
		}
		return ProccessInfoCreateFile(Enums.EasyOrderInterfaceFileMgrAction.CreateFiles, interfaceType, list, overWriteFiles: true, ref errorString, ref infoString);
	}

	public void Dispose()
	{
	}
}
