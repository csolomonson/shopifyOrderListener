using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core.Transaction;

/// <summary>
/// Implementation of the receipt repository, implementing the <see cref="T:M1.API.Repositories.Core.Transaction.IReceiptRepository" /> interface.
/// </summary>
public class ReceiptRepository : APIBaseRepository, IReceiptRepository, IAPIBaseRepository, IDisposable
{
	private readonly string SELECT_RECEIPT_LINES = "SELECT rmlReceiptLineID, rmlReceiptID, rmlPurchaseOrderID, rmlPurchaseOrderLineID, \r\n                                                                rmlJobID, rmlJobAssemblyID, rmlJobType, rmlJobMaterialID, rmlJobOperationID, rmlPartID, rmlPartRevisionID,\r\n                                                                rmlOrgPartID, rmlOrgPartShortDescription, rmlDescription, rmlPartWarehouseLocationID,\r\n                                                                rmlPartBinID, rmlPurchaseQuantityReceived, rmlPurchaseUnitOfMeasure, rmlPurchaseUnitCost,\r\n                                                                rmlSetupCharge, rmlConversionFactor, rmlInventoryUnitOfMeasure, rmlInventoryUnitCost,\r\n                                                                rmlPoReceivedComplete, rmlJobReceivedComplete, rmlRequiresInspection, rmlReference,\r\n                                                                rmlHeatLot, rmlProjectID, rmlProjectAreaID, rmlClosed, rmlPostedToGl, rmlReversed, rmlReverseReceiptID,\r\n                                                                rmlReverseReceiptLineID, rmlCreatedBy, rmlCreatedDate, rmlUniqueID, rmlRowVersion,\r\n                                                                rmlJobOprQuantityReceived, rmlJobMatQuantityReceived\r\n                                                    FROM ReceiptLines\r\n                                                    WHERE (rmlReceiptID = @ReceiptID)";

	private readonly string[] receiptFields = new string[27]
	{
		"rmpReceiptID", "rmpPlantDepartmentID", "rmpPlantID", "rmpReceiptDate", "rmpDeliveryDocket", "rmpSupplierOrganizationID", "rmpPurchaseLocationID", "rmpAPInvoiceLocationID", "rmpShippingMethodID", "rmpReceiptSubtotal",
		"rmpFreightCharge", "rmpProjectID", "rmpReceiptTotal", "rmpCurrencyRateID", "rmpExchangeRate", "rmpCustomRate", "rmpReversalEntry", "rmpReversed", "rmpPostedToGL", "rmpPostedDate",
		"rmpCreatedBy", "rmpCreatedDate", "rmpClosed", "rmpClosedDate", "rmpNestlinkProcessed", "rmpUniqueID", "rmpRowVersion"
	};

