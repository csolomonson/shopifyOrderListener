using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPLaserCalculatorLineRepository : APIBaseRepository, IERPLaserCalculatorLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPLaserCalculatorLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLaserCalculatorLineExist(Guid laserCalculatorLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("cclUniqueID|C", laserCalculatorLineId);
		base.selectList.Add("cclUniqueID");
		return Task.FromResult(GetAsObject("LaserCalculatorLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLaserCalculatorLineInformationDto>> GetAllLaserCalculatorLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLaserCalculatorLineInformationDto> collection = new List<ERPLaserCalculatorLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"cclCreatedBy", "cclCreatedDate", "cclCutTime", "cclDescription", "cclUniqueID", "cclLaserCalculatorID", "ccllength", "cclQuantity", "cclRate", "cclRowVersion",
			"cclLaserCalculatorLineID", "cclWidth"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LaserCalculatorLines");
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
		using (DataTable dataTable = GetAsDataTable("LaserCalculatorLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLaserCalculatorLineInformationDto eRPLaserCalculatorLineInformationDto = new ERPLaserCalculatorLineInformationDto();
				eRPLaserCalculatorLineInformationDto.cclCreatedBy = dataTable.Rows[i].Field<string>("cclCreatedBy");
				eRPLaserCalculatorLineInformationDto.cclCreatedDate = dataTable.Rows[i].Field<DateTime?>("cclCreatedDate");
				eRPLaserCalculatorLineInformationDto.cclCutTime = dataTable.Rows[i].Field<decimal>("cclCutTime");
				eRPLaserCalculatorLineInformationDto.cclDescription = dataTable.Rows[i].Field<string>("cclDescription");
				eRPLaserCalculatorLineInformationDto.cclUniqueID = dataTable.Rows[i].Field<Guid>("cclUniqueID");
				eRPLaserCalculatorLineInformationDto.cclLaserCalculatorID = dataTable.Rows[i].Field<Guid>("cclLaserCalculatorID");
				eRPLaserCalculatorLineInformationDto.ccllength = dataTable.Rows[i].Field<decimal>("ccllength");
				eRPLaserCalculatorLineInformationDto.cclQuantity = dataTable.Rows[i].Field<decimal>("cclQuantity");
				eRPLaserCalculatorLineInformationDto.cclRate = dataTable.Rows[i].Field<decimal>("cclRate");
				eRPLaserCalculatorLineInformationDto.cclRowVersion = dataTable.Rows[i].Field<byte[]>("cclRowVersion");
				eRPLaserCalculatorLineInformationDto.cclLaserCalculatorLineID = dataTable.Rows[i].Field<int>("cclLaserCalculatorLineID");
				eRPLaserCalculatorLineInformationDto.cclWidth = dataTable.Rows[i].Field<decimal>("cclWidth");
				eRPLaserCalculatorLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLaserCalculatorLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLaserCalculatorLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLaserCalculatorLineInformationDto> GetLaserCalculatorLine(Guid laserCalculatorLineId)
	{
		ERPLaserCalculatorLineInformationDto eRPLaserCalculatorLineInformationDto = new ERPLaserCalculatorLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"cclCreatedBy", "cclCreatedDate", "cclCutTime", "cclDescription", "cclUniqueID", "cclLaserCalculatorID", "ccllength", "cclQuantity", "cclRate", "cclRowVersion",
			"cclLaserCalculatorLineID", "cclWidth"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cclUniqueID|C", laserCalculatorLineId);
		AddCustomFieldsToSelectList("LaserCalculatorLines");
		using (DataTable dataTable = GetAsDataTable("LaserCalculatorLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLaserCalculatorLineInformationDto);
			}
			eRPLaserCalculatorLineInformationDto.cclCreatedBy = dataTable.Rows[0].Field<string>("cclCreatedBy");
			eRPLaserCalculatorLineInformationDto.cclCreatedDate = dataTable.Rows[0].Field<DateTime?>("cclCreatedDate");
			eRPLaserCalculatorLineInformationDto.cclCutTime = dataTable.Rows[0].Field<decimal>("cclCutTime");
			eRPLaserCalculatorLineInformationDto.cclDescription = dataTable.Rows[0].Field<string>("cclDescription");
			eRPLaserCalculatorLineInformationDto.cclUniqueID = dataTable.Rows[0].Field<Guid>("cclUniqueID");
			eRPLaserCalculatorLineInformationDto.cclLaserCalculatorID = dataTable.Rows[0].Field<Guid>("cclLaserCalculatorID");
			eRPLaserCalculatorLineInformationDto.ccllength = dataTable.Rows[0].Field<decimal>("ccllength");
			eRPLaserCalculatorLineInformationDto.cclQuantity = dataTable.Rows[0].Field<decimal>("cclQuantity");
			eRPLaserCalculatorLineInformationDto.cclRate = dataTable.Rows[0].Field<decimal>("cclRate");
			eRPLaserCalculatorLineInformationDto.cclRowVersion = dataTable.Rows[0].Field<byte[]>("cclRowVersion");
			eRPLaserCalculatorLineInformationDto.cclLaserCalculatorLineID = dataTable.Rows[0].Field<int>("cclLaserCalculatorLineID");
			eRPLaserCalculatorLineInformationDto.cclWidth = dataTable.Rows[0].Field<decimal>("cclWidth");
			eRPLaserCalculatorLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLaserCalculatorLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLaserCalculatorLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLaserCalculatorLine(ERPLaserCalculatorLineDto laserCalculatorLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LaserCalculatorLines WHERE cclUniqueID = " + M1Util.ConvertToLinq(laserCalculatorLine.cclUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cclLaserCalculatorID"] = laserCalculatorLine.cclLaserCalculatorID;
				dataRow["cclLaserCalculatorLineID"] = laserCalculatorLine.cclLaserCalculatorLineID;
				laserCalculatorLine.cclUniqueID = ((laserCalculatorLine.cclUniqueID == Guid.Empty) ? Guid.NewGuid() : laserCalculatorLine.cclUniqueID);
				dataRow["cclUniqueID"] = laserCalculatorLine.cclUniqueID;
				dataRow["cclCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cclCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LaserCalculatorLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (laserCalculatorLine.cclRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LaserCalculatorLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cclRowVersion"], laserCalculatorLine.cclRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LaserCalculatorLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LaserCalculatorLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cclCutTime"] = laserCalculatorLine.cclCutTime;
			dataRow["cclDescription"] = laserCalculatorLine.cclDescription;
			dataRow["ccllength"] = laserCalculatorLine.ccllength;
			dataRow["cclQuantity"] = laserCalculatorLine.cclQuantity;
			dataRow["cclRate"] = laserCalculatorLine.cclRate;
			dataRow["cclWidth"] = laserCalculatorLine.cclWidth;
			if (laserCalculatorLine.CustomFields != null && laserCalculatorLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in laserCalculatorLine.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LaserCalculatorLine [{laserCalculatorLine.cclUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LaserCalculatorLine [{laserCalculatorLine.cclUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
