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

public class ERPPurchaseOrderDeliveryRepository : APIBaseRepository, IERPPurchaseOrderDeliveryRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderDeliveryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderDeliveryExist(Guid purchaseOrderDeliveryId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmdUniqueID|C", purchaseOrderDeliveryId);
		base.selectList.Add("pmdUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrderDeliveries", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderDeliveryInformationDto>> GetAllPurchaseOrderDeliveries(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderDeliveryInformationDto> collection = new List<ERPPurchaseOrderDeliveryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[26]
		{
			"pmdContactID", "pmdCreatedBy", "pmdCreatedDate", "pmdDeliveryDate", "pmdDeliveryQuantity", "pmdDeliveryType", "pmdUniqueID", "pmdClosed", "pmdInTransit", "pmdInvoicedComplete",
			"pmdReceivedComplete", "pmdJobAssemblyID", "pmdJobID", "pmdJobMaterialID", "pmdJobOperationID", "pmdJobType", "pmdLocationID", "pmdOrganizationID", "pmdPurchaseOrderID", "pmdPurchaseOrderLineID",
			"pmdQuantityInvoiced", "pmdQuantityReceived", "pmdRowVersion", "pmdPurchaseOrderDeliveryID", "pmdShippingMethodID", "pmdTrackingNumber"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrderDeliveries");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderDeliveries", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderDeliveryInformationDto eRPPurchaseOrderDeliveryInformationDto = new ERPPurchaseOrderDeliveryInformationDto();
				eRPPurchaseOrderDeliveryInformationDto.pmdContactID = dataTable.Rows[i].Field<string>("pmdContactID");
				eRPPurchaseOrderDeliveryInformationDto.pmdCreatedBy = dataTable.Rows[i].Field<string>("pmdCreatedBy");
				eRPPurchaseOrderDeliveryInformationDto.pmdCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmdCreatedDate");
				eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryDate = dataTable.Rows[i].Field<DateTime?>("pmdDeliveryDate");
				eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryQuantity = dataTable.Rows[i].Field<decimal>("pmdDeliveryQuantity");
				eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryType = dataTable.Rows[i].Field<byte>("pmdDeliveryType");
				eRPPurchaseOrderDeliveryInformationDto.pmdUniqueID = dataTable.Rows[i].Field<Guid>("pmdUniqueID");
				eRPPurchaseOrderDeliveryInformationDto.pmdClosed = dataTable.Rows[i].Field<bool>("pmdClosed");
				eRPPurchaseOrderDeliveryInformationDto.pmdInTransit = dataTable.Rows[i].Field<bool>("pmdInTransit");
				eRPPurchaseOrderDeliveryInformationDto.pmdInvoicedComplete = dataTable.Rows[i].Field<bool>("pmdInvoicedComplete");
				eRPPurchaseOrderDeliveryInformationDto.pmdReceivedComplete = dataTable.Rows[i].Field<bool>("pmdReceivedComplete");
				eRPPurchaseOrderDeliveryInformationDto.pmdJobAssemblyID = dataTable.Rows[i].Field<int>("pmdJobAssemblyID");
				eRPPurchaseOrderDeliveryInformationDto.pmdJobID = dataTable.Rows[i].Field<string>("pmdJobID");
				eRPPurchaseOrderDeliveryInformationDto.pmdJobMaterialID = dataTable.Rows[i].Field<int>("pmdJobMaterialID");
				eRPPurchaseOrderDeliveryInformationDto.pmdJobOperationID = dataTable.Rows[i].Field<int>("pmdJobOperationID");
				eRPPurchaseOrderDeliveryInformationDto.pmdJobType = dataTable.Rows[i].Field<byte>("pmdJobType");
				eRPPurchaseOrderDeliveryInformationDto.pmdLocationID = dataTable.Rows[i].Field<string>("pmdLocationID");
				eRPPurchaseOrderDeliveryInformationDto.pmdOrganizationID = dataTable.Rows[i].Field<string>("pmdOrganizationID");
				eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderID = dataTable.Rows[i].Field<string>("pmdPurchaseOrderID");
				eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderLineID = dataTable.Rows[i].Field<short>("pmdPurchaseOrderLineID");
				eRPPurchaseOrderDeliveryInformationDto.pmdQuantityInvoiced = dataTable.Rows[i].Field<decimal>("pmdQuantityInvoiced");
				eRPPurchaseOrderDeliveryInformationDto.pmdQuantityReceived = dataTable.Rows[i].Field<decimal>("pmdQuantityReceived");
				eRPPurchaseOrderDeliveryInformationDto.pmdRowVersion = dataTable.Rows[i].Field<byte[]>("pmdRowVersion");
				eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderDeliveryID = dataTable.Rows[i].Field<short>("pmdPurchaseOrderDeliveryID");
				eRPPurchaseOrderDeliveryInformationDto.pmdShippingMethodID = dataTable.Rows[i].Field<string>("pmdShippingMethodID");
				eRPPurchaseOrderDeliveryInformationDto.pmdTrackingNumber = dataTable.Rows[i].Field<string>("pmdTrackingNumber");
				eRPPurchaseOrderDeliveryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderDeliveryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderDeliveryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderDeliveryInformationDto> GetPurchaseOrderDelivery(Guid purchaseOrderDeliveryId)
	{
		ERPPurchaseOrderDeliveryInformationDto eRPPurchaseOrderDeliveryInformationDto = new ERPPurchaseOrderDeliveryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[26]
		{
			"pmdContactID", "pmdCreatedBy", "pmdCreatedDate", "pmdDeliveryDate", "pmdDeliveryQuantity", "pmdDeliveryType", "pmdUniqueID", "pmdClosed", "pmdInTransit", "pmdInvoicedComplete",
			"pmdReceivedComplete", "pmdJobAssemblyID", "pmdJobID", "pmdJobMaterialID", "pmdJobOperationID", "pmdJobType", "pmdLocationID", "pmdOrganizationID", "pmdPurchaseOrderID", "pmdPurchaseOrderLineID",
			"pmdQuantityInvoiced", "pmdQuantityReceived", "pmdRowVersion", "pmdPurchaseOrderDeliveryID", "pmdShippingMethodID", "pmdTrackingNumber"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pmdUniqueID|C", purchaseOrderDeliveryId);
		AddCustomFieldsToSelectList("PurchaseOrderDeliveries");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderDeliveries", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderDeliveryInformationDto);
			}
			eRPPurchaseOrderDeliveryInformationDto.pmdContactID = dataTable.Rows[0].Field<string>("pmdContactID");
			eRPPurchaseOrderDeliveryInformationDto.pmdCreatedBy = dataTable.Rows[0].Field<string>("pmdCreatedBy");
			eRPPurchaseOrderDeliveryInformationDto.pmdCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmdCreatedDate");
			eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryDate = dataTable.Rows[0].Field<DateTime?>("pmdDeliveryDate");
			eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryQuantity = dataTable.Rows[0].Field<decimal>("pmdDeliveryQuantity");
			eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryType = dataTable.Rows[0].Field<byte>("pmdDeliveryType");
			eRPPurchaseOrderDeliveryInformationDto.pmdUniqueID = dataTable.Rows[0].Field<Guid>("pmdUniqueID");
			eRPPurchaseOrderDeliveryInformationDto.pmdClosed = dataTable.Rows[0].Field<bool>("pmdClosed");
			eRPPurchaseOrderDeliveryInformationDto.pmdInTransit = dataTable.Rows[0].Field<bool>("pmdInTransit");
			eRPPurchaseOrderDeliveryInformationDto.pmdInvoicedComplete = dataTable.Rows[0].Field<bool>("pmdInvoicedComplete");
			eRPPurchaseOrderDeliveryInformationDto.pmdReceivedComplete = dataTable.Rows[0].Field<bool>("pmdReceivedComplete");
			eRPPurchaseOrderDeliveryInformationDto.pmdJobAssemblyID = dataTable.Rows[0].Field<int>("pmdJobAssemblyID");
			eRPPurchaseOrderDeliveryInformationDto.pmdJobID = dataTable.Rows[0].Field<string>("pmdJobID");
			eRPPurchaseOrderDeliveryInformationDto.pmdJobMaterialID = dataTable.Rows[0].Field<int>("pmdJobMaterialID");
			eRPPurchaseOrderDeliveryInformationDto.pmdJobOperationID = dataTable.Rows[0].Field<int>("pmdJobOperationID");
			eRPPurchaseOrderDeliveryInformationDto.pmdJobType = dataTable.Rows[0].Field<byte>("pmdJobType");
			eRPPurchaseOrderDeliveryInformationDto.pmdLocationID = dataTable.Rows[0].Field<string>("pmdLocationID");
			eRPPurchaseOrderDeliveryInformationDto.pmdOrganizationID = dataTable.Rows[0].Field<string>("pmdOrganizationID");
			eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderID = dataTable.Rows[0].Field<string>("pmdPurchaseOrderID");
			eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderLineID = dataTable.Rows[0].Field<short>("pmdPurchaseOrderLineID");
			eRPPurchaseOrderDeliveryInformationDto.pmdQuantityInvoiced = dataTable.Rows[0].Field<decimal>("pmdQuantityInvoiced");
			eRPPurchaseOrderDeliveryInformationDto.pmdQuantityReceived = dataTable.Rows[0].Field<decimal>("pmdQuantityReceived");
			eRPPurchaseOrderDeliveryInformationDto.pmdRowVersion = dataTable.Rows[0].Field<byte[]>("pmdRowVersion");
			eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderDeliveryID = dataTable.Rows[0].Field<short>("pmdPurchaseOrderDeliveryID");
			eRPPurchaseOrderDeliveryInformationDto.pmdShippingMethodID = dataTable.Rows[0].Field<string>("pmdShippingMethodID");
			eRPPurchaseOrderDeliveryInformationDto.pmdTrackingNumber = dataTable.Rows[0].Field<string>("pmdTrackingNumber");
			eRPPurchaseOrderDeliveryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderDeliveryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderDeliveryInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrderDelivery(ERPPurchaseOrderDeliveryDto purchaseOrderDelivery)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrderDeliveries WHERE pmdUniqueID = " + M1Util.ConvertToLinq(purchaseOrderDelivery.pmdUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmdPurchaseOrderID"] = purchaseOrderDelivery.pmdPurchaseOrderID.ToUpper();
				dataRow["pmdPurchaseOrderLineID"] = purchaseOrderDelivery.pmdPurchaseOrderLineID;
				dataRow["pmdPurchaseOrderDeliveryID"] = purchaseOrderDelivery.pmdPurchaseOrderDeliveryID;
				purchaseOrderDelivery.pmdUniqueID = ((purchaseOrderDelivery.pmdUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrderDelivery.pmdUniqueID);
				dataRow["pmdUniqueID"] = purchaseOrderDelivery.pmdUniqueID;
				dataRow["pmdCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmdCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrderDelivery could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrderDelivery.pmdRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrderDelivery is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmdRowVersion"], purchaseOrderDelivery.pmdRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrderDelivery has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrderDelivery again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmdContactID"] = purchaseOrderDelivery.pmdContactID;
			DataRow dataRow2 = dataRow;
			DateTime? pmdDeliveryDate = purchaseOrderDelivery.pmdDeliveryDate;
			dataRow2["pmdDeliveryDate"] = (pmdDeliveryDate.HasValue ? ((object)pmdDeliveryDate.GetValueOrDefault()) : dataRow["pmdDeliveryDate"]);
			dataRow["pmdDeliveryQuantity"] = purchaseOrderDelivery.pmdDeliveryQuantity;
			dataRow["pmdDeliveryType"] = purchaseOrderDelivery.pmdDeliveryType;
			dataRow["pmdClosed"] = purchaseOrderDelivery.pmdClosed;
			dataRow["pmdInTransit"] = purchaseOrderDelivery.pmdInTransit;
			dataRow["pmdInvoicedComplete"] = purchaseOrderDelivery.pmdInvoicedComplete;
			dataRow["pmdReceivedComplete"] = purchaseOrderDelivery.pmdReceivedComplete;
			dataRow["pmdJobAssemblyID"] = purchaseOrderDelivery.pmdJobAssemblyID;
			dataRow["pmdJobID"] = purchaseOrderDelivery.pmdJobID;
			dataRow["pmdJobMaterialID"] = purchaseOrderDelivery.pmdJobMaterialID;
			dataRow["pmdJobOperationID"] = purchaseOrderDelivery.pmdJobOperationID;
			dataRow["pmdJobType"] = purchaseOrderDelivery.pmdJobType;
			dataRow["pmdLocationID"] = purchaseOrderDelivery.pmdLocationID;
			dataRow["pmdOrganizationID"] = purchaseOrderDelivery.pmdOrganizationID;
			dataRow["pmdQuantityInvoiced"] = purchaseOrderDelivery.pmdQuantityInvoiced;
			dataRow["pmdQuantityReceived"] = purchaseOrderDelivery.pmdQuantityReceived;
			dataRow["pmdShippingMethodID"] = purchaseOrderDelivery.pmdShippingMethodID;
			dataRow["pmdTrackingNumber"] = purchaseOrderDelivery.pmdTrackingNumber;
			if (purchaseOrderDelivery.CustomFields != null && purchaseOrderDelivery.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrderDelivery.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrderDelivery [{purchaseOrderDelivery.pmdUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrderDelivery [{purchaseOrderDelivery.pmdUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
