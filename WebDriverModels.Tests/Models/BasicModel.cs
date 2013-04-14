
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverModels.Tests.Models
{
    public class BasicModel
    {
        [FindsBy(How = How.Id, Using = "itemOne")]
        public IWebElement ItemOne { get; set; }

        public BasicModel(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }
    }
}
