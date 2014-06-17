using System;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace WebDriverModels
{
	public static class WebDriverExtensions
	{
		public static T FindModel<T>(this IWebDriver driver) where T : class
		{
			return ModelFinder.FindModel<T>(driver);
		}

		public static T WaitForModel<T>(this IWebDriver driver, TimeSpan timeout) where T: class
		{
			return ModelFinder.WaitForModel<T>(driver, timeout);
		}

		public static bool ModelExists<T>(this IWebDriver driver) where T : class
		{
			return ModelFinder.ModelExists<T>(driver);
		}

		public static bool ModelExists<T>(this IWebDriver driver, TimeSpan timeout) where T : class
		{
			return ModelFinder.ModelExists<T>(driver, timeout);
		}

		public static bool ModelPropertyExists<T>(this IWebDriver driver, Expression<Action<T>> func)
		{
			return ModelFinder.ModelPropertyExists(driver, func);
		}

		public static bool ModelPropertyExists<T>(this IWebDriver driver, Expression<Func<T, object>> expression)
		{
			return ModelFinder.ModelPropertyExists(driver, expression);
		}

		public static bool ModelPropertyIsVisible<T>(this IWebDriver driver, Expression<Action<T>> func)
		{
			return ModelFinder.ModelPropertyIsVisible(driver, func);
		}

		public static bool ModelPropertyIsVisible<T>(this IWebDriver driver, Expression<Func<T, object>> expression)
		{
			return ModelFinder.ModelPropertyIsVisible(driver, expression);
		}
	}
}