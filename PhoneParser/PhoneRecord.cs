namespace PhoneParser
{
    /// <summary>
    /// Record che contiene le informazioni di ingresso e di risultato di processo
    /// </summary>
    public class PhoneRecord
    {
        /// <summary>
        /// La stringa d'ingresso del parser
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// L'identificatore del numero di telefono
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Il numero di telefono risultante
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Il risultato dell'operazione di parsing
        /// </summary>
        public string Result { get; set; }
    }
}