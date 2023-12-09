using System;

namespace Crawler.Core.Constants
{
    public class MessageText
    {
        public static string TestMessageSubject = "Test message";
        public static string TerminFindedSubject = "Termin finded!";

        public static string TestMailMessage = $"{DateTime.Now} -> email message works!";
        public static string TestTelegramMessage = $"{DateTime.Now} -> telegram message works!";

        public static string TerminFindedMessage(string uri) => "Program finded a termin for you!!!" + Environment.NewLine +
                                                              "Follow the link: " + Environment.NewLine +
                                                             $"{uri}";
    }
}
