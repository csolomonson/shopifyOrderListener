using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPSkillCompetencyRepository : APIBaseRepository, IERPSkillCompetencyRepository, IAPIBaseRepository, IDisposable
{
	public ERPSkillCompetencyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSkillCompetencyExist(Guid skillCompetencyId)
	{
		InitializeParameterLists();
		base.filterList.Add("lecUniqueID|C", skillCompetencyId);
		base.selectList.Add("lecUniqueID");
		return Task.FromResult(GetAsObject("SkillCompetencies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSkillCompetencyInformationDto>> GetAllSkillCompetencies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSkillCompetencyInformationDto> collection = new List<ERPSkillCompetencyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"lecColor", "lecCompetencyID", "lecCreatedBy", "lecCreatedDate", "lecDescription", "lecUniqueID", "lecInactiveDate", "lecInactive", "lecLevel", "lecLongDescriptionRtf",
			"lecLongDescriptionText", "lecRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SkillCompetencies");
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
		using (DataTable dataTable = GetAsDataTable("SkillCompetencies", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSkillCompetencyInformationDto eRPSkillCompetencyInformationDto = new ERPSkillCompetencyInformationDto();
				eRPSkillCompetencyInformationDto.lecColor = dataTable.Rows[i].Field<int>("lecColor");
				eRPSkillCompetencyInformationDto.lecCompetencyID = dataTable.Rows[i].Field<string>("lecCompetencyID");
				eRPSkillCompetencyInformationDto.lecCreatedBy = dataTable.Rows[i].Field<string>("lecCreatedBy");
				eRPSkillCompetencyInformationDto.lecCreatedDate = dataTable.Rows[i].Field<DateTime?>("lecCreatedDate");
				eRPSkillCompetencyInformationDto.lecDescription = dataTable.Rows[i].Field<string>("lecDescription");
				eRPSkillCompetencyInformationDto.lecUniqueID = dataTable.Rows[i].Field<Guid>("lecUniqueID");
				eRPSkillCompetencyInformationDto.lecInactiveDate = dataTable.Rows[i].Field<DateTime?>("lecInactiveDate");
				eRPSkillCompetencyInformationDto.lecInactive = dataTable.Rows[i].Field<bool>("lecInactive");
				eRPSkillCompetencyInformationDto.lecLevel = dataTable.Rows[i].Field<byte>("lecLevel");
				eRPSkillCompetencyInformationDto.lecLongDescriptionRtf = dataTable.Rows[i].Field<string>("lecLongDescriptionRtf");
				eRPSkillCompetencyInformationDto.lecLongDescriptionText = dataTable.Rows[i].Field<string>("lecLongDescriptionText");
				eRPSkillCompetencyInformationDto.lecRowVersion = dataTable.Rows[i].Field<byte[]>("lecRowVersion");
				eRPSkillCompetencyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSkillCompetencyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSkillCompetencyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSkillCompetencyInformationDto> GetSkillCompetency(Guid skillCompetencyId)
	{
		ERPSkillCompetencyInformationDto eRPSkillCompetencyInformationDto = new ERPSkillCompetencyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"lecColor", "lecCompetencyID", "lecCreatedBy", "lecCreatedDate", "lecDescription", "lecUniqueID", "lecInactiveDate", "lecInactive", "lecLevel", "lecLongDescriptionRtf",
			"lecLongDescriptionText", "lecRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lecUniqueID|C", skillCompetencyId);
		AddCustomFieldsToSelectList("SkillCompetencies");
		using (DataTable dataTable = GetAsDataTable("SkillCompetencies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSkillCompetencyInformationDto);
			}
			eRPSkillCompetencyInformationDto.lecColor = dataTable.Rows[0].Field<int>("lecColor");
			eRPSkillCompetencyInformationDto.lecCompetencyID = dataTable.Rows[0].Field<string>("lecCompetencyID");
			eRPSkillCompetencyInformationDto.lecCreatedBy = dataTable.Rows[0].Field<string>("lecCreatedBy");
			eRPSkillCompetencyInformationDto.lecCreatedDate = dataTable.Rows[0].Field<DateTime?>("lecCreatedDate");
			eRPSkillCompetencyInformationDto.lecDescription = dataTable.Rows[0].Field<string>("lecDescription");
			eRPSkillCompetencyInformationDto.lecUniqueID = dataTable.Rows[0].Field<Guid>("lecUniqueID");
			eRPSkillCompetencyInformationDto.lecInactiveDate = dataTable.Rows[0].Field<DateTime?>("lecInactiveDate");
			eRPSkillCompetencyInformationDto.lecInactive = dataTable.Rows[0].Field<bool>("lecInactive");
			eRPSkillCompetencyInformationDto.lecLevel = dataTable.Rows[0].Field<byte>("lecLevel");
			eRPSkillCompetencyInformationDto.lecLongDescriptionRtf = dataTable.Rows[0].Field<string>("lecLongDescriptionRtf");
			eRPSkillCompetencyInformationDto.lecLongDescriptionText = dataTable.Rows[0].Field<string>("lecLongDescriptionText");
			eRPSkillCompetencyInformationDto.lecRowVersion = dataTable.Rows[0].Field<byte[]>("lecRowVersion");
			eRPSkillCompetencyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSkillCompetencyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSkillCompetencyInformationDto);
	}
}
