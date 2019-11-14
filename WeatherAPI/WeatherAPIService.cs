using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Xml;

namespace WeatherAPI
{
	class WeatherAPIService
	{
		private const string APIKEY = "8d9822b63fc95ec65a071696165ac14f";
		private string currentURL;
		public XmlDocument xmlDocument;

		public WeatherAPIService(string city)
		{
			SetCurrentURL(city);
			xmlDocument = GetXML(currentURL);
		}

		public List<TimeTemperature> GetTemp()
		{
			CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";

			List<TimeTemperature> timeTemperatures = new List<TimeTemperature>();

			XmlNodeList timeNodes = xmlDocument.SelectNodes("//time");

			foreach (XmlNode timeNode in timeNodes)
			{
				XmlAttribute fromAttribute = timeNode.Attributes["from"];
				XmlAttribute toAttribute = timeNode.Attributes["to"];

				XmlNode temperatureNode = timeNode.SelectSingleNode("temperature");

				XmlAttribute temperatureValueAttribute = temperatureNode.Attributes["value"];

				TimeTemperature timeTemperature = new TimeTemperature
				{
					From = DateTime.Parse(fromAttribute.Value).ToString("g"),
					To = DateTime.Parse(toAttribute.Value).ToString("g"),
					Value = float.Parse(temperatureValueAttribute.Value, NumberStyles.Any, cultureInfo)
				};

				timeTemperatures.Add(timeTemperature);
			}
			return timeTemperatures;
		}

		private void SetCurrentURL(string location)
		{
			currentURL = "http://api.openweathermap.org/data/2.5/forecast?q="
				+ location + "&mode=xml&units=metric&appid=" + APIKEY;
		}

		private XmlDocument GetXML(string CurrentURL)
		{
			using (WebClient client = new WebClient())
			{
				try
				{
					string xmlContent = client.DownloadString(CurrentURL);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xmlContent);
					return xmlDocument;
				}
				catch
				{
					return null;
				}
			}
		}
	}
}