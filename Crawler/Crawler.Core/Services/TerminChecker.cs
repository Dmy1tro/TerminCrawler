using Crawler.Anticaptcha.Interfaces;
using Crawler.Anticaptcha.Models;
using Crawler.Core.Configs;
using Crawler.Core.Constants;
using Crawler.Core.Interfaces;
using Crawler.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Crawler.Core.Services
{
    public class TerminChecker : ICrawlerService
    {
        private readonly IAnticaptchaService _anticaptchaService;
        private readonly CrawlerConfig _crawlerConfig;
        private readonly ChromeDriver _chromeDriver;

        public TerminChecker(ChromeDriver chromeDriver, IAnticaptchaService anticaptchaService, CrawlerConfig crawlerConfig)
        {
            _chromeDriver = chromeDriver;
            _anticaptchaService = anticaptchaService;
            _crawlerConfig = crawlerConfig;
        }

        public async Task<SearchResult> Search()
        {
            _chromeDriver.Navigate().GoToUrl(_crawlerConfig.Uri);

            var captchaInput = GetCaptcha();

            if (captchaInput != null)
            {
                await FillCaptcha(captchaInput);
            }

            var languageEn = _chromeDriver.FindElement(By.XPath("//*[@id=\"nav-main\"]/ul/li[2]/a"));
            languageEn.Click();
            await Task.Delay(700);

            var hasTermin = await CheckAllTabs();

            return hasTermin ? SearchResult.Success : SearchResult.NotFound;
        }

        private IWebElement GetCaptcha()
        {
            try
            {
                var captchaNode = _chromeDriver.FindElement(By.XPath("//captcha/div"));

                return captchaNode;
            }
            catch
            {
                return null;
            }
        }

        private async Task FillCaptcha(IWebElement captcha)
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

            var captchaResult = await _anticaptchaService.Process(imageToText);

            var captchaInput = _chromeDriver.FindElement(By.Id("appointment_captcha_month_captchaText"));
            captchaInput.SendKeys(captchaResult);

            var captchaSubmit = _chromeDriver.FindElement(By.Id("appointment_captcha_month_appointment_showMonth"));
            captchaSubmit.Click();
            Task.Delay(700).Wait();

            // If captcha still not satisfied then try again.
            var captchaNode = GetCaptcha();
            if (captchaNode != null)
            {
                await FillCaptcha(captchaNode);
            }
        }

        private async Task<bool> CheckAllTabs()
        {
            var hasTermin = false;

            for (var i = 0; i < 5; i++)
            {
                _chromeDriver.FindElement(By.XPath("//*[@id=\"content\"]/div[1]/h2[2]/a[1]")).Click();
                await Task.Delay(800);
            }

            var lastUrl = _chromeDriver.Url;
            var currentUrl = string.Empty;

            while (lastUrl != currentUrl)
            {
                var terminInfoMessage = _chromeDriver.FindElement(By.XPath("//*[@id=\"content\"]/div[1]/h2[1]")).Text;

                if (!terminInfoMessage.Contains(SiteMessages.NotFoundEn))
                {
                    hasTermin = true;
                }

                lastUrl = _chromeDriver.Url;
                _chromeDriver.FindElement(By.XPath("//*[@id=\"content\"]/div[1]/h2[2]/a[2]")).Click();
                await Task.Delay(800);
                currentUrl = _chromeDriver.Url;
            }

            return hasTermin;
        }
    }
}
