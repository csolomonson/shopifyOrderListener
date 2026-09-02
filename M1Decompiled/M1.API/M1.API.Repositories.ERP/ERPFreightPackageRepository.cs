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

public class ERPFreightPackageRepository : APIBaseRepository, IERPFreightPackageRepository, IAPIBaseRepository, IDisposable
{
	public ERPFreightPackageRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFreightPackageExist(Guid freightPackageId)
	{
		InitializeParameterLists();
		base.filterList.Add("fslUniqueID|C", freightPackageId);
		base.selectList.Add("fslUniqueID");
		return Task.FromResult(GetAsObject("FreightPackages", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFreightPackageInformationDto>> GetAllFreightPackages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFreightPackageInformationDto> collection = new List<ERPFreightPackageInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"fslCreatedBy", "fslCreatedDate", "fslDimensionsUnitOfMeasure", "fslDistributeCostsOption", "fslUniqueID", "fslFdxPackageHeight", "fslFdxPackageLength", "fslFdxPackageWidth", "fslFdxPackaging", "fslFreightShipmentID",
			"fslFdxNonstandardContainer", "fslVoidOnUps", "fslNotesRTF", "fslNotesText", "fslPackageCharge", "fslPackageFullWeight", "fslPackagePublishedCharge", "fslRowVersion", "fslFreightPackageID", "fslTrackingNumber",
			"fslUpsPackageType", "fslWeightUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("FreightPackages");
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
		using (DataTable dataTable = GetAsDataTable("FreightPackages", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFreightPackageInformationDto eRPFreightPackageInformationDto = new ERPFreightPackageInformationDto();
				eRPFreightPackageInformationDto.fslCreatedBy = dataTable.Rows[i].Field<string>("fslCreatedBy");
				eRPFreightPackageInformationDto.fslCreatedDate = dataTable.Rows[i].Field<DateTime?>("fslCreatedDate");
				eRPFreightPackageInformationDto.fslDimensionsUnitOfMeasure = dataTable.Rows[i].Field<string>("fslDimensionsUnitOfMeasure");
				eRPFreightPackageInformationDto.fslDistributeCostsOption = dataTable.Rows[i].Field<byte>("fslDistributeCostsOption");
				eRPFreightPackageInformationDto.fslUniqueID = dataTable.Rows[i].Field<Guid>("fslUniqueID");
				eRPFreightPackageInformationDto.fslFdxPackageHeight = dataTable.Rows[i].Field<int>("fslFdxPackageHeight");
				eRPFreightPackageInformationDto.fslFdxPackageLength = dataTable.Rows[i].Field<int>("fslFdxPackageLength");
				eRPFreightPackageInformationDto.fslFdxPackageWidth = dataTable.Rows[i].Field<int>("fslFdxPackageWidth");
				eRPFreightPackageInformationDto.fslFdxPackaging = dataTable.Rows[i].Field<string>("fslFdxPackaging");
				eRPFreightPackageInformationDto.fslFreightShipmentID = dataTable.Rows[i].Field<string>("fslFreightShipmentID");
				eRPFreightPackageInformationDto.fslFdxNonstandardContainer = dataTable.Rows[i].Field<bool>("fslFdxNonstandardContainer");
				eRPFreightPackageInformationDto.fslVoidOnUps = dataTable.Rows[i].Field<bool>("fslVoidOnUps");
				eRPFreightPackageInformationDto.fslNotesRTF = dataTable.Rows[i].Field<string>("fslNotesRTF");
				eRPFreightPackageInformationDto.fslNotesText = dataTable.Rows[i].Field<string>("fslNotesText");
				eRPFreightPackageInformationDto.fslPackageCharge = dataTable.Rows[i].Field<decimal>("fslPackageCharge");
				eRPFreightPackageInformationDto.fslPackageFullWeight = dataTable.Rows[i].Field<decimal>("fslPackageFullWeight");
				eRPFreightPackageInformationDto.fslPackagePublishedCharge = dataTable.Rows[i].Field<decimal>("fslPackagePublishedCharge");
				eRPFreightPackageInformationDto.fslRowVersion = dataTable.Rows[i].Field<byte[]>("fslRowVersion");
				eRPFreightPackageInformationDto.fslFreightPackageID = dataTable.Rows[i].Field<short>("fslFreightPackageID");
				eRPFreightPackageInformationDto.fslTrackingNumber = dataTable.Rows[i].Field<string>("fslTrackingNumber");
				eRPFreightPackageInformationDto.fslUpsPackageType = dataTable.Rows[i].Field<string>("fslUpsPackageType");
				eRPFreightPackageInformationDto.fslWeightUnitOfMeasure = dataTable.Rows[i].Field<string>("fslWeightUnitOfMeasure");
				eRPFreightPackageInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFreightPackageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFreightPackageInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFreightPackageInformationDto> GetFreightPackage(Guid freightPackageId)
	{
		ERPFreightPackageInformationDto eRPFreightPackageInformationDto = new ERPFreightPackageInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"fslCreatedBy", "fslCreatedDate", "fslDimensionsUnitOfMeasure", "fslDistributeCostsOption", "fslUniqueID", "fslFdxPackageHeight", "fslFdxPackageLength", "fslFdxPackageWidth", "fslFdxPackaging", "fslFreightShipmentID",
			"fslFdxNonstandardContainer", "fslVoidOnUps", "fslNotesRTF", "fslNotesText", "fslPackageCharge", "fslPackageFullWeight", "fslPackagePublishedCharge", "fslRowVersion", "fslFreightPackageID", "fslTrackingNumber",
			"fslUpsPackageType", "fslWeightUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fslUniqueID|C", freightPackageId);
		AddCustomFieldsToSelectList("FreightPackages");
		using (DataTable dataTable = GetAsDataTable("FreightPackages", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFreightPackageInformationDto);
			}
			eRPFreightPackageInformationDto.fslCreatedBy = dataTable.Rows[0].Field<string>("fslCreatedBy");
			eRPFreightPackageInformationDto.fslCreatedDate = dataTable.Rows[0].Field<DateTime?>("fslCreatedDate");
			eRPFreightPackageInformationDto.fslDimensionsUnitOfMeasure = dataTable.Rows[0].Field<string>("fslDimensionsUnitOfMeasure");
			eRPFreightPackageInformationDto.fslDistributeCostsOption = dataTable.Rows[0].Field<byte>("fslDistributeCostsOption");
			eRPFreightPackageInformationDto.fslUniqueID = dataTable.Rows[0].Field<Guid>("fslUniqueID");
			eRPFreightPackageInformationDto.fslFdxPackageHeight = dataTable.Rows[0].Field<int>("fslFdxPackageHeight");
			eRPFreightPackageInformationDto.fslFdxPackageLength = dataTable.Rows[0].Field<int>("fslFdxPackageLength");
			eRPFreightPackageInformationDto.fslFdxPackageWidth = dataTable.Rows[0].Field<int>("fslFdxPackageWidth");
			eRPFreightPackageInformationDto.fslFdxPackaging = dataTable.Rows[0].Field<string>("fslFdxPackaging");
			eRPFreightPackageInformationDto.fslFreightShipmentID = dataTable.Rows[0].Field<string>("fslFreightShipmentID");
			eRPFreightPackageInformationDto.fslFdxNonstandardContainer = dataTable.Rows[0].Field<bool>("fslFdxNonstandardContainer");
			eRPFreightPackageInformationDto.fslVoidOnUps = dataTable.Rows[0].Field<bool>("fslVoidOnUps");
			eRPFreightPackageInformationDto.fslNotesRTF = dataTable.Rows[0].Field<string>("fslNotesRTF");
			eRPFreightPackageInformationDto.fslNotesText = dataTable.Rows[0].Field<string>("fslNotesText");
			eRPFreightPackageInformationDto.fslPackageCharge = dataTable.Rows[0].Field<decimal>("fslPackageCharge");
			eRPFreightPackageInformationDto.fslPackageFullWeight = dataTable.Rows[0].Field<decimal>("fslPackageFullWeight");
			eRPFreightPackageInformationDto.fslPackagePublishedCharge = dataTable.Rows[0].Field<decimal>("fslPackagePublishedCharge");
			eRPFreightPackageInformationDto.fslRowVersion = dataTable.Rows[0].Field<byte[]>("fslRowVersion");
			eRPFreightPackageInformationDto.fslFreightPackageID = dataTable.Rows[0].Field<short>("fslFreightPackageID");
			eRPFreightPackageInformationDto.fslTrackingNumber = dataTable.Rows[0].Field<string>("fslTrackingNumber");
			eRPFreightPackageInformationDto.fslUpsPackageType = dataTable.Rows[0].Field<string>("fslUpsPackageType");
			eRPFreightPackageInformationDto.fslWeightUnitOfMeasure = dataTable.Rows[0].Field<string>("fslWeightUnitOfMeasure");
			eRPFreightPackageInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFreightPackageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFreightPackageInformationDto);
	}

	public Task<APIValidationInfoDto> SaveFreightPackage(ERPFreightPackageDto freightPackage)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM FreightPackages WHERE fslUniqueID = " + M1Util.ConvertToLinq(freightPackage.fslUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fslFreightShipmentID"] = freightPackage.fslFreightShipmentID.ToUpper();
				dataRow["fslFreightPackageID"] = freightPackage.fslFreightPackageID;
				freightPackage.fslUniqueID = ((freightPackage.fslUniqueID == Guid.Empty) ? Guid.NewGuid() : freightPackage.fslUniqueID);
				dataRow["fslUniqueID"] = freightPackage.fslUniqueID;
				dataRow["fslCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fslCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The FreightPackage could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (freightPackage.fslRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the FreightPackage is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fslRowVersion"], freightPackage.fslRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the FreightPackage has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the FreightPackage again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fslDimensionsUnitOfMeasure"] = freightPackage.fslDimensionsUnitOfMeasure;
			dataRow["fslDistributeCostsOption"] = freightPackage.fslDistributeCostsOption;
			dataRow["fslFdxPackageHeight"] = freightPackage.fslFdxPackageHeight;
			dataRow["fslFdxPackageLength"] = freightPackage.fslFdxPackageLength;
			dataRow["fslFdxPackageWidth"] = freightPackage.fslFdxPackageWidth;
			dataRow["fslFdxPackaging"] = freightPackage.fslFdxPackaging;
			dataRow["fslFdxNonstandardContainer"] = freightPackage.fslFdxNonstandardContainer;
			dataRow["fslVoidOnUps"] = freightPackage.fslVoidOnUps;
			dataRow["fslNotesRTF"] = freightPackage.fslNotesRTF ?? dataRow["fslNotesRTF"];
			dataRow["fslNotesText"] = freightPackage.fslNotesText ?? dataRow["fslNotesText"];
			dataRow["fslPackageCharge"] = freightPackage.fslPackageCharge;
			dataRow["fslPackageFullWeight"] = freightPackage.fslPackageFullWeight;
			dataRow["fslPackagePublishedCharge"] = freightPackage.fslPackagePublishedCharge;
			dataRow["fslTrackingNumber"] = freightPackage.fslTrackingNumber;
			dataRow["fslUpsPackageType"] = freightPackage.fslUpsPackageType;
			dataRow["fslWeightUnitOfMeasure"] = freightPackage.fslWeightUnitOfMeasure;
			if (freightPackage.CustomFields != null && freightPackage.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in freightPackage.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the FreightPackage [{freightPackage.fslUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the FreightPackage [{freightPackage.fslUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
