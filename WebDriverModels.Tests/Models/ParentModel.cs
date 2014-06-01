
namespace WebDriverModels.Tests.Models
{
	[ModelLocator("container")]
	public class ParentModel
	{
		[ModelLocator("subModel")]
		public virtual SimpleSubModel SubModel { get; set; }
	}
}
