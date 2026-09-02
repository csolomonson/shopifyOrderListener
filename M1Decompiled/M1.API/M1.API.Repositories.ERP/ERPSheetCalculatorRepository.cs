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

public class ERPSheetCalculatorRepository : APIBaseRepository, IERPSheetCalculatorRepository, IAPIBaseRepository, IDisposable
{
	public ERPSheetCalculatorRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSheetCalculatorExist(Guid sheetCalculatorId)
	{
		InitializeParameterLists();
		base.filterList.Add("ccsUniqueID|C", sheetCalculatorId);
		base.selectList.Add("ccsUniqueID");
		return Task.FromResult(GetAsObject("SheetCalculators", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSheetCalculatorInformationDto>> GetAllSheetCalculators(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSheetCalculatorInformationDto> collection = new List<ERPSheetCalculatorInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"ccs0Rotation", "ccs90Rotation", "ccsSheetCalculatorID", "ccsCreatedBy", "ccsCreatedDate", "ccsUniqueID", "ccsGrain", "ccsMeasurementType", "ccsPartSizeX", "ccsPartSizeY",
			"ccsPartSpacingX", "ccsPartSpacingY", "ccsRowVersion", "ccsSheetSizeX", "ccsSheetSizeY", "ccsTotalTrimX", "ccsTotalTrimY"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SheetCalculators");
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
		using (DataTable dataTable = GetAsDataTable("SheetCalculators", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSheetCalculatorInformationDto eRPSheetCalculatorInformationDto = new ERPSheetCalculatorInformationDto();
				eRPSheetCalculatorInformationDto.ccs0Rotation = dataTable.Rows[i].Field<decimal>("ccs0Rotation");
				eRPSheetCalculatorInformationDto.ccs90Rotation = dataTable.Rows[i].Field<decimal>("ccs90Rotation");
				eRPSheetCalculatorInformationDto.ccsSheetCalculatorID = dataTable.Rows[i].Field<Guid>("ccsSheetCalculatorID");
				eRPSheetCalculatorInformationDto.ccsCreatedBy = dataTable.Rows[i].Field<string>("ccsCreatedBy");
				eRPSheetCalculatorInformationDto.ccsCreatedDate = dataTable.Rows[i].Field<DateTime?>("ccsCreatedDate");
				eRPSheetCalculatorInformationDto.ccsUniqueID = dataTable.Rows[i].Field<Guid>("ccsUniqueID");
				eRPSheetCalculatorInformationDto.ccsGrain = dataTable.Rows[i].Field<bool>("ccsGrain");
				eRPSheetCalculatorInformationDto.ccsMeasurementType = dataTable.Rows[i].Field<string>("ccsMeasurementType");
				eRPSheetCalculatorInformationDto.ccsPartSizeX = dataTable.Rows[i].Field<decimal>("ccsPartSizeX");
				eRPSheetCalculatorInformationDto.ccsPartSizeY = dataTable.Rows[i].Field<decimal>("ccsPartSizeY");
				eRPSheetCalculatorInformationDto.ccsPartSpacingX = dataTable.Rows[i].Field<decimal>("ccsPartSpacingX");
				eRPSheetCalculatorInformationDto.ccsPartSpacingY = dataTable.Rows[i].Field<decimal>("ccsPartSpacingY");
				eRPSheetCalculatorInformationDto.ccsRowVersion = dataTable.Rows[i].Field<byte[]>("ccsRowVersion");
				eRPSheetCalculatorInformationDto.ccsSheetSizeX = dataTable.Rows[i].Field<decimal>("ccsSheetSizeX");
				eRPSheetCalculatorInformationDto.ccsSheetSizeY = dataTable.Rows[i].Field<decimal>("ccsSheetSizeY");
				eRPSheetCalculatorInformationDto.ccsTotalTrimX = dataTable.Rows[i].Field<decimal>("ccsTotalTrimX");
				eRPSheetCalculatorInformationDto.ccsTotalTrimY = dataTable.Rows[i].Field<decimal>("ccsTotalTrimY");
				eRPSheetCalculatorInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSheetCalculatorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSheetCalculatorInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSheetCalculatorInformationDto> GetSheetCalculator(Guid sheetCalculatorId)
	{
		ERPSheetCalculatorInformationDto eRPSheetCalculatorInformationDto = new ERPSheetCalculatorInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"ccs0Rotation", "ccs90Rotation", "ccsSheetCalculatorID", "ccsCreatedBy", "ccsCreatedDate", "ccsUniqueID", "ccsGrain", "ccsMeasurementType", "ccsPartSizeX", "ccsPartSizeY",
			"ccsPartSpacingX", "ccsPartSpacingY", "ccsRowVersion", "ccsSheetSizeX", "ccsSheetSizeY", "ccsTotalTrimX", "ccsTotalTrimY"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ccsUniqueID|C", sheetCalculatorId);
		AddCustomFieldsToSelectList("SheetCalculators");
		using (DataTable dataTable = GetAsDataTable("SheetCalculators", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSheetCalculatorInformationDto);
			}
			eRPSheetCalculatorInformationDto.ccs0Rotation = dataTable.Rows[0].Field<decimal>("ccs0Rotation");
			eRPSheetCalculatorInformationDto.ccs90Rotation = dataTable.Rows[0].Field<decimal>("ccs90Rotation");
			eRPSheetCalculatorInformationDto.ccsSheetCalculatorID = dataTable.Rows[0].Field<Guid>("ccsSheetCalculatorID");
			eRPSheetCalculatorInformationDto.ccsCreatedBy = dataTable.Rows[0].Field<string>("ccsCreatedBy");
			eRPSheetCalculatorInformationDto.ccsCreatedDate = dataTable.Rows[0].Field<DateTime?>("ccsCreatedDate");
			eRPSheetCalculatorInformationDto.ccsUniqueID = dataTable.Rows[0].Field<Guid>("ccsUniqueID");
			eRPSheetCalculatorInformationDto.ccsGrain = dataTable.Rows[0].Field<bool>("ccsGrain");
			eRPSheetCalculatorInformationDto.ccsMeasurementType = dataTable.Rows[0].Field<string>("ccsMeasurementType");
			eRPSheetCalculatorInformationDto.ccsPartSizeX = dataTable.Rows[0].Field<decimal>("ccsPartSizeX");
			eRPSheetCalculatorInformationDto.ccsPartSizeY = dataTable.Rows[0].Field<decimal>("ccsPartSizeY");
			eRPSheetCalculatorInformationDto.ccsPartSpacingX = dataTable.Rows[0].Field<decimal>("ccsPartSpacingX");
			eRPSheetCalculatorInformationDto.ccsPartSpacingY = dataTable.Rows[0].Field<decimal>("ccsPartSpacingY");
			eRPSheetCalculatorInformationDto.ccsRowVersion = dataTable.Rows[0].Field<byte[]>("ccsRowVersion");
			eRPSheetCalculatorInformationDto.ccsSheetSizeX = dataTable.Rows[0].Field<decimal>("ccsSheetSizeX");
			eRPSheetCalculatorInformationDto.ccsSheetSizeY = dataTable.Rows[0].Field<decimal>("ccsSheetSizeY");
			eRPSheetCalculatorInformationDto.ccsTotalTrimX = dataTable.Rows[0].Field<decimal>("ccsTotalTrimX");
			eRPSheetCalculatorInformationDto.ccsTotalTrimY = dataTable.Rows[0].Field<decimal>("ccsTotalTrimY");
			eRPSheetCalculatorInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSheetCalculatorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSheetCalculatorInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSheetCalculator(ERPSheetCalculatorDto sheetCalculator)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SheetCalculators WHERE ccsUniqueID = " + M1Util.ConvertToLinq(sheetCalculator.ccsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ccsSheetCalculatorID"] = sheetCalculator.ccsSheetCalculatorID;
				sheetCalculator.ccsUniqueID = ((sheetCalculator.ccsUniqueID == Guid.Empty) ? Guid.NewGuid() : sheetCalculator.ccsUniqueID);
				dataRow["ccsUniqueID"] = sheetCalculator.ccsUniqueID;
				dataRow["ccsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ccsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SheetCalculator could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (sheetCalculator.ccsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SheetCalculator is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ccsRowVersion"], sheetCalculator.ccsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SheetCalculator has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SheetCalculator again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ccs0Rotation"] = sheetCalculator.ccs0Rotation;
			dataRow["ccs90Rotation"] = sheetCalculator.ccs90Rotation;
			dataRow["ccsGrain"] = sheetCalculator.ccsGrain;
			dataRow["ccsMeasurementType"] = sheetCalculator.ccsMeasurementType;
			dataRow["ccsPartSizeX"] = sheetCalculator.ccsPartSizeX;
			dataRow["ccsPartSizeY"] = sheetCalculator.ccsPartSizeY;
			dataRow["ccsPartSpacingX"] = sheetCalculator.ccsPartSpacingX;
			dataRow["ccsPartSpacingY"] = sheetCalculator.ccsPartSpacingY;
			dataRow["ccsSheetSizeX"] = sheetCalculator.ccsSheetSizeX;
			dataRow["ccsSheetSizeY"] = sheetCalculator.ccsSheetSizeY;
			dataRow["ccsTotalTrimX"] = sheetCalculator.ccsTotalTrimX;
			dataRow["ccsTotalTrimY"] = sheetCalculator.ccsTotalTrimY;
			if (sheetCalculator.CustomFields != null && sheetCalculator.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in sheetCalculator.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SheetCalculator [{sheetCalculator.ccsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SheetCalculator [{sheetCalculator.ccsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
