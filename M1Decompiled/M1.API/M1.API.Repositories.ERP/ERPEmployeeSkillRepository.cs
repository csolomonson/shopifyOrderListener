using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPEmployeeSkillRepository : APIBaseRepository, IERPEmployeeSkillRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeSkillRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeSkillExist(Guid employeeSkillId)
	{
		InitializeParameterLists();
		base.filterList.Add("lnkUniqueID|C", employeeSkillId);
		base.selectList.Add("lnkUniqueID");
		return Task.FromResult(GetAsObject("EmployeeSkills", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeSkillInformationDto>> GetAllEmployeeSkills(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeSkillInformationDto> collection = new List<ERPEmployeeSkillInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "lnkCreatedBy", "lnkCreatedDate", "lnkDocuments", "lnkEmployeeID", "lnkUniqueID", "lnkNotesRTF", "lnkNotesText", "lnkRowVersion", "lnkEmployeeSkillID", "lnkSkillID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeSkills");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeSkills", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeSkillInformationDto eRPEmployeeSkillInformationDto = new ERPEmployeeSkillInformationDto();
				eRPEmployeeSkillInformationDto.lnkCreatedBy = dataTable.Rows[i].Field<string>("lnkCreatedBy");
				eRPEmployeeSkillInformationDto.lnkCreatedDate = dataTable.Rows[i].Field<DateTime?>("lnkCreatedDate");
				eRPEmployeeSkillInformationDto.lnkDocuments = dataTable.Rows[i].Field<string>("lnkDocuments");
				eRPEmployeeSkillInformationDto.lnkEmployeeID = dataTable.Rows[i].Field<string>("lnkEmployeeID");
				eRPEmployeeSkillInformationDto.lnkUniqueID = dataTable.Rows[i].Field<Guid>("lnkUniqueID");
				eRPEmployeeSkillInformationDto.lnkNotesRTF = dataTable.Rows[i].Field<string>("lnkNotesRTF");
				eRPEmployeeSkillInformationDto.lnkNotesText = dataTable.Rows[i].Field<string>("lnkNotesText");
				eRPEmployeeSkillInformationDto.lnkRowVersion = dataTable.Rows[i].Field<byte[]>("lnkRowVersion");
				eRPEmployeeSkillInformationDto.lnkEmployeeSkillID = dataTable.Rows[i].Field<short>("lnkEmployeeSkillID");
				eRPEmployeeSkillInformationDto.lnkSkillID = dataTable.Rows[i].Field<string>("lnkSkillID");
				eRPEmployeeSkillInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeSkillInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeSkillInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeSkillInformationDto> GetEmployeeSkill(Guid employeeSkillId)
	{
		ERPEmployeeSkillInformationDto eRPEmployeeSkillInformationDto = new ERPEmployeeSkillInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "lnkCreatedBy", "lnkCreatedDate", "lnkDocuments", "lnkEmployeeID", "lnkUniqueID", "lnkNotesRTF", "lnkNotesText", "lnkRowVersion", "lnkEmployeeSkillID", "lnkSkillID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lnkUniqueID|C", employeeSkillId);
		AddCustomFieldsToSelectList("EmployeeSkills");
		using (DataTable dataTable = GetAsDataTable("EmployeeSkills", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeSkillInformationDto);
			}
			eRPEmployeeSkillInformationDto.lnkCreatedBy = dataTable.Rows[0].Field<string>("lnkCreatedBy");
			eRPEmployeeSkillInformationDto.lnkCreatedDate = dataTable.Rows[0].Field<DateTime?>("lnkCreatedDate");
			eRPEmployeeSkillInformationDto.lnkDocuments = dataTable.Rows[0].Field<string>("lnkDocuments");
			eRPEmployeeSkillInformationDto.lnkEmployeeID = dataTable.Rows[0].Field<string>("lnkEmployeeID");
			eRPEmployeeSkillInformationDto.lnkUniqueID = dataTable.Rows[0].Field<Guid>("lnkUniqueID");
			eRPEmployeeSkillInformationDto.lnkNotesRTF = dataTable.Rows[0].Field<string>("lnkNotesRTF");
			eRPEmployeeSkillInformationDto.lnkNotesText = dataTable.Rows[0].Field<string>("lnkNotesText");
			eRPEmployeeSkillInformationDto.lnkRowVersion = dataTable.Rows[0].Field<byte[]>("lnkRowVersion");
			eRPEmployeeSkillInformationDto.lnkEmployeeSkillID = dataTable.Rows[0].Field<short>("lnkEmployeeSkillID");
			eRPEmployeeSkillInformationDto.lnkSkillID = dataTable.Rows[0].Field<string>("lnkSkillID");
			eRPEmployeeSkillInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeSkillInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeSkillInformationDto);
	}
}
