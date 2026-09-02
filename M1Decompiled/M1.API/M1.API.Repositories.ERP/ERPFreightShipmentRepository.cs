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

public class ERPFreightShipmentRepository : APIBaseRepository, IERPFreightShipmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPFreightShipmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFreightShipmentExist(Guid freightShipmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("fspUniqueID|C", freightShipmentId);
		base.selectList.Add("fspUniqueID");
		return Task.FromResult(GetAsObject("FreightShipments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFreightShipmentInformationDto>> GetAllFreightShipments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFreightShipmentInformationDto> collection = new List<ERPFreightShipmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[62]
		{
			"fspCarrier", "fspFreightShipmentID", "fspCreatedBy", "fspCreatedDate", "fspDeclaredValue", "fspDistributeCostsOption", "fspUniqueID", "fspFdxAccessibility", "fspFdxCodCollectionAmount", "fspFdxCodCollectionType",
			"fspFdxDropOffType", "fspFdxHandlingCost", "fspFdxHomeDeliveryType", "fspFdxLastLogID", "fspFdxLastReplyErrorCode", "fspFdxLastReplyErrorMessage", "fspFdxLastReplySoftErrorCode", "fspFdxLastReplySoftErrorMsg", "fspFdxLastReplySoftErrorType", "fspFdxLastRequestDate",
			"fspFdxLastUTI", "fspFdxPackagingCost", "fspFdxPayorAccountNumber", "fspFdxPayorCountryCode", "fspFdxPayorType", "fspFdxRateRequestType", "fspFdxReturnShipIndicator", "fspFdxService", "fspFdxShipCostMarkupPct", "fspFdxSignatureOption",
			"fspFdxSignatureReleaseAuthNum", "fspFdxStatus", "fspFdxStatusText", "fspFdxVHCAmountOrPercentage", "fspFdxVHCLevel", "fspFdxVHCType", "fspFreightShipmentDate", "fspFdxCod", "fspFdxHoldAtLocation", "fspFdxInsideDelivery",
			"fspFdxInsidePickup", "fspFdxOneItemPerShipment", "fspFdxSaturdayDelivery", "fspFdxSaturdayPickup", "fspUpsSaturdayDelivery", "fspVoidOnUps", "fspNotesRTF", "fspNotesText", "fspRowVersion", "fspShipFromOrganizationID",
			"fspShipLocationID", "fspShipOrganizationID", "fspShipperAcctNumber", "fspShippingMethodID", "fspTotalCharges", "fspTotalPublishedCharges", "fspUps3rdPartyLocationID", "fspUps3rdPartyOrganizationID", "fspUpsBillAcctNumber", "fspUpsBillingOption",
			"fspUpsInterfaceStatus", "fspUpsServiceType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("FreightShipments");
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
		using (DataTable dataTable = GetAsDataTable("FreightShipments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFreightShipmentInformationDto eRPFreightShipmentInformationDto = new ERPFreightShipmentInformationDto();
				eRPFreightShipmentInformationDto.fspCarrier = dataTable.Rows[i].Field<string>("fspCarrier");
				eRPFreightShipmentInformationDto.fspFreightShipmentID = dataTable.Rows[i].Field<string>("fspFreightShipmentID");
				eRPFreightShipmentInformationDto.fspCreatedBy = dataTable.Rows[i].Field<string>("fspCreatedBy");
				eRPFreightShipmentInformationDto.fspCreatedDate = dataTable.Rows[i].Field<DateTime?>("fspCreatedDate");
				eRPFreightShipmentInformationDto.fspDeclaredValue = dataTable.Rows[i].Field<decimal>("fspDeclaredValue");
				eRPFreightShipmentInformationDto.fspDistributeCostsOption = dataTable.Rows[i].Field<byte>("fspDistributeCostsOption");
				eRPFreightShipmentInformationDto.fspUniqueID = dataTable.Rows[i].Field<Guid>("fspUniqueID");
				eRPFreightShipmentInformationDto.fspFdxAccessibility = dataTable.Rows[i].Field<string>("fspFdxAccessibility");
				eRPFreightShipmentInformationDto.fspFdxCodCollectionAmount = dataTable.Rows[i].Field<decimal>("fspFdxCodCollectionAmount");
				eRPFreightShipmentInformationDto.fspFdxCodCollectionType = dataTable.Rows[i].Field<string>("fspFdxCodCollectionType");
				eRPFreightShipmentInformationDto.fspFdxDropOffType = dataTable.Rows[i].Field<string>("fspFdxDropOffType");
				eRPFreightShipmentInformationDto.fspFdxHandlingCost = dataTable.Rows[i].Field<decimal>("fspFdxHandlingCost");
				eRPFreightShipmentInformationDto.fspFdxHomeDeliveryType = dataTable.Rows[i].Field<string>("fspFdxHomeDeliveryType");
				eRPFreightShipmentInformationDto.fspFdxLastLogID = dataTable.Rows[i].Field<int>("fspFdxLastLogID");
				eRPFreightShipmentInformationDto.fspFdxLastReplyErrorCode = dataTable.Rows[i].Field<string>("fspFdxLastReplyErrorCode");
				eRPFreightShipmentInformationDto.fspFdxLastReplyErrorMessage = dataTable.Rows[i].Field<string>("fspFdxLastReplyErrorMessage");
				eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorCode = dataTable.Rows[i].Field<string>("fspFdxLastReplySoftErrorCode");
				eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorMsg = dataTable.Rows[i].Field<string>("fspFdxLastReplySoftErrorMsg");
				eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorType = dataTable.Rows[i].Field<string>("fspFdxLastReplySoftErrorType");
				eRPFreightShipmentInformationDto.fspFdxLastRequestDate = dataTable.Rows[i].Field<DateTime?>("fspFdxLastRequestDate");
				eRPFreightShipmentInformationDto.fspFdxLastUTI = dataTable.Rows[i].Field<string>("fspFdxLastUTI");
				eRPFreightShipmentInformationDto.fspFdxPackagingCost = dataTable.Rows[i].Field<decimal>("fspFdxPackagingCost");
				eRPFreightShipmentInformationDto.fspFdxPayorAccountNumber = dataTable.Rows[i].Field<string>("fspFdxPayorAccountNumber");
				eRPFreightShipmentInformationDto.fspFdxPayorCountryCode = dataTable.Rows[i].Field<string>("fspFdxPayorCountryCode");
				eRPFreightShipmentInformationDto.fspFdxPayorType = dataTable.Rows[i].Field<string>("fspFdxPayorType");
				eRPFreightShipmentInformationDto.fspFdxRateRequestType = dataTable.Rows[i].Field<string>("fspFdxRateRequestType");
				eRPFreightShipmentInformationDto.fspFdxReturnShipIndicator = dataTable.Rows[i].Field<string>("fspFdxReturnShipIndicator");
				eRPFreightShipmentInformationDto.fspFdxService = dataTable.Rows[i].Field<string>("fspFdxService");
				eRPFreightShipmentInformationDto.fspFdxShipCostMarkupPct = dataTable.Rows[i].Field<decimal>("fspFdxShipCostMarkupPct");
				eRPFreightShipmentInformationDto.fspFdxSignatureOption = dataTable.Rows[i].Field<string>("fspFdxSignatureOption");
				eRPFreightShipmentInformationDto.fspFdxSignatureReleaseAuthNum = dataTable.Rows[i].Field<string>("fspFdxSignatureReleaseAuthNum");
				eRPFreightShipmentInformationDto.fspFdxStatus = dataTable.Rows[i].Field<byte>("fspFdxStatus");
				eRPFreightShipmentInformationDto.fspFdxStatusText = dataTable.Rows[i].Field<string>("fspFdxStatusText");
				eRPFreightShipmentInformationDto.fspFdxVHCAmountOrPercentage = dataTable.Rows[i].Field<decimal>("fspFdxVHCAmountOrPercentage");
				eRPFreightShipmentInformationDto.fspFdxVHCLevel = dataTable.Rows[i].Field<string>("fspFdxVHCLevel");
				eRPFreightShipmentInformationDto.fspFdxVHCType = dataTable.Rows[i].Field<string>("fspFdxVHCType");
				eRPFreightShipmentInformationDto.fspFreightShipmentDate = dataTable.Rows[i].Field<DateTime?>("fspFreightShipmentDate");
				eRPFreightShipmentInformationDto.fspFdxCod = dataTable.Rows[i].Field<bool>("fspFdxCod");
				eRPFreightShipmentInformationDto.fspFdxHoldAtLocation = dataTable.Rows[i].Field<bool>("fspFdxHoldAtLocation");
				eRPFreightShipmentInformationDto.fspFdxInsideDelivery = dataTable.Rows[i].Field<bool>("fspFdxInsideDelivery");
				eRPFreightShipmentInformationDto.fspFdxInsidePickup = dataTable.Rows[i].Field<bool>("fspFdxInsidePickup");
				eRPFreightShipmentInformationDto.fspFdxOneItemPerShipment = dataTable.Rows[i].Field<bool>("fspFdxOneItemPerShipment");
				eRPFreightShipmentInformationDto.fspFdxSaturdayDelivery = dataTable.Rows[i].Field<bool>("fspFdxSaturdayDelivery");
				eRPFreightShipmentInformationDto.fspFdxSaturdayPickup = dataTable.Rows[i].Field<bool>("fspFdxSaturdayPickup");
				eRPFreightShipmentInformationDto.fspUpsSaturdayDelivery = dataTable.Rows[i].Field<bool>("fspUpsSaturdayDelivery");
				eRPFreightShipmentInformationDto.fspVoidOnUps = dataTable.Rows[i].Field<bool>("fspVoidOnUps");
				eRPFreightShipmentInformationDto.fspNotesRTF = dataTable.Rows[i].Field<string>("fspNotesRTF");
				eRPFreightShipmentInformationDto.fspNotesText = dataTable.Rows[i].Field<string>("fspNotesText");
				eRPFreightShipmentInformationDto.fspRowVersion = dataTable.Rows[i].Field<byte[]>("fspRowVersion");
				eRPFreightShipmentInformationDto.fspShipFromOrganizationID = dataTable.Rows[i].Field<string>("fspShipFromOrganizationID");
				eRPFreightShipmentInformationDto.fspShipLocationID = dataTable.Rows[i].Field<string>("fspShipLocationID");
				eRPFreightShipmentInformationDto.fspShipOrganizationID = dataTable.Rows[i].Field<string>("fspShipOrganizationID");
				eRPFreightShipmentInformationDto.fspShipperAcctNumber = dataTable.Rows[i].Field<string>("fspShipperAcctNumber");
				eRPFreightShipmentInformationDto.fspShippingMethodID = dataTable.Rows[i].Field<string>("fspShippingMethodID");
				eRPFreightShipmentInformationDto.fspTotalCharges = dataTable.Rows[i].Field<decimal>("fspTotalCharges");
				eRPFreightShipmentInformationDto.fspTotalPublishedCharges = dataTable.Rows[i].Field<decimal>("fspTotalPublishedCharges");
				eRPFreightShipmentInformationDto.fspUps3rdPartyLocationID = dataTable.Rows[i].Field<string>("fspUps3rdPartyLocationID");
				eRPFreightShipmentInformationDto.fspUps3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("fspUps3rdPartyOrganizationID");
				eRPFreightShipmentInformationDto.fspUpsBillAcctNumber = dataTable.Rows[i].Field<string>("fspUpsBillAcctNumber");
				eRPFreightShipmentInformationDto.fspUpsBillingOption = dataTable.Rows[i].Field<string>("fspUpsBillingOption");
				eRPFreightShipmentInformationDto.fspUpsInterfaceStatus = dataTable.Rows[i].Field<byte>("fspUpsInterfaceStatus");
				eRPFreightShipmentInformationDto.fspUpsServiceType = dataTable.Rows[i].Field<string>("fspUpsServiceType");
				eRPFreightShipmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFreightShipmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFreightShipmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFreightShipmentInformationDto> GetFreightShipment(Guid freightShipmentId)
	{
		ERPFreightShipmentInformationDto eRPFreightShipmentInformationDto = new ERPFreightShipmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[62]
		{
			"fspCarrier", "fspFreightShipmentID", "fspCreatedBy", "fspCreatedDate", "fspDeclaredValue", "fspDistributeCostsOption", "fspUniqueID", "fspFdxAccessibility", "fspFdxCodCollectionAmount", "fspFdxCodCollectionType",
			"fspFdxDropOffType", "fspFdxHandlingCost", "fspFdxHomeDeliveryType", "fspFdxLastLogID", "fspFdxLastReplyErrorCode", "fspFdxLastReplyErrorMessage", "fspFdxLastReplySoftErrorCode", "fspFdxLastReplySoftErrorMsg", "fspFdxLastReplySoftErrorType", "fspFdxLastRequestDate",
			"fspFdxLastUTI", "fspFdxPackagingCost", "fspFdxPayorAccountNumber", "fspFdxPayorCountryCode", "fspFdxPayorType", "fspFdxRateRequestType", "fspFdxReturnShipIndicator", "fspFdxService", "fspFdxShipCostMarkupPct", "fspFdxSignatureOption",
			"fspFdxSignatureReleaseAuthNum", "fspFdxStatus", "fspFdxStatusText", "fspFdxVHCAmountOrPercentage", "fspFdxVHCLevel", "fspFdxVHCType", "fspFreightShipmentDate", "fspFdxCod", "fspFdxHoldAtLocation", "fspFdxInsideDelivery",
			"fspFdxInsidePickup", "fspFdxOneItemPerShipment", "fspFdxSaturdayDelivery", "fspFdxSaturdayPickup", "fspUpsSaturdayDelivery", "fspVoidOnUps", "fspNotesRTF", "fspNotesText", "fspRowVersion", "fspShipFromOrganizationID",
			"fspShipLocationID", "fspShipOrganizationID", "fspShipperAcctNumber", "fspShippingMethodID", "fspTotalCharges", "fspTotalPublishedCharges", "fspUps3rdPartyLocationID", "fspUps3rdPartyOrganizationID", "fspUpsBillAcctNumber", "fspUpsBillingOption",
			"fspUpsInterfaceStatus", "fspUpsServiceType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fspUniqueID|C", freightShipmentId);
		AddCustomFieldsToSelectList("FreightShipments");
		using (DataTable dataTable = GetAsDataTable("FreightShipments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFreightShipmentInformationDto);
			}
			eRPFreightShipmentInformationDto.fspCarrier = dataTable.Rows[0].Field<string>("fspCarrier");
			eRPFreightShipmentInformationDto.fspFreightShipmentID = dataTable.Rows[0].Field<string>("fspFreightShipmentID");
			eRPFreightShipmentInformationDto.fspCreatedBy = dataTable.Rows[0].Field<string>("fspCreatedBy");
			eRPFreightShipmentInformationDto.fspCreatedDate = dataTable.Rows[0].Field<DateTime?>("fspCreatedDate");
			eRPFreightShipmentInformationDto.fspDeclaredValue = dataTable.Rows[0].Field<decimal>("fspDeclaredValue");
			eRPFreightShipmentInformationDto.fspDistributeCostsOption = dataTable.Rows[0].Field<byte>("fspDistributeCostsOption");
			eRPFreightShipmentInformationDto.fspUniqueID = dataTable.Rows[0].Field<Guid>("fspUniqueID");
			eRPFreightShipmentInformationDto.fspFdxAccessibility = dataTable.Rows[0].Field<string>("fspFdxAccessibility");
			eRPFreightShipmentInformationDto.fspFdxCodCollectionAmount = dataTable.Rows[0].Field<decimal>("fspFdxCodCollectionAmount");
			eRPFreightShipmentInformationDto.fspFdxCodCollectionType = dataTable.Rows[0].Field<string>("fspFdxCodCollectionType");
			eRPFreightShipmentInformationDto.fspFdxDropOffType = dataTable.Rows[0].Field<string>("fspFdxDropOffType");
			eRPFreightShipmentInformationDto.fspFdxHandlingCost = dataTable.Rows[0].Field<decimal>("fspFdxHandlingCost");
			eRPFreightShipmentInformationDto.fspFdxHomeDeliveryType = dataTable.Rows[0].Field<string>("fspFdxHomeDeliveryType");
			eRPFreightShipmentInformationDto.fspFdxLastLogID = dataTable.Rows[0].Field<int>("fspFdxLastLogID");
			eRPFreightShipmentInformationDto.fspFdxLastReplyErrorCode = dataTable.Rows[0].Field<string>("fspFdxLastReplyErrorCode");
			eRPFreightShipmentInformationDto.fspFdxLastReplyErrorMessage = dataTable.Rows[0].Field<string>("fspFdxLastReplyErrorMessage");
			eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorCode = dataTable.Rows[0].Field<string>("fspFdxLastReplySoftErrorCode");
			eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorMsg = dataTable.Rows[0].Field<string>("fspFdxLastReplySoftErrorMsg");
			eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorType = dataTable.Rows[0].Field<string>("fspFdxLastReplySoftErrorType");
			eRPFreightShipmentInformationDto.fspFdxLastRequestDate = dataTable.Rows[0].Field<DateTime?>("fspFdxLastRequestDate");
			eRPFreightShipmentInformationDto.fspFdxLastUTI = dataTable.Rows[0].Field<string>("fspFdxLastUTI");
			eRPFreightShipmentInformationDto.fspFdxPackagingCost = dataTable.Rows[0].Field<decimal>("fspFdxPackagingCost");
			eRPFreightShipmentInformationDto.fspFdxPayorAccountNumber = dataTable.Rows[0].Field<string>("fspFdxPayorAccountNumber");
			eRPFreightShipmentInformationDto.fspFdxPayorCountryCode = dataTable.Rows[0].Field<string>("fspFdxPayorCountryCode");
			eRPFreightShipmentInformationDto.fspFdxPayorType = dataTable.Rows[0].Field<string>("fspFdxPayorType");
			eRPFreightShipmentInformationDto.fspFdxRateRequestType = dataTable.Rows[0].Field<string>("fspFdxRateRequestType");
			eRPFreightShipmentInformationDto.fspFdxReturnShipIndicator = dataTable.Rows[0].Field<string>("fspFdxReturnShipIndicator");
			eRPFreightShipmentInformationDto.fspFdxService = dataTable.Rows[0].Field<string>("fspFdxService");
			eRPFreightShipmentInformationDto.fspFdxShipCostMarkupPct = dataTable.Rows[0].Field<decimal>("fspFdxShipCostMarkupPct");
			eRPFreightShipmentInformationDto.fspFdxSignatureOption = dataTable.Rows[0].Field<string>("fspFdxSignatureOption");
			eRPFreightShipmentInformationDto.fspFdxSignatureReleaseAuthNum = dataTable.Rows[0].Field<string>("fspFdxSignatureReleaseAuthNum");
			eRPFreightShipmentInformationDto.fspFdxStatus = dataTable.Rows[0].Field<byte>("fspFdxStatus");
			eRPFreightShipmentInformationDto.fspFdxStatusText = dataTable.Rows[0].Field<string>("fspFdxStatusText");
			eRPFreightShipmentInformationDto.fspFdxVHCAmountOrPercentage = dataTable.Rows[0].Field<decimal>("fspFdxVHCAmountOrPercentage");
			eRPFreightShipmentInformationDto.fspFdxVHCLevel = dataTable.Rows[0].Field<string>("fspFdxVHCLevel");
			eRPFreightShipmentInformationDto.fspFdxVHCType = dataTable.Rows[0].Field<string>("fspFdxVHCType");
			eRPFreightShipmentInformationDto.fspFreightShipmentDate = dataTable.Rows[0].Field<DateTime?>("fspFreightShipmentDate");
			eRPFreightShipmentInformationDto.fspFdxCod = dataTable.Rows[0].Field<bool>("fspFdxCod");
			eRPFreightShipmentInformationDto.fspFdxHoldAtLocation = dataTable.Rows[0].Field<bool>("fspFdxHoldAtLocation");
			eRPFreightShipmentInformationDto.fspFdxInsideDelivery = dataTable.Rows[0].Field<bool>("fspFdxInsideDelivery");
			eRPFreightShipmentInformationDto.fspFdxInsidePickup = dataTable.Rows[0].Field<bool>("fspFdxInsidePickup");
			eRPFreightShipmentInformationDto.fspFdxOneItemPerShipment = dataTable.Rows[0].Field<bool>("fspFdxOneItemPerShipment");
			eRPFreightShipmentInformationDto.fspFdxSaturdayDelivery = dataTable.Rows[0].Field<bool>("fspFdxSaturdayDelivery");
			eRPFreightShipmentInformationDto.fspFdxSaturdayPickup = dataTable.Rows[0].Field<bool>("fspFdxSaturdayPickup");
			eRPFreightShipmentInformationDto.fspUpsSaturdayDelivery = dataTable.Rows[0].Field<bool>("fspUpsSaturdayDelivery");
			eRPFreightShipmentInformationDto.fspVoidOnUps = dataTable.Rows[0].Field<bool>("fspVoidOnUps");
			eRPFreightShipmentInformationDto.fspNotesRTF = dataTable.Rows[0].Field<string>("fspNotesRTF");
			eRPFreightShipmentInformationDto.fspNotesText = dataTable.Rows[0].Field<string>("fspNotesText");
			eRPFreightShipmentInformationDto.fspRowVersion = dataTable.Rows[0].Field<byte[]>("fspRowVersion");
			eRPFreightShipmentInformationDto.fspShipFromOrganizationID = dataTable.Rows[0].Field<string>("fspShipFromOrganizationID");
			eRPFreightShipmentInformationDto.fspShipLocationID = dataTable.Rows[0].Field<string>("fspShipLocationID");
			eRPFreightShipmentInformationDto.fspShipOrganizationID = dataTable.Rows[0].Field<string>("fspShipOrganizationID");
			eRPFreightShipmentInformationDto.fspShipperAcctNumber = dataTable.Rows[0].Field<string>("fspShipperAcctNumber");
			eRPFreightShipmentInformationDto.fspShippingMethodID = dataTable.Rows[0].Field<string>("fspShippingMethodID");
			eRPFreightShipmentInformationDto.fspTotalCharges = dataTable.Rows[0].Field<decimal>("fspTotalCharges");
			eRPFreightShipmentInformationDto.fspTotalPublishedCharges = dataTable.Rows[0].Field<decimal>("fspTotalPublishedCharges");
			eRPFreightShipmentInformationDto.fspUps3rdPartyLocationID = dataTable.Rows[0].Field<string>("fspUps3rdPartyLocationID");
			eRPFreightShipmentInformationDto.fspUps3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("fspUps3rdPartyOrganizationID");
			eRPFreightShipmentInformationDto.fspUpsBillAcctNumber = dataTable.Rows[0].Field<string>("fspUpsBillAcctNumber");
			eRPFreightShipmentInformationDto.fspUpsBillingOption = dataTable.Rows[0].Field<string>("fspUpsBillingOption");
			eRPFreightShipmentInformationDto.fspUpsInterfaceStatus = dataTable.Rows[0].Field<byte>("fspUpsInterfaceStatus");
			eRPFreightShipmentInformationDto.fspUpsServiceType = dataTable.Rows[0].Field<string>("fspUpsServiceType");
			eRPFreightShipmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFreightShipmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFreightShipmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveFreightShipment(ERPFreightShipmentDto freightShipment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM FreightShipments WHERE fspUniqueID = " + M1Util.ConvertToLinq(freightShipment.fspUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fspFreightShipmentID"] = freightShipment.fspFreightShipmentID.ToUpper();
				freightShipment.fspUniqueID = ((freightShipment.fspUniqueID == Guid.Empty) ? Guid.NewGuid() : freightShipment.fspUniqueID);
				dataRow["fspUniqueID"] = freightShipment.fspUniqueID;
				dataRow["fspCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fspCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The FreightShipment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (freightShipment.fspRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the FreightShipment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fspRowVersion"], freightShipment.fspRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the FreightShipment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the FreightShipment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fspCarrier"] = freightShipment.fspCarrier;
			dataRow["fspDeclaredValue"] = freightShipment.fspDeclaredValue;
			dataRow["fspDistributeCostsOption"] = freightShipment.fspDistributeCostsOption;
			dataRow["fspFdxAccessibility"] = freightShipment.fspFdxAccessibility;
			dataRow["fspFdxCodCollectionAmount"] = freightShipment.fspFdxCodCollectionAmount;
			dataRow["fspFdxCodCollectionType"] = freightShipment.fspFdxCodCollectionType;
			dataRow["fspFdxDropOffType"] = freightShipment.fspFdxDropOffType;
			dataRow["fspFdxHandlingCost"] = freightShipment.fspFdxHandlingCost;
			dataRow["fspFdxHomeDeliveryType"] = freightShipment.fspFdxHomeDeliveryType;
			dataRow["fspFdxLastLogID"] = freightShipment.fspFdxLastLogID;
			dataRow["fspFdxLastReplyErrorCode"] = freightShipment.fspFdxLastReplyErrorCode;
			dataRow["fspFdxLastReplyErrorMessage"] = freightShipment.fspFdxLastReplyErrorMessage;
			dataRow["fspFdxLastReplySoftErrorCode"] = freightShipment.fspFdxLastReplySoftErrorCode;
			dataRow["fspFdxLastReplySoftErrorMsg"] = freightShipment.fspFdxLastReplySoftErrorMsg ?? dataRow["fspFdxLastReplySoftErrorMsg"];
			dataRow["fspFdxLastReplySoftErrorType"] = freightShipment.fspFdxLastReplySoftErrorType;
			DataRow dataRow2 = dataRow;
			DateTime? fspFdxLastRequestDate = freightShipment.fspFdxLastRequestDate;
			dataRow2["fspFdxLastRequestDate"] = (fspFdxLastRequestDate.HasValue ? ((object)fspFdxLastRequestDate.GetValueOrDefault()) : dataRow["fspFdxLastRequestDate"]);
			dataRow["fspFdxLastUTI"] = freightShipment.fspFdxLastUTI;
			dataRow["fspFdxPackagingCost"] = freightShipment.fspFdxPackagingCost;
			dataRow["fspFdxPayorAccountNumber"] = freightShipment.fspFdxPayorAccountNumber;
			dataRow["fspFdxPayorCountryCode"] = freightShipment.fspFdxPayorCountryCode;
			dataRow["fspFdxPayorType"] = freightShipment.fspFdxPayorType;
			dataRow["fspFdxRateRequestType"] = freightShipment.fspFdxRateRequestType;
			dataRow["fspFdxReturnShipIndicator"] = freightShipment.fspFdxReturnShipIndicator;
			dataRow["fspFdxService"] = freightShipment.fspFdxService;
			dataRow["fspFdxShipCostMarkupPct"] = freightShipment.fspFdxShipCostMarkupPct;
			dataRow["fspFdxSignatureOption"] = freightShipment.fspFdxSignatureOption;
			dataRow["fspFdxSignatureReleaseAuthNum"] = freightShipment.fspFdxSignatureReleaseAuthNum;
			dataRow["fspFdxStatus"] = freightShipment.fspFdxStatus;
			dataRow["fspFdxStatusText"] = freightShipment.fspFdxStatusText ?? dataRow["fspFdxStatusText"];
			dataRow["fspFdxVHCAmountOrPercentage"] = freightShipment.fspFdxVHCAmountOrPercentage;
			dataRow["fspFdxVHCLevel"] = freightShipment.fspFdxVHCLevel;
			dataRow["fspFdxVHCType"] = freightShipment.fspFdxVHCType;
			DataRow dataRow3 = dataRow;
			fspFdxLastRequestDate = freightShipment.fspFreightShipmentDate;
			dataRow3["fspFreightShipmentDate"] = (fspFdxLastRequestDate.HasValue ? ((object)fspFdxLastRequestDate.GetValueOrDefault()) : dataRow["fspFreightShipmentDate"]);
			dataRow["fspFdxCod"] = freightShipment.fspFdxCod;
			dataRow["fspFdxHoldAtLocation"] = freightShipment.fspFdxHoldAtLocation;
			dataRow["fspFdxInsideDelivery"] = freightShipment.fspFdxInsideDelivery;
			dataRow["fspFdxInsidePickup"] = freightShipment.fspFdxInsidePickup;
			dataRow["fspFdxOneItemPerShipment"] = freightShipment.fspFdxOneItemPerShipment;
			dataRow["fspFdxSaturdayDelivery"] = freightShipment.fspFdxSaturdayDelivery;
			dataRow["fspFdxSaturdayPickup"] = freightShipment.fspFdxSaturdayPickup;
			dataRow["fspUpsSaturdayDelivery"] = freightShipment.fspUpsSaturdayDelivery;
			dataRow["fspVoidOnUps"] = freightShipment.fspVoidOnUps;
			dataRow["fspNotesRTF"] = freightShipment.fspNotesRTF ?? dataRow["fspNotesRTF"];
			dataRow["fspNotesText"] = freightShipment.fspNotesText ?? dataRow["fspNotesText"];
			dataRow["fspShipFromOrganizationID"] = freightShipment.fspShipFromOrganizationID;
			dataRow["fspShipLocationID"] = freightShipment.fspShipLocationID;
			dataRow["fspShipOrganizationID"] = freightShipment.fspShipOrganizationID;
			dataRow["fspShipperAcctNumber"] = freightShipment.fspShipperAcctNumber;
			dataRow["fspShippingMethodID"] = freightShipment.fspShippingMethodID;
			dataRow["fspTotalCharges"] = freightShipment.fspTotalCharges;
			dataRow["fspTotalPublishedCharges"] = freightShipment.fspTotalPublishedCharges;
			dataRow["fspUps3rdPartyLocationID"] = freightShipment.fspUps3rdPartyLocationID;
			dataRow["fspUps3rdPartyOrganizationID"] = freightShipment.fspUps3rdPartyOrganizationID;
			dataRow["fspUpsBillAcctNumber"] = freightShipment.fspUpsBillAcctNumber;
			dataRow["fspUpsBillingOption"] = freightShipment.fspUpsBillingOption;
			dataRow["fspUpsInterfaceStatus"] = freightShipment.fspUpsInterfaceStatus;
			dataRow["fspUpsServiceType"] = freightShipment.fspUpsServiceType;
			if (freightShipment.CustomFields != null && freightShipment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in freightShipment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the FreightShipment [{freightShipment.fspUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the FreightShipment [{freightShipment.fspUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
