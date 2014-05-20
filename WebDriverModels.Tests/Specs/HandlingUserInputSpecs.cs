using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class HandlingUserInputSpecs
	{
		[Specification]
		public void WritingToAnInputTextField()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When an Input[Text] Field backed property is updated"
				.Do(() => model.Text = "Updated text");

			"Then the value of the input field is updated"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("textField"));
					Assert.Equal("Updated text", inputField.GetAttribute("value"));
				});
		}


		[Specification]
		public void WritingToAnInputPasswordField()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When an Input[Password] Field backed property is updated"
				.Do(() => model.Password = "Updated text");

			"Then the value of the input field is updated"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("passwordField"));
					Assert.Equal("Updated text", inputField.GetAttribute("value"));
				});
		}

		[Specification]
		public void WritingToAnInputEmailField()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When an Input[Email] Field backed property is updated"
				.Do(() => model.Email = "Updated text");

			"Then the value of the input field is updated"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("emailField"));
					Assert.Equal("Updated text", inputField.GetAttribute("value"));
				});
		}

		[Specification]
		public void WritingToATextArea()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When an TextArea Field backed property is updated"
				.Do(() => model.TextArea = "Updated text");

			"Then the value of the textarea is updated"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("textarea"));
					Assert.Equal("Updated text", inputField.GetAttribute("value"));
				});
		}

		[Specification]
		public void SelectingACheckBox()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When a checkbox backed property is set to true"
				.Do(() => model.CheckboxIsChecked = true);

			"Then the checkbox is selected"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("checkbox"));
					Assert.True(inputField.Selected);
				});
		}

		[Specification]
		public void DeSelectingACheckBox()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					driver.FindElement(By.Id("checkbox")).Click();

					return driver;
				});

			"When a checkbox backed property is set to false"
				.Do(() => model.CheckboxIsChecked = false);

			"Then the checkbox is not selected"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("checkbox"));
					Assert.False(inputField.Selected);
				});
		}

		[Specification]
		public void SelectingACheckBoxThatIsAlreadySelected()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page, where the checkbox is already selected"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					driver.FindElement(By.Id("checkbox")).Click();

					return driver;
				});

			"When a checkbox backed property is set to true"
				.Do(() => model.CheckboxIsChecked = true);

			"Then the checkbox is still selected"
				.Assert(() =>
				{
					IWebElement inputField = driver.FindElement(By.Id("checkbox"));
					Assert.True(inputField.Selected);
				});
		}

		//[Specification]
		//public void SelectingARadioButton()
		//{
			
		//}

		//[Specification]
		//public void SelectingFromADropDownList()
		//{
			
		//}

		[Specification]
		public void ClickingAButton()
		{
			IWebDriver driver = null;
			InputModel model = null;

			"Given the input model is loaded from a test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Input.html");

					model = driver.FindModel<InputModel>();

					return driver;
				});

			"When a method is called"
				.Do(() => model.ClickButton());

			"Then the button that the method represents is clicked"
				.Assert(() =>
				{
					IWebElement message = driver.FindElement(By.Id("inputButtonMessage"));
					Assert.True(message.Displayed);
				});
		}
	}
}
