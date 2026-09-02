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

public class ERPRMAClaimComponentRepository : APIBaseRepository, IERPRMAClaimComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAClaimComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAClaimComponentExist(Guid rMAClaimComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("raoUniqueID|C", rMAClaimComponentId);
		base.selectList.Add("raoUniqueID");
		return Task.FromResult(GetAsObject("RMAClaimComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAClaimComponentInformationDto>> GetAllRMAClaimComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAClaimComponentInformationDto> collection = new List<ERPRMAClaimComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"raoAdditionalQuantity", "raoCreatedBy", "raoCreatedDate", "raoDescription", "raoUniqueID", "raoReceivedComplete", "raoParentQuantity", "raoPartBinID", "raoPartID", "raoPartRevisionID",
			"raoPartWarehouseLocationID", "raoQuantity", "raoQuantityPerParent", "raoQuantityReceived", "raoRmaClaimID", "raoRmaClaimLineID", "raoRowVersion", "raoRmaClaimComponentID", "raoShipmentComponentID", "raoShipmentID",
			"raoShipmentLineID", "raoUnitOfMeasure", "raoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAClaimComponents");
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
		using (DataTable dataTable = GetAsDataTable("RMAClaimComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAClaimComponentInformationDto eRPRMAClaimComponentInformationDto = new ERPRMAClaimComponentInformationDto();
				eRPRMAClaimComponentInformationDto.raoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("raoAdditionalQuantity");
				eRPRMAClaimComponentInformationDto.raoCreatedBy = dataTable.Rows[i].Field<string>("raoCreatedBy");
				eRPRMAClaimComponentInformationDto.raoCreatedDate = dataTable.Rows[i].Field<DateTime?>("raoCreatedDate");
				eRPRMAClaimComponentInformationDto.raoDescription = dataTable.Rows[i].Field<string>("raoDescription");
				eRPRMAClaimComponentInformationDto.raoUniqueID = dataTable.Rows[i].Field<Guid>("raoUniqueID");
				eRPRMAClaimComponentInformationDto.raoReceivedComplete = dataTable.Rows[i].Field<bool>("raoReceivedComplete");
				eRPRMAClaimComponentInformationDto.raoParentQuantity = dataTable.Rows[i].Field<decimal>("raoParentQuantity");
				eRPRMAClaimComponentInformationDto.raoPartBinID = dataTable.Rows[i].Field<string>("raoPartBinID");
				eRPRMAClaimComponentInformationDto.raoPartID = dataTable.Rows[i].Field<string>("raoPartID");
				eRPRMAClaimComponentInformationDto.raoPartRevisionID = dataTable.Rows[i].Field<string>("raoPartRevisionID");
				eRPRMAClaimComponentInformationDto.raoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("raoPartWarehouseLocationID");
				eRPRMAClaimComponentInformationDto.raoQuantity = dataTable.Rows[i].Field<decimal>("raoQuantity");
				eRPRMAClaimComponentInformationDto.raoQuantityPerParent = dataTable.Rows[i].Field<decimal>("raoQuantityPerParent");
				eRPRMAClaimComponentInformationDto.raoQuantityReceived = dataTable.Rows[i].Field<decimal>("raoQuantityReceived");
				eRPRMAClaimComponentInformationDto.raoRmaClaimID = dataTable.Rows[i].Field<string>("raoRmaClaimID");
				eRPRMAClaimComponentInformationDto.raoRmaClaimLineID = dataTable.Rows[i].Field<short>("raoRmaClaimLineID");
				eRPRMAClaimComponentInformationDto.raoRowVersion = dataTable.Rows[i].Field<byte[]>("raoRowVersion");
				eRPRMAClaimComponentInformationDto.raoRmaClaimComponentID = dataTable.Rows[i].Field<int>("raoRmaClaimComponentID");
				eRPRMAClaimComponentInformationDto.raoShipmentComponentID = dataTable.Rows[i].Field<short>("raoShipmentComponentID");
				eRPRMAClaimComponentInformationDto.raoShipmentID = dataTable.Rows[i].Field<string>("raoShipmentID");
				eRPRMAClaimComponentInformationDto.raoShipmentLineID = dataTable.Rows[i].Field<short>("raoShipmentLineID");
				eRPRMAClaimComponentInformationDto.raoUnitOfMeasure = dataTable.Rows[i].Field<string>("raoUnitOfMeasure");
				eRPRMAClaimComponentInformationDto.raoWeight = dataTable.Rows[i].Field<decimal>("raoWeight");
				eRPRMAClaimComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAClaimComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAClaimComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAClaimComponentInformationDto> GetRMAClaimComponent(Guid rMAClaimComponentId)
	{
		ERPRMAClaimComponentInformationDto eRPRMAClaimComponentInformationDto = new ERPRMAClaimComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"raoAdditionalQuantity", "raoCreatedBy", "raoCreatedDate", "raoDescription", "raoUniqueID", "raoReceivedComplete", "raoParentQuantity", "raoPartBinID", "raoPartID", "raoPartRevisionID",
			"raoPartWarehouseLocationID", "raoQuantity", "raoQuantityPerParent", "raoQuantityReceived", "raoRmaClaimID", "raoRmaClaimLineID", "raoRowVersion", "raoRmaClaimComponentID", "raoShipmentComponentID", "raoShipmentID",
			"raoShipmentLineID", "raoUnitOfMeasure", "raoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("raoUniqueID|C", rMAClaimComponentId);
		AddCustomFieldsToSelectList("RMAClaimComponents");
		using (DataTable dataTable = GetAsDataTable("RMAClaimComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAClaimComponentInformationDto);
			}
			eRPRMAClaimComponentInformationDto.raoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("raoAdditionalQuantity");
			eRPRMAClaimComponentInformationDto.raoCreatedBy = dataTable.Rows[0].Field<string>("raoCreatedBy");
			eRPRMAClaimComponentInformationDto.raoCreatedDate = dataTable.Rows[0].Field<DateTime?>("raoCreatedDate");
			eRPRMAClaimComponentInformationDto.raoDescription = dataTable.Rows[0].Field<string>("raoDescription");
			eRPRMAClaimComponentInformationDto.raoUniqueID = dataTable.Rows[0].Field<Guid>("raoUniqueID");
			eRPRMAClaimComponentInformationDto.raoReceivedComplete = dataTable.Rows[0].Field<bool>("raoReceivedComplete");
			eRPRMAClaimComponentInformationDto.raoParentQuantity = dataTable.Rows[0].Field<decimal>("raoParentQuantity");
			eRPRMAClaimComponentInformationDto.raoPartBinID = dataTable.Rows[0].Field<string>("raoPartBinID");
			eRPRMAClaimComponentInformationDto.raoPartID = dataTable.Rows[0].Field<string>("raoPartID");
			eRPRMAClaimComponentInformationDto.raoPartRevisionID = dataTable.Rows[0].Field<string>("raoPartRevisionID");
			eRPRMAClaimComponentInformationDto.raoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("raoPartWarehouseLocationID");
			eRPRMAClaimComponentInformationDto.raoQuantity = dataTable.Rows[0].Field<decimal>("raoQuantity");
			eRPRMAClaimComponentInformationDto.raoQuantityPerParent = dataTable.Rows[0].Field<decimal>("raoQuantityPerParent");
			eRPRMAClaimComponentInformationDto.raoQuantityReceived = dataTable.Rows[0].Field<decimal>("raoQuantityReceived");
			eRPRMAClaimComponentInformationDto.raoRmaClaimID = dataTable.Rows[0].Field<string>("raoRmaClaimID");
			eRPRMAClaimComponentInformationDto.raoRmaClaimLineID = dataTable.Rows[0].Field<short>("raoRmaClaimLineID");
			eRPRMAClaimComponentInformationDto.raoRowVersion = dataTable.Rows[0].Field<byte[]>("raoRowVersion");
			eRPRMAClaimComponentInformationDto.raoRmaClaimComponentID = dataTable.Rows[0].Field<int>("raoRmaClaimComponentID");
			eRPRMAClaimComponentInformationDto.raoShipmentComponentID = dataTable.Rows[0].Field<short>("raoShipmentComponentID");
			eRPRMAClaimComponentInformationDto.raoShipmentID = dataTable.Rows[0].Field<string>("raoShipmentID");
			eRPRMAClaimComponentInformationDto.raoShipmentLineID = dataTable.Rows[0].Field<short>("raoShipmentLineID");
			eRPRMAClaimComponentInformationDto.raoUnitOfMeasure = dataTable.Rows[0].Field<string>("raoUnitOfMeasure");
			eRPRMAClaimComponentInformationDto.raoWeight = dataTable.Rows[0].Field<decimal>("raoWeight");
			eRPRMAClaimComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAClaimComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAClaimComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRMAClaimComponent(ERPRMAClaimComponentDto rMAClaimComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RMAClaimComponents WHERE raoUniqueID = " + M1Util.ConvertToLinq(rMAClaimComponent.raoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["raoRmaClaimID"] = rMAClaimComponent.raoRmaClaimID.ToUpper();
				dataRow["raoRmaClaimLineID"] = rMAClaimComponent.raoRmaClaimLineID;
				dataRow["raoRmaClaimComponentID"] = rMAClaimComponent.raoRmaClaimComponentID;
				rMAClaimComponent.raoUniqueID = ((rMAClaimComponent.raoUniqueID == Guid.Empty) ? Guid.NewGuid() : rMAClaimComponent.raoUniqueID);
				dataRow["raoUniqueID"] = rMAClaimComponent.raoUniqueID;
				dataRow["raoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["raoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RMAClaimComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rMAClaimComponent.raoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RMAClaimComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["raoRowVersion"], rMAClaimComponent.raoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RMAClaimComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RMAClaimComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["raoAdditionalQuantity"] = rMAClaimComponent.raoAdditionalQuantity;
			dataRow["raoDescription"] = rMAClaimComponent.raoDescription;
			dataRow["raoReceivedComplete"] = rMAClaimComponent.raoReceivedComplete;
			dataRow["raoParentQuantity"] = rMAClaimComponent.raoParentQuantity;
			dataRow["raoPartBinID"] = rMAClaimComponent.raoPartBinID;
			dataRow["raoPartID"] = rMAClaimComponent.raoPartID;
			dataRow["raoPartRevisionID"] = rMAClaimComponent.raoPartRevisionID;
			dataRow["raoPartWarehouseLocationID"] = rMAClaimComponent.raoPartWarehouseLocationID;
			dataRow["raoQuantity"] = rMAClaimComponent.raoQuantity;
			dataRow["raoQuantityPerParent"] = rMAClaimComponent.raoQuantityPerParent;
			dataRow["raoQuantityReceived"] = rMAClaimComponent.raoQuantityReceived;
			dataRow["raoShipmentComponentID"] = rMAClaimComponent.raoShipmentComponentID;
			dataRow["raoShipmentID"] = rMAClaimComponent.raoShipmentID;
			dataRow["raoShipmentLineID"] = rMAClaimComponent.raoShipmentLineID;
			dataRow["raoUnitOfMeasure"] = rMAClaimComponent.raoUnitOfMeasure;
			dataRow["raoWeight"] = rMAClaimComponent.raoWeight;
			if (rMAClaimComponent.CustomFields != null && rMAClaimComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rMAClaimComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RMAClaimComponent [{rMAClaimComponent.raoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RMAClaimComponent [{rMAClaimComponent.raoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
