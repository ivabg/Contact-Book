using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Linq;

namespace AppiumMobileTests
{
    public class AppiumMobileTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";

        private const string appLocation = @""; //add path to your mobile application

        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [Test]
        public void Test_SearchContact()
        {
            var changeUrl = driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            changeUrl.Clear();
            changeUrl.SendKeys("https://contactbook.softuniqa.repl.co/api");
            var connectButton = driver.FindElementById("contactbook.androidclient:id/buttonConnect");
            connectButton.Click();
            var findelement = driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            findelement.Clear();
            findelement.SendKeys("steve");
            var buttonSearch = driver.FindElementById("contactbook.androidclient:id/buttonSearch");
            buttonSearch.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            var firstName = driver.FindElementById("contactbook.androidclient:id/textViewFirstName");
            var lastName = driver.FindElementById("contactbook.androidclient:id/textViewLastName");

            Assert.That(firstName.Text, Is.EqualTo("Steve"));
            Assert.That(lastName.Text, Is.EqualTo("Jobs"));
        }

        [TearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}
