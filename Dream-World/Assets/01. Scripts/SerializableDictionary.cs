using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
{
    public List<Tkey> InspectorKeys;
    public List<TValue> InspectorValues;

    public SerializableDictionary()
    {
        InspectorKeys = new List<Tkey>();
        InspectorValues = new List<TValue>();
        SyncInpectorFromDictionary();
    }

    public new void Add(Tkey key, TValue value)
    {
        base.Add(key, value);
        SyncInpectorFromDictionary();
    }

    public new void Remove(Tkey key)
    {
        base.Remove(key);
        SyncInpectorFromDictionary();
    }

    public void OnBeforeSerialize() { }
    public void SyncInpectorFromDictionary()
    {
        InspectorKeys.Clear();
        InspectorValues.Clear();

        foreach (KeyValuePair<Tkey, TValue> pair in this)
        {
            InspectorKeys.Add(pair.Key);
            InspectorValues.Add(pair.Value);
        }
    }

    public void SyncDictionaryFromInspector()
    {
        foreach (var key in Keys.ToList())
        {
            base.Remove(key);
        }

        for (int i = 0; i < InspectorKeys.Count; i++)
        {
            if (this.ContainsKey(InspectorKeys[i]))
            {
                Debug.LogError("등록된 ");
                break;
            }
            base.Add(InspectorKeys[i], InspectorValues[i]);
        }
    }

    public void OnAfterDeserialize()
    {
        if (InspectorKeys.Count == InspectorValues.Count)
        {
            SyncDictionaryFromInspector();
        }
    }
}