using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPShippingPropertyRepository : APIBaseRepository, IERPShippingPropertyRepository, IAPIBaseRepository, IDisposable
{
	public ERPShippingPropertyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShippingPropertyExist(Guid shippingPropertyId)
	{
		InitializeParameterLists();
		base.filterList.Add("xsmUniqueID|C", shippingPropertyId);
		base.selectList.Add("xsmUniqueID");
		return Task.FromResult(GetAsObject("ShippingProperties", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShippingPropertyInformationDto>> GetAllShippingProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShippingPropertyInformationDto> collection = new List<ERPShippingPropertyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[97]
		{
			"xsmCreatedBy", "xsmCreatedDate", "xsmUniqueID", "xsmFdxAccessibility", "xsmFdxAccountNumber", "xsmFdxAccountNumberOAuth", "xsmFdxAddressLine1", "xsmFdxAddressLine2", "xsmFdxAddrValAccuracyIndicator", "xsmFdxCity",
			"xsmFdxClientID", "xsmFdxClientIDTrack", "xsmFdxClientSecret", "xsmFdxClientSecretTrack", "xsmFdxCodCollectionAmount", "xsmFdxCodCollectionType", "xsmFdxCountry", "xsmFdxCurrencyType", "xsmFdxDeclaredValueCurrency", "xsmFdxDepartment",
			"xsmFdxDimensionsUnitOfMeasure", "xsmFdxDropOffType", "xsmFdxEmailAddress", "xsmFdxFaxNumber", "xsmFdxHandlingCost", "xsmFdxHomeDeliveryDate", "xsmFdxHomeDeliveryType", "xsmFdxHostAddress", "xsmFdxHostPort", "xsmFdxHostService",
			"xsmFdxLabelFormatType", "xsmFdxLabelImageType", "xsmFdxLabelStockType", "xsmFdxLabelStoreLocation", "xsmFdxLabelType", "xsmFdxLblPrintOrientType", "xsmFdxMeterNumber", "xsmFdxName", "xsmFdxPackageHeight", "xsmFdxPackageLength",
			"xsmFdxPackageWidth", "xsmFdxPackaging", "xsmFdxPackagingCost", "xsmFdxPagerNumber", "xsmFdxPayorType", "xsmFdxPersonName", "xsmFdxPhoneNumber", "xsmFdxPostCode", "xsmFdxRateElementBasis", "xsmFdxRateRequestType",
			"xsmFdxRateTypeBasis", "xsmFdxReturnShipIndicator", "xsmFdxShipCostMarkupPct", "xsmFdxShipDocImageType", "xsmFdxSignatureOption", "xsmFdxState", "xsmFdxSubscribedServices", "xsmFdxVHCAmountOrPercentage", "xsmFdxVHCLevel", "xsmFdxVHCType",
			"xsmFdxWeightUnitOfMeasure", "xsmFedExAccessKey", "xsmFedExAccessToken", "xsmFedExAccessTokenTrack", "xsmFedExAuthenticationMethod", "xsmFedExPassword", "xsmFedExTokenExpiresIn", "xsmFedExTokenExpiresInTrack", "xsmFedExUserName", "xsmFdxBareCostOfDuty",
			"xsmFdxBareTrasportationCost", "xsmFdxCod", "xsmFdxHoldAtLocation", "xsmFdxInsideDelivery", "xsmFdxInsidePickup", "xsmFdxNonstandardContainer", "xsmFdxOneItemPerShipment", "xsmFdxResidentialAddress", "xsmFdxSaturdayDelivery", "xsmFdxSaturdayPickup",
			"xsmFedExIsProduction", "xsmUpsIsProduction", "xsmRowVersion", "xsmUpsAccessKey", "xsmUpsAccessToken", "xsmUpsAccountNo", "xsmUpsAccountNoOAuth", "xsmUpsAuthenticationMethod", "xsmUpsLabelStockSize", "xsmUpsLabelStoreLocation",
			"xsmUpsLabelType", "xsmUpsLocIDPref", "xsmUpsLocPostCodePref", "xsmUpsPassword", "xsmUpsRefreshToken", "xsmUpsUsername", "xsmUSDcurrencyCode"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShippingProperties");
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
		using (DataTable dataTable = GetAsDataTable("ShippingProperties", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShippingPropertyInformationDto eRPShippingPropertyInformationDto = new ERPShippingPropertyInformationDto();
				eRPShippingPropertyInformationDto.xsmCreatedBy = dataTable.Rows[i].Field<string>("xsmCreatedBy");
				eRPShippingPropertyInformationDto.xsmCreatedDate = dataTable.Rows[i].Field<DateTime?>("xsmCreatedDate");
				eRPShippingPropertyInformationDto.xsmUniqueID = dataTable.Rows[i].Field<Guid>("xsmUniqueID");
				eRPShippingPropertyInformationDto.xsmFdxAccessibility = dataTable.Rows[i].Field<string>("xsmFdxAccessibility");
				eRPShippingPropertyInformationDto.xsmFdxAccountNumber = dataTable.Rows[i].Field<string>("xsmFdxAccountNumber");
				eRPShippingPropertyInformationDto.xsmFdxAccountNumberOAuth = dataTable.Rows[i].Field<string>("xsmFdxAccountNumberOAuth");
				eRPShippingPropertyInformationDto.xsmFdxAddressLine1 = dataTable.Rows[i].Field<string>("xsmFdxAddressLine1");
				eRPShippingPropertyInformationDto.xsmFdxAddressLine2 = dataTable.Rows[i].Field<string>("xsmFdxAddressLine2");
				eRPShippingPropertyInformationDto.xsmFdxAddrValAccuracyIndicator = dataTable.Rows[i].Field<string>("xsmFdxAddrValAccuracyIndicator");
				eRPShippingPropertyInformationDto.xsmFdxCity = dataTable.Rows[i].Field<string>("xsmFdxCity");
				eRPShippingPropertyInformationDto.xsmFdxClientID = dataTable.Rows[i].Field<string>("xsmFdxClientID");
				eRPShippingPropertyInformationDto.xsmFdxClientIDTrack = dataTable.Rows[i].Field<string>("xsmFdxClientIDTrack");
				eRPShippingPropertyInformationDto.xsmFdxClientSecret = dataTable.Rows[i].Field<string>("xsmFdxClientSecret");
				eRPShippingPropertyInformationDto.xsmFdxClientSecretTrack = dataTable.Rows[i].Field<string>("xsmFdxClientSecretTrack");
				eRPShippingPropertyInformationDto.xsmFdxCodCollectionAmount = dataTable.Rows[i].Field<decimal>("xsmFdxCodCollectionAmount");
				eRPShippingPropertyInformationDto.xsmFdxCodCollectionType = dataTable.Rows[i].Field<string>("xsmFdxCodCollectionType");
				eRPShippingPropertyInformationDto.xsmFdxCountry = dataTable.Rows[i].Field<string>("xsmFdxCountry");
				eRPShippingPropertyInformationDto.xsmFdxCurrencyType = dataTable.Rows[i].Field<string>("xsmFdxCurrencyType");
				eRPShippingPropertyInformationDto.xsmFdxDeclaredValueCurrency = dataTable.Rows[i].Field<string>("xsmFdxDeclaredValueCurrency");
				eRPShippingPropertyInformationDto.xsmFdxDepartment = dataTable.Rows[i].Field<string>("xsmFdxDepartment");
				eRPShippingPropertyInformationDto.xsmFdxDimensionsUnitOfMeasure = dataTable.Rows[i].Field<string>("xsmFdxDimensionsUnitOfMeasure");
				eRPShippingPropertyInformationDto.xsmFdxDropOffType = dataTable.Rows[i].Field<string>("xsmFdxDropOffType");
				eRPShippingPropertyInformationDto.xsmFdxEmailAddress = dataTable.Rows[i].Field<string>("xsmFdxEmailAddress");
				eRPShippingPropertyInformationDto.xsmFdxFaxNumber = dataTable.Rows[i].Field<string>("xsmFdxFaxNumber");
				eRPShippingPropertyInformationDto.xsmFdxHandlingCost = dataTable.Rows[i].Field<decimal>("xsmFdxHandlingCost");
				eRPShippingPropertyInformationDto.xsmFdxHomeDeliveryDate = dataTable.Rows[i].Field<DateTime?>("xsmFdxHomeDeliveryDate");
				eRPShippingPropertyInformationDto.xsmFdxHomeDeliveryType = dataTable.Rows[i].Field<string>("xsmFdxHomeDeliveryType");
				eRPShippingPropertyInformationDto.xsmFdxHostAddress = dataTable.Rows[i].Field<string>("xsmFdxHostAddress");
				eRPShippingPropertyInformationDto.xsmFdxHostPort = dataTable.Rows[i].Field<int>("xsmFdxHostPort");
				eRPShippingPropertyInformationDto.xsmFdxHostService = dataTable.Rows[i].Field<string>("xsmFdxHostService");
				eRPShippingPropertyInformationDto.xsmFdxLabelFormatType = dataTable.Rows[i].Field<string>("xsmFdxLabelFormatType");
				eRPShippingPropertyInformationDto.xsmFdxLabelImageType = dataTable.Rows[i].Field<string>("xsmFdxLabelImageType");
				eRPShippingPropertyInformationDto.xsmFdxLabelStockType = dataTable.Rows[i].Field<string>("xsmFdxLabelStockType");
				eRPShippingPropertyInformationDto.xsmFdxLabelStoreLocation = dataTable.Rows[i].Field<string>("xsmFdxLabelStoreLocation");
				eRPShippingPropertyInformationDto.xsmFdxLabelType = dataTable.Rows[i].Field<string>("xsmFdxLabelType");
				eRPShippingPropertyInformationDto.xsmFdxLblPrintOrientType = dataTable.Rows[i].Field<string>("xsmFdxLblPrintOrientType");
				eRPShippingPropertyInformationDto.xsmFdxMeterNumber = dataTable.Rows[i].Field<decimal>("xsmFdxMeterNumber");
				eRPShippingPropertyInformationDto.xsmFdxName = dataTable.Rows[i].Field<string>("xsmFdxName");
				eRPShippingPropertyInformationDto.xsmFdxPackageHeight = dataTable.Rows[i].Field<int>("xsmFdxPackageHeight");
				eRPShippingPropertyInformationDto.xsmFdxPackageLength = dataTable.Rows[i].Field<int>("xsmFdxPackageLength");
				eRPShippingPropertyInformationDto.xsmFdxPackageWidth = dataTable.Rows[i].Field<int>("xsmFdxPackageWidth");
				eRPShippingPropertyInformationDto.xsmFdxPackaging = dataTable.Rows[i].Field<string>("xsmFdxPackaging");
				eRPShippingPropertyInformationDto.xsmFdxPackagingCost = dataTable.Rows[i].Field<decimal>("xsmFdxPackagingCost");
				eRPShippingPropertyInformationDto.xsmFdxPagerNumber = dataTable.Rows[i].Field<string>("xsmFdxPagerNumber");
				eRPShippingPropertyInformationDto.xsmFdxPayorType = dataTable.Rows[i].Field<string>("xsmFdxPayorType");
				eRPShippingPropertyInformationDto.xsmFdxPersonName = dataTable.Rows[i].Field<string>("xsmFdxPersonName");
				eRPShippingPropertyInformationDto.xsmFdxPhoneNumber = dataTable.Rows[i].Field<string>("xsmFdxPhoneNumber");
				eRPShippingPropertyInformationDto.xsmFdxPostCode = dataTable.Rows[i].Field<string>("xsmFdxPostCode");
				eRPShippingPropertyInformationDto.xsmFdxRateElementBasis = dataTable.Rows[i].Field<string>("xsmFdxRateElementBasis");
				eRPShippingPropertyInformationDto.xsmFdxRateRequestType = dataTable.Rows[i].Field<string>("xsmFdxRateRequestType");
				eRPShippingPropertyInformationDto.xsmFdxRateTypeBasis = dataTable.Rows[i].Field<string>("xsmFdxRateTypeBasis");
				eRPShippingPropertyInformationDto.xsmFdxReturnShipIndicator = dataTable.Rows[i].Field<string>("xsmFdxReturnShipIndicator");
				eRPShippingPropertyInformationDto.xsmFdxShipCostMarkupPct = dataTable.Rows[i].Field<decimal>("xsmFdxShipCostMarkupPct");
				eRPShippingPropertyInformationDto.xsmFdxShipDocImageType = dataTable.Rows[i].Field<string>("xsmFdxShipDocImageType");
				eRPShippingPropertyInformationDto.xsmFdxSignatureOption = dataTable.Rows[i].Field<string>("xsmFdxSignatureOption");
				eRPShippingPropertyInformationDto.xsmFdxState = dataTable.Rows[i].Field<string>("xsmFdxState");
				eRPShippingPropertyInformationDto.xsmFdxSubscribedServices = dataTable.Rows[i].Field<string>("xsmFdxSubscribedServices");
				eRPShippingPropertyInformationDto.xsmFdxVHCAmountOrPercentage = dataTable.Rows[i].Field<decimal>("xsmFdxVHCAmountOrPercentage");
				eRPShippingPropertyInformationDto.xsmFdxVHCLevel = dataTable.Rows[i].Field<string>("xsmFdxVHCLevel");
				eRPShippingPropertyInformationDto.xsmFdxVHCType = dataTable.Rows[i].Field<string>("xsmFdxVHCType");
				eRPShippingPropertyInformationDto.xsmFdxWeightUnitOfMeasure = dataTable.Rows[i].Field<string>("xsmFdxWeightUnitOfMeasure");
				eRPShippingPropertyInformationDto.xsmFedExAccessKey = dataTable.Rows[i].Field<string>("xsmFedExAccessKey");
				eRPShippingPropertyInformationDto.xsmFedExAccessToken = dataTable.Rows[i].Field<string>("xsmFedExAccessToken");
				eRPShippingPropertyInformationDto.xsmFedExAccessTokenTrack = dataTable.Rows[i].Field<string>("xsmFedExAccessTokenTrack");
				eRPShippingPropertyInformationDto.xsmFedExAuthenticationMethod = dataTable.Rows[i].Field<string>("xsmFedExAuthenticationMethod");
				eRPShippingPropertyInformationDto.xsmFedExPassword = dataTable.Rows[i].Field<string>("xsmFedExPassword");
				eRPShippingPropertyInformationDto.xsmFedExTokenExpiresIn = dataTable.Rows[i].Field<DateTime?>("xsmFedExTokenExpiresIn");
				eRPShippingPropertyInformationDto.xsmFedExTokenExpiresInTrack = dataTable.Rows[i].Field<DateTime?>("xsmFedExTokenExpiresInTrack");
				eRPShippingPropertyInformationDto.xsmFedExUserName = dataTable.Rows[i].Field<string>("xsmFedExUserName");
				eRPShippingPropertyInformationDto.xsmFdxBareCostOfDuty = dataTable.Rows[i].Field<bool>("xsmFdxBareCostOfDuty");
				eRPShippingPropertyInformationDto.xsmFdxBareTrasportationCost = dataTable.Rows[i].Field<bool>("xsmFdxBareTrasportationCost");
				eRPShippingPropertyInformationDto.xsmFdxCod = dataTable.Rows[i].Field<bool>("xsmFdxCod");
				eRPShippingPropertyInformationDto.xsmFdxHoldAtLocation = dataTable.Rows[i].Field<bool>("xsmFdxHoldAtLocation");
				eRPShippingPropertyInformationDto.xsmFdxInsideDelivery = dataTable.Rows[i].Field<bool>("xsmFdxInsideDelivery");
				eRPShippingPropertyInformationDto.xsmFdxInsidePickup = dataTable.Rows[i].Field<bool>("xsmFdxInsidePickup");
				eRPShippingPropertyInformationDto.xsmFdxNonstandardContainer = dataTable.Rows[i].Field<bool>("xsmFdxNonstandardContainer");
				eRPShippingPropertyInformationDto.xsmFdxOneItemPerShipment = dataTable.Rows[i].Field<bool>("xsmFdxOneItemPerShipment");
				eRPShippingPropertyInformationDto.xsmFdxResidentialAddress = dataTable.Rows[i].Field<bool>("xsmFdxResidentialAddress");
				eRPShippingPropertyInformationDto.xsmFdxSaturdayDelivery = dataTable.Rows[i].Field<bool>("xsmFdxSaturdayDelivery");
				eRPShippingPropertyInformationDto.xsmFdxSaturdayPickup = dataTable.Rows[i].Field<bool>("xsmFdxSaturdayPickup");
				eRPShippingPropertyInformationDto.xsmFedExIsProduction = dataTable.Rows[i].Field<bool>("xsmFedExIsProduction");
				eRPShippingPropertyInformationDto.xsmUpsIsProduction = dataTable.Rows[i].Field<bool>("xsmUpsIsProduction");
				eRPShippingPropertyInformationDto.xsmRowVersion = dataTable.Rows[i].Field<byte[]>("xsmRowVersion");
				eRPShippingPropertyInformationDto.xsmUpsAccessKey = dataTable.Rows[i].Field<string>("xsmUpsAccessKey");
				eRPShippingPropertyInformationDto.xsmUpsAccessToken = dataTable.Rows[i].Field<string>("xsmUpsAccessToken");
				eRPShippingPropertyInformationDto.xsmUpsAccountNo = dataTable.Rows[i].Field<string>("xsmUpsAccountNo");
				eRPShippingPropertyInformationDto.xsmUpsAccountNoOAuth = dataTable.Rows[i].Field<string>("xsmUpsAccountNoOAuth");
				eRPShippingPropertyInformationDto.xsmUpsAuthenticationMethod = dataTable.Rows[i].Field<string>("xsmUpsAuthenticationMethod");
				eRPShippingPropertyInformationDto.xsmUpsLabelStockSize = dataTable.Rows[i].Field<string>("xsmUpsLabelStockSize");
				eRPShippingPropertyInformationDto.xsmUpsLabelStoreLocation = dataTable.Rows[i].Field<string>("xsmUpsLabelStoreLocation");
				eRPShippingPropertyInformationDto.xsmUpsLabelType = dataTable.Rows[i].Field<string>("xsmUpsLabelType");
				eRPShippingPropertyInformationDto.xsmUpsLocIDPref = dataTable.Rows[i].Field<string>("xsmUpsLocIDPref");
				eRPShippingPropertyInformationDto.xsmUpsLocPostCodePref = dataTable.Rows[i].Field<string>("xsmUpsLocPostCodePref");
				eRPShippingPropertyInformationDto.xsmUpsPassword = dataTable.Rows[i].Field<string>("xsmUpsPassword");
				eRPShippingPropertyInformationDto.xsmUpsRefreshToken = dataTable.Rows[i].Field<string>("xsmUpsRefreshToken");
				eRPShippingPropertyInformationDto.xsmUpsUsername = dataTable.Rows[i].Field<string>("xsmUpsUsername");
				eRPShippingPropertyInformationDto.xsmUSDcurrencyCode = dataTable.Rows[i].Field<string>("xsmUSDcurrencyCode");
				eRPShippingPropertyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShippingPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShippingPropertyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShippingPropertyInformationDto> GetShippingProperty(Guid shippingPropertyId)
	{
		ERPShippingPropertyInformationDto eRPShippingPropertyInformationDto = new ERPShippingPropertyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[97]
		{
			"xsmCreatedBy", "xsmCreatedDate", "xsmUniqueID", "xsmFdxAccessibility", "xsmFdxAccountNumber", "xsmFdxAccountNumberOAuth", "xsmFdxAddressLine1", "xsmFdxAddressLine2", "xsmFdxAddrValAccuracyIndicator", "xsmFdxCity",
			"xsmFdxClientID", "xsmFdxClientIDTrack", "xsmFdxClientSecret", "xsmFdxClientSecretTrack", "xsmFdxCodCollectionAmount", "xsmFdxCodCollectionType", "xsmFdxCountry", "xsmFdxCurrencyType", "xsmFdxDeclaredValueCurrency", "xsmFdxDepartment",
			"xsmFdxDimensionsUnitOfMeasure", "xsmFdxDropOffType", "xsmFdxEmailAddress", "xsmFdxFaxNumber", "xsmFdxHandlingCost", "xsmFdxHomeDeliveryDate", "xsmFdxHomeDeliveryType", "xsmFdxHostAddress", "xsmFdxHostPort", "xsmFdxHostService",
			"xsmFdxLabelFormatType", "xsmFdxLabelImageType", "xsmFdxLabelStockType", "xsmFdxLabelStoreLocation", "xsmFdxLabelType", "xsmFdxLblPrintOrientType", "xsmFdxMeterNumber", "xsmFdxName", "xsmFdxPackageHeight", "xsmFdxPackageLength",
			"xsmFdxPackageWidth", "xsmFdxPackaging", "xsmFdxPackagingCost", "xsmFdxPagerNumber", "xsmFdxPayorType", "xsmFdxPersonName", "xsmFdxPhoneNumber", "xsmFdxPostCode", "xsmFdxRateElementBasis", "xsmFdxRateRequestType",
			"xsmFdxRateTypeBasis", "xsmFdxReturnShipIndicator", "xsmFdxShipCostMarkupPct", "xsmFdxShipDocImageType", "xsmFdxSignatureOption", "xsmFdxState", "xsmFdxSubscribedServices", "xsmFdxVHCAmountOrPercentage", "xsmFdxVHCLevel", "xsmFdxVHCType",
			"xsmFdxWeightUnitOfMeasure", "xsmFedExAccessKey", "xsmFedExAccessToken", "xsmFedExAccessTokenTrack", "xsmFedExAuthenticationMethod", "xsmFedExPassword", "xsmFedExTokenExpiresIn", "xsmFedExTokenExpiresInTrack", "xsmFedExUserName", "xsmFdxBareCostOfDuty",
			"xsmFdxBareTrasportationCost", "xsmFdxCod", "xsmFdxHoldAtLocation", "xsmFdxInsideDelivery", "xsmFdxInsidePickup", "xsmFdxNonstandardContainer", "xsmFdxOneItemPerShipment", "xsmFdxResidentialAddress", "xsmFdxSaturdayDelivery", "xsmFdxSaturdayPickup",
			"xsmFedExIsProduction", "xsmUpsIsProduction", "xsmRowVersion", "xsmUpsAccessKey", "xsmUpsAccessToken", "xsmUpsAccountNo", "xsmUpsAccountNoOAuth", "xsmUpsAuthenticationMethod", "xsmUpsLabelStockSize", "xsmUpsLabelStoreLocation",
			"xsmUpsLabelType", "xsmUpsLocIDPref", "xsmUpsLocPostCodePref", "xsmUpsPassword", "xsmUpsRefreshToken", "xsmUpsUsername", "xsmUSDcurrencyCode"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xsmUniqueID|C", shippingPropertyId);
		AddCustomFieldsToSelectList("ShippingProperties");
		using (DataTable dataTable = GetAsDataTable("ShippingProperties", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShippingPropertyInformationDto);
			}
			eRPShippingPropertyInformationDto.xsmCreatedBy = dataTable.Rows[0].Field<string>("xsmCreatedBy");
			eRPShippingPropertyInformationDto.xsmCreatedDate = dataTable.Rows[0].Field<DateTime?>("xsmCreatedDate");
			eRPShippingPropertyInformationDto.xsmUniqueID = dataTable.Rows[0].Field<Guid>("xsmUniqueID");
			eRPShippingPropertyInformationDto.xsmFdxAccessibility = dataTable.Rows[0].Field<string>("xsmFdxAccessibility");
			eRPShippingPropertyInformationDto.xsmFdxAccountNumber = dataTable.Rows[0].Field<string>("xsmFdxAccountNumber");
			eRPShippingPropertyInformationDto.xsmFdxAccountNumberOAuth = dataTable.Rows[0].Field<string>("xsmFdxAccountNumberOAuth");
			eRPShippingPropertyInformationDto.xsmFdxAddressLine1 = dataTable.Rows[0].Field<string>("xsmFdxAddressLine1");
			eRPShippingPropertyInformationDto.xsmFdxAddressLine2 = dataTable.Rows[0].Field<string>("xsmFdxAddressLine2");
			eRPShippingPropertyInformationDto.xsmFdxAddrValAccuracyIndicator = dataTable.Rows[0].Field<string>("xsmFdxAddrValAccuracyIndicator");
			eRPShippingPropertyInformationDto.xsmFdxCity = dataTable.Rows[0].Field<string>("xsmFdxCity");
			eRPShippingPropertyInformationDto.xsmFdxClientID = dataTable.Rows[0].Field<string>("xsmFdxClientID");
			eRPShippingPropertyInformationDto.xsmFdxClientIDTrack = dataTable.Rows[0].Field<string>("xsmFdxClientIDTrack");
			eRPShippingPropertyInformationDto.xsmFdxClientSecret = dataTable.Rows[0].Field<string>("xsmFdxClientSecret");
			eRPShippingPropertyInformationDto.xsmFdxClientSecretTrack = dataTable.Rows[0].Field<string>("xsmFdxClientSecretTrack");
			eRPShippingPropertyInformationDto.xsmFdxCodCollectionAmount = dataTable.Rows[0].Field<decimal>("xsmFdxCodCollectionAmount");
			eRPShippingPropertyInformationDto.xsmFdxCodCollectionType = dataTable.Rows[0].Field<string>("xsmFdxCodCollectionType");
			eRPShippingPropertyInformationDto.xsmFdxCountry = dataTable.Rows[0].Field<string>("xsmFdxCountry");
			eRPShippingPropertyInformationDto.xsmFdxCurrencyType = dataTable.Rows[0].Field<string>("xsmFdxCurrencyType");
			eRPShippingPropertyInformationDto.xsmFdxDeclaredValueCurrency = dataTable.Rows[0].Field<string>("xsmFdxDeclaredValueCurrency");
			eRPShippingPropertyInformationDto.xsmFdxDepartment = dataTable.Rows[0].Field<string>("xsmFdxDepartment");
			eRPShippingPropertyInformationDto.xsmFdxDimensionsUnitOfMeasure = dataTable.Rows[0].Field<string>("xsmFdxDimensionsUnitOfMeasure");
			eRPShippingPropertyInformationDto.xsmFdxDropOffType = dataTable.Rows[0].Field<string>("xsmFdxDropOffType");
			eRPShippingPropertyInformationDto.xsmFdxEmailAddress = dataTable.Rows[0].Field<string>("xsmFdxEmailAddress");
			eRPShippingPropertyInformationDto.xsmFdxFaxNumber = dataTable.Rows[0].Field<string>("xsmFdxFaxNumber");
			eRPShippingPropertyInformationDto.xsmFdxHandlingCost = dataTable.Rows[0].Field<decimal>("xsmFdxHandlingCost");
			eRPShippingPropertyInformationDto.xsmFdxHomeDeliveryDate = dataTable.Rows[0].Field<DateTime?>("xsmFdxHomeDeliveryDate");
			eRPShippingPropertyInformationDto.xsmFdxHomeDeliveryType = dataTable.Rows[0].Field<string>("xsmFdxHomeDeliveryType");
			eRPShippingPropertyInformationDto.xsmFdxHostAddress = dataTable.Rows[0].Field<string>("xsmFdxHostAddress");
			eRPShippingPropertyInformationDto.xsmFdxHostPort = dataTable.Rows[0].Field<int>("xsmFdxHostPort");
			eRPShippingPropertyInformationDto.xsmFdxHostService = dataTable.Rows[0].Field<string>("xsmFdxHostService");
			eRPShippingPropertyInformationDto.xsmFdxLabelFormatType = dataTable.Rows[0].Field<string>("xsmFdxLabelFormatType");
			eRPShippingPropertyInformationDto.xsmFdxLabelImageType = dataTable.Rows[0].Field<string>("xsmFdxLabelImageType");
			eRPShippingPropertyInformationDto.xsmFdxLabelStockType = dataTable.Rows[0].Field<string>("xsmFdxLabelStockType");
			eRPShippingPropertyInformationDto.xsmFdxLabelStoreLocation = dataTable.Rows[0].Field<string>("xsmFdxLabelStoreLocation");
			eRPShippingPropertyInformationDto.xsmFdxLabelType = dataTable.Rows[0].Field<string>("xsmFdxLabelType");
			eRPShippingPropertyInformationDto.xsmFdxLblPrintOrientType = dataTable.Rows[0].Field<string>("xsmFdxLblPrintOrientType");
			eRPShippingPropertyInformationDto.xsmFdxMeterNumber = dataTable.Rows[0].Field<decimal>("xsmFdxMeterNumber");
			eRPShippingPropertyInformationDto.xsmFdxName = dataTable.Rows[0].Field<string>("xsmFdxName");
			eRPShippingPropertyInformationDto.xsmFdxPackageHeight = dataTable.Rows[0].Field<int>("xsmFdxPackageHeight");
			eRPShippingPropertyInformationDto.xsmFdxPackageLength = dataTable.Rows[0].Field<int>("xsmFdxPackageLength");
			eRPShippingPropertyInformationDto.xsmFdxPackageWidth = dataTable.Rows[0].Field<int>("xsmFdxPackageWidth");
			eRPShippingPropertyInformationDto.xsmFdxPackaging = dataTable.Rows[0].Field<string>("xsmFdxPackaging");
			eRPShippingPropertyInformationDto.xsmFdxPackagingCost = dataTable.Rows[0].Field<decimal>("xsmFdxPackagingCost");
			eRPShippingPropertyInformationDto.xsmFdxPagerNumber = dataTable.Rows[0].Field<string>("xsmFdxPagerNumber");
			eRPShippingPropertyInformationDto.xsmFdxPayorType = dataTable.Rows[0].Field<string>("xsmFdxPayorType");
			eRPShippingPropertyInformationDto.xsmFdxPersonName = dataTable.Rows[0].Field<string>("xsmFdxPersonName");
			eRPShippingPropertyInformationDto.xsmFdxPhoneNumber = dataTable.Rows[0].Field<string>("xsmFdxPhoneNumber");
			eRPShippingPropertyInformationDto.xsmFdxPostCode = dataTable.Rows[0].Field<string>("xsmFdxPostCode");
			eRPShippingPropertyInformationDto.xsmFdxRateElementBasis = dataTable.Rows[0].Field<string>("xsmFdxRateElementBasis");
			eRPShippingPropertyInformationDto.xsmFdxRateRequestType = dataTable.Rows[0].Field<string>("xsmFdxRateRequestType");
			eRPShippingPropertyInformationDto.xsmFdxRateTypeBasis = dataTable.Rows[0].Field<string>("xsmFdxRateTypeBasis");
			eRPShippingPropertyInformationDto.xsmFdxReturnShipIndicator = dataTable.Rows[0].Field<string>("xsmFdxReturnShipIndicator");
			eRPShippingPropertyInformationDto.xsmFdxShipCostMarkupPct = dataTable.Rows[0].Field<decimal>("xsmFdxShipCostMarkupPct");
			eRPShippingPropertyInformationDto.xsmFdxShipDocImageType = dataTable.Rows[0].Field<string>("xsmFdxShipDocImageType");
			eRPShippingPropertyInformationDto.xsmFdxSignatureOption = dataTable.Rows[0].Field<string>("xsmFdxSignatureOption");
			eRPShippingPropertyInformationDto.xsmFdxState = dataTable.Rows[0].Field<string>("xsmFdxState");
			eRPShippingPropertyInformationDto.xsmFdxSubscribedServices = dataTable.Rows[0].Field<string>("xsmFdxSubscribedServices");
			eRPShippingPropertyInformationDto.xsmFdxVHCAmountOrPercentage = dataTable.Rows[0].Field<decimal>("xsmFdxVHCAmountOrPercentage");
			eRPShippingPropertyInformationDto.xsmFdxVHCLevel = dataTable.Rows[0].Field<string>("xsmFdxVHCLevel");
			eRPShippingPropertyInformationDto.xsmFdxVHCType = dataTable.Rows[0].Field<string>("xsmFdxVHCType");
			eRPShippingPropertyInformationDto.xsmFdxWeightUnitOfMeasure = dataTable.Rows[0].Field<string>("xsmFdxWeightUnitOfMeasure");
			eRPShippingPropertyInformationDto.xsmFedExAccessKey = dataTable.Rows[0].Field<string>("xsmFedExAccessKey");
			eRPShippingPropertyInformationDto.xsmFedExAccessToken = dataTable.Rows[0].Field<string>("xsmFedExAccessToken");
			eRPShippingPropertyInformationDto.xsmFedExAccessTokenTrack = dataTable.Rows[0].Field<string>("xsmFedExAccessTokenTrack");
			eRPShippingPropertyInformationDto.xsmFedExAuthenticationMethod = dataTable.Rows[0].Field<string>("xsmFedExAuthenticationMethod");
			eRPShippingPropertyInformationDto.xsmFedExPassword = dataTable.Rows[0].Field<string>("xsmFedExPassword");
			eRPShippingPropertyInformationDto.xsmFedExTokenExpiresIn = dataTable.Rows[0].Field<DateTime?>("xsmFedExTokenExpiresIn");
			eRPShippingPropertyInformationDto.xsmFedExTokenExpiresInTrack = dataTable.Rows[0].Field<DateTime?>("xsmFedExTokenExpiresInTrack");
			eRPShippingPropertyInformationDto.xsmFedExUserName = dataTable.Rows[0].Field<string>("xsmFedExUserName");
			eRPShippingPropertyInformationDto.xsmFdxBareCostOfDuty = dataTable.Rows[0].Field<bool>("xsmFdxBareCostOfDuty");
			eRPShippingPropertyInformationDto.xsmFdxBareTrasportationCost = dataTable.Rows[0].Field<bool>("xsmFdxBareTrasportationCost");
			eRPShippingPropertyInformationDto.xsmFdxCod = dataTable.Rows[0].Field<bool>("xsmFdxCod");
			eRPShippingPropertyInformationDto.xsmFdxHoldAtLocation = dataTable.Rows[0].Field<bool>("xsmFdxHoldAtLocation");
			eRPShippingPropertyInformationDto.xsmFdxInsideDelivery = dataTable.Rows[0].Field<bool>("xsmFdxInsideDelivery");
			eRPShippingPropertyInformationDto.xsmFdxInsidePickup = dataTable.Rows[0].Field<bool>("xsmFdxInsidePickup");
			eRPShippingPropertyInformationDto.xsmFdxNonstandardContainer = dataTable.Rows[0].Field<bool>("xsmFdxNonstandardContainer");
			eRPShippingPropertyInformationDto.xsmFdxOneItemPerShipment = dataTable.Rows[0].Field<bool>("xsmFdxOneItemPerShipment");
			eRPShippingPropertyInformationDto.xsmFdxResidentialAddress = dataTable.Rows[0].Field<bool>("xsmFdxResidentialAddress");
			eRPShippingPropertyInformationDto.xsmFdxSaturdayDelivery = dataTable.Rows[0].Field<bool>("xsmFdxSaturdayDelivery");
			eRPShippingPropertyInformationDto.xsmFdxSaturdayPickup = dataTable.Rows[0].Field<bool>("xsmFdxSaturdayPickup");
			eRPShippingPropertyInformationDto.xsmFedExIsProduction = dataTable.Rows[0].Field<bool>("xsmFedExIsProduction");
			eRPShippingPropertyInformationDto.xsmUpsIsProduction = dataTable.Rows[0].Field<bool>("xsmUpsIsProduction");
			eRPShippingPropertyInformationDto.xsmRowVersion = dataTable.Rows[0].Field<byte[]>("xsmRowVersion");
			eRPShippingPropertyInformationDto.xsmUpsAccessKey = dataTable.Rows[0].Field<string>("xsmUpsAccessKey");
			eRPShippingPropertyInformationDto.xsmUpsAccessToken = dataTable.Rows[0].Field<string>("xsmUpsAccessToken");
			eRPShippingPropertyInformationDto.xsmUpsAccountNo = dataTable.Rows[0].Field<string>("xsmUpsAccountNo");
			eRPShippingPropertyInformationDto.xsmUpsAccountNoOAuth = dataTable.Rows[0].Field<string>("xsmUpsAccountNoOAuth");
			eRPShippingPropertyInformationDto.xsmUpsAuthenticationMethod = dataTable.Rows[0].Field<string>("xsmUpsAuthenticationMethod");
			eRPShippingPropertyInformationDto.xsmUpsLabelStockSize = dataTable.Rows[0].Field<string>("xsmUpsLabelStockSize");
			eRPShippingPropertyInformationDto.xsmUpsLabelStoreLocation = dataTable.Rows[0].Field<string>("xsmUpsLabelStoreLocation");
			eRPShippingPropertyInformationDto.xsmUpsLabelType = dataTable.Rows[0].Field<string>("xsmUpsLabelType");
			eRPShippingPropertyInformationDto.xsmUpsLocIDPref = dataTable.Rows[0].Field<string>("xsmUpsLocIDPref");
			eRPShippingPropertyInformationDto.xsmUpsLocPostCodePref = dataTable.Rows[0].Field<string>("xsmUpsLocPostCodePref");
			eRPShippingPropertyInformationDto.xsmUpsPassword = dataTable.Rows[0].Field<string>("xsmUpsPassword");
			eRPShippingPropertyInformationDto.xsmUpsRefreshToken = dataTable.Rows[0].Field<string>("xsmUpsRefreshToken");
			eRPShippingPropertyInformationDto.xsmUpsUsername = dataTable.Rows[0].Field<string>("xsmUpsUsername");
			eRPShippingPropertyInformationDto.xsmUSDcurrencyCode = dataTable.Rows[0].Field<string>("xsmUSDcurrencyCode");
			eRPShippingPropertyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShippingPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShippingPropertyInformationDto);
	}
}
