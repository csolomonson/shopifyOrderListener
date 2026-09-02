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

public class ERPShippingMethodRepository : APIBaseRepository, IERPShippingMethodRepository, IAPIBaseRepository, IDisposable
{
	public ERPShippingMethodRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShippingMethodExist(Guid shippingMethodId)
	{
		InitializeParameterLists();
		base.filterList.Add("xasUniqueID|C", shippingMethodId);
		base.selectList.Add("xasUniqueID");
		return Task.FromResult(GetAsObject("ShippingMethods", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShippingMethodInformationDto>> GetAllShippingMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShippingMethodInformationDto> collection = new List<ERPShippingMethodInformationDto>();
		InitializeParameterLists();
		string[] array = new string[63]
		{
			"xasAvalaraTaxCodeID", "xasCarrier", "xasCarrierAccountNumber", "xasShippingMethodID", "xasCreatedBy", "xasCreatedDate", "xasDescription", "xasDistributeCostsOption", "xasUniqueID", "xasFdxAccessibility",
			"xasFdxCodCollectionType", "xasFdxDropOffType", "xasFdxHomeDeliveryType", "xasFdxPackageType", "xasFdxRateElementBasis", "xasFdxRateRequestType", "xasFdxRateTypeBasis", "xasFdxReturnShipIndicator", "xasFdxService", "xasFdxSignatureOption",
			"xasFdxVHCAmountOrPercentage", "xasFdxVHCLevel", "xasFdxVHCType", "xasFedExBillingOption", "xasInactiveDate", "xasInactive", "xasFdxCertificateOfOrigin", "xasFdxCod", "xasFdxCommercialInvoice", "xasFdxExportDeclaration",
			"xasFdxHoldAtLocation", "xasFdxInsideDelivery", "xasFdxInsidePickup", "xasFdxNAFTACO", "xasFdxNonStandardContainer", "xasFdxReturnInstructions", "xasFdxSaturdayDelivery", "xasFdxSaturdayPickup", "xasUpsCertificateOfOrigin", "xasUpsCod",
			"xasUpsCommercialInvoice", "xasUpsNAFTACO", "xasUpsPackingList", "xasUpsPartialInvoice", "xasUpsSaturdayDelivery", "xasUpsUseInterface", "xasReferenceTrackingLink", "xasRowVersion", "xasSecondTaxCodeID", "xasShipChargeWeb",
			"xasShippingPaymentTypeID", "xasTaxCodeID", "xasTaxStatus", "xasTrackingLink", "xasUpsBillingOptionDefault", "xasUpsCodFundsCode", "xasUpsCostCenter", "xasUpsPackageType", "xasUpsServiceType", "xasUpsWsBillingOption",
			"xasUpsWSPackageType", "xasUpsWSServiceType", "xasUSPSEndorsement"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShippingMethods");
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
		using (DataTable dataTable = GetAsDataTable("ShippingMethods", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShippingMethodInformationDto eRPShippingMethodInformationDto = new ERPShippingMethodInformationDto();
				eRPShippingMethodInformationDto.xasAvalaraTaxCodeID = dataTable.Rows[i].Field<string>("xasAvalaraTaxCodeID");
				eRPShippingMethodInformationDto.xasCarrier = dataTable.Rows[i].Field<string>("xasCarrier");
				eRPShippingMethodInformationDto.xasCarrierAccountNumber = dataTable.Rows[i].Field<string>("xasCarrierAccountNumber");
				eRPShippingMethodInformationDto.xasShippingMethodID = dataTable.Rows[i].Field<string>("xasShippingMethodID");
				eRPShippingMethodInformationDto.xasCreatedBy = dataTable.Rows[i].Field<string>("xasCreatedBy");
				eRPShippingMethodInformationDto.xasCreatedDate = dataTable.Rows[i].Field<DateTime?>("xasCreatedDate");
				eRPShippingMethodInformationDto.xasDescription = dataTable.Rows[i].Field<string>("xasDescription");
				eRPShippingMethodInformationDto.xasDistributeCostsOption = dataTable.Rows[i].Field<byte>("xasDistributeCostsOption");
				eRPShippingMethodInformationDto.xasUniqueID = dataTable.Rows[i].Field<Guid>("xasUniqueID");
				eRPShippingMethodInformationDto.xasFdxAccessibility = dataTable.Rows[i].Field<string>("xasFdxAccessibility");
				eRPShippingMethodInformationDto.xasFdxCodCollectionType = dataTable.Rows[i].Field<string>("xasFdxCodCollectionType");
				eRPShippingMethodInformationDto.xasFdxDropOffType = dataTable.Rows[i].Field<string>("xasFdxDropOffType");
				eRPShippingMethodInformationDto.xasFdxHomeDeliveryType = dataTable.Rows[i].Field<string>("xasFdxHomeDeliveryType");
				eRPShippingMethodInformationDto.xasFdxPackageType = dataTable.Rows[i].Field<string>("xasFdxPackageType");
				eRPShippingMethodInformationDto.xasFdxRateElementBasis = dataTable.Rows[i].Field<string>("xasFdxRateElementBasis");
				eRPShippingMethodInformationDto.xasFdxRateRequestType = dataTable.Rows[i].Field<string>("xasFdxRateRequestType");
				eRPShippingMethodInformationDto.xasFdxRateTypeBasis = dataTable.Rows[i].Field<string>("xasFdxRateTypeBasis");
				eRPShippingMethodInformationDto.xasFdxReturnShipIndicator = dataTable.Rows[i].Field<string>("xasFdxReturnShipIndicator");
				eRPShippingMethodInformationDto.xasFdxService = dataTable.Rows[i].Field<string>("xasFdxService");
				eRPShippingMethodInformationDto.xasFdxSignatureOption = dataTable.Rows[i].Field<string>("xasFdxSignatureOption");
				eRPShippingMethodInformationDto.xasFdxVHCAmountOrPercentage = dataTable.Rows[i].Field<decimal>("xasFdxVHCAmountOrPercentage");
				eRPShippingMethodInformationDto.xasFdxVHCLevel = dataTable.Rows[i].Field<string>("xasFdxVHCLevel");
				eRPShippingMethodInformationDto.xasFdxVHCType = dataTable.Rows[i].Field<string>("xasFdxVHCType");
				eRPShippingMethodInformationDto.xasFedExBillingOption = dataTable.Rows[i].Field<string>("xasFedExBillingOption");
				eRPShippingMethodInformationDto.xasInactiveDate = dataTable.Rows[i].Field<DateTime?>("xasInactiveDate");
				eRPShippingMethodInformationDto.xasInactive = dataTable.Rows[i].Field<bool>("xasInactive");
				eRPShippingMethodInformationDto.xasFdxCertificateOfOrigin = dataTable.Rows[i].Field<bool>("xasFdxCertificateOfOrigin");
				eRPShippingMethodInformationDto.xasFdxCod = dataTable.Rows[i].Field<bool>("xasFdxCod");
				eRPShippingMethodInformationDto.xasFdxCommercialInvoice = dataTable.Rows[i].Field<bool>("xasFdxCommercialInvoice");
				eRPShippingMethodInformationDto.xasFdxExportDeclaration = dataTable.Rows[i].Field<bool>("xasFdxExportDeclaration");
				eRPShippingMethodInformationDto.xasFdxHoldAtLocation = dataTable.Rows[i].Field<bool>("xasFdxHoldAtLocation");
				eRPShippingMethodInformationDto.xasFdxInsideDelivery = dataTable.Rows[i].Field<bool>("xasFdxInsideDelivery");
				eRPShippingMethodInformationDto.xasFdxInsidePickup = dataTable.Rows[i].Field<bool>("xasFdxInsidePickup");
				eRPShippingMethodInformationDto.xasFdxNAFTACO = dataTable.Rows[i].Field<bool>("xasFdxNAFTACO");
				eRPShippingMethodInformationDto.xasFdxNonStandardContainer = dataTable.Rows[i].Field<bool>("xasFdxNonStandardContainer");
				eRPShippingMethodInformationDto.xasFdxReturnInstructions = dataTable.Rows[i].Field<bool>("xasFdxReturnInstructions");
				eRPShippingMethodInformationDto.xasFdxSaturdayDelivery = dataTable.Rows[i].Field<bool>("xasFdxSaturdayDelivery");
				eRPShippingMethodInformationDto.xasFdxSaturdayPickup = dataTable.Rows[i].Field<bool>("xasFdxSaturdayPickup");
				eRPShippingMethodInformationDto.xasUpsCertificateOfOrigin = dataTable.Rows[i].Field<bool>("xasUpsCertificateOfOrigin");
				eRPShippingMethodInformationDto.xasUpsCod = dataTable.Rows[i].Field<bool>("xasUpsCod");
				eRPShippingMethodInformationDto.xasUpsCommercialInvoice = dataTable.Rows[i].Field<bool>("xasUpsCommercialInvoice");
				eRPShippingMethodInformationDto.xasUpsNAFTACO = dataTable.Rows[i].Field<bool>("xasUpsNAFTACO");
				eRPShippingMethodInformationDto.xasUpsPackingList = dataTable.Rows[i].Field<bool>("xasUpsPackingList");
				eRPShippingMethodInformationDto.xasUpsPartialInvoice = dataTable.Rows[i].Field<bool>("xasUpsPartialInvoice");
				eRPShippingMethodInformationDto.xasUpsSaturdayDelivery = dataTable.Rows[i].Field<bool>("xasUpsSaturdayDelivery");
				eRPShippingMethodInformationDto.xasUpsUseInterface = dataTable.Rows[i].Field<bool>("xasUpsUseInterface");
				eRPShippingMethodInformationDto.xasReferenceTrackingLink = dataTable.Rows[i].Field<string>("xasReferenceTrackingLink");
				eRPShippingMethodInformationDto.xasRowVersion = dataTable.Rows[i].Field<byte[]>("xasRowVersion");
				eRPShippingMethodInformationDto.xasSecondTaxCodeID = dataTable.Rows[i].Field<string>("xasSecondTaxCodeID");
				eRPShippingMethodInformationDto.xasShipChargeWeb = dataTable.Rows[i].Field<decimal>("xasShipChargeWeb");
				eRPShippingMethodInformationDto.xasShippingPaymentTypeID = dataTable.Rows[i].Field<string>("xasShippingPaymentTypeID");
				eRPShippingMethodInformationDto.xasTaxCodeID = dataTable.Rows[i].Field<string>("xasTaxCodeID");
				eRPShippingMethodInformationDto.xasTaxStatus = dataTable.Rows[i].Field<byte>("xasTaxStatus");
				eRPShippingMethodInformationDto.xasTrackingLink = dataTable.Rows[i].Field<string>("xasTrackingLink");
				eRPShippingMethodInformationDto.xasUpsBillingOptionDefault = dataTable.Rows[i].Field<string>("xasUpsBillingOptionDefault");
				eRPShippingMethodInformationDto.xasUpsCodFundsCode = dataTable.Rows[i].Field<string>("xasUpsCodFundsCode");
				eRPShippingMethodInformationDto.xasUpsCostCenter = dataTable.Rows[i].Field<string>("xasUpsCostCenter");
				eRPShippingMethodInformationDto.xasUpsPackageType = dataTable.Rows[i].Field<string>("xasUpsPackageType");
				eRPShippingMethodInformationDto.xasUpsServiceType = dataTable.Rows[i].Field<string>("xasUpsServiceType");
				eRPShippingMethodInformationDto.xasUpsWsBillingOption = dataTable.Rows[i].Field<string>("xasUpsWsBillingOption");
				eRPShippingMethodInformationDto.xasUpsWSPackageType = dataTable.Rows[i].Field<string>("xasUpsWSPackageType");
				eRPShippingMethodInformationDto.xasUpsWSServiceType = dataTable.Rows[i].Field<string>("xasUpsWSServiceType");
				eRPShippingMethodInformationDto.xasUSPSEndorsement = dataTable.Rows[i].Field<string>("xasUSPSEndorsement");
				eRPShippingMethodInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShippingMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShippingMethodInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShippingMethodInformationDto> GetShippingMethod(Guid shippingMethodId)
	{
		ERPShippingMethodInformationDto eRPShippingMethodInformationDto = new ERPShippingMethodInformationDto();
		InitializeParameterLists();
		string[] collection = new string[63]
		{
			"xasAvalaraTaxCodeID", "xasCarrier", "xasCarrierAccountNumber", "xasShippingMethodID", "xasCreatedBy", "xasCreatedDate", "xasDescription", "xasDistributeCostsOption", "xasUniqueID", "xasFdxAccessibility",
			"xasFdxCodCollectionType", "xasFdxDropOffType", "xasFdxHomeDeliveryType", "xasFdxPackageType", "xasFdxRateElementBasis", "xasFdxRateRequestType", "xasFdxRateTypeBasis", "xasFdxReturnShipIndicator", "xasFdxService", "xasFdxSignatureOption",
			"xasFdxVHCAmountOrPercentage", "xasFdxVHCLevel", "xasFdxVHCType", "xasFedExBillingOption", "xasInactiveDate", "xasInactive", "xasFdxCertificateOfOrigin", "xasFdxCod", "xasFdxCommercialInvoice", "xasFdxExportDeclaration",
			"xasFdxHoldAtLocation", "xasFdxInsideDelivery", "xasFdxInsidePickup", "xasFdxNAFTACO", "xasFdxNonStandardContainer", "xasFdxReturnInstructions", "xasFdxSaturdayDelivery", "xasFdxSaturdayPickup", "xasUpsCertificateOfOrigin", "xasUpsCod",
			"xasUpsCommercialInvoice", "xasUpsNAFTACO", "xasUpsPackingList", "xasUpsPartialInvoice", "xasUpsSaturdayDelivery", "xasUpsUseInterface", "xasReferenceTrackingLink", "xasRowVersion", "xasSecondTaxCodeID", "xasShipChargeWeb",
			"xasShippingPaymentTypeID", "xasTaxCodeID", "xasTaxStatus", "xasTrackingLink", "xasUpsBillingOptionDefault", "xasUpsCodFundsCode", "xasUpsCostCenter", "xasUpsPackageType", "xasUpsServiceType", "xasUpsWsBillingOption",
			"xasUpsWSPackageType", "xasUpsWSServiceType", "xasUSPSEndorsement"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xasUniqueID|C", shippingMethodId);
		AddCustomFieldsToSelectList("ShippingMethods");
		using (DataTable dataTable = GetAsDataTable("ShippingMethods", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShippingMethodInformationDto);
			}
			eRPShippingMethodInformationDto.xasAvalaraTaxCodeID = dataTable.Rows[0].Field<string>("xasAvalaraTaxCodeID");
			eRPShippingMethodInformationDto.xasCarrier = dataTable.Rows[0].Field<string>("xasCarrier");
			eRPShippingMethodInformationDto.xasCarrierAccountNumber = dataTable.Rows[0].Field<string>("xasCarrierAccountNumber");
			eRPShippingMethodInformationDto.xasShippingMethodID = dataTable.Rows[0].Field<string>("xasShippingMethodID");
			eRPShippingMethodInformationDto.xasCreatedBy = dataTable.Rows[0].Field<string>("xasCreatedBy");
			eRPShippingMethodInformationDto.xasCreatedDate = dataTable.Rows[0].Field<DateTime?>("xasCreatedDate");
			eRPShippingMethodInformationDto.xasDescription = dataTable.Rows[0].Field<string>("xasDescription");
			eRPShippingMethodInformationDto.xasDistributeCostsOption = dataTable.Rows[0].Field<byte>("xasDistributeCostsOption");
			eRPShippingMethodInformationDto.xasUniqueID = dataTable.Rows[0].Field<Guid>("xasUniqueID");
			eRPShippingMethodInformationDto.xasFdxAccessibility = dataTable.Rows[0].Field<string>("xasFdxAccessibility");
			eRPShippingMethodInformationDto.xasFdxCodCollectionType = dataTable.Rows[0].Field<string>("xasFdxCodCollectionType");
			eRPShippingMethodInformationDto.xasFdxDropOffType = dataTable.Rows[0].Field<string>("xasFdxDropOffType");
			eRPShippingMethodInformationDto.xasFdxHomeDeliveryType = dataTable.Rows[0].Field<string>("xasFdxHomeDeliveryType");
			eRPShippingMethodInformationDto.xasFdxPackageType = dataTable.Rows[0].Field<string>("xasFdxPackageType");
			eRPShippingMethodInformationDto.xasFdxRateElementBasis = dataTable.Rows[0].Field<string>("xasFdxRateElementBasis");
			eRPShippingMethodInformationDto.xasFdxRateRequestType = dataTable.Rows[0].Field<string>("xasFdxRateRequestType");
			eRPShippingMethodInformationDto.xasFdxRateTypeBasis = dataTable.Rows[0].Field<string>("xasFdxRateTypeBasis");
			eRPShippingMethodInformationDto.xasFdxReturnShipIndicator = dataTable.Rows[0].Field<string>("xasFdxReturnShipIndicator");
			eRPShippingMethodInformationDto.xasFdxService = dataTable.Rows[0].Field<string>("xasFdxService");
			eRPShippingMethodInformationDto.xasFdxSignatureOption = dataTable.Rows[0].Field<string>("xasFdxSignatureOption");
			eRPShippingMethodInformationDto.xasFdxVHCAmountOrPercentage = dataTable.Rows[0].Field<decimal>("xasFdxVHCAmountOrPercentage");
			eRPShippingMethodInformationDto.xasFdxVHCLevel = dataTable.Rows[0].Field<string>("xasFdxVHCLevel");
			eRPShippingMethodInformationDto.xasFdxVHCType = dataTable.Rows[0].Field<string>("xasFdxVHCType");
			eRPShippingMethodInformationDto.xasFedExBillingOption = dataTable.Rows[0].Field<string>("xasFedExBillingOption");
			eRPShippingMethodInformationDto.xasInactiveDate = dataTable.Rows[0].Field<DateTime?>("xasInactiveDate");
			eRPShippingMethodInformationDto.xasInactive = dataTable.Rows[0].Field<bool>("xasInactive");
			eRPShippingMethodInformationDto.xasFdxCertificateOfOrigin = dataTable.Rows[0].Field<bool>("xasFdxCertificateOfOrigin");
			eRPShippingMethodInformationDto.xasFdxCod = dataTable.Rows[0].Field<bool>("xasFdxCod");
			eRPShippingMethodInformationDto.xasFdxCommercialInvoice = dataTable.Rows[0].Field<bool>("xasFdxCommercialInvoice");
			eRPShippingMethodInformationDto.xasFdxExportDeclaration = dataTable.Rows[0].Field<bool>("xasFdxExportDeclaration");
			eRPShippingMethodInformationDto.xasFdxHoldAtLocation = dataTable.Rows[0].Field<bool>("xasFdxHoldAtLocation");
			eRPShippingMethodInformationDto.xasFdxInsideDelivery = dataTable.Rows[0].Field<bool>("xasFdxInsideDelivery");
			eRPShippingMethodInformationDto.xasFdxInsidePickup = dataTable.Rows[0].Field<bool>("xasFdxInsidePickup");
			eRPShippingMethodInformationDto.xasFdxNAFTACO = dataTable.Rows[0].Field<bool>("xasFdxNAFTACO");
			eRPShippingMethodInformationDto.xasFdxNonStandardContainer = dataTable.Rows[0].Field<bool>("xasFdxNonStandardContainer");
			eRPShippingMethodInformationDto.xasFdxReturnInstructions = dataTable.Rows[0].Field<bool>("xasFdxReturnInstructions");
			eRPShippingMethodInformationDto.xasFdxSaturdayDelivery = dataTable.Rows[0].Field<bool>("xasFdxSaturdayDelivery");
			eRPShippingMethodInformationDto.xasFdxSaturdayPickup = dataTable.Rows[0].Field<bool>("xasFdxSaturdayPickup");
			eRPShippingMethodInformationDto.xasUpsCertificateOfOrigin = dataTable.Rows[0].Field<bool>("xasUpsCertificateOfOrigin");
			eRPShippingMethodInformationDto.xasUpsCod = dataTable.Rows[0].Field<bool>("xasUpsCod");
			eRPShippingMethodInformationDto.xasUpsCommercialInvoice = dataTable.Rows[0].Field<bool>("xasUpsCommercialInvoice");
			eRPShippingMethodInformationDto.xasUpsNAFTACO = dataTable.Rows[0].Field<bool>("xasUpsNAFTACO");
			eRPShippingMethodInformationDto.xasUpsPackingList = dataTable.Rows[0].Field<bool>("xasUpsPackingList");
			eRPShippingMethodInformationDto.xasUpsPartialInvoice = dataTable.Rows[0].Field<bool>("xasUpsPartialInvoice");
			eRPShippingMethodInformationDto.xasUpsSaturdayDelivery = dataTable.Rows[0].Field<bool>("xasUpsSaturdayDelivery");
			eRPShippingMethodInformationDto.xasUpsUseInterface = dataTable.Rows[0].Field<bool>("xasUpsUseInterface");
			eRPShippingMethodInformationDto.xasReferenceTrackingLink = dataTable.Rows[0].Field<string>("xasReferenceTrackingLink");
			eRPShippingMethodInformationDto.xasRowVersion = dataTable.Rows[0].Field<byte[]>("xasRowVersion");
			eRPShippingMethodInformationDto.xasSecondTaxCodeID = dataTable.Rows[0].Field<string>("xasSecondTaxCodeID");
			eRPShippingMethodInformationDto.xasShipChargeWeb = dataTable.Rows[0].Field<decimal>("xasShipChargeWeb");
			eRPShippingMethodInformationDto.xasShippingPaymentTypeID = dataTable.Rows[0].Field<string>("xasShippingPaymentTypeID");
			eRPShippingMethodInformationDto.xasTaxCodeID = dataTable.Rows[0].Field<string>("xasTaxCodeID");
			eRPShippingMethodInformationDto.xasTaxStatus = dataTable.Rows[0].Field<byte>("xasTaxStatus");
			eRPShippingMethodInformationDto.xasTrackingLink = dataTable.Rows[0].Field<string>("xasTrackingLink");
			eRPShippingMethodInformationDto.xasUpsBillingOptionDefault = dataTable.Rows[0].Field<string>("xasUpsBillingOptionDefault");
			eRPShippingMethodInformationDto.xasUpsCodFundsCode = dataTable.Rows[0].Field<string>("xasUpsCodFundsCode");
			eRPShippingMethodInformationDto.xasUpsCostCenter = dataTable.Rows[0].Field<string>("xasUpsCostCenter");
			eRPShippingMethodInformationDto.xasUpsPackageType = dataTable.Rows[0].Field<string>("xasUpsPackageType");
			eRPShippingMethodInformationDto.xasUpsServiceType = dataTable.Rows[0].Field<string>("xasUpsServiceType");
			eRPShippingMethodInformationDto.xasUpsWsBillingOption = dataTable.Rows[0].Field<string>("xasUpsWsBillingOption");
			eRPShippingMethodInformationDto.xasUpsWSPackageType = dataTable.Rows[0].Field<string>("xasUpsWSPackageType");
			eRPShippingMethodInformationDto.xasUpsWSServiceType = dataTable.Rows[0].Field<string>("xasUpsWSServiceType");
			eRPShippingMethodInformationDto.xasUSPSEndorsement = dataTable.Rows[0].Field<string>("xasUSPSEndorsement");
			eRPShippingMethodInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShippingMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShippingMethodInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShippingMethod(ERPShippingMethodDto shippingMethod)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShippingMethods WHERE xasUniqueID = " + M1Util.ConvertToLinq(shippingMethod.xasUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xasShippingMethodID"] = shippingMethod.xasShippingMethodID.ToUpper();
				shippingMethod.xasUniqueID = ((shippingMethod.xasUniqueID == Guid.Empty) ? Guid.NewGuid() : shippingMethod.xasUniqueID);
				dataRow["xasUniqueID"] = shippingMethod.xasUniqueID;
				dataRow["xasCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xasCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShippingMethod could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shippingMethod.xasRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShippingMethod is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xasRowVersion"], shippingMethod.xasRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShippingMethod has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShippingMethod again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xasAvalaraTaxCodeID"] = shippingMethod.xasAvalaraTaxCodeID;
			dataRow["xasCarrier"] = shippingMethod.xasCarrier;
			dataRow["xasCarrierAccountNumber"] = shippingMethod.xasCarrierAccountNumber;
			dataRow["xasDescription"] = shippingMethod.xasDescription;
			dataRow["xasDistributeCostsOption"] = shippingMethod.xasDistributeCostsOption;
			dataRow["xasFdxAccessibility"] = shippingMethod.xasFdxAccessibility;
			dataRow["xasFdxCodCollectionType"] = shippingMethod.xasFdxCodCollectionType;
			dataRow["xasFdxDropOffType"] = shippingMethod.xasFdxDropOffType;
			dataRow["xasFdxHomeDeliveryType"] = shippingMethod.xasFdxHomeDeliveryType;
			dataRow["xasFdxPackageType"] = shippingMethod.xasFdxPackageType;
			dataRow["xasFdxRateElementBasis"] = shippingMethod.xasFdxRateElementBasis;
			dataRow["xasFdxRateRequestType"] = shippingMethod.xasFdxRateRequestType;
			dataRow["xasFdxRateTypeBasis"] = shippingMethod.xasFdxRateTypeBasis;
			dataRow["xasFdxReturnShipIndicator"] = shippingMethod.xasFdxReturnShipIndicator;
			dataRow["xasFdxService"] = shippingMethod.xasFdxService;
			dataRow["xasFdxSignatureOption"] = shippingMethod.xasFdxSignatureOption;
			dataRow["xasFdxVHCAmountOrPercentage"] = shippingMethod.xasFdxVHCAmountOrPercentage;
			dataRow["xasFdxVHCLevel"] = shippingMethod.xasFdxVHCLevel;
			dataRow["xasFdxVHCType"] = shippingMethod.xasFdxVHCType;
			dataRow["xasFedExBillingOption"] = shippingMethod.xasFedExBillingOption;
			DataRow dataRow2 = dataRow;
			DateTime? xasInactiveDate = shippingMethod.xasInactiveDate;
			dataRow2["xasInactiveDate"] = (xasInactiveDate.HasValue ? ((object)xasInactiveDate.GetValueOrDefault()) : dataRow["xasInactiveDate"]);
			dataRow["xasInactive"] = shippingMethod.xasInactive;
			dataRow["xasFdxCertificateOfOrigin"] = shippingMethod.xasFdxCertificateOfOrigin;
			dataRow["xasFdxCod"] = shippingMethod.xasFdxCod;
			dataRow["xasFdxCommercialInvoice"] = shippingMethod.xasFdxCommercialInvoice;
			dataRow["xasFdxExportDeclaration"] = shippingMethod.xasFdxExportDeclaration;
			dataRow["xasFdxHoldAtLocation"] = shippingMethod.xasFdxHoldAtLocation;
			dataRow["xasFdxInsideDelivery"] = shippingMethod.xasFdxInsideDelivery;
			dataRow["xasFdxInsidePickup"] = shippingMethod.xasFdxInsidePickup;
			dataRow["xasFdxNAFTACO"] = shippingMethod.xasFdxNAFTACO;
			dataRow["xasFdxNonStandardContainer"] = shippingMethod.xasFdxNonStandardContainer;
			dataRow["xasFdxReturnInstructions"] = shippingMethod.xasFdxReturnInstructions;
			dataRow["xasFdxSaturdayDelivery"] = shippingMethod.xasFdxSaturdayDelivery;
			dataRow["xasFdxSaturdayPickup"] = shippingMethod.xasFdxSaturdayPickup;
			dataRow["xasUpsCertificateOfOrigin"] = shippingMethod.xasUpsCertificateOfOrigin;
			dataRow["xasUpsCod"] = shippingMethod.xasUpsCod;
			dataRow["xasUpsCommercialInvoice"] = shippingMethod.xasUpsCommercialInvoice;
			dataRow["xasUpsNAFTACO"] = shippingMethod.xasUpsNAFTACO;
			dataRow["xasUpsPackingList"] = shippingMethod.xasUpsPackingList;
			dataRow["xasUpsPartialInvoice"] = shippingMethod.xasUpsPartialInvoice;
			dataRow["xasUpsSaturdayDelivery"] = shippingMethod.xasUpsSaturdayDelivery;
			dataRow["xasUpsUseInterface"] = shippingMethod.xasUpsUseInterface;
			dataRow["xasReferenceTrackingLink"] = shippingMethod.xasReferenceTrackingLink ?? dataRow["xasReferenceTrackingLink"];
			dataRow["xasSecondTaxCodeID"] = shippingMethod.xasSecondTaxCodeID;
			dataRow["xasShipChargeWeb"] = shippingMethod.xasShipChargeWeb;
			dataRow["xasShippingPaymentTypeID"] = shippingMethod.xasShippingPaymentTypeID;
			dataRow["xasTaxCodeID"] = shippingMethod.xasTaxCodeID;
			dataRow["xasTaxStatus"] = shippingMethod.xasTaxStatus;
			dataRow["xasTrackingLink"] = shippingMethod.xasTrackingLink ?? dataRow["xasTrackingLink"];
			dataRow["xasUpsBillingOptionDefault"] = shippingMethod.xasUpsBillingOptionDefault;
			dataRow["xasUpsCodFundsCode"] = shippingMethod.xasUpsCodFundsCode;
			dataRow["xasUpsCostCenter"] = shippingMethod.xasUpsCostCenter;
			dataRow["xasUpsPackageType"] = shippingMethod.xasUpsPackageType;
			dataRow["xasUpsServiceType"] = shippingMethod.xasUpsServiceType;
			dataRow["xasUpsWsBillingOption"] = shippingMethod.xasUpsWsBillingOption;
			dataRow["xasUpsWSPackageType"] = shippingMethod.xasUpsWSPackageType;
			dataRow["xasUpsWSServiceType"] = shippingMethod.xasUpsWSServiceType;
			dataRow["xasUSPSEndorsement"] = shippingMethod.xasUSPSEndorsement;
			if (shippingMethod.CustomFields != null && shippingMethod.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shippingMethod.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShippingMethod [{shippingMethod.xasUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShippingMethod [{shippingMethod.xasUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
