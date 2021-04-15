namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Similar to KeyValuePair but each Key-Value pair will have a Confidence set by the Ocr Engine
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValueConfidence<TKey, TValue>  : ValueConfidence<TValue>
    {
        public KeyValueConfidence()
        {
        }

        public KeyValueConfidence(TKey key, TValue value, double confidence)
        {
            Key = key;
            Value = value;
            Confidence = confidence;
        }

        public TKey Key { get; set; }

    }


    public class ValueConfidence<TValue>
    {
        public ValueConfidence()
        {
        }

        public ValueConfidence(TValue value, double confidence)
        {
            Value = value;
            Confidence = confidence;
        }

        public TValue Value { get; set; }

        public double Confidence { get; set; }
    }


}
