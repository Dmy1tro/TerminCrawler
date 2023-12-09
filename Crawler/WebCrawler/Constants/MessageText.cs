using System;

namespace Crawler.App.Constants
{
    public class MessageText
    {
        public static string TestMessageSubject => "Test message";
        public static string ObjectFindedSubject => "Info finded!";

        public static string TestMailMessage => $"{DateTime.Now} -> email message works!";
        public static string TestTelegramMessage => $"{DateTime.Now} -> telegram message works!";

        public static string ObjectFindedMessage(string uri) => "Program finded an info for you!!!" + Environment.NewLine +
                                                                "Follow the link: " + Environment.NewLine +
                                                                $"{uri}";
    }
}
