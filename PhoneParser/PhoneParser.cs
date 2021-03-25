using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PhoneParser
{
    /// <summary>
    /// Classe usata per processare i record attraverso delle regole regex
    /// </summary>
    public static class PhoneParser
    {
        //Applica la regola regex all'input e restituisce vero se applicata on successo, aggiornando i riferimenti
        private static bool CheckForAcceptance(string input, string rule,  ref string id, ref string phone)
        {
            var regex = new Regex(rule);
            var matches = regex.Matches(input);
            if (matches.Count == 1)
            {
                if (matches[0].Groups.ContainsKey("id"))
                    id = matches[0].Groups["id"].Value;

                if (matches[0].Groups.ContainsKey("phone"))
                {
                    phone = matches[0].Groups["phone"].Value;
                    return true;
                }
            }
            return false;
        }

        //Applica la regola regex all'input e restituisce vero se applicata on successo, aggiornando i riferimenti
        //La regola regex è di questo formato match#subtitution, in cui il divisotio è #.
        private static bool CheckForCorrection(string input, string correctionRule, Dictionary<string,string> acceptanceRules, ref string id, ref string phone)
        {
            var fields = correctionRule.Split('#', 2);
            string rule = fields[0];
            string substitution = fields[1];
            var replaceResult = Regex.Replace(input, rule, substitution);
            foreach (var acceptanceRule in acceptanceRules)
            {
                string acceptedId = string.Empty;
                string acceptedPhone = string.Empty;
                if (CheckForAcceptance(replaceResult, acceptanceRule.Key, ref acceptedId, ref acceptedPhone))
                {
                    id = acceptedId;
                    phone = acceptedPhone;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Processa un singolo record, applicando le regole in successione:
        /// 1)AcceptanceRules, 2)CorrectionRules, 3)AllOtherRules
        /// Nel caso tutte le regole falliscano, l'esito sarà "Non riconosciuto"
        /// </summary>
        /// <param name="record"></param>
        /// <param name="acceptanceRules"></param>
        /// <param name="correctionRules"></param>
        /// <param name="allOthers"></param>
        internal static void Parse(PhoneRecord record, Dictionary<string, string> acceptanceRules, Dictionary<string, string> correctionRules, Dictionary<string, string> allOthers)
        {
            string id = string.Empty;
            string phone = string.Empty;
            foreach (var rule in acceptanceRules)
            {
                if (CheckForAcceptance(record.Input, rule.Key, ref id, ref phone))
                {
                    record.Id = id;
                    record.Phone = phone;
                    record.Result = rule.Value;
                    return;
                }
            }

            foreach (var ruleSubtitution in correctionRules)
            { 
            if(CheckForCorrection(record.Input, ruleSubtitution.Key, acceptanceRules, ref id, ref phone))
                {
                    record.Id = id;
                    record.Phone = phone;
                    record.Result = ruleSubtitution.Value;
                    return;
                }
            }

            foreach (var rule in allOthers)
            {
                if (CheckForAcceptance(record.Input, rule.Key, ref id, ref phone))
                {
                    record.Id = id;
                    record.Phone = phone;
                    record.Result = rule.Value;
                    return;
                }
            }
            record.Result = "Unrecognized";
        }
    }
}
