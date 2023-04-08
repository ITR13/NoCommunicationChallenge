using UnityEngine;

public class SavedInt : SavedValue<int>
{
    public SavedInt(int defaultValue = 0) : base(defaultValue)
    {
    }

    public override void Serialize(string key)
    {
        PlayerPrefs.SetInt(key, Value);
    }

    public override void Deserialize(string key)
    {
        SetValue(PlayerPrefs.GetInt(key, _defaultValue));
    }
}
