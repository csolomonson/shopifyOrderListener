using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
[DebuggerDisplay("ID = {ID}, Currency = {CurrencyID}, MatchType = {MatchType}")]
public class PriceData
{
	public int ID;

	public string CurrencyID;

	public bool InventoryPrice;

	public PartPriceMatchType MatchType;

	public List<PriceLineData> Lines;

	public PriceData(int id, string currencyID, bool inventoryPrice, PartPriceMatchType matchType)
	{
		ID = id;
		CurrencyID = currencyID;
		InventoryPrice = inventoryPrice;
		MatchType = matchType;
		Lines = new List<PriceLineData>();
	}

	public PriceLineData GetLineForQuantity(decimal quantity)
	{
		PriceLineData priceLineData = null;
		foreach (PriceLineData line in Lines)
		{
			if (line.Quantity <= quantity && (priceLineData == null || line.Quantity > priceLineData.Quantity))
			{
				priceLineData = line;
			}
		}
		return priceLineData;
	}
}
