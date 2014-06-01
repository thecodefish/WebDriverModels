
using System.Collections.Generic;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels.Tests.Models
{
	[ModelLocator(Method = How.ClassName, Identifier = "listTwo")]
	public class AdvancedCollectionsModel
	{
		[ModelLocator(Method = How.ClassName, Identifier = "item")]
		public virtual IEnumerable<KeyValueModel> ChildModels { get; set; }
	}
}
