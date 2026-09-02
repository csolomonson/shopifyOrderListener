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

public class ERPDMRShipmentComponentRepository : APIBaseRepository, IERPDMRShipmentComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPDMRShipmentComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDMRShipmentComponentExist(Guid dMRShipmentComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("dsoUniqueID|C", dMRShipmentComponentId);
		base.selectList.Add("dsoUniqueID");
		return Task.FromResult(GetAsObject("DMRShipmentComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDMRShipmentComponentInformationDto>> GetAllDMRShipmentComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDMRShipmentComponentInformationDto> collection = new List<ERPDMRShipmentComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[39]
		{
			"dsoAdditionalQuantity", "dsoCreatedBy", "dsoCreatedDate", "dsoDescription", "dsoDmrClaimComponentID", "dsoDmrClaimID", "dsoDmrClaimLineID", "dsoDmrShipmentID", "dsoDmrShipmentLineID", "dsoUniqueID",
			"dsoInspectionComponentID", "dsoInspectionID", "dsoInspectionLineID", "dsoInvParentQuantity", "dsoInvQuantityShipped", "dsoClosed", "dsoPosted", "dsoReversed", "dsoShippedComplete", "dsoJobAssemblyID",
			"dsoJobID", "dsoJobMaterialComponentID", "dsoJobMaterialID", "dsoJobMatParentQuantity", "dsoJobMatQuantityShipped", "dsoPartBinID", "dsoPartID", "dsoPartRevisionID", "dsoPartWarehouseLocationID", "dsoQuantityPerParent",
			"dsoReturnParentQuantity", "dsoReturnQuantityShipped", "dsoReverseDmrShipmentCompID", "dsoReverseDmrShipmentID", "dsoReverseDmrShipmentLineID", "dsoRowVersion", "dsoDmrShipmentComponentID", "dsoUnitOfMeasure", "dsoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DMRShipmentComponents");
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
		using (DataTable dataTable = GetAsDataTable("DMRShipmentComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDMRShipmentComponentInformationDto eRPDMRShipmentComponentInformationDto = new ERPDMRShipmentComponentInformationDto();
				eRPDMRShipmentComponentInformationDto.dsoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("dsoAdditionalQuantity");
				eRPDMRShipmentComponentInformationDto.dsoCreatedBy = dataTable.Rows[i].Field<string>("dsoCreatedBy");
				eRPDMRShipmentComponentInformationDto.dsoCreatedDate = dataTable.Rows[i].Field<DateTime?>("dsoCreatedDate");
				eRPDMRShipmentComponentInformationDto.dsoDescription = dataTable.Rows[i].Field<string>("dsoDescription");
				eRPDMRShipmentComponentInformationDto.dsoDmrClaimComponentID = dataTable.Rows[i].Field<int>("dsoDmrClaimComponentID");
				eRPDMRShipmentComponentInformationDto.dsoDmrClaimID = dataTable.Rows[i].Field<string>("dsoDmrClaimID");
				eRPDMRShipmentComponentInformationDto.dsoDmrClaimLineID = dataTable.Rows[i].Field<short>("dsoDmrClaimLineID");
				eRPDMRShipmentComponentInformationDto.dsoDmrShipmentID = dataTable.Rows[i].Field<string>("dsoDmrShipmentID");
				eRPDMRShipmentComponentInformationDto.dsoDmrShipmentLineID = dataTable.Rows[i].Field<short>("dsoDmrShipmentLineID");
				eRPDMRShipmentComponentInformationDto.dsoUniqueID = dataTable.Rows[i].Field<Guid>("dsoUniqueID");
				eRPDMRShipmentComponentInformationDto.dsoInspectionComponentID = dataTable.Rows[i].Field<int>("dsoInspectionComponentID");
				eRPDMRShipmentComponentInformationDto.dsoInspectionID = dataTable.Rows[i].Field<string>("dsoInspectionID");
				eRPDMRShipmentComponentInformationDto.dsoInspectionLineID = dataTable.Rows[i].Field<short>("dsoInspectionLineID");
				eRPDMRShipmentComponentInformationDto.dsoInvParentQuantity = dataTable.Rows[i].Field<decimal>("dsoInvParentQuantity");
				eRPDMRShipmentComponentInformationDto.dsoInvQuantityShipped = dataTable.Rows[i].Field<decimal>("dsoInvQuantityShipped");
				eRPDMRShipmentComponentInformationDto.dsoClosed = dataTable.Rows[i].Field<bool>("dsoClosed");
				eRPDMRShipmentComponentInformationDto.dsoPosted = dataTable.Rows[i].Field<bool>("dsoPosted");
				eRPDMRShipmentComponentInformationDto.dsoReversed = dataTable.Rows[i].Field<bool>("dsoReversed");
				eRPDMRShipmentComponentInformationDto.dsoShippedComplete = dataTable.Rows[i].Field<bool>("dsoShippedComplete");
				eRPDMRShipmentComponentInformationDto.dsoJobAssemblyID = dataTable.Rows[i].Field<int>("dsoJobAssemblyID");
				eRPDMRShipmentComponentInformationDto.dsoJobID = dataTable.Rows[i].Field<string>("dsoJobID");
				eRPDMRShipmentComponentInformationDto.dsoJobMaterialComponentID = dataTable.Rows[i].Field<int>("dsoJobMaterialComponentID");
				eRPDMRShipmentComponentInformationDto.dsoJobMaterialID = dataTable.Rows[i].Field<int>("dsoJobMaterialID");
				eRPDMRShipmentComponentInformationDto.dsoJobMatParentQuantity = dataTable.Rows[i].Field<decimal>("dsoJobMatParentQuantity");
				eRPDMRShipmentComponentInformationDto.dsoJobMatQuantityShipped = dataTable.Rows[i].Field<decimal>("dsoJobMatQuantityShipped");
				eRPDMRShipmentComponentInformationDto.dsoPartBinID = dataTable.Rows[i].Field<string>("dsoPartBinID");
				eRPDMRShipmentComponentInformationDto.dsoPartID = dataTable.Rows[i].Field<string>("dsoPartID");
				eRPDMRShipmentComponentInformationDto.dsoPartRevisionID = dataTable.Rows[i].Field<string>("dsoPartRevisionID");
				eRPDMRShipmentComponentInformationDto.dsoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("dsoPartWarehouseLocationID");
				eRPDMRShipmentComponentInformationDto.dsoQuantityPerParent = dataTable.Rows[i].Field<decimal>("dsoQuantityPerParent");
				eRPDMRShipmentComponentInformationDto.dsoReturnParentQuantity = dataTable.Rows[i].Field<decimal>("dsoReturnParentQuantity");
				eRPDMRShipmentComponentInformationDto.dsoReturnQuantityShipped = dataTable.Rows[i].Field<decimal>("dsoReturnQuantityShipped");
				eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentCompID = dataTable.Rows[i].Field<int>("dsoReverseDmrShipmentCompID");
				eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentID = dataTable.Rows[i].Field<string>("dsoReverseDmrShipmentID");
				eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentLineID = dataTable.Rows[i].Field<short>("dsoReverseDmrShipmentLineID");
				eRPDMRShipmentComponentInformationDto.dsoRowVersion = dataTable.Rows[i].Field<byte[]>("dsoRowVersion");
				eRPDMRShipmentComponentInformationDto.dsoDmrShipmentComponentID = dataTable.Rows[i].Field<int>("dsoDmrShipmentComponentID");
				eRPDMRShipmentComponentInformationDto.dsoUnitOfMeasure = dataTable.Rows[i].Field<string>("dsoUnitOfMeasure");
				eRPDMRShipmentComponentInformationDto.dsoWeight = dataTable.Rows[i].Field<decimal>("dsoWeight");
				eRPDMRShipmentComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDMRShipmentComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDMRShipmentComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDMRShipmentComponentInformationDto> GetDMRShipmentComponent(Guid dMRShipmentComponentId)
	{
		ERPDMRShipmentComponentInformationDto eRPDMRShipmentComponentInformationDto = new ERPDMRShipmentComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[39]
		{
			"dsoAdditionalQuantity", "dsoCreatedBy", "dsoCreatedDate", "dsoDescription", "dsoDmrClaimComponentID", "dsoDmrClaimID", "dsoDmrClaimLineID", "dsoDmrShipmentID", "dsoDmrShipmentLineID", "dsoUniqueID",
			"dsoInspectionComponentID", "dsoInspectionID", "dsoInspectionLineID", "dsoInvParentQuantity", "dsoInvQuantityShipped", "dsoClosed", "dsoPosted", "dsoReversed", "dsoShippedComplete", "dsoJobAssemblyID",
			"dsoJobID", "dsoJobMaterialComponentID", "dsoJobMaterialID", "dsoJobMatParentQuantity", "dsoJobMatQuantityShipped", "dsoPartBinID", "dsoPartID", "dsoPartRevisionID", "dsoPartWarehouseLocationID", "dsoQuantityPerParent",
			"dsoReturnParentQuantity", "dsoReturnQuantityShipped", "dsoReverseDmrShipmentCompID", "dsoReverseDmrShipmentID", "dsoReverseDmrShipmentLineID", "dsoRowVersion", "dsoDmrShipmentComponentID", "dsoUnitOfMeasure", "dsoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("dsoUniqueID|C", dMRShipmentComponentId);
		AddCustomFieldsToSelectList("DMRShipmentComponents");
		using (DataTable dataTable = GetAsDataTable("DMRShipmentComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDMRShipmentComponentInformationDto);
			}
			eRPDMRShipmentComponentInformationDto.dsoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("dsoAdditionalQuantity");
			eRPDMRShipmentComponentInformationDto.dsoCreatedBy = dataTable.Rows[0].Field<string>("dsoCreatedBy");
			eRPDMRShipmentComponentInformationDto.dsoCreatedDate = dataTable.Rows[0].Field<DateTime?>("dsoCreatedDate");
			eRPDMRShipmentComponentInformationDto.dsoDescription = dataTable.Rows[0].Field<string>("dsoDescription");
			eRPDMRShipmentComponentInformationDto.dsoDmrClaimComponentID = dataTable.Rows[0].Field<int>("dsoDmrClaimComponentID");
			eRPDMRShipmentComponentInformationDto.dsoDmrClaimID = dataTable.Rows[0].Field<string>("dsoDmrClaimID");
			eRPDMRShipmentComponentInformationDto.dsoDmrClaimLineID = dataTable.Rows[0].Field<short>("dsoDmrClaimLineID");
			eRPDMRShipmentComponentInformationDto.dsoDmrShipmentID = dataTable.Rows[0].Field<string>("dsoDmrShipmentID");
			eRPDMRShipmentComponentInformationDto.dsoDmrShipmentLineID = dataTable.Rows[0].Field<short>("dsoDmrShipmentLineID");
			eRPDMRShipmentComponentInformationDto.dsoUniqueID = dataTable.Rows[0].Field<Guid>("dsoUniqueID");
			eRPDMRShipmentComponentInformationDto.dsoInspectionComponentID = dataTable.Rows[0].Field<int>("dsoInspectionComponentID");
			eRPDMRShipmentComponentInformationDto.dsoInspectionID = dataTable.Rows[0].Field<string>("dsoInspectionID");
			eRPDMRShipmentComponentInformationDto.dsoInspectionLineID = dataTable.Rows[0].Field<short>("dsoInspectionLineID");
			eRPDMRShipmentComponentInformationDto.dsoInvParentQuantity = dataTable.Rows[0].Field<decimal>("dsoInvParentQuantity");
			eRPDMRShipmentComponentInformationDto.dsoInvQuantityShipped = dataTable.Rows[0].Field<decimal>("dsoInvQuantityShipped");
			eRPDMRShipmentComponentInformationDto.dsoClosed = dataTable.Rows[0].Field<bool>("dsoClosed");
			eRPDMRShipmentComponentInformationDto.dsoPosted = dataTable.Rows[0].Field<bool>("dsoPosted");
			eRPDMRShipmentComponentInformationDto.dsoReversed = dataTable.Rows[0].Field<bool>("dsoReversed");
			eRPDMRShipmentComponentInformationDto.dsoShippedComplete = dataTable.Rows[0].Field<bool>("dsoShippedComplete");
			eRPDMRShipmentComponentInformationDto.dsoJobAssemblyID = dataTable.Rows[0].Field<int>("dsoJobAssemblyID");
			eRPDMRShipmentComponentInformationDto.dsoJobID = dataTable.Rows[0].Field<string>("dsoJobID");
			eRPDMRShipmentComponentInformationDto.dsoJobMaterialComponentID = dataTable.Rows[0].Field<int>("dsoJobMaterialComponentID");
			eRPDMRShipmentComponentInformationDto.dsoJobMaterialID = dataTable.Rows[0].Field<int>("dsoJobMaterialID");
			eRPDMRShipmentComponentInformationDto.dsoJobMatParentQuantity = dataTable.Rows[0].Field<decimal>("dsoJobMatParentQuantity");
			eRPDMRShipmentComponentInformationDto.dsoJobMatQuantityShipped = dataTable.Rows[0].Field<decimal>("dsoJobMatQuantityShipped");
			eRPDMRShipmentComponentInformationDto.dsoPartBinID = dataTable.Rows[0].Field<string>("dsoPartBinID");
			eRPDMRShipmentComponentInformationDto.dsoPartID = dataTable.Rows[0].Field<string>("dsoPartID");
			eRPDMRShipmentComponentInformationDto.dsoPartRevisionID = dataTable.Rows[0].Field<string>("dsoPartRevisionID");
			eRPDMRShipmentComponentInformationDto.dsoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("dsoPartWarehouseLocationID");
			eRPDMRShipmentComponentInformationDto.dsoQuantityPerParent = dataTable.Rows[0].Field<decimal>("dsoQuantityPerParent");
			eRPDMRShipmentComponentInformationDto.dsoReturnParentQuantity = dataTable.Rows[0].Field<decimal>("dsoReturnParentQuantity");
			eRPDMRShipmentComponentInformationDto.dsoReturnQuantityShipped = dataTable.Rows[0].Field<decimal>("dsoReturnQuantityShipped");
			eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentCompID = dataTable.Rows[0].Field<int>("dsoReverseDmrShipmentCompID");
			eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentID = dataTable.Rows[0].Field<string>("dsoReverseDmrShipmentID");
			eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentLineID = dataTable.Rows[0].Field<short>("dsoReverseDmrShipmentLineID");
			eRPDMRShipmentComponentInformationDto.dsoRowVersion = dataTable.Rows[0].Field<byte[]>("dsoRowVersion");
			eRPDMRShipmentComponentInformationDto.dsoDmrShipmentComponentID = dataTable.Rows[0].Field<int>("dsoDmrShipmentComponentID");
			eRPDMRShipmentComponentInformationDto.dsoUnitOfMeasure = dataTable.Rows[0].Field<string>("dsoUnitOfMeasure");
			eRPDMRShipmentComponentInformationDto.dsoWeight = dataTable.Rows[0].Field<decimal>("dsoWeight");
			eRPDMRShipmentComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDMRShipmentComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDMRShipmentComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDMRShipmentComponent(ERPDMRShipmentComponentDto dMRShipmentComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DMRShipmentComponents WHERE dsoUniqueID = " + M1Util.ConvertToLinq(dMRShipmentComponent.dsoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["dsoDmrShipmentID"] = dMRShipmentComponent.dsoDmrShipmentID.ToUpper();
				dataRow["dsoDmrShipmentLineID"] = dMRShipmentComponent.dsoDmrShipmentLineID;
				dataRow["dsoDmrShipmentComponentID"] = dMRShipmentComponent.dsoDmrShipmentComponentID;
				dMRShipmentComponent.dsoUniqueID = ((dMRShipmentComponent.dsoUniqueID == Guid.Empty) ? Guid.NewGuid() : dMRShipmentComponent.dsoUniqueID);
				dataRow["dsoUniqueID"] = dMRShipmentComponent.dsoUniqueID;
				dataRow["dsoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["dsoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DMRShipmentComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (dMRShipmentComponent.dsoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DMRShipmentComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["dsoRowVersion"], dMRShipmentComponent.dsoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DMRShipmentComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DMRShipmentComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["dsoAdditionalQuantity"] = dMRShipmentComponent.dsoAdditionalQuantity;
			dataRow["dsoDescription"] = dMRShipmentComponent.dsoDescription;
			dataRow["dsoDmrClaimComponentID"] = dMRShipmentComponent.dsoDmrClaimComponentID;
			dataRow["dsoDmrClaimID"] = dMRShipmentComponent.dsoDmrClaimID;
			dataRow["dsoDmrClaimLineID"] = dMRShipmentComponent.dsoDmrClaimLineID;
			dataRow["dsoInspectionComponentID"] = dMRShipmentComponent.dsoInspectionComponentID;
			dataRow["dsoInspectionID"] = dMRShipmentComponent.dsoInspectionID;
			dataRow["dsoInspectionLineID"] = dMRShipmentComponent.dsoInspectionLineID;
			dataRow["dsoInvParentQuantity"] = dMRShipmentComponent.dsoInvParentQuantity;
			dataRow["dsoInvQuantityShipped"] = dMRShipmentComponent.dsoInvQuantityShipped;
			dataRow["dsoClosed"] = dMRShipmentComponent.dsoClosed;
			dataRow["dsoPosted"] = dMRShipmentComponent.dsoPosted;
			dataRow["dsoReversed"] = dMRShipmentComponent.dsoReversed;
			dataRow["dsoShippedComplete"] = dMRShipmentComponent.dsoShippedComplete;
			dataRow["dsoJobAssemblyID"] = dMRShipmentComponent.dsoJobAssemblyID;
			dataRow["dsoJobID"] = dMRShipmentComponent.dsoJobID;
			dataRow["dsoJobMaterialComponentID"] = dMRShipmentComponent.dsoJobMaterialComponentID;
			dataRow["dsoJobMaterialID"] = dMRShipmentComponent.dsoJobMaterialID;
			dataRow["dsoJobMatParentQuantity"] = dMRShipmentComponent.dsoJobMatParentQuantity;
			dataRow["dsoJobMatQuantityShipped"] = dMRShipmentComponent.dsoJobMatQuantityShipped;
			dataRow["dsoPartBinID"] = dMRShipmentComponent.dsoPartBinID;
			dataRow["dsoPartID"] = dMRShipmentComponent.dsoPartID;
			dataRow["dsoPartRevisionID"] = dMRShipmentComponent.dsoPartRevisionID;
			dataRow["dsoPartWarehouseLocationID"] = dMRShipmentComponent.dsoPartWarehouseLocationID;
			dataRow["dsoQuantityPerParent"] = dMRShipmentComponent.dsoQuantityPerParent;
			dataRow["dsoReturnParentQuantity"] = dMRShipmentComponent.dsoReturnParentQuantity;
			dataRow["dsoReturnQuantityShipped"] = dMRShipmentComponent.dsoReturnQuantityShipped;
			dataRow["dsoReverseDmrShipmentCompID"] = dMRShipmentComponent.dsoReverseDmrShipmentCompID;
			dataRow["dsoReverseDmrShipmentID"] = dMRShipmentComponent.dsoReverseDmrShipmentID;
			dataRow["dsoReverseDmrShipmentLineID"] = dMRShipmentComponent.dsoReverseDmrShipmentLineID;
			dataRow["dsoUnitOfMeasure"] = dMRShipmentComponent.dsoUnitOfMeasure;
			dataRow["dsoWeight"] = dMRShipmentComponent.dsoWeight;
			if (dMRShipmentComponent.CustomFields != null && dMRShipmentComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in dMRShipmentComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DMRShipmentComponent [{dMRShipmentComponent.dsoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DMRShipmentComponent [{dMRShipmentComponent.dsoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
