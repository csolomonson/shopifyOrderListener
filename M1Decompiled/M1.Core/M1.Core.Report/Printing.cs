using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace M1.Core.Report;

public static class Printing
{
	internal class CachedPrinterProps
	{
		public int? PaperBin;

		public int? PaperSize;

		public bool? CanDuplex;

		public List<PaperSource> PaperSources;
	}

	internal static string CachedDefaultPrinter;

	private static Dictionary<string, CachedPrinterProps> CachedPrinterProperties;

	public static string DefaultPrinter
	{
		get
		{
			if (CachedDefaultPrinter == null)
			{
				PrinterSettings printerSettings = new PrinterSettings();
				try
				{
					CachedDefaultPrinter = printerSettings.PrinterName;
				}
				catch
				{
					CachedDefaultPrinter = string.Empty;
				}
				finally
				{
					printerSettings = null;
				}
			}
			return CachedDefaultPrinter;
		}
	}

	internal static CachedPrinterProps GetCache(string printerName)
	{
		if (CachedPrinterProperties == null)
		{
			CachedPrinterProperties = new Dictionary<string, CachedPrinterProps>(StringComparer.CurrentCultureIgnoreCase);
		}
		if (!CachedPrinterProperties.ContainsKey(printerName))
		{
			CachedPrinterProperties.Add(printerName, new CachedPrinterProps());
		}
		return CachedPrinterProperties[printerName];
	}

	public static M1PrinterSettings GetPrinterSettings(string printer)
	{
		M1PrinterSettings m1PrinterSettings = new M1PrinterSettings();
		if (printer.Length != 0)
		{
			m1PrinterSettings.PrinterName = printer;
		}
		return m1PrinterSettings;
	}

	public static bool IsPrinterInstalled(string printer)
	{
		foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
		{
			if (installedPrinter.Equals(printer, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public static List<string> GetInstalledPrinters()
	{
		List<string> list = new List<string>();
		if (PrinterSettings.InstalledPrinters.Count != 0)
		{
			for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
			{
				list.Add(PrinterSettings.InstalledPrinters[i]);
			}
		}
		return list;
	}
}
