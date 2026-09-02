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

public class EDIPlanningScheduleModel : EDIBaseModel, IEDIPlanningScheduleModel, IEDIBaseModel, IAPIBaseModel, IDisposable
{
	private string buildOrderHeaderComments(IList<EDI830ScheduleNoteIN> orderNotes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (EDI830ScheduleNoteIN orderNote in orderNotes)
		{
			stringBuilder.Append(orderNote.NoteType?.Trim());
			stringBuilder.Append(":");
			stringBuilder.Append(orderNote.NoteText?.Trim());
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	public EDIPlanningScheduleModel(APIClientContext clientContext)
		: base(clientContext)
	{
		base.SalesOrderRepository = new SalesOrderRepository(clientContext);
		base.OrganizationRepository = new OrganizationRepository(clientContext);
		base.PartRepository = new PartRepository(clientContext);
		base.ShipmentRepository = new ShipmentRepository(clientContext);
	}

	public Task<PostOrderResponseDto> ValidateRequest_PostSchedule(List<EDI830ScheduleIN> salesOrders)
	{
		PostOrderResponseDto postOrderResponseDto = null;
		CTMSalesOrderDto cTMSalesOrderDto = null;
		SalesOrderDto salesOrderDto = null;
		OrganizationInformationDto organizationInformationDto = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		string empty = string.Empty;
		byte b = 0;
		int num = 0;
		PriceCalculation priceCalculation = new PriceCalculation();
		try
		{
			postOrderResponseDto = new PostOrderResponseDto();
			if (salesOrders != null && salesOrders.Count == 0)
			{
				base.ErrorsList.Add("No records found in the request or invalid format.");
				postOrderResponseDto.GeneralValidatationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, HttpStatusCode.InternalServerError);
			}
			else
			{
				foreach (EDI830ScheduleIN salesOrder in salesOrders)
				{
					cTMSalesOrderDto = new CTMSalesOrderDto();
					organizationInformationDto = new OrganizationInformationDto();
					base.ErrorsList = new List<string>();
					base.WarningsList = new List<string>();
					num++;
					salesOrderDto = new SalesOrderDto();
					salesOrderDto.OrderDate = salesOrder.ForecastCreateDate.Value;
					salesOrderDto.SalesOrderID = num.ToString();
					if (!string.IsNullOrWhiteSpace(salesOrder.CustomerOrganizationID))
					{
						GetOrganizationDataParam parameter = new GetOrganizationDataParam(salesOrder.CustomerOrganizationID, salesOrder.ScheduleID, salesOrder.CustomerPO, salesOrder.ShipLocationID, salesOrder.ARInvoiceLocationID);
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
							salesOrderDto.ShippingPaymentTypeID = organizationInformationDto.ShippingPaymentTypeID;
						}
					}
					salesOrderDto.OrderCommentsText = buildOrderHeaderComments(salesOrder.EDI830ScheduleNotes.EDI830ScheduleNoteSet) ?? string.Empty;
					if (!string.IsNullOrWhiteSpace(salesOrder.CustomerPO))
					{
						string result = base.SalesOrderRepository.GetSalesOrderList_ForCustomerPO(salesOrder.CustomerPO, salesOrder.CustomerOrganizationID, null).Result;
						salesOrderDto.CustomerPO = salesOrder.CustomerPO;
						if (!string.IsNullOrWhiteSpace(result))
						{
							cTMSalesOrderDto.CurrentM1SalesorderIDs = result;
							base.WarningsList.Add("Customer PO [" + salesOrderDto.CustomerPO + "] already has following sales order(s) : [" + result + "].");
						}
					}
					if (base.OrganizationRepository.DoesPlantExists(salesOrder.PlantID).Result)
					{
						salesOrderDto.PlantID = salesOrder.PlantID;
					}
					else
					{
						base.WarningsList.Add("PlantID [" + salesOrder.PlantID + "] in sales order [" + salesOrder.ScheduleID + "]/customer PO [" + salesOrder.CustomerPO + "] is invalid.PlantID in sales order will not be updated.");
						salesOrderDto.PlantID = string.Empty;
					}
					if (base.ShipmentRepository.DoesShippingMethodExists(salesOrder.ShippingMethodID).Result)
					{
						salesOrderDto.ShippingMethodID = salesOrder.ShippingMethodID;
					}
					else
					{
						base.WarningsList.Add("ShippingMethodID [" + salesOrder.ShippingMethodID + "] in sales order [" + salesOrder.ScheduleID + "]/customer PO [" + salesOrder.CustomerPO + "] is invalid.ShippingMethodID in sales order will not be updated.");
						salesOrderDto.ShippingMethodID = string.Empty;
					}
					salesOrderDto.FreeOnBoardDescription = salesOrder.FOBDescription;
					salesOrderDto.ExchangeRate = base.SalesOrderRepository.GetExchangeRate(salesOrderDto.CurrencyRateID, salesOrderDto.OrderDate, null).Result;
					salesOrderDto.Status = 1;
					salesOrderDto.CreatedBy = base.ApiClientContext.UserID;
					salesOrderDto.CreatedDate = DateTime.Now;
					salesOrderDto.CreatedByEDI = true;
					salesOrderDto.CreatedFromWeb = true;
					salesOrderDto.SalesOrderLines.Clear();
					foreach (EDI830ScheduleLineIN item in salesOrder.EDI830ScheduleLines.EDI830ScheduleLineSet)
					{
						priceCalculation = new PriceCalculation();
						SalesOrderLineDto salesOrderLineDto = new SalesOrderLineDto();
						decimal? num2 = item.EDI830ForecastSchedules.EDI830ForecastScheduleSet.Sum((EDI830ForecastScheduleIN x) => x.ForecastQuantity);
						if ((num2.GetValueOrDefault() == default(decimal)) & num2.HasValue)
						{
							base.ErrorsList.Add($"ForecastQuantity in sales order [{salesOrder.ScheduleID}] line [{item.ScheduleLineID}] is zero.");
						}
						salesOrderLineDto.SalesOrderID = salesOrderDto.SalesOrderID;
						salesOrderLineDto.SalesOrderLineID = item.ScheduleLineID.Value;
						salesOrderLineDto.PartID = item.OrgPartID;
						salesOrderLineDto.ReleaseNumber = item.ReleaseNumber ?? string.Empty;
						PartInformationDto result2 = GetPartInfo(base.PartRepository, salesOrder.ScheduleID, item.ScheduleLineID.Value, item.OrgPartID, item.OrgPartShortDescription, item.PartRevisionID, salesOrderDto.CustomerOrganizationID).Result;
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
						salesOrderLineDto.TaxCodeID = result3.FirstTaxCodeID;
						salesOrderLineDto.SecondTaxCodeID = result3.SecondTaxCodeID;
						if (item.EDI830ForecastSchedules.EDI830ForecastScheduleSet.Count > 0)
						{
							salesOrderLineDto.SalesOrderDeliveries = new List<SalesOrderDeliveryDto>();
							b = ((result2.DeliveryType != 0) ? result2.DeliveryType : base.SalesOrderRepository.GetDefaultSalesOrderDeliveryType().Result);
							short num3 = 1;
							foreach (EDI830ForecastScheduleIN item2 in item.EDI830ForecastSchedules.EDI830ForecastScheduleSet)
							{
								if (item2.ForecastQualifier.Equals("C", StringComparison.CurrentCultureIgnoreCase))
								{
									SalesOrderDeliveryDto salesOrderDeliveryDto = new SalesOrderDeliveryDto();
									salesOrderDeliveryDto.SalesOrderID = salesOrderLineDto.SalesOrderID;
									salesOrderDeliveryDto.SalesOrderLineID = salesOrderLineDto.SalesOrderLineID;
									salesOrderDeliveryDto.SalesOrderDeliveryID = num3;
									salesOrderDeliveryDto.PartID = result2.PartID;
									salesOrderDeliveryDto.PartRevisionID = result2.PartRevisionID;
									salesOrderDeliveryDto.DeliveryQuantity = item2.ForecastQuantity.Value;
									salesOrderDeliveryDto.DeliveryDate = item2.ForecastDate.Value;
									salesOrderDeliveryDto.DeliveryType = b;
									salesOrderDeliveryDto.CustomerOrganizationID = salesOrderDto.CustomerOrganizationID;
									salesOrderDeliveryDto.PartWarehouseLocationID = result2.PartWarehouseLocationID;
									salesOrderDeliveryDto.PartBinID = result2.PartBinID;
									salesOrderDeliveryDto.Firm = item2.ForecastQualifier.Equals("C", StringComparison.CurrentCultureIgnoreCase);
									salesOrderDeliveryDto.CreatedBy = salesOrderDto.CreatedBy;
									salesOrderDeliveryDto.CreatedDate = DateTime.Now;
									salesOrderLineDto.SalesOrderDeliveries.Add(salesOrderDeliveryDto);
									num3++;
								}
								else
								{
									DateTime? dateConvertedValue = APICommonFunctions.GetDateConvertedValue(item2.ForecastDate.Value.ToShortDateString() ?? DateTime.Parse("01/01/1901").ToShortDateString());
									base.WarningsList.Add($"Non-firm forecast on [{dateConvertedValue.Value.ToShortDateString()}] for Part [{item.OrgPartID}] will be ignored in Salesorder Line [{item.ScheduleLineID}] in Customer PO [{salesOrderDto.CustomerPO}].");
								}
							}
							decimal num4 = (salesOrderLineDto.OrderQuantity = salesOrderLineDto.SalesOrderDeliveries.Sum((SalesOrderDeliveryDto x) => x.DeliveryQuantity));
							if (num4 == 0m)
							{
								base.ErrorsList.Add($"Firm forecast quantity is zero in sales order [{salesOrder.ScheduleID}] line [{item.ScheduleLineID}].");
							}
							if (!string.IsNullOrWhiteSpace(salesOrderDto.CustomerOrganizationID))
							{
								priceCalculation = base.PartRepository.GetPartPrice(salesOrderLineDto.PartID, salesOrderLineDto.PartRevisionID, salesOrderLineDto.PartGroupID, salesOrderDto.CustomerOrganizationID, salesOrderDto.ARInvoiceLocationID, salesOrderLineDto.OrderQuantity, salesOrderDto.CurrencyRateID, salesOrderDto.CreatedDate).Result;
							}
						}
						else
						{
							base.ErrorsList.Add($"No Forecast Schedules in sales order [{salesOrder.ScheduleID}] line [{item.ScheduleLineID}].");
						}
						if (priceCalculation == null || priceCalculation.FullPrice == 0m)
						{
							base.ErrorsList.Add($"Unit Price of part [{item.OrgPartID}] in sales order [{salesOrder.ScheduleID}] line [{item.ScheduleLineID}] is 0 in M1.");
						}
						else
						{
							if (priceCalculation.Discount > 0m)
							{
								salesOrderLineDto.DiscountPercent = priceCalculation.Discount;
							}
							if (base.ApiClientContext.Database.CheckHomeCurrency(priceCalculation.CurrencyID))
							{
								salesOrderLineDto.FullUnitPriceBase = priceCalculation.FullPrice;
								salesOrderLineDto.NonTaxReasonID = result2.PartNonTaxReasonID;
								if (priceCalculation.DiscountedPrice > 0m)
								{
									salesOrderLineDto.UnitPriceBase = priceCalculation.DiscountedPrice;
								}
							}
							else
							{
								salesOrderLineDto.FullUnitPriceForeign = priceCalculation.FullPrice;
								salesOrderLineDto.NonTaxReasonID = result2.PartNonTaxReasonID;
								if (priceCalculation.DiscountedPrice > 0m)
								{
									salesOrderLineDto.UnitPriceForeign = priceCalculation.DiscountedPrice;
								}
							}
						}
						salesOrderLineDto.CreatedBy = salesOrderDto.CreatedBy;
						salesOrderLineDto.CreatedDate = DateTime.Now;
						salesOrderDto.SalesOrderLines.Add(salesOrderLineDto);
					}
					cTMSalesOrderDto.M1SalesOrderValidatationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList);
					if (cTMSalesOrderDto.M1SalesOrderValidatationInfo.IsValidationOk && salesOrderDto.SalesOrderLines.Sum((SalesOrderLineDto x) => x.SalesOrderDeliveries.Count()) > 0)
					{
						salesOrderDto.RequestedShipDate = salesOrderDto.SalesOrderLines.Select((SalesOrderLineDto x) => x.SalesOrderDeliveries.Select((SalesOrderDeliveryDto y) => y.DeliveryDate).Min()).Min();
					}
					cTMSalesOrderDto.M1SalesOrder = salesOrderDto;
					cTMSalesOrderDto.EDIOrderID = salesOrder.ScheduleID;
					cTMSalesOrderDto.EDIPurpose = salesOrder.Purpose;
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

	public Task<PostOrderResponseDto> Process_PostSchedule(PostOrderResponseDto postOrderResponseIn)
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
					stringBuilder.Append((item.M1SalesOrder.OrderCommentsText ?? string.Empty).Trim());
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
							item.DoesRequestValidated = false;
							item.DoesOrderCreated = true;
							item.M1SalesOrder.SalesOrderID = result.SalesOrder;
						}
					}
					else if (item.EDIPurpose.Equals(WebAPIConstants.EDIPurposeCodes.Replace, StringComparison.CurrentCultureIgnoreCase))
					{
						string result2 = base.SalesOrderRepository.CreateEDISalesOrderLog(item, sqlTransaction).Result;
						if (!string.IsNullOrWhiteSpace(result2))
						{
							base.ErrorsList.Add(result2);
							continue;
						}
						base.SalesOrderRepository.CreateSalesOrderMemo(item, sqlTransaction);
						item.DoesRequestValidated = false;
						item.DoesRequestProcessed = true;
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
		base.Dispose(disposing: true);
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
			base.ShipmentRepository.Dispose();
		}
	}
}
