using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class ModelPropertySpecs
	{
		[Specification]
		public void CheckingTheExistenceOfAModelProperty()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);

			"Given a browser pointed at the input test page"
				.ContextFixture(() =>
				{
					PhantomJSOptions options = new PhantomJSOptions();
					options.AddAdditionalCapability("takesScreenshot", false);
					driver = CurrentDriver.Driver = new PhantomJSDriver(options);
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					return driver;
				});

			"When testing for the existence of a model property that actually exists"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists<InputModel>(m => m.Text)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should be found"
				.Assert(() => Assert.True(propertyExists));
		}

		[Specification]
		public void CheckingTheExistenceOfABooleanModelProperty()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);

			"Given a browser pointed at the input test page"
				.ContextFixture(() =>
				{
					PhantomJSOptions options = new PhantomJSOptions();
					options.AddAdditionalCapability("takesScreenshot", false);
					driver = CurrentDriver.Driver = new PhantomJSDriver(options);
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					return driver;
				});

			"When testing for the existence of a boolean model property that actually exists"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists<InputModel>(m => m.CheckboxIsChecked)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should be found"
				.Assert(() => Assert.True(propertyExists));
		}

		[Specification]
		public void CheckingTheExistenceOfAModelPropertyWhenThatPropertyIsAttachedToAMethod()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);

			"Given a browser pointed at the input test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					return driver;
				});

			"When testing for the existence of a model property that refers to a method"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists<InputModel>(m => m.ClickButton())));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should be found"
				.Assert(() => Assert.True(propertyExists));
		}

		[Specification]
		public void CheckingTheExistenceOfAMissingModelProperty()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);

			"Given a browser pointed at the input test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					return driver;
				});

			"When testing for the existence of a model property that does not exist"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists<InputModel>(m => m.FieldThatDoesNotExist)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should not be found"
				.Assert(() => Assert.False(propertyExists));
		}

		[Specification]
		public void CheckingTheExistenceOfAModelPropertyWhenTheModelIsMissing()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);

			"Given a browser pointed at the basic test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Basic.html");

					return driver;
				});

			"When testing for the existence of a model property for a model that does not exist on the page"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists<InputModel>(m => m.Text)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should not be found"
				.Assert(() => Assert.False(propertyExists));
		}

		[Specification]
		public void CheckingTheExistenceOfAPropertyOnAnExistingModelInstanceWhenThatPropertyIsAttachedToAMethod()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);
			InputModel model = null;

			"Given an instance of the Input model loaded from the input test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When testing for the existence of a model property that exists on the page"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists(() => model.ClickButton())));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should be found"
				.Assert(() => Assert.True(propertyExists));
		}

		[Specification]
		public void CheckingTheExistenceOfAPropertyOnAnExistingModelInstance()
		{
			IWebDriver driver = null;
			bool propertyExists = false;
			var exception = default(Exception);
			AdvancedModel model = null;

			"Given an instance of the Advanced model loaded from the basic test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Basic.html");

					model = driver.FindModel<AdvancedModel>();

					return driver;
				});

			"When testing for the existence of a model property that exists on the page"
				.Do(() => exception = Record.Exception(() => propertyExists = driver.ModelPropertyExists(() => model.ItemOne)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The property should be found"
				.Assert(() => Assert.True(propertyExists));
		}
	}
}