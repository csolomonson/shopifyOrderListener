using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPAssetRepository : APIBaseRepository, IERPAssetRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetExist(Guid assetId)
	{
		InitializeParameterLists();
		base.filterList.Add("fapUniqueID|C", assetId);
		base.selectList.Add("fapUniqueID");
		return Task.FromResult(GetAsObject("Assets", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetInformationDto>> GetAllAssets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetInformationDto> collection = new List<ERPAssetInformationDto>();
		InitializeParameterLists();
		string[] array = new string[51]
		{
			"fapApInvoiceID", "fapApInvoiceLineID", "fapAssetTypeID", "fapBookDepreciationEndDate", "fapBookDepreciationRate", "fapBookEffectiveLife", "fapBookStartValue", "fapAssetID", "fapCreatedBy", "fapCreatedDate",
			"fapDeemedValue", "fapDepreciationLimit", "fapDepreciationStartDate", "fapDescription", "fapDisposalDate", "fapDisposalValue", "fapUniqueID", "fapEstimatedProductionUnits", "fapFinanceOrganizationID", "fapInServiceDate",
			"fapLowCostAsset", "fapLowValueAssetInPool", "fapItemType", "fapLeaseExpiryDate", "fapLeaseMonths", "fapLocation", "fapLongDescriptionRtf", "fapLongDescriptionText", "fapPaymentAmount", "fapPlantID",
			"fapPurchaseDate", "fapPurchaseOrderID", "fapPurchaseOrderLineID", "fapPurchaseType", "fapPurchaseValue", "fapQuantity", "fapReceiptDate", "fapReceiptID", "fapReceiptLineID", "fapResidualAmount",
			"fapRowVersion", "fapSerialNumber", "fapStartYearInPool", "fapStatus", "fapSupplierOrganizationID", "fapTaxableUsePercentage", "fapTaxDepreciationEndDate", "fapTaxDepreciationRate", "fapTaxEffectiveLife", "fapTaxStartValue",
			"fapWorkCenterID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Assets");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("Assets", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetInformationDto eRPAssetInformationDto = new ERPAssetInformationDto();
				eRPAssetInformationDto.fapApInvoiceID = dataTable.Rows[i].Field<string>("fapApInvoiceID");
				eRPAssetInformationDto.fapApInvoiceLineID = dataTable.Rows[i].Field<short>("fapApInvoiceLineID");
				eRPAssetInformationDto.fapAssetTypeID = dataTable.Rows[i].Field<string>("fapAssetTypeID");
				eRPAssetInformationDto.fapBookDepreciationEndDate = dataTable.Rows[i].Field<DateTime?>("fapBookDepreciationEndDate");
				eRPAssetInformationDto.fapBookDepreciationRate = dataTable.Rows[i].Field<decimal>("fapBookDepreciationRate");
				eRPAssetInformationDto.fapBookEffectiveLife = dataTable.Rows[i].Field<decimal>("fapBookEffectiveLife");
				eRPAssetInformationDto.fapBookStartValue = dataTable.Rows[i].Field<decimal>("fapBookStartValue");
				eRPAssetInformationDto.fapAssetID = dataTable.Rows[i].Field<string>("fapAssetID");
				eRPAssetInformationDto.fapCreatedBy = dataTable.Rows[i].Field<string>("fapCreatedBy");
				eRPAssetInformationDto.fapCreatedDate = dataTable.Rows[i].Field<DateTime?>("fapCreatedDate");
				eRPAssetInformationDto.fapDeemedValue = dataTable.Rows[i].Field<decimal>("fapDeemedValue");
				eRPAssetInformationDto.fapDepreciationLimit = dataTable.Rows[i].Field<decimal>("fapDepreciationLimit");
				eRPAssetInformationDto.fapDepreciationStartDate = dataTable.Rows[i].Field<DateTime?>("fapDepreciationStartDate");
				eRPAssetInformationDto.fapDescription = dataTable.Rows[i].Field<string>("fapDescription");
				eRPAssetInformationDto.fapDisposalDate = dataTable.Rows[i].Field<DateTime?>("fapDisposalDate");
				eRPAssetInformationDto.fapDisposalValue = dataTable.Rows[i].Field<decimal>("fapDisposalValue");
				eRPAssetInformationDto.fapUniqueID = dataTable.Rows[i].Field<Guid>("fapUniqueID");
				eRPAssetInformationDto.fapEstimatedProductionUnits = dataTable.Rows[i].Field<int>("fapEstimatedProductionUnits");
				eRPAssetInformationDto.fapFinanceOrganizationID = dataTable.Rows[i].Field<string>("fapFinanceOrganizationID");
				eRPAssetInformationDto.fapInServiceDate = dataTable.Rows[i].Field<DateTime?>("fapInServiceDate");
				eRPAssetInformationDto.fapLowCostAsset = dataTable.Rows[i].Field<bool>("fapLowCostAsset");
				eRPAssetInformationDto.fapLowValueAssetInPool = dataTable.Rows[i].Field<bool>("fapLowValueAssetInPool");
				eRPAssetInformationDto.fapItemType = dataTable.Rows[i].Field<string>("fapItemType");
				eRPAssetInformationDto.fapLeaseExpiryDate = dataTable.Rows[i].Field<DateTime?>("fapLeaseExpiryDate");
				eRPAssetInformationDto.fapLeaseMonths = dataTable.Rows[i].Field<short>("fapLeaseMonths");
				eRPAssetInformationDto.fapLocation = dataTable.Rows[i].Field<string>("fapLocation");
				eRPAssetInformationDto.fapLongDescriptionRtf = dataTable.Rows[i].Field<string>("fapLongDescriptionRtf");
				eRPAssetInformationDto.fapLongDescriptionText = dataTable.Rows[i].Field<string>("fapLongDescriptionText");
				eRPAssetInformationDto.fapPaymentAmount = dataTable.Rows[i].Field<decimal>("fapPaymentAmount");
				eRPAssetInformationDto.fapPlantID = dataTable.Rows[i].Field<string>("fapPlantID");
				eRPAssetInformationDto.fapPurchaseDate = dataTable.Rows[i].Field<DateTime?>("fapPurchaseDate");
				eRPAssetInformationDto.fapPurchaseOrderID = dataTable.Rows[i].Field<string>("fapPurchaseOrderID");
				eRPAssetInformationDto.fapPurchaseOrderLineID = dataTable.Rows[i].Field<short>("fapPurchaseOrderLineID");
				eRPAssetInformationDto.fapPurchaseType = dataTable.Rows[i].Field<string>("fapPurchaseType");
				eRPAssetInformationDto.fapPurchaseValue = dataTable.Rows[i].Field<decimal>("fapPurchaseValue");
				eRPAssetInformationDto.fapQuantity = dataTable.Rows[i].Field<int>("fapQuantity");
				eRPAssetInformationDto.fapReceiptDate = dataTable.Rows[i].Field<DateTime?>("fapReceiptDate");
				eRPAssetInformationDto.fapReceiptID = dataTable.Rows[i].Field<string>("fapReceiptID");
				eRPAssetInformationDto.fapReceiptLineID = dataTable.Rows[i].Field<short>("fapReceiptLineID");
				eRPAssetInformationDto.fapResidualAmount = dataTable.Rows[i].Field<decimal>("fapResidualAmount");
				eRPAssetInformationDto.fapRowVersion = dataTable.Rows[i].Field<byte[]>("fapRowVersion");
				eRPAssetInformationDto.fapSerialNumber = dataTable.Rows[i].Field<string>("fapSerialNumber");
				eRPAssetInformationDto.fapStartYearInPool = dataTable.Rows[i].Field<short>("fapStartYearInPool");
				eRPAssetInformationDto.fapStatus = dataTable.Rows[i].Field<string>("fapStatus");
				eRPAssetInformationDto.fapSupplierOrganizationID = dataTable.Rows[i].Field<string>("fapSupplierOrganizationID");
				eRPAssetInformationDto.fapTaxableUsePercentage = dataTable.Rows[i].Field<decimal>("fapTaxableUsePercentage");
				eRPAssetInformationDto.fapTaxDepreciationEndDate = dataTable.Rows[i].Field<DateTime?>("fapTaxDepreciationEndDate");
				eRPAssetInformationDto.fapTaxDepreciationRate = dataTable.Rows[i].Field<decimal>("fapTaxDepreciationRate");
				eRPAssetInformationDto.fapTaxEffectiveLife = dataTable.Rows[i].Field<decimal>("fapTaxEffectiveLife");
				eRPAssetInformationDto.fapTaxStartValue = dataTable.Rows[i].Field<decimal>("fapTaxStartValue");
				eRPAssetInformationDto.fapWorkCenterID = dataTable.Rows[i].Field<string>("fapWorkCenterID");
				eRPAssetInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetInformationDto> GetAsset(Guid assetId)
	{
		ERPAssetInformationDto eRPAssetInformationDto = new ERPAssetInformationDto();
		InitializeParameterLists();
		string[] collection = new string[51]
		{
			"fapApInvoiceID", "fapApInvoiceLineID", "fapAssetTypeID", "fapBookDepreciationEndDate", "fapBookDepreciationRate", "fapBookEffectiveLife", "fapBookStartValue", "fapAssetID", "fapCreatedBy", "fapCreatedDate",
			"fapDeemedValue", "fapDepreciationLimit", "fapDepreciationStartDate", "fapDescription", "fapDisposalDate", "fapDisposalValue", "fapUniqueID", "fapEstimatedProductionUnits", "fapFinanceOrganizationID", "fapInServiceDate",
			"fapLowCostAsset", "fapLowValueAssetInPool", "fapItemType", "fapLeaseExpiryDate", "fapLeaseMonths", "fapLocation", "fapLongDescriptionRtf", "fapLongDescriptionText", "fapPaymentAmount", "fapPlantID",
			"fapPurchaseDate", "fapPurchaseOrderID", "fapPurchaseOrderLineID", "fapPurchaseType", "fapPurchaseValue", "fapQuantity", "fapReceiptDate", "fapReceiptID", "fapReceiptLineID", "fapResidualAmount",
			"fapRowVersion", "fapSerialNumber", "fapStartYearInPool", "fapStatus", "fapSupplierOrganizationID", "fapTaxableUsePercentage", "fapTaxDepreciationEndDate", "fapTaxDepreciationRate", "fapTaxEffectiveLife", "fapTaxStartValue",
			"fapWorkCenterID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fapUniqueID|C", assetId);
		AddCustomFieldsToSelectList("Assets");
		using (DataTable dataTable = GetAsDataTable("Assets", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetInformationDto);
			}
			eRPAssetInformationDto.fapApInvoiceID = dataTable.Rows[0].Field<string>("fapApInvoiceID");
			eRPAssetInformationDto.fapApInvoiceLineID = dataTable.Rows[0].Field<short>("fapApInvoiceLineID");
			eRPAssetInformationDto.fapAssetTypeID = dataTable.Rows[0].Field<string>("fapAssetTypeID");
			eRPAssetInformationDto.fapBookDepreciationEndDate = dataTable.Rows[0].Field<DateTime?>("fapBookDepreciationEndDate");
			eRPAssetInformationDto.fapBookDepreciationRate = dataTable.Rows[0].Field<decimal>("fapBookDepreciationRate");
			eRPAssetInformationDto.fapBookEffectiveLife = dataTable.Rows[0].Field<decimal>("fapBookEffectiveLife");
			eRPAssetInformationDto.fapBookStartValue = dataTable.Rows[0].Field<decimal>("fapBookStartValue");
			eRPAssetInformationDto.fapAssetID = dataTable.Rows[0].Field<string>("fapAssetID");
			eRPAssetInformationDto.fapCreatedBy = dataTable.Rows[0].Field<string>("fapCreatedBy");
			eRPAssetInformationDto.fapCreatedDate = dataTable.Rows[0].Field<DateTime?>("fapCreatedDate");
			eRPAssetInformationDto.fapDeemedValue = dataTable.Rows[0].Field<decimal>("fapDeemedValue");
			eRPAssetInformationDto.fapDepreciationLimit = dataTable.Rows[0].Field<decimal>("fapDepreciationLimit");
			eRPAssetInformationDto.fapDepreciationStartDate = dataTable.Rows[0].Field<DateTime?>("fapDepreciationStartDate");
			eRPAssetInformationDto.fapDescription = dataTable.Rows[0].Field<string>("fapDescription");
			eRPAssetInformationDto.fapDisposalDate = dataTable.Rows[0].Field<DateTime?>("fapDisposalDate");
			eRPAssetInformationDto.fapDisposalValue = dataTable.Rows[0].Field<decimal>("fapDisposalValue");
			eRPAssetInformationDto.fapUniqueID = dataTable.Rows[0].Field<Guid>("fapUniqueID");
			eRPAssetInformationDto.fapEstimatedProductionUnits = dataTable.Rows[0].Field<int>("fapEstimatedProductionUnits");
			eRPAssetInformationDto.fapFinanceOrganizationID = dataTable.Rows[0].Field<string>("fapFinanceOrganizationID");
			eRPAssetInformationDto.fapInServiceDate = dataTable.Rows[0].Field<DateTime?>("fapInServiceDate");
			eRPAssetInformationDto.fapLowCostAsset = dataTable.Rows[0].Field<bool>("fapLowCostAsset");
			eRPAssetInformationDto.fapLowValueAssetInPool = dataTable.Rows[0].Field<bool>("fapLowValueAssetInPool");
			eRPAssetInformationDto.fapItemType = dataTable.Rows[0].Field<string>("fapItemType");
			eRPAssetInformationDto.fapLeaseExpiryDate = dataTable.Rows[0].Field<DateTime?>("fapLeaseExpiryDate");
			eRPAssetInformationDto.fapLeaseMonths = dataTable.Rows[0].Field<short>("fapLeaseMonths");
			eRPAssetInformationDto.fapLocation = dataTable.Rows[0].Field<string>("fapLocation");
			eRPAssetInformationDto.fapLongDescriptionRtf = dataTable.Rows[0].Field<string>("fapLongDescriptionRtf");
			eRPAssetInformationDto.fapLongDescriptionText = dataTable.Rows[0].Field<string>("fapLongDescriptionText");
			eRPAssetInformationDto.fapPaymentAmount = dataTable.Rows[0].Field<decimal>("fapPaymentAmount");
			eRPAssetInformationDto.fapPlantID = dataTable.Rows[0].Field<string>("fapPlantID");
			eRPAssetInformationDto.fapPurchaseDate = dataTable.Rows[0].Field<DateTime?>("fapPurchaseDate");
			eRPAssetInformationDto.fapPurchaseOrderID = dataTable.Rows[0].Field<string>("fapPurchaseOrderID");
			eRPAssetInformationDto.fapPurchaseOrderLineID = dataTable.Rows[0].Field<short>("fapPurchaseOrderLineID");
			eRPAssetInformationDto.fapPurchaseType = dataTable.Rows[0].Field<string>("fapPurchaseType");
			eRPAssetInformationDto.fapPurchaseValue = dataTable.Rows[0].Field<decimal>("fapPurchaseValue");
			eRPAssetInformationDto.fapQuantity = dataTable.Rows[0].Field<int>("fapQuantity");
			eRPAssetInformationDto.fapReceiptDate = dataTable.Rows[0].Field<DateTime?>("fapReceiptDate");
			eRPAssetInformationDto.fapReceiptID = dataTable.Rows[0].Field<string>("fapReceiptID");
			eRPAssetInformationDto.fapReceiptLineID = dataTable.Rows[0].Field<short>("fapReceiptLineID");
			eRPAssetInformationDto.fapResidualAmount = dataTable.Rows[0].Field<decimal>("fapResidualAmount");
			eRPAssetInformationDto.fapRowVersion = dataTable.Rows[0].Field<byte[]>("fapRowVersion");
			eRPAssetInformationDto.fapSerialNumber = dataTable.Rows[0].Field<string>("fapSerialNumber");
			eRPAssetInformationDto.fapStartYearInPool = dataTable.Rows[0].Field<short>("fapStartYearInPool");
			eRPAssetInformationDto.fapStatus = dataTable.Rows[0].Field<string>("fapStatus");
			eRPAssetInformationDto.fapSupplierOrganizationID = dataTable.Rows[0].Field<string>("fapSupplierOrganizationID");
			eRPAssetInformationDto.fapTaxableUsePercentage = dataTable.Rows[0].Field<decimal>("fapTaxableUsePercentage");
			eRPAssetInformationDto.fapTaxDepreciationEndDate = dataTable.Rows[0].Field<DateTime?>("fapTaxDepreciationEndDate");
			eRPAssetInformationDto.fapTaxDepreciationRate = dataTable.Rows[0].Field<decimal>("fapTaxDepreciationRate");
			eRPAssetInformationDto.fapTaxEffectiveLife = dataTable.Rows[0].Field<decimal>("fapTaxEffectiveLife");
			eRPAssetInformationDto.fapTaxStartValue = dataTable.Rows[0].Field<decimal>("fapTaxStartValue");
			eRPAssetInformationDto.fapWorkCenterID = dataTable.Rows[0].Field<string>("fapWorkCenterID");
			eRPAssetInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAsset(ERPAssetDto asset)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Assets WHERE fapUniqueID = " + M1Util.ConvertToLinq(asset.fapUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fapAssetID"] = asset.fapAssetID.ToUpper();
				asset.fapUniqueID = ((asset.fapUniqueID == Guid.Empty) ? Guid.NewGuid() : asset.fapUniqueID);
				dataRow["fapUniqueID"] = asset.fapUniqueID;
				dataRow["fapCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fapCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Asset could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (asset.fapRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Asset is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fapRowVersion"], asset.fapRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Asset has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Asset again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fapApInvoiceID"] = asset.fapApInvoiceID;
			dataRow["fapApInvoiceLineID"] = asset.fapApInvoiceLineID;
			dataRow["fapAssetTypeID"] = asset.fapAssetTypeID;
			DataRow dataRow2 = dataRow;
			DateTime? fapBookDepreciationEndDate = asset.fapBookDepreciationEndDate;
			dataRow2["fapBookDepreciationEndDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapBookDepreciationEndDate"]);
			dataRow["fapBookDepreciationRate"] = asset.fapBookDepreciationRate;
			dataRow["fapBookEffectiveLife"] = asset.fapBookEffectiveLife;
			dataRow["fapBookStartValue"] = asset.fapBookStartValue;
			dataRow["fapDeemedValue"] = asset.fapDeemedValue;
			dataRow["fapDepreciationLimit"] = asset.fapDepreciationLimit;
			DataRow dataRow3 = dataRow;
			fapBookDepreciationEndDate = asset.fapDepreciationStartDate;
			dataRow3["fapDepreciationStartDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapDepreciationStartDate"]);
			dataRow["fapDescription"] = asset.fapDescription;
			DataRow dataRow4 = dataRow;
			fapBookDepreciationEndDate = asset.fapDisposalDate;
			dataRow4["fapDisposalDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapDisposalDate"]);
			dataRow["fapDisposalValue"] = asset.fapDisposalValue;
			dataRow["fapEstimatedProductionUnits"] = asset.fapEstimatedProductionUnits;
			dataRow["fapFinanceOrganizationID"] = asset.fapFinanceOrganizationID;
			DataRow dataRow5 = dataRow;
			fapBookDepreciationEndDate = asset.fapInServiceDate;
			dataRow5["fapInServiceDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapInServiceDate"]);
			dataRow["fapLowCostAsset"] = asset.fapLowCostAsset;
			dataRow["fapLowValueAssetInPool"] = asset.fapLowValueAssetInPool;
			dataRow["fapItemType"] = asset.fapItemType;
			DataRow dataRow6 = dataRow;
			fapBookDepreciationEndDate = asset.fapLeaseExpiryDate;
			dataRow6["fapLeaseExpiryDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapLeaseExpiryDate"]);
			dataRow["fapLeaseMonths"] = asset.fapLeaseMonths;
			dataRow["fapLocation"] = asset.fapLocation;
			dataRow["fapLongDescriptionRtf"] = asset.fapLongDescriptionRtf ?? dataRow["fapLongDescriptionRtf"];
			dataRow["fapLongDescriptionText"] = asset.fapLongDescriptionText ?? dataRow["fapLongDescriptionText"];
			dataRow["fapPaymentAmount"] = asset.fapPaymentAmount;
			dataRow["fapPlantID"] = asset.fapPlantID;
			DataRow dataRow7 = dataRow;
			fapBookDepreciationEndDate = asset.fapPurchaseDate;
			dataRow7["fapPurchaseDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapPurchaseDate"]);
			dataRow["fapPurchaseOrderID"] = asset.fapPurchaseOrderID;
			dataRow["fapPurchaseOrderLineID"] = asset.fapPurchaseOrderLineID;
			dataRow["fapPurchaseType"] = asset.fapPurchaseType;
			dataRow["fapPurchaseValue"] = asset.fapPurchaseValue;
			dataRow["fapQuantity"] = asset.fapQuantity;
			DataRow dataRow8 = dataRow;
			fapBookDepreciationEndDate = asset.fapReceiptDate;
			dataRow8["fapReceiptDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapReceiptDate"]);
			dataRow["fapReceiptID"] = asset.fapReceiptID;
			dataRow["fapReceiptLineID"] = asset.fapReceiptLineID;
			dataRow["fapResidualAmount"] = asset.fapResidualAmount;
			dataRow["fapSerialNumber"] = asset.fapSerialNumber;
			dataRow["fapStartYearInPool"] = asset.fapStartYearInPool;
			dataRow["fapStatus"] = asset.fapStatus;
			dataRow["fapSupplierOrganizationID"] = asset.fapSupplierOrganizationID;
			dataRow["fapTaxableUsePercentage"] = asset.fapTaxableUsePercentage;
			DataRow dataRow9 = dataRow;
			fapBookDepreciationEndDate = asset.fapTaxDepreciationEndDate;
			dataRow9["fapTaxDepreciationEndDate"] = (fapBookDepreciationEndDate.HasValue ? ((object)fapBookDepreciationEndDate.GetValueOrDefault()) : dataRow["fapTaxDepreciationEndDate"]);
			dataRow["fapTaxDepreciationRate"] = asset.fapTaxDepreciationRate;
			dataRow["fapTaxEffectiveLife"] = asset.fapTaxEffectiveLife;
			dataRow["fapTaxStartValue"] = asset.fapTaxStartValue;
			dataRow["fapWorkCenterID"] = asset.fapWorkCenterID;
			if (asset.CustomFields != null && asset.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in asset.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Asset [{asset.fapUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Asset [{asset.fapUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
