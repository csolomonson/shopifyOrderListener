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

public class ERPMilestoneRepository : APIBaseRepository, IERPMilestoneRepository, IAPIBaseRepository, IDisposable
{
	public ERPMilestoneRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMilestoneExist(Guid milestoneId)
	{
		InitializeParameterLists();
		base.filterList.Add("losUniqueID|C", milestoneId);
		base.selectList.Add("losUniqueID");
		return Task.FromResult(GetAsObject("Milestones", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMilestoneInformationDto>> GetAllMilestones(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMilestoneInformationDto> collection = new List<ERPMilestoneInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "losMilestoneID", "losConfidenceFactor", "losCreatedBy", "losCreatedDate", "losUniqueID", "losLongDescriptionRtf", "losLongDescriptionText", "losRowVersion", "losShortDescription" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Milestones");
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
		using (DataTable dataTable = GetAsDataTable("Milestones", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMilestoneInformationDto eRPMilestoneInformationDto = new ERPMilestoneInformationDto();
				eRPMilestoneInformationDto.losMilestoneID = dataTable.Rows[i].Field<string>("losMilestoneID");
				eRPMilestoneInformationDto.losConfidenceFactor = dataTable.Rows[i].Field<decimal>("losConfidenceFactor");
				eRPMilestoneInformationDto.losCreatedBy = dataTable.Rows[i].Field<string>("losCreatedBy");
				eRPMilestoneInformationDto.losCreatedDate = dataTable.Rows[i].Field<DateTime?>("losCreatedDate");
				eRPMilestoneInformationDto.losUniqueID = dataTable.Rows[i].Field<Guid>("losUniqueID");
				eRPMilestoneInformationDto.losLongDescriptionRtf = dataTable.Rows[i].Field<string>("losLongDescriptionRtf");
				eRPMilestoneInformationDto.losLongDescriptionText = dataTable.Rows[i].Field<string>("losLongDescriptionText");
				eRPMilestoneInformationDto.losRowVersion = dataTable.Rows[i].Field<byte[]>("losRowVersion");
				eRPMilestoneInformationDto.losShortDescription = dataTable.Rows[i].Field<string>("losShortDescription");
				eRPMilestoneInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMilestoneInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMilestoneInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMilestoneInformationDto> GetMilestone(Guid milestoneId)
	{
		ERPMilestoneInformationDto eRPMilestoneInformationDto = new ERPMilestoneInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "losMilestoneID", "losConfidenceFactor", "losCreatedBy", "losCreatedDate", "losUniqueID", "losLongDescriptionRtf", "losLongDescriptionText", "losRowVersion", "losShortDescription" };
		base.selectList.AddRange(collection);
		base.filterList.Add("losUniqueID|C", milestoneId);
		AddCustomFieldsToSelectList("Milestones");
		using (DataTable dataTable = GetAsDataTable("Milestones", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMilestoneInformationDto);
			}
			eRPMilestoneInformationDto.losMilestoneID = dataTable.Rows[0].Field<string>("losMilestoneID");
			eRPMilestoneInformationDto.losConfidenceFactor = dataTable.Rows[0].Field<decimal>("losConfidenceFactor");
			eRPMilestoneInformationDto.losCreatedBy = dataTable.Rows[0].Field<string>("losCreatedBy");
			eRPMilestoneInformationDto.losCreatedDate = dataTable.Rows[0].Field<DateTime?>("losCreatedDate");
			eRPMilestoneInformationDto.losUniqueID = dataTable.Rows[0].Field<Guid>("losUniqueID");
			eRPMilestoneInformationDto.losLongDescriptionRtf = dataTable.Rows[0].Field<string>("losLongDescriptionRtf");
			eRPMilestoneInformationDto.losLongDescriptionText = dataTable.Rows[0].Field<string>("losLongDescriptionText");
			eRPMilestoneInformationDto.losRowVersion = dataTable.Rows[0].Field<byte[]>("losRowVersion");
			eRPMilestoneInformationDto.losShortDescription = dataTable.Rows[0].Field<string>("losShortDescription");
			eRPMilestoneInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMilestoneInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMilestoneInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMilestone(ERPMilestoneDto milestone)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Milestones WHERE losUniqueID = " + M1Util.ConvertToLinq(milestone.losUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["losMilestoneID"] = milestone.losMilestoneID.ToUpper();
				milestone.losUniqueID = ((milestone.losUniqueID == Guid.Empty) ? Guid.NewGuid() : milestone.losUniqueID);
				dataRow["losUniqueID"] = milestone.losUniqueID;
				dataRow["losCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["losCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Milestone could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (milestone.losRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Milestone is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["losRowVersion"], milestone.losRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Milestone has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Milestone again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["losConfidenceFactor"] = milestone.losConfidenceFactor;
			dataRow["losLongDescriptionRtf"] = milestone.losLongDescriptionRtf ?? dataRow["losLongDescriptionRtf"];
			dataRow["losLongDescriptionText"] = milestone.losLongDescriptionText ?? dataRow["losLongDescriptionText"];
			dataRow["losShortDescription"] = milestone.losShortDescription;
			if (milestone.CustomFields != null && milestone.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in milestone.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Milestone [{milestone.losUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Milestone [{milestone.losUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
