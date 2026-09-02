using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.EDI;
using M1.API.Repositories.Core;
using M1.API.Repositories.EDI;
using M1.API.Utilities;

namespace M1.API.Models.EDI;

public class EDIShipmentModel : EDIBaseModel, IEDIShipmentModel, IEDIBaseModel, IAPIBaseModel, IDisposable
{
	private IEDIShipmentRepository EdiShipmentRepository;

	private Task<IList<EDI856OutboundASN>> GetPendingEDITransferShipments_All()
	{
		IList<EDI856OutboundASN> list = new List<EDI856OutboundASN>();
		IList<ShipmentDto> list2 = new List<ShipmentDto>();
		list2 = EdiShipmentRepository.GetShipments_PendingEDITransfer_AllUnmapped().Result;
		if (list2.Count() > 0)
		{
			foreach (ShipmentDto item in list2)
			{
				list.Add(CreateEDI856Object(item).Result);
			}
		}
		return Task.FromResult(list);
	}

	private Task<EDI856OutboundASN> CreateEDI856Object(ShipmentDto shipment)
	{
		new EDI856OutboundASN();
		IList<EDI856ASNOutboundPackageDto> list = new List<EDI856ASNOutboundPackageDto>();
		IList<EDI856OutboundASNLineDto> list2 = new List<EDI856OutboundASNLineDto>();
		list2 = new List<EDI856OutboundASNLineDto>();
		if (shipment.ShipmentLines.Count > 0)
		{
			foreach (ShipmentLineDto shipmentLine in shipment.ShipmentLines)
			{
				SalesOrderDto result = base.SalesOrderRepository.GetSalesOrderInfor(shipmentLine.SalesOrderID, headerOnly: true).Result;
				PartRevisionInformationDto result2 = base.PartRepository.GetPartRevisionInfo(shipmentLine.PartID, shipmentLine.PartRevisionID).Result;
				SalesOrderLineDto result3 = base.SalesOrderRepository.GetSalesOrderLineInfor(shipmentLine.SalesOrderID, shipmentLine.SalesOrderLineID).Result;
				if (shipmentLine.ShipmentPackageDetails.Count > 0)
				{
					list = new List<EDI856ASNOutboundPackageDto>();
					foreach (ShipmentPackageDetailsDto shipmentPackageDetail in shipmentLine.ShipmentPackageDetails)
					{
						ShipmentPackageDto shipmentPackageDto = shipment.ShipmentPackages.Where((ShipmentPackageDto s) => s.ShipmentPackageID == shipmentPackageDetail.ShipmentPackageID).Single();
						EDI856ASNOutboundPackageDto eDI856ASNOutboundPackageDto = new EDI856ASNOutboundPackageDto
						{
							ShipmentPackageNo = shipmentPackageDetail.ShipmentPackageID,
							ShipmentPackageQuantity = shipmentPackageDetail.Quantity,
							PackageWeight = shipmentPackageDetail.Weight,
							CountryOfManufacture = shipmentPackageDetail.CountryOfManufacture,
							PackagingCode = (string.IsNullOrWhiteSpace(shipmentPackageDto.UPSPackageTypes) ? shipmentPackageDto.FedExPackageTypes : shipmentPackageDto.UPSPackageTypes),
							PackageWeightUOM = shipmentPackageDto.PackageWeightUOM,
							NumberofLoads = 1,
							LabelNumber = string.Empty,
							AdditionalNote = shipmentPackageDto.UserDefinedLabel
						};
						eDI856ASNOutboundPackageDto.SetLabelForAdditionalNoteField("hiii");
						list.Add(eDI856ASNOutboundPackageDto);
					}
				}
				list2.Add(new EDI856OutboundASNLineDto
				{
					ShipmentLineNo = shipmentLine.ShipmentLineID,
					CustomerPO = (result?.CustomerPO ?? string.Empty),
					SalesOrderID = (result?.SalesOrderID ?? string.Empty),
					SalesOrderLineID = shipmentLine.SalesOrderLineID,
					SalesOrderDeliveryID = shipmentLine.SalesOrderDeliveryID,
					ReleaseNumber = result3?.ReleaseNumber,
					OrderDate = ((result != null && result.OrderDate.Year == 1901) ? string.Empty : result?.OrderDate.ToString("dd/MM/yyyy")),
					VendorItemNo = shipmentLine.OrgPartID,
					PartID = shipmentLine.PartID,
					PartShortDescription = result2.PartShortDescription,
					Weight = shipmentLine.Weight,
					PartWeightUOM = result2.WeightUnitOfMeasure,
					PartCountryOfManufacture = result2.CountryOfManufacture,
					ShipmentQuantity = shipmentLine.QuantityShipped,
					QuantityUOM = shipmentLine.UnitOfMeasure,
					ItemPrice = shipmentLine.UnitPrice,
					EngineeringLevel = shipmentLine.PartRevisionID,
					CountryofOrigin = string.Empty,
					EDI856ASNShipmentPackages = list?.ToList()
				});
			}
		}
		EDIOrganizationLocationAddressDto result4 = GetOrganizationNameAndAddress_ForLocationId(base.ApiClientContext.Database, shipment.CustomerOrganizationID, shipment.ShipLocationID, billToLocation: false, shipToLocation: true).Result;
		EDIOrganizationLocationAddressDto result5 = ChangeLocationContact(result4, shipment.ShipContactID).Result;
		EDIOrganizationLocationAddressDto result6 = GetOrganizationNameAndAddress_ForLocationId(base.ApiClientContext.Database, shipment.CustomerOrganizationID, shipment.ARInvoiceLocationID, billToLocation: true, shipToLocation: false).Result;
		EDIOrganizationLocationAddressDto result7 = ChangeLocationContact(result6, shipment.ARInvoiceContactID).Result;
		return Task.FromResult(new EDI856OutboundASN
		{
			ShipmentName = shipment.ShipmentID,
			ShipmentNumber = shipment.ShipmentID,
			ShipmentDate = shipment.ShipDate.Value.ToString("dd/MM/yyyy"),
			CustomerOrganizationID = shipment.CustomerOrganizationID,
			ShipToLocation = result5,
			ShipFromLocation = result7,
			BillLocation = GetOrganizationNameAndAddress_ForLocationId(base.ApiClientContext.Database, shipment.CustomerOrganizationID, shipment.ARInvoiceLocationID, billToLocation: true, shipToLocation: false).Result,
			ShipmentWeight = shipment.WeightTotal,
			ShippingMethod = shipment.ShippingMethodID,
			CarrierReferenceNumber = shipment.TrackingNumber,
			CarrierCode = (string.IsNullOrWhiteSpace(shipment.ShippingMethodID) ? string.Empty : EdiShipmentRepository.GetShipmentCarrier(shipment.ShippingMethodID).Result),
			ShippingCommentsText = shipment.ShippingCommentsText,
			EDI856ASNShipmentLines = list2.ToList(),
			NumberOfLineItems = list2.Count()
		});
	}

