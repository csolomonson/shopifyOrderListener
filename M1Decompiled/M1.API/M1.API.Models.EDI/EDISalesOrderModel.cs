using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.EDI;
using M1.API.Repositories.Core;
using M1.API.Utilities;
using M1.Ax.Erp;

namespace M1.API.Models.EDI;

public class EDISalesOrderModel : EDIBaseModel, IEDISalesOrderModel, IEDIBaseModel, IAPIBaseModel, IDisposable
{
	public EDISalesOrderModel(APIClientContext clientContext)
		: base(clientContext)
	{
		base.SalesOrderRepository = new SalesOrderRepository(clientContext);
		base.OrganizationRepository = new OrganizationRepository(clientContext);
		base.PartRepository = new PartRepository(clientContext);
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetOrder(string m1SalesOrderId)
	{
		APIValidationInfoDto aPIValidationInfoDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			if (!base.SalesOrderRepository.DoesEDISalesOrderExists(m1SalesOrderId).Result)
			{
				httpValidationStatusCode = HttpStatusCode.OK;
				base.ErrorsList.Add("Sales Order " + m1SalesOrderId + " is invalid or not an EDI created order");
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the salesorder [" + m1SalesOrderId + "]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public Task<GetOrderResponseDto> Process_GetOrder(string m1SalesOrderId)
	{
		SalesOrderDto salesOrder = null;
		GetOrderResponseDto getOrderResponseDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		try
		{
			salesOrder = base.SalesOrderRepository.GetSalesOrderInfor(m1SalesOrderId).Result;
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Salesorder [" + m1SalesOrderId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			getOrderResponseDto = new GetOrderResponseDto
			{
				SalesOrder = salesOrder,
				ValidationInfo = validationInfo
			};
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(getOrderResponseDto);
	}

	public Task<PostOrderResponseDto> ValidateRequest_PostOrder(IList<EDI850SalesOrderIN> salesOrders)
	{
		PostOrderResponseDto postOrderResponseDto = null;
		CTMSalesOrderDto cTMSalesOrderDto = null;
		SalesOrderDto salesOrderDto = null;
		OrganizationInformationDto organizationInformationDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		string empty = string.Empty;
		byte b = 0;
		PriceCalculation priceCalculation = new PriceCalculation();
		try
		{
			postOrderResponseDto = new PostOrderResponseDto();
			if (salesOrders != null && salesOrders.Count == 0)
			{
				base.ErrorsList.Add("No records found in the request or invalid format.");
				postOrderResponseDto.GeneralValidatationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, HttpStatusCode.BadRequest);
			}
			else
			{
				foreach (EDI850SalesOrderIN salesOrder in salesOrders)
				{
					cTMSalesOrderDto = new CTMSalesOrderDto();
					organizationInformationDto = new OrganizationInformationDto();
					base.ErrorsList = new List<string>();
					base.WarningsList = new List<string>();
					salesOrderDto = new SalesOrderDto();
					salesOrderDto.OrderDate = salesOrder.OrderDate.Value;
					salesOrderDto.SalesOrderID = salesOrder.SalesOrderID.ToString();
					if (!string.IsNullOrWhiteSpace(salesOrder.CustomerOrganizationID))
					{
						GetOrganizationDataParam parameter = new GetOrganizationDataParam(salesOrder.CustomerOrganizationID, salesOrder.SalesOrderID, salesOrder.CustomerPO, salesOrder.ShipLocationID, salesOrder.ARInvoiceLocationID);
						organizationInformationDto = GetCustomerOrganizationData(base.OrganizationRepository, parameter);
						if (organizationInformationDto != null && organizationInformationDto.ErrorsList.Count() > 0)
						{
							((List<string>)base.ErrorsList).AddRange(new List<string>(organizationInformationDto?.ErrorsList));
						}
						if (organizationInformationDto != null && organizationInformationDto.WarningsList.Count() > 0)
						{
							((List<string>)base.WarningsList).AddRange(new List<string>(organizationInformationDto?.WarningsList));
						}
						if (!string.IsNullOrWhiteSpace(organizationInformationDto.CustomerOrganizationID))
						{
							salesOrderDto.CustomerOrganizationID = organizationInformationDto.CustomerOrganizationID;
							salesOrderDto.ShipOrganizationID = organizationInformationDto.ShipOrganizationID;
							salesOrderDto.ARInvoiceLocationID = organizationInformationDto.ARInvoiceLocationID;
							salesOrderDto.ARInvoiceContactID = organizationInformationDto.ARInvoiceContactID;
							salesOrderDto.ShipLocationID = organizationInformationDto.ShipLocationID;
							salesOrderDto.ShipContactID = organizationInformationDto.ShipContactID;
							salesOrderDto.PaymentTermID = organizationInformationDto.PaymentTermsID;
							salesOrderDto.CurrencyRateID = organizationInformationDto.CurrencyRateID;
							salesOrderDto.SalesOrderSalesPeople.AddRange(new List<SalesOrderSalespeopleDto>(organizationInformationDto.ShipLocationSalesPeople.ToList()));
						}
					}
					salesOrderDto.OrderCommentsText = salesOrder.OrderCommentsText ?? string.Empty;
					if (!string.IsNullOrWhiteSpace(salesOrder.CustomerPO))
					{
						string result = base.SalesOrderRepository.GetSalesOrderList_ForCustomerPO(salesOrder.CustomerPO, salesOrder.CustomerOrganizationID, null).Result;
						salesOrderDto.CustomerPO = salesOrder.CustomerPO;
						if (!string.IsNullOrWhiteSpace(result))
						{
							base.WarningsList.Add("Customer PO [" + salesOrderDto.CustomerPO + "] already has following sales order(s) : [" + result + "].");
						}
					}
					salesOrderDto.RequestedShipDate = salesOrder.RequestedShipDate ?? ((DateTime?)null);
					salesOrderDto.ExchangeRate = base.SalesOrderRepository.GetExchangeRate(salesOrderDto.CurrencyRateID, salesOrderDto.OrderDate, null).Result;
					salesOrderDto.Status = 3;
					salesOrderDto.CreatedBy = base.ApiClientContext.UserID;
					salesOrderDto.CreatedDate = DateTime.Now;
					salesOrderDto.CreatedByEDI = true;
					salesOrderDto.CreatedFromWeb = true;
					salesOrderDto.ShippingMethodID = salesOrder.ShippingMethodID;
					salesOrderDto.SalesOrderLines.Clear();
					foreach (EDI850SalesOrderLineIN item in salesOrder.EDI850SalesOrderLines.EDISalesOrderLineSet)
					{
						priceCalculation = new PriceCalculation();
						SalesOrderLineDto salesOrderLineDto = new SalesOrderLineDto();
						decimal? num = item.EDI850SalesOrderDeliveries.EDI850SalesOrderDeliverySet.Sum((EDI850SalesOrderDeliveryIN x) => x.DeliveryQuantity);
						if (!(num == item.OrderQuantity))
						{
							base.ErrorsList.Add($"OrderQuantity in sales order [{salesOrder.SalesOrderID}] line [{item.SalesOrderLineID}] is not equal to the total quantity in delivery lines [{num}].");
						}
						if ((from x in item.EDI850SalesOrderDeliveries.EDI850SalesOrderDeliverySet
							group x by x.SalesOrderDeliveryID).All((IGrouping<short?, EDI850SalesOrderDeliveryIN> g) => g.Count() > 1))
						{
							base.ErrorsList.Add($"Sales order [{salesOrder.SalesOrderID}] line [{item.SalesOrderLineID}] has duplicate SalesOrderDeliveryID(s).");
						}
						salesOrderLineDto.SalesOrderID = salesOrderDto.SalesOrderID;
						salesOrderLineDto.SalesOrderLineID = item.SalesOrderLineID.Value;
						salesOrderLineDto.PartID = item.OrgPartID;
						PartInformationDto result2 = GetPartInfo(base.PartRepository, salesOrder.SalesOrderID, item.SalesOrderLineID.Value, item.OrgPartID, item.OrgPartShortDescription, item.PartRevisionID, salesOrderDto.CustomerOrganizationID).Result;
						if (result2 != null && result2.ErrorsList.Count() > 0)
						{
							((List<string>)base.ErrorsList).AddRange(new List<string>(result2?.ErrorsList));
						}
						if (result2 != null && result2.WarningsList.Count() > 0)
						{
							((List<string>)base.WarningsList).AddRange(new List<string>(result2?.WarningsList));
						}
						if (string.IsNullOrWhiteSpace(result2.PartID))
						{
							continue;
						}
						TaxInformationDto result3 = GetTaxInformation(base.OrganizationRepository, result2, organizationInformationDto.ARInvoiceLocation, salesOrderDto.OrderDate).Result;
						salesOrderLineDto.PartID = result2.PartID;
						salesOrderLineDto.PartGroupID = result2.PartGroupID;
						salesOrderLineDto.PartShortDescription = result2.PartShortDescription;
						salesOrderLineDto.OrgPartID = result2.OrgPartID;
						salesOrderLineDto.OrgPartShortDescription = (string.IsNullOrWhiteSpace(item.OrgPartShortDescription) ? result2.OrgPartShortDescription : item.OrgPartShortDescription);
						salesOrderLineDto.PartShortDescription = result2.PartShortDescription;
						salesOrderLineDto.PartLongDescriptionText = result2.PartLongDescriptionText;
						salesOrderLineDto.PartRevisionID = result2.PartRevisionID;
						salesOrderLineDto.UnitOfMeasure = result2.UOM;
						salesOrderLineDto.Weight = result2.Weight;
						salesOrderLineDto.OrderQuantity = item.OrderQuantity.Value;
						salesOrderLineDto.TaxCodeID = result3.FirstTaxCodeID;
						salesOrderLineDto.SecondTaxCodeID = result3.SecondTaxCodeID;
						decimal? fullUnitPriceBase = item.FullUnitPriceBase;
						if (((fullUnitPriceBase.GetValueOrDefault() == default(decimal)) & fullUnitPriceBase.HasValue) && !string.IsNullOrWhiteSpace(salesOrderDto.CustomerOrganizationID))
						{
							priceCalculation = base.PartRepository.GetPartPrice(salesOrderLineDto.PartID, salesOrderLineDto.PartRevisionID, salesOrderLineDto.PartGroupID, salesOrderDto.CustomerOrganizationID, salesOrderDto.ARInvoiceLocationID, salesOrderLineDto.OrderQuantity, salesOrderDto.CurrencyRateID, salesOrderDto.CreatedDate).Result;
						}
						fullUnitPriceBase = item.FullUnitPriceBase;
						if (((fullUnitPriceBase.GetValueOrDefault() == default(decimal)) & fullUnitPriceBase.HasValue) && (priceCalculation == null || priceCalculation.FullPrice == 0m))
						{
							base.ErrorsList.Add($"FullUnitPriceBase in sales order [{salesOrder.SalesOrderID}] line [{item.SalesOrderLineID}] is 0 in both EDI file and M1.");
						}
						else
						{
							decimal num2 = default(decimal);
							fullUnitPriceBase = item.FullUnitPriceBase;
							if ((fullUnitPriceBase.GetValueOrDefault() > default(decimal)) & fullUnitPriceBase.HasValue)
							{
								num2 = item.FullUnitPriceBase.Value;
								item.FullUnitPriceBase = default(decimal);
								if (base.ApiClientContext.Database.CheckHomeCurrency(salesOrderDto.CurrencyRateID))
								{
									salesOrderLineDto.FullUnitPriceBase = num2;
									salesOrderLineDto.NonTaxReasonID = result2.PartNonTaxReasonID;
								}
								else
								{
									salesOrderLineDto.FullUnitPriceForeign = num2;
									salesOrderLineDto.NonTaxReasonID = result2.PartNonTaxReasonID;
								}
							}
							else
							{
								base.WarningsList.Add($"FullUnitPriceBase in sales order [{salesOrder.SalesOrderID}] line [{item.SalesOrderLineID}] is 0. M1 unit price was used.");
								if (priceCalculation != null && priceCalculation.Discount > 0m)
								{
									salesOrderLineDto.DiscountPercent = priceCalculation.Discount;
								}
								if (base.ApiClientContext.Database.CheckHomeCurrency(priceCalculation.CurrencyID))
								{
									salesOrderLineDto.FullUnitPriceBase = priceCalculation.FullPrice;
									salesOrderLineDto.NonTaxReasonID = result2.PartNonTaxReasonID;
									if (priceCalculation != null && priceCalculation.DiscountedPrice > 0m)
									{
										salesOrderLineDto.UnitPriceBase = priceCalculation.DiscountedPrice;
									}
								}
								else
								{
									salesOrderLineDto.FullUnitPriceForeign = priceCalculation.FullPrice;
									salesOrderLineDto.NonTaxReasonID = result2.PartNonTaxReasonID;
									if (priceCalculation != null && priceCalculation.DiscountedPrice > 0m)
									{
										salesOrderLineDto.UnitPriceForeign = priceCalculation.DiscountedPrice;
									}
								}
							}
						}
						salesOrderLineDto.CreatedBy = salesOrderDto.CreatedBy;
						salesOrderLineDto.CreatedDate = DateTime.Now;
						salesOrderLineDto.SalesOrderDeliveries.Clear();
						SalesOrderDeliveryDto salesOrderDeliveryDto = null;
						b = ((result2.DeliveryType != 0) ? result2.DeliveryType : base.SalesOrderRepository.GetDefaultSalesOrderDeliveryType().Result);
						if (item.EDI850SalesOrderDeliveries.EDI850SalesOrderDeliverySet.Count > 0)
						{
							foreach (EDI850SalesOrderDeliveryIN item2 in item.EDI850SalesOrderDeliveries.EDI850SalesOrderDeliverySet)
							{
								salesOrderDeliveryDto = new SalesOrderDeliveryDto();
								salesOrderDeliveryDto.SalesOrderID = salesOrderLineDto.SalesOrderID;
								salesOrderDeliveryDto.SalesOrderLineID = salesOrderLineDto.SalesOrderLineID;
								salesOrderDeliveryDto.SalesOrderDeliveryID = item2.SalesOrderDeliveryID.Value;
								salesOrderDeliveryDto.PartID = result2.PartID;
								salesOrderDeliveryDto.PartRevisionID = result2.PartRevisionID;
								salesOrderDeliveryDto.DeliveryQuantity = item2.DeliveryQuantity.Value;
								salesOrderDeliveryDto.DeliveryDate = item2.DeliveryDate.Value;
								salesOrderDeliveryDto.DeliveryType = b;
								salesOrderDeliveryDto.CustomerOrganizationID = salesOrderDto.CustomerOrganizationID;
								salesOrderDeliveryDto.PartWarehouseLocationID = result2.PartWarehouseLocationID;
								salesOrderDeliveryDto.PartBinID = result2.PartBinID;
								salesOrderDeliveryDto.Firm = true;
								salesOrderDeliveryDto.CreatedBy = salesOrderDto.CreatedBy;
								salesOrderDeliveryDto.CreatedDate = DateTime.Now;
								salesOrderLineDto.SalesOrderDeliveries.Add(salesOrderDeliveryDto);
							}
						}
						else
						{
							salesOrderDeliveryDto = new SalesOrderDeliveryDto();
							salesOrderDeliveryDto.SalesOrderID = salesOrderLineDto.SalesOrderID;
							salesOrderDeliveryDto.SalesOrderLineID = salesOrderLineDto.SalesOrderLineID;
							salesOrderDeliveryDto.SalesOrderDeliveryID = 1;
							salesOrderDeliveryDto.DeliveryQuantity = salesOrderLineDto.OrderQuantity;
							salesOrderDeliveryDto.DeliveryDate = salesOrderDto.RequestedShipDate.Value;
							salesOrderDeliveryDto.DeliveryType = b;
							salesOrderDeliveryDto.Firm = true;
							salesOrderDeliveryDto.CreatedBy = salesOrderDto.CreatedBy;
							salesOrderDeliveryDto.CreatedDate = DateTime.Now;
							salesOrderLineDto.SalesOrderDeliveries.Add(salesOrderDeliveryDto);
						}
						salesOrderDto.SalesOrderLines.Add(salesOrderLineDto);
					}
					cTMSalesOrderDto.M1SalesOrderValidatationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList);
					cTMSalesOrderDto.M1SalesOrder = salesOrderDto;
					cTMSalesOrderDto.EDIOrderID = salesOrder.SalesOrderID;
					cTMSalesOrderDto.EDIPurpose = WebAPIConstants.EDIPurposeCodes.Original;
					cTMSalesOrderDto.DoesRequestProcessed = false;
					if (cTMSalesOrderDto.M1SalesOrderValidatationInfo.IsValidationOk)
					{
						cTMSalesOrderDto.DoesRequestValidated = true;
					}
					postOrderResponseDto.M1OrderCollection.Add(cTMSalesOrderDto);
				}
			}
		}
		catch (Exception ex)
		{
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the customer PO [" + empty + "].");
			postOrderResponseDto.GeneralValidatationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, HttpStatusCode.InternalServerError);
		}
		finally
		{
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(postOrderResponseDto);
	}

	public Task<PostOrderResponseDto> Process_PostOrder(PostOrderResponseDto postOrderResponseIn)
	{
		PostOrderResponseDto postOrderResponseDto = new PostOrderResponseDto();
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		string empty = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		SqlTransaction sqlTransaction = null;
		try
		{
			IList<CTMSalesOrderDto> m1OrderCollection = postOrderResponseIn.M1OrderCollection;
			if (m1OrderCollection.Count() > 0)
			{
				sqlTransaction = base.ApiClientContext.Database.BeginTransaction();
				foreach (CTMSalesOrderDto item in m1OrderCollection)
				{
					stringBuilder.Length = 0;
					if (!item.M1SalesOrderValidatationInfo.IsValidationOk)
					{
						continue;
					}
					stringBuilder.Append(item.M1SalesOrder.OrderCommentsText.Trim());
					List<string> warningsList = item.M1SalesOrderValidatationInfo.WarningsList;
					if (warningsList != null && warningsList.Count > 0)
					{
						string value = string.Join("\n", item.M1SalesOrderValidatationInfo.WarningsList);
						stringBuilder.AppendLine();
						stringBuilder.Append("EDI Order Creation Warnings..");
						stringBuilder.AppendLine();
						stringBuilder.Append(value);
					}
					item.M1SalesOrder.OrderCommentsText = stringBuilder.ToString().Trim();
					item.M1SalesOrder.OrderCommentsRTF = APICommonFunctions.ConvertStringToRTF(stringBuilder.ToString().Trim());
					if (item.EDIPurpose.Equals(WebAPIConstants.EDIPurposeCodes.Original, StringComparison.CurrentCultureIgnoreCase))
					{
						SaveResponseDto result = base.SalesOrderRepository.SaveSalesOrder(item.M1SalesOrder, sqlTransaction).Result;
						item.DoesRequestProcessed = true;
						if (!result.IsSuccess)
						{
							item.DoesOrderCreated = false;
							item.M1SalesOrderValidatationInfo.ErrorsList.AddRange(new List<string>(result.SavingErrors));
						}
						else
						{
							item.DoesOrderCreated = true;
							item.M1SalesOrder.SalesOrderID = result.SalesOrder;
						}
					}
					else if (item.EDIPurpose.Equals(WebAPIConstants.EDIPurposeCodes.Replace, StringComparison.CurrentCultureIgnoreCase))
					{
						base.ErrorsList.Add("Replace orders are not supported.");
					}
				}
				postOrderResponseDto = postOrderResponseIn;
				postOrderResponseDto.M1OrderCollection = new List<CTMSalesOrderDto>(m1OrderCollection);
				postOrderResponseDto.GeneralValidatationInfo.ErrorsList.AddRange(new List<string>(base.ErrorsList));
				if (postOrderResponseDto.IsValidationOk)
				{
					sqlTransaction.Commit();
				}
				else
				{
					sqlTransaction.Rollback();
				}
			}
			else
			{
				base.ErrorsList.Add("Error occurred.No orders in order collection.");
			}
		}
		catch (Exception ex)
		{
			sqlTransaction.Rollback();
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the customer PO [" + empty + "].");
			postOrderResponseDto.GeneralValidatationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, HttpStatusCode.InternalServerError);
		}
		finally
		{
			sqlTransaction.Dispose();
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(postOrderResponseDto);
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
			GC.SuppressFinalize(this);
			base.SalesOrderRepository.Dispose();
			base.OrganizationRepository.Dispose();
			base.PartRepository.Dispose();
		}
	}
}
