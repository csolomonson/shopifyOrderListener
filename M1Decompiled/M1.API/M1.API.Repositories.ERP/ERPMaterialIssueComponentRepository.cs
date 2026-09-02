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

public class ERPMaterialIssueComponentRepository : APIBaseRepository, IERPMaterialIssueComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPMaterialIssueComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMaterialIssueComponentExist(Guid materialIssueComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("inkUniqueID|C", materialIssueComponentId);
		base.selectList.Add("inkUniqueID");
		return Task.FromResult(GetAsObject("MaterialIssueComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMaterialIssueComponentInformationDto>> GetAllMaterialIssueComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMaterialIssueComponentInformationDto> collection = new List<ERPMaterialIssueComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[38]
		{
			"inkAdditionalQuantity", "inkCreatedBy", "inkCreatedDate", "inkDescription", "inkUniqueID", "inkInvIssueQuantity", "inkInvParentQuantity", "inkInvParentQuantityScrap", "inkInvScrapQuantity", "inkPosted",
			"inkReceivedComplete", "inkReversed", "inkJobAssemblyID", "inkJobID", "inkJobMaterialComponentID", "inkJobMaterialID", "inkJobMatIssueQuantity", "inkJobMatParentQuantity", "inkJobMatParentQuantityScrap", "inkJobMatParentReturnQty",
			"inkJobMatParentReturnQtyScrap", "inkJobMatReturnIssueQuantity", "inkJobMatReturnScrapQuantity", "inkJobMatScrapQuantity", "inkMaterialIssueID", "inkMaterialIssueLineID", "inkPartBinID", "inkPartID", "inkPartRevisionID", "inkPartWarehouseLocationID",
			"inkQuantityPerParent", "inkReverseMaterialIssueCompID", "inkReverseMaterialIssueID", "inkReverseMaterialIssueLineID", "inkRowVersion", "inkMaterialIssueComponentID", "inkUnitOfMeasure", "inkWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MaterialIssueComponents");
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
		using (DataTable dataTable = GetAsDataTable("MaterialIssueComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMaterialIssueComponentInformationDto eRPMaterialIssueComponentInformationDto = new ERPMaterialIssueComponentInformationDto();
				eRPMaterialIssueComponentInformationDto.inkAdditionalQuantity = dataTable.Rows[i].Field<decimal>("inkAdditionalQuantity");
				eRPMaterialIssueComponentInformationDto.inkCreatedBy = dataTable.Rows[i].Field<string>("inkCreatedBy");
				eRPMaterialIssueComponentInformationDto.inkCreatedDate = dataTable.Rows[i].Field<DateTime?>("inkCreatedDate");
				eRPMaterialIssueComponentInformationDto.inkDescription = dataTable.Rows[i].Field<string>("inkDescription");
				eRPMaterialIssueComponentInformationDto.inkUniqueID = dataTable.Rows[i].Field<Guid>("inkUniqueID");
				eRPMaterialIssueComponentInformationDto.inkInvIssueQuantity = dataTable.Rows[i].Field<decimal>("inkInvIssueQuantity");
				eRPMaterialIssueComponentInformationDto.inkInvParentQuantity = dataTable.Rows[i].Field<decimal>("inkInvParentQuantity");
				eRPMaterialIssueComponentInformationDto.inkInvParentQuantityScrap = dataTable.Rows[i].Field<decimal>("inkInvParentQuantityScrap");
				eRPMaterialIssueComponentInformationDto.inkInvScrapQuantity = dataTable.Rows[i].Field<decimal>("inkInvScrapQuantity");
				eRPMaterialIssueComponentInformationDto.inkPosted = dataTable.Rows[i].Field<bool>("inkPosted");
				eRPMaterialIssueComponentInformationDto.inkReceivedComplete = dataTable.Rows[i].Field<bool>("inkReceivedComplete");
				eRPMaterialIssueComponentInformationDto.inkReversed = dataTable.Rows[i].Field<bool>("inkReversed");
				eRPMaterialIssueComponentInformationDto.inkJobAssemblyID = dataTable.Rows[i].Field<int>("inkJobAssemblyID");
				eRPMaterialIssueComponentInformationDto.inkJobID = dataTable.Rows[i].Field<string>("inkJobID");
				eRPMaterialIssueComponentInformationDto.inkJobMaterialComponentID = dataTable.Rows[i].Field<int>("inkJobMaterialComponentID");
				eRPMaterialIssueComponentInformationDto.inkJobMaterialID = dataTable.Rows[i].Field<int>("inkJobMaterialID");
				eRPMaterialIssueComponentInformationDto.inkJobMatIssueQuantity = dataTable.Rows[i].Field<decimal>("inkJobMatIssueQuantity");
				eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantity = dataTable.Rows[i].Field<decimal>("inkJobMatParentQuantity");
				eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantityScrap = dataTable.Rows[i].Field<decimal>("inkJobMatParentQuantityScrap");
				eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQty = dataTable.Rows[i].Field<decimal>("inkJobMatParentReturnQty");
				eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQtyScrap = dataTable.Rows[i].Field<decimal>("inkJobMatParentReturnQtyScrap");
				eRPMaterialIssueComponentInformationDto.inkJobMatReturnIssueQuantity = dataTable.Rows[i].Field<decimal>("inkJobMatReturnIssueQuantity");
				eRPMaterialIssueComponentInformationDto.inkJobMatReturnScrapQuantity = dataTable.Rows[i].Field<decimal>("inkJobMatReturnScrapQuantity");
				eRPMaterialIssueComponentInformationDto.inkJobMatScrapQuantity = dataTable.Rows[i].Field<decimal>("inkJobMatScrapQuantity");
				eRPMaterialIssueComponentInformationDto.inkMaterialIssueID = dataTable.Rows[i].Field<string>("inkMaterialIssueID");
				eRPMaterialIssueComponentInformationDto.inkMaterialIssueLineID = dataTable.Rows[i].Field<short>("inkMaterialIssueLineID");
				eRPMaterialIssueComponentInformationDto.inkPartBinID = dataTable.Rows[i].Field<string>("inkPartBinID");
				eRPMaterialIssueComponentInformationDto.inkPartID = dataTable.Rows[i].Field<string>("inkPartID");
				eRPMaterialIssueComponentInformationDto.inkPartRevisionID = dataTable.Rows[i].Field<string>("inkPartRevisionID");
				eRPMaterialIssueComponentInformationDto.inkPartWarehouseLocationID = dataTable.Rows[i].Field<string>("inkPartWarehouseLocationID");
				eRPMaterialIssueComponentInformationDto.inkQuantityPerParent = dataTable.Rows[i].Field<decimal>("inkQuantityPerParent");
				eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueCompID = dataTable.Rows[i].Field<int>("inkReverseMaterialIssueCompID");
				eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueID = dataTable.Rows[i].Field<string>("inkReverseMaterialIssueID");
				eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueLineID = dataTable.Rows[i].Field<short>("inkReverseMaterialIssueLineID");
				eRPMaterialIssueComponentInformationDto.inkRowVersion = dataTable.Rows[i].Field<byte[]>("inkRowVersion");
				eRPMaterialIssueComponentInformationDto.inkMaterialIssueComponentID = dataTable.Rows[i].Field<int>("inkMaterialIssueComponentID");
				eRPMaterialIssueComponentInformationDto.inkUnitOfMeasure = dataTable.Rows[i].Field<string>("inkUnitOfMeasure");
				eRPMaterialIssueComponentInformationDto.inkWeight = dataTable.Rows[i].Field<decimal>("inkWeight");
				eRPMaterialIssueComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMaterialIssueComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMaterialIssueComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMaterialIssueComponentInformationDto> GetMaterialIssueComponent(Guid materialIssueComponentId)
	{
		ERPMaterialIssueComponentInformationDto eRPMaterialIssueComponentInformationDto = new ERPMaterialIssueComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[38]
		{
			"inkAdditionalQuantity", "inkCreatedBy", "inkCreatedDate", "inkDescription", "inkUniqueID", "inkInvIssueQuantity", "inkInvParentQuantity", "inkInvParentQuantityScrap", "inkInvScrapQuantity", "inkPosted",
			"inkReceivedComplete", "inkReversed", "inkJobAssemblyID", "inkJobID", "inkJobMaterialComponentID", "inkJobMaterialID", "inkJobMatIssueQuantity", "inkJobMatParentQuantity", "inkJobMatParentQuantityScrap", "inkJobMatParentReturnQty",
			"inkJobMatParentReturnQtyScrap", "inkJobMatReturnIssueQuantity", "inkJobMatReturnScrapQuantity", "inkJobMatScrapQuantity", "inkMaterialIssueID", "inkMaterialIssueLineID", "inkPartBinID", "inkPartID", "inkPartRevisionID", "inkPartWarehouseLocationID",
			"inkQuantityPerParent", "inkReverseMaterialIssueCompID", "inkReverseMaterialIssueID", "inkReverseMaterialIssueLineID", "inkRowVersion", "inkMaterialIssueComponentID", "inkUnitOfMeasure", "inkWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("inkUniqueID|C", materialIssueComponentId);
		AddCustomFieldsToSelectList("MaterialIssueComponents");
		using (DataTable dataTable = GetAsDataTable("MaterialIssueComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMaterialIssueComponentInformationDto);
			}
			eRPMaterialIssueComponentInformationDto.inkAdditionalQuantity = dataTable.Rows[0].Field<decimal>("inkAdditionalQuantity");
			eRPMaterialIssueComponentInformationDto.inkCreatedBy = dataTable.Rows[0].Field<string>("inkCreatedBy");
			eRPMaterialIssueComponentInformationDto.inkCreatedDate = dataTable.Rows[0].Field<DateTime?>("inkCreatedDate");
			eRPMaterialIssueComponentInformationDto.inkDescription = dataTable.Rows[0].Field<string>("inkDescription");
			eRPMaterialIssueComponentInformationDto.inkUniqueID = dataTable.Rows[0].Field<Guid>("inkUniqueID");
			eRPMaterialIssueComponentInformationDto.inkInvIssueQuantity = dataTable.Rows[0].Field<decimal>("inkInvIssueQuantity");
			eRPMaterialIssueComponentInformationDto.inkInvParentQuantity = dataTable.Rows[0].Field<decimal>("inkInvParentQuantity");
			eRPMaterialIssueComponentInformationDto.inkInvParentQuantityScrap = dataTable.Rows[0].Field<decimal>("inkInvParentQuantityScrap");
			eRPMaterialIssueComponentInformationDto.inkInvScrapQuantity = dataTable.Rows[0].Field<decimal>("inkInvScrapQuantity");
			eRPMaterialIssueComponentInformationDto.inkPosted = dataTable.Rows[0].Field<bool>("inkPosted");
			eRPMaterialIssueComponentInformationDto.inkReceivedComplete = dataTable.Rows[0].Field<bool>("inkReceivedComplete");
			eRPMaterialIssueComponentInformationDto.inkReversed = dataTable.Rows[0].Field<bool>("inkReversed");
			eRPMaterialIssueComponentInformationDto.inkJobAssemblyID = dataTable.Rows[0].Field<int>("inkJobAssemblyID");
			eRPMaterialIssueComponentInformationDto.inkJobID = dataTable.Rows[0].Field<string>("inkJobID");
			eRPMaterialIssueComponentInformationDto.inkJobMaterialComponentID = dataTable.Rows[0].Field<int>("inkJobMaterialComponentID");
			eRPMaterialIssueComponentInformationDto.inkJobMaterialID = dataTable.Rows[0].Field<int>("inkJobMaterialID");
			eRPMaterialIssueComponentInformationDto.inkJobMatIssueQuantity = dataTable.Rows[0].Field<decimal>("inkJobMatIssueQuantity");
			eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantity = dataTable.Rows[0].Field<decimal>("inkJobMatParentQuantity");
			eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantityScrap = dataTable.Rows[0].Field<decimal>("inkJobMatParentQuantityScrap");
			eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQty = dataTable.Rows[0].Field<decimal>("inkJobMatParentReturnQty");
			eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQtyScrap = dataTable.Rows[0].Field<decimal>("inkJobMatParentReturnQtyScrap");
			eRPMaterialIssueComponentInformationDto.inkJobMatReturnIssueQuantity = dataTable.Rows[0].Field<decimal>("inkJobMatReturnIssueQuantity");
			eRPMaterialIssueComponentInformationDto.inkJobMatReturnScrapQuantity = dataTable.Rows[0].Field<decimal>("inkJobMatReturnScrapQuantity");
			eRPMaterialIssueComponentInformationDto.inkJobMatScrapQuantity = dataTable.Rows[0].Field<decimal>("inkJobMatScrapQuantity");
			eRPMaterialIssueComponentInformationDto.inkMaterialIssueID = dataTable.Rows[0].Field<string>("inkMaterialIssueID");
			eRPMaterialIssueComponentInformationDto.inkMaterialIssueLineID = dataTable.Rows[0].Field<short>("inkMaterialIssueLineID");
			eRPMaterialIssueComponentInformationDto.inkPartBinID = dataTable.Rows[0].Field<string>("inkPartBinID");
			eRPMaterialIssueComponentInformationDto.inkPartID = dataTable.Rows[0].Field<string>("inkPartID");
			eRPMaterialIssueComponentInformationDto.inkPartRevisionID = dataTable.Rows[0].Field<string>("inkPartRevisionID");
			eRPMaterialIssueComponentInformationDto.inkPartWarehouseLocationID = dataTable.Rows[0].Field<string>("inkPartWarehouseLocationID");
			eRPMaterialIssueComponentInformationDto.inkQuantityPerParent = dataTable.Rows[0].Field<decimal>("inkQuantityPerParent");
			eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueCompID = dataTable.Rows[0].Field<int>("inkReverseMaterialIssueCompID");
			eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueID = dataTable.Rows[0].Field<string>("inkReverseMaterialIssueID");
			eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueLineID = dataTable.Rows[0].Field<short>("inkReverseMaterialIssueLineID");
			eRPMaterialIssueComponentInformationDto.inkRowVersion = dataTable.Rows[0].Field<byte[]>("inkRowVersion");
			eRPMaterialIssueComponentInformationDto.inkMaterialIssueComponentID = dataTable.Rows[0].Field<int>("inkMaterialIssueComponentID");
			eRPMaterialIssueComponentInformationDto.inkUnitOfMeasure = dataTable.Rows[0].Field<string>("inkUnitOfMeasure");
			eRPMaterialIssueComponentInformationDto.inkWeight = dataTable.Rows[0].Field<decimal>("inkWeight");
			eRPMaterialIssueComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMaterialIssueComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMaterialIssueComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMaterialIssueComponent(ERPMaterialIssueComponentDto materialIssueComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MaterialIssueComponents WHERE inkUniqueID = " + M1Util.ConvertToLinq(materialIssueComponent.inkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["inkMaterialIssueID"] = materialIssueComponent.inkMaterialIssueID.ToUpper();
				dataRow["inkMaterialIssueLineID"] = materialIssueComponent.inkMaterialIssueLineID;
				dataRow["inkMaterialIssueComponentID"] = materialIssueComponent.inkMaterialIssueComponentID;
				materialIssueComponent.inkUniqueID = ((materialIssueComponent.inkUniqueID == Guid.Empty) ? Guid.NewGuid() : materialIssueComponent.inkUniqueID);
				dataRow["inkUniqueID"] = materialIssueComponent.inkUniqueID;
				dataRow["inkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["inkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MaterialIssueComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (materialIssueComponent.inkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MaterialIssueComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["inkRowVersion"], materialIssueComponent.inkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MaterialIssueComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MaterialIssueComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["inkAdditionalQuantity"] = materialIssueComponent.inkAdditionalQuantity;
			dataRow["inkDescription"] = materialIssueComponent.inkDescription;
			dataRow["inkInvIssueQuantity"] = materialIssueComponent.inkInvIssueQuantity;
			dataRow["inkInvParentQuantity"] = materialIssueComponent.inkInvParentQuantity;
			dataRow["inkInvParentQuantityScrap"] = materialIssueComponent.inkInvParentQuantityScrap;
			dataRow["inkInvScrapQuantity"] = materialIssueComponent.inkInvScrapQuantity;
			dataRow["inkPosted"] = materialIssueComponent.inkPosted;
			dataRow["inkReceivedComplete"] = materialIssueComponent.inkReceivedComplete;
			dataRow["inkReversed"] = materialIssueComponent.inkReversed;
			dataRow["inkJobAssemblyID"] = materialIssueComponent.inkJobAssemblyID;
			dataRow["inkJobID"] = materialIssueComponent.inkJobID;
			dataRow["inkJobMaterialComponentID"] = materialIssueComponent.inkJobMaterialComponentID;
			dataRow["inkJobMaterialID"] = materialIssueComponent.inkJobMaterialID;
			dataRow["inkJobMatIssueQuantity"] = materialIssueComponent.inkJobMatIssueQuantity;
			dataRow["inkJobMatParentQuantity"] = materialIssueComponent.inkJobMatParentQuantity;
			dataRow["inkJobMatParentQuantityScrap"] = materialIssueComponent.inkJobMatParentQuantityScrap;
			dataRow["inkJobMatParentReturnQty"] = materialIssueComponent.inkJobMatParentReturnQty;
			dataRow["inkJobMatParentReturnQtyScrap"] = materialIssueComponent.inkJobMatParentReturnQtyScrap;
			dataRow["inkJobMatReturnIssueQuantity"] = materialIssueComponent.inkJobMatReturnIssueQuantity;
			dataRow["inkJobMatReturnScrapQuantity"] = materialIssueComponent.inkJobMatReturnScrapQuantity;
			dataRow["inkJobMatScrapQuantity"] = materialIssueComponent.inkJobMatScrapQuantity;
			dataRow["inkPartBinID"] = materialIssueComponent.inkPartBinID;
			dataRow["inkPartID"] = materialIssueComponent.inkPartID;
			dataRow["inkPartRevisionID"] = materialIssueComponent.inkPartRevisionID;
			dataRow["inkPartWarehouseLocationID"] = materialIssueComponent.inkPartWarehouseLocationID;
			dataRow["inkQuantityPerParent"] = materialIssueComponent.inkQuantityPerParent;
			dataRow["inkReverseMaterialIssueCompID"] = materialIssueComponent.inkReverseMaterialIssueCompID;
			dataRow["inkReverseMaterialIssueID"] = materialIssueComponent.inkReverseMaterialIssueID;
			dataRow["inkReverseMaterialIssueLineID"] = materialIssueComponent.inkReverseMaterialIssueLineID;
			dataRow["inkUnitOfMeasure"] = materialIssueComponent.inkUnitOfMeasure;
			dataRow["inkWeight"] = materialIssueComponent.inkWeight;
			if (materialIssueComponent.CustomFields != null && materialIssueComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in materialIssueComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MaterialIssueComponent [{materialIssueComponent.inkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MaterialIssueComponent [{materialIssueComponent.inkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
