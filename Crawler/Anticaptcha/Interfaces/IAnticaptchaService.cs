using Anticaptcha.Models;

namespace Anticaptcha.Interfaces
{
    public interface IAnticaptchaService
    {
        public string Process(AnticaptchaInfoBase anticaptchaInfo);
    }
}
