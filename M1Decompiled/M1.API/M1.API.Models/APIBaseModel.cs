using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.EDI;
using M1.API.Repositories;
using M1.API.Repositories.Core;
using M1.API.Utilities;
using M1.Ax.Erp;
using M1.Core;

namespace M1.API.Models;

public abstract class APIBaseModel : IAPIBaseModel, IDisposable
{
	public APIClientContext ApiClientContext { get; set; }

	public IList<string> ErrorsList { get; set; }

	public IList<string> WarningsList { get; set; }

	public APIBaseModel()
	{
	}

	public APIBaseModel(APIClientContext apiClientContext)
	{
		ApiClientContext = apiClientContext;
		ErrorsList = new List<string>();
		WarningsList = new List<string>();
	}

	public Task<PartInformationDto> GetPartInfo(IAPIBaseRepository apiRepository, string currentSalesOrder, short currentSalesOrderLineId, string orgPartId, string orgPartShortDescription, string partRevisionId, string customerOrganizationID)
	{
		PartInformationDto partInformationDto = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string orgPartID = string.Empty;
		string orgPartShortDescription2 = string.Empty;
		IPartRepository partRepository = apiRepository as IPartRepository;
		string text = partRepository.GetPartIdFromPartOrgReference(orgPartId, customerOrganizationID).Result;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = orgPartId;
		}
		else
		{
			orgPartID = orgPartId;
			orgPartShortDescription2 = orgPartShortDescription;
		}
		partInformationDto = partRepository.GetPartInfo(text).Result;
		if (!string.IsNullOrWhiteSpace(partInformationDto.PartID))
		{
			string text2 = (partRevisionId ?? string.Empty).Trim();
			PartRevisionInformationDto result = partRepository.GetPartRevisionInfo(partInformationDto.PartID, text2).Result;
			Part part = new Part();
			if (!string.IsNullOrWhiteSpace(result.PartID))
			{
				empty = part.GetPreferredWarehouse(partRepository.M1database, partInformationDto.PartID, text2, string.Empty);
				empty2 = part.GetPreferredWarehouseBin(partRepository.M1database, partInformationDto.PartID, text2, empty, string.Empty);
				partInformationDto.PartRevisionID = text2;
				partInformationDto.UOM = result.InventoryUnitOfMeasure ?? string.Empty;
				partInformationDto.Weight = result.Weight;
				partInformationDto.PartWarehouseLocationID = empty ?? string.Empty;
				partInformationDto.PartBinID = empty2 ?? string.Empty;
				partInformationDto.OrgPartID = orgPartID;
				partInformationDto.OrgPartShortDescription = orgPartShortDescription2;
			}
			else
			{
				string partRevisionID = string.Empty;
				if (part.GetLatestPartRevision((apiRepository as IPartRepository).M1database, null, partInformationDto.PartID, ref partRevisionID))
				{
					partInformationDto.WarningsList.Add($"Part revision in sales order [{currentSalesOrder}] line [{currentSalesOrderLineId}] is invalid. The latest revision id for the part [{partInformationDto.PartID}] was taken.");
					PartRevisionInformationDto result2 = partRepository.GetPartRevisionInfo(partInformationDto.PartID, partRevisionID).Result;
					if (!string.IsNullOrWhiteSpace(result2.PartID))
					{
						empty = part.GetPreferredWarehouse(partRepository.M1database, partInformationDto.PartID, partRevisionID, string.Empty);
						empty2 = part.GetPreferredWarehouseBin(partRepository.M1database, partInformationDto.PartID, partRevisionID, empty, string.Empty);
						partInformationDto.PartRevisionID = partRevisionID;
						partInformationDto.UOM = result2.InventoryUnitOfMeasure ?? string.Empty;
						partInformationDto.Weight = result2.Weight;
						partInformationDto.PartWarehouseLocationID = empty ?? string.Empty;
						partInformationDto.PartBinID = empty2 ?? string.Empty;
						partInformationDto.OrgPartID = orgPartID;
						partInformationDto.OrgPartShortDescription = orgPartShortDescription2;
					}
				}
				else
				{
					partInformationDto.ErrorsList.Add("Part revision information for the part [" + orgPartId + "] in sales order [" + currentSalesOrder + "] is invalid.");
				}
			}
		}
		else
		{
			partInformationDto.ErrorsList.Add("PartID [" + orgPartId + "] in sales order [" + currentSalesOrder + "] is invalid.");
		}
		return Task.FromResult(partInformationDto);
	}

	public Task<TaxInformationDto> GetTaxInformation(IAPIBaseRepository apiRepository, PartInformationDto partInformation, OrganizationLocationDto organizationLocation, DateTime orderDate)
	{
		TaxInformationDto taxInformationDto = new TaxInformationDto();
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = organizationLocation?.CustomerTaxCodeID ?? string.Empty;
		empty2 = organizationLocation?.CustomerSecondTaxCodeID ?? string.Empty;
		if (partInformation.PartAlwaysNonTaxable)
		{
			taxInformationDto.FirstTaxCodeID = string.Empty;
			taxInformationDto.SecondTaxCodeID = string.Empty;
		}
		else
		{
			if (!string.IsNullOrEmpty(partInformation.PartTaxCodeID))
			{
				taxInformationDto.FirstTaxCodeID = partInformation.PartTaxCodeID;
				taxInformationDto.FirstTaxRate = apiRepository.GetTaxRate(partInformation.PartTaxCodeID, orderDate).Result;
			}
			else
			{
				taxInformationDto.FirstTaxCodeID = empty;
				if (!string.IsNullOrWhiteSpace(empty))
				{
					taxInformationDto.FirstTaxRate = apiRepository.GetTaxRate(empty, orderDate).Result;
				}
			}
			if (!string.IsNullOrWhiteSpace(partInformation.PartSecondTaxCodeID))
			{
				taxInformationDto.SecondTaxCodeID = partInformation.PartSecondTaxCodeID;
				if (!partInformation.PartSecondTaxCodeID.Trim().Equals(partInformation.PartTaxCodeID, StringComparison.CurrentCultureIgnoreCase))
				{
					taxInformationDto.SecondTaxRate = apiRepository.GetTaxRate(partInformation.PartSecondTaxCodeID, orderDate).Result;
				}
				else
				{
					taxInformationDto.SecondTaxRate = taxInformationDto.FirstTaxRate;
				}
			}
			else
			{
				taxInformationDto.SecondTaxCodeID = empty2;
				if (!empty2.Trim().Equals(empty, StringComparison.CurrentCultureIgnoreCase))
				{
					if (!string.IsNullOrWhiteSpace(empty2))
					{
						taxInformationDto.SecondTaxRate = apiRepository.GetTaxRate(empty2, orderDate).Result;
					}
				}
				else
				{
					taxInformationDto.SecondTaxRate = taxInformationDto.FirstTaxRate;
				}
			}
		}
		taxInformationDto.NonTaxReasonID = partInformation.PartNonTaxReasonID ?? string.Empty;
		return Task.FromResult(taxInformationDto);
	}

	public Task<OrganizationLocationAddressDto> GetM1CompanyAddressFromDP(M1Database database)
	{
		return Task.FromResult(new OrganizationLocationAddressDto
		{
			LocationName = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadName")) ? "" : database.Props("DatasetProperties").Field<string>("xadName").Trim()),
			ContactID = string.Empty,
			AddressLine = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadAddressLine1")) ? "" : database.Props("DatasetProperties").Field<string>("xadAddressLine1").Trim()),
			City = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadCity")) ? "" : database.Props("DatasetProperties").Field<string>("xadCity").Trim()),
			State = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadState")) ? "" : database.Props("DatasetProperties").Field<string>("xadState").Trim()),
			PostCode = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadPostCode")) ? "" : database.Props("DatasetProperties").Field<string>("xadPostCode").Trim()),
			Country = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadCountry")) ? "" : database.Props("DatasetProperties").Field<string>("xadCountry").Trim()),
			PhoneNumber = (string.IsNullOrWhiteSpace(database.Props("DatasetProperties").Field<string>("xadPhoneNumber")) ? "" : database.Props("DatasetProperties").Field<string>("xadPhoneNumber").Trim())
		});
	}

	public Task<OrganizationLocationAddressDto> GetM1CompanyAddressFromPlant(IAPIBaseRepository apiRepository, string plantId)
	{
		return Task.FromResult((apiRepository as IOrganizationRepository)?.GetM1CompanyAddressFromPlant(plantId)?.Result);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}
}
