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

public class ERPAPInvoiceLineRepository : APIBaseRepository, IERPAPInvoiceLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPInvoiceLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPInvoiceLineExist(Guid aPInvoiceLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("aplUniqueID|C", aPInvoiceLineId);
		base.selectList.Add("aplUniqueID");
		return Task.FromResult(GetAsObject("APInvoiceLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPInvoiceLineInformationDto>> GetAllAPInvoiceLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPInvoiceLineInformationDto> collection = new List<ERPAPInvoiceLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[63]
		{
			"aplApInvoiceID", "aplAssetID", "aplAssetTypeID", "aplConversionFactor", "aplCreatedBy", "aplCreatedDate", "aplDmrClaimID", "aplDmrClaimLineID", "aplDmrShipmentID", "aplDmrShipmentLineID",
			"aplUniqueID", "aplExtendedCostBase", "aplExtendedCostForeign", "aplForm1099Box", "aplInvoicedComplete", "aplPostedToGl", "aplRetention", "aplItemType", "aplJobAssemblyID", "aplJobID",
			"aplJobMaterialID", "aplJobOperationID", "aplJobType", "aplLandedCostChargeID", "aplLandedCostID", "aplNonTaxReasonID", "aplOrgPartID", "aplOrgPartShortDescription", "aplPartDescription", "aplPartID",
			"aplPartLongDescriptionRtf", "aplPartLongDescriptionText", "aplPartRevisionID", "aplProjectAreaID", "aplProjectID", "aplPurchaseOrderID", "aplPurchaseOrderLineID", "aplPurchaseQuantity", "aplPurchaseUnitCostBase", "aplPurchaseUnitCostForeign",
			"aplPurchaseUnitOfMeasure", "aplReceiptID", "aplReceiptLineID", "aplReceivedQuantity", "aplReceivedUnitOfMeasure", "aplRetentionAmountBase", "aplRetentionAmountForeign", "aplRetentionPercent", "aplRetentionReleaseDate", "aplRmaClaimID",
			"aplRmaClaimLineID", "aplRowVersion", "aplSecondTaxAmountBase", "aplSecondTaxAmountForeign", "aplSecondTaxCodeID", "aplApInvoiceLineID", "aplSetupChargeBase", "aplSetupChargeForeign", "aplTaxAmountBase", "aplTaxAmountForeign",
			"aplTaxCodeID", "aplTotalExtendedCostBase", "aplTotalExtendedCostForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APInvoiceLines");
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
		using (DataTable dataTable = GetAsDataTable("APInvoiceLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPInvoiceLineInformationDto eRPAPInvoiceLineInformationDto = new ERPAPInvoiceLineInformationDto();
				eRPAPInvoiceLineInformationDto.aplApInvoiceID = dataTable.Rows[i].Field<string>("aplApInvoiceID");
				eRPAPInvoiceLineInformationDto.aplAssetID = dataTable.Rows[i].Field<string>("aplAssetID");
				eRPAPInvoiceLineInformationDto.aplAssetTypeID = dataTable.Rows[i].Field<string>("aplAssetTypeID");
				eRPAPInvoiceLineInformationDto.aplConversionFactor = dataTable.Rows[i].Field<decimal>("aplConversionFactor");
				eRPAPInvoiceLineInformationDto.aplCreatedBy = dataTable.Rows[i].Field<string>("aplCreatedBy");
				eRPAPInvoiceLineInformationDto.aplCreatedDate = dataTable.Rows[i].Field<DateTime?>("aplCreatedDate");
				eRPAPInvoiceLineInformationDto.aplDmrClaimID = dataTable.Rows[i].Field<string>("aplDmrClaimID");
				eRPAPInvoiceLineInformationDto.aplDmrClaimLineID = dataTable.Rows[i].Field<short>("aplDmrClaimLineID");
				eRPAPInvoiceLineInformationDto.aplDmrShipmentID = dataTable.Rows[i].Field<string>("aplDmrShipmentID");
				eRPAPInvoiceLineInformationDto.aplDmrShipmentLineID = dataTable.Rows[i].Field<short>("aplDmrShipmentLineID");
				eRPAPInvoiceLineInformationDto.aplUniqueID = dataTable.Rows[i].Field<Guid>("aplUniqueID");
				eRPAPInvoiceLineInformationDto.aplExtendedCostBase = dataTable.Rows[i].Field<decimal>("aplExtendedCostBase");
				eRPAPInvoiceLineInformationDto.aplExtendedCostForeign = dataTable.Rows[i].Field<decimal>("aplExtendedCostForeign");
				eRPAPInvoiceLineInformationDto.aplForm1099Box = dataTable.Rows[i].Field<byte>("aplForm1099Box");
				eRPAPInvoiceLineInformationDto.aplInvoicedComplete = dataTable.Rows[i].Field<bool>("aplInvoicedComplete");
				eRPAPInvoiceLineInformationDto.aplPostedToGl = dataTable.Rows[i].Field<bool>("aplPostedToGl");
				eRPAPInvoiceLineInformationDto.aplRetention = dataTable.Rows[i].Field<bool>("aplRetention");
				eRPAPInvoiceLineInformationDto.aplItemType = dataTable.Rows[i].Field<string>("aplItemType");
				eRPAPInvoiceLineInformationDto.aplJobAssemblyID = dataTable.Rows[i].Field<int>("aplJobAssemblyID");
				eRPAPInvoiceLineInformationDto.aplJobID = dataTable.Rows[i].Field<string>("aplJobID");
				eRPAPInvoiceLineInformationDto.aplJobMaterialID = dataTable.Rows[i].Field<int>("aplJobMaterialID");
				eRPAPInvoiceLineInformationDto.aplJobOperationID = dataTable.Rows[i].Field<int>("aplJobOperationID");
				eRPAPInvoiceLineInformationDto.aplJobType = dataTable.Rows[i].Field<byte>("aplJobType");
				eRPAPInvoiceLineInformationDto.aplLandedCostChargeID = dataTable.Rows[i].Field<short>("aplLandedCostChargeID");
				eRPAPInvoiceLineInformationDto.aplLandedCostID = dataTable.Rows[i].Field<string>("aplLandedCostID");
				eRPAPInvoiceLineInformationDto.aplNonTaxReasonID = dataTable.Rows[i].Field<string>("aplNonTaxReasonID");
				eRPAPInvoiceLineInformationDto.aplOrgPartID = dataTable.Rows[i].Field<string>("aplOrgPartID");
				eRPAPInvoiceLineInformationDto.aplOrgPartShortDescription = dataTable.Rows[i].Field<string>("aplOrgPartShortDescription");
				eRPAPInvoiceLineInformationDto.aplPartDescription = dataTable.Rows[i].Field<string>("aplPartDescription");
				eRPAPInvoiceLineInformationDto.aplPartID = dataTable.Rows[i].Field<string>("aplPartID");
				eRPAPInvoiceLineInformationDto.aplPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("aplPartLongDescriptionRtf");
				eRPAPInvoiceLineInformationDto.aplPartLongDescriptionText = dataTable.Rows[i].Field<string>("aplPartLongDescriptionText");
				eRPAPInvoiceLineInformationDto.aplPartRevisionID = dataTable.Rows[i].Field<string>("aplPartRevisionID");
				eRPAPInvoiceLineInformationDto.aplProjectAreaID = dataTable.Rows[i].Field<string>("aplProjectAreaID");
				eRPAPInvoiceLineInformationDto.aplProjectID = dataTable.Rows[i].Field<string>("aplProjectID");
				eRPAPInvoiceLineInformationDto.aplPurchaseOrderID = dataTable.Rows[i].Field<string>("aplPurchaseOrderID");
				eRPAPInvoiceLineInformationDto.aplPurchaseOrderLineID = dataTable.Rows[i].Field<short>("aplPurchaseOrderLineID");
				eRPAPInvoiceLineInformationDto.aplPurchaseQuantity = dataTable.Rows[i].Field<decimal>("aplPurchaseQuantity");
				eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostBase = dataTable.Rows[i].Field<decimal>("aplPurchaseUnitCostBase");
				eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("aplPurchaseUnitCostForeign");
				eRPAPInvoiceLineInformationDto.aplPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("aplPurchaseUnitOfMeasure");
				eRPAPInvoiceLineInformationDto.aplReceiptID = dataTable.Rows[i].Field<string>("aplReceiptID");
				eRPAPInvoiceLineInformationDto.aplReceiptLineID = dataTable.Rows[i].Field<short>("aplReceiptLineID");
				eRPAPInvoiceLineInformationDto.aplReceivedQuantity = dataTable.Rows[i].Field<decimal>("aplReceivedQuantity");
				eRPAPInvoiceLineInformationDto.aplReceivedUnitOfMeasure = dataTable.Rows[i].Field<string>("aplReceivedUnitOfMeasure");
				eRPAPInvoiceLineInformationDto.aplRetentionAmountBase = dataTable.Rows[i].Field<decimal>("aplRetentionAmountBase");
				eRPAPInvoiceLineInformationDto.aplRetentionAmountForeign = dataTable.Rows[i].Field<decimal>("aplRetentionAmountForeign");
				eRPAPInvoiceLineInformationDto.aplRetentionPercent = dataTable.Rows[i].Field<decimal>("aplRetentionPercent");
				eRPAPInvoiceLineInformationDto.aplRetentionReleaseDate = dataTable.Rows[i].Field<DateTime?>("aplRetentionReleaseDate");
				eRPAPInvoiceLineInformationDto.aplRmaClaimID = dataTable.Rows[i].Field<string>("aplRmaClaimID");
				eRPAPInvoiceLineInformationDto.aplRmaClaimLineID = dataTable.Rows[i].Field<short>("aplRmaClaimLineID");
				eRPAPInvoiceLineInformationDto.aplRowVersion = dataTable.Rows[i].Field<byte[]>("aplRowVersion");
				eRPAPInvoiceLineInformationDto.aplSecondTaxAmountBase = dataTable.Rows[i].Field<decimal>("aplSecondTaxAmountBase");
				eRPAPInvoiceLineInformationDto.aplSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("aplSecondTaxAmountForeign");
				eRPAPInvoiceLineInformationDto.aplSecondTaxCodeID = dataTable.Rows[i].Field<string>("aplSecondTaxCodeID");
				eRPAPInvoiceLineInformationDto.aplApInvoiceLineID = dataTable.Rows[i].Field<short>("aplApInvoiceLineID");
				eRPAPInvoiceLineInformationDto.aplSetupChargeBase = dataTable.Rows[i].Field<decimal>("aplSetupChargeBase");
				eRPAPInvoiceLineInformationDto.aplSetupChargeForeign = dataTable.Rows[i].Field<decimal>("aplSetupChargeForeign");
				eRPAPInvoiceLineInformationDto.aplTaxAmountBase = dataTable.Rows[i].Field<decimal>("aplTaxAmountBase");
				eRPAPInvoiceLineInformationDto.aplTaxAmountForeign = dataTable.Rows[i].Field<decimal>("aplTaxAmountForeign");
				eRPAPInvoiceLineInformationDto.aplTaxCodeID = dataTable.Rows[i].Field<string>("aplTaxCodeID");
				eRPAPInvoiceLineInformationDto.aplTotalExtendedCostBase = dataTable.Rows[i].Field<decimal>("aplTotalExtendedCostBase");
				eRPAPInvoiceLineInformationDto.aplTotalExtendedCostForeign = dataTable.Rows[i].Field<decimal>("aplTotalExtendedCostForeign");
				eRPAPInvoiceLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPInvoiceLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPInvoiceLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPInvoiceLineInformationDto> GetAPInvoiceLine(Guid aPInvoiceLineId)
	{
		ERPAPInvoiceLineInformationDto eRPAPInvoiceLineInformationDto = new ERPAPInvoiceLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[63]
		{
			"aplApInvoiceID", "aplAssetID", "aplAssetTypeID", "aplConversionFactor", "aplCreatedBy", "aplCreatedDate", "aplDmrClaimID", "aplDmrClaimLineID", "aplDmrShipmentID", "aplDmrShipmentLineID",
			"aplUniqueID", "aplExtendedCostBase", "aplExtendedCostForeign", "aplForm1099Box", "aplInvoicedComplete", "aplPostedToGl", "aplRetention", "aplItemType", "aplJobAssemblyID", "aplJobID",
			"aplJobMaterialID", "aplJobOperationID", "aplJobType", "aplLandedCostChargeID", "aplLandedCostID", "aplNonTaxReasonID", "aplOrgPartID", "aplOrgPartShortDescription", "aplPartDescription", "aplPartID",
			"aplPartLongDescriptionRtf", "aplPartLongDescriptionText", "aplPartRevisionID", "aplProjectAreaID", "aplProjectID", "aplPurchaseOrderID", "aplPurchaseOrderLineID", "aplPurchaseQuantity", "aplPurchaseUnitCostBase", "aplPurchaseUnitCostForeign",
			"aplPurchaseUnitOfMeasure", "aplReceiptID", "aplReceiptLineID", "aplReceivedQuantity", "aplReceivedUnitOfMeasure", "aplRetentionAmountBase", "aplRetentionAmountForeign", "aplRetentionPercent", "aplRetentionReleaseDate", "aplRmaClaimID",
			"aplRmaClaimLineID", "aplRowVersion", "aplSecondTaxAmountBase", "aplSecondTaxAmountForeign", "aplSecondTaxCodeID", "aplApInvoiceLineID", "aplSetupChargeBase", "aplSetupChargeForeign", "aplTaxAmountBase", "aplTaxAmountForeign",
			"aplTaxCodeID", "aplTotalExtendedCostBase", "aplTotalExtendedCostForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("aplUniqueID|C", aPInvoiceLineId);
		AddCustomFieldsToSelectList("APInvoiceLines");
		using (DataTable dataTable = GetAsDataTable("APInvoiceLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPInvoiceLineInformationDto);
			}
			eRPAPInvoiceLineInformationDto.aplApInvoiceID = dataTable.Rows[0].Field<string>("aplApInvoiceID");
			eRPAPInvoiceLineInformationDto.aplAssetID = dataTable.Rows[0].Field<string>("aplAssetID");
			eRPAPInvoiceLineInformationDto.aplAssetTypeID = dataTable.Rows[0].Field<string>("aplAssetTypeID");
			eRPAPInvoiceLineInformationDto.aplConversionFactor = dataTable.Rows[0].Field<decimal>("aplConversionFactor");
			eRPAPInvoiceLineInformationDto.aplCreatedBy = dataTable.Rows[0].Field<string>("aplCreatedBy");
			eRPAPInvoiceLineInformationDto.aplCreatedDate = dataTable.Rows[0].Field<DateTime?>("aplCreatedDate");
			eRPAPInvoiceLineInformationDto.aplDmrClaimID = dataTable.Rows[0].Field<string>("aplDmrClaimID");
			eRPAPInvoiceLineInformationDto.aplDmrClaimLineID = dataTable.Rows[0].Field<short>("aplDmrClaimLineID");
			eRPAPInvoiceLineInformationDto.aplDmrShipmentID = dataTable.Rows[0].Field<string>("aplDmrShipmentID");
			eRPAPInvoiceLineInformationDto.aplDmrShipmentLineID = dataTable.Rows[0].Field<short>("aplDmrShipmentLineID");
			eRPAPInvoiceLineInformationDto.aplUniqueID = dataTable.Rows[0].Field<Guid>("aplUniqueID");
			eRPAPInvoiceLineInformationDto.aplExtendedCostBase = dataTable.Rows[0].Field<decimal>("aplExtendedCostBase");
			eRPAPInvoiceLineInformationDto.aplExtendedCostForeign = dataTable.Rows[0].Field<decimal>("aplExtendedCostForeign");
			eRPAPInvoiceLineInformationDto.aplForm1099Box = dataTable.Rows[0].Field<byte>("aplForm1099Box");
			eRPAPInvoiceLineInformationDto.aplInvoicedComplete = dataTable.Rows[0].Field<bool>("aplInvoicedComplete");
			eRPAPInvoiceLineInformationDto.aplPostedToGl = dataTable.Rows[0].Field<bool>("aplPostedToGl");
			eRPAPInvoiceLineInformationDto.aplRetention = dataTable.Rows[0].Field<bool>("aplRetention");
			eRPAPInvoiceLineInformationDto.aplItemType = dataTable.Rows[0].Field<string>("aplItemType");
			eRPAPInvoiceLineInformationDto.aplJobAssemblyID = dataTable.Rows[0].Field<int>("aplJobAssemblyID");
			eRPAPInvoiceLineInformationDto.aplJobID = dataTable.Rows[0].Field<string>("aplJobID");
			eRPAPInvoiceLineInformationDto.aplJobMaterialID = dataTable.Rows[0].Field<int>("aplJobMaterialID");
			eRPAPInvoiceLineInformationDto.aplJobOperationID = dataTable.Rows[0].Field<int>("aplJobOperationID");
			eRPAPInvoiceLineInformationDto.aplJobType = dataTable.Rows[0].Field<byte>("aplJobType");
			eRPAPInvoiceLineInformationDto.aplLandedCostChargeID = dataTable.Rows[0].Field<short>("aplLandedCostChargeID");
			eRPAPInvoiceLineInformationDto.aplLandedCostID = dataTable.Rows[0].Field<string>("aplLandedCostID");
			eRPAPInvoiceLineInformationDto.aplNonTaxReasonID = dataTable.Rows[0].Field<string>("aplNonTaxReasonID");
			eRPAPInvoiceLineInformationDto.aplOrgPartID = dataTable.Rows[0].Field<string>("aplOrgPartID");
			eRPAPInvoiceLineInformationDto.aplOrgPartShortDescription = dataTable.Rows[0].Field<string>("aplOrgPartShortDescription");
			eRPAPInvoiceLineInformationDto.aplPartDescription = dataTable.Rows[0].Field<string>("aplPartDescription");
			eRPAPInvoiceLineInformationDto.aplPartID = dataTable.Rows[0].Field<string>("aplPartID");
			eRPAPInvoiceLineInformationDto.aplPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("aplPartLongDescriptionRtf");
			eRPAPInvoiceLineInformationDto.aplPartLongDescriptionText = dataTable.Rows[0].Field<string>("aplPartLongDescriptionText");
			eRPAPInvoiceLineInformationDto.aplPartRevisionID = dataTable.Rows[0].Field<string>("aplPartRevisionID");
			eRPAPInvoiceLineInformationDto.aplProjectAreaID = dataTable.Rows[0].Field<string>("aplProjectAreaID");
			eRPAPInvoiceLineInformationDto.aplProjectID = dataTable.Rows[0].Field<string>("aplProjectID");
			eRPAPInvoiceLineInformationDto.aplPurchaseOrderID = dataTable.Rows[0].Field<string>("aplPurchaseOrderID");
			eRPAPInvoiceLineInformationDto.aplPurchaseOrderLineID = dataTable.Rows[0].Field<short>("aplPurchaseOrderLineID");
			eRPAPInvoiceLineInformationDto.aplPurchaseQuantity = dataTable.Rows[0].Field<decimal>("aplPurchaseQuantity");
			eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostBase = dataTable.Rows[0].Field<decimal>("aplPurchaseUnitCostBase");
			eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("aplPurchaseUnitCostForeign");
			eRPAPInvoiceLineInformationDto.aplPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("aplPurchaseUnitOfMeasure");
			eRPAPInvoiceLineInformationDto.aplReceiptID = dataTable.Rows[0].Field<string>("aplReceiptID");
			eRPAPInvoiceLineInformationDto.aplReceiptLineID = dataTable.Rows[0].Field<short>("aplReceiptLineID");
			eRPAPInvoiceLineInformationDto.aplReceivedQuantity = dataTable.Rows[0].Field<decimal>("aplReceivedQuantity");
			eRPAPInvoiceLineInformationDto.aplReceivedUnitOfMeasure = dataTable.Rows[0].Field<string>("aplReceivedUnitOfMeasure");
			eRPAPInvoiceLineInformationDto.aplRetentionAmountBase = dataTable.Rows[0].Field<decimal>("aplRetentionAmountBase");
			eRPAPInvoiceLineInformationDto.aplRetentionAmountForeign = dataTable.Rows[0].Field<decimal>("aplRetentionAmountForeign");
			eRPAPInvoiceLineInformationDto.aplRetentionPercent = dataTable.Rows[0].Field<decimal>("aplRetentionPercent");
			eRPAPInvoiceLineInformationDto.aplRetentionReleaseDate = dataTable.Rows[0].Field<DateTime?>("aplRetentionReleaseDate");
			eRPAPInvoiceLineInformationDto.aplRmaClaimID = dataTable.Rows[0].Field<string>("aplRmaClaimID");
			eRPAPInvoiceLineInformationDto.aplRmaClaimLineID = dataTable.Rows[0].Field<short>("aplRmaClaimLineID");
			eRPAPInvoiceLineInformationDto.aplRowVersion = dataTable.Rows[0].Field<byte[]>("aplRowVersion");
			eRPAPInvoiceLineInformationDto.aplSecondTaxAmountBase = dataTable.Rows[0].Field<decimal>("aplSecondTaxAmountBase");
			eRPAPInvoiceLineInformationDto.aplSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("aplSecondTaxAmountForeign");
			eRPAPInvoiceLineInformationDto.aplSecondTaxCodeID = dataTable.Rows[0].Field<string>("aplSecondTaxCodeID");
			eRPAPInvoiceLineInformationDto.aplApInvoiceLineID = dataTable.Rows[0].Field<short>("aplApInvoiceLineID");
			eRPAPInvoiceLineInformationDto.aplSetupChargeBase = dataTable.Rows[0].Field<decimal>("aplSetupChargeBase");
			eRPAPInvoiceLineInformationDto.aplSetupChargeForeign = dataTable.Rows[0].Field<decimal>("aplSetupChargeForeign");
			eRPAPInvoiceLineInformationDto.aplTaxAmountBase = dataTable.Rows[0].Field<decimal>("aplTaxAmountBase");
			eRPAPInvoiceLineInformationDto.aplTaxAmountForeign = dataTable.Rows[0].Field<decimal>("aplTaxAmountForeign");
			eRPAPInvoiceLineInformationDto.aplTaxCodeID = dataTable.Rows[0].Field<string>("aplTaxCodeID");
			eRPAPInvoiceLineInformationDto.aplTotalExtendedCostBase = dataTable.Rows[0].Field<decimal>("aplTotalExtendedCostBase");
			eRPAPInvoiceLineInformationDto.aplTotalExtendedCostForeign = dataTable.Rows[0].Field<decimal>("aplTotalExtendedCostForeign");
			eRPAPInvoiceLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPInvoiceLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPInvoiceLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPInvoiceLine(ERPAPInvoiceLineDto aPInvoiceLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APInvoiceLines WHERE aplUniqueID = " + M1Util.ConvertToLinq(aPInvoiceLine.aplUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["aplApInvoiceID"] = aPInvoiceLine.aplApInvoiceID.ToUpper();
				dataRow["aplApInvoiceLineID"] = aPInvoiceLine.aplApInvoiceLineID;
				aPInvoiceLine.aplUniqueID = ((aPInvoiceLine.aplUniqueID == Guid.Empty) ? Guid.NewGuid() : aPInvoiceLine.aplUniqueID);
				dataRow["aplUniqueID"] = aPInvoiceLine.aplUniqueID;
				dataRow["aplCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["aplCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APInvoiceLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPInvoiceLine.aplRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APInvoiceLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["aplRowVersion"], aPInvoiceLine.aplRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APInvoiceLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APInvoiceLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["aplAssetID"] = aPInvoiceLine.aplAssetID;
			dataRow["aplAssetTypeID"] = aPInvoiceLine.aplAssetTypeID;
			dataRow["aplConversionFactor"] = aPInvoiceLine.aplConversionFactor;
			dataRow["aplDmrClaimID"] = aPInvoiceLine.aplDmrClaimID;
			dataRow["aplDmrClaimLineID"] = aPInvoiceLine.aplDmrClaimLineID;
			dataRow["aplDmrShipmentID"] = aPInvoiceLine.aplDmrShipmentID;
			dataRow["aplDmrShipmentLineID"] = aPInvoiceLine.aplDmrShipmentLineID;
			dataRow["aplExtendedCostBase"] = aPInvoiceLine.aplExtendedCostBase;
			dataRow["aplExtendedCostForeign"] = aPInvoiceLine.aplExtendedCostForeign;
			dataRow["aplForm1099Box"] = aPInvoiceLine.aplForm1099Box;
			dataRow["aplInvoicedComplete"] = aPInvoiceLine.aplInvoicedComplete;
			dataRow["aplPostedToGl"] = aPInvoiceLine.aplPostedToGl;
			dataRow["aplRetention"] = aPInvoiceLine.aplRetention;
			dataRow["aplItemType"] = aPInvoiceLine.aplItemType;
			dataRow["aplJobAssemblyID"] = aPInvoiceLine.aplJobAssemblyID;
			dataRow["aplJobID"] = aPInvoiceLine.aplJobID;
			dataRow["aplJobMaterialID"] = aPInvoiceLine.aplJobMaterialID;
			dataRow["aplJobOperationID"] = aPInvoiceLine.aplJobOperationID;
			dataRow["aplJobType"] = aPInvoiceLine.aplJobType;
			dataRow["aplLandedCostChargeID"] = aPInvoiceLine.aplLandedCostChargeID;
			dataRow["aplLandedCostID"] = aPInvoiceLine.aplLandedCostID;
			dataRow["aplNonTaxReasonID"] = aPInvoiceLine.aplNonTaxReasonID;
			dataRow["aplOrgPartID"] = aPInvoiceLine.aplOrgPartID;
			dataRow["aplOrgPartShortDescription"] = aPInvoiceLine.aplOrgPartShortDescription;
			dataRow["aplPartDescription"] = aPInvoiceLine.aplPartDescription;
			dataRow["aplPartID"] = aPInvoiceLine.aplPartID;
			dataRow["aplPartLongDescriptionRtf"] = aPInvoiceLine.aplPartLongDescriptionRtf ?? dataRow["aplPartLongDescriptionRtf"];
			dataRow["aplPartLongDescriptionText"] = aPInvoiceLine.aplPartLongDescriptionText ?? dataRow["aplPartLongDescriptionText"];
			dataRow["aplPartRevisionID"] = aPInvoiceLine.aplPartRevisionID;
			dataRow["aplProjectAreaID"] = aPInvoiceLine.aplProjectAreaID;
			dataRow["aplProjectID"] = aPInvoiceLine.aplProjectID;
			dataRow["aplPurchaseOrderID"] = aPInvoiceLine.aplPurchaseOrderID;
			dataRow["aplPurchaseOrderLineID"] = aPInvoiceLine.aplPurchaseOrderLineID;
			dataRow["aplPurchaseQuantity"] = aPInvoiceLine.aplPurchaseQuantity;
			dataRow["aplPurchaseUnitCostBase"] = aPInvoiceLine.aplPurchaseUnitCostBase;
			dataRow["aplPurchaseUnitCostForeign"] = aPInvoiceLine.aplPurchaseUnitCostForeign;
			dataRow["aplPurchaseUnitOfMeasure"] = aPInvoiceLine.aplPurchaseUnitOfMeasure;
			dataRow["aplReceiptID"] = aPInvoiceLine.aplReceiptID;
			dataRow["aplReceiptLineID"] = aPInvoiceLine.aplReceiptLineID;
			dataRow["aplReceivedQuantity"] = aPInvoiceLine.aplReceivedQuantity;
			dataRow["aplReceivedUnitOfMeasure"] = aPInvoiceLine.aplReceivedUnitOfMeasure;
			dataRow["aplRetentionAmountBase"] = aPInvoiceLine.aplRetentionAmountBase;
			dataRow["aplRetentionAmountForeign"] = aPInvoiceLine.aplRetentionAmountForeign;
			dataRow["aplRetentionPercent"] = aPInvoiceLine.aplRetentionPercent;
			DataRow dataRow2 = dataRow;
			DateTime? aplRetentionReleaseDate = aPInvoiceLine.aplRetentionReleaseDate;
			dataRow2["aplRetentionReleaseDate"] = (aplRetentionReleaseDate.HasValue ? ((object)aplRetentionReleaseDate.GetValueOrDefault()) : dataRow["aplRetentionReleaseDate"]);
			dataRow["aplRmaClaimID"] = aPInvoiceLine.aplRmaClaimID;
			dataRow["aplRmaClaimLineID"] = aPInvoiceLine.aplRmaClaimLineID;
			dataRow["aplSecondTaxAmountBase"] = aPInvoiceLine.aplSecondTaxAmountBase;
			dataRow["aplSecondTaxAmountForeign"] = aPInvoiceLine.aplSecondTaxAmountForeign;
			dataRow["aplSecondTaxCodeID"] = aPInvoiceLine.aplSecondTaxCodeID;
			dataRow["aplSetupChargeBase"] = aPInvoiceLine.aplSetupChargeBase;
			dataRow["aplSetupChargeForeign"] = aPInvoiceLine.aplSetupChargeForeign;
			dataRow["aplTaxAmountBase"] = aPInvoiceLine.aplTaxAmountBase;
			dataRow["aplTaxAmountForeign"] = aPInvoiceLine.aplTaxAmountForeign;
			dataRow["aplTaxCodeID"] = aPInvoiceLine.aplTaxCodeID;
			dataRow["aplTotalExtendedCostBase"] = aPInvoiceLine.aplTotalExtendedCostBase;
			dataRow["aplTotalExtendedCostForeign"] = aPInvoiceLine.aplTotalExtendedCostForeign;
			if (aPInvoiceLine.CustomFields != null && aPInvoiceLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPInvoiceLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APInvoiceLine [{aPInvoiceLine.aplUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APInvoiceLine [{aPInvoiceLine.aplUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
