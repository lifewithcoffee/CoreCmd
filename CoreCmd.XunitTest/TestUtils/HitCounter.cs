using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.XunitTest.TestUtils
{
    public class HitCounter
    {
        Dictionary<string, int> hitDict = new Dictionary<string, int>();

        public void Hit(string key)
        {
            if (hitDict.ContainsKey(key))
                hitDict[key]++;
            else
                hitDict.Add(key, 1);
        }

        public int GetHitCount(string key)
        {
            if (hitDict.ContainsKey(key))
                return hitDict[key];
            else
                return 0;
        }

        public void ResetDict()
        {
            this.hitDict = new Dictionary<string, int>();
        }
    }
}
