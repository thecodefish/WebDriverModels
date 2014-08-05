using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels.Tests.Models
{
	[ModelLocator("container")]
	public class SimilarElementsModel
	{
		[ModelLocator(Method = How.ClassName, Identifier = "label", FindFirstVisibleElement = true)]
		public virtual string VisibleLabelText { get; set; }

		[ModelLocator(Method = How.ClassName, Identifier = "label", FindFirstVisibleElement = false)]
		public virtual string FirstLabelText { get; set; }
	}
}