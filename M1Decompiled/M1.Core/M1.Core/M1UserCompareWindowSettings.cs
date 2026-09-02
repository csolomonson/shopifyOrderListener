namespace M1.Core;

public class M1UserCompareWindowSettings
{
	public bool ShowLineNumbers;

	public bool ShowLineDetails = true;

	public bool ShowThumbnailView;

	public bool ShowWhitespace;

	public DifferencesFilter CurrentFilter;

	public void LoadDefaults()
	{
		ShowLineNumbers = false;
		ShowLineDetails = true;
		ShowThumbnailView = false;
		ShowWhitespace = false;
		CurrentFilter = DifferencesFilter.ShowAll;
	}
}
