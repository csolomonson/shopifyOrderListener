using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.Ax.Erp;

namespace M1.API.Repositories.Core;

public interface IPartRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesPartExists(string partId);

	Task<bool> DoesPartClassExists(string partClassID);

	Task<bool> DoesPartGroupExists(string partGroupID);

	Task<bool> DoesMethodRevisionExists(string methodID, string methodRevisionID);

	Task<bool> DoesMethodAssemblyExists(string methodID, string methodRevisionID, int parentAssemblyID);

	Task<bool> DoesMethodAssemblyOperationExists(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId);

	Task<bool> DoesMethodAssemblyMaterialExists(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId);

	Task<bool> DoesMethodAssemblyMaterialExists(string methodId, string methodRevisionId, int methodAssemblyId);

	Task<bool> DoesPartRevisionExists(string partID, string partRevisionID);

	Task<bool> DoesWorkCenterExists(string workCenterID);

	Task<bool> DoesProcessExists(string processID);

	Task<bool> DoesPartWarehouseLocationExists(string partId, string partRevisionID, string partWarehouseLocation);

	Task<bool> DoesPartBinExists(string partId, string partRevisionID, string partWarehouseLocation, string partBin);

	Task<APIValidationInfoDto> DeletePartOperation(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId);

	Task<bool> IsUseMethod_MethodAssembly(string methodID, string methodRevisionID, int assemblyID);

	Task<PartInformationDto> GetPartInfo(string partID);

	Task<string> GetPartIdFromPartOrgReference(string partID, string organizationId);

	Task<PartRevisionInformationDto> GetPartRevisionInfo(string partId, string partRevisionId);

	Task<IList<PartRevisionInformationDto>> GetPartRevisionsInfo(string partId);

	Task<decimal> GetFullUnitPriceBase(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, decimal quantity, string currencyID, DateTime? priceDate);

	Task<APIValidationInfoDto> SavePart(BOMPartDto partDto);

	Task<APIValidationInfoDto> SavePartRevision(CTMBOMPartRevisionDto bomPartRevision);

	Task<APIValidationInfoDto> SavePartAssembly(BOMPartAssemblyDto partAssembly);

	Task<APIValidationInfoDto> SavePartOperation(BOMPartOperationDto partOperation);

	Task<APIValidationInfoDto> SavePartMaterial(BOMPartMaterialDto partMaterial);

	Task<APIValidationInfoDto> DeletePartMaterial(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId);

	Task<IList<BOMPartMethodAssemblyDto>> GetPartMethodAssemblyList(string part, string partRevision, int baseAsmId);

	Task<BOMPartAssemblyDto> GetMethodAssemblyInfo(string part, string partRevision, int methodAsm);

	Task<IList<BOMPartOperationDto>> GetMethodOerationsForAsm(string part, string partRevision, int methodAsm);

	Task<IList<BOMPartMaterialDto>> GetMethodMaterialsForAsm(string part, string partRevision, int methodAsm);

	void IntializePartMethodLists();

	Task<CTMPartClassesDto> GetAllPartClasses();

	Task<CTMPartGroupsDto> GetAllPartGroups();

	Task<string> GetPartIdFromGuid(Guid guidOut);

	Task<IDictionary<string, object>> GetPartAsmKeysFromGuid(string guidOut);

	Task<bool> DoesRequirePartsToExistInventory();

	Task<IDictionary<string, object>> GetPartOperationKeysFromGuid(string methodOperationGuid);

	Task<IDictionary<string, object>> GetPartMaterialKeysFromGuid(string methodMaterialGuid);

	Task<PriceCalculation> GetPartPrice(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, decimal quantity, string currencyID, DateTime? priceDate);

	Task<DataTable> GetPartMethodGuidsAsDataTable(string partId);

	Task<bool> IsCogsEnabled();

	Task<ICollection<PartInformationDto>> GetAllPartInfo(int? pageSize = null, int? pageNumber = null);
}
