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

public class ERPRMAReceiptComponentRepository : APIBaseRepository, IERPRMAReceiptComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAReceiptComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAReceiptComponentExist(Guid rMAReceiptComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("rroUniqueID|C", rMAReceiptComponentId);
		base.selectList.Add("rroUniqueID");
		return Task.FromResult(GetAsObject("RMAReceiptComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAReceiptComponentInformationDto>> GetAllRMAReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAReceiptComponentInformationDto> collection = new List<ERPRMAReceiptComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[35]
		{
			"rroAdditionalQuantity", "rroCreatedBy", "rroCreatedDate", "rroDescription", "rroUniqueID", "rroExtendedCost", "rroExtendedCostForeign", "rroInspParentQuantity", "rroClosed", "rroInspectionComplete",
			"rroPosted", "rroReceivedComplete", "rroReversed", "rroParentQuantity", "rroPartBinID", "rroPartID", "rroPartRevisionID", "rroPartWarehouseLocationID", "rroQuantityPerParent", "rroQuantityReceived",
			"rroQuantityToInspect", "rroReverseRmaReceiptCompID", "rroReverseRmaReceiptID", "rroReverseRmaReceiptLineID", "rroRmaClaimComponentID", "rroRmaClaimID", "rroRmaClaimLineID", "rroRmaReceiptID", "rroRmaReceiptLineID", "rroRowVersion",
			"rroRmaReceiptComponentID", "rroUnitCost", "rroUnitCostForeign", "rroUnitOfMeasure", "rroWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAReceiptComponents");
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
		using (DataTable dataTable = GetAsDataTable("RMAReceiptComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAReceiptComponentInformationDto eRPRMAReceiptComponentInformationDto = new ERPRMAReceiptComponentInformationDto();
				eRPRMAReceiptComponentInformationDto.rroAdditionalQuantity = dataTable.Rows[i].Field<decimal>("rroAdditionalQuantity");
				eRPRMAReceiptComponentInformationDto.rroCreatedBy = dataTable.Rows[i].Field<string>("rroCreatedBy");
				eRPRMAReceiptComponentInformationDto.rroCreatedDate = dataTable.Rows[i].Field<DateTime?>("rroCreatedDate");
				eRPRMAReceiptComponentInformationDto.rroDescription = dataTable.Rows[i].Field<string>("rroDescription");
				eRPRMAReceiptComponentInformationDto.rroUniqueID = dataTable.Rows[i].Field<Guid>("rroUniqueID");
				eRPRMAReceiptComponentInformationDto.rroExtendedCost = dataTable.Rows[i].Field<decimal>("rroExtendedCost");
				eRPRMAReceiptComponentInformationDto.rroExtendedCostForeign = dataTable.Rows[i].Field<decimal>("rroExtendedCostForeign");
				eRPRMAReceiptComponentInformationDto.rroInspParentQuantity = dataTable.Rows[i].Field<decimal>("rroInspParentQuantity");
				eRPRMAReceiptComponentInformationDto.rroClosed = dataTable.Rows[i].Field<bool>("rroClosed");
				eRPRMAReceiptComponentInformationDto.rroInspectionComplete = dataTable.Rows[i].Field<bool>("rroInspectionComplete");
				eRPRMAReceiptComponentInformationDto.rroPosted = dataTable.Rows[i].Field<bool>("rroPosted");
				eRPRMAReceiptComponentInformationDto.rroReceivedComplete = dataTable.Rows[i].Field<bool>("rroReceivedComplete");
				eRPRMAReceiptComponentInformationDto.rroReversed = dataTable.Rows[i].Field<bool>("rroReversed");
				eRPRMAReceiptComponentInformationDto.rroParentQuantity = dataTable.Rows[i].Field<decimal>("rroParentQuantity");
				eRPRMAReceiptComponentInformationDto.rroPartBinID = dataTable.Rows[i].Field<string>("rroPartBinID");
				eRPRMAReceiptComponentInformationDto.rroPartID = dataTable.Rows[i].Field<string>("rroPartID");
				eRPRMAReceiptComponentInformationDto.rroPartRevisionID = dataTable.Rows[i].Field<string>("rroPartRevisionID");
				eRPRMAReceiptComponentInformationDto.rroPartWarehouseLocationID = dataTable.Rows[i].Field<string>("rroPartWarehouseLocationID");
				eRPRMAReceiptComponentInformationDto.rroQuantityPerParent = dataTable.Rows[i].Field<decimal>("rroQuantityPerParent");
				eRPRMAReceiptComponentInformationDto.rroQuantityReceived = dataTable.Rows[i].Field<decimal>("rroQuantityReceived");
				eRPRMAReceiptComponentInformationDto.rroQuantityToInspect = dataTable.Rows[i].Field<decimal>("rroQuantityToInspect");
				eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptCompID = dataTable.Rows[i].Field<int>("rroReverseRmaReceiptCompID");
				eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptID = dataTable.Rows[i].Field<string>("rroReverseRmaReceiptID");
				eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptLineID = dataTable.Rows[i].Field<short>("rroReverseRmaReceiptLineID");
				eRPRMAReceiptComponentInformationDto.rroRmaClaimComponentID = dataTable.Rows[i].Field<int>("rroRmaClaimComponentID");
				eRPRMAReceiptComponentInformationDto.rroRmaClaimID = dataTable.Rows[i].Field<string>("rroRmaClaimID");
				eRPRMAReceiptComponentInformationDto.rroRmaClaimLineID = dataTable.Rows[i].Field<short>("rroRmaClaimLineID");
				eRPRMAReceiptComponentInformationDto.rroRmaReceiptID = dataTable.Rows[i].Field<string>("rroRmaReceiptID");
				eRPRMAReceiptComponentInformationDto.rroRmaReceiptLineID = dataTable.Rows[i].Field<short>("rroRmaReceiptLineID");
				eRPRMAReceiptComponentInformationDto.rroRowVersion = dataTable.Rows[i].Field<byte[]>("rroRowVersion");
				eRPRMAReceiptComponentInformationDto.rroRmaReceiptComponentID = dataTable.Rows[i].Field<int>("rroRmaReceiptComponentID");
				eRPRMAReceiptComponentInformationDto.rroUnitCost = dataTable.Rows[i].Field<decimal>("rroUnitCost");
				eRPRMAReceiptComponentInformationDto.rroUnitCostForeign = dataTable.Rows[i].Field<decimal>("rroUnitCostForeign");
				eRPRMAReceiptComponentInformationDto.rroUnitOfMeasure = dataTable.Rows[i].Field<string>("rroUnitOfMeasure");
				eRPRMAReceiptComponentInformationDto.rroWeight = dataTable.Rows[i].Field<decimal>("rroWeight");
				eRPRMAReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAReceiptComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAReceiptComponentInformationDto> GetRMAReceiptComponent(Guid rMAReceiptComponentId)
	{
		ERPRMAReceiptComponentInformationDto eRPRMAReceiptComponentInformationDto = new ERPRMAReceiptComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[35]
		{
			"rroAdditionalQuantity", "rroCreatedBy", "rroCreatedDate", "rroDescription", "rroUniqueID", "rroExtendedCost", "rroExtendedCostForeign", "rroInspParentQuantity", "rroClosed", "rroInspectionComplete",
			"rroPosted", "rroReceivedComplete", "rroReversed", "rroParentQuantity", "rroPartBinID", "rroPartID", "rroPartRevisionID", "rroPartWarehouseLocationID", "rroQuantityPerParent", "rroQuantityReceived",
			"rroQuantityToInspect", "rroReverseRmaReceiptCompID", "rroReverseRmaReceiptID", "rroReverseRmaReceiptLineID", "rroRmaClaimComponentID", "rroRmaClaimID", "rroRmaClaimLineID", "rroRmaReceiptID", "rroRmaReceiptLineID", "rroRowVersion",
			"rroRmaReceiptComponentID", "rroUnitCost", "rroUnitCostForeign", "rroUnitOfMeasure", "rroWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rroUniqueID|C", rMAReceiptComponentId);
		AddCustomFieldsToSelectList("RMAReceiptComponents");
		using (DataTable dataTable = GetAsDataTable("RMAReceiptComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAReceiptComponentInformationDto);
			}
			eRPRMAReceiptComponentInformationDto.rroAdditionalQuantity = dataTable.Rows[0].Field<decimal>("rroAdditionalQuantity");
			eRPRMAReceiptComponentInformationDto.rroCreatedBy = dataTable.Rows[0].Field<string>("rroCreatedBy");
			eRPRMAReceiptComponentInformationDto.rroCreatedDate = dataTable.Rows[0].Field<DateTime?>("rroCreatedDate");
			eRPRMAReceiptComponentInformationDto.rroDescription = dataTable.Rows[0].Field<string>("rroDescription");
			eRPRMAReceiptComponentInformationDto.rroUniqueID = dataTable.Rows[0].Field<Guid>("rroUniqueID");
			eRPRMAReceiptComponentInformationDto.rroExtendedCost = dataTable.Rows[0].Field<decimal>("rroExtendedCost");
			eRPRMAReceiptComponentInformationDto.rroExtendedCostForeign = dataTable.Rows[0].Field<decimal>("rroExtendedCostForeign");
			eRPRMAReceiptComponentInformationDto.rroInspParentQuantity = dataTable.Rows[0].Field<decimal>("rroInspParentQuantity");
			eRPRMAReceiptComponentInformationDto.rroClosed = dataTable.Rows[0].Field<bool>("rroClosed");
			eRPRMAReceiptComponentInformationDto.rroInspectionComplete = dataTable.Rows[0].Field<bool>("rroInspectionComplete");
			eRPRMAReceiptComponentInformationDto.rroPosted = dataTable.Rows[0].Field<bool>("rroPosted");
			eRPRMAReceiptComponentInformationDto.rroReceivedComplete = dataTable.Rows[0].Field<bool>("rroReceivedComplete");
			eRPRMAReceiptComponentInformationDto.rroReversed = dataTable.Rows[0].Field<bool>("rroReversed");
			eRPRMAReceiptComponentInformationDto.rroParentQuantity = dataTable.Rows[0].Field<decimal>("rroParentQuantity");
			eRPRMAReceiptComponentInformationDto.rroPartBinID = dataTable.Rows[0].Field<string>("rroPartBinID");
			eRPRMAReceiptComponentInformationDto.rroPartID = dataTable.Rows[0].Field<string>("rroPartID");
			eRPRMAReceiptComponentInformationDto.rroPartRevisionID = dataTable.Rows[0].Field<string>("rroPartRevisionID");
			eRPRMAReceiptComponentInformationDto.rroPartWarehouseLocationID = dataTable.Rows[0].Field<string>("rroPartWarehouseLocationID");
			eRPRMAReceiptComponentInformationDto.rroQuantityPerParent = dataTable.Rows[0].Field<decimal>("rroQuantityPerParent");
			eRPRMAReceiptComponentInformationDto.rroQuantityReceived = dataTable.Rows[0].Field<decimal>("rroQuantityReceived");
			eRPRMAReceiptComponentInformationDto.rroQuantityToInspect = dataTable.Rows[0].Field<decimal>("rroQuantityToInspect");
			eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptCompID = dataTable.Rows[0].Field<int>("rroReverseRmaReceiptCompID");
			eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptID = dataTable.Rows[0].Field<string>("rroReverseRmaReceiptID");
			eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptLineID = dataTable.Rows[0].Field<short>("rroReverseRmaReceiptLineID");
			eRPRMAReceiptComponentInformationDto.rroRmaClaimComponentID = dataTable.Rows[0].Field<int>("rroRmaClaimComponentID");
			eRPRMAReceiptComponentInformationDto.rroRmaClaimID = dataTable.Rows[0].Field<string>("rroRmaClaimID");
			eRPRMAReceiptComponentInformationDto.rroRmaClaimLineID = dataTable.Rows[0].Field<short>("rroRmaClaimLineID");
			eRPRMAReceiptComponentInformationDto.rroRmaReceiptID = dataTable.Rows[0].Field<string>("rroRmaReceiptID");
			eRPRMAReceiptComponentInformationDto.rroRmaReceiptLineID = dataTable.Rows[0].Field<short>("rroRmaReceiptLineID");
			eRPRMAReceiptComponentInformationDto.rroRowVersion = dataTable.Rows[0].Field<byte[]>("rroRowVersion");
			eRPRMAReceiptComponentInformationDto.rroRmaReceiptComponentID = dataTable.Rows[0].Field<int>("rroRmaReceiptComponentID");
			eRPRMAReceiptComponentInformationDto.rroUnitCost = dataTable.Rows[0].Field<decimal>("rroUnitCost");
			eRPRMAReceiptComponentInformationDto.rroUnitCostForeign = dataTable.Rows[0].Field<decimal>("rroUnitCostForeign");
			eRPRMAReceiptComponentInformationDto.rroUnitOfMeasure = dataTable.Rows[0].Field<string>("rroUnitOfMeasure");
			eRPRMAReceiptComponentInformationDto.rroWeight = dataTable.Rows[0].Field<decimal>("rroWeight");
			eRPRMAReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAReceiptComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRMAReceiptComponent(ERPRMAReceiptComponentDto rMAReceiptComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RMAReceiptComponents WHERE rroUniqueID = " + M1Util.ConvertToLinq(rMAReceiptComponent.rroUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rroRmaReceiptID"] = rMAReceiptComponent.rroRmaReceiptID.ToUpper();
				dataRow["rroRmaReceiptLineID"] = rMAReceiptComponent.rroRmaReceiptLineID;
				dataRow["rroRmaReceiptComponentID"] = rMAReceiptComponent.rroRmaReceiptComponentID;
				rMAReceiptComponent.rroUniqueID = ((rMAReceiptComponent.rroUniqueID == Guid.Empty) ? Guid.NewGuid() : rMAReceiptComponent.rroUniqueID);
				dataRow["rroUniqueID"] = rMAReceiptComponent.rroUniqueID;
				dataRow["rroCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rroCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RMAReceiptComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rMAReceiptComponent.rroRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RMAReceiptComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rroRowVersion"], rMAReceiptComponent.rroRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RMAReceiptComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RMAReceiptComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rroAdditionalQuantity"] = rMAReceiptComponent.rroAdditionalQuantity;
			dataRow["rroDescription"] = rMAReceiptComponent.rroDescription;
			dataRow["rroExtendedCost"] = rMAReceiptComponent.rroExtendedCost;
			dataRow["rroExtendedCostForeign"] = rMAReceiptComponent.rroExtendedCostForeign;
			dataRow["rroInspParentQuantity"] = rMAReceiptComponent.rroInspParentQuantity;
			dataRow["rroClosed"] = rMAReceiptComponent.rroClosed;
			dataRow["rroInspectionComplete"] = rMAReceiptComponent.rroInspectionComplete;
			dataRow["rroPosted"] = rMAReceiptComponent.rroPosted;
			dataRow["rroReceivedComplete"] = rMAReceiptComponent.rroReceivedComplete;
			dataRow["rroReversed"] = rMAReceiptComponent.rroReversed;
			dataRow["rroParentQuantity"] = rMAReceiptComponent.rroParentQuantity;
			dataRow["rroPartBinID"] = rMAReceiptComponent.rroPartBinID;
			dataRow["rroPartID"] = rMAReceiptComponent.rroPartID;
			dataRow["rroPartRevisionID"] = rMAReceiptComponent.rroPartRevisionID;
			dataRow["rroPartWarehouseLocationID"] = rMAReceiptComponent.rroPartWarehouseLocationID;
			dataRow["rroQuantityPerParent"] = rMAReceiptComponent.rroQuantityPerParent;
			dataRow["rroQuantityReceived"] = rMAReceiptComponent.rroQuantityReceived;
			dataRow["rroQuantityToInspect"] = rMAReceiptComponent.rroQuantityToInspect;
			dataRow["rroReverseRmaReceiptCompID"] = rMAReceiptComponent.rroReverseRmaReceiptCompID;
			dataRow["rroReverseRmaReceiptID"] = rMAReceiptComponent.rroReverseRmaReceiptID;
			dataRow["rroReverseRmaReceiptLineID"] = rMAReceiptComponent.rroReverseRmaReceiptLineID;
			dataRow["rroRmaClaimComponentID"] = rMAReceiptComponent.rroRmaClaimComponentID;
			dataRow["rroRmaClaimID"] = rMAReceiptComponent.rroRmaClaimID;
			dataRow["rroRmaClaimLineID"] = rMAReceiptComponent.rroRmaClaimLineID;
			dataRow["rroUnitCost"] = rMAReceiptComponent.rroUnitCost;
			dataRow["rroUnitCostForeign"] = rMAReceiptComponent.rroUnitCostForeign;
			dataRow["rroUnitOfMeasure"] = rMAReceiptComponent.rroUnitOfMeasure;
			dataRow["rroWeight"] = rMAReceiptComponent.rroWeight;
			if (rMAReceiptComponent.CustomFields != null && rMAReceiptComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rMAReceiptComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RMAReceiptComponent [{rMAReceiptComponent.rroUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RMAReceiptComponent [{rMAReceiptComponent.rroUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
