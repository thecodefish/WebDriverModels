
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
			if (invocation.Method.Name.StartsWith("get_"))
			{
				string propertyName = invocation.Method.Name.Substring(4);
				var property = invocation.Method.DeclaringType.GetProperty(propertyName);
				var attribute = property.GetCustomAttributes(typeof (ModelLocatorAttribute), true).FirstOrDefault() as ModelLocatorAttribute;
				//var attribute = Attribute.GetCustomAttribute(invocation.Method, typeof (ModelLocatorAttribute));

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

			invocation.Proceed();
		}
	}
}
