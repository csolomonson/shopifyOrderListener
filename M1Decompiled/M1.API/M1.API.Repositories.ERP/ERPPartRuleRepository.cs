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

public class ERPPartRuleRepository : APIBaseRepository, IERPPartRuleRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartRuleRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartRuleExist(Guid partRuleId)
	{
		InitializeParameterLists();
		base.filterList.Add("pcrUniqueID|C", partRuleId);
		base.selectList.Add("pcrUniqueID");
		return Task.FromResult(GetAsObject("PartRules", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartRuleInformationDto>> GetAllPartRules(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartRuleInformationDto> collection = new List<ERPPartRuleInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"pcrCode", "pcrCreatedBy", "pcrCreatedDate", "pcrUniqueID", "pcrField", "pcrMethodAssemblyID", "pcrMethodID", "pcrMethodMaterialID", "pcrMethodOperationID", "pcrMethodRevisionID",
			"pcrMethodType", "pcrProcessSequence", "pcrRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartRules");
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
		using (DataTable dataTable = GetAsDataTable("PartRules", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartRuleInformationDto eRPPartRuleInformationDto = new ERPPartRuleInformationDto();
				eRPPartRuleInformationDto.pcrCode = dataTable.Rows[i].Field<string>("pcrCode");
				eRPPartRuleInformationDto.pcrCreatedBy = dataTable.Rows[i].Field<string>("pcrCreatedBy");
				eRPPartRuleInformationDto.pcrCreatedDate = dataTable.Rows[i].Field<DateTime?>("pcrCreatedDate");
				eRPPartRuleInformationDto.pcrUniqueID = dataTable.Rows[i].Field<Guid>("pcrUniqueID");
				eRPPartRuleInformationDto.pcrField = dataTable.Rows[i].Field<string>("pcrField");
				eRPPartRuleInformationDto.pcrMethodAssemblyID = dataTable.Rows[i].Field<int>("pcrMethodAssemblyID");
				eRPPartRuleInformationDto.pcrMethodID = dataTable.Rows[i].Field<string>("pcrMethodID");
				eRPPartRuleInformationDto.pcrMethodMaterialID = dataTable.Rows[i].Field<int>("pcrMethodMaterialID");
				eRPPartRuleInformationDto.pcrMethodOperationID = dataTable.Rows[i].Field<int>("pcrMethodOperationID");
				eRPPartRuleInformationDto.pcrMethodRevisionID = dataTable.Rows[i].Field<string>("pcrMethodRevisionID");
				eRPPartRuleInformationDto.pcrMethodType = dataTable.Rows[i].Field<byte>("pcrMethodType");
				eRPPartRuleInformationDto.pcrProcessSequence = dataTable.Rows[i].Field<short>("pcrProcessSequence");
				eRPPartRuleInformationDto.pcrRowVersion = dataTable.Rows[i].Field<byte[]>("pcrRowVersion");
				eRPPartRuleInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartRuleInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartRuleInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartRuleInformationDto> GetPartRule(Guid partRuleId)
	{
		ERPPartRuleInformationDto eRPPartRuleInformationDto = new ERPPartRuleInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"pcrCode", "pcrCreatedBy", "pcrCreatedDate", "pcrUniqueID", "pcrField", "pcrMethodAssemblyID", "pcrMethodID", "pcrMethodMaterialID", "pcrMethodOperationID", "pcrMethodRevisionID",
			"pcrMethodType", "pcrProcessSequence", "pcrRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pcrUniqueID|C", partRuleId);
		AddCustomFieldsToSelectList("PartRules");
		using (DataTable dataTable = GetAsDataTable("PartRules", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartRuleInformationDto);
			}
			eRPPartRuleInformationDto.pcrCode = dataTable.Rows[0].Field<string>("pcrCode");
			eRPPartRuleInformationDto.pcrCreatedBy = dataTable.Rows[0].Field<string>("pcrCreatedBy");
			eRPPartRuleInformationDto.pcrCreatedDate = dataTable.Rows[0].Field<DateTime?>("pcrCreatedDate");
			eRPPartRuleInformationDto.pcrUniqueID = dataTable.Rows[0].Field<Guid>("pcrUniqueID");
			eRPPartRuleInformationDto.pcrField = dataTable.Rows[0].Field<string>("pcrField");
			eRPPartRuleInformationDto.pcrMethodAssemblyID = dataTable.Rows[0].Field<int>("pcrMethodAssemblyID");
			eRPPartRuleInformationDto.pcrMethodID = dataTable.Rows[0].Field<string>("pcrMethodID");
			eRPPartRuleInformationDto.pcrMethodMaterialID = dataTable.Rows[0].Field<int>("pcrMethodMaterialID");
			eRPPartRuleInformationDto.pcrMethodOperationID = dataTable.Rows[0].Field<int>("pcrMethodOperationID");
			eRPPartRuleInformationDto.pcrMethodRevisionID = dataTable.Rows[0].Field<string>("pcrMethodRevisionID");
			eRPPartRuleInformationDto.pcrMethodType = dataTable.Rows[0].Field<byte>("pcrMethodType");
			eRPPartRuleInformationDto.pcrProcessSequence = dataTable.Rows[0].Field<short>("pcrProcessSequence");
			eRPPartRuleInformationDto.pcrRowVersion = dataTable.Rows[0].Field<byte[]>("pcrRowVersion");
			eRPPartRuleInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartRuleInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartRuleInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartRule(ERPPartRuleDto partRule)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartRules WHERE pcrUniqueID = " + M1Util.ConvertToLinq(partRule.pcrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pcrUniqueID"] = partRule.pcrUniqueID;
				partRule.pcrUniqueID = ((partRule.pcrUniqueID == Guid.Empty) ? Guid.NewGuid() : partRule.pcrUniqueID);
				dataRow["pcrUniqueID"] = partRule.pcrUniqueID;
				dataRow["pcrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pcrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartRule could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partRule.pcrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartRule is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pcrRowVersion"], partRule.pcrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartRule has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartRule again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pcrCode"] = partRule.pcrCode ?? dataRow["pcrCode"];
			dataRow["pcrField"] = partRule.pcrField;
			dataRow["pcrMethodAssemblyID"] = partRule.pcrMethodAssemblyID;
			dataRow["pcrMethodID"] = partRule.pcrMethodID;
			dataRow["pcrMethodMaterialID"] = partRule.pcrMethodMaterialID;
			dataRow["pcrMethodOperationID"] = partRule.pcrMethodOperationID;
			dataRow["pcrMethodRevisionID"] = partRule.pcrMethodRevisionID;
			dataRow["pcrMethodType"] = partRule.pcrMethodType;
			dataRow["pcrProcessSequence"] = partRule.pcrProcessSequence;
			if (partRule.CustomFields != null && partRule.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partRule.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartRule [{partRule.pcrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartRule [{partRule.pcrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
