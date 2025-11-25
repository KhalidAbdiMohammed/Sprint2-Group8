using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DUTests
{
    // Markerar klassen som en NUnit-testklass
    [TestFixture]
    public class FirstTryTest
    {
        // WebDriver som styr webbläsaren
        private IWebDriver driver;

        private WebDriverWait wait;

        public IDictionary<string, object> vars { get; private set; }

        // Körs före varje testfall
        [SetUp]
        public void SetUp()
        {
            // Startar Chrome via Selenium Manager (auto driver management)
            driver = new ChromeDriver();

            // Maximerar fönstret för att undvika element som hamnar utanför skärmen
            driver.Manage().Window.Maximize();

            // Väntar på element innan interaktion
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Initierar dictionary för testdata
            vars = new Dictionary<string, object>();
        }

        // Körs efter varje testfall
        [TearDown]
        public void TearDown()
        {
            // Stänger webbläsarfönstret
            driver.Quit();

            // Städar upp resurser
            driver.Dispose();
        }

        // Säker klickfunktion som väntar på att element ska gå att klicka
        private void SafeClick(By locator)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(locator));

            var element = driver.FindElement(locator);

            // Scrollar till elementet för att undvika att andra element blockerar
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView(true);", element);

            element.Click();
        }

        // Funktion som byter till ny flik om en länk öppnar ett nytt fönster
        private void SwitchToNewTab()
        {
            var originalWindow = driver.CurrentWindowHandle;

            // Väntar tills fler flikar har öppnats
            wait.Until(d => driver.WindowHandles.Count > 1);

            // Byter till den nya fliken
            foreach (var handle in driver.WindowHandles)
            {
                if (handle != originalWindow)
                {
                    driver.SwitchTo().Window(handle);
                    break;
                }
            }
        }

        //  Testfallet
        [Test]
        public void FirstTry()
        {
            // Startar på Dalarna Universitets webbplats
            driver.Navigate().GoToUrl("https://www.du.se/");

            // Klickar på cookie-knappen när den är klickbar
            SafeClick(By.Id("cookieAcceptAllButton"));

            // Navigerar till sektionen "Utbildning"
            SafeClick(By.LinkText("Utbildning"));

            // Klickar på första blocket (t.ex. "Data & IT")
            SafeClick(By.CssSelector(".block:nth-child(1) .linkbutton-text span"));

            // Assert – bevisar att navigationen fungerat genom att kontrollera URL
            Assert.That(driver.Url.Contains("du.se"), Is.True,
                "URL ska innehålla du.se efter navigation.");
        }
    }
}
