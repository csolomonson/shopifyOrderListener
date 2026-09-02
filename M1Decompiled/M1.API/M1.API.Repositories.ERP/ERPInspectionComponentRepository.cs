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

public class ERPInspectionComponentRepository : APIBaseRepository, IERPInspectionComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPInspectionComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesInspectionComponentExist(Guid inspectionComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("qamUniqueID|C", inspectionComponentId);
		base.selectList.Add("qamUniqueID");
		return Task.FromResult(GetAsObject("InspectionComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPInspectionComponentInformationDto>> GetAllInspectionComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPInspectionComponentInformationDto> collection = new List<ERPInspectionComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[39]
		{
			"qamAdditionalQuantity", "qamComponentQtyToInspect", "qamCreatedBy", "qamCreatedDate", "qamDescription", "qamUniqueID", "qamInspectionID", "qamInspectionLineID", "qamInspectionType", "qamInvParentQtyAccepted",
			"qamInvParentQtyToReturn", "qamInvParentQtyToScrap", "qamInvQuantityAccepted", "qamInvQuantityToReturn", "qamInvQuantityToScrap", "qamInspectionComplete", "qamManualInspectionFinalized", "qamPosted", "qamJobAssemblyID", "qamJobID",
			"qamJobMaterialComponentID", "qamJobMaterialID", "qamJobMatParentQtyAccepted", "qamJobMatParentQtyToReturn", "qamJobMatParentQtyToScrap", "qamJobMatQuantityAccepted", "qamJobMatQuantityToReturn", "qamJobMatQuantityToScrap", "qamParentQtyToInspect", "qamPartBinID",
			"qamPartID", "qamPartRevisionID", "qamPartWarehouseLocationID", "qamQuantityPerParent", "qamInspectionComponentID", "qamSourceTableName", "qamSourceTableUniqueID", "qamUnitOfMeasure", "qamWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("InspectionComponents");
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
		using (DataTable dataTable = GetAsDataTable("InspectionComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPInspectionComponentInformationDto eRPInspectionComponentInformationDto = new ERPInspectionComponentInformationDto();
				eRPInspectionComponentInformationDto.qamAdditionalQuantity = dataTable.Rows[i].Field<decimal>("qamAdditionalQuantity");
				eRPInspectionComponentInformationDto.qamComponentQtyToInspect = dataTable.Rows[i].Field<decimal>("qamComponentQtyToInspect");
				eRPInspectionComponentInformationDto.qamCreatedBy = dataTable.Rows[i].Field<string>("qamCreatedBy");
				eRPInspectionComponentInformationDto.qamCreatedDate = dataTable.Rows[i].Field<DateTime?>("qamCreatedDate");
				eRPInspectionComponentInformationDto.qamDescription = dataTable.Rows[i].Field<string>("qamDescription");
				eRPInspectionComponentInformationDto.qamUniqueID = dataTable.Rows[i].Field<Guid>("qamUniqueID");
				eRPInspectionComponentInformationDto.qamInspectionID = dataTable.Rows[i].Field<string>("qamInspectionID");
				eRPInspectionComponentInformationDto.qamInspectionLineID = dataTable.Rows[i].Field<short>("qamInspectionLineID");
				eRPInspectionComponentInformationDto.qamInspectionType = dataTable.Rows[i].Field<byte>("qamInspectionType");
				eRPInspectionComponentInformationDto.qamInvParentQtyAccepted = dataTable.Rows[i].Field<decimal>("qamInvParentQtyAccepted");
				eRPInspectionComponentInformationDto.qamInvParentQtyToReturn = dataTable.Rows[i].Field<decimal>("qamInvParentQtyToReturn");
				eRPInspectionComponentInformationDto.qamInvParentQtyToScrap = dataTable.Rows[i].Field<decimal>("qamInvParentQtyToScrap");
				eRPInspectionComponentInformationDto.qamInvQuantityAccepted = dataTable.Rows[i].Field<decimal>("qamInvQuantityAccepted");
				eRPInspectionComponentInformationDto.qamInvQuantityToReturn = dataTable.Rows[i].Field<decimal>("qamInvQuantityToReturn");
				eRPInspectionComponentInformationDto.qamInvQuantityToScrap = dataTable.Rows[i].Field<decimal>("qamInvQuantityToScrap");
				eRPInspectionComponentInformationDto.qamInspectionComplete = dataTable.Rows[i].Field<bool>("qamInspectionComplete");
				eRPInspectionComponentInformationDto.qamManualInspectionFinalized = dataTable.Rows[i].Field<bool>("qamManualInspectionFinalized");
				eRPInspectionComponentInformationDto.qamPosted = dataTable.Rows[i].Field<bool>("qamPosted");
				eRPInspectionComponentInformationDto.qamJobAssemblyID = dataTable.Rows[i].Field<int>("qamJobAssemblyID");
				eRPInspectionComponentInformationDto.qamJobID = dataTable.Rows[i].Field<string>("qamJobID");
				eRPInspectionComponentInformationDto.qamJobMaterialComponentID = dataTable.Rows[i].Field<int>("qamJobMaterialComponentID");
				eRPInspectionComponentInformationDto.qamJobMaterialID = dataTable.Rows[i].Field<int>("qamJobMaterialID");
				eRPInspectionComponentInformationDto.qamJobMatParentQtyAccepted = dataTable.Rows[i].Field<decimal>("qamJobMatParentQtyAccepted");
				eRPInspectionComponentInformationDto.qamJobMatParentQtyToReturn = dataTable.Rows[i].Field<decimal>("qamJobMatParentQtyToReturn");
				eRPInspectionComponentInformationDto.qamJobMatParentQtyToScrap = dataTable.Rows[i].Field<decimal>("qamJobMatParentQtyToScrap");
				eRPInspectionComponentInformationDto.qamJobMatQuantityAccepted = dataTable.Rows[i].Field<decimal>("qamJobMatQuantityAccepted");
				eRPInspectionComponentInformationDto.qamJobMatQuantityToReturn = dataTable.Rows[i].Field<decimal>("qamJobMatQuantityToReturn");
				eRPInspectionComponentInformationDto.qamJobMatQuantityToScrap = dataTable.Rows[i].Field<decimal>("qamJobMatQuantityToScrap");
				eRPInspectionComponentInformationDto.qamParentQtyToInspect = dataTable.Rows[i].Field<decimal>("qamParentQtyToInspect");
				eRPInspectionComponentInformationDto.qamPartBinID = dataTable.Rows[i].Field<string>("qamPartBinID");
				eRPInspectionComponentInformationDto.qamPartID = dataTable.Rows[i].Field<string>("qamPartID");
				eRPInspectionComponentInformationDto.qamPartRevisionID = dataTable.Rows[i].Field<string>("qamPartRevisionID");
				eRPInspectionComponentInformationDto.qamPartWarehouseLocationID = dataTable.Rows[i].Field<string>("qamPartWarehouseLocationID");
				eRPInspectionComponentInformationDto.qamQuantityPerParent = dataTable.Rows[i].Field<decimal>("qamQuantityPerParent");
				eRPInspectionComponentInformationDto.qamInspectionComponentID = dataTable.Rows[i].Field<int>("qamInspectionComponentID");
				eRPInspectionComponentInformationDto.qamSourceTableName = dataTable.Rows[i].Field<string>("qamSourceTableName");
				eRPInspectionComponentInformationDto.qamSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("qamSourceTableUniqueID");
				eRPInspectionComponentInformationDto.qamUnitOfMeasure = dataTable.Rows[i].Field<string>("qamUnitOfMeasure");
				eRPInspectionComponentInformationDto.qamWeight = dataTable.Rows[i].Field<decimal>("qamWeight");
				eRPInspectionComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPInspectionComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPInspectionComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPInspectionComponentInformationDto> GetInspectionComponent(Guid inspectionComponentId)
	{
		ERPInspectionComponentInformationDto eRPInspectionComponentInformationDto = new ERPInspectionComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[39]
		{
			"qamAdditionalQuantity", "qamComponentQtyToInspect", "qamCreatedBy", "qamCreatedDate", "qamDescription", "qamUniqueID", "qamInspectionID", "qamInspectionLineID", "qamInspectionType", "qamInvParentQtyAccepted",
			"qamInvParentQtyToReturn", "qamInvParentQtyToScrap", "qamInvQuantityAccepted", "qamInvQuantityToReturn", "qamInvQuantityToScrap", "qamInspectionComplete", "qamManualInspectionFinalized", "qamPosted", "qamJobAssemblyID", "qamJobID",
			"qamJobMaterialComponentID", "qamJobMaterialID", "qamJobMatParentQtyAccepted", "qamJobMatParentQtyToReturn", "qamJobMatParentQtyToScrap", "qamJobMatQuantityAccepted", "qamJobMatQuantityToReturn", "qamJobMatQuantityToScrap", "qamParentQtyToInspect", "qamPartBinID",
			"qamPartID", "qamPartRevisionID", "qamPartWarehouseLocationID", "qamQuantityPerParent", "qamInspectionComponentID", "qamSourceTableName", "qamSourceTableUniqueID", "qamUnitOfMeasure", "qamWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qamUniqueID|C", inspectionComponentId);
		AddCustomFieldsToSelectList("InspectionComponents");
		using (DataTable dataTable = GetAsDataTable("InspectionComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPInspectionComponentInformationDto);
			}
			eRPInspectionComponentInformationDto.qamAdditionalQuantity = dataTable.Rows[0].Field<decimal>("qamAdditionalQuantity");
			eRPInspectionComponentInformationDto.qamComponentQtyToInspect = dataTable.Rows[0].Field<decimal>("qamComponentQtyToInspect");
			eRPInspectionComponentInformationDto.qamCreatedBy = dataTable.Rows[0].Field<string>("qamCreatedBy");
			eRPInspectionComponentInformationDto.qamCreatedDate = dataTable.Rows[0].Field<DateTime?>("qamCreatedDate");
			eRPInspectionComponentInformationDto.qamDescription = dataTable.Rows[0].Field<string>("qamDescription");
			eRPInspectionComponentInformationDto.qamUniqueID = dataTable.Rows[0].Field<Guid>("qamUniqueID");
			eRPInspectionComponentInformationDto.qamInspectionID = dataTable.Rows[0].Field<string>("qamInspectionID");
			eRPInspectionComponentInformationDto.qamInspectionLineID = dataTable.Rows[0].Field<short>("qamInspectionLineID");
			eRPInspectionComponentInformationDto.qamInspectionType = dataTable.Rows[0].Field<byte>("qamInspectionType");
			eRPInspectionComponentInformationDto.qamInvParentQtyAccepted = dataTable.Rows[0].Field<decimal>("qamInvParentQtyAccepted");
			eRPInspectionComponentInformationDto.qamInvParentQtyToReturn = dataTable.Rows[0].Field<decimal>("qamInvParentQtyToReturn");
			eRPInspectionComponentInformationDto.qamInvParentQtyToScrap = dataTable.Rows[0].Field<decimal>("qamInvParentQtyToScrap");
			eRPInspectionComponentInformationDto.qamInvQuantityAccepted = dataTable.Rows[0].Field<decimal>("qamInvQuantityAccepted");
			eRPInspectionComponentInformationDto.qamInvQuantityToReturn = dataTable.Rows[0].Field<decimal>("qamInvQuantityToReturn");
			eRPInspectionComponentInformationDto.qamInvQuantityToScrap = dataTable.Rows[0].Field<decimal>("qamInvQuantityToScrap");
			eRPInspectionComponentInformationDto.qamInspectionComplete = dataTable.Rows[0].Field<bool>("qamInspectionComplete");
			eRPInspectionComponentInformationDto.qamManualInspectionFinalized = dataTable.Rows[0].Field<bool>("qamManualInspectionFinalized");
			eRPInspectionComponentInformationDto.qamPosted = dataTable.Rows[0].Field<bool>("qamPosted");
			eRPInspectionComponentInformationDto.qamJobAssemblyID = dataTable.Rows[0].Field<int>("qamJobAssemblyID");
			eRPInspectionComponentInformationDto.qamJobID = dataTable.Rows[0].Field<string>("qamJobID");
			eRPInspectionComponentInformationDto.qamJobMaterialComponentID = dataTable.Rows[0].Field<int>("qamJobMaterialComponentID");
			eRPInspectionComponentInformationDto.qamJobMaterialID = dataTable.Rows[0].Field<int>("qamJobMaterialID");
			eRPInspectionComponentInformationDto.qamJobMatParentQtyAccepted = dataTable.Rows[0].Field<decimal>("qamJobMatParentQtyAccepted");
			eRPInspectionComponentInformationDto.qamJobMatParentQtyToReturn = dataTable.Rows[0].Field<decimal>("qamJobMatParentQtyToReturn");
			eRPInspectionComponentInformationDto.qamJobMatParentQtyToScrap = dataTable.Rows[0].Field<decimal>("qamJobMatParentQtyToScrap");
			eRPInspectionComponentInformationDto.qamJobMatQuantityAccepted = dataTable.Rows[0].Field<decimal>("qamJobMatQuantityAccepted");
			eRPInspectionComponentInformationDto.qamJobMatQuantityToReturn = dataTable.Rows[0].Field<decimal>("qamJobMatQuantityToReturn");
			eRPInspectionComponentInformationDto.qamJobMatQuantityToScrap = dataTable.Rows[0].Field<decimal>("qamJobMatQuantityToScrap");
			eRPInspectionComponentInformationDto.qamParentQtyToInspect = dataTable.Rows[0].Field<decimal>("qamParentQtyToInspect");
			eRPInspectionComponentInformationDto.qamPartBinID = dataTable.Rows[0].Field<string>("qamPartBinID");
			eRPInspectionComponentInformationDto.qamPartID = dataTable.Rows[0].Field<string>("qamPartID");
			eRPInspectionComponentInformationDto.qamPartRevisionID = dataTable.Rows[0].Field<string>("qamPartRevisionID");
			eRPInspectionComponentInformationDto.qamPartWarehouseLocationID = dataTable.Rows[0].Field<string>("qamPartWarehouseLocationID");
			eRPInspectionComponentInformationDto.qamQuantityPerParent = dataTable.Rows[0].Field<decimal>("qamQuantityPerParent");
			eRPInspectionComponentInformationDto.qamInspectionComponentID = dataTable.Rows[0].Field<int>("qamInspectionComponentID");
			eRPInspectionComponentInformationDto.qamSourceTableName = dataTable.Rows[0].Field<string>("qamSourceTableName");
			eRPInspectionComponentInformationDto.qamSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("qamSourceTableUniqueID");
			eRPInspectionComponentInformationDto.qamUnitOfMeasure = dataTable.Rows[0].Field<string>("qamUnitOfMeasure");
			eRPInspectionComponentInformationDto.qamWeight = dataTable.Rows[0].Field<decimal>("qamWeight");
			eRPInspectionComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPInspectionComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPInspectionComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveInspectionComponent(ERPInspectionComponentDto inspectionComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM InspectionComponents WHERE qamUniqueID = " + M1Util.ConvertToLinq(inspectionComponent.qamUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qamInspectionID"] = inspectionComponent.qamInspectionID.ToUpper();
				dataRow["qamInspectionLineID"] = inspectionComponent.qamInspectionLineID;
				dataRow["qamInspectionComponentID"] = inspectionComponent.qamInspectionComponentID;
				inspectionComponent.qamUniqueID = ((inspectionComponent.qamUniqueID == Guid.Empty) ? Guid.NewGuid() : inspectionComponent.qamUniqueID);
				dataRow["qamUniqueID"] = inspectionComponent.qamUniqueID;
				dataRow["qamCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qamCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The InspectionComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qamAdditionalQuantity"] = inspectionComponent.qamAdditionalQuantity;
			dataRow["qamComponentQtyToInspect"] = inspectionComponent.qamComponentQtyToInspect;
			dataRow["qamDescription"] = inspectionComponent.qamDescription;
			dataRow["qamInspectionType"] = inspectionComponent.qamInspectionType;
			dataRow["qamInvParentQtyAccepted"] = inspectionComponent.qamInvParentQtyAccepted;
			dataRow["qamInvParentQtyToReturn"] = inspectionComponent.qamInvParentQtyToReturn;
			dataRow["qamInvParentQtyToScrap"] = inspectionComponent.qamInvParentQtyToScrap;
			dataRow["qamInvQuantityAccepted"] = inspectionComponent.qamInvQuantityAccepted;
			dataRow["qamInvQuantityToReturn"] = inspectionComponent.qamInvQuantityToReturn;
			dataRow["qamInvQuantityToScrap"] = inspectionComponent.qamInvQuantityToScrap;
			dataRow["qamInspectionComplete"] = inspectionComponent.qamInspectionComplete;
			dataRow["qamManualInspectionFinalized"] = inspectionComponent.qamManualInspectionFinalized;
			dataRow["qamPosted"] = inspectionComponent.qamPosted;
			dataRow["qamJobAssemblyID"] = inspectionComponent.qamJobAssemblyID;
			dataRow["qamJobID"] = inspectionComponent.qamJobID;
			dataRow["qamJobMaterialComponentID"] = inspectionComponent.qamJobMaterialComponentID;
			dataRow["qamJobMaterialID"] = inspectionComponent.qamJobMaterialID;
			dataRow["qamJobMatParentQtyAccepted"] = inspectionComponent.qamJobMatParentQtyAccepted;
			dataRow["qamJobMatParentQtyToReturn"] = inspectionComponent.qamJobMatParentQtyToReturn;
			dataRow["qamJobMatParentQtyToScrap"] = inspectionComponent.qamJobMatParentQtyToScrap;
			dataRow["qamJobMatQuantityAccepted"] = inspectionComponent.qamJobMatQuantityAccepted;
			dataRow["qamJobMatQuantityToReturn"] = inspectionComponent.qamJobMatQuantityToReturn;
			dataRow["qamJobMatQuantityToScrap"] = inspectionComponent.qamJobMatQuantityToScrap;
			dataRow["qamParentQtyToInspect"] = inspectionComponent.qamParentQtyToInspect;
			dataRow["qamPartBinID"] = inspectionComponent.qamPartBinID;
			dataRow["qamPartID"] = inspectionComponent.qamPartID;
			dataRow["qamPartRevisionID"] = inspectionComponent.qamPartRevisionID;
			dataRow["qamPartWarehouseLocationID"] = inspectionComponent.qamPartWarehouseLocationID;
			dataRow["qamQuantityPerParent"] = inspectionComponent.qamQuantityPerParent;
			dataRow["qamSourceTableName"] = inspectionComponent.qamSourceTableName;
			dataRow["qamSourceTableUniqueID"] = inspectionComponent.qamSourceTableUniqueID;
			dataRow["qamUnitOfMeasure"] = inspectionComponent.qamUnitOfMeasure;
			dataRow["qamWeight"] = inspectionComponent.qamWeight;
			if (inspectionComponent.CustomFields != null && inspectionComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in inspectionComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the InspectionComponent [{inspectionComponent.qamUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the InspectionComponent [{inspectionComponent.qamUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
