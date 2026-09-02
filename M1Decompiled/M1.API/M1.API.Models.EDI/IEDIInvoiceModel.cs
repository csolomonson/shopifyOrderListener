using System;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.API.Repositories.EDI;

namespace M1.API.Models.EDI;

public interface IEDIInvoiceModel : IEDIBaseModel, IAPIBaseModel, IDisposable
{
	IEDIInvoiceRepository EdiInvoiceRepository { get; set; }

	Task<EDI810OutboundInvoice> Create810InvoiceObject(string invoiceId);

	Task<APIValidationInfoDto> ValidateRequest_SetEDIFlag(EDI810InvoicesIN ediInvoices);

	Task<APIValidationInfoDto> Process_SetEDIFlag(EDI810InvoicesIN ediInvoices);

	Task<EDI810InvoiceCollectionDto> Process_AllUnmapped(int page, int pagesize);
}
