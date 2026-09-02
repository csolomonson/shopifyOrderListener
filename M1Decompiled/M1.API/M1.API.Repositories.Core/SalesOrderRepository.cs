using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core;

public class SalesOrderRepository : APIBaseRepository, ISalesOrderRepository, IAPIBaseRepository, IDisposable
{
	private Task<DataRow> saveSalesOrderHeader(SalesOrderDto salesOrderDto, M1BindingSource salesOrderBs)
	{
		DataRow dataRow = salesOrderBs.AddNew() as DataRow;
		salesOrderBs.SetKeyToNextAvailable(dataRow);
		dataRow.BeginEdit();
		if (salesOrderDto.CustomerOrganizationID != null)
		{
			dataRow["ompCustomerOrganizationID"] = salesOrderDto.CustomerOrganizationID;
		}
		if (salesOrderDto.ShipOrganizationID != null)
		{
			dataRow["ompShipOrganizationID"] = salesOrderDto.ShipOrganizationID;
		}
		if (salesOrderDto.CustomerPO != null)
		{
			dataRow["ompCustomerPO"] = salesOrderDto.CustomerPO;
		}
		if (salesOrderDto.RequestedShipDate.HasValue && salesOrderDto.RequestedShipDate.Value.Year > 2000)
		{
			dataRow["ompRequestedShipDate"] = salesOrderDto.RequestedShipDate;
		}
		if (salesOrderDto.OrderDate.Year > 2000)
		{
			dataRow["ompOrderDate"] = salesOrderDto.OrderDate;
		}
		if (salesOrderDto.PaymentTermID != null && !string.IsNullOrEmpty(salesOrderDto.PaymentTermID))
		{
			dataRow["ompPaymentTermID"] = salesOrderDto.PaymentTermID;
		}
		if (salesOrderDto.TotalOrderWeight != 0m)
		{
			dataRow["ompTotalOrderWeight"] = salesOrderDto.TotalOrderWeight;
		}
		if (salesOrderDto.Status != 0)
		{
			dataRow["ompStatus"] = salesOrderDto.Status;
		}
		if (salesOrderDto.CreatedBy != null && !string.IsNullOrEmpty(salesOrderDto.CreatedBy))
		{
			dataRow["ompCreatedBy"] = salesOrderDto.CreatedBy;
		}
		if (salesOrderDto.CreatedDate.HasValue && salesOrderDto.CreatedDate.Value.Year > 2000)
		{
			dataRow["ompCreatedDate"] = salesOrderDto.CreatedDate;
		}
		if (salesOrderDto.CreatedByEDI)
		{
			dataRow["ompCreatedByEDI"] = salesOrderDto.CreatedByEDI;
		}
		if (salesOrderDto.CreatedFromWeb)
		{
			dataRow["ompCreatedFromWeb"] = salesOrderDto.CreatedFromWeb;
		}
		if (salesOrderDto.ShipLocationID != null)
		{
			dataRow["ompShipLocationID"] = salesOrderDto.ShipLocationID;
		}
		if (salesOrderDto.ShipContactID != null)
		{
			dataRow["ompShipContactID"] = salesOrderDto.ShipContactID;
		}
		if (salesOrderDto.ARInvoiceLocationID != null)
		{
			dataRow["ompARInvoiceLocationID"] = salesOrderDto.ARInvoiceLocationID;
		}
		if (salesOrderDto.ARInvoiceContactID != null)
		{
			dataRow["ompARInvoiceContactID"] = salesOrderDto.ARInvoiceContactID;
		}
		if (salesOrderDto.OrderCommentsText != null)
		{
			dataRow["ompOrderCommentsText"] = salesOrderDto.OrderCommentsText;
		}
		if (salesOrderDto.OrderCommentsRTF != null)
		{
			dataRow["ompOrderCommentsRTF"] = salesOrderDto.OrderCommentsRTF;
		}
		if (salesOrderDto.EasyOrderID != null)
		{
			dataRow["ompEasyOrderID"] = salesOrderDto.EasyOrderID;
		}
		if (salesOrderDto.CurrencyRateID != null && !string.IsNullOrEmpty(salesOrderDto.CurrencyRateID))
		{
			dataRow["ompCurrencyRateID"] = salesOrderDto.CurrencyRateID;
		}
		if (salesOrderDto.ExchangeRate != 0m)
		{
			dataRow["ompExchangeRate"] = salesOrderDto.ExchangeRate;
		}
		if (salesOrderDto.FreightAmountBase != 0m)
		{
			dataRow["ompFreightAmountBase"] = salesOrderDto.FreightAmountBase;
		}
		if (salesOrderDto.FreightAmountForeign != 0m)
		{
			dataRow["ompFreightAmountForeign"] = salesOrderDto.FreightAmountForeign;
		}
		if (salesOrderDto.FreightTotalBase != 0m)
		{
			dataRow["ompFreightTotalBase"] = salesOrderDto.FreightTotalBase;
		}
		if (salesOrderDto.FreightTotalForeign != 0m)
		{
			dataRow["ompFreightTotalForeign"] = salesOrderDto.FreightTotalForeign;
		}
		if (salesOrderDto.EasyOrderStatus != 0)
		{
			dataRow["ompEasyOrderStatus"] = salesOrderDto.EasyOrderStatus;
		}
		if (salesOrderDto.EasyOrderEnabled)
		{
			dataRow["ompEasyOrderEnabled"] = salesOrderDto.EasyOrderEnabled;
		}
		if (salesOrderDto.FreightTaxCodeID != null)
		{
			dataRow["ompFreightTaxCodeID"] = salesOrderDto.FreightTaxCodeID;
		}
		if (salesOrderDto.SecondFreightTaxCodeID != null)
		{
			dataRow["ompSecondFreightTaxCodeID"] = salesOrderDto.SecondFreightTaxCodeID;
		}
		if (salesOrderDto.PaidFromEasyOrder != 0)
		{
			dataRow["ompEasyOrderPaid"] = 1;
		}
		if (salesOrderDto.PlantID != null)
		{
			dataRow["ompPlantID"] = salesOrderDto.PlantID;
		}
		if (salesOrderDto.ShippingMethodID != null)
		{
			dataRow["ompShippingMethodID"] = salesOrderDto.ShippingMethodID;
		}
		if (salesOrderDto.FreeOnBoardDescription != null)
		{
			dataRow["ompFreeOnBoardDescription"] = salesOrderDto.FreeOnBoardDescription;
		}
		dataRow.EndEdit();
		return Task.FromResult(dataRow);
	}

