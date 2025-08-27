using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ControleDeCinema.Testes.Interface.Helpers
{
    public static class TimeHelper
    {
        public static IWebElement EsperarElemento(By locator, IWebDriver driver, int timeoutSegundos = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSegundos));
                return wait.Until(drv => drv.FindElement(locator));
            }
            catch (WebDriverTimeoutException)
            {          
                return null!;
            }
        }

        public static bool EsperarElementoVisivel(By locator, IWebDriver driver, int timeoutSegundos = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSegundos));
                return wait.Until(drv =>
                {
                    var el = drv.FindElement(locator);
                    return el.Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public static bool EsperarElementoVisivelGeneroso(By locator, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return true;

            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

    }
}
