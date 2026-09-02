using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPSkillRepository : APIBaseRepository, IERPSkillRepository, IAPIBaseRepository, IDisposable
{
	public ERPSkillRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSkillExist(Guid skillId)
	{
		InitializeParameterLists();
		base.filterList.Add("lesUniqueID|C", skillId);
		base.selectList.Add("lesUniqueID");
		return Task.FromResult(GetAsObject("Skills", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSkillInformationDto>> GetAllSkills(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSkillInformationDto> collection = new List<ERPSkillInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "lesSkillID", "lesCreatedBy", "lesCreatedDate", "lesDescription", "lesUniqueID", "lesInactiveDate", "lesInactive", "lesLongDescriptionRtf", "lesLongDescriptionText", "lesRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Skills");
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
		using (DataTable dataTable = GetAsDataTable("Skills", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSkillInformationDto eRPSkillInformationDto = new ERPSkillInformationDto();
				eRPSkillInformationDto.lesSkillID = dataTable.Rows[i].Field<string>("lesSkillID");
				eRPSkillInformationDto.lesCreatedBy = dataTable.Rows[i].Field<string>("lesCreatedBy");
				eRPSkillInformationDto.lesCreatedDate = dataTable.Rows[i].Field<DateTime?>("lesCreatedDate");
				eRPSkillInformationDto.lesDescription = dataTable.Rows[i].Field<string>("lesDescription");
				eRPSkillInformationDto.lesUniqueID = dataTable.Rows[i].Field<Guid>("lesUniqueID");
				eRPSkillInformationDto.lesInactiveDate = dataTable.Rows[i].Field<DateTime?>("lesInactiveDate");
				eRPSkillInformationDto.lesInactive = dataTable.Rows[i].Field<bool>("lesInactive");
				eRPSkillInformationDto.lesLongDescriptionRtf = dataTable.Rows[i].Field<string>("lesLongDescriptionRtf");
				eRPSkillInformationDto.lesLongDescriptionText = dataTable.Rows[i].Field<string>("lesLongDescriptionText");
				eRPSkillInformationDto.lesRowVersion = dataTable.Rows[i].Field<byte[]>("lesRowVersion");
				eRPSkillInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSkillInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSkillInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSkillInformationDto> GetSkill(Guid skillId)
	{
		ERPSkillInformationDto eRPSkillInformationDto = new ERPSkillInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "lesSkillID", "lesCreatedBy", "lesCreatedDate", "lesDescription", "lesUniqueID", "lesInactiveDate", "lesInactive", "lesLongDescriptionRtf", "lesLongDescriptionText", "lesRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lesUniqueID|C", skillId);
		AddCustomFieldsToSelectList("Skills");
		using (DataTable dataTable = GetAsDataTable("Skills", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSkillInformationDto);
			}
			eRPSkillInformationDto.lesSkillID = dataTable.Rows[0].Field<string>("lesSkillID");
			eRPSkillInformationDto.lesCreatedBy = dataTable.Rows[0].Field<string>("lesCreatedBy");
			eRPSkillInformationDto.lesCreatedDate = dataTable.Rows[0].Field<DateTime?>("lesCreatedDate");
			eRPSkillInformationDto.lesDescription = dataTable.Rows[0].Field<string>("lesDescription");
			eRPSkillInformationDto.lesUniqueID = dataTable.Rows[0].Field<Guid>("lesUniqueID");
			eRPSkillInformationDto.lesInactiveDate = dataTable.Rows[0].Field<DateTime?>("lesInactiveDate");
			eRPSkillInformationDto.lesInactive = dataTable.Rows[0].Field<bool>("lesInactive");
			eRPSkillInformationDto.lesLongDescriptionRtf = dataTable.Rows[0].Field<string>("lesLongDescriptionRtf");
			eRPSkillInformationDto.lesLongDescriptionText = dataTable.Rows[0].Field<string>("lesLongDescriptionText");
			eRPSkillInformationDto.lesRowVersion = dataTable.Rows[0].Field<byte[]>("lesRowVersion");
			eRPSkillInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSkillInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSkillInformationDto);
	}
}
