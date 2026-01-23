using UnityEngine;

public static class SavesUtility
{
    public static void SaveGame(object obj, string keyName)
    {
        var data = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(keyName, data);
    }

    public static T GetLoadedSave<T>(string keyName)
    {
        var serializedSave = PlayerPrefs.GetString(keyName);

        if (serializedSave != string.Empty)
        {
            return JsonUtility.FromJson<T>(serializedSave);
        }

        return default;
    }
}