	private Task<DataRow> saveSalesOrderLine(SalesOrderLineDto salesOrderLine, DataRow soHeaderRow, M1BindingSource salesOrderLineBs)
	{
		DataRow dataRow = null;
		string empty = string.Empty;
		string s = soHeaderRow.Field<string>("ompSalesOrderID");
		empty = "omlSalesOrderID = " + s.ToLinq() + " And omlSalesOrderLineID = " + salesOrderLine.SalesOrderLineID.ToLinq();
		DataRow[] array = salesOrderLineBs.GetDataTable().Select(empty);
		if (array.Length == 0)
		{
			dataRow = salesOrderLineBs.AddNew(salesOrderLineBs.Database, soHeaderRow, null, null) as DataRow;
			if (string.IsNullOrEmpty(salesOrderLine.SalesOrderLineID.ToString()))
			{
				salesOrderLineBs.SetKeyToNextAvailable(dataRow);
			}
			else
			{
				dataRow["omlSalesOrderLineID"] = salesOrderLine.SalesOrderLineID.ToString();
			}
		}
		else
		{
			dataRow = array[0];
		}
		dataRow.BeginEdit();
		if (salesOrderLine.PartID != null)
		{
			dataRow["omlPartID"] = salesOrderLine.PartID;
		}
		if (salesOrderLine.OrgPartID != null)
		{
			dataRow["omlOrgPartID"] = salesOrderLine.OrgPartID;
		}
		if (salesOrderLine.PartRevisionID != null)
		{
			dataRow["omlPartRevisionID"] = salesOrderLine.PartRevisionID;
		}
		if (salesOrderLine.UnitOfMeasure != null)
		{
			dataRow["omlUnitOfMeasure"] = salesOrderLine.UnitOfMeasure;
		}
		if (salesOrderLine.PartGroupID != null)
		{
			dataRow["omlPartGroupID"] = salesOrderLine.PartGroupID;
		}
		if (salesOrderLine.PartShortDescription != null)
		{
			dataRow["omlPartShortDescription"] = salesOrderLine.PartShortDescription;
		}
		if (salesOrderLine.OrgPartShortDescription != null)
		{
			dataRow["omlOrgPartShortDescription"] = salesOrderLine.OrgPartShortDescription;
		}
		if (salesOrderLine.PartLongDescriptionText != null)
		{
			dataRow["omlPartLongDescriptionText"] = salesOrderLine.PartLongDescriptionText;
		}
		if (salesOrderLine.PartLongDescriptionRTF != null)
		{
			dataRow["omlPartLongDescriptionRTF"] = salesOrderLine.PartLongDescriptionRTF;
		}
		if (salesOrderLine.OrderQuantity != 0m)
		{
			dataRow["omlOrderQuantity"] = salesOrderLine.OrderQuantity;
		}
		if (salesOrderLine.FullUnitPriceBase != 0m)
		{
			dataRow["omlFullUnitPriceBase"] = salesOrderLine.FullUnitPriceBase;
		}
		if (salesOrderLine.FullUnitPriceForeign != 0m)
		{
			dataRow["omlFullUnitPriceForeign"] = salesOrderLine.FullUnitPriceForeign;
		}
		if (salesOrderLine.UnitPriceBase != 0m)
		{
			dataRow["omlUnitPriceBase"] = salesOrderLine.UnitPriceBase;
		}
		if (salesOrderLine.UnitPriceForeign != 0m)
		{
			dataRow["omlUnitPriceForeign"] = salesOrderLine.UnitPriceForeign;
		}
		if (salesOrderLine.TaxCodeID != null)
		{
			dataRow["omlTaxCodeID"] = salesOrderLine.TaxCodeID;
		}
		if (salesOrderLine.TaxAmountBase != 0m)
		{
			dataRow["omlTaxAmountBase"] = salesOrderLine.TaxAmountBase;
		}
		if (salesOrderLine.TaxAmountForeign != 0m)
		{
			dataRow["omlTaxAmountForeign"] = salesOrderLine.TaxAmountForeign;
		}
		if (salesOrderLine.Weight != 0m)
		{
			dataRow["omlWeight"] = salesOrderLine.Weight;
		}
		if (salesOrderLine.DiscountPercent != 0m)
		{
			dataRow["omlDiscountPercent"] = salesOrderLine.DiscountPercent;
		}
		if (salesOrderLine.UnitDiscountBase != 0m)
		{
			dataRow["omlUnitDiscountBase"] = salesOrderLine.UnitDiscountBase;
		}
		if (salesOrderLine.UnitDiscountForeign != 0m)
		{
			dataRow["omlUnitDiscountForeign"] = salesOrderLine.UnitDiscountForeign;
		}
		if (salesOrderLine.SecondTaxAmountForeign != 0m)
		{
			dataRow["omlSecondTaxAmountForeign"] = salesOrderLine.SecondTaxAmountForeign;
		}
		if (salesOrderLine.ReleaseNumber != null)
		{
			dataRow["omlReleaseNumber"] = salesOrderLine.ReleaseNumber;
		}
		if (salesOrderLine.CreatedBy != null)
		{
			dataRow["omlCreatedBy"] = salesOrderLine.CreatedBy;
		}
		if (salesOrderLine.CreatedDate.HasValue && salesOrderLine.CreatedDate.Value.Year > 2000)
		{
			dataRow["omlCreatedDate"] = salesOrderLine.CreatedDate;
		}
		dataRow.EndEdit();
		return Task.FromResult(dataRow);
	}

