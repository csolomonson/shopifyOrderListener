using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace M1.Core;

public static class HtmlFormat
{
	public const string Img = "img";

	public const string Doctype = "DOCTYPE";

	public const string Base64 = "base64";

	public const string ImageLogo = "ImageLogo";

	public const string mailError = "about:mail";

	public const string mailErrorHref = "href=\":mail";

	private static string DTD = "<!DOCTYPE html>";

	private static string body = "<body></body>";

	private static string header = "<head>{0}</head>";

	private static string html = "<html>{0}{1}</html>";

	private static string[] thirdPartyCSSLibraries = new string[1] { "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" };

	private static string Header
	{
		get
		{
			string text = "";
			string[] array = thirdPartyCSSLibraries;
			foreach (string arg in array)
			{
				text += $"<link rel=\"stylesheet\" href=\"{arg}\">";
			}
			return string.Format(header, text);
		}
	}

	private static string HTMLBody => string.Format(html, Header, body);

	public static string HTMLDocument => $"{DTD}{HTMLBody}";

	public static void AddElementToHTMLDocument(ref string body, string element)
	{
		if (!string.IsNullOrWhiteSpace(element))
		{
			string value = "</body>";
			int num = body.IndexOf(value);
			if (num > 0)
			{
				body = body.Insert(num, $"<br>{element}");
			}
			else
			{
				body = body + "<br>" + element + "<br>";
			}
		}
	}

	public static string GetImgHtmlWithCID(string imgNameId)
	{
		return $"cid:{imgNameId}";
	}

	public static string GetImgHtmlwithBase64(string logo, string imgNameId)
	{
		return $"data:image/png;base64,{logo}\" id=\"{imgNameId}";
	}

	public static string GetImageBase64Format(string message)
	{
		return message.Replace("data:image/png;base64,", "");
	}

	public static List<string> GetImageOrPath(string codeHtml)
	{
		List<string> list = (from Match x in Regex.Matches(codeHtml, "src=\"[^\"\\\\]*(?:\\\\.[^\"\\\\]*)*\"")
			select x.Value).ToList();
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			string codePreviewImage = GetCodePreviewImage(item);
			string codePostImage = GetCodePostImage(item, codePreviewImage.Length);
			list2.Add(item.Substring(codePreviewImage.Length, item.Length - codePostImage.Length - codePreviewImage.Length));
		}
		return list2;
	}

	public static string GetCodePreviewImage(string code)
	{
		int num = code.IndexOf("src=\"");
		return code.Substring(0, num + 5);
	}

	public static string GetCodePostImage(string code, int preview)
	{
		string text = code.Substring(preview);
		int startIndex = text.IndexOf("\"");
		return text.Substring(startIndex);
	}

	public static string ReplaceImageSource(string signatureData, string image64, string path, int numberImage)
	{
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(image64))
		{
			string imgHtmlwithBase = GetImgHtmlwithBase64(image64, "ImageLogo" + numberImage);
			return signatureData.Replace(path, imgHtmlwithBase);
		}
		return DeleteImgSignature(signatureData);
	}

	private static string DeleteImgSignature(string signatureData)
	{
		_ = string.Empty;
		return Regex.Replace(Regex.Replace(signatureData, "(<img\\/?[^>]+>)", "", RegexOptions.IgnoreCase), "(cid\\/?[^>]+)", "", RegexOptions.IgnoreCase);
	}

	public static void FixMail(ref string signatureData)
	{
		if (signatureData.Contains("about:mail"))
		{
			signatureData = signatureData.Replace("about:mail", "about:mail" + "\"");
		}
		if (signatureData.Contains("href=\":mail"))
		{
			signatureData = signatureData.Replace("about:mail", "about:mail" + "\"");
		}
	}
}
