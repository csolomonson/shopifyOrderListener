using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.API.Repositories.Core;
using M1.API.Repositories.EDI;
using M1.API.Utilities;

namespace M1.API.Models.EDI;

public class EDIInvoiceModel : EDIBaseModel, IEDIInvoiceModel, IEDIBaseModel, IAPIBaseModel, IDisposable
{
	public IEDIInvoiceRepository EdiInvoiceRepository { get; set; }

	private Task<SalesOrderDto> GetSalesOrderDateAndMoreData(string salesOrderId)
	{
		SalesOrderDto result = new SalesOrderDto();
		if (!string.IsNullOrEmpty(salesOrderId))
		{
			result = base.SalesOrderRepository.GetSalesOrderInfor(salesOrderId, headerOnly: true).Result;
		}
		return Task.FromResult(result);
	}

	private Task<List<EDI810OutboundInvoice>> GetPendingEDITransferInvoices_ForAllCustomers()
	{
		List<EDI810OutboundInvoice> list = new List<EDI810OutboundInvoice>();
		List<string> list2 = new List<string>();
		EDI810OutboundInvoice eDI810OutboundInvoice = null;
		list2 = EdiInvoiceRepository.GetInvoices_PendingEDITransfer_ForAllCustomers().Result.ToList();
		if (list2.Count() > 0)
		{
			foreach (string item in list2)
			{
				eDI810OutboundInvoice = Create810InvoiceObject(item).Result;
				list.Add(eDI810OutboundInvoice);
			}
		}
		return Task.FromResult(list);
	}

	public EDIInvoiceModel(APIClientContext clientContext)
		: base(clientContext)
	{
		base.SalesOrderRepository = new SalesOrderRepository(clientContext);
		EdiInvoiceRepository = new EDIInvoiceRepository(clientContext);
		base.OrganizationRepository = new OrganizationRepository(clientContext);
	}

