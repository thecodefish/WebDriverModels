

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels.Tests.Models
{
	[ModelLocator("container")]
	public class AdvancedModel
	{
		[ModelLocator("itemOne")]
		public virtual string ItemOne { get; set; }

		[ModelLocator(Method = How.Id, Identifier = "itemTwo")]
		public virtual string ItemTwo { get; set; }
	}
}
