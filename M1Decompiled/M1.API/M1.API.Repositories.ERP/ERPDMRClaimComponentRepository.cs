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

public class ERPDMRClaimComponentRepository : APIBaseRepository, IERPDMRClaimComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPDMRClaimComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDMRClaimComponentExist(Guid dMRClaimComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("dmoUniqueID|C", dMRClaimComponentId);
		base.selectList.Add("dmoUniqueID");
		return Task.FromResult(GetAsObject("DMRClaimComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDMRClaimComponentInformationDto>> GetAllDMRClaimComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDMRClaimComponentInformationDto> collection = new List<ERPDMRClaimComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[27]
		{
			"dmoAdditionalQuantity", "dmoCreatedBy", "dmoCreatedDate", "dmoDescription", "dmoDmrClaimID", "dmoDmrClaimLineID", "dmoUniqueID", "dmoInspectionComponentID", "dmoInspectionID", "dmoInspectionLineID",
			"dmoShippedComplete", "dmoJobAssemblyID", "dmoJobID", "dmoJobMaterialComponentID", "dmoJobMaterialID", "dmoParentQuantity", "dmoPartBinID", "dmoPartID", "dmoPartRevisionID", "dmoPartWarehouseLocationID",
			"dmoQuantity", "dmoQuantityPerParent", "dmoQuantityShipped", "dmoRowVersion", "dmoDmrClaimComponentID", "dmoUnitOfMeasure", "dmoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DMRClaimComponents");
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
		using (DataTable dataTable = GetAsDataTable("DMRClaimComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDMRClaimComponentInformationDto eRPDMRClaimComponentInformationDto = new ERPDMRClaimComponentInformationDto();
				eRPDMRClaimComponentInformationDto.dmoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("dmoAdditionalQuantity");
				eRPDMRClaimComponentInformationDto.dmoCreatedBy = dataTable.Rows[i].Field<string>("dmoCreatedBy");
				eRPDMRClaimComponentInformationDto.dmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("dmoCreatedDate");
				eRPDMRClaimComponentInformationDto.dmoDescription = dataTable.Rows[i].Field<string>("dmoDescription");
				eRPDMRClaimComponentInformationDto.dmoDmrClaimID = dataTable.Rows[i].Field<string>("dmoDmrClaimID");
				eRPDMRClaimComponentInformationDto.dmoDmrClaimLineID = dataTable.Rows[i].Field<short>("dmoDmrClaimLineID");
				eRPDMRClaimComponentInformationDto.dmoUniqueID = dataTable.Rows[i].Field<Guid>("dmoUniqueID");
				eRPDMRClaimComponentInformationDto.dmoInspectionComponentID = dataTable.Rows[i].Field<int>("dmoInspectionComponentID");
				eRPDMRClaimComponentInformationDto.dmoInspectionID = dataTable.Rows[i].Field<string>("dmoInspectionID");
				eRPDMRClaimComponentInformationDto.dmoInspectionLineID = dataTable.Rows[i].Field<short>("dmoInspectionLineID");
				eRPDMRClaimComponentInformationDto.dmoShippedComplete = dataTable.Rows[i].Field<bool>("dmoShippedComplete");
				eRPDMRClaimComponentInformationDto.dmoJobAssemblyID = dataTable.Rows[i].Field<int>("dmoJobAssemblyID");
				eRPDMRClaimComponentInformationDto.dmoJobID = dataTable.Rows[i].Field<string>("dmoJobID");
				eRPDMRClaimComponentInformationDto.dmoJobMaterialComponentID = dataTable.Rows[i].Field<int>("dmoJobMaterialComponentID");
				eRPDMRClaimComponentInformationDto.dmoJobMaterialID = dataTable.Rows[i].Field<int>("dmoJobMaterialID");
				eRPDMRClaimComponentInformationDto.dmoParentQuantity = dataTable.Rows[i].Field<decimal>("dmoParentQuantity");
				eRPDMRClaimComponentInformationDto.dmoPartBinID = dataTable.Rows[i].Field<string>("dmoPartBinID");
				eRPDMRClaimComponentInformationDto.dmoPartID = dataTable.Rows[i].Field<string>("dmoPartID");
				eRPDMRClaimComponentInformationDto.dmoPartRevisionID = dataTable.Rows[i].Field<string>("dmoPartRevisionID");
				eRPDMRClaimComponentInformationDto.dmoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("dmoPartWarehouseLocationID");
				eRPDMRClaimComponentInformationDto.dmoQuantity = dataTable.Rows[i].Field<decimal>("dmoQuantity");
				eRPDMRClaimComponentInformationDto.dmoQuantityPerParent = dataTable.Rows[i].Field<decimal>("dmoQuantityPerParent");
				eRPDMRClaimComponentInformationDto.dmoQuantityShipped = dataTable.Rows[i].Field<decimal>("dmoQuantityShipped");
				eRPDMRClaimComponentInformationDto.dmoRowVersion = dataTable.Rows[i].Field<byte[]>("dmoRowVersion");
				eRPDMRClaimComponentInformationDto.dmoDmrClaimComponentID = dataTable.Rows[i].Field<int>("dmoDmrClaimComponentID");
				eRPDMRClaimComponentInformationDto.dmoUnitOfMeasure = dataTable.Rows[i].Field<string>("dmoUnitOfMeasure");
				eRPDMRClaimComponentInformationDto.dmoWeight = dataTable.Rows[i].Field<decimal>("dmoWeight");
				eRPDMRClaimComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDMRClaimComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDMRClaimComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDMRClaimComponentInformationDto> GetDMRClaimComponent(Guid dMRClaimComponentId)
	{
		ERPDMRClaimComponentInformationDto eRPDMRClaimComponentInformationDto = new ERPDMRClaimComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[27]
		{
			"dmoAdditionalQuantity", "dmoCreatedBy", "dmoCreatedDate", "dmoDescription", "dmoDmrClaimID", "dmoDmrClaimLineID", "dmoUniqueID", "dmoInspectionComponentID", "dmoInspectionID", "dmoInspectionLineID",
			"dmoShippedComplete", "dmoJobAssemblyID", "dmoJobID", "dmoJobMaterialComponentID", "dmoJobMaterialID", "dmoParentQuantity", "dmoPartBinID", "dmoPartID", "dmoPartRevisionID", "dmoPartWarehouseLocationID",
			"dmoQuantity", "dmoQuantityPerParent", "dmoQuantityShipped", "dmoRowVersion", "dmoDmrClaimComponentID", "dmoUnitOfMeasure", "dmoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("dmoUniqueID|C", dMRClaimComponentId);
		AddCustomFieldsToSelectList("DMRClaimComponents");
		using (DataTable dataTable = GetAsDataTable("DMRClaimComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDMRClaimComponentInformationDto);
			}
			eRPDMRClaimComponentInformationDto.dmoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("dmoAdditionalQuantity");
			eRPDMRClaimComponentInformationDto.dmoCreatedBy = dataTable.Rows[0].Field<string>("dmoCreatedBy");
			eRPDMRClaimComponentInformationDto.dmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("dmoCreatedDate");
			eRPDMRClaimComponentInformationDto.dmoDescription = dataTable.Rows[0].Field<string>("dmoDescription");
			eRPDMRClaimComponentInformationDto.dmoDmrClaimID = dataTable.Rows[0].Field<string>("dmoDmrClaimID");
			eRPDMRClaimComponentInformationDto.dmoDmrClaimLineID = dataTable.Rows[0].Field<short>("dmoDmrClaimLineID");
			eRPDMRClaimComponentInformationDto.dmoUniqueID = dataTable.Rows[0].Field<Guid>("dmoUniqueID");
			eRPDMRClaimComponentInformationDto.dmoInspectionComponentID = dataTable.Rows[0].Field<int>("dmoInspectionComponentID");
			eRPDMRClaimComponentInformationDto.dmoInspectionID = dataTable.Rows[0].Field<string>("dmoInspectionID");
			eRPDMRClaimComponentInformationDto.dmoInspectionLineID = dataTable.Rows[0].Field<short>("dmoInspectionLineID");
			eRPDMRClaimComponentInformationDto.dmoShippedComplete = dataTable.Rows[0].Field<bool>("dmoShippedComplete");
			eRPDMRClaimComponentInformationDto.dmoJobAssemblyID = dataTable.Rows[0].Field<int>("dmoJobAssemblyID");
			eRPDMRClaimComponentInformationDto.dmoJobID = dataTable.Rows[0].Field<string>("dmoJobID");
			eRPDMRClaimComponentInformationDto.dmoJobMaterialComponentID = dataTable.Rows[0].Field<int>("dmoJobMaterialComponentID");
			eRPDMRClaimComponentInformationDto.dmoJobMaterialID = dataTable.Rows[0].Field<int>("dmoJobMaterialID");
			eRPDMRClaimComponentInformationDto.dmoParentQuantity = dataTable.Rows[0].Field<decimal>("dmoParentQuantity");
			eRPDMRClaimComponentInformationDto.dmoPartBinID = dataTable.Rows[0].Field<string>("dmoPartBinID");
			eRPDMRClaimComponentInformationDto.dmoPartID = dataTable.Rows[0].Field<string>("dmoPartID");
			eRPDMRClaimComponentInformationDto.dmoPartRevisionID = dataTable.Rows[0].Field<string>("dmoPartRevisionID");
			eRPDMRClaimComponentInformationDto.dmoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("dmoPartWarehouseLocationID");
			eRPDMRClaimComponentInformationDto.dmoQuantity = dataTable.Rows[0].Field<decimal>("dmoQuantity");
			eRPDMRClaimComponentInformationDto.dmoQuantityPerParent = dataTable.Rows[0].Field<decimal>("dmoQuantityPerParent");
			eRPDMRClaimComponentInformationDto.dmoQuantityShipped = dataTable.Rows[0].Field<decimal>("dmoQuantityShipped");
			eRPDMRClaimComponentInformationDto.dmoRowVersion = dataTable.Rows[0].Field<byte[]>("dmoRowVersion");
			eRPDMRClaimComponentInformationDto.dmoDmrClaimComponentID = dataTable.Rows[0].Field<int>("dmoDmrClaimComponentID");
			eRPDMRClaimComponentInformationDto.dmoUnitOfMeasure = dataTable.Rows[0].Field<string>("dmoUnitOfMeasure");
			eRPDMRClaimComponentInformationDto.dmoWeight = dataTable.Rows[0].Field<decimal>("dmoWeight");
			eRPDMRClaimComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDMRClaimComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDMRClaimComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDMRClaimComponent(ERPDMRClaimComponentDto dMRClaimComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DMRClaimComponents WHERE dmoUniqueID = " + M1Util.ConvertToLinq(dMRClaimComponent.dmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["dmoDmrClaimID"] = dMRClaimComponent.dmoDmrClaimID.ToUpper();
				dataRow["dmoDmrClaimLineID"] = dMRClaimComponent.dmoDmrClaimLineID;
				dataRow["dmoDmrClaimComponentID"] = dMRClaimComponent.dmoDmrClaimComponentID;
				dMRClaimComponent.dmoUniqueID = ((dMRClaimComponent.dmoUniqueID == Guid.Empty) ? Guid.NewGuid() : dMRClaimComponent.dmoUniqueID);
				dataRow["dmoUniqueID"] = dMRClaimComponent.dmoUniqueID;
				dataRow["dmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["dmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DMRClaimComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (dMRClaimComponent.dmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DMRClaimComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["dmoRowVersion"], dMRClaimComponent.dmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DMRClaimComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DMRClaimComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["dmoAdditionalQuantity"] = dMRClaimComponent.dmoAdditionalQuantity;
			dataRow["dmoDescription"] = dMRClaimComponent.dmoDescription;
			dataRow["dmoInspectionComponentID"] = dMRClaimComponent.dmoInspectionComponentID;
			dataRow["dmoInspectionID"] = dMRClaimComponent.dmoInspectionID;
			dataRow["dmoInspectionLineID"] = dMRClaimComponent.dmoInspectionLineID;
			dataRow["dmoShippedComplete"] = dMRClaimComponent.dmoShippedComplete;
			dataRow["dmoJobAssemblyID"] = dMRClaimComponent.dmoJobAssemblyID;
			dataRow["dmoJobID"] = dMRClaimComponent.dmoJobID;
			dataRow["dmoJobMaterialComponentID"] = dMRClaimComponent.dmoJobMaterialComponentID;
			dataRow["dmoJobMaterialID"] = dMRClaimComponent.dmoJobMaterialID;
			dataRow["dmoParentQuantity"] = dMRClaimComponent.dmoParentQuantity;
			dataRow["dmoPartBinID"] = dMRClaimComponent.dmoPartBinID;
			dataRow["dmoPartID"] = dMRClaimComponent.dmoPartID;
			dataRow["dmoPartRevisionID"] = dMRClaimComponent.dmoPartRevisionID;
			dataRow["dmoPartWarehouseLocationID"] = dMRClaimComponent.dmoPartWarehouseLocationID;
			dataRow["dmoQuantity"] = dMRClaimComponent.dmoQuantity;
			dataRow["dmoQuantityPerParent"] = dMRClaimComponent.dmoQuantityPerParent;
			dataRow["dmoQuantityShipped"] = dMRClaimComponent.dmoQuantityShipped;
			dataRow["dmoUnitOfMeasure"] = dMRClaimComponent.dmoUnitOfMeasure;
			dataRow["dmoWeight"] = dMRClaimComponent.dmoWeight;
			if (dMRClaimComponent.CustomFields != null && dMRClaimComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in dMRClaimComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DMRClaimComponent [{dMRClaimComponent.dmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DMRClaimComponent [{dMRClaimComponent.dmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
