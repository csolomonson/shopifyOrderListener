using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPGLFiscalYearOpeningBalanceRepository : APIBaseRepository, IERPGLFiscalYearOpeningBalanceRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearOpeningBalanceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearOpeningBalanceExist(Guid gLFiscalYearOpeningBalanceId)
	{
		InitializeParameterLists();
		base.filterList.Add("glyUniqueID|C", gLFiscalYearOpeningBalanceId);
		base.selectList.Add("glyUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYearOpeningBalances", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearOpeningBalanceInformationDto>> GetAllGLFiscalYearOpeningBalances(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearOpeningBalanceInformationDto> collection = new List<ERPGLFiscalYearOpeningBalanceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "glyCreatedBy", "glyCreatedDate", "glyUniqueID", "glyGlAccountID", "glyGlFiscalYearID", "glyRowVersion", "glyYearOpeningBalance" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYearOpeningBalances");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearOpeningBalances", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearOpeningBalanceInformationDto eRPGLFiscalYearOpeningBalanceInformationDto = new ERPGLFiscalYearOpeningBalanceInformationDto();
				eRPGLFiscalYearOpeningBalanceInformationDto.glyCreatedBy = dataTable.Rows[i].Field<string>("glyCreatedBy");
				eRPGLFiscalYearOpeningBalanceInformationDto.glyCreatedDate = dataTable.Rows[i].Field<DateTime?>("glyCreatedDate");
				eRPGLFiscalYearOpeningBalanceInformationDto.glyUniqueID = dataTable.Rows[i].Field<Guid>("glyUniqueID");
				eRPGLFiscalYearOpeningBalanceInformationDto.glyGlAccountID = dataTable.Rows[i].Field<string>("glyGlAccountID");
				eRPGLFiscalYearOpeningBalanceInformationDto.glyGlFiscalYearID = dataTable.Rows[i].Field<short>("glyGlFiscalYearID");
				eRPGLFiscalYearOpeningBalanceInformationDto.glyRowVersion = dataTable.Rows[i].Field<byte[]>("glyRowVersion");
				eRPGLFiscalYearOpeningBalanceInformationDto.glyYearOpeningBalance = dataTable.Rows[i].Field<decimal>("glyYearOpeningBalance");
				eRPGLFiscalYearOpeningBalanceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearOpeningBalanceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearOpeningBalanceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearOpeningBalanceInformationDto> GetGLFiscalYearOpeningBalance(Guid gLFiscalYearOpeningBalanceId)
	{
		ERPGLFiscalYearOpeningBalanceInformationDto eRPGLFiscalYearOpeningBalanceInformationDto = new ERPGLFiscalYearOpeningBalanceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "glyCreatedBy", "glyCreatedDate", "glyUniqueID", "glyGlAccountID", "glyGlFiscalYearID", "glyRowVersion", "glyYearOpeningBalance" };
		base.selectList.AddRange(collection);
		base.filterList.Add("glyUniqueID|C", gLFiscalYearOpeningBalanceId);
		AddCustomFieldsToSelectList("GLFiscalYearOpeningBalances");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearOpeningBalances", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearOpeningBalanceInformationDto);
			}
			eRPGLFiscalYearOpeningBalanceInformationDto.glyCreatedBy = dataTable.Rows[0].Field<string>("glyCreatedBy");
			eRPGLFiscalYearOpeningBalanceInformationDto.glyCreatedDate = dataTable.Rows[0].Field<DateTime?>("glyCreatedDate");
			eRPGLFiscalYearOpeningBalanceInformationDto.glyUniqueID = dataTable.Rows[0].Field<Guid>("glyUniqueID");
			eRPGLFiscalYearOpeningBalanceInformationDto.glyGlAccountID = dataTable.Rows[0].Field<string>("glyGlAccountID");
			eRPGLFiscalYearOpeningBalanceInformationDto.glyGlFiscalYearID = dataTable.Rows[0].Field<short>("glyGlFiscalYearID");
			eRPGLFiscalYearOpeningBalanceInformationDto.glyRowVersion = dataTable.Rows[0].Field<byte[]>("glyRowVersion");
			eRPGLFiscalYearOpeningBalanceInformationDto.glyYearOpeningBalance = dataTable.Rows[0].Field<decimal>("glyYearOpeningBalance");
			eRPGLFiscalYearOpeningBalanceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearOpeningBalanceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearOpeningBalanceInformationDto);
	}
}
