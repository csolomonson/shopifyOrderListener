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

public class ERPDMRShipmentRepository : APIBaseRepository, IERPDMRShipmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPDMRShipmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDMRShipmentExist(Guid dMRShipmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("dspUniqueID|C", dMRShipmentId);
		base.selectList.Add("dspUniqueID");
		return Task.FromResult(GetAsObject("DMRShipments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDMRShipmentInformationDto>> GetAllDMRShipments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDMRShipmentInformationDto> collection = new List<ERPDMRShipmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[35]
		{
			"dspApInvoiceLocationID", "dspClosedDate", "dspDmrShipmentID", "dspCreatedBy", "dspCreatedDate", "dspCurrencyRateID", "dspUniqueID", "dspExchangeRate", "dspFreightCharge", "dspFreightChargeForeign",
			"dspFreightSubtotal", "dspFreightTotal", "dspClosed", "dspCustomRate", "dspPosted", "dspPrintDmrPackingSlip", "dspPrintLabels", "dspReversalEntry", "dspReversed", "dspNumberOfLabels",
			"dspPlantDepartmentID", "dspPlantID", "dspPostedDate", "dspProjectID", "dspRowVersion", "dspShipContactID", "dspShipDate", "dspShipLocationID", "dspShippingCommentsRTF", "dspShippingCommentsText",
			"dspShippingMethodID", "dspShippingPaymentTypeID", "dspStandardMessageID", "dspSupplierOrganizationID", "dspTrackingNumber"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DMRShipments");
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
		using (DataTable dataTable = GetAsDataTable("DMRShipments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDMRShipmentInformationDto eRPDMRShipmentInformationDto = new ERPDMRShipmentInformationDto();
				eRPDMRShipmentInformationDto.dspApInvoiceLocationID = dataTable.Rows[i].Field<string>("dspApInvoiceLocationID");
				eRPDMRShipmentInformationDto.dspClosedDate = dataTable.Rows[i].Field<DateTime?>("dspClosedDate");
				eRPDMRShipmentInformationDto.dspDmrShipmentID = dataTable.Rows[i].Field<string>("dspDmrShipmentID");
				eRPDMRShipmentInformationDto.dspCreatedBy = dataTable.Rows[i].Field<string>("dspCreatedBy");
				eRPDMRShipmentInformationDto.dspCreatedDate = dataTable.Rows[i].Field<DateTime?>("dspCreatedDate");
				eRPDMRShipmentInformationDto.dspCurrencyRateID = dataTable.Rows[i].Field<string>("dspCurrencyRateID");
				eRPDMRShipmentInformationDto.dspUniqueID = dataTable.Rows[i].Field<Guid>("dspUniqueID");
				eRPDMRShipmentInformationDto.dspExchangeRate = dataTable.Rows[i].Field<decimal>("dspExchangeRate");
				eRPDMRShipmentInformationDto.dspFreightCharge = dataTable.Rows[i].Field<decimal>("dspFreightCharge");
				eRPDMRShipmentInformationDto.dspFreightChargeForeign = dataTable.Rows[i].Field<decimal>("dspFreightChargeForeign");
				eRPDMRShipmentInformationDto.dspFreightSubtotal = dataTable.Rows[i].Field<decimal>("dspFreightSubtotal");
				eRPDMRShipmentInformationDto.dspFreightTotal = dataTable.Rows[i].Field<decimal>("dspFreightTotal");
				eRPDMRShipmentInformationDto.dspClosed = dataTable.Rows[i].Field<bool>("dspClosed");
				eRPDMRShipmentInformationDto.dspCustomRate = dataTable.Rows[i].Field<bool>("dspCustomRate");
				eRPDMRShipmentInformationDto.dspPosted = dataTable.Rows[i].Field<bool>("dspPosted");
				eRPDMRShipmentInformationDto.dspPrintDmrPackingSlip = dataTable.Rows[i].Field<bool>("dspPrintDmrPackingSlip");
				eRPDMRShipmentInformationDto.dspPrintLabels = dataTable.Rows[i].Field<bool>("dspPrintLabels");
				eRPDMRShipmentInformationDto.dspReversalEntry = dataTable.Rows[i].Field<bool>("dspReversalEntry");
				eRPDMRShipmentInformationDto.dspReversed = dataTable.Rows[i].Field<bool>("dspReversed");
				eRPDMRShipmentInformationDto.dspNumberOfLabels = dataTable.Rows[i].Field<short>("dspNumberOfLabels");
				eRPDMRShipmentInformationDto.dspPlantDepartmentID = dataTable.Rows[i].Field<string>("dspPlantDepartmentID");
				eRPDMRShipmentInformationDto.dspPlantID = dataTable.Rows[i].Field<string>("dspPlantID");
				eRPDMRShipmentInformationDto.dspPostedDate = dataTable.Rows[i].Field<DateTime?>("dspPostedDate");
				eRPDMRShipmentInformationDto.dspProjectID = dataTable.Rows[i].Field<string>("dspProjectID");
				eRPDMRShipmentInformationDto.dspRowVersion = dataTable.Rows[i].Field<byte[]>("dspRowVersion");
				eRPDMRShipmentInformationDto.dspShipContactID = dataTable.Rows[i].Field<string>("dspShipContactID");
				eRPDMRShipmentInformationDto.dspShipDate = dataTable.Rows[i].Field<DateTime?>("dspShipDate");
				eRPDMRShipmentInformationDto.dspShipLocationID = dataTable.Rows[i].Field<string>("dspShipLocationID");
				eRPDMRShipmentInformationDto.dspShippingCommentsRTF = dataTable.Rows[i].Field<string>("dspShippingCommentsRTF");
				eRPDMRShipmentInformationDto.dspShippingCommentsText = dataTable.Rows[i].Field<string>("dspShippingCommentsText");
				eRPDMRShipmentInformationDto.dspShippingMethodID = dataTable.Rows[i].Field<string>("dspShippingMethodID");
				eRPDMRShipmentInformationDto.dspShippingPaymentTypeID = dataTable.Rows[i].Field<string>("dspShippingPaymentTypeID");
				eRPDMRShipmentInformationDto.dspStandardMessageID = dataTable.Rows[i].Field<string>("dspStandardMessageID");
				eRPDMRShipmentInformationDto.dspSupplierOrganizationID = dataTable.Rows[i].Field<string>("dspSupplierOrganizationID");
				eRPDMRShipmentInformationDto.dspTrackingNumber = dataTable.Rows[i].Field<string>("dspTrackingNumber");
				eRPDMRShipmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDMRShipmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDMRShipmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDMRShipmentInformationDto> GetDMRShipment(Guid dMRShipmentId)
	{
		ERPDMRShipmentInformationDto eRPDMRShipmentInformationDto = new ERPDMRShipmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[35]
		{
			"dspApInvoiceLocationID", "dspClosedDate", "dspDmrShipmentID", "dspCreatedBy", "dspCreatedDate", "dspCurrencyRateID", "dspUniqueID", "dspExchangeRate", "dspFreightCharge", "dspFreightChargeForeign",
			"dspFreightSubtotal", "dspFreightTotal", "dspClosed", "dspCustomRate", "dspPosted", "dspPrintDmrPackingSlip", "dspPrintLabels", "dspReversalEntry", "dspReversed", "dspNumberOfLabels",
			"dspPlantDepartmentID", "dspPlantID", "dspPostedDate", "dspProjectID", "dspRowVersion", "dspShipContactID", "dspShipDate", "dspShipLocationID", "dspShippingCommentsRTF", "dspShippingCommentsText",
			"dspShippingMethodID", "dspShippingPaymentTypeID", "dspStandardMessageID", "dspSupplierOrganizationID", "dspTrackingNumber"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("dspUniqueID|C", dMRShipmentId);
		AddCustomFieldsToSelectList("DMRShipments");
		using (DataTable dataTable = GetAsDataTable("DMRShipments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDMRShipmentInformationDto);
			}
			eRPDMRShipmentInformationDto.dspApInvoiceLocationID = dataTable.Rows[0].Field<string>("dspApInvoiceLocationID");
			eRPDMRShipmentInformationDto.dspClosedDate = dataTable.Rows[0].Field<DateTime?>("dspClosedDate");
			eRPDMRShipmentInformationDto.dspDmrShipmentID = dataTable.Rows[0].Field<string>("dspDmrShipmentID");
			eRPDMRShipmentInformationDto.dspCreatedBy = dataTable.Rows[0].Field<string>("dspCreatedBy");
			eRPDMRShipmentInformationDto.dspCreatedDate = dataTable.Rows[0].Field<DateTime?>("dspCreatedDate");
			eRPDMRShipmentInformationDto.dspCurrencyRateID = dataTable.Rows[0].Field<string>("dspCurrencyRateID");
			eRPDMRShipmentInformationDto.dspUniqueID = dataTable.Rows[0].Field<Guid>("dspUniqueID");
			eRPDMRShipmentInformationDto.dspExchangeRate = dataTable.Rows[0].Field<decimal>("dspExchangeRate");
			eRPDMRShipmentInformationDto.dspFreightCharge = dataTable.Rows[0].Field<decimal>("dspFreightCharge");
			eRPDMRShipmentInformationDto.dspFreightChargeForeign = dataTable.Rows[0].Field<decimal>("dspFreightChargeForeign");
			eRPDMRShipmentInformationDto.dspFreightSubtotal = dataTable.Rows[0].Field<decimal>("dspFreightSubtotal");
			eRPDMRShipmentInformationDto.dspFreightTotal = dataTable.Rows[0].Field<decimal>("dspFreightTotal");
			eRPDMRShipmentInformationDto.dspClosed = dataTable.Rows[0].Field<bool>("dspClosed");
			eRPDMRShipmentInformationDto.dspCustomRate = dataTable.Rows[0].Field<bool>("dspCustomRate");
			eRPDMRShipmentInformationDto.dspPosted = dataTable.Rows[0].Field<bool>("dspPosted");
			eRPDMRShipmentInformationDto.dspPrintDmrPackingSlip = dataTable.Rows[0].Field<bool>("dspPrintDmrPackingSlip");
			eRPDMRShipmentInformationDto.dspPrintLabels = dataTable.Rows[0].Field<bool>("dspPrintLabels");
			eRPDMRShipmentInformationDto.dspReversalEntry = dataTable.Rows[0].Field<bool>("dspReversalEntry");
			eRPDMRShipmentInformationDto.dspReversed = dataTable.Rows[0].Field<bool>("dspReversed");
			eRPDMRShipmentInformationDto.dspNumberOfLabels = dataTable.Rows[0].Field<short>("dspNumberOfLabels");
			eRPDMRShipmentInformationDto.dspPlantDepartmentID = dataTable.Rows[0].Field<string>("dspPlantDepartmentID");
			eRPDMRShipmentInformationDto.dspPlantID = dataTable.Rows[0].Field<string>("dspPlantID");
			eRPDMRShipmentInformationDto.dspPostedDate = dataTable.Rows[0].Field<DateTime?>("dspPostedDate");
			eRPDMRShipmentInformationDto.dspProjectID = dataTable.Rows[0].Field<string>("dspProjectID");
			eRPDMRShipmentInformationDto.dspRowVersion = dataTable.Rows[0].Field<byte[]>("dspRowVersion");
			eRPDMRShipmentInformationDto.dspShipContactID = dataTable.Rows[0].Field<string>("dspShipContactID");
			eRPDMRShipmentInformationDto.dspShipDate = dataTable.Rows[0].Field<DateTime?>("dspShipDate");
			eRPDMRShipmentInformationDto.dspShipLocationID = dataTable.Rows[0].Field<string>("dspShipLocationID");
			eRPDMRShipmentInformationDto.dspShippingCommentsRTF = dataTable.Rows[0].Field<string>("dspShippingCommentsRTF");
			eRPDMRShipmentInformationDto.dspShippingCommentsText = dataTable.Rows[0].Field<string>("dspShippingCommentsText");
			eRPDMRShipmentInformationDto.dspShippingMethodID = dataTable.Rows[0].Field<string>("dspShippingMethodID");
			eRPDMRShipmentInformationDto.dspShippingPaymentTypeID = dataTable.Rows[0].Field<string>("dspShippingPaymentTypeID");
			eRPDMRShipmentInformationDto.dspStandardMessageID = dataTable.Rows[0].Field<string>("dspStandardMessageID");
			eRPDMRShipmentInformationDto.dspSupplierOrganizationID = dataTable.Rows[0].Field<string>("dspSupplierOrganizationID");
			eRPDMRShipmentInformationDto.dspTrackingNumber = dataTable.Rows[0].Field<string>("dspTrackingNumber");
			eRPDMRShipmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDMRShipmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDMRShipmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDMRShipment(ERPDMRShipmentDto dMRShipment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DMRShipments WHERE dspUniqueID = " + M1Util.ConvertToLinq(dMRShipment.dspUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["dspDmrShipmentID"] = dMRShipment.dspDmrShipmentID.ToUpper();
				dMRShipment.dspUniqueID = ((dMRShipment.dspUniqueID == Guid.Empty) ? Guid.NewGuid() : dMRShipment.dspUniqueID);
				dataRow["dspUniqueID"] = dMRShipment.dspUniqueID;
				dataRow["dspCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["dspCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DMRShipment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (dMRShipment.dspRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DMRShipment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["dspRowVersion"], dMRShipment.dspRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DMRShipment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DMRShipment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["dspApInvoiceLocationID"] = dMRShipment.dspApInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? dspClosedDate = dMRShipment.dspClosedDate;
			dataRow2["dspClosedDate"] = (dspClosedDate.HasValue ? ((object)dspClosedDate.GetValueOrDefault()) : dataRow["dspClosedDate"]);
			dataRow["dspCurrencyRateID"] = dMRShipment.dspCurrencyRateID;
			dataRow["dspExchangeRate"] = dMRShipment.dspExchangeRate;
			dataRow["dspFreightCharge"] = dMRShipment.dspFreightCharge;
			dataRow["dspFreightChargeForeign"] = dMRShipment.dspFreightChargeForeign;
			dataRow["dspFreightSubtotal"] = dMRShipment.dspFreightSubtotal;
			dataRow["dspFreightTotal"] = dMRShipment.dspFreightTotal;
			dataRow["dspClosed"] = dMRShipment.dspClosed;
			dataRow["dspCustomRate"] = dMRShipment.dspCustomRate;
			dataRow["dspPosted"] = dMRShipment.dspPosted;
			dataRow["dspPrintDmrPackingSlip"] = dMRShipment.dspPrintDmrPackingSlip;
			dataRow["dspPrintLabels"] = dMRShipment.dspPrintLabels;
			dataRow["dspReversalEntry"] = dMRShipment.dspReversalEntry;
			dataRow["dspReversed"] = dMRShipment.dspReversed;
			dataRow["dspNumberOfLabels"] = dMRShipment.dspNumberOfLabels;
			dataRow["dspPlantDepartmentID"] = dMRShipment.dspPlantDepartmentID;
			dataRow["dspPlantID"] = dMRShipment.dspPlantID;
			DataRow dataRow3 = dataRow;
			dspClosedDate = dMRShipment.dspPostedDate;
			dataRow3["dspPostedDate"] = (dspClosedDate.HasValue ? ((object)dspClosedDate.GetValueOrDefault()) : dataRow["dspPostedDate"]);
			dataRow["dspProjectID"] = dMRShipment.dspProjectID;
			dataRow["dspShipContactID"] = dMRShipment.dspShipContactID;
			DataRow dataRow4 = dataRow;
			dspClosedDate = dMRShipment.dspShipDate;
			dataRow4["dspShipDate"] = (dspClosedDate.HasValue ? ((object)dspClosedDate.GetValueOrDefault()) : dataRow["dspShipDate"]);
			dataRow["dspShipLocationID"] = dMRShipment.dspShipLocationID;
			dataRow["dspShippingCommentsRTF"] = dMRShipment.dspShippingCommentsRTF ?? dataRow["dspShippingCommentsRTF"];
			dataRow["dspShippingCommentsText"] = dMRShipment.dspShippingCommentsText ?? dataRow["dspShippingCommentsText"];
			dataRow["dspShippingMethodID"] = dMRShipment.dspShippingMethodID;
			dataRow["dspShippingPaymentTypeID"] = dMRShipment.dspShippingPaymentTypeID;
			dataRow["dspStandardMessageID"] = dMRShipment.dspStandardMessageID;
			dataRow["dspSupplierOrganizationID"] = dMRShipment.dspSupplierOrganizationID;
			dataRow["dspTrackingNumber"] = dMRShipment.dspTrackingNumber;
			if (dMRShipment.CustomFields != null && dMRShipment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in dMRShipment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DMRShipment [{dMRShipment.dspUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DMRShipment [{dMRShipment.dspUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
