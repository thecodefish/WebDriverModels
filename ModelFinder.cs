
using Castle.DynamicProxy;
using OpenQA.Selenium;

namespace WebDriverModels
{
	public class ModelFinder
	{
		private static readonly ProxyGenerator Generator = new ProxyGenerator();

		public static T FindModel<T>(IWebDriver driver) where T : class
		{
			var modelInterceptor = new ModelInterceptor();
			var proxy = Generator.CreateClassProxy<T>(modelInterceptor);

			return proxy;
		}
	}
}
