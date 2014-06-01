
using System.Collections.Generic;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels.Tests.Models
{
	[ModelLocator(Method = How.ClassName, Identifier = "listOne")]
	public class SimpleCollectionsModel
	{
		[ModelLocator(Method = How.ClassName, Identifier = "item")]
		public virtual List<string> Values { get; set; } 
	}
}
