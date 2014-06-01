
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class SubModelSpecs
	{
		[Specification]
		public void LoadingASubModel()
		{
			IWebDriver driver = null;
			ParentModel model = null;
			SimpleSubModel subModel = null;
			var exception = default(Exception);

			"Given a browser pointed at the submodel test page, with the parent model loaded"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "SubModels.html");

					model = driver.FindModel<ParentModel>();

					return driver;
				});

			"When the submodel property is loaded"
				.Do(() => exception = Record.Exception(() => subModel = model.SubModel));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The sub model should not be null"
				.Assert(() => Assert.NotNull(subModel));

			"Properties of the sub model should be able to be evaluated"
				.Assert(() => Assert.Equal("Item #2 Text", subModel.ItemTwo));
		}
	}
}