	private Task<string> saveSalesOrderDelivery(SalesOrderDeliveryDto salesOrderDelivery, DataRow soLineRow, M1BindingSource salesOrderDeliveryBs)
	{
		DataRow dataRow = null;
		string empty = string.Empty;
		string text = soLineRow.Field<string>("omlSalesOrderID");
		empty = "omdSalesOrderID = " + text.ToLinq() + " And omdSalesOrderLineID = " + salesOrderDelivery.SalesOrderLineID.ToLinq() + " And omdSalesOrderDeliveryID = " + salesOrderDelivery.SalesOrderDeliveryID.ToLinq();
		DataRow[] array = salesOrderDeliveryBs.GetDataTable().Select(empty);
		if (array.Length == 0)
		{
			dataRow = salesOrderDeliveryBs.AddNew(salesOrderDeliveryBs.Database, soLineRow, null, null) as DataRow;
			if (string.IsNullOrEmpty(salesOrderDelivery.SalesOrderDeliveryID.ToString()))
			{
				salesOrderDeliveryBs.SetKeyToNextAvailable(dataRow);
			}
			else
			{
				dataRow["omdSalesOrderDeliveryID"] = salesOrderDelivery.SalesOrderDeliveryID.ToString();
			}
		}
		else
		{
			dataRow = array[0];
		}
		dataRow.BeginEdit();
		if (salesOrderDelivery.PartID != null)
		{
			dataRow["omdPartID"] = salesOrderDelivery.PartID;
		}
		if (salesOrderDelivery.PartRevisionID != null)
		{
			dataRow["omdPartRevisionID"] = salesOrderDelivery.PartRevisionID;
		}
		if (salesOrderDelivery.PartWarehouseLocationID != null)
		{
			dataRow["omdPartWarehouseLocationID"] = salesOrderDelivery.PartWarehouseLocationID;
		}
		if (salesOrderDelivery.PartBinID != null)
		{
			dataRow["omdPartBinID"] = salesOrderDelivery.PartBinID;
		}
		if (salesOrderDelivery.DeliveryQuantity != 0m)
		{
			dataRow["omdDeliveryQuantity"] = salesOrderDelivery.DeliveryQuantity;
		}
		_ = salesOrderDelivery.DeliveryDate;
		dataRow["omdDeliveryDate"] = salesOrderDelivery.DeliveryDate;
		if (salesOrderDelivery.DeliveryType != 0)
		{
			dataRow["omdDeliveryType"] = salesOrderDelivery.DeliveryType;
		}
		if (salesOrderDelivery.CustomerOrganizationID != null)
		{
			dataRow["omdCustomerOrganizationID"] = salesOrderDelivery.CustomerOrganizationID;
		}
		if (salesOrderDelivery.CreatedBy != null)
		{
			dataRow["omdCreatedBy"] = salesOrderDelivery.CreatedBy;
		}
		if (salesOrderDelivery.CreatedDate.HasValue && salesOrderDelivery.CreatedDate.Value.Year > 2000)
		{
			dataRow["omdCreatedDate"] = salesOrderDelivery.CreatedDate;
		}
		if (salesOrderDelivery.Firm)
		{
			dataRow["omdFirm"] = salesOrderDelivery.Firm;
		}
		dataRow.EndEdit();
		return Task.FromResult(text);
	}

	private Task<string> saveFullSalesOrder(SalesOrderDto salesOrderDto, M1BindingSource salesOrderBs)
	{
		DataRow result = saveSalesOrderHeader(salesOrderDto, salesOrderBs).Result;
		string result2 = result.Field<string>("ompSalesOrderID");
		if (salesOrderDto.SalesOrderLines != null)
		{
			M1BindingSource childBindingSource = salesOrderBs.PrimaryTable.GetChildBindingSource("SalesOrderLines");
			childBindingSource.ClearCache();
			M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries");
			childBindingSource2.ClearCache();
			foreach (SalesOrderLineDto salesOrderLine in salesOrderDto.SalesOrderLines)
			{
				DataRow result3 = saveSalesOrderLine(salesOrderLine, result, childBindingSource).Result;
				if (salesOrderLine.SalesOrderDeliveries == null)
				{
					continue;
				}
				foreach (SalesOrderDeliveryDto salesOrderDelivery in salesOrderLine.SalesOrderDeliveries)
				{
					saveSalesOrderDelivery(salesOrderDelivery, result3, childBindingSource2);
				}
			}
		}
		return Task.FromResult(result2);
	}

	public SalesOrderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public SalesOrderRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesSalesOrderExists(string orderId)
	{
		return DoesSalesOrderExists(orderId, null);
	}

	public Task<bool> DoesSalesOrderExists(string orderId, SqlTransaction sqlTransaction)
	{
		InitializeParameterLists();
		base.filterList.Add("ompSalesOrderID|C", orderId);
		base.selectList.Add("ompSalesOrderID");
		return Task.FromResult(GetAsObject("SalesOrders", base.filterList, base.selectList, null, sqlTransaction) != null);
	}

	public Task<bool> DoesEOSalesOrderExists(string m1OrderId)
	{
		return DoesEOSalesOrderExists(m1OrderId, null);
	}

	public Task<bool> DoesEOSalesOrderExists(string m1OrderId, SqlTransaction sqlTransaction)
	{
		InitializeParameterLists();
		base.filterList.Add("ompSalesOrderID|C", m1OrderId);
		base.filterList.Add("ompEasyOrderEnabled|L", true);
		base.selectList.Add("ompSalesOrderID");
		return Task.FromResult(GetAsObject("SalesOrders", base.filterList, base.selectList, null, sqlTransaction) != null);
	}

