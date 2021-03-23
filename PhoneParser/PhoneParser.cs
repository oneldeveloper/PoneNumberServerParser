using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PhoneParser
{
    public static class PhoneParser
    {
        internal static void Parse(PhoneRecord record, Dictionary<string, string> acceptanceRules, Dictionary<string, string> correctionRules)
        {
            foreach (var rule in acceptanceRules)
            {
                var regex = new Regex(rule.Key);
                var matches = regex.Matches(record.Input);
                if (matches.Count == 1)
                {
                    if (matches[0].Groups.ContainsKey("id"))
                        record.Id = matches[0].Groups["id"].Value;

                    if (matches[0].Groups.ContainsKey("phone"))
                    {
                        record.Phone = matches[0].Groups["phone"].Value;
                        record.Result = rule.Value;
                        return;
                    }
                }
            }

            foreach (var rule in correctionRules)
            {
                var regex = new Regex(rule.Key);
                var matches = regex.Matches(record.Input);
                if (matches.Count == 1)
                {
                    if (matches[0].Groups.ContainsKey("id"))
                        record.Id = matches[0].Groups["id"].Value;

                    if (matches[0].Groups.ContainsKey("phone"))
                    {
                        record.Phone = matches[0].Groups["phone"].Value;
                        record.Result = rule.Value;
                        return;
                    }
                }
            }
            record.Result = "Rejected";
        }
    }
}
