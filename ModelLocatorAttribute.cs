using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels
{
	public class ModelLocatorAttribute : Attribute
	{
		public How Method { get; set; }

		public string Identifier { get; set; }

		public By Locator { get
		{
			switch (Method)
			{
				case How.ClassName:
					return By.ClassName(Identifier);
				case How.CssSelector:
					return By.CssSelector(Identifier);
				case How.Id:
					return By.Id(Identifier);
				case How.LinkText:
					return By.LinkText(Identifier);
				case How.Name:
					return By.Name(Identifier);
				case How.PartialLinkText:
					return By.PartialLinkText(Identifier);
				case How.TagName:
					return By.TagName(Identifier);
				case How.XPath:
					return By.XPath(Identifier);
				default:
					throw new NotImplementedException();
			}
		}}

		public ModelLocatorAttribute()
		{
			
		}

		public ModelLocatorAttribute(string idLocator)
		{
			Method = How.Id;
			Identifier = idLocator;
		}
	}
}
