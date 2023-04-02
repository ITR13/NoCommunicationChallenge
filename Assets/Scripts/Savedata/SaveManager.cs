using UnityEngine;

public static class SaveManager
{
    private static SaveFile _saveFile;
    public static bool CheckForSave()
    {
        return _saveFile != null;
    }

    public static SaveFile GetSave()
    {
        if (CheckForSave())
        {
            return _saveFile;
        }
        _saveFile = new SaveFile("MainSave");
        return _saveFile;
    }

    public static void DeleteSave()
    {
        _saveFile = null;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void OnGameStart()
    {
        var saveName = PlayerPrefs.GetString("HasSaveFile", null);
        if (string.IsNullOrWhiteSpace(saveName))
        {
            return;
        }

        _saveFile = new SaveFile(saveName);
        _saveFile.Load();
    }

    public static void Save()
    {
        _saveFile.Save();
    }
}
