using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPWorkCenterSkillCompetencyRepository : APIBaseRepository, IERPWorkCenterSkillCompetencyRepository, IAPIBaseRepository, IDisposable
{
	public ERPWorkCenterSkillCompetencyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWorkCenterSkillCompetencyExist(Guid workCenterSkillCompetencyId)
	{
		InitializeParameterLists();
		base.filterList.Add("xbbUniqueID|C", workCenterSkillCompetencyId);
		base.selectList.Add("xbbUniqueID");
		return Task.FromResult(GetAsObject("WorkCenterSkillCompetencies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWorkCenterSkillCompetencyInformationDto>> GetAllWorkCenterSkillCompetencies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWorkCenterSkillCompetencyInformationDto> collection = new List<ERPWorkCenterSkillCompetencyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"xbbCommentsRTF", "xbbCommentsText", "xbbCompetencyID", "xbbCreatedBy", "xbbCreatedDate", "xbbDateAchieved", "xbbDateExpires", "xbbUniqueID", "xbbRowVersion", "xbbWorkCenterSkillCompetencyID",
			"xbbSkillID", "xbbWorkCenterID", "xbbWorkCenterSkillID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WorkCenterSkillCompetencies");
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
		using (DataTable dataTable = GetAsDataTable("WorkCenterSkillCompetencies", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWorkCenterSkillCompetencyInformationDto eRPWorkCenterSkillCompetencyInformationDto = new ERPWorkCenterSkillCompetencyInformationDto();
				eRPWorkCenterSkillCompetencyInformationDto.xbbCommentsRTF = dataTable.Rows[i].Field<string>("xbbCommentsRTF");
				eRPWorkCenterSkillCompetencyInformationDto.xbbCommentsText = dataTable.Rows[i].Field<string>("xbbCommentsText");
				eRPWorkCenterSkillCompetencyInformationDto.xbbCompetencyID = dataTable.Rows[i].Field<string>("xbbCompetencyID");
				eRPWorkCenterSkillCompetencyInformationDto.xbbCreatedBy = dataTable.Rows[i].Field<string>("xbbCreatedBy");
				eRPWorkCenterSkillCompetencyInformationDto.xbbCreatedDate = dataTable.Rows[i].Field<DateTime?>("xbbCreatedDate");
				eRPWorkCenterSkillCompetencyInformationDto.xbbDateAchieved = dataTable.Rows[i].Field<DateTime?>("xbbDateAchieved");
				eRPWorkCenterSkillCompetencyInformationDto.xbbDateExpires = dataTable.Rows[i].Field<DateTime?>("xbbDateExpires");
				eRPWorkCenterSkillCompetencyInformationDto.xbbUniqueID = dataTable.Rows[i].Field<Guid>("xbbUniqueID");
				eRPWorkCenterSkillCompetencyInformationDto.xbbRowVersion = dataTable.Rows[i].Field<byte[]>("xbbRowVersion");
				eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterSkillCompetencyID = dataTable.Rows[i].Field<short>("xbbWorkCenterSkillCompetencyID");
				eRPWorkCenterSkillCompetencyInformationDto.xbbSkillID = dataTable.Rows[i].Field<string>("xbbSkillID");
				eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterID = dataTable.Rows[i].Field<string>("xbbWorkCenterID");
				eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterSkillID = dataTable.Rows[i].Field<short>("xbbWorkCenterSkillID");
				eRPWorkCenterSkillCompetencyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWorkCenterSkillCompetencyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWorkCenterSkillCompetencyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWorkCenterSkillCompetencyInformationDto> GetWorkCenterSkillCompetency(Guid workCenterSkillCompetencyId)
	{
		ERPWorkCenterSkillCompetencyInformationDto eRPWorkCenterSkillCompetencyInformationDto = new ERPWorkCenterSkillCompetencyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"xbbCommentsRTF", "xbbCommentsText", "xbbCompetencyID", "xbbCreatedBy", "xbbCreatedDate", "xbbDateAchieved", "xbbDateExpires", "xbbUniqueID", "xbbRowVersion", "xbbWorkCenterSkillCompetencyID",
			"xbbSkillID", "xbbWorkCenterID", "xbbWorkCenterSkillID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xbbUniqueID|C", workCenterSkillCompetencyId);
		AddCustomFieldsToSelectList("WorkCenterSkillCompetencies");
		using (DataTable dataTable = GetAsDataTable("WorkCenterSkillCompetencies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWorkCenterSkillCompetencyInformationDto);
			}
			eRPWorkCenterSkillCompetencyInformationDto.xbbCommentsRTF = dataTable.Rows[0].Field<string>("xbbCommentsRTF");
			eRPWorkCenterSkillCompetencyInformationDto.xbbCommentsText = dataTable.Rows[0].Field<string>("xbbCommentsText");
			eRPWorkCenterSkillCompetencyInformationDto.xbbCompetencyID = dataTable.Rows[0].Field<string>("xbbCompetencyID");
			eRPWorkCenterSkillCompetencyInformationDto.xbbCreatedBy = dataTable.Rows[0].Field<string>("xbbCreatedBy");
			eRPWorkCenterSkillCompetencyInformationDto.xbbCreatedDate = dataTable.Rows[0].Field<DateTime?>("xbbCreatedDate");
			eRPWorkCenterSkillCompetencyInformationDto.xbbDateAchieved = dataTable.Rows[0].Field<DateTime?>("xbbDateAchieved");
			eRPWorkCenterSkillCompetencyInformationDto.xbbDateExpires = dataTable.Rows[0].Field<DateTime?>("xbbDateExpires");
			eRPWorkCenterSkillCompetencyInformationDto.xbbUniqueID = dataTable.Rows[0].Field<Guid>("xbbUniqueID");
			eRPWorkCenterSkillCompetencyInformationDto.xbbRowVersion = dataTable.Rows[0].Field<byte[]>("xbbRowVersion");
			eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterSkillCompetencyID = dataTable.Rows[0].Field<short>("xbbWorkCenterSkillCompetencyID");
			eRPWorkCenterSkillCompetencyInformationDto.xbbSkillID = dataTable.Rows[0].Field<string>("xbbSkillID");
			eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterID = dataTable.Rows[0].Field<string>("xbbWorkCenterID");
			eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterSkillID = dataTable.Rows[0].Field<short>("xbbWorkCenterSkillID");
			eRPWorkCenterSkillCompetencyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWorkCenterSkillCompetencyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWorkCenterSkillCompetencyInformationDto);
	}
}