	public Task<bool> DoesEOSalesOrderIDExists(string easyOrderID, out string salesOrderID)
	{
		return DoesEOSalesOrderIDExists(easyOrderID, null, out salesOrderID);
	}

	public Task<bool> DoesEOSalesOrderIDExists(string easyOrderID, SqlTransaction sqlTransaction, out string salesOrderID)
	{
		bool result = false;
		salesOrderID = string.Empty;
		InitializeParameterLists();
		base.filterList.Add("ompEasyOrderID|C", easyOrderID);
		base.selectList.Add("ompSalesOrderID");
		object asObject = GetAsObject("SalesOrders", base.filterList, base.selectList, null, sqlTransaction);
		if (asObject != null)
		{
			salesOrderID = asObject.ToString().Trim();
			result = true;
		}
		return Task.FromResult(result);
	}

	public Task<bool> DoesEDISalesOrderExists(string orderId)
	{
		return DoesEDISalesOrderExists(orderId, null);
	}

	public Task<bool> DoesEDISalesOrderExists(string orderId, SqlTransaction sqlTransaction)
	{
		InitializeParameterLists();
		base.filterList.Add("ompSalesOrderID|C", orderId);
		base.filterList.Add("ompCreatedByEDI|C", true);
		base.selectList.Add("ompSalesOrderID");
		return Task.FromResult(GetAsObject("SalesOrders", base.filterList, base.selectList, null, sqlTransaction) != null);
	}

	public Task<SalesOrderDto> GetSalesOrderInfor(string orderId, bool headerOnly = false)
	{
		return GetSalesOrderInfor(orderId, null, headerOnly);
	}

