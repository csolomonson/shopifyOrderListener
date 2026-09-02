using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;
using M1.API.Utilities;

namespace M1.API.Repositories.Core.Transaction;

public class MaterialIssueRepository : APIBaseRepository, IMaterialIssueRepository, IAPIBaseRepository, IDisposable
{
	private const string _olDate = "01/01/1900";

	private readonly string[] _fields = new string[10] { "iniMaterialIssueID", "iniCreatedBy", "iniCreatedDate", "iniUniqueID", "iniPosted", "iniReversalEntry", "iniReversed", "iniMaterialIssueDate", "iniPostedDate", "iniRowVersion" };

	public MaterialIssueRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesMaterialIssueExists(string materialIssueId)
	{
		InitializeParameterLists();
		base.filterList.Add("iniMaterialIssueID|C", materialIssueId);
		base.selectList.Add("iniMaterialIssueID");
		return Task.FromResult(GetAsObject("MaterialIssues", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<MaterialIssueDto>> GetAllMaterialIssues(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<MaterialIssueDto> collection = new List<MaterialIssueDto>();
		InitializeParameterLists();
		base.selectList.AddRange(_fields);
		List<string> orderbyList = new List<string> { "iniMaterialIssueID" };
		using (DataTable dataTable = GetAsDataTable("MaterialIssues", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				MaterialIssueDto materialIssueDto = new MaterialIssueDto();
				materialIssueDto.MaterialIssueID = dataTable.Rows[i].Field<string>("iniMaterialIssueID").ToString().Trim();
				materialIssueDto.CreatedBy = dataTable.Rows[i].Field<string>("iniCreatedBy").ToString().Trim();
				materialIssueDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("iniCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("iniCreatedDate"));
				materialIssueDto.UniqueID = dataTable.Rows[i].Field<Guid>("iniUniqueID");
				materialIssueDto.Posted = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["iniPosted"]));
				materialIssueDto.ReversalEntry = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["iniReversalEntry"]));
				materialIssueDto.Reversed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["iniReversed"]));
				materialIssueDto.MaterialIssueDate = dataTable.Rows[i].Field<DateTime?>("iniMaterialIssueDate");
				materialIssueDto.PostedDate = dataTable.Rows[i].Field<DateTime?>("iniPostedDate");
				materialIssueDto.RowVersion = dataTable.Rows[i].Field<byte[]>("iniRowVersion");
				collection.Add(materialIssueDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<MaterialIssueDto> GetMaterialIssue(string materialIssueId)
	{
		MaterialIssueDto materialIssueDto = new MaterialIssueDto();
		InitializeParameterLists();
		base.selectList.AddRange(_fields);
		base.filterList.Add(Guid.TryParse(materialIssueId, out var _) ? "iniUniqueID|C" : "iniMaterialIssueID|C", materialIssueId);
		using (DataTable dataTable = GetAsDataTable("MaterialIssues", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(materialIssueDto);
			}
			materialIssueDto.MaterialIssueID = dataTable.Rows[0].Field<string>("iniMaterialIssueID").ToString().Trim();
			materialIssueDto.CreatedBy = dataTable.Rows[0].Field<string>("iniCreatedBy").ToString().Trim();
			materialIssueDto.CreatedDate = ((!dataTable.Rows[0].Field<DateTime?>("iniCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[0].Field<DateTime?>("iniCreatedDate"));
			materialIssueDto.UniqueID = dataTable.Rows[0].Field<Guid>("iniUniqueID");
			materialIssueDto.Posted = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["iniPosted"]));
			materialIssueDto.ReversalEntry = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["iniReversalEntry"]));
			materialIssueDto.Reversed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["iniReversed"]));
			materialIssueDto.MaterialIssueDate = dataTable.Rows[0].Field<DateTime?>("iniMaterialIssueDate");
			materialIssueDto.PostedDate = dataTable.Rows[0].Field<DateTime?>("iniPostedDate");
			materialIssueDto.RowVersion = dataTable.Rows[0].Field<byte[]>("iniRowVersion");
		}
		return Task.FromResult(materialIssueDto);
	}

	public Task<string> GetMaterialIssueIdFromGuid(string materialIssueId)
	{
		InitializeParameterLists();
		base.filterList.Add("iniUniqueID|C", materialIssueId);
		base.selectList.Add("iniMaterialIssueID");
		return Task.FromResult(GetAsObject("MaterialIssues", base.filterList, base.selectList, null, null)?.ToString());
	}
}
