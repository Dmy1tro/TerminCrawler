using System.Threading.Tasks;
using Crawler.Anticaptcha.Models;

namespace Crawler.Anticaptcha.Interfaces
{
    public interface IAnticaptchaService
    {
        Task<string> Process(ImageToTextInfo imageToText);
    }
}
