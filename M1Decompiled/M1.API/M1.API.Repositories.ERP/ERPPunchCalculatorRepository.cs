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

public class ERPPunchCalculatorRepository : APIBaseRepository, IERPPunchCalculatorRepository, IAPIBaseRepository, IDisposable
{
	public ERPPunchCalculatorRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPunchCalculatorExist(Guid punchCalculatorId)
	{
		InitializeParameterLists();
		base.filterList.Add("ccuUniqueID|C", punchCalculatorId);
		base.selectList.Add("ccuUniqueID");
		return Task.FromResult(GetAsObject("PunchCalculators", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPunchCalculatorInformationDto>> GetAllPunchCalculators(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPunchCalculatorInformationDto> collection = new List<ERPPunchCalculatorInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"ccuPunchCalculatorId", "ccuCreatedBy", "ccuCreatedDate", "ccuUniqueID", "ccuHitRate", "ccuHitsPerPart", "ccuPartsPerHour", "ccuPartsPerSheet", "ccuRepositions", "ccuRepositionTime",
			"ccuRepositionTimeSec", "ccuRowVersion", "ccuSheetLoadTime", "ccuSheetLoadTimeSec", "ccuSheetsPerHour", "ccuTimeToPiece", "ccuToolChangeTimeSec", "ccuToolChangeTimeTotal", "ccuTools", "ccuTotalTimeMinutes",
			"ccuTotalTimeSeconds", "ccuTurns"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PunchCalculators");
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
		using (DataTable dataTable = GetAsDataTable("PunchCalculators", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPunchCalculatorInformationDto eRPPunchCalculatorInformationDto = new ERPPunchCalculatorInformationDto();
				eRPPunchCalculatorInformationDto.ccuPunchCalculatorId = dataTable.Rows[i].Field<Guid>("ccuPunchCalculatorId");
				eRPPunchCalculatorInformationDto.ccuCreatedBy = dataTable.Rows[i].Field<string>("ccuCreatedBy");
				eRPPunchCalculatorInformationDto.ccuCreatedDate = dataTable.Rows[i].Field<DateTime?>("ccuCreatedDate");
				eRPPunchCalculatorInformationDto.ccuUniqueID = dataTable.Rows[i].Field<Guid>("ccuUniqueID");
				eRPPunchCalculatorInformationDto.ccuHitRate = dataTable.Rows[i].Field<int>("ccuHitRate");
				eRPPunchCalculatorInformationDto.ccuHitsPerPart = dataTable.Rows[i].Field<int>("ccuHitsPerPart");
				eRPPunchCalculatorInformationDto.ccuPartsPerHour = dataTable.Rows[i].Field<decimal>("ccuPartsPerHour");
				eRPPunchCalculatorInformationDto.ccuPartsPerSheet = dataTable.Rows[i].Field<int>("ccuPartsPerSheet");
				eRPPunchCalculatorInformationDto.ccuRepositions = dataTable.Rows[i].Field<int>("ccuRepositions");
				eRPPunchCalculatorInformationDto.ccuRepositionTime = dataTable.Rows[i].Field<decimal>("ccuRepositionTime");
				eRPPunchCalculatorInformationDto.ccuRepositionTimeSec = dataTable.Rows[i].Field<int>("ccuRepositionTimeSec");
				eRPPunchCalculatorInformationDto.ccuRowVersion = dataTable.Rows[i].Field<byte[]>("ccuRowVersion");
				eRPPunchCalculatorInformationDto.ccuSheetLoadTime = dataTable.Rows[i].Field<decimal>("ccuSheetLoadTime");
				eRPPunchCalculatorInformationDto.ccuSheetLoadTimeSec = dataTable.Rows[i].Field<int>("ccuSheetLoadTimeSec");
				eRPPunchCalculatorInformationDto.ccuSheetsPerHour = dataTable.Rows[i].Field<decimal>("ccuSheetsPerHour");
				eRPPunchCalculatorInformationDto.ccuTimeToPiece = dataTable.Rows[i].Field<decimal>("ccuTimeToPiece");
				eRPPunchCalculatorInformationDto.ccuToolChangeTimeSec = dataTable.Rows[i].Field<int>("ccuToolChangeTimeSec");
				eRPPunchCalculatorInformationDto.ccuToolChangeTimeTotal = dataTable.Rows[i].Field<int>("ccuToolChangeTimeTotal");
				eRPPunchCalculatorInformationDto.ccuTools = dataTable.Rows[i].Field<int>("ccuTools");
				eRPPunchCalculatorInformationDto.ccuTotalTimeMinutes = dataTable.Rows[i].Field<decimal>("ccuTotalTimeMinutes");
				eRPPunchCalculatorInformationDto.ccuTotalTimeSeconds = dataTable.Rows[i].Field<int>("ccuTotalTimeSeconds");
				eRPPunchCalculatorInformationDto.ccuTurns = dataTable.Rows[i].Field<int>("ccuTurns");
				eRPPunchCalculatorInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPunchCalculatorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPunchCalculatorInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPunchCalculatorInformationDto> GetPunchCalculator(Guid punchCalculatorId)
	{
		ERPPunchCalculatorInformationDto eRPPunchCalculatorInformationDto = new ERPPunchCalculatorInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"ccuPunchCalculatorId", "ccuCreatedBy", "ccuCreatedDate", "ccuUniqueID", "ccuHitRate", "ccuHitsPerPart", "ccuPartsPerHour", "ccuPartsPerSheet", "ccuRepositions", "ccuRepositionTime",
			"ccuRepositionTimeSec", "ccuRowVersion", "ccuSheetLoadTime", "ccuSheetLoadTimeSec", "ccuSheetsPerHour", "ccuTimeToPiece", "ccuToolChangeTimeSec", "ccuToolChangeTimeTotal", "ccuTools", "ccuTotalTimeMinutes",
			"ccuTotalTimeSeconds", "ccuTurns"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ccuUniqueID|C", punchCalculatorId);
		AddCustomFieldsToSelectList("PunchCalculators");
		using (DataTable dataTable = GetAsDataTable("PunchCalculators", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPunchCalculatorInformationDto);
			}
			eRPPunchCalculatorInformationDto.ccuPunchCalculatorId = dataTable.Rows[0].Field<Guid>("ccuPunchCalculatorId");
			eRPPunchCalculatorInformationDto.ccuCreatedBy = dataTable.Rows[0].Field<string>("ccuCreatedBy");
			eRPPunchCalculatorInformationDto.ccuCreatedDate = dataTable.Rows[0].Field<DateTime?>("ccuCreatedDate");
			eRPPunchCalculatorInformationDto.ccuUniqueID = dataTable.Rows[0].Field<Guid>("ccuUniqueID");
			eRPPunchCalculatorInformationDto.ccuHitRate = dataTable.Rows[0].Field<int>("ccuHitRate");
			eRPPunchCalculatorInformationDto.ccuHitsPerPart = dataTable.Rows[0].Field<int>("ccuHitsPerPart");
			eRPPunchCalculatorInformationDto.ccuPartsPerHour = dataTable.Rows[0].Field<decimal>("ccuPartsPerHour");
			eRPPunchCalculatorInformationDto.ccuPartsPerSheet = dataTable.Rows[0].Field<int>("ccuPartsPerSheet");
			eRPPunchCalculatorInformationDto.ccuRepositions = dataTable.Rows[0].Field<int>("ccuRepositions");
			eRPPunchCalculatorInformationDto.ccuRepositionTime = dataTable.Rows[0].Field<decimal>("ccuRepositionTime");
			eRPPunchCalculatorInformationDto.ccuRepositionTimeSec = dataTable.Rows[0].Field<int>("ccuRepositionTimeSec");
			eRPPunchCalculatorInformationDto.ccuRowVersion = dataTable.Rows[0].Field<byte[]>("ccuRowVersion");
			eRPPunchCalculatorInformationDto.ccuSheetLoadTime = dataTable.Rows[0].Field<decimal>("ccuSheetLoadTime");
			eRPPunchCalculatorInformationDto.ccuSheetLoadTimeSec = dataTable.Rows[0].Field<int>("ccuSheetLoadTimeSec");
			eRPPunchCalculatorInformationDto.ccuSheetsPerHour = dataTable.Rows[0].Field<decimal>("ccuSheetsPerHour");
			eRPPunchCalculatorInformationDto.ccuTimeToPiece = dataTable.Rows[0].Field<decimal>("ccuTimeToPiece");
			eRPPunchCalculatorInformationDto.ccuToolChangeTimeSec = dataTable.Rows[0].Field<int>("ccuToolChangeTimeSec");
			eRPPunchCalculatorInformationDto.ccuToolChangeTimeTotal = dataTable.Rows[0].Field<int>("ccuToolChangeTimeTotal");
			eRPPunchCalculatorInformationDto.ccuTools = dataTable.Rows[0].Field<int>("ccuTools");
			eRPPunchCalculatorInformationDto.ccuTotalTimeMinutes = dataTable.Rows[0].Field<decimal>("ccuTotalTimeMinutes");
			eRPPunchCalculatorInformationDto.ccuTotalTimeSeconds = dataTable.Rows[0].Field<int>("ccuTotalTimeSeconds");
			eRPPunchCalculatorInformationDto.ccuTurns = dataTable.Rows[0].Field<int>("ccuTurns");
			eRPPunchCalculatorInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPunchCalculatorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPunchCalculatorInformationDto);
	}

	public Task<APIValidationInfoDto> SavePunchCalculator(ERPPunchCalculatorDto punchCalculator)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PunchCalculators WHERE ccuUniqueID = " + M1Util.ConvertToLinq(punchCalculator.ccuUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ccuPunchCalculatorId"] = punchCalculator.ccuPunchCalculatorId;
				punchCalculator.ccuUniqueID = ((punchCalculator.ccuUniqueID == Guid.Empty) ? Guid.NewGuid() : punchCalculator.ccuUniqueID);
				dataRow["ccuUniqueID"] = punchCalculator.ccuUniqueID;
				dataRow["ccuCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ccuCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PunchCalculator could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (punchCalculator.ccuRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PunchCalculator is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ccuRowVersion"], punchCalculator.ccuRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PunchCalculator has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PunchCalculator again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ccuHitRate"] = punchCalculator.ccuHitRate;
			dataRow["ccuHitsPerPart"] = punchCalculator.ccuHitsPerPart;
			dataRow["ccuPartsPerHour"] = punchCalculator.ccuPartsPerHour;
			dataRow["ccuPartsPerSheet"] = punchCalculator.ccuPartsPerSheet;
			dataRow["ccuRepositions"] = punchCalculator.ccuRepositions;
			dataRow["ccuRepositionTime"] = punchCalculator.ccuRepositionTime;
			dataRow["ccuRepositionTimeSec"] = punchCalculator.ccuRepositionTimeSec;
			dataRow["ccuSheetLoadTime"] = punchCalculator.ccuSheetLoadTime;
			dataRow["ccuSheetLoadTimeSec"] = punchCalculator.ccuSheetLoadTimeSec;
			dataRow["ccuSheetsPerHour"] = punchCalculator.ccuSheetsPerHour;
			dataRow["ccuTimeToPiece"] = punchCalculator.ccuTimeToPiece;
			dataRow["ccuToolChangeTimeSec"] = punchCalculator.ccuToolChangeTimeSec;
			dataRow["ccuToolChangeTimeTotal"] = punchCalculator.ccuToolChangeTimeTotal;
			dataRow["ccuTools"] = punchCalculator.ccuTools;
			dataRow["ccuTotalTimeMinutes"] = punchCalculator.ccuTotalTimeMinutes;
			dataRow["ccuTotalTimeSeconds"] = punchCalculator.ccuTotalTimeSeconds;
			dataRow["ccuTurns"] = punchCalculator.ccuTurns;
			if (punchCalculator.CustomFields != null && punchCalculator.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in punchCalculator.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PunchCalculator [{punchCalculator.ccuUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PunchCalculator [{punchCalculator.ccuUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
