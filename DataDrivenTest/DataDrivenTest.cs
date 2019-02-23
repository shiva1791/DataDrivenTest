using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Protractor;

namespace DataDrivenTest
{
    [TestClass]
    public class DataDrivenTest
    {
        public TestContext TestContext { get; set; }

        private IWebDriver driver;
        private NgWebDriver ngDriver;
        private WebDriverWait WaitTime;
        private string URL = "https://s3.ap-south-1.amazonaws.com/shiva1792.tk/DiscountCalculator/ShoppingDiscount.html";

        [TestInitialize]
        public void Start()
        {
            //Change the geckodriver path to a location on your machine below
            driver = new FirefoxDriver(@"C:\Users\Shiva\Downloads");
            driver.Manage().Window.Maximize();
            WaitTime = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            ngDriver = new NgWebDriver(driver);
        }

        [DataSource("ShoppingDiscountTestData")]
        [TestMethod]
        public void FirstTest()
        {
            driver.Navigate().GoToUrl(URL);
            ngDriver.WaitForAngular();

            //Find product price text box using the ng-model 
            NgWebElement ProductPrice = ngDriver.FindElement(NgBy.Model("productPrice"));
            ProductPrice.SendKeys(TestContext.DataRow["ProductPrice"].ToString());
            Thread.Sleep(2000);

            //Find discount text box using the ng-model 
            NgWebElement DiscountOnProduct = ngDriver.FindElement(NgBy.Model("discountPercent"));
            DiscountOnProduct.SendKeys(TestContext.DataRow["DiscountOnProduct"].ToString());
            Thread.Sleep(2000);

            //Find button using selenium locator XPath
            NgWebElement BtnPriceAfterDiscount = ngDriver.FindElement(By.XPath("//*[@id='f1']/fieldset[2]/input[1]"));
            BtnPriceAfterDiscount.Click();
            Thread.Sleep(2000);

            //Find discounted product text box using the ng-model 
            NgWebElement afterDiscountValue = ngDriver.FindElement(NgBy.Model("afterDiscount"));
            string value = afterDiscountValue.GetAttribute("value");
            Thread.Sleep(2000);

            //Assert for corect value
            Assert.AreEqual<string>(TestContext.DataRow["AfterDiscountPrice"].ToString(), value);
        }

        [TestCleanup]
        public void Stop()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
