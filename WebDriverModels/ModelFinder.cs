using System;
using System.Collections.Generic;
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


		internal static IWebElement FindElement(IWebDriver driver, ModelLocatorAttribute locatorAttribute)
		{
			if (driver == null || locatorAttribute == null)
			{
				throw new ArgumentException("Arguments can not be null");
			}

			if (!locatorAttribute.FindFirstVisibleElement)
			{
				//let WebDriver find the first match
				return driver.FindElement(locatorAttribute.Locator);
			}
			else
			{
				//loop through all matches until we find one that is visible to the user
				var matches = driver.FindElements(locatorAttribute.Locator);

				if (matches.Count == 0)
				{
					throw new NoSuchElementException("No element could be found");
				}

				var element = matches.FirstOrDefault(match => match.Displayed);

				if (element == null)
				{
					throw new ElementNotVisibleException("The element may be in the html but is not visible");
				}

				return element;
			}
		}

		internal static IWebElement FindElement(IWebElement container, ModelLocatorAttribute locatorAttribute)
		{
			if (container == null || locatorAttribute == null)
			{
				throw new ArgumentException("No arguments can be null");
			}

			if (!locatorAttribute.FindFirstVisibleElement)
			{
				//let WebDriver find the first match
				return container.FindElement(locatorAttribute.Locator);
			}
			else
			{
				//loop through all matches until we find one that is visible to the user
				var matches = container.FindElements(locatorAttribute.Locator);

				if (matches.Count == 0)
				{
					throw new NoSuchElementException("No element could be found");
				}

				var element = matches.FirstOrDefault(match => match.Displayed);

				if (element == null)
				{
					throw new ElementNotVisibleException("The element may be in the html but is not visible");
				}

				return element;
			}
		}

		public static T FindModel<T>(IWebDriver driver) where T : class
		{
			//get model attribute on class
			var modelAttribute =
				typeof(T).GetCustomAttributes(typeof (ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			IWebElement container;

			if (modelAttribute != null)
			{
				//container = driver.FindElement(modelAttribute.Locator);
				container = FindElement(driver, modelAttribute);
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
				//container = driver.FindElement(modelAttribute.Locator);
				container = FindElement(driver, modelAttribute);
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
				//model = container.FindElement(modelAttribute.Locator);
				model = FindElement(container, modelAttribute);
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

		private static bool EvaluateMethodExpression(IWebDriver driver, MethodCallExpression expression)
		{
			MemberExpression memberExpression = expression.Object as MemberExpression;


			ModelLocatorAttribute modelAttribute = expression.Method.GetCustomAttributes(typeof (ModelLocatorAttribute), false)
				.FirstOrDefault()
				as ModelLocatorAttribute;

			if (modelAttribute != null)
			{
				try
				{
					var element = FindElement(driver, modelAttribute);
					return element != null;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}

		private static bool EvaluatePropertyExpression(IWebDriver driver, MemberExpression expression)
		{
			ModelLocatorAttribute modelAttribute = expression.Member.GetCustomAttributes(typeof(ModelLocatorAttribute), false)
				.FirstOrDefault()
				as ModelLocatorAttribute;

			if (modelAttribute != null)
			{
				try
				{
					var element = FindElement(driver, modelAttribute);
					return element != null;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}

		//void func()
		public static bool ModelPropertyExists<T>(IWebDriver driver, Expression<Action<T>> func)
		{
			MethodCallExpression method = func.Body as MethodCallExpression;

			if (method != null)
			{
				return EvaluateMethodExpression(driver, method);
			}

			return false;
		}

		public static bool ModelPropertyExists(IWebDriver driver, Expression<Action> expression)
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;

			if (methodCallExpression != null)
			{
				return EvaluateMethodExpression(driver, methodCallExpression);
			}

			MemberExpression propertyExpression = expression.Body as MemberExpression;

			if (propertyExpression != null)
			{
				return EvaluatePropertyExpression(driver, propertyExpression);
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

			return EvaluatePropertyExpression(driver, memberExpression);
		}

		//model instance (property or method)
		public static bool ModelPropertyExists<T>(IWebDriver driver, Expression<Func<T>> expression)
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;

			if (methodCallExpression != null)
			{
				return EvaluateMethodExpression(driver, methodCallExpression);
			}

			MemberExpression propertyExpression = expression.Body as MemberExpression;

			if (propertyExpression != null)
			{
				return EvaluatePropertyExpression(driver, propertyExpression);
			}

			return false;
		}

		private static bool EvaluateMethodIsVisible(IWebDriver driver, MethodCallExpression expression)
		{
			ModelLocatorAttribute modelAttribute = expression
				.Method
				.GetCustomAttributes(typeof (ModelLocatorAttribute), false)
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

		public static bool ModelPropertyIsVisible<T>(IWebDriver driver, Expression<Action<T>> func)
		{
			MethodCallExpression method = func.Body as MethodCallExpression;

			if (method != null)
			{
				return EvaluateMethodIsVisible(driver, method);
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

			return EvaluatePropertyIsVisible(driver, memberExpression);
		}

		private static bool EvaluatePropertyIsVisible(IWebDriver driver, MemberExpression expression)
		{
			ModelLocatorAttribute modelAttribute = expression.Member.GetCustomAttributes(typeof(ModelLocatorAttribute), false)
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

		//model instance (property or method)
		public static bool ModelPropertyIsVisible<T>(IWebDriver driver, Expression<Func<T>> expression)
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;

			if (methodCallExpression != null)
			{
				return EvaluateMethodIsVisible(driver, methodCallExpression);
			}

			MemberExpression propertyExpression = expression.Body as MemberExpression;

			if (propertyExpression != null)
			{
				return EvaluatePropertyIsVisible(driver, propertyExpression);
			}

			return false;
		}
	}
}
