using System.Diagnostics;
using System.Runtime.InteropServices;
using M1.ServiceCore.AxScript;

namespace M1.Ax.Erp;

[ComVisible(true)]
[DebuggerDisplay("Full = {FullPrice}, Discounted = {DiscountedPrice}, Currency = {CurrencyID}, Foreign = {IsForeignCurrency}, CalcType = {CalculationType}")]
public class PriceCalculation : IWebPriceCalculation
{
	public PriceData PartPrice;

	private string _CurrencyID = string.Empty;

	private decimal _FullPrice;

	private decimal _DiscountedPrice;

	private bool _IsForeignCurrency;

	private short _LeadTime;

	private decimal _ConversionFactor = 1m;

	private decimal _Discount;

	public PriceCalculationType CalculationType;

	public string CurrencyID
	{
		get
		{
			return _CurrencyID;
		}
		set
		{
			_CurrencyID = value;
		}
	}

	public decimal FullPrice
	{
		get
		{
			return _FullPrice;
		}
		set
		{
			_FullPrice = value;
		}
	}

	public decimal DiscountedPrice
	{
		get
		{
			return _DiscountedPrice;
		}
		set
		{
			_DiscountedPrice = value;
		}
	}

	public bool IsForeignCurrency
	{
		get
		{
			return _IsForeignCurrency;
		}
		set
		{
			_IsForeignCurrency = value;
		}
	}

	public short LeadTime
	{
		get
		{
			return _LeadTime;
		}
		set
		{
			_LeadTime = value;
		}
	}

	public decimal ConversionFactor
	{
		get
		{
			return _ConversionFactor;
		}
		set
		{
			_ConversionFactor = value;
		}
	}

	public decimal Discount
	{
		get
		{
			return _Discount;
		}
		set
		{
			_Discount = value;
		}
	}
}
