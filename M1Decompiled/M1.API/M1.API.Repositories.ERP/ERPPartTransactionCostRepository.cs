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

public class ERPPartTransactionCostRepository : APIBaseRepository, IERPPartTransactionCostRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartTransactionCostRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartTransactionCostExist(Guid partTransactionCostId)
	{
		InitializeParameterLists();
		base.filterList.Add("intUniqueID|C", partTransactionCostId);
		base.selectList.Add("intUniqueID");
		return Task.FromResult(GetAsObject("PartTransactionCosts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartTransactionCostInformationDto>> GetAllPartTransactionCosts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartTransactionCostInformationDto> collection = new List<ERPPartTransactionCostInformationDto>();
		InitializeParameterLists();
		string[] array = new string[31]
		{
			"intActualUnitDutyCost", "intActualUnitFreightCost", "intActualUnitLaborCost", "intActualUnitMaterialCost", "intActualUnitMiscCost", "intActualUnitOverheadCost", "intActualUnitSubcontractCost", "intCostType", "intCreatedBy", "intCreatedDate",
			"intUniqueID", "intPartTransactionID", "intPrevUnitDutyCost", "intPrevUnitFreightCost", "intPrevUnitLaborCost", "intPrevUnitMaterialCost", "intPrevUnitMiscCost", "intPrevUnitOverheadCost", "intPrevUnitSubcontractCost", "intQuantity",
			"intRowVersion", "intPartTransactionCostID", "intSourceTableName", "intSourceTableUniqueID", "intUnitDutyCost", "intUnitFreightCost", "intUnitLaborCost", "intUnitMaterialCost", "intUnitMiscCost", "intUnitOverheadCost",
			"intUnitSubcontractCost"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartTransactionCosts");
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
		using (DataTable dataTable = GetAsDataTable("PartTransactionCosts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartTransactionCostInformationDto eRPPartTransactionCostInformationDto = new ERPPartTransactionCostInformationDto();
				eRPPartTransactionCostInformationDto.intActualUnitDutyCost = dataTable.Rows[i].Field<decimal>("intActualUnitDutyCost");
				eRPPartTransactionCostInformationDto.intActualUnitFreightCost = dataTable.Rows[i].Field<decimal>("intActualUnitFreightCost");
				eRPPartTransactionCostInformationDto.intActualUnitLaborCost = dataTable.Rows[i].Field<decimal>("intActualUnitLaborCost");
				eRPPartTransactionCostInformationDto.intActualUnitMaterialCost = dataTable.Rows[i].Field<decimal>("intActualUnitMaterialCost");
				eRPPartTransactionCostInformationDto.intActualUnitMiscCost = dataTable.Rows[i].Field<decimal>("intActualUnitMiscCost");
				eRPPartTransactionCostInformationDto.intActualUnitOverheadCost = dataTable.Rows[i].Field<decimal>("intActualUnitOverheadCost");
				eRPPartTransactionCostInformationDto.intActualUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("intActualUnitSubcontractCost");
				eRPPartTransactionCostInformationDto.intCostType = dataTable.Rows[i].Field<byte>("intCostType");
				eRPPartTransactionCostInformationDto.intCreatedBy = dataTable.Rows[i].Field<string>("intCreatedBy");
				eRPPartTransactionCostInformationDto.intCreatedDate = dataTable.Rows[i].Field<DateTime?>("intCreatedDate");
				eRPPartTransactionCostInformationDto.intUniqueID = dataTable.Rows[i].Field<Guid>("intUniqueID");
				eRPPartTransactionCostInformationDto.intPartTransactionID = dataTable.Rows[i].Field<int>("intPartTransactionID");
				eRPPartTransactionCostInformationDto.intPrevUnitDutyCost = dataTable.Rows[i].Field<decimal>("intPrevUnitDutyCost");
				eRPPartTransactionCostInformationDto.intPrevUnitFreightCost = dataTable.Rows[i].Field<decimal>("intPrevUnitFreightCost");
				eRPPartTransactionCostInformationDto.intPrevUnitLaborCost = dataTable.Rows[i].Field<decimal>("intPrevUnitLaborCost");
				eRPPartTransactionCostInformationDto.intPrevUnitMaterialCost = dataTable.Rows[i].Field<decimal>("intPrevUnitMaterialCost");
				eRPPartTransactionCostInformationDto.intPrevUnitMiscCost = dataTable.Rows[i].Field<decimal>("intPrevUnitMiscCost");
				eRPPartTransactionCostInformationDto.intPrevUnitOverheadCost = dataTable.Rows[i].Field<decimal>("intPrevUnitOverheadCost");
				eRPPartTransactionCostInformationDto.intPrevUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("intPrevUnitSubcontractCost");
				eRPPartTransactionCostInformationDto.intQuantity = dataTable.Rows[i].Field<decimal>("intQuantity");
				eRPPartTransactionCostInformationDto.intRowVersion = dataTable.Rows[i].Field<byte[]>("intRowVersion");
				eRPPartTransactionCostInformationDto.intPartTransactionCostID = dataTable.Rows[i].Field<int>("intPartTransactionCostID");
				eRPPartTransactionCostInformationDto.intSourceTableName = dataTable.Rows[i].Field<string>("intSourceTableName");
				eRPPartTransactionCostInformationDto.intSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("intSourceTableUniqueID");
				eRPPartTransactionCostInformationDto.intUnitDutyCost = dataTable.Rows[i].Field<decimal>("intUnitDutyCost");
				eRPPartTransactionCostInformationDto.intUnitFreightCost = dataTable.Rows[i].Field<decimal>("intUnitFreightCost");
				eRPPartTransactionCostInformationDto.intUnitLaborCost = dataTable.Rows[i].Field<decimal>("intUnitLaborCost");
				eRPPartTransactionCostInformationDto.intUnitMaterialCost = dataTable.Rows[i].Field<decimal>("intUnitMaterialCost");
				eRPPartTransactionCostInformationDto.intUnitMiscCost = dataTable.Rows[i].Field<decimal>("intUnitMiscCost");
				eRPPartTransactionCostInformationDto.intUnitOverheadCost = dataTable.Rows[i].Field<decimal>("intUnitOverheadCost");
				eRPPartTransactionCostInformationDto.intUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("intUnitSubcontractCost");
				eRPPartTransactionCostInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartTransactionCostInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartTransactionCostInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartTransactionCostInformationDto> GetPartTransactionCost(Guid partTransactionCostId)
	{
		ERPPartTransactionCostInformationDto eRPPartTransactionCostInformationDto = new ERPPartTransactionCostInformationDto();
		InitializeParameterLists();
		string[] collection = new string[31]
		{
			"intActualUnitDutyCost", "intActualUnitFreightCost", "intActualUnitLaborCost", "intActualUnitMaterialCost", "intActualUnitMiscCost", "intActualUnitOverheadCost", "intActualUnitSubcontractCost", "intCostType", "intCreatedBy", "intCreatedDate",
			"intUniqueID", "intPartTransactionID", "intPrevUnitDutyCost", "intPrevUnitFreightCost", "intPrevUnitLaborCost", "intPrevUnitMaterialCost", "intPrevUnitMiscCost", "intPrevUnitOverheadCost", "intPrevUnitSubcontractCost", "intQuantity",
			"intRowVersion", "intPartTransactionCostID", "intSourceTableName", "intSourceTableUniqueID", "intUnitDutyCost", "intUnitFreightCost", "intUnitLaborCost", "intUnitMaterialCost", "intUnitMiscCost", "intUnitOverheadCost",
			"intUnitSubcontractCost"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("intUniqueID|C", partTransactionCostId);
		AddCustomFieldsToSelectList("PartTransactionCosts");
		using (DataTable dataTable = GetAsDataTable("PartTransactionCosts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartTransactionCostInformationDto);
			}
			eRPPartTransactionCostInformationDto.intActualUnitDutyCost = dataTable.Rows[0].Field<decimal>("intActualUnitDutyCost");
			eRPPartTransactionCostInformationDto.intActualUnitFreightCost = dataTable.Rows[0].Field<decimal>("intActualUnitFreightCost");
			eRPPartTransactionCostInformationDto.intActualUnitLaborCost = dataTable.Rows[0].Field<decimal>("intActualUnitLaborCost");
			eRPPartTransactionCostInformationDto.intActualUnitMaterialCost = dataTable.Rows[0].Field<decimal>("intActualUnitMaterialCost");
			eRPPartTransactionCostInformationDto.intActualUnitMiscCost = dataTable.Rows[0].Field<decimal>("intActualUnitMiscCost");
			eRPPartTransactionCostInformationDto.intActualUnitOverheadCost = dataTable.Rows[0].Field<decimal>("intActualUnitOverheadCost");
			eRPPartTransactionCostInformationDto.intActualUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("intActualUnitSubcontractCost");
			eRPPartTransactionCostInformationDto.intCostType = dataTable.Rows[0].Field<byte>("intCostType");
			eRPPartTransactionCostInformationDto.intCreatedBy = dataTable.Rows[0].Field<string>("intCreatedBy");
			eRPPartTransactionCostInformationDto.intCreatedDate = dataTable.Rows[0].Field<DateTime?>("intCreatedDate");
			eRPPartTransactionCostInformationDto.intUniqueID = dataTable.Rows[0].Field<Guid>("intUniqueID");
			eRPPartTransactionCostInformationDto.intPartTransactionID = dataTable.Rows[0].Field<int>("intPartTransactionID");
			eRPPartTransactionCostInformationDto.intPrevUnitDutyCost = dataTable.Rows[0].Field<decimal>("intPrevUnitDutyCost");
			eRPPartTransactionCostInformationDto.intPrevUnitFreightCost = dataTable.Rows[0].Field<decimal>("intPrevUnitFreightCost");
			eRPPartTransactionCostInformationDto.intPrevUnitLaborCost = dataTable.Rows[0].Field<decimal>("intPrevUnitLaborCost");
			eRPPartTransactionCostInformationDto.intPrevUnitMaterialCost = dataTable.Rows[0].Field<decimal>("intPrevUnitMaterialCost");
			eRPPartTransactionCostInformationDto.intPrevUnitMiscCost = dataTable.Rows[0].Field<decimal>("intPrevUnitMiscCost");
			eRPPartTransactionCostInformationDto.intPrevUnitOverheadCost = dataTable.Rows[0].Field<decimal>("intPrevUnitOverheadCost");
			eRPPartTransactionCostInformationDto.intPrevUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("intPrevUnitSubcontractCost");
			eRPPartTransactionCostInformationDto.intQuantity = dataTable.Rows[0].Field<decimal>("intQuantity");
			eRPPartTransactionCostInformationDto.intRowVersion = dataTable.Rows[0].Field<byte[]>("intRowVersion");
			eRPPartTransactionCostInformationDto.intPartTransactionCostID = dataTable.Rows[0].Field<int>("intPartTransactionCostID");
			eRPPartTransactionCostInformationDto.intSourceTableName = dataTable.Rows[0].Field<string>("intSourceTableName");
			eRPPartTransactionCostInformationDto.intSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("intSourceTableUniqueID");
			eRPPartTransactionCostInformationDto.intUnitDutyCost = dataTable.Rows[0].Field<decimal>("intUnitDutyCost");
			eRPPartTransactionCostInformationDto.intUnitFreightCost = dataTable.Rows[0].Field<decimal>("intUnitFreightCost");
			eRPPartTransactionCostInformationDto.intUnitLaborCost = dataTable.Rows[0].Field<decimal>("intUnitLaborCost");
			eRPPartTransactionCostInformationDto.intUnitMaterialCost = dataTable.Rows[0].Field<decimal>("intUnitMaterialCost");
			eRPPartTransactionCostInformationDto.intUnitMiscCost = dataTable.Rows[0].Field<decimal>("intUnitMiscCost");
			eRPPartTransactionCostInformationDto.intUnitOverheadCost = dataTable.Rows[0].Field<decimal>("intUnitOverheadCost");
			eRPPartTransactionCostInformationDto.intUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("intUnitSubcontractCost");
			eRPPartTransactionCostInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartTransactionCostInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartTransactionCostInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartTransactionCost(ERPPartTransactionCostDto partTransactionCost)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartTransactionCosts WHERE intUniqueID = " + M1Util.ConvertToLinq(partTransactionCost.intUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["intPartTransactionID"] = partTransactionCost.intPartTransactionID;
				dataRow["intPartTransactionCostID"] = partTransactionCost.intPartTransactionCostID;
				partTransactionCost.intUniqueID = ((partTransactionCost.intUniqueID == Guid.Empty) ? Guid.NewGuid() : partTransactionCost.intUniqueID);
				dataRow["intUniqueID"] = partTransactionCost.intUniqueID;
				dataRow["intCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["intCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartTransactionCost could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partTransactionCost.intRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartTransactionCost is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["intRowVersion"], partTransactionCost.intRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartTransactionCost has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartTransactionCost again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["intActualUnitDutyCost"] = partTransactionCost.intActualUnitDutyCost;
			dataRow["intActualUnitFreightCost"] = partTransactionCost.intActualUnitFreightCost;
			dataRow["intActualUnitLaborCost"] = partTransactionCost.intActualUnitLaborCost;
			dataRow["intActualUnitMaterialCost"] = partTransactionCost.intActualUnitMaterialCost;
			dataRow["intActualUnitMiscCost"] = partTransactionCost.intActualUnitMiscCost;
			dataRow["intActualUnitOverheadCost"] = partTransactionCost.intActualUnitOverheadCost;
			dataRow["intActualUnitSubcontractCost"] = partTransactionCost.intActualUnitSubcontractCost;
			dataRow["intCostType"] = partTransactionCost.intCostType;
			dataRow["intPrevUnitDutyCost"] = partTransactionCost.intPrevUnitDutyCost;
			dataRow["intPrevUnitFreightCost"] = partTransactionCost.intPrevUnitFreightCost;
			dataRow["intPrevUnitLaborCost"] = partTransactionCost.intPrevUnitLaborCost;
			dataRow["intPrevUnitMaterialCost"] = partTransactionCost.intPrevUnitMaterialCost;
			dataRow["intPrevUnitMiscCost"] = partTransactionCost.intPrevUnitMiscCost;
			dataRow["intPrevUnitOverheadCost"] = partTransactionCost.intPrevUnitOverheadCost;
			dataRow["intPrevUnitSubcontractCost"] = partTransactionCost.intPrevUnitSubcontractCost;
			dataRow["intQuantity"] = partTransactionCost.intQuantity;
			dataRow["intSourceTableName"] = partTransactionCost.intSourceTableName;
			dataRow["intSourceTableUniqueID"] = partTransactionCost.intSourceTableUniqueID;
			dataRow["intUnitDutyCost"] = partTransactionCost.intUnitDutyCost;
			dataRow["intUnitFreightCost"] = partTransactionCost.intUnitFreightCost;
			dataRow["intUnitLaborCost"] = partTransactionCost.intUnitLaborCost;
			dataRow["intUnitMaterialCost"] = partTransactionCost.intUnitMaterialCost;
			dataRow["intUnitMiscCost"] = partTransactionCost.intUnitMiscCost;
			dataRow["intUnitOverheadCost"] = partTransactionCost.intUnitOverheadCost;
			dataRow["intUnitSubcontractCost"] = partTransactionCost.intUnitSubcontractCost;
			if (partTransactionCost.CustomFields != null && partTransactionCost.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partTransactionCost.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartTransactionCost [{partTransactionCost.intUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartTransactionCost [{partTransactionCost.intUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
