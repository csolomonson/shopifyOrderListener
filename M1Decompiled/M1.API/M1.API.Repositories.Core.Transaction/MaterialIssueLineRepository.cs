using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;
using M1.API.Utilities;

namespace M1.API.Repositories.Core.Transaction;

public class MaterialIssueLineRepository : APIBaseRepository, IMaterialIssueLineRepository, IAPIBaseRepository, IDisposable
{
	private readonly string SELECT_RECEIPT_LINES = "SELECT injMaterialIssueID, injMaterialIssueLineID, injIssueType, injJobID, injReverseMaterialIssueLineID, injPlantID,\r\n                                                                injJobAssemblyID, injCreateJobSeq, injJobMaterialID, injJobType, injEstimatedQuantity, injKitPart,\r\n                                                                injJobOpenQuantity, injIssueComplete, injPartID, injPartRevisionID, injPartWarehouseLocationID, injLongDescriptionText,\r\n                                                                injPartBinID, injInvIssueQuantity, injInvScrapQuantity, injReference, injHeatLot, injMiscIssueReasonID,\r\n                                                                injProjectID, injProjectAreaID, injJobMatScrapQuantity, injJobAsmScrapQuantity, injJobAsmIssueQuantity,\r\n                                                                injJobMatIssueQuantity, injJobMatReturnScrapQuantity,injJobMatReturnIssueQuantity, injReverseMaterialIssueID, \r\n                                                                injReversed, injCreatedBy, injCreatedDate, injUniqueID, injPosted, injQuantityAllocated, injQuantityOnHand, injRowVersion\r\n                                                         FROM MaterialIssueLines\r\n                                                         WHERE (injMaterialIssueID = @MaterialIssueID)";

	private readonly string[] materialIssueFields = new string[10] { "iniMaterialIssueID", "iniCreatedBy", "iniCreatedDate", "iniUniqueID", "iniPosted", "iniReversalEntry", "iniReversed", "iniMaterialIssueDate", "iniPostedDate", "iniRowVersion" };

	private readonly string[] materialIssueLineFields = new string[41]
	{
		"injMaterialIssueID", "injMaterialIssueLineID", "injIssueType", "injJobID", "injJobAssemblyID", "injCreateJobSeq", "injJobMaterialID", "injJobType", "injEstimatedQuantity", "injJobOpenQuantity",
		"injIssueComplete", "injPartID", "injPartRevisionID", "injPartWarehouseLocationID", "injPartBinID", "injInvIssueQuantity", "injInvScrapQuantity", "injReference", "injHeatLot", "injMiscIssueReasonID",
		"injProjectID", "injPlantID", "injLongDescriptionText", "injKitPart", "injProjectAreaID", "injJobMatScrapQuantity", "injJobAsmScrapQuantity", "injJobAsmIssueQuantity", "injJobMatIssueQuantity", "injQuantityAllocated",
		"injQuantityOnHand", "injJobMatReturnScrapQuantity", "injJobMatReturnIssueQuantity", "injReverseMaterialIssueID", "injReverseMaterialIssueLineID", "injReversed", "injCreatedBy", "injCreatedDate", "injUniqueID", "injPosted",
		"injRowVersion"
	};

