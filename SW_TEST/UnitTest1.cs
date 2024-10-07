using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumTests
{
    [TestFixture]
    public class TTHKFullTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver(@"C:\Users\opilane\source\repos\SW_TEST\driver");
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
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

        // 1. Test: Verify Logi sisse modal
        [Test]
        public void VerifyLogiSisseModalTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("martinkemppi22.thkit.ee"));
                Console.WriteLine("Navigated to: " + driver.Url);

                IWebElement logiSisseButton = FindElementWithWait(By.Id("logisisse"));
                logiSisseButton.Click();
                System.Threading.Thread.Sleep(1000);

                Assert.AreEqual("https://martinkemppi22.thkit.ee/#modal_log", driver.Url, "Login modal URL did not match.");
                Console.WriteLine("Login modal URL matched successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during Logi sisse modal test: " + ex.Message);
                throw;
            }
        }

        // 2. Test: Verify Registreeri modal
        [Test]
        public void VerifyRegistreeriModalTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("martinkemppi22.thkit.ee"));
                Console.WriteLine("Navigated to: " + driver.Url);

                IWebElement registreeriButton = FindElementWithWait(By.Id("regimind"));
                registreeriButton.Click();
                System.Threading.Thread.Sleep(1000);

                Assert.AreEqual("https://martinkemppi22.thkit.ee/#modal_reg", driver.Url, "Register modal URL did not match.");
                Console.WriteLine("Register modal URL matched successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during Registreeri modal test: " + ex.Message);
                throw;
            }
        }

        // 3. Test: Verify redirection from tood.html to content/index.php
        [Test]
        public void VerifyRedirectionToContentIndexTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/tood.html");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("tood.html"));
                Console.WriteLine("Navigated to: " + driver.Url);

                IWebElement contentLink = FindElementWithWait(By.XPath("//a[@href='content/index.php']"));
                Assert.IsNotNull(contentLink, "Content link not found.");
                contentLink.Click();
                Console.WriteLine("Clicked on content link.");

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

        // 4. Test: Verify navigation elements on content/index.php
        // Test: Verify presence of text "Aknaruloode tootmine" on content/index.php
        [Test]
        public void VerifyAknaruloodeTootmineTextTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/content/index.php");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("content/index.php"));
                Console.WriteLine("Navigated to: " + driver.Url);

                IWebElement textElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[contains(text(), 'Aknaruloode tootmine')]")));
                Assert.IsNotNull(textElement, "'Aknaruloode tootmine' text not found on the page.");
                Console.WriteLine("'Aknaruloode tootmine' text exists on the page.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during text presence test on content/index.php: " + ex.Message);
                throw;
            }
        }



        // Test: Navigate to content/index.php, click on 'Aknaruloode tootmine' and verify redirection
        [Test]
        public void ClickAknaruloodeTootmineAndVerifyRedirectionTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/content/index.php");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("content/index.php"));
                Console.WriteLine("Navigated to: " + driver.Url);

                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement textElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(), 'Aknaruloode tootmine')]")));
                Assert.IsNotNull(textElement, "'Aknaruloode tootmine' text not found on the page.");
                Console.WriteLine("'Aknaruloode tootmine' text found. Clicking on it.");

                Thread.Sleep(new Random().Next(2000, 4000));
                textElement.Click();

                Thread.Sleep(new Random().Next(2000, 4000));

                string expectedUrl = "https://martinkemppi22.thkit.ee/content/Aknarulood/index.php";
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe(expectedUrl));

                Assert.AreEqual(expectedUrl, driver.Url, "Redirection to the expected URL failed.");
                Console.WriteLine("Successfully redirected to: " + driver.Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during click and redirection test: " + ex.Message);
                throw;
            }
        }

        // Test: Navigate to content/index.php, interact with Muusika küsitlus, and fill out the form
        [Test]
        public void InteractWithMuusikaKysitlusTest()
        {
            try
            {
                driver.Navigate().GoToUrl("https://martinkemppi22.thkit.ee/content/index.php");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("content/index.php"));
                Console.WriteLine("Navigated to: " + driver.Url);

                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement muusikaKysitlusElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(), 'Muusika küsitlus')]")));
                Assert.IsNotNull(muusikaKysitlusElement, "'Muusika küsitlus' text not found on the page.");
                Console.WriteLine("'Muusika küsitlus' text found. Clicking on it.");

                Thread.Sleep(new Random().Next(2000, 4000));
                muusikaKysitlusElement.Click();

                Thread.Sleep(new Random().Next(2000, 4000));

                string expectedUrl = "https://martinkemppi22.thkit.ee/content/index.php?veebileht=muusikakysitlus.php";
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe(expectedUrl));

                Assert.AreEqual(expectedUrl, driver.Url, "Redirection to the expected URL failed.");
                Console.WriteLine("Successfully redirected to: " + driver.Url);

                IWebElement nameInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("name")));
                nameInput.SendKeys("Martin");
                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement emailInput = driver.FindElement(By.Id("email"));
                emailInput.SendKeys("tthk@tthk.ee");
                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement rockCheckbox = driver.FindElement(By.Id("rock"));
                if (!rockCheckbox.Selected)
                {
                    rockCheckbox.Click();
                    Thread.Sleep(new Random().Next(2000, 4000));
                }

                IWebElement technoCheckbox = driver.FindElement(By.Id("techno"));
                if (!technoCheckbox.Selected)
                {
                    technoCheckbox.Click();
                    Thread.Sleep(new Random().Next(2000, 4000));
                }

                IWebElement eurodanceCheckbox = driver.FindElement(By.Id("eurodance"));
                if (!eurodanceCheckbox.Selected)
                {
                    eurodanceCheckbox.Click();
                    Thread.Sleep(new Random().Next(2000, 4000));
                }

                IWebElement xkordaInput = driver.FindElement(By.Id("xkorda"));
                xkordaInput.SendKeys("5");
                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement kultelefonSelect = driver.FindElement(By.Id("kultelefon"));
                var kultelefonSelectElement = new SelectElement(kultelefonSelect);
                kultelefonSelectElement.SelectByText("Jah");
                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement kulradioSelect = driver.FindElement(By.Id("kulradio"));
                var kulradioSelectElement = new SelectElement(kulradioSelect);
                kulradioSelectElement.SelectByText("Ei");
                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement raadiojamanimedInput = driver.FindElement(By.Id("raadiojamanimed"));
                raadiojamanimedInput.SendKeys("Radio FM");
                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement tihtimuusikaInput = driver.FindElement(By.Id("tihtimuusika"));

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].value = arguments[1];", tihtimuusikaInput, 70);

                js.ExecuteScript("arguments[0].dispatchEvent(new Event('input'));", tihtimuusikaInput);

                Thread.Sleep(new Random().Next(2000, 4000));

                IWebElement esmCheckbox = driver.FindElement(By.Id("esm"));
                if (!esmCheckbox.Selected)
                {
                    esmCheckbox.Click();
                    Thread.Sleep(new Random().Next(2000, 4000));
                }

                IWebElement xkordakontserdileSelect = driver.FindElement(By.Id("xkordakontserdile"));
                var xkordakontserdileSelectElement = new SelectElement(xkordakontserdileSelect);
                xkordakontserdileSelectElement.SelectByText("0");
                Thread.Sleep(new Random().Next(2000, 4000));

                Console.WriteLine("All form fields filled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during Muusika küsitlus test: " + ex.Message);
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                    driver = null;
                    Console.WriteLine("Browser closed and resources released.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during TearDown: " + ex.Message);
                throw;
            }
        }
    }
}