	public EDIShipmentModel(APIClientContext clientContext)
		: base(clientContext)
	{
		EdiShipmentRepository = new EDIShipmentRepository(clientContext);
		base.SalesOrderRepository = new SalesOrderRepository(clientContext);
		base.PartRepository = new PartRepository(clientContext);
	}

	public Task<EDI856ASNCollectionDto> Process_AllUnmapped(int page, int pagesize)
	{
		EDI856ASNCollectionDto eDI856ASNCollectionDto = new EDI856ASNCollectionDto();
		eDI856ASNCollectionDto.EDI856ShipmentSet = GetPendingEDITransferShipments_All().Result.ToList();
		if (eDI856ASNCollectionDto.EDI856ShipmentSet.Count > 0)
		{
			int num = eDI856ASNCollectionDto.EDI856ShipmentSet.Count();
			int totalPages = (int)Math.Ceiling((double)num / (double)pagesize);
			int num2 = page - 1;
			num2 = ((num2 >= 0) ? num2 : 0);
			eDI856ASNCollectionDto.TotalRecords = num;
			List<EDI856OutboundASN> eDI856ShipmentSet = eDI856ASNCollectionDto.EDI856ShipmentSet.Skip(pagesize * num2).Take(pagesize).ToList();
			eDI856ASNCollectionDto.EDI856ShipmentSet = eDI856ShipmentSet;
			eDI856ASNCollectionDto.TotalPages = totalPages;
			eDI856ASNCollectionDto.PageSize = pagesize;
			eDI856ASNCollectionDto.CurrentPageIndex = page;
		}
		return Task.FromResult(eDI856ASNCollectionDto);
	}

	public Task<APIValidationInfoDto> ValidateRequest_SetEDIFlag(EDI856ASNsIN ediShipments)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			if (ediShipments.EDI856ASNSet.Count == 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
				base.ErrorsList.Add("No records found in the request or invalid format.");
			}
			else
			{
				foreach (EDI856ASNIN item in ediShipments.EDI856ASNSet)
				{
					if (!EdiShipmentRepository.DoesShipmentExists(item.ShipmentNumber).Result)
					{
						httpValidationStatusCode = HttpStatusCode.OK;
						base.ErrorsList.Add("ShipmentNumber " + item.ShipmentNumber + " is invalid.");
					}
					else if (!EdiShipmentRepository.IsEDIShipment(item.ShipmentNumber).Result)
					{
						httpValidationStatusCode = HttpStatusCode.OK;
						base.ErrorsList.Add("Shipment " + item.ShipmentNumber + " is not an EDI invoice.");
					}
				}
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating inbound shipment numbers.");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<APIValidationInfoDto> Process_SetEDIFlag(EDI856ASNsIN ediShipments)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		SqlTransaction sqlTransaction = null;
		try
		{
			sqlTransaction = EdiShipmentRepository.M1database.BeginTransaction();
			Dictionary<string, bool> shipmentDictionary = ediShipments.EDI856ASNSet.ToDictionary((EDI856ASNIN e) => e.ShipmentNumber, (EDI856ASNIN e) => e.EDIUpdateStatus);
			if (EdiShipmentRepository.UpdateEdiFlag(shipmentDictionary, sqlTransaction).Result)
			{
				sqlTransaction.Commit();
			}
			else
			{
				sqlTransaction.Rollback();
				base.ErrorsList.Add("Error occurred while updating inbound shipment numbers.");
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while updating inbound shipment numbers.");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
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
			EdiShipmentRepository.Dispose();
			base.PartRepository.Dispose();
		}
	}
}
