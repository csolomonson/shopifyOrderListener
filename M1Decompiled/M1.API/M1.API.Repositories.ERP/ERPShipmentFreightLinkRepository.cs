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

public class ERPShipmentFreightLinkRepository : APIBaseRepository, IERPShipmentFreightLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentFreightLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentFreightLinkExist(Guid shipmentFreightLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("smxUniqueID|C", shipmentFreightLinkId);
		base.selectList.Add("smxUniqueID");
		return Task.FromResult(GetAsObject("ShipmentFreightLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentFreightLinkInformationDto>> GetAllShipmentFreightLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentFreightLinkInformationDto> collection = new List<ERPShipmentFreightLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"smxCreatedBy", "smxCreatedDate", "smxUniqueID", "smxFreightCharges", "smxFreightPackageID", "smxFreightShipmentID", "smxClosed", "smxLinkPctCharge", "smxPackagePartialCount", "smxPackagePartialWeight",
			"smxRowVersion", "smxShipmentFreightLinkID", "smxShipmentID", "smxShipmentLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShipmentFreightLinks");
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
		using (DataTable dataTable = GetAsDataTable("ShipmentFreightLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentFreightLinkInformationDto eRPShipmentFreightLinkInformationDto = new ERPShipmentFreightLinkInformationDto();
				eRPShipmentFreightLinkInformationDto.smxCreatedBy = dataTable.Rows[i].Field<string>("smxCreatedBy");
				eRPShipmentFreightLinkInformationDto.smxCreatedDate = dataTable.Rows[i].Field<DateTime?>("smxCreatedDate");
				eRPShipmentFreightLinkInformationDto.smxUniqueID = dataTable.Rows[i].Field<Guid>("smxUniqueID");
				eRPShipmentFreightLinkInformationDto.smxFreightCharges = dataTable.Rows[i].Field<decimal>("smxFreightCharges");
				eRPShipmentFreightLinkInformationDto.smxFreightPackageID = dataTable.Rows[i].Field<short>("smxFreightPackageID");
				eRPShipmentFreightLinkInformationDto.smxFreightShipmentID = dataTable.Rows[i].Field<string>("smxFreightShipmentID");
				eRPShipmentFreightLinkInformationDto.smxClosed = dataTable.Rows[i].Field<bool>("smxClosed");
				eRPShipmentFreightLinkInformationDto.smxLinkPctCharge = dataTable.Rows[i].Field<decimal>("smxLinkPctCharge");
				eRPShipmentFreightLinkInformationDto.smxPackagePartialCount = dataTable.Rows[i].Field<decimal>("smxPackagePartialCount");
				eRPShipmentFreightLinkInformationDto.smxPackagePartialWeight = dataTable.Rows[i].Field<decimal>("smxPackagePartialWeight");
				eRPShipmentFreightLinkInformationDto.smxRowVersion = dataTable.Rows[i].Field<byte[]>("smxRowVersion");
				eRPShipmentFreightLinkInformationDto.smxShipmentFreightLinkID = dataTable.Rows[i].Field<short>("smxShipmentFreightLinkID");
				eRPShipmentFreightLinkInformationDto.smxShipmentID = dataTable.Rows[i].Field<string>("smxShipmentID");
				eRPShipmentFreightLinkInformationDto.smxShipmentLineID = dataTable.Rows[i].Field<short>("smxShipmentLineID");
				eRPShipmentFreightLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentFreightLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentFreightLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentFreightLinkInformationDto> GetShipmentFreightLink(Guid shipmentFreightLinkId)
	{
		ERPShipmentFreightLinkInformationDto eRPShipmentFreightLinkInformationDto = new ERPShipmentFreightLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"smxCreatedBy", "smxCreatedDate", "smxUniqueID", "smxFreightCharges", "smxFreightPackageID", "smxFreightShipmentID", "smxClosed", "smxLinkPctCharge", "smxPackagePartialCount", "smxPackagePartialWeight",
			"smxRowVersion", "smxShipmentFreightLinkID", "smxShipmentID", "smxShipmentLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("smxUniqueID|C", shipmentFreightLinkId);
		AddCustomFieldsToSelectList("ShipmentFreightLinks");
		using (DataTable dataTable = GetAsDataTable("ShipmentFreightLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentFreightLinkInformationDto);
			}
			eRPShipmentFreightLinkInformationDto.smxCreatedBy = dataTable.Rows[0].Field<string>("smxCreatedBy");
			eRPShipmentFreightLinkInformationDto.smxCreatedDate = dataTable.Rows[0].Field<DateTime?>("smxCreatedDate");
			eRPShipmentFreightLinkInformationDto.smxUniqueID = dataTable.Rows[0].Field<Guid>("smxUniqueID");
			eRPShipmentFreightLinkInformationDto.smxFreightCharges = dataTable.Rows[0].Field<decimal>("smxFreightCharges");
			eRPShipmentFreightLinkInformationDto.smxFreightPackageID = dataTable.Rows[0].Field<short>("smxFreightPackageID");
			eRPShipmentFreightLinkInformationDto.smxFreightShipmentID = dataTable.Rows[0].Field<string>("smxFreightShipmentID");
			eRPShipmentFreightLinkInformationDto.smxClosed = dataTable.Rows[0].Field<bool>("smxClosed");
			eRPShipmentFreightLinkInformationDto.smxLinkPctCharge = dataTable.Rows[0].Field<decimal>("smxLinkPctCharge");
			eRPShipmentFreightLinkInformationDto.smxPackagePartialCount = dataTable.Rows[0].Field<decimal>("smxPackagePartialCount");
			eRPShipmentFreightLinkInformationDto.smxPackagePartialWeight = dataTable.Rows[0].Field<decimal>("smxPackagePartialWeight");
			eRPShipmentFreightLinkInformationDto.smxRowVersion = dataTable.Rows[0].Field<byte[]>("smxRowVersion");
			eRPShipmentFreightLinkInformationDto.smxShipmentFreightLinkID = dataTable.Rows[0].Field<short>("smxShipmentFreightLinkID");
			eRPShipmentFreightLinkInformationDto.smxShipmentID = dataTable.Rows[0].Field<string>("smxShipmentID");
			eRPShipmentFreightLinkInformationDto.smxShipmentLineID = dataTable.Rows[0].Field<short>("smxShipmentLineID");
			eRPShipmentFreightLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentFreightLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentFreightLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipmentFreightLink(ERPShipmentFreightLinkDto shipmentFreightLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShipmentFreightLinks WHERE smxUniqueID = " + M1Util.ConvertToLinq(shipmentFreightLink.smxUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["smxFreightShipmentID"] = shipmentFreightLink.smxFreightShipmentID.ToUpper();
				dataRow["smxFreightPackageID"] = shipmentFreightLink.smxFreightPackageID;
				dataRow["smxShipmentFreightLinkID"] = shipmentFreightLink.smxShipmentFreightLinkID;
				shipmentFreightLink.smxUniqueID = ((shipmentFreightLink.smxUniqueID == Guid.Empty) ? Guid.NewGuid() : shipmentFreightLink.smxUniqueID);
				dataRow["smxUniqueID"] = shipmentFreightLink.smxUniqueID;
				dataRow["smxCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["smxCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShipmentFreightLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipmentFreightLink.smxRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShipmentFreightLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["smxRowVersion"], shipmentFreightLink.smxRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShipmentFreightLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShipmentFreightLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["smxFreightCharges"] = shipmentFreightLink.smxFreightCharges;
			dataRow["smxClosed"] = shipmentFreightLink.smxClosed;
			dataRow["smxLinkPctCharge"] = shipmentFreightLink.smxLinkPctCharge;
			dataRow["smxPackagePartialCount"] = shipmentFreightLink.smxPackagePartialCount;
			dataRow["smxPackagePartialWeight"] = shipmentFreightLink.smxPackagePartialWeight;
			dataRow["smxShipmentID"] = shipmentFreightLink.smxShipmentID;
			dataRow["smxShipmentLineID"] = shipmentFreightLink.smxShipmentLineID;
			if (shipmentFreightLink.CustomFields != null && shipmentFreightLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipmentFreightLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShipmentFreightLink [{shipmentFreightLink.smxUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShipmentFreightLink [{shipmentFreightLink.smxUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
