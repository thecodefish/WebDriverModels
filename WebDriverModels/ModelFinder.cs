
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
				throw new NotSupportedException("Model must have a ModelLocator attribute on the class.");
			}

			var modelInterceptor = new ModelInterceptor(container, driver);
			var proxy = Generator.CreateClassProxy<T>(modelInterceptor);

			return proxy;
		}

		internal static object FindModel(Type type, IWebDriver driver)
		{
			//get model attribute on class
			var modelAttribute =
				type.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			IWebElement container;

			if (modelAttribute != null)
			{
				container = driver.FindElement(modelAttribute.Locator);
			}
			else
			{
				//no model attribute on class?
				throw new NotSupportedException("Model must have a ModelLocator attribute on the class.");
			}

			var modelInterceptor = new ModelInterceptor(container, driver);
			var proxy = Generator.CreateClassProxy(type, modelInterceptor);

			return proxy;
		}

		internal static object FindModel(Type type, IWebElement container, IWebDriver driver)
		{
			//get model attribute on class
			var modelAttribute =
				type.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			IWebElement model;

			if (modelAttribute != null)
			{
				model = container.FindElement(modelAttribute.Locator);
			}
			else
			{
				//no model attribute on class?
				throw new NotSupportedException("Model must have a ModelLocator attribute on the class.");
			}

			var modelInterceptor = new ModelInterceptor(model, driver);
			var proxy = Generator.CreateClassProxy(type, modelInterceptor);

			return proxy;
		}

		internal static IEnumerable<object> FindModels(Type type, IWebElement container, IWebDriver driver)
		{
			//get model attribute on class
			var modelAttribute =
				type.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			IReadOnlyCollection<IWebElement> elements;

			if (modelAttribute != null)
			{
				elements = container.FindElements(modelAttribute.Locator);
			}
			else
			{
				//no model attribute on class?
				throw new NotSupportedException("Model must have a ModelLocator attribute on the class.");
			}

			List<object> items = new List<object>();

			foreach (IWebElement element in elements)
			{
				var modelInterceptor = new ModelInterceptor(element, driver);
				var proxy = Generator.CreateClassProxy(type, modelInterceptor);
				items.Add(proxy);
			}
			return items;
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
			ModelLocatorAttribute modelAttribute = null;

			if (func.Body is MethodCallExpression)
			{
				MethodCallExpression method = func.Body as MethodCallExpression;

				modelAttribute = method.Method.GetCustomAttributes(typeof (ModelLocatorAttribute), false)
					.FirstOrDefault()
					as ModelLocatorAttribute;
			}

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
		public static bool ModelPropertyExists<T>(IWebDriver driver, Expression<Func<T, object>> expression)
		{
			MemberExpression memberExpression;

			if (expression.Body.NodeType == ExpressionType.Convert || expression.Body.NodeType == ExpressionType.ConvertChecked)
			{
				UnaryExpression unaryExpression = expression.Body as UnaryExpression;

				memberExpression = ((unaryExpression != null) ? unaryExpression.Operand : null) as MemberExpression;
			}
			else
			{
				memberExpression = expression.Body as MemberExpression;
			}

			if (memberExpression == null)
			{
				return false;
			}

			ModelLocatorAttribute modelAttribute = memberExpression.Member.GetCustomAttributes(typeof (ModelLocatorAttribute), false)
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

		public static bool ModelPropertyIsVisible<T>(IWebDriver driver, Expression<Action<T>> func)
		{
			ModelLocatorAttribute modelAttribute = null;

			if (func.Body is MethodCallExpression)
			{
				MethodCallExpression method = func.Body as MethodCallExpression;

				modelAttribute = method.Method.GetCustomAttributes(typeof(ModelLocatorAttribute), false)
					.FirstOrDefault()
					as ModelLocatorAttribute;
			}

			if (modelAttribute != null)
			{
				try
				{
					IWebElement element = driver.FindElement(modelAttribute.Locator);
					return element != null && element.Displayed;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}

		public static bool ModelPropertyIsVisible<T>(IWebDriver driver, Expression<Func<T, object>> expression)
		{
			MemberExpression memberExpression;

			if (expression.Body.NodeType == ExpressionType.Convert || expression.Body.NodeType == ExpressionType.ConvertChecked)
			{
				UnaryExpression unaryExpression = expression.Body as UnaryExpression;

				memberExpression = ((unaryExpression != null) ? unaryExpression.Operand : null) as MemberExpression;
			}
			else
			{
				memberExpression = expression.Body as MemberExpression;
			}

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
					IWebElement element = driver.FindElement(modelAttribute.Locator);
					return element != null && element.Displayed;
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
