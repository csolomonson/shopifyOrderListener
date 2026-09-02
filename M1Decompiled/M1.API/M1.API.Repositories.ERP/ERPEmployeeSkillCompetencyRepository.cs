using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPEmployeeSkillCompetencyRepository : APIBaseRepository, IERPEmployeeSkillCompetencyRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeSkillCompetencyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeSkillCompetencyExist(Guid employeeSkillCompetencyId)
	{
		InitializeParameterLists();
		base.filterList.Add("lnpUniqueID|C", employeeSkillCompetencyId);
		base.selectList.Add("lnpUniqueID");
		return Task.FromResult(GetAsObject("EmployeeSkillCompetencies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeSkillCompetencyInformationDto>> GetAllEmployeeSkillCompetencies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeSkillCompetencyInformationDto> collection = new List<ERPEmployeeSkillCompetencyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"lnpCommentsRTF", "lnpCommentsText", "lnpCompetencyID", "lnpCreatedBy", "lnpCreatedDate", "lnpDateAchieved", "lnpDateExpires", "lnpEmployeeID", "lnpEmployeeSkillID", "lnpUniqueID",
			"lnpRowVersion", "lnpEmployeeSkillCompetencyID", "lnpSkillID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeSkillCompetencies");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeSkillCompetencies", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeSkillCompetencyInformationDto eRPEmployeeSkillCompetencyInformationDto = new ERPEmployeeSkillCompetencyInformationDto();
				eRPEmployeeSkillCompetencyInformationDto.lnpCommentsRTF = dataTable.Rows[i].Field<string>("lnpCommentsRTF");
				eRPEmployeeSkillCompetencyInformationDto.lnpCommentsText = dataTable.Rows[i].Field<string>("lnpCommentsText");
				eRPEmployeeSkillCompetencyInformationDto.lnpCompetencyID = dataTable.Rows[i].Field<string>("lnpCompetencyID");
				eRPEmployeeSkillCompetencyInformationDto.lnpCreatedBy = dataTable.Rows[i].Field<string>("lnpCreatedBy");
				eRPEmployeeSkillCompetencyInformationDto.lnpCreatedDate = dataTable.Rows[i].Field<DateTime?>("lnpCreatedDate");
				eRPEmployeeSkillCompetencyInformationDto.lnpDateAchieved = dataTable.Rows[i].Field<DateTime?>("lnpDateAchieved");
				eRPEmployeeSkillCompetencyInformationDto.lnpDateExpires = dataTable.Rows[i].Field<DateTime?>("lnpDateExpires");
				eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeID = dataTable.Rows[i].Field<string>("lnpEmployeeID");
				eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeSkillID = dataTable.Rows[i].Field<short>("lnpEmployeeSkillID");
				eRPEmployeeSkillCompetencyInformationDto.lnpUniqueID = dataTable.Rows[i].Field<Guid>("lnpUniqueID");
				eRPEmployeeSkillCompetencyInformationDto.lnpRowVersion = dataTable.Rows[i].Field<byte[]>("lnpRowVersion");
				eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeSkillCompetencyID = dataTable.Rows[i].Field<short>("lnpEmployeeSkillCompetencyID");
				eRPEmployeeSkillCompetencyInformationDto.lnpSkillID = dataTable.Rows[i].Field<string>("lnpSkillID");
				eRPEmployeeSkillCompetencyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeSkillCompetencyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeSkillCompetencyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeSkillCompetencyInformationDto> GetEmployeeSkillCompetency(Guid employeeSkillCompetencyId)
	{
		ERPEmployeeSkillCompetencyInformationDto eRPEmployeeSkillCompetencyInformationDto = new ERPEmployeeSkillCompetencyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"lnpCommentsRTF", "lnpCommentsText", "lnpCompetencyID", "lnpCreatedBy", "lnpCreatedDate", "lnpDateAchieved", "lnpDateExpires", "lnpEmployeeID", "lnpEmployeeSkillID", "lnpUniqueID",
			"lnpRowVersion", "lnpEmployeeSkillCompetencyID", "lnpSkillID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lnpUniqueID|C", employeeSkillCompetencyId);
		AddCustomFieldsToSelectList("EmployeeSkillCompetencies");
		using (DataTable dataTable = GetAsDataTable("EmployeeSkillCompetencies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeSkillCompetencyInformationDto);
			}
			eRPEmployeeSkillCompetencyInformationDto.lnpCommentsRTF = dataTable.Rows[0].Field<string>("lnpCommentsRTF");
			eRPEmployeeSkillCompetencyInformationDto.lnpCommentsText = dataTable.Rows[0].Field<string>("lnpCommentsText");
			eRPEmployeeSkillCompetencyInformationDto.lnpCompetencyID = dataTable.Rows[0].Field<string>("lnpCompetencyID");
			eRPEmployeeSkillCompetencyInformationDto.lnpCreatedBy = dataTable.Rows[0].Field<string>("lnpCreatedBy");
			eRPEmployeeSkillCompetencyInformationDto.lnpCreatedDate = dataTable.Rows[0].Field<DateTime?>("lnpCreatedDate");
			eRPEmployeeSkillCompetencyInformationDto.lnpDateAchieved = dataTable.Rows[0].Field<DateTime?>("lnpDateAchieved");
			eRPEmployeeSkillCompetencyInformationDto.lnpDateExpires = dataTable.Rows[0].Field<DateTime?>("lnpDateExpires");
			eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeID = dataTable.Rows[0].Field<string>("lnpEmployeeID");
			eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeSkillID = dataTable.Rows[0].Field<short>("lnpEmployeeSkillID");
			eRPEmployeeSkillCompetencyInformationDto.lnpUniqueID = dataTable.Rows[0].Field<Guid>("lnpUniqueID");
			eRPEmployeeSkillCompetencyInformationDto.lnpRowVersion = dataTable.Rows[0].Field<byte[]>("lnpRowVersion");
			eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeSkillCompetencyID = dataTable.Rows[0].Field<short>("lnpEmployeeSkillCompetencyID");
			eRPEmployeeSkillCompetencyInformationDto.lnpSkillID = dataTable.Rows[0].Field<string>("lnpSkillID");
			eRPEmployeeSkillCompetencyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeSkillCompetencyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeSkillCompetencyInformationDto);
	}
}
