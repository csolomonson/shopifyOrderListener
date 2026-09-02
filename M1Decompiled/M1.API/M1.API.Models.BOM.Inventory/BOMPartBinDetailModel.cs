using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Inventory;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.Custom.Inventory;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Inventory;

namespace M1.API.Models.BOM.Inventory;

public class BOMPartBinDetailModel : BOMBaseModel, IBOMPartBinDetailModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public IDictionary<string, object> PartKeyDictionary { get; set; }

	public BOMPartBinDetailModel()
	{
		PartKeyDictionary = new Dictionary<string, object>();
	}

	private async Task<string> GetM1PartIDFromGuid(string partIdString)
	{
		Guid result = Guid.Empty;
		if (Guid.TryParse(partIdString, out result))
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				return base.PartRepository.GetPartIdFromGuid(result).Result;
			}
		}
		return partIdString;
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetPartId(string partId)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				string result = GetM1PartIDFromGuid(partId).Result;
				if (string.IsNullOrWhiteSpace(result))
				{
					base.ErrorsList.Add("Part [" + partId + "] is invalid");
				}
				else
				{
					PartKeyDictionary.Add("impPartID", result);
					if (!base.PartRepository.DoesPartExists(result).Result)
					{
						base.ErrorsList.Add("Part [" + partId + "] is invalid");
					}
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the part revision [" + partId + "]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public async Task<BOMResponseMessageDto<CTMBOMPartBinDetailDto>> Process_PostPartBinDetail(string partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMBOMPartBinDetailDto cTMBOMPartBinDetailDto = new CTMBOMPartBinDetailDto();
		BOMResponseMessageDto<CTMBOMPartBinDetailDto> result3;
		try
		{
			IPartBinDetailRepository partBinDetailRepository = (base.PartBinDetailRepository = new PartBinDetailRepository(base.ApiClientContext));
			using (partBinDetailRepository)
			{
				PartInformationDto result = base.PartRepository.GetPartInfo(partId).Result;
				IList<PartBinDetailInformationDto> result2 = base.PartBinDetailRepository.GetPartBinDetailsInfo(partId).Result;
				cTMBOMPartBinDetailDto.Part = new BOMPartDto
				{
					PartID = result.PartID,
					ShortDescription = result.PartShortDescription,
					PartType = result.PartType,
					PartClassID = result.PartClassID,
					PartGroupID = result.PartGroupID,
					LongDescription = result.PartLongDescriptionText,
					BuyForInventory = result.BuyForInventory,
					NonStockedItem = result.NonStockedItem,
					DeliveryType = result.DeliveryType
				};
				foreach (PartBinDetailInformationDto item in result2)
				{
					cTMBOMPartBinDetailDto.PartBinDetails.Add(new BOMPartBinDetailDto
					{
						PartID = item.PartID,
						PartRevisionID = (item.PartRevisionID ?? string.Empty),
						PartBinID = (item.PartBinID ?? string.Empty),
						PartBinDetailID = item.PartBinDetailID,
						WarehouseID = item.WarehouseID,
						TransactionDate = item.TransactionDate,
						QuantityType = item.QuantityType,
						OriginalQuantity = item.OriginalQuantity,
						RemainingQuantity = item.RemainingQuantity,
						UnitCost = item.UnitCost,
						SourceTableName = item.SourceTableName,
						CreatedBy = item.CreatedBy,
						CreatedDate = item.CreatedDate,
						UniqueID = item.UniqueID,
						RowVersion = item.RowVersion
					});
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Part [" + partId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result3 = new BOMResponseMessageDto<CTMBOMPartBinDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = cTMBOMPartBinDetailDto
			};
		}
		return result3;
	}

	public async Task<BOMResponseMessageDto<IList<BOMPartBinDetailDto>>> Process_GetAllPartBinDetails(int? pageSize = null, int? pageNumber = null)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMPartBinDetailDto> allPartBinDetailsDto = new List<BOMPartBinDetailDto>();
		BOMResponseMessageDto<IList<BOMPartBinDetailDto>> result;
		try
		{
			IPartBinDetailRepository partBinDetailRepository = (base.PartBinDetailRepository = new PartBinDetailRepository(base.ApiClientContext));
			using (partBinDetailRepository)
			{
				foreach (PartBinDetailInformationDto item2 in await base.PartBinDetailRepository.GetAllPartBinDetails(pageSize, pageNumber))
				{
					BOMPartBinDetailDto item = new BOMPartBinDetailDto
					{
						PartID = item2.PartID,
						PartRevisionID = item2.PartRevisionID,
						PartBinID = item2.PartBinID,
						PartBinDetailID = item2.PartBinDetailID,
						WarehouseID = item2.WarehouseID,
						TransactionDate = item2.TransactionDate,
						QuantityType = item2.QuantityType,
						OriginalQuantity = item2.OriginalQuantity,
						RemainingQuantity = item2.RemainingQuantity,
						UnitCost = item2.UnitCost,
						SourceTableName = item2.SourceTableName,
						CreatedBy = item2.CreatedBy,
						CreatedDate = item2.CreatedDate,
						UniqueID = item2.UniqueID,
						RowVersion = item2.RowVersion
					};
					allPartBinDetailsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartBinDetails]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMPartBinDetailDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartBinDetailsDto
			};
		}
		return result;
	}

	public Task<BOMResponseMessageDto<BOMPartBinDetailDto>> Process_GetPartBinDetail(Guid uniqueId)
	{
		throw new NotImplementedException();
	}
}
