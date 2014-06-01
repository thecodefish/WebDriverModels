
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels.Tests.Models
{
	[ModelLocator("subModel")]
	public class SimpleSubModel
	{
		[ModelLocator(Method = How.ClassName, Identifier = "itemTwo")]
		public virtual string ItemTwo { get; set; }

		[ModelLocator(Method = How.ClassName, Identifier = "itemThree")]
		public virtual string ItemThree { get; set; }
	}
}
