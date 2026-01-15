using System;
using System.Collections.Generic;

namespace FightDojo.Data
{
    [Serializable]
    public class RecordedKeyData
    {
        public List<KeyData> Records;
    }

    [Serializable]
    public class KeyData
    {
        public string Action;
        public long Delta;
        public string KeyName;

        public KeyData(string action, long delta, string keyName)
        {
            Action = action;
            Delta = delta;
            KeyName = keyName;
        }

        public override string ToString() => $"[{Action}] key={KeyName} delta_ms={Delta}";
    }
}
