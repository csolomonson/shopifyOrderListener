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

public class ERPPartClassRepository : APIBaseRepository, IERPPartClassRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartClassRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartClassExist(Guid partClassId)
	{
		InitializeParameterLists();
		base.filterList.Add("imcUniqueID|C", partClassId);
		base.selectList.Add("imcUniqueID");
		return Task.FromResult(GetAsObject("PartClasses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartClassInformationDto>> GetAllPartClasses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartClassInformationDto> collection = new List<ERPPartClassInformationDto>();
		InitializeParameterLists();
		string[] array = new string[27]
		{
			"imcPartClassID", "imcCreatedBy", "imcCreatedDate", "imcDescription", "imcUniqueID", "imcFdxHandlingCost", "imcFdxPackageHeight", "imcFdxPackageLength", "imcFdxPackageWidth", "imcFdxPackaging",
			"imcFdxPackagingCost", "imcFdxShipCostMarkupPct", "imcInactiveDate", "imcInventoryGlAccountID", "imcInvInInspectionGlAccountID", "imcInvInTransferGlAccountID", "imcInvToReturnGlAccountID", "imcInactive", "imcFdxNonstandardContainer", "imcFdxOneItemPerShipment",
			"imcRequiresInspection", "imcParentPartClassID", "imcPartImageFileName", "imcPickingMethod", "imcReorderMethod", "imcRowVersion", "imcWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartClasses");
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
		using (DataTable dataTable = GetAsDataTable("PartClasses", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartClassInformationDto eRPPartClassInformationDto = new ERPPartClassInformationDto();
				eRPPartClassInformationDto.imcPartClassID = dataTable.Rows[i].Field<string>("imcPartClassID");
				eRPPartClassInformationDto.imcCreatedBy = dataTable.Rows[i].Field<string>("imcCreatedBy");
				eRPPartClassInformationDto.imcCreatedDate = dataTable.Rows[i].Field<DateTime?>("imcCreatedDate");
				eRPPartClassInformationDto.imcDescription = dataTable.Rows[i].Field<string>("imcDescription");
				eRPPartClassInformationDto.imcUniqueID = dataTable.Rows[i].Field<Guid>("imcUniqueID");
				eRPPartClassInformationDto.imcFdxHandlingCost = dataTable.Rows[i].Field<decimal>("imcFdxHandlingCost");
				eRPPartClassInformationDto.imcFdxPackageHeight = dataTable.Rows[i].Field<int>("imcFdxPackageHeight");
				eRPPartClassInformationDto.imcFdxPackageLength = dataTable.Rows[i].Field<int>("imcFdxPackageLength");
				eRPPartClassInformationDto.imcFdxPackageWidth = dataTable.Rows[i].Field<int>("imcFdxPackageWidth");
				eRPPartClassInformationDto.imcFdxPackaging = dataTable.Rows[i].Field<string>("imcFdxPackaging");
				eRPPartClassInformationDto.imcFdxPackagingCost = dataTable.Rows[i].Field<decimal>("imcFdxPackagingCost");
				eRPPartClassInformationDto.imcFdxShipCostMarkupPct = dataTable.Rows[i].Field<decimal>("imcFdxShipCostMarkupPct");
				eRPPartClassInformationDto.imcInactiveDate = dataTable.Rows[i].Field<DateTime?>("imcInactiveDate");
				eRPPartClassInformationDto.imcInventoryGlAccountID = dataTable.Rows[i].Field<string>("imcInventoryGlAccountID");
				eRPPartClassInformationDto.imcInvInInspectionGlAccountID = dataTable.Rows[i].Field<string>("imcInvInInspectionGlAccountID");
				eRPPartClassInformationDto.imcInvInTransferGlAccountID = dataTable.Rows[i].Field<string>("imcInvInTransferGlAccountID");
				eRPPartClassInformationDto.imcInvToReturnGlAccountID = dataTable.Rows[i].Field<string>("imcInvToReturnGlAccountID");
				eRPPartClassInformationDto.imcInactive = dataTable.Rows[i].Field<bool>("imcInactive");
				eRPPartClassInformationDto.imcFdxNonstandardContainer = dataTable.Rows[i].Field<bool>("imcFdxNonstandardContainer");
				eRPPartClassInformationDto.imcFdxOneItemPerShipment = dataTable.Rows[i].Field<bool>("imcFdxOneItemPerShipment");
				eRPPartClassInformationDto.imcRequiresInspection = dataTable.Rows[i].Field<bool>("imcRequiresInspection");
				eRPPartClassInformationDto.imcParentPartClassID = dataTable.Rows[i].Field<string>("imcParentPartClassID");
				eRPPartClassInformationDto.imcPartImageFileName = dataTable.Rows[i].Field<string>("imcPartImageFileName");
				eRPPartClassInformationDto.imcPickingMethod = dataTable.Rows[i].Field<byte>("imcPickingMethod");
				eRPPartClassInformationDto.imcReorderMethod = dataTable.Rows[i].Field<byte>("imcReorderMethod");
				eRPPartClassInformationDto.imcRowVersion = dataTable.Rows[i].Field<byte[]>("imcRowVersion");
				eRPPartClassInformationDto.imcWeight = dataTable.Rows[i].Field<decimal>("imcWeight");
				eRPPartClassInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartClassInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartClassInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartClassInformationDto> GetPartClass(Guid partClassId)
	{
		ERPPartClassInformationDto eRPPartClassInformationDto = new ERPPartClassInformationDto();
		InitializeParameterLists();
		string[] collection = new string[27]
		{
			"imcPartClassID", "imcCreatedBy", "imcCreatedDate", "imcDescription", "imcUniqueID", "imcFdxHandlingCost", "imcFdxPackageHeight", "imcFdxPackageLength", "imcFdxPackageWidth", "imcFdxPackaging",
			"imcFdxPackagingCost", "imcFdxShipCostMarkupPct", "imcInactiveDate", "imcInventoryGlAccountID", "imcInvInInspectionGlAccountID", "imcInvInTransferGlAccountID", "imcInvToReturnGlAccountID", "imcInactive", "imcFdxNonstandardContainer", "imcFdxOneItemPerShipment",
			"imcRequiresInspection", "imcParentPartClassID", "imcPartImageFileName", "imcPickingMethod", "imcReorderMethod", "imcRowVersion", "imcWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imcUniqueID|C", partClassId);
		AddCustomFieldsToSelectList("PartClasses");
		using (DataTable dataTable = GetAsDataTable("PartClasses", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartClassInformationDto);
			}
			eRPPartClassInformationDto.imcPartClassID = dataTable.Rows[0].Field<string>("imcPartClassID");
			eRPPartClassInformationDto.imcCreatedBy = dataTable.Rows[0].Field<string>("imcCreatedBy");
			eRPPartClassInformationDto.imcCreatedDate = dataTable.Rows[0].Field<DateTime?>("imcCreatedDate");
			eRPPartClassInformationDto.imcDescription = dataTable.Rows[0].Field<string>("imcDescription");
			eRPPartClassInformationDto.imcUniqueID = dataTable.Rows[0].Field<Guid>("imcUniqueID");
			eRPPartClassInformationDto.imcFdxHandlingCost = dataTable.Rows[0].Field<decimal>("imcFdxHandlingCost");
			eRPPartClassInformationDto.imcFdxPackageHeight = dataTable.Rows[0].Field<int>("imcFdxPackageHeight");
			eRPPartClassInformationDto.imcFdxPackageLength = dataTable.Rows[0].Field<int>("imcFdxPackageLength");
			eRPPartClassInformationDto.imcFdxPackageWidth = dataTable.Rows[0].Field<int>("imcFdxPackageWidth");
			eRPPartClassInformationDto.imcFdxPackaging = dataTable.Rows[0].Field<string>("imcFdxPackaging");
			eRPPartClassInformationDto.imcFdxPackagingCost = dataTable.Rows[0].Field<decimal>("imcFdxPackagingCost");
			eRPPartClassInformationDto.imcFdxShipCostMarkupPct = dataTable.Rows[0].Field<decimal>("imcFdxShipCostMarkupPct");
			eRPPartClassInformationDto.imcInactiveDate = dataTable.Rows[0].Field<DateTime?>("imcInactiveDate");
			eRPPartClassInformationDto.imcInventoryGlAccountID = dataTable.Rows[0].Field<string>("imcInventoryGlAccountID");
			eRPPartClassInformationDto.imcInvInInspectionGlAccountID = dataTable.Rows[0].Field<string>("imcInvInInspectionGlAccountID");
			eRPPartClassInformationDto.imcInvInTransferGlAccountID = dataTable.Rows[0].Field<string>("imcInvInTransferGlAccountID");
			eRPPartClassInformationDto.imcInvToReturnGlAccountID = dataTable.Rows[0].Field<string>("imcInvToReturnGlAccountID");
			eRPPartClassInformationDto.imcInactive = dataTable.Rows[0].Field<bool>("imcInactive");
			eRPPartClassInformationDto.imcFdxNonstandardContainer = dataTable.Rows[0].Field<bool>("imcFdxNonstandardContainer");
			eRPPartClassInformationDto.imcFdxOneItemPerShipment = dataTable.Rows[0].Field<bool>("imcFdxOneItemPerShipment");
			eRPPartClassInformationDto.imcRequiresInspection = dataTable.Rows[0].Field<bool>("imcRequiresInspection");
			eRPPartClassInformationDto.imcParentPartClassID = dataTable.Rows[0].Field<string>("imcParentPartClassID");
			eRPPartClassInformationDto.imcPartImageFileName = dataTable.Rows[0].Field<string>("imcPartImageFileName");
			eRPPartClassInformationDto.imcPickingMethod = dataTable.Rows[0].Field<byte>("imcPickingMethod");
			eRPPartClassInformationDto.imcReorderMethod = dataTable.Rows[0].Field<byte>("imcReorderMethod");
			eRPPartClassInformationDto.imcRowVersion = dataTable.Rows[0].Field<byte[]>("imcRowVersion");
			eRPPartClassInformationDto.imcWeight = dataTable.Rows[0].Field<decimal>("imcWeight");
			eRPPartClassInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartClassInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartClassInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartClass(ERPPartClassDto partClass)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartClasses WHERE imcUniqueID = " + M1Util.ConvertToLinq(partClass.imcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imcPartClassID"] = partClass.imcPartClassID.ToUpper();
				partClass.imcUniqueID = ((partClass.imcUniqueID == Guid.Empty) ? Guid.NewGuid() : partClass.imcUniqueID);
				dataRow["imcUniqueID"] = partClass.imcUniqueID;
				dataRow["imcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartClass could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partClass.imcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartClass is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imcRowVersion"], partClass.imcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartClass has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartClass again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imcDescription"] = partClass.imcDescription;
			dataRow["imcFdxHandlingCost"] = partClass.imcFdxHandlingCost;
			dataRow["imcFdxPackageHeight"] = partClass.imcFdxPackageHeight;
			dataRow["imcFdxPackageLength"] = partClass.imcFdxPackageLength;
			dataRow["imcFdxPackageWidth"] = partClass.imcFdxPackageWidth;
			dataRow["imcFdxPackaging"] = partClass.imcFdxPackaging;
			dataRow["imcFdxPackagingCost"] = partClass.imcFdxPackagingCost;
			dataRow["imcFdxShipCostMarkupPct"] = partClass.imcFdxShipCostMarkupPct;
			DataRow dataRow2 = dataRow;
			DateTime? imcInactiveDate = partClass.imcInactiveDate;
			dataRow2["imcInactiveDate"] = (imcInactiveDate.HasValue ? ((object)imcInactiveDate.GetValueOrDefault()) : dataRow["imcInactiveDate"]);
			dataRow["imcInventoryGlAccountID"] = partClass.imcInventoryGlAccountID;
			dataRow["imcInvInInspectionGlAccountID"] = partClass.imcInvInInspectionGlAccountID;
			dataRow["imcInvInTransferGlAccountID"] = partClass.imcInvInTransferGlAccountID;
			dataRow["imcInvToReturnGlAccountID"] = partClass.imcInvToReturnGlAccountID;
			dataRow["imcInactive"] = partClass.imcInactive;
			dataRow["imcFdxNonstandardContainer"] = partClass.imcFdxNonstandardContainer;
			dataRow["imcFdxOneItemPerShipment"] = partClass.imcFdxOneItemPerShipment;
			dataRow["imcRequiresInspection"] = partClass.imcRequiresInspection;
			dataRow["imcParentPartClassID"] = partClass.imcParentPartClassID;
			dataRow["imcPartImageFileName"] = partClass.imcPartImageFileName;
			dataRow["imcPickingMethod"] = partClass.imcPickingMethod;
			dataRow["imcReorderMethod"] = partClass.imcReorderMethod;
			dataRow["imcWeight"] = partClass.imcWeight;
			if (partClass.CustomFields != null && partClass.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partClass.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartClass [{partClass.imcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartClass [{partClass.imcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