	public Task<EDI810OutboundInvoice> Create810InvoiceObject(string invoiceId)
	{
		EDI810OutboundInvoice result = new EDI810OutboundInvoice();
		List<EDI810OutboundInvoiceLinesDto> invoiceLineList = null;
		List<EDI810OutboundInvoiceSACLineDto> invoiceSACLineList = null;
		using (DataTable source = EdiInvoiceRepository.GetInvoiceLineInfo(invoiceId).Result)
		{
			invoiceSACLineList = (from invLine in source.AsEnumerable()
				where invLine.Field<decimal>("arlExtendedDiscountForeign") > 0m
				select new EDI810OutboundInvoiceSACLineDto
				{
					AC_Indicator = APIEnums.XML810SACIndicatorTypes.A.ToString().Trim(),
					AC_Code = "DISC",
					AC_Amount = invLine.Field<decimal>("arlExtendedDiscountForeign"),
					InvoiceNumber = invoiceId,
					InvoiceLineID = invLine.Field<short>("arlARInvoiceLineID"),
					SalesOrderLineID = invLine.Field<short>("arlSalesOrderLineID"),
					SalesOrderDeliveryID = invLine.Field<short>("arlSalesOrderDeliveryID")
				}).ToList();
			invoiceSACLineList.AddRange((from invLine in source.AsEnumerable()
				where invLine.Field<decimal>("arlTaxAmountForeign") > 0m
				select new EDI810OutboundInvoiceSACLineDto
				{
					AC_Indicator = APIEnums.XML810SACIndicatorTypes.C.ToString().Trim(),
					AC_Code = "TAX",
					AC_Amount = invLine.Field<decimal>("arlTaxAmountForeign"),
					InvoiceNumber = invoiceId,
					InvoiceLineID = invLine.Field<short>("arlARInvoiceLineID"),
					SalesOrderLineID = invLine.Field<short>("arlSalesOrderLineID"),
					SalesOrderDeliveryID = invLine.Field<short>("arlSalesOrderDeliveryID")
				}).ToList());
			invoiceSACLineList.AddRange((from invLine in source.AsEnumerable()
				where invLine.Field<decimal>("arlFreightAmountForeign") > 0m
				select new EDI810OutboundInvoiceSACLineDto
				{
					AC_Indicator = APIEnums.XML810SACIndicatorTypes.C.ToString().Trim(),
					AC_Code = "FRE",
					AC_Amount = invLine.Field<decimal>("arlFreightAmountForeign"),
					InvoiceNumber = invoiceId,
					InvoiceLineID = invLine.Field<short>("arlARInvoiceLineID"),
					SalesOrderLineID = invLine.Field<short>("arlSalesOrderLineID"),
					SalesOrderDeliveryID = invLine.Field<short>("arlSalesOrderDeliveryID")
				}).ToList());
			invoiceSACLineList.AddRange((from invLine in source.AsEnumerable()
				where invLine.Field<decimal>("arlSecondTaxAmountForeign") > 0m
				select new EDI810OutboundInvoiceSACLineDto
				{
					AC_Indicator = APIEnums.XML810SACIndicatorTypes.C.ToString().Trim(),
					AC_Code = "TAX2",
					AC_Amount = invLine.Field<decimal>("arlSecondTaxAmountForeign"),
					InvoiceNumber = invoiceId,
					InvoiceLineID = invLine.Field<short>("arlARInvoiceLineID"),
					SalesOrderLineID = invLine.Field<short>("arlSalesOrderLineID"),
					SalesOrderDeliveryID = invLine.Field<short>("arlSalesOrderDeliveryID")
				}).ToList());
			invoiceLineList = (from invLine in source.AsEnumerable()
				let orderH = GetSalesOrderDateAndMoreData(invLine.Field<string>("arlSalesOrderID")).Result
				let shipInfo = EdiInvoiceRepository.GetShipmentDate(invLine.Field<string>("arlShipmentID")).Result
				let orderDate = orderH?.OrderDate
				let ediCreatedSalesOrder = orderH?.CreatedByEDI
				let invoiceLineID = invLine.Field<short>("arlARInvoiceLineID")
				let shipDate = shipInfo?.ShipDate
				let shipTrackNo = shipInfo?.TrackingNo
				let soLineReleaseNo = EdiInvoiceRepository.GetSalesOrderLineReleaseNo(invLine.Field<string>("arlSalesOrderID"), invLine.Field<short>("arlSalesOrderLineID")).Result
				select new EDI810OutboundInvoiceLinesDto
				{
					InvoiceNumber = invoiceId.Trim(),
					InvoiceLineID = invoiceLineID,
					SalesOrderID = invLine.Field<string>("arlSalesOrderID").Trim(),
					CustomerPO = invLine.Field<string>("arlCustomerPO").Trim(),
					OrderDate = orderDate.Value.ToString("dd/MM/yyyy"),
					EDICreatedSalesOrder = ediCreatedSalesOrder.Value,
					SalesOrderLineID = invLine.Field<short>("arlSalesOrderLineID"),
					SalesOrderDeliveryID = invLine.Field<short>("arlSalesOrderDeliveryID"),
					VendorItemNo = invLine.Field<string>("arlPartID").Trim(),
					PartID = invLine.Field<string>("arlPartID").Trim(),
					OrgPartID = invLine.Field<string>("arlOrgPartID").Trim(),
					PartShortDescription = invLine.Field<string>("arlPartShortDescription").Trim(),
					OrderQuantity = invLine.Field<decimal>("arlInvoiceQuantity"),
					UnitOfMeasure = invLine.Field<string>("arlUnitOfMeasure").Trim(),
					ItemPrice = invLine.Field<decimal>("arlFullUnitPriceForeign"),
					FullUnitPriceForeign = invLine.Field<decimal>("arlFullUnitPriceForeign"),
					ExtendedDiscountForeign = invLine.Field<decimal>("arlExtendedDiscountForeign"),
					UnitPriceForeign = invLine.Field<decimal>("arlUnitPriceForeign"),
					FullExtendedPriceForeign = invLine.Field<decimal>("arlFullExtendedPriceForeign"),
					ExtendedPriceForeign = invLine.Field<decimal>("arlExtendedPriceForeign"),
					TaxAmountForeign = invLine.Field<decimal>("arlTaxAmountForeign"),
					FreightAmountForeign = invLine.Field<decimal>("arlFreightAmountForeign"),
					ShipmentID = invLine.Field<string>("arlShipmentID").Trim(),
					ShipmentLineID = invLine.Field<short>("arlShipmentLineID"),
					ShipDate = (shipDate ?? DateTime.Parse("01/01/1901")).ToString("dd/MM/yyyy").ToString(),
					ShipmentTrackingNumber = (shipTrackNo ?? string.Empty).Trim(),
					ReleaseNumber = soLineReleaseNo,
					EDI810SACLines = invoiceSACLineList.Where((EDI810OutboundInvoiceSACLineDto x) => x.InvoiceLineID == invoiceLineID).ToList()
				}).ToList();
		}
		using (DataTable source2 = EdiInvoiceRepository.GetInvoiceHeaderInfo(invoiceId).Result)
		{
			result = (from invH in source2.AsEnumerable()
				let BillOrganization = GetOrganizationNameAndAddress_ForLocationId(base.ApiClientContext.Database, invH.Field<string>("arpCustomerOrganizationID"), invH.Field<string>("arpARInvoiceLocationID"), billToLocation: true, shipToLocation: false).Result
				let ShipOrganization = GetOrganizationNameAndAddress_ForLocationId(base.ApiClientContext.Database, invH.Field<string>("arpShipOrganizationID"), invH.Field<string>("arpShipLocationID"), billToLocation: false, shipToLocation: true).Result
				let ShipFromOrganization = GetShipFromOrganizationNameAndAddress(base.OrganizationRepository, invH.Field<string>("arpPlantID")).Result
				let dueDate = invH.Field<DateTime>("arpDueDate")
				let ShipLocation = ChangeLocationContact(ShipOrganization, invH.Field<string>("arpShipContactID")).Result
				let ARLocation = ChangeLocationContact(BillOrganization, invH.Field<string>("arpARInvoiceContactID")).Result
				select new EDI810OutboundInvoice
				{
					InvoiceNumber = invoiceId.Trim(),
					InvoiceDate = invH.Field<DateTime>("arpInvoiceDate").ToString("dd/MM/yyyy"),
					InvoiceType = invH.Field<byte>("arpInvoiceType"),
					CustomerOrganizationID = invH.Field<string>("arpCustomerOrganizationID").Trim(),
					ShipToLocation = ShipLocation,
					BillToLocation = ARLocation,
					ShipFromLocation = ShipFromOrganization,
					PaymentTerm = invH.Field<string>("xatDescription").Trim(),
					ShippingMethod = invH.Field<string>("xasDescription").Trim(),
					FreeOnBoardDescription = invH.Field<string>("arpFreeOnBoardDescription").Trim(),
					DueDate = dueDate.ToString("dd/MM/yyyy"),
					InvoiceBalanceForeign = invH.Field<decimal>("arpInvoiceBalanceForeign"),
					FreightAmountForeign = invH.Field<decimal>("arpFreightAmountForeign"),
					CurrencyCode = invH.Field<string>("arpCurrencyRateID").Trim(),
					NumberOfLineItems = invoiceLineList.Count(),
					TotalQuantity = invoiceLineList.Sum((EDI810OutboundInvoiceLinesDto x) => x.OrderQuantity),
					FinalInvAmt = invH.Field<decimal>("arpInvoiceTotalForeign"),
					FullInvoiceSubtotalForeign = invH.Field<decimal>("arpFullInvoiceSubtotalForeign"),
					DiscountTotalForeign = invH.Field<decimal>("arpDiscountTotalForeign"),
					InvoiceSubtotalForeign = invH.Field<decimal>("arpInvoiceSubtotalForeign"),
					FreightTotalForeign = invH.Field<decimal>("arpFreightTotalForeign"),
					FreightTaxAmountForeign = invH.Field<decimal>("arpFreightTaxAmountForeign"),
					InvoiceTaxAmountForeign = invH.Field<decimal>("arpInvoiceTaxAmountForeign"),
					InvoiceTotalForeign = invH.Field<decimal>("arpInvoiceTotalForeign"),
					EDI810InvoiceLines = invoiceLineList
				}).SingleOrDefault();
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> ValidateRequest_SetEDIFlag(EDI810InvoicesIN ediInvoices)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			if (ediInvoices.EDI810InvoiceSet.Count == 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
				base.ErrorsList.Add("No records found in the request or invalid format.");
			}
			else
			{
				foreach (EDI810InvoiceIN item in ediInvoices.EDI810InvoiceSet)
				{
					if (!EdiInvoiceRepository.DoesInvoiceExists(item.InvoiceNumber).Result)
					{
						httpValidationStatusCode = HttpStatusCode.OK;
						base.ErrorsList.Add("AR Invoice " + item.InvoiceNumber + " is invalid.");
					}
					else if (!EdiInvoiceRepository.IsEdiInvoice(item.InvoiceNumber).Result)
					{
						httpValidationStatusCode = HttpStatusCode.OK;
						base.ErrorsList.Add("AR Invoice " + item.InvoiceNumber + " is not an EDI invoice.");
					}
				}
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating inbound invoice numbers.");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<APIValidationInfoDto> Process_SetEDIFlag(EDI810InvoicesIN ediInvoices)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		SqlTransaction sqlTransaction = null;
		try
		{
			sqlTransaction = EdiInvoiceRepository.M1database.BeginTransaction();
			Dictionary<string, bool> invoiceDictionary = ediInvoices.EDI810InvoiceSet.ToDictionary((EDI810InvoiceIN e) => e.InvoiceNumber, (EDI810InvoiceIN e) => e.EDIUpdateStatus);
			if (EdiInvoiceRepository.UpdateEdiFlag(invoiceDictionary, sqlTransaction).Result)
			{
				sqlTransaction.Commit();
			}
			else
			{
				sqlTransaction.Rollback();
				base.ErrorsList.Add("Error occurred while updating inbound invoice numbers.");
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while updating inbound invoice numbers.");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<EDI810InvoiceCollectionDto> Process_AllUnmapped(int page, int pagesize)
	{
		EDI810InvoiceCollectionDto eDI810InvoiceCollectionDto = new EDI810InvoiceCollectionDto();
		eDI810InvoiceCollectionDto.EDI810InvoiceSet = GetPendingEDITransferInvoices_ForAllCustomers().Result;
		if (eDI810InvoiceCollectionDto.EDI810InvoiceSet.Count > 0)
		{
			int num = eDI810InvoiceCollectionDto.EDI810InvoiceSet.Count();
			int totalPages = (int)Math.Ceiling((double)num / (double)pagesize);
			int num2 = page - 1;
			num2 = ((num2 >= 0) ? num2 : 0);
			eDI810InvoiceCollectionDto.TotalRecords = num;
			List<EDI810OutboundInvoice> eDI810InvoiceSet = eDI810InvoiceCollectionDto.EDI810InvoiceSet.Skip(pagesize * num2).Take(pagesize).ToList();
			eDI810InvoiceCollectionDto.EDI810InvoiceSet = eDI810InvoiceSet;
			eDI810InvoiceCollectionDto.TotalPages = totalPages;
			eDI810InvoiceCollectionDto.PageSize = pagesize;
			eDI810InvoiceCollectionDto.CurrentPageIndex = page;
		}
		return Task.FromResult(eDI810InvoiceCollectionDto);
	}

	public override void Dispose()
	{
		base.Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (disposing)
		{
			GC.SuppressFinalize(this);
			base.SalesOrderRepository.Dispose();
			EdiInvoiceRepository.Dispose();
		}
	}
}
