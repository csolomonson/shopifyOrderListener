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

public class ERPLaserCalculatorRepository : APIBaseRepository, IERPLaserCalculatorRepository, IAPIBaseRepository, IDisposable
{
	public ERPLaserCalculatorRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLaserCalculatorExist(Guid laserCalculatorId)
	{
		InitializeParameterLists();
		base.filterList.Add("ccpUniqueID|C", laserCalculatorId);
		base.selectList.Add("ccpUniqueID");
		return Task.FromResult(GetAsObject("LaserCalculators", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLaserCalculatorInformationDto>> GetAllLaserCalculators(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLaserCalculatorInformationDto> collection = new List<ERPLaserCalculatorInformationDto>();
		InitializeParameterLists();
		string[] array = new string[31]
		{
			"ccpLaserCalculatorID", "ccpCreatedBy", "ccpCreatedDate", "ccpdescription", "ccpUniqueID", "ccpExternalFeed", "ccpHoleCutTime", "ccpObround", "ccpOther", "ccpRectangle",
			"ccpRound", "ccpSquare", "ccpLaserMaterialTypeID", "ccpLeadInOut", "ccpLeadInOutFeed", "ccpLeadInOutTime", "ccplength", "ccpMeasurementType", "ccpNumberOfHoles", "ccpPartPerimeter",
			"ccpPerimeterCutTime", "ccpPiercedHoles", "ccpPierceTime", "ccpQuantity", "ccpRate", "ccpRowVersion", "ccpThickness", "ccpTotalCutTime", "ccpTotalLeadInOutTime", "ccpTotalPierceTime",
			"ccpWidth"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LaserCalculators");
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
		using (DataTable dataTable = GetAsDataTable("LaserCalculators", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLaserCalculatorInformationDto eRPLaserCalculatorInformationDto = new ERPLaserCalculatorInformationDto();
				eRPLaserCalculatorInformationDto.ccpLaserCalculatorID = dataTable.Rows[i].Field<Guid>("ccpLaserCalculatorID");
				eRPLaserCalculatorInformationDto.ccpCreatedBy = dataTable.Rows[i].Field<string>("ccpCreatedBy");
				eRPLaserCalculatorInformationDto.ccpCreatedDate = dataTable.Rows[i].Field<DateTime?>("ccpCreatedDate");
				eRPLaserCalculatorInformationDto.ccpdescription = dataTable.Rows[i].Field<string>("ccpdescription");
				eRPLaserCalculatorInformationDto.ccpUniqueID = dataTable.Rows[i].Field<Guid>("ccpUniqueID");
				eRPLaserCalculatorInformationDto.ccpExternalFeed = dataTable.Rows[i].Field<decimal>("ccpExternalFeed");
				eRPLaserCalculatorInformationDto.ccpHoleCutTime = dataTable.Rows[i].Field<decimal>("ccpHoleCutTime");
				eRPLaserCalculatorInformationDto.ccpObround = dataTable.Rows[i].Field<bool>("ccpObround");
				eRPLaserCalculatorInformationDto.ccpOther = dataTable.Rows[i].Field<bool>("ccpOther");
				eRPLaserCalculatorInformationDto.ccpRectangle = dataTable.Rows[i].Field<bool>("ccpRectangle");
				eRPLaserCalculatorInformationDto.ccpRound = dataTable.Rows[i].Field<bool>("ccpRound");
				eRPLaserCalculatorInformationDto.ccpSquare = dataTable.Rows[i].Field<bool>("ccpSquare");
				eRPLaserCalculatorInformationDto.ccpLaserMaterialTypeID = dataTable.Rows[i].Field<string>("ccpLaserMaterialTypeID");
				eRPLaserCalculatorInformationDto.ccpLeadInOut = dataTable.Rows[i].Field<decimal>("ccpLeadInOut");
				eRPLaserCalculatorInformationDto.ccpLeadInOutFeed = dataTable.Rows[i].Field<decimal>("ccpLeadInOutFeed");
				eRPLaserCalculatorInformationDto.ccpLeadInOutTime = dataTable.Rows[i].Field<decimal>("ccpLeadInOutTime");
				eRPLaserCalculatorInformationDto.ccplength = dataTable.Rows[i].Field<decimal>("ccplength");
				eRPLaserCalculatorInformationDto.ccpMeasurementType = dataTable.Rows[i].Field<string>("ccpMeasurementType");
				eRPLaserCalculatorInformationDto.ccpNumberOfHoles = dataTable.Rows[i].Field<int>("ccpNumberOfHoles");
				eRPLaserCalculatorInformationDto.ccpPartPerimeter = dataTable.Rows[i].Field<decimal>("ccpPartPerimeter");
				eRPLaserCalculatorInformationDto.ccpPerimeterCutTime = dataTable.Rows[i].Field<decimal>("ccpPerimeterCutTime");
				eRPLaserCalculatorInformationDto.ccpPiercedHoles = dataTable.Rows[i].Field<decimal>("ccpPiercedHoles");
				eRPLaserCalculatorInformationDto.ccpPierceTime = dataTable.Rows[i].Field<decimal>("ccpPierceTime");
				eRPLaserCalculatorInformationDto.ccpQuantity = dataTable.Rows[i].Field<decimal>("ccpQuantity");
				eRPLaserCalculatorInformationDto.ccpRate = dataTable.Rows[i].Field<decimal>("ccpRate");
				eRPLaserCalculatorInformationDto.ccpRowVersion = dataTable.Rows[i].Field<byte[]>("ccpRowVersion");
				eRPLaserCalculatorInformationDto.ccpThickness = dataTable.Rows[i].Field<decimal>("ccpThickness");
				eRPLaserCalculatorInformationDto.ccpTotalCutTime = dataTable.Rows[i].Field<decimal>("ccpTotalCutTime");
				eRPLaserCalculatorInformationDto.ccpTotalLeadInOutTime = dataTable.Rows[i].Field<decimal>("ccpTotalLeadInOutTime");
				eRPLaserCalculatorInformationDto.ccpTotalPierceTime = dataTable.Rows[i].Field<decimal>("ccpTotalPierceTime");
				eRPLaserCalculatorInformationDto.ccpWidth = dataTable.Rows[i].Field<decimal>("ccpWidth");
				eRPLaserCalculatorInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLaserCalculatorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLaserCalculatorInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLaserCalculatorInformationDto> GetLaserCalculator(Guid laserCalculatorId)
	{
		ERPLaserCalculatorInformationDto eRPLaserCalculatorInformationDto = new ERPLaserCalculatorInformationDto();
		InitializeParameterLists();
		string[] collection = new string[31]
		{
			"ccpLaserCalculatorID", "ccpCreatedBy", "ccpCreatedDate", "ccpdescription", "ccpUniqueID", "ccpExternalFeed", "ccpHoleCutTime", "ccpObround", "ccpOther", "ccpRectangle",
			"ccpRound", "ccpSquare", "ccpLaserMaterialTypeID", "ccpLeadInOut", "ccpLeadInOutFeed", "ccpLeadInOutTime", "ccplength", "ccpMeasurementType", "ccpNumberOfHoles", "ccpPartPerimeter",
			"ccpPerimeterCutTime", "ccpPiercedHoles", "ccpPierceTime", "ccpQuantity", "ccpRate", "ccpRowVersion", "ccpThickness", "ccpTotalCutTime", "ccpTotalLeadInOutTime", "ccpTotalPierceTime",
			"ccpWidth"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ccpUniqueID|C", laserCalculatorId);
		AddCustomFieldsToSelectList("LaserCalculators");
		using (DataTable dataTable = GetAsDataTable("LaserCalculators", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLaserCalculatorInformationDto);
			}
			eRPLaserCalculatorInformationDto.ccpLaserCalculatorID = dataTable.Rows[0].Field<Guid>("ccpLaserCalculatorID");
			eRPLaserCalculatorInformationDto.ccpCreatedBy = dataTable.Rows[0].Field<string>("ccpCreatedBy");
			eRPLaserCalculatorInformationDto.ccpCreatedDate = dataTable.Rows[0].Field<DateTime?>("ccpCreatedDate");
			eRPLaserCalculatorInformationDto.ccpdescription = dataTable.Rows[0].Field<string>("ccpdescription");
			eRPLaserCalculatorInformationDto.ccpUniqueID = dataTable.Rows[0].Field<Guid>("ccpUniqueID");
			eRPLaserCalculatorInformationDto.ccpExternalFeed = dataTable.Rows[0].Field<decimal>("ccpExternalFeed");
			eRPLaserCalculatorInformationDto.ccpHoleCutTime = dataTable.Rows[0].Field<decimal>("ccpHoleCutTime");
			eRPLaserCalculatorInformationDto.ccpObround = dataTable.Rows[0].Field<bool>("ccpObround");
			eRPLaserCalculatorInformationDto.ccpOther = dataTable.Rows[0].Field<bool>("ccpOther");
			eRPLaserCalculatorInformationDto.ccpRectangle = dataTable.Rows[0].Field<bool>("ccpRectangle");
			eRPLaserCalculatorInformationDto.ccpRound = dataTable.Rows[0].Field<bool>("ccpRound");
			eRPLaserCalculatorInformationDto.ccpSquare = dataTable.Rows[0].Field<bool>("ccpSquare");
			eRPLaserCalculatorInformationDto.ccpLaserMaterialTypeID = dataTable.Rows[0].Field<string>("ccpLaserMaterialTypeID");
			eRPLaserCalculatorInformationDto.ccpLeadInOut = dataTable.Rows[0].Field<decimal>("ccpLeadInOut");
			eRPLaserCalculatorInformationDto.ccpLeadInOutFeed = dataTable.Rows[0].Field<decimal>("ccpLeadInOutFeed");
			eRPLaserCalculatorInformationDto.ccpLeadInOutTime = dataTable.Rows[0].Field<decimal>("ccpLeadInOutTime");
			eRPLaserCalculatorInformationDto.ccplength = dataTable.Rows[0].Field<decimal>("ccplength");
			eRPLaserCalculatorInformationDto.ccpMeasurementType = dataTable.Rows[0].Field<string>("ccpMeasurementType");
			eRPLaserCalculatorInformationDto.ccpNumberOfHoles = dataTable.Rows[0].Field<int>("ccpNumberOfHoles");
			eRPLaserCalculatorInformationDto.ccpPartPerimeter = dataTable.Rows[0].Field<decimal>("ccpPartPerimeter");
			eRPLaserCalculatorInformationDto.ccpPerimeterCutTime = dataTable.Rows[0].Field<decimal>("ccpPerimeterCutTime");
			eRPLaserCalculatorInformationDto.ccpPiercedHoles = dataTable.Rows[0].Field<decimal>("ccpPiercedHoles");
			eRPLaserCalculatorInformationDto.ccpPierceTime = dataTable.Rows[0].Field<decimal>("ccpPierceTime");
			eRPLaserCalculatorInformationDto.ccpQuantity = dataTable.Rows[0].Field<decimal>("ccpQuantity");
			eRPLaserCalculatorInformationDto.ccpRate = dataTable.Rows[0].Field<decimal>("ccpRate");
			eRPLaserCalculatorInformationDto.ccpRowVersion = dataTable.Rows[0].Field<byte[]>("ccpRowVersion");
			eRPLaserCalculatorInformationDto.ccpThickness = dataTable.Rows[0].Field<decimal>("ccpThickness");
			eRPLaserCalculatorInformationDto.ccpTotalCutTime = dataTable.Rows[0].Field<decimal>("ccpTotalCutTime");
			eRPLaserCalculatorInformationDto.ccpTotalLeadInOutTime = dataTable.Rows[0].Field<decimal>("ccpTotalLeadInOutTime");
			eRPLaserCalculatorInformationDto.ccpTotalPierceTime = dataTable.Rows[0].Field<decimal>("ccpTotalPierceTime");
			eRPLaserCalculatorInformationDto.ccpWidth = dataTable.Rows[0].Field<decimal>("ccpWidth");
			eRPLaserCalculatorInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLaserCalculatorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLaserCalculatorInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLaserCalculator(ERPLaserCalculatorDto laserCalculator)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LaserCalculators WHERE ccpUniqueID = " + M1Util.ConvertToLinq(laserCalculator.ccpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ccpLaserCalculatorID"] = laserCalculator.ccpLaserCalculatorID;
				laserCalculator.ccpUniqueID = ((laserCalculator.ccpUniqueID == Guid.Empty) ? Guid.NewGuid() : laserCalculator.ccpUniqueID);
				dataRow["ccpUniqueID"] = laserCalculator.ccpUniqueID;
				dataRow["ccpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ccpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LaserCalculator could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (laserCalculator.ccpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LaserCalculator is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ccpRowVersion"], laserCalculator.ccpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LaserCalculator has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LaserCalculator again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ccpdescription"] = laserCalculator.ccpdescription;
			dataRow["ccpExternalFeed"] = laserCalculator.ccpExternalFeed;
			dataRow["ccpHoleCutTime"] = laserCalculator.ccpHoleCutTime;
			dataRow["ccpObround"] = laserCalculator.ccpObround;
			dataRow["ccpOther"] = laserCalculator.ccpOther;
			dataRow["ccpRectangle"] = laserCalculator.ccpRectangle;
			dataRow["ccpRound"] = laserCalculator.ccpRound;
			dataRow["ccpSquare"] = laserCalculator.ccpSquare;
			dataRow["ccpLaserMaterialTypeID"] = laserCalculator.ccpLaserMaterialTypeID;
			dataRow["ccpLeadInOut"] = laserCalculator.ccpLeadInOut;
			dataRow["ccpLeadInOutFeed"] = laserCalculator.ccpLeadInOutFeed;
			dataRow["ccpLeadInOutTime"] = laserCalculator.ccpLeadInOutTime;
			dataRow["ccplength"] = laserCalculator.ccplength;
			dataRow["ccpMeasurementType"] = laserCalculator.ccpMeasurementType;
			dataRow["ccpNumberOfHoles"] = laserCalculator.ccpNumberOfHoles;
			dataRow["ccpPartPerimeter"] = laserCalculator.ccpPartPerimeter;
			dataRow["ccpPerimeterCutTime"] = laserCalculator.ccpPerimeterCutTime;
			dataRow["ccpPiercedHoles"] = laserCalculator.ccpPiercedHoles;
			dataRow["ccpPierceTime"] = laserCalculator.ccpPierceTime;
			dataRow["ccpQuantity"] = laserCalculator.ccpQuantity;
			dataRow["ccpRate"] = laserCalculator.ccpRate;
			dataRow["ccpThickness"] = laserCalculator.ccpThickness;
			dataRow["ccpTotalCutTime"] = laserCalculator.ccpTotalCutTime;
			dataRow["ccpTotalLeadInOutTime"] = laserCalculator.ccpTotalLeadInOutTime;
			dataRow["ccpTotalPierceTime"] = laserCalculator.ccpTotalPierceTime;
			dataRow["ccpWidth"] = laserCalculator.ccpWidth;
			if (laserCalculator.CustomFields != null && laserCalculator.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in laserCalculator.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LaserCalculator [{laserCalculator.ccpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LaserCalculator [{laserCalculator.ccpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
