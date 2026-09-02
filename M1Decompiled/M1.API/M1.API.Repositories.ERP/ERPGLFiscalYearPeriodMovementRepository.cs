using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPGLFiscalYearPeriodMovementRepository : APIBaseRepository, IERPGLFiscalYearPeriodMovementRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearPeriodMovementRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearPeriodMovementExist(Guid gLFiscalYearPeriodMovementId)
	{
		InitializeParameterLists();
		base.filterList.Add("gliUniqueID|C", gLFiscalYearPeriodMovementId);
		base.selectList.Add("gliUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYearPeriodMovements", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearPeriodMovementInformationDto>> GetAllGLFiscalYearPeriodMovements(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearPeriodMovementInformationDto> collection = new List<ERPGLFiscalYearPeriodMovementInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "gliCreatedBy", "gliCreatedDate", "gliUniqueID", "gliGlAccountID", "gliGlFiscalYearID", "gliGlFiscalYearPeriodID", "gliRowVersion", "gliTotalCredits", "gliTotalDebits" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYearPeriodMovements");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearPeriodMovements", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearPeriodMovementInformationDto eRPGLFiscalYearPeriodMovementInformationDto = new ERPGLFiscalYearPeriodMovementInformationDto();
				eRPGLFiscalYearPeriodMovementInformationDto.gliCreatedBy = dataTable.Rows[i].Field<string>("gliCreatedBy");
				eRPGLFiscalYearPeriodMovementInformationDto.gliCreatedDate = dataTable.Rows[i].Field<DateTime?>("gliCreatedDate");
				eRPGLFiscalYearPeriodMovementInformationDto.gliUniqueID = dataTable.Rows[i].Field<Guid>("gliUniqueID");
				eRPGLFiscalYearPeriodMovementInformationDto.gliGlAccountID = dataTable.Rows[i].Field<string>("gliGlAccountID");
				eRPGLFiscalYearPeriodMovementInformationDto.gliGlFiscalYearID = dataTable.Rows[i].Field<short>("gliGlFiscalYearID");
				eRPGLFiscalYearPeriodMovementInformationDto.gliGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("gliGlFiscalYearPeriodID");
				eRPGLFiscalYearPeriodMovementInformationDto.gliRowVersion = dataTable.Rows[i].Field<byte[]>("gliRowVersion");
				eRPGLFiscalYearPeriodMovementInformationDto.gliTotalCredits = dataTable.Rows[i].Field<decimal>("gliTotalCredits");
				eRPGLFiscalYearPeriodMovementInformationDto.gliTotalDebits = dataTable.Rows[i].Field<decimal>("gliTotalDebits");
				eRPGLFiscalYearPeriodMovementInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearPeriodMovementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearPeriodMovementInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearPeriodMovementInformationDto> GetGLFiscalYearPeriodMovement(Guid gLFiscalYearPeriodMovementId)
	{
		ERPGLFiscalYearPeriodMovementInformationDto eRPGLFiscalYearPeriodMovementInformationDto = new ERPGLFiscalYearPeriodMovementInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "gliCreatedBy", "gliCreatedDate", "gliUniqueID", "gliGlAccountID", "gliGlFiscalYearID", "gliGlFiscalYearPeriodID", "gliRowVersion", "gliTotalCredits", "gliTotalDebits" };
		base.selectList.AddRange(collection);
		base.filterList.Add("gliUniqueID|C", gLFiscalYearPeriodMovementId);
		AddCustomFieldsToSelectList("GLFiscalYearPeriodMovements");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearPeriodMovements", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearPeriodMovementInformationDto);
			}
			eRPGLFiscalYearPeriodMovementInformationDto.gliCreatedBy = dataTable.Rows[0].Field<string>("gliCreatedBy");
			eRPGLFiscalYearPeriodMovementInformationDto.gliCreatedDate = dataTable.Rows[0].Field<DateTime?>("gliCreatedDate");
			eRPGLFiscalYearPeriodMovementInformationDto.gliUniqueID = dataTable.Rows[0].Field<Guid>("gliUniqueID");
			eRPGLFiscalYearPeriodMovementInformationDto.gliGlAccountID = dataTable.Rows[0].Field<string>("gliGlAccountID");
			eRPGLFiscalYearPeriodMovementInformationDto.gliGlFiscalYearID = dataTable.Rows[0].Field<short>("gliGlFiscalYearID");
			eRPGLFiscalYearPeriodMovementInformationDto.gliGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("gliGlFiscalYearPeriodID");
			eRPGLFiscalYearPeriodMovementInformationDto.gliRowVersion = dataTable.Rows[0].Field<byte[]>("gliRowVersion");
			eRPGLFiscalYearPeriodMovementInformationDto.gliTotalCredits = dataTable.Rows[0].Field<decimal>("gliTotalCredits");
			eRPGLFiscalYearPeriodMovementInformationDto.gliTotalDebits = dataTable.Rows[0].Field<decimal>("gliTotalDebits");
			eRPGLFiscalYearPeriodMovementInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearPeriodMovementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearPeriodMovementInformationDto);
	}
}
