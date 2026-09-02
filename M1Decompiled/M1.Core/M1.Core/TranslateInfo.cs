namespace M1.Core;

public class TranslateInfo
{
	public string SourceText = string.Empty;

	public string DestinationText = string.Empty;

	public bool IgnoreCase;

	public bool MatchWholeWord;

	public TranslateInfo(string sourceText, string destinationText, bool ignoreCase)
	{
		SourceText = sourceText;
		DestinationText = destinationText;
		IgnoreCase = ignoreCase;
	}

	public TranslateInfo(string sourceText, string destinationText, bool ignoreCase, bool matchWholeWord)
	{
		SourceText = sourceText;
		DestinationText = destinationText;
		IgnoreCase = ignoreCase;
		MatchWholeWord = matchWholeWord;
	}
}
