using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using OpenQA.Selenium;

namespace WebDriverModels
{
	public class ModelInterceptor : IInterceptor
	{
		private readonly IWebElement _container;
		private readonly IWebDriver _driver;

		public ModelInterceptor(IWebElement container, IWebDriver driver)
		{
			_container = container;
			_driver = driver;
		}

		private void HandlePropertyGet(IInvocation invocation)
		{
			string propertyName = invocation.Method.Name.Substring(4);
			var property = invocation.Method.DeclaringType.GetProperty(propertyName);
			ModelLocatorAttribute attribute = property.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			if (attribute == null)
			{
				invocation.Proceed();
				return;
			}

			Type propertyType = property.PropertyType;

			//is this property a model in its own right?
			var propertyModelAttribute =
				propertyType.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			if (propertyModelAttribute != null)
			{
				invocation.ReturnValue = ModelFinder.FindModel(propertyType, _container, _driver);
				return;
			}

			//is this property a string?
			//if (propertyType == typeof (string))
			//{
			//	element = _container.FindElement(attribute.Locator);

			//	invocation.ReturnValue = EvaluateElementValue(element);
			//	return;
			//}

			//is this property a collection?
			Type collectionType = typeof (IEnumerable<>);
			if (propertyType != typeof(string) &&
				(propertyType.IsGenericType && (collectionType.IsAssignableFrom(propertyType.GetGenericTypeDefinition())) ||
					propertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == collectionType)))
			{
				var elements = _container.FindElements(attribute.Locator);

				//is this a collection of objects?
				Type itemType = propertyType.GetGenericArguments().First();

				if (itemType != typeof (string))
				{
					//is this property a model in its own right?
					var itemModelAttribute =
						itemType.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

					if (itemModelAttribute != null)
					{
						//get model attribute on class
						var modelAttribute =
							itemType.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

						IReadOnlyCollection<IWebElement> itemElements;

						if (modelAttribute != null)
						{
							itemElements = _container.FindElements(modelAttribute.Locator);
						}
						else
						{
							//no model attribute on class?
							throw new NotSupportedException("Model must have a ModelLocator attribute on the class.");
						}

						var listType = typeof(List<>).MakeGenericType(itemType);
						var items = Activator.CreateInstance(listType) as IList;

						ProxyGenerator generator = new ProxyGenerator();
						foreach (IWebElement itemElement in itemElements)
						{
							var modelInterceptor = new ModelInterceptor(itemElement, _driver);
							var proxy = generator.CreateClassProxy(itemType, modelInterceptor);
							//items.Add(DynamicCast(proxy, itemType));
							items.Add(proxy);
							//items[i++] = proxy;
						}
						invocation.ReturnValue = items;

						return;
					}
					else
					{
						throw new NotSupportedException("Model Property collections can only by of type string or of other Models.");
					}
				}

				//collection of strings
				invocation.ReturnValue = elements
					.Select(EvaluateElementValue)
					.ToList();

				return;
			}

			//just a standard property
			//IWebElement element = _container.FindElement(attribute.Locator);
			IWebElement element = ModelFinder.FindElement(_container, attribute);

			//is this looking at an attribute on the element?
			if (!string.IsNullOrWhiteSpace(attribute.AttributeName))
			{
				invocation.ReturnValue = element.GetAttribute(attribute.AttributeName);
				return;
			}

			//grab the value of the element
			invocation.ReturnValue = EvaluateElementValue(element);
		}

		private dynamic DynamicCast(object entity, Type to)
		{
			var openCast = this.GetType().GetMethod("Cast", BindingFlags.Static | BindingFlags.NonPublic);
			var closeCast = openCast.MakeGenericMethod(to);
			return closeCast.Invoke(entity, new[] { entity });
		}
		private static T Cast<T>(object entity) where T : class
		{
			return entity as T;
		}

		private string EvaluateElementValue(IWebElement element)
		{
			if (element.TagName == "input")
			{
				return element.GetAttribute("value");
			}
			else
			{
				return element.Text;
			}
		}

		private void HandlePropertySet(IInvocation invocation)
		{
			string propertyName = invocation.Method.Name.Substring(4);
			var property = invocation.Method.DeclaringType.GetProperty(propertyName);
			ModelLocatorAttribute attribute = property.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

			if (attribute == null ||
				!(property.PropertyType.IsAssignableFrom(typeof(string)) ||
					property.PropertyType.IsAssignableFrom(typeof(bool))) ||
				!property.CanWrite)
			{
				invocation.Proceed();
				return;
			}

			if (!string.IsNullOrWhiteSpace(attribute.AttributeName))
			{
				//cannot set attributes
				throw new NotSupportedException("Unable to change attribute values");
			}

			//IWebElement element = _container.FindElement(attribute.Locator);
			IWebElement element = ModelFinder.FindElement(_container, attribute);

			UpdateInputEnabledElement(element, invocation.GetArgumentValue(0));
		}

		public void Intercept(IInvocation invocation)
		{
			if (invocation.Method.Name.StartsWith("get_"))
			{
				HandlePropertyGet(invocation);
			}
			
			else if (invocation.Method.Name.StartsWith("set_"))
			{
				HandlePropertySet(invocation);
			}
			else
			{
				//not a property method - check if there's an attribute on the method, and intercept it if so
				ModelLocatorAttribute attribute = invocation.Method.GetCustomAttributes(typeof (ModelLocatorAttribute), true).FirstOrDefault() as
					ModelLocatorAttribute;

				if (attribute != null)
				{
					//IWebElement element = _container.FindElement(attribute.Locator);
					IWebElement element = ModelFinder.FindElement(_container, attribute);

					//scroll the element into view to try and stop intermittent Chrome errors
					IJavaScriptExecutor javaScriptExecutor = _driver as IJavaScriptExecutor;
					if (javaScriptExecutor != null)
					{
						javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView(false)", element);
					}

					element.Click();

					return;
				}

				invocation.Proceed();
			}
		}

		private void UpdateInputEnabledElement(IWebElement element, object value)
		{
			var tagName = element.TagName;
			var type = element.GetAttribute("type");

			if (tagName != "input" && tagName != "textarea")
			{
				//can't handle input
				throw new NotSupportedException(string.Format("Unable to set values for elements of type <{0}>", tagName));
			}

			//special cases
			if (type == "checkbox")
			{
				if (value is bool)
				{
					if (element.Selected != (bool) value)
					{
						element.Click();
					}
					return;
				}
				else
				{
					//todo throw exception? wrong value type for checkbox
					return;
				}
			}

			//general case (just send keyboard input)
			element.Clear();
			element.SendKeys(value.ToString());
		}
	}
}