	public Task<SalesOrderDto> GetSalesOrderInfor(string orderId, SqlTransaction sqlTransaction, bool headerOnly = false)
	{
		SalesOrderDto salesOrderDto = null;
		new SalesOrderLineDto();
		new SalesOrderDeliveryDto();
		List<SalesOrderDeliveryDto> orderDeliveryList = null;
		List<SalesOrderLineDto> orderLineList = new List<SalesOrderLineDto>();
		List<SalesOrderSalespeopleDto> salespeopleList = new List<SalesOrderSalespeopleDto>();
		if (!headerOnly)
		{
			InitializeParameterLists();
			base.filterList.Add("omdSalesOrderID|C", orderId);
			base.selectList.AddRange(new string[14]
			{
				"omdSalesOrderID", "omdCustomerOrganizationID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omdPartID", "omdPartRevisionID", "omdDeliveryQuantity", "omdDeliveryType", "omdDeliveryDate", "omdPartWarehouseLocationID",
				"omdPartBinID", "omdFirm", "omdCreatedBy", "omdCreatedDate"
			});
			using (DataTable source = GetAsDataTable("SalesOrderDeliveries", base.filterList, base.selectList, null, sqlTransaction))
			{
				orderDeliveryList = (from del in source.AsEnumerable()
					select new SalesOrderDeliveryDto
					{
						SalesOrderID = del.Field<string>("omdSalesOrderID"),
						CustomerOrganizationID = del.Field<string>("omdCustomerOrganizationID"),
						SalesOrderLineID = del.Field<short>("omdSalesOrderLineID"),
						SalesOrderDeliveryID = del.Field<short>("omdSalesOrderDeliveryID"),
						PartID = del.Field<string>("omdPartID"),
						PartRevisionID = del.Field<string>("omdPartRevisionID"),
						DeliveryQuantity = del.Field<decimal>("omdDeliveryQuantity"),
						DeliveryType = del.Field<byte>("omdDeliveryType"),
						DeliveryDate = del.Field<DateTime>("omdDeliveryDate"),
						PartWarehouseLocationID = del.Field<string>("omdPartWarehouseLocationID"),
						PartBinID = del.Field<string>("omdPartBinID"),
						Firm = del.Field<bool>("omdFirm"),
						CreatedBy = del.Field<string>("omdCreatedBy"),
						CreatedDate = del.Field<DateTime?>("omdCreatedDate")
					}).ToList();
			}
			InitializeParameterLists();
			base.filterList.Add("omlSalesOrderID|C", orderId);
			base.selectList.AddRange(new string[31]
			{
				"omlSalesOrderID", "omlSalesOrderLineID", "omlOrgPartID", "omlPartID", "omlPartGroupID", "omlOrgPartShortDescription", "omlPartShortDescription", "omlPartLongDescriptionText", "omlPartLongDescriptionRtf", "omlPartRevisionID",
				"omlUnitOfMeasure", "omlWeight", "omlOrderQuantity", "omlTaxCodeID", "omlFullUnitPriceBase", "omlFullUnitPriceForeign", "omlUnitPriceBase", "omlUnitPriceForeign", "omlFullExtendedPriceBase", "omlFullExtendedPriceForeign",
				"omlExtendedPriceBase", "omlExtendedPriceForeign", "omlTaxAmountBase", "omlTaxAmountForeign", "omlDiscountPercent", "omlSecondTaxAmountForeign", "omlCreatedBy", "omlCreatedDate", "omlReleaseNumber", "omlSecondTaxCodeID",
				"omlNonTaxReasonID"
			});
			using DataTable source2 = GetAsDataTable("SalesOrderLines", base.filterList, base.selectList, null, sqlTransaction);
			orderLineList = (from line in source2.AsEnumerable()
				let lineDelivery = orderDeliveryList.Where((SalesOrderDeliveryDto d) => d.SalesOrderLineID.Equals(line.Field<short>("omlSalesOrderLineID"))).ToList()
				select new SalesOrderLineDto
				{
					SalesOrderID = line.Field<string>("omlSalesOrderID"),
					SalesOrderLineID = line.Field<short>("omlSalesOrderLineID"),
					OrgPartID = line.Field<string>("omlOrgPartID"),
					PartID = line.Field<string>("omlPartID"),
					PartGroupID = line.Field<string>("omlPartGroupID"),
					OrgPartShortDescription = line.Field<string>("omlOrgPartShortDescription"),
					PartShortDescription = line.Field<string>("omlPartShortDescription"),
					PartLongDescriptionText = line.Field<string>("omlPartLongDescriptionText"),
					PartLongDescriptionRTF = line.Field<string>("omlPartLongDescriptionRtf"),
					PartRevisionID = line.Field<string>("omlPartRevisionID"),
					UnitOfMeasure = line.Field<string>("omlUnitOfMeasure"),
					Weight = line.Field<decimal>("omlWeight"),
					OrderQuantity = line.Field<decimal>("omlOrderQuantity"),
					TaxCodeID = line.Field<string>("omlTaxCodeID"),
					SecondTaxCodeID = line.Field<string>("omlSecondTaxCodeID"),
					FullUnitPriceBase = line.Field<decimal>("omlFullUnitPriceBase"),
					FullUnitPriceForeign = line.Field<decimal>("omlFullUnitPriceForeign"),
					UnitPriceBase = line.Field<decimal>("omlUnitPriceBase"),
					UnitPriceForeign = line.Field<decimal>("omlUnitPriceForeign"),
					FullExtendedPriceBase = line.Field<decimal>("omlFullExtendedPriceBase"),
					FullExtendedPriceForeign = line.Field<decimal>("omlFullExtendedPriceForeign"),
					ExtendedPriceBase = line.Field<decimal>("omlExtendedPriceBase"),
					ExtendedPriceForeign = line.Field<decimal>("omlExtendedPriceForeign"),
					TaxAmountBase = line.Field<decimal>("omlTaxAmountBase"),
					TaxAmountForeign = line.Field<decimal>("omlTaxAmountForeign"),
					DiscountPercent = line.Field<decimal>("omlDiscountPercent"),
					SecondTaxAmountForeign = line.Field<decimal>("omlSecondTaxAmountForeign"),
					ReleaseNumber = line.Field<string>("omlReleaseNumber"),
					CreatedBy = line.Field<string>("omlCreatedBy"),
					CreatedDate = line.Field<DateTime?>("omlCreatedDate"),
					NonTaxReasonID = line.Field<string>("omlNonTaxReasonID"),
					SalesOrderDeliveries = lineDelivery
				}).ToList();
		}
		InitializeParameterLists();
		base.filterList.Add("omiSalesOrderID|C", orderId);
		base.selectList.AddRange(new string[4] { "omiSalesOrderID", "omiSequenceID", "omiSalesEmployeeID", "omiPercent" });
		using (DataTable dataTable = GetAsDataTable("SalesOrderSalesPeople", base.filterList, base.selectList, null, sqlTransaction))
		{
			salespeopleList = (from r in dataTable?.AsEnumerable()
				select new SalesOrderSalespeopleDto
				{
					SequenceID = r.Field<short>("omiSequenceID"),
					SalesEmployeeID = r.Field<string>("omiSalesEmployeeID"),
					Percent = r.Field<decimal>("omiPercent"),
					SalesOrderID = r.Field<string>("omiSalesOrderID")
				}).ToList();
		}
		InitializeParameterLists();
		base.filterList.Add("ompSalesOrderID|C", orderId);
		base.selectList.AddRange(new string[43]
		{
			"ompSalesOrderID", "ompCustomerOrganizationID", "ompOrderDate", "ompRequestedShipDate", "ompShipOrganizationID", "ompShipLocationID", "ompARInvoiceLocationID", "ompCustomerPO", "ompPaymentTermID", "ompPaymentTermID",
			"ompCurrencyRateID", "ompExchangeRate", "ompStatus", "ompFullOrderSubtotalBase", "ompFullOrderSubtotalForeign", "ompOrderSubtotalBase", "ompOrderSubTotalForeign", "ompOrderTaxAmountBase", "ompOrderTaxAmountForeign", "ompOrderTotalBase",
			"ompOrderTotalForeign", "ompTotalOrderWeight", "ompOrderCommentsText", "ompCreatedBy", "ompCreatedDate", "ompCreatedByEDI", "ompCreatedFromWeb", "ompFreightAmountBase", "ompFreightAmountForeign", "ompFreightTotalBase",
			"ompFreightTotalForeign", "ompEasyOrderEnabled", "ompEasyOrderStatus", "ompEasyOrderID", "ompShipContactID", "ompARInvoiceContactID", "ompPlantID", "ompShippingMethodID", "ompFreeOnBoardDescription", "ompShippingPaymentTypeID",
			"ompOrderCommentsRtf", "ompFreightTaxCodeID", "ompSecondFreightTaxCodeID"
		});
		using (DataTable source3 = GetAsDataTable("SalesOrders", base.filterList, base.selectList, null, sqlTransaction))
		{
			salesOrderDto = (from order in source3.AsEnumerable()
				select new SalesOrderDto
				{
					SalesOrderID = order.Field<string>("ompSalesOrderID"),
					CustomerOrganizationID = order.Field<string>("ompCustomerOrganizationID"),
					OrderDate = order.Field<DateTime>("ompOrderDate"),
					RequestedShipDate = order.Field<DateTime?>("ompRequestedShipDate"),
					ShipOrganizationID = order.Field<string>("ompShipOrganizationID"),
					ShipLocationID = order.Field<string>("ompShipLocationID"),
					ShipContactID = order.Field<string>("ompShipContactID"),
					ARInvoiceLocationID = order.Field<string>("ompARInvoiceLocationID"),
					ARInvoiceContactID = order.Field<string>("ompARInvoiceContactID"),
					CustomerPO = order.Field<string>("ompCustomerPO"),
					PaymentTermID = order.Field<string>("ompPaymentTermID"),
					CurrencyRateID = order.Field<string>("ompCurrencyRateID"),
					ExchangeRate = order.Field<decimal>("ompExchangeRate"),
					Status = order.Field<byte>("ompStatus"),
					FullOrderSubtotalBase = order.Field<decimal>("ompFullOrderSubtotalBase"),
					FullOrderSubtotalForeign = order.Field<decimal>("ompFullOrderSubtotalForeign"),
					OrderSubtotalBase = order.Field<decimal>("ompOrderSubtotalBase"),
					OrderSubTotalForeign = order.Field<decimal>("ompOrderSubTotalForeign"),
					OrderTaxAmountBase = order.Field<decimal>("ompOrderTaxAmountBase"),
					OrderTaxAmountForeign = order.Field<decimal>("ompOrderTaxAmountForeign"),
					OrderTotalBase = order.Field<decimal>("ompOrderTotalBase"),
					OrderTotalForeign = order.Field<decimal>("ompOrderTotalForeign"),
					TotalOrderWeight = order.Field<decimal>("ompTotalOrderWeight"),
					OrderCommentsText = order.Field<string>("ompOrderCommentsText"),
					OrderCommentsRTF = order.Field<string>("ompOrderCommentsRtf"),
					CreatedBy = order.Field<string>("ompCreatedBy"),
					CreatedDate = order.Field<DateTime?>("ompCreatedDate"),
					CreatedByEDI = order.Field<bool>("ompCreatedByEDI"),
					CreatedFromWeb = order.Field<bool>("ompCreatedFromWeb"),
					FreightAmountBase = order.Field<decimal>("ompFreightAmountBase"),
					FreightAmountForeign = order.Field<decimal>("ompFreightAmountForeign"),
					FreightTotalBase = order.Field<decimal>("ompFreightTotalBase"),
					FreightTotalForeign = order.Field<decimal>("ompFreightTotalForeign"),
					EasyOrderEnabled = order.Field<bool>("ompEasyOrderEnabled"),
					EasyOrderStatus = order.Field<byte>("ompEasyOrderStatus"),
					EasyOrderID = order.Field<string>("ompEasyOrderID"),
					PlantID = order.Field<string>("ompPlantID"),
					ShippingMethodID = order.Field<string>("ompShippingMethodID"),
					FreeOnBoardDescription = order.Field<string>("ompFreeOnBoardDescription"),
					ShippingPaymentTypeID = order.Field<string>("ompShippingPaymentTypeID"),
					FreightTaxCodeID = order.Field<string>("ompFreightTaxCodeID"),
					SecondFreightTaxCodeID = order.Field<string>("ompSecondFreightTaxCodeID"),
					SalesOrderLines = ((orderLineList != null) ? orderLineList : new List<SalesOrderLineDto>()),
					SalesOrderSalesPeople = new List<SalesOrderSalespeopleDto>(salespeopleList)
				}).SingleOrDefault();
		}
		return Task.FromResult((salesOrderDto == null) ? new SalesOrderDto() : salesOrderDto);
	}

