using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class ModelPropertyIsVisibleSpecs
	{
		[Specification]
		public void ShouldReturnTrueIfAPropertyIsVisible()
		{
			IWebDriver driver = null;
			bool propertyIsVisible = false;
			var exception = default(Exception);

			"Given a browser pointed at the hidden elements test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "HiddenElements.html");

					return driver;
				});

			"When a visible element is checked"
				.Do(() => exception = Record.Exception(() => propertyIsVisible = driver.ModelPropertyIsVisible<HiddenElementsModel>(m => m.ItemFour)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The result should be true"
				.Assert(() => Assert.True(propertyIsVisible));
		}

		[Specification]
		public void ShouldReturnFalseIfAnElementHasDisplayNone()
		{
			IWebDriver driver = null;
			bool propertyIsVisible = false;
			var exception = default(Exception);

			"Given a browser pointed at the hidden elements test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "HiddenElements.html");

					return driver;
				});

			"When an element with display:none is checked"
				.Do(() => exception = Record.Exception(() => propertyIsVisible = driver.ModelPropertyIsVisible<HiddenElementsModel>(m => m.ItemOne)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The result should be false"
				.Assert(() => Assert.False(propertyIsVisible));
		}

		[Specification]
		public void ShouldReturnFalseIfAnElementHasAParentWithDisplayNone()
		{
			IWebDriver driver = null;
			bool propertyIsVisible = false;
			var exception = default(Exception);

			"Given a browser pointed at the hidden elements test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "HiddenElements.html");

					return driver;
				});

			"When an element with a parent with display:none is checked"
				.Do(() => exception = Record.Exception(() => propertyIsVisible = driver.ModelPropertyIsVisible<HiddenElementsModel>(m => m.ItemTwo)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The result should be false"
				.Assert(() => Assert.False(propertyIsVisible));
		}

		[Specification]
		public void ShouldReturnFalseIfAnElementHasVisibilityHidden()
		{
			IWebDriver driver = null;
			bool propertyIsVisible = false;
			var exception = default(Exception);

			"Given a browser pointed at the hidden elements test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "HiddenElements.html");

					return driver;
				});

			"When an element with visibility:hidden is checked"
				.Do(() => exception = Record.Exception(() => propertyIsVisible = driver.ModelPropertyIsVisible<HiddenElementsModel>(m => m.ItemThree)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The result should be false"
				.Assert(() => Assert.False(propertyIsVisible));
		}
	}
}