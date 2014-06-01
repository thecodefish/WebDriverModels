
using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class CollectionSpecs
	{
		[Specification]
		public void LoadingAModelWithACollectionProperty()
		{
			IWebDriver driver = null;
			SimpleCollectionsModel model = null;
			var exception = default(Exception);

			"Given a browser pointed at the collections test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Collections.html");

					return driver;
				});

			"When the simpleCollections model is loaded"
				.Do(() => exception = Record.Exception(() => model = driver.FindModel<SimpleCollectionsModel>()));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The model should evaluate the collection property properly"
				.Assert(() => Assert.Contains("Item 2", model.Values));
		}

		[Specification]
		public void LoadingAModelWithACollectionOfSubModels()
		{
			IWebDriver driver = null;
			AdvancedCollectionsModel model = null;
			IEnumerable<KeyValueModel> subModels = null;
			var exception = default(Exception);

			"Given a browser pointed at the collections test page, with the advanced collections model loaded"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Collections.html");

					model = driver.FindModel<AdvancedCollectionsModel>();

					return driver;
				});

			"When the collection property is loaded"
				.Do(() => exception = Record.Exception(() => subModels = model.ChildModels));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The collection should not be null"
				.Assert(() => Assert.NotNull(subModels));

			"The collection should have the right number of items"
				.Assert(() => Assert.Equal(2, subModels.Count()));

			"The sub models should be able to have their properties evaluated"
				.Assert(() => Assert.Equal("Key 1", subModels.First().Key));
		}
	}
}
