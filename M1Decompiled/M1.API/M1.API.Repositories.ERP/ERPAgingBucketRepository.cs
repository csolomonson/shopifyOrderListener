using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPAgingBucketRepository : APIBaseRepository, IERPAgingBucketRepository, IAPIBaseRepository, IDisposable
{
	public ERPAgingBucketRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAgingBucketExist(Guid agingBucketId)
	{
		InitializeParameterLists();
		base.filterList.Add("xaaUniqueID|C", agingBucketId);
		base.selectList.Add("xaaUniqueID");
		return Task.FromResult(GetAsObject("AgingBuckets", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAgingBucketInformationDto>> GetAllAgingBuckets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAgingBucketInformationDto> collection = new List<ERPAgingBucketInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"xaaBucket1DaysOver", "xaaBucket1Description", "xaaBucket2DaysOver", "xaaBucket2Description", "xaaBucket3DaysOver", "xaaBucket3Description", "xaaBucket4DaysOver", "xaaBucket4Description", "xaaBucket5DaysOver", "xaaBucket5Description",
			"xaaCalculationType", "xaaAgingBucketID", "xaaCreatedBy", "xaaCreatedDate", "xaaDescription", "xaaUniqueID", "xaaRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AgingBuckets");
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
		using (DataTable dataTable = GetAsDataTable("AgingBuckets", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAgingBucketInformationDto eRPAgingBucketInformationDto = new ERPAgingBucketInformationDto();
				eRPAgingBucketInformationDto.xaaBucket1DaysOver = dataTable.Rows[i].Field<int>("xaaBucket1DaysOver");
				eRPAgingBucketInformationDto.xaaBucket1Description = dataTable.Rows[i].Field<string>("xaaBucket1Description");
				eRPAgingBucketInformationDto.xaaBucket2DaysOver = dataTable.Rows[i].Field<int>("xaaBucket2DaysOver");
				eRPAgingBucketInformationDto.xaaBucket2Description = dataTable.Rows[i].Field<string>("xaaBucket2Description");
				eRPAgingBucketInformationDto.xaaBucket3DaysOver = dataTable.Rows[i].Field<int>("xaaBucket3DaysOver");
				eRPAgingBucketInformationDto.xaaBucket3Description = dataTable.Rows[i].Field<string>("xaaBucket3Description");
				eRPAgingBucketInformationDto.xaaBucket4DaysOver = dataTable.Rows[i].Field<int>("xaaBucket4DaysOver");
				eRPAgingBucketInformationDto.xaaBucket4Description = dataTable.Rows[i].Field<string>("xaaBucket4Description");
				eRPAgingBucketInformationDto.xaaBucket5DaysOver = dataTable.Rows[i].Field<int>("xaaBucket5DaysOver");
				eRPAgingBucketInformationDto.xaaBucket5Description = dataTable.Rows[i].Field<string>("xaaBucket5Description");
				eRPAgingBucketInformationDto.xaaCalculationType = dataTable.Rows[i].Field<byte>("xaaCalculationType");
				eRPAgingBucketInformationDto.xaaAgingBucketID = dataTable.Rows[i].Field<string>("xaaAgingBucketID");
				eRPAgingBucketInformationDto.xaaCreatedBy = dataTable.Rows[i].Field<string>("xaaCreatedBy");
				eRPAgingBucketInformationDto.xaaCreatedDate = dataTable.Rows[i].Field<DateTime?>("xaaCreatedDate");
				eRPAgingBucketInformationDto.xaaDescription = dataTable.Rows[i].Field<string>("xaaDescription");
				eRPAgingBucketInformationDto.xaaUniqueID = dataTable.Rows[i].Field<Guid>("xaaUniqueID");
				eRPAgingBucketInformationDto.xaaRowVersion = dataTable.Rows[i].Field<byte[]>("xaaRowVersion");
				eRPAgingBucketInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAgingBucketInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAgingBucketInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAgingBucketInformationDto> GetAgingBucket(Guid agingBucketId)
	{
		ERPAgingBucketInformationDto eRPAgingBucketInformationDto = new ERPAgingBucketInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"xaaBucket1DaysOver", "xaaBucket1Description", "xaaBucket2DaysOver", "xaaBucket2Description", "xaaBucket3DaysOver", "xaaBucket3Description", "xaaBucket4DaysOver", "xaaBucket4Description", "xaaBucket5DaysOver", "xaaBucket5Description",
			"xaaCalculationType", "xaaAgingBucketID", "xaaCreatedBy", "xaaCreatedDate", "xaaDescription", "xaaUniqueID", "xaaRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xaaUniqueID|C", agingBucketId);
		AddCustomFieldsToSelectList("AgingBuckets");
		using (DataTable dataTable = GetAsDataTable("AgingBuckets", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAgingBucketInformationDto);
			}
			eRPAgingBucketInformationDto.xaaBucket1DaysOver = dataTable.Rows[0].Field<int>("xaaBucket1DaysOver");
			eRPAgingBucketInformationDto.xaaBucket1Description = dataTable.Rows[0].Field<string>("xaaBucket1Description");
			eRPAgingBucketInformationDto.xaaBucket2DaysOver = dataTable.Rows[0].Field<int>("xaaBucket2DaysOver");
			eRPAgingBucketInformationDto.xaaBucket2Description = dataTable.Rows[0].Field<string>("xaaBucket2Description");
			eRPAgingBucketInformationDto.xaaBucket3DaysOver = dataTable.Rows[0].Field<int>("xaaBucket3DaysOver");
			eRPAgingBucketInformationDto.xaaBucket3Description = dataTable.Rows[0].Field<string>("xaaBucket3Description");
			eRPAgingBucketInformationDto.xaaBucket4DaysOver = dataTable.Rows[0].Field<int>("xaaBucket4DaysOver");
			eRPAgingBucketInformationDto.xaaBucket4Description = dataTable.Rows[0].Field<string>("xaaBucket4Description");
			eRPAgingBucketInformationDto.xaaBucket5DaysOver = dataTable.Rows[0].Field<int>("xaaBucket5DaysOver");
			eRPAgingBucketInformationDto.xaaBucket5Description = dataTable.Rows[0].Field<string>("xaaBucket5Description");
			eRPAgingBucketInformationDto.xaaCalculationType = dataTable.Rows[0].Field<byte>("xaaCalculationType");
			eRPAgingBucketInformationDto.xaaAgingBucketID = dataTable.Rows[0].Field<string>("xaaAgingBucketID");
			eRPAgingBucketInformationDto.xaaCreatedBy = dataTable.Rows[0].Field<string>("xaaCreatedBy");
			eRPAgingBucketInformationDto.xaaCreatedDate = dataTable.Rows[0].Field<DateTime?>("xaaCreatedDate");
			eRPAgingBucketInformationDto.xaaDescription = dataTable.Rows[0].Field<string>("xaaDescription");
			eRPAgingBucketInformationDto.xaaUniqueID = dataTable.Rows[0].Field<Guid>("xaaUniqueID");
			eRPAgingBucketInformationDto.xaaRowVersion = dataTable.Rows[0].Field<byte[]>("xaaRowVersion");
			eRPAgingBucketInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAgingBucketInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAgingBucketInformationDto);
	}
}