	public Task<string> GetSalesOrderList_ForCustomerPO(string customerPO, string orgId, SqlTransaction sqlTransaction)
	{
		StringBuilder stringBuilder = new StringBuilder();
		InitializeParameterLists();
		base.filterList.Add("ompCustomerOrganizationID|C", orgId);
		base.filterList.Add("ompCustomerPO|C", customerPO);
		base.OrderOrGroupByList.Add("ompSalesOrderID DESC");
		base.selectList.Add("ompSalesOrderID");
		using (DataTable dataTable = GetAsDataTable("SalesOrders", base.filterList, base.selectList, base.OrderOrGroupByList, sqlTransaction))
		{
			foreach (DataRow row in dataTable.Rows)
			{
				stringBuilder.Append(row.Field<string>("ompSalesOrderID").Trim());
				stringBuilder.Append("|");
			}
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return Task.FromResult(stringBuilder.ToString().Trim());
	}

	public Task<byte> GetDefaultSalesOrderDeliveryType()
	{
		byte b = Convert.ToByte(base.M1database.Props("PM").Field<byte>("xapOMDeliveryType"));
		b = (byte)((b == 0) ? 1 : b);
		return Task.FromResult(b);
	}

	public Task<bool> SaveSalesOrderHeader(SalesOrderDto salesOrder)
	{
		try
		{
			M1BindingSource m1BindingSource = new M1BindingSource(base.M1database);
			m1BindingSource.DataSourceTable = "SalesOrders";
			saveSalesOrderHeader(salesOrder, m1BindingSource);
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return Task.FromResult(result: true);
	}

	public Task<SaveResponseDto> SaveSalesOrder(SalesOrderDto salesOrder)
	{
		string salesOrder2 = string.Empty;
		SqlTransaction sqlTransaction = null;
		SaveResponseDto result = null;
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using (M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, sqlTransaction))
			{
				m1BindingSource.ClearCache();
				m1BindingSource.DataSourceTable = "SalesOrders";
				salesOrder2 = saveFullSalesOrder(salesOrder, m1BindingSource).Result;
				m1BindingSource.SaveData();
				base.M1database.CommitTransaction(sqlTransaction);
			}
			result = new SaveResponseDto(isSuccess: true, salesOrder2, new List<string>());
		}
		catch (Exception ex)
		{
			salesOrder2 = string.Empty;
			base.M1database.RollbackTransaction(sqlTransaction);
			result = new SaveResponseDto(isSuccess: false, string.Empty, ex.Message);
		}
		finally
		{
			sqlTransaction.Dispose();
		}
		return Task.FromResult(result);
	}

