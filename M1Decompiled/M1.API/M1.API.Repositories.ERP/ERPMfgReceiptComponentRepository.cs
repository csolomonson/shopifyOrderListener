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

public class ERPMfgReceiptComponentRepository : APIBaseRepository, IERPMfgReceiptComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPMfgReceiptComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMfgReceiptComponentExist(Guid mfgReceiptComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmnUniqueID|C", mfgReceiptComponentId);
		base.selectList.Add("rmnUniqueID");
		return Task.FromResult(GetAsObject("MfgReceiptComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMfgReceiptComponentInformationDto>> GetAllMfgReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMfgReceiptComponentInformationDto> collection = new List<ERPMfgReceiptComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"rmnAdditionalQuantity", "rmnCreatedBy", "rmnCreatedDate", "rmnDescription", "rmnUniqueID", "rmnExtendedCost", "rmnInvParentQuantity", "rmnInvReceiptQuantity", "rmnPosted", "rmnReceivedComplete",
			"rmnReversed", "rmnJobAssemblyID", "rmnJobID", "rmnJobMaterialComponentID", "rmnJobMaterialID", "rmnJobMatParentQuantity", "rmnJobMatReceiptQuantity", "rmnMfgReceiptID", "rmnPartBinID", "rmnPartID",
			"rmnPartRevisionID", "rmnPartWarehouseLocationID", "rmnQuantityPerParent", "rmnReverseMfgReceiptCompID", "rmnReverseMfgReceiptID", "rmnRowVersion", "rmnMfgReceiptComponentID", "rmnUnitCost", "rmnUnitOfMeasure", "rmnWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MfgReceiptComponents");
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
		using (DataTable dataTable = GetAsDataTable("MfgReceiptComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMfgReceiptComponentInformationDto eRPMfgReceiptComponentInformationDto = new ERPMfgReceiptComponentInformationDto();
				eRPMfgReceiptComponentInformationDto.rmnAdditionalQuantity = dataTable.Rows[i].Field<decimal>("rmnAdditionalQuantity");
				eRPMfgReceiptComponentInformationDto.rmnCreatedBy = dataTable.Rows[i].Field<string>("rmnCreatedBy");
				eRPMfgReceiptComponentInformationDto.rmnCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmnCreatedDate");
				eRPMfgReceiptComponentInformationDto.rmnDescription = dataTable.Rows[i].Field<string>("rmnDescription");
				eRPMfgReceiptComponentInformationDto.rmnUniqueID = dataTable.Rows[i].Field<Guid>("rmnUniqueID");
				eRPMfgReceiptComponentInformationDto.rmnExtendedCost = dataTable.Rows[i].Field<decimal>("rmnExtendedCost");
				eRPMfgReceiptComponentInformationDto.rmnInvParentQuantity = dataTable.Rows[i].Field<decimal>("rmnInvParentQuantity");
				eRPMfgReceiptComponentInformationDto.rmnInvReceiptQuantity = dataTable.Rows[i].Field<decimal>("rmnInvReceiptQuantity");
				eRPMfgReceiptComponentInformationDto.rmnPosted = dataTable.Rows[i].Field<bool>("rmnPosted");
				eRPMfgReceiptComponentInformationDto.rmnReceivedComplete = dataTable.Rows[i].Field<bool>("rmnReceivedComplete");
				eRPMfgReceiptComponentInformationDto.rmnReversed = dataTable.Rows[i].Field<bool>("rmnReversed");
				eRPMfgReceiptComponentInformationDto.rmnJobAssemblyID = dataTable.Rows[i].Field<int>("rmnJobAssemblyID");
				eRPMfgReceiptComponentInformationDto.rmnJobID = dataTable.Rows[i].Field<string>("rmnJobID");
				eRPMfgReceiptComponentInformationDto.rmnJobMaterialComponentID = dataTable.Rows[i].Field<int>("rmnJobMaterialComponentID");
				eRPMfgReceiptComponentInformationDto.rmnJobMaterialID = dataTable.Rows[i].Field<int>("rmnJobMaterialID");
				eRPMfgReceiptComponentInformationDto.rmnJobMatParentQuantity = dataTable.Rows[i].Field<decimal>("rmnJobMatParentQuantity");
				eRPMfgReceiptComponentInformationDto.rmnJobMatReceiptQuantity = dataTable.Rows[i].Field<decimal>("rmnJobMatReceiptQuantity");
				eRPMfgReceiptComponentInformationDto.rmnMfgReceiptID = dataTable.Rows[i].Field<string>("rmnMfgReceiptID");
				eRPMfgReceiptComponentInformationDto.rmnPartBinID = dataTable.Rows[i].Field<string>("rmnPartBinID");
				eRPMfgReceiptComponentInformationDto.rmnPartID = dataTable.Rows[i].Field<string>("rmnPartID");
				eRPMfgReceiptComponentInformationDto.rmnPartRevisionID = dataTable.Rows[i].Field<string>("rmnPartRevisionID");
				eRPMfgReceiptComponentInformationDto.rmnPartWarehouseLocationID = dataTable.Rows[i].Field<string>("rmnPartWarehouseLocationID");
				eRPMfgReceiptComponentInformationDto.rmnQuantityPerParent = dataTable.Rows[i].Field<decimal>("rmnQuantityPerParent");
				eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptCompID = dataTable.Rows[i].Field<int>("rmnReverseMfgReceiptCompID");
				eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptID = dataTable.Rows[i].Field<string>("rmnReverseMfgReceiptID");
				eRPMfgReceiptComponentInformationDto.rmnRowVersion = dataTable.Rows[i].Field<byte[]>("rmnRowVersion");
				eRPMfgReceiptComponentInformationDto.rmnMfgReceiptComponentID = dataTable.Rows[i].Field<int>("rmnMfgReceiptComponentID");
				eRPMfgReceiptComponentInformationDto.rmnUnitCost = dataTable.Rows[i].Field<decimal>("rmnUnitCost");
				eRPMfgReceiptComponentInformationDto.rmnUnitOfMeasure = dataTable.Rows[i].Field<string>("rmnUnitOfMeasure");
				eRPMfgReceiptComponentInformationDto.rmnWeight = dataTable.Rows[i].Field<decimal>("rmnWeight");
				eRPMfgReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMfgReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMfgReceiptComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMfgReceiptComponentInformationDto> GetMfgReceiptComponent(Guid mfgReceiptComponentId)
	{
		ERPMfgReceiptComponentInformationDto eRPMfgReceiptComponentInformationDto = new ERPMfgReceiptComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"rmnAdditionalQuantity", "rmnCreatedBy", "rmnCreatedDate", "rmnDescription", "rmnUniqueID", "rmnExtendedCost", "rmnInvParentQuantity", "rmnInvReceiptQuantity", "rmnPosted", "rmnReceivedComplete",
			"rmnReversed", "rmnJobAssemblyID", "rmnJobID", "rmnJobMaterialComponentID", "rmnJobMaterialID", "rmnJobMatParentQuantity", "rmnJobMatReceiptQuantity", "rmnMfgReceiptID", "rmnPartBinID", "rmnPartID",
			"rmnPartRevisionID", "rmnPartWarehouseLocationID", "rmnQuantityPerParent", "rmnReverseMfgReceiptCompID", "rmnReverseMfgReceiptID", "rmnRowVersion", "rmnMfgReceiptComponentID", "rmnUnitCost", "rmnUnitOfMeasure", "rmnWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmnUniqueID|C", mfgReceiptComponentId);
		AddCustomFieldsToSelectList("MfgReceiptComponents");
		using (DataTable dataTable = GetAsDataTable("MfgReceiptComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMfgReceiptComponentInformationDto);
			}
			eRPMfgReceiptComponentInformationDto.rmnAdditionalQuantity = dataTable.Rows[0].Field<decimal>("rmnAdditionalQuantity");
			eRPMfgReceiptComponentInformationDto.rmnCreatedBy = dataTable.Rows[0].Field<string>("rmnCreatedBy");
			eRPMfgReceiptComponentInformationDto.rmnCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmnCreatedDate");
			eRPMfgReceiptComponentInformationDto.rmnDescription = dataTable.Rows[0].Field<string>("rmnDescription");
			eRPMfgReceiptComponentInformationDto.rmnUniqueID = dataTable.Rows[0].Field<Guid>("rmnUniqueID");
			eRPMfgReceiptComponentInformationDto.rmnExtendedCost = dataTable.Rows[0].Field<decimal>("rmnExtendedCost");
			eRPMfgReceiptComponentInformationDto.rmnInvParentQuantity = dataTable.Rows[0].Field<decimal>("rmnInvParentQuantity");
			eRPMfgReceiptComponentInformationDto.rmnInvReceiptQuantity = dataTable.Rows[0].Field<decimal>("rmnInvReceiptQuantity");
			eRPMfgReceiptComponentInformationDto.rmnPosted = dataTable.Rows[0].Field<bool>("rmnPosted");
			eRPMfgReceiptComponentInformationDto.rmnReceivedComplete = dataTable.Rows[0].Field<bool>("rmnReceivedComplete");
			eRPMfgReceiptComponentInformationDto.rmnReversed = dataTable.Rows[0].Field<bool>("rmnReversed");
			eRPMfgReceiptComponentInformationDto.rmnJobAssemblyID = dataTable.Rows[0].Field<int>("rmnJobAssemblyID");
			eRPMfgReceiptComponentInformationDto.rmnJobID = dataTable.Rows[0].Field<string>("rmnJobID");
			eRPMfgReceiptComponentInformationDto.rmnJobMaterialComponentID = dataTable.Rows[0].Field<int>("rmnJobMaterialComponentID");
			eRPMfgReceiptComponentInformationDto.rmnJobMaterialID = dataTable.Rows[0].Field<int>("rmnJobMaterialID");
			eRPMfgReceiptComponentInformationDto.rmnJobMatParentQuantity = dataTable.Rows[0].Field<decimal>("rmnJobMatParentQuantity");
			eRPMfgReceiptComponentInformationDto.rmnJobMatReceiptQuantity = dataTable.Rows[0].Field<decimal>("rmnJobMatReceiptQuantity");
			eRPMfgReceiptComponentInformationDto.rmnMfgReceiptID = dataTable.Rows[0].Field<string>("rmnMfgReceiptID");
			eRPMfgReceiptComponentInformationDto.rmnPartBinID = dataTable.Rows[0].Field<string>("rmnPartBinID");
			eRPMfgReceiptComponentInformationDto.rmnPartID = dataTable.Rows[0].Field<string>("rmnPartID");
			eRPMfgReceiptComponentInformationDto.rmnPartRevisionID = dataTable.Rows[0].Field<string>("rmnPartRevisionID");
			eRPMfgReceiptComponentInformationDto.rmnPartWarehouseLocationID = dataTable.Rows[0].Field<string>("rmnPartWarehouseLocationID");
			eRPMfgReceiptComponentInformationDto.rmnQuantityPerParent = dataTable.Rows[0].Field<decimal>("rmnQuantityPerParent");
			eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptCompID = dataTable.Rows[0].Field<int>("rmnReverseMfgReceiptCompID");
			eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptID = dataTable.Rows[0].Field<string>("rmnReverseMfgReceiptID");
			eRPMfgReceiptComponentInformationDto.rmnRowVersion = dataTable.Rows[0].Field<byte[]>("rmnRowVersion");
			eRPMfgReceiptComponentInformationDto.rmnMfgReceiptComponentID = dataTable.Rows[0].Field<int>("rmnMfgReceiptComponentID");
			eRPMfgReceiptComponentInformationDto.rmnUnitCost = dataTable.Rows[0].Field<decimal>("rmnUnitCost");
			eRPMfgReceiptComponentInformationDto.rmnUnitOfMeasure = dataTable.Rows[0].Field<string>("rmnUnitOfMeasure");
			eRPMfgReceiptComponentInformationDto.rmnWeight = dataTable.Rows[0].Field<decimal>("rmnWeight");
			eRPMfgReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMfgReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMfgReceiptComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMfgReceiptComponent(ERPMfgReceiptComponentDto mfgReceiptComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MfgReceiptComponents WHERE rmnUniqueID = " + M1Util.ConvertToLinq(mfgReceiptComponent.rmnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmnMfgReceiptID"] = mfgReceiptComponent.rmnMfgReceiptID.ToUpper();
				dataRow["rmnMfgReceiptComponentID"] = mfgReceiptComponent.rmnMfgReceiptComponentID;
				mfgReceiptComponent.rmnUniqueID = ((mfgReceiptComponent.rmnUniqueID == Guid.Empty) ? Guid.NewGuid() : mfgReceiptComponent.rmnUniqueID);
				dataRow["rmnUniqueID"] = mfgReceiptComponent.rmnUniqueID;
				dataRow["rmnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MfgReceiptComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mfgReceiptComponent.rmnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MfgReceiptComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmnRowVersion"], mfgReceiptComponent.rmnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MfgReceiptComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MfgReceiptComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmnAdditionalQuantity"] = mfgReceiptComponent.rmnAdditionalQuantity;
			dataRow["rmnDescription"] = mfgReceiptComponent.rmnDescription;
			dataRow["rmnExtendedCost"] = mfgReceiptComponent.rmnExtendedCost;
			dataRow["rmnInvParentQuantity"] = mfgReceiptComponent.rmnInvParentQuantity;
			dataRow["rmnInvReceiptQuantity"] = mfgReceiptComponent.rmnInvReceiptQuantity;
			dataRow["rmnPosted"] = mfgReceiptComponent.rmnPosted;
			dataRow["rmnReceivedComplete"] = mfgReceiptComponent.rmnReceivedComplete;
			dataRow["rmnReversed"] = mfgReceiptComponent.rmnReversed;
			dataRow["rmnJobAssemblyID"] = mfgReceiptComponent.rmnJobAssemblyID;
			dataRow["rmnJobID"] = mfgReceiptComponent.rmnJobID;
			dataRow["rmnJobMaterialComponentID"] = mfgReceiptComponent.rmnJobMaterialComponentID;
			dataRow["rmnJobMaterialID"] = mfgReceiptComponent.rmnJobMaterialID;
			dataRow["rmnJobMatParentQuantity"] = mfgReceiptComponent.rmnJobMatParentQuantity;
			dataRow["rmnJobMatReceiptQuantity"] = mfgReceiptComponent.rmnJobMatReceiptQuantity;
			dataRow["rmnPartBinID"] = mfgReceiptComponent.rmnPartBinID;
			dataRow["rmnPartID"] = mfgReceiptComponent.rmnPartID;
			dataRow["rmnPartRevisionID"] = mfgReceiptComponent.rmnPartRevisionID;
			dataRow["rmnPartWarehouseLocationID"] = mfgReceiptComponent.rmnPartWarehouseLocationID;
			dataRow["rmnQuantityPerParent"] = mfgReceiptComponent.rmnQuantityPerParent;
			dataRow["rmnReverseMfgReceiptCompID"] = mfgReceiptComponent.rmnReverseMfgReceiptCompID;
			dataRow["rmnReverseMfgReceiptID"] = mfgReceiptComponent.rmnReverseMfgReceiptID;
			dataRow["rmnUnitCost"] = mfgReceiptComponent.rmnUnitCost;
			dataRow["rmnUnitOfMeasure"] = mfgReceiptComponent.rmnUnitOfMeasure;
			dataRow["rmnWeight"] = mfgReceiptComponent.rmnWeight;
			if (mfgReceiptComponent.CustomFields != null && mfgReceiptComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mfgReceiptComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MfgReceiptComponent [{mfgReceiptComponent.rmnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MfgReceiptComponent [{mfgReceiptComponent.rmnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
