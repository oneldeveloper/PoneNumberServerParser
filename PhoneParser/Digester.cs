using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoneParser
{
    public class Digester
    {
        public List<PhoneRecord> PhoneRecords { get; set; }

        public Dictionary<string,string> AcceptanceRules { get; set; }
        public Dictionary<string,string> CorrectionRules { get; set; }

        public Digester()
        {
            PhoneRecords = new List<PhoneRecord>();
            AcceptanceRules = new Dictionary<string, string>();
            CorrectionRules = new Dictionary<string, string>();
        }


        public async Task Digest()
        {
            await Task.Run(() => {
                foreach (var number in PhoneRecords)
                {
                    PhoneParser.Parse(number, AcceptanceRules, CorrectionRules);
                }           
            });
        }     
        
        public PhoneRecord DigestRecord(PhoneRecord record)
        {
            PhoneParser.Parse(record, AcceptanceRules, CorrectionRules);
            return record;
        }
     }
}
