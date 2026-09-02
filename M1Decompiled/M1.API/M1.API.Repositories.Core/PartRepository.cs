using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Ax.Erp;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core;

public class PartRepository : APIBaseRepository, IPartRepository, IAPIBaseRepository, IDisposable
{
	private IList<BOMPartAssemblyDto> UseMethodAssemblyList = new List<BOMPartAssemblyDto>();

	private IList<BOMPartMethodAssemblyDto> MethodAssemblyList = new List<BOMPartMethodAssemblyDto>();

	private static readonly string[] PartAssemblyFields = new string[13]
	{
		"imaMethodID", "imaMethodRevisionID", "imaMethodAssemblyID", "imaLevel", "imaUseMethod", "imaParentAssemblyID", "imaPartID", "imaPartRevisionID", "imaPartShortDescription", "imaUnitOfMeasure",
		"imaQuantityPerParent", "imaOverlapOperationID", "imaPartLongDescriptionText"
	};

	private static readonly string[] PartMaterialFields = new string[19]
	{
		"immMethodID", "immMethodRevisionID", "immMethodAssemblyID", "immMethodMaterialID", "immPartID", "immPartRevisionID", "immRelatedPartOperationID", "immPartShortDescription", "immUnitOfMeasure", "immEstimatedUnitCost",
		"immLeadTime", "immSupplierOrganizationID", "immPurchaseLocationID", "immQuantityPerAssembly", "immManualPart", "immPartLongDescriptionText", "immBackflush", "immScrapQuantity", "immScrapPercent"
	};

	private readonly string GET_PARTMETHOD_GUIDS = "SELECT  ASM.imaMethodID, ASM.imaMethodRevisionID, ASM.imaMethodAssemblyID, ASM.imaUniqueID, ASM.imaLevel, PartRevisions.imrPartRevisionID, \r\n                                     PartRevisions.imrUniqueID, Parts.impPartID, Parts.impUniqueID, PartOperations.imoMethodOperationID, PartOperations.imoUniqueID, \r\n                                     PartMaterials.immMethodMaterialID, PartMaterials.immUniqueID\r\n            FROM            (SELECT        imaMethodID, imaMethodRevisionID, imaMethodAssemblyID, imaUniqueID, imaLevel\r\n                                      FROM            PartAssemblies\r\n                                      WHERE        ({0})) AS ASM INNER JOIN\r\n                                     PartRevisions ON ASM.imaMethodID = PartRevisions.imrPartID AND ASM.imaMethodRevisionID = PartRevisions.imrPartRevisionID INNER JOIN\r\n                                     Parts ON ASM.imaMethodID = Parts.impPartID LEFT OUTER JOIN\r\n                                     PartOperations ON ASM.imaMethodID = PartOperations.imoMethodID AND ASM.imaMethodRevisionID = PartOperations.imoMethodRevisionID AND \r\n                                     ASM.imaMethodAssemblyID = PartOperations.imoMethodAssemblyID LEFT OUTER JOIN\r\n                                     PartMaterials ON ASM.imaMethodID = PartMaterials.immMethodID AND ASM.imaMethodRevisionID = PartMaterials.immMethodRevisionID AND \r\n                                     ASM.imaMethodAssemblyID = PartMaterials.immMethodAssemblyID\r\n            ORDER BY ASM.imaMethodID, ASM.imaMethodRevisionID, ASM.imaMethodAssemblyID";

	private readonly string GET_ACTIVE_PART_REVISION = "SELECT imrPartRevisionID,imrShortDescription,imrLongDescriptionText,imrInventoryUnitOfMeasure,\r\n                        imrWeight,imrWeightUnitOfMeasure,imrCountryOfManufacture,imrEasyOrderPartID,imrBlanketPeriodBegin,imrBlanketPeriodEnd,\r\n                        imrNetCostBeginDate,imrNetCostCode,imrNetCostEndDate,imrPreferenceCriteria,imrProducerDetermination,imrCommodityCode,\r\n                        imrEffectiveEndDate,imrEffectiveStartDate,imrPurchaseUnitOfMeasure,imrSupplierOrganizationID,imrConversionFactor,imrCreatedBy,imrCreatedDate FROM PartRevisions\r\n                        WHERE imrPartID=@p1  AND imrPartRevisionID=@p2 AND imrInactive=0 AND (ISNULL(imrEffectiveEndDate,Convert(datetime,'01/01/2999',103))>GEtDATE())";

	private readonly string GET_ACTIVE_PART_REVISIONS = "SELECT imrPartRevisionID,imrShortDescription,imrLongDescriptionText,imrInventoryUnitOfMeasure,\r\n                        imrWeight,imrWeightUnitOfMeasure,imrCountryOfManufacture,imrEasyOrderPartID,imrBlanketPeriodBegin,imrBlanketPeriodEnd,\r\n                        imrNetCostBeginDate,imrNetCostCode,imrNetCostEndDate,imrPreferenceCriteria,imrProducerDetermination,imrCommodityCode,\r\n                        imrEffectiveEndDate,imrPurchaseUnitOfMeasure,imrSupplierOrganizationID,imrConversionFactor,imrEffectiveStartDate,imrCreatedBy,imrCreatedDate,\r\n                        imrLeadTime,imrPurchaseLocationID,imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrLastLaborCost,\r\n                        imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,\r\n                        imrAverageMiscCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost,imrLastTransactionDate,imrLastReceiptDate,imrInactive,imrManufacturingLotSize,\r\n                        imrExpenseSplitPercentTotal,imrRequiresInspection, imrThickness, imrSheetSizeX, imrSheetSizeY, imrBarLength,\r\n                        ISNULL(SUM(imrAverageMaterialCost + imrAverageLaborCost + imrAverageOverheadCost + imrAverageSubcontractCost + imrAverageDutyCost + imrAverageFreightCost + imrAverageMiscCost), 0) AS averageUnitCost,\r\n                        ISNULL(SUM(imrStandardMaterialCost + imrStandardLaborCost + imrStandardOverheadCost + imrStandardSubcontractCost + imrStandardDutyCost + imrStandardFreightCost + imrStandardMiscCost), 0) AS standardUnitCost,\r\n                        ISNULL(SUM(imrLastMaterialCost + imrLastLaborCost + imrLastOverheadCost + imrLastSubcontractCost + imrLastDutyCost + imrLastFreightCost + imrLastMiscCost), 0) AS lastUnitCost\r\n                        FROM PartRevisions\r\n                        WHERE imrPartID=@p1 AND imrInactive=0 AND (ISNULL(imrEffectiveEndDate,Convert(datetime,'01/01/2999',103))>GEtDATE())\r\n                        GROUP BY imrPartRevisionID, imrShortDescription, imrLongDescriptionText, imrInventoryUnitOfMeasure, imrWeight, imrWeightUnitOfMeasure, imrCountryOfManufacture, imrEasyOrderPartID\r\n                        ,imrBlanketPeriodBegin, imrBlanketPeriodBegin,imrBlanketPeriodEnd, imrNetCostBeginDate,  imrNetCostCode, imrNetCostEndDate, imrPreferenceCriteria, imrProducerDetermination\r\n                        ,imrCommodityCode, imrEffectiveEndDate, imrPurchaseUnitOfMeasure, imrSupplierOrganizationID, imrConversionFactor, imrEffectiveStartDate, imrCreatedBy, imrCreatedDate, imrLeadTime\r\n                        ,imrPurchaseLocationID, imrAverageLaborCost, imrAverageOverheadCost, imrAverageMaterialCost, imrAverageSubcontractCost, imrLastLaborCost, imrLastOverheadCost, imrLastMaterialCost,imrLastSubcontractCost\r\n                        ,imrStandardLaborCost, imrStandardOverheadCost, imrStandardMaterialCost, imrStandardSubcontractCost, imrAverageDutyCost, imrAverageFreightCost, imrAverageMiscCost, imrLastDutyCost\r\n                        ,imrLastFreightCost, imrLastMiscCost, imrStandardDutyCost, imrStandardFreightCost, imrStandardMiscCost, imrLastTransactionDate, imrLastReceiptDate, imrInactive, imrManufacturingLotSize\r\n                        ,imrExpenseSplitPercentTotal,imrRequiresInspection,imrThickness,imrSheetSizeX,imrSheetSizeY,imrBarLength";

	private readonly string DELETE_PART_METHODOPERATION = "DELETE FROM PartOperations WHERE imoMethodID = @PartID And imoMethodRevisionID = @RevisionID AND imoMethodAssemblyID = @AsmID AND imoMethodOperationID = @OprID\rDELETE FROM PartRules WHERE pcrMethodID = @PartID And pcrMethodRevisionID = @RevisionID AND pcrMethodAssemblyID = @AsmID AND pcrMethodOperationID = @OprID";

