
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels
{
	[ModelLocator(Method = How.ClassName, Identifier = "item")]
	public class KeyValueModel : BaseModel
	{
		[ModelLocator(Method = How.ClassName, Identifier = "key")]
		public virtual string Key { get; set; }

		[ModelLocator(Method = How.ClassName, Identifier = "value")]
		public virtual string Value { get; set; }
	}
}
