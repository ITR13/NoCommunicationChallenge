using System;
using UnityEngine;

public abstract class SavedValue<T> where T : IEquatable<T>
{
    public T Value { get; protected set; }
    protected T _defaultValue;
    public event Action<T> OnValueChanged;

    public SavedValue(T defaultValue)
    {
        _defaultValue = defaultValue;
        Value = defaultValue;
    }

    public void SetValue(T newValue)
    {
        if (newValue.Equals(Value)) return;
        Value = newValue;
        OnValueChanged?.Invoke(Value);
    }

    public abstract void Serialize(string key);

    public abstract void Deserialize(string key);
}
