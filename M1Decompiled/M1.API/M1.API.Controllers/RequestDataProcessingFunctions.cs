using System;
using System.IO;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using M1.API.App_Start;

namespace M1.API.Controllers;

public static class RequestDataProcessingFunctions
{
	public static XDocument GetXmlDocumentFromRequest(HttpRequestMessage request, string folderName)
	{
		string aPILogFilePath = APIStartup.APILogFilePath;
		XmlDocument xmlDocument = new XmlDocument();
		new XDocument();
		xmlDocument.Load(request.Content.ReadAsStreamAsync().Result);
		return SaveXML(xmlDocument.DocumentElement.InnerXml, folderName, aPILogFilePath);
	}

	public static XDocument SaveXML(string data, string folderName, string folderPath)
	{
		XDocument xDocument = new XDocument();
		if (!string.IsNullOrEmpty(data.Trim()))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xDocument = XDocument.Parse(data.Trim());
			if (!Directory.Exists(Path.Combine(folderPath, folderName)))
			{
				Directory.CreateDirectory(Path.Combine(folderPath, folderName));
			}
			string text = Path.Combine(Path.Combine(folderPath, folderName).Trim(), "cXml_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xml");
			xDocument.Save(text);
			xmlDocument.Load(text);
		}
		return xDocument;
	}
}
