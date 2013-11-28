
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Castle.DynamicProxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebDriverModels
{
	public class ModelFinder
	{
		private static readonly ProxyGenerator Generator = new ProxyGenerator();

		public static T FindModel<T>(IWebDriver driver) where T : class
		{
			//get model attribute on class
			var modelAttribute =
				typeof(T).GetCustomAttributes(typeof (ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			IWebElement container;

			if (modelAttribute != null)
			{
				container = driver.FindElement(modelAttribute.Locator);
			}
			else
			{
				//no model attribute on class?
			}

			var modelInterceptor = new ModelInterceptor();
			var proxy = Generator.CreateClassProxy<T>(modelInterceptor);

			return proxy;
		}

		public static T WaitForModel<T>(IWebDriver driver, TimeSpan timeout) where T : class
		{
			var wait = new WebDriverWait(driver, timeout);

			return wait.Until(FindModel<T>);
		}

		public static bool ModelExists<T>(IWebDriver driver) where T : class
		{
			try
			{
				return FindModel<T>(driver) != null;
			}
			catch
			{
				return false;
			}
		}

		public static bool ModelExists<T>(IWebDriver driver, TimeSpan timeout) where T : class
		{
			var wait = new WebDriverWait(driver, timeout);

			try
			{
				return wait.Until(ModelExists<T>);
			}
			catch
			{
				return false;
			}
		}

		//void func()
		public static bool ModelPropertyExists<T>(IWebDriver driver, Expression<Action<T>> func)
		{
			MethodCallExpression method = func.Body as MethodCallExpression;

			if (method == null)
			{
				return false;
			}

			ModelLocatorAttribute modelAttribute = method.Method.GetCustomAttributes(typeof(ModelLocatorAttribute), false)
				.FirstOrDefault()
				as ModelLocatorAttribute;

			if (modelAttribute != null)
			{
				try
				{
					return driver.FindElement(modelAttribute.Locator) != null;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}

		//property
		public static bool ModelPropertyExists<T>(IWebDriver driver, Expression<Func<T, object>> func)
		{
			MemberExpression memberExpression = func.Body as MemberExpression;

			if (memberExpression == null)
			{
				return false;
			}

			ModelLocatorAttribute modelAttribute = memberExpression.Member.GetCustomAttributes(typeof(ModelLocatorAttribute), false)
				.FirstOrDefault()
				as ModelLocatorAttribute;

			if (modelAttribute != null)
			{
				try
				{
					return driver.FindElement(modelAttribute.Locator) != null;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}
	}
}