	private readonly string DELETE_PART_METHODOMATERIAL = "DELETE FROM PartMaterials WHERE immMethodID = @PartID And immMethodRevisionID = @RevisionID AND immMethodAssemblyID = @AsmID AND immMethodMaterialID = @MaterialID\rDELETE FROM PartRules WHERE pcrMethodID = @PartID And pcrMethodRevisionID = @RevisionID AND pcrMethodAssemblyID = @AsmID AND pcrMethodMaterialID = @MaterialID";

	private Task<IList<int>> GetParentAsms(string part, string partRevision, int baseAsmId)
	{
		IList<int> list = new List<int>();
		InitializeParameterLists();
		if (baseAsmId == 0)
		{
			base.selectList.AddRange(new string[1] { " DISTINCT imaParentAssemblyID" });
			base.filterList.Add("imaMethodID|C", part);
			base.filterList.Add("imaMethodRevisionID|C", partRevision);
		}
		else
		{
			base.selectList.AddRange(new string[1] { "imaParentAssemblyID" });
			base.filterList.Add("imaMethodID|C", part);
			base.filterList.Add("imaMethodRevisionID|C", partRevision);
			base.filterList.Add("imaMethodAssemblyID", baseAsmId);
		}
		using (DataTable dataTable = GetAsDataTable("PartAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(row.Field<int>("imaParentAssemblyID"));
				}
			}
		}
		return Task.FromResult(list);
	}

	private Task<IList<int>> GetMethodAsmsToParentAsm(string part, string partRevision, int parentAsm, int baseAsm)
	{
		IList<int> list = new List<int>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[1] { " DISTINCT imaMethodAssemblyID,imaParentAssemblyID" });
		if (baseAsm == 0)
		{
			base.filterList.Add("imaMethodID|C", part);
			base.filterList.Add("imaMethodRevisionID|C", partRevision);
			base.filterList.Add("imaParentAssemblyID", parentAsm);
		}
		else
		{
			base.filterList.Add("imaMethodID|C", part);
			base.filterList.Add("imaMethodRevisionID|C", partRevision);
		}
		using (DataTable dataTable = GetAsDataTable("PartAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataRowCollection rows = dataTable.Rows;
				if (baseAsm != 0)
				{
					rows = (from x in dataTable.AsEnumerable()
						where x.Field<int>("imaParentAssemblyID") == baseAsm || x.Field<int>("imaMethodAssemblyID") == baseAsm
						select x).CopyToDataTable().Rows;
				}
				foreach (DataRow item in rows)
				{
					list.Add(item.Field<int>("imaMethodAssemblyID"));
				}
			}
		}
		return Task.FromResult(list);
	}

	public PartRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public PartRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesRequirePartsToExistInventory()
	{
		return Task.FromResult(Convert.ToBoolean(base.M1database.Props("FN")["xafPartsMustExist"].ToString().Trim()));
	}

	public Task<bool> DoesPartExists(string partId)
	{
		InitializeParameterLists();
		base.filterList.Add("impPartID|C", partId);
		base.selectList.Add("impPartID");
		return Task.FromResult(GetAsObject("Parts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesPartClassExists(string partClassID)
	{
		InitializeParameterLists();
		base.filterList.Add("imcPartClassID|C", partClassID);
		base.filterList.Add("imcInactive", 0);
		base.selectList.Add("imcPartClassID");
		return Task.FromResult(GetAsObject("PartClasses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesPartGroupExists(string partGroupID)
	{
		InitializeParameterLists();
		base.filterList.Add("imuPartGroupID|C", partGroupID);
		base.filterList.Add("imuInactive", 0);
		base.selectList.Add("imuPartGroupID");
		return Task.FromResult(GetAsObject("PartGroups", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesMethodRevisionExists(string methodID, string methodRevisionID)
	{
		InitializeParameterLists();
		base.filterList.Add("imaMethodID|C", methodID);
		base.filterList.Add("imaMethodRevisionID|C", methodRevisionID);
		base.selectList.Add("imaMethodID");
		return Task.FromResult(GetAsObject("PartAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesMethodAssemblyExists(string methodID, string methodRevisionID, int parentAssemblyID)
	{
		InitializeParameterLists();
		base.filterList.Add("imaMethodID|C", methodID);
		base.filterList.Add("imaMethodRevisionID|C", methodRevisionID);
		base.filterList.Add("imaMethodAssemblyID", parentAssemblyID);
		base.selectList.Add("imaMethodID");
		return Task.FromResult(GetAsObject("PartAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesMethodAssemblyOperationExists(string methodID, string methodRevisionID, int methodAssemblyID, int methodOperationID)
	{
		InitializeParameterLists();
		base.filterList.Add("imoMethodID|C", methodID);
		base.filterList.Add("imoMethodRevisionID|C", methodRevisionID);
		base.filterList.Add("imoMethodAssemblyID", methodAssemblyID);
		base.filterList.Add("imoMethodOperationID", methodOperationID);
		base.selectList.Add("imoMethodID");
		return Task.FromResult(GetAsObject("PartOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesMethodAssemblyMaterialExists(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialID)
	{
		InitializeParameterLists();
		base.filterList.Add("immMethodID|C", methodId);
		base.filterList.Add("immMethodRevisionID|C", methodRevisionId);
		base.filterList.Add("immMethodAssemblyID", methodAssemblyId);
		base.filterList.Add("immMethodMaterialID", methodMaterialID);
		base.selectList.Add("immMethodID");
		return Task.FromResult(GetAsObject("PartMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesMethodAssemblyMaterialExists(string methodId, string methodRevisionId, int methodAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("immMethodID|C", methodId);
		base.filterList.Add("immMethodRevisionID|C", methodRevisionId);
		base.filterList.Add("immMethodAssemblyID", methodAssemblyId);
		base.selectList.Add("immMethodID");
		return Task.FromResult(GetAsObject("PartMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> IsUseMethod_MethodAssembly(string methodID, string methodRevisionID, int assemblyID)
	{
		InitializeParameterLists();
		base.filterList.Add("imaMethodID|C", methodID);
		base.filterList.Add("imaMethodRevisionID|C", methodRevisionID);
		base.filterList.Add("imaMethodAssemblyID", assemblyID);
		base.selectList.Add("imaUseMethod");
		return Task.FromResult(Convert.ToBoolean(GetAsObject("PartAssemblies", base.filterList, base.selectList, null, null)));
	}

	public Task<bool> DoesPartRevisionExists(string partID, string partRevisionID)
	{
		InitializeParameterLists();
		base.filterList.Add("imrPartID|C", partID);
		base.filterList.Add("imrPartRevisionID|C", partRevisionID);
		base.filterList.Add("imrInactive", 0);
		base.selectList.Add("imrPartID");
		return Task.FromResult(GetAsObject("PartRevisions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesWorkCenterExists(string workCenterID)
	{
		InitializeParameterLists();
		base.filterList.Add("xawWorkCenterID|C", workCenterID);
		base.filterList.Add("xawInactive", 0);
		base.selectList.Add("xawWorkCenterID");
		return Task.FromResult(GetAsObject("WorkCenters", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesProcessExists(string processID)
	{
		InitializeParameterLists();
		base.filterList.Add("xacProcessID|C", processID);
		base.filterList.Add("xacInactive", 0);
		base.selectList.Add("xacProcessID");
		return Task.FromResult(GetAsObject("Processes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<string> GetPartIdFromPartOrgReference(string partID, string organizationId)
	{
		new PartInformationDto();
		string result = string.Empty;
		InitializeParameterLists();
		base.filterList.Add("imzOrgPartID|C", partID);
		if (!string.IsNullOrWhiteSpace(organizationId))
		{
			base.filterList.Add("imzOrganizationID|C", organizationId);
		}
		base.selectList.Add("imzPartID");
		using (DataTable dataTable = GetAsDataTable("PartOrgReferences", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0].Field<string>("imzPartID") ?? string.Empty;
			}
		}
		return Task.FromResult(result);
	}

	public Task<PartInformationDto> GetPartInfo(string partID)
	{
		PartInformationDto partInformationDto = new PartInformationDto();
		InitializeParameterLists();
		base.selectList.AddRange(new string[15]
		{
			"impPartID", "impShortDescription", "impPartGroupID", "impLongDescriptionText", "impDeliveryType", "impTaxCodeID", "impSecondTaxCodeID", "impNonTaxReasonID", "impAlwaysNonTaxable", "impPartType",
			"impPartClassID", "impCreatedBy", "impCreatedDate", "impBuyForInventory", "impNonStockedItem"
		});
		base.filterList.Add(Guid.TryParse(partID, out var _) ? "impUniqueID|C" : "impPartID|C", partID);
		using (DataTable dataTable = GetAsDataTable("Parts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(partInformationDto);
			}
			partInformationDto.PartID = dataTable.Rows[0]["impPartID"].ToString().Trim();
			partInformationDto.PartGroupID = dataTable.Rows[0]["impPartGroupID"].ToString().Trim();
			partInformationDto.PartShortDescription = dataTable.Rows[0]["impShortDescription"].ToString().Trim();
			partInformationDto.PartLongDescriptionText = dataTable.Rows[0]["impLongDescriptionText"].ToString().Trim();
			partInformationDto.DeliveryType = Convert.ToByte(dataTable.Rows[0]["impDeliveryType"].ToString().Trim());
			partInformationDto.PartTaxCodeID = dataTable.Rows[0]["impTaxCodeID"].ToString().Trim();
			partInformationDto.PartSecondTaxCodeID = dataTable.Rows[0]["impSecondTaxCodeID"].ToString().Trim();
			partInformationDto.PartNonTaxReasonID = dataTable.Rows[0]["impNonTaxReasonID"].ToString().Trim();
			partInformationDto.PartAlwaysNonTaxable = Convert.ToInt16(dataTable.Rows[0]["impAlwaysNonTaxable"]) != 0;
			partInformationDto.PartType = Convert.ToByte(dataTable.Rows[0]["impPartType"]);
			partInformationDto.PartClassID = dataTable.Rows[0]["impPartClassID"].ToString().Trim();
			partInformationDto.CreatedBy = dataTable.Rows[0]["impCreatedBy"].ToString().Trim();
			partInformationDto.CreatedDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(dataTable.Rows[0]["impCreatedDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[0]["impCreatedDate"].ToString()));
			partInformationDto.BuyForInventory = Convert.ToInt16(dataTable.Rows[0]["impBuyForInventory"]) != 0;
			partInformationDto.NonStockedItem = Convert.ToInt16(dataTable.Rows[0]["impNonStockedItem"]) != 0;
		}
		return Task.FromResult(partInformationDto);
	}

	public Task<PartRevisionInformationDto> GetPartRevisionInfo(string partId, string partRevisionId)
	{
		new DataTable();
		PartRevisionInformationDto partRevisionInformationDto = null;
		InitializeParameterLists();
		base.filterList.Add("@p1", partId);
		base.filterList.Add("@p2", partRevisionId);
		using (DataTable dataTable = GetAsDataTable(GET_ACTIVE_PART_REVISION, base.filterList, null))
		{
			partRevisionInformationDto = ((dataTable == null || dataTable.Rows.Count <= 0) ? new PartRevisionInformationDto
			{
				PartID = ""
			} : new PartRevisionInformationDto
			{
				PartID = partId,
				PartRevisionID = partRevisionId,
				InventoryUnitOfMeasure = dataTable.Rows[0]["imrInventoryUnitOfMeasure"].ToString(),
				Weight = Convert.ToDecimal(dataTable.Rows[0]["imrWeight"].ToString()),
				EasyOrderPartID = dataTable.Rows[0]["imrEasyOrderPartID"].ToString().Trim(),
				BlanketPeriodBegin = (string.IsNullOrWhiteSpace(dataTable.Rows[0]["imrBlanketPeriodBegin"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[0]["imrBlanketPeriodBegin"].ToString())),
				BlanketPeriodEnd = (string.IsNullOrWhiteSpace(dataTable.Rows[0]["imrBlanketPeriodEnd"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[0]["imrBlanketPeriodEnd"].ToString())),
				NetCostBeginDate = (string.IsNullOrWhiteSpace(dataTable.Rows[0]["imrNetCostBeginDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[0]["imrNetCostBeginDate"].ToString())),
				NetCostEndDate = (string.IsNullOrWhiteSpace(dataTable.Rows[0]["imrNetCostEndDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[0]["imrNetCostEndDate"].ToString())),
				NetCostCode = dataTable.Rows[0]["imrNetCostCode"].ToString().Trim(),
				PreferenceCriteria = dataTable.Rows[0]["imrPreferenceCriteria"].ToString().Trim(),
				ProducerDetermination = dataTable.Rows[0]["imrProducerDetermination"].ToString().Trim(),
				CommodityCode = dataTable.Rows[0]["imrCommodityCode"].ToString().Trim(),
				CountryOfManufacture = dataTable.Rows[0]["imrCountryOfManufacture"].ToString().Trim(),
				WeightUnitOfMeasure = dataTable.Rows[0]["imrWeightUnitOfMeasure"].ToString().Trim(),
				PartShortDescription = dataTable.Rows[0]["imrShortDescription"].ToString().Trim(),
				PartLongDescriptionText = dataTable.Rows[0]["imrLongDescriptionText"].ToString().Trim(),
				PurchaseUnitOfMeasure = dataTable.Rows[0]["imrPurchaseUnitOfMeasure"].ToString().Trim(),
				SupplierOrganizationID = dataTable.Rows[0]["imrSupplierOrganizationID"].ToString().Trim(),
				ConversionFactor = Convert.ToDecimal(dataTable.Rows[0]["imrConversionFactor"].ToString().Trim()),
				EffectiveStartDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(dataTable.Rows[0]["imrEffectiveStartDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[0]["imrEffectiveStartDate"].ToString())),
				EffectiveEndDate = (string.IsNullOrWhiteSpace(dataTable.Rows[0]["imrEffectiveEndDate"].ToString()) ? ((DateTime?)null) : new DateTime?(DateTime.Parse(dataTable.Rows[0]["imrEffectiveEndDate"].ToString())))
			});
		}
		return Task.FromResult(partRevisionInformationDto ?? new PartRevisionInformationDto());
	}

	public Task<IList<PartRevisionInformationDto>> GetPartRevisionsInfo(string partId)
	{
		IList<PartRevisionInformationDto> list = new List<PartRevisionInformationDto>();
		InitializeParameterLists();
		base.filterList.Add("@p1", partId);
		using (DataTable dataTable = GetAsDataTable(GET_ACTIVE_PART_REVISIONS, base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				DateTime result;
				DateTime value = (DateTime.TryParse(row["imrEffectiveEndDate"]?.ToString(), out result) ? result : DateTime.MinValue);
				DateTime result2;
				DateTime value2 = (DateTime.TryParse(row["imrLastTransactionDate"]?.ToString(), out result2) ? result2 : DateTime.MinValue);
				DateTime result3;
				DateTime value3 = (DateTime.TryParse(row["imrLastReceiptDate"]?.ToString(), out result3) ? result3 : DateTime.MinValue);
				PartRevisionInformationDto item = new PartRevisionInformationDto
				{
					PartID = partId,
					PartRevisionID = row["imrPartRevisionID"].ToString().Trim(),
					InventoryUnitOfMeasure = row["imrInventoryUnitOfMeasure"].ToString(),
					Weight = Convert.ToDecimal(row["imrWeight"].ToString()),
					EasyOrderPartID = row["imrEasyOrderPartID"].ToString().Trim(),
					BlanketPeriodBegin = (string.IsNullOrWhiteSpace(row["imrBlanketPeriodBegin"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["imrBlanketPeriodBegin"].ToString())),
					BlanketPeriodEnd = (string.IsNullOrWhiteSpace(row["imrBlanketPeriodEnd"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["imrBlanketPeriodEnd"].ToString())),
					NetCostBeginDate = (string.IsNullOrWhiteSpace(row["imrNetCostBeginDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["imrNetCostBeginDate"].ToString())),
					NetCostEndDate = (string.IsNullOrWhiteSpace(row["imrNetCostEndDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["imrNetCostEndDate"].ToString())),
					NetCostCode = row["imrNetCostCode"].ToString().Trim(),
					PreferenceCriteria = row["imrPreferenceCriteria"].ToString().Trim(),
					ProducerDetermination = row["imrProducerDetermination"].ToString().Trim(),
					CommodityCode = row["imrCommodityCode"].ToString().Trim(),
					CountryOfManufacture = row["imrCountryOfManufacture"].ToString().Trim(),
					WeightUnitOfMeasure = row["imrWeightUnitOfMeasure"].ToString().Trim(),
					PartShortDescription = row["imrShortDescription"].ToString().Trim(),
					PartLongDescriptionText = row["imrLongDescriptionText"].ToString().Trim(),
					PurchaseUnitOfMeasure = row["imrPurchaseUnitOfMeasure"].ToString().Trim(),
					SupplierOrganizationID = row["imrSupplierOrganizationID"].ToString().Trim(),
					ConversionFactor = Convert.ToDecimal(row["imrConversionFactor"].ToString().Trim()),
					EffectiveStartDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(row["imrEffectiveStartDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["imrEffectiveStartDate"].ToString())),
					CreatedBy = row["imrCreatedBy"].ToString().Trim(),
					CreatedDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(row["imrCreatedDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["imrCreatedDate"].ToString())),
					LeadTime = Convert.ToInt16(row["imrLeadTime"].ToString().Trim()),
					EffectiveEndDate = value,
					PurchaseLocationId = row["imrPurchaseLocationID"].ToString().Trim(),
					LastDutyCost = Convert.ToDecimal(row["imrLastDutyCost"].ToString().Trim()),
					LastMiscCost = Convert.ToDecimal(row["imrLastMiscCost"].ToString().Trim()),
					LastLaborCost = Convert.ToDecimal(row["imrLastLaborCost"].ToString().Trim()),
					AverageMiscCost = Convert.ToDecimal(row["imrAverageMiscCost"].ToString().Trim()),
					AverageDutyCost = Convert.ToDecimal(row["imrAverageDutyCost"].ToString().Trim()),
					LastFreightCost = Convert.ToDecimal(row["imrLastFreightCost"].ToString().Trim()),
					LastOverheadCost = Convert.ToDecimal(row["imrLastOverheadCost"].ToString().Trim()),
					LastMaterialCost = Convert.ToDecimal(row["imrLastMaterialCost"].ToString().Trim()),
					AverageLaborCost = Convert.ToDecimal(row["imrAverageLaborCost"].ToString().Trim()),
					StandardDutyCost = Convert.ToDecimal(row["imrStandardDutyCost"].ToString().Trim()),
					StandardMiscCost = Convert.ToDecimal(row["imrStandardMiscCost"].ToString().Trim()),
					StandardLaborCost = Convert.ToDecimal(row["imrStandardLaborCost"].ToString().Trim()),
					AverageFreightCost = Convert.ToDecimal(row["imrAverageFreightCost"].ToString().Trim()),
					AverageOverheadCost = Convert.ToDecimal(row["imrAverageOverheadCost"].ToString().Trim()),
					LastSubcontractCost = Convert.ToDecimal(row["imrLastSubcontractCost"].ToString().Trim()),
					AverageMaterialCost = Convert.ToDecimal(row["imrAverageMaterialCost"].ToString().Trim()),
					StandardFreightCost = Convert.ToDecimal(row["imrStandardFreightCost"].ToString().Trim()),
					StandardOverheadCost = Convert.ToDecimal(row["imrStandardOverheadCost"].ToString().Trim()),
					StandardMaterialCost = Convert.ToDecimal(row["imrStandardMaterialCost"].ToString().Trim()),
					AverageSubcontractCost = Convert.ToDecimal(row["imrAverageSubcontractCost"].ToString().Trim()),
					StandardSubcontractCost = Convert.ToDecimal(row["imrStandardSubcontractCost"].ToString().Trim()),
					LastTransactionDate = value2,
					ExpenseSplitPercentTotal = Convert.ToDecimal(row["imrExpenseSplitPercentTotal"].ToString().Trim()),
					Inactive = Convert.ToBoolean(Convert.ToInt16(row["imrInactive"])),
					RequiresInspection = Convert.ToBoolean(Convert.ToInt16(row["imrRequiresInspection"])),
					LastReceiptDate = value3,
					ManufacturingLotSize = Convert.ToDecimal(row["imrManufacturingLotSize"].ToString().Trim()),
					AverageUnitCost = Convert.ToDecimal(row["averageUnitCost"].ToString()),
					StandardUnitCost = Convert.ToDecimal(row["standardUnitCost"].ToString()),
					LastUnitCost = Convert.ToDecimal(row["lastUnitCost"].ToString()),
					SheetSizeX = Convert.ToDecimal(row["imrSheetSizeX"].ToString()),
					SheetSizeY = Convert.ToDecimal(row["imrSheetSizeY"].ToString()),
					BarLength = Convert.ToDecimal(row["imrBarLength"].ToString()),
					Thickness = Convert.ToDecimal(row["imrThickness"].ToString().Trim())
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}

	public Task<decimal> GetFullUnitPriceBase(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, decimal quantity, string currencyID, DateTime? priceDate)
	{
		return Task.FromResult(new Part().GetSellingPrice(base.M1database, partID, partRevisionID, partGroupID, orgID, locationID, quantity, currencyID, priceDate).FullPrice);
	}

	public Task<PriceCalculation> GetPartPrice(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, decimal quantity, string currencyID, DateTime? priceDate)
	{
		return Task.FromResult(new Part().GetSellingPrice(base.M1database, partID, partRevisionID, partGroupID, orgID, locationID, quantity, currencyID, priceDate));
	}

	public void IntializePartMethodLists()
	{
		UseMethodAssemblyList = new List<BOMPartAssemblyDto>();
		MethodAssemblyList = new List<BOMPartMethodAssemblyDto>();
	}

	public Task<IList<BOMPartMethodAssemblyDto>> GetPartMethodAssemblyList(string part, string partRevision, int baseAsmId)
	{
		IList<int> result = GetParentAsms(part, partRevision, baseAsmId).Result;
		List<BOMPartAssemblyDto> list = new List<BOMPartAssemblyDto>();
		foreach (int item in result)
		{
			foreach (int item2 in GetMethodAsmsToParentAsm(part, partRevision, item, baseAsmId).Result)
			{
				BOMPartAssemblyDto result2 = GetMethodAssemblyInfo(part, partRevision, item2).Result;
				List<BOMPartOperationDto> partOperations = new List<BOMPartOperationDto>();
				List<BOMPartMaterialDto> partMaterials = new List<BOMPartMaterialDto>();
				if (!result2.UseMethod)
				{
					partOperations = (List<BOMPartOperationDto>)GetMethodOerationsForAsm(part, partRevision, item2).Result;
					partMaterials = (List<BOMPartMaterialDto>)GetMethodMaterialsForAsm(part, partRevision, item2).Result;
				}
				else
				{
					list.Add(result2);
				}
				MethodAssemblyList.Add(new BOMPartMethodAssemblyDto
				{
					PartAssembly = result2,
					PartOperations = partOperations,
					PartMaterials = partMaterials
				});
			}
		}
		if (list.Count > 0)
		{
			foreach (BOMPartAssemblyDto item3 in new List<BOMPartAssemblyDto>(list))
			{
				list.Remove(item3);
				GetPartMethodAssemblyList(item3.PartID, item3.PartRevisionID, 0);
			}
		}
		return Task.FromResult(MethodAssemblyList);
	}

	public Task<BOMPartAssemblyDto> GetMethodAssemblyInfo(string part, string partRevision, int methodAsm)
	{
		BOMPartAssemblyDto bOMPartAssemblyDto = new BOMPartAssemblyDto();
		InitializeParameterLists();
		base.selectList.AddRange(PartAssemblyFields);
		base.filterList.Add("imaMethodID|C", part);
		base.filterList.Add("imaMethodRevisionID|C", partRevision);
		base.filterList.Add("imaMethodAssemblyID", methodAsm);
		using (DataTable dataTable = GetAsDataTable("PartAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMPartAssemblyDto);
			}
			bOMPartAssemblyDto.Level = Convert.ToInt16(dataTable.Rows[0][PartAssemblyFields[3]]);
			bOMPartAssemblyDto.PartID = dataTable.Rows[0][PartAssemblyFields[6]].ToString().Trim();
			bOMPartAssemblyDto.MethodID = dataTable.Rows[0][PartAssemblyFields[0]].ToString().Trim();
			bOMPartAssemblyDto.UseMethod = Convert.ToBoolean(dataTable.Rows[0][PartAssemblyFields[4]]);
			bOMPartAssemblyDto.UnitOfMeasure = dataTable.Rows[0][PartAssemblyFields[9]].ToString().Trim();
			bOMPartAssemblyDto.PartRevisionID = dataTable.Rows[0][PartAssemblyFields[7]].ToString().Trim();
			bOMPartAssemblyDto.MethodRevisionID = dataTable.Rows[0][PartAssemblyFields[1]].ToString().Trim();
			bOMPartAssemblyDto.MethodAssemblyID = Convert.ToInt32(dataTable.Rows[0][PartAssemblyFields[2]]);
			bOMPartAssemblyDto.ParentAssemblyID = Convert.ToInt32(dataTable.Rows[0][PartAssemblyFields[5]]);
			bOMPartAssemblyDto.QuantityPerParent = Convert.ToDecimal(dataTable.Rows[0][PartAssemblyFields[10]]);
			bOMPartAssemblyDto.OverlapOperationId = Convert.ToInt32(dataTable.Rows[0][PartAssemblyFields[11]].ToString().Trim());
			bOMPartAssemblyDto.PartLongDescription = dataTable.Rows[0][PartAssemblyFields[12]].ToString().Trim();
			bOMPartAssemblyDto.PartShortDescription = dataTable.Rows[0][PartAssemblyFields[8]].ToString().Trim();
		}
		return Task.FromResult(bOMPartAssemblyDto);
	}

	public Task<IList<BOMPartOperationDto>> GetMethodOerationsForAsm(string part, string partRevision, int methodAsm)
	{
		IList<BOMPartOperationDto> list = new List<BOMPartOperationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[13]
		{
			"imoMethodID", "imoMethodRevisionID", "imoMethodAssemblyID", "imoOperationType", "imoMethodOperationID", "imoWorkCenterID", "imoProcessID", "imoProcessShortDescription", "imoProductionStandard", "imoStandardFactor",
			"imoMachinesToSchedule", "imoMachineType", "imoQuantityPerAssembly"
		});
		base.filterList.Add("imoMethodID|C", part);
		base.filterList.Add("imoMethodRevisionID|C", partRevision);
		base.filterList.Add("imoMethodAssemblyID", methodAsm);
		using (DataTable dataTable = GetAsDataTable("PartOperations", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new BOMPartOperationDto
					{
						MethodID = row.Field<string>("imoMethodID"),
						MethodRevisionID = row.Field<string>("imoMethodRevisionID"),
						MethodAssemblyID = row.Field<int>("imoMethodAssemblyID"),
						OperationType = row.Field<byte>("imoOperationType"),
						MethodOperationID = row.Field<int>("imoMethodOperationID"),
						WorkCenterID = row.Field<string>("imoWorkCenterID"),
						ProcessID = row.Field<string>("imoProcessID"),
						ProcessShortDescription = row.Field<string>("imoProcessShortDescription"),
						ProductionStandard = row.Field<decimal>("imoProductionStandard"),
						StandardFactor = row.Field<string>("imoStandardFactor"),
						MachinesToSchedule = row.Field<short>("imoMachinesToSchedule"),
						MachineType = row.Field<byte>("imoMachineType"),
						QuantityPerAssembly = row.Field<decimal>("imoQuantityPerAssembly")
					});
				}
			}
		}
		return Task.FromResult(list);
	}

	public Task<IList<BOMPartMaterialDto>> GetMethodMaterialsForAsm(string part, string partRevision, int methodAsm)
	{
		IList<BOMPartMaterialDto> list = new List<BOMPartMaterialDto>();
		InitializeParameterLists();
		base.selectList.AddRange(PartMaterialFields);
		base.filterList.Add("immMethodID|C", part);
		base.filterList.Add("immMethodRevisionID|C", partRevision);
		base.filterList.Add("immMethodAssemblyID", methodAsm);
		using (DataTable dataTable = GetAsDataTable("PartMaterials", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				list.Add(new BOMPartMaterialDto
				{
					MethodID = row.Field<string>(PartMaterialFields[0]),
					MethodRevisionID = row.Field<string>(PartMaterialFields[1]),
					MethodAssemblyID = row.Field<int>(PartMaterialFields[2]),
					MethodMaterialID = row.Field<int>(PartMaterialFields[3]),
					PartID = row.Field<string>(PartMaterialFields[4]),
					PartRevisionID = row.Field<string>(PartMaterialFields[5]),
					RelatedPartOperationID = row.Field<int>(PartMaterialFields[6]),
					PartShortDescription = row.Field<string>(PartMaterialFields[7]),
					UnitOfMeasure = row.Field<string>(PartMaterialFields[8]),
					EstimatedUnitCost = row.Field<decimal>(PartMaterialFields[9]),
					LeadTime = row.Field<short>(PartMaterialFields[10]),
					SupplierOrganizationID = row.Field<string>(PartMaterialFields[11]),
					PurchaseLocationID = row.Field<string>(PartMaterialFields[12]),
					QuantityPerAssembly = row.Field<decimal>(PartMaterialFields[13]),
					ManualPart = row.Field<bool>(PartMaterialFields[14]),
					PartLongDescription = row.Field<string>(PartMaterialFields[15]),
					BackFlush = row.Field<bool>(PartMaterialFields[16]),
					ScrapQuantity = row.Field<decimal>(PartMaterialFields[17]),
					ScrapPercent = row.Field<decimal>(PartMaterialFields[18])
				});
			}
		}
		return Task.FromResult(list);
	}

	public Task<CTMPartClassesDto> GetAllPartClasses()
	{
		CTMPartClassesDto cTMPartClassesDto = new CTMPartClassesDto();
		List<PartClassDto> list = new List<PartClassDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[3] { "imcPartClassID", "imcDescription", "imcInactive" });
		base.filterList.Add("imcInactive", 0);
		base.OrderOrGroupByList.Add("imcPartClassID ASC");
		using (DataTable dataTable = GetAsDataTable("PartClasses", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new PartClassDto
					{
						PartClassID = row["imcPartClassID"].ToString().Trim(),
						Description = row["imcDescription"].ToString().Trim()
					});
				}
			}
		}
		cTMPartClassesDto.PartClasses = list;
		return Task.FromResult(cTMPartClassesDto);
	}

	public Task<CTMPartGroupsDto> GetAllPartGroups()
	{
		CTMPartGroupsDto cTMPartGroupsDto = new CTMPartGroupsDto();
		List<PartGroupDto> list = new List<PartGroupDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[3] { "imuPartGroupID", "imuDescription", "imuInactive" });
		base.filterList.Add("imuInactive", 0);
		base.OrderOrGroupByList.Add("imuPartGroupID ASC");
		using (DataTable dataTable = GetAsDataTable("PartGroups", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new PartGroupDto
					{
						PartGroupID = row["imuPartGroupID"].ToString().Trim(),
						Description = row["imuDescription"].ToString().Trim()
					});
				}
			}
		}
		cTMPartGroupsDto.PartGroups = list;
		return Task.FromResult(cTMPartGroupsDto);
	}

	public Task<string> GetPartIdFromGuid(Guid guidOut)
	{
		InitializeParameterLists();
		base.filterList.Add("impUniqueID|C", guidOut);
		base.selectList.Add("impPartID");
		return Task.FromResult(GetAsObject("Parts", base.filterList, base.selectList, null, null)?.ToString());
	}

	public async Task<APIValidationInfoDto> SavePart(BOMPartDto partDto)
	{
		SqlTransaction sqlTransaction = null;
		StringBuilder stringBuilder = new StringBuilder();
		APIValidationInfoDto result = new APIValidationInfoDto();
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, sqlTransaction);
			m1BindingSource.ClearCache();
			m1BindingSource.DataSourceTable = "Parts";
			stringBuilder.Append("impPartID= " + M1Util.ConvertToLinq(partDto.PartID));
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				if (dataRow != null)
				{
					dataRow["impPartID"] = partDto.PartID.ToUpper();
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["impPartGroupID"] = partDto.PartGroupID ?? dataRow["impPartGroupID"];
			dataRow["impPartClassID"] = partDto.PartClassID ?? dataRow["impPartClassID"];
			dataRow["impPartType"] = ((partDto.PartType != 0) ? ((object)partDto.PartType) : dataRow["impPartType"]);
			dataRow["impShortDescription"] = partDto.ShortDescription ?? dataRow["impShortDescription"];
			dataRow["impLongDescriptionText"] = partDto.LongDescription ?? dataRow["impLongDescriptionText"];
			dataRow["impLongDescriptionRTF"] = partDto.LongDescription ?? dataRow["impLongDescriptionRTF"];
			DataRow dataRow2 = dataRow;
			byte? deliveryType = partDto.DeliveryType;
			dataRow2["impDeliveryType"] = (deliveryType.HasValue ? ((object)deliveryType.GetValueOrDefault()) : dataRow["impDeliveryType"]);
			DataRow dataRow3 = dataRow;
			bool? buyForInventory = partDto.BuyForInventory;
			dataRow3["impBuyForInventory"] = (buyForInventory.HasValue ? ((object)(buyForInventory == true)) : dataRow["impBuyForInventory"]);
			DataRow dataRow4 = dataRow;
			buyForInventory = partDto.NonStockedItem;
			dataRow4["impNonStockedItem"] = (buyForInventory.HasValue ? ((object)(buyForInventory == true)) : dataRow["impNonStockedItem"]);
			m1BindingSource.SaveData();
			base.M1database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the Part [" + partDto.PartID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
			base.M1database.RollbackTransaction(sqlTransaction);
		}
		finally
		{
			sqlTransaction.Dispose();
		}
		return result;
	}

	public Task<APIValidationInfoDto> SavePartRevision(CTMBOMPartRevisionDto bomPartRevision)
	{
		DataRow dataRow = null;
		DataRow dataRow2 = null;
		SqlTransaction sqlTransaction = null;
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		BOMPartDto part = bomPartRevision.Part;
		List<BOMPartRevisionDto> partRevisions = bomPartRevision.PartRevisions;
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, sqlTransaction);
			m1BindingSource.ClearCache();
			m1BindingSource.DataSourceTable = "Parts";
			stringBuilder.Append("impPartID= " + M1Util.ConvertToLinq(part.PartID));
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				if (dataRow != null)
				{
					dataRow["impPartID"] = part.PartID.ToUpper();
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["impPartGroupID"] = part.PartGroupID ?? dataRow["impPartGroupID"];
			dataRow["impPartClassID"] = part.PartClassID ?? dataRow["impPartClassID"];
			dataRow["impPartType"] = ((part.PartType != 0) ? ((object)part.PartType) : dataRow["impPartType"]);
			dataRow["impShortDescription"] = part.ShortDescription ?? dataRow["impShortDescription"];
			dataRow["impLongDescriptionText"] = part.LongDescription ?? dataRow["impLongDescriptionText"];
			dataRow["impLongDescriptionRTF"] = part.LongDescription ?? dataRow["impLongDescriptionRTF"];
			DataRow dataRow3 = dataRow;
			byte? deliveryType = part.DeliveryType;
			dataRow3["impDeliveryType"] = (deliveryType.HasValue ? ((object)deliveryType.GetValueOrDefault()) : dataRow["impDeliveryType"]);
			DataRow dataRow4 = dataRow;
			bool? buyForInventory = part.BuyForInventory;
			dataRow4["impBuyForInventory"] = (buyForInventory.HasValue ? ((object)(buyForInventory == true)) : dataRow["impBuyForInventory"]);
			DataRow dataRow5 = dataRow;
			buyForInventory = part.NonStockedItem;
			dataRow5["impNonStockedItem"] = (buyForInventory.HasValue ? ((object)(buyForInventory == true)) : dataRow["impNonStockedItem"]);
			m1BindingSource.SaveData();
			if (partRevisions.Count > 0)
			{
				stringBuilder.Length = 0;
				using M1BindingSource m1BindingSource2 = m1BindingSource.PrimaryTable.GetChildBindingSource("PartRevisions");
				foreach (BOMPartRevisionDto item in partRevisions)
				{
					m1BindingSource2.ClearCache();
					stringBuilder.Length = 0;
					stringBuilder.Append("imrPartID= " + M1Util.ConvertToLinq(item.PartID) + " And imrPartRevisionID= " + M1Util.ConvertToLinq(item.PartRevisionID));
					m1BindingSource2.NavigateTo(stringBuilder.ToString());
					if (m1BindingSource2.Count == 0)
					{
						dataRow2 = m1BindingSource2.AddNew() as DataRow;
						dataRow2["imrPartID"] = item.PartID.ToUpper();
						dataRow2["imrPartRevisionID"] = item.PartRevisionID.ToUpper();
					}
					else
					{
						dataRow2 = m1BindingSource2.CurrentAsDataRow;
					}
					dataRow2["imrShortDescription"] = item.ShortDescription ?? dataRow2["imrShortDescription"];
					dataRow2["imrInventoryUnitOfMeasure"] = item.InventoryUnitOfMeasure ?? dataRow2["imrInventoryUnitOfMeasure"];
					dataRow2["imrPurchaseUnitOfMeasure"] = item.PurchaseUnitOfMeasure ?? dataRow2["imrPurchaseUnitOfMeasure"];
					dataRow2["imrSupplierOrganizationID"] = item.SupplierOrganizationID ?? dataRow2["imrSupplierOrganizationID"];
					dataRow2["imrConversionFactor"] = ((item.ConversionFactor != 0m) ? item.ConversionFactor : 1m);
					dataRow2["imrEffectiveStartDate"] = ((item.EffectiveStartDate.HasValue && item.EffectiveStartDate.Value.Year > 2000) ? item.EffectiveStartDate : new DateTime?(DateTime.Today));
					dataRow2["imrLongDescriptionText"] = item.LongDescription ?? dataRow2["imrLongDescriptionText"];
					dataRow2["imrLongDescriptionRTF"] = item.LongDescription ?? dataRow2["imrLongDescriptionRTF"];
					dataRow2["imrLongDescriptionHTML"] = item.LongDescription ?? dataRow2["imrLongDescriptionHTML"];
					dataRow2["imrLeadTime"] = ((item.LeadTime != 0) ? item.LeadTime : 0);
					if (item.EffectiveEndDate.HasValue && item.EffectiveEndDate.Value.Year > 2000)
					{
						dataRow2["imrEffectiveEndDate"] = item.EffectiveEndDate;
					}
					dataRow2["imrPurchaseLocationID"] = item.PurchaseLocationId ?? dataRow2["imrPurchaseLocationID"];
					DataRow dataRow6 = dataRow2;
					decimal? averageLaborCost = item.AverageLaborCost;
					dataRow6["imrAverageLaborCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageLaborCost"]);
					DataRow dataRow7 = dataRow2;
					averageLaborCost = item.AverageOverheadCost;
					dataRow7["imrAverageOverheadCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageOverheadCost"]);
					DataRow dataRow8 = dataRow2;
					averageLaborCost = item.AverageMaterialCost;
					dataRow8["imrAverageMaterialCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageMaterialCost"]);
					DataRow dataRow9 = dataRow2;
					averageLaborCost = item.AverageSubcontractCost;
					dataRow9["imrAverageSubcontractCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageSubcontractCost"]);
					DataRow dataRow10 = dataRow2;
					averageLaborCost = item.LastLaborCost;
					dataRow10["imrLastLaborCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastLaborCost"]);
					DataRow dataRow11 = dataRow2;
					averageLaborCost = item.LastOverheadCost;
					dataRow11["imrLastOverheadCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastOverheadCost"]);
					DataRow dataRow12 = dataRow2;
					averageLaborCost = item.LastMaterialCost;
					dataRow12["imrLastMaterialCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastMaterialCost"]);
					DataRow dataRow13 = dataRow2;
					averageLaborCost = item.LastSubcontractCost;
					dataRow13["imrLastSubcontractCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastSubcontractCost"]);
					DataRow dataRow14 = dataRow2;
					averageLaborCost = item.StandardLaborCost;
					dataRow14["imrStandardLaborCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardLaborCost"]);
					DataRow dataRow15 = dataRow2;
					averageLaborCost = item.StandardOverheadCost;
					dataRow15["imrStandardOverheadCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardOverheadCost"]);
					DataRow dataRow16 = dataRow2;
					averageLaborCost = item.StandardMaterialCost;
					dataRow16["imrStandardMaterialCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardMaterialCost"]);
					DataRow dataRow17 = dataRow2;
					averageLaborCost = item.StandardSubcontractCost;
					dataRow17["imrStandardSubcontractCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardSubcontractCost"]);
					DataRow dataRow18 = dataRow2;
					averageLaborCost = item.AverageDutyCost;
					dataRow18["imrAverageDutyCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageDutyCost"]);
					DataRow dataRow19 = dataRow2;
					averageLaborCost = item.AverageFreightCost;
					dataRow19["imrAverageFreightCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageFreightCost"]);
					DataRow dataRow20 = dataRow2;
					averageLaborCost = item.AverageMiscCost;
					dataRow20["imrAverageMiscCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrAverageMiscCost"]);
					DataRow dataRow21 = dataRow2;
					averageLaborCost = item.LastDutyCost;
					dataRow21["imrLastDutyCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastDutyCost"]);
					DataRow dataRow22 = dataRow2;
					averageLaborCost = item.LastFreightCost;
					dataRow22["imrLastFreightCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastFreightCost"]);
					DataRow dataRow23 = dataRow2;
					averageLaborCost = item.LastMiscCost;
					dataRow23["imrLastMiscCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrLastMiscCost"]);
					DataRow dataRow24 = dataRow2;
					averageLaborCost = item.StandardDutyCost;
					dataRow24["imrStandardDutyCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardDutyCost"]);
					DataRow dataRow25 = dataRow2;
					averageLaborCost = item.StandardFreightCost;
					dataRow25["imrStandardFreightCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardFreightCost"]);
					DataRow dataRow26 = dataRow2;
					averageLaborCost = item.StandardMiscCost;
					dataRow26["imrStandardMiscCost"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrStandardMiscCost"]);
					DataRow dataRow27 = dataRow2;
					averageLaborCost = item.ManufacturingLotSize;
					dataRow27["imrManufacturingLotSize"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrManufacturingLotSize"]);
					DataRow dataRow28 = dataRow2;
					buyForInventory = item.RequiresInspection;
					dataRow28["imrRequiresInspection"] = (buyForInventory.HasValue ? ((object)(buyForInventory == true)) : dataRow2["imrRequiresInspection"]);
					dataRow2["imrCreatedBy"] = item.CreatedBy ?? dataRow2["imrCreatedBy"];
					DataRow dataRow29 = dataRow2;
					DateTime? createdDate = item.CreatedDate;
					dataRow29["imrCreatedDate"] = (createdDate.HasValue ? ((object)createdDate.GetValueOrDefault()) : dataRow2["imrCreatedDate"]);
					DataRow dataRow30 = dataRow2;
					averageLaborCost = item.Weight;
					dataRow30["imrWeight"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrWeight"]);
					dataRow2["imrWeightUnitOfMeasure"] = item.WeightUnitOfMeasure ?? dataRow2["imrWeightUnitOfMeasure"];
					DataRow dataRow31 = dataRow2;
					averageLaborCost = item.Thickness;
					dataRow31["imrThickness"] = (averageLaborCost.HasValue ? ((object)averageLaborCost.GetValueOrDefault()) : dataRow2["imrThickness"]);
					DataRow dataRow32 = dataRow2;
					buyForInventory = item.Inactive;
					dataRow32["imrInactive"] = (buyForInventory.HasValue ? ((object)(buyForInventory == true)) : dataRow2["imrInactive"]);
					m1BindingSource2.SaveData();
				}
			}
			base.M1database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the Part [" + part.PartID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
			base.M1database.RollbackTransaction(sqlTransaction);
		}
		finally
		{
			sqlTransaction.Dispose();
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> DeletePartOperation(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		SqlTransaction sqlTransaction = null;
		List<string> list = new List<string>();
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using (SqlCommand sqlCommand = base.M1database.NewSqlCommand(DELETE_PART_METHODOPERATION))
			{
				sqlCommand.Parameters.AddWithValue("@PartID", methodId);
				sqlCommand.Parameters.AddWithValue("@RevisionID", methodRevisionId);
				sqlCommand.Parameters.AddWithValue("@AsmID", methodAssemblyId);
				sqlCommand.Parameters.AddWithValue("@OprID", methodOperationId);
				base.M1database.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			base.M1database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			base.M1database.RollbackTransaction(sqlTransaction);
			list.Add("Error occurred [" + ex.Message + "] while processing the Part [" + methodId + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> DeletePartMaterial(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		SqlTransaction sqlTransaction = null;
		List<string> list = new List<string>();
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using (SqlCommand sqlCommand = base.M1database.NewSqlCommand(DELETE_PART_METHODOMATERIAL))
			{
				sqlCommand.Parameters.AddWithValue("@PartID", methodId);
				sqlCommand.Parameters.AddWithValue("@RevisionID", methodRevisionId);
				sqlCommand.Parameters.AddWithValue("@AsmID", methodAssemblyId);
				sqlCommand.Parameters.AddWithValue("@MaterialID", methodMaterialId);
				base.M1database.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			base.M1database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			base.M1database.RollbackTransaction(sqlTransaction);
			list.Add("Error occurred [" + ex.Message + "] while processing the Part [" + methodId + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> SavePartAssembly(BOMPartAssemblyDto partAssembly)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("imaMethodID = " + M1Util.ConvertToLinq(partAssembly.MethodID) + " And imaMethodRevisionID = " + M1Util.ConvertToLinq(partAssembly.MethodRevisionID) + " " + $"And imaMethodAssemblyID = {partAssembly.MethodAssemblyID}");
			m1BindingSource.DataSourceTable = "PartAssemblies";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["imaMethodID"] = partAssembly.MethodID.ToUpper();
				dataRow["imaMethodRevisionID"] = partAssembly.MethodRevisionID.ToUpper();
				dataRow["imaMethodAssemblyID"] = partAssembly.MethodAssemblyID;
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["imaLevel"] = partAssembly.Level;
			dataRow["imaPartID"] = partAssembly.PartID ?? dataRow["imaPartID"];
			dataRow["imaUseMethod"] = partAssembly.UseMethod;
			dataRow["imaUnitOfMeasure"] = partAssembly.UnitOfMeasure ?? dataRow["imaUnitOfMeasure"];
			dataRow["imaPartRevisionID"] = partAssembly.PartRevisionID ?? dataRow["imaPartRevisionID"];
			dataRow["imaParentAssemblyID"] = partAssembly.ParentAssemblyID;
			dataRow["imaQuantityPerParent"] = partAssembly.QuantityPerParent;
			DataRow dataRow2 = dataRow;
			int? overlapOperationId = partAssembly.OverlapOperationId;
			dataRow2["imaOverlapOperationID"] = (overlapOperationId.HasValue ? ((object)overlapOperationId.GetValueOrDefault()) : dataRow["imaOverlapOperationID"]);
			dataRow["imaPartShortDescription"] = partAssembly.PartShortDescription ?? dataRow["imaPartShortDescription"];
			dataRow["imaPartLongDescriptionRTF"] = partAssembly.PartLongDescription ?? dataRow["imaPartLongDescriptionRTF"];
			dataRow["imaPartLongDescriptionText"] = partAssembly.PartLongDescription ?? dataRow["imaPartLongDescriptionText"];
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the part method [" + partAssembly.MethodID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> SavePartOperation(BOMPartOperationDto partOperation)
	{
		DataRow dataRow = null;
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("imoMethodID = " + M1Util.ConvertToLinq(partOperation.MethodID) + " And imoMethodRevisionID = " + M1Util.ConvertToLinq(partOperation.MethodRevisionID) + " " + $"And imoMethodAssemblyID = {partOperation.MethodAssemblyID} " + $"And imoMethodOperationID = {partOperation.MethodOperationID}");
			m1BindingSource.DataSourceTable = "PartOperations";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["imoMethodID"] = partOperation.MethodID;
				dataRow["imoMethodRevisionID"] = partOperation.MethodRevisionID;
				dataRow["imoMethodAssemblyID"] = partOperation.MethodAssemblyID;
				if (partOperation.MethodOperationID == 0)
				{
					m1BindingSource.SetKeyToNextAvailable();
				}
				else
				{
					dataRow["imoMethodOperationID"] = partOperation.MethodOperationID;
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["imoOperationType"] = partOperation.OperationType;
			if (partOperation.WorkCenterID != null)
			{
				dataRow["imoWorkCenterID"] = partOperation.WorkCenterID;
			}
			if (partOperation.ProcessID != null)
			{
				dataRow["imoProcessID"] = partOperation.ProcessID;
			}
			if (partOperation.ProcessShortDescription != null)
			{
				dataRow["imoProcessShortDescription"] = partOperation.ProcessShortDescription;
			}
			dataRow["imoProductionStandard"] = partOperation.ProductionStandard;
			if (partOperation.StandardFactor != null)
			{
				dataRow["imoStandardFactor"] = partOperation.StandardFactor;
			}
			dataRow["imoMachinesToSchedule"] = partOperation.MachinesToSchedule;
			dataRow["imoMachineType"] = partOperation.MachineType;
			dataRow["imoQuantityPerAssembly"] = partOperation.QuantityPerAssembly;
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the part method [" + partOperation.MethodID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> SavePartMaterial(BOMPartMaterialDto partMaterial)
	{
		DataRow dataRow = null;
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			m1BindingSource.DataSourceTable = "PartMaterials";
			stringBuilder.Append("immMethodID = " + M1Util.ConvertToLinq(partMaterial.MethodID) + " And immMethodRevisionID = " + M1Util.ConvertToLinq(partMaterial.MethodRevisionID) + " " + $"And immMethodAssemblyID = {partMaterial.MethodAssemblyID} " + $"And immMethodMaterialID = {partMaterial.MethodMaterialID}");
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["immMethodID"] = partMaterial.MethodID;
				dataRow["immMethodRevisionID"] = partMaterial.MethodRevisionID;
				dataRow["immMethodAssemblyID"] = partMaterial.MethodAssemblyID;
				if (partMaterial.MethodMaterialID == 0)
				{
					m1BindingSource.SetKeyToNextAvailable();
				}
				else
				{
					dataRow["immMethodMaterialID"] = partMaterial.MethodMaterialID;
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["immPartID"] = partMaterial.PartID;
			dataRow["immLeadTime"] = partMaterial.LeadTime;
			DataRow dataRow2 = dataRow;
			bool? backFlush = partMaterial.BackFlush;
			dataRow2["immBackflush"] = (backFlush.HasValue ? ((object)(backFlush == true)) : dataRow["immBackflush"]);
			dataRow["immManualPart"] = partMaterial.ManualPart;
			DataRow dataRow3 = dataRow;
			decimal? scrapPercent = partMaterial.ScrapPercent;
			dataRow3["immScrapPercent"] = (scrapPercent.HasValue ? ((object)scrapPercent.GetValueOrDefault()) : dataRow["immScrapPercent"]);
			DataRow dataRow4 = dataRow;
			scrapPercent = partMaterial.ScrapQuantity;
			dataRow4["immScrapQuantity"] = (scrapPercent.HasValue ? ((object)scrapPercent.GetValueOrDefault()) : dataRow["immScrapQuantity"]);
			dataRow["immUnitOfMeasure"] = partMaterial.UnitOfMeasure ?? dataRow["immUnitOfMeasure"];
			dataRow["immPartRevisionID"] = partMaterial.PartRevisionID;
			dataRow["immEstimatedUnitCost"] = partMaterial.EstimatedUnitCost;
			dataRow["immPurchaseLocationID"] = partMaterial.PurchaseLocationID ?? dataRow["immPurchaseLocationID"];
			dataRow["immQuantityPerAssembly"] = partMaterial.QuantityPerAssembly;
			dataRow["immPartShortDescription"] = partMaterial.PartShortDescription;
			dataRow["immRelatedPartOperationID"] = partMaterial.RelatedPartOperationID;
			dataRow["immSupplierOrganizationID"] = partMaterial.SupplierOrganizationID ?? dataRow["immSupplierOrganizationID"];
			dataRow["immPartLongDescriptionRTF"] = partMaterial.PartLongDescription ?? dataRow["immPartLongDescriptionRTF"];
			dataRow["immPartLongDescriptionText"] = partMaterial.PartLongDescription ?? dataRow["immPartLongDescriptionText"];
			dataRow["immUseDefaultWarehouseAndBin"] = partMaterial.UseDefaultWarehouseAndBin;
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the part method [" + partMaterial.MethodID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}

	public Task<IDictionary<string, object>> GetPartAsmKeysFromGuid(string guidOut)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		InitializeParameterLists();
		base.filterList.Add("imaUniqueID|C", guidOut);
		base.selectList.AddRange(new string[3] { "imaMethodID", "imaMethodRevisionID", "imaMethodAssemblyID" });
		using (DataTable dataTable = GetAsDataTable("PartAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					dictionary.Add("imaMethodID", row.Field<string>("imaMethodID"));
					dictionary.Add("imaMethodRevisionID", row.Field<string>("imaMethodRevisionID"));
					dictionary.Add("imaMethodAssemblyID", row.Field<int>("imaMethodAssemblyID"));
				}
			}
		}
		return Task.FromResult((IDictionary<string, object>)dictionary);
	}

	public Task<IDictionary<string, object>> GetPartOperationKeysFromGuid(string methodOperationGuid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		InitializeParameterLists();
		base.filterList.Add("imoUniqueID|C", methodOperationGuid);
		base.selectList.AddRange(new string[4] { "imoMethodID", "imoMethodRevisionID", "imoMethodAssemblyID", "imoMethodOperationID" });
		using (DataTable dataTable = GetAsDataTable("PartOperations", base.filterList, base.selectList, null, null))
		{
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					dictionary.Add("imoMethodID", row.Field<string>("imoMethodID"));
					dictionary.Add("imoMethodRevisionID", row.Field<string>("imoMethodRevisionID"));
					dictionary.Add("imoMethodAssemblyID", row.Field<int>("imoMethodAssemblyID"));
					dictionary.Add("imoMethodOperationID", row.Field<int>("imoMethodOperationID"));
				}
			}
		}
		return Task.FromResult((IDictionary<string, object>)dictionary);
	}

	public Task<IDictionary<string, object>> GetPartMaterialKeysFromGuid(string methodMaterialGuid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		InitializeParameterLists();
		base.filterList.Add("immUniqueID|C", methodMaterialGuid);
		base.selectList.AddRange(new string[4] { "immMethodID", "immMethodRevisionID", "immMethodAssemblyID", "immMethodMaterialID" });
		using (DataTable dataTable = GetAsDataTable("PartMaterials", base.filterList, base.selectList, null, null))
		{
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					dictionary.Add("immMethodID", row.Field<string>("immMethodID"));
					dictionary.Add("immMethodRevisionID", row.Field<string>("immMethodRevisionID"));
					dictionary.Add("immMethodAssemblyID", row.Field<int>("immMethodAssemblyID"));
					dictionary.Add("immMethodMaterialID", row.Field<int>("immMethodMaterialID"));
				}
			}
		}
		return Task.FromResult((IDictionary<string, object>)dictionary);
	}

	public Task<DataTable> GetPartMethodGuidsAsDataTable(string partId)
	{
		string arg = "imaMethodID LIKE '%'+@partId+'%'";
		InitializeParameterLists();
		using SqlCommand sqlCommand = new SqlCommand(string.Format(GET_PARTMETHOD_GUIDS, arg));
		sqlCommand.Parameters.AddWithValue("@partId", partId);
		return Task.FromResult(base.M1database.GetDataTable(sqlCommand));
	}

	public Task<bool> IsCogsEnabled()
	{
		return Task.FromResult(Convert.ToBoolean(base.M1database.Props("FN").Field<bool>("xafGLCreateStockJournals")));
	}

	public Task<ICollection<PartInformationDto>> GetAllPartInfo(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<PartInformationDto> collection = new List<PartInformationDto>();
		InitializeParameterLists();
		string[] array = new string[15]
		{
			"impPartID", "impShortDescription", "impPartGroupID", "impLongDescriptionText", "impDeliveryType", "impTaxCodeID", "impSecondTaxCodeID", "impNonTaxReasonID", "impAlwaysNonTaxable", "impPartType",
			"impPartClassID", "impCreatedBy", "impCreatedDate", "impBuyForInventory", "impNonStockedItem"
		};
		base.selectList.AddRange(array);
		List<string> orderbyList = new List<string> { "impPartID" };
		using (DataTable dataTable = GetAsDataTable("Parts", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				PartInformationDto partInformationDto = new PartInformationDto();
				partInformationDto.PartID = dataTable.Rows[i][array[0]].ToString().Trim();
				partInformationDto.PartType = Convert.ToByte(dataTable.Rows[i][array[9]]);
				partInformationDto.CreatedBy = dataTable.Rows[i][array[11]].ToString().Trim();
				partInformationDto.PartGroupID = dataTable.Rows[i][array[2]].ToString().Trim();
				partInformationDto.PartClassID = dataTable.Rows[i][array[10]].ToString().Trim();
				partInformationDto.PartTaxCodeID = dataTable.Rows[i][array[5]].ToString().Trim();
				partInformationDto.PartNonTaxReasonID = dataTable.Rows[i][array[7]].ToString().Trim();
				partInformationDto.PartSecondTaxCodeID = dataTable.Rows[i][array[6]].ToString().Trim();
				partInformationDto.NonStockedItem = Convert.ToInt16(dataTable.Rows[i][array[14]]) != 0;
				partInformationDto.PartShortDescription = dataTable.Rows[i][array[1]].ToString().Trim();
				partInformationDto.BuyForInventory = Convert.ToInt16(dataTable.Rows[i][array[13]]) != 0;
				partInformationDto.PartLongDescriptionText = dataTable.Rows[i][array[3]].ToString().Trim();
				partInformationDto.PartAlwaysNonTaxable = Convert.ToInt16(dataTable.Rows[i][array[8]]) != 0;
				partInformationDto.DeliveryType = Convert.ToByte(dataTable.Rows[i][array[4]].ToString().Trim());
				partInformationDto.CreatedDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(dataTable.Rows[i][array[12]].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[i][array[12]].ToString()));
				collection.Add(partInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}

	public Task<bool> DoesPartWarehouseLocationExists(string partId, string partRevisionID, string partWarehouseLocation)
	{
		InitializeParameterLists();
		base.filterList.Add("imlPartID|C", partId);
		base.filterList.Add("imlPartRevisionID|C", partRevisionID);
		base.filterList.Add("imlPartWarehouseID|C", partWarehouseLocation);
		base.selectList.Add("imlPartID,imlPartRevisionID,imlPartWarehouseID");
		return Task.FromResult(GetAsObject("PartWarehouseLocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesPartBinExists(string partId, string partRevisionID, string partWarehouseLocation, string partBinID)
	{
		InitializeParameterLists();
		base.filterList.Add("imbPartID|C", partId);
		base.filterList.Add("imbPartRevisionID|C", partRevisionID);
		base.filterList.Add("imbWarehouseID|C", partWarehouseLocation);
		base.filterList.Add("imbPartBinID|C", partBinID);
		base.filterList.Add("imbInactiveBin", 0);
		base.selectList.Add("imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID");
		return Task.FromResult(GetAsObject("PartBins", base.filterList, base.selectList, null, null) != null);
	}
}
