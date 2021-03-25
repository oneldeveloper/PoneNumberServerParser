using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoneParser
{
    /// <summary>
    /// Classe deputata al processo dei numeri di telefono 
    /// </summary>
    public class Digester
    {
        /// <summary>
        /// Collezione dei record da processare
        /// </summary>
        public List<PhoneRecord> PhoneRecords { get; set; }
        /// <summary>
        /// Regole Regex utilizzate per accettare i record di ingresso
        /// La chiave è la regola, il valore descrive l'esito qualora la regola venga applicata con successo
        /// </summary>
        public Dictionary<string,string> AcceptanceRules { get; set; }
        /// <summary>
        /// Regole Regex utilizzate per accettare i record di ingresso
        /// La chiave è la regola, il valore descrive l'esito qualora la regola venga applicata con successo
        /// La regola è composta da regola di match e regola di sostituzione, separati da #
        /// </summary>
        public Dictionary<string,string> CorrectionRules { get; set; }
        /// <summary>
        /// Regole Regex utilizzate per determinare esiti differentidai precedenti
        /// La chiave è la regola, il valore descrive l'esito qualora la regola venga applicata con successo
        /// </summary>
        public Dictionary<string, string> AllOtherRules { get; set; }

        public Digester()
        {
            PhoneRecords = new List<PhoneRecord>();
            AcceptanceRules = new Dictionary<string, string>();
            CorrectionRules = new Dictionary<string, string>();
            AllOtherRules = new Dictionary<string, string>();
        }

        /// <summary>
        /// Processa la collezione di record, applicando le regole in successione:
        /// 1)AcceptanceRules, 2)CorrectionRules, 3)AllOtherRules
        /// Nel caso tutte le regole falliscano, l'esito sarà "Non riconosciuto"
        /// </summary>
        /// <returns></returns>
        public async Task Digest()
        {
            await Task.Run(() => {
                foreach (var number in PhoneRecords)
                {
                    PhoneParser.Parse(number, AcceptanceRules, CorrectionRules, AllOtherRules);
                }           
            });
        }
        /// <summary>
        /// Processa un singolo record, applicando le regole in successione:
        /// 1)AcceptanceRules, 2)CorrectionRules, 3)AllOtherRules
        /// Nel caso tutte le regole falliscano, l'esito sarà "Non riconosciuto"
        /// </summary>
        /// <returns></returns>
        public PhoneRecord DigestRecord(PhoneRecord record)
        {
            PhoneParser.Parse(record, AcceptanceRules, CorrectionRules, AllOtherRules);
            return record;
        }
     }
}
