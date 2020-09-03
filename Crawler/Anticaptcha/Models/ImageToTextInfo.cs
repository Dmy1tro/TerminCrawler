using Anticaptcha.Enums;
using Newtonsoft.Json;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Anticaptcha.Models
{
    public class ImageToTextInfo : AnticaptchaInfoBase
    {
        public ImageToTextInfo()
        {
            BodyBase64 = string.Empty;
            Phrase = false;
            Case = false;
            Numeric = NumericOption.NoRequirements;
            Math = 0;
            MinLength = 0;
            MaxLength = 0;
        }

        public string BodyBase64 { private get; set; }

        public string FilePath
        {
            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException($"File {value} not found");
                }
                else
                {
                    BodyBase64 = FileHelper.FileToBase64String(value);

                    if (BodyBase64 == null)
                    {
                        throw new ArgumentException($"Could not convert the file {value} to base64. Is this an image file?");
                    }
                }
            }
        }

        public bool Phrase { private get; set; }
        public bool Case { private get; set; }
        public NumericOption Numeric { private get; set; }
        public int Math { private get; set; }
        public int MinLength { private get; set; }
        public int MaxLength { private get; set; }

        public override string ToJson()
        {
            var data = new
            {
                clientKey = ClientKey,
                task = GetTaskInfo()
            };

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            return json;
        }

        private Dictionary<string, object> GetTaskInfo()
        {
            var data = new Dictionary<string, object>
            {
                {"type", "ImageToTextTask"},
                {"body", BodyBase64.Replace("\r", "").Replace("\n", "")},
                {"phrase", Phrase},
                {"case", Case},
                {"numeric", Numeric.Equals(NumericOption.NoRequirements)
                    ? 0
                    : Numeric.Equals(NumericOption.NumbersOnly)
                        ? 1
                        : 2 },
                {"math", Math},
                {"minLength", MinLength},
                {"maxLength", MaxLength}
            };

            return data;
        }
    }
}
