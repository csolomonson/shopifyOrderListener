using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;
using M1.API.Utilities;
using M1.Ax.Erp;

namespace M1.API.Models.BOM;

public class BOMPartModel : BOMBaseModel, IBOMPartModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	private struct TempGuids
	{
		public string MethodID { get; set; }

		public string MethodGUID { get; set; }

		public string PartRevisionID { get; set; }

		public string PartRevisionGUID { get; set; }

		public int PartAssemblyID { get; set; }

		public string PartAssemblyGUID { get; set; }
	}

	public IDictionary<string, object> PartKeyDictionary { get; set; }

	public BOMPartModel()
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

	private Task<List<CTMBOMPartMethodGuidDto>> GetPartMethodGuids(string partId)
	{
		DataTable dataTable = new DataTable();
		List<CTMBOMPartMethodGuidDto> list = new List<CTMBOMPartMethodGuidDto>();
		IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
		using (partRepository)
		{
			dataTable = base.PartRepository.GetPartMethodGuidsAsDataTable(partId).Result;
		}
		if (dataTable != null && dataTable.Rows.Count > 0)
		{
			foreach (TempGuids item2 in (from r in dataTable.AsEnumerable()
				select new TempGuids
				{
					MethodID = r.Field<string>("imaMethodID").ToUpper(),
					MethodGUID = r.Field<Guid>("impUniqueID").ToString()
				}).Distinct().ToList())
			{
				CTMBOMPartMethodGuidDto item = new CTMBOMPartMethodGuidDto
				{
					PartId = item2.MethodID.Trim(),
					PartGuid = item2.MethodGUID,
					PartRevisionGuids = new List<CTMBOMPartRevisionGuidDto>(GetPartRevisionGuids(dataTable, item2))
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}

	private List<CTMBOMPartRevisionGuidDto> GetPartRevisionGuids(DataTable partsTable, TempGuids currentPart)
	{
		List<CTMBOMPartRevisionGuidDto> list = new List<CTMBOMPartRevisionGuidDto>();
		foreach (TempGuids item2 in (from r in partsTable.AsEnumerable()
			where r.Field<string>("imaMethodID").Equals(currentPart.MethodID, StringComparison.CurrentCultureIgnoreCase)
			select new TempGuids
			{
				MethodID = r.Field<string>("imaMethodID"),
				PartRevisionID = r.Field<string>("imrPartRevisionID"),
				PartRevisionGUID = r.Field<Guid>("imrUniqueID").ToString()
			}).Distinct().ToList())
		{
			CTMBOMPartRevisionGuidDto item = new CTMBOMPartRevisionGuidDto
			{
				RevisionId = item2.PartRevisionID,
				RevisionGuid = item2.PartRevisionGUID,
				PartAssemblyGuids = new List<CTMBOMPartAssemblyGuidDto>(GetPartAssemblyGuids(partsTable, item2))
			};
			list.Add(item);
		}
		return list;
	}

	private List<CTMBOMPartAssemblyGuidDto> GetPartAssemblyGuids(DataTable partsTable, TempGuids currentRevision)
	{
		List<CTMBOMPartAssemblyGuidDto> list = new List<CTMBOMPartAssemblyGuidDto>();
		foreach (TempGuids item2 in (from a in partsTable.AsEnumerable()
			where a.Field<string>("imaMethodID").Equals(currentRevision.MethodID, StringComparison.CurrentCultureIgnoreCase) && a.Field<string>("imaMethodRevisionID").Equals(currentRevision.PartRevisionID, StringComparison.CurrentCultureIgnoreCase)
			select new TempGuids
			{
				MethodID = a.Field<string>("imaMethodID"),
				PartRevisionID = a.Field<string>("imaMethodRevisionID"),
				PartAssemblyID = a.Field<int>("imaMethodAssemblyID"),
				PartAssemblyGUID = a.Field<Guid>("imaUniqueID").ToString()
			}).Distinct().ToList())
		{
			CTMBOMPartAssemblyGuidDto item = new CTMBOMPartAssemblyGuidDto
			{
				AssemblyId = item2.PartAssemblyID,
				AssemblyGuid = item2.PartAssemblyGUID,
				PartMaterialGuids = new List<CTMBOMPartMaterialGuidDto>(GetPartMaterialGuids(partsTable, item2)),
				PartOperationGuids = new List<CTMBOMPartOperationGuidDto>(GetPartOperationGuids(partsTable, item2))
			};
			list.Add(item);
		}
		return list;
	}

	private IList<CTMBOMPartMaterialGuidDto> GetPartMaterialGuids(DataTable partsTable, TempGuids currentAssembly)
	{
		List<CTMBOMPartMaterialGuidDto> list = new List<CTMBOMPartMaterialGuidDto>();
		foreach (var item2 in (from r in (from m in partsTable.AsEnumerable()
				where m.Field<string>("imaMethodID").Equals(currentAssembly.MethodID, StringComparison.CurrentCultureIgnoreCase) && m.Field<string>("imaMethodRevisionID").Equals(currentAssembly.PartRevisionID, StringComparison.CurrentCultureIgnoreCase) && m.Field<int>("imaMethodAssemblyID") == currentAssembly.PartAssemblyID
				select new
				{
					MaterialID = m.Field<int?>("immMethodMaterialID"),
					MaterialGUID = m.Field<Guid?>("immUniqueID").ToString()
				}).Distinct()
			orderby r.MaterialID
			select r).ToList())
		{
			if (item2.MaterialID.HasValue)
			{
				CTMBOMPartMaterialGuidDto item = new CTMBOMPartMaterialGuidDto
				{
					MaterialId = item2.MaterialID.GetValueOrDefault(),
					MaterialGuid = item2.MaterialGUID
				};
				list.Add(item);
			}
		}
		return list;
	}

	private IList<CTMBOMPartOperationGuidDto> GetPartOperationGuids(DataTable partsTable, TempGuids currentAssembly)
	{
		List<CTMBOMPartOperationGuidDto> list = new List<CTMBOMPartOperationGuidDto>();
		foreach (var item2 in (from o in partsTable.AsEnumerable()
			where o.Field<string>("imaMethodID").Equals(currentAssembly.MethodID, StringComparison.CurrentCultureIgnoreCase) && o.Field<string>("imaMethodRevisionID").Equals(currentAssembly.PartRevisionID, StringComparison.CurrentCultureIgnoreCase) && o.Field<int>("imaMethodAssemblyID") == currentAssembly.PartAssemblyID
			select new
			{
				OperationID = o.Field<int?>("imoMethodOperationID"),
				OperationGUID = o.Field<Guid?>("imoUniqueID").ToString()
			}).Distinct().ToList())
		{
			if (item2.OperationID.HasValue)
			{
				CTMBOMPartOperationGuidDto item = new CTMBOMPartOperationGuidDto
				{
					OperationId = item2.OperationID.GetValueOrDefault(),
					OperationGuid = item2.OperationGUID
				};
				list.Add(item);
			}
		}
		return list;
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

	public async Task<APIValidationInfoDto> ValidateRequest_ProcessPart(BOMPartDto partDto)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				if (partDto.PartType != 1 && partDto.PartType != 2)
				{
					base.ErrorsList.Add($"Part type [{partDto.PartType}] is invalid");
				}
				if (!string.IsNullOrWhiteSpace(partDto.PartClassID))
				{
					if (!(await base.PartRepository.DoesPartClassExists(partDto.PartClassID)))
					{
						base.ErrorsList.Add("Part class [" + partDto.PartClassID + "] is invalid");
					}
				}
				else if (await base.PartRepository.IsCogsEnabled())
				{
					base.ErrorsList.Add("Part class [" + partDto.PartClassID + "] is empty");
				}
				if (!string.IsNullOrWhiteSpace(partDto.PartGroupID) && !(await base.PartRepository.DoesPartGroupExists(partDto.PartGroupID)))
				{
					base.ErrorsList.Add("Part group [" + partDto.PartGroupID + "] is invalid");
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the part [" + partDto.PartID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return result;
	}

	public Task<APIValidationInfoDto> ValidateRequest_ProcessPartRevision(CTMBOMPartRevisionDto partRevision)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			IOrganizationRepository organizationRepository = (base.OrganizationRepository = new OrganizationRepository(base.ApiClientContext));
			using (organizationRepository)
			{
				IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
				using (partRepository)
				{
					if (partRevision.Part.PartType != 1 && partRevision.Part.PartType != 2)
					{
						base.ErrorsList.Add($"Part type [{partRevision.Part.PartType}] is invalid");
					}
					if (!string.IsNullOrWhiteSpace(partRevision.Part.PartClassID))
					{
						if (!base.PartRepository.DoesPartClassExists(partRevision.Part.PartClassID).Result)
						{
							base.ErrorsList.Add("Part class [" + partRevision.Part.PartClassID + "] is invalid");
						}
					}
					else if (base.PartRepository.IsCogsEnabled().Result)
					{
						base.ErrorsList.Add("Part class [" + partRevision.Part.PartClassID + "] is empty");
					}
					if (!string.IsNullOrWhiteSpace(partRevision.Part.PartGroupID) && !base.PartRepository.DoesPartGroupExists(partRevision.Part.PartGroupID).Result)
					{
						base.ErrorsList.Add("Part group [" + partRevision.Part.PartGroupID + "] is invalid");
					}
					foreach (BOMPartRevisionDto partRevision2 in partRevision.PartRevisions)
					{
						if (string.IsNullOrWhiteSpace(partRevision2.PartID))
						{
							base.ErrorsList.Add("Part revision id [" + partRevision2.PartID + "] is invalid");
						}
						if (!string.IsNullOrWhiteSpace(partRevision2.SupplierOrganizationID) && !base.OrganizationRepository.DoesSupplierOrganizationExists(partRevision2.SupplierOrganizationID).Result)
						{
							base.ErrorsList.Add("Supplier organizationID [" + partRevision2.SupplierOrganizationID + "] is invalid");
						}
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the part [" + partRevision.Part.PartID + "]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<APIValidationInfoDto> ValidateRequest_ProcessPartAssembly(BOMPartAssemblyDto partAssembly)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			if (partAssembly.QuantityPerParent <= 0m)
			{
				base.ErrorsList.Add($"Quantity per parent [{partAssembly.QuantityPerParent}]is invalid");
			}
			if (partAssembly.MethodAssemblyID == 0)
			{
				if (partAssembly.Level != 1)
				{
					base.ErrorsList.Add($"Level [{partAssembly.Level}] is invalid. It should be 1 for the assembly 0");
				}
				if (partAssembly.ParentAssemblyID != 0)
				{
					base.ErrorsList.Add($"Parent assembly [{partAssembly.ParentAssemblyID}] is invalid. It should be 0 for the assembly 0");
				}
			}
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				if ((!string.IsNullOrWhiteSpace(partAssembly.MethodID ?? string.Empty) || !string.IsNullOrWhiteSpace(partAssembly.MethodRevisionID ?? string.Empty)) && !base.PartRepository.DoesMethodRevisionExists(partAssembly.MethodID, partAssembly.MethodRevisionID).Result)
				{
					base.ErrorsList.Add("Part method [" + partAssembly.MethodID + "] or method revision [" + partAssembly.MethodRevisionID + "] is invalid");
				}
				if ((!string.IsNullOrWhiteSpace(partAssembly.MethodID ?? string.Empty) || !string.IsNullOrWhiteSpace(partAssembly.MethodRevisionID ?? string.Empty)) && !base.PartRepository.DoesMethodAssemblyExists(partAssembly.MethodID, partAssembly.MethodRevisionID, partAssembly.ParentAssemblyID).Result)
				{
					base.ErrorsList.Add($"Part method [{partAssembly.MethodID}] or method revision [{partAssembly.MethodRevisionID}] or parent assembly [{partAssembly.ParentAssemblyID}] is invalid");
				}
				if ((!string.IsNullOrWhiteSpace(partAssembly.PartID ?? string.Empty) || !string.IsNullOrWhiteSpace(partAssembly.PartRevisionID ?? string.Empty)) && base.PartRepository.DoesRequirePartsToExistInventory().Result && !base.PartRepository.DoesPartRevisionExists(partAssembly.PartID, partAssembly.PartRevisionID).Result)
				{
					base.ErrorsList.Add("Part [" + partAssembly.PartID + "] or part revision [" + partAssembly.PartRevisionID + "] is invalid");
				}
				if (base.PartRepository.IsUseMethod_MethodAssembly(partAssembly.MethodID, partAssembly.MethodRevisionID, partAssembly.ParentAssemblyID).Result)
				{
					base.ErrorsList.Add($"Parent assembly [{partAssembly.ParentAssemblyID}] cannot be used as parent because it uses Use Method");
				}
				if (partAssembly.UseMethod && partAssembly.MethodID.Equals(partAssembly.PartID, StringComparison.CurrentCultureIgnoreCase) && partAssembly.MethodRevisionID.Equals(partAssembly.PartRevisionID, StringComparison.CurrentCultureIgnoreCase))
				{
					base.WarningsList.Add("Use method cannot be used when method revision details and part revision details are same");
					partAssembly.UseMethod = false;
				}
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpValidationStatusCode = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the part method [" + partAssembly.MethodID + "]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeletePartAssembly(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
		using (partRepository)
		{
			if (!base.PartRepository.DoesMethodAssemblyExists(methodId, methodRevisionId, methodAssemblyId).Result)
			{
				base.ErrorsList.Add($"Part method [{methodId}] or method revision [{methodRevisionId}] or method assembly [{methodAssemblyId}] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeletePartAssembly_Guid(string methodAsmGuid)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		if (!Guid.TryParse(methodAsmGuid, out var _))
		{
			base.ErrorsList.Add("GUID [" + methodAsmGuid + "] is invalid");
		}
		else
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				IDictionary<string, object> result2 = base.PartRepository.GetPartAsmKeysFromGuid(methodAsmGuid).Result;
				if (result2.Count == 0)
				{
					base.ErrorsList.Add("Part assembly guid[" + methodAsmGuid + "] is invalid");
				}
				else
				{
					PartKeyDictionary = result2;
				}
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_ProcessPartOperation(BOMPartOperationDto partOperation)
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
				if (!string.IsNullOrWhiteSpace(partOperation.MethodID) && !string.IsNullOrWhiteSpace(partOperation.MethodRevisionID) && !base.PartRepository.DoesMethodAssemblyExists(partOperation.MethodID, partOperation.MethodRevisionID, partOperation.MethodAssemblyID).Result)
				{
					base.ErrorsList.Add($"Part method [{partOperation.MethodID}] or method revision [{partOperation.MethodRevisionID}] or parent assembly [{partOperation.MethodAssemblyID}] is invalid");
				}
				if (base.PartRepository.IsUseMethod_MethodAssembly(partOperation.MethodID, partOperation.MethodRevisionID, partOperation.MethodAssemblyID).Result)
				{
					base.ErrorsList.Add($"Assembly [{partOperation.MethodAssemblyID}] cannot be used because it uses Use Method");
				}
				if (partOperation.OperationType != 1 && partOperation.OperationType != 2)
				{
					base.ErrorsList.Add($"Part type [{partOperation.OperationType}] is invalid");
				}
				if (!string.IsNullOrWhiteSpace(partOperation.WorkCenterID) && !base.PartRepository.DoesWorkCenterExists(partOperation.WorkCenterID).Result)
				{
					base.ErrorsList.Add("Workcentre id [" + partOperation.WorkCenterID + "] is invalid");
				}
				if (!string.IsNullOrWhiteSpace(partOperation.ProcessID) && !base.PartRepository.DoesProcessExists(partOperation.ProcessID).Result)
				{
					base.ErrorsList.Add("Process id [" + partOperation.ProcessID + "] is invalid");
				}
				if (partOperation.MachineType != 1 && partOperation.MachineType != 2 && partOperation.MachineType != 3)
				{
					base.ErrorsList.Add($"Machine type [{partOperation.MachineType}] is invalid");
				}
				if (partOperation.QuantityPerAssembly <= 0m)
				{
					base.ErrorsList.Add($"Quantity per assembly [{partOperation.QuantityPerAssembly}] is invalid");
				}
			}
			if (!WebAPIConstants.M1_STANDARD_FACTORS_ARRAY.Contains(partOperation.StandardFactor.ToUpper()))
			{
				base.ErrorsList.Add("Standard factor [" + partOperation.StandardFactor + "] is invalid. It should be from one of these [" + string.Join(",", WebAPIConstants.M1_STANDARD_FACTORS_ARRAY) + "]");
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the part method [" + partOperation.MethodID + "]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeletePartOperation(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
		using (partRepository)
		{
			if (!base.PartRepository.DoesMethodAssemblyOperationExists(methodId, methodRevisionId, methodAssemblyId, methodOperationId).Result)
			{
				base.ErrorsList.Add("Part method [" + methodId + "] or method revision [" + methodRevisionId + "] " + $"or method assembly [{methodAssemblyId}] or method operation [{methodOperationId}] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeletePartOperation_Guid(string methodOperationGuid)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		Guid result = Guid.Empty;
		if (!Guid.TryParse(methodOperationGuid, out result))
		{
			base.ErrorsList.Add("GUID [" + methodOperationGuid + "] is invalid");
		}
		else
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				IDictionary<string, object> result2 = base.PartRepository.GetPartOperationKeysFromGuid(methodOperationGuid).Result;
				if (result2.Count() == 0)
				{
					base.ErrorsList.Add("Part operation guid[" + methodOperationGuid + "] is invalid");
				}
				else
				{
					PartKeyDictionary = result2;
				}
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_ProcessPartMaterial(BOMPartMaterialDto partMaterial)
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
				if (!base.PartRepository.DoesMethodAssemblyExists(partMaterial.MethodID, partMaterial.MethodRevisionID, partMaterial.MethodAssemblyID).Result)
				{
					base.ErrorsList.Add("Part method [" + partMaterial.MethodID + "] or method revision [" + partMaterial.MethodRevisionID + "] " + $"or parent assembly [{partMaterial.MethodAssemblyID}] is invalid");
				}
				if (base.PartRepository.IsUseMethod_MethodAssembly(partMaterial.MethodID, partMaterial.MethodRevisionID, partMaterial.MethodAssemblyID).Result)
				{
					base.ErrorsList.Add($"Assembly [{partMaterial.MethodAssemblyID}] cannot be used because it uses Use Method");
				}
				if ((!string.IsNullOrWhiteSpace(partMaterial.PartID) || !string.IsNullOrWhiteSpace(partMaterial.PartRevisionID)) && base.PartRepository.DoesRequirePartsToExistInventory().Result && !base.PartRepository.DoesPartRevisionExists(partMaterial.PartID, partMaterial.PartRevisionID).Result)
				{
					base.ErrorsList.Add("Part [" + partMaterial.PartID + "] or part revision [" + partMaterial.PartRevisionID + "] is invalid");
				}
				if (partMaterial.RelatedPartOperationID > 0 && !base.PartRepository.DoesMethodAssemblyOperationExists(partMaterial.MethodID, partMaterial.MethodRevisionID, partMaterial.MethodAssemblyID, partMaterial.RelatedPartOperationID).Result)
				{
					base.ErrorsList.Add($"Related operation [{partMaterial.RelatedPartOperationID}] is invalid");
				}
				if (partMaterial.QuantityPerAssembly <= 0m)
				{
					base.ErrorsList.Add($"Quantity per assembly [{partMaterial.QuantityPerAssembly}] is invalid");
				}
			}
			IOrganizationRepository organizationRepository = (base.OrganizationRepository = new OrganizationRepository(base.ApiClientContext));
			using (organizationRepository)
			{
				if ((!string.IsNullOrWhiteSpace(partMaterial.SupplierOrganizationID) || !string.IsNullOrWhiteSpace(partMaterial.PurchaseLocationID)) && !base.OrganizationRepository.DoesSupplierPurchaseLocationExists(partMaterial.SupplierOrganizationID, partMaterial.PurchaseLocationID).Result)
				{
					base.ErrorsList.Add("Supplier [" + partMaterial.SupplierOrganizationID + "] or purchase location [" + partMaterial.PurchaseLocationID + "] is invalid");
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the part method [" + partMaterial.MethodID + "]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetPartMaterial(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
		using (partRepository)
		{
			if (!base.PartRepository.DoesMethodAssemblyMaterialExists(methodId, methodRevisionId, methodAssemblyId).Result)
			{
				base.ErrorsList.Add("Part method [" + methodId + "] or method revision [" + methodRevisionId + "] " + $"or method assembly [{methodAssemblyId}] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeletePartMaterial(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
		using (partRepository)
		{
			if (!base.PartRepository.DoesMethodAssemblyMaterialExists(methodId, methodRevisionId, methodAssemblyId, methodMaterialId).Result)
			{
				base.ErrorsList.Add("Part method [" + methodId + "] or method revision [" + methodRevisionId + "] " + $"or method assembly [{methodAssemblyId}] or method material [{methodMaterialId}] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeletePartMaterial_Guid(string methodMaterialGuid)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		Guid result = Guid.Empty;
		if (!Guid.TryParse(methodMaterialGuid, out result))
		{
			base.ErrorsList.Add("GUID [" + methodMaterialGuid + "] is invalid");
		}
		else
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				IDictionary<string, object> result2 = base.PartRepository.GetPartMaterialKeysFromGuid(methodMaterialGuid).Result;
				if (result2.Count() == 0)
				{
					base.ErrorsList.Add("Part material guid[" + methodMaterialGuid + "] is invalid");
				}
				else
				{
					PartKeyDictionary = result2;
				}
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMPartMaterialDto>>> Process_GetPartMaterial(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMPartMaterialDto> returnObject = new List<BOMPartMaterialDto>();
		BOMResponseMessageDto<IList<BOMPartMaterialDto>> result;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				returnObject = base.PartRepository.GetMethodMaterialsForAsm(methodId, methodRevisionId, methodAssemblyId).Result;
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while retrieving part materials [{methodId}/{methodRevisionId}/{methodAssemblyId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMPartMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetPartMethodBOM(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
		using (partRepository)
		{
			if (!base.PartRepository.DoesMethodAssemblyExists(methodId, methodRevisionId, methodAssemblyId).Result)
			{
				base.ErrorsList.Add($"Part method [{methodId}] or method revision [{methodRevisionId}] or method assembly [{methodAssemblyId}] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetPartMethodBOM_Guid(string partAssemblyGuid)
	{
		return ValidateRequest_DeletePartAssembly_Guid(partAssemblyGuid);
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetPartMethodGUIDs(string partId)
	{
		base.ErrorsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		if (string.IsNullOrWhiteSpace(partId))
		{
			base.ErrorsList.Add("Part id cannot be empty");
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, new List<string>(), httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<CTMBOMPartRevisionDto>> Process_GetPartRevision(string partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		CTMBOMPartRevisionDto partRevisionDto = new CTMBOMPartRevisionDto();
		BOMResponseMessageDto<CTMBOMPartRevisionDto> result;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				PartInformationDto partInfo = await base.PartRepository.GetPartInfo(partId);
				IList<PartRevisionInformationDto> obj = await base.PartRepository.GetPartRevisionsInfo(partId);
				partRevisionDto.Part = new BOMPartDto
				{
					PartID = partInfo.PartID,
					ShortDescription = partInfo.PartShortDescription,
					PartType = partInfo.PartType,
					PartClassID = partInfo.PartClassID,
					PartGroupID = partInfo.PartGroupID,
					LongDescription = partInfo.PartLongDescriptionText,
					BuyForInventory = partInfo.BuyForInventory,
					NonStockedItem = partInfo.NonStockedItem,
					DeliveryType = partInfo.DeliveryType
				};
				foreach (PartRevisionInformationDto item in obj)
				{
					partRevisionDto.PartRevisions.Add(new BOMPartRevisionDto
					{
						PartID = item.PartID,
						PartRevisionID = (item.PartRevisionID ?? string.Empty),
						ShortDescription = (item.PartShortDescription ?? string.Empty),
						InventoryUnitOfMeasure = (item.InventoryUnitOfMeasure ?? string.Empty),
						PurchaseUnitOfMeasure = (item.PurchaseUnitOfMeasure ?? string.Empty),
						SupplierOrganizationID = (item.SupplierOrganizationID ?? string.Empty),
						ConversionFactor = item.ConversionFactor,
						EffectiveStartDate = item.EffectiveStartDate,
						LeadTime = item.LeadTime,
						LastMiscCost = item.LastMiscCost,
						LastDutyCost = item.LastDutyCost,
						LastLaborCost = item.LastLaborCost,
						AverageMiscCost = item.AverageMiscCost,
						AverageDutyCost = item.AverageDutyCost,
						LongDescription = item.PartLongDescriptionText,
						LastFreightCost = item.LastFreightCost,
						EffectiveEndDate = item.EffectiveEndDate,
						AverageLaborCost = item.AverageLaborCost,
						StandardDutyCost = item.StandardDutyCost,
						LastOverheadCost = item.LastOverheadCost,
						LastMaterialCost = item.LastMaterialCost,
						StandardMiscCost = item.StandardMiscCost,
						StandardLaborCost = item.StandardLaborCost,
						PurchaseLocationId = item.PurchaseLocationId,
						AverageFreightCost = item.AverageFreightCost,
						StandardFreightCost = item.StandardFreightCost,
						AverageOverheadCost = item.AverageOverheadCost,
						AverageMaterialCost = item.AverageMaterialCost,
						LastSubcontractCost = item.LastSubcontractCost,
						StandardOverheadCost = item.StandardOverheadCost,
						StandardMaterialCost = item.StandardMaterialCost,
						AverageSubcontractCost = item.AverageSubcontractCost,
						StandardSubcontractCost = item.StandardSubcontractCost,
						LastTransactionDate = item.LastTransactionDate,
						ManufacturingLotSize = item.ManufacturingLotSize,
						Inactive = item.Inactive,
						LastReceiptDate = item.LastReceiptDate,
						RequiresInspection = item.RequiresInspection,
						ExpenseSplitPercentTotal = item.ExpenseSplitPercentTotal,
						Weight = item.Weight,
						WeightUnitOfMeasure = item.WeightUnitOfMeasure,
						AverageUnitCost = item.AverageUnitCost,
						StandardUnitCost = item.StandardUnitCost,
						LastUnitCost = item.LastUnitCost,
						SheetSizeX = item.SheetSizeX,
						SheetSizeY = item.SheetSizeY,
						BarLength = item.BarLength,
						Thickness = item.Thickness,
						CreatedBy = item.CreatedBy,
						CreatedDate = item.CreatedDate
					});
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Part [" + partId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<CTMBOMPartRevisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partRevisionDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMPartDto>> Process_PostPart(BOMPartDto partDto)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartDto> result;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.PartRepository.SavePart(partDto);
				base.ErrorsList = aPIValidationInfoDto.ErrorsList.ToList();
				base.WarningsList = aPIValidationInfoDto.WarningsList.ToList();
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Part [" + partDto.PartID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMPartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMBOMPartRevisionDto>> Process_PostPartRevision(CTMBOMPartRevisionDto partRevision)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<CTMBOMPartRevisionDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto result = base.PartRepository.SavePartRevision(partRevision).Result;
				base.ErrorsList = result.ErrorsList.ToList();
				base.WarningsList = result.WarningsList.ToList();
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Part [" + partRevision.Part.PartID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<CTMBOMPartRevisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partRevision
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<BOMPartAssemblyDto>> Process_PostPartAssembly(BOMPartAssemblyDto partAssembly)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartAssemblyDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto result = base.PartRepository.SavePartAssembly(partAssembly).Result;
				base.ErrorsList = result.ErrorsList.ToList();
				base.WarningsList = result.WarningsList.ToList();
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the part method [" + partAssembly.MethodID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMPartAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partAssembly
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<IList<BOMPartMethodAssemblyDto>>> Process_GetPartAssembly(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMPartMethodAssemblyDto> returnObject = null;
		BOMResponseMessageDto<IList<BOMPartMethodAssemblyDto>> result;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				base.PartRepository.IntializePartMethodLists();
				returnObject = base.PartRepository.GetPartMethodAssemblyList(methodId, methodRevisionId, methodAssemblyId).Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while retrieving the part assemblies for [{methodId}/{methodRevisionId}/{methodAssemblyId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMPartMethodAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMPartAssemblyDto>> Process_DeletePartAssembly(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartAssemblyDto> result;
		try
		{
			new Part().DeletePartAssembly(base.ApiClientContext.Database, null, methodId, methodRevisionId, methodAssemblyId, deleteInitAsm: true);
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while deleting the part assembly [{methodId}/{methodRevisionId}/{methodAssemblyId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<BOMPartAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = null
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMPartOperationDto>> Process_PostPartOperation(BOMPartOperationDto partOperation)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartOperationDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto result = base.PartRepository.SavePartOperation(partOperation).Result;
				((List<string>)base.ErrorsList).AddRange(new List<string>(result.ErrorsList.ToList()));
				((List<string>)base.WarningsList).AddRange(new List<string>(result.WarningsList.ToList()));
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the part method [" + partOperation.MethodID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMPartOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partOperation
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<BOMPartOperationDto>> Process_DeletePartOperation(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartOperationDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto result = base.PartRepository.DeletePartOperation(methodId, methodRevisionId, methodAssemblyId, methodOperationId).Result;
				((List<string>)base.ErrorsList).AddRange(new List<string>(result.ErrorsList.ToList()));
				((List<string>)base.WarningsList).AddRange(new List<string>(result.WarningsList.ToList()));
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while deleting the part assembly [{methodId}/{methodRevisionId}/{methodAssemblyId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMPartOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = null
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<BOMPartMaterialDto>> Process_PostPartMaterial(BOMPartMaterialDto partMaterial)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartMaterialDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto result = base.PartRepository.SavePartMaterial(partMaterial).Result;
				base.ErrorsList = result.ErrorsList.ToList();
				base.WarningsList = result.WarningsList.ToList();
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the part method [" + partMaterial.MethodID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMPartMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partMaterial
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<BOMPartMaterialDto>> Process_DeletePartMaterial(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMPartMaterialDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				APIValidationInfoDto result = base.PartRepository.DeletePartMaterial(methodId, methodRevisionId, methodAssemblyId, methodMaterialId).Result;
				((List<string>)base.ErrorsList).AddRange(new List<string>(result.ErrorsList.ToList()));
				((List<string>)base.WarningsList).AddRange(new List<string>(result.WarningsList.ToList()));
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while deleting the part assembly [{methodId}/{methodRevisionId}/{methodAssemblyId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMPartMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = null
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<CTMBOMPartMethodDto>> Process_GetPartMethodBOM(string partId, string partRevisionId, int partAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMBOMPartMethodDto cTMBOMPartMethodDto = new CTMBOMPartMethodDto();
		BOMResponseMessageDto<CTMBOMPartMethodDto> result3;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				base.PartRepository.IntializePartMethodLists();
				PartInformationDto result = base.PartRepository.GetPartInfo(partId).Result;
				PartRevisionInformationDto result2 = base.PartRepository.GetPartRevisionInfo(partId, partRevisionId).Result;
				cTMBOMPartMethodDto.Part = new BOMPartDto
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
				cTMBOMPartMethodDto.PartRevision = new BOMPartRevisionDto
				{
					PartID = result2.PartID,
					PartRevisionID = (result2.PartRevisionID ?? string.Empty),
					ShortDescription = (result2.PartShortDescription ?? string.Empty),
					InventoryUnitOfMeasure = (result2.InventoryUnitOfMeasure ?? string.Empty),
					PurchaseUnitOfMeasure = (result2.PurchaseUnitOfMeasure ?? string.Empty),
					SupplierOrganizationID = (result2.SupplierOrganizationID ?? string.Empty),
					ConversionFactor = result2.ConversionFactor,
					EffectiveStartDate = result2.EffectiveStartDate,
					LeadTime = result2.LeadTime,
					LastMiscCost = result2.LastMiscCost,
					LastDutyCost = result2.LastDutyCost,
					LastLaborCost = result2.LastLaborCost,
					AverageMiscCost = result2.AverageMiscCost,
					AverageDutyCost = result2.AverageDutyCost,
					LongDescription = result2.PartLongDescriptionText,
					LastFreightCost = result2.LastFreightCost,
					EffectiveEndDate = result2.EffectiveEndDate,
					AverageLaborCost = result2.AverageLaborCost,
					StandardDutyCost = result2.StandardDutyCost,
					LastOverheadCost = result2.LastOverheadCost,
					LastMaterialCost = result2.LastMaterialCost,
					StandardMiscCost = result2.StandardMiscCost,
					StandardLaborCost = result2.StandardLaborCost,
					PurchaseLocationId = result2.PurchaseLocationId,
					AverageFreightCost = result2.AverageFreightCost,
					StandardFreightCost = result2.StandardFreightCost,
					AverageOverheadCost = result2.AverageOverheadCost,
					AverageMaterialCost = result2.AverageMaterialCost,
					LastSubcontractCost = result2.LastSubcontractCost,
					StandardOverheadCost = result2.StandardOverheadCost,
					StandardMaterialCost = result2.StandardMaterialCost,
					AverageSubcontractCost = result2.AverageSubcontractCost,
					StandardSubcontractCost = result2.StandardSubcontractCost,
					LastTransactionDate = result2.LastTransactionDate,
					ManufacturingLotSize = result2.ManufacturingLotSize,
					Inactive = result2.Inactive,
					LastReceiptDate = result2.LastReceiptDate,
					RequiresInspection = result2.RequiresInspection,
					ExpenseSplitPercentTotal = result2.ExpenseSplitPercentTotal,
					Weight = result2.Weight,
					WeightUnitOfMeasure = result2.WeightUnitOfMeasure,
					AverageUnitCost = result2.AverageUnitCost,
					StandardUnitCost = result2.StandardUnitCost,
					LastUnitCost = result2.LastUnitCost,
					SheetSizeX = result2.SheetSizeX,
					SheetSizeY = result2?.SheetSizeY,
					BarLength = result2?.BarLength,
					Thickness = result2.Thickness,
					CreatedBy = result2.CreatedBy,
					CreatedDate = result2.CreatedDate
				};
				cTMBOMPartMethodDto.PartAssemblies = (List<BOMPartMethodAssemblyDto>)base.PartRepository.GetPartMethodAssemblyList(partId, partRevisionId, partAssemblyId).Result;
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
			result3 = new BOMResponseMessageDto<CTMBOMPartMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = cTMBOMPartMethodDto
			};
		}
		return result3;
	}

	public async Task<BOMResponseMessageDto<CTMBOMPartMethodGuidsDto>> Process_GetPartMethodGUIDs(string partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMBOMPartMethodGuidsDto cTMBOMPartMethodGuidsDto = new CTMBOMPartMethodGuidsDto();
		BOMResponseMessageDto<CTMBOMPartMethodGuidsDto> result;
		try
		{
			cTMBOMPartMethodGuidsDto.PartMethodGuids = new List<CTMBOMPartMethodGuidDto>(GetPartMethodGuids(partId).Result.ToList());
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Part [" + partId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMBOMPartMethodGuidsDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = cTMBOMPartMethodGuidsDto
			};
		}
		return result;
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (disposing)
		{
			base.PartRepository?.Dispose();
			base.OrganizationRepository?.Dispose();
		}
	}

	public async Task<BOMResponseMessageDto<BOMPartDto>> Process_GetParts(string partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMPartDto returnObject = null;
		BOMResponseMessageDto<BOMPartDto> result2;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				PartInformationDto result = base.PartRepository.GetPartInfo(partId).Result;
				returnObject = new BOMPartDto
				{
					PartID = result.PartID,
					ShortDescription = result.PartShortDescription,
					PartType = result.PartType,
					PartClassID = result.PartClassID,
					PartGroupID = result.PartGroupID,
					DeliveryType = result.DeliveryType,
					NonStockedItem = result.NonStockedItem,
					LongDescription = result.PartLongDescriptionText,
					BuyForInventory = result.BuyForInventory
				};
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
			result2 = new BOMResponseMessageDto<BOMPartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<IList<BOMPartDto>>> Process_GetAllParts(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMPartDto> list = new List<BOMPartDto>();
		BOMResponseMessageDto<IList<BOMPartDto>> result;
		try
		{
			IPartRepository partRepository = (base.PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				foreach (PartInformationDto item2 in base.PartRepository.GetAllPartInfo(pageSize, pageNumber).Result)
				{
					BOMPartDto item = new BOMPartDto
					{
						PartID = item2.PartID,
						ShortDescription = item2.PartShortDescription,
						PartType = item2.PartType,
						PartClassID = item2.PartClassID,
						PartGroupID = item2.PartGroupID,
						DeliveryType = item2.DeliveryType,
						LongDescription = item2.PartLongDescriptionText,
						BuyForInventory = item2.BuyForInventory,
						NonStockedItem = item2.NonStockedItem
					};
					list.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Parts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMPartDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = list
			};
		}
		return result;
	}
}