	public MaterialIssueLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<ICollection<MaterialIssueLineInformationDto>> GetAllMaterialIssueLines(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<MaterialIssueLineInformationDto> collection = new List<MaterialIssueLineInformationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(materialIssueLineFields);
		List<string> orderbyList = new List<string> { "injMaterialIssueLineID" };
		using (DataTable dataTable = GetAsDataTable("MaterialIssueLines", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				MaterialIssueLineInformationDto materialIssueLineInformationDto = new MaterialIssueLineInformationDto();
				materialIssueLineInformationDto.CreatedBy = dataTable.Rows[i].Field<string>("injCreatedBy");
				materialIssueLineInformationDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("injCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("injCreatedDate"));
				materialIssueLineInformationDto.UniqueID = dataTable.Rows[i].Field<Guid>("injUniqueID");
				materialIssueLineInformationDto.EstimatedQuantity = dataTable.Rows[i].Field<decimal>("injEstimatedQuantity");
				materialIssueLineInformationDto.HeatLot = dataTable.Rows[i].Field<string>("injHeatLot");
				materialIssueLineInformationDto.InvIssueQuantity = dataTable.Rows[i].Field<decimal>("injInvIssueQuantity");
				materialIssueLineInformationDto.InvScrapQuantity = dataTable.Rows[i].Field<decimal>("injInvScrapQuantity");
				materialIssueLineInformationDto.CreateJobSeq = dataTable.Rows[i].Field<bool>("injCreateJobSeq");
				materialIssueLineInformationDto.IssueComplete = dataTable.Rows[i].Field<bool>("injIssueComplete");
				materialIssueLineInformationDto.KitPart = dataTable.Rows[i].Field<bool>("injKitPart");
				materialIssueLineInformationDto.Posted = dataTable.Rows[i].Field<bool>("injPosted");
				materialIssueLineInformationDto.Reversed = dataTable.Rows[i].Field<bool>("injReversed");
				materialIssueLineInformationDto.IssueType = dataTable.Rows[i].Field<byte>("injIssueType");
				materialIssueLineInformationDto.JobAsmIssueQuantity = dataTable.Rows[i].Field<decimal>("injJobAsmIssueQuantity");
				materialIssueLineInformationDto.JobAsmScrapQuantity = dataTable.Rows[i].Field<decimal>("injJobAsmScrapQuantity");
				materialIssueLineInformationDto.JobAssemblyID = dataTable.Rows[i].Field<int>("injJobAssemblyID");
				materialIssueLineInformationDto.JobID = dataTable.Rows[i].Field<string>("injJobID");
				materialIssueLineInformationDto.JobMaterialID = dataTable.Rows[i].Field<int>("injJobMaterialID");
				materialIssueLineInformationDto.JobMatIssueQuantity = dataTable.Rows[i].Field<decimal>("injJobMatIssueQuantity");
				materialIssueLineInformationDto.JobMatReturnIssueQuantity = dataTable.Rows[i].Field<decimal>("injJobMatReturnIssueQuantity");
				materialIssueLineInformationDto.JobMatReturnScrapQuantity = dataTable.Rows[i].Field<decimal>("injJobMatReturnScrapQuantity");
				materialIssueLineInformationDto.JobMatScrapQuantity = dataTable.Rows[i].Field<decimal>("injJobMatScrapQuantity");
				materialIssueLineInformationDto.JobOpenQuantity = dataTable.Rows[i].Field<decimal>("injJobOpenQuantity");
				materialIssueLineInformationDto.JobType = dataTable.Rows[i].Field<byte>("injJobType");
				materialIssueLineInformationDto.LongDescriptionText = dataTable.Rows[i].Field<string>("injLongDescriptionText");
				materialIssueLineInformationDto.MaterialIssueID = dataTable.Rows[i].Field<string>("injMaterialIssueID");
				materialIssueLineInformationDto.MiscIssueReasonID = dataTable.Rows[i].Field<string>("injMiscIssueReasonID");
				materialIssueLineInformationDto.PartBinID = dataTable.Rows[i].Field<string>("injPartBinID");
				materialIssueLineInformationDto.PartID = dataTable.Rows[i].Field<string>("injPartID");
				materialIssueLineInformationDto.PartRevisionID = dataTable.Rows[i].Field<string>("injPartRevisionID");
				materialIssueLineInformationDto.PartWarehouseLocationID = dataTable.Rows[i].Field<string>("injPartWarehouseLocationID");
				materialIssueLineInformationDto.PlantID = dataTable.Rows[i].Field<string>("injPlantID");
				materialIssueLineInformationDto.ProjectAreaID = dataTable.Rows[i].Field<string>("injProjectAreaID");
				materialIssueLineInformationDto.ProjectID = dataTable.Rows[i].Field<string>("injProjectID");
				materialIssueLineInformationDto.QuantityAllocated = dataTable.Rows[i].Field<decimal>("injQuantityAllocated");
				materialIssueLineInformationDto.QuantityOnHand = dataTable.Rows[i].Field<decimal>("injQuantityOnHand");
				materialIssueLineInformationDto.Reference = dataTable.Rows[i].Field<string>("injReference");
				materialIssueLineInformationDto.ReverseMaterialIssueID = dataTable.Rows[i].Field<string>("injReverseMaterialIssueID");
				materialIssueLineInformationDto.ReverseMaterialIssueLineID = dataTable.Rows[i].Field<short>("injReverseMaterialIssueLineID");
				materialIssueLineInformationDto.MaterialIssueLineID = dataTable.Rows[i].Field<short>("injMaterialIssueLineID");
				materialIssueLineInformationDto.RowVersion = dataTable.Rows[i].Field<byte[]>("injRowVersion");
				collection.Add(materialIssueLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<MaterialIssueDto> GetMaterialIssueInfo(string materialIssueId)
	{
		MaterialIssueDto materialIssueDto = new MaterialIssueDto();
		InitializeParameterLists();
		base.selectList.AddRange(materialIssueFields);
		base.filterList.Add("iniMaterialIssueID|C", materialIssueId);
		using (DataTable dataTable = GetAsDataTable("MaterialIssues", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(materialIssueDto);
			}
			materialIssueDto.MaterialIssueID = dataTable.Rows[0].Field<string>("iniMaterialIssueID").ToString().Trim();
			materialIssueDto.MaterialIssueDate = dataTable.Rows[0].Field<DateTime?>("iniMaterialIssueDate");
			materialIssueDto.Posted = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["iniPosted"]));
			materialIssueDto.PostedDate = dataTable.Rows[0].Field<DateTime?>("iniPostedDate");
			materialIssueDto.ReversalEntry = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["iniReversalEntry"]));
			materialIssueDto.Reversed = Convert.ToBoolean(Convert.ToInt16(dataTable.Rows[0]["iniReversed"]));
			materialIssueDto.UniqueID = dataTable.Rows[0].Field<Guid>("iniUniqueID");
			materialIssueDto.CreatedBy = dataTable.Rows[0].Field<string>("iniCreatedBy").ToString().Trim();
			materialIssueDto.CreatedDate = dataTable.Rows[0].Field<DateTime?>("iniCreatedDate");
			materialIssueDto.RowVersion = dataTable.Rows[0].Field<byte[]>("iniRowVersion");
		}
		return Task.FromResult(materialIssueDto);
	}

	public Task<IList<MaterialIssueLineInformationDto>> GetMaterialIssueLineInfo(string materialIssueId)
	{
		IList<MaterialIssueLineInformationDto> list = new List<MaterialIssueLineInformationDto>();
		InitializeParameterLists();
		base.filterList.Add("@MaterialIssueID", materialIssueId);
		using (DataTable dataTable = GetAsDataTable(SELECT_RECEIPT_LINES, base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				MaterialIssueLineInformationDto item = new MaterialIssueLineInformationDto
				{
					MaterialIssueID = row.Field<string>("injMaterialIssueID"),
					MaterialIssueLineID = row.Field<short>("injMaterialIssueLineID"),
					IssueType = row.Field<byte>("injIssueType"),
					JobID = row.Field<string>("injJobID"),
					JobAssemblyID = row.Field<int>("injJobAssemblyID"),
					CreateJobSeq = row.Field<bool>("injCreateJobSeq"),
					JobMaterialID = row.Field<int>("injJobMaterialID"),
					JobType = row.Field<byte>("injJobType"),
					EstimatedQuantity = row.Field<decimal>("injEstimatedQuantity"),
					JobOpenQuantity = row.Field<decimal>("injJobOpenQuantity"),
					IssueComplete = row.Field<bool>("injIssueComplete"),
					PartID = row.Field<string>("injPartID"),
					PartRevisionID = row.Field<string>("injPartRevisionID"),
					PartWarehouseLocationID = row.Field<string>("injPartWarehouseLocationID"),
					PartBinID = row.Field<string>("injPartBinID"),
					InvIssueQuantity = row.Field<decimal>("injInvIssueQuantity"),
					InvScrapQuantity = row.Field<decimal>("injInvScrapQuantity"),
					Reference = row.Field<string>("injReference"),
					HeatLot = row.Field<string>("injHeatLot"),
					MiscIssueReasonID = row.Field<string>("injMiscIssueReasonID"),
					ProjectID = row.Field<string>("injProjectID"),
					ProjectAreaID = row.Field<string>("injProjectAreaID"),
					JobMatScrapQuantity = row.Field<decimal>("injJobMatScrapQuantity"),
					JobAsmScrapQuantity = row.Field<decimal>("injJobAsmScrapQuantity"),
					JobAsmIssueQuantity = row.Field<decimal>("injJobAsmIssueQuantity"),
					JobMatIssueQuantity = row.Field<decimal>("injJobMatIssueQuantity"),
					JobMatReturnScrapQuantity = row.Field<decimal>("injJobMatReturnScrapQuantity"),
					JobMatReturnIssueQuantity = row.Field<decimal>("injJobMatReturnIssueQuantity"),
					ReverseMaterialIssueID = row.Field<string>("injReverseMaterialIssueID"),
					ReverseMaterialIssueLineID = row.Field<short>("injReverseMaterialIssueLineID"),
					Reversed = row.Field<bool>("injReversed"),
					CreatedBy = row.Field<string>("injCreatedBy"),
					CreatedDate = ((!row.Field<DateTime?>("injCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : row.Field<DateTime?>("injCreatedDate")),
					UniqueID = row.Field<Guid>("injUniqueID"),
					KitPart = row.Field<bool>("injKitPart"),
					Posted = row.Field<bool>("injPosted"),
					LongDescriptionText = row.Field<string>("injLongDescriptionText"),
					PlantID = row.Field<string>("injPlantID"),
					QuantityAllocated = row.Field<decimal>("injQuantityAllocated"),
					QuantityOnHand = row.Field<decimal>("injQuantityOnHand"),
					RowVersion = row.Field<byte[]>("injRowVersion")
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}
}
