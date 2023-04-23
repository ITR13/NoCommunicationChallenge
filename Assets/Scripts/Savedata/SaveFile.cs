
using System.Collections.Generic;
using UnityEngine;

public class SaveFile
{
    private string _saveName;
    public Dictionary<string, SavedInt> SavedInts = new Dictionary<string, SavedInt>()
    {
        { "TotalAirJumps", new() },
        { "TotalAirJumpsFromShop", new() },
        { "AirJumpsRemaining", new() },
        { "Money", new(0) },
        { "CoolStuff", new() },
    };
    
    public Dictionary<string, SavedFloat> SavedFloats = new Dictionary<string, SavedFloat>()
    {
        { "CheckpointX", new() },
        { "CheckpointY", new() },
    };

    public SaveFile(string saveName)
    {
        _saveName = saveName;

        for(var i = 0; i<100; i++)
        {
            SavedInts.Add($"CollectibleSet.{i}", new SavedInt());
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString("HasSaveFile", _saveName);
        foreach (var (key, saved) in SavedInts)
        {
            saved.Serialize($"{_saveName}.int.{key}");
        }
        foreach (var (key, saved) in SavedFloats)
        {
            saved.Serialize($"{_saveName}.float.{key}");
        }
    }

    public void Load()
    {
        foreach (var (key, saved) in SavedInts)
        {
            saved.Serialize($"{_saveName}.int.{key}");
        }
        foreach (var (key, saved) in SavedFloats)
        {
            saved.Serialize($"{_saveName}.float.{key}");
        }
    }
}