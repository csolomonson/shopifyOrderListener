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

public class ERPAssetAdjustmentRepository : APIBaseRepository, IERPAssetAdjustmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetAdjustmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetAdjustmentExist(Guid assetAdjustmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("faaUniqueID|C", assetAdjustmentId);
		base.selectList.Add("faaUniqueID");
		return Task.FromResult(GetAsObject("AssetAdjustments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetAdjustmentInformationDto>> GetAllAssetAdjustments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetAdjustmentInformationDto> collection = new List<ERPAssetAdjustmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[33]
		{
			"faaAccumulatedDepreciation", "faaAdjustmentDate", "faaAdjustmentType", "faaArInvoiceContactID", "faaArInvoiceLocationID", "faaAssetID", "faaAuthorizedByEmployeeID", "faaClosingPercent", "faaClosingPeriodDepreciation", "faaCreatedBy",
			"faaCreatedDate", "faaCurrencyRateID", "faaCustomerOrganizationID", "faaDepreciationThisYear", "faaDestinationPlantID", "faaUniqueID", "faaExchangeRate", "faaGlFiscalYearID", "faaGlFiscalYearPeriodID", "faaCustomRate",
			"faaPostedToGl", "faaLongDescriptionRtf", "faaLongDescriptionText", "faaNetAssetValue", "faaOpeningAssetValue", "faaPostedDate", "faaProfitOrLoss", "faaQuantity", "faaRowVersion", "faaAssetAdjustmentID",
			"faaSourcePlantID", "faaValue", "faaValueForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetAdjustments");
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
		using (DataTable dataTable = GetAsDataTable("AssetAdjustments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetAdjustmentInformationDto eRPAssetAdjustmentInformationDto = new ERPAssetAdjustmentInformationDto();
				eRPAssetAdjustmentInformationDto.faaAccumulatedDepreciation = dataTable.Rows[i].Field<decimal>("faaAccumulatedDepreciation");
				eRPAssetAdjustmentInformationDto.faaAdjustmentDate = dataTable.Rows[i].Field<DateTime?>("faaAdjustmentDate");
				eRPAssetAdjustmentInformationDto.faaAdjustmentType = dataTable.Rows[i].Field<string>("faaAdjustmentType");
				eRPAssetAdjustmentInformationDto.faaArInvoiceContactID = dataTable.Rows[i].Field<string>("faaArInvoiceContactID");
				eRPAssetAdjustmentInformationDto.faaArInvoiceLocationID = dataTable.Rows[i].Field<string>("faaArInvoiceLocationID");
				eRPAssetAdjustmentInformationDto.faaAssetID = dataTable.Rows[i].Field<string>("faaAssetID");
				eRPAssetAdjustmentInformationDto.faaAuthorizedByEmployeeID = dataTable.Rows[i].Field<string>("faaAuthorizedByEmployeeID");
				eRPAssetAdjustmentInformationDto.faaClosingPercent = dataTable.Rows[i].Field<decimal>("faaClosingPercent");
				eRPAssetAdjustmentInformationDto.faaClosingPeriodDepreciation = dataTable.Rows[i].Field<decimal>("faaClosingPeriodDepreciation");
				eRPAssetAdjustmentInformationDto.faaCreatedBy = dataTable.Rows[i].Field<string>("faaCreatedBy");
				eRPAssetAdjustmentInformationDto.faaCreatedDate = dataTable.Rows[i].Field<DateTime?>("faaCreatedDate");
				eRPAssetAdjustmentInformationDto.faaCurrencyRateID = dataTable.Rows[i].Field<string>("faaCurrencyRateID");
				eRPAssetAdjustmentInformationDto.faaCustomerOrganizationID = dataTable.Rows[i].Field<string>("faaCustomerOrganizationID");
				eRPAssetAdjustmentInformationDto.faaDepreciationThisYear = dataTable.Rows[i].Field<decimal>("faaDepreciationThisYear");
				eRPAssetAdjustmentInformationDto.faaDestinationPlantID = dataTable.Rows[i].Field<string>("faaDestinationPlantID");
				eRPAssetAdjustmentInformationDto.faaUniqueID = dataTable.Rows[i].Field<Guid>("faaUniqueID");
				eRPAssetAdjustmentInformationDto.faaExchangeRate = dataTable.Rows[i].Field<decimal>("faaExchangeRate");
				eRPAssetAdjustmentInformationDto.faaGlFiscalYearID = dataTable.Rows[i].Field<short>("faaGlFiscalYearID");
				eRPAssetAdjustmentInformationDto.faaGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("faaGlFiscalYearPeriodID");
				eRPAssetAdjustmentInformationDto.faaCustomRate = dataTable.Rows[i].Field<bool>("faaCustomRate");
				eRPAssetAdjustmentInformationDto.faaPostedToGl = dataTable.Rows[i].Field<bool>("faaPostedToGl");
				eRPAssetAdjustmentInformationDto.faaLongDescriptionRtf = dataTable.Rows[i].Field<string>("faaLongDescriptionRtf");
				eRPAssetAdjustmentInformationDto.faaLongDescriptionText = dataTable.Rows[i].Field<string>("faaLongDescriptionText");
				eRPAssetAdjustmentInformationDto.faaNetAssetValue = dataTable.Rows[i].Field<decimal>("faaNetAssetValue");
				eRPAssetAdjustmentInformationDto.faaOpeningAssetValue = dataTable.Rows[i].Field<decimal>("faaOpeningAssetValue");
				eRPAssetAdjustmentInformationDto.faaPostedDate = dataTable.Rows[i].Field<DateTime?>("faaPostedDate");
				eRPAssetAdjustmentInformationDto.faaProfitOrLoss = dataTable.Rows[i].Field<decimal>("faaProfitOrLoss");
				eRPAssetAdjustmentInformationDto.faaQuantity = dataTable.Rows[i].Field<int>("faaQuantity");
				eRPAssetAdjustmentInformationDto.faaRowVersion = dataTable.Rows[i].Field<byte[]>("faaRowVersion");
				eRPAssetAdjustmentInformationDto.faaAssetAdjustmentID = dataTable.Rows[i].Field<int>("faaAssetAdjustmentID");
				eRPAssetAdjustmentInformationDto.faaSourcePlantID = dataTable.Rows[i].Field<string>("faaSourcePlantID");
				eRPAssetAdjustmentInformationDto.faaValue = dataTable.Rows[i].Field<decimal>("faaValue");
				eRPAssetAdjustmentInformationDto.faaValueForeign = dataTable.Rows[i].Field<decimal>("faaValueForeign");
				eRPAssetAdjustmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetAdjustmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetAdjustmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetAdjustmentInformationDto> GetAssetAdjustment(Guid assetAdjustmentId)
	{
		ERPAssetAdjustmentInformationDto eRPAssetAdjustmentInformationDto = new ERPAssetAdjustmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[33]
		{
			"faaAccumulatedDepreciation", "faaAdjustmentDate", "faaAdjustmentType", "faaArInvoiceContactID", "faaArInvoiceLocationID", "faaAssetID", "faaAuthorizedByEmployeeID", "faaClosingPercent", "faaClosingPeriodDepreciation", "faaCreatedBy",
			"faaCreatedDate", "faaCurrencyRateID", "faaCustomerOrganizationID", "faaDepreciationThisYear", "faaDestinationPlantID", "faaUniqueID", "faaExchangeRate", "faaGlFiscalYearID", "faaGlFiscalYearPeriodID", "faaCustomRate",
			"faaPostedToGl", "faaLongDescriptionRtf", "faaLongDescriptionText", "faaNetAssetValue", "faaOpeningAssetValue", "faaPostedDate", "faaProfitOrLoss", "faaQuantity", "faaRowVersion", "faaAssetAdjustmentID",
			"faaSourcePlantID", "faaValue", "faaValueForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("faaUniqueID|C", assetAdjustmentId);
		AddCustomFieldsToSelectList("AssetAdjustments");
		using (DataTable dataTable = GetAsDataTable("AssetAdjustments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetAdjustmentInformationDto);
			}
			eRPAssetAdjustmentInformationDto.faaAccumulatedDepreciation = dataTable.Rows[0].Field<decimal>("faaAccumulatedDepreciation");
			eRPAssetAdjustmentInformationDto.faaAdjustmentDate = dataTable.Rows[0].Field<DateTime?>("faaAdjustmentDate");
			eRPAssetAdjustmentInformationDto.faaAdjustmentType = dataTable.Rows[0].Field<string>("faaAdjustmentType");
			eRPAssetAdjustmentInformationDto.faaArInvoiceContactID = dataTable.Rows[0].Field<string>("faaArInvoiceContactID");
			eRPAssetAdjustmentInformationDto.faaArInvoiceLocationID = dataTable.Rows[0].Field<string>("faaArInvoiceLocationID");
			eRPAssetAdjustmentInformationDto.faaAssetID = dataTable.Rows[0].Field<string>("faaAssetID");
			eRPAssetAdjustmentInformationDto.faaAuthorizedByEmployeeID = dataTable.Rows[0].Field<string>("faaAuthorizedByEmployeeID");
			eRPAssetAdjustmentInformationDto.faaClosingPercent = dataTable.Rows[0].Field<decimal>("faaClosingPercent");
			eRPAssetAdjustmentInformationDto.faaClosingPeriodDepreciation = dataTable.Rows[0].Field<decimal>("faaClosingPeriodDepreciation");
			eRPAssetAdjustmentInformationDto.faaCreatedBy = dataTable.Rows[0].Field<string>("faaCreatedBy");
			eRPAssetAdjustmentInformationDto.faaCreatedDate = dataTable.Rows[0].Field<DateTime?>("faaCreatedDate");
			eRPAssetAdjustmentInformationDto.faaCurrencyRateID = dataTable.Rows[0].Field<string>("faaCurrencyRateID");
			eRPAssetAdjustmentInformationDto.faaCustomerOrganizationID = dataTable.Rows[0].Field<string>("faaCustomerOrganizationID");
			eRPAssetAdjustmentInformationDto.faaDepreciationThisYear = dataTable.Rows[0].Field<decimal>("faaDepreciationThisYear");
			eRPAssetAdjustmentInformationDto.faaDestinationPlantID = dataTable.Rows[0].Field<string>("faaDestinationPlantID");
			eRPAssetAdjustmentInformationDto.faaUniqueID = dataTable.Rows[0].Field<Guid>("faaUniqueID");
			eRPAssetAdjustmentInformationDto.faaExchangeRate = dataTable.Rows[0].Field<decimal>("faaExchangeRate");
			eRPAssetAdjustmentInformationDto.faaGlFiscalYearID = dataTable.Rows[0].Field<short>("faaGlFiscalYearID");
			eRPAssetAdjustmentInformationDto.faaGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("faaGlFiscalYearPeriodID");
			eRPAssetAdjustmentInformationDto.faaCustomRate = dataTable.Rows[0].Field<bool>("faaCustomRate");
			eRPAssetAdjustmentInformationDto.faaPostedToGl = dataTable.Rows[0].Field<bool>("faaPostedToGl");
			eRPAssetAdjustmentInformationDto.faaLongDescriptionRtf = dataTable.Rows[0].Field<string>("faaLongDescriptionRtf");
			eRPAssetAdjustmentInformationDto.faaLongDescriptionText = dataTable.Rows[0].Field<string>("faaLongDescriptionText");
			eRPAssetAdjustmentInformationDto.faaNetAssetValue = dataTable.Rows[0].Field<decimal>("faaNetAssetValue");
			eRPAssetAdjustmentInformationDto.faaOpeningAssetValue = dataTable.Rows[0].Field<decimal>("faaOpeningAssetValue");
			eRPAssetAdjustmentInformationDto.faaPostedDate = dataTable.Rows[0].Field<DateTime?>("faaPostedDate");
			eRPAssetAdjustmentInformationDto.faaProfitOrLoss = dataTable.Rows[0].Field<decimal>("faaProfitOrLoss");
			eRPAssetAdjustmentInformationDto.faaQuantity = dataTable.Rows[0].Field<int>("faaQuantity");
			eRPAssetAdjustmentInformationDto.faaRowVersion = dataTable.Rows[0].Field<byte[]>("faaRowVersion");
			eRPAssetAdjustmentInformationDto.faaAssetAdjustmentID = dataTable.Rows[0].Field<int>("faaAssetAdjustmentID");
			eRPAssetAdjustmentInformationDto.faaSourcePlantID = dataTable.Rows[0].Field<string>("faaSourcePlantID");
			eRPAssetAdjustmentInformationDto.faaValue = dataTable.Rows[0].Field<decimal>("faaValue");
			eRPAssetAdjustmentInformationDto.faaValueForeign = dataTable.Rows[0].Field<decimal>("faaValueForeign");
			eRPAssetAdjustmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetAdjustmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetAdjustmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAssetAdjustment(ERPAssetAdjustmentDto assetAdjustment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AssetAdjustments WHERE faaUniqueID = " + M1Util.ConvertToLinq(assetAdjustment.faaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["faaAssetAdjustmentID"] = assetAdjustment.faaAssetAdjustmentID;
				assetAdjustment.faaUniqueID = ((assetAdjustment.faaUniqueID == Guid.Empty) ? Guid.NewGuid() : assetAdjustment.faaUniqueID);
				dataRow["faaUniqueID"] = assetAdjustment.faaUniqueID;
				dataRow["faaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["faaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AssetAdjustment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (assetAdjustment.faaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AssetAdjustment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["faaRowVersion"], assetAdjustment.faaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AssetAdjustment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AssetAdjustment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["faaAccumulatedDepreciation"] = assetAdjustment.faaAccumulatedDepreciation;
			DataRow dataRow2 = dataRow;
			DateTime? faaAdjustmentDate = assetAdjustment.faaAdjustmentDate;
			dataRow2["faaAdjustmentDate"] = (faaAdjustmentDate.HasValue ? ((object)faaAdjustmentDate.GetValueOrDefault()) : dataRow["faaAdjustmentDate"]);
			dataRow["faaAdjustmentType"] = assetAdjustment.faaAdjustmentType;
			dataRow["faaArInvoiceContactID"] = assetAdjustment.faaArInvoiceContactID;
			dataRow["faaArInvoiceLocationID"] = assetAdjustment.faaArInvoiceLocationID;
			dataRow["faaAssetID"] = assetAdjustment.faaAssetID;
			dataRow["faaAuthorizedByEmployeeID"] = assetAdjustment.faaAuthorizedByEmployeeID;
			dataRow["faaClosingPercent"] = assetAdjustment.faaClosingPercent;
			dataRow["faaClosingPeriodDepreciation"] = assetAdjustment.faaClosingPeriodDepreciation;
			dataRow["faaCurrencyRateID"] = assetAdjustment.faaCurrencyRateID;
			dataRow["faaCustomerOrganizationID"] = assetAdjustment.faaCustomerOrganizationID;
			dataRow["faaDepreciationThisYear"] = assetAdjustment.faaDepreciationThisYear;
			dataRow["faaDestinationPlantID"] = assetAdjustment.faaDestinationPlantID;
			dataRow["faaExchangeRate"] = assetAdjustment.faaExchangeRate;
			dataRow["faaGlFiscalYearID"] = assetAdjustment.faaGlFiscalYearID;
			dataRow["faaGlFiscalYearPeriodID"] = assetAdjustment.faaGlFiscalYearPeriodID;
			dataRow["faaCustomRate"] = assetAdjustment.faaCustomRate;
			dataRow["faaPostedToGl"] = assetAdjustment.faaPostedToGl;
			dataRow["faaLongDescriptionRtf"] = assetAdjustment.faaLongDescriptionRtf ?? dataRow["faaLongDescriptionRtf"];
			dataRow["faaLongDescriptionText"] = assetAdjustment.faaLongDescriptionText ?? dataRow["faaLongDescriptionText"];
			dataRow["faaNetAssetValue"] = assetAdjustment.faaNetAssetValue;
			dataRow["faaOpeningAssetValue"] = assetAdjustment.faaOpeningAssetValue;
			DataRow dataRow3 = dataRow;
			faaAdjustmentDate = assetAdjustment.faaPostedDate;
			dataRow3["faaPostedDate"] = (faaAdjustmentDate.HasValue ? ((object)faaAdjustmentDate.GetValueOrDefault()) : dataRow["faaPostedDate"]);
			dataRow["faaProfitOrLoss"] = assetAdjustment.faaProfitOrLoss;
			dataRow["faaQuantity"] = assetAdjustment.faaQuantity;
			dataRow["faaSourcePlantID"] = assetAdjustment.faaSourcePlantID;
			dataRow["faaValue"] = assetAdjustment.faaValue;
			dataRow["faaValueForeign"] = assetAdjustment.faaValueForeign;
			if (assetAdjustment.CustomFields != null && assetAdjustment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in assetAdjustment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AssetAdjustment [{assetAdjustment.faaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AssetAdjustment [{assetAdjustment.faaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
