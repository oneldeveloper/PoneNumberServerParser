using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneNumberParser.Data
{
    public class DigesterOptions
    {
        public const string PhoneParser = "PhoneParser";
        public Dictionary<string,string> AcceptanceRules { get; set; }
        public Dictionary<string, string> CorrectionRules { get; set; }
        public Dictionary<string, string> AllOtherRules { get; set; }
    }
}
