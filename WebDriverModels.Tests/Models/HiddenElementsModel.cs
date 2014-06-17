namespace WebDriverModels.Tests.Models
{
	[ModelLocator("container")]
	public class HiddenElementsModel
	{
		[ModelLocator("itemOne")]
		public virtual string ItemOne { get; set; }

		[ModelLocator("iteTwo")]
		public virtual string ItemTwo { get; set; }

		[ModelLocator("itemThree")]
		public virtual string ItemThree { get; set; }

		[ModelLocator("itemFour")]
		public virtual string ItemFour { get; set; }
	}
}