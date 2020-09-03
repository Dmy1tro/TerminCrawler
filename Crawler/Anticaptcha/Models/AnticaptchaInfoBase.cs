using Anticaptcha.Enums;
using Anticaptcha.Response;

namespace Anticaptcha.Models
{
    public abstract class AnticaptchaInfoBase
    {
        internal const string Host = "api.anti-captcha.com";
        internal const SchemeType Scheme = SchemeType.Https;

        public string ErrorMessage { get; private set; }
        public int TaskId { get; private set; }
        internal string ClientKey { get; set; }
        internal TaskResultResponse TaskInfo { get; set; }

        public abstract string ToJson();
    }
}
