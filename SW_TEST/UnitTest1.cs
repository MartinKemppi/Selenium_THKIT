using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace SeleniumTests
{
    [TestFixture]
    public class TTHKFullTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [OneTimeSetUp]
        public void SetUp()
        {
            if (driver != null)
            {
                try
                {
                    //var tabs = driver.WindowHandles.ToList();

                    //foreach (var tab in tabs)
                    //{
                    //    driver.SwitchTo().Window(tab);
                    //    driver.Close();
                    //}

                    driver.Quit();
                    driver.Dispose();
                    Console.WriteLine("Closed previous browser instance.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during closing previous browser: " + ex.Message);
                }
            }

            driver = new ChromeDriver(@"C:\Users\opilane\source\repos\SW_TEST\driver");
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Console.WriteLine("New browser instance opened.");
        }

        private IWebElement FindElementWithWait(By by)
        {
            try
            {
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Element with locator: {by} not found within the timeout.");
                return null;
            }
        }

        [Test, Order(1)]
        public void VerifyModalFormsTestWithPauses()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("martinkemppi22.thkit.ee"));
                Console.WriteLine("Navigated to: " + driver.Url);

                Thread.Sleep(2000);

                IWebElement logiSisseButton = FindElementWithWait(By.Id("logisisse"));
                logiSisseButton.Click();
                Thread.Sleep(1000);

                Assert.AreEqual("https://martinkemppi22.thkit.ee/#modal_log", driver.Url, "Login modal URL did not match.");
                Console.WriteLine("Login modal URL matched successfully.");

                Thread.Sleep(2000);

                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("martinkemppi22.thkit.ee"));
                Console.WriteLine("Navigated back to: " + driver.Url);

                Thread.Sleep(2000);

                IWebElement registreeriButton = FindElementWithWait(By.Id("regimind"));
                registreeriButton.Click();
                Thread.Sleep(1000);

                Assert.AreEqual("https://martinkemppi22.thkit.ee/#modal_reg", driver.Url, "Register modal URL did not match.");
                Console.WriteLine("Register modal URL matched successfully.");

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during modal form test: " + ex.Message);
                throw;
            }
        }

        [Test, Order(2)]
        public void VerifyRedirectionToContentIndexTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/tood.html");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("tood.html"));
                Console.WriteLine("Navigated to: " + driver.Url);

                Thread.Sleep(2000);

                IWebElement contentLink = FindElementWithWait(By.XPath("//a[@href='content/index.php']"));
                Assert.IsNotNull(contentLink, "Content link not found.");
                contentLink.Click();
                Console.WriteLine("Clicked on content link.");

                Thread.Sleep(2000);

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe("https://martinkemppi22.thkit.ee/content/index.php"));
                Assert.AreEqual("https://martinkemppi22.thkit.ee/content/index.php", driver.Url, "Did not navigate to the correct URL for content/index.php.");
                Console.WriteLine("Redirected to content/index.php successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during redirection to content/index.php test: " + ex.Message);
                throw;
            }
        }

        [Test, Order(3)]
        public void InteractWithMuusikaKysitlusTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/content/index.php");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("content/index.php"));
                Console.WriteLine("Navigated to: " + driver.Url);

                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement muusikaKysitlusElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(), 'Muusika küsitlus')]")));
                muusikaKysitlusElement.Click();
                Thread.Sleep(new Random().Next(2000, 4000));

                string expectedUrl = "https://martinkemppi22.thkit.ee/content/index.php?veebileht=muusikakysitlus.php";
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe(expectedUrl));
                Assert.AreEqual(expectedUrl, driver.Url, "Redirection to the expected URL failed.");

                Console.WriteLine("Form filled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during Muusika küsitlus test: " + ex.Message);
                throw;
            }
        }

        [Test, Order(4)]
        public void ClickAknaruloodeTootmineAndVerifyRedirectionTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/content/index.php");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("content/index.php"));
                Console.WriteLine("Navigated to: " + driver.Url);

                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement textElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(), 'Aknaruloode tootmine')]")));
                textElement.Click();
                Thread.Sleep(new Random().Next(2000, 4000));

                var tabs = driver.WindowHandles;
                driver.SwitchTo().Window(tabs[1]);

                string expectedUrl = "https://martinkemppi22.thkit.ee/content/Aknarulood/index.php";
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe(expectedUrl));
                Assert.AreEqual(expectedUrl, driver.Url, "Redirection to the expected URL failed.");
                Console.WriteLine("Successfully redirected to: " + driver.Url);

                driver.Close();
                driver.SwitchTo().Window(tabs[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during click and redirection test: " + ex.Message);
                throw;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try
            {
                if (driver != null)
                {
                    var tabs = driver.WindowHandles.ToList();

                    foreach (var tab in tabs)
                    {
                        driver.SwitchTo().Window(tab);
                        driver.Close();
                    }

                    driver.Quit();
                    driver.Dispose();
                    Console.WriteLine("Browser closed and resources released after all tests.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during OneTimeTearDown: " + ex.Message);
                throw;
            }
        }
    }
}
