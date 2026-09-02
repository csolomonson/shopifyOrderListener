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

public class ERPDMRShipmentLineRepository : APIBaseRepository, IERPDMRShipmentLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPDMRShipmentLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDMRShipmentLineExist(Guid dMRShipmentLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("dslUniqueID|C", dMRShipmentLineId);
		base.selectList.Add("dslUniqueID");
		return Task.FromResult(GetAsObject("DMRShipmentLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDMRShipmentLineInformationDto>> GetAllDMRShipmentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDMRShipmentLineInformationDto> collection = new List<ERPDMRShipmentLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[43]
		{
			"dslConversionFactor", "dslCreatedBy", "dslCreatedDate", "dslDescription", "dslDmrClaimID", "dslDmrClaimLineID", "dslDmrClaimQuantity", "dslDmrOpenQuantity", "dslDmrShipmentID", "dslUniqueID",
			"dslInspectionID", "dslInspectionLineID", "dslInventoryQuantityShipped", "dslInventoryUnitOfMeasure", "dslClosed", "dslInvoicedComplete", "dslKitPart", "dslPosted", "dslReversed", "dslShippedComplete",
			"dslJobAssemblyID", "dslJobID", "dslJobMaterialID", "dslJobMatQuantityShipped", "dslJobOperationID", "dslJobOprQuantityShipped", "dslPartBinID", "dslPartID", "dslPartLongDescriptionRtf", "dslPartLongDescriptionText",
			"dslPartRevisionID", "dslPartWarehouseLocationID", "dslProjectAreaID", "dslProjectID", "dslQuantityShipped", "dslReturnQuantityShipped", "dslReverseDmrShipmentID", "dslReverseDmrShipmentLineID", "dslRowVersion", "dslDmrShipmentLineID",
			"dslUnitOfMeasure", "dslUnitPrice", "dslUnitPriceForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DMRShipmentLines");
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
		using (DataTable dataTable = GetAsDataTable("DMRShipmentLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDMRShipmentLineInformationDto eRPDMRShipmentLineInformationDto = new ERPDMRShipmentLineInformationDto();
				eRPDMRShipmentLineInformationDto.dslConversionFactor = dataTable.Rows[i].Field<decimal>("dslConversionFactor");
				eRPDMRShipmentLineInformationDto.dslCreatedBy = dataTable.Rows[i].Field<string>("dslCreatedBy");
				eRPDMRShipmentLineInformationDto.dslCreatedDate = dataTable.Rows[i].Field<DateTime?>("dslCreatedDate");
				eRPDMRShipmentLineInformationDto.dslDescription = dataTable.Rows[i].Field<string>("dslDescription");
				eRPDMRShipmentLineInformationDto.dslDmrClaimID = dataTable.Rows[i].Field<string>("dslDmrClaimID");
				eRPDMRShipmentLineInformationDto.dslDmrClaimLineID = dataTable.Rows[i].Field<short>("dslDmrClaimLineID");
				eRPDMRShipmentLineInformationDto.dslDmrClaimQuantity = dataTable.Rows[i].Field<decimal>("dslDmrClaimQuantity");
				eRPDMRShipmentLineInformationDto.dslDmrOpenQuantity = dataTable.Rows[i].Field<decimal>("dslDmrOpenQuantity");
				eRPDMRShipmentLineInformationDto.dslDmrShipmentID = dataTable.Rows[i].Field<string>("dslDmrShipmentID");
				eRPDMRShipmentLineInformationDto.dslUniqueID = dataTable.Rows[i].Field<Guid>("dslUniqueID");
				eRPDMRShipmentLineInformationDto.dslInspectionID = dataTable.Rows[i].Field<string>("dslInspectionID");
				eRPDMRShipmentLineInformationDto.dslInspectionLineID = dataTable.Rows[i].Field<short>("dslInspectionLineID");
				eRPDMRShipmentLineInformationDto.dslInventoryQuantityShipped = dataTable.Rows[i].Field<decimal>("dslInventoryQuantityShipped");
				eRPDMRShipmentLineInformationDto.dslInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("dslInventoryUnitOfMeasure");
				eRPDMRShipmentLineInformationDto.dslClosed = dataTable.Rows[i].Field<bool>("dslClosed");
				eRPDMRShipmentLineInformationDto.dslInvoicedComplete = dataTable.Rows[i].Field<bool>("dslInvoicedComplete");
				eRPDMRShipmentLineInformationDto.dslKitPart = dataTable.Rows[i].Field<bool>("dslKitPart");
				eRPDMRShipmentLineInformationDto.dslPosted = dataTable.Rows[i].Field<bool>("dslPosted");
				eRPDMRShipmentLineInformationDto.dslReversed = dataTable.Rows[i].Field<bool>("dslReversed");
				eRPDMRShipmentLineInformationDto.dslShippedComplete = dataTable.Rows[i].Field<bool>("dslShippedComplete");
				eRPDMRShipmentLineInformationDto.dslJobAssemblyID = dataTable.Rows[i].Field<int>("dslJobAssemblyID");
				eRPDMRShipmentLineInformationDto.dslJobID = dataTable.Rows[i].Field<string>("dslJobID");
				eRPDMRShipmentLineInformationDto.dslJobMaterialID = dataTable.Rows[i].Field<int>("dslJobMaterialID");
				eRPDMRShipmentLineInformationDto.dslJobMatQuantityShipped = dataTable.Rows[i].Field<decimal>("dslJobMatQuantityShipped");
				eRPDMRShipmentLineInformationDto.dslJobOperationID = dataTable.Rows[i].Field<int>("dslJobOperationID");
				eRPDMRShipmentLineInformationDto.dslJobOprQuantityShipped = dataTable.Rows[i].Field<decimal>("dslJobOprQuantityShipped");
				eRPDMRShipmentLineInformationDto.dslPartBinID = dataTable.Rows[i].Field<string>("dslPartBinID");
				eRPDMRShipmentLineInformationDto.dslPartID = dataTable.Rows[i].Field<string>("dslPartID");
				eRPDMRShipmentLineInformationDto.dslPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("dslPartLongDescriptionRtf");
				eRPDMRShipmentLineInformationDto.dslPartLongDescriptionText = dataTable.Rows[i].Field<string>("dslPartLongDescriptionText");
				eRPDMRShipmentLineInformationDto.dslPartRevisionID = dataTable.Rows[i].Field<string>("dslPartRevisionID");
				eRPDMRShipmentLineInformationDto.dslPartWarehouseLocationID = dataTable.Rows[i].Field<string>("dslPartWarehouseLocationID");
				eRPDMRShipmentLineInformationDto.dslProjectAreaID = dataTable.Rows[i].Field<string>("dslProjectAreaID");
				eRPDMRShipmentLineInformationDto.dslProjectID = dataTable.Rows[i].Field<string>("dslProjectID");
				eRPDMRShipmentLineInformationDto.dslQuantityShipped = dataTable.Rows[i].Field<decimal>("dslQuantityShipped");
				eRPDMRShipmentLineInformationDto.dslReturnQuantityShipped = dataTable.Rows[i].Field<decimal>("dslReturnQuantityShipped");
				eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentID = dataTable.Rows[i].Field<string>("dslReverseDmrShipmentID");
				eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentLineID = dataTable.Rows[i].Field<short>("dslReverseDmrShipmentLineID");
				eRPDMRShipmentLineInformationDto.dslRowVersion = dataTable.Rows[i].Field<byte[]>("dslRowVersion");
				eRPDMRShipmentLineInformationDto.dslDmrShipmentLineID = dataTable.Rows[i].Field<short>("dslDmrShipmentLineID");
				eRPDMRShipmentLineInformationDto.dslUnitOfMeasure = dataTable.Rows[i].Field<string>("dslUnitOfMeasure");
				eRPDMRShipmentLineInformationDto.dslUnitPrice = dataTable.Rows[i].Field<decimal>("dslUnitPrice");
				eRPDMRShipmentLineInformationDto.dslUnitPriceForeign = dataTable.Rows[i].Field<decimal>("dslUnitPriceForeign");
				eRPDMRShipmentLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDMRShipmentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDMRShipmentLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDMRShipmentLineInformationDto> GetDMRShipmentLine(Guid dMRShipmentLineId)
	{
		ERPDMRShipmentLineInformationDto eRPDMRShipmentLineInformationDto = new ERPDMRShipmentLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[43]
		{
			"dslConversionFactor", "dslCreatedBy", "dslCreatedDate", "dslDescription", "dslDmrClaimID", "dslDmrClaimLineID", "dslDmrClaimQuantity", "dslDmrOpenQuantity", "dslDmrShipmentID", "dslUniqueID",
			"dslInspectionID", "dslInspectionLineID", "dslInventoryQuantityShipped", "dslInventoryUnitOfMeasure", "dslClosed", "dslInvoicedComplete", "dslKitPart", "dslPosted", "dslReversed", "dslShippedComplete",
			"dslJobAssemblyID", "dslJobID", "dslJobMaterialID", "dslJobMatQuantityShipped", "dslJobOperationID", "dslJobOprQuantityShipped", "dslPartBinID", "dslPartID", "dslPartLongDescriptionRtf", "dslPartLongDescriptionText",
			"dslPartRevisionID", "dslPartWarehouseLocationID", "dslProjectAreaID", "dslProjectID", "dslQuantityShipped", "dslReturnQuantityShipped", "dslReverseDmrShipmentID", "dslReverseDmrShipmentLineID", "dslRowVersion", "dslDmrShipmentLineID",
			"dslUnitOfMeasure", "dslUnitPrice", "dslUnitPriceForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("dslUniqueID|C", dMRShipmentLineId);
		AddCustomFieldsToSelectList("DMRShipmentLines");
		using (DataTable dataTable = GetAsDataTable("DMRShipmentLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDMRShipmentLineInformationDto);
			}
			eRPDMRShipmentLineInformationDto.dslConversionFactor = dataTable.Rows[0].Field<decimal>("dslConversionFactor");
			eRPDMRShipmentLineInformationDto.dslCreatedBy = dataTable.Rows[0].Field<string>("dslCreatedBy");
			eRPDMRShipmentLineInformationDto.dslCreatedDate = dataTable.Rows[0].Field<DateTime?>("dslCreatedDate");
			eRPDMRShipmentLineInformationDto.dslDescription = dataTable.Rows[0].Field<string>("dslDescription");
			eRPDMRShipmentLineInformationDto.dslDmrClaimID = dataTable.Rows[0].Field<string>("dslDmrClaimID");
			eRPDMRShipmentLineInformationDto.dslDmrClaimLineID = dataTable.Rows[0].Field<short>("dslDmrClaimLineID");
			eRPDMRShipmentLineInformationDto.dslDmrClaimQuantity = dataTable.Rows[0].Field<decimal>("dslDmrClaimQuantity");
			eRPDMRShipmentLineInformationDto.dslDmrOpenQuantity = dataTable.Rows[0].Field<decimal>("dslDmrOpenQuantity");
			eRPDMRShipmentLineInformationDto.dslDmrShipmentID = dataTable.Rows[0].Field<string>("dslDmrShipmentID");
			eRPDMRShipmentLineInformationDto.dslUniqueID = dataTable.Rows[0].Field<Guid>("dslUniqueID");
			eRPDMRShipmentLineInformationDto.dslInspectionID = dataTable.Rows[0].Field<string>("dslInspectionID");
			eRPDMRShipmentLineInformationDto.dslInspectionLineID = dataTable.Rows[0].Field<short>("dslInspectionLineID");
			eRPDMRShipmentLineInformationDto.dslInventoryQuantityShipped = dataTable.Rows[0].Field<decimal>("dslInventoryQuantityShipped");
			eRPDMRShipmentLineInformationDto.dslInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("dslInventoryUnitOfMeasure");
			eRPDMRShipmentLineInformationDto.dslClosed = dataTable.Rows[0].Field<bool>("dslClosed");
			eRPDMRShipmentLineInformationDto.dslInvoicedComplete = dataTable.Rows[0].Field<bool>("dslInvoicedComplete");
			eRPDMRShipmentLineInformationDto.dslKitPart = dataTable.Rows[0].Field<bool>("dslKitPart");
			eRPDMRShipmentLineInformationDto.dslPosted = dataTable.Rows[0].Field<bool>("dslPosted");
			eRPDMRShipmentLineInformationDto.dslReversed = dataTable.Rows[0].Field<bool>("dslReversed");
			eRPDMRShipmentLineInformationDto.dslShippedComplete = dataTable.Rows[0].Field<bool>("dslShippedComplete");
			eRPDMRShipmentLineInformationDto.dslJobAssemblyID = dataTable.Rows[0].Field<int>("dslJobAssemblyID");
			eRPDMRShipmentLineInformationDto.dslJobID = dataTable.Rows[0].Field<string>("dslJobID");
			eRPDMRShipmentLineInformationDto.dslJobMaterialID = dataTable.Rows[0].Field<int>("dslJobMaterialID");
			eRPDMRShipmentLineInformationDto.dslJobMatQuantityShipped = dataTable.Rows[0].Field<decimal>("dslJobMatQuantityShipped");
			eRPDMRShipmentLineInformationDto.dslJobOperationID = dataTable.Rows[0].Field<int>("dslJobOperationID");
			eRPDMRShipmentLineInformationDto.dslJobOprQuantityShipped = dataTable.Rows[0].Field<decimal>("dslJobOprQuantityShipped");
			eRPDMRShipmentLineInformationDto.dslPartBinID = dataTable.Rows[0].Field<string>("dslPartBinID");
			eRPDMRShipmentLineInformationDto.dslPartID = dataTable.Rows[0].Field<string>("dslPartID");
			eRPDMRShipmentLineInformationDto.dslPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("dslPartLongDescriptionRtf");
			eRPDMRShipmentLineInformationDto.dslPartLongDescriptionText = dataTable.Rows[0].Field<string>("dslPartLongDescriptionText");
			eRPDMRShipmentLineInformationDto.dslPartRevisionID = dataTable.Rows[0].Field<string>("dslPartRevisionID");
			eRPDMRShipmentLineInformationDto.dslPartWarehouseLocationID = dataTable.Rows[0].Field<string>("dslPartWarehouseLocationID");
			eRPDMRShipmentLineInformationDto.dslProjectAreaID = dataTable.Rows[0].Field<string>("dslProjectAreaID");
			eRPDMRShipmentLineInformationDto.dslProjectID = dataTable.Rows[0].Field<string>("dslProjectID");
			eRPDMRShipmentLineInformationDto.dslQuantityShipped = dataTable.Rows[0].Field<decimal>("dslQuantityShipped");
			eRPDMRShipmentLineInformationDto.dslReturnQuantityShipped = dataTable.Rows[0].Field<decimal>("dslReturnQuantityShipped");
			eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentID = dataTable.Rows[0].Field<string>("dslReverseDmrShipmentID");
			eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentLineID = dataTable.Rows[0].Field<short>("dslReverseDmrShipmentLineID");
			eRPDMRShipmentLineInformationDto.dslRowVersion = dataTable.Rows[0].Field<byte[]>("dslRowVersion");
			eRPDMRShipmentLineInformationDto.dslDmrShipmentLineID = dataTable.Rows[0].Field<short>("dslDmrShipmentLineID");
			eRPDMRShipmentLineInformationDto.dslUnitOfMeasure = dataTable.Rows[0].Field<string>("dslUnitOfMeasure");
			eRPDMRShipmentLineInformationDto.dslUnitPrice = dataTable.Rows[0].Field<decimal>("dslUnitPrice");
			eRPDMRShipmentLineInformationDto.dslUnitPriceForeign = dataTable.Rows[0].Field<decimal>("dslUnitPriceForeign");
			eRPDMRShipmentLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDMRShipmentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDMRShipmentLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDMRShipmentLine(ERPDMRShipmentLineDto dMRShipmentLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DMRShipmentLines WHERE dslUniqueID = " + M1Util.ConvertToLinq(dMRShipmentLine.dslUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["dslDmrShipmentID"] = dMRShipmentLine.dslDmrShipmentID.ToUpper();
				dataRow["dslDmrShipmentLineID"] = dMRShipmentLine.dslDmrShipmentLineID;
				dMRShipmentLine.dslUniqueID = ((dMRShipmentLine.dslUniqueID == Guid.Empty) ? Guid.NewGuid() : dMRShipmentLine.dslUniqueID);
				dataRow["dslUniqueID"] = dMRShipmentLine.dslUniqueID;
				dataRow["dslCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["dslCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DMRShipmentLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (dMRShipmentLine.dslRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DMRShipmentLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["dslRowVersion"], dMRShipmentLine.dslRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DMRShipmentLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DMRShipmentLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["dslConversionFactor"] = dMRShipmentLine.dslConversionFactor;
			dataRow["dslDescription"] = dMRShipmentLine.dslDescription;
			dataRow["dslDmrClaimID"] = dMRShipmentLine.dslDmrClaimID;
			dataRow["dslDmrClaimLineID"] = dMRShipmentLine.dslDmrClaimLineID;
			dataRow["dslDmrClaimQuantity"] = dMRShipmentLine.dslDmrClaimQuantity;
			dataRow["dslDmrOpenQuantity"] = dMRShipmentLine.dslDmrOpenQuantity;
			dataRow["dslInspectionID"] = dMRShipmentLine.dslInspectionID;
			dataRow["dslInspectionLineID"] = dMRShipmentLine.dslInspectionLineID;
			dataRow["dslInventoryQuantityShipped"] = dMRShipmentLine.dslInventoryQuantityShipped;
			dataRow["dslInventoryUnitOfMeasure"] = dMRShipmentLine.dslInventoryUnitOfMeasure;
			dataRow["dslClosed"] = dMRShipmentLine.dslClosed;
			dataRow["dslInvoicedComplete"] = dMRShipmentLine.dslInvoicedComplete;
			dataRow["dslKitPart"] = dMRShipmentLine.dslKitPart;
			dataRow["dslPosted"] = dMRShipmentLine.dslPosted;
			dataRow["dslReversed"] = dMRShipmentLine.dslReversed;
			dataRow["dslShippedComplete"] = dMRShipmentLine.dslShippedComplete;
			dataRow["dslJobAssemblyID"] = dMRShipmentLine.dslJobAssemblyID;
			dataRow["dslJobID"] = dMRShipmentLine.dslJobID;
			dataRow["dslJobMaterialID"] = dMRShipmentLine.dslJobMaterialID;
			dataRow["dslJobMatQuantityShipped"] = dMRShipmentLine.dslJobMatQuantityShipped;
			dataRow["dslJobOperationID"] = dMRShipmentLine.dslJobOperationID;
			dataRow["dslJobOprQuantityShipped"] = dMRShipmentLine.dslJobOprQuantityShipped;
			dataRow["dslPartBinID"] = dMRShipmentLine.dslPartBinID;
			dataRow["dslPartID"] = dMRShipmentLine.dslPartID;
			dataRow["dslPartLongDescriptionRtf"] = dMRShipmentLine.dslPartLongDescriptionRtf ?? dataRow["dslPartLongDescriptionRtf"];
			dataRow["dslPartLongDescriptionText"] = dMRShipmentLine.dslPartLongDescriptionText ?? dataRow["dslPartLongDescriptionText"];
			dataRow["dslPartRevisionID"] = dMRShipmentLine.dslPartRevisionID;
			dataRow["dslPartWarehouseLocationID"] = dMRShipmentLine.dslPartWarehouseLocationID;
			dataRow["dslProjectAreaID"] = dMRShipmentLine.dslProjectAreaID;
			dataRow["dslProjectID"] = dMRShipmentLine.dslProjectID;
			dataRow["dslQuantityShipped"] = dMRShipmentLine.dslQuantityShipped;
			dataRow["dslReturnQuantityShipped"] = dMRShipmentLine.dslReturnQuantityShipped;
			dataRow["dslReverseDmrShipmentID"] = dMRShipmentLine.dslReverseDmrShipmentID;
			dataRow["dslReverseDmrShipmentLineID"] = dMRShipmentLine.dslReverseDmrShipmentLineID;
			dataRow["dslUnitOfMeasure"] = dMRShipmentLine.dslUnitOfMeasure;
			dataRow["dslUnitPrice"] = dMRShipmentLine.dslUnitPrice;
			dataRow["dslUnitPriceForeign"] = dMRShipmentLine.dslUnitPriceForeign;
			if (dMRShipmentLine.CustomFields != null && dMRShipmentLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in dMRShipmentLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DMRShipmentLine [{dMRShipmentLine.dslUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DMRShipmentLine [{dMRShipmentLine.dslUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
