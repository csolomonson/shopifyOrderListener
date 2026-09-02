using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using M1.API.Attributes;
using M1.Extensions;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

namespace M1.API.App_Start;

public class APIBuilder
{
	public class MyDateTimeConvertor : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return DateTime.Parse(reader.Value.ToString());
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(((DateTime)value).ToString("dd/MM/yyyy"));
		}
	}

	public class ApiFormatters : MediaTypeFormatter
	{
		public override bool CanReadType(Type type)
		{
			return true;
		}

		public override bool CanWriteType(Type type)
		{
			return true;
		}

		public ApiFormatters()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
			base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
			base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
		}
	}

	private string GetWebContentsDirectory()
	{
		string text = Path.Combine(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).AddBackslash()).Parent.FullName, "WebAPIClients\\");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	private string GetApiWebContentsDirectory()
	{
		string text = Path.Combine(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).AddBackslash()).FullName, "Content\\");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	/// <summary>
	/// Build the main configuration of the service.
	/// </summary>
	/// <param name="appBuilder"> The appBuilder as IAppBuilder</param>
	public void Configuration(IAppBuilder appBuilder)
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		HttpConfiguration httpConfiguration = new HttpConfiguration();
		appBuilder.UseFileServer(new FileServerOptions
		{
			FileSystem = new PhysicalFileSystem(GetWebContentsDirectory()),
			EnableDirectoryBrowsing = true,
			EnableDefaultFiles = true
		});
		appBuilder.UseCors(CorsOptions.AllowAll);
		httpConfiguration.MapHttpAttributeRoutes();
		httpConfiguration.Routes.IgnoreRoute("Resource", "{resource}.axd/{*pathInfo}");
		httpConfiguration.Routes.MapHttpRoute("swagger/docs/index", "help", null, null, (HttpMessageHandler)(object)new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "api/help/index.html"));
		httpConfiguration.Formatters.Clear();
		JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		{
			DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
			DateFormatHandling = DateFormatHandling.IsoDateFormat,
			Culture = CultureInfo.InvariantCulture,
			Formatting = Formatting.Indented,
			ContractResolver = new CamelCasePropertyNamesContractResolver
			{
				IgnoreSerializableAttribute = true
			},
			PreserveReferencesHandling = PreserveReferencesHandling.All,
			NullValueHandling = NullValueHandling.Ignore,
			MissingMemberHandling = MissingMemberHandling.Ignore
		};
		jsonSerializerSettings.Converters.Clear();
		jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter
		{
			DateTimeFormat = "yyyy-MM-dd"
		});
		JsonMediaTypeFormatter jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
		jsonMediaTypeFormatter.SerializerSettings = jsonSerializerSettings;
		jsonMediaTypeFormatter.SupportedMediaTypes.Clear();
		jsonMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
		jsonMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
		XmlMediaTypeFormatter xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
		xmlMediaTypeFormatter.SupportedMediaTypes.Clear();
		xmlMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
		xmlMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
		xmlMediaTypeFormatter.UseXmlSerializer = true;
		xmlMediaTypeFormatter.Indent = true;
		httpConfiguration.Formatters.Add(xmlMediaTypeFormatter);
		httpConfiguration.Formatters.Add(jsonMediaTypeFormatter);
		httpConfiguration.Filters.Add(new ExceptionHandlingAttribute());
		SwaggerConfig.Register(httpConfiguration);
		appBuilder.UseStaticFiles(new StaticFileOptions
		{
			RequestPath = new PathString("/api/help"),
			FileSystem = new PhysicalFileSystem(GetApiWebContentsDirectory())
		});
		appBuilder.UseWebApi(httpConfiguration);
	}
}