	public ReceiptRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public ReceiptRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesReceiptExists(string receiptId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmpReceiptID|C", receiptId);
		base.selectList.Add("rmpReceiptID");
		return Task.FromResult(GetAsObject("Receipts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ReceiptInformationDto>> GetAllReceipts(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<ReceiptInformationDto> collection = new List<ReceiptInformationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(receiptFields);
		List<string> orderbyList = new List<string> { "rmpReceiptID" };
		using (DataTable dataTable = GetAsDataTable("Receipts", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ReceiptInformationDto receiptInformationDto = new ReceiptInformationDto();
				receiptInformationDto.ReceiptID = dataTable.Rows[i].Field<string>("rmpReceiptID").ToString().Trim();
				receiptInformationDto.PlantID = dataTable.Rows[i].Field<string>("rmpPlantID").ToString().Trim();
				receiptInformationDto.PlantDepartmentID = dataTable.Rows[i].Field<string>("rmpPlantDepartmentID").ToString().Trim();
				receiptInformationDto.ReceiptDate = dataTable.Rows[i].Field<DateTime?>("rmpReceiptDate");
				receiptInformationDto.DeliveryDocket = dataTable.Rows[i].Field<string>("rmpDeliveryDocket").ToString().Trim();
				receiptInformationDto.SupplierOrganizationID = dataTable.Rows[i].Field<string>("rmpSupplierOrganizationID").ToString().Trim();
				receiptInformationDto.PurchaseLocationID = dataTable.Rows[i].Field<string>("rmpPurchaseLocationID").ToString().Trim();
				receiptInformationDto.ApInvoiceLocationID = dataTable.Rows[i].Field<string>("rmpApInvoiceLocationID").ToString().Trim();
				receiptInformationDto.ShippingMethodID = dataTable.Rows[i].Field<string>("rmpShippingMethodID").ToString().Trim();
				receiptInformationDto.ReceiptSubtotal = Convert.ToDecimal(dataTable.Rows[i].Field<decimal>("rmpReceiptSubtotal").ToString());
				receiptInformationDto.FreightCharge = Convert.ToDecimal(dataTable.Rows[i].Field<decimal>("rmpFreightCharge").ToString());
				receiptInformationDto.ReceiptTotal = Convert.ToDecimal(dataTable.Rows[i].Field<decimal>("rmpReceiptTotal").ToString());
				receiptInformationDto.ProjectID = dataTable.Rows[i].Field<string>("rmpProjectID").ToString().Trim();
				receiptInformationDto.CurrencyRateID = dataTable.Rows[i].Field<string>("rmpCurrencyRateID").ToString().Trim();
				receiptInformationDto.ExchangeRate = Convert.ToDecimal(dataTable.Rows[i].Field<decimal>("rmpExchangeRate").ToString());
				receiptInformationDto.CustomRate = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["rmpCustomRate"]));
				receiptInformationDto.ReversalEntry = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["rmpReversalEntry"]));
				receiptInformationDto.Reversed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["rmpReversed"]));
				receiptInformationDto.PostedToGl = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["rmpPostedToGl"]));
				receiptInformationDto.PostedDate = dataTable.Rows[i].Field<DateTime?>("rmpPostedDate");
				receiptInformationDto.CreatedBy = dataTable.Rows[i].Field<string>("rmpCreatedBy").ToString().Trim();
				receiptInformationDto.CreatedDate = (DateTime.TryParse(dataTable.Rows[i]["rmpCreatedDate"]?.ToString(), out var result) ? result : DateTime.MinValue);
				receiptInformationDto.Closed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["rmpClosed"]));
				receiptInformationDto.ClosedDate = dataTable.Rows[i].Field<DateTime?>("rmpClosedDate");
				receiptInformationDto.UniqueID = dataTable.Rows[i].Field<Guid>("rmpUniqueID");
				receiptInformationDto.RowVersion = dataTable.Rows[i].Field<byte[]>("rmpRowVersion");
				receiptInformationDto.NestlinkProcessed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[i]["rmpNestlinkProcessed"]));
				collection.Add(receiptInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ReceiptInformationDto> GetReceiptInfo(string receiptId)
	{
		ReceiptInformationDto receiptInformationDto = new ReceiptInformationDto();
		InitializeParameterLists();
		base.selectList.AddRange(receiptFields);
		base.filterList.Add("rmpReceiptID|C", receiptId);
		using (DataTable dataTable = GetAsDataTable("Receipts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(receiptInformationDto);
			}
			receiptInformationDto.ReceiptID = dataTable.Rows[0].Field<string>("rmpReceiptID").ToString().Trim();
			receiptInformationDto.PlantID = dataTable.Rows[0].Field<string>("rmpPlantID").ToString().Trim();
			receiptInformationDto.PlantDepartmentID = dataTable.Rows[0].Field<string>("rmpPlantDepartmentID").ToString().Trim();
			receiptInformationDto.ReceiptDate = dataTable.Rows[0].Field<DateTime?>("rmpReceiptDate");
			receiptInformationDto.DeliveryDocket = dataTable.Rows[0].Field<string>("rmpDeliveryDocket").ToString().Trim();
			receiptInformationDto.SupplierOrganizationID = dataTable.Rows[0].Field<string>("rmpSupplierOrganizationID").ToString().Trim();
			receiptInformationDto.PurchaseLocationID = dataTable.Rows[0].Field<string>("rmpPurchaseLocationID").ToString().Trim();
			receiptInformationDto.ApInvoiceLocationID = dataTable.Rows[0].Field<string>("rmpApInvoiceLocationID").ToString().Trim();
			receiptInformationDto.ShippingMethodID = dataTable.Rows[0].Field<string>("rmpShippingMethodID").ToString().Trim();
			receiptInformationDto.ReceiptSubtotal = Convert.ToDecimal(dataTable.Rows[0].Field<decimal>("rmpReceiptSubtotal").ToString());
			receiptInformationDto.FreightCharge = Convert.ToDecimal(dataTable.Rows[0].Field<decimal>("rmpFreightCharge").ToString());
			receiptInformationDto.ReceiptTotal = Convert.ToDecimal(dataTable.Rows[0].Field<decimal>("rmpReceiptTotal").ToString());
			receiptInformationDto.ProjectID = dataTable.Rows[0].Field<string>("rmpProjectID").ToString().Trim();
			receiptInformationDto.CurrencyRateID = dataTable.Rows[0].Field<string>("rmpCurrencyRateID").ToString().Trim();
			receiptInformationDto.ExchangeRate = Convert.ToDecimal(dataTable.Rows[0].Field<decimal>("rmpExchangeRate").ToString());
			receiptInformationDto.CustomRate = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["rmpCustomRate"]));
			receiptInformationDto.ReversalEntry = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["rmpReversalEntry"]));
			receiptInformationDto.Reversed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["rmpReversed"]));
			receiptInformationDto.PostedToGl = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["rmpPostedToGl"]));
			receiptInformationDto.PostedDate = dataTable.Rows[0].Field<DateTime?>("rmpPostedDate");
			receiptInformationDto.CreatedBy = dataTable.Rows[0].Field<string>("rmpCreatedBy").ToString().Trim();
			receiptInformationDto.CreatedDate = (DateTime.TryParse(dataTable.Rows[0]["rmpCreatedDate"]?.ToString(), out var result) ? result : DateTime.MinValue);
			receiptInformationDto.Closed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["rmpClosed"]));
			receiptInformationDto.ClosedDate = dataTable.Rows[0].Field<DateTime?>("rmpClosedDate");
			receiptInformationDto.UniqueID = dataTable.Rows[0].Field<Guid>("rmpUniqueID");
			receiptInformationDto.RowVersion = dataTable.Rows[0].Field<byte[]>("rmpRowVersion");
			receiptInformationDto.NestlinkProcessed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["rmpNestlinkProcessed"]));
		}
		return Task.FromResult(receiptInformationDto);
	}

	public Task<IList<ReceiptLineInformationDto>> GetReceiptLineInfo(string receiptId)
	{
		IList<ReceiptLineInformationDto> list = new List<ReceiptLineInformationDto>();
		InitializeParameterLists();
		base.filterList.Add("@ReceiptID", receiptId);
		using (DataTable dataTable = GetAsDataTable(SELECT_RECEIPT_LINES, base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				ReceiptLineInformationDto item = new ReceiptLineInformationDto
				{
					ReceiptLineID = row.Field<short>("rmlReceiptLineID"),
					ReceiptID = row.Field<string>("rmlReceiptID").ToString().Trim(),
					PurchaseOrderID = row.Field<string>("rmlPurchaseOrderID").ToString().Trim(),
					PurchaseOrderLineID = row.Field<short>("rmlPurchaseOrderLineID"),
					JobID = row.Field<string>("rmlJobID").ToString().Trim(),
					JobAssemblyID = row.Field<int>("rmlJobAssemblyID"),
					JobType = row.Field<byte>("rmlJobType"),
					JobMaterialID = row.Field<int>("rmlJobMaterialID"),
					JobOperationID = row.Field<int>("rmlJobOperationID"),
					PartID = row.Field<string>("rmlPartID").ToString().Trim(),
					PartRevisionID = row.Field<string>("rmlPartRevisionID").ToString().Trim(),
					OrgPartID = row.Field<string>("rmlOrgPartID").ToString().Trim(),
					OrgPartShortDescription = row.Field<string>("rmlOrgPartShortDescription").ToString().Trim(),
					Description = row.Field<string>("rmlDescription").ToString().Trim(),
					PartWarehouseLocationID = row.Field<string>("rmlPartWarehouseLocationID").ToString().Trim(),
					PartBinID = row.Field<string>("rmlPartBinID").ToString().Trim(),
					PurchaseQuantityReceived = row.Field<decimal>("rmlPurchaseQuantityReceived"),
					PurchaseUnitOfMeasure = row.Field<string>("rmlPurchaseUnitOfMeasure").ToString().Trim(),
					PurchaseUnitCost = row.Field<decimal>("rmlPurchaseUnitCost"),
					SetupCharge = row.Field<decimal>("rmlSetupCharge"),
					ConversionFactor = row.Field<decimal>("rmlConversionFactor"),
					InventoryUnitOfMeasure = row.Field<string>("rmlInventoryUnitOfMeasure").ToString().Trim(),
					InventoryUnitCost = row.Field<decimal>("rmlInventoryUnitCost"),
					PoReceivedComplete = Convert.ToBoolean(Convert.ToInt16(row["rmlPoReceivedComplete"])),
					JobReceivedComplete = Convert.ToBoolean(Convert.ToInt16(row["rmlJobReceivedComplete"])),
					RequiresInspection = Convert.ToBoolean(Convert.ToInt16(row["rmlRequiresInspection"])),
					Reference = row.Field<string>("rmlReference").ToString().Trim(),
					HeatLot = row.Field<string>("rmlHeatLot").ToString().Trim(),
					ProjectID = row.Field<string>("rmlProjectID").ToString().Trim(),
					ProjectAreaID = row.Field<string>("rmlProjectAreaID").ToString().Trim(),
					Closed = Convert.ToBoolean(Convert.ToInt16(row["rmlClosed"])),
					PostedToGl = Convert.ToBoolean(Convert.ToInt16(row["rmlPostedToGl"])),
					Reversed = Convert.ToBoolean(Convert.ToInt16(row["rmlReversed"])),
					ReverseReceiptID = row.Field<string>("rmlReverseReceiptID").ToString().Trim(),
					ReverseReceiptLineID = row.Field<short>("rmlReverseReceiptLineID"),
					CreatedBy = row.Field<string>("rmlCreatedBy").ToString().Trim(),
					CreatedDate = (row.Field<DateTime?>("rmlCreatedDate") ?? DateTime.Parse("01/01/1900")),
					UniqueID = row.Field<Guid>("rmlUniqueID"),
					RowVersion = row.Field<byte[]>("rmlRowVersion"),
					JobMatQuantityReceived = row.Field<decimal>("rmlJobMatQuantityReceived"),
					JobOprQuantityReceived = row.Field<decimal>("rmlJobOprQuantityReceived")
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}
}
