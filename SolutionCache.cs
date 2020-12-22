using System.Collections.Generic;

namespace AoC_2020
{
    public class SolutionCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> solutionDictionary;

        public SolutionCache()
        {
            solutionDictionary = new Dictionary<TKey, TValue>();
        }

        public void AddSolutionFor(TKey solutionKey, TValue solutionValue)
        {
            solutionDictionary.Add(solutionKey, solutionValue);    
        }
        
        public bool HasSolutionStored(TKey solutionKey)
        {
            return solutionDictionary.ContainsKey(solutionKey);
        }
        
        public TValue GetSolutionFor(TKey solutionKey)
        {
            return solutionDictionary[solutionKey];
        }
    }
}