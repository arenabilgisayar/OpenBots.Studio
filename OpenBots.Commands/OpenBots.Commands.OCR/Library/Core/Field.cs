namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Data Structure for Additional Capability to Extract Forms from Images 
    /// </summary>
    public class Field : KeyValueConfidence<string,string>
    {
        /// <summary>
        /// Serial Order of the Field
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// If fields are grouped, then the name of the group
        /// </summary>
        public string Group { get; set; }


        /// <summary>
        /// If a field has multiple occurences, then the Occurence count can be set here.
        /// Eg. If there is an Invoice that has multiple line items, Occurence can be an iterator foreach line.
        /// </summary>
        public int Occurence { get; set; }
    }
}
