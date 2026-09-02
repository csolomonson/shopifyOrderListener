using System.Collections.Generic;
using System.Drawing.Printing;

namespace M1.Core.Report;

public class M1PrinterSettings : PrinterSettings
{
	public int PaperBin
	{
		get
		{
			Printing.CachedPrinterProps cache = Printing.GetCache(base.PrinterName);
			if (!cache.PaperBin.HasValue)
			{
				cache.PaperBin = base.DefaultPageSettings.PaperSource.RawKind;
			}
			return cache.PaperBin.Value;
		}
	}

	public new bool CanDuplex
	{
		get
		{
			Printing.CachedPrinterProps cache = Printing.GetCache(base.PrinterName);
			if (!cache.CanDuplex.HasValue)
			{
				cache.CanDuplex = base.CanDuplex;
			}
			return cache.CanDuplex.Value;
		}
	}

	public int PaperSize
	{
		get
		{
			Printing.CachedPrinterProps cache = Printing.GetCache(base.PrinterName);
			if (!cache.PaperSize.HasValue)
			{
				cache.PaperSize = base.DefaultPageSettings.PaperSize.RawKind;
			}
			return cache.PaperSize.Value;
		}
	}

	public Dictionary<int, string> GetPaperSources()
	{
		Printing.CachedPrinterProps cache = Printing.GetCache(base.PrinterName);
		if (cache.PaperSources == null)
		{
			cache.PaperSources = new List<PaperSource>();
			foreach (PaperSource paperSource in base.PaperSources)
			{
				if (paperSource.RawKind < 512)
				{
					cache.PaperSources.Add(paperSource);
				}
			}
		}
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		if (cache.PaperSources.Count != 0)
		{
			for (int i = 0; i < cache.PaperSources.Count; i++)
			{
				dictionary.Add(cache.PaperSources[i].RawKind, cache.PaperSources[i].SourceName);
			}
		}
		else
		{
			dictionary.Add(15, "Automatically Select");
		}
		return dictionary;
	}
}
