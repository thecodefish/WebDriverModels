
using System;
using System.Linq;
using Castle.DynamicProxy;
using OpenQA.Selenium;

namespace WebDriverModels
{
	public class ModelInterceptor : IInterceptor 
	{
		public void Intercept(IInvocation invocation)
		{
			ModelLocatorAttribute attribute;

			if (invocation.Method.Name.StartsWith("get_"))
			{
				string propertyName = invocation.Method.Name.Substring(4);
				var property = invocation.Method.DeclaringType.GetProperty(propertyName);
				attribute = property.GetCustomAttributes(typeof (ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

				if (attribute == null)
				{
					invocation.Proceed();
					return;
				}

				IWebDriver driver = CurrentDriver.Driver;

				IWebElement element = driver.FindElement(attribute.Locator);

				if (element.TagName == "input")
				{
					invocation.ReturnValue = element.GetAttribute("value");
					return;
				}
				else
				{
					invocation.ReturnValue = element.Text;
					return;
				}
			}
			
			if (invocation.Method.Name.StartsWith("set_"))
			{
				string propertyName = invocation.Method.Name.Substring(4);
				var property = invocation.Method.DeclaringType.GetProperty(propertyName);
				attribute = property.GetCustomAttributes(typeof(ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;

				if (attribute == null ||
					!(property.PropertyType.IsAssignableFrom(typeof(string)) ||
						property.PropertyType.IsAssignableFrom(typeof(bool))) ||
					!property.CanWrite)
				{
					invocation.Proceed();
					return;
				}

				IWebDriver driver = CurrentDriver.Driver;

				IWebElement element = driver.FindElement(attribute.Locator);

				UpdateInputEnabledElement(driver, element, invocation.GetArgumentValue(0));

				return;
			}

			//not a property method - check if there's an attribute on the method, and intercept it if so
			attribute =
				invocation.Method.GetCustomAttributes(typeof (ModelLocatorAttribute), true).FirstOrDefault() as
				ModelLocatorAttribute;

			if (attribute != null)
			{
				IWebDriver driver = CurrentDriver.Driver;

				IWebElement element = driver.FindElement(attribute.Locator);

				element.Click();

				return;
			}

			invocation.Proceed();
		}

		private void UpdateInputEnabledElement(IWebDriver driver, IWebElement element, object value)
		{
			var tagName = element.TagName;
			var type = element.GetAttribute("type");

			if (tagName != "input" && tagName != "textarea")
			{
				//can't handle input
				//todo: throw exception? value not writeable
				return;
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
