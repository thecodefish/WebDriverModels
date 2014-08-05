using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class SelectingVisibleElementsOnlySpecs
	{
		[Specification]
		public void SelectingVisibleOrFirstElements()
		{
			IWebDriver driver = null;
			SimilarElementsModel model = null;
			var exception = default(Exception);

			"Given a browser pointed at the similar elements test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "SimilarElements.html");

					return driver;
				});

			"When the similar elements model is loaded"
				.Do(() => exception = Record.Exception(() => model = driver.FindModel<SimilarElementsModel>()));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"Then the visible label text should be as expected"
				.Assert(() => Assert.Equal("Label Three", model.VisibleLabelText));

			"Then the first label text should be as expected"
				.Assert(() => Assert.Equal(string.Empty, model.FirstLabelText));
		}
	}
}