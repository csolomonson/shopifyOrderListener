using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPWorkCenterSkillRepository : APIBaseRepository, IERPWorkCenterSkillRepository, IAPIBaseRepository, IDisposable
{
	public ERPWorkCenterSkillRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWorkCenterSkillExist(Guid workCenterSkillId)
	{
		InitializeParameterLists();
		base.filterList.Add("xbaUniqueID|C", workCenterSkillId);
		base.selectList.Add("xbaUniqueID");
		return Task.FromResult(GetAsObject("WorkCenterSkills", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWorkCenterSkillInformationDto>> GetAllWorkCenterSkills(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWorkCenterSkillInformationDto> collection = new List<ERPWorkCenterSkillInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "xbaCreatedBy", "xbaCreatedDate", "xbaDocuments", "xbaUniqueID", "xbaNotesRTF", "xbaNotesText", "xbaRowVersion", "xbaWorkCenterSkillID", "xbaSkillID", "xbaWorkCenterID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WorkCenterSkills");
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
		using (DataTable dataTable = GetAsDataTable("WorkCenterSkills", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWorkCenterSkillInformationDto eRPWorkCenterSkillInformationDto = new ERPWorkCenterSkillInformationDto();
				eRPWorkCenterSkillInformationDto.xbaCreatedBy = dataTable.Rows[i].Field<string>("xbaCreatedBy");
				eRPWorkCenterSkillInformationDto.xbaCreatedDate = dataTable.Rows[i].Field<DateTime?>("xbaCreatedDate");
				eRPWorkCenterSkillInformationDto.xbaDocuments = dataTable.Rows[i].Field<string>("xbaDocuments");
				eRPWorkCenterSkillInformationDto.xbaUniqueID = dataTable.Rows[i].Field<Guid>("xbaUniqueID");
				eRPWorkCenterSkillInformationDto.xbaNotesRTF = dataTable.Rows[i].Field<string>("xbaNotesRTF");
				eRPWorkCenterSkillInformationDto.xbaNotesText = dataTable.Rows[i].Field<string>("xbaNotesText");
				eRPWorkCenterSkillInformationDto.xbaRowVersion = dataTable.Rows[i].Field<byte[]>("xbaRowVersion");
				eRPWorkCenterSkillInformationDto.xbaWorkCenterSkillID = dataTable.Rows[i].Field<short>("xbaWorkCenterSkillID");
				eRPWorkCenterSkillInformationDto.xbaSkillID = dataTable.Rows[i].Field<string>("xbaSkillID");
				eRPWorkCenterSkillInformationDto.xbaWorkCenterID = dataTable.Rows[i].Field<string>("xbaWorkCenterID");
				eRPWorkCenterSkillInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWorkCenterSkillInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWorkCenterSkillInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWorkCenterSkillInformationDto> GetWorkCenterSkill(Guid workCenterSkillId)
	{
		ERPWorkCenterSkillInformationDto eRPWorkCenterSkillInformationDto = new ERPWorkCenterSkillInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "xbaCreatedBy", "xbaCreatedDate", "xbaDocuments", "xbaUniqueID", "xbaNotesRTF", "xbaNotesText", "xbaRowVersion", "xbaWorkCenterSkillID", "xbaSkillID", "xbaWorkCenterID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xbaUniqueID|C", workCenterSkillId);
		AddCustomFieldsToSelectList("WorkCenterSkills");
		using (DataTable dataTable = GetAsDataTable("WorkCenterSkills", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWorkCenterSkillInformationDto);
			}
			eRPWorkCenterSkillInformationDto.xbaCreatedBy = dataTable.Rows[0].Field<string>("xbaCreatedBy");
			eRPWorkCenterSkillInformationDto.xbaCreatedDate = dataTable.Rows[0].Field<DateTime?>("xbaCreatedDate");
			eRPWorkCenterSkillInformationDto.xbaDocuments = dataTable.Rows[0].Field<string>("xbaDocuments");
			eRPWorkCenterSkillInformationDto.xbaUniqueID = dataTable.Rows[0].Field<Guid>("xbaUniqueID");
			eRPWorkCenterSkillInformationDto.xbaNotesRTF = dataTable.Rows[0].Field<string>("xbaNotesRTF");
			eRPWorkCenterSkillInformationDto.xbaNotesText = dataTable.Rows[0].Field<string>("xbaNotesText");
			eRPWorkCenterSkillInformationDto.xbaRowVersion = dataTable.Rows[0].Field<byte[]>("xbaRowVersion");
			eRPWorkCenterSkillInformationDto.xbaWorkCenterSkillID = dataTable.Rows[0].Field<short>("xbaWorkCenterSkillID");
			eRPWorkCenterSkillInformationDto.xbaSkillID = dataTable.Rows[0].Field<string>("xbaSkillID");
			eRPWorkCenterSkillInformationDto.xbaWorkCenterID = dataTable.Rows[0].Field<string>("xbaWorkCenterID");
			eRPWorkCenterSkillInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWorkCenterSkillInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWorkCenterSkillInformationDto);
	}
}
