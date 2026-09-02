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

public class ERPFreightReferenceRepository : APIBaseRepository, IERPFreightReferenceRepository, IAPIBaseRepository, IDisposable
{
	public ERPFreightReferenceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFreightReferenceExist(Guid freightReferenceId)
	{
		InitializeParameterLists();
		base.filterList.Add("frcUniqueID|C", freightReferenceId);
		base.selectList.Add("frcUniqueID");
		return Task.FromResult(GetAsObject("FreightReferences", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFreightReferenceInformationDto>> GetAllFreightReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFreightReferenceInformationDto> collection = new List<ERPFreightReferenceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "frcFreightReferenceID", "frcUniqueID", "frcFreightShipmentID", "frcQuoteID", "frcRowVersion", "frcShipmentID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("FreightReferences");
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
		using (DataTable dataTable = GetAsDataTable("FreightReferences", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFreightReferenceInformationDto eRPFreightReferenceInformationDto = new ERPFreightReferenceInformationDto();
				eRPFreightReferenceInformationDto.frcFreightReferenceID = dataTable.Rows[i].Field<string>("frcFreightReferenceID");
				eRPFreightReferenceInformationDto.frcUniqueID = dataTable.Rows[i].Field<Guid>("frcUniqueID");
				eRPFreightReferenceInformationDto.frcFreightShipmentID = dataTable.Rows[i].Field<string>("frcFreightShipmentID");
				eRPFreightReferenceInformationDto.frcQuoteID = dataTable.Rows[i].Field<string>("frcQuoteID");
				eRPFreightReferenceInformationDto.frcRowVersion = dataTable.Rows[i].Field<byte[]>("frcRowVersion");
				eRPFreightReferenceInformationDto.frcShipmentID = dataTable.Rows[i].Field<string>("frcShipmentID");
				eRPFreightReferenceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFreightReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFreightReferenceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFreightReferenceInformationDto> GetFreightReference(Guid freightReferenceId)
	{
		ERPFreightReferenceInformationDto eRPFreightReferenceInformationDto = new ERPFreightReferenceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "frcFreightReferenceID", "frcUniqueID", "frcFreightShipmentID", "frcQuoteID", "frcRowVersion", "frcShipmentID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("frcUniqueID|C", freightReferenceId);
		AddCustomFieldsToSelectList("FreightReferences");
		using (DataTable dataTable = GetAsDataTable("FreightReferences", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFreightReferenceInformationDto);
			}
			eRPFreightReferenceInformationDto.frcFreightReferenceID = dataTable.Rows[0].Field<string>("frcFreightReferenceID");
			eRPFreightReferenceInformationDto.frcUniqueID = dataTable.Rows[0].Field<Guid>("frcUniqueID");
			eRPFreightReferenceInformationDto.frcFreightShipmentID = dataTable.Rows[0].Field<string>("frcFreightShipmentID");
			eRPFreightReferenceInformationDto.frcQuoteID = dataTable.Rows[0].Field<string>("frcQuoteID");
			eRPFreightReferenceInformationDto.frcRowVersion = dataTable.Rows[0].Field<byte[]>("frcRowVersion");
			eRPFreightReferenceInformationDto.frcShipmentID = dataTable.Rows[0].Field<string>("frcShipmentID");
			eRPFreightReferenceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFreightReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFreightReferenceInformationDto);
	}

	public Task<APIValidationInfoDto> SaveFreightReference(ERPFreightReferenceDto freightReference)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM FreightReferences WHERE frcUniqueID = " + M1Util.ConvertToLinq(freightReference.frcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["frcFreightReferenceID"] = freightReference.frcFreightReferenceID.ToUpper();
				freightReference.frcUniqueID = ((freightReference.frcUniqueID == Guid.Empty) ? Guid.NewGuid() : freightReference.frcUniqueID);
				dataRow["frcUniqueID"] = freightReference.frcUniqueID;
				dataRow["frcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["frcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The FreightReference could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (freightReference.frcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the FreightReference is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["frcRowVersion"], freightReference.frcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the FreightReference has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the FreightReference again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["frcFreightShipmentID"] = freightReference.frcFreightShipmentID;
			dataRow["frcQuoteID"] = freightReference.frcQuoteID;
			dataRow["frcShipmentID"] = freightReference.frcShipmentID;
			if (freightReference.CustomFields != null && freightReference.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in freightReference.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the FreightReference [{freightReference.frcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the FreightReference [{freightReference.frcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
