using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core;

public interface IInvoiceRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesInvoiceExists(string invoiceId);

	Task<bool> IsInvoicePostedToGL(string invoiceId);

	Task<IDictionary<string, ARInvoiceDto>> GetInvoicesListForSalesOrder(string salesOrderId);
}
