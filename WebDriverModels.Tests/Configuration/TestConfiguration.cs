using System;
using System.IO;

namespace WebDriverModels.Tests.Configuration
{
	public static class TestConfiguration
	{
		public static string BaseUrl
		{
			get
			{
				return string.Format("file:///{0}/Html/", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..").Replace('\\', '/'));
			}
		}
	}
}