namespace M1.Core;

public static class LanguageChooser
{
	public static string ChooseLanguage(M1DataDictionary DataDictionary, string sentence)
	{
		if (DataDictionary != null)
		{
			sentence = DataDictionary.Language.GetLocalString(sentence);
		}
		return sentence;
	}
}
