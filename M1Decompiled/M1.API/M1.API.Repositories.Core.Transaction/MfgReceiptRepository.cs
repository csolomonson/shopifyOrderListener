using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Core.Transaction;
using M1.API.DTOs.Custom;
using M1.API.Utilities;

namespace M1.API.Repositories.Core.Transaction;

public class MfgReceiptRepository : APIBaseRepository, IMfgReceiptRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] _fields = new string[23]
	{
		"rmmMfgReceiptID", "rmmReceiptType", "rmmReceiptDate", "rmmPartID", "rmmPartRevisionID", "rmmPartWarehouseLocationID", "rmmPartBinID", "rmmPosted", "rmmPostedDate", "rmmCreatedBy",
		"rmmCreatedDate", "rmmUniqueID", "rmmRowVersion", "rmmProjectID", "rmmProjectAreaID", "rmmMiscInvQuantityReceived", "rmmInventoryQuantityReceived", "rmmJobOprQuantityReceived", "rmmJobAsmQuantityReceived", "rmmJobMatQuantityReceived",
		"rmmReference", "rmmHeatLot", "rmmRowVersion"
	};

	public MfgReceiptRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesMfgReceiptExists(string mfgReceiptId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmmMfgReceiptID|C", mfgReceiptId);
		base.selectList.Add("rmmMfgReceiptID");
		return Task.FromResult(GetAsObject("MfgReceipts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<MfgReceiptInformationDto>> GetAllMfgReceipts(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<MfgReceiptInformationDto> collection = new List<MfgReceiptInformationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(_fields);
		List<string> orderbyList = new List<string> { "rmmMfgReceiptID" };
		using (DataTable dataTable = GetAsDataTable("MfgReceipts", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				MfgReceiptInformationDto mfgReceiptInformationDto = new MfgReceiptInformationDto();
				mfgReceiptInformationDto.MfgReceiptID = dataTable.Rows[i].Field<string>("rmmMfgReceiptID");
				mfgReceiptInformationDto.CreatedBy = dataTable.Rows[i].Field<string>("rmmCreatedBy");
				mfgReceiptInformationDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("rmmCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("rmmCreatedDate"));
				mfgReceiptInformationDto.UniqueID = dataTable.Rows[i].Field<Guid>("rmmUniqueID");
				mfgReceiptInformationDto.HeatLot = dataTable.Rows[i].Field<string>("rmmHeatLot");
				mfgReceiptInformationDto.Posted = dataTable.Rows[i].Field<bool>("rmmPosted");
				mfgReceiptInformationDto.MiscInvQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmMiscInvQuantityReceived");
				mfgReceiptInformationDto.InventoryQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmInventoryQuantityReceived");
				mfgReceiptInformationDto.JobOprQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmJobOprQuantityReceived");
				mfgReceiptInformationDto.JobAsmQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmJobAsmQuantityReceived");
				mfgReceiptInformationDto.JobMatQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmJobMatQuantityReceived");
				mfgReceiptInformationDto.PartBinID = dataTable.Rows[i].Field<string>("rmmPartBinID");
				mfgReceiptInformationDto.PartID = dataTable.Rows[i].Field<string>("rmmPartID");
				mfgReceiptInformationDto.PartRevisionID = dataTable.Rows[i].Field<string>("rmmPartRevisionID");
				mfgReceiptInformationDto.PartWarehouseLocationID = dataTable.Rows[i].Field<string>("rmmPartWarehouseLocationID");
				mfgReceiptInformationDto.PostedDate = dataTable.Rows[i].Field<DateTime?>("rmmPostedDate");
				mfgReceiptInformationDto.ProjectAreaID = dataTable.Rows[i].Field<string>("rmmProjectAreaID");
				mfgReceiptInformationDto.ProjectID = dataTable.Rows[i].Field<string>("rmmProjectID");
				mfgReceiptInformationDto.ReceiptDate = dataTable.Rows[i].Field<DateTime?>("rmmReceiptDate");
				mfgReceiptInformationDto.ReceiptType = dataTable.Rows[i].Field<byte>("rmmReceiptType");
				mfgReceiptInformationDto.Reference = dataTable.Rows[i].Field<string>("rmmReference");
				mfgReceiptInformationDto.RowVersion = dataTable.Rows[i].Field<byte[]>("rmmRowVersion");
				collection.Add(mfgReceiptInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<MfgReceiptDto> GetMfgReceipt(string mfgReceiptId)
	{
		MfgReceiptDto mfgReceiptDto = new MfgReceiptDto();
		InitializeParameterLists();
		base.selectList.AddRange(_fields);
		base.filterList.Add(Guid.TryParse(mfgReceiptId, out var _) ? "rmmUniqueID|C" : "rmmMfgReceiptID|C", mfgReceiptId);
		using (DataTable dataTable = GetAsDataTable("MfgReceipts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(mfgReceiptDto);
			}
			mfgReceiptDto.MfgReceiptID = dataTable.Rows[0].Field<string>("rmmMfgReceiptID");
			mfgReceiptDto.CreatedBy = dataTable.Rows[0].Field<string>("rmmCreatedBy");
			mfgReceiptDto.CreatedDate = ((!dataTable.Rows[0].Field<DateTime?>("rmmCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[0].Field<DateTime?>("rmmCreatedDate"));
			mfgReceiptDto.UniqueID = dataTable.Rows[0].Field<Guid>("rmmUniqueID");
			mfgReceiptDto.HeatLot = dataTable.Rows[0].Field<string>("rmmHeatLot");
			mfgReceiptDto.Posted = dataTable.Rows[0].Field<bool>("rmmPosted");
			mfgReceiptDto.MiscInvQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmMiscInvQuantityReceived");
			mfgReceiptDto.InventoryQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmInventoryQuantityReceived");
			mfgReceiptDto.JobOprQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmJobOprQuantityReceived");
			mfgReceiptDto.JobAsmQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmJobAsmQuantityReceived");
			mfgReceiptDto.JobMatQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmJobMatQuantityReceived");
			mfgReceiptDto.PartBinID = dataTable.Rows[0].Field<string>("rmmPartBinID");
			mfgReceiptDto.PartID = dataTable.Rows[0].Field<string>("rmmPartID");
			mfgReceiptDto.PartRevisionID = dataTable.Rows[0].Field<string>("rmmPartRevisionID");
			mfgReceiptDto.PartWarehouseLocationID = dataTable.Rows[0].Field<string>("rmmPartWarehouseLocationID");
			mfgReceiptDto.PostedDate = dataTable.Rows[0].Field<DateTime?>("rmmPostedDate");
			mfgReceiptDto.ProjectAreaID = dataTable.Rows[0].Field<string>("rmmProjectAreaID");
			mfgReceiptDto.ProjectID = dataTable.Rows[0].Field<string>("rmmProjectID");
			mfgReceiptDto.ReceiptDate = dataTable.Rows[0].Field<DateTime?>("rmmReceiptDate");
			mfgReceiptDto.ReceiptType = dataTable.Rows[0].Field<byte>("rmmReceiptType");
			mfgReceiptDto.Reference = dataTable.Rows[0].Field<string>("rmmReference");
			mfgReceiptDto.RowVersion = dataTable.Rows[0].Field<byte[]>("rmmRowVersion");
		}
		return Task.FromResult(mfgReceiptDto);
	}
}
