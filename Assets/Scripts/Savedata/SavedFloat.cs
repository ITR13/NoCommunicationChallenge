using UnityEngine;

public class SavedFloat : SavedValue<float>
{
    public SavedFloat(float defaultValue=0) : base(defaultValue)
    {
    }

    public override void Serialize(string key)
    {
        PlayerPrefs.SetFloat(key, Value);
    }

    public override void Deserialize(string key)
    {
        SetValue(PlayerPrefs.GetFloat(key, _defaultValue));
    }
}
