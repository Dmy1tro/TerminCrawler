using Anticaptcha.Interfaces;
using Anticaptcha.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Shared.Configuration;
using System.Threading.Tasks;
using WebCrawler.Constants;
using WebCrawler.Interfaces;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class TerminChecker : ITerminChecker
    {
        private readonly RemoteWebDriver _webDriver;
        private readonly IAnticaptchaService _anticaptchaService;
        private readonly string _terminUri;

        public TerminChecker(RemoteWebDriver webDriver, IAnticaptchaService anticaptchaService, string terminUri)
        {
            _webDriver = webDriver;
            _anticaptchaService = anticaptchaService;
            _terminUri = terminUri;
        }

        public bool HasTermin()
        {
            _webDriver.Navigate().GoToUrl(_terminUri);

            var captchaNode = GetCaptcha();

            if (captchaNode != null)
            {
                FillCaptcha(captchaNode);
            }

            var languageEn = _webDriver.FindElementByXPath("//*[@id=\"nav-main\"]/ul/li[2]/a");
            languageEn.Click();
            Task.Delay(700).Wait();

            var hasTermin = CheckAllTabs();

            return hasTermin;
        }

        private IWebElement GetCaptcha()
        {
            try
            {
                var captchaNode = _webDriver.FindElementByXPath("//captcha/div");

                return captchaNode;
            }
            catch
            {
                return null;
            }
        }

        private void FillCaptcha(IWebElement captcha)
        {
            var captchaStyle = captcha.GetAttribute("style");

            var startIndex = captchaStyle.IndexOf("base64,") + "base64,".Length;
            var endIndex = captchaStyle.IndexOf("')") > 0 ? captchaStyle.IndexOf("')") : captchaStyle.IndexOf("\")");
            var length = endIndex - startIndex;

            var base64Image = captchaStyle.Substring(startIndex, length);

            var imageToText = new ImageToTextInfo
            {
                BodyBase64 = base64Image
            };

            var captchaResult = _anticaptchaService.Process(imageToText);

            var captchaInput = _webDriver.FindElementById("appointment_captcha_month_captchaText");
            captchaInput.SendKeys(captchaResult);

            var captchaSubmit = _webDriver.FindElementById("appointment_captcha_month_appointment_showMonth");
            captchaSubmit.Click();
            Task.Delay(700).Wait();

            var captchaNode = GetCaptcha();

            if (captchaNode != null)
            {
                FillCaptcha(captchaNode);
            }
        }

        private bool CheckAllTabs()
        {
            var hasTermin = false;

            for (var i = 0; i < 5; i++)
            {
                _webDriver.FindElementByXPath("//*[@id=\"content\"]/div[1]/h2[2]/a[1]").Click();
                Task.Delay(800).Wait();
            }

            var lastUrl = _webDriver.Url;
            var currentUrl = string.Empty;

            while (lastUrl != currentUrl)
            {
                var terminInfoMessage = _webDriver.FindElementByXPath("//*[@id=\"content\"]/div[1]/h2[1]").Text;

                if (!terminInfoMessage.Contains(SiteMessages.NotFoundEn))
                {
                    hasTermin = true;
                }

                lastUrl = _webDriver.Url;
                _webDriver.FindElementByXPath("//*[@id=\"content\"]/div[1]/h2[2]/a[2]").Click();
                Task.Delay(800).Wait();
                currentUrl = _webDriver.Url;
            }

            return hasTermin;
        }
    }
}
