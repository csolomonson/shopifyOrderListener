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

public class ERPShipmentFreightReferenceRepository : APIBaseRepository, IERPShipmentFreightReferenceRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentFreightReferenceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentFreightReferenceExist(Guid shipmentFreightReferenceId)
	{
		InitializeParameterLists();
		base.filterList.Add("smrUniqueID|C", shipmentFreightReferenceId);
		base.selectList.Add("smrUniqueID");
		return Task.FromResult(GetAsObject("ShipmentFreightReferences", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentFreightReferenceInformationDto>> GetAllShipmentFreightReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentFreightReferenceInformationDto> collection = new List<ERPShipmentFreightReferenceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "smrCreatedBy", "smrCreatedDate", "smrUniqueID", "smrFreightShipmentID", "smrRowVersion", "smrShipmentFreightReferenceID", "smrShipmentID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShipmentFreightReferences");
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
		using (DataTable dataTable = GetAsDataTable("ShipmentFreightReferences", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentFreightReferenceInformationDto eRPShipmentFreightReferenceInformationDto = new ERPShipmentFreightReferenceInformationDto();
				eRPShipmentFreightReferenceInformationDto.smrCreatedBy = dataTable.Rows[i].Field<string>("smrCreatedBy");
				eRPShipmentFreightReferenceInformationDto.smrCreatedDate = dataTable.Rows[i].Field<DateTime?>("smrCreatedDate");
				eRPShipmentFreightReferenceInformationDto.smrUniqueID = dataTable.Rows[i].Field<Guid>("smrUniqueID");
				eRPShipmentFreightReferenceInformationDto.smrFreightShipmentID = dataTable.Rows[i].Field<string>("smrFreightShipmentID");
				eRPShipmentFreightReferenceInformationDto.smrRowVersion = dataTable.Rows[i].Field<byte[]>("smrRowVersion");
				eRPShipmentFreightReferenceInformationDto.smrShipmentFreightReferenceID = dataTable.Rows[i].Field<short>("smrShipmentFreightReferenceID");
				eRPShipmentFreightReferenceInformationDto.smrShipmentID = dataTable.Rows[i].Field<string>("smrShipmentID");
				eRPShipmentFreightReferenceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentFreightReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentFreightReferenceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentFreightReferenceInformationDto> GetShipmentFreightReference(Guid shipmentFreightReferenceId)
	{
		ERPShipmentFreightReferenceInformationDto eRPShipmentFreightReferenceInformationDto = new ERPShipmentFreightReferenceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "smrCreatedBy", "smrCreatedDate", "smrUniqueID", "smrFreightShipmentID", "smrRowVersion", "smrShipmentFreightReferenceID", "smrShipmentID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("smrUniqueID|C", shipmentFreightReferenceId);
		AddCustomFieldsToSelectList("ShipmentFreightReferences");
		using (DataTable dataTable = GetAsDataTable("ShipmentFreightReferences", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentFreightReferenceInformationDto);
			}
			eRPShipmentFreightReferenceInformationDto.smrCreatedBy = dataTable.Rows[0].Field<string>("smrCreatedBy");
			eRPShipmentFreightReferenceInformationDto.smrCreatedDate = dataTable.Rows[0].Field<DateTime?>("smrCreatedDate");
			eRPShipmentFreightReferenceInformationDto.smrUniqueID = dataTable.Rows[0].Field<Guid>("smrUniqueID");
			eRPShipmentFreightReferenceInformationDto.smrFreightShipmentID = dataTable.Rows[0].Field<string>("smrFreightShipmentID");
			eRPShipmentFreightReferenceInformationDto.smrRowVersion = dataTable.Rows[0].Field<byte[]>("smrRowVersion");
			eRPShipmentFreightReferenceInformationDto.smrShipmentFreightReferenceID = dataTable.Rows[0].Field<short>("smrShipmentFreightReferenceID");
			eRPShipmentFreightReferenceInformationDto.smrShipmentID = dataTable.Rows[0].Field<string>("smrShipmentID");
			eRPShipmentFreightReferenceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentFreightReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentFreightReferenceInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipmentFreightReference(ERPShipmentFreightReferenceDto shipmentFreightReference)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShipmentFreightReferences WHERE smrUniqueID = " + M1Util.ConvertToLinq(shipmentFreightReference.smrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["smrShipmentID"] = shipmentFreightReference.smrShipmentID.ToUpper();
				dataRow["smrShipmentFreightReferenceID"] = shipmentFreightReference.smrShipmentFreightReferenceID;
				shipmentFreightReference.smrUniqueID = ((shipmentFreightReference.smrUniqueID == Guid.Empty) ? Guid.NewGuid() : shipmentFreightReference.smrUniqueID);
				dataRow["smrUniqueID"] = shipmentFreightReference.smrUniqueID;
				dataRow["smrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["smrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShipmentFreightReference could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipmentFreightReference.smrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShipmentFreightReference is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["smrRowVersion"], shipmentFreightReference.smrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShipmentFreightReference has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShipmentFreightReference again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["smrFreightShipmentID"] = shipmentFreightReference.smrFreightShipmentID;
			if (shipmentFreightReference.CustomFields != null && shipmentFreightReference.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipmentFreightReference.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShipmentFreightReference [{shipmentFreightReference.smrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShipmentFreightReference [{shipmentFreightReference.smrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
