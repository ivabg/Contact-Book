using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {

        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactbookUrl = "https://contactbook.softuniqa.repl.co/api/";
        private const string appLocation = @""; //add path to your .NET5 desktop application

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;


        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Test_AssertFirst()
        {
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ContactbookUrl);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            string windowName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowName);

            var textBoxSearch = driver.FindElementByAccessibilityId("textBoxSearch");
            textBoxSearch.Clear();
            textBoxSearch.SendKeys("steve");

            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            // Wait until the search results appear
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                var labelResult = driver.FindElementByAccessibilityId("labelResult");
                return labelResult.Text.StartsWith("Contacts found:");
            });

            var labelResult = driver.FindElementByAccessibilityId("labelResult");

            var tableContacts = driver.FindElementByAccessibilityId("dataGridViewContacts");
            var cellFirstName = tableContacts.FindElementByXPath("//Edit[@Name='FirstName Row 0, Not sorted.']");
            var cellLastName = tableContacts.FindElementByXPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]");

            Assert.That(cellFirstName.Text, Is.EqualTo("Steve"));
            Assert.That(cellLastName.Text, Is.EqualTo("Jobs"));
        }
    

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }
    }
}
