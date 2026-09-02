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

public class ERPRMAClaimLineRepository : APIBaseRepository, IERPRMAClaimLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAClaimLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAClaimLineExist(Guid rMAClaimLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("ralUniqueID|C", rMAClaimLineId);
		base.selectList.Add("ralUniqueID");
		return Task.FromResult(GetAsObject("RMAClaimLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAClaimLineInformationDto>> GetAllRMAClaimLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAClaimLineInformationDto> collection = new List<ERPRMAClaimLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[68]
		{
			"ralActionType", "ralConversionFactor", "ralCreatedBy", "ralCreatedDate", "ralCustomerPo", "ralDiscountPercent", "ralUniqueID", "ralExtendedCost", "ralExtendedCostForeign", "ralExtendedDiscountBase",
			"ralExtendedDiscountForeign", "ralExtendedPrice", "ralExtendedPriceForeign", "ralFullExtendedPriceBase", "ralFullExtendedPriceForeign", "ralFullUnitPriceBase", "ralFullUnitPriceForeign", "ralCustomerToPayForShipping", "ralInvoicedComplete", "ralKitPart",
			"ralReceivedComplete", "ralRequiresInspection", "ralReturnToSupplier", "ralTransferredToSalesOrder", "ralOrgPartID", "ralOrgPartShortDescription", "ralPartBinID", "ralPartGroupID", "ralPartID", "ralPartLongDescriptionRtf",
			"ralPartLongDescriptionText", "ralPartRevisionID", "ralPartShortDescription", "ralPartWarehouseLocationID", "ralProjectAreaID", "ralProjectID", "ralPurchaseLocationID", "ralQuantity", "ralQuantityReceived", "ralReceivedDate",
			"ralRequiredDate", "ralReturnedDate", "ralReturnReasonID", "ralRmaClaimID", "ralRowVersion", "ralSalesOrderDeliveryID", "ralSalesOrderID", "ralSalesOrderLineID", "ralSalesQuantity", "ralSalesUnitOfMeasure",
			"ralRmaClaimLineID", "ralShipmentID", "ralShipmentLineID", "ralShippedDate", "ralShippingMethodID", "ralShippingPaymentTypeID", "ralSupplierAuthorizationNumber", "ralSupplierOrganizationID", "ralSupplierShippingMethodID", "ralSupplierTrackingNumber",
			"ralTrackingNumber", "ralUnitCost", "ralUnitCostForeign", "ralUnitDiscountBase", "ralUnitDiscountForeign", "ralUnitOfMeasure", "ralUnitPrice", "ralUnitPriceForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAClaimLines");
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
		using (DataTable dataTable = GetAsDataTable("RMAClaimLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAClaimLineInformationDto eRPRMAClaimLineInformationDto = new ERPRMAClaimLineInformationDto();
				eRPRMAClaimLineInformationDto.ralActionType = dataTable.Rows[i].Field<string>("ralActionType");
				eRPRMAClaimLineInformationDto.ralConversionFactor = dataTable.Rows[i].Field<decimal>("ralConversionFactor");
				eRPRMAClaimLineInformationDto.ralCreatedBy = dataTable.Rows[i].Field<string>("ralCreatedBy");
				eRPRMAClaimLineInformationDto.ralCreatedDate = dataTable.Rows[i].Field<DateTime?>("ralCreatedDate");
				eRPRMAClaimLineInformationDto.ralCustomerPo = dataTable.Rows[i].Field<string>("ralCustomerPo");
				eRPRMAClaimLineInformationDto.ralDiscountPercent = dataTable.Rows[i].Field<decimal>("ralDiscountPercent");
				eRPRMAClaimLineInformationDto.ralUniqueID = dataTable.Rows[i].Field<Guid>("ralUniqueID");
				eRPRMAClaimLineInformationDto.ralExtendedCost = dataTable.Rows[i].Field<decimal>("ralExtendedCost");
				eRPRMAClaimLineInformationDto.ralExtendedCostForeign = dataTable.Rows[i].Field<decimal>("ralExtendedCostForeign");
				eRPRMAClaimLineInformationDto.ralExtendedDiscountBase = dataTable.Rows[i].Field<decimal>("ralExtendedDiscountBase");
				eRPRMAClaimLineInformationDto.ralExtendedDiscountForeign = dataTable.Rows[i].Field<decimal>("ralExtendedDiscountForeign");
				eRPRMAClaimLineInformationDto.ralExtendedPrice = dataTable.Rows[i].Field<decimal>("ralExtendedPrice");
				eRPRMAClaimLineInformationDto.ralExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("ralExtendedPriceForeign");
				eRPRMAClaimLineInformationDto.ralFullExtendedPriceBase = dataTable.Rows[i].Field<decimal>("ralFullExtendedPriceBase");
				eRPRMAClaimLineInformationDto.ralFullExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("ralFullExtendedPriceForeign");
				eRPRMAClaimLineInformationDto.ralFullUnitPriceBase = dataTable.Rows[i].Field<decimal>("ralFullUnitPriceBase");
				eRPRMAClaimLineInformationDto.ralFullUnitPriceForeign = dataTable.Rows[i].Field<decimal>("ralFullUnitPriceForeign");
				eRPRMAClaimLineInformationDto.ralCustomerToPayForShipping = dataTable.Rows[i].Field<bool>("ralCustomerToPayForShipping");
				eRPRMAClaimLineInformationDto.ralInvoicedComplete = dataTable.Rows[i].Field<bool>("ralInvoicedComplete");
				eRPRMAClaimLineInformationDto.ralKitPart = dataTable.Rows[i].Field<bool>("ralKitPart");
				eRPRMAClaimLineInformationDto.ralReceivedComplete = dataTable.Rows[i].Field<bool>("ralReceivedComplete");
				eRPRMAClaimLineInformationDto.ralRequiresInspection = dataTable.Rows[i].Field<bool>("ralRequiresInspection");
				eRPRMAClaimLineInformationDto.ralReturnToSupplier = dataTable.Rows[i].Field<bool>("ralReturnToSupplier");
				eRPRMAClaimLineInformationDto.ralTransferredToSalesOrder = dataTable.Rows[i].Field<bool>("ralTransferredToSalesOrder");
				eRPRMAClaimLineInformationDto.ralOrgPartID = dataTable.Rows[i].Field<string>("ralOrgPartID");
				eRPRMAClaimLineInformationDto.ralOrgPartShortDescription = dataTable.Rows[i].Field<string>("ralOrgPartShortDescription");
				eRPRMAClaimLineInformationDto.ralPartBinID = dataTable.Rows[i].Field<string>("ralPartBinID");
				eRPRMAClaimLineInformationDto.ralPartGroupID = dataTable.Rows[i].Field<string>("ralPartGroupID");
				eRPRMAClaimLineInformationDto.ralPartID = dataTable.Rows[i].Field<string>("ralPartID");
				eRPRMAClaimLineInformationDto.ralPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("ralPartLongDescriptionRtf");
				eRPRMAClaimLineInformationDto.ralPartLongDescriptionText = dataTable.Rows[i].Field<string>("ralPartLongDescriptionText");
				eRPRMAClaimLineInformationDto.ralPartRevisionID = dataTable.Rows[i].Field<string>("ralPartRevisionID");
				eRPRMAClaimLineInformationDto.ralPartShortDescription = dataTable.Rows[i].Field<string>("ralPartShortDescription");
				eRPRMAClaimLineInformationDto.ralPartWarehouseLocationID = dataTable.Rows[i].Field<string>("ralPartWarehouseLocationID");
				eRPRMAClaimLineInformationDto.ralProjectAreaID = dataTable.Rows[i].Field<string>("ralProjectAreaID");
				eRPRMAClaimLineInformationDto.ralProjectID = dataTable.Rows[i].Field<string>("ralProjectID");
				eRPRMAClaimLineInformationDto.ralPurchaseLocationID = dataTable.Rows[i].Field<string>("ralPurchaseLocationID");
				eRPRMAClaimLineInformationDto.ralQuantity = dataTable.Rows[i].Field<decimal>("ralQuantity");
				eRPRMAClaimLineInformationDto.ralQuantityReceived = dataTable.Rows[i].Field<decimal>("ralQuantityReceived");
				eRPRMAClaimLineInformationDto.ralReceivedDate = dataTable.Rows[i].Field<DateTime?>("ralReceivedDate");
				eRPRMAClaimLineInformationDto.ralRequiredDate = dataTable.Rows[i].Field<DateTime?>("ralRequiredDate");
				eRPRMAClaimLineInformationDto.ralReturnedDate = dataTable.Rows[i].Field<DateTime?>("ralReturnedDate");
				eRPRMAClaimLineInformationDto.ralReturnReasonID = dataTable.Rows[i].Field<string>("ralReturnReasonID");
				eRPRMAClaimLineInformationDto.ralRmaClaimID = dataTable.Rows[i].Field<string>("ralRmaClaimID");
				eRPRMAClaimLineInformationDto.ralRowVersion = dataTable.Rows[i].Field<byte[]>("ralRowVersion");
				eRPRMAClaimLineInformationDto.ralSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("ralSalesOrderDeliveryID");
				eRPRMAClaimLineInformationDto.ralSalesOrderID = dataTable.Rows[i].Field<string>("ralSalesOrderID");
				eRPRMAClaimLineInformationDto.ralSalesOrderLineID = dataTable.Rows[i].Field<short>("ralSalesOrderLineID");
				eRPRMAClaimLineInformationDto.ralSalesQuantity = dataTable.Rows[i].Field<decimal>("ralSalesQuantity");
				eRPRMAClaimLineInformationDto.ralSalesUnitOfMeasure = dataTable.Rows[i].Field<string>("ralSalesUnitOfMeasure");
				eRPRMAClaimLineInformationDto.ralRmaClaimLineID = dataTable.Rows[i].Field<short>("ralRmaClaimLineID");
				eRPRMAClaimLineInformationDto.ralShipmentID = dataTable.Rows[i].Field<string>("ralShipmentID");
				eRPRMAClaimLineInformationDto.ralShipmentLineID = dataTable.Rows[i].Field<short>("ralShipmentLineID");
				eRPRMAClaimLineInformationDto.ralShippedDate = dataTable.Rows[i].Field<DateTime?>("ralShippedDate");
				eRPRMAClaimLineInformationDto.ralShippingMethodID = dataTable.Rows[i].Field<string>("ralShippingMethodID");
				eRPRMAClaimLineInformationDto.ralShippingPaymentTypeID = dataTable.Rows[i].Field<string>("ralShippingPaymentTypeID");
				eRPRMAClaimLineInformationDto.ralSupplierAuthorizationNumber = dataTable.Rows[i].Field<string>("ralSupplierAuthorizationNumber");
				eRPRMAClaimLineInformationDto.ralSupplierOrganizationID = dataTable.Rows[i].Field<string>("ralSupplierOrganizationID");
				eRPRMAClaimLineInformationDto.ralSupplierShippingMethodID = dataTable.Rows[i].Field<string>("ralSupplierShippingMethodID");
				eRPRMAClaimLineInformationDto.ralSupplierTrackingNumber = dataTable.Rows[i].Field<string>("ralSupplierTrackingNumber");
				eRPRMAClaimLineInformationDto.ralTrackingNumber = dataTable.Rows[i].Field<string>("ralTrackingNumber");
				eRPRMAClaimLineInformationDto.ralUnitCost = dataTable.Rows[i].Field<decimal>("ralUnitCost");
				eRPRMAClaimLineInformationDto.ralUnitCostForeign = dataTable.Rows[i].Field<decimal>("ralUnitCostForeign");
				eRPRMAClaimLineInformationDto.ralUnitDiscountBase = dataTable.Rows[i].Field<decimal>("ralUnitDiscountBase");
				eRPRMAClaimLineInformationDto.ralUnitDiscountForeign = dataTable.Rows[i].Field<decimal>("ralUnitDiscountForeign");
				eRPRMAClaimLineInformationDto.ralUnitOfMeasure = dataTable.Rows[i].Field<string>("ralUnitOfMeasure");
				eRPRMAClaimLineInformationDto.ralUnitPrice = dataTable.Rows[i].Field<decimal>("ralUnitPrice");
				eRPRMAClaimLineInformationDto.ralUnitPriceForeign = dataTable.Rows[i].Field<decimal>("ralUnitPriceForeign");
				eRPRMAClaimLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAClaimLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAClaimLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAClaimLineInformationDto> GetRMAClaimLine(Guid rMAClaimLineId)
	{
		ERPRMAClaimLineInformationDto eRPRMAClaimLineInformationDto = new ERPRMAClaimLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[68]
		{
			"ralActionType", "ralConversionFactor", "ralCreatedBy", "ralCreatedDate", "ralCustomerPo", "ralDiscountPercent", "ralUniqueID", "ralExtendedCost", "ralExtendedCostForeign", "ralExtendedDiscountBase",
			"ralExtendedDiscountForeign", "ralExtendedPrice", "ralExtendedPriceForeign", "ralFullExtendedPriceBase", "ralFullExtendedPriceForeign", "ralFullUnitPriceBase", "ralFullUnitPriceForeign", "ralCustomerToPayForShipping", "ralInvoicedComplete", "ralKitPart",
			"ralReceivedComplete", "ralRequiresInspection", "ralReturnToSupplier", "ralTransferredToSalesOrder", "ralOrgPartID", "ralOrgPartShortDescription", "ralPartBinID", "ralPartGroupID", "ralPartID", "ralPartLongDescriptionRtf",
			"ralPartLongDescriptionText", "ralPartRevisionID", "ralPartShortDescription", "ralPartWarehouseLocationID", "ralProjectAreaID", "ralProjectID", "ralPurchaseLocationID", "ralQuantity", "ralQuantityReceived", "ralReceivedDate",
			"ralRequiredDate", "ralReturnedDate", "ralReturnReasonID", "ralRmaClaimID", "ralRowVersion", "ralSalesOrderDeliveryID", "ralSalesOrderID", "ralSalesOrderLineID", "ralSalesQuantity", "ralSalesUnitOfMeasure",
			"ralRmaClaimLineID", "ralShipmentID", "ralShipmentLineID", "ralShippedDate", "ralShippingMethodID", "ralShippingPaymentTypeID", "ralSupplierAuthorizationNumber", "ralSupplierOrganizationID", "ralSupplierShippingMethodID", "ralSupplierTrackingNumber",
			"ralTrackingNumber", "ralUnitCost", "ralUnitCostForeign", "ralUnitDiscountBase", "ralUnitDiscountForeign", "ralUnitOfMeasure", "ralUnitPrice", "ralUnitPriceForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ralUniqueID|C", rMAClaimLineId);
		AddCustomFieldsToSelectList("RMAClaimLines");
		using (DataTable dataTable = GetAsDataTable("RMAClaimLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAClaimLineInformationDto);
			}
			eRPRMAClaimLineInformationDto.ralActionType = dataTable.Rows[0].Field<string>("ralActionType");
			eRPRMAClaimLineInformationDto.ralConversionFactor = dataTable.Rows[0].Field<decimal>("ralConversionFactor");
			eRPRMAClaimLineInformationDto.ralCreatedBy = dataTable.Rows[0].Field<string>("ralCreatedBy");
			eRPRMAClaimLineInformationDto.ralCreatedDate = dataTable.Rows[0].Field<DateTime?>("ralCreatedDate");
			eRPRMAClaimLineInformationDto.ralCustomerPo = dataTable.Rows[0].Field<string>("ralCustomerPo");
			eRPRMAClaimLineInformationDto.ralDiscountPercent = dataTable.Rows[0].Field<decimal>("ralDiscountPercent");
			eRPRMAClaimLineInformationDto.ralUniqueID = dataTable.Rows[0].Field<Guid>("ralUniqueID");
			eRPRMAClaimLineInformationDto.ralExtendedCost = dataTable.Rows[0].Field<decimal>("ralExtendedCost");
			eRPRMAClaimLineInformationDto.ralExtendedCostForeign = dataTable.Rows[0].Field<decimal>("ralExtendedCostForeign");
			eRPRMAClaimLineInformationDto.ralExtendedDiscountBase = dataTable.Rows[0].Field<decimal>("ralExtendedDiscountBase");
			eRPRMAClaimLineInformationDto.ralExtendedDiscountForeign = dataTable.Rows[0].Field<decimal>("ralExtendedDiscountForeign");
			eRPRMAClaimLineInformationDto.ralExtendedPrice = dataTable.Rows[0].Field<decimal>("ralExtendedPrice");
			eRPRMAClaimLineInformationDto.ralExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("ralExtendedPriceForeign");
			eRPRMAClaimLineInformationDto.ralFullExtendedPriceBase = dataTable.Rows[0].Field<decimal>("ralFullExtendedPriceBase");
			eRPRMAClaimLineInformationDto.ralFullExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("ralFullExtendedPriceForeign");
			eRPRMAClaimLineInformationDto.ralFullUnitPriceBase = dataTable.Rows[0].Field<decimal>("ralFullUnitPriceBase");
			eRPRMAClaimLineInformationDto.ralFullUnitPriceForeign = dataTable.Rows[0].Field<decimal>("ralFullUnitPriceForeign");
			eRPRMAClaimLineInformationDto.ralCustomerToPayForShipping = dataTable.Rows[0].Field<bool>("ralCustomerToPayForShipping");
			eRPRMAClaimLineInformationDto.ralInvoicedComplete = dataTable.Rows[0].Field<bool>("ralInvoicedComplete");
			eRPRMAClaimLineInformationDto.ralKitPart = dataTable.Rows[0].Field<bool>("ralKitPart");
			eRPRMAClaimLineInformationDto.ralReceivedComplete = dataTable.Rows[0].Field<bool>("ralReceivedComplete");
			eRPRMAClaimLineInformationDto.ralRequiresInspection = dataTable.Rows[0].Field<bool>("ralRequiresInspection");
			eRPRMAClaimLineInformationDto.ralReturnToSupplier = dataTable.Rows[0].Field<bool>("ralReturnToSupplier");
			eRPRMAClaimLineInformationDto.ralTransferredToSalesOrder = dataTable.Rows[0].Field<bool>("ralTransferredToSalesOrder");
			eRPRMAClaimLineInformationDto.ralOrgPartID = dataTable.Rows[0].Field<string>("ralOrgPartID");
			eRPRMAClaimLineInformationDto.ralOrgPartShortDescription = dataTable.Rows[0].Field<string>("ralOrgPartShortDescription");
			eRPRMAClaimLineInformationDto.ralPartBinID = dataTable.Rows[0].Field<string>("ralPartBinID");
			eRPRMAClaimLineInformationDto.ralPartGroupID = dataTable.Rows[0].Field<string>("ralPartGroupID");
			eRPRMAClaimLineInformationDto.ralPartID = dataTable.Rows[0].Field<string>("ralPartID");
			eRPRMAClaimLineInformationDto.ralPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("ralPartLongDescriptionRtf");
			eRPRMAClaimLineInformationDto.ralPartLongDescriptionText = dataTable.Rows[0].Field<string>("ralPartLongDescriptionText");
			eRPRMAClaimLineInformationDto.ralPartRevisionID = dataTable.Rows[0].Field<string>("ralPartRevisionID");
			eRPRMAClaimLineInformationDto.ralPartShortDescription = dataTable.Rows[0].Field<string>("ralPartShortDescription");
			eRPRMAClaimLineInformationDto.ralPartWarehouseLocationID = dataTable.Rows[0].Field<string>("ralPartWarehouseLocationID");
			eRPRMAClaimLineInformationDto.ralProjectAreaID = dataTable.Rows[0].Field<string>("ralProjectAreaID");
			eRPRMAClaimLineInformationDto.ralProjectID = dataTable.Rows[0].Field<string>("ralProjectID");
			eRPRMAClaimLineInformationDto.ralPurchaseLocationID = dataTable.Rows[0].Field<string>("ralPurchaseLocationID");
			eRPRMAClaimLineInformationDto.ralQuantity = dataTable.Rows[0].Field<decimal>("ralQuantity");
			eRPRMAClaimLineInformationDto.ralQuantityReceived = dataTable.Rows[0].Field<decimal>("ralQuantityReceived");
			eRPRMAClaimLineInformationDto.ralReceivedDate = dataTable.Rows[0].Field<DateTime?>("ralReceivedDate");
			eRPRMAClaimLineInformationDto.ralRequiredDate = dataTable.Rows[0].Field<DateTime?>("ralRequiredDate");
			eRPRMAClaimLineInformationDto.ralReturnedDate = dataTable.Rows[0].Field<DateTime?>("ralReturnedDate");
			eRPRMAClaimLineInformationDto.ralReturnReasonID = dataTable.Rows[0].Field<string>("ralReturnReasonID");
			eRPRMAClaimLineInformationDto.ralRmaClaimID = dataTable.Rows[0].Field<string>("ralRmaClaimID");
			eRPRMAClaimLineInformationDto.ralRowVersion = dataTable.Rows[0].Field<byte[]>("ralRowVersion");
			eRPRMAClaimLineInformationDto.ralSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("ralSalesOrderDeliveryID");
			eRPRMAClaimLineInformationDto.ralSalesOrderID = dataTable.Rows[0].Field<string>("ralSalesOrderID");
			eRPRMAClaimLineInformationDto.ralSalesOrderLineID = dataTable.Rows[0].Field<short>("ralSalesOrderLineID");
			eRPRMAClaimLineInformationDto.ralSalesQuantity = dataTable.Rows[0].Field<decimal>("ralSalesQuantity");
			eRPRMAClaimLineInformationDto.ralSalesUnitOfMeasure = dataTable.Rows[0].Field<string>("ralSalesUnitOfMeasure");
			eRPRMAClaimLineInformationDto.ralRmaClaimLineID = dataTable.Rows[0].Field<short>("ralRmaClaimLineID");
			eRPRMAClaimLineInformationDto.ralShipmentID = dataTable.Rows[0].Field<string>("ralShipmentID");
			eRPRMAClaimLineInformationDto.ralShipmentLineID = dataTable.Rows[0].Field<short>("ralShipmentLineID");
			eRPRMAClaimLineInformationDto.ralShippedDate = dataTable.Rows[0].Field<DateTime?>("ralShippedDate");
			eRPRMAClaimLineInformationDto.ralShippingMethodID = dataTable.Rows[0].Field<string>("ralShippingMethodID");
			eRPRMAClaimLineInformationDto.ralShippingPaymentTypeID = dataTable.Rows[0].Field<string>("ralShippingPaymentTypeID");
			eRPRMAClaimLineInformationDto.ralSupplierAuthorizationNumber = dataTable.Rows[0].Field<string>("ralSupplierAuthorizationNumber");
			eRPRMAClaimLineInformationDto.ralSupplierOrganizationID = dataTable.Rows[0].Field<string>("ralSupplierOrganizationID");
			eRPRMAClaimLineInformationDto.ralSupplierShippingMethodID = dataTable.Rows[0].Field<string>("ralSupplierShippingMethodID");
			eRPRMAClaimLineInformationDto.ralSupplierTrackingNumber = dataTable.Rows[0].Field<string>("ralSupplierTrackingNumber");
			eRPRMAClaimLineInformationDto.ralTrackingNumber = dataTable.Rows[0].Field<string>("ralTrackingNumber");
			eRPRMAClaimLineInformationDto.ralUnitCost = dataTable.Rows[0].Field<decimal>("ralUnitCost");
			eRPRMAClaimLineInformationDto.ralUnitCostForeign = dataTable.Rows[0].Field<decimal>("ralUnitCostForeign");
			eRPRMAClaimLineInformationDto.ralUnitDiscountBase = dataTable.Rows[0].Field<decimal>("ralUnitDiscountBase");
			eRPRMAClaimLineInformationDto.ralUnitDiscountForeign = dataTable.Rows[0].Field<decimal>("ralUnitDiscountForeign");
			eRPRMAClaimLineInformationDto.ralUnitOfMeasure = dataTable.Rows[0].Field<string>("ralUnitOfMeasure");
			eRPRMAClaimLineInformationDto.ralUnitPrice = dataTable.Rows[0].Field<decimal>("ralUnitPrice");
			eRPRMAClaimLineInformationDto.ralUnitPriceForeign = dataTable.Rows[0].Field<decimal>("ralUnitPriceForeign");
			eRPRMAClaimLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAClaimLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAClaimLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRMAClaimLine(ERPRMAClaimLineDto rMAClaimLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RMAClaimLines WHERE ralUniqueID = " + M1Util.ConvertToLinq(rMAClaimLine.ralUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ralRmaClaimID"] = rMAClaimLine.ralRmaClaimID.ToUpper();
				dataRow["ralRmaClaimLineID"] = rMAClaimLine.ralRmaClaimLineID;
				rMAClaimLine.ralUniqueID = ((rMAClaimLine.ralUniqueID == Guid.Empty) ? Guid.NewGuid() : rMAClaimLine.ralUniqueID);
				dataRow["ralUniqueID"] = rMAClaimLine.ralUniqueID;
				dataRow["ralCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ralCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RMAClaimLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rMAClaimLine.ralRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RMAClaimLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ralRowVersion"], rMAClaimLine.ralRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RMAClaimLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RMAClaimLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ralActionType"] = rMAClaimLine.ralActionType;
			dataRow["ralConversionFactor"] = rMAClaimLine.ralConversionFactor;
			dataRow["ralCustomerPo"] = rMAClaimLine.ralCustomerPo;
			dataRow["ralDiscountPercent"] = rMAClaimLine.ralDiscountPercent;
			dataRow["ralExtendedCost"] = rMAClaimLine.ralExtendedCost;
			dataRow["ralExtendedCostForeign"] = rMAClaimLine.ralExtendedCostForeign;
			dataRow["ralExtendedDiscountBase"] = rMAClaimLine.ralExtendedDiscountBase;
			dataRow["ralExtendedDiscountForeign"] = rMAClaimLine.ralExtendedDiscountForeign;
			dataRow["ralExtendedPrice"] = rMAClaimLine.ralExtendedPrice;
			dataRow["ralExtendedPriceForeign"] = rMAClaimLine.ralExtendedPriceForeign;
			dataRow["ralFullExtendedPriceBase"] = rMAClaimLine.ralFullExtendedPriceBase;
			dataRow["ralFullExtendedPriceForeign"] = rMAClaimLine.ralFullExtendedPriceForeign;
			dataRow["ralFullUnitPriceBase"] = rMAClaimLine.ralFullUnitPriceBase;
			dataRow["ralFullUnitPriceForeign"] = rMAClaimLine.ralFullUnitPriceForeign;
			dataRow["ralCustomerToPayForShipping"] = rMAClaimLine.ralCustomerToPayForShipping;
			dataRow["ralInvoicedComplete"] = rMAClaimLine.ralInvoicedComplete;
			dataRow["ralKitPart"] = rMAClaimLine.ralKitPart;
			dataRow["ralReceivedComplete"] = rMAClaimLine.ralReceivedComplete;
			dataRow["ralRequiresInspection"] = rMAClaimLine.ralRequiresInspection;
			dataRow["ralReturnToSupplier"] = rMAClaimLine.ralReturnToSupplier;
			dataRow["ralTransferredToSalesOrder"] = rMAClaimLine.ralTransferredToSalesOrder;
			dataRow["ralOrgPartID"] = rMAClaimLine.ralOrgPartID;
			dataRow["ralOrgPartShortDescription"] = rMAClaimLine.ralOrgPartShortDescription;
			dataRow["ralPartBinID"] = rMAClaimLine.ralPartBinID;
			dataRow["ralPartGroupID"] = rMAClaimLine.ralPartGroupID;
			dataRow["ralPartID"] = rMAClaimLine.ralPartID;
			dataRow["ralPartLongDescriptionRtf"] = rMAClaimLine.ralPartLongDescriptionRtf ?? dataRow["ralPartLongDescriptionRtf"];
			dataRow["ralPartLongDescriptionText"] = rMAClaimLine.ralPartLongDescriptionText ?? dataRow["ralPartLongDescriptionText"];
			dataRow["ralPartRevisionID"] = rMAClaimLine.ralPartRevisionID;
			dataRow["ralPartShortDescription"] = rMAClaimLine.ralPartShortDescription;
			dataRow["ralPartWarehouseLocationID"] = rMAClaimLine.ralPartWarehouseLocationID;
			dataRow["ralProjectAreaID"] = rMAClaimLine.ralProjectAreaID;
			dataRow["ralProjectID"] = rMAClaimLine.ralProjectID;
			dataRow["ralPurchaseLocationID"] = rMAClaimLine.ralPurchaseLocationID;
			dataRow["ralQuantity"] = rMAClaimLine.ralQuantity;
			dataRow["ralQuantityReceived"] = rMAClaimLine.ralQuantityReceived;
			DataRow dataRow2 = dataRow;
			DateTime? ralReceivedDate = rMAClaimLine.ralReceivedDate;
			dataRow2["ralReceivedDate"] = (ralReceivedDate.HasValue ? ((object)ralReceivedDate.GetValueOrDefault()) : dataRow["ralReceivedDate"]);
			DataRow dataRow3 = dataRow;
			ralReceivedDate = rMAClaimLine.ralRequiredDate;
			dataRow3["ralRequiredDate"] = (ralReceivedDate.HasValue ? ((object)ralReceivedDate.GetValueOrDefault()) : dataRow["ralRequiredDate"]);
			DataRow dataRow4 = dataRow;
			ralReceivedDate = rMAClaimLine.ralReturnedDate;
			dataRow4["ralReturnedDate"] = (ralReceivedDate.HasValue ? ((object)ralReceivedDate.GetValueOrDefault()) : dataRow["ralReturnedDate"]);
			dataRow["ralReturnReasonID"] = rMAClaimLine.ralReturnReasonID;
			dataRow["ralSalesOrderDeliveryID"] = rMAClaimLine.ralSalesOrderDeliveryID;
			dataRow["ralSalesOrderID"] = rMAClaimLine.ralSalesOrderID;
			dataRow["ralSalesOrderLineID"] = rMAClaimLine.ralSalesOrderLineID;
			dataRow["ralSalesQuantity"] = rMAClaimLine.ralSalesQuantity;
			dataRow["ralSalesUnitOfMeasure"] = rMAClaimLine.ralSalesUnitOfMeasure;
			dataRow["ralShipmentID"] = rMAClaimLine.ralShipmentID;
			dataRow["ralShipmentLineID"] = rMAClaimLine.ralShipmentLineID;
			DataRow dataRow5 = dataRow;
			ralReceivedDate = rMAClaimLine.ralShippedDate;
			dataRow5["ralShippedDate"] = (ralReceivedDate.HasValue ? ((object)ralReceivedDate.GetValueOrDefault()) : dataRow["ralShippedDate"]);
			dataRow["ralShippingMethodID"] = rMAClaimLine.ralShippingMethodID;
			dataRow["ralShippingPaymentTypeID"] = rMAClaimLine.ralShippingPaymentTypeID;
			dataRow["ralSupplierAuthorizationNumber"] = rMAClaimLine.ralSupplierAuthorizationNumber;
			dataRow["ralSupplierOrganizationID"] = rMAClaimLine.ralSupplierOrganizationID;
			dataRow["ralSupplierShippingMethodID"] = rMAClaimLine.ralSupplierShippingMethodID;
			dataRow["ralSupplierTrackingNumber"] = rMAClaimLine.ralSupplierTrackingNumber;
			dataRow["ralTrackingNumber"] = rMAClaimLine.ralTrackingNumber;
			dataRow["ralUnitCost"] = rMAClaimLine.ralUnitCost;
			dataRow["ralUnitCostForeign"] = rMAClaimLine.ralUnitCostForeign;
			dataRow["ralUnitDiscountBase"] = rMAClaimLine.ralUnitDiscountBase;
			dataRow["ralUnitDiscountForeign"] = rMAClaimLine.ralUnitDiscountForeign;
			dataRow["ralUnitOfMeasure"] = rMAClaimLine.ralUnitOfMeasure;
			dataRow["ralUnitPrice"] = rMAClaimLine.ralUnitPrice;
			dataRow["ralUnitPriceForeign"] = rMAClaimLine.ralUnitPriceForeign;
			if (rMAClaimLine.CustomFields != null && rMAClaimLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rMAClaimLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RMAClaimLine [{rMAClaimLine.ralUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RMAClaimLine [{rMAClaimLine.ralUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
