using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace SeleniumTests
{
    public class SeleniumTests
    {

        private const string url = "https://contactbook.softuniqa.repl.co/";
        private WebDriver driver;

        [OneTimeSetUp]
        public void OpenBrowser()
        {

            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [Test]
        public void Test_ListContacts_CheckForSteveJobs()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var iconViewContacts = driver.FindElement(By.PartialLinkText("View contacts"));

            // Act
            iconViewContacts.Click();

            // Assert
            var cellFirstName = driver.FindElement(By.CssSelector("table tr.fname td"));
            Assert.That(cellFirstName.Text, Is.EqualTo("Steve"));
            var cellLastName = driver.FindElement(By.CssSelector("table tr.lname td"));
            Assert.That(cellLastName.Text, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_ListContacts_SearchForAlbert()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var iconSearch = driver.FindElement(By.PartialLinkText("Search contacts"));
            iconSearch.Click();

            Thread.Sleep(500);

            // Act
            var textBoxSearch = driver.FindElement(By.CssSelector("input#keyword"));
            textBoxSearch.SendKeys("albert");

            var buttonSearch = driver.FindElement(By.CssSelector("button#search"));
            buttonSearch.Click();

            // Assert
            var pageHeading = driver.FindElement(By.CssSelector("main > h1"));
            Assert.That(pageHeading.Text, Is.EqualTo("Contacts Matching Keyword \"albert\""));

            var cellFirstName = driver.FindElement(By.CssSelector("table tr.fname td"));
            Assert.That(cellFirstName.Text, Is.EqualTo("Albert"));
            var cellLastName = driver.FindElement(By.CssSelector("table tr.lname td"));
            Assert.That(cellLastName.Text, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_SearchContacts_InvalidKeyword()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var iconSearch = driver.FindElement(By.PartialLinkText("Search contacts"));
            iconSearch.Click();

            Thread.Sleep(500);

            // Act
            var textBoxSearch = driver.FindElement(By.CssSelector("input#keyword"));
            textBoxSearch.SendKeys("invalid67433646321");

            var buttonSearch = driver.FindElement(By.CssSelector("button#search"));
            buttonSearch.Click();

            // Assert
            var searchResults = driver.FindElement(By.CssSelector("div#searchResult"));
            Assert.That(searchResults.Text, Is.EqualTo("No contacts found."));
        }

        [Test]
        public void Test_CreateContact_InvalidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var iconSearch = driver.FindElement(By.PartialLinkText("Create new contact"));
            iconSearch.Click();

            Thread.Sleep(500);

            // Act
            var textBoxFirstName = driver.FindElement(By.CssSelector("input#firstName"));
            textBoxFirstName.SendKeys("Pesho");

            var buttonCreate = driver.FindElement(By.CssSelector("button#create"));
            buttonCreate.Click();

            // Assert
            var errorMsg = driver.FindElement(By.CssSelector("div.err"));
            Assert.That(errorMsg.Text, Is.EqualTo("Error: Last name cannot be empty!"));
        }


        [Test]
        public void Test_CreateContact_ValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var iconSearch = driver.FindElement(By.PartialLinkText("Create new contact"));
            iconSearch.Click();
            Thread.Sleep(500);
            string firstName = "First Name " + DateTime.Now.Ticks;
            string lastName = "Last Name " + DateTime.Now.Ticks;
            string email = "email" + DateTime.Now.Ticks + "@gmail.com";

            // Act
            var textBoxFirstName = driver.FindElement(By.CssSelector("input#firstName"));
            textBoxFirstName.SendKeys(firstName);

            var textBoxLastName = driver.FindElement(By.CssSelector("input#lastName"));
            textBoxLastName.SendKeys(lastName);

            var textBoxEmail = driver.FindElement(By.CssSelector("input#email"));
            textBoxEmail.SendKeys(email);

            var buttonCreate = driver.FindElement(By.CssSelector("button#create"));
            buttonCreate.Click();

            // Assert
            var contactEntries = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = contactEntries.Last();

            var cellFirstName = lastContact.FindElement(By.CssSelector("tr.fname > td"));
            Assert.That(cellFirstName.Text, Is.EqualTo(firstName));

            var cellLastName = lastContact.FindElement(By.CssSelector("tr.lname > td"));
            Assert.That(cellLastName.Text, Is.EqualTo(lastName));

            var cellEmail = lastContact.FindElement(By.CssSelector("tr.email > td"));
            Assert.That(cellEmail.Text, Is.EqualTo(email));
        }

        [OneTimeTearDown]
        public void Shutdown()
        {
            this.driver.Quit();
        }
    }
}