	public Task<SaveResponseDto> SaveSalesOrder(SalesOrderDto salesOrder, SqlTransaction sqlTransaction)
	{
		string empty = string.Empty;
		SaveResponseDto saveResponseDto = null;
		try
		{
			M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, sqlTransaction);
			m1BindingSource.DataSourceTable = "SalesOrders";
			empty = saveFullSalesOrder(salesOrder, m1BindingSource).Result;
			m1BindingSource.SaveData();
			saveResponseDto = new SaveResponseDto(isSuccess: true, empty, new List<string>());
		}
		catch (Exception ex)
		{
			empty = string.Empty;
			saveResponseDto = new SaveResponseDto(isSuccess: false, string.Empty, ex.Message);
		}
		return Task.FromResult(saveResponseDto);
	}

	public Task<SaveResponseDto> SaveSalesOrder(SalesOrderDto salesOrder, M1BindingSource salesOrderBs)
	{
		string empty = string.Empty;
		SaveResponseDto saveResponseDto = null;
		try
		{
			empty = saveFullSalesOrder(salesOrder, salesOrderBs).Result;
			saveResponseDto = new SaveResponseDto(isSuccess: true, empty, new List<string>());
		}
		catch (Exception ex)
		{
			empty = string.Empty;
			saveResponseDto = new SaveResponseDto(isSuccess: false, string.Empty, ex.Message);
		}
		return Task.FromResult(saveResponseDto);
	}

	public Task<SalesOrderLineDto> GetSalesOrderLineInfor(string orderId, short orderLineId)
	{
		SalesOrderLineDto result = null;
		IList<SalesOrderDeliveryDto> orderDeliveryList = null;
		InitializeParameterLists();
		base.filterList.Add("omdSalesOrderID|C", orderId);
		base.filterList.Add("omdSalesOrderLineID", orderLineId);
		base.selectList.AddRange(new string[14]
		{
			"omdSalesOrderID", "omdCustomerOrganizationID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omdPartID", "omdPartRevisionID", "omdDeliveryQuantity", "omdDeliveryType", "omdDeliveryDate", "omdPartWarehouseLocationID",
			"omdPartBinID", "omdFirm", "omdCreatedBy", "omdCreatedDate"
		});
		using (DataTable source = GetAsDataTable("SalesOrderDeliveries", base.filterList, base.selectList, null, null))
		{
			orderDeliveryList = (from del in source.AsEnumerable()
				select new SalesOrderDeliveryDto
				{
					SalesOrderID = del.Field<string>("omdSalesOrderID"),
					CustomerOrganizationID = del.Field<string>("omdCustomerOrganizationID"),
					SalesOrderLineID = del.Field<short>("omdSalesOrderLineID"),
					SalesOrderDeliveryID = del.Field<short>("omdSalesOrderDeliveryID"),
					PartID = del.Field<string>("omdPartID"),
					PartRevisionID = del.Field<string>("omdPartRevisionID"),
					DeliveryQuantity = del.Field<decimal>("omdDeliveryQuantity"),
					DeliveryType = del.Field<byte>("omdDeliveryType"),
					DeliveryDate = del.Field<DateTime>("omdDeliveryDate"),
					PartWarehouseLocationID = del.Field<string>("omdPartWarehouseLocationID"),
					PartBinID = del.Field<string>("omdPartBinID"),
					Firm = del.Field<bool>("omdFirm"),
					CreatedBy = del.Field<string>("omdCreatedBy"),
					CreatedDate = del.Field<DateTime?>("omdCreatedDate")
				}).ToList();
		}
		InitializeParameterLists();
		base.filterList.Add("omlSalesOrderID|C", orderId);
		base.filterList.Add("omlSalesOrderLineID|C", orderLineId);
		base.selectList.AddRange(new string[28]
		{
			"omlSalesOrderID", "omlSalesOrderLineID", "omlOrgPartID", "omlPartID", "omlPartGroupID", "omlOrgPartShortDescription", "omlPartShortDescription", "omlPartLongDescriptionText", "omlPartRevisionID", "omlUnitOfMeasure",
			"omlWeight", "omlOrderQuantity", "omlTaxCodeID", "omlFullUnitPriceBase", "omlFullUnitPriceForeign", "omlUnitPriceBase", "omlUnitPriceForeign", "omlFullExtendedPriceBase", "omlFullExtendedPriceForeign", "omlExtendedPriceBase",
			"omlExtendedPriceForeign", "omlTaxAmountBase", "omlTaxAmountForeign", "omlDiscountPercent", "omlSecondTaxAmountForeign", "omlCreatedBy", "omlCreatedDate", "omlReleaseNumber"
		});
		using (DataTable source2 = GetAsDataTable("SalesOrderLines", base.filterList, base.selectList, null, null))
		{
			result = (from line in source2.AsEnumerable()
				select new SalesOrderLineDto
				{
					SalesOrderID = line.Field<string>("omlSalesOrderID"),
					SalesOrderLineID = line.Field<short>("omlSalesOrderLineID"),
					OrgPartID = line.Field<string>("omlOrgPartID"),
					PartID = line.Field<string>("omlPartID"),
					PartGroupID = line.Field<string>("omlPartGroupID"),
					OrgPartShortDescription = line.Field<string>("omlOrgPartShortDescription"),
					PartShortDescription = line.Field<string>("omlPartShortDescription"),
					PartLongDescriptionText = line.Field<string>("omlPartLongDescriptionText"),
					PartRevisionID = line.Field<string>("omlPartRevisionID"),
					UnitOfMeasure = line.Field<string>("omlUnitOfMeasure"),
					Weight = line.Field<decimal>("omlWeight"),
					OrderQuantity = line.Field<decimal>("omlOrderQuantity"),
					TaxCodeID = line.Field<string>("omlTaxCodeID"),
					FullUnitPriceBase = line.Field<decimal>("omlFullUnitPriceBase"),
					FullUnitPriceForeign = line.Field<decimal>("omlFullUnitPriceForeign"),
					UnitPriceBase = line.Field<decimal>("omlUnitPriceBase"),
					UnitPriceForeign = line.Field<decimal>("omlUnitPriceForeign"),
					FullExtendedPriceBase = line.Field<decimal>("omlFullExtendedPriceBase"),
					FullExtendedPriceForeign = line.Field<decimal>("omlFullExtendedPriceForeign"),
					ExtendedPriceBase = line.Field<decimal>("omlExtendedPriceBase"),
					ExtendedPriceForeign = line.Field<decimal>("omlExtendedPriceForeign"),
					TaxAmountBase = line.Field<decimal>("omlTaxAmountBase"),
					TaxAmountForeign = line.Field<decimal>("omlTaxAmountForeign"),
					DiscountPercent = line.Field<decimal>("omlDiscountPercent"),
					SecondTaxAmountForeign = line.Field<decimal>("omlSecondTaxAmountForeign"),
					CreatedBy = line.Field<string>("omlCreatedBy"),
					CreatedDate = line.Field<DateTime?>("omlCreatedDate"),
					ReleaseNumber = line.Field<string>("omlReleaseNumber"),
					SalesOrderDeliveries = orderDeliveryList.ToList()
				}).FirstOrDefault();
		}
		return Task.FromResult(result);
	}

	public Task<string> CreateEDISalesOrderLog(CTMSalesOrderDto ctmOrder, SqlTransaction sqlTransaction)
	{
		string result = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		SalesOrderDto m1SalesOrder = ctmOrder.M1SalesOrder;
		try
		{
			stringBuilder.Length = 0;
			stringBuilder.Append("Ship Location ID=");
			stringBuilder.Append(m1SalesOrder.ShipLocationID.Trim());
			stringBuilder.AppendLine();
			stringBuilder.Append("AR Invoice Location ID=");
			stringBuilder.Append(m1SalesOrder.ARInvoiceLocationID.Trim());
			stringBuilder.AppendLine();
			stringBuilder.Append("Shipping Method ID=");
			stringBuilder.AppendLine(m1SalesOrder.ShippingMethodID.Trim());
			foreach (SalesOrderLineDto salesOrderLine in m1SalesOrder.SalesOrderLines)
			{
				stringBuilder2.Length = 0;
				stringBuilder3.Length = 0;
				stringBuilder2.AppendLine(stringBuilder.ToString().Trim());
				stringBuilder2.AppendLine();
				stringBuilder2.Append("Sales Order Line ID=");
				stringBuilder2.AppendLine(salesOrderLine.SalesOrderLineID.ToString().Trim());
				stringBuilder2.Append("Part ID=");
				stringBuilder2.AppendLine(salesOrderLine.PartID.Trim());
				stringBuilder2.Append("Part Revision ID=");
				stringBuilder2.AppendLine(salesOrderLine.PartRevisionID.Trim());
				stringBuilder2.Append("Release Number=");
				stringBuilder2.AppendLine(salesOrderLine.ReleaseNumber.Trim());
				stringBuilder3.AppendLine("[");
				foreach (SalesOrderDeliveryDto salesOrderDelivery in salesOrderLine.SalesOrderDeliveries)
				{
					if (stringBuilder3.Length > 3)
					{
						stringBuilder3.AppendLine();
					}
					stringBuilder3.Append("Delivery ID=");
					stringBuilder3.AppendLine(salesOrderDelivery.SalesOrderDeliveryID.ToString().Trim());
					stringBuilder3.Append("Delivery Quantity=");
					stringBuilder3.AppendLine(salesOrderDelivery.DeliveryQuantity.ToString().Trim());
					stringBuilder3.Append("Delivery Date=");
					stringBuilder3.AppendLine(salesOrderDelivery.DeliveryDate.ToShortDateString().Trim());
				}
				stringBuilder3.Append("]");
				stringBuilder2.Append(stringBuilder3);
				using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, sqlTransaction);
				m1BindingSource.DataSourceTable = "EDISalesOrderChangeLog";
				DataRow obj = m1BindingSource.AddNew() as DataRow;
				obj.BeginEdit();
				obj["omeCustomerPO"] = m1SalesOrder.CustomerPO;
				obj["omeCustomerPOLineID"] = salesOrderLine.SalesOrderLineID;
				obj["omeChangeType"] = "U";
				obj["omeChangeRequestDate"] = salesOrderLine.CreatedDate;
				obj["omeTableNewValues"] = stringBuilder2.ToString().Trim();
				obj["omeSalesOrderIDsText"] = ctmOrder.CurrentM1SalesorderIDs.Trim();
				obj["omeVerifyStatus"] = 0;
				obj.EndEdit();
				m1BindingSource.SaveData();
			}
		}
		catch (Exception ex)
		{
			result = ex.Message;
		}
		return Task.FromResult(result);
	}

	public Task<bool> CreateSalesOrderMemo(CTMSalesOrderDto ctmOrder, SqlTransaction sqlTransaction)
	{
		DataRow dataRow = null;
		string value = ctmOrder.CurrentM1SalesorderIDs.Split('|').ToList().FirstOrDefault();
		SalesOrderDto m1SalesOrder = ctmOrder.M1SalesOrder;
		if (!string.IsNullOrWhiteSpace(value))
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, sqlTransaction);
			m1BindingSource.DataSourceTable = "SalesOrderMemos";
			dataRow = m1BindingSource.AddNew() as DataRow;
			m1BindingSource.SetKeyToNextAvailable(dataRow);
			string value2 = "EDI Replacement order received.";
			string text = "View EDI sales order change log for more details.";
			string value3 = APICommonFunctions.ConvertStringToRTF(text.Trim());
			dataRow.BeginEdit();
			dataRow["omkSalesOrderID"] = value;
			dataRow["omkMemoDate"] = m1SalesOrder.CreatedDate;
			dataRow["omkShortDescription"] = value2;
			dataRow["omkClosed"] = false;
			dataRow["omkCreatedBy"] = m1SalesOrder.CreatedBy;
			dataRow["omkCreatedDate"] = m1SalesOrder.CreatedDate;
			dataRow["omkLongDescriptionRTF"] = value3;
			dataRow["omkLongDescriptionText"] = text;
			dataRow["omkShowInSalesOrders"] = true;
			dataRow.EndEdit();
			m1BindingSource.SaveData();
		}
		return Task.FromResult(result: true);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
