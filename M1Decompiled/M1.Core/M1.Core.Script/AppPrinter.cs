using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using M1.Core.Report;
using M1.Script.Interfaces;

namespace M1.Core.Script;

public class AppPrinter : IPrinter
{
	[ComVisible(true)]
	public class ComPrinterSettings : PrinterSettings, IPrinterSettings
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

		public object GetPaperSources()
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
			if (cache.PaperSources.Count != 0)
			{
				object[,] array = new object[2, cache.PaperSources.Count];
				for (int i = 0; i < cache.PaperSources.Count; i++)
				{
					array[0, i] = cache.PaperSources[i].RawKind;
					array[1, i] = cache.PaperSources[i].SourceName;
				}
				return array;
			}
			return new object[2, 1]
			{
				{ 15 },
				{ "Automatically Select" }
			};
		}
	}

	[Guid("A4C46780-499F-101B-BB78-00AA00383CBB")]
	[ComVisible(true)]
	public interface IPrintersComCollection
	{
		int Count { get; }

		[IndexerName("_Default")]
		[DispId(0)]
		string this[object name] { get; }

		bool Contains(string value);

		int IndexOf(string value);

		[DispId(-4)]
		IEnumerator GetEnumerator();
	}

	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(IPrintersComCollection))]
	public class ComPrintersList : List<string>, IPrintersComCollection
	{
		public string this[object id] => base[Convert.ToInt32(id)];

		public new IEnumerator GetEnumerator()
		{
			return base.GetEnumerator();
		}

		public new int IndexOf(string value)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].Equals(value, StringComparison.CurrentCultureIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public new bool Contains(string value)
		{
			return IndexOf(value) != -1;
		}

		string IPrintersComCollection.get__Default(object name)
		{
			return this[name];
		}
	}

	public string DefaultPrinter
	{
		get
		{
			if (Printing.CachedDefaultPrinter == null)
			{
				PrinterSettings printerSettings = new PrinterSettings();
				try
				{
					Printing.CachedDefaultPrinter = printerSettings.PrinterName;
				}
				catch
				{
					Printing.CachedDefaultPrinter = string.Empty;
				}
				finally
				{
					printerSettings = null;
				}
			}
			return Printing.CachedDefaultPrinter;
		}
	}

	public IPrinterSettings GetPrinterSettings(string printer)
	{
		ComPrinterSettings comPrinterSettings = new ComPrinterSettings();
		if (printer.Length != 0)
		{
			comPrinterSettings.PrinterName = printer;
		}
		return comPrinterSettings;
	}

	public bool IsPrinterInstalled(string printer)
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

	public object GetInstalledPrinters()
	{
		ComPrintersList comPrintersList = new ComPrintersList();
		if (PrinterSettings.InstalledPrinters.Count != 0)
		{
			for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
			{
				comPrintersList.Add(PrinterSettings.InstalledPrinters[i]);
			}
		}
		return comPrintersList;
	}
}
