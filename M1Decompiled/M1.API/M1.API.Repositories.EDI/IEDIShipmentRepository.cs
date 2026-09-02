using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;

namespace M1.API.Repositories.EDI;

public interface IEDIShipmentRepository : IShipmentRepository, IAPIBaseRepository, IDisposable
{
	Task<bool> IsEDIShipment(string shipmentId);

	Task<bool> DoesNonEDISalesordersExist_ForShipment(string shipmentId);

	Task<IList<ShipmentDto>> GetShipments_PendingEDITransfer_AllUnmapped();

	Task<ShipmentDto> GetEDIShipment_Details_ForShipmentID(string shipmentId);

	Task<bool> UpdateEdiFlag(IDictionary<string, bool> shipmentDictionary, SqlTransaction sqlTransaction);
}
