using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;
using M1.API.Utilities;

namespace M1.API.Repositories.Core.Transaction;

public class ReceiptLineRepository : APIBaseRepository, IReceiptLineRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] fields = new string[41]
	{
		"rmlReceiptLineID", "rmlReceiptID", "rmlPurchaseOrderID", "rmlPurchaseOrderLineID", "rmlJobID", "rmlJobAssemblyID", "rmlJobType", "rmlJobMaterialID", "rmlJobOperationID", "rmlPartID",
		"rmlPartRevisionID", "rmlOrgPartID", "rmlOrgPartShortDescription", "rmlDescription", "rmlPartWarehouseLocationID", "rmlPartBinID", "rmlPurchaseQuantityReceived", "rmlPurchaseUnitOfMeasure", "rmlPurchaseUnitCost", "rmlSetupCharge",
		"rmlConversionFactor", "rmlInventoryUnitOfMeasure", "rmlInventoryUnitCost", "rmlPoReceivedComplete", "rmlJobReceivedComplete", "rmlRequiresInspection", "rmlReference", "rmlHeatLot", "rmlProjectID", "rmlProjectAreaID",
		"rmlClosed", "rmlPostedToGl", "rmlReversed", "rmlReverseReceiptID", "rmlReverseReceiptLineID", "rmlCreatedBy", "rmlCreatedDate", "rmlUniqueID", "rmlRowVersion", "rmlJobOprQuantityReceived",
		"rmlJobMatQuantityReceived"
	};

	public ReceiptLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<ICollection<ReceiptLineDto>> GetAllReceiptLines(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<ReceiptLineDto> collection = new List<ReceiptLineDto>();
		InitializeParameterLists();
		base.selectList.AddRange(fields);
		List<string> orderbyList = new List<string> { "rmlReceiptID" };
		using (DataTable dataTable = GetAsDataTable("ReceiptLines", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ReceiptLineDto receiptLineDto = new ReceiptLineDto();
				receiptLineDto.ConversionFactor = dataTable.Rows[i].Field<decimal>("rmlConversionFactor");
				receiptLineDto.CreatedBy = dataTable.Rows[i].Field<string>("rmlCreatedBy");
				receiptLineDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("rmlCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("rmlCreatedDate"));
				receiptLineDto.Description = dataTable.Rows[i].Field<string>("rmlDescription");
				receiptLineDto.UniqueID = dataTable.Rows[i].Field<Guid>("rmlUniqueID");
				receiptLineDto.HeatLot = dataTable.Rows[i].Field<string>("rmlHeatLot");
				receiptLineDto.InventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("rmlInventoryUnitOfMeasure");
				receiptLineDto.Closed = dataTable.Rows[i].Field<bool>("rmlClosed");
				receiptLineDto.JobReceivedComplete = dataTable.Rows[i].Field<bool>("rmlJobReceivedComplete");
				receiptLineDto.PoReceivedComplete = dataTable.Rows[i].Field<bool>("rmlPoReceivedComplete");
				receiptLineDto.PostedToGl = dataTable.Rows[i].Field<bool>("rmlPostedToGl");
				receiptLineDto.RequiresInspection = dataTable.Rows[i].Field<bool>("rmlRequiresInspection");
				receiptLineDto.Reversed = dataTable.Rows[i].Field<bool>("rmlReversed");
				receiptLineDto.JobAssemblyID = dataTable.Rows[i].Field<int>("rmlJobAssemblyID");
				receiptLineDto.JobID = dataTable.Rows[i].Field<string>("rmlJobID");
				receiptLineDto.JobMaterialID = dataTable.Rows[i].Field<int>("rmlJobMaterialID");
				receiptLineDto.JobMatQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlJobMatQuantityReceived");
				receiptLineDto.JobOperationID = dataTable.Rows[i].Field<int>("rmlJobOperationID");
				receiptLineDto.JobOprQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlJobOprQuantityReceived");
				receiptLineDto.JobType = dataTable.Rows[i].Field<byte>("rmlJobType");
				receiptLineDto.OrgPartID = dataTable.Rows[i].Field<string>("rmlOrgPartID");
				receiptLineDto.OrgPartShortDescription = dataTable.Rows[i].Field<string>("rmlOrgPartShortDescription");
				receiptLineDto.PartBinID = dataTable.Rows[i].Field<string>("rmlPartBinID");
				receiptLineDto.PartID = dataTable.Rows[i].Field<string>("rmlPartID");
				receiptLineDto.PartRevisionID = dataTable.Rows[i].Field<string>("rmlPartRevisionID");
				receiptLineDto.PartWarehouseLocationID = dataTable.Rows[i].Field<string>("rmlPartWarehouseLocationID");
				receiptLineDto.ProjectAreaID = dataTable.Rows[i].Field<string>("rmlProjectAreaID");
				receiptLineDto.ProjectID = dataTable.Rows[i].Field<string>("rmlProjectID");
				receiptLineDto.PurchaseOrderID = dataTable.Rows[i].Field<string>("rmlPurchaseOrderID");
				receiptLineDto.PurchaseOrderLineID = dataTable.Rows[i].Field<short>("rmlPurchaseOrderLineID");
				receiptLineDto.PurchaseQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlPurchaseQuantityReceived");
				receiptLineDto.PurchaseUnitCost = dataTable.Rows[i].Field<decimal>("rmlPurchaseUnitCost");
				receiptLineDto.PurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("rmlPurchaseUnitOfMeasure");
				receiptLineDto.ReceiptID = dataTable.Rows[i].Field<string>("rmlReceiptID");
				receiptLineDto.Reference = dataTable.Rows[i].Field<string>("rmlReference");
				receiptLineDto.ReverseReceiptID = dataTable.Rows[i].Field<string>("rmlReverseReceiptID");
				receiptLineDto.ReverseReceiptLineID = dataTable.Rows[i].Field<short>("rmlReverseReceiptLineID");
				receiptLineDto.ReceiptLineID = dataTable.Rows[i].Field<short>("rmlReceiptLineID");
				receiptLineDto.SetupCharge = dataTable.Rows[i].Field<decimal>("rmlSetupCharge");
				receiptLineDto.RowVersion = dataTable.Rows[i].Field<byte[]>("rmlRowVersion");
				collection.Add(receiptLineDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ReceiptLineDto> GetReceiptLine(string partId)
	{
		ReceiptLineDto result = new ReceiptLineDto();
		InitializeParameterLists();
		base.selectList.AddRange(fields);
		base.filterList.Add("rmlPartID|C", partId);
		using (DataTable dataTable = GetAsDataTable("ReceiptLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataRow row = dataTable.Rows[0];
				result = new ReceiptLineDto
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
					PoReceivedComplete = row.Field<bool>("rmlPoReceivedComplete"),
					JobReceivedComplete = row.Field<bool>("rmlJobReceivedComplete"),
					RequiresInspection = row.Field<bool>("rmlRequiresInspection"),
					Reference = row.Field<string>("rmlReference").ToString().Trim(),
					HeatLot = row.Field<string>("rmlHeatLot").ToString().Trim(),
					ProjectID = row.Field<string>("rmlProjectID").ToString().Trim(),
					ProjectAreaID = row.Field<string>("rmlProjectAreaID").ToString().Trim(),
					Closed = row.Field<bool>("rmlClosed"),
					PostedToGl = row.Field<bool>("rmlPostedToGl"),
					Reversed = row.Field<bool>("rmlReversed"),
					ReverseReceiptID = row.Field<string>("rmlReverseReceiptID").ToString().Trim(),
					ReverseReceiptLineID = row.Field<short>("rmlReverseReceiptLineID"),
					CreatedBy = row.Field<string>("rmlCreatedBy").ToString().Trim(),
					CreatedDate = (row.Field<DateTime?>("rmlCreatedDate") ?? DateTime.Parse("01/01/1900")),
					UniqueID = row.Field<Guid>("rmlUniqueID"),
					RowVersion = row.Field<byte[]>("rmlRowVersion"),
					JobMatQuantityReceived = row.Field<decimal>("rmlJobMatQuantityReceived"),
					JobOprQuantityReceived = row.Field<decimal>("rmlJobOprQuantityReceived")
				};
			}
		}
		return Task.FromResult(result);
	}
}
