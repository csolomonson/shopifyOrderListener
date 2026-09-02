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

public class ERPRMAReceiptLineRepository : APIBaseRepository, IERPRMAReceiptLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAReceiptLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAReceiptLineExist(Guid rMAReceiptLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("rrlUniqueID|C", rMAReceiptLineId);
		base.selectList.Add("rrlUniqueID");
		return Task.FromResult(GetAsObject("RMAReceiptLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAReceiptLineInformationDto>> GetAllRMAReceiptLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAReceiptLineInformationDto> collection = new List<ERPRMAReceiptLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[45]
		{
			"rrlConversionFactor", "rrlCreatedBy", "rrlCreatedDate", "rrlDescription", "rrlUniqueID", "rrlExtendedCost", "rrlExtendedCostForeign", "rrlHeatLot", "rrlInventoryQuantityReceived", "rrlInventoryUnitOfMeasure",
			"rrlClosed", "rrlInInspection", "rrlInspectionComplete", "rrlInvoicedComplete", "rrlKitPart", "rrlPosted", "rrlReceivedComplete", "rrlRequiresInspection", "rrlReversed", "rrlOrgPartID",
			"rrlOrgPartShortDescription", "rrlPartBinID", "rrlPartID", "rrlPartLongDescriptionRtf", "rrlPartLongDescriptionText", "rrlPartRevisionID", "rrlPartWarehouseLocationID", "rrlProjectAreaID", "rrlProjectID", "rrlQuantityToInspect",
			"rrlReference", "rrlReverseRmaReceiptID", "rrlReverseRmaReceiptLineID", "rrlRmaClaimID", "rrlRmaClaimLineID", "rrlRmaClaimQuantity", "rrlRmaOpenQuantity", "rrlRmaReceiptID", "rrlRowVersion", "rrlSalesQuantityReceived",
			"rrlSalesUnitOfMeasure", "rrlRmaReceiptLineID", "rrlTotalComponentCosts", "rrlUnitCost", "rrlUnitCostForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAReceiptLines");
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
		using (DataTable dataTable = GetAsDataTable("RMAReceiptLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAReceiptLineInformationDto eRPRMAReceiptLineInformationDto = new ERPRMAReceiptLineInformationDto();
				eRPRMAReceiptLineInformationDto.rrlConversionFactor = dataTable.Rows[i].Field<decimal>("rrlConversionFactor");
				eRPRMAReceiptLineInformationDto.rrlCreatedBy = dataTable.Rows[i].Field<string>("rrlCreatedBy");
				eRPRMAReceiptLineInformationDto.rrlCreatedDate = dataTable.Rows[i].Field<DateTime?>("rrlCreatedDate");
				eRPRMAReceiptLineInformationDto.rrlDescription = dataTable.Rows[i].Field<string>("rrlDescription");
				eRPRMAReceiptLineInformationDto.rrlUniqueID = dataTable.Rows[i].Field<Guid>("rrlUniqueID");
				eRPRMAReceiptLineInformationDto.rrlExtendedCost = dataTable.Rows[i].Field<decimal>("rrlExtendedCost");
				eRPRMAReceiptLineInformationDto.rrlExtendedCostForeign = dataTable.Rows[i].Field<decimal>("rrlExtendedCostForeign");
				eRPRMAReceiptLineInformationDto.rrlHeatLot = dataTable.Rows[i].Field<string>("rrlHeatLot");
				eRPRMAReceiptLineInformationDto.rrlInventoryQuantityReceived = dataTable.Rows[i].Field<decimal>("rrlInventoryQuantityReceived");
				eRPRMAReceiptLineInformationDto.rrlInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("rrlInventoryUnitOfMeasure");
				eRPRMAReceiptLineInformationDto.rrlClosed = dataTable.Rows[i].Field<bool>("rrlClosed");
				eRPRMAReceiptLineInformationDto.rrlInInspection = dataTable.Rows[i].Field<bool>("rrlInInspection");
				eRPRMAReceiptLineInformationDto.rrlInspectionComplete = dataTable.Rows[i].Field<bool>("rrlInspectionComplete");
				eRPRMAReceiptLineInformationDto.rrlInvoicedComplete = dataTable.Rows[i].Field<bool>("rrlInvoicedComplete");
				eRPRMAReceiptLineInformationDto.rrlKitPart = dataTable.Rows[i].Field<bool>("rrlKitPart");
				eRPRMAReceiptLineInformationDto.rrlPosted = dataTable.Rows[i].Field<bool>("rrlPosted");
				eRPRMAReceiptLineInformationDto.rrlReceivedComplete = dataTable.Rows[i].Field<bool>("rrlReceivedComplete");
				eRPRMAReceiptLineInformationDto.rrlRequiresInspection = dataTable.Rows[i].Field<bool>("rrlRequiresInspection");
				eRPRMAReceiptLineInformationDto.rrlReversed = dataTable.Rows[i].Field<bool>("rrlReversed");
				eRPRMAReceiptLineInformationDto.rrlOrgPartID = dataTable.Rows[i].Field<string>("rrlOrgPartID");
				eRPRMAReceiptLineInformationDto.rrlOrgPartShortDescription = dataTable.Rows[i].Field<string>("rrlOrgPartShortDescription");
				eRPRMAReceiptLineInformationDto.rrlPartBinID = dataTable.Rows[i].Field<string>("rrlPartBinID");
				eRPRMAReceiptLineInformationDto.rrlPartID = dataTable.Rows[i].Field<string>("rrlPartID");
				eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("rrlPartLongDescriptionRtf");
				eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionText = dataTable.Rows[i].Field<string>("rrlPartLongDescriptionText");
				eRPRMAReceiptLineInformationDto.rrlPartRevisionID = dataTable.Rows[i].Field<string>("rrlPartRevisionID");
				eRPRMAReceiptLineInformationDto.rrlPartWarehouseLocationID = dataTable.Rows[i].Field<string>("rrlPartWarehouseLocationID");
				eRPRMAReceiptLineInformationDto.rrlProjectAreaID = dataTable.Rows[i].Field<string>("rrlProjectAreaID");
				eRPRMAReceiptLineInformationDto.rrlProjectID = dataTable.Rows[i].Field<string>("rrlProjectID");
				eRPRMAReceiptLineInformationDto.rrlQuantityToInspect = dataTable.Rows[i].Field<decimal>("rrlQuantityToInspect");
				eRPRMAReceiptLineInformationDto.rrlReference = dataTable.Rows[i].Field<string>("rrlReference");
				eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptID = dataTable.Rows[i].Field<string>("rrlReverseRmaReceiptID");
				eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptLineID = dataTable.Rows[i].Field<short>("rrlReverseRmaReceiptLineID");
				eRPRMAReceiptLineInformationDto.rrlRmaClaimID = dataTable.Rows[i].Field<string>("rrlRmaClaimID");
				eRPRMAReceiptLineInformationDto.rrlRmaClaimLineID = dataTable.Rows[i].Field<short>("rrlRmaClaimLineID");
				eRPRMAReceiptLineInformationDto.rrlRmaClaimQuantity = dataTable.Rows[i].Field<decimal>("rrlRmaClaimQuantity");
				eRPRMAReceiptLineInformationDto.rrlRmaOpenQuantity = dataTable.Rows[i].Field<decimal>("rrlRmaOpenQuantity");
				eRPRMAReceiptLineInformationDto.rrlRmaReceiptID = dataTable.Rows[i].Field<string>("rrlRmaReceiptID");
				eRPRMAReceiptLineInformationDto.rrlRowVersion = dataTable.Rows[i].Field<byte[]>("rrlRowVersion");
				eRPRMAReceiptLineInformationDto.rrlSalesQuantityReceived = dataTable.Rows[i].Field<decimal>("rrlSalesQuantityReceived");
				eRPRMAReceiptLineInformationDto.rrlSalesUnitOfMeasure = dataTable.Rows[i].Field<string>("rrlSalesUnitOfMeasure");
				eRPRMAReceiptLineInformationDto.rrlRmaReceiptLineID = dataTable.Rows[i].Field<short>("rrlRmaReceiptLineID");
				eRPRMAReceiptLineInformationDto.rrlTotalComponentCosts = dataTable.Rows[i].Field<decimal>("rrlTotalComponentCosts");
				eRPRMAReceiptLineInformationDto.rrlUnitCost = dataTable.Rows[i].Field<decimal>("rrlUnitCost");
				eRPRMAReceiptLineInformationDto.rrlUnitCostForeign = dataTable.Rows[i].Field<decimal>("rrlUnitCostForeign");
				eRPRMAReceiptLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAReceiptLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAReceiptLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAReceiptLineInformationDto> GetRMAReceiptLine(Guid rMAReceiptLineId)
	{
		ERPRMAReceiptLineInformationDto eRPRMAReceiptLineInformationDto = new ERPRMAReceiptLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[45]
		{
			"rrlConversionFactor", "rrlCreatedBy", "rrlCreatedDate", "rrlDescription", "rrlUniqueID", "rrlExtendedCost", "rrlExtendedCostForeign", "rrlHeatLot", "rrlInventoryQuantityReceived", "rrlInventoryUnitOfMeasure",
			"rrlClosed", "rrlInInspection", "rrlInspectionComplete", "rrlInvoicedComplete", "rrlKitPart", "rrlPosted", "rrlReceivedComplete", "rrlRequiresInspection", "rrlReversed", "rrlOrgPartID",
			"rrlOrgPartShortDescription", "rrlPartBinID", "rrlPartID", "rrlPartLongDescriptionRtf", "rrlPartLongDescriptionText", "rrlPartRevisionID", "rrlPartWarehouseLocationID", "rrlProjectAreaID", "rrlProjectID", "rrlQuantityToInspect",
			"rrlReference", "rrlReverseRmaReceiptID", "rrlReverseRmaReceiptLineID", "rrlRmaClaimID", "rrlRmaClaimLineID", "rrlRmaClaimQuantity", "rrlRmaOpenQuantity", "rrlRmaReceiptID", "rrlRowVersion", "rrlSalesQuantityReceived",
			"rrlSalesUnitOfMeasure", "rrlRmaReceiptLineID", "rrlTotalComponentCosts", "rrlUnitCost", "rrlUnitCostForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rrlUniqueID|C", rMAReceiptLineId);
		AddCustomFieldsToSelectList("RMAReceiptLines");
		using (DataTable dataTable = GetAsDataTable("RMAReceiptLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAReceiptLineInformationDto);
			}
			eRPRMAReceiptLineInformationDto.rrlConversionFactor = dataTable.Rows[0].Field<decimal>("rrlConversionFactor");
			eRPRMAReceiptLineInformationDto.rrlCreatedBy = dataTable.Rows[0].Field<string>("rrlCreatedBy");
			eRPRMAReceiptLineInformationDto.rrlCreatedDate = dataTable.Rows[0].Field<DateTime?>("rrlCreatedDate");
			eRPRMAReceiptLineInformationDto.rrlDescription = dataTable.Rows[0].Field<string>("rrlDescription");
			eRPRMAReceiptLineInformationDto.rrlUniqueID = dataTable.Rows[0].Field<Guid>("rrlUniqueID");
			eRPRMAReceiptLineInformationDto.rrlExtendedCost = dataTable.Rows[0].Field<decimal>("rrlExtendedCost");
			eRPRMAReceiptLineInformationDto.rrlExtendedCostForeign = dataTable.Rows[0].Field<decimal>("rrlExtendedCostForeign");
			eRPRMAReceiptLineInformationDto.rrlHeatLot = dataTable.Rows[0].Field<string>("rrlHeatLot");
			eRPRMAReceiptLineInformationDto.rrlInventoryQuantityReceived = dataTable.Rows[0].Field<decimal>("rrlInventoryQuantityReceived");
			eRPRMAReceiptLineInformationDto.rrlInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("rrlInventoryUnitOfMeasure");
			eRPRMAReceiptLineInformationDto.rrlClosed = dataTable.Rows[0].Field<bool>("rrlClosed");
			eRPRMAReceiptLineInformationDto.rrlInInspection = dataTable.Rows[0].Field<bool>("rrlInInspection");
			eRPRMAReceiptLineInformationDto.rrlInspectionComplete = dataTable.Rows[0].Field<bool>("rrlInspectionComplete");
			eRPRMAReceiptLineInformationDto.rrlInvoicedComplete = dataTable.Rows[0].Field<bool>("rrlInvoicedComplete");
			eRPRMAReceiptLineInformationDto.rrlKitPart = dataTable.Rows[0].Field<bool>("rrlKitPart");
			eRPRMAReceiptLineInformationDto.rrlPosted = dataTable.Rows[0].Field<bool>("rrlPosted");
			eRPRMAReceiptLineInformationDto.rrlReceivedComplete = dataTable.Rows[0].Field<bool>("rrlReceivedComplete");
			eRPRMAReceiptLineInformationDto.rrlRequiresInspection = dataTable.Rows[0].Field<bool>("rrlRequiresInspection");
			eRPRMAReceiptLineInformationDto.rrlReversed = dataTable.Rows[0].Field<bool>("rrlReversed");
			eRPRMAReceiptLineInformationDto.rrlOrgPartID = dataTable.Rows[0].Field<string>("rrlOrgPartID");
			eRPRMAReceiptLineInformationDto.rrlOrgPartShortDescription = dataTable.Rows[0].Field<string>("rrlOrgPartShortDescription");
			eRPRMAReceiptLineInformationDto.rrlPartBinID = dataTable.Rows[0].Field<string>("rrlPartBinID");
			eRPRMAReceiptLineInformationDto.rrlPartID = dataTable.Rows[0].Field<string>("rrlPartID");
			eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("rrlPartLongDescriptionRtf");
			eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionText = dataTable.Rows[0].Field<string>("rrlPartLongDescriptionText");
			eRPRMAReceiptLineInformationDto.rrlPartRevisionID = dataTable.Rows[0].Field<string>("rrlPartRevisionID");
			eRPRMAReceiptLineInformationDto.rrlPartWarehouseLocationID = dataTable.Rows[0].Field<string>("rrlPartWarehouseLocationID");
			eRPRMAReceiptLineInformationDto.rrlProjectAreaID = dataTable.Rows[0].Field<string>("rrlProjectAreaID");
			eRPRMAReceiptLineInformationDto.rrlProjectID = dataTable.Rows[0].Field<string>("rrlProjectID");
			eRPRMAReceiptLineInformationDto.rrlQuantityToInspect = dataTable.Rows[0].Field<decimal>("rrlQuantityToInspect");
			eRPRMAReceiptLineInformationDto.rrlReference = dataTable.Rows[0].Field<string>("rrlReference");
			eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptID = dataTable.Rows[0].Field<string>("rrlReverseRmaReceiptID");
			eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptLineID = dataTable.Rows[0].Field<short>("rrlReverseRmaReceiptLineID");
			eRPRMAReceiptLineInformationDto.rrlRmaClaimID = dataTable.Rows[0].Field<string>("rrlRmaClaimID");
			eRPRMAReceiptLineInformationDto.rrlRmaClaimLineID = dataTable.Rows[0].Field<short>("rrlRmaClaimLineID");
			eRPRMAReceiptLineInformationDto.rrlRmaClaimQuantity = dataTable.Rows[0].Field<decimal>("rrlRmaClaimQuantity");
			eRPRMAReceiptLineInformationDto.rrlRmaOpenQuantity = dataTable.Rows[0].Field<decimal>("rrlRmaOpenQuantity");
			eRPRMAReceiptLineInformationDto.rrlRmaReceiptID = dataTable.Rows[0].Field<string>("rrlRmaReceiptID");
			eRPRMAReceiptLineInformationDto.rrlRowVersion = dataTable.Rows[0].Field<byte[]>("rrlRowVersion");
			eRPRMAReceiptLineInformationDto.rrlSalesQuantityReceived = dataTable.Rows[0].Field<decimal>("rrlSalesQuantityReceived");
			eRPRMAReceiptLineInformationDto.rrlSalesUnitOfMeasure = dataTable.Rows[0].Field<string>("rrlSalesUnitOfMeasure");
			eRPRMAReceiptLineInformationDto.rrlRmaReceiptLineID = dataTable.Rows[0].Field<short>("rrlRmaReceiptLineID");
			eRPRMAReceiptLineInformationDto.rrlTotalComponentCosts = dataTable.Rows[0].Field<decimal>("rrlTotalComponentCosts");
			eRPRMAReceiptLineInformationDto.rrlUnitCost = dataTable.Rows[0].Field<decimal>("rrlUnitCost");
			eRPRMAReceiptLineInformationDto.rrlUnitCostForeign = dataTable.Rows[0].Field<decimal>("rrlUnitCostForeign");
			eRPRMAReceiptLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAReceiptLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAReceiptLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRMAReceiptLine(ERPRMAReceiptLineDto rMAReceiptLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RMAReceiptLines WHERE rrlUniqueID = " + M1Util.ConvertToLinq(rMAReceiptLine.rrlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rrlRmaReceiptID"] = rMAReceiptLine.rrlRmaReceiptID.ToUpper();
				dataRow["rrlRmaReceiptLineID"] = rMAReceiptLine.rrlRmaReceiptLineID;
				rMAReceiptLine.rrlUniqueID = ((rMAReceiptLine.rrlUniqueID == Guid.Empty) ? Guid.NewGuid() : rMAReceiptLine.rrlUniqueID);
				dataRow["rrlUniqueID"] = rMAReceiptLine.rrlUniqueID;
				dataRow["rrlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rrlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RMAReceiptLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rMAReceiptLine.rrlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RMAReceiptLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rrlRowVersion"], rMAReceiptLine.rrlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RMAReceiptLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RMAReceiptLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rrlConversionFactor"] = rMAReceiptLine.rrlConversionFactor;
			dataRow["rrlDescription"] = rMAReceiptLine.rrlDescription;
			dataRow["rrlExtendedCost"] = rMAReceiptLine.rrlExtendedCost;
			dataRow["rrlExtendedCostForeign"] = rMAReceiptLine.rrlExtendedCostForeign;
			dataRow["rrlHeatLot"] = rMAReceiptLine.rrlHeatLot;
			dataRow["rrlInventoryQuantityReceived"] = rMAReceiptLine.rrlInventoryQuantityReceived;
			dataRow["rrlInventoryUnitOfMeasure"] = rMAReceiptLine.rrlInventoryUnitOfMeasure;
			dataRow["rrlClosed"] = rMAReceiptLine.rrlClosed;
			dataRow["rrlInInspection"] = rMAReceiptLine.rrlInInspection;
			dataRow["rrlInspectionComplete"] = rMAReceiptLine.rrlInspectionComplete;
			dataRow["rrlInvoicedComplete"] = rMAReceiptLine.rrlInvoicedComplete;
			dataRow["rrlKitPart"] = rMAReceiptLine.rrlKitPart;
			dataRow["rrlPosted"] = rMAReceiptLine.rrlPosted;
			dataRow["rrlReceivedComplete"] = rMAReceiptLine.rrlReceivedComplete;
			dataRow["rrlRequiresInspection"] = rMAReceiptLine.rrlRequiresInspection;
			dataRow["rrlReversed"] = rMAReceiptLine.rrlReversed;
			dataRow["rrlOrgPartID"] = rMAReceiptLine.rrlOrgPartID;
			dataRow["rrlOrgPartShortDescription"] = rMAReceiptLine.rrlOrgPartShortDescription;
			dataRow["rrlPartBinID"] = rMAReceiptLine.rrlPartBinID;
			dataRow["rrlPartID"] = rMAReceiptLine.rrlPartID;
			dataRow["rrlPartLongDescriptionRtf"] = rMAReceiptLine.rrlPartLongDescriptionRtf ?? dataRow["rrlPartLongDescriptionRtf"];
			dataRow["rrlPartLongDescriptionText"] = rMAReceiptLine.rrlPartLongDescriptionText ?? dataRow["rrlPartLongDescriptionText"];
			dataRow["rrlPartRevisionID"] = rMAReceiptLine.rrlPartRevisionID;
			dataRow["rrlPartWarehouseLocationID"] = rMAReceiptLine.rrlPartWarehouseLocationID;
			dataRow["rrlProjectAreaID"] = rMAReceiptLine.rrlProjectAreaID;
			dataRow["rrlProjectID"] = rMAReceiptLine.rrlProjectID;
			dataRow["rrlQuantityToInspect"] = rMAReceiptLine.rrlQuantityToInspect;
			dataRow["rrlReference"] = rMAReceiptLine.rrlReference;
			dataRow["rrlReverseRmaReceiptID"] = rMAReceiptLine.rrlReverseRmaReceiptID;
			dataRow["rrlReverseRmaReceiptLineID"] = rMAReceiptLine.rrlReverseRmaReceiptLineID;
			dataRow["rrlRmaClaimID"] = rMAReceiptLine.rrlRmaClaimID;
			dataRow["rrlRmaClaimLineID"] = rMAReceiptLine.rrlRmaClaimLineID;
			dataRow["rrlRmaClaimQuantity"] = rMAReceiptLine.rrlRmaClaimQuantity;
			dataRow["rrlRmaOpenQuantity"] = rMAReceiptLine.rrlRmaOpenQuantity;
			dataRow["rrlSalesQuantityReceived"] = rMAReceiptLine.rrlSalesQuantityReceived;
			dataRow["rrlSalesUnitOfMeasure"] = rMAReceiptLine.rrlSalesUnitOfMeasure;
			dataRow["rrlTotalComponentCosts"] = rMAReceiptLine.rrlTotalComponentCosts;
			dataRow["rrlUnitCost"] = rMAReceiptLine.rrlUnitCost;
			dataRow["rrlUnitCostForeign"] = rMAReceiptLine.rrlUnitCostForeign;
			if (rMAReceiptLine.CustomFields != null && rMAReceiptLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rMAReceiptLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RMAReceiptLine [{rMAReceiptLine.rrlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RMAReceiptLine [{rMAReceiptLine.rrlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
